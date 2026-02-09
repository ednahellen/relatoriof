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
    public partial class frmListaVoluntarios : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmListaVoluntarios()
        {
            InitializeComponent();
        }

        private void frmListaVoluntarios_Load(object sender, EventArgs e)
        {
            CarregarDadosNaListaDeVoluntarios();
            ConfigDgvVoluntarios();
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

            dgvVoluntarios.MultiSelect = false; DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Action";
            buttonColumn.Name = "EditarDados"; // Name for programmatic reference
            buttonColumn.Text = "Editar"; // The text displayed on the button
            buttonColumn.UseColumnTextForButtonValue = true; // Use the Text property value for all buttons
            dgvVoluntarios.Columns.Add(buttonColumn);
        }

        private void dgvVoluntarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked column is the button column
            if (e.ColumnIndex == dgvVoluntarios.Columns["EditarDados"].Index && e.RowIndex >= 0)
            {
                // Perform action for the clicked button
                string codVol = dgvVoluntarios.Rows[e.RowIndex].Cells[0].Value.ToString();
                frmVoluntarios abrir = new frmVoluntarios(codVol);
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

                query.Append("SELECT codVol, nome, cpf, telCel FROM tbVoluntarios;");

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
            frmVoluntarios abrir = new frmVoluntarios();
            abrir.Show();
            this.Close();
        }
    }
}
