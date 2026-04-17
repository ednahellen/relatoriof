using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
 

namespace GPSFA_WinForms
{
    public partial class frmCestas : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmCestas()
        {
            InitializeComponent();
        }

        public frmCestas(int codUsu)
        {
            codUsuLogado = codUsu;
            InitializeComponent();
            ConfigurarDataGridViewCestas();
            carregarModelosDeCestaNaComboBox();

            // ✅ Adicionar botão de exportar
            AdicionarBotaoExportar();
        }

        int codUsuLogado, codModeloCestaSelecionado;

        // MÉTODOS DE QUERIES NO BANCO DE DADOS
        private void carregarModelosDeCestaNaComboBox()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT * FROM tbModeloCesta ORDER BY descricao ASC;";
            comm.CommandType = CommandType.Text;
            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbModeloDeCesta.Items.Add(DR.GetString(1));
            }

            DataBaseConnection.CloseConnection();
        }

        private void buscarCodModeloPorDescricao(string cestaModeloNome)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT codModelo FROM tbModeloCesta WHERE descricao = @descricao;";
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = cestaModeloNome;
                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    if (DR.Read())
                    {
                        if (DR.GetInt32("codModelo") > 0)
                        {
                            codModeloCestaSelecionado = DR.GetInt32("codModelo");
                            DataBaseConnection.CloseConnection();
                        }
                        else
                        {
                            MessageBox.Show("Codigo não encontrado");
                            DataBaseConnection.CloseConnection();
                        }
                    }
                }
            }
        }

        // CONFIGURAÇÃO DO DATAGRIDVIEW
        private void ConfigurarDataGridViewCestas()
        {
            dgvItensDaCesta.Columns.Clear();

            // Ajustes gerais
            dgvItensDaCesta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItensDaCesta.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgvItensDaCesta.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvItensDaCesta.RowsDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Regular);
            dgvItensDaCesta.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            dgvItensDaCesta.RowTemplate.Height = 35;
            dgvItensDaCesta.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvItensDaCesta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItensDaCesta.AllowUserToAddRows = false;
            dgvItensDaCesta.MultiSelect = false;
            dgvItensDaCesta.EditMode = DataGridViewEditMode.EditOnEnter;

            // Colunas
            dgvItensDaCesta.Columns.Add("codList", "Código");
            dgvItensDaCesta.Columns["codList"].Visible = false;

            dgvItensDaCesta.Columns.Add("Produto", "Produto");
            dgvItensDaCesta.Columns.Add("QtdePorCesta", "Qtde por cesta");
            dgvItensDaCesta.Columns.Add("EstoqueAtual", "Estoque atual");
            dgvItensDaCesta.Columns.Add("TotalNecessario", "Total necessário");
            dgvItensDaCesta.Columns.Add("Status", "Status");
            dgvItensDaCesta.Columns.Add("QuantoFalta", "Quanto falta");
            dgvItensDaCesta.Columns.Add("Sobra", "Sobra");

            // Larguras
            dgvItensDaCesta.Columns["Produto"].Width = 200;
            dgvItensDaCesta.Columns["QtdePorCesta"].Width = 100;
            dgvItensDaCesta.Columns["EstoqueAtual"].Width = 100;
            dgvItensDaCesta.Columns["TotalNecessario"].Width = 110;
            dgvItensDaCesta.Columns["Status"].Width = 90;
            dgvItensDaCesta.Columns["QuantoFalta"].Width = 90;
            dgvItensDaCesta.Columns["Sobra"].Width = 80;

            // Alinhamento
            dgvItensDaCesta.Columns["QtdePorCesta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItensDaCesta.Columns["EstoqueAtual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItensDaCesta.Columns["TotalNecessario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItensDaCesta.Columns["QuantoFalta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItensDaCesta.Columns["Sobra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItensDaCesta.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Estilo cabeçalho
            dgvItensDaCesta.EnableHeadersVisualStyles = false;
            dgvItensDaCesta.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvItensDaCesta.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvItensDaCesta.ColumnHeadersHeight = 40;

            // Botão Remover
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "";
            buttonColumn.Name = "RemoverProduto";
            buttonColumn.Text = "Remover";
            buttonColumn.UseColumnTextForButtonValue = true;
            buttonColumn.Width = 60;
            dgvItensDaCesta.Columns.Add(buttonColumn);
        }

        // Carregar dados no datagrid view
        private void carregarDadosNoDgvItensDaCesta(int codModelo)
        {
            dgvItensDaCesta.Rows.Clear();

            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = @"SELECT l.codList, l.descricao, imc.quantidadeMinima, 
                                            COALESCE(SUM(CASE WHEN p.quantidade > 0 THEN p.quantidade ELSE 0 END), 0) -
                                            COALESCE(SUM(CASE WHEN p.quantidade < 0 THEN ABS(p.quantidade) ELSE 0 END), 0) AS estoqueAtual 
                                     FROM tbItensDoModeloCesta imc 
                                     INNER JOIN tbLista l ON l.codList = imc.codList 
                                     LEFT JOIN tbProdutos p ON p.codList = l.codList 
                                     WHERE imc.codModelo = @codModelo 
                                     GROUP BY imc.codModelo, imc.codList, l.descricao, l.unidade, imc.quantidadeMinima;";
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@codModelo", MySqlDbType.Int32).Value = codModelo;
                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        dgvItensDaCesta.Rows.Add(
                            DR["codList"].ToString(),
                            DR["descricao"].ToString(),
                            DR["quantidadeMinima"].ToString(),
                            DR["estoqueAtual"].ToString(),
                            "", // Total Necessário
                            "", // Status
                            "", // Quanto Falta
                            ""  // Sobra
                        );
                    }
                    DataBaseConnection.CloseConnection();
                }
            }

            if (!string.IsNullOrEmpty(txtQtdCestas.Text) && int.TryParse(txtQtdCestas.Text, out int qtd))
            {
                calcularTotalNecessario();
            }
        }

        // ✅ Distribui cestas sem validar estoque
        private void montarCestas(int quantidadeDeCestas, int codUsu)
        {
            try
            {
                MySqlConnection conn = DataBaseConnection.OpenConnection();
                StringBuilder logDistribuicao = new StringBuilder();
                int totalCestasMontadas = 0;
                int totalItensRegistrados = 0;

                // Buscar dados dos produtos da grade
                Dictionary<int, string> nomesProdutos = new Dictionary<int, string>();
                Dictionary<int, int> quantidadePorCestaOriginal = new Dictionary<int, int>();

                foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                {
                    if (row.IsNewRow) continue;

                    int codList = Convert.ToInt32(row.Cells["codList"].Value);
                    int qtdPorCesta = Convert.ToInt32(row.Cells["QtdePorCesta"].Value);
                    string produto = row.Cells["Produto"].Value.ToString();

                    nomesProdutos[codList] = produto;
                    quantidadePorCestaOriginal[codList] = qtdPorCesta;
                }

                // Para cada cesta solicitada
                for (int i = 0; i < quantidadeDeCestas; i++)
                {
                    // Criar a cesta
                    string sqlCesta = "INSERT INTO tbCestas(codUsu) VALUES(@codUsu); SELECT LAST_INSERT_ID();";
                    var cmdCesta = new MySqlCommand(sqlCesta, conn);
                    cmdCesta.Parameters.Add("@codUsu", MySqlDbType.Int32).Value = codUsu;
                    int codCesta = Convert.ToInt32(cmdCesta.ExecuteScalar());

                    // Para cada item da cesta
                    foreach (var item in quantidadePorCestaOriginal)
                    {
                        int codList = item.Key;
                        int quantidadePorCesta = item.Value;
                        string produto = nomesProdutos[codList];

                        try
                        {
                            string sqlItem = "INSERT INTO tbItensCesta(codCes, codList, quantidade) VALUES(@codCes, @codList, @quantidade)";
                            var cmdItem = new MySqlCommand(sqlItem, conn);
                            cmdItem.Parameters.Add("@codCes", MySqlDbType.Int32).Value = codCesta;
                            cmdItem.Parameters.Add("@codList", MySqlDbType.Int32).Value = codList;
                            cmdItem.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = quantidadePorCesta;
                            cmdItem.ExecuteNonQuery();

                            totalItensRegistrados += quantidadePorCesta;
                            logDistribuicao.AppendLine($"✅ Cesta {i + 1}: {produto} - {quantidadePorCesta} unidades");
                        }
                        catch (Exception ex)
                        {
                            logDistribuicao.AppendLine($"❌ Cesta {i + 1}: {produto} - ERRO: {ex.Message}");
                        }
                    }

                    totalCestasMontadas++;
                }

                DataBaseConnection.CloseConnection();

                string mensagem = $"✅ {totalCestasMontadas} de {quantidadeDeCestas} cesta(s) montada(s)!\n";
                mensagem += $"📦 Total de itens registrados: {totalItensRegistrados}\n\n";
                mensagem += $"📋 DETALHES:\n{logDistribuicao}";

                MessageBox.Show(mensagem, "Distribuição de Cestas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Erro ao montar cestas!\n\n{error.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataBaseConnection.CloseConnection();
            }
        }


        // Relatorio de Montagem para Excel (com checklist por cesta - todas as cestas)
        //private void ExportarMontagemExcel(string caminho, int qtdCestas)
        //{
        //    using (var workbook = new ClosedXML.Excel.XLWorkbook())
        //    {
        //        // Planilha 1: Lista de Itens para Separar
        //        var worksheetItens = workbook.Worksheets.Add("Itens para Separar");

        //        worksheetItens.Cell(1, 1).Value = "LISTA PARA MONTAGEM DE CESTAS - ITENS TOTAIS";
        //        worksheetItens.Cell(1, 1).Style.Font.Bold = true;
        //        worksheetItens.Cell(1, 1).Style.Font.FontSize = 16;
        //        worksheetItens.Range(1, 1, 1, 4).Merge();

        //        worksheetItens.Cell(2, 1).Value = $"Data de emissao: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        //        worksheetItens.Cell(3, 1).Value = $"Total de Cestas: {qtdCestas}";

        //        // Cabecalho
        //        worksheetItens.Cell(5, 1).Value = "Produto";
        //        worksheetItens.Cell(5, 2).Value = "Quantidade Total";
        //        worksheetItens.Cell(5, 3).Value = "Unidade";
        //        worksheetItens.Cell(5, 4).Value = "Check";

        //        var headerRange = worksheetItens.Range(5, 1, 5, 4);
        //        headerRange.Style.Font.Bold = true;
        //        headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(52, 73, 94);
        //        headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

        //        int rowIndex = 6;
        //        foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
        //        {
        //            if (row.IsNewRow) continue;

        //            string produto = row.Cells["Produto"].Value?.ToString() ?? "";
        //            string totalNecessario = row.Cells["TotalNecessario"].Value?.ToString() ?? "0";

        //            worksheetItens.Cell(rowIndex, 1).Value = produto;
        //            worksheetItens.Cell(rowIndex, 2).Value = Convert.ToInt32(totalNecessario);
        //            worksheetItens.Cell(rowIndex, 3).Value = "UN";
        //            worksheetItens.Cell(rowIndex, 4).Value = "[ ]";

        //            rowIndex++;
        //        }

        //        worksheetItens.Columns().AdjustToContents();

        //        // Planilha 2: Checklist por Cesta (TODAS as cestas)
        //        var worksheetChecklist = workbook.Worksheets.Add("Checklist por Cesta");

        //        worksheetChecklist.Cell(1, 1).Value = "CHECKLIST POR CESTA";
        //        worksheetChecklist.Cell(1, 1).Style.Font.Bold = true;
        //        worksheetChecklist.Cell(1, 1).Style.Font.FontSize = 16;
        //        worksheetChecklist.Range(1, 1, 1, 4).Merge();

        //        worksheetChecklist.Cell(2, 1).Value = $"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        //        worksheetChecklist.Cell(3, 1).Value = $"Total de Cestas: {qtdCestas}";

        //        int linhaAtual = 5;

        //        // Gerar todas as cestas (sem limite)
        //        for (int cesta = 1; cesta <= qtdCestas; cesta++)
        //        {
        //            // Titulo da cesta
        //            worksheetChecklist.Cell(linhaAtual, 1).Value = $"CESTA {cesta}";
        //            worksheetChecklist.Cell(linhaAtual, 1).Style.Font.Bold = true;
        //            worksheetChecklist.Cell(linhaAtual, 1).Style.Font.FontSize = 14;
        //            worksheetChecklist.Cell(linhaAtual, 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(52, 152, 219);
        //            worksheetChecklist.Cell(linhaAtual, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
        //            worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4).Merge();

        //            linhaAtual++;

        //            // Cabecalho da tabela da cesta
        //            worksheetChecklist.Cell(linhaAtual, 1).Value = "Produto";
        //            worksheetChecklist.Cell(linhaAtual, 2).Value = "Quantidade";
        //            worksheetChecklist.Cell(linhaAtual, 3).Value = "Conferido";
        //            worksheetChecklist.Cell(linhaAtual, 4).Value = "Observacao";

        //            var subHeader = worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4);
        //            subHeader.Style.Font.Bold = true;
        //            subHeader.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

        //            linhaAtual++;

        //            // Itens da cesta
        //            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
        //            {
        //                if (row.IsNewRow) continue;

        //                string produto = row.Cells["Produto"].Value?.ToString() ?? "";
        //                string qtdePorCesta = row.Cells["QtdePorCesta"].Value?.ToString() ?? "0";
        //                string status = row.Cells["Status"].Value?.ToString() ?? "";
        //                string observacao = status == "Insuficiente" ? "Estoque insuficiente" : "";

        //                worksheetChecklist.Cell(linhaAtual, 1).Value = produto;
        //                worksheetChecklist.Cell(linhaAtual, 2).Value = Convert.ToInt32(qtdePorCesta);
        //                worksheetChecklist.Cell(linhaAtual, 3).Value = "[ ]";
        //                worksheetChecklist.Cell(linhaAtual, 4).Value = observacao;

        //                linhaAtual++;
        //            }

        //            // Linha de total da cesta
        //            worksheetChecklist.Cell(linhaAtual, 1).Value = "TOTAL ITENS";
        //            worksheetChecklist.Cell(linhaAtual, 1).Style.Font.Bold = true;
        //            worksheetChecklist.Cell(linhaAtual, 2).Value = "--";
        //            worksheetChecklist.Cell(linhaAtual, 3).Value = "[ ]";
        //            worksheetChecklist.Cell(linhaAtual, 4).Value = "Conferir se todos os itens estao na cesta";
        //            worksheetChecklist.Row(linhaAtual).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

        //            linhaAtual += 2;
        //        }

        //        worksheetChecklist.Columns().AdjustToContents();

        //        workbook.SaveAs(caminho);
        //    }
        //}

        // Relatorio de Montagem para Excel (com checklist por cesta - todas as cestas)
        private void ExportarMontagemExcel(string caminho, int qtdCestas)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                // Planilha 1: Lista de Itens para Separar
                var worksheetItens = workbook.Worksheets.Add("Itens para Separar");

                worksheetItens.Cell(1, 1).Value = "LISTA PARA MONTAGEM DE CESTAS - ITENS TOTAIS";
                worksheetItens.Cell(1, 1).Style.Font.Bold = true;
                worksheetItens.Cell(1, 1).Style.Font.FontSize = 16;
                worksheetItens.Range(1, 1, 1, 4).Merge();

                worksheetItens.Cell(2, 1).Value = $"Data de emissao: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheetItens.Cell(3, 1).Value = $"Total de Cestas: {qtdCestas}";

                // Cabecalho
                worksheetItens.Cell(5, 1).Value = "Produto";
                worksheetItens.Cell(5, 2).Value = "Quantidade Total";
                worksheetItens.Cell(5, 3).Value = "Unidade";
                worksheetItens.Cell(5, 4).Value = "Check";

                var headerRange = worksheetItens.Range(5, 1, 5, 4);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(52, 73, 94);
                headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                int rowIndex = 6;
                foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                {
                    if (row.IsNewRow) continue;

                    string produto = row.Cells["Produto"].Value?.ToString() ?? "";
                    string totalNecessario = row.Cells["TotalNecessario"].Value?.ToString() ?? "0";

                    worksheetItens.Cell(rowIndex, 1).Value = produto;
                    worksheetItens.Cell(rowIndex, 2).Value = Convert.ToInt32(totalNecessario);
                    worksheetItens.Cell(rowIndex, 3).Value = "UN";
                    worksheetItens.Cell(rowIndex, 4).Value = "[ ]";

                    rowIndex++;
                }

                worksheetItens.Columns().AdjustToContents();

                // Planilha 2: Checklist por Cesta (TODAS as cestas)
                var worksheetChecklist = workbook.Worksheets.Add("Checklist por Cesta");

                worksheetChecklist.Cell(1, 1).Value = "CHECKLIST POR CESTA";
                worksheetChecklist.Cell(1, 1).Style.Font.Bold = true;
                worksheetChecklist.Cell(1, 1).Style.Font.FontSize = 16;
                worksheetChecklist.Range(1, 1, 1, 4).Merge();

                worksheetChecklist.Cell(2, 1).Value = $"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheetChecklist.Cell(3, 1).Value = $"Total de Cestas: {qtdCestas}";

                int linhaAtual = 5;

                // Gerar todas as cestas (sem limite)
                for (int cesta = 1; cesta <= qtdCestas; cesta++)
                {
                    // Titulo da cesta
                    worksheetChecklist.Cell(linhaAtual, 1).Value = $"CESTA {cesta}";
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.Bold = true;
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.FontSize = 14;
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(52, 152, 219);
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                    worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4).Merge();

                    linhaAtual++;

                    // Cabecalho da tabela da cesta
                    worksheetChecklist.Cell(linhaAtual, 1).Value = "Produto";
                    worksheetChecklist.Cell(linhaAtual, 2).Value = "Quantidade";
                    worksheetChecklist.Cell(linhaAtual, 3).Value = "Conferido";
                    worksheetChecklist.Cell(linhaAtual, 4).Value = "Observacao";

                    var subHeader = worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4);
                    subHeader.Style.Font.Bold = true;
                    subHeader.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

                    linhaAtual++;

                    // Itens da cesta
                    foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string produto = row.Cells["Produto"].Value?.ToString() ?? "";
                        string qtdePorCesta = row.Cells["QtdePorCesta"].Value?.ToString() ?? "0";
                        string status = row.Cells["Status"].Value?.ToString() ?? "";
                        string observacao = status == "Insuficiente" ? "Estoque insuficiente" : "";

                        worksheetChecklist.Cell(linhaAtual, 1).Value = produto;
                        worksheetChecklist.Cell(linhaAtual, 2).Value = Convert.ToInt32(qtdePorCesta);
                        worksheetChecklist.Cell(linhaAtual, 3).Value = "[ ]";
                        worksheetChecklist.Cell(linhaAtual, 4).Value = observacao;

                        linhaAtual++;
                    }

                    // Linha de total da cesta - ajustada para coluna D apenas
                    worksheetChecklist.Cell(linhaAtual, 1).Value = "TOTAL ITENS";
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.Bold = true;
                    worksheetChecklist.Cell(linhaAtual, 2).Value = "--";
                    worksheetChecklist.Cell(linhaAtual, 3).Value = "[ ]";
                    worksheetChecklist.Cell(linhaAtual, 4).Value = "Conferir se todos os itens estao na cesta";
                    // Aplicar fundo cinza apenas nas colunas de A a D
                    worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

                    linhaAtual += 2;
                }

                worksheetChecklist.Columns().AdjustToContents();

                workbook.SaveAs(caminho);
            }
        }


        // ✅ Exportar resultado apenas para Excel
        private void ExportarResultadoCestas()
        {
            if (dgvItensDaCesta.Rows.Count <= 1)
            {
                MessageBox.Show("Não há dados para exportar. Adicione itens à cesta primeiro.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qtdCestas = 0;
            if (!string.IsNullOrEmpty(txtQtdCestas.Text))
            {
                int.TryParse(txtQtdCestas.Text, out qtdCestas);
            }

            if (qtdCestas <= 0)
            {
                MessageBox.Show("Informe a quantidade de cestas antes de exportar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Menu de opções de exportação
            using (var frmOpcoes = new Form())
            {
                frmOpcoes.Text = "Opções de Exportação";
                frmOpcoes.Size = new Size(400, 250);
                frmOpcoes.StartPosition = FormStartPosition.CenterParent;
                frmOpcoes.FormBorderStyle = FormBorderStyle.FixedDialog;
                frmOpcoes.MaximizeBox = false;
                frmOpcoes.MinimizeBox = false;

                Label lblTitulo = new Label()
                {
                    Text = "Selecione o tipo de relatório:",
                    Location = new Point(20, 20),
                    Size = new Size(350, 25),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };

                RadioButton rbPlanejamento = new RadioButton()
                {
                    Text = "Relatorio de Planejamento (detalhado)",
                    Location = new Point(30, 60),
                    Size = new Size(320, 30),
                    Font = new Font("Segoe UI", 10),
                    Checked = true
                };

                RadioButton rbMontagem = new RadioButton()
                {
                    Text = "Relatorio de Montagem (checklist por cesta)",
                    Location = new Point(30, 100),
                    Size = new Size(320, 30),
                    Font = new Font("Segoe UI", 10)
                };

                RadioButton rbAmbos = new RadioButton()
                {
                    Text = "Ambos os relatorios",
                    Location = new Point(30, 140),
                    Size = new Size(320, 30),
                    Font = new Font("Segoe UI", 10)
                };

                Button btnConfirmar = new Button()
                {
                    Text = "Exportar",
                    Location = new Point(140, 190),
                    Size = new Size(100, 35),
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                frmOpcoes.Controls.AddRange(new Control[] { lblTitulo, rbPlanejamento, rbMontagem, rbAmbos, btnConfirmar });

                bool exportar = false;
                string tipoExportacao = "";

                btnConfirmar.Click += (s, e) =>
                {
                    if (rbPlanejamento.Checked) tipoExportacao = "planejamento";
                    else if (rbMontagem.Checked) tipoExportacao = "montagem";
                    else tipoExportacao = "ambos";
                    exportar = true;
                    frmOpcoes.Close();
                };

                frmOpcoes.ShowDialog();

                if (!exportar) return;

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Arquivos Excel (*.xlsx)|*.xlsx|Arquivos Excel 97-2003 (*.xls)|*.xls",
                    Title = "Exportar Resultado",
                    FileName = tipoExportacao == "ambos" ? $"Cestas_{DateTime.Now:yyyyMMdd_HHmmss}" : $"Cestas_{tipoExportacao}_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    string extensao = System.IO.Path.GetExtension(sfd.FileName).ToLower();
                    string caminhoBase = System.IO.Path.GetDirectoryName(sfd.FileName);
                    string nomeBase = System.IO.Path.GetFileNameWithoutExtension(sfd.FileName);

                    if (tipoExportacao == "planejamento")
                    {
                        ExportarPlanejamentoExcel(sfd.FileName, qtdCestas);
                        MessageBox.Show($"Relatorio de Planejamento exportado!\n\nLocal: {sfd.FileName}",
                            "Exportacao Concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (tipoExportacao == "montagem")
                    {
                        ExportarMontagemExcel(sfd.FileName, qtdCestas);
                        MessageBox.Show($"Relatorio de Montagem exportado!\n\nLocal: {sfd.FileName}",
                            "Exportacao Concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else // ambos
                    {
                        string arquivoPlanejamento = System.IO.Path.Combine(caminhoBase, $"{nomeBase}_planejamento{extensao}");
                        string arquivoMontagem = System.IO.Path.Combine(caminhoBase, $"{nomeBase}_montagem{extensao}");

                        ExportarPlanejamentoExcel(arquivoPlanejamento, qtdCestas);
                        ExportarMontagemExcel(arquivoMontagem, qtdCestas);

                        MessageBox.Show($"Ambos os relatorios exportados!\n\nPlanejamento: {arquivoPlanejamento}\nMontagem: {arquivoMontagem}",
                            "Exportacao Concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        

        // Relatorio de Planejamento para Excel
        private void ExportarPlanejamentoExcel(string caminho, int qtdCestas)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                // Planilha de Planejamento
                var worksheet = workbook.Worksheets.Add("Planejamento");

                // Cabecalho
                worksheet.Cell(1, 1).Value = "RELATORIO DE PLANEJAMENTO DE CESTAS";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, 7).Merge();

                worksheet.Cell(2, 1).Value = $"Data de emissao: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cell(3, 1).Value = $"Quantidade de cestas: {qtdCestas}";

                // Calcular resumo
                int totalOk = 0;
                int totalInsuficiente = 0;
                int totalFaltando = 0;
                int totalSobra = 0;

                foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                {
                    if (row.IsNewRow) continue;
                    string status = row.Cells["Status"].Value?.ToString() ?? "";
                    if (status == "Ok") totalOk++;
                    else if (status == "Insuficiente") totalInsuficiente++;

                    int falta = 0;
                    int.TryParse(row.Cells["QuantoFalta"].Value?.ToString(), out falta);
                    totalFaltando += falta;

                    int sobra = 0;
                    int.TryParse(row.Cells["Sobra"].Value?.ToString(), out sobra);
                    totalSobra += sobra;
                }

                // Resumo
                worksheet.Cell(5, 1).Value = "RESUMO GERAL";
                worksheet.Cell(5, 1).Style.Font.Bold = true;
                worksheet.Cell(6, 1).Value = "Itens com estoque OK:";
                worksheet.Cell(6, 2).Value = totalOk;
                worksheet.Cell(7, 1).Value = "Itens com estoque insuficiente:";
                worksheet.Cell(7, 2).Value = totalInsuficiente;
                worksheet.Cell(8, 1).Value = "Total de unidades faltando:";
                worksheet.Cell(8, 2).Value = totalFaltando;
                worksheet.Cell(9, 1).Value = "Total de unidades que sobram:";
                worksheet.Cell(9, 2).Value = totalSobra;

                // Tabela detalhada
                worksheet.Cell(11, 1).Value = "DETALHAMENTO DOS ITENS";
                worksheet.Cell(11, 1).Style.Font.Bold = true;

                // Cabecalho da tabela
                worksheet.Cell(12, 1).Value = "Produto";
                worksheet.Cell(12, 2).Value = "Qtde por Cesta";
                worksheet.Cell(12, 3).Value = "Estoque Atual";
                worksheet.Cell(12, 4).Value = "Total Necessario";
                worksheet.Cell(12, 5).Value = "Status";
                worksheet.Cell(12, 6).Value = "Quanto Falta";
                worksheet.Cell(12, 7).Value = "Sobra";

                var headerRange = worksheet.Range(12, 1, 12, 7);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(52, 73, 94);
                headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                int rowIndex = 13;
                foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                {
                    if (row.IsNewRow) continue;

                    string produto = row.Cells["Produto"].Value?.ToString() ?? "";
                    string qtdePorCesta = row.Cells["QtdePorCesta"].Value?.ToString() ?? "0";
                    string estoqueAtual = row.Cells["EstoqueAtual"].Value?.ToString() ?? "0";
                    string totalNecessario = row.Cells["TotalNecessario"].Value?.ToString() ?? "0";
                    string status = row.Cells["Status"].Value?.ToString() ?? "";
                    string quantoFalta = row.Cells["QuantoFalta"].Value?.ToString() ?? "0";
                    string sobra = row.Cells["Sobra"].Value?.ToString() ?? "0";

                    worksheet.Cell(rowIndex, 1).Value = produto;
                    worksheet.Cell(rowIndex, 2).Value = Convert.ToInt32(qtdePorCesta);
                    worksheet.Cell(rowIndex, 3).Value = Convert.ToInt32(estoqueAtual);
                    worksheet.Cell(rowIndex, 4).Value = Convert.ToInt32(totalNecessario);
                    worksheet.Cell(rowIndex, 5).Value = status;
                    worksheet.Cell(rowIndex, 6).Value = status == "Insuficiente" ? Convert.ToInt32(quantoFalta) : 0;
                    worksheet.Cell(rowIndex, 7).Value = status == "Ok" ? Convert.ToInt32(sobra) : 0;

                    rowIndex++;
                }

                // Ajustar largura das colunas
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(caminho);
            }
        }

        private void ExportarParaExcel(string caminho, int qtdCestas)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta charset='UTF-8'><title>Relatório de Cestas</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #2c3e50; border-bottom: 2px solid #3498db; }");
            sb.AppendLine("table { border-collapse: collapse; width: 100%; margin-top: 20px; }");
            sb.AppendLine("th { background-color: #2c3e50; color: white; padding: 12px; border: 1px solid #ddd; }");
            sb.AppendLine("td { padding: 10px; border: 1px solid #ddd; }");
            sb.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
            sb.AppendLine(".insuficiente { background-color: #ffcccc; }");
            sb.AppendLine(".ok { background-color: #ccffcc; }");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine($"<h1>📦 Relatório de Distribuição de Cestas</h1>");
            sb.AppendLine($"<p><strong>Data:</strong> {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");
            sb.AppendLine($"<p><strong>Quantidade de Cestas:</strong> {qtdCestas}</p>");
            sb.AppendLine("<table><thead><tr><th>Produto</th><th>Qtde por Cesta</th><th>Estoque Atual</th><th>Total Necessário</th><th>Status</th><th>Quanto Falta</th><th>Sobra</th></tr></thead><tbody>");

            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;

                string produto = row.Cells["Produto"].Value?.ToString() ?? "";
                string qtdePorCesta = row.Cells["QtdePorCesta"].Value?.ToString() ?? "0";
                string estoqueAtual = row.Cells["EstoqueAtual"].Value?.ToString() ?? "0";
                string totalNecessario = row.Cells["TotalNecessario"].Value?.ToString() ?? "0";
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                string quantoFalta = row.Cells["QuantoFalta"].Value?.ToString() ?? "0";
                string sobra = row.Cells["Sobra"].Value?.ToString() ?? "0";
                string rowClass = status == "Insuficiente" ? "insuficiente" : "ok";

                sb.AppendLine($"<tr class='{rowClass}'>");
                sb.AppendLine($"<td>{produto}</td><td style='text-align:right'>{qtdePorCesta}</td>");
                sb.AppendLine($"<td style='text-align:right'>{estoqueAtual}</td><td style='text-align:right'>{totalNecessario}</td>");
                sb.AppendLine($"<td style='text-align:center'>{status}</td><td style='text-align:right'>{quantoFalta}</td>");
                sb.AppendLine($"<td style='text-align:right'>{sobra}</td></tr>");
            }

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<h2>📋 Lista de Itens para Montagem</h2>");
            sb.AppendLine("<table><thead><tr><th>Produto</th><th>Quantidade para " + qtdCestas + " Cestas</th></tr></thead><tbody>");

            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;
                string produto = row.Cells["Produto"].Value?.ToString() ?? "";
                int qtdePorCesta = Convert.ToInt32(row.Cells["QtdePorCesta"].Value ?? "0");
                int totalNecessario = qtdePorCesta * qtdCestas;
                sb.AppendLine($"<tr><td>{produto}</td><td style='text-align:right'>{totalNecessario}</td></tr>");
            }

            sb.AppendLine("</tbody></table></body></html>");
            System.IO.File.WriteAllText(caminho, sb.ToString(), Encoding.UTF8);
        }

        private void ExportarParaHTML(string caminho, int qtdCestas)
        {
            ExportarParaExcel(caminho, qtdCestas);
        }

        // ✅ Adicionar botão de exportar
        private void AdicionarBotaoExportar()
        {
            Button btnExportar = new Button();
            btnExportar.Text = "📊 Exportar Resultado";
            btnExportar.Size = new Size(150, 35);
            btnExportar.BackColor = Color.FromArgb(52, 152, 219);
            btnExportar.ForeColor = Color.White;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnExportar.Location = new Point(btnMontar.Location.X + btnMontar.Width + 10, btnMontar.Location.Y);
            btnExportar.Click += (sender, e) => ExportarResultadoCestas();
            this.Controls.Add(btnExportar);
        }

        private void ApenasNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void limparDados()
        {
            codModeloCestaSelecionado = 0;
            cbbModeloDeCesta.SelectedItem = null;
            txtQtdCestas.Clear();
            dgvItensDaCesta.Rows.Clear();
        }

        private void calcularTotalNecessario()
        {
            int qtdCestas = string.IsNullOrEmpty(txtQtdCestas.Text) ? 0 : Convert.ToInt32(txtQtdCestas.Text);

            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;

                int quantidadePorCesta = Convert.ToInt32(row.Cells["QtdePorCesta"].Value ?? 0);
                int estoqueAtual = Convert.ToInt32(row.Cells["EstoqueAtual"].Value ?? 0);
                int totalNecessario = quantidadePorCesta * qtdCestas;
                row.Cells["TotalNecessario"].Value = totalNecessario;

                if (estoqueAtual < totalNecessario)
                {
                    row.Cells["Status"].Style.BackColor = Color.LightCoral;
                    row.Cells["Status"].Value = "Insuficiente";
                    row.Cells["QuantoFalta"].Value = totalNecessario - estoqueAtual;
                    row.Cells["Sobra"].Value = 0;
                }
                else
                {
                    row.Cells["Status"].Style.BackColor = Color.Honeydew;
                    row.Cells["Status"].Value = "Ok";
                    row.Cells["QuantoFalta"].Value = "";
                    row.Cells["Sobra"].Value = estoqueAtual - totalNecessario;
                }
            }
        }

        private int obterEstoqueAtual(string descricao)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = @"SELECT COALESCE(SUM(CASE WHEN quantidade > 0 THEN quantidade ELSE 0 END), 0) -
                                           COALESCE(SUM(CASE WHEN quantidade < 0 THEN ABS(quantidade) ELSE 0 END), 0) AS estoqueAtual 
                                    FROM tbProdutos WHERE descricao = @descricao;";
                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = descricao;
                comm.Connection = DataBaseConnection.OpenConnection();
                var result = comm.ExecuteScalar();
                DataBaseConnection.CloseConnection();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void AdicionarProdutoNoGrid(string nomeProduto, int quantidadePorCesta)
        {
            int estoqueAtual = obterEstoqueAtual(nomeProduto);

            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT codList FROM tbLista WHERE descricao = @descricao";
                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = nomeProduto;
                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        if (ProdutoJaExisteNoDgv(DR.GetInt32("codList")))
                        {
                            MessageBox.Show("Produto já está na lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            DataBaseConnection.CloseConnection();
                            return;
                        }

                        dgvItensDaCesta.Rows.Add(
                            DR["codList"].ToString(), nomeProduto, quantidadePorCesta, estoqueAtual, "", "", "", "");
                    }
                    DataBaseConnection.CloseConnection();
                }
            }

            if (!string.IsNullOrEmpty(txtQtdCestas.Text) && int.TryParse(txtQtdCestas.Text, out int qtd))
                calcularTotalNecessario();
        }

        private bool ProdutoJaExisteNoDgv(int codProduto)
        {
            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["codList"].Value != null && Convert.ToInt32(row.Cells["codList"].Value) == codProduto)
                    return true;
            }
            return false;
        }

        private bool QuantidadeValida()
        {
            return int.TryParse(txtQtdCestas.Text, out int quantidade) && quantidade > 0;
        }

        // EVENTOS
        private void cbbModeloDeCesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbModeloDeCesta.SelectedItem != null)
            {
                buscarCodModeloPorDescricao(cbbModeloDeCesta.SelectedItem.ToString());
                carregarDadosNoDgvItensDaCesta(codModeloCestaSelecionado);
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void dgvItensDaCesta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvItensDaCesta.Columns["RemoverProduto"].Index)
            {
                dgvItensDaCesta.Rows.RemoveAt(e.RowIndex);
                calcularTotalNecessario();
            }
        }

        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAdicionarItemNaCesta(codUsuLogado))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    AdicionarProdutoNoGrid(frm.NomeProdutoSelecionado, frm.QuantidadeSelecionada);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparDados();
        }

        private void txtQtdCestas_TextChanged(object sender, EventArgs e)
        {
            calcularTotalNecessario();
        }

        private void txtQtdCestas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void dgvItensDaCesta_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItensDaCesta.Columns[e.ColumnIndex].Name == "QtdePorCesta")
                calcularTotalNecessario();
        }

        private void dgvItensDaCesta_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvItensDaCesta.CurrentCell.ColumnIndex == dgvItensDaCesta.Columns["QtdePorCesta"].Index)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress -= ApenasNumeros_KeyPress;
                    tb.KeyPress += ApenasNumeros_KeyPress;
                }
            }
        }

        private void btnModeloDeCesta_Click(object sender, EventArgs e)
        {
            frmModelosDeCestas abrir = new frmModelosDeCestas(codUsuLogado, 1);
            abrir.Show();
            this.Close();
        }

        private void frmCestas_Load(object sender, EventArgs e)
        {
            // Mantido vazio
        }



        private void btnMontar_Click(object sender, EventArgs e)
        {
            if (dgvItensDaCesta.Rows.Count <= 1 || string.IsNullOrEmpty(txtQtdCestas.Text) || !QuantidadeValida())
            {
                MessageBox.Show("Adicione itens à cesta e informe uma quantidade válida", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Deseja montar {txtQtdCestas.Text} cesta(s)?",
                "Confirmar Montagem", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                montarCestas(Convert.ToInt32(txtQtdCestas.Text), codUsuLogado);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            ExportarResultadoCestas();
        }
    }
}

