using CpfLibrary;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPSFA_WinForms.classes;


namespace GPSFA_WinForms
{
    public partial class frmVoluntarios : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmVoluntarios()
        {
            InitializeComponent();
            desativarBotoes();
            desabilitarCampos();
        }
        
        // Instância da Janela com variável imbutida
        public frmVoluntarios(string text)
        {
            InitializeComponent();
            codVol = Convert.ToInt32(text);
            buscarDadosDoVoluntarioPeloCodigo(codVol);
            buscarUsuarioAtivo(codVol);
            habilitarCampos();
            btnNovo.Enabled = false;
            btnCadastrar.Enabled = false;
        }

        // Variaveis globais 
        int codVol = 0; // Código do voluntário
        bool isVoluntarioActive = true; // Estado do voluntário
        bool isUsuarioActive = true; // Estado do usuário do voluntário


        // Métodos para ações CRUD e queries do banco de dados
        private void buscarDadosDoVoluntarioPeloCodigo(int codVoluntario)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT * FROM tbVoluntarios WHERE codVol = @codVol;";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Clear();

            comm.Parameters.Add("@codVol", MySqlDbType.VarChar, 100).Value = codVoluntario;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            while (DR.Read())
            {
                codVol = DR.GetInt32(0);
                txtNomeVoluntario.Text = DR.GetString(1);
                mskTelefone.Text = DR.GetString(2);
                mskCpf.Text = DR.GetString(3);
                mskCep.Text = DR.GetString(4);
                txtRua.Text = DR.GetString(5);
                txtNumero.Text = DR.GetString(6);
                txtComplemento.Text = DR.GetString(7);
                txtBairro.Text = DR.GetString(8);
                txtCidade.Text = DR.GetString(9);
                cbbEstado.Text = DR.GetString(10);
                isVoluntarioActive = DR.GetBoolean(11);
            }

