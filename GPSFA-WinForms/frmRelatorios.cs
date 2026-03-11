using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using Projeto_Socorrista;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace GPSFA_WinForms
{
    public partial class frmRelatorios : Form
    {
        // ===== VARIÁVEIS PRIVADAS =====
        private int _codUsuLogado;
        private DataGridView dgvSaidasEstoque;
        private Button btnImportarDados;

        // ===== CONSTRUTORES =====
        public frmRelatorios()
        {
            InitializeComponent();
        }

        public frmRelatorios(int codUsu) : this()
        {
            _codUsuLogado = codUsu;
            this.Load += frmRelatorios_Load;
            InicializarControlesAdicionais();
            ConfigurarEventos();
        }

        // ===== INICIALIZAÇÃO =====
        private void frmRelatorios_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView();
            dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
            dtpDataFinalPeriodo.Value = DateTime.Now;
            CarregarUsuarios();
            CarregarProdutos();
            CarregarStatus();
            CarregarDados();
            CarregarProdutosSaida();
            CriarGridSaidasEstoque();
        }

        private void InicializarControlesAdicionais()
        {
            btnImportarDados = new Button
            {
                Text = "Importar Dados",
                Location = new Point(584, 12),
                Size = new Size(164, 33),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei", 11.25F, FontStyle.Regular)
            };
            btnImportarDados.Click += BtnImportarDados_Click;
            pnlFiltrosDeBusca.Controls.Add(btnImportarDados);
            btnExportarExcel.Location = new Point(754, 12);
            btnPesquisar.Location = new Point(976, 7);
        }

        private void ConfigurarEventos()
        {
            if (btnPesquisar != null) btnPesquisar.Click += btnPesquisar_Click;
            if (btnLimparFiltros != null) btnLimparFiltros.Click += btnLimparFiltros_Click;
            if (btnExportarExcel != null) btnExportarExcel.Click += btnExportarExcel_Click;
            if (btnMenu != null) btnMenu.Click += btnMenu_Click;
            if (button1 != null) button1.Click += button1_Click;
            if (btnRegistrarSaida != null) btnRegistrarSaida.Click += BtnRegistrarSaida_Click;
            if (btnAtualizarSaidas != null) btnAtualizarSaidas.Click += (s, e) => CarregarSaidasEstoque();
            if (tabControl1 != null) tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        private void ConfigurarDataGridView()
        {
            dgvRelatorios.AutoGenerateColumns = true;
            dgvRelatorios.AllowUserToAddRows = false;
            dgvRelatorios.ReadOnly = false;
            dgvRelatorios.RowHeadersVisible = false;
            dgvRelatorios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelatorios.BackgroundColor = Color.White;
            dgvRelatorios.Dock = DockStyle.Fill;
            dgvRelatorios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRelatorios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelatorios.EnableHeadersVisualStyles = false;

            dgvRelatorios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(48, 112, 99);
            dgvRelatorios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRelatorios.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei", 10, FontStyle.Bold);
            dgvRelatorios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRelatorios.ColumnHeadersHeight = 40;

            dgvRelatorios.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            dgvRelatorios.DefaultCellStyle.Padding = new Padding(5);
            dgvRelatorios.RowTemplate.Height = 35;
            dgvRelatorios.GridColor = Color.FromArgb(230, 230, 230);

            dgvRelatorios.CellFormatting += DgvRelatorios_CellFormatting;
            dgvRelatorios.DataBindingComplete += DgvRelatorios_DataBindingComplete;
            dgvRelatorios.CellEndEdit += DgvRelatorios_CellEndEdit;
        }

        // ===== CARREGAMENTO DE DADOS =====
        private void CarregarUsuarios()
        {
            if (cbbUsuario == null) return;
            cbbUsuario.Items.Clear();
            cbbUsuario.Items.Add("TODOS");

            using (var conexao = DataBaseConnection.OpenConnection())
            {
                string sql = "SELECT nome FROM tbVoluntarios ORDER BY nome";
                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        cbbUsuario.Items.Add(reader["nome"].ToString());
                }
            }
            cbbUsuario.SelectedIndex = 0;
        }

        private void CarregarProdutos()
        {
            if (cbxProduto == null) return;
            cbxProduto.Items.Clear();
            cbxProduto.Items.Add("TODOS");

            using (var conexao = DataBaseConnection.OpenConnection())
            {
                string sql = "SELECT descricao FROM tbLista ORDER BY descricao";
                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        cbxProduto.Items.Add(reader["descricao"].ToString());
                }
            }
            cbxProduto.SelectedIndex = 0;
        }

        private void CarregarStatus()
        {
            cbxStatus.Items.Clear();
            cbxStatus.Items.Add("TODOS");
            cbxStatus.Items.Add("VENCIDO");
            cbxStatus.Items.Add("7 DIAS");
            cbxStatus.Items.Add("15 DIAS");
            cbxStatus.Items.Add("30 DIAS");
            cbxStatus.Items.Add("OK");
            cbxStatus.SelectedIndex = 0;
        }

        private void CarregarDados()
        {
            DataTable tabela = new DataTable();

            try
            {
                using (var conexao = DataBaseConnection.OpenConnection())
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"SELECT 
                        p.codProd AS 'Código',
                        DATE_FORMAT(p.dataDeEntrada, '%d/%m/%Y %H:%i') AS 'Data Entrada',
                        l.descricao AS Produto,
                        p.quantidade AS Qtd,
                        l.peso AS 'Peso (g)',
                        l.unidade AS Unidade,
                        CASE 
                            WHEN p.dataDeValidade IS NULL OR p.dataDeValidade < '2000-01-01' THEN ''
                            ELSE DATE_FORMAT(p.dataDeValidade, '%d/%m/%Y')
                        END AS Validade,
                        DATEDIFF(p.dataDeValidade, CURDATE()) AS 'Dias Restantes'
                    FROM tbProdutos p
                    INNER JOIN tbLista l ON p.codList = l.codList
                    WHERE p.dataDeEntrada >= @ini 
                    AND p.dataDeEntrada < DATE_ADD(@fim, INTERVAL 1 DAY)");

                    if (cbxProduto.SelectedItem != null && cbxProduto.SelectedItem.ToString() != "TODOS")
                        sql.AppendLine("AND l.descricao = @produto");

                    sql.AppendLine("ORDER BY p.dataDeEntrada DESC");

                    using (var cmd = new MySqlCommand(sql.ToString(), conexao))
                    {
                        cmd.Parameters.AddWithValue("@ini", dtpDataInicialPeriodo.Value.Date);
                        cmd.Parameters.AddWithValue("@fim", dtpDataFinalPeriodo.Value.Date);

                        if (cbxProduto.SelectedItem != null && cbxProduto.SelectedItem.ToString() != "TODOS")
                            cmd.Parameters.AddWithValue("@produto", cbxProduto.SelectedItem.ToString());

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(tabela);
                    }
                }

                tabela.Columns.Add("Status", typeof(string));

                foreach (DataRow row in tabela.Rows)
                {
                    if (row["Dias Restantes"] != DBNull.Value)
                    {
                        int dias = Convert.ToInt32(row["Dias Restantes"]);
                        if (dias < 0)
                            row["Status"] = "VENCIDO";
                        else if (dias <= 7)
                            row["Status"] = "7 DIAS";
                        else if (dias <= 15)
                            row["Status"] = "15 DIAS";
                        else if (dias <= 30)
                            row["Status"] = "30 DIAS";
                        else
                            row["Status"] = "OK";
                    }
                    else
                    {
                        row["Status"] = "DATA INVÁLIDA";
                    }
                }

                AdicionarLinhaTotais(tabela);
                dgvRelatorios.DataSource = tabela;
                ConfigurarAlinhamentoColunas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar relatório: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarAlinhamentoColunas()
        {
            foreach (DataGridViewColumn col in dgvRelatorios.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (col.Name == "Código")
                {
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Data Entrada")
                {
                    col.FillWeight = 15;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Produto")
                {
                    col.FillWeight = 20;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (col.Name == "Qtd")
                {
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.Name == "Peso (g)")
                {
                    col.FillWeight = 12;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.Name == "Unidade")
                {
                    col.FillWeight = 12;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (col.Name == "Validade")
                {
                    col.FillWeight = 12;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Status")
                {
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Dias Restantes")
                {
                    col.Visible = false;
                }
            }
        }

        // ===== FORMATAÇÃO DE PESO =====
        private string FormatarPeso(decimal pesoGramas)
        {
            if (pesoGramas >= 1000000)
            {
                decimal toneladas = pesoGramas / 1000000;
                return toneladas.ToString("N3") + " t";
            }
            else if (pesoGramas >= 1000)
            {
                decimal kg = pesoGramas / 1000;
                return kg.ToString("N3") + " KG";
            }
            else
            {
                return pesoGramas.ToString("N0") + " g";
            }
        }

        // ===== EVENTOS DO DATAGRIDVIEW =====
        private void DgvRelatorios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRelatorios.Rows[e.RowIndex].Cells["Produto"].Value?.ToString() == "❖ TOTAL GERAL ❖")
                return;

            if (dgvRelatorios.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();

                switch (status)
                {
                    case "VENCIDO":
                        e.CellStyle.BackColor = Color.FromArgb(255, 200, 200);
                        e.CellStyle.ForeColor = Color.DarkRed;
                        e.CellStyle.Font = new Font(dgvRelatorios.Font, FontStyle.Bold);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(255, 150, 150);
                        break;
                    case "7 DIAS":
                        e.CellStyle.BackColor = Color.FromArgb(255, 230, 200);
                        e.CellStyle.ForeColor = Color.DarkOrange;
                        e.CellStyle.Font = new Font(dgvRelatorios.Font, FontStyle.Bold);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(255, 200, 150);
                        break;
                    case "15 DIAS":
                        e.CellStyle.BackColor = Color.FromArgb(255, 255, 200);
                        e.CellStyle.ForeColor = Color.Goldenrod;
                        e.CellStyle.Font = new Font(dgvRelatorios.Font, FontStyle.Bold);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(255, 255, 150);
                        break;
                    case "30 DIAS":
                        e.CellStyle.BackColor = Color.FromArgb(200, 230, 255);
                        e.CellStyle.ForeColor = Color.SteelBlue;
                        e.CellStyle.Font = new Font(dgvRelatorios.Font, FontStyle.Bold);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(150, 200, 255);
                        break;
                    default:
                        e.CellStyle.BackColor = Color.FromArgb(220, 255, 220);
                        e.CellStyle.ForeColor = Color.DarkGreen;
                        e.CellStyle.SelectionBackColor = Color.FromArgb(180, 255, 180);
                        break;
                }
            }
        }

        private void DgvRelatorios_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataTable tabela = (DataTable)dgvRelatorios.DataSource;
            if (tabela == null) return;

            try
            {
                DataRow row = tabela.Rows[e.RowIndex];
                string colunaEditada = dgvRelatorios.Columns[e.ColumnIndex].Name;
                int codProd = Convert.ToInt32(row["Código"]);

                using (var conexao = DataBaseConnection.OpenConnection())
                {
                    if (colunaEditada == "Validade")
                    {
                        if (DateTime.TryParse(row["Validade"].ToString(), out DateTime novaData))
                        {
                            string sqlUpdate = "UPDATE tbProdutos SET dataDeValidade = @data WHERE codProd = @codProd";
                            using (var cmd = new MySqlCommand(sqlUpdate, conexao))
                            {
                                cmd.Parameters.AddWithValue("@data", novaData.Date);
                                cmd.Parameters.AddWithValue("@codProd", codProd);
                                cmd.ExecuteNonQuery();
                            }
                            int dias = (novaData.Date - DateTime.Now.Date).Days;
                            row["Dias Restantes"] = dias;
                        }
                    }
                    else if (colunaEditada == "Qtd")
                    {
                        if (int.TryParse(row["Qtd"].ToString(), out int novaQtd))
                        {
                            string sqlUpdate = "UPDATE tbProdutos SET quantidade = @qtd WHERE codProd = @codProd";
                            using (var cmd = new MySqlCommand(sqlUpdate, conexao))
                            {
                                cmd.Parameters.AddWithValue("@qtd", novaQtd);
                                cmd.Parameters.AddWithValue("@codProd", codProd);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                if (row["Dias Restantes"] != DBNull.Value)
                {
                    int dias = Convert.ToInt32(row["Dias Restantes"]);
                    if (dias < 0)
                        row["Status"] = "VENCIDO";
                    else if (dias <= 7)
                        row["Status"] = "7 DIAS";
                    else if (dias <= 15)
                        row["Status"] = "15 DIAS";
                    else if (dias <= 30)
                        row["Status"] = "30 DIAS";
                    else
                        row["Status"] = "OK";
                }

                if (tabela.Rows.Count > 0 && tabela.Rows[tabela.Rows.Count - 1]["Produto"].ToString() == "❖ TOTAL GERAL ❖")
                    tabela.Rows.RemoveAt(tabela.Rows.Count - 1);

                AdicionarLinhaTotais(tabela);
                dgvRelatorios.Refresh();
                MessageBox.Show("Alteração salva com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar alteração: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CarregarDados();
            }
        }

        private void DgvRelatorios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvRelatorios.Rows.Count > 0)
            {
                int ultimaLinha = dgvRelatorios.Rows.Count - 1;
                if (dgvRelatorios.Rows[ultimaLinha].Cells["Produto"].Value?.ToString() == "❖ TOTAL GERAL ❖")
                {
                    dgvRelatorios.Rows[ultimaLinha].DefaultCellStyle.BackColor = Color.FromArgb(48, 112, 99);
                    dgvRelatorios.Rows[ultimaLinha].DefaultCellStyle.ForeColor = Color.White;
                    dgvRelatorios.Rows[ultimaLinha].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                    dgvRelatorios.Rows[ultimaLinha].ReadOnly = true;
                }
            }
        }

        // ===== LINHA DE TOTAIS =====
        private void AdicionarLinhaTotais(DataTable tabela)
        {
            if (tabela.Rows.Count == 0) return;

            decimal totalQtd = 0;
            decimal totalPesoGramas = 0;

            foreach (DataRow row in tabela.Rows)
            {
                if (row["Produto"].ToString() == "❖ TOTAL GERAL ❖") continue;

                decimal qtd = 0, peso = 0;
                if (row["Qtd"] != DBNull.Value) decimal.TryParse(row["Qtd"].ToString(), out qtd);
                if (row["Peso (g)"] != DBNull.Value) decimal.TryParse(row["Peso (g)"].ToString(), out peso);

                totalQtd += qtd;
                totalPesoGramas += (qtd * peso);
            }

            DataRow totalRow = tabela.NewRow();
            totalRow["Produto"] = "❖ TOTAL GERAL ❖";
            totalRow["Qtd"] = totalQtd.ToString("N0");
            totalRow["Peso (g)"] = totalPesoGramas;
            totalRow["Unidade"] = FormatarPeso(totalPesoGramas);
            totalRow["Status"] = "";
            totalRow["Validade"] = DBNull.Value;
            totalRow["Data Entrada"] = DBNull.Value;
            totalRow["Código"] = DBNull.Value;

            tabela.Rows.Add(totalRow);
        }

        // ===== GRID DE SAÍDAS =====
        private void CriarGridSaidasEstoque()
        {
            dgvSaidasEstoque = new DataGridView
            {
                Name = "dgvSaidasEstoque",
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            tabPage3.Controls.Add(dgvSaidasEstoque);
            ConfigurarColunasSaidas();
        }

        private void ConfigurarColunasSaidas()
        {
            dgvSaidasEstoque.Columns.Clear();
            dgvSaidasEstoque.Columns.Add("data", "Data");
            dgvSaidasEstoque.Columns.Add("produto", "Produto");
            dgvSaidasEstoque.Columns.Add("quantidade", "Quantidade");
            dgvSaidasEstoque.Columns.Add("destino", "Destino");
            dgvSaidasEstoque.Columns.Add("responsavel", "Responsável");

            dgvSaidasEstoque.Columns["quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSaidasEstoque.Columns["quantidade"].DefaultCellStyle.Format = "N0";

            dgvSaidasEstoque.EnableHeadersVisualStyles = false;
            dgvSaidasEstoque.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(48, 112, 99);
            dgvSaidasEstoque.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSaidasEstoque.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei", 10, FontStyle.Bold);
            dgvSaidasEstoque.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSaidasEstoque.ColumnHeadersHeight = 40;
        }

        private void CarregarProdutosSaida()
        {
            if (cmbProdutoSaida == null) return;
            cmbProdutoSaida.Items.Clear();
            cmbProdutoSaida.Items.Add("SELECIONE UM PRODUTO");

            using (var conexao = DataBaseConnection.OpenConnection())
            {
                string sql = "SELECT descricao FROM tbLista ORDER BY descricao";
                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        cmbProdutoSaida.Items.Add(reader["descricao"].ToString());
                }
            }
            cmbProdutoSaida.SelectedIndex = 0;
        }

        private void CarregarSaidasEstoque()
        {
            if (dgvSaidasEstoque == null) return;
            dgvSaidasEstoque.Rows.Clear();

            using (var conexao = DataBaseConnection.OpenConnection())
            {
                string sql = @"
                SELECT 
                    DATE_FORMAT(c.dataDeSaida, '%d/%m/%Y %H:%i') AS data,
                    l.descricao AS produto,
                    ic.quantidade,
                    'Doação' AS destino,
                    'Sistema' AS responsavel
                FROM tbItensCesta ic
                INNER JOIN tbCestas c ON c.codCes = ic.codCes
                INNER JOIN tbLista l ON l.codList = ic.codList
                ORDER BY c.dataDeSaida DESC
                LIMIT 500";

                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvSaidasEstoque.Rows.Add(
                            reader["data"],
                            reader["produto"],
                            Convert.ToDecimal(reader["quantidade"]).ToString("N0"),
                            reader["destino"],
                            reader["responsavel"]
                        );
                    }
                }
            }

            foreach (DataGridViewRow row in dgvSaidasEstoque.Rows)
            {
                row.DefaultCellStyle.BackColor = (row.Index % 2 == 0) ? Color.White : Color.FromArgb(250, 250, 250);
            }
        }

        private int ObterSaldoProduto(int codList, MySqlConnection conn)
        {
            string sql = "SELECT IFNULL(SUM(quantidade),0) FROM tbProdutos WHERE codList = @codList";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@codList", codList);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ===== MÉTODOS DE IMPORTAÇÃO =====
        private void BtnImportarDados_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivos CSV (*.csv)|*.csv|Arquivos Excel (*.xlsx;*.xls)|*.xlsx;*.xls";
                openFileDialog.Title = "Importar Dados de Estoque";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string extensao = Path.GetExtension(openFileDialog.FileName).ToLower();
                        DataTable dadosImportados = new DataTable();

                        if (extensao == ".csv")
                            dadosImportados = ImportarCSV(openFileDialog.FileName);
                        else if (extensao == ".xlsx" || extensao == ".xls")
                            dadosImportados = ImportarExcel(openFileDialog.FileName);

                        if (dadosImportados != null && dadosImportados.Rows.Count > 0)
                        {
                            VisualizarProdutosDoArquivo(dadosImportados);
                            DialogResult result = MessageBox.Show(
                                $"{dadosImportados.Rows.Count} registro(s) encontrado(s). Deseja importar para o estoque?",
                                "Confirmar Importação",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                int registrosImportados = SalvarDadosImportados(dadosImportados);
                                if (registrosImportados > 0) CarregarDados();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao importar dados: " + ex.Message, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private DataTable ImportarCSV(string caminhoArquivo)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(caminhoArquivo, Encoding.Default))
            {
                string[] cabecalhos = sr.ReadLine().Split(';');
                foreach (string cabecalho in cabecalhos)
                    dt.Columns.Add(cabecalho.Trim());

                while (!sr.EndOfStream)
                {
                    string[] linhas = sr.ReadLine().Split(';');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < cabecalhos.Length; i++)
                        if (i < linhas.Length) dr[i] = linhas[i].Trim();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private DataTable ImportarExcel(string caminhoArquivo)
        {
            using (var workbook = new XLWorkbook(caminhoArquivo))
            {
                var worksheet = workbook.Worksheet(1);
                DataTable dt = new DataTable();
                dt.Columns.Add("Data Entrada", typeof(string));
                dt.Columns.Add("Origem", typeof(string));
                dt.Columns.Add("Produto", typeof(string));
                dt.Columns.Add("Quantidade", typeof(string));
                dt.Columns.Add("Validade", typeof(string));

                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
                foreach (var row in rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["Data Entrada"] = row.Cell(1).Value.ToString();
                    dr["Origem"] = row.Cell(2).Value.ToString();
                    dr["Produto"] = row.Cell(3).Value.ToString();
                    dr["Quantidade"] = row.Cell(4).Value.ToString();
                    dr["Validade"] = row.Cell(5).Value.ToString();
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }

        private void VisualizarProdutosDoArquivo(DataTable dados)
        {
            var produtosUnicos = new HashSet<string>();
            foreach (DataRow row in dados.Rows)
            {
                string produto = row["Produto"]?.ToString()?.Trim().ToUpper() ?? "";
                if (!string.IsNullOrEmpty(produto))
                    produtosUnicos.Add(produto);
            }
            string listaProdutos = string.Join("\n", produtosUnicos.OrderBy(p => p));
            MessageBox.Show($"Produtos encontrados no arquivo ({produtosUnicos.Count}):\n\n{listaProdutos}",
                "Produtos do Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            byte[] bytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(texto);
            return Encoding.UTF8.GetString(bytes);
        }

        private int SalvarDadosImportados(DataTable dados)
        {
            int registrosInseridos = 0;
            int erros = 0;
            StringBuilder produtosNaoEncontrados = new StringBuilder();

            using (var conexao = DataBaseConnection.OpenConnection())
            {
                // ===== 1. DESCOBRIR O ID DO USUÁRIO ADMIN =====
                int codUsu = 0;
                string sqlBuscaUsu = "SELECT codUsu FROM tbUsuarios WHERE usuario = 'admin' LIMIT 1";
                using (MySqlCommand cmd = new MySqlCommand(sqlBuscaUsu, conexao))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        codUsu = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Se não existir, tenta criar
                        try
                        {
                            // Primeiro cria voluntário
                            string sqlVoluntario = "INSERT INTO tbVoluntarios (nome) VALUES ('Admin'); SELECT LAST_INSERT_ID();";
                            int codVol = 0;
                            using (MySqlCommand cmdVol = new MySqlCommand(sqlVoluntario, conexao))
                            {
                                codVol = Convert.ToInt32(cmdVol.ExecuteScalar());
                            }

                            // Depois cria usuário
                            string sqlNovoUsu = "INSERT INTO tbUsuarios (usuario, senha, tipo, codVol) VALUES ('admin', '123', 'ADMIN', @codVol); SELECT LAST_INSERT_ID();";
                            using (MySqlCommand cmdUsu = new MySqlCommand(sqlNovoUsu, conexao))
                            {
                                cmdUsu.Parameters.AddWithValue("@codVol", codVol);
                                codUsu = Convert.ToInt32(cmdUsu.ExecuteScalar());
                            }

                            MessageBox.Show("Usuário admin criado com ID: " + codUsu, "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao criar usuário: " + ex.Message, "Erro");
                            return 0;
                        }
                    }
                }

                // ===== 2. DESCOBRIR O ID DA ORIGEM ROTARY =====
                int codOri = 0;
                string sqlBuscaOri = "SELECT codOri FROM tbOrigemDoacao WHERE nome = 'ROTARY' LIMIT 1";
                using (MySqlCommand cmd = new MySqlCommand(sqlBuscaOri, conexao))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        codOri = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Se não existir, cria
                        try
                        {
                            string sqlNovaOri = "INSERT INTO tbOrigemDoacao (nome) VALUES ('ROTARY'); SELECT LAST_INSERT_ID();";
                            using (MySqlCommand cmdOri = new MySqlCommand(sqlNovaOri, conexao))
                            {
                                codOri = Convert.ToInt32(cmdOri.ExecuteScalar());
                            }
                            MessageBox.Show("Origem ROTARY criada com ID: " + codOri, "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao criar origem: " + ex.Message, "Erro");
                            return 0;
                        }
                    }
                }

                // ===== 3. CARREGAR PRODUTOS DO BANCO =====
                Dictionary<string, int> produtosBanco = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                string sql = "SELECT codList, descricao FROM tbLista";
                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string descricao = reader["descricao"].ToString().Trim();
                        int codList = Convert.ToInt32(reader["codList"]);
                        produtosBanco[descricao] = codList;
                    }
                }

                // ===== 4. MOSTRAR OS IDs QUE SERÃO USADOS =====
                MessageBox.Show($"Importando com:\nUsuário ID: {codUsu}\nOrigem ID: {codOri}", "IDs Utilizados");

                // ===== 5. PROCESSAR CADA LINHA DO EXCEL =====
                foreach (DataRow row in dados.Rows)
                {
                    try
                    {
                        string produtoArquivo = row["Produto"]?.ToString()?.Trim() ?? "";
                        if (string.IsNullOrEmpty(produtoArquivo)) continue;

                        int? codListEncontrado = null;
                        if (produtosBanco.ContainsKey(produtoArquivo))
                        {
                            codListEncontrado = produtosBanco[produtoArquivo];
                        }
                        else
                        {
                            foreach (var prodBanco in produtosBanco)
                            {
                                string prodBancoNormalizado = RemoverAcentos(prodBanco.Key).Replace(" ", "").ToUpper();
                                string prodArquivoNormalizado = RemoverAcentos(produtoArquivo).Replace(" ", "").ToUpper();
                                if (prodBancoNormalizado.Contains(prodArquivoNormalizado) ||
                                    prodArquivoNormalizado.Contains(prodBancoNormalizado))
                                {
                                    codListEncontrado = prodBanco.Value;
                                    break;
                                }
                            }
                        }

                        if (codListEncontrado.HasValue)
                        {
                            string quantidade = row["Quantidade"]?.ToString()?.Trim() ?? "0";
                            string validade = row["Validade"]?.ToString()?.Trim() ?? "";
                            string dataEntrada = row["Data Entrada"]?.ToString()?.Trim() ?? "";
                            if (string.IsNullOrEmpty(dataEntrada))
                                dataEntrada = DateTime.Now.ToString("yyyy-MM-dd");

                            string sqlInsert = @"INSERT INTO tbProdutos 
                        (codList, quantidade, dataDeEntrada, dataDeValidade, descricao, peso, unidade, dataLimiteDeSaida, codUsu, codOri, tipoMovimentacao) 
                        VALUES (@codList, @quantidade, @dataEntrada, @validade, '', 0, '', @validade, @codUsu, @codOri, 'ENTRADA')";

                            using (var cmdInsert = new MySqlCommand(sqlInsert, conexao))
                            {
                                cmdInsert.Parameters.AddWithValue("@codList", codListEncontrado.Value);
                                cmdInsert.Parameters.AddWithValue("@quantidade", Convert.ToInt32(Convert.ToDecimal(quantidade)));
                                cmdInsert.Parameters.AddWithValue("@dataEntrada", Convert.ToDateTime(dataEntrada));
                                cmdInsert.Parameters.AddWithValue("@validade", Convert.ToDateTime(validade));
                                cmdInsert.Parameters.AddWithValue("@codUsu", codUsu);
                                cmdInsert.Parameters.AddWithValue("@codOri", codOri);
                                cmdInsert.ExecuteNonQuery();
                                registrosInseridos++;
                            }
                        }
                        else
                        {
                            if (!produtosNaoEncontrados.ToString().Contains(produtoArquivo))
                                produtosNaoEncontrados.AppendLine($"• {produtoArquivo}");
                            erros++;
                        }
                    }
                    catch (Exception ex)
                    {
                        erros++;
                        MessageBox.Show($"Erro na linha: {ex.Message}\nProduto: {row["Produto"]}", "Erro");
                    }
                }
            }

            string mensagem = $"Total de registros: {dados.Rows.Count}\n" +
                             $"Importados com sucesso: {registrosInseridos}\nErros: {erros}";

            if (produtosNaoEncontrados.Length > 0)
                mensagem += $"\n\nProdutos não encontrados:\n{produtosNaoEncontrados}";

            MessageBox.Show(mensagem, "Resultado da Importação",
                MessageBoxButtons.OK, registrosInseridos > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            return registrosInseridos;
        }


        // ===== BOTÕES DE AÇÃO =====
        private void BtnRegistrarSaida_Click(object sender, EventArgs e)
        {
            if (cmbProdutoSaida.SelectedIndex <= 0)
            {
                MessageBox.Show("Selecione um produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantidade = (int)nudQuantidadeSaida.Value;
            if (quantidade <= 0)
            {
                MessageBox.Show("Informe uma quantidade válida.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string produto = cmbProdutoSaida.SelectedItem.ToString();

            using (var conn = DataBaseConnection.OpenConnection())
            {
                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlProduto = "SELECT codList FROM tbLista WHERE descricao = @descricao LIMIT 1";
                    int codList;
                    using (MySqlCommand cmd = new MySqlCommand(sqlProduto, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@descricao", produto);
                        codList = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    int saldo = ObterSaldoProduto(codList, conn);
                    if (saldo < quantidade)
                    {
                        MessageBox.Show("Estoque insuficiente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        trans.Rollback();
                        return;
                    }

                    string sqlSaida = @"INSERT INTO tbProdutos
                                (codList, quantidade, dataDeEntrada, tipoMovimentacao)
                                VALUES (@codList, @quantidade, NOW(), 'SAIDA')";
                    using (MySqlCommand cmd = new MySqlCommand(sqlSaida, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@codList", codList);
                        cmd.Parameters.AddWithValue("@quantidade", quantidade * -1);
                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                    MessageBox.Show("Saída registrada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    nudQuantidadeSaida.Value = 1;
                    txtDestinoSaida.Clear();
                    CarregarDados();
                    CarregarSaidasEstoque();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e) => CarregarDados();

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
            dtpDataFinalPeriodo.Value = DateTime.Now;
            cbxProduto.SelectedIndex = 0;
            cbbUsuario.SelectedIndex = 0;
            cbxStatus.SelectedIndex = 0;
            CarregarDados();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo Excel (*.xlsx)|*.xlsx";
                    sfd.FileName = $"Relatorio_Estoque_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Relatório de Estoque");

                            for (int i = 0; i < dgvRelatorios.Columns.Count; i++)
                            {
                                worksheet.Cell(1, i + 1).Value = dgvRelatorios.Columns[i].HeaderText;
                                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(48, 112, 99);
                                worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                            }

                            for (int i = 0; i < dgvRelatorios.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvRelatorios.Columns.Count; j++)
                                {
                                    var cell = dgvRelatorios.Rows[i].Cells[j];
                                    worksheet.Cell(i + 2, j + 1).Value = cell.Value?.ToString() ?? "";
                                    if (dgvRelatorios.Rows[i].Cells["Produto"].Value?.ToString() == "❖ TOTAL GERAL ❖")
                                    {
                                        worksheet.Cell(i + 2, j + 1).Style.Font.Bold = true;
                                        worksheet.Cell(i + 2, j + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(48, 112, 99);
                                        worksheet.Cell(i + 2, j + 1).Style.Font.FontColor = XLColor.White;
                                    }
                                }
                            }
                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Relatório exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            var menu = new frmMenuPrincipal(_codUsuLogado);
            menu.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cadastro = new frmGerenciarProdutos(_codUsuLogado);
            cadastro.Show();
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage3)
                CarregarSaidasEstoque();
        }
    }
}