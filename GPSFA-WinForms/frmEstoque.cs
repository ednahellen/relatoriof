using ClosedXML.Excel;
using ExcelDataReader;
using MySql.Data.MySqlClient;
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

        // Controles da aba de saída
        private Label lblSaldoAtual;
        private Label lblProdutoSelecionado;
        private ComboBox cmbProdutoSaida;
        private NumericUpDown numQuantidadeSaida;
        private TextBox txtDestinoSaida;
        private Button btnRegistrarSaida;
        private DataGridView dgvHistoricoSaidas;

        // 🔥 LISTA DE PRODUTOS PRINCIPAIS - ÚNICA DECLARAÇÃO 🔥
        private List<string> produtosPrincipais = new List<string>
        {
            "ARROZ 1KG", "ARROZ 2KG", "ARROZ 5KG", "FEIJAO 1KG", "MOLHO DE TOMATE",
            "MACARRAO 500G", "ACUCAR 1KG", "SAL 1KG", "OLEO 900ML", "FUBA 400G",
            "FUBA 500G", "LEITE 1L"
        };

        
        private Button btnSincronizarEstoque;
        private TabControl tabControlEstoque;
        

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
            // Verificar se os controles existem no designer
            if (btnAplicarFiltros != null)
                btnAplicarFiltros.Click += btnAplicarFiltros_Click;
            if (btnLimparFiltros != null)
                btnLimparFiltros.Click += btnLimparFiltros_Click;
            if (btnProdutosPrincipais != null)
                btnProdutosPrincipais.Click += btnProdutosPrincipais_Click;
            if (btnImportar != null)
                btnImportar.Click += BtnImportar_Click;
            if (btnExportar != null)
                btnExportar.Click += BtnExportar_Click;
            if (btnMenu != null)
                btnMenu.Click += btnMenu_Click;
            if (btnCadastrar != null)
                btnCadastrar.Click += btnCadastrar_Click;
            if (btnSincronizarEstoque != null)
                btnSincronizarEstoque.Click += btnSincronizarEstoque_Click;
            if (dgvEstoque != null)
                dgvEstoque.CellFormatting += dgvEstoque_CellFormatting;

            ConfigurarDataGridView();
            CarregarProdutos();
            CarregarDados();
            ConfigurarAbaHistorico();
            ConfigurarTabRegistrarSaida();
            CarregarProdutosSaida();
            VerificacaoSistema();

            // Botão imprimir (opcional)
            Button btnImprimir = new Button
            {
                Text = "🖨️ Imprimir",
                Location = new Point(500, 10),
                Size = new Size(150, 30),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnImprimir.Click += (s, ev) => ImprimirEstoque();
            this.Controls.Add(btnImprimir);
        }

        private void ConfigurarDataGridView()
        {
            if (dgvEstoque == null) return;

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

        //private void CarregarDados()
        //{
        //    if (dgvEstoque == null) return;

        //    dgvEstoque.Rows.Clear();

        //    int totalQuantidade = 0;
        //    decimal pesoTotalGramas = 0;
        //    int totalProdutos = 0;

        //    using (var conn = DataBaseConnection.OpenConnection())
        //    {
        //        string sql = @"
        //            SELECT 
        //                l.descricao AS produto,
        //                l.unidade,
        //                l.peso,
        //                COALESCE((SELECT SUM(p.quantidade) FROM tbProdutos p WHERE p.codList = l.codList), 0) AS quantidade,
        //                (SELECT MIN(p.dataDeValidade) FROM tbProdutos p WHERE p.codList = l.codList AND p.quantidade > 0) AS validade,
        //                (SELECT o.nome 
        //                 FROM tbProdutos p
        //                 INNER JOIN tbOrigemDoacao o ON o.codOri = p.codOri
        //                 WHERE p.codList = l.codList AND p.quantidade > 0
        //                 ORDER BY p.dataDeEntrada DESC
        //                 LIMIT 1) AS origem
        //            FROM tbLista l";

        //        // 🔥 FILTRO DE PRODUTOS PRINCIPAIS
        //        if (somentePrincipais && produtosPrincipais.Count > 0)
        //        {
        //            string produtosFiltro = string.Join("','", produtosPrincipais);
        //            sql += $" WHERE l.descricao IN ('{produtosFiltro}')";
        //        }

        //        // 🔥 FILTRO POR PRODUTO ESPECÍFICO (do combo box)
        //        if (!string.IsNullOrEmpty(produtoSelecionado) && produtoSelecionado != "Todos os produtos")
        //        {
        //            if (sql.Contains("WHERE"))
        //                sql += $" AND l.descricao = @produto";
        //            else
        //                sql += $" WHERE l.descricao = @produto";
        //        }

        //        sql += " HAVING quantidade > 0 ORDER BY l.descricao";

        //        using (var cmd = new MySqlCommand(sql, conn))
        //        {
        //            if (!string.IsNullOrEmpty(produtoSelecionado) && produtoSelecionado != "Todos os produtos")
        //            {
        //                cmd.Parameters.AddWithValue("@produto", produtoSelecionado);
        //            }

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    int qtd = Convert.ToInt32(reader["quantidade"]);
        //                    if (qtd <= 0) continue;

        //                    decimal pesoUnitario = Convert.ToDecimal(reader["peso"]);
        //                    decimal pesoTotalProduto = qtd * pesoUnitario;

        //                    DateTime? validade = reader["validade"] != DBNull.Value ? Convert.ToDateTime(reader["validade"]) : (DateTime?)null;
        //                    string status = CalcularStatus(validade);
        //                    string origem = reader["origem"] != DBNull.Value ? reader["origem"].ToString() : "Nao informado";

        //                    totalQuantidade += qtd;
        //                    pesoTotalGramas += pesoTotalProduto;
        //                    totalProdutos++;

        //                    int rowIndex = dgvEstoque.Rows.Add(
        //                        reader["produto"], qtd, reader["unidade"].ToString(),
        //                        pesoUnitario.ToString("0"), (pesoTotalProduto / 1000m).ToString("0.00"),
        //                        status, FormatarData(validade), origem
        //                    );

        //                    if (status == "Vencido")
        //                        dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
        //                    else if (status == "Proximo")
        //                        dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Khaki;
        //                    else if (status == "Valido")
        //                        dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Honeydew;
        //                }
        //            }
        //        }
        //    }

        //    string pesoTotalFormatado = (pesoTotalGramas / 1000m).ToString("0.00") + " kg";
        //    dgvEstoque.Rows.Add(">>> TOTAL GERAL", totalQuantidade.ToString("N0"), totalProdutos.ToString() + " tipos", "", pesoTotalFormatado, "", "", "");
        //    dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.BackColor = Color.DarkSlateGray;
        //    dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
        //    dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        //}

        private void CarregarDados()
        {
            if (dgvEstoque == null) return;

            dgvEstoque.Rows.Clear();

            int totalQuantidade = 0;
            decimal pesoTotalGramas = 0;
            int totalProdutos = 0;

            using (var conn = DataBaseConnection.OpenConnection())
            {
                // Query base
                string sql = @"
            SELECT 
                l.descricao AS produto,
                l.unidade,
                l.peso,
                COALESCE((SELECT SUM(p.quantidade) FROM tbProdutos p WHERE p.codList = l.codList), 0) AS quantidade,
                (SELECT MIN(p.dataDeValidade) FROM tbProdutos p WHERE p.codList = l.codList AND p.quantidade > 0) AS validade,
                (SELECT o.nome 
                 FROM tbProdutos p
                 INNER JOIN tbOrigemDoacao o ON o.codOri = p.codOri
                 WHERE p.codList = l.codList AND p.quantidade > 0
                 ORDER BY p.dataDeEntrada DESC
                 LIMIT 1) AS origem
            FROM tbLista l";

                List<string> whereConditions = new List<string>();

                // 🔥 FILTRO DE PRODUTOS PRINCIPAIS (prioridade máxima)
                if (somentePrincipais && produtosPrincipais.Count > 0)
                {
                    // Escapa os nomes dos produtos para evitar SQL injection
                    var escapedProdutos = produtosPrincipais.Select(p => $"'{p.Replace("'", "''")}'");
                    string produtosFiltro = string.Join(",", escapedProdutos);
                    whereConditions.Add($"l.descricao IN ({produtosFiltro})");

                    // DEBUG: Mostra quantos produtos estão sendo filtrados
                    System.Diagnostics.Debug.WriteLine($"Filtrando produtos principais: {produtosPrincipais.Count} produtos");
                }

                // 🔥 FILTRO POR PRODUTO ESPECÍFICO (só aplica se NÃO estiver em modo principais)
                if (!somentePrincipais && !string.IsNullOrEmpty(produtoSelecionado) && produtoSelecionado != "Todos os produtos")
                {
                    whereConditions.Add("l.descricao = @produto");
                }

                // Aplicar WHERE se houver condições
                if (whereConditions.Count > 0)
                {
                    sql += " WHERE " + string.Join(" AND ", whereConditions);
                }

                sql += " ORDER BY l.descricao";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    // Adicionar parâmetro apenas se necessário
                    if (!somentePrincipais && !string.IsNullOrEmpty(produtoSelecionado) && produtoSelecionado != "Todos os produtos")
                    {
                        cmd.Parameters.AddWithValue("@produto", produtoSelecionado);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int qtd = Convert.ToInt32(reader["quantidade"]);
                            if (qtd <= 0) continue;

                            decimal pesoUnitario = Convert.ToDecimal(reader["peso"]);
                            decimal pesoTotalProduto = qtd * pesoUnitario;

                            DateTime? validade = reader["validade"] != DBNull.Value ? Convert.ToDateTime(reader["validade"]) : (DateTime?)null;
                            string status = CalcularStatus(validade);
                            string origem = reader["origem"] != DBNull.Value ? reader["origem"].ToString() : "Nao informado";

                            totalQuantidade += qtd;
                            pesoTotalGramas += pesoTotalProduto;
                            totalProdutos++;

                            int rowIndex = dgvEstoque.Rows.Add(
                                reader["produto"], qtd, reader["unidade"].ToString(),
                                pesoUnitario.ToString("0"), (pesoTotalProduto / 1000m).ToString("0.00"),
                                status, FormatarData(validade), origem
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
            }

            // Adicionar total
            string pesoTotalFormatado = (pesoTotalGramas / 1000m).ToString("0.00") + " kg";
            dgvEstoque.Rows.Add(">>> TOTAL GERAL", totalQuantidade.ToString("N0"), totalProdutos.ToString() + " tipos", "", pesoTotalFormatado, "", "", "");
            dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.BackColor = Color.DarkSlateGray;
            dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
            dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);

            // DEBUG: Mostra quantos registros foram carregados
            System.Diagnostics.Debug.WriteLine($"Total de produtos carregados: {totalProdutos}");
        }


        private void CarregarProdutos()
        {
            if (cbxprodutoSelecionado == null) return;

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
                    SELECT l.descricao, COALESCE(SUM(p.quantidade), 0) as saldo
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
            if (cmbProdutoSaida == null || cmbProdutoSaida.SelectedItem == null) return;
            string item = cmbProdutoSaida.SelectedItem.ToString();
            if (item == "Nenhum produto com estoque") return;

            string produto = item.Split('|')[0].Trim();
            if (lblProdutoSelecionado != null)
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
                    if (lblSaldoAtual != null)
                        lblSaldoAtual.Text = saldo.ToString();
                    if (numQuantidadeSaida != null)
                    {
                        numQuantidadeSaida.Maximum = saldo > 0 ? saldo : 0;
                        numQuantidadeSaida.Value = 0;
                    }
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

                    int saldoAtual = 0;
                    string sqlSaldo = @"
                        SELECT COALESCE(SUM(CASE WHEN tipoMovimentacao = 'ENTRADA' THEN quantidade ELSE 0 END),0) -
                               COALESCE(SUM(CASE WHEN tipoMovimentacao = 'SAIDA' THEN ABS(quantidade) ELSE 0 END),0)
                        FROM tbProdutos WHERE codList = @codList";

                    using (var cmd = new MySqlCommand(sqlSaldo, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@codList", codList);
                        saldoAtual = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (saldoAtual < quantidade)
                        throw new Exception($"Estoque insuficiente!\nDisponível: {saldoAtual}");

                    int codOri = 1;
                    using (var cmd = new MySqlCommand("SELECT codOri FROM tbOrigemDoacao LIMIT 1", conn, trans))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null) codOri = Convert.ToInt32(result);
                    }

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
                    MessageBox.Show($"Saída registrada!\n\nProduto: {produto}\nQtd: {quantidade}\nSaldo anterior: {saldoAtual}\nNovo saldo: {novoSaldo}",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CarregarDados();
                    CarregarProdutosSaida();
                    CarregarHistoricoSaidas("");
                }
                catch (Exception ex)
                {
                    if (trans.Connection != null) trans.Rollback();
                    MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnRegistrarSaida_Click(object sender, EventArgs e)
        {
            if (btnRegistrarSaida == null) return;

            btnRegistrarSaida.Enabled = false;
            btnRegistrarSaida.Text = "Processando...";

            try
            {
                if (cmbProdutoSaida == null || cmbProdutoSaida.SelectedItem == null)
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

                if (numQuantidadeSaida == null)
                {
                    MessageBox.Show("Controle de quantidade não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int qtd = (int)numQuantidadeSaida.Value;
                if (qtd <= 0)
                {
                    MessageBox.Show("Informe uma quantidade válida (maior que zero).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string produto = item.Split('|')[0].Trim();
                string destino = txtDestinoSaida != null ? txtDestinoSaida.Text.Trim() : "";

                RegistrarSaida(produto, qtd, destino);

                if (txtDestinoSaida != null)
                    txtDestinoSaida.Clear();
                if (numQuantidadeSaida != null)
                    numQuantidadeSaida.Value = 0;
                if (lblSaldoAtual != null)
                    lblSaldoAtual.Text = "0";
                if (lblProdutoSelecionado != null)
                    lblProdutoSelecionado.Text = "-";
                if (cmbProdutoSaida != null)
                    cmbProdutoSaida.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (btnRegistrarSaida != null)
                {
                    btnRegistrarSaida.Enabled = true;
                    btnRegistrarSaida.Text = "📦 REGISTRAR SAÍDA";
                }
            }
        }

        private void ConfigurarTabRegistrarSaida()
        {
            if (tabPageRegistrarSaida == null)
            {
                // Criar a tab page se não existir
                if (tabControlEstoque == null)
                {
                    tabControlEstoque = new TabControl { Dock = DockStyle.Fill };
                    this.Controls.Add(tabControlEstoque);
                }

                tabPageRegistrarSaida = new TabPage("Registrar Saída");
                tabControlEstoque.TabPages.Add(tabPageRegistrarSaida);

                tabPageSaidas = new TabPage("Histórico de Saídas");
                tabControlEstoque.TabPages.Add(tabPageSaidas);
            }

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
            if (cbxprodutoSelecionado == null) return;
            produtoSelecionado = cbxprodutoSelecionado.SelectedIndex == 0 ? "" : cbxprodutoSelecionado.Text;
            somentePrincipais = false;
            CarregarDados();
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "";
            if (cbxprodutoSelecionado != null)
                cbxprodutoSelecionado.SelectedIndex = 0;
            somentePrincipais = false;
            CarregarProdutos();
            CarregarDados();
            if (btnProdutosPrincipais != null)
            {
                btnProdutosPrincipais.BackColor = SystemColors.Control;
                btnProdutosPrincipais.Text = "Produtos Principais";
            }
        }

        private void btnProdutosPrincipais_Click(object sender, EventArgs e)
        {
            somentePrincipais = true; // Força o modo principais
            produtoSelecionado = ""; // Limpa qualquer filtro anterior

            CarregarDados(); // Recarrega os dados

            btnProdutosPrincipais.BackColor = Color.LightGreen;
            btnProdutosPrincipais.Text = "✔ Produtos Principais";

            MessageBox.Show($"Filtrando apenas {produtosPrincipais.Count} produtos principais!",
                "Filtro Ativado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Arquivos Excel|*.xlsx;*.xls",
                Title = "Selecione o arquivo Excel com as entradas de estoque"
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                };

                var result = reader.AsDataSet(conf);
                DataTable dt = result.Tables[0];

                if (!dt.Columns.Contains("Produto") || !dt.Columns.Contains("Qtd"))
                {
                    MessageBox.Show("A planilha precisa ter as colunas: Produto e Qtd");
                    return;
                }

                using (var conn = DataBaseConnection.OpenConnection())
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string produto = row["Produto"]?.ToString()?.Trim();
                            if (!int.TryParse(row["Qtd"]?.ToString(), out int qtd)) continue;
                            if (string.IsNullOrEmpty(produto) || qtd <= 0) continue;

                            int codList;
                            using (var cmd = new MySqlCommand("SELECT codList FROM tbLista WHERE descricao = @desc", conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@desc", produto);
                                var resultCod = cmd.ExecuteScalar();
                                if (resultCod == null) continue;
                                codList = Convert.ToInt32(resultCod);
                            }

                            string sql = @"
                                INSERT INTO tbProdutos
                                (descricao, quantidade, peso, unidade, dataDeEntrada,
                                 dataDeValidade, dataLimiteDeSaida, tipoMovimentacao,
                                 codUsu, codOri, codList)
                                VALUES
                                (@desc, @qtd,
                                 (SELECT peso FROM tbLista WHERE codList = @codList),
                                 'UNIDADES (UN)',
                                 NOW(),
                                 DATE_ADD(NOW(), INTERVAL 30 DAY),
                                 DATE_ADD(NOW(), INTERVAL 60 DAY),
                                 'ENTRADA',
                                 @codUsu,
                                 1,
                                 @codList)";

                            using (var cmd = new MySqlCommand(sql, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@desc", produto);
                                cmd.Parameters.AddWithValue("@qtd", qtd);
                                cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
                                cmd.Parameters.AddWithValue("@codList", codList);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                        MessageBox.Show("Importação concluída com sucesso.");
                        CarregarDados();
                        CarregarProdutosSaida();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Erro: " + ex.Message);
                    }
                }
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            if (tabPageSaidas != null && tabPageSaidas.Visible)
            {
                ContextMenuStrip menuExportar = new ContextMenuStrip();
                ToolStripMenuItem itemCSV = new ToolStripMenuItem("📄 Exportar Histórico de Saídas para CSV");
                ToolStripMenuItem itemExcel = new ToolStripMenuItem("📊 Exportar Histórico de Saídas para Excel");
                ToolStripMenuItem itemImprimir = new ToolStripMenuItem("🖨️ Imprimir Histórico de Saídas");

                itemCSV.Click += (s, ev) => ExportarHistoricoSaidasCSV();
                itemExcel.Click += (s, ev) => ExportarHistoricoSaidasExcel();
                itemImprimir.Click += (s, ev) => ImprimirHistoricoSaidas();

                menuExportar.Items.AddRange(new ToolStripItem[] { itemCSV, itemExcel, itemImprimir });
                Button btn = sender as Button;
                if (btn != null) menuExportar.Show(btn, new Point(0, btn.Height));
            }
            else
            {
                ContextMenuStrip menuExportar = new ContextMenuStrip();
                ToolStripMenuItem itemCSV = new ToolStripMenuItem("📄 Exportar Estoque para CSV");
                ToolStripMenuItem itemExcel = new ToolStripMenuItem("📊 Exportar Estoque para Excel");
                ToolStripMenuItem itemImprimir = new ToolStripMenuItem("🖨️ Imprimir Estoque");

                itemCSV.Click += (s, ev) => ExportarEstoqueCSV();
                itemExcel.Click += (s, ev) => ExportarEstoqueExcel();
                itemImprimir.Click += (s, ev) => ImprimirEstoque();

                menuExportar.Items.AddRange(new ToolStripItem[] { itemCSV, itemExcel, itemImprimir });
                Button btn = sender as Button;
                if (btn != null) menuExportar.Show(btn, new Point(0, btn.Height));
            }
        }

        private void ExportarEstoqueCSV()
        {
            if (dgvEstoque == null || dgvEstoque.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Arquivos CSV (*.csv)|*.csv",
                Title = "Exportar Estoque para CSV",
                FileName = $"Estoque_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dgvEstoque.Columns.Count; i++)
                {
                    sb.Append($"\"{dgvEstoque.Columns[i].HeaderText}\"");
                    if (i < dgvEstoque.Columns.Count - 1) sb.Append(";");
                }
                sb.AppendLine();

                foreach (DataGridViewRow row in dgvEstoque.Rows)
                {
                    if (row.IsNewRow) continue;
                    for (int i = 0; i < dgvEstoque.Columns.Count; i++)
                    {
                        string valor = row.Cells[i].Value?.ToString() ?? "";
                        sb.Append($"\"{valor.Replace("\"", "\"\"")}\"");
                        if (i < dgvEstoque.Columns.Count - 1) sb.Append(";");
                    }
                    sb.AppendLine();
                }

                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"✅ Arquivo exportado com sucesso!\n\nLocal: {sfd.FileName}", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarEstoqueExcel()
        {
            if (dgvEstoque == null || dgvEstoque.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Arquivos HTML (*.html)|*.html",
                Title = "Exportar Estoque para Excel",
                FileName = $"Estoque_{DateTime.Now:yyyyMMdd_HHmmss}.html"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Relatório de Estoque</title>");
                sb.AppendLine("<style>");
                sb.AppendLine("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; }");
                sb.AppendLine("h1 { color: #2c3e50; border-bottom: 2px solid #3498db; }");
                sb.AppendLine("table { border-collapse: collapse; width: 100%; margin-top: 20px; }");
                sb.AppendLine("th { background-color: #2c3e50; color: white; padding: 12px; border: 1px solid #ddd; }");
                sb.AppendLine("td { padding: 10px; border: 1px solid #ddd; }");
                sb.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
                sb.AppendLine(".total { background-color: #2c3e50; color: white; font-weight: bold; }");
                sb.AppendLine("</style></head><body>");
                sb.AppendLine($"<h1>📊 Relatório de Estoque</h1>");
                sb.AppendLine($"<p><strong>Data de emissão:</strong> {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");
                sb.AppendLine("<table><thead><tr>");

                foreach (DataGridViewColumn col in dgvEstoque.Columns)
                    sb.AppendLine($"<th>{col.HeaderText}</th>");
                sb.AppendLine("</thead><tbody>");

                foreach (DataGridViewRow row in dgvEstoque.Rows)
                {
                    if (row.IsNewRow) continue;
                    sb.AppendLine("<tr>");
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string valor = cell.Value?.ToString() ?? "";
                        if (row.Cells[0].Value?.ToString()?.Contains("TOTAL") == true)
                            sb.AppendLine($"<td class='total'><strong>{valor}</strong></td>");
                        else
                            sb.AppendLine($"<td style='text-align:center'>{valor}</td>");
                    }
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</tbody></table>");
                sb.AppendLine("<div class='footer'>");
                sb.AppendLine($"<p>Relatório gerado automaticamente pelo Sistema GPSFA</p>");
                sb.AppendLine($"<p>Total de registros: {dgvEstoque.Rows.Count - 1} itens</p>");
                sb.AppendLine("</div></body></html>");

                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"✅ Arquivo exportado com sucesso!\n\nLocal: {sfd.FileName}\n\nAbra o arquivo no Excel.", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImprimirEstoque()
        {
            if (dgvEstoque == null || dgvEstoque.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            int linhaAtual = 0;

            printDocument.PrintPage += (sender, e) =>
            {
                Font tituloFont = new Font("Segoe UI", 16, FontStyle.Bold);
                Font subtituloFont = new Font("Segoe UI", 12, FontStyle.Regular);
                Font cabecalhoFont = new Font("Segoe UI", 10, FontStyle.Bold);
                Font textoFont = new Font("Segoe UI", 9, FontStyle.Regular);

                float yPos = e.MarginBounds.Top;
                float leftMargin = e.MarginBounds.Left;
                float pageWidth = e.MarginBounds.Width;

                e.Graphics.DrawString("RELATÓRIO DE ESTOQUE", tituloFont, Brushes.Black, leftMargin, yPos);
                yPos += 35;
                e.Graphics.DrawString($"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", subtituloFont, Brushes.Black, leftMargin, yPos);
                yPos += 25;
                e.Graphics.DrawString($"Total de itens: {dgvEstoque.Rows.Count - 1} registros", subtituloFont, Brushes.Black, leftMargin, yPos);
                yPos += 35;

                float colX = leftMargin;
                float colWidth = pageWidth / dgvEstoque.Columns.Count;

                e.Graphics.FillRectangle(Brushes.LightGray, leftMargin, yPos, pageWidth, 25);
                for (int i = 0; i < dgvEstoque.Columns.Count; i++)
                {
                    e.Graphics.DrawString(dgvEstoque.Columns[i].HeaderText, cabecalhoFont, Brushes.Black, colX + 5, yPos + 5);
                    colX += colWidth;
                }
                yPos += 30;

                for (int i = linhaAtual; i < dgvEstoque.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvEstoque.Rows[i];
                    if (row.IsNewRow) continue;

                    colX = leftMargin;
                    if (yPos + 25 > e.MarginBounds.Bottom)
                    {
                        linhaAtual = i;
                        e.HasMorePages = true;
                        return;
                    }

                    bool isTotal = row.Cells[0].Value?.ToString()?.Contains("TOTAL") == true;
                    if (isTotal)
                        e.Graphics.FillRectangle(Brushes.DarkGray, leftMargin, yPos, pageWidth, 22);

                    for (int j = 0; j < dgvEstoque.Columns.Count; j++)
                    {
                        string valor = row.Cells[j].Value?.ToString() ?? "";
                        e.Graphics.DrawString(valor, textoFont, isTotal ? Brushes.White : Brushes.Black, colX + 5, yPos + 3);
                        colX += colWidth;
                    }
                    yPos += 22;
                }

                linhaAtual = 0;
                e.HasMorePages = false;
                e.Graphics.DrawString($"Relatório gerado pelo Sistema GPSFA", subtituloFont, Brushes.Gray, leftMargin, yPos + 10);
            };

            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                linhaAtual = 0;
                printDocument.Print();
            }
        }

        private void ExportarHistoricoSaidasCSV()
        {
            if (dgvHistoricoSaidas == null || dgvHistoricoSaidas.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados no histórico para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Arquivos CSV (*.csv)|*.csv",
                Title = "Exportar Histórico de Saídas para CSV",
                FileName = $"Historico_Saidas_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dgvHistoricoSaidas.Columns.Count; i++)
                {
                    sb.Append($"\"{dgvHistoricoSaidas.Columns[i].HeaderText}\"");
                    if (i < dgvHistoricoSaidas.Columns.Count - 1) sb.Append(";");
                }
                sb.AppendLine();

                foreach (DataGridViewRow row in dgvHistoricoSaidas.Rows)
                {
                    if (row.IsNewRow) continue;
                    for (int i = 0; i < dgvHistoricoSaidas.Columns.Count; i++)
                    {
                        string valor = row.Cells[i].Value?.ToString() ?? "";
                        sb.Append($"\"{valor.Replace("\"", "\"\"")}\"");
                        if (i < dgvHistoricoSaidas.Columns.Count - 1) sb.Append(";");
                    }
                    sb.AppendLine();
                }

                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"✅ Histórico exportado!\n\nLocal: {sfd.FileName}", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarHistoricoSaidasExcel()
        {
            if (dgvHistoricoSaidas == null || dgvHistoricoSaidas.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados no histórico para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Arquivos Excel (*.xlsx)|*.xlsx",
                Title = "Exportar Histórico de Saídas para Excel",
                FileName = $"Historico_Saidas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Historico_Saidas");

                    for (int i = 0; i < dgvHistoricoSaidas.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dgvHistoricoSaidas.Columns[i].HeaderText;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(52, 73, 94);
                        worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                    }

                    for (int i = 0; i < dgvHistoricoSaidas.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvHistoricoSaidas.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j + 1).Value = dgvHistoricoSaidas.Rows[i].Cells[j].Value?.ToString() ?? "";
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(sfd.FileName);
                }

                MessageBox.Show($"✅ Histórico exportado!\n\nLocal: {sfd.FileName}", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImprimirHistoricoSaidas()
        {
            if (dgvHistoricoSaidas == null || dgvHistoricoSaidas.Rows.Count == 0)
            {
                MessageBox.Show("Não há registros de saídas para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            int linhaAtual = 0;

            printDocument.PrintPage += (senderPrint, ePrint) =>
            {
                Font tituloFont = new Font("Segoe UI", 14, FontStyle.Bold);
                Font subtituloFont = new Font("Segoe UI", 10, FontStyle.Regular);
                Font cabecalhoFont = new Font("Segoe UI", 9, FontStyle.Bold);
                Font textoFont = new Font("Segoe UI", 8, FontStyle.Regular);

                float yPos = ePrint.MarginBounds.Top;
                float leftMargin = ePrint.MarginBounds.Left;
                float pageWidth = ePrint.MarginBounds.Width;

                ePrint.Graphics.DrawString("RELATÓRIO DE SAÍDAS - ESTOQUE", tituloFont, Brushes.Black, leftMargin, yPos);
                yPos += 35;
                ePrint.Graphics.DrawString($"Data de emissão: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", subtituloFont, Brushes.Black, leftMargin, yPos);
                yPos += 25;
                ePrint.Graphics.DrawString($"Total de saídas: {dgvHistoricoSaidas.Rows.Count} registros", subtituloFont, Brushes.Black, leftMargin, yPos);
                yPos += 30;

                float[] colWidths = { 100, 200, 60, 100, 150 };
                float colX = leftMargin;

                ePrint.Graphics.FillRectangle(Brushes.LightGray, leftMargin, yPos, pageWidth, 22);
                string[] headers = { "Data/Hora", "Produto", "Qtd", "Usuário", "Destino" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ePrint.Graphics.DrawString(headers[i], cabecalhoFont, Brushes.Black, colX + 3, yPos + 3);
                    colX += colWidths[i];
                }
                yPos += 25;

                for (int i = linhaAtual; i < dgvHistoricoSaidas.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvHistoricoSaidas.Rows[i];
                    if (row.IsNewRow) continue;

                    colX = leftMargin;
                    if (yPos + 25 > ePrint.MarginBounds.Bottom)
                    {
                        linhaAtual = i;
                        ePrint.HasMorePages = true;
                        return;
                    }

                    ePrint.Graphics.DrawString(row.Cells["data"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
                    colX += colWidths[0];
                    ePrint.Graphics.DrawString(row.Cells["produto"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
                    colX += colWidths[1];
                    ePrint.Graphics.DrawString(row.Cells["quantidade"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
                    colX += colWidths[2];
                    ePrint.Graphics.DrawString(row.Cells["usuario"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
                    colX += colWidths[3];
                    ePrint.Graphics.DrawString(row.Cells["destino"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
                    yPos += 22;
                }

                linhaAtual = 0;
                ePrint.HasMorePages = false;
                ePrint.Graphics.DrawString($"Relatório gerado pelo Sistema GPSFA", subtituloFont, Brushes.Gray, leftMargin, yPos + 10);
            };

            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                linhaAtual = 0;
                printDocument.Print();
            }
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

                                int saldoAtual;
                                string sqlSaldo = "SELECT COALESCE(SUM(quantidade), 0) FROM tbProdutos WHERE codList = @codList";
                                using (var cmd = new MySqlCommand(sqlSaldo, conexao, transacao))
                                {
                                    cmd.Parameters.AddWithValue("@codList", codigoProduto);
                                    saldoAtual = Convert.ToInt32(cmd.ExecuteScalar());
                                }

                                int diferenca = quantidadeCorreta - saldoAtual;
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
                            MessageBox.Show($"Sincronização concluída.\n\nAjustes: {ajustesRealizados}\n\n{log}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}

//using ClosedXML.Excel;
//using ExcelDataReader;
//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Printing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace GPSFA_WinForms
//{
//    public partial class frmEstoque : Form
//    {
//        // ===== Variáveis =====
//        private string produtoSelecionado = "";
//        private bool modoAgrupado = true;
//        private bool somentePrincipais = false;
//        private int codUsuLogado;
//        private Label lblSaldoAtual;
//        private Label lblProdutoSelecionado;
//        private ComboBox cmbProdutoSaida;
//        private NumericUpDown numQuantidadeSaida;
//        private TextBox txtDestinoSaida;
//        private Button btnRegistrarSaida;
//        private DataGridView dgvHistoricoSaidas;



//    // 🔥 LISTA DE PRODUTOS PRINCIPAIS - ADICIONE AQUI 🔥
//    private List<string> produtosPrincipais = new List<string>
//    {
//        "ARROZ 1KG", "ARROZ 2KG", "ARROZ 5KG", "FEIJAO 1KG", "MOLHO DE TOMATE",
//        "MACARRAO 500G", "ACUCAR 1KG", "SAL 1KG", "OLEO 900ML", "FUBA 400G", "FUBA 500G", "LEITE 1L"
//    };



//        public frmEstoque()
//        {
//            InitializeComponent();
//            this.Load += frmEstoque_Load;
//        }

//        public frmEstoque(int codUsu) : this()
//        {
//            codUsuLogado = codUsu;
//        }

//        private void frmEstoque_Load(object sender, EventArgs e)
//        {
//            btnAplicarFiltros.Click += btnAplicarFiltros_Click;
//            btnLimparFiltros.Click += btnLimparFiltros_Click;
//            btnProdutosPrincipais.Click += btnProdutosPrincipais_Click;
//            btnImportar.Click += BtnImportar_Click;
//            btnExportar.Click += BtnExportar_Click;
//            btnMenu.Click += btnMenu_Click;
//            btnCadastrar.Click += btnCadastrar_Click;
//            dgvEstoque.CellFormatting += dgvEstoque_CellFormatting;

//            ConfigurarDataGridView();
//            CarregarProdutos();
//            CarregarDados();
//            ConfigurarAbaHistorico();
//            ConfigurarTabRegistrarSaida();
//            CarregarProdutosSaida();
//            VerificacaoSistema();

//            Button btnImprimir = new Button
//            {
//                Text = "🖨️ Imprimir",
//                Location = new Point(500, 10),
//                Size = new Size(150, 30),
//                BackColor = Color.Gray,
//                ForeColor = Color.White,
//                FlatStyle = FlatStyle.Flat
//            };
//            btnImprimir.Click += (s, ev) => ImprimirEstoque();
//            this.Controls.Add(btnImprimir);
//        }

//        private void ConfigurarDataGridView()
//        {
//            dgvEstoque.Columns.Clear();
//            dgvEstoque.Rows.Clear();
//            dgvEstoque.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            dgvEstoque.AllowUserToAddRows = false;
//            dgvEstoque.ReadOnly = true;
//            dgvEstoque.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

//            dgvEstoque.Columns.Add("Produto", "Produto");
//            dgvEstoque.Columns.Add("Quantidade", "Qtd");
//            dgvEstoque.Columns.Add("Unidade", "Unid");
//            dgvEstoque.Columns.Add("Peso", "Peso (g)");
//            dgvEstoque.Columns.Add("PesoTotal", "Peso Total (kg)");
//            dgvEstoque.Columns.Add("Status", "Status");
//            dgvEstoque.Columns.Add("Validade", "Validade");
//            dgvEstoque.Columns.Add("Origem", "Origem");
//        }



//        private void CarregarDados()
//        {
//            dgvEstoque.Rows.Clear();

//            int totalQuantidade = 0;
//            decimal pesoTotalGramas = 0;
//            int totalProdutos = 0;

//            using (var conn = DataBaseConnection.OpenConnection())
//            {
//                string sql = @"
//            SELECT 
//                l.descricao AS produto,
//                l.unidade,
//                l.peso,
//                COALESCE((SELECT SUM(p.quantidade) FROM tbProdutos p WHERE p.codList = l.codList), 0) AS quantidade,
//                (SELECT MIN(p.dataDeValidade) FROM tbProdutos p WHERE p.codList = l.codList AND p.quantidade > 0) AS validade,
//                (SELECT o.nome 
//                 FROM tbProdutos p
//                 INNER JOIN tbOrigemDoacao o ON o.codOri = p.codOri
//                 WHERE p.codList = l.codList AND p.quantidade > 0
//                 ORDER BY p.dataDeEntrada DESC
//                 LIMIT 1) AS origem
//            FROM tbLista l";

//                // 🔥 FILTRO DE PRODUTOS PRINCIPAIS
//                if (somentePrincipais && produtosPrincipais.Count > 0)
//                {
//                    string produtosFiltro = string.Join("','", produtosPrincipais);
//                    sql += $" WHERE l.descricao IN ('{produtosFiltro}')";
//                }

//                // 🔥 FILTRO POR PRODUTO ESPECÍFICO (do combo box)
//                if (!string.IsNullOrEmpty(produtoSelecionado) && produtoSelecionado != "Todos os produtos")
//                {
//                    if (sql.Contains("WHERE"))
//                        sql += $" AND l.descricao = @produto";
//                    else
//                        sql += $" WHERE l.descricao = @produto";
//                }

//                sql += " HAVING quantidade > 0 ORDER BY l.descricao";

//                using (var cmd = new MySqlCommand(sql, conn))
//                {
//                    if (!string.IsNullOrEmpty(produtoSelecionado) && produtoSelecionado != "Todos os produtos")
//                    {
//                        cmd.Parameters.AddWithValue("@produto", produtoSelecionado);
//                    }

//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            int qtd = Convert.ToInt32(reader["quantidade"]);
//                            if (qtd <= 0) continue;

//                            decimal pesoUnitario = Convert.ToDecimal(reader["peso"]);
//                            decimal pesoTotalProduto = qtd * pesoUnitario;

//                            DateTime? validade = reader["validade"] != DBNull.Value ? Convert.ToDateTime(reader["validade"]) : (DateTime?)null;
//                            string status = CalcularStatus(validade);
//                            string origem = reader["origem"] != DBNull.Value ? reader["origem"].ToString() : "Nao informado";

//                            totalQuantidade += qtd;
//                            pesoTotalGramas += pesoTotalProduto;
//                            totalProdutos++;

//                            int rowIndex = dgvEstoque.Rows.Add(
//                                reader["produto"], qtd, reader["unidade"].ToString(),
//                                pesoUnitario.ToString("0"), (pesoTotalProduto / 1000m).ToString("0.00"),
//                                status, FormatarData(validade), origem
//                            );

//                            if (status == "Vencido")
//                                dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
//                            else if (status == "Proximo")
//                                dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Khaki;
//                            else if (status == "Valido")
//                                dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Honeydew;
//                        }
//                    }
//                }
//            }

//            string pesoTotalFormatado = (pesoTotalGramas / 1000m).ToString("0.00") + " kg";
//            dgvEstoque.Rows.Add(">>> TOTAL GERAL", totalQuantidade.ToString("N0"), totalProdutos.ToString() + " tipos", "", pesoTotalFormatado, "", "", "");
//            dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.BackColor = Color.DarkSlateGray;
//            dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
//            dgvEstoque.Rows[dgvEstoque.Rows.Count - 1].DefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
//        }


//        private void CarregarProdutos()
//        {
//            cbxprodutoSelecionado.Items.Clear();
//            using (var conn = DataBaseConnection.OpenConnection())
//            {
//                string sql = "SELECT descricao FROM tbLista ORDER BY descricao";
//                using (var cmd = new MySqlCommand(sql, conn))
//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                        cbxprodutoSelecionado.Items.Add(reader["descricao"].ToString());
//                }
//            }
//            cbxprodutoSelecionado.Items.Insert(0, "Todos os produtos");
//            cbxprodutoSelecionado.SelectedIndex = 0;
//        }

//        private string CalcularStatus(DateTime? validade)
//        {
//            if (!validade.HasValue) return "Sem validade";
//            int dias = (validade.Value - DateTime.Today).Days;
//            if (dias < 0) return "Vencido";
//            if (dias <= 60) return "Proximo";
//            return "Valido";
//        }

//        private string FormatarData(object data)
//        {
//            if (data == null || data == DBNull.Value) return "";
//            if (DateTime.TryParse(data.ToString(), out DateTime dt))
//                return dt.ToString("dd/MM/yyyy");
//            return "";
//        }

//        private void CarregarProdutosSaida()
//        {
//            if (cmbProdutoSaida == null) return;
//            cmbProdutoSaida.Items.Clear();

//            using (var conn = DataBaseConnection.OpenConnection())
//            {
//                string sql = @"
//                    SELECT l.descricao, COALESCE(SUM(p.quantidade), 0) as saldo
//                    FROM tbLista l
//                    LEFT JOIN tbProdutos p ON p.codList = l.codList
//                    GROUP BY l.codList, l.descricao
//                    HAVING saldo > 0
//                    ORDER BY l.descricao";

//                using (var cmd = new MySqlCommand(sql, conn))
//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        string nome = reader["descricao"].ToString();
//                        int qtd = Convert.ToInt32(reader["saldo"]);
//                        cmbProdutoSaida.Items.Add($"{nome} | Estoque: {qtd}");
//                    }
//                }
//            }
//            if (cmbProdutoSaida.Items.Count == 0)
//                cmbProdutoSaida.Items.Add("Nenhum produto com estoque");
//        }

//        private void CmbProdutoSaida_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (cmbProdutoSaida.SelectedItem == null) return;
//            string item = cmbProdutoSaida.SelectedItem.ToString();
//            if (item == "Nenhum produto com estoque") return;

//            string produto = item.Split('|')[0].Trim();
//            lblProdutoSelecionado.Text = produto;

//            using (var conn = DataBaseConnection.OpenConnection())
//            {
//                string sql = @"
//                    SELECT COALESCE(SUM(p.quantidade), 0)
//                    FROM tbProdutos p
//                    INNER JOIN tbLista l ON l.codList = p.codList
//                    WHERE l.descricao = @produto";

//                using (var cmd = new MySqlCommand(sql, conn))
//                {
//                    cmd.Parameters.AddWithValue("@produto", produto);
//                    object result = cmd.ExecuteScalar();
//                    int saldo = result != null ? Convert.ToInt32(result) : 0;
//                    lblSaldoAtual.Text = saldo.ToString();
//                    numQuantidadeSaida.Maximum = saldo > 0 ? saldo : 0;
//                    numQuantidadeSaida.Value = 0;
//                }
//            }
//        }

//        private void RegistrarSaida(string produto, int quantidade, string destino)
//        {
//            using (var conn = DataBaseConnection.OpenConnection())
//            using (var trans = conn.BeginTransaction())
//            {
//                try
//                {
//                    int codList = 0;
//                    int peso = 0;

//                    using (var cmd = new MySqlCommand("SELECT codList, peso FROM tbLista WHERE descricao = @produto", conn, trans))
//                    {
//                        cmd.Parameters.AddWithValue("@produto", produto);
//                        using (var reader = cmd.ExecuteReader())
//                        {
//                            if (reader.Read())
//                            {
//                                codList = Convert.ToInt32(reader["codList"]);
//                                peso = Convert.ToInt32(reader["peso"]);
//                            }
//                        }
//                    }

//                    if (codList == 0)
//                        throw new Exception($"Produto '{produto}' não encontrado.");

//                    int saldoAtual = 0;
//                    string sqlSaldo = @"
//                        SELECT COALESCE(SUM(CASE WHEN tipoMovimentacao = 'ENTRADA' THEN quantidade ELSE 0 END),0) -
//                               COALESCE(SUM(CASE WHEN tipoMovimentacao = 'SAIDA' THEN ABS(quantidade) ELSE 0 END),0)
//                        FROM tbProdutos WHERE codList = @codList";

//                    using (var cmd = new MySqlCommand(sqlSaldo, conn, trans))
//                    {
//                        cmd.Parameters.AddWithValue("@codList", codList);
//                        saldoAtual = Convert.ToInt32(cmd.ExecuteScalar());
//                    }

//                    if (saldoAtual < quantidade)
//                        throw new Exception($"Estoque insuficiente!\nDisponível: {saldoAtual}");

//                    int codOri = 1;
//                    using (var cmd = new MySqlCommand("SELECT codOri FROM tbOrigemDoacao LIMIT 1", conn, trans))
//                    {
//                        var result = cmd.ExecuteScalar();
//                        if (result != null) codOri = Convert.ToInt32(result);
//                    }

//                    string sqlInsert = @"
//                        INSERT INTO tbProdutos 
//                            (descricao, quantidade, peso, unidade, dataDeEntrada, 
//                             dataDeValidade, dataLimiteDeSaida, tipoMovimentacao, 
//                             codUsu, codOri, codList, destino)
//                        VALUES 
//                            (@descricao, @quantidade, @peso, 'UNIDADES (UN)', NOW(),
//                             DATE_ADD(NOW(), INTERVAL 30 DAY), DATE_ADD(NOW(), INTERVAL 60 DAY),
//                             'SAIDA', @codUsu, @codOri, @codList, @destino)";

//                    using (var cmd = new MySqlCommand(sqlInsert, conn, trans))
//                    {
//                        cmd.Parameters.AddWithValue("@descricao", produto);
//                        cmd.Parameters.AddWithValue("@quantidade", -quantidade);
//                        cmd.Parameters.AddWithValue("@peso", peso);
//                        cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
//                        cmd.Parameters.AddWithValue("@codOri", codOri);
//                        cmd.Parameters.AddWithValue("@codList", codList);
//                        cmd.Parameters.AddWithValue("@destino", string.IsNullOrEmpty(destino) ? "Não informado" : destino);
//                        cmd.ExecuteNonQuery();
//                    }

//                    trans.Commit();

//                    int novoSaldo = saldoAtual - quantidade;
//                    MessageBox.Show($"Saída registrada!\n\nProduto: {produto}\nQtd: {quantidade}\nSaldo anterior: {saldoAtual}\nNovo saldo: {novoSaldo}",
//                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                    CarregarDados();
//                    CarregarProdutosSaida();
//                    CarregarHistoricoSaidas("");
//                }
//                catch (Exception ex)
//                {
//                    if (trans.Connection != null) trans.Rollback();
//                    MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//        private async void BtnRegistrarSaida_Click(object sender, EventArgs e)
//        {
//            btnRegistrarSaida.Enabled = false;
//            btnRegistrarSaida.Text = "Processando...";

//            try
//            {
//                if (cmbProdutoSaida.SelectedItem == null)
//                {
//                    MessageBox.Show("Selecione um produto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }

//                string item = cmbProdutoSaida.SelectedItem.ToString();
//                if (item == "Nenhum produto com estoque")
//                {
//                    MessageBox.Show("Não há produtos com estoque disponível.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }

//                int qtd = (int)numQuantidadeSaida.Value;
//                if (qtd <= 0)
//                {
//                    MessageBox.Show("Informe uma quantidade válida (maior que zero).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }

//                string produto = item.Split('|')[0].Trim();
//                string destino = txtDestinoSaida.Text.Trim();

//                RegistrarSaida(produto, qtd, destino);

//                txtDestinoSaida.Clear();
//                numQuantidadeSaida.Value = 0;
//                lblSaldoAtual.Text = "0";
//                lblProdutoSelecionado.Text = "-";
//                cmbProdutoSaida.SelectedIndex = -1;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//            finally
//            {
//                btnRegistrarSaida.Enabled = true;
//                btnRegistrarSaida.Text = "📦 REGISTRAR SAÍDA";
//            }
//        }

//        private void ConfigurarTabRegistrarSaida()
//        {
//            if (tabPageRegistrarSaida == null) return;
//            tabPageRegistrarSaida.Controls.Clear();

//            GroupBox grpSelecao = new GroupBox
//            {
//                Text = " SELEÇÃO DO PRODUTO ",
//                Location = new Point(20, 20),
//                Size = new Size(450, 130),
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                BackColor = Color.FromArgb(250, 250, 250)
//            };

//            Label lblProduto = new Label { Text = "Produto:", Location = new Point(15, 30), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
//            cmbProdutoSaida = new ComboBox { Location = new Point(100, 28), Size = new Size(320, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
//            cmbProdutoSaida.SelectedIndexChanged += CmbProdutoSaida_SelectedIndexChanged;

//            Label lblSelecionado = new Label { Text = "Selecionado:", Location = new Point(15, 60), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
//            lblProdutoSelecionado = new Label { Text = "-", Location = new Point(100, 60), Size = new Size(320, 25), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Navy };
//            Label lblSaldo = new Label { Text = "Saldo:", Location = new Point(15, 90), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
//            lblSaldoAtual = new Label { Text = "0", Location = new Point(100, 90), Size = new Size(150, 25), Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DarkGreen };

//            grpSelecao.Controls.AddRange(new Control[] { lblProduto, cmbProdutoSaida, lblSelecionado, lblProdutoSelecionado, lblSaldo, lblSaldoAtual });

//            GroupBox grpSaida = new GroupBox
//            {
//                Text = " DADOS DA SAÍDA ",
//                Location = new Point(20, 160),
//                Size = new Size(450, 120),
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                BackColor = Color.FromArgb(250, 250, 250)
//            };

//            Label lblQtd = new Label { Text = "Quantidade:", Location = new Point(15, 30), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
//            numQuantidadeSaida = new NumericUpDown { Location = new Point(100, 28), Size = new Size(120, 25), Minimum = 0, Maximum = 100000, Value = 0, Font = new Font("Segoe UI", 10) };
//            Label lblDestino = new Label { Text = "Destino:", Location = new Point(15, 65), Size = new Size(80, 25), Font = new Font("Segoe UI", 10) };
//            txtDestinoSaida = new TextBox { Location = new Point(100, 63), Size = new Size(320, 25), Font = new Font("Segoe UI", 10) };

//            grpSaida.Controls.AddRange(new Control[] { lblQtd, numQuantidadeSaida, lblDestino, txtDestinoSaida });

//            btnRegistrarSaida = new Button
//            {
//                Text = "📦 REGISTRAR SAÍDA",
//                Location = new Point(20, 300),
//                Size = new Size(200, 40),
//                Font = new Font("Segoe UI", 11, FontStyle.Bold),
//                BackColor = Color.FromArgb(52, 152, 219),
//                ForeColor = Color.White,
//                FlatStyle = FlatStyle.Flat
//            };
//            btnRegistrarSaida.Click += BtnRegistrarSaida_Click;

//            tabPageRegistrarSaida.Controls.AddRange(new Control[] { grpSelecao, grpSaida, btnRegistrarSaida });
//        }

//        private void ConfigurarAbaHistorico()
//        {
//            if (tabPageSaidas == null) return;
//            tabPageSaidas.Controls.Clear();

//            dgvHistoricoSaidas = new DataGridView
//            {
//                Dock = DockStyle.Fill,
//                AllowUserToAddRows = false,
//                ReadOnly = true,
//                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                RowHeadersVisible = false,
//                BackgroundColor = Color.White,
//                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                Font = new Font("Segoe UI", 10)
//            };
//            dgvHistoricoSaidas.EnableHeadersVisualStyles = false;
//            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
//            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
//            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
//            dgvHistoricoSaidas.ColumnHeadersHeight = 35;

//            dgvHistoricoSaidas.Columns.Add("data", "Data/Hora");
//            dgvHistoricoSaidas.Columns.Add("produto", "Produto");
//            dgvHistoricoSaidas.Columns.Add("quantidade", "Qtd");
//            dgvHistoricoSaidas.Columns.Add("usuario", "Usuário");
//            dgvHistoricoSaidas.Columns.Add("destino", "Destino");
//            dgvHistoricoSaidas.Columns["quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

//            tabPageSaidas.Controls.Add(dgvHistoricoSaidas);
//            CarregarHistoricoSaidas("");
//        }

//        private void CarregarHistoricoSaidas(string filtro = "")
//        {
//            if (dgvHistoricoSaidas == null) return;
//            dgvHistoricoSaidas.Rows.Clear();

//            using (var conn = DataBaseConnection.OpenConnection())
//            {
//                string sql = @"
//                    SELECT DATE_FORMAT(p.dataDeEntrada, '%d/%m/%Y %H:%i') as data,
//                           l.descricao as produto, ABS(p.quantidade) as quantidade,
//                           u.usuario, p.destino
//                    FROM tbProdutos p
//                    INNER JOIN tbLista l ON l.codList = p.codList
//                    INNER JOIN tbUsuarios u ON u.codUsu = p.codUsu
//                    WHERE p.tipoMovimentacao = 'SAIDA'";

//                if (!string.IsNullOrEmpty(filtro))
//                    sql += " AND l.descricao LIKE @filtro";
//                sql += " ORDER BY p.dataDeEntrada DESC LIMIT 500";

//                using (var cmd = new MySqlCommand(sql, conn))
//                {
//                    if (!string.IsNullOrEmpty(filtro))
//                        cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                            dgvHistoricoSaidas.Rows.Add(reader["data"], reader["produto"], reader["quantidade"], reader["usuario"], reader["destino"]);
//                    }
//                }
//            }
//        }

//        // ==================== EVENTOS DOS BOTÕES ====================

//        private void btnAplicarFiltros_Click(object sender, EventArgs e)
//        {
//            produtoSelecionado = cbxprodutoSelecionado.SelectedIndex == 0 ? "" : cbxprodutoSelecionado.Text;
//            somentePrincipais = false;
//            CarregarDados();
//        }


//        private void btnLimparFiltros_Click(object sender, EventArgs e)
//        {
//            produtoSelecionado = "";
//            cbxprodutoSelecionado.SelectedIndex = 0;
//            somentePrincipais = false;
//            CarregarProdutos();
//            CarregarDados();
//            btnProdutosPrincipais.BackColor = SystemColors.Control;
//            btnProdutosPrincipais.Text = "Produtos Principais";
//        }

//        private void btnProdutosPrincipais_Click(object sender, EventArgs e)
//        {
//            // Alterna entre modo principais e todos os produtos
//            somentePrincipais = !somentePrincipais;

//            if (somentePrincipais)
//            {
//                // Modo produtos principais ativado
//                produtoSelecionado = "";
//                cbxprodutoSelecionado.Items.Clear();
//                cbxprodutoSelecionado.Items.Add("Todos os produtos");
//                foreach (var produto in produtosPrincipais)
//                {
//                    cbxprodutoSelecionado.Items.Add(produto);
//                }
//                cbxprodutoSelecionado.SelectedIndex = 0;
//                btnProdutosPrincipais.BackColor = Color.LightGreen;
//                btnProdutosPrincipais.Text = "✔ Produtos Principais";
//            }
//            else
//            {
//                // Modo todos os produtos
//                CarregarProdutos(); // Recarrega todos os produtos
//                btnProdutosPrincipais.BackColor = SystemColors.Control;
//                btnProdutosPrincipais.Text = "Produtos Principais";
//            }

//            CarregarDados();
//        }

//        // Lista de produtos da cesta básica (USE OS NOMES EXATOS DO BANCO)
//        private List<string> produtosPrincipais = new List<string>
//            {
//                "ARROZ 1KG",
//                "ARROZ 2KG",
//                "ARROZ 5KG",
//                "FEIJAO 1KG",
//                "MOLHO DE TOMATE",
//                "MACARRAO 500G",
//                "ACUCAR 1KG",
//                "SAL 1KG",
//                "OLEO 900ML",
//                "FUBA 400G",
//                "FUBA 500G",
//                "LEITE 1L"
//            };


//        private void btnMenu_Click(object sender, EventArgs e)
//        {
//            frmMenuPrincipal menu = new frmMenuPrincipal(codUsuLogado);
//            menu.Show();
//            this.Close();
//        }

//        private void btnCadastrar_Click(object sender, EventArgs e)
//        {
//            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado);
//            abrir.Show();
//            this.Close();
//        }

//        private void VerificacaoSistema()
//        {
//            using (var conn = DataBaseConnection.OpenConnection())
//            {
//                int total = Convert.ToInt32(new MySqlCommand("SELECT COUNT(*) FROM tbLista", conn).ExecuteScalar());
//                if (total == 0)
//                    MessageBox.Show("Nenhum produto cadastrado.");
//            }
//        }

//        private void dgvEstoque_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) { }

//        // ==================== MÉTODOS DE IMPORTAÇÃO/EXPORTAÇÃO ====================

//        private void BtnImportar_Click(object sender, EventArgs e)
//        {
//            OpenFileDialog ofd = new OpenFileDialog
//            {
//                Filter = "Arquivos Excel|*.xlsx;*.xls",
//                Title = "Selecione o arquivo Excel com as entradas de estoque"
//            };

//            if (ofd.ShowDialog() != DialogResult.OK) return;

//            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//            using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
//            using (var reader = ExcelReaderFactory.CreateReader(stream))
//            {
//                var conf = new ExcelDataSetConfiguration
//                {
//                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
//                };

//                var result = reader.AsDataSet(conf);
//                DataTable dt = result.Tables[0];

//                if (!dt.Columns.Contains("Produto") || !dt.Columns.Contains("Qtd"))
//                {
//                    MessageBox.Show("A planilha precisa ter as colunas: Produto e Qtd");
//                    return;
//                }

//                using (var conn = DataBaseConnection.OpenConnection())
//                using (var trans = conn.BeginTransaction())
//                {
//                    try
//                    {
//                        foreach (DataRow row in dt.Rows)
//                        {
//                            string produto = row["Produto"]?.ToString()?.Trim();
//                            if (!int.TryParse(row["Qtd"]?.ToString(), out int qtd)) continue;
//                            if (string.IsNullOrEmpty(produto) || qtd <= 0) continue;

//                            int codList;
//                            using (var cmd = new MySqlCommand("SELECT codList FROM tbLista WHERE descricao = @desc", conn, trans))
//                            {
//                                cmd.Parameters.AddWithValue("@desc", produto);
//                                var resultCod = cmd.ExecuteScalar();
//                                if (resultCod == null) continue;
//                                codList = Convert.ToInt32(resultCod);
//                            }

//                            string sql = @"
//                                INSERT INTO tbProdutos
//                                (descricao, quantidade, peso, unidade, dataDeEntrada,
//                                 dataDeValidade, dataLimiteDeSaida, tipoMovimentacao,
//                                 codUsu, codOri, codList)
//                                VALUES
//                                (@desc, @qtd,
//                                 (SELECT peso FROM tbLista WHERE codList = @codList),
//                                 'UNIDADES (UN)',
//                                 NOW(),
//                                 DATE_ADD(NOW(), INTERVAL 30 DAY),
//                                 DATE_ADD(NOW(), INTERVAL 60 DAY),
//                                 'ENTRADA',
//                                 @codUsu,
//                                 1,
//                                 @codList)";

//                            using (var cmd = new MySqlCommand(sql, conn, trans))
//                            {
//                                cmd.Parameters.AddWithValue("@desc", produto);
//                                cmd.Parameters.AddWithValue("@qtd", qtd);
//                                cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
//                                cmd.Parameters.AddWithValue("@codList", codList);
//                                cmd.ExecuteNonQuery();
//                            }
//                        }
//                        trans.Commit();
//                        MessageBox.Show("Importação concluída com sucesso.");
//                        CarregarDados();
//                        CarregarProdutosSaida();
//                    }
//                    catch (Exception ex)
//                    {
//                        trans.Rollback();
//                        MessageBox.Show("Erro: " + ex.Message);
//                    }
//                }
//            }
//        }

//        private void BtnExportar_Click(object sender, EventArgs e)
//        {
//            if (tabPageSaidas != null && tabPageSaidas.Visible)
//            {
//                ContextMenuStrip menuExportar = new ContextMenuStrip();
//                ToolStripMenuItem itemCSV = new ToolStripMenuItem("📄 Exportar Histórico de Saídas para CSV");
//                ToolStripMenuItem itemExcel = new ToolStripMenuItem("📊 Exportar Histórico de Saídas para Excel");
//                ToolStripMenuItem itemImprimir = new ToolStripMenuItem("🖨️ Imprimir Histórico de Saídas");

//                itemCSV.Click += (s, ev) => ExportarHistoricoSaidasCSV();
//                itemExcel.Click += (s, ev) => ExportarHistoricoSaidasExcel();
//                itemImprimir.Click += (s, ev) => ImprimirHistoricoSaidas();

//                menuExportar.Items.AddRange(new ToolStripItem[] { itemCSV, itemExcel, itemImprimir });
//                Button btn = sender as Button;
//                if (btn != null) menuExportar.Show(btn, new Point(0, btn.Height));
//            }
//            else
//            {
//                ContextMenuStrip menuExportar = new ContextMenuStrip();
//                ToolStripMenuItem itemCSV = new ToolStripMenuItem("📄 Exportar Estoque para CSV");
//                ToolStripMenuItem itemExcel = new ToolStripMenuItem("📊 Exportar Estoque para Excel");
//                ToolStripMenuItem itemImprimir = new ToolStripMenuItem("🖨️ Imprimir Estoque");

//                itemCSV.Click += (s, ev) => ExportarEstoqueCSV();
//                itemExcel.Click += (s, ev) => ExportarEstoqueExcel();
//                itemImprimir.Click += (s, ev) => ImprimirEstoque();

//                menuExportar.Items.AddRange(new ToolStripItem[] { itemCSV, itemExcel, itemImprimir });
//                Button btn = sender as Button;
//                if (btn != null) menuExportar.Show(btn, new Point(0, btn.Height));
//            }
//        }

//        private void ExportarEstoqueCSV()
//        {
//            if (dgvEstoque.Rows.Count == 0)
//            {
//                MessageBox.Show("Não há dados para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            SaveFileDialog sfd = new SaveFileDialog
//            {
//                Filter = "Arquivos CSV (*.csv)|*.csv",
//                Title = "Exportar Estoque para CSV",
//                FileName = $"Estoque_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
//            };

//            if (sfd.ShowDialog() != DialogResult.OK) return;

//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                for (int i = 0; i < dgvEstoque.Columns.Count; i++)
//                {
//                    sb.Append($"\"{dgvEstoque.Columns[i].HeaderText}\"");
//                    if (i < dgvEstoque.Columns.Count - 1) sb.Append(";");
//                }
//                sb.AppendLine();

//                foreach (DataGridViewRow row in dgvEstoque.Rows)
//                {
//                    if (row.IsNewRow) continue;
//                    for (int i = 0; i < dgvEstoque.Columns.Count; i++)
//                    {
//                        string valor = row.Cells[i].Value?.ToString() ?? "";
//                        sb.Append($"\"{valor.Replace("\"", "\"\"")}\"");
//                        if (i < dgvEstoque.Columns.Count - 1) sb.Append(";");
//                    }
//                    sb.AppendLine();
//                }

//                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
//                MessageBox.Show($"✅ Arquivo exportado com sucesso!\n\nLocal: {sfd.FileName}", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void ExportarEstoqueExcel()
//        {
//            if (dgvEstoque.Rows.Count == 0)
//            {
//                MessageBox.Show("Não há dados para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            SaveFileDialog sfd = new SaveFileDialog
//            {
//                Filter = "Arquivos HTML (*.html)|*.html",
//                Title = "Exportar Estoque para Excel",
//                FileName = $"Estoque_{DateTime.Now:yyyyMMdd_HHmmss}.html"
//            };

//            if (sfd.ShowDialog() != DialogResult.OK) return;

//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                sb.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Relatório de Estoque</title>");
//                sb.AppendLine("<style>");
//                sb.AppendLine("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; }");
//                sb.AppendLine("h1 { color: #2c3e50; border-bottom: 2px solid #3498db; }");
//                sb.AppendLine("table { border-collapse: collapse; width: 100%; margin-top: 20px; }");
//                sb.AppendLine("th { background-color: #2c3e50; color: white; padding: 12px; border: 1px solid #ddd; }");
//                sb.AppendLine("td { padding: 10px; border: 1px solid #ddd; }");
//                sb.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
//                sb.AppendLine(".total { background-color: #2c3e50; color: white; font-weight: bold; }");
//                sb.AppendLine("</style></head><body>");
//                sb.AppendLine($"<h1>📊 Relatório de Estoque</h1>");
//                sb.AppendLine($"<p><strong>Data de emissão:</strong> {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");
//                sb.AppendLine("<table><thead><tr>");

//                foreach (DataGridViewColumn col in dgvEstoque.Columns)
//                    sb.AppendLine($"<th>{col.HeaderText}</th>");
//                sb.AppendLine("</thead><tbody>");

//                foreach (DataGridViewRow row in dgvEstoque.Rows)
//                {
//                    if (row.IsNewRow) continue;
//                    sb.AppendLine("<tr>");
//                    foreach (DataGridViewCell cell in row.Cells)
//                    {
//                        string valor = cell.Value?.ToString() ?? "";
//                        if (row.Cells[0].Value?.ToString()?.Contains("TOTAL") == true)
//                            sb.AppendLine($"<td class='total'><strong>{valor}</strong></td>");
//                        else
//                            sb.AppendLine($"<td style='text-align:center'>{valor}</td>");
//                    }
//                    sb.AppendLine("</tr>");
//                }

//                sb.AppendLine("</tbody></table>");
//                sb.AppendLine("<div class='footer'>");
//                sb.AppendLine($"<p>Relatório gerado automaticamente pelo Sistema GPSFA</p>");
//                sb.AppendLine($"<p>Total de registros: {dgvEstoque.Rows.Count - 1} itens</p>");
//                sb.AppendLine("</div></body></html>");

//                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
//                MessageBox.Show($"✅ Arquivo exportado com sucesso!\n\nLocal: {sfd.FileName}\n\nAbra o arquivo no Excel.", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void ImprimirEstoque()
//        {
//            if (dgvEstoque.Rows.Count == 0)
//            {
//                MessageBox.Show("Não há dados para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            PrintDialog printDialog = new PrintDialog();
//            PrintDocument printDocument = new PrintDocument();
//            int linhaAtual = 0;

//            printDocument.PrintPage += (sender, e) =>
//            {
//                Font tituloFont = new Font("Segoe UI", 16, FontStyle.Bold);
//                Font subtituloFont = new Font("Segoe UI", 12, FontStyle.Regular);
//                Font cabecalhoFont = new Font("Segoe UI", 10, FontStyle.Bold);
//                Font textoFont = new Font("Segoe UI", 9, FontStyle.Regular);

//                float yPos = e.MarginBounds.Top;
//                float leftMargin = e.MarginBounds.Left;
//                float pageWidth = e.MarginBounds.Width;

//                e.Graphics.DrawString("RELATÓRIO DE ESTOQUE", tituloFont, Brushes.Black, leftMargin, yPos);
//                yPos += 35;
//                e.Graphics.DrawString($"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", subtituloFont, Brushes.Black, leftMargin, yPos);
//                yPos += 25;
//                e.Graphics.DrawString($"Total de itens: {dgvEstoque.Rows.Count - 1} registros", subtituloFont, Brushes.Black, leftMargin, yPos);
//                yPos += 35;

//                float colX = leftMargin;
//                float colWidth = pageWidth / dgvEstoque.Columns.Count;

//                e.Graphics.FillRectangle(Brushes.LightGray, leftMargin, yPos, pageWidth, 25);
//                for (int i = 0; i < dgvEstoque.Columns.Count; i++)
//                {
//                    e.Graphics.DrawString(dgvEstoque.Columns[i].HeaderText, cabecalhoFont, Brushes.Black, colX + 5, yPos + 5);
//                    colX += colWidth;
//                }
//                yPos += 30;

//                for (int i = linhaAtual; i < dgvEstoque.Rows.Count; i++)
//                {
//                    DataGridViewRow row = dgvEstoque.Rows[i];
//                    if (row.IsNewRow) continue;

//                    colX = leftMargin;
//                    if (yPos + 25 > e.MarginBounds.Bottom)
//                    {
//                        linhaAtual = i;
//                        e.HasMorePages = true;
//                        return;
//                    }

//                    bool isTotal = row.Cells[0].Value?.ToString()?.Contains("TOTAL") == true;
//                    if (isTotal)
//                        e.Graphics.FillRectangle(Brushes.DarkGray, leftMargin, yPos, pageWidth, 22);

//                    for (int j = 0; j < dgvEstoque.Columns.Count; j++)
//                    {
//                        string valor = row.Cells[j].Value?.ToString() ?? "";
//                        e.Graphics.DrawString(valor, textoFont, isTotal ? Brushes.White : Brushes.Black, colX + 5, yPos + 3);
//                        colX += colWidth;
//                    }
//                    yPos += 22;
//                }

//                linhaAtual = 0;
//                e.HasMorePages = false;
//                e.Graphics.DrawString($"Relatório gerado pelo Sistema GPSFA", subtituloFont, Brushes.Gray, leftMargin, yPos + 10);
//            };

//            printDialog.Document = printDocument;
//            if (printDialog.ShowDialog() == DialogResult.OK)
//            {
//                linhaAtual = 0;
//                printDocument.Print();
//            }
//        }

//        private void ExportarHistoricoSaidasCSV()
//        {
//            if (dgvHistoricoSaidas == null || dgvHistoricoSaidas.Rows.Count == 0)
//            {
//                MessageBox.Show("Não há dados no histórico para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            SaveFileDialog sfd = new SaveFileDialog
//            {
//                Filter = "Arquivos CSV (*.csv)|*.csv",
//                Title = "Exportar Histórico de Saídas para CSV",
//                FileName = $"Historico_Saidas_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
//            };

//            if (sfd.ShowDialog() != DialogResult.OK) return;

//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                for (int i = 0; i < dgvHistoricoSaidas.Columns.Count; i++)
//                {
//                    sb.Append($"\"{dgvHistoricoSaidas.Columns[i].HeaderText}\"");
//                    if (i < dgvHistoricoSaidas.Columns.Count - 1) sb.Append(";");
//                }
//                sb.AppendLine();

//                foreach (DataGridViewRow row in dgvHistoricoSaidas.Rows)
//                {
//                    if (row.IsNewRow) continue;
//                    for (int i = 0; i < dgvHistoricoSaidas.Columns.Count; i++)
//                    {
//                        string valor = row.Cells[i].Value?.ToString() ?? "";
//                        sb.Append($"\"{valor.Replace("\"", "\"\"")}\"");
//                        if (i < dgvHistoricoSaidas.Columns.Count - 1) sb.Append(";");
//                    }
//                    sb.AppendLine();
//                }

//                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
//                MessageBox.Show($"✅ Histórico exportado!\n\nLocal: {sfd.FileName}", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void ExportarHistoricoSaidasExcel()
//        {
//            if (dgvHistoricoSaidas == null || dgvHistoricoSaidas.Rows.Count == 0)
//            {
//                MessageBox.Show("Não há dados no histórico para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            SaveFileDialog sfd = new SaveFileDialog
//            {
//                Filter = "Arquivos Excel (*.xlsx)|*.xlsx",
//                Title = "Exportar Histórico de Saídas para Excel",
//                FileName = $"Historico_Saidas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
//            };

//            if (sfd.ShowDialog() != DialogResult.OK) return;

//            try
//            {
//                using (var workbook = new XLWorkbook())
//                {
//                    var worksheet = workbook.Worksheets.Add("Historico_Saidas");

//                    for (int i = 0; i < dgvHistoricoSaidas.Columns.Count; i++)
//                    {
//                        worksheet.Cell(1, i + 1).Value = dgvHistoricoSaidas.Columns[i].HeaderText;
//                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
//                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(52, 73, 94);
//                        worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
//                    }

//                    for (int i = 0; i < dgvHistoricoSaidas.Rows.Count; i++)
//                    {
//                        for (int j = 0; j < dgvHistoricoSaidas.Columns.Count; j++)
//                        {
//                            worksheet.Cell(i + 2, j + 1).Value = dgvHistoricoSaidas.Rows[i].Cells[j].Value?.ToString() ?? "";
//                        }
//                    }

//                    worksheet.Columns().AdjustToContents();
//                    workbook.SaveAs(sfd.FileName);
//                }

//                MessageBox.Show($"✅ Histórico exportado!\n\nLocal: {sfd.FileName}", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void ImprimirHistoricoSaidas()
//        {
//            if (dgvHistoricoSaidas == null || dgvHistoricoSaidas.Rows.Count == 0)
//            {
//                MessageBox.Show("Não há registros de saídas para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            PrintDialog printDialog = new PrintDialog();
//            PrintDocument printDocument = new PrintDocument();
//            int linhaAtual = 0;

//            printDocument.PrintPage += (senderPrint, ePrint) =>
//            {
//                Font tituloFont = new Font("Segoe UI", 14, FontStyle.Bold);
//                Font subtituloFont = new Font("Segoe UI", 10, FontStyle.Regular);
//                Font cabecalhoFont = new Font("Segoe UI", 9, FontStyle.Bold);
//                Font textoFont = new Font("Segoe UI", 8, FontStyle.Regular);

//                float yPos = ePrint.MarginBounds.Top;
//                float leftMargin = ePrint.MarginBounds.Left;
//                float pageWidth = ePrint.MarginBounds.Width;

//                ePrint.Graphics.DrawString("RELATÓRIO DE SAÍDAS - ESTOQUE", tituloFont, Brushes.Black, leftMargin, yPos);
//                yPos += 35;
//                ePrint.Graphics.DrawString($"Data de emissão: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", subtituloFont, Brushes.Black, leftMargin, yPos);
//                yPos += 25;
//                ePrint.Graphics.DrawString($"Total de saídas: {dgvHistoricoSaidas.Rows.Count} registros", subtituloFont, Brushes.Black, leftMargin, yPos);
//                yPos += 30;

//                float[] colWidths = { 100, 200, 60, 100, 150 };
//                float colX = leftMargin;

//                ePrint.Graphics.FillRectangle(Brushes.LightGray, leftMargin, yPos, pageWidth, 22);
//                string[] headers = { "Data/Hora", "Produto", "Qtd", "Usuário", "Destino" };
//                for (int i = 0; i < headers.Length; i++)
//                {
//                    ePrint.Graphics.DrawString(headers[i], cabecalhoFont, Brushes.Black, colX + 3, yPos + 3);
//                    colX += colWidths[i];
//                }
//                yPos += 25;

//                for (int i = linhaAtual; i < dgvHistoricoSaidas.Rows.Count; i++)
//                {
//                    DataGridViewRow row = dgvHistoricoSaidas.Rows[i];
//                    if (row.IsNewRow) continue;

//                    colX = leftMargin;
//                    if (yPos + 25 > ePrint.MarginBounds.Bottom)
//                    {
//                        linhaAtual = i;
//                        ePrint.HasMorePages = true;
//                        return;
//                    }

//                    ePrint.Graphics.DrawString(row.Cells["data"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
//                    colX += colWidths[0];
//                    ePrint.Graphics.DrawString(row.Cells["produto"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
//                    colX += colWidths[1];
//                    ePrint.Graphics.DrawString(row.Cells["quantidade"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
//                    colX += colWidths[2];
//                    ePrint.Graphics.DrawString(row.Cells["usuario"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
//                    colX += colWidths[3];
//                    ePrint.Graphics.DrawString(row.Cells["destino"].Value?.ToString() ?? "", textoFont, Brushes.Black, colX + 3, yPos + 3);
//                    yPos += 22;
//                }

//                linhaAtual = 0;
//                ePrint.HasMorePages = false;
//                ePrint.Graphics.DrawString($"Relatório gerado pelo Sistema GPSFA", subtituloFont, Brushes.Gray, leftMargin, yPos + 10);
//            };

//            printDialog.Document = printDocument;
//            if (printDialog.ShowDialog() == DialogResult.OK)
//            {
//                linhaAtual = 0;
//                printDocument.Print();
//            }
//        }

//        private void btnSincronizarEstoque_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                OpenFileDialog ofd = new OpenFileDialog
//                {
//                    Filter = "Arquivos Excel|*.xlsx;*.xls",
//                    Title = "Selecione a planilha com o estoque corrigido"
//                };

//                if (ofd.ShowDialog() != DialogResult.OK) return;

//                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//                using (var fileStream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
//                using (var excelReader = ExcelReaderFactory.CreateReader(fileStream))
//                {
//                    var config = new ExcelDataSetConfiguration
//                    {
//                        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
//                    };

//                    var dataSet = excelReader.AsDataSet(config);
//                    DataTable tabela = dataSet.Tables[0];

//                    if (!tabela.Columns.Contains("Produto") || !tabela.Columns.Contains("Qtd"))
//                    {
//                        MessageBox.Show("A planilha precisa ter as colunas: Produto e Qtd");
//                        return;
//                    }

//                    int ajustesRealizados = 0;
//                    StringBuilder log = new StringBuilder();

//                    using (var conexao = DataBaseConnection.OpenConnection())
//                    using (var transacao = conexao.BeginTransaction())
//                    {
//                        try
//                        {
//                            foreach (DataRow linha in tabela.Rows)
//                            {
//                                string nomeProduto = linha["Produto"]?.ToString()?.Trim();
//                                string qtdStr = linha["Qtd"]?.ToString()?.Trim();

//                                if (string.IsNullOrEmpty(nomeProduto) || nomeProduto.ToUpper().Contains("TOTAL"))
//                                    continue;

//                                if (!int.TryParse(qtdStr, out int quantidadeCorreta))
//                                    continue;

//                                int codigoProduto;
//                                string sqlBusca = "SELECT codList FROM tbLista WHERE TRIM(descricao) = @produto";
//                                using (var cmd = new MySqlCommand(sqlBusca, conexao, transacao))
//                                {
//                                    cmd.Parameters.AddWithValue("@produto", nomeProduto);
//                                    var result = cmd.ExecuteScalar();
//                                    if (result == null)
//                                    {
//                                        log.AppendLine($"Produto não encontrado: {nomeProduto}");
//                                        continue;
//                                    }
//                                    codigoProduto = Convert.ToInt32(result);
//                                }

//                                int saldoAtual;
//                                string sqlSaldo = "SELECT COALESCE(SUM(quantidade), 0) FROM tbProdutos WHERE codList = @codList";
//                                using (var cmd = new MySqlCommand(sqlSaldo, conexao, transacao))
//                                {
//                                    cmd.Parameters.AddWithValue("@codList", codigoProduto);
//                                    saldoAtual = Convert.ToInt32(cmd.ExecuteScalar());
//                                }

//                                int diferenca = quantidadeCorreta - saldoAtual;
//                                if (diferenca != 0)
//                                {
//                                    string sqlInsert = @"
//                                        INSERT INTO tbProdutos
//                                            (descricao, quantidade, peso, unidade, dataDeEntrada,
//                                             dataDeValidade, dataLimiteDeSaida, tipoMovimentacao,
//                                             codUsu, codOri, codList, destino)
//                                        VALUES
//                                            ((SELECT descricao FROM tbLista WHERE codList = @codList),
//                                             @quantidade,
//                                             (SELECT peso FROM tbLista WHERE codList = @codList),
//                                             'UNIDADES (UN)',
//                                             NOW(),
//                                             DATE_ADD(NOW(), INTERVAL 30 DAY),
//                                             DATE_ADD(NOW(), INTERVAL 60 DAY),
//                                             'AJUSTE',
//                                             @codUsu,
//                                             1,
//                                             @codList,
//                                             'AJUSTE AUTOMATICO (SINCRONIZACAO EXCEL)')";

//                                    using (var cmd = new MySqlCommand(sqlInsert, conexao, transacao))
//                                    {
//                                        cmd.Parameters.AddWithValue("@codList", codigoProduto);
//                                        cmd.Parameters.AddWithValue("@quantidade", diferenca);
//                                        cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
//                                        cmd.ExecuteNonQuery();
//                                    }

//                                    ajustesRealizados++;
//                                    log.AppendLine($"OK: {nomeProduto} | Ajuste: {diferenca}");
//                                }
//                            }

//                            transacao.Commit();
//                            CarregarDados();
//                            CarregarProdutosSaida();
//                            CarregarHistoricoSaidas("");
//                            MessageBox.Show($"Sincronização concluída.\n\nAjustes: {ajustesRealizados}\n\n{log}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                        }
//                        catch (Exception ex)
//                        {
//                            transacao.Rollback();
//                            MessageBox.Show($"Erro na sincronização:\n{ex.Message}");
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Erro ao abrir arquivo:\n{ex.Message}");
//            }
//        }
//    }
//}
