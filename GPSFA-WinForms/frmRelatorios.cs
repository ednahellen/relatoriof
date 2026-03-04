using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using Projeto_Socorrista;

namespace GPSFA_WinForms
{
    public partial class frmRelatorios : Form
    {
        // Variável para armazenar o usuário logado
        private int _codUsuLogado;

        // Construtor vazio
        public frmRelatorios()
        {
            InitializeComponent();
        }

        // Construtor com parâmetro do usuário
        public frmRelatorios(int codUsu)
        {
            InitializeComponent();
            _codUsuLogado = codUsu;
        }

        // Evento Load do formulário
        private void frmRelatorios_Load(object sender, EventArgs e)
        {
            // Configurar o grid
            ConfigurarDataGridView();

            // Configurar datas padrão (últimos 30 dias)
            dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
            dtpDataFinalPeriodo.Value = DateTime.Now;

            // Carregar combos
            CarregarUsuarios();
            CarregarProdutos();
            CarregarStatus();

            // Configurar Label de Status
            ConfigurarLabelStatus();

            // Carregar os dados no grid
            CarregarDados();
        }

        // Método para configurar o Label de Status
        private void ConfigurarLabelStatus()
        {
            if (lblStatus != null)
            {
                lblStatus.ForeColor = Color.White;
                lblStatus.BackColor = Color.Transparent;
                lblStatus.Font = new Font("Arial", 10, FontStyle.Bold);
                lblStatus.Text = "Aguardando pesquisa...";
                lblStatus.Visible = true;
            }
        }

        // Método para configurar o DataGridView
        private void ConfigurarDataGridView()
        {
            // Limpar configurações automáticas
            dgvRelatorios.AutoGenerateColumns = false;
            dgvRelatorios.Columns.Clear();

            // Configurar propriedades básicas
            dgvRelatorios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelatorios.AllowUserToAddRows = false;
            dgvRelatorios.ReadOnly = true;
            dgvRelatorios.RowHeadersVisible = false;
            dgvRelatorios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelatorios.MultiSelect = false;
            dgvRelatorios.BackgroundColor = Color.White;

            // Altura das linhas
            dgvRelatorios.RowTemplate.Height = 35;

            // Estilo do cabeçalho
            dgvRelatorios.EnableHeadersVisualStyles = false;
            dgvRelatorios.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dgvRelatorios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRelatorios.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvRelatorios.ColumnHeadersHeight = 40;

            // Estilo das células
            dgvRelatorios.DefaultCellStyle.Font = new Font("Arial", 10);
            dgvRelatorios.DefaultCellStyle.ForeColor = Color.Black;
            dgvRelatorios.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvRelatorios.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        // Método para carregar usuários no combo
        private void CarregarUsuarios()
        {
            try
            {
                if (cbbUsuario != null)
                {
                    cbbUsuario.Items.Clear();
                    cbbUsuario.Items.Add("TODOS");

                    using (MySqlConnection conexao = DataBaseConnection.OpenConnection())
                    {
                        string sql = "SELECT nome FROM tbVoluntarios ORDER BY nome";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbbUsuario.Items.Add(reader["nome"].ToString());
                            }
                        }
                    }

                    cbbUsuario.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar usuários: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DataBaseConnection.CloseConnection();
            }
        }

        // Método para carregar produtos no combo
        private void CarregarProdutos()
        {
            try
            {
                if (cbxProduto != null)
                {
                    cbxProduto.Items.Clear();
                    cbxProduto.Items.Add("TODOS");

                    using (MySqlConnection conexao = DataBaseConnection.OpenConnection())
                    {
                        string sql = "SELECT descricao FROM tbLista ORDER BY descricao";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbxProduto.Items.Add(reader["descricao"].ToString());
                            }
                        }
                    }

                    cbxProduto.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar produtos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DataBaseConnection.CloseConnection();
            }
        }

        // Método para carregar status no combo
        private void CarregarStatus()
        {
            if (cbxStatus != null)
            {
                cbxStatus.Items.Clear();
                cbxStatus.Items.Add("TODOS");
                cbxStatus.Items.Add("VENCIDO");
                cbxStatus.Items.Add("CRÍTICO (7 dias)");
                cbxStatus.Items.Add("ATENÇÃO (15 dias)");
                cbxStatus.Items.Add("ALERTA (30 dias)");
                cbxStatus.Items.Add("OK");

                cbxStatus.SelectedIndex = 0;
            }
        }

