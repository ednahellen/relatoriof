using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GPSFA_WinForms
{
    public partial class frmDashboard : Form
    {
        private int codUsuLogado;

        public frmDashboard()
        {
            InitializeComponent();
        }

        public frmDashboard(int codUsu)
        {
            InitializeComponent();
            codUsuLogado = codUsu;
            AtualizarPesoMesAtual();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            AtualizarTotais();
            AtualizarLabelMesAtual();
            CarregarDadosNoChartProdutos();
            CarregarDadosNoGraficoMensal();
            CarregarGraficoAnual();
        }

        #region TOTAIS

        private void AtualizarTotais()
        {
            string query = @"SELECT 
                                SUM(quantidade) AS totalQuantidade,
                                SUM(quantidade * peso) AS totalPeso
                             FROM tbProdutos;";

            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTotalQuantidade.Text =
                            reader["totalQuantidade"] != DBNull.Value
                            ? Convert.ToInt64(reader["totalQuantidade"]).ToString("N0")
                            : "0";

                        lblTotalItens.Text =
                            reader["totalPeso"] != DBNull.Value
                            ? Convert.ToDecimal(reader["totalPeso"]).ToString("N2") + " kg"
                            : "0 kg";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar totais: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region GRÁFICO PRODUTOS MAIS RECEBIDOS

        private void CarregarDadosNoChartProdutos()
        {
            chartProdutos.Series.Clear();
            chartProdutos.Titles.Clear();

            var series = new Series("Produtos Recebidos")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            string query = @"SELECT descricao,
                             SUM(quantidade) AS totalQuantidade
                             FROM tbProdutos
                             GROUP BY descricao
                             ORDER BY totalQuantidade DESC
                             LIMIT 8;";

            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string descricao = reader["descricao"].ToString();

                        if (descricao.Length > 15)
                            descricao = descricao.Substring(0, 12) + "...";

                        series.Points.AddXY(
                            descricao,
                            Convert.ToDouble(reader["totalQuantidade"])
                        );
                    }
                }

                chartProdutos.Series.Add(series);
                chartProdutos.Titles.Add("Top 8 Produtos Mais Recebidos");
                chartProdutos.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar gráfico de produtos: " + ex.Message);
            }
        }

        #endregion

        #region GRÁFICO MENSAL

        private void CarregarDadosNoGraficoMensal()
        {
            chartDoacaoMensal.Series.Clear();
            chartDoacaoMensal.Titles.Clear();

            var seriesQuantidade = new Series("Quantidade")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = true
            };

            var seriesPeso = new Series("Peso (kg)")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = true
            };

            string query = @"SELECT 
                            YEAR(dataDeEntrada) AS ano,
                            MONTH(dataDeEntrada) AS mes,
                            SUM(quantidade) AS totalQuantidade,
                            SUM(quantidade * peso) AS totalPeso
                            FROM tbProdutos
                            GROUP BY YEAR(dataDeEntrada), MONTH(dataDeEntrada)
                            ORDER BY ano, mes;";

            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int mes = Convert.ToInt32(reader["mes"]);
                        int ano = Convert.ToInt32(reader["ano"]);

                        string mesNome = new DateTime(ano, mes, 1).ToString("MMM/yyyy");

                        seriesQuantidade.Points.AddXY(
                            mesNome,
                            Convert.ToDouble(reader["totalQuantidade"])
                        );

                        seriesPeso.Points.AddXY(
                            mesNome,
                            Convert.ToDouble(reader["totalPeso"])
                        );
                    }
                }

                chartDoacaoMensal.Series.Add(seriesQuantidade);
                chartDoacaoMensal.Series.Add(seriesPeso);
                chartDoacaoMensal.Titles.Add("Itens Recebidos por Mês");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar gráfico mensal: " + ex.Message);
            }
        }

        #endregion

        #region GRÁFICO ANUAL

        private void CarregarGraficoAnual()
        {
            chartAnual.Series.Clear();
            chartAnual.Titles.Clear();

            var seriesQuantidade = new Series("Quantidade Anual")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            var seriesPeso = new Series("Peso Anual (kg)")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            string query = @"SELECT 
                            YEAR(dataDeEntrada) AS ano,
                            SUM(quantidade) AS totalQuantidade,
                            SUM(quantidade * peso) AS totalPeso
                            FROM tbProdutos
                            GROUP BY YEAR(dataDeEntrada)
                            ORDER BY ano;";

            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ano = reader["ano"].ToString();

                        seriesQuantidade.Points.AddXY(
                            ano,
                            Convert.ToDouble(reader["totalQuantidade"])
                        );

                        seriesPeso.Points.AddXY(
                            ano,
                            Convert.ToDouble(reader["totalPeso"])
                        );
                    }
                }

                chartAnual.Series.Add(seriesQuantidade);
                chartAnual.Series.Add(seriesPeso);
                chartAnual.Titles.Add("Histórico Anual");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar gráfico anual: " + ex.Message);
            }
        }

        #endregion

        #region MÊS ATUAL

        private void AtualizarPesoMesAtual()
        {
            try
            {
                decimal pesoMesAtual = 0;
                decimal pesoMesAnterior = 0;
                decimal pesoMesmoMesAnoAnterior = 0;

                using (var conn = DataBaseConnection.OpenConnection())
                {
                    // PESO MÊS ATUAL
                    string queryMesAtual = @"SELECT SUM(quantidade * peso) 
                                     FROM tbProdutos
                                     WHERE MONTH(dataDeEntrada) = MONTH(CURDATE())
                                     AND YEAR(dataDeEntrada) = YEAR(CURDATE());";

                    using (var cmd = new MySqlCommand(queryMesAtual, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                            pesoMesAtual = Convert.ToDecimal(result);
                    }

                    // PESO MÊS ANTERIOR
                    string queryMesAnterior = @"SELECT SUM(quantidade * peso) 
                                        FROM tbProdutos
                                        WHERE MONTH(dataDeEntrada) = MONTH(DATE_SUB(CURDATE(), INTERVAL 1 MONTH))
                                        AND YEAR(dataDeEntrada) = YEAR(DATE_SUB(CURDATE(), INTERVAL 1 MONTH));";

                    using (var cmd = new MySqlCommand(queryMesAnterior, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                            pesoMesAnterior = Convert.ToDecimal(result);
                    }

                    // MESMO MÊS ANO ANTERIOR
                    string queryAnoAnterior = @"SELECT SUM(quantidade * peso) 
                                        FROM tbProdutos
                                        WHERE MONTH(dataDeEntrada) = MONTH(CURDATE())
                                        AND YEAR(dataDeEntrada) = YEAR(CURDATE()) - 1;";

                    using (var cmd = new MySqlCommand(queryAnoAnterior, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                            pesoMesmoMesAnoAnterior = Convert.ToDecimal(result);
                    }
                }

                // 🔹 Atualiza peso do mês atual
                lblPeso.Text = pesoMesAtual.ToString("N2") + " kg";

                // 🔹 Comparativo com mês anterior
                if (pesoMesAnterior > 0)
                {
                    decimal percentualMes = ((pesoMesAtual - pesoMesAnterior) / pesoMesAnterior) * 100;
                    lblComparativoMes.Text = percentualMes.ToString("N1") + "% vs mês anterior";
                }
                else
                {
                    lblComparativoMes.Text = "Sem dados mês anterior";
                }

                //Comparativo com ano anterior
                if (pesoMesmoMesAnoAnterior > 0)
                {
                    decimal percentualAno = ((pesoMesAtual - pesoMesmoMesAnoAnterior) / pesoMesmoMesAnoAnterior) * 100;
                    lblComparativoAno.Text = percentualAno.ToString("N1") + "% vs ano anterior";
                }
                else
                {
                    lblComparativoAno.Text = "Sem dados ano anterior";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao calcular comparativos: " + ex.Message);
            }
        }
        private void AtualizarLabelMesAtual()
        {
            lblMesAtualDataReceiver.Text = DateTime.Now.ToString("MMMM");
            lblMesAtualDataReceiver.ForeColor = Color.Orange;
            lblMesAtualDataReceiver.Visible = true;
        }

        #endregion

        #region NAVEGAÇÃO

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos abrir = new frmGerenciarProdutos(codUsuLogado);
            abrir.Show();
            this.Hide();
        }

        #endregion
    }
}