            DataBaseConnection.CloseConnection();
        }

        private void buscarUsuarioAtivo(int codVoluntario)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT * FROM tbUsuarios WHERE codVol = @codVol;";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Clear();

            comm.Parameters.Add("@codVol", MySqlDbType.VarChar, 100).Value = codVoluntario;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            while (DR.Read())
            {
                isUsuarioActive = DR.GetBoolean(5);
                txtUsuario.Text = DR.GetString(1);
                txtSenha.Text = DR.GetString(2);
                txtConfirmaSenha.Text = DR.GetString(2);
                string acesso = DR.GetString(3);
                BuscarAcessoDoUsuario(acesso);
            }

            if (isUsuarioActive)
            {
                ckbUsuarioAtivo.Checked = true;
            }
            else if (isUsuarioActive == false)
            {
                ckbUsuarioAtivo.Checked = false;
            }
        }
        private void BuscarAcessoDoUsuario(string acesso)
        {
            if (string.IsNullOrWhiteSpace(acesso))
                return;

            acesso = acesso.Trim().ToUpper();

            for (int i = 0; i < cbbTipoDeAcesso.Items.Count; i++)
            {
                string item = cbbTipoDeAcesso.Items[i].ToString();

                if (item.Equals($"{acesso}"))
                {
                    cbbTipoDeAcesso.SelectedIndex = i;
                    return;
                }
            }
        }

        private int buscarVoluntarioPorDescricao(string nome, string cpf)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT * FROM tbVoluntarios WHERE nome = @nome OR cpf = @cpf;";

            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 20).Value = nome;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 20).Value = cpf;

            comm.Connection = DataBaseConnection.OpenConnection();

            int resp = comm.ExecuteNonQuery();

            DataBaseConnection.CloseConnection();

            return resp;
        }

        public int cadastrarVoluntario(string nome, string telCel, string cpf, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "INSERT INTO tbVoluntarios(nome,telCel,cpf,cep,rua,numero,complemento,bairro,cidade,estado)VALUES(@nome,@telCel,@cpf,@cep,@rua,@numero,@complemento,@bairro,@cidade,@estado);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 20).Value = nome;
            comm.Parameters.Add("@telCel", MySqlDbType.VarChar, 20).Value = telCel;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 20).Value = cpf;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 20).Value = cep;
            comm.Parameters.Add("@rua", MySqlDbType.VarChar, 20).Value = rua;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 20).Value = numero;
            comm.Parameters.Add("@complemento", MySqlDbType.VarChar, 20).Value = complemento;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 20).Value = bairro;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 20).Value = cidade;
            comm.Parameters.Add("@estado", MySqlDbType.VarChar, 20).Value = estado;

            // Será adicionado métodos para inserção + edição dos dados de usuário 
            //comm.Parameters.Add("@usuario", MySqlDbType.VarChar, 20).Value = usuario;
            //comm.Parameters.Add("@senha", MySqlDbType.VarChar, 20).Value = senha;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception)
            {
                MessageBox.Show("Este voluntario já existe!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
        }

        public int cadastrarUsuario(int codVol, string usuario, string senha)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "INSERT INTO tbVoluntarios(usuario,senha,codVol)VALUES(@usuario,@senha,@codVol)";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@usuario", MySqlDbType.VarChar, 20).Value = usuario;
            comm.Parameters.Add("@senha", MySqlDbType.VarChar, 20).Value = senha;
            comm.Parameters.Add("@codVol", MySqlDbType.VarChar, 20).Value = codVol;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao editar dados do Voluntário!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
        }

        public int alterarDadosVoluntario(string nome, string telCel, string cpf, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado, string usuario, string senha, int codVol)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "UPDATE tbVoluntarios SET nome = @nome, telCel = @telCel, cpf = @cpf, cep = @cep, rua = @rua, numero = @numero, complemento  = @complemento, bairro = @bairro, cidade = @cidade, estado = @estado, usuario = @usuario, senha = @senha WHERE codVol = @codVol;";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 20).Value = nome;
            comm.Parameters.Add("@telCel", MySqlDbType.VarChar, 20).Value = telCel;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 20).Value = cpf;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 20).Value = cep;
            comm.Parameters.Add("@rua", MySqlDbType.VarChar, 20).Value = rua;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 20).Value = numero;
            comm.Parameters.Add("@complemento", MySqlDbType.VarChar, 20).Value = complemento;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 20).Value = bairro;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 20).Value = cidade;
            comm.Parameters.Add("@estado", MySqlDbType.VarChar, 20).Value = estado;
            comm.Parameters.Add("@usuario", MySqlDbType.VarChar, 20).Value = usuario;

            //comm.Parameters.Add("@senha", MySqlDbType.VarChar, 20).Value = senha;
            //comm.Parameters.Add("@codVol", MySqlDbType.VarChar, 20).Value = codVol;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao editar dados do Voluntário!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
        }

        public int excluirDadosVoluntario(int codVol)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "UPDATE tbVoluntarios SET  telCel = '', cpf = '', cep = '', rua = '', numero = '', complemento  = '', bairro = '', cidade = '', estado = '', ativo = 0 WHERE codVol = @codVol;";
            
            //comm.CommandText = "UPDATE tbVoluntarios SET  telCel = '', cpf = '', cep = '', rua = '', numero = '', complemento  = '', bairro = '', cidade = '', estado = '' WHERE codVol = @codVol;";

            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@codVol", MySqlDbType.VarChar, 20).Value = codVol;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao editar dados do Voluntário!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
        }


        // Métodos para atalizações dos campos e recursos da janela
        private void limparCampos()
        {
            txtNomeVoluntario.Clear();
            txtNomeVoluntario.Focus();
            mskTelefone.Clear();
            mskCpf.Clear();
            txtRua.Clear();
            txtNumero.Clear();
            txtComplemento.Clear();
            mskCep.Clear();
            txtBairro.Clear();
            txtUsuario.Clear();
            txtSenha.Clear();
            txtConfirmaSenha.Clear();
            cbbEstado.Items.Clear();
            txtCidade.Clear();
        }

        private void desabilitarCampos()
        {
            // Recursos relacionados a dados do voluntário
            txtNomeVoluntario.Enabled = false;
            txtNomeVoluntario.Enabled = false;
            txtRua.Enabled = false;
            txtNumero.Enabled = false;
            txtComplemento.Enabled = false;
            txtBairro.Enabled = false;
            mskCpf.Enabled = false;
            mskTelefone.Enabled = false;
            mskCep.Enabled = false;
            cbbEstado.Enabled = false;
            txtCidade.Enabled = false;

            // Recursos relacionados a dados de usuário
            ckbUsuarioAtivo.Enabled = false;
            txtUsuario.Enabled = false;
            txtSenha.Enabled = false;
            txtConfirmaSenha.Enabled = false;
            cbbTipoDeAcesso.Enabled = false;
        }

        private void habilitarCampos()
        {
            txtNomeVoluntario.Enabled = true;
            txtNomeVoluntario.Enabled = true;
            txtRua.Enabled = true;
            txtNumero.Enabled = true;
            txtComplemento.Enabled = true;
            txtBairro.Enabled = true;
            ckbUsuarioAtivo.Enabled = true;
            mskCpf.Enabled = true;
            mskTelefone.Enabled = true;
            mskCep.Enabled = true;
            cbbEstado.Enabled = true;
            txtCidade.Enabled = true;
        }

        private void desativarBotoes()
        {
            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnLimpar.Enabled = false;
            btnExcluir.Enabled = false;
        }


        // Integração com API do ViaCep para buscar endereço através do CEP
        private async void buscarEnderecoPorCep()
        {
            string cep = Regex.Replace(mskCep.Text, @"\D", "");

            if (cep.Length != 8)
            {
                MessageBox.Show("CEP inválido.");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // O viacep retorna dados apenas caso o cep eteja em sua base de dados
                    // Se~não houver dados sobre aquele cep, a api retorna erro.
                    string url = $"https://viacep.com.br/ws/{cep}/json/"; 

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Erro ao consultar o CEP.");
                        return;
                    }

                    string json = await response.Content.ReadAsStringAsync();

                    var endereco = JsonConvert.DeserializeObject<ViaCepResponse>(json);

                    if (endereco == null || endereco.erro)
                    {
                        MessageBox.Show("CEP não encontrado.");
                        return;
                    }

                    txtRua.Text = endereco.logradouro;
                    txtBairro.Text = endereco.bairro;
                    txtCidade.Text = endereco.localidade;
                    SelecionarEstadoPorUF(endereco.uf);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar CEP: " + ex.Message);
            }
        }

        private void SelecionarEstadoPorUF(string uf)
        {
            if (string.IsNullOrWhiteSpace(uf))
                return;

            uf = uf.Trim().ToUpper();

            for (int i = 0; i < cbbEstado.Items.Count; i++)
            {
                string item = cbbEstado.Items[i].ToString();

                if (item.EndsWith($"({uf})"))
                {
                    cbbEstado.SelectedIndex = i;
                    return;
                }
            }

            // Caso não encontre
            MessageBox.Show($"Estado com sigla '{uf}' não encontrado na lista.");

        }


        // Métodos de click dos botões e suas ações
        private void btnNovo_Click(object sender, EventArgs e) // Habilita o botão cadastrar e campos da janela para criação de um novo voluntário
        {
            habilitarCampos();
            btnCadastrar.Enabled = true;
            btnNovo.Enabled = false;
            btnLimpar.Enabled = true;
            txtNomeVoluntario.Focus();
        }

        private void btnCadastrar_Click(object sender, EventArgs e) // Com base no preenchimento dos campos da janela, faz o insert dos dados
        {
            if (txtNomeVoluntario.Text.Equals("") || mskTelefone.Text.Equals("") || mskCpf.Text.Equals("") || mskCep.Text.Equals("") || txtRua.Text.Equals("") || txtNumero.Text.Equals("") || txtBairro.Text.Equals("") || txtCidade.Text.Equals("")) // Falta adicionar mais validações
            {
                MessageBox.Show("Favor inserir valores nos campos vazios!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNomeVoluntario.Focus();
            }
            else if (txtNomeVoluntario.Text.Equals("") && mskTelefone.Text.Equals("") && mskCpf.Text.Equals("") && mskCep.Text.Equals("") && txtRua.Text.Equals("") && txtNumero.Text.Equals("") && txtRua.Text.Equals("") && txtCidade.Text.Equals("")) // Falta adicionar mais validações
            {
                MessageBox.Show("Todos os campos devem estar preenchidos!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNomeVoluntario.Focus();
            }
            else if (buscarVoluntarioPorDescricao(txtNomeVoluntario.Text, mskCpf.Text).Equals(1))
            {
                MessageBox.Show("Este registro já existe!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                limparCampos();
                desabilitarCampos();
                desativarBotoes();
                btnNovo.Enabled = true;
            }
            else
            {
                int resp = cadastrarVoluntario(txtNomeVoluntario.Text, mskTelefone.Text, mskCpf.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, txtCidade.Text, cbbEstado.SelectedItem.ToString());

                if (resp.Equals(1))
                {
                    MessageBox.Show("Cadastrado com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    desabilitarCampos();
                    desativarBotoes();
                    btnNovo.Enabled = true;
                    btnNovo.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao Cadastrar!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

                    limparCampos();
                    desabilitarCampos();
                    desativarBotoes();
                    btnNovo.Enabled = true;
                    btnNovo.Focus();

                }
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e) // Após pesquisar um voluntário e instânciar ele, permite alterar os dados
        {
            if (txtNomeVoluntario.Text.Equals("") || mskTelefone.Text.Equals("") || mskCpf.Text.Equals("") || mskCep.Text.Equals("") || txtRua.Text.Equals("") || txtNumero.Text.Equals("") || txtBairro.Text.Equals("") || txtCidade.Text.Equals("")) // Falta adicionar mais validações
            {
                MessageBox.Show("Favor inserir valores!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNomeVoluntario.Focus();
            }
            else
            {
                int resp = alterarDadosVoluntario(txtNomeVoluntario.Text, mskTelefone.Text, mskCpf.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, txtCidade.Text, cbbEstado.SelectedItem.ToString(), txtUsuario.Text, txtSenha.Text, codVol);

                if (resp.Equals(1))
                {
                    MessageBox.Show("dados do voluntário alterados com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    desabilitarCampos();
                    desativarBotoes();
                    btnNovo.Enabled = true;
                    btnNovo.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao alterar dados!", "Mensagem do sistema",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);

                    limparCampos();
                    desabilitarCampos();
                    desativarBotoes();
                    btnNovo.Enabled = true;
                }
            }

        }

        private void btnLimpar_Click(object sender, EventArgs e) // Aciona os métodos de limpeza de campos  e "reseta" a janela
        {
            limparCampos();
            desabilitarCampos();
            desativarBotoes();
            btnNovo.Enabled = true;
            ckbUsuarioAtivo.Checked = false;
        }

        private void btnExcluir_Click(object sender, EventArgs e) // Aciona o método de exclusão e limpeza de dados com base na instância dos dados de um voluntário
        {
            if (txtNomeVoluntario.Text.Equals("") || mskTelefone.Text.Equals("") || mskCpf.Text.Equals("") || mskCep.Text.Equals("") || txtRua.Text.Equals("") || txtNumero.Text.Equals("") || txtComplemento.Text.Equals("") || txtBairro.Text.Equals("") || txtCidade.Text.Equals(""))
            {
                MessageBox.Show("Não há usuário selecionado para exclusão!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            else
            {

            }

        }

        private void btnPesquisar_Click(object sender, EventArgs e) // Aciona a janela de pesquisa de voluntários
        {
            frmPesquisarVoluntarios abrir = new frmPesquisarVoluntarios();
            abrir.Show();
            this.Close();
        }

        private void btnVoltar_Click(object sender, EventArgs e) // Volta para a anela de Menu Principal
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Close();
        }

        private void mskCep_KeyDown(object sender, KeyEventArgs e) // Aciona o método de buscar endereço por CEP ao digitar o cep no campo e clicar na tecla "Enter"
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita beep do Enter
                if (mskCep.Text.Equals(""))
                {
                    MessageBox.Show("Erro ao buscar endereço, digite um CEP para buscar o endereço!", "Mensagem do sistema",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                    mskCep.Focus();
                }
                else
                {
                    buscarEnderecoPorCep();
                }
            }
        }

        private void ckbUsuarioAtivo_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbUsuarioAtivo.Checked)
            {
                txtUsuario.Enabled = true;
                txtSenha.Enabled = true;
                txtConfirmaSenha.Enabled = true;
                cbbTipoDeAcesso.Enabled = true;
            }
            else
            {
                txtUsuario.Enabled = false;
                txtSenha.Enabled = false;
                txtConfirmaSenha.Enabled = false;
                cbbTipoDeAcesso.Enabled = false;
            }
        }
    }
}
