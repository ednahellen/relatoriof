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
            desabilitarCamposVoluntario();
            desabilitarCamposUsuario();
        }
        
        // Instância da Janela com variável imbutida
        public frmVoluntarios(string text)
        {
            InitializeComponent();
            codVolSelected = Convert.ToInt32(text);
            buscarDadosDoVoluntarioPeloCodigo(codVolSelected);
            buscarUsuarioPorCodVol(codVolSelected);
            habilitarCamposVoluntario();
            desabilitarCamposUsuario();
            btnNovo.Enabled = false;
            btnCadastrar.Enabled = false;
            ckbEditarUsuario.Checked = false;
        }

        //    -----    Variaveis globais para edição dos dados do voluntário / usuário
        int codVolSelected = 0; // Código do voluntário
        bool isVoluntarioActive; // Estado do voluntário
        bool isUsuarioActive; // Estado do usuário do voluntário


//    -----    Métodos para ações CRUD e queries do banco de dados

// Busca os dados de um voluntário através do código - para busca de dados exata
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
                isVoluntarioActive = DR.GetBoolean(11);
                codVolSelected = DR.GetInt32(0);

                txtNomeVoluntario.Text = DR.GetString(1);
                mskTelefone.Text = DR.GetString(2);
                mskCpf.Text = DR.GetString(3);
                mskCep.Text = DR.GetString(4);
                txtRua.Text = DR.GetString(5);
                txtNumero.Text = DR.GetString(6);
                txtComplemento.Text = DR.GetString(7);
                txtBairro.Text = DR.GetString(8);
                txtCidade.Text = DR.GetString(9);
                SelecionarEstadoPorUF(DR.GetString(10));
            }

            DataBaseConnection.CloseConnection();
        }

        // Busca dados de um usuario com base no código do voluntário
        private void buscarUsuarioPorCodVol(int codVoluntario)
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
                rdbtnUsuarioAtivo.Checked = true;
                rdbtnUsuarioDesativado.Checked = false;
            }
            else if (isUsuarioActive == false)
            {
                rdbtnUsuarioDesativado.Checked = true;
                rdbtnUsuarioAtivo.Checked = false;
            }
        }

        // Com base nos dados retornados do usuário, faz uma busca do seu tipo de acesso e seleciona a opção equivalente da combobox
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

        // Busca os dados de um voluntário com base no nome e/ou cpf
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
        
        // Busca o código do voluntário através do CPF
        private void buscarCodVolPorCPF(string volCpf)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT codVol FROM tbVoluntarios WHERE cpf = @volCpf;";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Clear();

            comm.Parameters.Add("@volCpf", MySqlDbType.VarChar).Value = volCpf;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            while (DR.Read())
            {
                codVolSelected = DR.GetInt32(0);
            }
        }

        // Cria um novo registro de voluntário na tabela de voluntários
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

        // Edita os dados de um voluntário na tabela de voluntários e os atualiza no seu registro
        public int editarVoluntario(string nome, string telCel, string cpf, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado, string usuario, string senha, int codVol)
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
        
        // Apaga os dados do registro do voluntário, mas não apaga o registro na tabela de voluntários
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
        
        // Cria um novo usuário associado e o associa a um voluntário por meio do código dele
        public int cadastrarUsuario(bool isActive, string usuario, string senha, string tipoAcesso, int codVol)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "INSERT INTO tbUsuarios(ativo, usuario, senha, tipo, salt, codVol)VALUES(@isActive, @usuario, @senha, @tipo, @salt, @codVol);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Add("@isActive", MySqlDbType.Byte).Value = isActive;
            comm.Parameters.Add("@usuario", MySqlDbType.VarChar, 20).Value = usuario;
            comm.Parameters.Add("@senha", MySqlDbType.VarChar, 20).Value = senha;
            comm.Parameters.Add("@tipo", MySqlDbType.VarChar, 20).Value = tipoAcesso;
            comm.Parameters.Add("@salt", MySqlDbType.VarChar, 20).Value = "salt-teste";
            comm.Parameters.Add("@codVol", MySqlDbType.Int32).Value = codVol;

            comm.Connection = DataBaseConnection.OpenConnection();

            try
            {
                int resp = comm.ExecuteNonQuery();

                DataBaseConnection.CloseConnection();

                return resp;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Erro ao cadastrar usuário: {error}", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
        }

        // Edita os dados de um usuário com base no código do voluntário
        public int editarUsuario(int codVol, string usuario, string senha)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "UPDATE tbUsuarios SET usuario = @usuario, senha = @senha WHERE codVol = @codVol";
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
    


        //    -----    Métodos para limpeza de dados da janela

        // Limpa somente os campos de voluntario
        private void limparCamposVoluntario()
        {
            codVolSelected = 0;
            isVoluntarioActive = true;

            txtNomeVoluntario.Clear();
            txtNomeVoluntario.Focus();
            mskTelefone.Clear();
            mskCpf.Clear();
            txtRua.Clear();
            txtNumero.Clear();
            txtComplemento.Clear();
            mskCep.Clear();
            txtBairro.Clear();
            cbbEstado.Items.Clear();
            txtCidade.Clear();
        }

        // limpa somente os campos de usuario
        private void limparCamposUsuario()
        {
            isUsuarioActive = true;

            txtUsuario.Clear();
            txtSenha.Clear();
            txtConfirmaSenha.Clear();
            rdbtnUsuarioAtivo.Checked = false;
            rdbtnUsuarioDesativado.Checked = false;
            cbbTipoDeAcesso.SelectedItem = null;
        }


        //    -----    Metodos para desabilitar ou habilitar campos da janela
        
        // Desabilita somente campos de voluntário
        private void desabilitarCamposVoluntario()
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
            ckbEditarUsuario.Enabled = false;
        }

        // Habilita somente campos de voluntário
        private void habilitarCamposVoluntario()
        {
            txtNomeVoluntario.Enabled = true;
            txtNomeVoluntario.Enabled = true;
            txtRua.Enabled = true;
            txtNumero.Enabled = true;
            txtComplemento.Enabled = true;
            txtBairro.Enabled = true;
            mskCpf.Enabled = true;
            mskTelefone.Enabled = true;
            mskCep.Enabled = true;
            cbbEstado.Enabled = true;
            txtCidade.Enabled = true;
            ckbEditarUsuario.Enabled = true;
        }

        // Desabilita somente os campos de usuário
        private void desabilitarCamposUsuario()
        {
            txtUsuario.Enabled = false;
            txtSenha.Enabled = false;
            txtConfirmaSenha.Enabled = false;
            cbbTipoDeAcesso.Enabled = false;
            rdbtnUsuarioAtivo.Enabled = false;
            rdbtnUsuarioDesativado.Enabled = false;
        }

        // Habilita somente os campos de usuário
        private void habilitarCamposUsuario()
        {
            txtUsuario.Enabled = true;
            txtSenha.Enabled = true;
            txtConfirmaSenha.Enabled = true;
            cbbTipoDeAcesso.Enabled = true;
            rdbtnUsuarioAtivo.Enabled = true;
            rdbtnUsuarioDesativado.Enabled = true;
        }

        // Desabilita botões da janela
        private void desativarBotoes()
        {
            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnLimpar.Enabled = false;
            btnExcluir.Enabled = false;
        }


        //    -----    Métodos para integrações com APIs externas

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

        // Seleciona um estado da lista, com base na UF passada no método
        private void SelecionarEstadoPorUF(string uf) 
        {
            if (string.IsNullOrWhiteSpace(uf))
                return;

            uf = uf.Trim().ToUpper();

            for (int i = 0; i < cbbEstado.Items.Count; i++)
            {
                string item = cbbEstado.Items[i].ToString();

                if (item.StartsWith($"{uf}"))
                {
                    cbbEstado.SelectedIndex = i;
                    return;
                }
            }

            // Caso não encontre
            MessageBox.Show($"Estado com sigla '{uf}' não encontrado na lista.");

        }


        //    -----    Eventos diversos de botões e outros recursos da janela 

        // Evento de clique do botão "Novo"
        private void btnNovo_Click(object sender, EventArgs e)
        {
            // Habilita os campos de voluntário e check box de campos de usuário
            habilitarCamposVoluntario();
            btnCadastrar.Enabled = true;
            btnNovo.Enabled = false;
            btnLimpar.Enabled = true;
            txtNomeVoluntario.Focus();
        }

        // Evento de clique do botão de cadastrar
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            //    -----    Primeira etapa de validações -> dados de voluntário
            // Valida se algum dos campos de voluntário, estão preenchidos
            if (txtNomeVoluntario.Text.Equals("") || mskTelefone.Text.Equals("") || mskCpf.Text.Equals("") || mskCep.Text.Equals("") || txtRua.Text.Equals("") || txtNumero.Text.Equals("") || txtBairro.Text.Equals("") || txtCidade.Text.Equals("")) // Falta adicionar mais validações
            {
                MessageBox.Show("Preencha os campos vazios para continuar!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                txtNomeVoluntario.Focus();
            }
            
            
            //    -----    Segunda etapa de validações -> Os campos do voluntário estão preenchidos
            else
            {
                // Valida se a edição de usuário está desabilitada e limpa seus campos> CADASTRAR APENAS O VOLUNTÁRIO
                if (ckbEditarUsuario.Checked == false)
                {
                    limparCamposUsuario();

                    // Mensagem de confirmação para cadastrar voluntário sem usuário
                    DialogResult resultado = MessageBox.Show("O voluntário será cadastrado SEM UM USUÁRIO.\nDeseja continuar?\n\nObs: A opção 'Editar usuário' deve estar habilitada", "Mensagem do sistema",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    // Se o usuário clicar em SIM > Segue com a criação de voluntário sem usuário
                    if (resultado == DialogResult.Yes)
                    {
                        //  Valida se o voluntário já existe, com base no nome e cpf
                        if (buscarVoluntarioPorDescricao(txtNomeVoluntario.Text, mskCpf.Text).Equals(1))
                        {
                            MessageBox.Show("Este Voluntário já existe!", "Mensagem do sistema",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1);
                            limparCamposVoluntario();
                            desabilitarCamposVoluntario();
                            limparCamposUsuario();
                            desabilitarCamposUsuario();
                            desativarBotoes();
                            btnNovo.Enabled = true;
                        }
                        
                        // Segue por aqui caso não exista registro do voluntário
                        else
                        {
                            // Faz a criação de registro na tabela de voluntários com os dados da janela
                            int resp = cadastrarVoluntario(txtNomeVoluntario.Text, mskTelefone.Text, mskCpf.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, txtCidade.Text, cbbEstado.SelectedItem.ToString());

                            // Se houver sucesso na criação do registro retorna mensagem de sucesso
                            if (resp.Equals(1))
                            {
                                MessageBox.Show("Voluntário cadastrado com sucesso!", "Mensagem do sistema",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);
                                limparCamposVoluntario();
                                desabilitarCamposVoluntario();
                                limparCamposUsuario();
                                desabilitarCamposUsuario();
                                desativarBotoes();
                                desativarBotoes();
                                btnNovo.Enabled = true;
                                btnNovo.Focus();
                            }
                            else 
                            {
                                // Se houver alguma falha na criação do registro é retornada mensagem de erro
                                MessageBox.Show("Erro ao Cadastrar Voluntário!", "Mensagem do sistema",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1);

                                limparCamposVoluntario();
                                desabilitarCamposVoluntario();
                                limparCamposUsuario();
                                desabilitarCamposUsuario();
                                desativarBotoes();
                                btnNovo.Enabled = true;
                                btnNovo.Focus();
                            }
                        }
                    }

                    // Caso o usuário selecione NÃO na mensagem de confirmação para criação de voluntário sem usuário,
                    // o sistema não segue com a criação do voluntário
                    else if (resultado == DialogResult.No)
                    {
                        // Após isso, é colocado em foco o check box para habilitar a edição de dados de usuário
                        ckbEditarUsuario.Focus();
                        return;
                    }
                }

                //    -----    Terceira etapa de validações - Para cadastro de Voluntário + Usuário
                else
                {
                    // Atribui o valor TRUE ou FALSE para a variável global que guarda o estado do usuário (se ativo ou não) com base no radio button selecionado
                    if (rdbtnUsuarioAtivo.Checked)
                    {
                        isUsuarioActive = true;
                    }
                    else if (rdbtnUsuarioDesativado.Checked)
                    {
                        isUsuarioActive = false;
                    }

                    // Valida se os campos de usuário estão preenchidos
                    if (txtUsuario.Text.Equals("") || txtSenha.Text.Equals("") || txtConfirmaSenha.Text.Equals("") || cbbTipoDeAcesso.SelectedItem == null || (!rdbtnUsuarioAtivo.Checked && !rdbtnUsuarioDesativado.Checked))
                    {
                        MessageBox.Show("Preencha os campos vazios para continuar!", "Mensagem do sistema",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                    }

                    // Se todos os campos de usuário estiverem preenchidos ou selecionados segue para a próxima validação
                    else
                    {
                        // Confirmação para cadastrar voluntário com usuário desativado
                        if (isUsuarioActive == false)
                        {
                            DialogResult resultado = MessageBox.Show("O usuário será cadastrado como DESATIVADO.\nDeseja continuar?", "Mensagem do sistema",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            // Segue com o cadastro de voluntário + usuário desativado
                            if (resultado == DialogResult.Yes)
                            {
                                //  Valida se o voluntário já existe, com base no nome e cpf
                                if (buscarVoluntarioPorDescricao(txtNomeVoluntario.Text, mskCpf.Text).Equals(1))
                                {
                                    MessageBox.Show("Este Voluntário já existe!", "Mensagem do sistema",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                                    limparCamposVoluntario();
                                    desabilitarCamposVoluntario();
                                    limparCamposUsuario();
                                    desabilitarCamposUsuario();
                                    desativarBotoes();
                                    btnNovo.Enabled = true;
                                }
                                else
                                {   
                                    // Se o voluntário não tiver registro no banco, o sistema segue validando se as senhas são iguais
                                    if (!txtConfirmaSenha.Text.Equals(txtSenha.Text))
                                    {
                                        MessageBox.Show("As senhas devem ser iguais!", "Mensagem do sistema",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning,
                                            MessageBoxDefaultButton.Button1);
                                    }
                                    else
                                    {
                                        // realiza o cadastro do voluntário e usuário - mas com o usuário desativado
                                        int volResp = cadastrarVoluntario(txtNomeVoluntario.Text, mskTelefone.Text, mskCpf.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, txtCidade.Text, cbbEstado.SelectedItem.ToString());

                                        // Retorno se o cadastro de voluntário foi bem sucedido
                                        if (volResp == 1) 
                                        {
                                            // Busca o código do voluntário cadastrado para criação do usuário
                                            buscarCodVolPorCPF(mskCpf.Text);

                                            // Faz a criação do usuário com os dados dos campos e código do voluntário registrado salvo globalmente
                                            int userResp = cadastrarUsuario(isUsuarioActive, txtUsuario.Text, txtSenha.Text, cbbTipoDeAcesso.SelectedItem.ToString(), codVolSelected);

                                            // Retorno se a criação de usuário for bem sucedida
                                            if (userResp == 1)
                                            {
                                                // Mensagem de sucesso na criação de voluntário e usuário
                                                MessageBox.Show("Voluntário cadastrado e Usuário criado!", "Mensagem do sistema",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information,
                                                    MessageBoxDefaultButton.Button1);

                                                limparCamposVoluntario();
                                                desabilitarCamposVoluntario();
                                                limparCamposUsuario();
                                                desabilitarCamposUsuario();
                                                desativarBotoes();
                                                desativarBotoes();
                                                btnNovo.Enabled = true;
                                                btnNovo.Focus();
                                            }

                                            else
                                            {
                                                // Mensagem de erro na criação do usuário
                                                MessageBox.Show("Erro ao cadastrar Usuário!", "Mensagem do sistema",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error,
                                                MessageBoxDefaultButton.Button1);

                                                limparCamposVoluntario();
                                                desabilitarCamposVoluntario();
                                                limparCamposUsuario();
                                                desabilitarCamposUsuario();
                                                desativarBotoes();
                                                btnNovo.Enabled = true;
                                                btnNovo.Focus();
                                            }
                                        }
                                        else
                                        {
                                            // Mensagem de erro na criação do Voluntário
                                            MessageBox.Show("Erro ao cadastrar Voluntário!", "Mensagem do sistema",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1);

                                            limparCamposVoluntario();
                                            desabilitarCamposVoluntario();
                                            limparCamposUsuario();
                                            desabilitarCamposUsuario();
                                            desativarBotoes();
                                            btnNovo.Enabled = true;
                                            btnNovo.Focus();
                                        }
                                    }
                                }
                            }

                            // Caso o usuário selecione a opção NÃO para criação de voluntário + usuário desativado,
                            // o sistema retorna para a janela para que sejam aplicadas alterações
                            else if (resultado == DialogResult.No)
                            {
                                rdbtnUsuarioAtivo.Focus();
                                return;
                            }
                        }
                            
                        // realiza o cadastro do voluntário e usuário - mas com o usuário ativo > isUsuarioActive == true
                        else
                        {
                            // Busca se já há registro de voluntário no banco com base em nome e cpf
                            if (buscarVoluntarioPorDescricao(txtNomeVoluntario.Text, mskCpf.Text).Equals(1))
                            {
                                MessageBox.Show("Este Voluntário já existe!", "Mensagem do sistema",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);
                                limparCamposVoluntario();
                                desabilitarCamposVoluntario();
                                limparCamposUsuario();
                                desabilitarCamposUsuario();
                                desativarBotoes();
                                btnNovo.Enabled = true;
                            }
                            else
                            {
                                // Valida se as senhas são iguais
                                if (!txtConfirmaSenha.Text.Equals(txtSenha.Text))
                                {
                                    MessageBox.Show("As senhas devem ser iguais!", "Mensagem do sistema",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button1);
                                }
                                else
                                {
                                    // Realiza a criação de Voluntário no banco
                                    int volResp = cadastrarVoluntario(txtNomeVoluntario.Text, mskTelefone.Text, mskCpf.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, txtCidade.Text, cbbEstado.SelectedItem.ToString());

                                    // Se a criação do voluntário for bem sucedida
                                    if (volResp == 1)
                                    {
                                        // Faz a busca do código do voluntário criado utilizando o cpf
                                        buscarCodVolPorCPF(mskCpf.Text);

                                        // Realiza a criação do usuário a partir do código do voluntário capturado
                                        int userResp = cadastrarUsuario(isUsuarioActive, txtUsuario.Text, txtSenha.Text, cbbTipoDeAcesso.SelectedItem.ToString(), codVolSelected);

                                        // Se a criação do usuário for bem sucedida - é criado voluntário + usuário ativo
                                        if (userResp == 1)
                                        {
                                            MessageBox.Show("Voluntário cadastrado e Usuário criado!", "Mensagem do sistema",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information,
                                                MessageBoxDefaultButton.Button1);

                                            limparCamposVoluntario();
                                            desabilitarCamposVoluntario();
                                            limparCamposUsuario();
                                            desabilitarCamposUsuario();
                                            desativarBotoes();
                                            desativarBotoes();
                                            btnNovo.Enabled = true;
                                            btnNovo.Focus();
                                        }

                                        else
                                        {
                                            // Mensagem de erro na criação de usuário
                                            MessageBox.Show("Erro ao cadastrar Usuário!", "Mensagem do sistema",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1);

                                            limparCamposVoluntario();
                                            desabilitarCamposVoluntario();
                                            limparCamposUsuario();
                                            desabilitarCamposUsuario();
                                            desativarBotoes();
                                            btnNovo.Enabled = true;
                                            btnNovo.Focus();
                                        }
                                    }
                                    else
                                    {
                                        // Mensagem de erro na criação de voluntário
                                        MessageBox.Show("Erro ao Cadastrar Voluntário!", "Mensagem do sistema",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1);

                                        limparCamposVoluntario();
                                        desabilitarCamposVoluntario();
                                        limparCamposUsuario();
                                        desabilitarCamposUsuario();
                                        desativarBotoes();
                                        btnNovo.Enabled = true;
                                        btnNovo.Focus();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Após pesquisar um voluntário e instânciar ele, permite alterar os dados
        private void btnAlterar_Click(object sender, EventArgs e)
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
                int resp = editarVoluntario(txtNomeVoluntario.Text, mskTelefone.Text, mskCpf.Text, mskCep.Text, txtRua.Text, txtNumero.Text, txtComplemento.Text, txtBairro.Text, txtCidade.Text, cbbEstado.SelectedItem.ToString(), txtUsuario.Text, txtSenha.Text, codVolSelected);

                if (resp.Equals(1))
                {
                    MessageBox.Show("dados do voluntário alterados com sucesso!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                    desativarBotoes();
                    desabilitarCamposVoluntario();
                    //desabilitarCamposUsuario();
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

                    limparCamposVoluntario();
                    desabilitarCamposVoluntario();
                    desativarBotoes();
                    btnNovo.Enabled = true;
                }
            }

        }

        // Aciona os métodos de limpeza dos campos e "reseta" a janela - finalizado
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCamposVoluntario();
            desabilitarCamposVoluntario();
            limparCamposUsuario();
            desabilitarCamposUsuario();
            desativarBotoes();
            btnNovo.Enabled = true;
            ckbEditarUsuario.Checked = false;
        }

        // Aciona o método de exclusão e limpeza de dados com base na instância dos dados de um voluntário
        private void btnExcluir_Click(object sender, EventArgs e)
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

        // Aciona a janela de pesquisa de voluntários - finalizado
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            frmPesquisarVoluntarios abrir = new frmPesquisarVoluntarios();
            abrir.Show();
            this.Close();
        }

        // Volta para a anela de Menu Principal - finalizado
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Close();
        }

        // Aciona o método de buscar endereço por CEP ao digitar o cep no campo e clicar na tecla "Enter" - finalizado
        private void mskCep_KeyDown(object sender, KeyEventArgs e)
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

        // Habilita ou desabilita os campos de usuarios - finalizado
        private void ckbEditarUsuario_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEditarUsuario.Checked)
            {
                habilitarCamposUsuario();
            }
            else if (ckbEditarUsuario.Checked == false)
            {
                desabilitarCamposUsuario();
            }
        }
    }
}
