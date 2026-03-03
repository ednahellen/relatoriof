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

        //Métodos construtores

        public frmGerenciarProdutos()
        {
            InitializeComponent();
            carregarOrigemCbb();
            carregarUnidadesCbb();
            carregarProdutosCbb();
            desativaBotoes();
            desativaCampos();
        }

        public frmGerenciarProdutos(int codUsu)
        {
            codUsuLogado = codUsu;
            InitializeComponent();
            carregarOrigemCbb();
            carregarUnidadesCbb();
            carregarProdutosCbb();
            desativaBotoes();
            desativaCampos();
            dtpDiaDistribuicao.Value = diaDeDistribuicao;
        }

        public frmGerenciarProdutos(int codUsu, string origemSelecionada)
        {
            nomeOrigem = origemSelecionada;
            codUsuLogado = codUsu;
            InitializeComponent();
            carregarOrigemCbb();
            carregarUnidadesCbb();
            carregarProdutosCbb();

            desativaBotoes();
            desativaCampos();

            cbbOrigemDoacao.Text = nomeOrigem;
            dtpDiaDistribuicao.Value = diaDeDistribuicao;
        }

        // Variavel global da janela para salvar o código do usuário logado
        int codUsuLogado;

        // Variavel global da janela para salvar o código de produto da tabela TBLista
        int codList;

        // Variavel global da janela para salvar o código de Origem da tabela TBOrigemDoacao
        int codOri;

        // Variavel global da janela para salvar o código de Origem da tabela TBOrigemDoacao
        string nomeOrigem;

        int codOrigem;

        //Desativando botão fechar da janela

        private void frmCadastrarAlimentos_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);
        }

        //Método para carregar Origem na CBBOrigem

        private void carregarOrigemCbb()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT nome FROM TBOrigemDoacao ORDER BY nome ASC;";
            comm.CommandType = CommandType.Text;
            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbOrigemDoacao.Items.Add(DR.GetString(0));
            }

            DataBaseConnection.CloseConnection();
        }

        //Método para carregar Unidades na CBBUnidade

        private void carregarUnidadesCbb()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT descricao FROM tbUnidades ORDER BY descricao ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbUnidadeMedida.Items.Add(DR.GetString(0));
            }

            DataBaseConnection.CloseConnection();
        }

        //Método para carregar Descrição dos Produtos na CBBDescrição

        private void carregarProdutosCbb()
        {
            cbbDescricao.Items.Clear();
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT descricao FROM tbLista ORDER BY descricao ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbDescricao.Items.Add(DR.GetString(0));
            }

            DataBaseConnection.CloseConnection();
        }

        //Método para validar DataLimite de Saída

        int diaDeArrecadacao = DateTime.Now.Day;

        DateTime diaDeDistribuicao = DateTime.Now.AddDays(7);

        private void validaSaida()
        {
            int dataValidade = Convert.ToInt32(dtpDataValidade.Value.Day);

            int calculoData = Convert.ToInt32(dtpDiaDistribuicao.Value.Day);
        }

        //Método para verificar Formatação de Campos

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
                MessageBox.Show("O período para cadastro de doação excedeu!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        //Método para cadastrar Produtos na TBPRODUTOS

        private int cadastrarProdutos(string descricao, int quantidade, decimal peso, string unidade, string codBar, DateTime dataDeEntrada, DateTime dataDeValidade, DateTime dataLimiteDeSaida, int codUsu, int codOri, int codList)
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
                MessageBox.Show("Este registro já existe!", "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            return 0;
        }


        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            if (dtpDataValidade.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Data de validade inválida.");
                return;
            }

            using (var conn = DataBaseConnection.OpenConnection())

                if (dtpDataValidade.Value < DateTime.Today)
                {
                    MessageBox.Show("A data de validade não pode ser anterior a data atual!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpDataValidade.Focus();
                    return;
                }

                else if (cadastrarProdutos(cbbDescricao.Text, Convert.ToInt32(txtQuantidade.Text), Convert.ToInt32(txtPeso.Text), cbbUnidadeMedida.Text, txtCodBarras.Text, dtpDataEntrada.Value, dtpDataValidade.Value, dtpDataEntrada.Value, codUsuLogado, codOri, codList).Equals(1))
                {
                    // Verifica se já existe produto
                    string sqlVerifica = "SELECT codProd, estoqueAtual FROM tbProdutos WHERE codBar = @codBar";

                    using (var cmdVerifica = new MySqlCommand(sqlVerifica, conn))
                    {
                        cmdVerifica.Parameters.AddWithValue("@codBar", txtCodBarras.Text);

                        using (var reader = cmdVerifica.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int codProd = reader.GetInt32("codProd");
                                int qtdAtual = reader.GetInt32("estoqueAtual");
                                reader.Close();

                                // Atualiza estoque
                                string sqlUpdate = "UPDATE tbProdutos SET estoqueAtual = @novaQtd WHERE codProd = @codProd";

                                using (var cmdUpdate = new MySqlCommand(sqlUpdate, conn))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@novaQtd",
                                        qtdAtual + Convert.ToInt32(txtQuantidade.Text));
                                    cmdUpdate.Parameters.AddWithValue("@codProd", codProd);
                                    cmdUpdate.ExecuteNonQuery();
                                }

                                MessageBox.Show("Quantidade atualizada no estoque.");

                                limparCampos();

                                return;


                            }
                        }
                    }

                    // Se não existir, insere novo produto
                    string sqlInsert = @"INSERT INTO tbProdutos
                             (descricao, quantidade, peso, unidade, codBar,
                              dataDeEntrada, dataDeValidade, codUsu, codOri, codList)
                             VALUES
                             (@descricao, @qtd, @peso, @un, @codBar,
                              @entrada, @validade, @codUsu, @codOri, @codList)";

                    using (var cmdInsert = new MySqlCommand(sqlInsert, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@descricao", cbbDescricao.Text);
                        cmdInsert.Parameters.AddWithValue("@qtd", Convert.ToInt32(txtQuantidade.Text));
                        cmdInsert.Parameters.AddWithValue("@peso", Convert.ToDecimal(txtPeso.Text));
                        cmdInsert.Parameters.AddWithValue("@un", cbbUnidadeMedida.Text);
                        cmdInsert.Parameters.AddWithValue("@codBar", txtCodBarras.Text);
                        cmdInsert.Parameters.AddWithValue("@entrada", DateTime.Now);
                        cmdInsert.Parameters.AddWithValue("@validade", dtpDataValidade.Value);
                        cmdInsert.Parameters.AddWithValue("@codUsu", codUsuLogado);
                        cmdInsert.Parameters.AddWithValue("@codOri", 1);
                        cmdInsert.Parameters.AddWithValue("@codList", 1);

                        cmdInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produto cadastrado com sucesso.");

                    limparCampos();
                }

                else if (cadastrarProdutos(cbbDescricao.Text, Convert.ToInt32(txtQuantidade.Text), Convert.ToInt32(txtPeso.Text), cbbUnidadeMedida.Text, txtCodBarras.Text, dtpDataEntrada.Value, dtpDataValidade.Value, dtpDataEntrada.Value, codUsuLogado, codOri, codList).Equals(1))

                {
                    // Verifica se já existe produto
                    string sqlVerifica = "SELECT codProd, quantidade FROM tbProdutos WHERE codBar = @codBar";

                    using (var cmdVerifica = new MySqlCommand(sqlVerifica, conn))
                    {
                        cmdVerifica.Parameters.AddWithValue("@codBar", txtCodBarras.Text);

                        using (var reader = cmdVerifica.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int codProd = reader.GetInt32("codProd");
                                int qtdAtual = reader.GetInt32("quantidade");
                                reader.Close();

                                // Atualiza estoque
                                string sqlUpdate = "UPDATE tbProdutos SET quantidade = @novaQtd WHERE codProd = @codProd";

                                using (var cmdUpdate = new MySqlCommand(sqlUpdate, conn))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@novaQtd",
                                        qtdAtual + Convert.ToInt32(txtQuantidade.Text));
                                    cmdUpdate.Parameters.AddWithValue("@codProd", codProd);
                                    cmdUpdate.ExecuteNonQuery();
                                }

                                MessageBox.Show("Quantidade atualizada no estoque.");
                                return;
                            }
                        }
                    }

                    // Se não existir, insere novo produto
                    string sqlInsert = @"INSERT INTO tbProdutos(descricao, quantidade, peso, unidade, codBar,dataDeEntrada, dataDeValidade, codUsu, codOri, codList)
                                            VALUES (@descricao, @qtd, @peso, @un, @codBar,@entrada, @validade, @codUsu, @codOri, @codList)";

                    using (var cmdInsert = new MySqlCommand(sqlInsert, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@descricao", cbbDescricao.Text);
                        cmdInsert.Parameters.AddWithValue("@qtd", Convert.ToInt32(txtQuantidade.Text));
                        cmdInsert.Parameters.AddWithValue("@peso", Convert.ToDecimal(txtPeso.Text));
                        cmdInsert.Parameters.AddWithValue("@un", cbbUnidadeMedida.Text);
                        cmdInsert.Parameters.AddWithValue("@codBar", txtCodBarras.Text);
                        cmdInsert.Parameters.AddWithValue("@entrada", DateTime.Now);
                        cmdInsert.Parameters.AddWithValue("@validade", dtpDataValidade.Value);
                        cmdInsert.Parameters.AddWithValue("@codUsu", codUsuLogado);
                        cmdInsert.Parameters.AddWithValue("@codOri", 1);
                        cmdInsert.Parameters.AddWithValue("@codList", 1);

                        cmdInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produto cadastrado com sucesso.");
                }

        }

        // Criada instância das janelas com o código do usuário imbutido
        private void btnDoacao_Click(object sender, EventArgs e)
        {
            frmOrigemDoacao abrir = new frmOrigemDoacao(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            frmListaProdutos abrir = new frmListaProdutos(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        private void btnMedida_Click(object sender, EventArgs e)
        {
            frmUnidadeMedida abrir = new frmUnidadeMedida(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        private void gpbCamposDoProduto_Enter(object sender, EventArgs e)
        {

        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void cbbDescricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbDescricao.SelectedItem == null) return;

            string nomeSelecionado = cbbDescricao.SelectedItem.ToString();

            cbbUnidadeMedida.Items.Clear();
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT codList, peso, unidade FROM tbLista WHERE descricao = @descricao;";
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@descricao", nomeSelecionado);

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            if (DR.Read())
            {
                codList = DR.GetInt32(0);
                txtPeso.Text = DR.GetInt32(1).ToString();
                cbbUnidadeMedida.Text = DR.GetString(2);
                cbbUnidadeMedida.Enabled = false;
                txtPeso.Enabled = false;
            }

            DataBaseConnection.CloseConnection();
        }

        private void cbbOrigemDoacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbOrigemDoacao.SelectedItem == null) return;

            string origemSelecionada = cbbOrigemDoacao.SelectedItem.ToString();

            cbbUnidadeMedida.Items.Clear();
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT codOri FROM tborigemdoacao WHERE nome = @nome;";
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddWithValue("@nome", origemSelecionada);

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            if (DR.Read())
            {
                codOri = DR.GetInt32(0);
            }

            DataBaseConnection.CloseConnection();
        }

        private void txtQuantidade_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public void desativaBotoes()
        {
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnPesquisar.Enabled = false;
            btnCadastrar.Enabled = false;
            btnLimpar.Enabled = false;
        }

        public void desativaCampos()
        {
            txtCodBarras.Enabled = false;
            cbbOrigemDoacao.Enabled = false;
            cbbDescricao.Enabled = false;
            btnLista.Enabled = false;
            btnDoacao.Enabled = false;
            btnMedida.Enabled = false;
            txtPeso.Enabled = false;
            txtPeso.Enabled = false;
            cbbUnidadeMedida.Enabled = false;
            dtpDiaDistribuicao.Enabled = false;
            dtpDataEntrada.Enabled = false;
            dtpDataValidade.Enabled = false;
            txtQuantidade.Enabled = false;
        }

        public void habilitaCampos()
        {
            txtCodBarras.Enabled = false;
            cbbOrigemDoacao.Enabled = true;
            cbbDescricao.Enabled = true;
            btnLista.Enabled = true;
            btnDoacao.Enabled = true;
            btnMedida.Enabled = true;
            txtPeso.Enabled = true;
            cbbUnidadeMedida.Enabled = true;
            dtpDiaDistribuicao.Enabled = true;
            dtpDataEntrada.Enabled = true;
            dtpDataValidade.Enabled = true;
            txtQuantidade.Enabled = true;
        }

        public void limparCampos()
        {
            txtCodBarras.Clear();
            cbbDescricao.Text = "";
            cbbUnidadeMedida.Text = "";
            txtPeso.Clear();
            txtQuantidade.Clear();
            cbbUnidadeMedida.Text = "";
            btnNovo.Enabled = true;
        }

        private void txtPeso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitaCampos();
            btnCadastrar.Enabled = true;
            limparCampos();
            btnNovo.Enabled = false;
        }
    }

}