//using MySql.Data.MySqlClient;
//using System;
//using System.Data;
//using System.Drawing;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;

//namespace GPSFA_WinForms
//{
//    public partial class frmCestas : Form
//    {
//        const int MF_BYCOMMAND = 0X400;
//        [DllImport("user32")]
//        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
//        [DllImport("user32")]
//        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
//        [DllImport("user32")]
//        static extern int GetMenuItemCount(IntPtr hWnd);

//        public frmCestas()
//        {
//            InitializeComponent();
//        }
//        public frmCestas(int codUsu)
//        {
//            codUsuLogado = codUsu;
//            InitializeComponent();
//            ConfigDgvItensDaCesta();
//            carregarModelosDeCestaNaComboBox();
//        }

//        int codUsuLogado, codModeloCestaSelecionado;

//        // MÉTODOS DE QUERIES NO BANCO DE DADOS
//        // Carrega os presets de cesta básica configurados no banco de dados - OK
//        private void carregarModelosDeCestaNaComboBox()
//        {
//            MySqlCommand comm = new MySqlCommand();
//            comm.CommandText = "SELECT * FROM tbModeloCesta ORDER BY descricao ASC;";
//            comm.CommandType = CommandType.Text;

//            comm.Connection = DataBaseConnection.OpenConnection();

