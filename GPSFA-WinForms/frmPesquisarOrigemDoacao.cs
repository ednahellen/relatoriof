using CpfLibrary;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmPesquisarOrigemDoacao : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);
        public frmPesquisarOrigemDoacao()
        {
            InitializeComponent();
            btnPesquisarOrigem.Enabled = false;
            ltbPesquisarOrigem.Enabled = false;
            btnLimpar.Enabled = false;
        }

        private void frmPesquisarOrigemDoacao_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);
        }

        string cpfconsulta = "";
        string cnpjconsulta = "";
        string cepconsulta = "";
        string ruaconsulta = "";
        string numeroconsulta = "";
        string complementoconsulta = "";
        string bairroconsulta = "";
        string cidadeconsulta = "";
        string estadoconsulta = "";
        string telCelconsulta = "";
        string referenciaconsulta = ""; 

        public void buscaOrigemDoacao(string nome)
        {

            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT nome, cpf, cnpj, cep, rua, numero, complemento, bairro, cidade, estado, telCel, referencia FROM tborigemdoacao WHERE nome LIKE '%{nome}%';";

            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            ltbPesquisarOrigem.Items.Clear();

            if (DR.HasRows == false)
            {
                MessageBox.Show("Nenhuma origem encontrada com este valor.",
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
                    ltbPesquisarOrigem.Items.Add(DR.GetString(0));

                    try
                    {
                        cpfconsulta = DR.GetString(1);
                    }
                    catch (Exception) 
                    {
                        cpfconsulta = "";
                    }
                    try
                    {
                        cnpjconsulta = DR.GetString(2);
                    }    
                    catch (Exception)
                    {
                        cnpjconsulta = "";
                    }                  

                        cepconsulta = DR.GetString(3);
                        ruaconsulta = DR.GetString(4);
                        numeroconsulta = DR.GetString(5);
                        complementoconsulta = DR.GetString(6);
                        bairroconsulta = DR.GetString(7);
                        cidadeconsulta = DR.GetString(8);
                        estadoconsulta = DR.GetString(9);
                        telCelconsulta = DR.GetString(10);
                        referenciaconsulta = DR.GetString(11);
                        
                }
                
                
            }
            DataBaseConnection.CloseConnection();
        }        

        private void btnPesquisarOrigem_Click(object sender, EventArgs e)
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
                buscaOrigemDoacao(txtDescricao.Text);
                ltbPesquisarOrigem.Enabled = true;
                txtDescricao.Clear();
                txtDescricao.Enabled = false;
                btnPesquisarOrigem.Enabled = false;
                btnLimpar.Enabled = true;
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmOrigemDoacao abrir = new frmOrigemDoacao();
            abrir.Show();
            this.Hide();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            
            ltbPesquisarOrigem.Items.Clear();
            btnPesquisarOrigem.Enabled = false;
            btnLimpar.Enabled = false;
            txtDescricao.Enabled = true;
            txtDescricao.Clear();
            txtDescricao.Focus();
        }

        private void txtDescricao_TextChanged(object sender, EventArgs e)
        {
            btnPesquisarOrigem.Enabled = true;
            btnLimpar.Enabled = true;
            if (txtDescricao.Text.Equals(""))
            {
                btnPesquisarOrigem.Enabled = false;
                btnLimpar.Enabled = false;
            }
        }     
        
        private void ltbPesquisarOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {            
            
                string nome = ltbPesquisarOrigem.SelectedItem.ToString();
                string cpf = cpfconsulta;
                string cnpj = cnpjconsulta;
                string cep = cepconsulta;
                string rua = ruaconsulta;
                string numero = numeroconsulta;
                string complemento = complementoconsulta;
                string bairro = bairroconsulta;
                string cidade = cidadeconsulta;
                string estado = estadoconsulta;
                string telCel = telCelconsulta;
                string referencia = referenciaconsulta;

                frmOrigemDoacao abrir = new frmOrigemDoacao(nome, cpf, cnpj, cep, rua, numero, complemento, bairro, cidade, estado, telCel, referencia);
                abrir.Show();
                this.Hide();          
           
        }
            
    }
}
