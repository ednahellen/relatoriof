using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ViaCep; // API BUSCA CEP
using CpfCnpjLibrary; //API VALIDA CNPJ | CPF

namespace GPSFA_WinForms
{
    public partial class frmOrigemDoacao : Form
    {
        //Desativando o botão fechar da janela Parte 1/2.
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        // Método Construtor Base da Janela.
        public frmOrigemDoacao()
        {
            InitializeComponent();
            desativarBotoes();
            desativarCampos();           
        }
        // Método Construtor da Janela com parâmetro do CodUsu logado.
        public frmOrigemDoacao(int codUsu)
        {   
            codUsuLogado = codUsu;
            InitializeComponent();
            desativarBotoes();
            desativarCampos();
        }
        // Método Construtor Base da Janela com parâmetros para preenchimento dos campos da janela.
        public frmOrigemDoacao(string nome, string cpf, string cnpj, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado, string telCel, string referencia)
        {            
            InitializeComponent();
            buscaCodigoOrigem(nome);
            btnNovo.Enabled = false;
            btnCadastrar.Enabled = false;
            txtNomeFornecedor.Text = nome;
            mskCpf.Text = cpf;
            mskCnpj.Text = cnpj;
            mskCep.Text = cep;
            txtRua.Text = rua;
            txtNumero.Text = numero;
            txtComplemento.Text = complemento;
            txtBairro.Text = bairro;
            cbbCidade.Text = cidade;
            cbbEstado.Text = estado;
            mskTelefone.Text = telCel;
            txtReferencia.Text = referencia;
            mskCnpj.Enabled = false;
            mskCpf.Enabled = false;
            cbbCidade.Enabled = false;
            cbbEstado.Enabled = false;
        }
        // Método Construtor Base da Janela com parâmetro do NOME para preenchimento dos campos da janela.
        public frmOrigemDoacao(string nome)
        {
            InitializeComponent();
            buscaCodigoOrigem(nome); 
            txtNomeFornecedor.Text = nome;
            buscaOrigemDoacao(nome);
            desativarBotoes();            
            btnNovo.Enabled = false;
            btnAlterar.Enabled = true;
            btnLimpar.Enabled = true;
            btnExcluir.Enabled = true;
            cbbEstado.Enabled = false;
            cbbCidade.Enabled = false;
        }
        //Métodos de manipulação de campos e botões da janela
        public void desativarBotoes()
        {
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnCadastrar.Enabled = false;
            btnLimpar.Enabled = false;
        }
        public void desativarCampos()
        {
            txtNomeFornecedor.Enabled = false;
            txtRua.Enabled = false;
            txtNumero.Enabled = false;
            txtComplemento.Enabled = false;
            txtBairro.Enabled = false;
            txtReferencia.Enabled = false;
            mskCep.Enabled = false;
            mskTelefone.Enabled = false;
            mskCpf.Enabled = false;
            mskCnpj.Enabled = false;
            cbbEstado.Enabled = false;
            cbbCidade.Enabled = false;
            rdbCpf.Enabled = false;
            rdbCnpj.Enabled = false;
        }
        public void habilitaCampos()
        {
            txtNomeFornecedor.Enabled = true;
            txtRua.Enabled = true;
            txtNumero.Enabled = true;
            txtComplemento.Enabled = true;
            txtBairro.Enabled = true;
            txtReferencia.Enabled = true;
            mskCep.Enabled = true;
            mskTelefone.Enabled = true;
            mskCpf.Enabled = false;
            mskCnpj.Enabled = false;
            cbbEstado.Enabled = false;
            cbbCidade.Enabled = false;
            rdbCpf.Enabled = true;
            rdbCnpj.Enabled = true;
        }
        public void limparCampos()
        {
            txtNomeFornecedor.Clear();
        }
        public void limparCamposNovo()
        {
            txtNomeFornecedor.Clear();
            txtRua.Clear();
            txtNumero.Clear();
            txtComplemento.Clear();
            txtBairro.Clear();
            txtReferencia.Clear();
            mskCep.Clear();
            mskTelefone.Clear();
            mskCpf.Clear();
            mskCnpj.Clear();
            cbbEstado.Text = "";
            cbbCidade.Text = "";
        }       

        // Variavel global do código do usuário logado
        int codUsuLogado;

        // Variável global para receber o valor da resposta do método de busca de dados da origem de doação, utilizada para validação no cadastro e alteração de dados da origem de doação.
        int respBuscar;