//            MySqlDataReader DR = comm.ExecuteReader();

//            while (DR.Read())
//            {
//                cbbModeloDeCesta.Items.Add(DR.GetString(1));
//            }

//            DataBaseConnection.CloseConnection();
//        }

//        // Busca o código do modelo de cesta pela descrição - OK
//        private void buscarCodModeloPorDescricao(string cestaModeloNome)
//        {
//            using (MySqlCommand comm = new MySqlCommand())
//            {
//                comm.CommandText = $"SELECT codModelo FROM tbModeloCesta WHERE descricao = @descricao;";

//                comm.CommandType = CommandType.Text;
//                comm.Parameters.Clear();
//                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = cestaModeloNome;

//                comm.Connection = DataBaseConnection.OpenConnection();

//                using (MySqlDataReader DR = comm.ExecuteReader())
//                {
//                    if (DR.Read())
//                    {
//                        if (DR.GetInt32("codModelo") > 0)
//                        {
//                            codModeloCestaSelecionado = DR.GetInt32("codModelo");

//                            DataBaseConnection.CloseConnection();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Codigo não encontrado");
//                            DataBaseConnection.CloseConnection();
//                        }
//                    }
//                }
//            }
//        }

//        // Carregar dados no datagrid view ao selecionar o modelo de cesta e a quantidade de cestas - OK
//        private void carregarDadosNoDgvItensDaCesta(int codModelo)
//        {
//            using (MySqlCommand comm = new MySqlCommand())
//            {
//                comm.CommandText = $"SELECT l.codList, l.descricao, imc.quantidadeMinima, IFNULL(SUM(p.quantidade), 0) AS estoqueAtual FROM tbItensDoModeloCesta imc INNER JOIN tbLista l ON l.codList = imc.codList LEFT JOIN tbEstoqueItens p ON p.codList = l.codList WHERE imc.codModelo = 1 GROUP BY imc.codModelo, imc.codList, l.descricao, l.unidade, imc.quantidadeMinima;";
//                comm.CommandType = CommandType.Text;
//                comm.Parameters.Clear();
//                comm.Parameters.Add("@codModelo", MySqlDbType.Int32).Value = codModelo;

