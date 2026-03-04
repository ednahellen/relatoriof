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
    public partial class frmLogin : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        // Implementar função de 'lembre-se de mim' somente com o uso de API + Token persistente - não seria o mais seguro e viável nesse primeiro momento

        public frmLogin()
        {
            InitializeComponent();
        }

        //Criando método de limpar campos
        public void limparCampos()
        {
            txtUsuario.Clear();
            txtSenha.Clear();
            txtUsuario.Focus();
        }

        int codUsuLogado;
        bool usuarioAtivo;
        string tipoAcesso;

        bool resp = false;

        //Criando método para autenticação do Usúario 
        public int acessaUsuario(string usuario, string senha)
        {
            int resp;
            try 
            {
                MySqlCommand comm = new MySqlCommand();
                comm.CommandText = "SELECT codUsu, ativo, tipo FROM tbUsuarios where usuario=@usuario and senha=@senha;";
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@usuario", MySqlDbType.VarChar, 100).Value = usuario;
                comm.Parameters.Add("@senha", MySqlDbType.VarChar, 100).Value = senha;

                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    if (DR.Read())
                    {
                        codUsuLogado = DR.GetInt32(0);
                        usuarioAtivo = DR.GetBoolean(1);
                        tipoAcesso = DR.GetString(2);

                        DataBaseConnection.CloseConnection();

                        return resp = 1;
                    }
                    else 
                    {
                        MessageBox.Show($"Erro ao autenticar usuário.", "Mensagem do sistema",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);

                        DataBaseConnection.CloseConnection();
                        return resp = 0;
                    }
                }
            }
            catch (Exception)
            {
                DataBaseConnection.CloseConnection();
                return resp = 0;
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string usuario, senha;

            usuario = txtUsuario.Text;
            senha = txtSenha.Text;

            if (acessaUsuario(usuario, senha) == 1)
            {
                if (usuarioAtivo)
                {
                    frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
                    abrir.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Acesso negado! O usuário informado se encontra desativado.", "Mensagem do sistema",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    limparCampos();
                }
            }
            else
            {
                MessageBox.Show("Usuário ou senha incorretos.", "Mensagem do sistema",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error,
                   MessageBoxDefaultButton.Button1);
                limparCampos();
                    
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Deseja sair do sistema?", "Mensagem do sistema",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }

            else
            {
                txtUsuario.Focus();
                return;
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