        //Métodos de BUSCA

        // Método de pesquisa com o parametro NOME para cadastro de OrigemDoacao
        public int buscaDadosOrigemDoacao(string nome)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT nome, cpf, cnpj, cep, rua, numero, complemento, bairro, cidade, estado, telCel, referencia FROM tborigemdoacao WHERE nome LIKE '%{nome}%';";
            comm.CommandType = CommandType.Text;
            comm.Connection = DataBaseConnection.OpenConnection();
            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            if (DR.HasRows == false)
            {               
                respBuscar = 0;
            }
            else
            {
                MessageBox.Show("Esta Origem já existe!.",
                    "Mensagem do sistema",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                limparCamposNovo();
                txtNomeFornecedor.Focus();
                mskCpf.Text = "";
                mskCpf.Enabled = false;
                mskCnpj.Text = "";
                mskCnpj.Enabled = false;
                rdbCpf.Checked = false;
                rdbCnpj.Checked = false;
                respBuscar = 1;
            }

            return respBuscar;
        }
        // Método de pesquisa com o parametro NOME para cadastro de OrigemDoacao
        public int buscaOrigemDoacao(string nome)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT nome, cpf, cnpj, cep, rua, numero, complemento, bairro, cidade, estado, telCel, referencia FROM tborigemdoacao WHERE nome LIKE '%{nome}%';";
            comm.CommandType = CommandType.Text;
            comm.Connection = DataBaseConnection.OpenConnection();
            MySqlDataReader DR;
            DR = comm.ExecuteReader();


            if (DR.HasRows == false)
            {
                MessageBox.Show("Nenhuma origem encontrada com este valor.",
                    "Mensagem do sistema",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                limparCamposNovo();
                respBuscar = 0;
            }
            else
            {
                while (DR.Read())
                {
                    txtNomeFornecedor.Text = (DR.GetString(0));

                    try
                    {
                        mskCpf.Text = DR.GetString(1);
                        mskCpf.Enabled = false;
                    }
                    catch (Exception)
                    {                       
                        mskCpf.Enabled = false;                        
                    }
                    try
                    {
                        mskCnpj.Text = DR.GetString(2);
                        mskCnpj.Enabled = false;
                    }
                    catch (Exception)
                    {
                        mskCnpj.Enabled = false;                        
                    }
                    try
                    {
                        mskCep.Text = DR.GetString(3);
                        txtRua.Text = DR.GetString(4);
                        txtNumero.Text = DR.GetString(5);
                        txtComplemento.Text = DR.GetString(6);
                        txtBairro.Text = DR.GetString(7);
                        cbbCidade.Items.Add(DR.GetString(8));
                        cbbEstado.Items.Add(DR.GetString(9));
                        mskTelefone.Text = DR.GetString(10);
                        txtReferencia.Text = DR.GetString(11);
                    }
                    catch (Exception)
                    {
                        mskCep.Text = "";
                        txtRua.Text = "";
                        txtNumero.Text = "";
                        txtComplemento.Text = "";
                        txtBairro.Text = "";
                        cbbCidade.Items.Add("");
                        cbbEstado.Items.Add("");
                        mskTelefone.Text = "";
                        txtReferencia.Text = "";
                    }                    
                }
            }
            DataBaseConnection.CloseConnection();
            return respBuscar;
        }

