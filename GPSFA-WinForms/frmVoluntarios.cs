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

        public frmVoluntarios(string text)
        {
            InitializeComponent();
            buscarDadosDoVoluntario(text);
            desativarBotoesNovo();
            desabilitarBotaoCadastrar();

            txtNomeVoluntario.Text = nomeVol;
            mskTelefone.Text = telCel;
            mskCpf.Text = cpf;
            mskCep.Text = cep;
            txtRua.Text = rua;
            txtNumero.Text = numero;
            txtComplemento.Text = complemento;
            txtBairro.Text = bairro;
            txtUsuario.Text = usuario;
            txtSenha.Text = senha;

            habilitarCampos();
        }

        // Instância global do código do voluntário
        int codVol = 0;
        string nomeVol;
        string telCel;
        string cpf;
        string cep;
        string rua;
        string numero;
        string complemento;
        string bairro;
        string usuario;
        string senha;

        public void buscarCodigoVoluntario(string descricao)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = $"SELECT codVol FROM tbVoluntarios WHERE nome = @descricao OR cpf = @descricao;";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Clear();

            comm.Parameters.Add("@descricao", MySqlDbType.VarChar, 100).Value = descricao;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            codVol = 0;

            while (DR.Read())
            {
                codVol = DR.GetInt32(0);
            }

            DataBaseConnection.CloseConnection();
        }

        private void buscarDadosDoVoluntario(string codVoluntario)
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
                nomeVol = DR.GetString(1);
                telCel = DR.GetString(2);
                cpf = DR.GetString(3);
                cep = DR.GetString(4);
                rua = DR.GetString(5);
                numero = DR.GetString(6);
                complemento = DR.GetString(7);
                bairro = DR.GetString(8);
                usuario = DR.GetString(11);
                senha = DR.GetString(12);
            }

            DataBaseConnection.CloseConnection();
        }

        // Métodos para habilitar ou desabilitar campos da janela
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
            cbbCidade.Dispose();
            cbbEstado.Dispose();
        }
        
        private void desabilitarCampos()
        {
            txtNomeVoluntario.Enabled = false;
            txtNomeVoluntario.Enabled = false;
            txtRua.Enabled = false;
            txtNumero.Enabled = false;
            txtComplemento.Enabled = false;
            txtBairro.Enabled = false;
            txtUsuario.Enabled = false;
            txtSenha.Enabled = false;
            mskCpf.Enabled = false;
            mskTelefone.Enabled = false;
            mskCep.Enabled = false;
            cbbCidade.Enabled = false;
            cbbEstado.Enabled = false;
        }

        private void habilitarCampos()
        {
            txtNomeVoluntario.Enabled = true;
            txtNomeVoluntario.Enabled = true;
            txtRua.Enabled = true;
            txtNumero.Enabled = true;
            txtComplemento.Enabled = true;
            txtBairro.Enabled = true;
            txtUsuario.Enabled = true;
            txtSenha.Enabled = true;
            mskCpf.Enabled = true;
            mskTelefone.Enabled = true;
            mskCep.Enabled = true;
            cbbCidade.Enabled = true;
            cbbEstado.Enabled = true;
        }

        // Métodos para desabilitar ou habilitar recursos da janela
        private void desativarBotoes()
        {
            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnLimpar.Enabled = false;
            btnExcluir.Enabled = false;
        }
        
        private void desabilitarBotaoCadastrar()
        {
            btnCadastrar.Enabled = false;
        }

        private void habilitarBotoesCadastrar()
        {
            btnCadastrar.Enabled = true;
        }

        private void desativarBotoesNovo()
        {
            btnNovo.Enabled = false;
        }

        // Conigurações dos botões e suas respectivas ações
        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            habilitarBotoesCadastrar();
            desativarBotoesNovo();
            txtNomeVoluntario.Focus();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            frmListaVoluntarios abrir = new frmListaVoluntarios();
            abrir.Show();
            this.Close();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Close();
        }

        private void frmVoluntarios_Load(object sender, EventArgs e)
        {

        }
    }
}
