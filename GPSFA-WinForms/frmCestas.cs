using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
    public partial class frmCestas : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmCestas()
        {
            InitializeComponent();
        }
        public frmCestas(int codUsu)
        {
            codUsuLogado = codUsu;
            InitializeComponent();
            ConfigDgvItensDaCesta();
            carregarModelosDeCestaNaComboBox();
        }

        int codUsuLogado, codModeloCestaSelecionado;

        // MÉTODOS DE QUERIES NO BANCO DE DADOS
        // Carrega os presets de cesta básica configurados no banco de dados - OK
        private void carregarModelosDeCestaNaComboBox()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT * FROM tbModeloCesta ORDER BY descricao ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbModeloDeCesta.Items.Add(DR.GetString(1));
            }

            DataBaseConnection.CloseConnection();
        }

        // Busca o código do modelo de cesta pela descrição - OK
        private void buscarCodModeloPorDescricao(string cestaModeloNome)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT codModelo FROM tbModeloCesta WHERE descricao = @descricao;";

                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = cestaModeloNome;

                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    if (DR.Read())
                    {
                        if (DR.GetInt32("codModelo") > 0)
                        {
                            codModeloCestaSelecionado = DR.GetInt32("codModelo");

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

        // Carregar dados no datagrid view ao selecionar o modelo de cesta e a quantidade de cestas - OK
        private void carregarDadosNoDgvItensDaCesta(int codModelo)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT l.codList, l.descricao, imc.quantidadeMinima, IFNULL(SUM(p.quantidade), 0) AS estoqueAtual FROM tbItensDoModeloCesta imc INNER JOIN tbLista l ON l.codList = imc.codList LEFT JOIN tbEstoqueItens p ON p.codList = l.codList WHERE imc.codModelo = 1 GROUP BY imc.codModelo, imc.codList, l.descricao, l.unidade, imc.quantidadeMinima;";
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@codModelo", MySqlDbType.Int32).Value = codModelo;

                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        dgvItensDaCesta.Rows.Add(
                            DR["codList"].ToString(),
                            DR["descricao"].ToString(),
                            DR["quantidadeMinima"].ToString(),
                            DR["estoqueAtual"].ToString()
                        );
                    }

                    DataBaseConnection.CloseConnection();
                }
            }
        }

        // Faz a criação de cestas com base na quantidade e registra itens em uma cesta - OK
        private void montarCestas(int quantidadeDeCestas, int codUsu)
        {
            try
            {
                MySqlCommand comm = new MySqlCommand();
                MySqlConnection conn = DataBaseConnection.OpenConnection();

                //using (var transaction = conn.BeginTransaction())

                for (int i = 0; i < quantidadeDeCestas; i++)
                {
                    // 1️⃣ Inserir cesta
                    var cmdCesta = new MySqlCommand(
                        "INSERT INTO tbCestas(codUsu) VALUES(@codUsu); SELECT LAST_INSERT_ID();",
                        conn
                    //transaction
                    );

                    cmdCesta.Parameters.Add("@codUsu", MySqlDbType.Int32).Value = codUsu;

                    int codCesta = Convert.ToInt32(cmdCesta.ExecuteScalar());

                    // 2️⃣ Inserir itens da cesta
                    foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                    {
                        int codList = Convert.ToInt32(row.Cells["codList"].Value);
                        int quantidade = Convert.ToInt32(row.Cells["QtdePorCesta"].Value);

                        var cmdItem = new MySqlCommand(
                            "INSERT INTO tbItensCesta(codCes, codList, quantidade) VALUES(@codCes, @codList, @quantidade)",
                            conn
                        //transaction
                        );

                        cmdItem.Parameters.Add("@codCes", MySqlDbType.Int32).Value = codCesta;
                        cmdItem.Parameters.Add("@codList", MySqlDbType.Int32).Value = codList;
                        cmdItem.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = quantidade;

                        cmdItem.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Erro ao montar cestas! Erro:\n\n{error}", "Mensagem do sistema");
            }

            DataBaseConnection.CloseConnection();
        }


        // CONFIGURAÇÕES E AÇÕES DA JANELA
        // Configuração adicional DO DESIGN do datagrid view de intes da cesta - OK
        private void ConfigDgvItensDaCesta()
        {
            // Ajustar para ocupar toda a largura
            dgvItensDaCesta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Alternar cores das linhas
            dgvItensDaCesta.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            // Aumentar fonte
            dgvItensDaCesta.RowsDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
            dgvItensDaCesta.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            //// Ajustar altura das linhas
            dgvItensDaCesta.RowTemplate.Height = 40;
            //// Habilitar quebra de texto
            dgvItensDaCesta.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //// Ajustar seleção de célula
            dgvItensDaCesta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvItensDaCesta.AllowUserToAddRows = false;
            dgvItensDaCesta.MultiSelect = false;

            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "";
            buttonColumn.Name = "RemoverProduto"; // Name for programmatic reference
            buttonColumn.Text = "Remover"; // The text displayed on the button
            buttonColumn.UseColumnTextForButtonValue = true; // Use the Text property value for all buttons
            dgvItensDaCesta.Columns.Add(buttonColumn);

        }

        private void ApenasNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Limpeza de dados da janela
        private void limparDados()
        {
            codModeloCestaSelecionado = 0;
            cbbModeloDeCesta.SelectedItem = null;
            txtQtdCestas.Clear();
            dgvItensDaCesta.Rows.Clear();
        }

        // Calcular o total necessário de um produto com base em: Quantidade de cestas x Quantidade de itens por cesta
        private void calcularTotalNecessario()
        {
            int qtdCestas;
            if (txtQtdCestas.Text.Equals(""))
            {
                qtdCestas = 0;
            }
            else
            {
                qtdCestas = Convert.ToInt32(txtQtdCestas.Text);
            }

            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;

                int quantidadePorCesta = 0;
                int estoqueAtual = 0;

                int.TryParse(row.Cells["QtdePorCesta"].Value?.ToString(), out quantidadePorCesta);
                int.TryParse(row.Cells["EstoqueAtual"].Value?.ToString(), out estoqueAtual);

                int totalNecessario = quantidadePorCesta * qtdCestas;

                row.Cells["TotalNecessario"].Value = totalNecessario;
                calcularQuantoFalta();

                if (estoqueAtual < totalNecessario)
                {
                    row.Cells["Status"].Style.BackColor = System.Drawing.Color.LightCoral;
                    row.Cells["Status"].Value = "Insuficiente";

                }
                else
                {
                    row.Cells["Status"].Style.BackColor = System.Drawing.Color.LightGray;
                    row.Cells["Status"].Value = "Ok";
                }
            }
        }

        private void calcularQuantoFalta()
        {
            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;

                int estoqueAtual = 0;
                int totalNecessario = 0;

                int.TryParse(row.Cells["EstoqueAtual"].Value?.ToString(), out estoqueAtual);
                int.TryParse(row.Cells["TotalNecessario"].Value?.ToString(), out totalNecessario);

                int quantoFalta = totalNecessario - estoqueAtual;

                if (quantoFalta > 0)
                {
                    row.Cells["QuantoFalta"].Value = quantoFalta.ToString();
                }
                else
                {
                    row.Cells["QuantoFalta"].Value = "";
                }
            }
        }

        // Busca os dados da quantidade atual em estoque para retornar na inserção de item individual na cesta
        private int obterEstoqueAtual(string descricao)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT IFNULL(SUM(quantidade),0) AS estoqueAtual FROM tbProdutos WHERE descricao = @descricao;";
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = descricao;

                comm.Connection = DataBaseConnection.OpenConnection();

                var result = comm.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }

        // Adiciona um produto na cesta manualmente
        private void AdicionarProdutoNoGrid(string nomeProduto, int quantidadePorCesta)
        {
            // aqui é realizada a busca dos dados do produto salvando na variável {estoqueAtual}
            int estoqueAtual = obterEstoqueAtual(nomeProduto);

            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT codList FROM tbLista WHERE descricao = @descricao";
                comm.CommandType = CommandType.Text;
                comm.Parameters.Clear();
                comm.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = nomeProduto;

                comm.Connection = DataBaseConnection.OpenConnection();

                using (MySqlDataReader DR = comm.ExecuteReader())
                {
                    while (DR.Read())
                    {
                        if (ProdutoJaExisteNoDgv(DR.GetInt32("codList")))
                        {
                            MessageBox.Show("Produto já está na lista.");
                            return;
                        }
                        dgvItensDaCesta.Rows.Add(
                            DR["codList"].ToString(),
                            nomeProduto,
                            quantidadePorCesta,
                            estoqueAtual
                        );
                    }

                    DataBaseConnection.CloseConnection();

                }
            }

        }


        // MÉTODOS DE EVENTO DE CLIQUE
        // Aciona o método de buscar os itens de um determinado modelo de cesta
        private void cbbModeloDeCesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbModeloDeCesta.SelectedItem != null)
            {
                // Busca o código do modelo de cesta pela descrição do item selecionado
                buscarCodModeloPorDescricao(cbbModeloDeCesta.SelectedItem.ToString());

                // Carrega os dados referentes ao modelo de cesta selecionado
                carregarDadosNoDgvItensDaCesta(codModeloCestaSelecionado);
            }
            else
            {
                return;
            }
        }

        // Instancia do evento de clique do botão de voltar - OK
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        // Instancia do evento de clique de algum botão da coluna de remover produtos
        private void dgvItensDaCesta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvItensDaCesta.Columns["RemoverProduto"].Index)
            {
                dgvItensDaCesta.Rows.RemoveAt(e.RowIndex);
            }
        }

        // Aciona o modal para adicionar um produto na cesta
        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAdicionarItemNaCesta(codUsuLogado))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AdicionarProdutoNoGrid(
                        frm.NomeProdutoSelecionado,
                        frm.QuantidadeSelecionada
                    );
                    calcularTotalNecessario();
                }
            }
        }

        // Instancia do evento de clique do botão de limpar - OK
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparDados();
        }

        // Evento de alterar o valor de quantidade de cestas - OK
        private void txtQtdCestas_TextChanged(object sender, EventArgs e)
        {
            calcularTotalNecessario();
        }

        // Evento de pressionar teclas na caixa de texto de quantidade - limita a entrada de dados a números
        private void txtQtdCestas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Evento de alterar o valor de alguma célular dentro do dgv - OK
        private void dgvItensDaCesta_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItensDaCesta.Columns[e.ColumnIndex].Name == "QtdePorCesta")
            {
                calcularTotalNecessario();
            }
        }

        // Evento de pressionar teclas na caixa de texto de quantidade - limita a entrada de dados a números
        private void dgvItensDaCesta_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvItensDaCesta.CurrentCell.ColumnIndex == dgvItensDaCesta.Columns["QtdePorCesta"].Index)
            {
                TextBox tb = e.Control as TextBox;

                if (tb != null)
                {
                    tb.KeyPress -= ApenasNumeros_KeyPress;
                    tb.KeyPress += ApenasNumeros_KeyPress;
                }
            }
        }

        // Abre o modal para configurar modelos de cesta
        private void btnModeloDeCesta_Click(object sender, EventArgs e)
        {
            frmModelosDeCestas abrir = new frmModelosDeCestas(codUsuLogado, 1);
            abrir.Show();
            this.Close();
        }

        // Parte crírica fluxo de montagem de cestas

        // Método para validar se alguma linha da coluna "Status" contém a informação "Insuficiente"
        private bool ExisteItemInsuficiente()
        {
            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;

                var valor = row.Cells["Status"].Value?.ToString();

                if (!string.IsNullOrEmpty(valor) &&
                    valor.Equals("Insuficiente", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        private bool ProdutoJaExisteNoDgv(int codProduto)
        {
            foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
            {
                if (row.IsNewRow) continue;

                int codigoExistente = Convert.ToInt32(row.Cells["codList"].Value);

                if (codigoExistente == codProduto)
                    return true;
            }

            return false;
        }

        private bool QuantidadeValida()
        {
            if (!int.TryParse(txtQtdCestas.Text, out int quantidade))
                return false;

            return quantidade > 0;
        }

        // Realiza o registro de montagem de cestas - A FAZER
        private void btnMontar_Click(object sender, EventArgs e)
        {   
            // Valida se o DGV ou txtQtdCestas está vazio
            if (dgvItensDaCesta.Rows.Count < 5 || txtQtdCestas.Text.Equals("") || !QuantidadeValida())
            {
                MessageBox.Show("A cesta deve conter pelo menos 5 itens e a quantidade precisa ser maior que 0", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                // valida se o usuário confirma a montagem
                DialogResult result = MessageBox.Show($"Deseja confirmar a montagem de {txtQtdCestas.Text} cestas?", "Mensagem do sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes) 
                {
                    if (!ExisteItemInsuficiente())
                    {
                        montarCestas(Convert.ToInt32(txtQtdCestas.Text), codUsuLogado);
                        MessageBox.Show("Cestas montadas com sucesso", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limparDados();
                    }
                    else
                    {
                        MessageBox.Show("Existem itens com estoque insuficiente!", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    limparDados();
                    return;
                }
            }
        }
    }
}
