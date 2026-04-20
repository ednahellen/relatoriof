using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using ExcelDataReader;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmEstoque : Form
    {
        // ===== Variáveis =====
        private string produtoSelecionado = "";
        private bool modoAgrupado = true;
        private bool somentePrincipais = false;
        private int codUsuLogado;
        private Label lblSaldoAtual;
        private Label lblProdutoSelecionado;
        private ComboBox cmbProdutoSaida;
        private NumericUpDown numQuantidadeSaida;
        private TextBox txtDestinoSaida;
        private Button btnRegistrarSaida;
        private DataGridView dgvHistoricoSaidas;

        // Lista de produtos da cesta básica
        private List<string> produtosPrincipais = new List<string>
        {
            "ARROZ 1KG", "ARROZ 2KG", "ARROZ 5KG", "FEIJAO 1KG", "MOLHO DE TOMATE",
            "MACARRAO 500G", "ACUCAR 1KG", "SAL 1KG", "OLEO 900ML", "FUBA 400G", "FUBA 500G", "LEITE 1L"
        };

        public frmEstoque()
        {
            InitializeComponent();
            this.Load += frmEstoque_Load;
        }

        public frmEstoque(int codUsu) : this()
        {
            codUsuLogado = codUsu;
        }

        private void frmEstoque_Load(object sender, EventArgs e)
        {
            btnAplicarFiltros.Click += btnAplicarFiltros_Click;
            btnLimparFiltros.Click += btnLimparFiltros_Click;
            btnProdutosPrincipais.Click += btnProdutosPrincipais_Click;
            btnImportar.Click += BtnImportar_Click;
            btnExportar.Click += BtnExportar_Click;
            btnMenu.Click += btnMenu_Click;
            btnCadastrar.Click += btnCadastrar_Click;
            dgvEstoque.CellFormatting += dgvEstoque_CellFormatting;

            ConfigurarDataGridView();
            CarregarProdutos();
            CarregarDados();
            ConfigurarAbaHistorico();
            ConfigurarTabRegistrarSaida();
            CarregarProdutosSaida();
            VerificacaoSistema();

            Button btnSincronizar = new Button
            {
                Text = "📥 Sincronizar com Planilha",
                Location = new Point(700, 10),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSincronizar.Click += btnSincronizarEstoque_Click;
            this.Controls.Add(btnSincronizar);
        }

        private void ConfigurarDataGridView()
        {
            dgvEstoque.Columns.Clear();
            dgvEstoque.Rows.Clear();
            dgvEstoque.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstoque.AllowUserToAddRows = false;
            dgvEstoque.ReadOnly = true;
            dgvEstoque.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvEstoque.Columns.Add("Produto", "Produto");
            dgvEstoque.Columns.Add("Quantidade", "Qtd");
            dgvEstoque.Columns.Add("Unidade", "Unid");
            dgvEstoque.Columns.Add("Peso", "Peso (g)");
            dgvEstoque.Columns.Add("PesoTotal", "Peso Total (kg)");
            dgvEstoque.Columns.Add("Status", "Status");
            dgvEstoque.Columns.Add("Validade", "Validade");
            dgvEstoque.Columns.Add("Origem", "Origem");
        }

        private void CarregarDados()
        {
            dgvEstoque.Rows.Clear();

            int totalQuantidade = 0;
            decimal pesoTotalGramas = 0;
            int totalProdutos = 0;

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"
        SELECT 
            l.codList,
            l.descricao AS produto,
            l.unidade,
            l.peso,
            COALESCE((
                SELECT SUM(p.quantidade)
                FROM tbProdutos p
                WHERE p.codList = l.codList
            ), 0) AS quantidade,
            (
                SELECT MIN(p.dataDeValidade) 
                FROM tbProdutos p 
                WHERE p.codList = l.codList 
                  AND p.quantidade > 0
            ) AS validade,
            (
                SELECT o.nome 
                FROM tbProdutos p
                INNER JOIN tbOrigemDoacao o ON o.codOri = p.codOri
                WHERE p.codList = l.codList 
                  AND p.quantidade > 0
                ORDER BY p.dataDeEntrada DESC
                LIMIT 1
            ) AS origem
        FROM tbLista l
        HAVING quantidade > 0
        ORDER BY l.descricao";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int qtd = Convert.ToInt32(reader["quantidade"]);
                        if (qtd <= 0) continue;

                        decimal pesoUnitario = Convert.ToDecimal(reader["peso"]);
                        decimal pesoTotalProduto = qtd * pesoUnitario;

                        DateTime? validade = reader["validade"] != DBNull.Value
                            ? Convert.ToDateTime(reader["validade"])
                            : (DateTime?)null;

                        string status = CalcularStatus(validade);
                        string origem = reader["origem"] != DBNull.Value
                            ? reader["origem"].ToString()
                            : "Nao informado";

                        totalQuantidade += qtd;
                        pesoTotalGramas += pesoTotalProduto;
                        totalProdutos++;

                        int rowIndex = dgvEstoque.Rows.Add(
                            reader["produto"],
                            qtd,
                            reader["unidade"].ToString(),
                            pesoUnitario.ToString("0"),
                            (pesoTotalProduto / 1000m).ToString("0.00"),
                            status,
                            FormatarData(validade),
                            origem
                        );

                        if (status == "Vencido")
                            dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                        else if (status == "Proximo")
                            dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Khaki;
                        else if (status == "Valido")
                            dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Honeydew;
                    }
                }
            }

            // Sempre adicionar o TOTAL, mesmo que não haja linhas
            string pesoTotalFormatado = (pesoTotalGramas / 1000m).ToString("0.00") + " kg";

            int linhaTotal = dgvEstoque.Rows.Add(
                ">>> TOTAL GERAL",
                totalQuantidade.ToString("N0"),
                totalProdutos.ToString() + " tipos",
                "",
                pesoTotalFormatado,
                "",
                "",
                ""
            );

            dgvEstoque.Rows[linhaTotal].DefaultCellStyle.BackColor = Color.DarkSlateGray;
            dgvEstoque.Rows[linhaTotal].DefaultCellStyle.ForeColor = Color.White;
            dgvEstoque.Rows[linhaTotal].DefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        }


        private void CarregarProdutos()
        {
            cbxprodutoSelecionado.Items.Clear();
            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = "SELECT descricao FROM tbLista ORDER BY descricao";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        cbxprodutoSelecionado.Items.Add(reader["descricao"].ToString());
                }
            }
            cbxprodutoSelecionado.Items.Insert(0, "Todos os produtos");
            cbxprodutoSelecionado.SelectedIndex = 0;
        }

        private string CalcularStatus(DateTime? validade)
        {
            if (!validade.HasValue) return "Sem validade";
            int dias = (validade.Value - DateTime.Today).Days;
            if (dias < 0) return "Vencido";
            if (dias <= 60) return "Proximo";
            return "Valido";
        }

        private string FormatarData(object data)
        {
            if (data == null || data == DBNull.Value) return "";
            if (DateTime.TryParse(data.ToString(), out DateTime dt))
                return dt.ToString("dd/MM/yyyy");
            return "";
        }

        private void CarregarProdutosSaida()
        {
            if (cmbProdutoSaida == null) return;
            cmbProdutoSaida.Items.Clear();

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"
            SELECT l.descricao, 
                   COALESCE(SUM(p.quantidade), 0) as saldo
            FROM tbLista l
            LEFT JOIN tbProdutos p ON p.codList = l.codList
            GROUP BY l.codList, l.descricao
            HAVING saldo > 0
            ORDER BY l.descricao";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nome = reader["descricao"].ToString();
                        int qtd = Convert.ToInt32(reader["saldo"]);
                        cmbProdutoSaida.Items.Add($"{nome} | Estoque: {qtd}");
                    }
                }
            }

            if (cmbProdutoSaida.Items.Count == 0)
                cmbProdutoSaida.Items.Add("Nenhum produto com estoque");
        }


        private void CmbProdutoSaida_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProdutoSaida.SelectedItem == null) return;

            string item = cmbProdutoSaida.SelectedItem.ToString();
            if (item == "Nenhum produto com estoque") return;

            string produto = item.Split('|')[0].Trim();
            lblProdutoSelecionado.Text = produto;

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"
            SELECT COALESCE(SUM(p.quantidade), 0)
            FROM tbProdutos p
            INNER JOIN tbLista l ON l.codList = p.codList
            WHERE l.descricao = @produto";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@produto", produto);

                    object result = cmd.ExecuteScalar();
                    int saldo = result != null ? Convert.ToInt32(result) : 0;

                    lblSaldoAtual.Text = saldo.ToString();
                    numQuantidadeSaida.Maximum = saldo > 0 ? saldo : 0;
                    numQuantidadeSaida.Value = 0;
                }
            }
        }

        private void RegistrarSaida(string produto, int quantidade, string destino)
        {
            using (var conn = DataBaseConnection.OpenConnection())
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    int codList = 0;
                    int peso = 0;

                    // Buscar produto
                    using (var cmd = new MySqlCommand("SELECT codList, peso FROM tbLista WHERE descricao = @produto", conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@produto", produto);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                codList = Convert.ToInt32(reader["codList"]);
                                peso = Convert.ToInt32(reader["peso"]);
                            }
                        }
                    }

                    if (codList == 0)
                        throw new Exception($"Produto '{produto}' não encontrado.");

                    // Saldo atual
                    int saldoAtual = 0;
                    string sqlSaldo = @"
                SELECT 
                    COALESCE(SUM(CASE WHEN tipoMovimentacao = 'ENTRADA' THEN quantidade ELSE 0 END),0)
                    -
                    COALESCE(SUM(CASE WHEN tipoMovimentacao = 'SAIDA' THEN ABS(quantidade) ELSE 0 END),0)
                FROM tbProdutos
                WHERE codList = @codList";

                    using (var cmd = new MySqlCommand(sqlSaldo, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@codList", codList);
                        saldoAtual = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (saldoAtual < quantidade)
                        throw new Exception($"Estoque insuficiente!\nDisponível: {saldoAtual}");

                    // Origem
                    int codOri = 1;
                    using (var cmd = new MySqlCommand("SELECT codOri FROM tbOrigemDoacao LIMIT 1", conn, trans))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null) codOri = Convert.ToInt32(result);
                    }

                    // Inserir saída
                    string sqlInsert = @"
                INSERT INTO tbProdutos 
                    (descricao, quantidade, peso, unidade, dataDeEntrada, 
                     dataDeValidade, dataLimiteDeSaida, tipoMovimentacao, 
                     codUsu, codOri, codList, destino)
                VALUES 
                    (@descricao, @quantidade, @peso, 'UNIDADES (UN)', NOW(),
                     DATE_ADD(NOW(), INTERVAL 30 DAY), DATE_ADD(NOW(), INTERVAL 60 DAY),
                     'SAIDA', @codUsu, @codOri, @codList, @destino)";

                    using (var cmd = new MySqlCommand(sqlInsert, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@descricao", produto);
                        cmd.Parameters.AddWithValue("@quantidade", -quantidade);
                        cmd.Parameters.AddWithValue("@peso", peso);
                        cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
                        cmd.Parameters.AddWithValue("@codOri", codOri);
                        cmd.Parameters.AddWithValue("@codList", codList);
                        cmd.Parameters.AddWithValue("@destino", string.IsNullOrEmpty(destino) ? "Não informado" : destino);
                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();

                    int novoSaldo = saldoAtual - quantidade;

                    // ✅ AGORA SIM (thread correta)
                    MessageBox.Show(
                        $"Saída registrada!\n\nProduto: {produto}\nQtd: {quantidade}\nSaldo anterior: {saldoAtual}\nNovo saldo: {novoSaldo}",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    CarregarDados();
                    CarregarProdutosSaida();
                    CarregarHistoricoSaidas("");
                }
                catch (Exception ex)
                {
                    if (trans.Connection != null)
                        trans.Rollback();

                    MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnRegistrarSaida_Click(object sender, EventArgs e)
        {
            btnRegistrarSaida.Enabled = false;
            btnRegistrarSaida.Text = "Processando...";

            try
            {
                if (cmbProdutoSaida.SelectedItem == null)
                {
                    MessageBox.Show("Selecione um produto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string item = cmbProdutoSaida.SelectedItem.ToString();
                if (item == "Nenhum produto com estoque")
                {
                    MessageBox.Show("Não há produtos com estoque disponível.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int qtd = (int)numQuantidadeSaida.Value;
                if (qtd <= 0)
                {
                    MessageBox.Show("Informe uma quantidade válida (maior que zero).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string produto = item.Split('|')[0].Trim();
                string destino = txtDestinoSaida.Text.Trim();

                RegistrarSaida(produto, qtd, destino);

                txtDestinoSaida.Clear();
                numQuantidadeSaida.Value = 0;
                lblSaldoAtual.Text = "0";
                lblProdutoSelecionado.Text = "-";
                cmbProdutoSaida.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegistrarSaida.Enabled = true;
                btnRegistrarSaida.Text = "📦 REGISTRAR SAÍDA";
            }
        }

        private void ConfigurarTabRegistrarSaida()
        {
            if (tabPageRegistrarSaida == null) return;
            tabPageRegistrarSaida.Controls.Clear();

            GroupBox grpSelecao = new GroupBox
            {
                Text = " SELEÇÃO DO PRODUTO ",
                Location = new Point(20, 20),
                Size = new Size(450, 130),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(250, 250, 250)
            };

            Label lblProduto = new Label { Text = "Produto:", Location = new Point(15, 30), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
            cmbProdutoSaida = new ComboBox { Location = new Point(100, 28), Size = new Size(320, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbProdutoSaida.SelectedIndexChanged += CmbProdutoSaida_SelectedIndexChanged;

            Label lblSelecionado = new Label { Text = "Selecionado:", Location = new Point(15, 60), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
            lblProdutoSelecionado = new Label { Text = "-", Location = new Point(100, 60), Size = new Size(320, 25), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Navy };
            Label lblSaldo = new Label { Text = "Saldo:", Location = new Point(15, 90), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
            lblSaldoAtual = new Label { Text = "0", Location = new Point(100, 90), Size = new Size(150, 25), Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DarkGreen };

            grpSelecao.Controls.AddRange(new Control[] { lblProduto, cmbProdutoSaida, lblSelecionado, lblProdutoSelecionado, lblSaldo, lblSaldoAtual });

            GroupBox grpSaida = new GroupBox
            {
                Text = " DADOS DA SAÍDA ",
                Location = new Point(20, 160),
                Size = new Size(450, 120),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(250, 250, 250)
            };

            Label lblQtd = new Label { Text = "Quantidade:", Location = new Point(15, 30), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
            numQuantidadeSaida = new NumericUpDown { Location = new Point(100, 28), Size = new Size(120, 25), Minimum = 0, Maximum = 100000, Value = 0, Font = new Font("Segoe UI", 10) };
            Label lblDestino = new Label { Text = "Destino:", Location = new Point(15, 65), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
            txtDestinoSaida = new TextBox { Location = new Point(100, 63), Size = new Size(320, 25), Font = new Font("Segoe UI", 10) };

            grpSaida.Controls.AddRange(new Control[] { lblQtd, numQuantidadeSaida, lblDestino, txtDestinoSaida });

            btnRegistrarSaida = new Button
            {
                Text = "📦 REGISTRAR SAÍDA",
                Location = new Point(20, 300),
                Size = new Size(200, 40),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRegistrarSaida.Click += BtnRegistrarSaida_Click;

            tabPageRegistrarSaida.Controls.AddRange(new Control[] { grpSelecao, grpSaida, btnRegistrarSaida });
        }

        private void ConfigurarAbaHistorico()
        {
            if (tabPageSaidas == null) return;
            tabPageSaidas.Controls.Clear();

            dgvHistoricoSaidas = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 10)
            };
            dgvHistoricoSaidas.EnableHeadersVisualStyles = false;
            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvHistoricoSaidas.ColumnHeadersHeight = 35;

            dgvHistoricoSaidas.Columns.Add("data", "Data/Hora");
            dgvHistoricoSaidas.Columns.Add("produto", "Produto");
            dgvHistoricoSaidas.Columns.Add("quantidade", "Qtd");
            dgvHistoricoSaidas.Columns.Add("usuario", "Usuário");
            dgvHistoricoSaidas.Columns.Add("destino", "Destino");
            dgvHistoricoSaidas.Columns["quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            tabPageSaidas.Controls.Add(dgvHistoricoSaidas);
            CarregarHistoricoSaidas("");
        }

        private void CarregarHistoricoSaidas(string filtro = "")
        {
            if (dgvHistoricoSaidas == null) return;
            dgvHistoricoSaidas.Rows.Clear();

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"
                    SELECT DATE_FORMAT(p.dataDeEntrada, '%d/%m/%Y %H:%i') as data,
                           l.descricao as produto, ABS(p.quantidade) as quantidade,
                           u.usuario, p.destino
                    FROM tbProdutos p
                    INNER JOIN tbLista l ON l.codList = p.codList
                    INNER JOIN tbUsuarios u ON u.codUsu = p.codUsu
                    WHERE p.tipoMovimentacao = 'SAIDA'";

                if (!string.IsNullOrEmpty(filtro))
                    sql += " AND l.descricao LIKE @filtro";
                sql += " ORDER BY p.dataDeEntrada DESC LIMIT 500";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(filtro))
                        cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            dgvHistoricoSaidas.Rows.Add(reader["data"], reader["produto"], reader["quantidade"], reader["usuario"], reader["destino"]);
                    }
                }
            }
        }

        // ==================== EVENTOS DOS BOTÕES ====================

        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            produtoSelecionado = cbxprodutoSelecionado.SelectedIndex == 0 ? "" : cbxprodutoSelecionado.Text;
            somentePrincipais = false;
            CarregarDados();
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "";
            cbxprodutoSelecionado.SelectedIndex = 0;
            somentePrincipais = false;
            CarregarProdutos();
            CarregarDados();
            btnProdutosPrincipais.BackColor = SystemColors.Control;
        }

        private void btnProdutosPrincipais_Click(object sender, EventArgs e)
        {
            somentePrincipais = true;
            produtoSelecionado = "";
            cbxprodutoSelecionado.Items.Clear();
            cbxprodutoSelecionado.Items.Add("Todos os produtos");
            foreach (var produto in produtosPrincipais)
                cbxprodutoSelecionado.Items.Add(produto);
            cbxprodutoSelecionado.SelectedIndex = 0;
            CarregarDados();
            btnProdutosPrincipais.BackColor = Color.LightGreen;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal menu = new frmMenuPrincipal(codUsuLogado);
            menu.Show();
            this.Close();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal menu = new frmMenuPrincipal(codUsuLogado);
            menu.Show();
            this.Close();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void VerificacaoSistema()
        {
            using (var conn = DataBaseConnection.OpenConnection())
            {
                int total = Convert.ToInt32(new MySqlCommand("SELECT COUNT(*) FROM tbLista", conn).ExecuteScalar());
                if (total == 0)
                    MessageBox.Show("Nenhum produto cadastrado.");
            }
        }

        private void dgvEstoque_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) { }

        // ==================== MÉTODOS DE IMPORTAÇÃO/EXPORTAÇÃO ====================

        private void BtnImportar_Click(object sender, EventArgs e)
        {
            // Mantenha seu código de importação existente
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            // Mantenha seu código de exportação existente
        }

        private void btnSincronizarEstoque_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Arquivos Excel|*.xlsx;*.xls",
                    Title = "Selecione a planilha com o estoque corrigido"
                };

                if (ofd.ShowDialog() != DialogResult.OK) return;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var fileStream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                using (var excelReader = ExcelReaderFactory.CreateReader(fileStream))
                {
                    var config = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                    };

                    var dataSet = excelReader.AsDataSet(config);
                    DataTable tabela = dataSet.Tables[0];

                    if (!tabela.Columns.Contains("Produto") || !tabela.Columns.Contains("Qtd"))
                    {
                        MessageBox.Show("A planilha precisa ter as colunas: Produto e Qtd");
                        return;
                    }

                    int ajustesRealizados = 0;
                    StringBuilder log = new StringBuilder();

                    using (var conexao = DataBaseConnection.OpenConnection())
                    using (var transacao = conexao.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow linha in tabela.Rows)
                            {
                                string nomeProduto = linha["Produto"]?.ToString()?.Trim();
                                string qtdStr = linha["Qtd"]?.ToString()?.Trim();

                                if (string.IsNullOrEmpty(nomeProduto) || nomeProduto.ToUpper().Contains("TOTAL"))
                                    continue;

                                if (!int.TryParse(qtdStr, out int quantidadeCorreta))
                                    continue;

                                // 🔹 BUSCA CODIGO DO PRODUTO
                                int codigoProduto;
                                string sqlBusca = "SELECT codList FROM tbLista WHERE TRIM(descricao) = @produto";

                                using (var cmd = new MySqlCommand(sqlBusca, conexao, transacao))
                                {
                                    cmd.Parameters.AddWithValue("@produto", nomeProduto);
                                    var result = cmd.ExecuteScalar();

                                    if (result == null)
                                    {
                                        log.AppendLine($"Produto não encontrado: {nomeProduto}");
                                        continue;
                                    }

                                    codigoProduto = Convert.ToInt32(result);
                                }

                                // 🔹 SALDO REAL
                                int saldoAtual;
                                string sqlSaldo = @"
                            SELECT COALESCE(SUM(quantidade), 0)
                            FROM tbProdutos
                            WHERE codList = @codList";

                                using (var cmd = new MySqlCommand(sqlSaldo, conexao, transacao))
                                {
                                    cmd.Parameters.AddWithValue("@codList", codigoProduto);
                                    saldoAtual = Convert.ToInt32(cmd.ExecuteScalar());
                                }

                                // 🔹 DIFERENÇA
                                int diferenca = quantidadeCorreta - saldoAtual;

                                // 🔹 AJUSTE REAL
                                if (diferenca != 0)
                                {
                                    string sqlInsert = @"
                                INSERT INTO tbProdutos
                                    (descricao, quantidade, peso, unidade, dataDeEntrada,
                                     dataDeValidade, dataLimiteDeSaida, tipoMovimentacao,
                                     codUsu, codOri, codList, destino)
                                VALUES
                                    ((SELECT descricao FROM tbLista WHERE codList = @codList),
                                     @quantidade,
                                     (SELECT peso FROM tbLista WHERE codList = @codList),
                                     'UNIDADES (UN)',
                                     NOW(),
                                     DATE_ADD(NOW(), INTERVAL 30 DAY),
                                     DATE_ADD(NOW(), INTERVAL 60 DAY),
                                     'AJUSTE',
                                     @codUsu,
                                     1,
                                     @codList,
                                     'AJUSTE AUTOMATICO (SINCRONIZACAO EXCEL)')";

                                    using (var cmd = new MySqlCommand(sqlInsert, conexao, transacao))
                                    {
                                        cmd.Parameters.AddWithValue("@codList", codigoProduto);
                                        cmd.Parameters.AddWithValue("@quantidade", diferenca);
                                        cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
                                        cmd.ExecuteNonQuery();
                                    }

                                    ajustesRealizados++;
                                    log.AppendLine($"OK: {nomeProduto} | Ajuste: {diferenca}");
                                }
                            }

                            transacao.Commit();

                            CarregarDados();
                            CarregarProdutosSaida();
                            CarregarHistoricoSaidas("");

                            MessageBox.Show(
                                $"Sincronização concluída.\n\nAjustes: {ajustesRealizados}\n\n{log}",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        catch (Exception ex)
                        {
                            transacao.Rollback();
                            MessageBox.Show($"Erro na sincronização:\n{ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir arquivo:\n{ex.Message}");
            }
        }

        private void ExportarHistoricoSaidasCSV() { }
        private void ExportarHistoricoSaidasExcel() { }
        private void ExportarEstoqueCSV() { }
        private void ExportarEstoqueExcel() { }
        private void ImprimirEstoque() { }
        private void ImprimirHistoricoSaidas() { }
    }
}

