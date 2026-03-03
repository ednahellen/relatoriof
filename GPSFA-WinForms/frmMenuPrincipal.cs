using MySql.Data.MySqlClient;
using Projeto_Socorrista;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace GPSFA_WinForms
{
    public partial class frmMenuPrincipal : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmMenuPrincipal()
        {
            InitializeComponent();            
        }

        // Variáveis globais da janela para salvar o usuário atualmente logado e seu tipo de acesso
        int codUsuLogado; 
        string tipoAcessoUsuLogado;

        // Instancia da janela para receber o código do usuário loado
        public frmMenuPrincipal(int codUsu)
        {
            InitializeComponent();
            codUsuLogado = codUsu;
            buscaDoTipoDeAcessoDoUsuario(codUsu);
        }

        // Criando método para buscar o tipo de acesso do usuário e salvar o resultado em uma variável global
        public int buscaDoTipoDeAcessoDoUsuario(int codUsu)
        {
            int resp;
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT tipo FROM tbUsuarios WHERE codUsu = @codUsu;";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@codUsu", MySqlDbType.Int32).Value = codUsu;

            comm.Connection = DataBaseConnection.OpenConnection();

            using (MySqlDataReader DR = comm.ExecuteReader())
            {
                if (DR.Read())
                {
                    try
                    {
                        tipoAcessoUsuLogado = DR.GetString(0);

                        DataBaseConnection.CloseConnection();

                        return resp = 1;
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show($"Erro ao validar tipo de acesso do usuário! Erro:\n\n{error}", "Mensagem do sistema",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);

                        DataBaseConnection.CloseConnection();
                    }
                }
            }
            return resp = 0;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            //    ActiveButton(btnDashboard);
            //    FormShow(new frmDashboard());

            frmDashboard abrir = new frmDashboard(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        private void btnVoluntarios_Click(object sender, EventArgs e)
        {
            //ActiveButton(btnVoluntarios);
            //FormShow(new frmGestaoDeVoluntarios());

            if (tipoAcessoUsuLogado.Equals("ADMIN")) // validação simples para limitar o acesso do usuário
            {
                frmVoluntarios abrir = new frmVoluntarios(codUsuLogado);

                abrir.Show();
                this.Hide();
            }
            else {
                MessageBox.Show("Acesso negado!\n\nVocê precisa ser um administrador para acessar esta função.", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }


        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            //ActiveButton(btnRelatorios);
            //FormShow(new frmRelatorios());

            frmRelatorios abrir = new frmRelatorios(codUsuLogado);

            abrir.Show();
            this.Hide();
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            //ActiveButton(btnAlimentos);
            //FormShow(new frmEstoque());

            frmEstoque abrir = new frmEstoque(codUsuLogado);

            abrir.Show();
            this.Hide();

        }    

        private void frmMenuPrincipal_Load(object sender, EventArgs e)
        {
            //if (tipoAcessoUsuLogado.Equals("USER"))
            //{
            //    btnVoluntarios.Enabled = false;
            //}
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
                return;
            }
        }

        private void btnGerenciarProdutos_Click(object sender, EventArgs e)
        {        
            frmTipoDeArrecadacao abrir = new frmTipoDeArrecadacao(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        private void btnCestas_Click(object sender, EventArgs e)
        {
            frmCestas abrir = new frmCestas(codUsuLogado);
            abrir.Show();
            this.Close();
        }
    }
}
