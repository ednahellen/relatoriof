using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmGerenciarProdutos : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmGerenciarProdutos()
        {
            InitializeComponent();
            carregarUnidadesCbb();
            carregarOrigemCbb();
            carregarProdutoCbb();
        }

        private void frmCadastrarAlimentos_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);

            //carregaProdutosNaLista();
            limparCamposDeCadastro();
            //CarregarListaProdutos();
            //cbbTipoDoacao.SelectedIndex = 0;
            //cbbUnidadeMedida.SelectedIndex = 0;
        }

<<<<<<< HEAD
        //Novo Cadastro

        public int cadastrarProdutos(string descricao, int quantidade, int peso, string unidade, string codBar, DateTime dataDeEntrada, DateTime dataDeValidade, DateTime dataLimiteDeSaida, int codUsu, int codOri, int codList)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "INSERT INTO tbprodutos(descricao, quantidade, peso, unidade, codBar, dataDeEntrada, dataDeValidade, dataLimiteDeSaida, codUsu, codOri, codList)VALUES(@descricao, @quantidade, @peso, @unidade, @codBar, @dataDeEntrada, @dataDeValidade, @dataLimiteDeSaida, @codUsu, @codOri, @codList);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@descricao", MySqlDbType.VarChar, 100).Value = descricao;
            comm.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = quantidade;
            comm.Parameters.Add("@peso", MySqlDbType.Int32).Value = peso;
            comm.Parameters.Add("@unidade", MySqlDbType.VarChar, 20).Value = unidade;
            comm.Parameters.Add("@codBar", MySqlDbType.VarChar, 13).Value = codBar;
            comm.Parameters.Add("@dataDeEntrada", MySqlDbType.Date).Value = dataDeEntrada;
            comm.Parameters.Add("@dataDeValidade", MySqlDbType.Date).Value = dataDeValidade;
            comm.Parameters.Add("@dataLimiteDeSaida", MySqlDbType.Date).Value = dataLimiteDeSaida;
            comm.Parameters.Add("@codUsu", MySqlDbType.Int32).Value = codUsu;
            comm.Parameters.Add("@codOri", MySqlDbType.Int32).Value = codOri;
            comm.Parameters.Add("@codLis", MySqlDbType.Int32).Value = codList;            

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

        private int enviarDoacoes(string nomeProduto, int quantidade, int peso, string unidadeMedida, string codBar, DateTime dataArrecadacao, DateTime dataDeValidade, DateTime dataLimiteDeSaida, int codUsu)
=======
        private (int codOrigem, int codProduto) BuscaCodigoDeListEOrigem(string nomeOrigem, string nomeProduto){
            nomeOrigem = cbbOrigemDoacao.Text;
            nomeProduto = cbbDescricao.Text;

            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT codOri FROM tbOrigemDoacao WHERE nome = @nomeOrigem; SELECT codList FROM tbLista WHERE descricao = @nomeProduto;";

            comm.Parameters.AddWithValue("@nomeOrigem", nomeOrigem);
            comm.Parameters.AddWithValue("@nomeProduto", nomeProduto);

            comm.Connection = DataBaseConnection.OpenConnection();
            MySqlDataReader DR = comm.ExecuteReader();

            int codOri = 0;
            int codList = 0;

            if (DR.Read())
            {
                codOri = DR.GetInt32(0);
            }

          
            if (DR.NextResult() && DR.Read())
            {
                codList = DR.GetInt32(0);
            }
            return (codOri, codList);
        }

        private int enviarDoacoes(string nomeProduto, int quantidade, int peso, string unidadeMedida, string codBar, DateTime dataArrecadacao, DateTime dataDeValidade, DateTime dataLimiteDeSaida, int codUsu, int codOri, int codList)
