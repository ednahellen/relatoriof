using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.Word;
using GPSFA_WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Projeto_Socorrista
{
    public partial class frmEstoque : Form
    {
        private string busca = "";
        private string unidadeEscolhida = "";
        private string status_validade = "";
        private DateTime? dataValidade = null;
        private bool modoAgrupado = true;
        private int codUsuLogado;
        private System.Windows.Forms.Button btDarBaixa;
        public frmEstoque()
        {
            InitializeComponent();
        }

        public frmEstoque(int codUsu)
        {
            InitializeComponent();
            codUsuLogado = codUsu;
            this.btnDarBaixa.Click += new System.EventHandler(this.btDarBaixa_Click);
        }

        private void frmEstoque_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView(modoAgrupado);
            CarregarUnidades();

            if (cbxStatus.Items.Count == 0)
            {
                cbxStatus.Items.Add("Selecione...");
                cbxStatus.Items.Add("Válido");
                cbxStatus.Items.Add("Próximo do vencimento");
                cbxStatus.Items.Add("Vencido");
            }
            cbxStatus.SelectedIndex = 0;

            if (cbxModoExibicao.Items.Count == 0)
            {
                cbxModoExibicao.Items.Add("Agrupado");
                cbxModoExibicao.Items.Add("Detalhado");
            }
            cbxModoExibicao.SelectedIndex = modoAgrupado ? 0 : 1;

            dtpDataValidade.Value = DateTime.Today;
            dtpDataValidade.Checked = false;

            CarregarDados();
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

                dgvEstoque.Columns["QuantidadeTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvEstoque.Columns["Peso"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

                dgvEstoque.Columns["Peso"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgvEstoque.ResumeLayout();
        }

        private void CarregarUnidades()
        {
            cbxCategoria.Items.Clear();
            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                {
                    string sql = "SELECT descricao FROM tbunidades ORDER BY descricao ASC";
                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbxCategoria.Items.Add(reader["descricao"].ToString());
                        }
                    }
                }
                cbxCategoria.Items.Insert(0, "Selecione...");
                cbxCategoria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar unidades: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarDados()
        {
            dgvEstoque.Rows.Clear();
            ConfigurarDataGridView(modoAgrupado);

            if (modoAgrupado)
            {
                CarregarDadosAgrupado();
            }
            else
            {
                CarregarDadosDetalhado();
            }
        }

        private void CarregarDadosAgrupado()
        {
            dgvEstoque.Rows.Clear();

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"
                    SELECT
                        l.descricao AS descricao,
                        SUM(p.quantidade) AS quantidade_total,
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
                    GROUP BY l.codList, l.descricao, u.descricao, l.peso
                    HAVING SUM(p.quantidade) > 0
                    ORDER BY l.descricao;";



                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@busca", busca ?? "");
                    cmd.Parameters.AddWithValue("@buscaPattern", "%" + (busca ?? "") + "%");
                    cmd.Parameters.AddWithValue("@validade", dataValidade.HasValue ? dataValidade.Value.Date : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@unidade", unidadeEscolhida == "Selecione..." ? "" : unidadeEscolhida ?? "");
                    cmd.Parameters.AddWithValue("@status", status_validade == "Selecione..." ? "" : status_validade ?? "");

                    using (var reader = cmd.ExecuteReader())
                    {
                        decimal totalQuantidade = 0;
                        decimal totalPeso = 0;

                        while (reader.Read())
                        {
                            string validadeMinima = reader["validade_minima"] == DBNull.Value
                                ? ""
                                : Convert.ToDateTime(reader["validade_minima"]).ToString("dd/MM/yyyy");

                            decimal qtd = Convert.ToDecimal(reader["quantidade_total"]);
                            decimal peso = Convert.ToDecimal(reader["peso"]);

                            totalQuantidade += qtd;
                            totalPeso += (qtd * peso);

                            dgvEstoque.Rows.Add(
                                reader["descricao"].ToString(),
                                qtd.ToString("N0"),
                                reader["unidade"].ToString(),
                                peso.ToString("N0"),
                                reader["status_validade"].ToString(),
                                validadeMinima
                            );
                        }

                        if (dgvEstoque.Rows.Count > 0)
                        {
                            string pesoTotalFormatado = totalPeso >= 1000
                                ? (totalPeso / 1000).ToString("N2") + " kg"
                                : totalPeso.ToString("N0") + " g";

                            int linhaTotal = dgvEstoque.Rows.Add(
                                "🔹 TOTAL GERAL 🔹",
                                totalQuantidade.ToString("N0"),
                                "",
                                pesoTotalFormatado,
                                "",
                                ""
                            );

                            dgvEstoque.Rows[linhaTotal].DefaultCellStyle.BackColor = Color.FromArgb(48, 112, 99);
                            dgvEstoque.Rows[linhaTotal].DefaultCellStyle.ForeColor = Color.White;
                            dgvEstoque.Rows[linhaTotal].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                            dgvEstoque.Rows[linhaTotal].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dgvEstoque.Rows[linhaTotal].Selected = false;
                        }
                    }
                }
            }
        }

        private void CarregarDadosDetalhado()
        {
            dgvEstoque.Rows.Clear();

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"
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
                    WHERE p.quantidade > 0 AND (@busca = '' OR l.descricao LIKE @buscaPattern OR p.codProd LIKE @buscaPattern)
                        AND (@unidade = '' OR u.descricao = @unidade)
                        AND (@validade IS NULL OR DATE(p.dataDeValidade) = @validade)
                        AND (@status = ''
                            OR (@status = 'Válido' AND DATEDIFF(p.dataDeValidade, CURDATE()) > 60)
                            OR (@status = 'Próximo do vencimento' AND DATEDIFF(p.dataDeValidade, CURDATE()) BETWEEN 0 AND 60)
                            OR (@status = 'Vencido' AND p.dataDeValidade < CURDATE()))
                    ORDER BY l.descricao, p.dataDeValidade ASC;";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@busca", busca ?? "");
                    cmd.Parameters.AddWithValue("@buscaPattern", "%" + (busca ?? "") + "%");
                    cmd.Parameters.AddWithValue("@validade", dataValidade.HasValue ? dataValidade.Value.Date : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@unidade", unidadeEscolhida == "Selecione..." ? "" : unidadeEscolhida ?? "");
                    cmd.Parameters.AddWithValue("@status", status_validade == "Selecione..." ? "" : status_validade ?? "");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string entrada = reader["dataDeEntrada"] == DBNull.Value ? "" : Convert.ToDateTime(reader["dataDeEntrada"]).ToString("dd/MM/yyyy");
                            string validadeStr = reader["dataDeValidade"] == DBNull.Value ? "" : Convert.ToDateTime(reader["dataDeValidade"]).ToString("dd/MM/yyyy");

                            dgvEstoque.Rows.Add(
                                reader["codProd"].ToString(),
                                reader["nomeProduto"].ToString(),
                                reader["peso"].ToString(),
                                reader["unidade"].ToString(),
                                reader["status_validade"].ToString(),
                                entrada,
                                validadeStr
                            );
                        }
                    }
                }
            }
        }

        private void AplicarFiltros()
        {
            bool categoriaSelecionada = cbxCategoria.SelectedIndex > 0;
            bool statusSelecionado = cbxStatus.SelectedIndex > 0;
            bool validadeSelecionada = dtpDataValidade.Checked;

            if (!categoriaSelecionada && !statusSelecionado && !validadeSelecionada && string.IsNullOrWhiteSpace(txtNomeOrCod.Text))
            {
                MessageBox.Show("Nenhum filtro foi selecionado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            unidadeEscolhida = categoriaSelecionada ? cbxCategoria.Text : "";
            status_validade = statusSelecionado ? cbxStatus.Text : "";
            dataValidade = validadeSelecionada ? dtpDataValidade.Value.Date : (DateTime?)null;
            busca = txtNomeOrCod.Text.Trim();

            CarregarDados();
        }

        private void LimparFiltros()
        {
            cbxCategoria.SelectedIndex = 0;
            cbxStatus.SelectedIndex = 0;
            dtpDataValidade.Value = DateTime.Today;
            dtpDataValidade.Checked = false;
            txtNomeOrCod.Clear();

            unidadeEscolhida = "";
            status_validade = "";
            dataValidade = null;
            busca = "";

            CarregarDados();
        }

        private void ProcessarBaixaEstoque(string nomeProduto, int quantidadeBaixa, string destino)
        {
            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                {
                    string sqlProduto = "SELECT codList FROM tbLista WHERE descricao = @descricao LIMIT 1";
                    int codList;
                    using (var cmd = new MySqlCommand(sqlProduto, conn))
                    {
                        cmd.Parameters.AddWithValue("@descricao", nomeProduto);
                        codList = Convert.ToInt32(cmd.ExecuteScalar());
                        

                    }

                    string sqlEstoque = "SELECT IFNULL(SUM(quantidade),0) FROM tbprodutos WHERE codList = @codList";
int estoqueAtual;

using (var cmd = new MySqlCommand(sqlEstoque, conn))
{
    cmd.Parameters.AddWithValue("@codList", codList);
    estoqueAtual = Convert.ToInt32(cmd.ExecuteScalar());
}

                    if (estoqueAtual < quantidadeBaixa)
                    {
                        MessageBox.Show($"Estoque insuficiente! Disponível: {estoqueAtual}", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string sqlInsert = @"INSERT INTO tbProdutos 
                        (codList, quantidade, dataDeEntrada, tipoMovimentacao, codUsu, codOri, destino) 
                        VALUES (@codList, @quantidade, NOW(), 'SAIDA', @codUsu, 1, @destino)";

                    using (var cmd = new MySqlCommand(sqlInsert, conn))
                    {
                        cmd.Parameters.AddWithValue("@codList", codList);
                        cmd.Parameters.AddWithValue("@quantidade", -quantidadeBaixa);
                        cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
                        cmd.Parameters.AddWithValue("@destino",string.IsNullOrWhiteSpace(destino) ? (object)DBNull.Value : destino);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Baixa de {quantidadeBaixa} unidade(s) realizada com sucesso!\nDestino: {destino}",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CarregarDados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== EVENTOS DOS BOTÕES ====================

        private void btDarBaixa_Click(object sender, EventArgs e)
        {
            if (dgvEstoque.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um produto para dar baixa.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvEstoque.SelectedRows[0];
            if (selectedRow.Cells[0].Value?.ToString().Contains("TOTAL GERAL") == true ||
                selectedRow.Cells[0].Value?.ToString().Contains("🔹") == true)
            {
                MessageBox.Show("Selecione um produto, não a linha de total.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string produto = "";
            string quantidadeDisponivelStr = "0";

            if (modoAgrupado)
            {
                produto = selectedRow.Cells["ProdutoAgrupado"].Value?.ToString() ?? "";
                quantidadeDisponivelStr = selectedRow.Cells["QuantidadeTotal"].Value?.ToString() ?? "0";
            }
            else
            {
                produto = selectedRow.Cells["Nome"].Value?.ToString() ?? "";
                quantidadeDisponivelStr = "1";
            }

            if (string.IsNullOrEmpty(produto))
            {
                MessageBox.Show("Produto não identificado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            quantidadeDisponivelStr = Regex.Replace(quantidadeDisponivelStr, @"[^\d]", "");

            if (!int.TryParse(quantidadeDisponivelStr, out int quantidadeDisponivel) || quantidadeDisponivel <= 0)
                quantidadeDisponivel = 1;

            using (var formBaixa = new Form())
            {
                formBaixa.Text = "Dar Baixa no Estoque";
                formBaixa.Size = new Size(600, 450);
                formBaixa.StartPosition = FormStartPosition.CenterParent;
                formBaixa.FormBorderStyle = FormBorderStyle.FixedDialog;
                formBaixa.MaximizeBox = false;
                formBaixa.MinimizeBox = false;
                formBaixa.BackColor = Color.FromArgb(242, 237, 228);

                var lblProduto = new Label { Text = "Produto:", Location = new Point(20, 20), Size = new Size(80, 25) };
                var lblProdutoNome = new Label
                {
                    Text = produto.Length > 40 ? produto.Substring(0, 40) + "..." : produto,
                    Location = new Point(110, 20),
                    Size = new Size(300, 25),
                    Font = new Font("Microsoft YaHei", 10, FontStyle.Bold)
                };
                var lblDisponivel = new Label { Text = "Disponível:", Location = new Point(20, 50), Size = new Size(80, 25) };
                var lblDisponivelQtd = new Label
                {
                    Text = quantidadeDisponivel.ToString(),
                    Location = new Point(110, 50),
                    Size = new Size(50, 25),
                    Font = new Font("Microsoft YaHei", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(48, 112, 99)
                };
                var lblQuantidade = new Label { Text = "Quantidade:", Location = new Point(20, 80), Size = new Size(80, 25) };
                var nudQuantidade = new NumericUpDown { Location = new Point(110, 80), Size = new Size(100, 25), Minimum = 1, Maximum = quantidadeDisponivel };
                var lblDestino = new Label { Text = "Destino:", Location = new Point(20, 110), Size = new Size(80, 25) };
                var txtDestino = new TextBox { Location = new Point(110, 110), Size = new Size(250, 25) };
                var lblObservacao = new Label { Text = "Obs:", Location = new Point(20, 140), Size = new Size(80, 25) };
                var txtObservacao = new TextBox { Location = new Point(110, 140), Size = new Size(250, 25) };

                var btnConfirmar = new Button
                {
                    Text = "Confirmar",
                    Location = new Point(110, 180),
                    Size = new Size(120, 35),
                    BackColor = Color.FromArgb(48, 112, 99),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnConfirmar.Click += (s, ev) => formBaixa.DialogResult = DialogResult.OK;

                var btnCancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new Point(240, 180),
                    Size = new Size(120, 35),
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnCancelar.Click += (s, ev) => formBaixa.DialogResult = DialogResult.Cancel;

                formBaixa.Controls.AddRange(new Control[] {
                    lblProduto, lblProdutoNome, lblDisponivel, lblDisponivelQtd, lblQuantidade, nudQuantidade,
                    lblDestino, txtDestino, lblObservacao, txtObservacao, btnConfirmar, btnCancelar
                });

                if (formBaixa.ShowDialog() == DialogResult.OK)
                {
                    int quantidadeBaixa = (int)nudQuantidade.Value;
                    string destino = txtDestino.Text.Trim();

                    
                    //if (string.IsNullOrEmpty(destino))
                    //{
                    //    MessageBox.Show("Informe o destino da baixa.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}

                    ProcessarBaixaEstoque(produto, quantidadeBaixa, destino);
                }
               
            }
        }

        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            LimparFiltros();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeOrCod.Text))
            {
                MessageBox.Show("Digite o nome ou código do produto para pesquisar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AplicarFiltros();
        }

        private void btnCarregaTodosProdutos_Click(object sender, EventArgs e)
        {
            LimparFiltros();
        }

        private void btnAplicarModo_Click(object sender, EventArgs e)
        {
            modoAgrupado = !modoAgrupado;
            cbxModoExibicao.SelectedIndex = modoAgrupado ? 0 : 1;
            CarregarDados();
        }

        private void txtNomeOrCod_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeOrCod.Text))
            {
                busca = "";
                CarregarDados();
            }
        }

        private void txtNomeOrCod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (!string.IsNullOrWhiteSpace(txtNomeOrCod.Text))
                    AplicarFiltros();
            }
        }

        private void btnProdutosPrincipais_Click(object sender, EventArgs e)
        {
            var palavrasBusca = new List<string>
            {
                "MACARRÃO", "MOLHO DE TOMATE", "ARROZ", "FEIJÃO", "LEITE",
                "FUBÁ", "ÓLEO", "AÇÚCAR", "SAL", "FARINHA", "CAFÉ"
            };

            modoAgrupado = true;
            cbxModoExibicao.SelectedIndex = 0;
            CarregarProdutosPrincipais(palavrasBusca);
        }

        private void CarregarProdutosPrincipais(List<string> palavras)
        {
            dgvEstoque.Rows.Clear();

            using (var conn = DataBaseConnection.OpenConnection())
            {
                var buscaQueries = new List<string>();
                var cmd = new MySqlCommand();

                for (int i = 0; i < palavras.Count; i++)
                {
                    string paramName = "@busca" + i;
                    buscaQueries.Add($"(l.descricao LIKE {paramName})");
                    cmd.Parameters.AddWithValue(paramName, "%" + palavras[i] + "%");
                }

                string filtroBusca = buscaQueries.Count > 0 ? $"({string.Join(" OR ", buscaQueries)})" : "1=1";

                cmd.CommandText = $@"
                    SELECT
                        l.descricao AS descricao,
                        SUM(p.quantidade) AS quantidade_total,
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
                    WHERE {filtroBusca}
                    GROUP BY l.codList, l.descricao, u.descricao, l.peso
                    ORDER BY l.descricao;";

                cmd.Connection = conn;

                using (var reader = cmd.ExecuteReader())
                {
                    decimal totalQuantidade = 0;
                    decimal totalPeso = 0;

                    while (reader.Read())
                    {
                        string validadeMinima = reader["validade_minima"] == DBNull.Value
                            ? ""
                            : Convert.ToDateTime(reader["validade_minima"]).ToString("dd/MM/yyyy");

                        decimal qtd = Convert.ToDecimal(reader["quantidade_total"]);
                        decimal peso = Convert.ToDecimal(reader["peso"]);

                        totalQuantidade += qtd;
                        totalPeso += (qtd * peso);

                        dgvEstoque.Rows.Add(
                            reader["descricao"].ToString(),
                            qtd.ToString("N0"),
                            reader["unidade"].ToString(),
                            peso.ToString("N0"),
                            reader["status_validade"].ToString(),
                            validadeMinima
                        );
                    }

                    if (dgvEstoque.Rows.Count > 0)
                    {
                        string pesoTotalFormatado = totalPeso >= 1000
                            ? (totalPeso / 1000).ToString("N2") + " kg"
                            : totalPeso.ToString("N0") + " g";

                        int linhaTotal = dgvEstoque.Rows.Add(
                            "🔹 TOTAL GERAL 🔹",
                            totalQuantidade.ToString("N0"),
                            "",
                            pesoTotalFormatado,
                            "",
                            ""
                        );

                        dgvEstoque.Rows[linhaTotal].DefaultCellStyle.BackColor = Color.FromArgb(48, 112, 99);
                        dgvEstoque.Rows[linhaTotal].DefaultCellStyle.ForeColor = Color.White;
                        dgvEstoque.Rows[linhaTotal].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                        dgvEstoque.Rows[linhaTotal].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvEstoque.Rows[linhaTotal].Selected = false;
                    }
                }
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            var menu = new frmMenuPrincipal(codUsuLogado);
            menu.Show();
            this.Close();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvEstoque.Rows.Count == 0)
                return;

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo Excel (*.xlsx)|*.xlsx";
                sfd.FileName = "Relatorio.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Relatório");

                    for (int i = 0; i < dgvEstoque.Columns.Count; i++)
                        ws.Cell(1, i + 1).Value = dgvEstoque.Columns[i].HeaderText;

                    for (int i = 0; i < dgvEstoque.Rows.Count; i++)
                        for (int j = 0; j < dgvEstoque.Columns.Count; j++)
                            ws.Cell(i + 2, j + 1).Value = dgvEstoque.Rows[i].Cells[j].Value?.ToString();

                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Relatório exportado com sucesso.");
            }
        }

        private void dgvEstoque_Paint(object sender, PaintEventArgs e)
        {
            if (dgvEstoque.Rows.Count == 0)
            {
                string mensagem = "Nenhum produto encontrado.";
                using (var fonte = new Font("Segoe UI", 14, FontStyle.Bold))
                {
                    TextRenderer.DrawText(e.Graphics, mensagem, fonte, dgvEstoque.ClientRectangle,
                        Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        
    }
}