//                comm.Connection = DataBaseConnection.OpenConnection();

//                using (MySqlDataReader DR = comm.ExecuteReader())
//                {
//                    while (DR.Read())
//                    {
//                        dgvItensDaCesta.Rows.Add(
//                            DR["codList"].ToString(),
//                            DR["descricao"].ToString(),
//                            DR["quantidadeMinima"].ToString(),
//                            DR["estoqueAtual"].ToString()
//                        );
//                    }

//                    DataBaseConnection.CloseConnection();
//                }
//            }
//        }

//        // Faz a criação de cestas com base na quantidade e registra itens em uma cesta - OK
//        private void montarCestas(int quantidadeDeCestas, int codUsu)
//        {
//            try
//            {
//                MySqlConnection conn = DataBaseConnection.OpenConnection();

//                for (int i = 0; i < quantidadeDeCestas; i++)
//                {
//                    // 1️⃣ Inserir cesta
//                    var cmdCesta = new MySqlCommand(
//                        "INSERT INTO tbCestas(codUsu) VALUES(@codUsu); SELECT LAST_INSERT_ID();",
//                        conn
//                    );

//                    cmdCesta.Parameters.Add("@codUsu", MySqlDbType.Int32).Value = codUsu;
//                    int codCesta = Convert.ToInt32(cmdCesta.ExecuteScalar());