>>>>>>> dc32c7218e4572ed7061238d5db8060bc574e6f6
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = @"INSERT INTO tbProdutos( descricao,
                                quantidade,
                                peso,
                                unidade,
                                codBar,
                                dataDeEntrada,
                                dataDeValidade,
                                dataLimiteDeSaida,
                                codUsu, 
                                codOri,
                                codList) 
                                VALUES(
                                @descricao, 
                                @quantidade, 
                                @peso, 
                                @unidade,
                                @codBar,
                                @dataArrecadacao, 
                                @dataDeValidade, 
                                @dataLimiteDeSaida, 
                                @codUsu,
                                @codOri,
                                @codList);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();

            comm.Parameters.Add("@descricao", MySqlDbType.VarChar, 100).Value = nomeProduto;
            comm.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = quantidade;
            comm.Parameters.Add("@peso", MySqlDbType.Int32).Value = peso;
            comm.Parameters.Add("@unidade", MySqlDbType.VarChar, 5).Value = unidadeMedida;
            comm.Parameters.Add("@codBar", MySqlDbType.VarChar, 13).Value = codBar;
            comm.Parameters.Add("@dataArrecadacao", MySqlDbType.Date).Value = dataArrecadacao;
            comm.Parameters.Add("@dataDeValidade", MySqlDbType.Date).Value = dataDeValidade;
            comm.Parameters.Add("@dataLimiteDeSaida", MySqlDbType.VarChar, 300).Value = dataLimiteDeSaida;
            comm.Parameters.Add("@codUsu", MySqlDbType.Int32).Value = codUsu;

           var(codOrigem, codLista) = BuscaCodigoDeListEOrigem(cbbOrigemDoacao.Text, cbbDescricao.Text);

            comm.Parameters.Add("@codOri", MySqlDbType.Int32).Value = codOrigem;
            comm.Parameters.Add("@codList", MySqlDbType.Int32).Value = codLista;

            comm.Connection = DataBaseConnection.OpenConnection();

            int resp = comm.ExecuteNonQuery();


            DataBaseConnection.CloseConnection();

            limparCamposDeCadastro();

            return resp;
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

        private void carregarOrigemCbb()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT * FROM tbOrigemDoacao ORDER BY nome ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbOrigemDoacao.Items.Add(DR.GetString(1));
            }

            DataBaseConnection.CloseConnection();
        }

        private void carregarProdutoCbb()
        {
            cbbDescricao.Items.Clear();
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT * FROM tbLista ORDER BY descricao ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
               cbbDescricao.Items.Add(DR.GetString(1));
            }

            DataBaseConnection.CloseConnection();
        }



        private bool VerificaFormatacaoDosCampos()
        {
            DateTime.TryParse(dtpDataEntrada.Text, out DateTime dataRecebimento);
            if (dataRecebimento > DateTime.Today)
            {
                MessageBox.Show("Você inseriu uma data futura", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDataEntrada.Focus();
                return false;
            }

            DateTime.TryParse(dtpDataValidade.Text, out DateTime dataValidade);

            if (dataValidade < DateTime.Now.AddMonths(-3))
            {
                MessageBox.Show("O periodo para cadastro de doação excedeu!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDataEntrada.Focus();
                return false;
            }

            if (Regex.IsMatch(txtQuantidade.Text, @"[a-zA-Z]"))
            {
                MessageBox.Show("Quantidade inválida", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantidade.Focus();
                return false;
            }
            return true;
        }

        private string SimplificarUnidade(string unidadeSelecionada)
        {
            switch (unidadeSelecionada)
            {
                case "Quilogramas (Kg)":
                    return "Kg";
                case "Gramas (g)":
                    return "g";
                case "Litros (L)":
                    return "L";
                case "Mililitros (ml)":
                    return "ml";
                case "Unidades (und)":
                    return "und";
                case "Caixas (cx)":
                    return "cx";
                default:
                    return "";
            }
        }


        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            //if (cbbDescricao.Text.Equals("") || txtQuantidade.Text.Equals(""))
            //{
            //    MessageBox.Show("Um ou mais campos não foram preenchidos corretamente", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

<<<<<<< HEAD
            //if (!VerificaFormatacaoDosCampos())
            //{
            //    return;
            //}
            //string nomeItem = cbbDescricao.Text;
            //int quantidade = Convert.ToInt32(txtQuantidade.Text);
            //int peso = Convert.ToInt32(txtPeso.Text);
            //string tipoUnidade = SimplificarUnidade(cbbUnidadeMedida.Text);
            //string codBar = txtCodBarras.Text;
            //DateTime dataRecebimento = Convert.ToDateTime(dtpDataEntrada.Text);
            //DateTime dataValidade = Convert.ToDateTime(dtpDataValidade.Text);
            //DateTime dataLimiteDeSaida = dataValidade.AddDays(21);
            //int codUsu = 1;

            //if (enviarDoacoes(nomeItem, quantidade, peso, tipoUnidade, codBar, dataRecebimento, dataValidade, dataLimiteDeSaida, codUsu) == 1)
            //{
            //    MessageBox.Show("Doação cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    //dgvProdutos.Columns.Clear();
            //    //CarregarListaProdutos();
            //}
            //else
            //{
            //    MessageBox.Show("Erro ao cadastrar doação!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    limparCamposDeCadastro();
            //}
=======
            if (!VerificaFormatacaoDosCampos())
            {
                return;
            }
            string nomeItem = cbbDescricao.Text;
            int quantidade = Convert.ToInt32(txtQuantidade.Text);
            int peso = Convert.ToInt32(txtPeso.Text);
            string tipoUnidade = SimplificarUnidade(cbbUnidadeMedida.Text);
            string codBar = txtCodBarras.Text;
            DateTime dataRecebimento = Convert.ToDateTime(dtpDataEntrada.Text);
            DateTime dataValidade = Convert.ToDateTime(dtpDataValidade.Text);
            DateTime dataLimiteDeSaida = dataValidade.AddDays(21);
            int codUsu = 1;
            var (codOrigem, codLista) = BuscaCodigoDeListEOrigem(cbbOrigemDoacao.Text, cbbDescricao.Text);

            if (dtpDataValidade.Value < DateTime.Today){
                MessageBox.Show("A data de validade não pode ser anterior a data atual!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDataValidade.Focus();
                return;
            }

            if (enviarDoacoes(nomeItem, quantidade, peso, tipoUnidade, codBar, dataRecebimento, dataValidade, dataLimiteDeSaida, codUsu, codOrigem, codLista) == 1)
            {
                MessageBox.Show("Doação cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //dgvProdutos.Columns.Clear();
                //CarregarListaProdutos();
            }
            else
            {
                MessageBox.Show("Erro ao cadastrar doação!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                limparCamposDeCadastro();
            }
>>>>>>> dc32c7218e4572ed7061238d5db8060bc574e6f6

            ////dgvProdutos.Columns.Clear();
            ////CarregarListaProdutos();
            ///

            cadastrarProdutos(cbbDescricao.Text,Convert.ToInt32(txtQuantidade.Text), peso, cbbUnidadeMedida.Text, dtpDataEntrada.Text, dtpDataValidade.Text, dtpDataValidade.Text, 1, 1, 1 );
        }

        private void btnAtualizarDados_Click(object sender, EventArgs e)
        {
            //dgvRegistro.Rows.Clear();
            //CarregarListaProdutos();
            limparCamposDeCadastro();
            //carregaProdutosNaLista();
        }

        public void limparCamposDeCadastro()
        {
            
            txtQuantidade.Clear();
            dtpDataValidade.Value = DateTime.Now;
            DateTime dataRecebimento = Convert.ToDateTime(dtpDataEntrada.Text);
            dtpDataValidade.Value = DateTime.Now;
            //cbbUnidadeMedida.SelectedIndex = 0;
            txtPeso.Clear();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDoacao_Click(object sender, EventArgs e)
        {
            frmOrigemDoacao abrir = new frmOrigemDoacao();
            abrir.Show();
            this.Hide();

        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            frmListaProdutos abrir = new frmListaProdutos();
            abrir.Show();
            this.Hide();

        }

        private void btnMedida_Click(object sender, EventArgs e)
        {
            frmUnidadeMedida abrir = new frmUnidadeMedida();
            abrir.Show();
            this.Hide();
        }

        private void gpbCamposDoProduto_Enter(object sender, EventArgs e)
        {

        }

        private void cbbDescricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbDescricao.SelectedItem == null) return;

            string nomeSelecionado = cbbDescricao.SelectedItem.ToString();

            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT peso, unidade FROM tbLista WHERE descricao = @descricao;";
            comm.CommandType = CommandType.Text;

            comm.Parameters.AddWithValue("@descricao", nomeSelecionado);

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            if (DR.Read()) { 
                    txtPeso.Text = DR.GetInt32(0).ToString();
                    cbbUnidadeMedida.Text = DR.GetString(1);
            }
            DataBaseConnection.CloseConnection();
        }
    }
}