        // Método para carregar os dados do banco
        private void CarregarDados()
        {
            try
            {
                // Limpar o grid
                dgvRelatorios.Rows.Clear();

                // Verificar se o grid tem colunas configuradas
                if (dgvRelatorios.Columns.Count == 0)
                {
                    // Se não tiver colunas, criar por código
                    CriarColunasGrid();
                }

                // Atualizar status
                if (lblStatus != null)
                    lblStatus.Text = "Pesquisando...";

                // Query SQL completa com peso
                string sql = @"
                    SELECT 
                        p.dataDeEntrada,
                        l.descricao AS produto,
                        p.quantidade,
                        l.peso,
                        l.unidade,
                        c.dataDeSaida,
                        DATEDIFF(p.dataDeValidade, CURDATE()) AS diasRestantes,
                        CASE
                            WHEN p.dataDeValidade < CURDATE() THEN 'VENCIDO'
                            WHEN DATEDIFF(p.dataDeValidade, CURDATE()) <= 7 THEN 'CRÍTICO (7 dias)'
                            WHEN DATEDIFF(p.dataDeValidade, CURDATE()) <= 15 THEN 'ATENÇÃO (15 dias)'
                            WHEN DATEDIFF(p.dataDeValidade, CURDATE()) <= 30 THEN 'ALERTA (30 dias)'
                            ELSE 'OK'
                        END AS status,
                        v.nome AS usuario
                    FROM tbProdutos p
                    INNER JOIN tbLista l ON p.codList = l.codList
                    INNER JOIN tbUsuarios u ON p.codUsu = u.codUsu
                    INNER JOIN tbVoluntarios v ON u.codVol = v.codVol
                    LEFT JOIN tbItensCesta ic ON ic.codList = l.codList
                    LEFT JOIN tbCestas c ON c.codCes = ic.codCes
                    WHERE p.dataDeEntrada BETWEEN @dataInicial AND @dataFinal";

                // Adicionar filtros conforme selecionados
                if (cbxProduto != null && cbxProduto.SelectedIndex > 0 && cbxProduto.Text != "TODOS")
                {
                    sql += " AND l.descricao = @produto";
                }

                if (cbbUsuario != null && cbbUsuario.SelectedIndex > 0 && cbbUsuario.Text != "TODOS")
                {
                    sql += " AND v.nome = @usuario";
                }

                // Ordenar por data de entrada (mais recente primeiro)
                sql += " ORDER BY p.dataDeEntrada DESC;";

                int contador = 0;
                int totalQuantidade = 0;
                decimal totalPeso = 0;

                using (MySqlConnection conexao = DataBaseConnection.OpenConnection())
                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    // Parâmetros de data
                    comando.Parameters.AddWithValue("@dataInicial", dtpDataInicialPeriodo.Value.Date);
                    comando.Parameters.AddWithValue("@dataFinal", dtpDataFinalPeriodo.Value.Date);

                    // Parâmetros de filtros
                    if (cbxProduto != null && cbxProduto.SelectedIndex > 0 && cbxProduto.Text != "TODOS")
                        comando.Parameters.AddWithValue("@produto", cbxProduto.Text);

                    if (cbbUsuario != null && cbbUsuario.SelectedIndex > 0 && cbbUsuario.Text != "TODOS")
                        comando.Parameters.AddWithValue("@usuario", cbbUsuario.Text);

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dataSaida = reader["dataDeSaida"] != DBNull.Value
                                ? Convert.ToDateTime(reader["dataDeSaida"]).ToString("dd/MM/yyyy")
                                : "-";

                            string status = reader["status"].ToString();
                            int diasRestantes = Convert.ToInt32(reader["diasRestantes"]);
                            int quantidade = Convert.ToInt32(reader["quantidade"]);
                            decimal peso = Convert.ToDecimal(reader["peso"]);

                            // Formatar peso com pontuação correta
                            string pesoFormatado = FormatarPeso(peso);

                            // Calcular peso total (quantidade * peso)
                            decimal pesoTotal = quantidade * peso;

                            // Aplicar filtro de status (se selecionado)
                            if (cbxStatus != null && cbxStatus.SelectedIndex > 0 && cbxStatus.Text != "TODOS")
                            {
                                if (status != cbxStatus.Text)
                                    continue; // Pula registros que não correspondem ao status
                            }

                            // Adicionar linha ao grid
                            int rowIndex = dgvRelatorios.Rows.Add(
                                Convert.ToDateTime(reader["dataDeEntrada"]).ToString("dd/MM/yyyy"),
                                reader["produto"].ToString(),
                                quantidade.ToString(),
                                pesoFormatado,
                                reader["unidade"].ToString(),
                                dataSaida,
                                status
                            );

                            // Aplicar cor conforme status
                            AplicarCorStatus(dgvRelatorios.Rows[rowIndex], diasRestantes);

                            // Acumular totais
                            totalQuantidade += quantidade;
                            totalPeso += pesoTotal;

                            contador++;
                        }
                    }
                }

                // Adicionar linha de totais se houver registros
                if (contador > 0)
                {
                    AdicionarLinhaTotais(totalQuantidade, totalPeso);
                }

                // Atualizar label de status
                if (lblStatus != null)
                {
                    if (contador > 0)
                    {
                        lblStatus.Text = $"Encontrados {contador} registros. Total de itens: {totalQuantidade:N0} | Peso total: {totalPeso:N3} kg";
                    }
                    else
                    {
                        lblStatus.Text = "Nenhum registro encontrado com os filtros selecionados.";
                    }
                }

                // Habilitar botão de exportar
                if (btnExportarExcel != null)
                    btnExportarExcel.Enabled = contador > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (lblStatus != null)
                    lblStatus.Text = "Erro ao carregar dados.";
            }
            finally
            {
                DataBaseConnection.CloseConnection();
            }
        }

        // Método para formatar peso com pontuação correta
        private string FormatarPeso(decimal peso)
        {
            // Usar o formato N3 que adiciona separadores de milhar e 3 casas decimais
            return peso.ToString("N3");
        }

        // Método para adicionar linha de totais
        private void AdicionarLinhaTotais(int totalQuantidade, decimal totalPeso)
        {
            int rowIndex = dgvRelatorios.Rows.Add(
                "",
                ">>> TOTAL GERAL <<<",
                totalQuantidade.ToString("N0"), // Formato com separadores de milhar
                totalPeso.ToString("N3"),       // Formato com separadores de milhar e 3 casas decimais
                "kg",
                "",
                ""
            );

            DataGridViewRow totalRow = dgvRelatorios.Rows[rowIndex];
            totalRow.DefaultCellStyle.BackColor = Color.DarkSlateGray;
            totalRow.DefaultCellStyle.ForeColor = Color.White;
            totalRow.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            // Alinhar corretamente
            totalRow.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            totalRow.Cells[3].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            totalRow.Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Mesclar células visualmente
            totalRow.Cells[0].Style.ForeColor = Color.DarkSlateGray; // "Invisível"
        }

        // Método para criar colunas do grid (caso não tenha no designer)
        private void CriarColunasGrid()
        {
            // Data de Entrada
            DataGridViewTextBoxColumn colDataEntrada = new DataGridViewTextBoxColumn();
            colDataEntrada.Name = "dataEntrada";
            colDataEntrada.HeaderText = "Data de Entrada";
            colDataEntrada.Width = 100;
            colDataEntrada.DefaultCellStyle.Format = "dd/MM/yyyy";
            colDataEntrada.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Produto
            DataGridViewTextBoxColumn colProduto = new DataGridViewTextBoxColumn();
            colProduto.Name = "produto";
            colProduto.HeaderText = "Nome do Produto";
            colProduto.Width = 250;

            // Quantidade
            DataGridViewTextBoxColumn colQuantidade = new DataGridViewTextBoxColumn();
            colQuantidade.Name = "quantidade";
            colQuantidade.HeaderText = "Qtd";
            colQuantidade.Width = 70;
            colQuantidade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colQuantidade.DefaultCellStyle.Format = "N0"; // Formato número com separadores

            // Peso
            DataGridViewTextBoxColumn colPeso = new DataGridViewTextBoxColumn();
            colPeso.Name = "peso";
            colPeso.HeaderText = "Peso (kg)";
            colPeso.Width = 100;
            colPeso.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colPeso.DefaultCellStyle.Format = "N3"; // Formato número com 3 casas decimais e separadores

            // Unidade
            DataGridViewTextBoxColumn colUnidade = new DataGridViewTextBoxColumn();
            colUnidade.Name = "unidade";
            colUnidade.HeaderText = "Unidade";
            colUnidade.Width = 80;
            colUnidade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Data de Saída
            DataGridViewTextBoxColumn colDataSaida = new DataGridViewTextBoxColumn();
            colDataSaida.Name = "dataSaida";
            colDataSaida.HeaderText = "Data Saída";
            colDataSaida.Width = 100;
            colDataSaida.DefaultCellStyle.Format = "dd/MM/yyyy";
            colDataSaida.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Status
            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "status";
            colStatus.HeaderText = "Status";
            colStatus.Width = 120;
            colStatus.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colStatus.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            // Adicionar colunas ao grid
            dgvRelatorios.Columns.Add(colDataEntrada);
            dgvRelatorios.Columns.Add(colProduto);
            dgvRelatorios.Columns.Add(colQuantidade);
            dgvRelatorios.Columns.Add(colPeso);
            dgvRelatorios.Columns.Add(colUnidade);
            dgvRelatorios.Columns.Add(colDataSaida);
            dgvRelatorios.Columns.Add(colStatus);
        }

        // Método para aplicar cor conforme status
        private void AplicarCorStatus(DataGridViewRow row, int diasRestantes)
        {
            if (diasRestantes < 0) // VENCIDO
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 199, 206); // Vermelho claro
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
                row.Cells["status"].Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }
            else if (diasRestantes <= 7) // CRÍTICO (7 dias)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 153); // Laranja claro
                row.DefaultCellStyle.ForeColor = Color.Red;
                row.Cells["status"].Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }
            else if (diasRestantes <= 15) // ATENÇÃO (15 dias)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 242, 204); // Amarelo claro
                row.DefaultCellStyle.ForeColor = Color.DarkOrange;
                row.Cells["status"].Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }
            else if (diasRestantes <= 30) // ALERTA (30 dias)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218); // Verde claro
                row.DefaultCellStyle.ForeColor = Color.Goldenrod;
                row.Cells["status"].Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }
            else // OK
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.Green;
            }
        }

        // Evento do botão Pesquisar
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            // Validar as datas
            if (dtpDataInicialPeriodo.Value.Date > dtpDataFinalPeriodo.Value.Date)
            {
                MessageBox.Show("A data inicial não pode ser maior que a data final.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Carregar os dados com os filtros
            CarregarDados();
        }

        // Evento do botão Limpar Filtros
        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            // Resetar todos os filtros
            dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
            dtpDataFinalPeriodo.Value = DateTime.Now;

            if (cbxProduto != null)
                cbxProduto.SelectedIndex = 0;

            if (cbbUsuario != null)
                cbbUsuario.SelectedIndex = 0;

            if (cbxStatus != null)
                cbxStatus.SelectedIndex = 0;

            // Recarregar os dados
            CarregarDados();
        }

        // Evento do botão Exportar Excel
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvRelatorios.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo Excel (*.xlsx)|*.xlsx";
                sfd.FileName = $"Relatorio_Estoque_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                sfd.Title = "Salvar Relatório em Excel";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Relatório de Estoque");

                            // Título
                            worksheet.Cell(1, 1).Value = "RELATÓRIO DE ESTOQUE";
                            worksheet.Cell(1, 1).Style.Font.Bold = true;
                            worksheet.Cell(1, 1).Style.Font.FontSize = 14;

                            // Período e filtros
                            string produto = cbxProduto != null ? cbxProduto.Text : "TODOS";
                            string usuario = cbbUsuario != null ? cbbUsuario.Text : "TODOS";
                            string status = cbxStatus != null ? cbxStatus.Text : "TODOS";

                            worksheet.Cell(2, 1).Value = $"Período: {dtpDataInicialPeriodo.Value:dd/MM/yyyy} a {dtpDataFinalPeriodo.Value:dd/MM/yyyy}";
                            worksheet.Cell(2, 1).Style.Font.Italic = true;

                            worksheet.Cell(3, 1).Value = $"Produto: {produto} | Usuário: {usuario} | Status: {status}";
                            worksheet.Cell(3, 1).Style.Font.Italic = true;

                            // Cabeçalhos
                            for (int i = 0; i < dgvRelatorios.Columns.Count; i++)
                            {
                                worksheet.Cell(5, i + 1).Value = dgvRelatorios.Columns[i].HeaderText;
                                worksheet.Cell(5, i + 1).Style.Font.Bold = true;
                                worksheet.Cell(5, i + 1).Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
                                worksheet.Cell(5, i + 1).Style.Font.FontColor = XLColor.White;
                            }

                            // Dados
                            for (int i = 0; i < dgvRelatorios.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvRelatorios.Columns.Count; j++)
                                {
                                    worksheet.Cell(i + 6, j + 1).Value = dgvRelatorios.Rows[i].Cells[j].Value?.ToString() ?? "";
                                }
                            }

                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Relatório exportado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Evento do botão Menu
        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal menu = new frmMenuPrincipal(_codUsuLogado);
            menu.Show();
            this.Close();
        }

        // Evento do botão Cadastrar
        private void button1_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos cadastro = new frmGerenciarProdutos(_codUsuLogado);
            cadastro.Show();
            this.Close();
        }
    }
}