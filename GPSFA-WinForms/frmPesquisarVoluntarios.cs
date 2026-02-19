using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmPesquisarVoluntarios : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmPesquisarVoluntarios()
        {
            InitializeComponent();
            desabilitarBotoes();
        }

        int codUsuLogado;

        public frmPesquisarVoluntarios(int codUsu)
        {
            codUsuLogado = codUsu;
            InitializeComponent();
            desabilitarBotoes();
        }

        private void frmListaVoluntarios_Load(object sender, EventArgs e)
        {
            CarregarDadosNaListaDeVoluntarios();
            ConfigDgvVoluntarios(); 
            this.dgvVoluntarios.AllowUserToAddRows = false;
        }

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

        private void ConfigDgvVoluntarios()
        { // Ajustar para ocupar toda a largura
            dgvVoluntarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Alternar cores das linhas
            dgvVoluntarios.RowsDefaultCellStyle.BackColor = Color.LightGray;
            // Aumentar fonte
            dgvVoluntarios.RowsDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Regular);
            dgvVoluntarios.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            //// Ajustar altura das linhas
            dgvVoluntarios.RowTemplate.Height = 40;
            //// Habilitar quebra de texto
            dgvVoluntarios.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //// Ajustar seleção de célula
            dgvVoluntarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvVoluntarios.MultiSelect = false; 
            
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Action";
            buttonColumn.Name = "EditarDados"; // Name for programmatic reference
            buttonColumn.Text = "Editar"; // The text displayed on the button
            buttonColumn.UseColumnTextForButtonValue = true; // Use the Text property value for all buttons
            dgvVoluntarios.Columns.Add(buttonColumn);
        }

        public void buscarVoluntarioPorDescricao(string descricao)
        {
            dgvVoluntarios.Columns.Clear();

            DataTable tabela = new DataTable();

            using (MySqlConnection conexao = DataBaseConnection.OpenConnection())
            {
                StringBuilder query = new StringBuilder();

                query.Append("SELECT v.nome, v.cpf, v.telCel, CASE WHEN v.ativo = 1 THEN 'Sim' ELSE 'Não' END AS ativo FROM tbVoluntarios AS v LEFT JOIN tbUsuarios AS u ON u.codVol = v.codVol WHERE LOWER(v.nome) LIKE LOWER(@descricao) OR v.cpf LIKE @descricao OR v.telCel LIKE @descricao OR LOWER(u.usuario) LIKE LOWER(@descricao);");

                MySqlCommand comm = new MySqlCommand();
                comm.Connection = conexao;

                comm.CommandText = query.ToString();

                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@descricao", "%" + descricao + "%");

                MySqlDataAdapter DA = new MySqlDataAdapter(comm);
                DA.Fill(tabela);

                dgvVoluntarios.DataSource = tabela;

                dgvVoluntarios.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                DataBaseConnection.CloseConnection();
            }
        }

        private void dgvVoluntarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked column is the button column
            if (e.ColumnIndex == dgvVoluntarios.Columns["EditarDados"].Index && e.RowIndex >= 0)
            {
                // Perform action for the clicked button
                int codVol = Convert.ToInt32(dgvVoluntarios.Rows[e.RowIndex].Cells[0].Value);
                frmVoluntarios abrir = new frmVoluntarios(codVol, codUsuLogado);
                abrir.Show();
                this.Close();
            }
        }

        private void CarregarDadosNaListaDeVoluntarios()
        {
            dgvVoluntarios.Columns.Clear();

            DataTable tabela = new DataTable();

            using (MySqlConnection conexao = DataBaseConnection.OpenConnection())
            {
                StringBuilder query = new StringBuilder();

                query.Append("SELECT u.codVol, u.usuario, v.nome, v.cpf, v.telCel, CASE WHEN v.ativo = 1 THEN 'Sim' ELSE 'Não' END AS ativo FROM tbVoluntarios AS v LEFT JOIN tbUsuarios AS u ON u.codVol = v.codVol;");

                MySqlCommand comm = new MySqlCommand();
                comm.Connection = conexao;

                comm.CommandText = query.ToString();

                MySqlDataAdapter DA = new MySqlDataAdapter(comm);
                    DA.Fill(tabela);

                dgvVoluntarios.DataSource = tabela;

                dgvVoluntarios.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                DataBaseConnection.CloseConnection();
            }
        }


        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmVoluntarios abrir = new frmVoluntarios(codUsuLogado);
            abrir.Show();
            this.Close();
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
                ConfigDgvVoluntarios();
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDescricao.Clear();
            CarregarDadosNaListaDeVoluntarios();
            desabilitarBotoes();
            ConfigDgvVoluntarios();
        }

        private void txtDescricao_TextChanged(object sender, EventArgs e)
        {
            habilitarBotoes();
        }
    }
}
