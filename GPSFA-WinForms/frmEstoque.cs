using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ExcelDataReader;

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

        // ===== Controles para histórico de saídas =====
        private DataGridView dgvHistoricoSaidas;

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
            btnAplicarFiltros.Click += btnAplicarFiltro_Click;
            btnLimparFiltros.Click += btnLimparFiltro_Click;
            btnProdutosPrincipais.Click += btnPrincipaisProdutos_Click;
            btnAplicarModo.Click += btnAlternarModo_Click;

            // Usar o botão que já existe no Designer
            btnImportar.Click += BtnImportar_Click;

            ConfigurarDataGridView(modoAgrupado);
            CarregarProdutos();
            CarregarDados();

            ConfigurarAbaHistorico();
            ConfigurarTabRegistrarSaida();
            CarregarProdutosSaida();

            VerificacaoSistema();
        }

        // ===== Importar dados do Excel =====
        private void BtnImportar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Arquivos Excel|*.xlsx;*.xls",
                Title = "Selecione o arquivo Excel com as entradas de estoque"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var result = reader.AsDataSet(conf);
                        DataTable dt = result.Tables[0];

                        int registrosImportados = 0;
                        int registrosErro = 0;
                        int registrosIgnorados = 0;
                        StringBuilder erros = new StringBuilder();
                        HashSet<string> linhasProcessadas = new HashSet<string>();

                        using (var conn = DataBaseConnection.OpenConnection())
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                System.Diagnostics.Debug.WriteLine($"Total de linhas no Excel: {dt.Rows.Count}");

                                for (int i = 1; i < dt.Rows.Count; i++)
                                {
                                    DataRow row = dt.Rows[i];

                                    try
                                    {
                                        if (row.ItemArray == null || row.ItemArray.Length < 5)
                                        {
                                            registrosIgnorados++;
                                            continue;
                                        }

                                        bool linhaVazia = true;
                                        for (int j = 0; j < 5; j++)
                                        {
                                            if (row[j] != null && !string.IsNullOrWhiteSpace(row[j]?.ToString()))
                                            {
                                                linhaVazia = false;
                                                break;
                                            }
                                        }

                                        if (linhaVazia)
                                        {
                                            registrosIgnorados++;
                                            continue;
                                        }

                                        string dataEntradaStr = row[0]?.ToString()?.Trim();
                                        string origem = row[1]?.ToString()?.Trim();
                                        string produto = row[2]?.ToString()?.Trim();
                                        string quantidadeStr = row[3]?.ToString()?.Trim();
                                        string validadeStr = row[4]?.ToString()?.Trim();

                                        string chaveLinha = $"{dataEntradaStr}|{origem}|{produto}|{quantidadeStr}|{validadeStr}";
                                        if (linhasProcessadas.Contains(chaveLinha))
                                        {
                                            registrosIgnorados++;
                                            erros.AppendLine($"Linha {i + 1}: Linha duplicada ignorada");
                                            continue;
                                        }
                                        linhasProcessadas.Add(chaveLinha);

                                        if (string.IsNullOrWhiteSpace(produto))
                                        {
                                            registrosErro++;
                                            erros.AppendLine($"Linha {i + 1}: Produto não informado");
                                            continue;
                                        }

                                        if (string.IsNullOrWhiteSpace(quantidadeStr))
                                        {
                                            registrosErro++;
                                            erros.AppendLine($"Linha {i + 1}: Quantidade não informada para o produto {produto}");
                                            continue;
                                        }

                                        DateTime dataEntrada;
                                        if (!DateTime.TryParse(dataEntradaStr, out dataEntrada))
                                        {
                                            dataEntrada = DateTime.Now;
                                            erros.AppendLine($"Linha {i + 1}: Data de entrada inválida para {produto}, usando data atual");
                                        }

                                        DateTime validade;
                                        if (!DateTime.TryParse(validadeStr, out validade))
                                        {
                                            validade = DateTime.Now.AddMonths(6);
                                            erros.AppendLine($"Linha {i + 1}: Data de validade inválida para {produto}, usando data padrão");
                                        }

                                        quantidadeStr = quantidadeStr.Replace(',', '.').Trim();

                                        if (quantidadeStr.Contains(".") && quantidadeStr.IndexOf(".") != quantidadeStr.LastIndexOf("."))
                                        {
                                            quantidadeStr = quantidadeStr.Replace(".", "");
                                        }

                                        if (!decimal.TryParse(quantidadeStr, System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture, out decimal quantidadeDecimal))
                                        {
                                            registrosErro++;
                                            erros.AppendLine($"Linha {i + 1}: Quantidade inválida '{quantidadeStr}' para o produto {produto}");
                                            continue;
                                        }

                                        int quantidade = (int)Math.Round(quantidadeDecimal, MidpointRounding.AwayFromZero);

                                        if (quantidade <= 0)
                                        {
                                            registrosErro++;
                                            erros.AppendLine($"Linha {i + 1}: Quantidade deve ser maior que zero para o produto {produto}");
                                            continue;
                                        }

                                        int codList = ObterCodigoLista(conn, trans, produto);
                                        if (codList == 0)
                                        {
                                            registrosErro++;
                                            erros.AppendLine($"Linha {i + 1}: Produto não encontrado no cadastro: {produto}");
                                            continue;
                                        }

                                        int codOri = ObterCodigoOrigem(conn, trans, origem);
                                        if (codOri == 0 && !string.IsNullOrWhiteSpace(origem))
                                        {
                                            codOri = InserirNovaOrigem(conn, trans, origem);
                                            erros.AppendLine($"Linha {i + 1}: Nova origem criada: {origem}");
                                        }
                                        else if (codOri == 0)
                                        {
                                            codOri = 1;
                                        }

                                        int peso = ObterPesoProduto(conn, trans, codList);

                                        InserirEntradaEstoque(conn, trans, codList, codOri, produto, quantidade, peso, dataEntrada, validade);

                                        registrosImportados++;
                                    }
                                    catch (Exception ex)
                                    {
                                        registrosErro++;
                                        erros.AppendLine($"Linha {i + 1}: Erro inesperado - {ex.Message}");
                                    }
                                }

                                trans.Commit();

                                System.Diagnostics.Debug.WriteLine($"Registros importados: {registrosImportados}");
                                System.Diagnostics.Debug.WriteLine($"Registros com erro: {registrosErro}");
                                System.Diagnostics.Debug.WriteLine($"Registros ignorados: {registrosIgnorados}");

                                string mensagem = $"✅ Importação concluída!\n\n" +
                                                $"Registros importados com sucesso: {registrosImportados}\n" +
                                                $"Registros ignorados (vazios/duplicados): {registrosIgnorados}\n" +
                                                $"Registros com erro: {registrosErro}";

                                if (erros.Length > 0)
                                {
                                    mensagem += $"\n\n📋 Detalhes dos erros e avisos:\n{erros.ToString()}";
                                }

                                MessageBox.Show(mensagem, "Resultado da Importação",
                                    MessageBoxButtons.OK, registrosErro > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                                CarregarDados();
                                CarregarProdutosSaida();
                                CarregarHistoricoSaidas("");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                MessageBox.Show($"❌ Erro durante a importação: {ex.Message}", "Erro",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao abrir arquivo: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== Métodos auxiliares para importação =====
        private int ObterCodigoLista(MySqlConnection conn, MySqlTransaction trans, string produto)
        {
            string sql = "SELECT codList FROM tbLista WHERE descricao = @produto";
            using (var cmd = new MySqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@produto", produto);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private int ObterCodigoOrigem(MySqlConnection conn, MySqlTransaction trans, string origem)
        {
            if (string.IsNullOrEmpty(origem))
                return 0;

            string sql = "SELECT codOri FROM tbOrigemDoacao WHERE nome = @origem";
            using (var cmd = new MySqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@origem", origem.Trim());
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private int InserirNovaOrigem(MySqlConnection conn, MySqlTransaction trans, string origem)
        {
            if (string.IsNullOrEmpty(origem))
                return 1;

            string sql = @"INSERT INTO tbOrigemDoacao (nome) VALUES (@nome);
                          SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@nome", origem.Trim());
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int ObterPesoProduto(MySqlConnection conn, MySqlTransaction trans, int codList)
        {
            string sql = "SELECT peso FROM tbLista WHERE codList = @codList";
            using (var cmd = new MySqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@codList", codList);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 1000;
            }
        }

        private void InserirEntradaEstoque(MySqlConnection conn, MySqlTransaction trans,
            int codList, int codOri, string produto, int quantidade, int peso,
            DateTime dataEntrada, DateTime validade)
        {
            string sqlInsert = @"
                INSERT INTO tbProdutos 
                    (descricao, quantidade, peso, unidade, codBar, 
                     dataDeEntrada, dataDeValidade, dataLimiteDeSaida, 
                     tipoMovimentacao, codUsu, codOri, codList)
                VALUES 
                    (@descricao, @quantidade, @peso, 'UNIDADES (UN)', NULL,
                     @dataEntrada, @validade, DATE_ADD(@validade, INTERVAL -30 DAY),
                     'ENTRADA', @codUsu, @codOri, @codList)";

            using (var cmd = new MySqlCommand(sqlInsert, conn, trans))
            {
                cmd.Parameters.AddWithValue("@descricao", produto);
                cmd.Parameters.AddWithValue("@quantidade", quantidade);
                cmd.Parameters.AddWithValue("@peso", peso);
                cmd.Parameters.AddWithValue("@dataEntrada", dataEntrada);
                cmd.Parameters.AddWithValue("@validade", validade);
                cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
                cmd.Parameters.AddWithValue("@codOri", codOri);
                cmd.Parameters.AddWithValue("@codList", codList);
                cmd.ExecuteNonQuery();
            }

            string sqlUpdateEstoque = @"
                UPDATE tbEstoqueItens 
                SET quantidade = quantidade + @quantidade,
                    dataMovimentacao = CURRENT_DATE(),
                    horaMovimentacao = CURRENT_TIME()
                WHERE codList = @codList";

            using (var cmd = new MySqlCommand(sqlUpdateEstoque, conn, trans))
            {
                cmd.Parameters.AddWithValue("@quantidade", quantidade);
                cmd.Parameters.AddWithValue("@codList", codList);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas == 0)
                {
                    string sqlInsertEstoque = @"
                        INSERT INTO tbEstoqueItens (codList, quantidade, dataMovimentacao, horaMovimentacao)
                        VALUES (@codList, @quantidade, CURRENT_DATE(), CURRENT_TIME())";

                    using (var cmdInsert = new MySqlCommand(sqlInsertEstoque, conn, trans))
                    {
                        cmdInsert.Parameters.AddWithValue("@codList", codList);
                        cmdInsert.Parameters.AddWithValue("@quantidade", quantidade);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        // ===== Configurar aba de histórico =====
        // ===== Configurar aba de histórico (COM COLUNA DESTINO) =====
        private void ConfigurarAbaHistorico()
        {
            if (tabPageSaidas == null) return;

            tabPageSaidas.Controls.Clear();

            // Painel de filtros
            Panel panelFiltrosHistorico = new Panel
            {
                Height = 35,
                Dock = DockStyle.Top,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(5)
            };

            Label lblFiltroHistorico = new Label
            {
                Location = new Point(10, 8),
                Size = new Size(45, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            TextBox txtFiltroHistorico = new TextBox
            {
                Location = new Point(60, 5),
                Width = 200,
                Height = 25
            };

            Button btnFiltrarHistorico = new Button
            {
                Text = "Filtrar",
                Location = new Point(270, 5),
                Width = 70,
                Height = 25
            };

            Button btnLimparFiltroHistorico = new Button
            {
                Text = "Limpar",
                Location = new Point(350, 5),
                Width = 70,
                Height = 25
            };

            panelFiltrosHistorico.Controls.AddRange(new Control[] {
        lblFiltroHistorico, txtFiltroHistorico, btnFiltrarHistorico, btnLimparFiltroHistorico
    });

            // DataGridView
            dgvHistoricoSaidas = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.LightGray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 14)
            };

            // Estilo das linhas alternadas
            dgvHistoricoSaidas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvHistoricoSaidas.RowsDefaultCellStyle.BackColor = Color.White;
            dgvHistoricoSaidas.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvHistoricoSaidas.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvHistoricoSaidas.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Estilo do cabeçalho
            dgvHistoricoSaidas.EnableHeadersVisualStyles = false;
            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistoricoSaidas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dgvHistoricoSaidas.ColumnHeadersHeight = 30;

            // Configurar colunas COM DESTINO
            DataGridViewTextBoxColumn colData = new DataGridViewTextBoxColumn
            {
                Name = "data",
                HeaderText = "Data/Hora",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "dd/MM/yyyy HH:mm"
                }
            };

            DataGridViewTextBoxColumn colProduto = new DataGridViewTextBoxColumn
            {
                Name = "produto",
                HeaderText = "Produto",
                FillWeight = 35,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            };

            DataGridViewTextBoxColumn colQuantidade = new DataGridViewTextBoxColumn
            {
                Name = "quantidade",
                HeaderText = "Qtd",
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0"
                }
            };

            DataGridViewTextBoxColumn colUsuario = new DataGridViewTextBoxColumn
            {
                Name = "usuario",
                HeaderText = "Usuário",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            };

            DataGridViewTextBoxColumn colDestino = new DataGridViewTextBoxColumn
            {
                Name = "destino",
                HeaderText = "Destino",
                FillWeight = 25, // 25% do espaço
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            };

            dgvHistoricoSaidas.Columns.AddRange(new DataGridViewColumn[] {
        colData, colProduto, colQuantidade, colUsuario, colDestino
    });

            // Eventos de filtro
            btnFiltrarHistorico.Click += (s, ev) => CarregarHistoricoSaidas(txtFiltroHistorico.Text);
            btnLimparFiltroHistorico.Click += (s, ev) =>
            {
                txtFiltroHistorico.Clear();
                CarregarHistoricoSaidas("");
            };
            txtFiltroHistorico.KeyPress += (s, ev) =>
            {
                if (ev.KeyChar == (char)Keys.Enter)
                    CarregarHistoricoSaidas(txtFiltroHistorico.Text);
            };

            tabPageSaidas.Controls.Add(dgvHistoricoSaidas);
            tabPageSaidas.Controls.Add(panelFiltrosHistorico);

            CarregarHistoricoSaidas("");
        }


        // ===== Carregar histórico de saídas =====
        // ===== Carregar histórico de saídas (COM COLUNA DESTINO) =====
        private void CarregarHistoricoSaidas(string filtro = "")
        {
            if (dgvHistoricoSaidas == null) return;

            dgvHistoricoSaidas.Rows.Clear();

            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                {
                    string sql = @"
                SELECT 
                    DATE_FORMAT(p.dataDeEntrada, '%d/%m/%Y %H:%i') as data,
                    l.descricao as produto,
                    ABS(p.quantidade) as quantidade,
                    u.usuario as usuario,
                    p.destino as destino
                FROM tbProdutos p
                INNER JOIN tbLista l ON l.codList = p.codList
                INNER JOIN tbUsuarios u ON u.codUsu = p.codUsu
                WHERE p.quantidade < 0";

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        sql += " AND l.descricao LIKE @filtro";
                    }

                    sql += " ORDER BY p.dataDeEntrada DESC LIMIT 500";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        if (!string.IsNullOrEmpty(filtro))
                        {
                            cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string destino = reader["destino"]?.ToString() ?? "";

                                dgvHistoricoSaidas.Rows.Add(
                                    reader["data"],
                                    reader["produto"],
                                    reader["quantidade"],
                                    reader["usuario"],
                                    destino
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico: {ex.Message}");
            }
        }

        // ===== GRID (COM COLUNA ORIGEM) =====
        private void ConfigurarDataGridView(bool agrupado)
        {
            dgvEstoque.Columns.Clear();
            dgvEstoque.Rows.Clear();

            dgvEstoque.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstoque.AllowUserToAddRows = false;
            dgvEstoque.ReadOnly = true;
            dgvEstoque.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (agrupado)
            {
                dgvEstoque.Columns.Add("Produto", "Produto");

                DataGridViewTextBoxColumn colQtd = new DataGridViewTextBoxColumn
                {
                    Name = "Quantidade",
                    HeaderText = "Qtd",
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                };
                dgvEstoque.Columns.Add(colQtd);

                dgvEstoque.Columns.Add("Unidade", "Unid");
                dgvEstoque.Columns.Add("Peso", "Peso (g)");
                dgvEstoque.Columns.Add("PesoTotal", "Peso Total (kg)");
                dgvEstoque.Columns.Add("Status", "Status");
                dgvEstoque.Columns.Add("Validade", "Validade");
                dgvEstoque.Columns.Add("Origem", "Origem"); // NOVA COLUNA
            }
            else
            {
                dgvEstoque.Columns.Add("Codigo", "Código");
                dgvEstoque.Columns.Add("Produto", "Produto");
                dgvEstoque.Columns.Add("Peso", "Peso (kg)");
                dgvEstoque.Columns.Add("Unidade", "Unid");
                dgvEstoque.Columns.Add("Status", "Status");
                dgvEstoque.Columns.Add("Entrada", "Entrada");
                dgvEstoque.Columns.Add("Validade", "Validade");
                dgvEstoque.Columns.Add("Origem", "Origem"); // NOVA COLUNA
            }
        }

        // ===== PRODUTOS FILTRO =====
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

        // ===== Formatar peso =====
        private string FormatarPeso(decimal pesoGramas)
        {
            return (pesoGramas / 1000m).ToString("0.00");
        }

        // ===== Calcular status =====
        private string CalcularStatus(DateTime? validade)
        {
            if (!validade.HasValue) return "Sem validade";

            int dias = (validade.Value - DateTime.Today).Days;

            if (dias < 0) return "Vencido";
            if (dias <= 60) return "Próximo";
            return "Válido";
        }


        
        // ===== CARREGAR GRID (COM COLUNA ORIGEM) =====
        private void CarregarDados()
        {
            dgvEstoque.Rows.Clear();

            int totalQuantidade = 0;
            decimal pesoTotalGramas = 0;
            int totalProdutos = 0;

            using (var conn = DataBaseConnection.OpenConnection())
            {
                if (modoAgrupado)
                {
                    // NOVA QUERY: Agrupa por produto e traz a origem mais comum ou a mais recente
                    string sql = @"
                SELECT 
                    l.descricao AS produto, 
                    ei.quantidade,
                    l.unidade AS unidade,
                    l.peso,
                    (SELECT MIN(p2.dataDeValidade) 
                     FROM tbProdutos p2 
                     WHERE p2.codList = l.codList AND p2.quantidade > 0) AS validade,
                    (SELECT o.nome 
                     FROM tbProdutos p3
                     INNER JOIN tbOrigemDoacao o ON o.codOri = p3.codOri
                     WHERE p3.codList = l.codList AND p3.quantidade > 0
                     ORDER BY p3.dataDeEntrada DESC
                     LIMIT 1) AS origem
                FROM tbLista l
                INNER JOIN tbEstoqueItens ei ON ei.codList = l.codList
                WHERE ei.quantidade > 0
                AND (@produto = '' OR l.descricao = @produto)
                ORDER BY l.descricao";

                    if (somentePrincipais)
                        sql += " LIMIT 10";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@produto", produtoSelecionado);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int qtd = Convert.ToInt32(reader["quantidade"]);
                                decimal pesoUnitario = Convert.ToDecimal(reader["peso"]);
                                decimal pesoTotalProduto = qtd * pesoUnitario;
                                DateTime? validade = reader["validade"] != DBNull.Value ? Convert.ToDateTime(reader["validade"]) : (DateTime?)null;
                                string status = CalcularStatus(validade);
                                string origem = reader["origem"] != DBNull.Value ? reader["origem"].ToString() : "Não informado";

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
                                    origem  // NOVA COLUNA
                                );

                                if (status == "Vencido")
                                    dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                                else if (status == "Próximo")
                                    dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Khaki;
                                else if (status == "Válido")
                                    dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Honeydew;
                            }
                        }
                    }
                }
                else
                {
                    // MODO DETALHADO: Mostra a origem de cada lote individual
                    string sql = @"
                SELECT p.codProd codigo,
                       l.descricao produto,
                       l.peso,
                       l.unidade unidade,
                       p.dataDeEntrada entrada,
                       p.dataDeValidade validade,
                       o.nome origem
                FROM tbProdutos p
                INNER JOIN tbLista l ON l.codList = p.codList
                INNER JOIN tbOrigemDoacao o ON o.codOri = p.codOri
                WHERE p.quantidade > 0
                AND (@produto = '' OR l.descricao = @produto)
                ORDER BY l.descricao, p.dataDeEntrada";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@produto", produtoSelecionado);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                totalQuantidade++;

                                DateTime? validade = reader["validade"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["validade"])
                                    : (DateTime?)null;

                                string status = CalcularStatus(validade);
                                decimal pesoKg = Convert.ToDecimal(reader["peso"]) / 1000m;
                                string origem = reader["origem"] != DBNull.Value ? reader["origem"].ToString() : "Não informado";

                                int rowIndex = dgvEstoque.Rows.Add(
                                    reader["codigo"],
                                    reader["produto"],
                                    pesoKg.ToString("0.00"),
                                    reader["unidade"],
                                    status,
                                    FormatarData(reader["entrada"]),
                                    FormatarData(reader["validade"]),
                                    origem  // NOVA COLUNA
                                );

                                if (status == "Vencido")
                                    dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                                else if (status == "Próximo")
                                    dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Khaki;
                                else if (status == "Válido")
                                    dgvEstoque.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Honeydew;
                            }
                        }
                    }
                }
            }

            // Adicionar linha de total no modo agrupado
            if (modoAgrupado && dgvEstoque.Rows.Count > 0)
            {
                string pesoTotalFormatado = (pesoTotalGramas / 1000m).ToString("0.00") + " kg";

                int linhaTotal = dgvEstoque.Rows.Add(
                    "▶ TOTAL GERAL",
                    totalQuantidade.ToString("N0"),
                    $"{totalProdutos} tipos",
                    "",
                    pesoTotalFormatado,
                    "",
                    "",
                    ""  // Coluna Origem vazia no total
                );

                DataGridViewRow row = dgvEstoque.Rows[linhaTotal];
                row.DefaultCellStyle.BackColor = Color.DarkSlateGray;
                row.DefaultCellStyle.ForeColor = Color.White;
                row.DefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);

                if (row.Cells["Produto"] != null)
                    row.Cells["Produto"].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                if (row.Cells["PesoTotal"] != null)
                    row.Cells["PesoTotal"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private string FormatarData(object data)
        {
            if (data == DBNull.Value) return "";

            DateTime dt;
            if (DateTime.TryParse(data.ToString(), out dt))
                return dt.ToString("dd/MM/yyyy");

            return "";
        }

        // ===== Configurar aba de registrar saída =====
        // ===== Configurar aba de registrar saída (VERSÃO AMPLIADA) =====
        private void ConfigurarTabRegistrarSaida()
        {
            if (tabPageRegistrarSaida == null) return;

            tabPageRegistrarSaida.Controls.Clear();

            // Panel principal com scroll
            Panel panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };

            // ===== GRUPO SELEÇÃO DO PRODUTO (AMPLIADO) =====
            GroupBox grpSelecao = new GroupBox
            {
                Text = " SELEÇÃO DO PRODUTO ",
                Location = new Point(30, 30),
                Size = new Size(700, 200), // Aumentado significativamente
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(250, 250, 250)
            };

            // Layout interno do grupo
            TableLayoutPanel layoutSelecao = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(20),
                BackColor = Color.Transparent
            };
            layoutSelecao.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); // Label maior
            layoutSelecao.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layoutSelecao.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layoutSelecao.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layoutSelecao.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            // Linha 1: Combo Produto (FONTE 14)
            Label lblProduto = new Label
            {
                Text = "Produto:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14)
            };

            cmbProdutoSaida = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 14),
                Height = 40
            };
            cmbProdutoSaida.SelectedIndexChanged += CmbProdutoSaida_SelectedIndexChanged;

            // Linha 2: Selecionado (FONTE 14)
            Label lblSelecionado = new Label
            {
                Text = "Selecionado:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14)
            };

            lblProdutoSelecionado = new Label
            {
                Text = "-",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Navy
            };

            // Linha 3: Saldo (FONTE 14)
            Label lblSaldo = new Label
            {
                Text = "Saldo:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14)
            };

            lblSaldoAtual = new Label
            {
                Text = "0",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };

            layoutSelecao.Controls.Add(lblProduto, 0, 0);
            layoutSelecao.Controls.Add(cmbProdutoSaida, 1, 0);
            layoutSelecao.Controls.Add(lblSelecionado, 0, 1);
            layoutSelecao.Controls.Add(lblProdutoSelecionado, 1, 1);
            layoutSelecao.Controls.Add(lblSaldo, 0, 2);
            layoutSelecao.Controls.Add(lblSaldoAtual, 1, 2);

            grpSelecao.Controls.Add(layoutSelecao);

            // ===== GRUPO DADOS DA SAÍDA (AMPLIADO) =====
            GroupBox grpSaida = new GroupBox
            {
                Text = " DADOS DA SAÍDA ",
                Location = new Point(30, 250),
                Size = new Size(700, 180),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(250, 250, 250)
            };

            TableLayoutPanel layoutSaida = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(20),
                BackColor = Color.Transparent
            };
            layoutSaida.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            layoutSaida.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layoutSaida.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            layoutSaida.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            // Linha 1: Quantidade (FONTE 14)
            Label lblQtd = new Label
            {
                Text = "Quantidade:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14)
            };

            numQuantidadeSaida = new NumericUpDown
            {
                Dock = DockStyle.Left,
                Width = 200,
                Height = 40,
                Minimum = 1,
                Maximum = 100000,
                Value = 1,
                Font = new Font("Segoe UI", 14),
                TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            };

            // Linha 2: Destino (FONTE 14)
            Label lblDestino = new Label
            {
                Text = "Destino:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14)
            };

            txtDestinoSaida = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14),
                Height = 40
            };

            layoutSaida.Controls.Add(lblQtd, 0, 0);
            layoutSaida.Controls.Add(numQuantidadeSaida, 1, 0);
            layoutSaida.Controls.Add(lblDestino, 0, 1);
            layoutSaida.Controls.Add(txtDestinoSaida, 1, 1);

            grpSaida.Controls.Add(layoutSaida);

            // ===== BOTÃO REGISTRAR (AMPLIADO) =====
            btnRegistrarSaida = new Button
            {
                Text = "📦 REGISTRAR SAÍDA",
                Location = new Point(30, 450),
                Size = new Size(300, 60),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegistrarSaida.FlatAppearance.BorderSize = 0;
            btnRegistrarSaida.Click += BtnRegistrarSaida_Click;

            // ===== PAINEL DE INFORMAÇÕES (LATERAL DIREITA) =====
            Panel panelInfo = new Panel
            {
                Location = new Point(750, 30),
                Size = new Size(350, 480),
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblInfoTitulo = new Label
            {
                Text = "ℹ️ INFORMAÇÕES",
                Location = new Point(15, 15),
                Size = new Size(320, 40),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            ListBox lstInfo = new ListBox
            {
                Location = new Point(15, 65),
                Size = new Size(320, 400),
                Font = new Font("Segoe UI", 14),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(240, 240, 240),
                SelectionMode = SelectionMode.None
            };

            lstInfo.Items.Add("• Selecione um produto com");
            lstInfo.Items.Add("  estoque disponível");
            lstInfo.Items.Add("");
            lstInfo.Items.Add("• A quantidade não pode");
            lstInfo.Items.Add("  exceder o saldo atual");
            lstInfo.Items.Add("");
            lstInfo.Items.Add("• O sistema usa FIFO");
            lstInfo.Items.Add("  (primeiro que entra,");
            lstInfo.Items.Add("  primeiro que sai)");
            lstInfo.Items.Add("");
            lstInfo.Items.Add("• Informe o destino para");
            lstInfo.Items.Add("  melhor rastreabilidade");
            lstInfo.Items.Add("");
            

            panelInfo.Controls.AddRange(new Control[] { lblInfoTitulo, lstInfo });

            // Adicionar controles ao painel principal
            panelPrincipal.Controls.AddRange(new Control[] {
        grpSelecao,
        grpSaida,
        btnRegistrarSaida,
        panelInfo
    });

            tabPageRegistrarSaida.Controls.Add(panelPrincipal);
        }

        // ===== CARREGAR PRODUTOS SAÍDA =====
        private void CarregarProdutosSaida()
        {
            if (cmbProdutoSaida == null) return;

            cmbProdutoSaida.Items.Clear();

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"
                    SELECT l.descricao, ei.quantidade
                    FROM tbLista l
                    INNER JOIN tbEstoqueItens ei ON ei.codList = l.codList
                    WHERE ei.quantidade > 0
                    ORDER BY l.descricao";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nome = reader["descricao"].ToString();
                        int qtd = Convert.ToInt32(reader["quantidade"]);
                        cmbProdutoSaida.Items.Add($"{nome} | Estoque: {qtd}");
                    }
                }

                if (cmbProdutoSaida.Items.Count == 0)
                {
                    cmbProdutoSaida.Items.Add("Nenhum produto com estoque");
                }
            }
        }

        private void CmbProdutoSaida_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProdutoSaida.SelectedItem == null) return;

            string item = cmbProdutoSaida.SelectedItem.ToString();
            if (item == "Nenhum produto com estoque")
            {
                lblProdutoSelecionado.Text = "-";
                lblSaldoAtual.Text = "0";
                return;
            }

            string produto = item.Split('|')[0].Trim();
            lblProdutoSelecionado.Text = produto;

            using (var conn = DataBaseConnection.OpenConnection())
            {
                string sql = @"SELECT ei.quantidade
                               FROM tbEstoqueItens ei
                               INNER JOIN tbLista l ON l.codList = ei.codList
                               WHERE l.descricao = @produto";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@produto", produto);
                    object result = cmd.ExecuteScalar();
                    int saldo = result != null ? Convert.ToInt32(result) : 0;
                    lblSaldoAtual.Text = saldo.ToString();

                    numQuantidadeSaida.Maximum = saldo;
                }
            }
        }

        // ===== REGISTRAR SAÍDA =====
        private void BtnRegistrarSaida_Click(object sender, EventArgs e)
        {
            if (cmbProdutoSaida.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto.");
                return;
            }

            string item = cmbProdutoSaida.SelectedItem.ToString();
            if (item == "Nenhum produto com estoque")
            {
                MessageBox.Show("Não há produtos com estoque disponível.");
                return;
            }

            int qtd = (int)numQuantidadeSaida.Value;
            if (qtd <= 0)
            {
                MessageBox.Show("Quantidade inválida.");
                return;
            }

            string produto = item.Split('|')[0].Trim();
            string destino = txtDestinoSaida.Text.Trim();

            RegistrarSaida(produto, qtd, destino);

            txtDestinoSaida.Clear();
            numQuantidadeSaida.Value = 1;
            lblSaldoAtual.Text = "0";
            lblProdutoSelecionado.Text = "-";
            cmbProdutoSaida.SelectedIndex = -1;
        }

        // ===== MÉTODO PARA REGISTRAR SAÍDA =====
        // ===== MÉTODO PARA REGISTRAR SAÍDA (COM DESTINO) =====
        private void RegistrarSaida(string produto, int quantidade, string destino)
        {
            using (var conn = DataBaseConnection.OpenConnection())
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    // 1. Buscar código do produto
                    int codList;
                    int peso;
                    string sqlProduto = "SELECT codList, peso FROM tbLista WHERE descricao = @produto";
                    using (var cmd = new MySqlCommand(sqlProduto, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@produto", produto);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new Exception("Produto não encontrado");
                            codList = Convert.ToInt32(reader["codList"]);
                            peso = Convert.ToInt32(reader["peso"]);
                        }
                    }

                    // 2. Verificar estoque disponível
                    string sqlSaldo = @"
                SELECT COALESCE(SUM(quantidade), 0) 
                FROM tbProdutos 
                WHERE codList = @codList AND quantidade > 0";

                    int saldoAtual;
                    using (var cmd = new MySqlCommand(sqlSaldo, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@codList", codList);
                        saldoAtual = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (saldoAtual < quantidade)
                    {
                        trans.Rollback();
                        MessageBox.Show($"Estoque insuficiente!\n\nEstoque disponível: {saldoAtual}");
                        return;
                    }

                    // 3. Buscar registros mais antigos para FIFO
                    string sqlBuscar = @"
                SELECT codProd, quantidade 
                FROM tbProdutos 
                WHERE codList = @codList AND quantidade > 0 
                ORDER BY dataDeEntrada ASC";

                    List<(int codProd, int qtd)> itensEstoque = new List<(int, int)>();
                    using (var cmd = new MySqlCommand(sqlBuscar, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@codList", codList);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                itensEstoque.Add((Convert.ToInt32(reader["codProd"]), Convert.ToInt32(reader["quantidade"])));
                            }
                        }
                    }

                    // 4. Dar baixa nos itens (FIFO)
                    int restante = quantidade;
                    foreach (var item in itensEstoque)
                    {
                        if (restante <= 0) break;

                        int retirar = Math.Min(restante, item.qtd);
                        string sqlUpdate = "UPDATE tbProdutos SET quantidade = quantidade - @retirar WHERE codProd = @codProd";
                        using (var cmd = new MySqlCommand(sqlUpdate, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@retirar", retirar);
                            cmd.Parameters.AddWithValue("@codProd", item.codProd);
                            cmd.ExecuteNonQuery();
                        }

                        restante -= retirar;
                    }

                    // 5. Registrar a saída no histórico COM DESTINO
                    int codOri = 1;
                    string sqlInsert = @"
                INSERT INTO tbProdutos 
                    (descricao, quantidade, peso, unidade, dataDeEntrada, 
                     dataDeValidade, dataLimiteDeSaida, tipoMovimentacao, 
                     codUsu, codOri, codList, destino)
                VALUES 
                    (@descricao, -@qtd, @peso, 'UNIDADES (UN)', NOW(),
                     DATE_ADD(NOW(), INTERVAL 30 DAY), DATE_ADD(NOW(), INTERVAL 60 DAY),
                     'SAIDA', @codUsu, @codOri, @codList, @destino)";

                    using (var cmd = new MySqlCommand(sqlInsert, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@descricao", produto);
                        cmd.Parameters.AddWithValue("@qtd", quantidade);
                        cmd.Parameters.AddWithValue("@peso", peso);
                        cmd.Parameters.AddWithValue("@codUsu", codUsuLogado);
                        cmd.Parameters.AddWithValue("@codOri", codOri);
                        cmd.Parameters.AddWithValue("@codList", codList);
                        cmd.Parameters.AddWithValue("@destino", string.IsNullOrEmpty(destino) ? "Não informado" : destino);
                        cmd.ExecuteNonQuery();
                    }

                    // 6. Atualizar o estoque total
                    string sqlUpdateEstoque = @"
                UPDATE tbEstoqueItens 
                SET quantidade = quantidade - @qtd
                WHERE codList = @codList";

                    using (var cmd = new MySqlCommand(sqlUpdateEstoque, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@qtd", quantidade);
                        cmd.Parameters.AddWithValue("@codList", codList);
                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();

                    int novoSaldo = saldoAtual - quantidade;
                    MessageBox.Show($"✅ Saída de {quantidade} unidades registrada!\n" +
                                  $"Produto: {produto}\n" +
                                  $"Destino: {(string.IsNullOrEmpty(destino) ? "Não informado" : destino)}\n" +
                                  $"Saldo anterior: {saldoAtual}\n" +
                                  $"Novo saldo: {novoSaldo}");

                    // Atualizar telas
                    CarregarDados();
                    CarregarProdutosSaida();
                    CarregarHistoricoSaidas("");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show($"❌ Erro: {ex.Message}");
                }
            }
        }

        // ===== MÉTODO DE DIAGNÓSTICO =====
        private void DiagnosticarEstoque(string produto)
        {
            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                {
                    string sql1 = @"
                        SELECT SUM(p.quantidade) 
                        FROM tbProdutos p
                        INNER JOIN tbLista l ON l.codList = p.codList
                        WHERE l.descricao = @produto AND p.quantidade > 0";

                    using (var cmd = new MySqlCommand(sql1, conn))
                    {
                        cmd.Parameters.AddWithValue("@produto", produto);
                        object result = cmd.ExecuteScalar();
                        int somaPositivos = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        MessageBox.Show($"📦 Soma dos registros POSITIVOS em tbProdutos: {somaPositivos}", "Diagnóstico");
                    }

                    string sql2 = @"
                        SELECT ei.quantidade 
                        FROM tbEstoqueItens ei
                        INNER JOIN tbLista l ON l.codList = ei.codList
                        WHERE l.descricao = @produto";

                    using (var cmd = new MySqlCommand(sql2, conn))
                    {
                        cmd.Parameters.AddWithValue("@produto", produto);
                        object result = cmd.ExecuteScalar();
                        int estoqueItens = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        MessageBox.Show($"📊 tbEstoqueItens: {estoqueItens}", "Diagnóstico");
                    }

                    string sql3 = @"
                        SELECT SUM(p.quantidade) 
                        FROM tbProdutos p
                        INNER JOIN tbLista l ON l.codList = p.codList
                        WHERE l.descricao = @produto";

                    using (var cmd = new MySqlCommand(sql3, conn))
                    {
                        cmd.Parameters.AddWithValue("@produto", produto);
                        object result = cmd.ExecuteScalar();
                        int somaTotal = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        MessageBox.Show($"📈 Soma TOTAL (inclui saídas negativas): {somaTotal}", "Diagnóstico");
                    }

                    string sql4 = @"
                        SELECT p.codProd, p.quantidade, p.dataDeEntrada, p.tipoMovimentacao
                        FROM tbProdutos p
                        INNER JOIN tbLista l ON l.codList = p.codList
                        WHERE l.descricao = @produto
                        ORDER BY p.dataDeEntrada DESC
                        LIMIT 5";

                    StringBuilder detalhes = new StringBuilder();
                    detalhes.AppendLine("📋 Últimos 5 registros:");

                    using (var cmd = new MySqlCommand(sql4, conn))
                    {
                        cmd.Parameters.AddWithValue("@produto", produto);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int codProd = Convert.ToInt32(reader["codProd"]);
                                int qtd = Convert.ToInt32(reader["quantidade"]);
                                DateTime data = Convert.ToDateTime(reader["dataDeEntrada"]);
                                string tipo = reader["tipoMovimentacao"]?.ToString() ?? "N/A";

                                detalhes.AppendLine($"  • {data:dd/MM/yyyy HH:mm} | {tipo} | {qtd} un");
                            }
                        }
                    }

                    MessageBox.Show(detalhes.ToString(), "Detalhes");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro no diagnóstico: {ex.Message}");
            }
        }

        // ===== BOTÕES ORIGINAIS =====
        private void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            produtoSelecionado = cbxprodutoSelecionado.SelectedIndex == 0 ? "" : cbxprodutoSelecionado.Text;
            somentePrincipais = false;
            CarregarDados();
        }


        private void btnLimparFiltro_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "";
            cbxprodutoSelecionado.SelectedIndex = 0;
            somentePrincipais = false;
            CarregarDados();
        }

        private void btnPrincipaisProdutos_Click(object sender, EventArgs e)
        {
            somentePrincipais = true;
            produtoSelecionado = "";
            CarregarDados();
        }

        private void btnAlternarModo_Click(object sender, EventArgs e)
        {
            modoAgrupado = !modoAgrupado;
            btnAplicarModo.Text = modoAgrupado ? "Modo: Agrupado" : "Modo: Detalhado";
            ConfigurarDataGridView(modoAgrupado);
            CarregarDados();
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

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal menu = new frmMenuPrincipal(codUsuLogado);
            menu.Show();
            this.Close();
        }

        // ===== DIAGNÓSTICO RÁPIDO =====
        private void DiagnosticarSal()
        {
            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                {
                    // Verificar soma dos registros positivos em tbProdutos
                    string sql1 = @"
                SELECT SUM(p.quantidade) 
                FROM tbProdutos p
                INNER JOIN tbLista l ON l.codList = p.codList
                WHERE l.descricao LIKE '%SAL%' AND p.quantidade > 0";

                    using (var cmd = new MySqlCommand(sql1, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        int somaPositivos = result != null ? Convert.ToInt32(result) : 0;
                        MessageBox.Show($"Soma dos registros POSITIVOS: {somaPositivos}", "Diagnóstico");
                    }

                    // Verificar tbEstoqueItens
                    string sql2 = @"
                SELECT ei.quantidade 
                FROM tbEstoqueItens ei
                INNER JOIN tbLista l ON l.codList = ei.codList
                WHERE l.descricao LIKE '%SAL%'";

                    using (var cmd = new MySqlCommand(sql2, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        int estoqueItens = result != null ? Convert.ToInt32(result) : 0;
                        MessageBox.Show($"tbEstoqueItens: {estoqueItens}", "Diagnóstico");
                    }

                    // Listar todos os registros de SAL
                    string sql3 = @"
                SELECT p.codProd, p.quantidade, p.dataDeEntrada, p.tipoMovimentacao
                FROM tbProdutos p
                INNER JOIN tbLista l ON l.codList = p.codList
                WHERE l.descricao LIKE '%SAL%'
                ORDER BY p.dataDeEntrada DESC";

                    StringBuilder detalhes = new StringBuilder();
                    detalhes.AppendLine("Registros de SAL:");

                    using (var cmd = new MySqlCommand(sql3, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int qtd = Convert.ToInt32(reader["quantidade"]);
                            DateTime data = Convert.ToDateTime(reader["dataDeEntrada"]);
                            string tipo = reader["tipoMovimentacao"]?.ToString() ?? "N/A";
                            detalhes.AppendLine($"  {data:dd/MM/yyyy} | {tipo} | {qtd}");
                        }
                    }

                    MessageBox.Show(detalhes.ToString(), "Registros");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
           

        }

        private void btnMenu_Click_1(object sender, EventArgs e)
        {
            frmMenuPrincipal menu = new frmMenuPrincipal(codUsuLogado);
            menu.Show();
            this.Close();
        }
    }
}