//                    // 2️⃣ Inserir itens da cesta
//                    foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
//                    {
//                        if (row.IsNewRow) continue;

//                        int codList = Convert.ToInt32(row.Cells["codList"].Value);
//                        int quantidadePorCesta = Convert.ToInt32(row.Cells["QtdePorCesta"].Value);

//                        // Insere o item mesmo se não tiver estoque (vai dar negativo depois)
//                        var cmdItem = new MySqlCommand(
//                            "INSERT INTO tbItensCesta(codCes, codList, quantidade) VALUES(@codCes, @codList, @quantidade)",
//                            conn
//                        );

//                        cmdItem.Parameters.Add("@codCes", MySqlDbType.Int32).Value = codCesta;
//                        cmdItem.Parameters.Add("@codList", MySqlDbType.Int32).Value = codList;
//                        cmdItem.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = quantidadePorCesta;

//                        cmdItem.ExecuteNonQuery();
//                    }
//                }

//                DataBaseConnection.CloseConnection();
//                MessageBox.Show($"{quantidadeDeCestas} cesta(s) montada(s) com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception error)
//            {
//                MessageBox.Show($"Erro ao montar cestas! Erro:\n\n{error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                DataBaseConnection.CloseConnection();
//            }
//        }


//        // CONFIGURAÇÕES E AÇÕES DA JANELA
//        // Configuração adicional DO DESIGN do datagrid view de intes da cesta - OK
//        private void ConfigDgvItensDaCesta()
//        {
//            // Ajustar para ocupar toda a largura
//            dgvItensDaCesta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            // Alternar cores das linhas
//            dgvItensDaCesta.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

