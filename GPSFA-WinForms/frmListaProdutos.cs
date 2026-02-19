using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GPSFA_WinForms
{
    public partial class frmListaProdutos : Form
    {
        public frmListaProdutos()
        {
            InitializeComponent();
            carregarUnidadesCbb();
        }

        // Variável global para salvar o código do usuário logado
        int codUsuLogado;
        public frmListaProdutos(int codUsu)
        {
            codUsuLogado = codUsu;
            InitializeComponent();
            carregarUnidadesCbb();
        }

        private void btnMedida_Click(object sender, EventArgs e)
        {
            frmUnidadeMedida abrir = new frmUnidadeMedida(codUsuLogado);
            abrir.ShowDialog();
        }      

        private void carregarUnidadesCbb()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT * FROM tbUnidades ORDER BY descricao ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbUnidadeMedida.Items.Add(DR.GetString(1));
            }

            DataBaseConnection.CloseConnection();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtDescricao.Text.Equals(""))
            {
                MessageBox.Show("Favor inserir valores!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtDescricao.Focus();
            }     
            else
            {

                //Regex utilizado para remover espaços extras entre as palavras.
                double peso = Double.Parse(txtPeso.Text);
               double resp = cadastrarProdutos(Regex.Replace(txtDescricao.Text, @"\s+", " ").Trim().ToUpper() + " - " + txtPeso.Text + cbbUnidadeMedida.Text, peso, cbbUnidadeMedida.Text, 1);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Cadastrado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);                    
                    txtDescricao.Clear();
                    txtDescricao.Enabled = false;
                    btnNovo.Enabled = true;
                    btnNovo.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao Cadastrar!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

                   
                    btnCadastrar.Enabled = false;
                    btnLimpar.Enabled = false;
                    btnNovo.Enabled = true;
                    txtDescricao.Enabled = false;

                }
            }
        }

        public int cadastrarProdutos(string descricao, double peso, string unidade, int codUni)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "INSERT INTO tbLista(descricao, peso, unidade, codUni)VALUES(@descricao, @peso, @unidade, @codUni);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@descricao", MySqlDbType.VarChar, 100).Value = descricao;
            comm.Parameters.Add("@peso", MySqlDbType.Int32).Value = peso;
            comm.Parameters.Add("@unidade", MySqlDbType.VarChar, 20).Value = unidade;
            comm.Parameters.Add("@codUni", MySqlDbType.Int32).Value = codUni;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception)
            {
                MessageBox.Show("Este registro já existe!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
        }
    }
}
