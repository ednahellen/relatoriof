using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmRelatorios : Form
    {
        private int _codUsuLogado;
        private int linhaAtual = 0;
        private PrintDocument printDocument = new PrintDocument();

        public frmRelatorios()
        {
            InitializeComponent();
        }

        public frmRelatorios(int codUsu) : this()
        {
            _codUsuLogado = codUsu;
            this.Load += frmRelatorios_Load;
            ConfigurarEventos();
        }

        private void frmRelatorios_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView();
            dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
            dtpDataFinalPeriodo.Value = DateTime.Now;

            CarregarUsuarios();
            CarregarProdutos();
            CarregarOrigens();
            CarregarDados();
        }

        private void ConfigurarEventos()
        {
            btnPesquisar.Click += (s, e) => CarregarDados();
            btnLimparFiltros.Click += (s, e) =>
            {
                dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
                dtpDataFinalPeriodo.Value = DateTime.Now;
                cbxProduto.SelectedIndex = 0;
                cbbUsuario.SelectedIndex = 0;
                cbxOrigem.SelectedIndex = 0;
                CarregarDados();
            };

            btnMenu.Click += btnMenu_Click;
            btnImprimir.Click += BtnExportarMenu_Click;
        }

        private void ConfigurarDataGridView()
        {
            dgvRelatorios.AutoGenerateColumns = true;
            dgvRelatorios.AllowUserToAddRows = false;
            dgvRelatorios.RowHeadersVisible = false;
            dgvRelatorios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelatorios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelatorios.BackgroundColor = Color.White;

            dgvRelatorios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(48, 112, 99);
            dgvRelatorios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRelatorios.EnableHeadersVisualStyles = false;

            if (!dgvRelatorios.Columns.Contains("Excluir"))
            {
                var btn = new DataGridViewButtonColumn();
                btn.Name = "Excluir";
                btn.Text = "Excluir";
                btn.UseColumnTextForButtonValue = true;
                dgvRelatorios.Columns.Add(btn);
            }

            dgvRelatorios.CellClick += DgvRelatorios_CellClick;
        }

        

        private void CarregarDados()
        {
            DataTable tabela = new DataTable();

            try
            {
                using (var conexao = DataBaseConnection.OpenConnection())
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"
                SELECT 
                    p.codProd AS Codigo,
                    DATE_FORMAT(p.dataDeEntrada, '%d/%m/%Y %H:%i') AS Data,
                    l.descricao AS Produto,
                    p.quantidade AS Qtd,
                    l.peso AS Peso,
                    (p.quantidade * l.peso / 1000) AS PesoTotal,
                    l.unidade AS Unidade,
                    o.nome AS Origem,
                    u.usuario AS Usuario,
                    p.dataDeValidade AS Validade,
                    CASE 
                        WHEN p.dataDeValidade < CURDATE() THEN 'VENCIDO'
                        WHEN DATEDIFF(p.dataDeValidade, CURDATE()) <= 7 THEN '7 DIAS'
                        WHEN DATEDIFF(p.dataDeValidade, CURDATE()) <= 15 THEN '15 DIAS'
                        WHEN DATEDIFF(p.dataDeValidade, CURDATE()) <= 30 THEN '30 DIAS'
                        ELSE 'OK'
                    END AS Status
                FROM tbProdutos p
                INNER JOIN tbLista l ON p.codList = l.codList
                INNER JOIN tbOrigemDoacao o ON o.codOri = p.codOri
                INNER JOIN tbUsuarios u ON u.codUsu = p.codUsu
                WHERE p.tipoMovimentacao = 'ENTRADA'
                AND p.quantidade > 0
                AND p.dataDeEntrada BETWEEN @ini AND @fim");

                    if (cbxProduto.SelectedItem != null && cbxProduto.SelectedItem.ToString() != "TODOS")
                    {
                        sql.AppendLine(" AND l.descricao = @produto");
                    }

                    if (cbbUsuario.SelectedItem != null && cbbUsuario.SelectedItem.ToString() != "TODOS")
                    {
                        sql.AppendLine(" AND u.usuario = @usuario");
                    }

                    if (cbxOrigem.SelectedItem != null && cbxOrigem.SelectedItem.ToString() != "TODOS")
                    {
                        sql.AppendLine(" AND o.nome = @origem");
                    }

                    sql.AppendLine(" ORDER BY p.dataDeEntrada DESC");

                    using (var cmd = new MySqlCommand(sql.ToString(), conexao))
                    {
                        DateTime dataInicial = dtpDataInicialPeriodo.Value.Date;
                        DateTime dataFinal = dtpDataFinalPeriodo.Value.Date.AddDays(1).AddSeconds(-1);

                        cmd.Parameters.AddWithValue("@ini", dataInicial);
                        cmd.Parameters.AddWithValue("@fim", dataFinal);

                        if (cbxProduto.SelectedItem != null && cbxProduto.SelectedItem.ToString() != "TODOS")
                        {
                            cmd.Parameters.AddWithValue("@produto", cbxProduto.SelectedItem.ToString());
                        }

                        if (cbbUsuario.SelectedItem != null && cbbUsuario.SelectedItem.ToString() != "TODOS")
                        {
                            cmd.Parameters.AddWithValue("@usuario", cbbUsuario.SelectedItem.ToString());
                        }

                        if (cbxOrigem.SelectedItem != null && cbxOrigem.SelectedItem.ToString() != "TODOS")
                        {
                            cmd.Parameters.AddWithValue("@origem", cbxOrigem.SelectedItem.ToString());
                        }

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(tabela);
                    }
                }

                // Formatar números - CORRIGIDO
                foreach (DataRow row in tabela.Rows)
                {
                    // Corrigir Peso
                    if (row["Peso"] != DBNull.Value)
                    {
                        string pesoStr = row["Peso"].ToString();
                        // Remove pontos e substitui vírgula
                        pesoStr = pesoStr.Replace(".", "").Replace(",", "");
                        if (int.TryParse(pesoStr, out int pesoInt))
                        {
                            row["Peso"] = pesoInt;
                        }
                        else
                        {
                            row["Peso"] = 0;
                        }
                    }

                    // Corrigir PesoTotal (remover "kg" e converter)
                    if (row["PesoTotal"] != DBNull.Value)
                    {
                        string pesoTotalStr = row["PesoTotal"].ToString();
                        // Remover "kg" se existir
                        pesoTotalStr = pesoTotalStr.Replace("kg", "").Replace("KG", "").Trim();
                        // Substituir vírgula por ponto
                        pesoTotalStr = pesoTotalStr.Replace(".", "").Replace(",", ".");

                        if (decimal.TryParse(pesoTotalStr, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out decimal pesoTotal))
                        {
                            row["PesoTotal"] = pesoTotal;
                        }
                        else
                        {
                            row["PesoTotal"] = 0;
                        }
                    }
                }

                AdicionarTotal(tabela);
                dgvRelatorios.DataSource = tabela;
                ConfigurarAlinhamentoColunas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar: " + ex.Message);
            }
        }

        private void ConfigurarAlinhamentoColunas()
        {
            if (dgvRelatorios.Columns.Count == 0) return;

            foreach (DataGridViewColumn col in dgvRelatorios.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (col.Name == "Codigo")
                {
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Data")
                {
                    col.FillWeight = 12;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Produto")
                {
                    col.FillWeight = 18;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (col.Name == "Qtd")
                {
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.Name == "Peso")
                {
                    col.FillWeight = 10;
                    col.HeaderText = "Peso (g)";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.Name == "PesoTotal")
                {
                    col.FillWeight = 12;
                    col.HeaderText = "Peso Total (kg)";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N2";
                }

                else if (col.Name == "Unidade")
                {
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (col.Name == "Origem")
                {
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (col.Name == "Usuario")
                {
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (col.Name == "Validade")
                {
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Status")
                {
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (col.Name == "Excluir")
                {
                    col.FillWeight = 6;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void CarregarOrigens()
        {
            if (cbxOrigem == null) return;
            cbxOrigem.Items.Clear();
            cbxOrigem.Items.Add("TODOS");

            using (var conexao = DataBaseConnection.OpenConnection())
            {
                string sql = "SELECT nome FROM tbOrigemDoacao ORDER BY nome";
                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbxOrigem.Items.Add(reader["nome"].ToString());
                    }
                }
            }
            cbxOrigem.SelectedIndex = 0;
        }

        private void CarregarUsuarios()
        {
            if (cbbUsuario == null) return;
            cbbUsuario.Items.Clear();
            cbbUsuario.Items.Add("TODOS");

            using (var conexao = DataBaseConnection.OpenConnection())
            {
                string sql = "SELECT usuario FROM tbUsuarios ORDER BY usuario";
                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbbUsuario.Items.Add(reader["usuario"].ToString());
                    }
                }
            }

            if (cbbUsuario.Items.Count == 0)
            {
                cbbUsuario.Items.Add("TODOS");
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
                    {
                        cbxProduto.Items.Add(reader["descricao"].ToString());
                    }
                }
            }
            cbxProduto.SelectedIndex = 0;
        }

        

        private void AdicionarTotal(DataTable tabela)
        {
            if (tabela.Rows.Count == 0) return;

            int totalQtd = 0;
            decimal totalPesoGramas = 0;

            foreach (DataRow row in tabela.Rows)
            {
                if (row["Produto"].ToString() == "❖ TOTAL DE ENTRADAS ❖") continue;

                int qtd = 0;
                if (row["Qtd"] != DBNull.Value)
                {
                    int.TryParse(row["Qtd"].ToString(), out qtd);
                }

                decimal peso = 0;
                if (row["Peso"] != DBNull.Value)
                {
                    // Converter corretamente
                    string pesoStr = row["Peso"].ToString();
                    pesoStr = pesoStr.Replace(".", "").Replace(",", "");
                    int.TryParse(pesoStr, out int pesoInt);
                    peso = pesoInt;
                }

                totalQtd += qtd;
                totalPesoGramas += (qtd * peso);
            }

            DataRow totalRow = tabela.NewRow();
            totalRow["Produto"] = "❖ TOTAL DE ENTRADAS ❖";
            totalRow["Qtd"] = totalQtd;
            totalRow["PesoTotal"] = (totalPesoGramas / 1000).ToString("N2");
            totalRow["Codigo"] = DBNull.Value;
            totalRow["Data"] = DBNull.Value;
            totalRow["Peso"] = DBNull.Value;
            totalRow["Unidade"] = DBNull.Value;
            totalRow["Origem"] = DBNull.Value;
            totalRow["Usuario"] = DBNull.Value;
            totalRow["Validade"] = DBNull.Value;
            totalRow["Status"] = DBNull.Value;

            tabela.Rows.Add(totalRow);
        }

        private void DgvRelatorios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRelatorios.Columns[e.ColumnIndex].Name == "Excluir")
            {
                if (dgvRelatorios.Rows[e.RowIndex].Cells["Produto"].Value?.ToString() == "❖ TOTAL DE ENTRADAS ❖")
                {
                    MessageBox.Show("Não é possível excluir a linha de total.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cod = Convert.ToInt32(dgvRelatorios.Rows[e.RowIndex].Cells["Codigo"].Value);

                if (MessageBox.Show("Excluir este registro?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (var conexao = DataBaseConnection.OpenConnection())
                        {
                            string sql = "DELETE FROM tbProdutos WHERE codProd = @cod";
                            var cmd = new MySqlCommand(sql, conexao);
                            cmd.Parameters.AddWithValue("@cod", cod);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro excluído com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CarregarDados();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnExportarMenu_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            var itemImprimir = new ToolStripMenuItem("🖨️ Imprimir");
            var itemExcel = new ToolStripMenuItem("📊 Exportar para Excel");
            var itemCSV = new ToolStripMenuItem("📄 Exportar para CSV (Power BI)");

            itemImprimir.Click += (s, ev) => Imprimir();
            itemExcel.Click += (s, ev) => ExportarExcel();
            itemCSV.Click += (s, ev) => ExportarCSV();

            menu.Items.AddRange(new ToolStripItem[] { itemImprimir, itemExcel, itemCSV });

            menu.Show(btnImprimir, new Point(0, btnImprimir.Height));
        }

        private void ExportarExcel()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Arquivos Excel (*.xlsx)|*.xlsx",
                    FileName = $"Relatorio_Entradas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (sfd.ShowDialog() != DialogResult.OK) return;

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Relatorio_Entradas");

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
                            worksheet.Cell(i + 2, j + 1).Value = dgvRelatorios.Rows[i].Cells[j].Value?.ToString() ?? "";
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(sfd.FileName);
                }

                MessageBox.Show("✅ Relatório exportado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarCSV()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Arquivos CSV (*.csv)|*.csv",
                    FileName = $"Relatorio_Entradas_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() != DialogResult.OK) return;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < dgvRelatorios.Columns.Count; i++)
                {
                    sb.Append($"\"{dgvRelatorios.Columns[i].HeaderText}\"");
                    if (i < dgvRelatorios.Columns.Count - 1)
                        sb.Append(";");
                }
                sb.AppendLine();

                foreach (DataGridViewRow row in dgvRelatorios.Rows)
                {
                    if (row.IsNewRow) continue;

                    for (int i = 0; i < dgvRelatorios.Columns.Count; i++)
                    {
                        string valor = row.Cells[i].Value?.ToString() ?? "";
                        sb.Append($"\"{valor.Replace("\"", "\"\"")}\"");
                        if (i < dgvRelatorios.Columns.Count - 1)
                            sb.Append(";");
                    }
                    sb.AppendLine();
                }

                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"✅ Relatório exportado com sucesso!\n\nLocal: {sfd.FileName}\n\nEste arquivo pode ser importado no Power BI.",
                    "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao exportar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Imprimir()
        {
            if (dgvRelatorios.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para imprimir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            linhaAtual = 0;
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPage;

            PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 1000,
                Height = 800,
                WindowState = FormWindowState.Maximized
            };
            preview.ShowDialog();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Font tituloFont = new Font("Segoe UI", 14, FontStyle.Bold);
            Font subtituloFont = new Font("Segoe UI", 10, FontStyle.Regular);
            Font cabecalhoFont = new Font("Segoe UI", 9, FontStyle.Bold);
            Font textoFont = new Font("Segoe UI", 8, FontStyle.Regular);

            float yPos = e.MarginBounds.Top;
            float leftMargin = e.MarginBounds.Left;
            float pageWidth = e.MarginBounds.Width;

            e.Graphics.DrawString("RELATÓRIO DE ENTRADAS - ESTOQUE", tituloFont, Brushes.Black, leftMargin, yPos);
            yPos += 35;

            e.Graphics.DrawString($"Período: {dtpDataInicialPeriodo.Value:dd/MM/yyyy} a {dtpDataFinalPeriodo.Value:dd/MM/yyyy}",
                subtituloFont, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            e.Graphics.DrawString($"Total de registros: {dgvRelatorios.Rows.Count - 1} entradas",
                subtituloFont, Brushes.Black, leftMargin, yPos);
            yPos += 30;

            float colWidth = pageWidth / dgvRelatorios.Columns.Count;
            float colX = leftMargin;

            e.Graphics.FillRectangle(Brushes.LightGray, leftMargin, yPos, pageWidth, 22);

            for (int i = 0; i < dgvRelatorios.Columns.Count; i++)
            {
                e.Graphics.DrawString(dgvRelatorios.Columns[i].HeaderText, cabecalhoFont, Brushes.Black,
                    colX + 3, yPos + 3);
                colX += colWidth;
            }

            yPos += 25;

            for (int i = linhaAtual; i < dgvRelatorios.Rows.Count; i++)
            {
                DataGridViewRow row = dgvRelatorios.Rows[i];
                if (row.IsNewRow) continue;

                colX = leftMargin;

                if (yPos + 22 > e.MarginBounds.Bottom)
                {
                    linhaAtual = i;
                    e.HasMorePages = true;
                    return;
                }

                bool isTotal = row.Cells["Produto"].Value?.ToString() == "❖ TOTAL DE ENTRADAS ❖";
                if (isTotal)
                {
                    e.Graphics.FillRectangle(Brushes.DarkGray, leftMargin, yPos, pageWidth, 22);
                }

                for (int j = 0; j < dgvRelatorios.Columns.Count; j++)
                {
                    string valor = row.Cells[j].Value?.ToString() ?? "";
                    e.Graphics.DrawString(valor, textoFont, isTotal ? Brushes.White : Brushes.Black,
                        colX + 3, yPos + 3);
                    colX += colWidth;
                }

                yPos += 22;
            }

            linhaAtual = 0;
            e.HasMorePages = false;

            e.Graphics.DrawString($"Emissão: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                subtituloFont, Brushes.Gray, leftMargin, yPos + 10);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            new frmMenuPrincipal(_codUsuLogado).Show();
            this.Close();
        }
    }
}