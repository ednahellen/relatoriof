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
    public partial class frmPesquisarVoluntario : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmPesquisarVoluntario()
        {
            InitializeComponent();
            btnPesquisarVoluntario.Enabled = false;
            ltbPesquisarVoluntarios.Enabled = false;
            btnLimpar.Enabled = false;
        }

        public void buscarVoluntarios(string descricao)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT * FROM tbVoluntarios WHERE nome LIKE @descricao OR cpf LIKE @descricao;";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Clear();

            comm.Parameters.Add("@descricao", MySqlDbType.VarChar, 100).Value = descricao;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            ltbPesquisarVoluntarios.Items.Clear();

            if (DR.HasRows == false)
            {
                MessageBox.Show("Nenhum voluntário encontrado.",
                    "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtDescricao.Clear();
                txtDescricao.Focus();
            }
            else
            {
                while (DR.Read())
                {
                    ltbPesquisarVoluntarios.Items.Add(DR.GetString(1));
                }
            }
            DataBaseConnection.CloseConnection();
        }

        private void btnPesquisarVoluntario_Click(object sender, EventArgs e)
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
                buscarVoluntarios(txtDescricao.Text);
                ltbPesquisarVoluntarios.Enabled = true;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDescricao.Clear();
            txtDescricao.Focus();
            btnPesquisarVoluntario.Enabled = false;
            btnLimpar.Enabled = false;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmVoluntarios abrir = new frmVoluntarios();
            abrir.Show();
            this.Close();
        }

        private void frmPesquisarVoluntario_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);
        }

        private void ltbPesquisarVoluntarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            string descricao = ltbPesquisarVoluntarios.SelectedItem.ToString();

            frmVoluntarios abrir = new frmVoluntarios(descricao);
            abrir.Show();
            this.Hide();
        }

        private void txtDescricao_TextChanged(object sender, EventArgs e)
        {
            btnPesquisarVoluntario.Enabled = true;
            btnLimpar.Enabled = true;
            if (txtDescricao.Text.Equals(""))
            {
                btnPesquisarVoluntario.Enabled = false;
                btnLimpar.Enabled = false;
            }
        }
    }
}
