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

        // Variável global para salvar o código do usuário logado e codigo do produto da lista, e codigo da unidade de medida selecionados
        int codUsuLogado, codListSelecionado, codUniSelecionado;

        public frmListaProdutos(int codUsu)
        {
            codUsuLogado = codUsu;
            InitializeComponent();
            carregarUnidadesCbb();
        }

        public frmListaProdutos(int codUsu, int codList)
        {
            codUsuLogado = codUsu;
            codListSelecionado = codList;
            InitializeComponent();
            carregarUnidadesCbb();
            buscarProdutoPorCodList(codList);
            buscarCodUniPorUnidadeSelecionada(cbbUnidadeMedida.SelectedItem.ToString());
        }

        private void limparCampos()
        {
            codListSelecionado = 0;
            cbbUnidadeMedida.SelectedItem = null;
            txtPeso.Clear();
            txtDescricao.Clear();
        }

        //  ----  Métodos para realizar queries no banco de dados
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

        public int atualizarProduto(int codList, string descricao, int peso, string unidade)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "INSERT INTO tbLista(descricao, peso, unidade, codUni)VALUES(@descricao, @peso, @unidade, @codList);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@descricao", MySqlDbType.VarChar, 100).Value = descricao;
            comm.Parameters.Add("@peso", MySqlDbType.Int32).Value = peso;
            comm.Parameters.Add("@unidade", MySqlDbType.VarChar, 20).Value = unidade;
            comm.Parameters.Add("@codList", MySqlDbType.Int32).Value = codList;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception)
            {
<<<<<<< Updated upstream
                MessageBox.Show("Este registro já existe!", "Mensagem do sistema",
=======

                //Regex utilizado para remover espaços extras entre as palavras.
                double peso = Double.Parse(txtPeso.Text);              
                int resp = cadastrarProdutos(Regex.Replace(txtDescricao.Text, @"\s+", " ").Trim().ToUpper() + " " + peso + cbbUnidadeMedida.Text, peso, cbbUnidadeMedida.Text, 1);

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
>>>>>>> Stashed changes
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
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

        // Busca o código da unidade de medida ao selecionar um item da combobox
        private void buscarCodUniPorUnidadeSelecionada(string unidade)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT codUni FROM tbunidades WHERE descricao = @descricao;";

                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = unidade;

                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    if (DR.Read())
                    {
                        if (DR.GetInt32("codUni") > 0)
                        {
                            codUniSelecionado = DR.GetInt32("codUni");

                            DataBaseConnection.CloseConnection();
                        }
                        else 
                        {
                            MessageBox.Show("Codigo não encontrado");
                            DataBaseConnection.CloseConnection();
                        }
                    }
                }
            }
        }

        // Faz a busca do produto com seu código para retornar em tela seus dados
        private void buscarProdutoPorCodList(int codList)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT * FROM tblista WHERE codList = @codList;";

                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@codList", MySqlDbType.Int32).Value = codList;

                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    if (DR.Read())
                    {
                        // caso o usuário associado ao voluntário seja encontrado, os dados são coletados e aplicados nos campos da janela

                        txtDescricao.Text = DR.GetString("descricao");
                        txtPeso.Text = DR.GetInt32("peso").ToString();
                        SelecionarUnidadeDoProduto(DR.GetString("unidade"));
                    }
                }
            }

            DataBaseConnection.CloseConnection();

            return;
        }

        // Mapeia a unidade de medida do produto e retorna ela no campo cbbUnidadeMedida
        private void SelecionarUnidadeDoProduto(string unidade)
        {
            if (string.IsNullOrWhiteSpace(unidade))
                return;

            unidade = unidade.ToUpper();

            for (int i = 0; i < cbbUnidadeMedida.Items.Count; i++)
            {
                string item = cbbUnidadeMedida.Items[i].ToString();

                if (item.Equals($"{unidade}"))
                {
                    cbbUnidadeMedida.SelectedIndex = i;
                    return;
                }
            }
        }
        

        //  ----  Métodos para eventos de clique dos botões
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
                int resp = cadastrarProdutos(Regex.Replace(txtDescricao.Text, @"\s+", " ").Trim().ToUpper(), peso, cbbUnidadeMedida.SelectedItem.ToString(), codUniSelecionado);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Cadastrado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    limparCampos();
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

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void btnMedida_Click(object sender, EventArgs e)
        {
            frmUnidadeMedida abrir = new frmUnidadeMedida(codUsuLogado);
            abrir.Show();
            this.Close();
        }      
        
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            frmPesquisarProdutosLista abrir = new frmPesquisarProdutosLista(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {

        }
        
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        // faz a busca do código da unidade de medida ao selecionar algo da combobox
        private void cbbUnidadeMedida_SelectedIndexChanged(object sender, EventArgs e)
        {
            buscarCodUniPorUnidadeSelecionada(cbbUnidadeMedida.Text);
        }

    }
}