//            // Aumentar fonte
//            dgvItensDaCesta.RowsDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
//            dgvItensDaCesta.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
//            //// Ajustar altura das linhas
//            dgvItensDaCesta.RowTemplate.Height = 40;
//            //// Habilitar quebra de texto
//            dgvItensDaCesta.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
//            //// Ajustar seleção de célula
//            dgvItensDaCesta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

//            dgvItensDaCesta.AllowUserToAddRows = false;
//            dgvItensDaCesta.MultiSelect = false;

//            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
//            buttonColumn.HeaderText = "";
//            buttonColumn.Name = "RemoverProduto"; // Name for programmatic reference
//            buttonColumn.Text = "Remover"; // The text displayed on the button
//            buttonColumn.UseColumnTextForButtonValue = true; // Use the Text property value for all buttons
//            dgvItensDaCesta.Columns.Add(buttonColumn);

//        }

//        private void ApenasNumeros_KeyPress(object sender, KeyPressEventArgs e)
//        {
//            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
//            {
//                e.Handled = true;
//            }
//        }

//        // Limpeza de dados da janela
//        private void limparDados()
//        {
//            codModeloCestaSelecionado = 0;
//            cbbModeloDeCesta.SelectedItem = null;
//            txtQtdCestas.Clear();
//            dgvItensDaCesta.Rows.Clear();
//        }

