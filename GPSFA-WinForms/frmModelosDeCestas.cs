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
    public partial class frmModelosDeCestas : Form
    {
        public frmModelosDeCestas()
        {
            InitializeComponent();
        }
        public frmModelosDeCestas(int codUsu, int codModeloCesta)
        {
            InitializeComponent();
            codUsuLogado = codUsu;
            carregarDadosNoDgvItensDaCesta(codModeloCesta);
            ConfigDgvItensDaCesta();
        }

        int codUsuLogado, ModeloCestaSelected;

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

        private void buscarCodModeloPorDescricao(string cestaModeloNome)
        {
            int resp;

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
                            resp = DR.GetInt32("codModelo");

                            DataBaseConnection.CloseConnection();
                        }
                        else
                        {
                            MessageBox.Show("Codigo não encontrado");
                            resp = 0;
                            DataBaseConnection.CloseConnection();
                        }
                    }
                }
            }
        }

        private void carregarDadosNoDgvItensDaCesta(int codModelo)
        {
            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.CommandText = $"SELECT l.codList, l.descricao, imc.quantidadeMinima FROM tbItensDoModeloCesta imc INNER JOIN tbLista l ON l.codList = imc.codList WHERE imc.codModelo = @codModelo GROUP BY imc.codModelo, imc.codList, l.descricao, l.unidade, imc.quantidadeMinima;";
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
                            DR["quantidadeMinima"].ToString()
                        );
                    }

                    DataBaseConnection.CloseConnection();
                }
            }
        }
        private void AdicionarProdutoNoGrid(string nomeProduto, int quantidadePorCesta)
        {
            // aqui é realizada a busca dos dados do produto salvando na variável {estoqueAtual}

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
                        dgvItensDaCesta.Rows.Add(
                            DR["codList"].ToString(),
                            nomeProduto,
                            quantidadePorCesta
                        );
                    }

                    DataBaseConnection.CloseConnection();
                }
            }
        }

        private void AtualizarModeloCesta(int codModelo)
        {
            using (MySqlConnection conn = DataBaseConnection.OpenConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // 1️⃣ Remove itens atuais do modelo
                    var cmdDelete = new MySqlCommand(
                        "DELETE FROM tbItensDoModeloCesta WHERE codModelo = @codModelo",
                        conn, transaction);

                    cmdDelete.Parameters.Add("@codModelo", MySqlDbType.Int32).Value = codModelo;
                    cmdDelete.ExecuteNonQuery();

                    // 2️⃣ Insere novamente conforme DGV
                    foreach (DataGridViewRow row in dgvItensDaCesta.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int codList = Convert.ToInt32(row.Cells["codList"].Value);
                        int quantidade = Convert.ToInt32(row.Cells["QtdePorCesta"].Value);

                        var cmdInsert = new MySqlCommand(
                            @"INSERT INTO tbItensDoModeloCesta
                      (codModelo, codList, quantidadeMinima)
                      VALUES (@codModelo, @codList, @quantidade)",
                            conn, transaction);

                        cmdInsert.Parameters.Add("@codModelo", MySqlDbType.Int32).Value = codModelo;
                        cmdInsert.Parameters.Add("@codList", MySqlDbType.Int32).Value = codList;
                        cmdInsert.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = quantidade;

                        cmdInsert.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

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
                }
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmCestas abrir = new frmCestas(codUsuLogado);
            abrir.Show();
            this.Close();
        }

        private void btnMontar_Click(object sender, EventArgs e)
        {
            // Valida se o DGV ou txtQtdCestas está vazio
            if (dgvItensDaCesta.Rows.Count < 5)
            {
                MessageBox.Show("A cesta deve conter pelo menos 5 itens e a quantidade não pode estar vazia", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // valida se o usuário confirma a montagem
                DialogResult result = MessageBox.Show($"Deseja salvar o modelo de cesta?", "Mensagem do sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    AtualizarModeloCesta(1);
                    MessageBox.Show("Cestas montadas com sucesso", "Mensagem do sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
