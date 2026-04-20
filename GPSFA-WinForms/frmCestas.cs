using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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

        // Declaração das variáveis
        int codUsuLogado, codModeloCestaSelecionado;

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

            // Criar o botão Exportar manualmente
            Button btnExportar = new Button();
            btnExportar.Text = "Exportar Resultado";
            btnExportar.Size = new Size(150, 35);
            btnExportar.BackColor = Color.FromArgb(52, 152, 219);
            btnExportar.ForeColor = Color.White;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnExportar.Location = new Point(btnMontar.Location.X + btnMontar.Width + 10, btnMontar.Location.Y);
            btnExportar.Click += (sender, e) => ExportarResultadoCestas();
            this.Controls.Add(btnExportar);
        }

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

        private void ConfigurarDataGridViewCestas()
        {
            dgvItensDaCesta.Columns.Clear();

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

            dgvItensDaCesta.Columns.Add("codList", "Código");
            dgvItensDaCesta.Columns["codList"].Visible = false;

            dgvItensDaCesta.Columns.Add("Produto", "Produto");
            dgvItensDaCesta.Columns.Add("QtdePorCesta", "Qtde por cesta");
            dgvItensDaCesta.Columns.Add("EstoqueAtual", "Estoque atual");
            dgvItensDaCesta.Columns.Add("TotalNecessario", "Total necessário");
            dgvItensDaCesta.Columns.Add("Status", "Status");
            dgvItensDaCesta.Columns.Add("QuantoFalta", "Quanto falta");
            dgvItensDaCesta.Columns.Add("Sobra", "Sobra");

            dgvItensDaCesta.Columns["Produto"].Width = 200;
            dgvItensDaCesta.Columns["QtdePorCesta"].Width = 100;
            dgvItensDaCesta.Columns["EstoqueAtual"].Width = 100;
            dgvItensDaCesta.Columns["TotalNecessario"].Width = 110;
            dgvItensDaCesta.Columns["Status"].Width = 90;
            dgvItensDaCesta.Columns["QuantoFalta"].Width = 90;
            dgvItensDaCesta.Columns["Sobra"].Width = 80;

            dgvItensDaCesta.EnableHeadersVisualStyles = false;
            dgvItensDaCesta.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvItensDaCesta.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvItensDaCesta.ColumnHeadersHeight = 40;

            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "";
            buttonColumn.Name = "RemoverProduto";
            buttonColumn.Text = "Remover";
            buttonColumn.UseColumnTextForButtonValue = true;
            buttonColumn.Width = 60;
            dgvItensDaCesta.Columns.Add(buttonColumn);
        }

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
                            "", "", "", "");
                    }
                    DataBaseConnection.CloseConnection();
                }
            }

            if (!string.IsNullOrEmpty(txtQtdCestas.Text) && int.TryParse(txtQtdCestas.Text, out int qtd))
            {
                calcularTotalNecessario();
            }
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

        private void montarCestas(int quantidadeDeCestas, int codUsu)
        {
            try
            {
                MySqlConnection conn = DataBaseConnection.OpenConnection();
                StringBuilder logDistribuicao = new StringBuilder();
                int totalCestasMontadas = 0;
                int totalItensRegistrados = 0;

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

                for (int i = 0; i < quantidadeDeCestas; i++)
                {
                    string sqlCesta = "INSERT INTO tbCestas(codUsu) VALUES(@codUsu); SELECT LAST_INSERT_ID();";
                    var cmdCesta = new MySqlCommand(sqlCesta, conn);
                    cmdCesta.Parameters.AddWithValue("@codUsu", 1);
                    int codCesta = Convert.ToInt32(cmdCesta.ExecuteScalar());

                    foreach (var item in quantidadePorCestaOriginal)
                    {
                        int codList = item.Key;
                        int quantidadePorCesta = item.Value;
                        string produto = nomesProdutos[codList];

                        try
                        {
                            string sqlItem = "INSERT INTO tbItensCesta(codCes, codList, quantidade) VALUES(@codCes, @codList, @quantidade)";
                            var cmdItem = new MySqlCommand(sqlItem, conn);
                            cmdItem.Parameters.AddWithValue("@codCes", codCesta);
                            cmdItem.Parameters.AddWithValue("@codList", codList);
                            cmdItem.Parameters.AddWithValue("@quantidade", quantidadePorCesta);
                            cmdItem.ExecuteNonQuery();

                            totalItensRegistrados += quantidadePorCesta;
                            logDistribuicao.AppendLine($"Cesta {i + 1}: {produto} - {quantidadePorCesta} unidades");
                        }
                        catch (Exception ex)
                        {
                            logDistribuicao.AppendLine($"Cesta {i + 1}: {produto} - ERRO: {ex.Message}");
                        }
                    }

                    totalCestasMontadas++;
                }

                DataBaseConnection.CloseConnection();

                string mensagem = $"{totalCestasMontadas} de {quantidadeDeCestas} cesta(s) montada(s)!\n";
                mensagem += $"Total de itens registrados: {totalItensRegistrados}\n\n";
                mensagem += $"DETALHES:\n{logDistribuicao}";

                MessageBox.Show(mensagem, "Distribuicao de Cestas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Erro ao montar cestas!\n\n{error.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataBaseConnection.CloseConnection();
            }
        }

        private void ExportarResultadoCestas()
        {
            if (dgvItensDaCesta.Rows.Count <= 1)
            {
                MessageBox.Show("Não há dados para exportar. Adicione itens à cesta primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qtdCestas = 0;
            if (!string.IsNullOrEmpty(txtQtdCestas.Text))
            {
                int.TryParse(txtQtdCestas.Text, out qtdCestas);
            }

            if (qtdCestas <= 0)
            {
                MessageBox.Show("Informe a quantidade de cestas antes de exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frmOpcoes = new Form())
            {
                frmOpcoes.Text = "Opções de Exportação";
                frmOpcoes.Size = new Size(450, 280);
                frmOpcoes.StartPosition = FormStartPosition.CenterParent;
                frmOpcoes.FormBorderStyle = FormBorderStyle.FixedDialog;
                frmOpcoes.MaximizeBox = false;
                frmOpcoes.MinimizeBox = false;

                Label lblTitulo = new Label() { Text = "Selecione o tipo de relatório:", Location = new Point(20, 20), Size = new Size(350, 30), Font = new Font("Segoe UI", 12, FontStyle.Bold) };
                RadioButton rbPlanejamento = new RadioButton() { Text = "Relatorio de Planejamento (detalhado)", Location = new Point(30, 60), Size = new Size(350, 30), Font = new Font("Segoe UI", 11), Checked = true };
                RadioButton rbMontagem = new RadioButton() { Text = "Relatorio de Montagem (checklist por cesta)", Location = new Point(30, 100), Size = new Size(350, 30), Font = new Font("Segoe UI", 11) };
                RadioButton rbAmbos = new RadioButton() { Text = "Ambos os relatorios", Location = new Point(30, 140), Size = new Size(350, 30), Font = new Font("Segoe UI", 11) };
                Button btnConfirmar = new Button() { Text = "Exportar", Location = new Point(160, 190), Size = new Size(120, 40), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

                frmOpcoes.Controls.AddRange(new Control[] { lblTitulo, rbPlanejamento, rbMontagem, rbAmbos, btnConfirmar });

                bool exportar = false;
                string tipoExportacao = "";

                btnConfirmar.Click += (s, ev) =>
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

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    string extensao = System.IO.Path.GetExtension(sfd.FileName).ToLower();
                    string caminhoBase = System.IO.Path.GetDirectoryName(sfd.FileName);
                    string nomeBase = System.IO.Path.GetFileNameWithoutExtension(sfd.FileName);

                    if (tipoExportacao == "planejamento")
                    {
                        ExportarPlanejamentoExcel(sfd.FileName, qtdCestas);
                        MessageBox.Show($"Relatorio de Planejamento exportado!\n\nLocal: {sfd.FileName}", "Exportacao Concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (tipoExportacao == "montagem")
                    {
                        ExportarMontagemExcel(sfd.FileName, qtdCestas);
                        MessageBox.Show($"Relatorio de Montagem exportado!\n\nLocal: {sfd.FileName}", "Exportacao Concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string arquivoPlanejamento = System.IO.Path.Combine(caminhoBase, $"{nomeBase}_planejamento{extensao}");
                        string arquivoMontagem = System.IO.Path.Combine(caminhoBase, $"{nomeBase}_montagem{extensao}");
                        ExportarPlanejamentoExcel(arquivoPlanejamento, qtdCestas);
                        ExportarMontagemExcel(arquivoMontagem, qtdCestas);
                        MessageBox.Show($"Ambos os relatorios exportados!\n\nPlanejamento: {arquivoPlanejamento}\nMontagem: {arquivoMontagem}", "Exportacao Concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportarPlanejamentoExcel(string caminho, int qtdCestas)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Planejamento");
                worksheet.Cell(1, 1).Value = "RELATORIO DE PLANEJAMENTO DE CESTAS";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, 7).Merge();
                worksheet.Cell(2, 1).Value = $"Data de emissao: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cell(3, 1).Value = $"Quantidade de cestas: {qtdCestas}";

                int totalOk = 0, totalInsuficiente = 0, totalFaltando = 0, totalSobra = 0;

                foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                {
                    if (row.IsNewRow) continue;

                    string status = row.Cells["Status"].Value?.ToString() ?? "";
                    if (status == "Ok") totalOk++;
                    else if (status == "Insuficiente") totalInsuficiente++;

                    int falta = 0, sobra = 0;
                    if (row.Cells["QuantoFalta"].Value != null)
                        int.TryParse(row.Cells["QuantoFalta"].Value.ToString(), out falta);
                    if (row.Cells["Sobra"].Value != null)
                        int.TryParse(row.Cells["Sobra"].Value.ToString(), out sobra);
                    totalFaltando += falta;
                    totalSobra += sobra;
                }

                worksheet.Cell(5, 1).Value = "RESUMO GERAL";
                worksheet.Cell(5, 1).Style.Font.Bold = true;
                worksheet.Cell(6, 1).Value = "Itens com estoque OK:"; worksheet.Cell(6, 2).Value = totalOk;
                worksheet.Cell(7, 1).Value = "Itens com estoque insuficiente:"; worksheet.Cell(7, 2).Value = totalInsuficiente;
                worksheet.Cell(8, 1).Value = "Total de unidades faltando:"; worksheet.Cell(8, 2).Value = totalFaltando;
                worksheet.Cell(9, 1).Value = "Total de unidades que sobram:"; worksheet.Cell(9, 2).Value = totalSobra;

                worksheet.Cell(11, 1).Value = "DETALHAMENTO DOS ITENS";
                worksheet.Cell(11, 1).Style.Font.Bold = true;
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

                    // Conversões seguras com TryParse
                    int qtdePorCesta = 0;
                    int.TryParse(row.Cells["QtdePorCesta"].Value?.ToString(), out qtdePorCesta);

                    int estoqueAtual = 0;
                    int.TryParse(row.Cells["EstoqueAtual"].Value?.ToString(), out estoqueAtual);

                    int totalNecessario = 0;
                    int.TryParse(row.Cells["TotalNecessario"].Value?.ToString(), out totalNecessario);

                    int quantoFalta = 0;
                    int.TryParse(row.Cells["QuantoFalta"].Value?.ToString(), out quantoFalta);

                    int sobra = 0;
                    int.TryParse(row.Cells["Sobra"].Value?.ToString(), out sobra);

                    worksheet.Cell(rowIndex, 1).Value = row.Cells["Produto"].Value?.ToString() ?? "";
                    worksheet.Cell(rowIndex, 2).Value = qtdePorCesta;
                    worksheet.Cell(rowIndex, 3).Value = estoqueAtual;
                    worksheet.Cell(rowIndex, 4).Value = totalNecessario;
                    worksheet.Cell(rowIndex, 5).Value = row.Cells["Status"].Value?.ToString() ?? "";
                    worksheet.Cell(rowIndex, 6).Value = quantoFalta;
                    worksheet.Cell(rowIndex, 7).Value = sobra;
                    rowIndex++;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(caminho);
            }
        }

        private void ExportarMontagemExcel(string caminho, int qtdCestas)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheetItens = workbook.Worksheets.Add("Itens para Separar");
                worksheetItens.Cell(1, 1).Value = "LISTA PARA MONTAGEM DE CESTAS - ITENS TOTAIS";
                worksheetItens.Cell(1, 1).Style.Font.Bold = true;
                worksheetItens.Cell(1, 1).Style.Font.FontSize = 16;
                worksheetItens.Range(1, 1, 1, 4).Merge();
                worksheetItens.Cell(2, 1).Value = $"Data de emissao: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheetItens.Cell(3, 1).Value = $"Total de Cestas: {qtdCestas}";
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

                    int totalNecessario = 0;
                    int.TryParse(row.Cells["TotalNecessario"].Value?.ToString(), out totalNecessario);

                    worksheetItens.Cell(rowIndex, 1).Value = row.Cells["Produto"].Value?.ToString() ?? "";
                    worksheetItens.Cell(rowIndex, 2).Value = totalNecessario;
                    worksheetItens.Cell(rowIndex, 3).Value = "UN";
                    worksheetItens.Cell(rowIndex, 4).Value = "[ ]";
                    rowIndex++;
                }
                worksheetItens.Columns().AdjustToContents();

                var worksheetChecklist = workbook.Worksheets.Add("Checklist por Cesta");
                worksheetChecklist.Cell(1, 1).Value = "CHECKLIST POR CESTA";
                worksheetChecklist.Cell(1, 1).Style.Font.Bold = true;
                worksheetChecklist.Cell(1, 1).Style.Font.FontSize = 16;
                worksheetChecklist.Range(1, 1, 1, 4).Merge();
                worksheetChecklist.Cell(2, 1).Value = $"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheetChecklist.Cell(3, 1).Value = $"Total de Cestas: {qtdCestas}";

                int linhaAtual = 5;
                for (int cesta = 1; cesta <= qtdCestas; cesta++)
                {
                    worksheetChecklist.Cell(linhaAtual, 1).Value = $"CESTA {cesta}";
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.Bold = true;
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.FontSize = 14;
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(52, 152, 219);
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                    worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4).Merge();
                    linhaAtual++;

                    worksheetChecklist.Cell(linhaAtual, 1).Value = "Produto";
                    worksheetChecklist.Cell(linhaAtual, 2).Value = "Quantidade";
                    worksheetChecklist.Cell(linhaAtual, 3).Value = "Conferido";
                    worksheetChecklist.Cell(linhaAtual, 4).Value = "Observacao";
                    var subHeader = worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4);
                    subHeader.Style.Font.Bold = true;
                    subHeader.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                    linhaAtual++;

                    foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int qtdePorCesta = 0;
                        int.TryParse(row.Cells["QtdePorCesta"].Value?.ToString(), out qtdePorCesta);

                        worksheetChecklist.Cell(linhaAtual, 1).Value = row.Cells["Produto"].Value?.ToString() ?? "";
                        worksheetChecklist.Cell(linhaAtual, 2).Value = qtdePorCesta;
                        worksheetChecklist.Cell(linhaAtual, 3).Value = "[ ]";
                        worksheetChecklist.Cell(linhaAtual, 4).Value = row.Cells["Status"].Value?.ToString() == "Insuficiente" ? "Estoque insuficiente" : "";
                        linhaAtual++;
                    }

                    int somaQuantidades = 0;
                    foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                    {
                        if (row.IsNewRow) continue;
                        int qtde = 0;
                        int.TryParse(row.Cells["QtdePorCesta"].Value?.ToString(), out qtde);
                        somaQuantidades += qtde;
                    }

                    worksheetChecklist.Cell(linhaAtual, 1).Value = "TOTAL ITENS";
                    worksheetChecklist.Cell(linhaAtual, 1).Style.Font.Bold = true;
                    worksheetChecklist.Cell(linhaAtual, 2).Value = somaQuantidades;
                    worksheetChecklist.Cell(linhaAtual, 2).Style.Font.Bold = true;
                    worksheetChecklist.Cell(linhaAtual, 3).Value = "[ ]";
                    worksheetChecklist.Cell(linhaAtual, 4).Value = "Conferir se todos os itens estao na cesta";
                    worksheetChecklist.Range(linhaAtual, 1, linhaAtual, 4).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                    linhaAtual += 2;
                }
                worksheetChecklist.Columns().AdjustToContents();
                workbook.SaveAs(caminho);
            }
        }

        // ==================== EVENTOS ====================
        // ✅ CORRETO — apenas recalcula visualização, zero gravação no banco
        private void btnMontar_Click(object sender, EventArgs e)
        {
            if (dgvItensDaCesta.Rows.Count <= 1 || string.IsNullOrEmpty(txtQtdCestas.Text) || !QuantidadeValida())
            {
                MessageBox.Show("Adicione itens à cesta e informe uma quantidade válida.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            calcularTotalNecessario();
            MessageBox.Show($"Simulação calculada para {txtQtdCestas.Text} cesta(s).\n\n" +
                            "Use 'Exportar Resultado' para salvar o relatório.",
                "Simulação de montagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparDados();
        }

        private void limparDados()
        {
            codModeloCestaSelecionado = 0;
            cbbModeloDeCesta.SelectedItem = null;
            txtQtdCestas.Clear();
            dgvItensDaCesta.Rows.Clear();
        }

        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAdicionarItemNaCesta(codUsuLogado))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    AdicionarProdutoNoGrid(frm.NomeProdutoSelecionado, frm.QuantidadeSelecionada);
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

                        dgvItensDaCesta.Rows.Add(DR["codList"].ToString(), nomeProduto, quantidadePorCesta, estoqueAtual, "", "", "", "");
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

        // ✅ CORRETO — busca por codList via tbLista, usa SUM direto consistente com o resto do sistema
        private int obterEstoqueAtual(string descricao)
        {
            using (var conn = DataBaseConnection.OpenConnection())
            using (var cmd = new MySqlCommand(@"
        SELECT COALESCE(SUM(p.quantidade), 0)
        FROM tbProdutos p
        INNER JOIN tbLista l ON l.codList = p.codList
        WHERE l.descricao = @descricao", conn))
            {
                cmd.Parameters.AddWithValue("@descricao", descricao);
                var result = cmd.ExecuteScalar();
                DataBaseConnection.CloseConnection();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }


        private bool QuantidadeValida()
        {
            return int.TryParse(txtQtdCestas.Text, out int quantidade) && quantidade > 0;
        }

        private void btnModeloDeCesta_Click(object sender, EventArgs e)
        {
            frmModelosDeCestas abrir = new frmModelosDeCestas(codUsuLogado, 1);
            abrir.Show();
            this.Close();
        }

        private void cbbModeloDeCesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbModeloDeCesta.SelectedItem != null)
            {
                buscarCodModeloPorDescricao(cbbModeloDeCesta.SelectedItem.ToString());
                carregarDadosNoDgvItensDaCesta(codModeloCestaSelecionado);
            }
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

        private void dgvItensDaCesta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvItensDaCesta.Columns["RemoverProduto"].Index)
            {
                dgvItensDaCesta.Rows.RemoveAt(e.RowIndex);
                calcularTotalNecessario();
            }
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

        private void ApenasNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            ExportarResultadoCestas();
        }

        private void frmCestas_Load(object sender, EventArgs e)
        {
        }
    }
}