using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using Projeto_Socorrista;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmRelatorios : Form
    {
        public frmRelatorios()
        {
            InitializeComponent();
        }

        int codUsuLogado;

        public frmRelatorios(int codUsu)
        {
            InitializeComponent();
            codUsuLogado = codUsu;
        }

        #region LOAD

        private void frmRelatorios_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CarregarUsuarios();
            CarregarTodosProdutos();

            dtpDataInicialPeriodo.Value = DateTime.Now.AddMonths(-1);
            dtpDataFinalPeriodo.Value = DateTime.Today;

            btnExportarExcel.Enabled = false;
        }

        #endregion

        #region CONFIGURAÇÕES

        private void ConfigurarGrid()
        {
            dgvProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProdutos.MultiSelect = false;
            dgvProdutos.ReadOnly = true;
        }

        #endregion

        #region CONSULTAS

        private void CarregarTodosProdutos()
        {
            string sql = BaseQuery() + " ORDER BY prod.dataDeEntrada DESC;";

            using (var conn = DataBaseConnection.OpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProdutos.DataSource = dt;
            }

            btnExportarExcel.Enabled = dgvProdutos.Rows.Count > 0;
        }

        private void BuscarRelatorio(DateTime dataInicial, DateTime dataFinal, string usuario)
        {
            StringBuilder sql = new StringBuilder(BaseQuery());
            sql.Append(" WHERE prod.dataDeEntrada BETWEEN @dataInicial AND @dataFinal ");

            if (!string.IsNullOrWhiteSpace(usuario))
                sql.Append(" AND vol.nome LIKE @usuario ");

            sql.Append(" ORDER BY prod.dataDeEntrada DESC;");

            using (var conn = DataBaseConnection.OpenConnection())
            using (var cmd = new MySqlCommand(sql.ToString(), conn))
            {
                cmd.Parameters.AddWithValue("@dataInicial", dataInicial);
                cmd.Parameters.AddWithValue("@dataFinal", dataFinal);

                if (!string.IsNullOrWhiteSpace(usuario))
                    cmd.Parameters.AddWithValue("@usuario", "%" + usuario + "%");

                using (var da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProdutos.DataSource = dt;
                }
            }

            btnExportarExcel.Enabled = dgvProdutos.Rows.Count > 0;
        }

        private string BaseQuery()
        {
            return @"SELECT 
                        prod.descricao AS 'Descrição do Produto',
                        prod.quantidade AS 'Quantidade',
                        CONCAT(prod.peso,' ', prod.unidade) AS 'Peso',
                        prod.dataDeEntrada AS 'Data de Cadastro',
                        prod.dataDeValidade AS 'Data de Validade',
                        vol.nome AS 'Quem Cadastrou'
                     FROM tbprodutos AS prod
                     INNER JOIN tbUsuarios AS usr ON prod.codUsu = usr.codUsu
                     INNER JOIN tbVoluntarios AS vol ON usr.codVol = vol.codVol";
        }

        #endregion

        #region FILTROS

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            BuscarRelatorio(
                dtpDataInicialPeriodo.Value.Date,
                dtpDataFinalPeriodo.Value.Date,
                cbbUsuario.Text
            );
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            dtpDataInicialPeriodo.Value = DateTime.Now.AddMonths(-1);
            dtpDataFinalPeriodo.Value = DateTime.Today;
            cbbUsuario.SelectedIndex = -1;

            CarregarTodosProdutos();
        }

        #endregion

        #region USUÁRIOS

        private void CarregarUsuarios()
        {
            using (var conn = DataBaseConnection.OpenConnection())
            using (var cmd = new MySqlCommand("SELECT nome FROM tbVoluntarios ORDER BY nome;", conn))
            using (var reader = cmd.ExecuteReader())
            {
                cbbUsuario.Items.Clear();

                while (reader.Read())
                    cbbUsuario.Items.Add(reader.GetString("nome"));
            }
        }

        #endregion

        #region EXPORTAÇÃO

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvProdutos.Rows.Count == 0)
                return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo Excel (*.xlsx)|*.xlsx";
                sfd.FileName = "Relatorio.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Relatório");

                    for (int i = 0; i < dgvProdutos.Columns.Count; i++)
                        ws.Cell(1, i + 1).Value = dgvProdutos.Columns[i].HeaderText;

                    for (int i = 0; i < dgvProdutos.Rows.Count; i++)
                        for (int j = 0; j < dgvProdutos.Columns.Count; j++)
                            ws.Cell(i + 2, j + 1).Value =
                                dgvProdutos.Rows[i].Cells[j].Value?.ToString();

                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Relatório exportado com sucesso.");
            }
        }

        #endregion

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado);
            abrir.Show();
            this.Hide();
        }
    }
}