//        // Calcular o total necessário de um produto com base em: Quantidade de cestas x Quantidade de itens por cesta
//        private void calcularTotalNecessario()
//        {
//            int qtdCestas;
//            if (txtQtdCestas.Text.Equals(""))
//            {
//                qtdCestas = 0;
//            }
//            else
//            {
//                qtdCestas = Convert.ToInt32(txtQtdCestas.Text);
//            }

//            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
//            {
//                if (row.IsNewRow) continue;

//                int quantidadePorCesta = 0;
//                int estoqueAtual = 0;

//                int.TryParse(row.Cells["QtdePorCesta"].Value?.ToString(), out quantidadePorCesta);
//                int.TryParse(row.Cells["EstoqueAtual"].Value?.ToString(), out estoqueAtual);

//                int totalNecessario = quantidadePorCesta * qtdCestas;

//                row.Cells["TotalNecessario"].Value = totalNecessario;
//                calcularQuantoFalta();

//                if (estoqueAtual < totalNecessario)
//                {
//                    row.Cells["Status"].Style.BackColor = System.Drawing.Color.LightCoral;
//                    row.Cells["Status"].Value = "Insuficiente";

//                }
//                else
//                {
//                    row.Cells["Status"].Style.BackColor = System.Drawing.Color.LightGray;
//                    row.Cells["Status"].Value = "Ok";
//                }
//            }
//        }

//        private void calcularQuantoFalta()
//        {
//            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
//            {
//                if (row.IsNewRow) continue;

//                int estoqueAtual = 0;
//                int totalNecessario = 0;

//                int.TryParse(row.Cells["EstoqueAtual"].Value?.ToString(), out estoqueAtual);
//                int.TryParse(row.Cells["TotalNecessario"].Value?.ToString(), out totalNecessario);

//                int quantoFalta = totalNecessario - estoqueAtual;

//                if (quantoFalta > 0)
//                {
//                    row.Cells["QuantoFalta"].Value = quantoFalta.ToString();
//                }
//                else
//                {
//                    row.Cells["QuantoFalta"].Value = "";
//                }
//            }
//        }

//        // Busca os dados da quantidade atual em estoque para retornar na inserção de item individual na cesta
//        private int obterEstoqueAtual(string descricao)
//        {
//            using (MySqlCommand comm = new MySqlCommand())
//            {
//                comm.CommandText = $"SELECT IFNULL(SUM(quantidade),0) AS estoqueAtual FROM tbProdutos WHERE descricao = @descricao;";
//                comm.CommandType = CommandType.Text;
//                comm.Parameters.Clear();
//                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = descricao;

//                comm.Connection = DataBaseConnection.OpenConnection();

//                var result = comm.ExecuteScalar();

//                return Convert.ToInt32(result);
//            }
//        }

