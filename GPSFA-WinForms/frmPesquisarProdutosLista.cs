using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmPesquisarProdutosLista : Form
    {
        public frmPesquisarProdutosLista()
        {
            InitializeComponent();
        }

        public frmPesquisarProdutosLista(int codUsu)
        {
            InitializeComponent();
            codUsuLogado = codUsu;
            desabilitarBotoes();
        }

        int codUsuLogado;

        private void desabilitarBotoes()
        {
            btnPesquisar.Enabled = false;
            btnLimpar.Enabled = false;
        }

        private void habilitarBotoes()
        {
            btnPesquisar.Enabled = true;
            btnLimpar.Enabled = true;
        }

        private void frmPesquisarProdutosLista_Load(object sender, EventArgs e)
        {
            CarregarDadosNaListaDeProdutos();
            ConfigDgvListaDeProdutos();
            this.dgvListaDeProdutos.AllowUserToAddRows = false;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmListaProdutos abrir = new frmListaProdutos(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void CarregarDadosNaListaDeProdutos()
        {
            dgvListaDeProdutos.Columns.Clear();

            DataTable tabela = new DataTable();

            using (MySqlConnection conexao = DataBaseConnection.OpenConnection())
            {
                StringBuilder query = new StringBuilder();

                query.Append("SELECT p.codList AS 'Código', p.descricao as 'Nome do produto' FROM tblista AS p;");

                MySqlCommand comm = new MySqlCommand();
                comm.Connection = conexao;

                comm.CommandText = query.ToString();

                MySqlDataAdapter DA = new MySqlDataAdapter(comm);
                DA.Fill(tabela);

                dgvListaDeProdutos.DataSource = tabela;

                dgvListaDeProdutos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DataBaseConnection.CloseConnection();
            }
        }

        public void buscarVoluntarioPorDescricao(string descricao)
        {
            dgvListaDeProdutos.Columns.Clear();

            DataTable tabela = new DataTable();

            using (MySqlConnection conexao = DataBaseConnection.OpenConnection())
            {
                StringBuilder query = new StringBuilder();

                query.Append("SELECT p.codList AS 'Código', p.descricao as 'Nome do produto' FROM tblista AS p WHERE UPPER(p.descricao) LIKE UPPER(@descricao) OR p.peso LIKE @descricao OR p.cod LIKE @descricao;");

                MySqlCommand comm = new MySqlCommand();
                comm.Connection = conexao;

                comm.CommandText = query.ToString();

                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@descricao", "%" + descricao + "%");

                MySqlDataAdapter DA = new MySqlDataAdapter(comm);
                DA.Fill(tabela);

                dgvListaDeProdutos.DataSource = tabela;

                dgvListaDeProdutos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                DataBaseConnection.CloseConnection();
            }
        }

        private void ConfigDgvListaDeProdutos()
        { // Ajustar para ocupar toda a largura
            dgvListaDeProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Alternar cores das linhas
            dgvListaDeProdutos.RowsDefaultCellStyle.BackColor = Color.LightGray;
            // Aumentar fonte
            dgvListaDeProdutos.RowsDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Regular);
            dgvListaDeProdutos.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            //// Ajustar altura das linhas
            dgvListaDeProdutos.RowTemplate.Height = 40;
            //// Habilitar quebra de texto
            dgvListaDeProdutos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //// Ajustar seleção de célula
            dgvListaDeProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvListaDeProdutos.MultiSelect = false;

            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Action";
            buttonColumn.Name = "EditarDados"; // Name for programmatic reference
            buttonColumn.Text = "Editar"; // The text displayed on the button
            buttonColumn.UseColumnTextForButtonValue = true; // Use the Text property value for all buttons
            dgvListaDeProdutos.Columns.Add(buttonColumn);
        }

        private void dgvListaDeProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked column is the button column
            if (e.ColumnIndex == dgvListaDeProdutos.Columns["EditarDados"].Index && e.RowIndex >= 0)
            {
                // Perform action for the clicked button
                int codList = Convert.ToInt32(dgvListaDeProdutos.Rows[e.RowIndex].Cells[0].Value);
                frmListaProdutos abrir = new frmListaProdutos(codUsuLogado, codList);
                abrir.Show();
                this.Close();
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (txtDescricao.Text.Equals(""))
            {
                MessageBox.Show("Favor inserir um valor",
                    "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                txtDescricao.Focus();
            }
            else
            {
                buscarVoluntarioPorDescricao(txtDescricao.Text);
                ConfigDgvListaDeProdutos();
            }
        }

        private void txtDescricao_TextChanged(object sender, EventArgs e)
        {
            habilitarBotoes();
        }
    }
}