        //Método de busca de endereço utilizando a biblioteca ViaCep, com o parâmetro do CEP para preenchimento automático dos campos de endereço, e tratamento de erro caso o CEP seja inválido ou não encontrado.
        public void BuscaCEP(string CEP)
        {
            try
            {
                ViaCepClient viaCep = new ViaCepClient();               

                var endereco = viaCep.Search(CEP);

                txtRua.Text = endereco.Street;
                txtBairro.Text = endereco.Neighborhood;
                cbbCidade.Text = endereco.City;
                cbbEstado.Text = endereco.StateInitials;
            }
            catch (Exception)
            {
                MessageBox.Show("CEP não encontrado!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                mskCep.Clear();
                mskCep.Focus();
            }
        }

        // Métodos CRUD

        // Variável global para receber o valor do código da origem de doação, utilizada para validação no cadastro, alteração e exclusão de dados da origem de doação.
        int codOri = 0;

        // Variável global para receber o valor da resposta do método de alteração de dados da origem de doação, utilizada para validação da alteração de dados da origem de doação.
        int respAlterar = 0;

        // Método CRUD de cadastro de fornecedores, com os parâmetros necessários para o cadastro da origem de doação, retornando um valor inteiro para validação do cadastro.
        public int cadastrarFornecedores(string nome, string cpf, string cnpj, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado, string telCel, string referencia)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "INSERT INTO TBOrigemDoacao(nome, cpf, cnpj, cep, rua, numero, complemento, bairro, cidade, estado, telCel, referencia)VALUES(@nome, @cpf, @cnpj, @cep, @rua, @numero, @complemento, @bairro, @cidade, @estado, @telCel, @referencia);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = nome;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 14).Value = cpf;
            comm.Parameters.Add("@cnpj", MySqlDbType.VarChar, 18).Value = cnpj;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 9).Value = cep;
            comm.Parameters.Add("@rua", MySqlDbType.VarChar, 100).Value = rua;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 5).Value = numero;
            comm.Parameters.Add("@complemento", MySqlDbType.VarChar, 100).Value = complemento;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 100).Value = bairro;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 100).Value = cidade;
            comm.Parameters.Add("@estado", MySqlDbType.VarChar, 2).Value = estado;
            comm.Parameters.Add("@telCel", MySqlDbType.VarChar, 15).Value = telCel;
            comm.Parameters.Add("@referencia", MySqlDbType.VarChar, 200).Value = referencia;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception)
            {

            }
            return 0;
        }

        //Método de busca do código da origem de doação, com o parâmetro NOME, para utilização em outros métodos da janela.
        public void buscaCodigoOrigem(string nome)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT codOri FROM tborigemdoacao WHERE nome LIKE '%{nome}%';";
            comm.CommandType = CommandType.Text;
            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            codOri = 0;

            while (DR.Read())
            {
                codOri = DR.GetInt32(0);
            }

            DataBaseConnection.CloseConnection();
        }

        // Método CRUD de Alterar fornecedores, com os parâmetros necessários para o ALTERAÇÃO da origem de doação, retornando um valor inteiro para validação da alteração.
        private int alterarOrigemDoacao(string nome, string cpf, string cnpj, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado, string telCel, string referencia)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "UPDATE tbOrigemDoacao SET nome = @nome, cpf = @cpf, cnpj = @cnpj, cep = @cep, rua = @rua, numero = @numero, complemento = @complemento, bairro = @bairro, cidade = @cidade, estado = @estado, telCel = @telCel, referencia = @referencia WHERE codOri = " + codOri;
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = nome;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 14).Value = cpf;
            comm.Parameters.Add("@cnpj", MySqlDbType.VarChar, 18).Value = cnpj;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 9).Value = cep;
            comm.Parameters.Add("@rua", MySqlDbType.VarChar, 100).Value = rua;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 5).Value = numero;
            comm.Parameters.Add("@complemento", MySqlDbType.VarChar, 100).Value = complemento;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 100).Value = bairro;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 100).Value = cidade;
            comm.Parameters.Add("@estado", MySqlDbType.VarChar, 2).Value = estado;
            comm.Parameters.Add("@telCel", MySqlDbType.VarChar, 15).Value = telCel;
            comm.Parameters.Add("@referencia", MySqlDbType.VarChar, 200).Value = referencia;

            comm.Connection = DataBaseConnection.OpenConnection();
           
            respAlterar = comm.ExecuteNonQuery();
            DataBaseConnection.CloseConnection();           

            return respAlterar;
        }

        //Método CRUD de Excluir fornecedores, com o parâmetro do código da origem de doação, retornando um valor inteiro para validação da exclusão.
        private int excluirOrigem(int codOri)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "DELETE FROM tborigemdoacao WHERE codOri = " + codOri;
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();

            comm.Connection = DataBaseConnection.OpenConnection();

            int resp = comm.ExecuteNonQuery();

            DataBaseConnection.CloseConnection();

            return resp;
        }     
                
        //Desativando o botão fechar da janela Parte 2/2.
        private void frmOrigemDoacao_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);           
        }

        //Ações dos botões e campos
        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitaCampos();
            txtNomeFornecedor.Focus();
            btnNovo.Enabled = false;
            btnCadastrar.Enabled = true;
        }
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado);
            abrir.Show();
            this.Hide();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            if (txtNomeFornecedor.Text.Equals(""))
            {
                MessageBox.Show("Campo já está vazio!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNomeFornecedor.Focus();
            }
            else
                limparCampos();
            btnCadastrar.Enabled = false;
            btnNovo.Enabled = true;
            btnLimpar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            desativarCampos();
            btnNovo.Focus();
        }
        private void mskCep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscaCEP(mskCep.Text);
                txtNumero.Focus();
            }
        }

        // Validação dos campos de CPF e CNPJ para habilitação dos campos de preenchimento, utilizando os radio buttons para escolha do tipo de pessoa, e desabilitando os campos caso o tipo de pessoa seja alterado ou caso o campo seja deixado em branco.
        private void rdbCpf_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCpf.Checked.Equals(true))
            {
                mskCpf.Enabled = true;
                mskCpf.Focus();
            }
            else if (rdbCpf.Checked.Equals(true) && mskCpf.Text.Equals(""))
            {
                mskCpf.Enabled = false;
                mskCpf.Clear();
            }
            else
            {
                mskCpf.Enabled= false;
            }
        }

        private void rdbCnpj_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCnpj.Checked.Equals(true))
            {
                mskCnpj.Enabled = true;
                mskCnpj.Focus();
            }
            else if (rdbCnpj.Checked.Equals(true) && mskCnpj.Text.Equals(""))
            {
                mskCnpj.Enabled = false;
                mskCnpj.Clear();
            }
            else
            {
                mskCnpj.Enabled = false;
            }
        }

        //Validação para habilitação dos botões de alteração, exclusão e limpeza de campos, caso haja algum valor no campo de nome da origem de doação, e desabilitação dos botões caso o campo seja deixado em branco.
        private void txtNomeFornecedor_TextChanged(object sender, EventArgs e)
        {
            if (txtNomeFornecedor.Text.Length > 0)
            {
                btnLimpar.Enabled = true;
            }
            else
            {
                btnAlterar.Enabled = false;
                btnExcluir.Enabled = false;
            }
        }

        //Botões CRUD 

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtNomeFornecedor.Text.Equals(""))
            {
                MessageBox.Show("Favor inserir um nome!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNomeFornecedor.Focus();
            }
            else if (rdbCpf.Checked && !buscaDadosOrigemDoacao(txtNomeFornecedor.Text).Equals(1))
            {
                string cnpj = null;

                //Regex utilizado para remover espaços extras entre as palavras.

                if (!mskCpf.MaskCompleted)
                {
                    MessageBox.Show("Preencha o campo CPF corretamente!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    mskCpf.Focus();
                    return;
                }
                else if (Cpf.Validar(mskCpf.Text).Equals("False"))
                {
                    MessageBox.Show("Inserir um CPF válido!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    mskCpf.Focus();
                    return;
                }               
               
                    int resp = cadastrarFornecedores(Regex.Replace(txtNomeFornecedor.Text, @"\s+", " ").Trim().ToUpper(), mskCpf.Text, cnpj, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, cbbCidade.Text, cbbEstado.Text, mskTelefone.Text, txtReferencia.Text);


                if (resp.Equals(1))
                {
                    MessageBox.Show("Cadastrado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    limparCamposNovo();
                    desativarCampos();
                    rdbCpf.Checked = false;
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
                    desativarCampos();
                }
            }
            else if (rdbCnpj.Checked && !buscaDadosOrigemDoacao(txtNomeFornecedor.Text).Equals(1))
            {
                string cpf = null;

                if (!mskCnpj.MaskCompleted)
                {
                    MessageBox.Show("Preencha o campo CNPJ corretamente!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    mskCnpj.Focus();
                    return;
                }
                else if (Cnpj.Validar(mskCnpj.Text).Equals("False"))
                {
                    MessageBox.Show("Inserir um CNPJ válido!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    mskCpf.Focus();
                    return;
                }

                //Regex utilizado para remover espaços extras entre as palavras.
                int resp = cadastrarFornecedores(Regex.Replace(txtNomeFornecedor.Text, @"\s+", " ").Trim().ToUpper(), cpf, mskCnpj.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, cbbCidade.Text, cbbEstado.Text, mskTelefone.Text, txtReferencia.Text);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Cadastrado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    limparCamposNovo();
                    desativarCampos();
                    rdbCnpj.Checked = false;
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
                    desativarCampos();
                }
            }
            else if (!txtNomeFornecedor.Text.Equals("") && !buscaDadosOrigemDoacao(txtNomeFornecedor.Text).Equals(1))
            {
                string cpf = null;
                string cnpj = null;

                //Regex utilizado para remover espaços extras entre as palavras.
                int resp = cadastrarFornecedores(Regex.Replace(txtNomeFornecedor.Text, @"\s+", " ").Trim().ToUpper(), cpf, cnpj, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, cbbCidade.Text, cbbEstado.Text, mskTelefone.Text, txtReferencia.Text);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Cadastrado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    limparCamposNovo();
                    desativarCampos();
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
                    desativarCampos();
                }
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (txtNomeFornecedor.Text.Equals(""))
            {
                MessageBox.Show("Favor inserir um nome!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNomeFornecedor.Focus();
            }
            else if (rdbCpf.Checked)
            {
                string cnpj = null;
                //Regex utilizado para remover espaços extras entre as palavras.

                if (!mskCpf.MaskCompleted)
                {
                    MessageBox.Show("Preencha o campo CPF corretamente!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    mskCpf.Focus();
                    return;
                }

                int resp = alterarOrigemDoacao(Regex.Replace(txtNomeFornecedor.Text, @"\s+", " ").Trim().ToUpper(), mskCpf.Text, cnpj, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, cbbCidade.Text, cbbEstado.Text, mskTelefone.Text, txtReferencia.Text);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Alterado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    limparCamposNovo();
                    desativarCampos();
                    btnNovo.Enabled = true;
                    btnNovo.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao Alterar!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                    btnCadastrar.Enabled = false;
                    btnLimpar.Enabled = false;
                    btnNovo.Enabled = true;
                    desativarCampos();
                }
            }
            else if (rdbCnpj.Checked)
            {
                string cpf = null;

                if (!mskCnpj.MaskCompleted)
                {
                    MessageBox.Show("Preencha o campo CNPJ corretamente!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    mskCnpj.Focus();
                    return;
                }

                //Regex utilizado para remover espaços extras entre as palavras.
                int resp = alterarOrigemDoacao(Regex.Replace(txtNomeFornecedor.Text, @"\s+", " ").Trim().ToUpper(), cpf, mskCnpj.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, cbbCidade.Text, cbbEstado.Text, mskTelefone.Text, txtReferencia.Text);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Alterado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    limparCamposNovo();
                    desativarCampos();
                    btnNovo.Enabled = true;
                    btnNovo.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao Alterar!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                    btnCadastrar.Enabled = false;
                    btnLimpar.Enabled = false;
                    btnNovo.Enabled = true;
                    desativarCampos();
                }
            }
            else if (!txtNomeFornecedor.Text.Equals(""))
            {
                string cpf = null;
                string cnpj = null;

                //Regex utilizado para remover espaços extras entre as palavras.
                int resp = alterarOrigemDoacao(Regex.Replace(txtNomeFornecedor.Text, @"\s+", " ").Trim().ToUpper(), cpf, cnpj, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, cbbCidade.Text, cbbEstado.Text, mskTelefone.Text, txtReferencia.Text);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Alterado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    limparCamposNovo();
                    desativarCampos();
                    btnNovo.Enabled = true;
                    btnNovo.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao Alterar!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                    btnCadastrar.Enabled = false;
                    btnLimpar.Enabled = false;
                    btnNovo.Enabled = true;
                    desativarCampos();
                }
            }
        }
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            frmPesquisarOrigemDoacao abrir = new frmPesquisarOrigemDoacao(codUsuLogado);
            abrir.Show();
            this.Hide();
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir?", "Mensagem do Sistema",
                  MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result.Equals(DialogResult.Yes))
            {
                int resp = excluirOrigem(codOri);

                if (resp.Equals(1))
                {
                    MessageBox.Show("Excluido com Sucesso!", "Mensagem do Sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    btnExcluir.Enabled = false;
                    btnAlterar.Enabled = false;
                    btnLimpar.Enabled = false;
                    btnNovo.Enabled = true;
                    limparCamposNovo();
                    desativarCampos();
                }
                else
                {
                    MessageBox.Show("Erro ao excluir!", "Mensagem do Sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    txtNomeFornecedor.Clear();
                    txtNomeFornecedor.Focus();
                    btnExcluir.Enabled = false;
                    btnAlterar.Enabled = false;
                    btnLimpar.Enabled = false;
                    btnNovo.Enabled = true;
                    txtNomeFornecedor.Enabled = false;
                    limparCamposNovo();
                }
            }
        }

        public void BuscaCidade()
        {
            try
            {
              

            }
            catch (Exception)
            {
                MessageBox.Show("CEP não encontrado!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                mskCep.Clear();
                mskCep.Focus();
            }
        }

        private void cbbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