//        // Adiciona um produto na cesta manualmente
//        private void AdicionarProdutoNoGrid(string nomeProduto, int quantidadePorCesta)
//        {
//            // aqui é realizada a busca dos dados do produto salvando na variável {estoqueAtual}
//            int estoqueAtual = obterEstoqueAtual(nomeProduto);

//            using (MySqlCommand comm = new MySqlCommand())
//            {
//                comm.CommandText = $"SELECT codList FROM tbLista WHERE descricao = @descricao";
//                comm.CommandType = CommandType.Text;
//                comm.Parameters.Clear();
//                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = nomeProduto;

//                comm.Connection = DataBaseConnection.OpenConnection();

//                using (MySqlDataReader DR = comm.ExecuteReader())
//                {
//                    while (DR.Read())
//                    {
//                        if (ProdutoJaExisteNoDgv(DR.GetInt32("codList")))
//                        {
//                            MessageBox.Show("Produto já está na lista.");
//                            return;
//                        }
//                        dgvItensDaCesta.Rows.Add(
//                            DR["codList"].ToString(),
//                            nomeProduto,
//                            quantidadePorCesta,
//                            estoqueAtual
//                        );
//                    }

//                    DataBaseConnection.CloseConnection();

//                }
//            }

//        }


//        // MÉTODOS DE EVENTO DE CLIQUE
//        // Aciona o método de buscar os itens de um determinado modelo de cesta
//        private void cbbModeloDeCesta_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (cbbModeloDeCesta.SelectedItem != null)
//            {
//                // Busca o código do modelo de cesta pela descrição do item selecionado
//                buscarCodModeloPorDescricao(cbbModeloDeCesta.SelectedItem.ToString());

//                // Carrega os dados referentes ao modelo de cesta selecionado
//                carregarDadosNoDgvItensDaCesta(codModeloCestaSelecionado);
//            }
//            else
//            {
//                return;
//            }
//        }

//        // Instancia do evento de clique do botão de voltar - OK
//        private void btnVoltar_Click(object sender, EventArgs e)
//        {
//            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
//            abrir.Show();
//            this.Close();
//        }

//        // Instancia do evento de clique de algum botão da coluna de remover produtos
//        private void dgvItensDaCesta_CellContentClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0 && e.ColumnIndex == dgvItensDaCesta.Columns["RemoverProduto"].Index)
//            {
//                dgvItensDaCesta.Rows.RemoveAt(e.RowIndex);
//            }
//        }

//        // Aciona o modal para adicionar um produto na cesta
//        private void btnAdicionarItem_Click(object sender, EventArgs e)
//        {
//            using (var frm = new frmAdicionarItemNaCesta(codUsuLogado))
//            {
//                if (frm.ShowDialog() == DialogResult.OK)
//                {
//                    AdicionarProdutoNoGrid(
//                        frm.NomeProdutoSelecionado,
//                        frm.QuantidadeSelecionada
//                    );
//                    calcularTotalNecessario();
//                }
//            }
//        }

//        // Instancia do evento de clique do botão de limpar - OK
//        private void btnLimpar_Click(object sender, EventArgs e)
//        {
//            limparDados();
//        }

//        // Evento de alterar o valor de quantidade de cestas - OK
//        private void txtQtdCestas_TextChanged(object sender, EventArgs e)
//        {
//            calcularTotalNecessario();
//        }

//        // Evento de pressionar teclas na caixa de texto de quantidade - limita a entrada de dados a números
//        private void txtQtdCestas_KeyPress(object sender, KeyPressEventArgs e)
//        {
//            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
//            {
//                e.Handled = true;
//            }
//        }

//        // Evento de alterar o valor de alguma célular dentro do dgv - OK
//        private void dgvItensDaCesta_CellEndEdit(object sender, DataGridViewCellEventArgs e)
//        {
//            if (dgvItensDaCesta.Columns[e.ColumnIndex].Name == "QtdePorCesta")
//            {
//                calcularTotalNecessario();
//            }
//        }

//        // Evento de pressionar teclas na caixa de texto de quantidade - limita a entrada de dados a números
//        private void dgvItensDaCesta_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
//        {
//            if (dgvItensDaCesta.CurrentCell.ColumnIndex == dgvItensDaCesta.Columns["QtdePorCesta"].Index)
//            {
//                TextBox tb = e.Control as TextBox;

//                if (tb != null)
//                {
//                    tb.KeyPress -= ApenasNumeros_KeyPress;
//                    tb.KeyPress += ApenasNumeros_KeyPress;
//                }
//            }
//        }

//        // Abre o modal para configurar modelos de cesta
//        private void btnModeloDeCesta_Click(object sender, EventArgs e)
//        {
//            frmModelosDeCestas abrir = new frmModelosDeCestas(codUsuLogado, 1);
//            abrir.Show();
//            this.Close();
//        }

//        // Parte crírica fluxo de montagem de cestas

//        // Método para validar se alguma linha da coluna "Status" contém a informação "Insuficiente"
//        private bool ExisteItemInsuficiente()
//        {
//            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
//            {
//                if (row.IsNewRow) continue;

//                var valor = row.Cells["Status"].Value?.ToString();

//                //if (!string.IsNullOrEmpty(valor) &&
//                //    valor.Equals("Insuficiente", StringComparison.OrdinalIgnoreCase))
//                //{
//                //    return true;
//                //}
//            }

//            return false;
//        }


//        private bool ProdutoJaExisteNoDgv(int codProduto)
//        {
//            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
//            {
//                if (row.IsNewRow) continue;

//                int codigoExistente = Convert.ToInt32(row.Cells["codList"].Value);

//                if (codigoExistente == codProduto)
//                    return true;
//            }

//            return false;
//        }

//        private bool QuantidadeValida()
//        {
//            if (!int.TryParse(txtQtdCestas.Text, out int quantidade))
//                return false;

//            return quantidade > 0;
//        }

//        private void frmCestas_Load(object sender, EventArgs e)
//        {

//        }

//        // Realiza o registro de montagem de cestas - A FAZER
//        private void btnMontar_Click(object sender, EventArgs e)
//        {
//            // Valida se o DGV está vazio ou se a quantidade é inválida
//            if (dgvItensDaCesta.Rows.Count <= 1 || string.IsNullOrEmpty(txtQtdCestas.Text) || !QuantidadeValida())
//            {
//                MessageBox.Show("Adicione itens à cesta e informe uma quantidade válida", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            // valida se o usuário confirma a montagem
//            DialogResult result = MessageBox.Show($"Deseja confirmar a montagem de {txtQtdCestas.Text} cestas?\n\nItens com estoque insuficiente serão distribuídos apenas o disponível.", "Mensagem do sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//            if (result == DialogResult.Yes)
//            {
//                montarCestas(Convert.ToInt32(txtQtdCestas.Text), codUsuLogado);
//                MessageBox.Show("Cestas montadas com sucesso", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                limparDados();
//            }
//        }
//    }

//}
