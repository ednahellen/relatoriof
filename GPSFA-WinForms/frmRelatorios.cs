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
            ConfigurarGrid();

            dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
            dtpDataFinalPeriodo.Value = DateTime.Now;

            CarregarDados();
        }

        private void ConfigurarEventos()
        {
            btnPesquisar.Click += (s, e) => CarregarDados();
            btnLimparFiltros.Click += (s, e) =>
            {
                dtpDataInicialPeriodo.Value = DateTime.Now.AddDays(-30);
                dtpDataFinalPeriodo.Value = DateTime.Now;
                CarregarDados();
            };

            btnMenu.Click += btnMenu_Click;
            btnImprimir.Click += BtnExportarMenu_Click;
        }

        private void ConfigurarGrid()
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
                    string sql = @"
                                    SELECT 
                                        p.codProd AS Codigo,
                                        DATE_FORMAT(p.dataDeEntrada, '%d/%m/%Y %H:%i') AS Data,
                                        l.descricao AS Produto,
                                        p.quantidade AS Qtd,
                                        o.nome AS Origem,
                                        u.usuario AS Usuario
                                    FROM tbProdutos p
                                    INNER JOIN tbLista l ON p.codList = l.codList
                                    INNER JOIN tbOrigemDoacao o ON o.codOri = p.codOri
                                    INNER JOIN tbUsuarios u ON u.codUsu = p.codUsu

                                    WHERE p.tipoMovimentacao = 'ENTRADA'
                                    AND p.quantidade > 0
                                    AND p.dataDeEntrada BETWEEN @ini AND @fim

                                    ORDER BY p.dataDeEntrada DESC";


                    using (var cmd = new MySqlCommand(sql, conexao))
                    {

                        DateTime dataInicial = dtpDataInicialPeriodo.Value.Date;
                        DateTime dataFinal = dtpDataFinalPeriodo.Value.Date.AddDays(1).AddSeconds(-1);

                        cmd.Parameters.AddWithValue("@ini", dataInicial);
                        cmd.Parameters.AddWithValue("@fim", dataFinal);

                        cmd.Parameters.AddWithValue("@produto", cbxProduto.SelectedItem?.ToString() ?? "TODOS");
                        cmd.Parameters.AddWithValue("@usuario", cbbUsuario.SelectedItem?.ToString() ?? "TODOS");
                        cmd.Parameters.AddWithValue("@tipo", cbxStatus.SelectedItem?.ToString() ?? "TODOS");

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(tabela);
                    }
                }

                AdicionarTotal(tabela);
                dgvRelatorios.DataSource = tabela;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar: " + ex.Message);
            }
        }

        private void AdicionarTotal(DataTable tabela)
        {
            if (tabela.Rows.Count == 0) return;

            int total = 0;

            foreach (DataRow row in tabela.Rows)
            {
                if (row["Qtd"] != DBNull.Value)
                    total += Convert.ToInt32(row["Qtd"]);
            }

            DataRow totalRow = tabela.NewRow();
            totalRow["Produto"] = "❖ TOTAL DE ENTRADAS ❖";
            totalRow["Qtd"] = total;

            tabela.Rows.Add(totalRow);
        }

        private void DgvRelatorios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRelatorios.Columns[e.ColumnIndex].Name == "Excluir")
            {
                int cod = Convert.ToInt32(dgvRelatorios.Rows[e.RowIndex].Cells["Código"].Value);

                if (MessageBox.Show("Excluir?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var conexao = DataBaseConnection.OpenConnection())
                    {
                        string sql = "DELETE FROM tbProdutos WHERE codProd = @cod";
                        var cmd = new MySqlCommand(sql, conexao);
                        cmd.Parameters.AddWithValue("@cod", cod);
                        cmd.ExecuteNonQuery();
                    }

                    CarregarDados();
                }
            }
        }

        private void BtnExportarMenu_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            var itemImprimir = new ToolStripMenuItem("🖨️ Imprimir");
            var itemExcel = new ToolStripMenuItem("📊 Excel");
            var itemCSV = new ToolStripMenuItem("📄 CSV (Power BI)");

            itemImprimir.Click += (s, ev) => Imprimir();
            itemExcel.Click += (s, ev) => ExportarExcel();
            itemCSV.Click += (s, ev) => ExportarCSV();

            menu.Items.AddRange(new ToolStripItem[] { itemImprimir, itemExcel, itemCSV });

            menu.Show(btnImprimir, new Point(0, btnImprimir.Height));
        }

        private void ExportarExcel()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Relatorio");

            for (int i = 0; i < dgvRelatorios.Columns.Count; i++)
                ws.Cell(1, i + 1).Value = dgvRelatorios.Columns[i].HeaderText;

            for (int i = 0; i < dgvRelatorios.Rows.Count; i++)
                for (int j = 0; j < dgvRelatorios.Columns.Count; j++)
                    ws.Cell(i + 2, j + 1).Value = dgvRelatorios.Rows[i].Cells[j].Value?.ToString();

            wb.SaveAs(sfd.FileName);
        }

        private void ExportarCSV()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dgvRelatorios.Columns.Count; i++)
                sb.Append(dgvRelatorios.Columns[i].HeaderText + ";");

            sb.AppendLine();

            foreach (DataGridViewRow row in dgvRelatorios.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                    sb.Append((cell.Value ?? "").ToString() + ";");

                sb.AppendLine();
            }

            System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
        }

        private void Imprimir()
        {
            linhaAtual = 0;

            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPage;

            new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 1000,
                Height = 800
            }.ShowDialog();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            int y = 50;

            foreach (DataGridViewRow row in dgvRelatorios.Rows)
            {
                int x = 50;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    e.Graphics.DrawString(cell.Value?.ToString(), new Font("Arial", 9), Brushes.Black, x, y);
                    x += 120;
                }

                y += 25;

                if (y > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            new frmMenuPrincipal(_codUsuLogado).Show();
            this.Close();
        }
    }
}
