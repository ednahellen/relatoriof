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
    public partial class frmTipoDeArrecadacao : Form
    {
        public frmTipoDeArrecadacao()
        {
            InitializeComponent();
            carregaTipoArrecadacao();
        }

        public frmTipoDeArrecadacao(int codUsu)
        {
            codUsuLogado = codUsu;
            InitializeComponent();
            carregaTipoArrecadacao();
        }

        // Variavel global da janela para salvar o código do usuário logado
        int codUsuLogado;

        //Variavel global da janela para salvar o código de origem da doação
        int codOrigem;

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Hide();
        }

        private void carregaTipoArrecadacao()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT * FROM tbOrigemDoacao ORDER BY nome ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbTipoDeArrecadacao.Items.Add(DR.GetString(1));
            }

            DataBaseConnection.CloseConnection();
        }

        private void cbbTipoDeArrecadacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbTipoDeArrecadacao.SelectedItem == null) return;

            string origemSelecionada = cbbTipoDeArrecadacao.SelectedItem.ToString();

            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado, origemSelecionada);
            abrir.Show();
            this.Hide();
            
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT codOri, nome FROM tbOrigemDoacao WHERE nome = @nome;";
            comm.CommandType = CommandType.Text;
            comm.Parameters.Clear();
            comm.Parameters.AddWithValue("@nome", origemSelecionada);

            comm.Connection = DataBaseConnection.OpenConnection();

            using (MySqlDataReader DR = comm.ExecuteReader())
            {
                if (DR.Read())
                {
                    try
                    {
                        codOrigem = DR.GetInt32(0);
                        
                        origemSelecionada = DR.GetString(1);

                        DataBaseConnection.CloseConnection();
                        
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show($"Erro ao selecionar o tipo de arrecadação. Erro:\n\n{error}", "Mensagem do sistema",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                        DataBaseConnection.CloseConnection();
                    }
                }
            }          
           
        }
    }
}
