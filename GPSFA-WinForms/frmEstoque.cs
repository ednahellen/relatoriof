using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using GPSFA_WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Projeto_Socorrista
{
    public partial class frmEstoque : Form
    {
        private string busca = "";
        private string unidadeEscolhida = "";
        private string status_validade = "";
        private DateTime? dataValidade = null;
        private bool modoAgrupado = true;

        public frmEstoque()
        {
            InitializeComponent();
        }

        int codUsuLogado;

        public frmEstoque(int codUsu)
        {
            InitializeComponent();
            codUsuLogado = codUsu;
        }

        private void ConfigurarDataGridView(bool modoAgrupado)
        {
            dgvEstoque.SuspendLayout();
            dgvEstoque.Columns.Clear();
            dgvEstoque.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstoque.AllowUserToAddRows = false;
            dgvEstoque.ReadOnly = true;
            dgvEstoque.RowHeadersVisible = false;
            dgvEstoque.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstoque.MultiSelect = false;
            dgvEstoque.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvEstoque.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvEstoque.RowsDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Regular);
            dgvEstoque.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvEstoque.RowTemplate.Height = 38;
            dgvEstoque.ClearSelection();

            if (modoAgrupado)
            {
                dgvEstoque.Columns.Add("ProdutoAgrupado", "Produto");
                dgvEstoque.Columns.Add("QuantidadeTotal", "Quantidade Total");
                dgvEstoque.Columns.Add("Unidade", "Unidade");
                dgvEstoque.Columns.Add("Peso", "Peso");
                dgvEstoque.Columns.Add("StatusValidade", "Status");
                dgvEstoque.Columns.Add("ValidadeMinima", "Validade Mínima");
            }
            else
            {
                dgvEstoque.Columns.Add("CodProd", "Código");
                dgvEstoque.Columns.Add("Nome", "Nome");
                dgvEstoque.Columns.Add("Peso", "Peso");
                dgvEstoque.Columns.Add("Unidade", "Unidade");
                dgvEstoque.Columns.Add("StatusValidade", "Status");
                dgvEstoque.Columns.Add("DataDeEntrada", "Entrada");
                dgvEstoque.Columns.Add("DataDeValidade", "Validade");
            }

            dgvEstoque.ResumeLayout();
        }

        private void CarregarDadosAgrupado(string busca = "", DateTime? validade = null, string unidade = "", string status = "")
        {
            dgvEstoque.Rows.Clear();

            MySqlCommand comm = new MySqlCommand();

            comm.CommandText = @"
        SELECT
            l.descricao AS descricao,
            SUM(p.estoqueAtual) AS quantidade_total,
            u.descricao AS unidade,
            l.peso AS peso,
            MIN(p.dataDeValidade) AS validade_minima,
            CASE
                WHEN MIN(p.dataDeValidade) < CURDATE() THEN 'Vencido'
                WHEN DATEDIFF(MIN(p.dataDeValidade), CURDATE()) <= 60 THEN 'Próximo do vencimento'
                ELSE 'Válido'
            END AS status_validade
        FROM tbprodutos p
        INNER JOIN tblista l ON l.codList = p.codList
        INNER JOIN tbunidades u ON u.codUni = l.codUni
        WHERE
            (@busca = '' OR l.descricao LIKE @buscaPattern OR p.codProd LIKE @buscaPattern)
            AND (@unidade = '' OR u.descricao = @unidade)
            AND (@validade IS NULL OR DATE(p.dataDeValidade) = @validade)
        GROUP BY l.codList, l.descricao, u.descricao, l.peso
        HAVING
            (@status = '')
            OR (@status = 'Vencido' AND validade_minima < CURDATE())
            OR (@status = 'Próximo do vencimento' AND DATEDIFF(validade_minima, CURDATE()) <= 60 AND validade_minima >= CURDATE())
            OR (@status = 'Válido' AND DATEDIFF(validade_minima, CURDATE()) > 60)
        ORDER BY l.descricao;
        ";

            comm.CommandType = CommandType.Text;

            comm.Parameters.AddWithValue("@busca", busca ?? "");
            comm.Parameters.AddWithValue("@buscaPattern", "%" + (busca ?? "") + "%");
            comm.Parameters.AddWithValue("@validade", validade.HasValue ? validade.Value.Date : (object)DBNull.Value);
            comm.Parameters.AddWithValue("@unidade", unidade == "Selecione..." ? "" : unidade ?? "");
            comm.Parameters.AddWithValue("@status", status == "Selecione..." ? "" : status ?? "");

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                string validadeMinima = DR["validade_minima"] == DBNull.Value
                    ? ""
                    : Convert.ToDateTime(DR["validade_minima"]).ToString("dd/MM/yyyy");

                dgvEstoque.Rows.Add(
                    DR["descricao"].ToString(),
                    DR["quantidade_total"].ToString(),
                    DR["unidade"].ToString(),
                    DR["peso"].ToString(),
                    DR["status_validade"].ToString(),
                    validadeMinima
                );
            }

            DR.Close();
            DataBaseConnection.CloseConnection();
        }


        private void CarregarDadosDetalhados(string busca = "", DateTime? validade = null, string unidade = "", string status = "")
        {
            dgvEstoque.Rows.Clear();

            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = @"
                    SELECT
                        p.codProd,
                        l.descricao AS nomeProduto,
                        l.peso,
                        u.descricao AS unidade,
                        p.dataDeEntrada,
                        p.dataDeValidade,
                        CASE
                            WHEN p.dataDeValidade < CURDATE() THEN 'Vencido'
                            WHEN DATEDIFF(p.dataDeValidade, CURDATE()) <= 60 THEN 'Próximo do vencimento'
                            ELSE 'Válido'
                        END AS status_validade
                    FROM tbprodutos p
                    INNER JOIN tblista l ON l.codList = p.codList
                    INNER JOIN tbunidades u ON u.codUni = l.codUni
                    WHERE
                        (@busca = '' OR l.descricao LIKE @buscaPattern OR p.codProd LIKE @buscaPattern)
                        AND (@unidade = '' OR u.descricao = @unidade)
                        AND (@validade IS NULL OR DATE(p.dataDeValidade) = @validade)
                        AND (
                            @status = ''
                            OR (@status = 'Válido' AND DATEDIFF(p.dataDeValidade, CURDATE()) > 60)
                            OR (@status = 'Próximo do vencimento' AND DATEDIFF(p.dataDeValidade, CURDATE()) BETWEEN 0 AND 60)
                            OR (@status = 'Vencido' AND p.dataDeValidade < CURDATE())
                        )
                    ORDER BY l.descricao, p.dataDeValidade ASC;
                    ";
                comm.CommandType = CommandType.Text;

                comm.Parameters.AddWithValue("@busca", busca ?? "");
                comm.Parameters.AddWithValue("@buscaPattern", "%" + (busca ?? "") + "%");
                comm.Parameters.AddWithValue("@validade", validade.HasValue ? validade.Value.Date : (object)DBNull.Value);
                comm.Parameters.AddWithValue("@unidade", unidade == "Selecione..." ? "" : unidade ?? "");
                comm.Parameters.AddWithValue("@status", status == "Selecione..." ? "" : status ?? "");

                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        string entrada = DR["dataDeEntrada"] == DBNull.Value ? "" : Convert.ToDateTime(DR["dataDeEntrada"]).ToString("dd/MM/yyyy");
                        string validadeStr = DR["dataDeValidade"] == DBNull.Value ? "" : Convert.ToDateTime(DR["dataDeValidade"]).ToString("dd/MM/yyyy");

                        dgvEstoque.Rows.Add(
                            DR["codProd"].ToString(),
                            DR["nomeProduto"].ToString(),
                            DR["peso"].ToString(),
                            DR["unidade"].ToString(),
                            DR["status_validade"].ToString(),
                            entrada,
                            validadeStr
                        );
                    }
                }

                DataBaseConnection.CloseConnection();
            }
        }
        private void CarregarUnidades()
        {
            cbxCategoria.Items.Clear();
            try
            {
                using (MySqlCommand comm = new MySqlCommand())
                {
                    comm.CommandText = @"SELECT descricao FROM tbunidades ORDER BY descricao ASC";
                    comm.CommandType = CommandType.Text;
                    comm.Connection = DataBaseConnection.OpenConnection();

                    using (MySqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbxCategoria.Items.Add(dr["descricao"].ToString());
                        }
                    }

                    DataBaseConnection.CloseConnection();
                }

                // garante item de "nenhuma seleção"
                cbxCategoria.Items.Insert(0, "Selecione...");
                cbxCategoria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                DataBaseConnection.CloseConnection();
                MessageBox.Show("Erro ao carregar unidades: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void carregaDados()
        {
            dgvEstoque.Rows.Clear();
            ConfigurarDataGridView(modoAgrupado);

            if (modoAgrupado)
            {
                CarregarDadosAgrupado(busca, dataValidade, unidadeEscolhida, status_validade);
            }
            else
            {
                CarregarDadosDetalhados(busca, dataValidade, unidadeEscolhida, status_validade);
            }
        }

        private void frmEstoque_Load(object sender, EventArgs e)
        {
            // inicializa visual e dados
            ConfigurarDataGridView(modoAgrupado);
            CarregarUnidades();

            // garantir itens do status (exemplo)
            if (cbxStatus.Items.Count == 0)
            {
                cbxStatus.Items.Add("Selecione...");
                cbxStatus.Items.Add("Válido");
                cbxStatus.Items.Add("Próximo do vencimento");
                cbxStatus.Items.Add("Vencido");
            }

            cbxStatus.SelectedIndex = 0;

            // modo exibicao combo (sincronize itens com seu uso)
            if (cbxModoExibicao.Items.Count == 0)
            {
                cbxModoExibicao.Items.Add("Agrupado");
                cbxModoExibicao.Items.Add("Exibidor");
            }
            cbxModoExibicao.SelectedIndex = modoAgrupado ? 0 : 1;

            // data inicial sem checked
            dtpDataValidade.Value = DateTime.Today;
            dtpDataValidade.Checked = false;

            // carrega primeira vez
            carregaDados();
        }
        private void AplicarFiltros()
        {
            bool categoriaSelecionada = cbxCategoria.SelectedIndex > 0;
            bool statusSelecionado = cbxStatus.SelectedIndex > 0;
            bool validadeSelecionada = dtpDataValidade.Checked;

            if (!categoriaSelecionada && !statusSelecionado && !validadeSelecionada)
            {
                MessageBox.Show("Nenhum filtro foi selecionado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            unidadeEscolhida = categoriaSelecionada ? cbxCategoria.Text : "";
            status_validade = statusSelecionado ? cbxStatus.Text : "";
            dataValidade = validadeSelecionada ? dtpDataValidade.Value.Date : (DateTime?)null;

            carregaDados();
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            bool isClean = (cbxCategoria.SelectedIndex == 0 || cbxCategoria.SelectedIndex == -1)
                           && (cbxStatus.SelectedIndex == 0 || cbxStatus.SelectedIndex == -1)
                           && !dtpDataValidade.Checked
                           && string.IsNullOrWhiteSpace(txtNomeOrCod.Text);

            if (isClean)
            {
                MessageBox.Show("Não há filtros para limpar", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // limpar
            cbxCategoria.SelectedIndex = 0;
            cbxStatus.SelectedIndex = 0;
            dtpDataValidade.Value = DateTime.Today;
            dtpDataValidade.Checked = false;
            txtNomeOrCod.Clear();

            // reset globals
            busca = "";
            unidadeEscolhida = "";
            status_validade = "";
            dataValidade = null;

            carregaDados();
        }

        private void AtualizarStatusValidade()
        {
            try
            {
                using (MySqlCommand comm = new MySqlCommand())
                {
                    comm.CommandText = @"
                        UPDATE tbprodutos
                        SET status_validade =
                            CASE
                                WHEN dataDeValidade < CURDATE() THEN 'Vencido'
                                WHEN DATEDIFF(dataDeValidade, CURDATE()) BETWEEN 0 AND 60 THEN 'Próximo do vencimento'
                                ELSE 'Válido'
                            END
                        WHERE dataDeValidade IS NOT NULL;
                        ";
                    comm.CommandType = CommandType.Text;
                    comm.Connection = DataBaseConnection.OpenConnection();
                    comm.ExecuteNonQuery();
                    DataBaseConnection.CloseConnection();
                }
            }
            catch (MySqlException mex)
            {
                DataBaseConnection.CloseConnection();
                // pode ser que a coluna status_validade não exista; trate se necessário
                Console.WriteLine("AtualizarStatusValidade failed: " + mex.Message);
            }
        }
        private void btnCarregaTodosProdutos_Click(object sender, EventArgs e)
        {
            txtNomeOrCod.Clear();
            busca = "";
            carregaDados();
        }

        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeOrCod.Text))
            {
                MessageBox.Show("Digite o nome ou código do produto para pesquisar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AplicarFiltros();
        }

        private void btnAplicarModoExibicao_Click(object sender, EventArgs e)
        {
            modoAgrupado = !modoAgrupado;

            // Combo mostra o modo atual
            cbxModoExibicao.SelectedIndex = modoAgrupado ? cbxModoExibicao.SelectedIndex = 1 : cbxModoExibicao.SelectedIndex = 2;

            // Botão mostra para onde vai (inverter)
            btnAplicarModo.Text = modoAgrupado ? "Modo Detalhado" : "Modo Agrupado";

            carregaDados();
        }

        private void txtNomeOrCod_TextChanged(object sender, EventArgs e)
        {
            busca = txtNomeOrCod.Text.Trim();
            carregaDados();
        }

        private void txtNomeOrCod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
            }
        }

        private void dgvEstoque_Paint(object sender, PaintEventArgs e)
        {
            if (dgvEstoque.Rows.Count == 0)
            {
                string mensagem = "Nenhum produto encontrado.";
                using (Font fonte = new Font("Segoe UI", 14, FontStyle.Bold))
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        mensagem,
                        fonte,
                        dgvEstoque.ClientRectangle,
                        Color.Gray,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                }
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Hide();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {

            if (dgvEstoque.Rows.Count == 0)
                return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo Excel (*.xlsx)|*.xlsx";
                sfd.FileName = "Relatorio.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Relatório");

                    for (int i = 0; i < dgvEstoque.Columns.Count; i++)
                        ws.Cell(1, i + 1).Value = dgvEstoque.Columns[i].HeaderText;

                    for (int i = 0; i < dgvEstoque.Rows.Count; i++)
                        for (int j = 0; j < dgvEstoque.Columns.Count; j++)
                            ws.Cell(i + 2, j + 1).Value =
                                dgvEstoque.Rows[i].Cells[j].Value?.ToString();

                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Relatório exportado com sucesso.");
            }
        }

        private void btnAplicarModo_Click(object sender, EventArgs e)
        {

            modoAgrupado = !modoAgrupado;

            // Combo mostra o modo atual
            cbxModoExibicao.SelectedIndex = modoAgrupado ? cbxModoExibicao.SelectedIndex = 1 : cbxModoExibicao.SelectedIndex = 2;

            // Botão mostra para onde vai (inverter)
            btnAplicarModo.Text = modoAgrupado ? "Modo Detalhado" : "Modo Agrupado";

            carregaDados();
        }

        private void btnLimparFiltros_Click_1(object sender, EventArgs e)
        {

            bool isClean = (cbxCategoria.SelectedIndex == 0 || cbxCategoria.SelectedIndex == -1)
                           && (cbxStatus.SelectedIndex == 0 || cbxStatus.SelectedIndex == -1)
                           && !dtpDataValidade.Checked
                           && string.IsNullOrWhiteSpace(txtNomeOrCod.Text);

            if (isClean)
            {
                MessageBox.Show("Não há filtros para limpar", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // limpar
            cbxCategoria.SelectedIndex = 0;
            cbxStatus.SelectedIndex = 0;
            dtpDataValidade.Value = DateTime.Today;
            dtpDataValidade.Checked = false;
            txtNomeOrCod.Clear();

            // reset globals
            busca = "";
            unidadeEscolhida = "";
            status_validade = "";
            dataValidade = null;

            carregaDados();
        }

        private void btnAplicarFiltros_Click_1(object sender, EventArgs e)
        {
            AplicarFiltros();

        }

        private void btnProdutosPrincipais_Click(object sender, EventArgs e)
        {
            List<string> palavrasBusca = new List<string>
                {
                    "MACARRAO",
                    "MOLHO DE TOMATE",
                    "ARROZ 1KG",
                    "FEIJAO 1KG",
                    "LEITE",
                    "FUBA",
                    "OLEO",
                    "AÇUCAR 1KG",
                    "SAL 1KG",
                    "ARROZ 5KG",
                    "ARROZ 2KG"
                };

            carregarProdutosPrincipais(palavrasBusca, null, "Selecione...", "Selecione...");
        }

        private void carregarProdutosPrincipais(List<string> palavras, DateTime? validade = null, string unidade = "", string status = "") {
            dgvEstoque.Rows.Clear();
            MySqlCommand comm = new MySqlCommand();

            // Lista para guardar as cláusulas de busca dinâmica
            List<string> buscaQueries = new List<string>();

            // Se a lista tiver palavras, montamos a query dinâmica
            if (palavras != null && palavras.Count > 0)
            {
                for (int i = 0; i < palavras.Count; i++)
                {
                    string paramName = "@busca" + i;
                    // Cria a cláusula buscando em descricao ou codProd
                    buscaQueries.Add($"(l.descricao LIKE {paramName} OR p.codProd LIKE {paramName})");
                    // Adiciona o valor com as porcentagens do LIKE
                    comm.Parameters.AddWithValue(paramName, "%" + palavras[i] + "%");
                }
            }

            // Une as cláusulas com OR. Se não houver palavras, usamos "1=1" (sempre verdadeiro)
            string filtroBusca = buscaQueries.Count > 0 ? $"({string.Join(" OR ", buscaQueries)})" : "1=1";

            comm.CommandText = $@"
                SELECT
                    l.descricao AS descricao,
                    SUM(p.estoqueAtual) AS quantidade_total,
                    u.descricao AS unidade,
                    l.peso AS peso,
                    MIN(p.dataDeValidade) AS validade_minima,
                    CASE
                        WHEN MIN(p.dataDeValidade) < CURDATE() THEN 'Vencido'
                        WHEN DATEDIFF(MIN(p.dataDeValidade), CURDATE()) <= 60 THEN 'Próximo do vencimento'
                        ELSE 'Válido'
                    END AS status_validade
                FROM tbprodutos p
                INNER JOIN tblista l ON l.codList = p.codList
                INNER JOIN tbunidades u ON u.codUni = l.codUni
                WHERE
                    {filtroBusca}
                    AND (@unidade = '' OR u.descricao = @unidade)
                    AND (@validade IS NULL OR DATE(p.dataDeValidade) = @validade)
                GROUP BY l.codList, l.descricao, u.descricao, l.peso
                HAVING
                    (@status = '')
                    OR (@status = 'Vencido' AND validade_minima < CURDATE())
                    OR (@status = 'Próximo do vencimento' AND DATEDIFF(validade_minima, CURDATE()) <= 60 AND validade_minima >= CURDATE())
                    OR (@status = 'Válido' AND DATEDIFF(validade_minima, CURDATE()) > 60)
                ORDER BY l.descricao;";

            // Demais parâmetros fixos
            comm.Parameters.AddWithValue("@unidade", unidade == "Selecione..." ? "" : unidade ?? "");
            comm.Parameters.AddWithValue("@validade", validade.HasValue ? validade.Value.Date : (object)DBNull.Value);
            comm.Parameters.AddWithValue("@status", status == "Selecione..." ? "" : status ?? "");

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                string validadeMinima = DR["validade_minima"] == DBNull.Value
                    ? ""
                    : Convert.ToDateTime(DR["validade_minima"]).ToString("dd/MM/yyyy");

                dgvEstoque.Rows.Add(
                    DR["descricao"].ToString(),
                    DR["quantidade_total"].ToString(),
                    DR["unidade"].ToString(),
                    DR["peso"].ToString(),
                    DR["status_validade"].ToString(),
                    validadeMinima
                );
            }

            DR.Close();
            DataBaseConnection.CloseConnection();
        }
    }
}



