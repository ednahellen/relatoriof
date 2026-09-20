using MySql.Data.MySqlClient;
using System;
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
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            AtualizarTotais();
            AtualizarLabelMesAtual();
            AtualizarComparativos();
            CarregarDadosNoChartProdutos();
            CarregarDadosNoGraficoMensal();
            CarregarGraficoAnual();
        }

        #region TOTAIS DO MÊS ATUAL


private void AtualizarTotais()
        {
            string query = @"
        SELECT
            COALESCE(
                SUM(
                    CASE
                        WHEN quantidade > 0
                        THEN quantidade
                        ELSE 0
                    END
                ), 0
            ) AS totalQuantidade,

            COALESCE(
                SUM(
                    CASE
                        WHEN quantidade > 0
                        THEN quantidade * peso
                        ELSE 0
                    END
                ), 0
            ) AS totalPeso

        FROM tbProdutos

        WHERE dataDeEntrada >= @inicio
          AND dataDeEntrada < @fim;";

            try
            {
                DateTime hoje = DateTime.Today;

                DateTime inicioMes =
                    new DateTime(
                        hoje.Year,
                        hoje.Month,
                        1);

                DateTime fimPeriodo =
                    hoje.AddDays(1);

                decimal totalQuantidade = 0;
                decimal totalPeso = 0;

                using (var conn =
                    DataBaseConnection.OpenConnection())
                using (var cmd =
                    new MySqlCommand(query, conn))
                {
                    cmd.Parameters.Add(
                        "@inicio",
                        MySqlDbType.DateTime)
                        .Value = inicioMes;

                    cmd.Parameters.Add(
                        "@fim",
                        MySqlDbType.DateTime)
                        .Value = fimPeriodo;

                    using (var reader =
                        cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalQuantidade =
                                Convert.ToDecimal(
                                    reader["totalQuantidade"]);

                            totalPeso =
                                Convert.ToDecimal(
                                    reader["totalPeso"]);
                        }
                    }
                }

                // =========================================================
                // TOTAL DE QUANTIDADE RECEBIDA NO MÊS
                // =========================================================

                lblTotalQuantidade.Text =
                    totalQuantidade.ToString("N0");

                // =========================================================
                // PESO TOTAL RECEBIDO NO MÊS
                //
                // O banco armazena o peso em gramas.
                // Convertemos para kg.
                // A partir de 1.000 kg mostramos toneladas.
                // =========================================================

                decimal pesoKg =
                    totalPeso / 1000m;

                if (pesoKg >= 1000)
                {
                    decimal toneladas =
                        pesoKg / 1000m;

                    lblTotalItens.Text =
                        toneladas.ToString("N2") + " t";
                }
                else
                {
                    lblTotalItens.Text =
                        pesoKg.ToString("N2") + " kg";
                }

                // =========================================================
                // PESO MÉDIO POR UNIDADE
                // =========================================================

                if (totalQuantidade > 0)
                {
                    decimal pesoMedioKg =
                        pesoKg / totalQuantidade;

                    lblPeso.Text =
                        pesoMedioKg.ToString("N2")
                        + " kg/un";
                }
                else
                {
                    lblPeso.Text = "0,00 kg/un";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar indicadores do mês: "
                    + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        #endregion

        #region GRÁFICO PRODUTOS MAIS RECEBIDOS

        private void CarregarDadosNoChartProdutos()
        {
            chartProdutos.Series.Clear();
            chartProdutos.Titles.Clear();

            var series =
                new Series("Produtos Recebidos")
                {
                    ChartType =
                        SeriesChartType.Column,

                    IsValueShownAsLabel = true
                };

            string query = @"
                SELECT
                    descricao,
                    SUM(quantidade) AS totalQuantidade

                FROM tbProdutos

                WHERE quantidade > 0

                GROUP BY descricao

                ORDER BY totalQuantidade DESC

                LIMIT 8;";

            try
            {
                using (var conn =
                    DataBaseConnection.OpenConnection())
                using (var cmd =
                    new MySqlCommand(query, conn))
                using (var reader =
                    cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string descricao =
                            reader["descricao"].ToString();

                        if (descricao.Length > 15)
                            descricao =
                                descricao.Substring(0, 12)
                                + "...";

                        series.Points.AddXY(
                            descricao,
                            Convert.ToDouble(
                                reader["totalQuantidade"])
                        );
                    }
                }

                chartProdutos.Series.Add(series);

                chartProdutos.Titles.Add(
                    "Top 8 Produtos Mais Recebidos");

                chartProdutos.ChartAreas[0]
                    .AxisX
                    .LabelStyle
                    .Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar gráfico de produtos: "
                    + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region GRÁFICO MENSAL

        //private void CarregarDadosNoGraficoMensal()
        //{
        //    chartDoacaoMensal.Series.Clear();
        //    chartDoacaoMensal.Titles.Clear();

        //    var seriesQuantidade =
        //        new Series("Quantidade")
        //        {
        //            ChartType =
        //                SeriesChartType.Line,

        //            IsValueShownAsLabel = true
        //        };

        //    var seriesPeso =
        //        new Series("Peso (kg)")
        //        {
        //            ChartType =
        //                SeriesChartType.Line,

        //            IsValueShownAsLabel = true
        //        };

        //    string query = @"
        //        SELECT
        //            YEAR(dataDeEntrada) AS ano,
        //            MONTH(dataDeEntrada) AS mes,

        //            SUM(
        //                CASE
        //                    WHEN quantidade > 0
        //                    THEN quantidade
        //                    ELSE 0
        //                END
        //            ) AS totalQuantidade,

        //            SUM(
        //                CASE
        //                    WHEN quantidade > 0
        //                    THEN quantidade * peso
        //                    ELSE 0
        //                END
        //            ) AS totalPeso

        //        FROM tbProdutos

        //        GROUP BY
        //            YEAR(dataDeEntrada),
        //            MONTH(dataDeEntrada)

        //        ORDER BY
        //            ano,
        //            mes;";

        //    try
        //    {
        //        using (var conn =
        //            DataBaseConnection.OpenConnection())
        //        using (var cmd =
        //            new MySqlCommand(query, conn))
        //        using (var reader =
        //            cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                int mes =
        //                    Convert.ToInt32(
        //                        reader["mes"]);

        //                int ano =
        //                    Convert.ToInt32(
        //                        reader["ano"]);

        //                string mesNome =
        //                    new DateTime(
        //                        ano,
        //                        mes,
        //                        1)
        //                    .ToString("MMM/yyyy");

        //                seriesQuantidade.Points.AddXY(
        //                    mesNome,
        //                    Convert.ToDouble(
        //                        reader["totalQuantidade"])
        //                );

        //                decimal pesoKg =
        //                    Convert.ToDecimal(
        //                        reader["totalPeso"])
        //                    / 1000m;

        //                seriesPeso.Points.AddXY(
        //                    mesNome,
        //                    Convert.ToDouble(
        //                        pesoKg)
        //                );
        //            }
        //        }

        //        chartDoacaoMensal.Series.Add(
        //            seriesQuantidade);

        //        chartDoacaoMensal.Series.Add(
        //            seriesPeso);

        //        chartDoacaoMensal.Titles.Add(
        //            "Itens Recebidos por Mês");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            "Erro ao carregar gráfico mensal: "
        //            + ex.Message,
        //            "Erro",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);
        //    }
        //}

        //#endregion

        //#region GRÁFICO ANUAL

        //private void CarregarGraficoAnual()
        //{
        //    chartAnual.Series.Clear();
        //    chartAnual.Titles.Clear();

        //    var seriesQuantidade =
        //        new Series("Quantidade Anual")
        //        {
        //            ChartType =
        //                SeriesChartType.Column,

        //            IsValueShownAsLabel = true
        //        };

        //    var seriesPeso =
        //        new Series("Peso Anual (kg)")
        //        {
        //            ChartType =
        //                SeriesChartType.Column,

        //            IsValueShownAsLabel = true
        //        };

        //    string query = @"
        //        SELECT
        //            YEAR(dataDeEntrada) AS ano,

        //            SUM(
        //                CASE
        //                    WHEN quantidade > 0
        //                    THEN quantidade
        //                    ELSE 0
        //                END
        //            ) AS totalQuantidade,

        //            SUM(
        //                CASE
        //                    WHEN quantidade > 0
        //                    THEN quantidade * peso
        //                    ELSE 0
        //                END
        //            ) AS totalPeso

        //        FROM tbProdutos

        //        GROUP BY YEAR(dataDeEntrada)

        //        ORDER BY ano;";

        //    try
        //    {
        //        using (var conn =
        //            DataBaseConnection.OpenConnection())
        //        using (var cmd =
        //            new MySqlCommand(query, conn))
        //        using (var reader =
        //            cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                string ano =
        //                    reader["ano"].ToString();

        //                seriesQuantidade.Points.AddXY(
        //                    ano,
        //                    Convert.ToDouble(
        //                        reader["totalQuantidade"])
        //                );

        //                decimal pesoKg =
        //                    Convert.ToDecimal(
        //                        reader["totalPeso"])
        //                    / 1000m;

        //                seriesPeso.Points.AddXY(
        //                    ano,
        //                    Convert.ToDouble(
        //                        pesoKg)
        //                );
        //            }
        //        }

        //        chartAnual.Series.Add(
        //            seriesQuantidade);

        //        chartAnual.Series.Add(
        //            seriesPeso);

        //        chartAnual.Titles.Add(
        //            "Histórico Anual");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            "Erro ao carregar gráfico anual: "
        //            + ex.Message,
        //            "Erro",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);
        //    }
        //}

       
// =========================================================
// FORMATA PESO
// =========================================================
private string FormatarPeso(decimal pesoGramas)
        {
            decimal pesoKg = pesoGramas / 1000m;

            if (pesoKg >= 1000)
            {
                decimal toneladas = pesoKg / 1000m;
                return toneladas.ToString("N2") + " t";
            }

            return pesoKg.ToString("N2") + " kg";
        }


        // =========================================================
        // GRÁFICO MENSAL
        // =========================================================
        private void CarregarDadosNoGraficoMensal()
        {
            string query = @"
        SELECT
            YEAR(dataDeEntrada) AS ano,
            MONTH(dataDeEntrada) AS mes,

            COALESCE(
                SUM(
                    CASE
                        WHEN quantidade > 0
                        THEN quantidade
                        ELSE 0
                    END
                ), 0
            ) AS quantidade,

            COALESCE(
                SUM(
                    CASE
                        WHEN quantidade > 0
                        THEN quantidade * peso
                        ELSE 0
                    END
                ), 0
            ) AS peso

        FROM tbProdutos

        GROUP BY
            YEAR(dataDeEntrada),
            MONTH(dataDeEntrada)

        ORDER BY
            ano,
            mes;";

            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    chartDoacaoMensal.Series.Clear();
                    chartDoacaoMensal.Titles.Clear();

                    Series serie = new Series("Peso recebido");

                    serie.ChartType = SeriesChartType.Column;
                    serie.IsValueShownAsLabel = true;
                    serie.LabelFormat = "N2";

                    while (reader.Read())
                    {
                        int ano = Convert.ToInt32(reader["ano"]);
                        int mes = Convert.ToInt32(reader["mes"]);

                        decimal peso = Convert.ToDecimal(reader["peso"]);

                        DateTime data =
                            new DateTime(ano, mes, 1);

                        decimal pesoKg =
                            peso / 1000m;

                        DataPoint ponto =
                            new DataPoint();

                        ponto.SetValueY(pesoKg);

                        ponto.AxisLabel =
                            data.ToString("MMM/yyyy");

                        ponto.Label =
                            FormatarPeso(peso);

                        serie.Points.Add(ponto);
                    }

                    chartDoacaoMensal.Series.Add(serie);

                    chartDoacaoMensal.ChartAreas[0]
                        .AxisY.Title = "Peso recebido";

                    chartDoacaoMensal.ChartAreas[0]
                        .AxisX.Title = "Mês";

                    chartDoacaoMensal.ChartAreas[0]
                        .AxisY.LabelStyle.Format = "N2";

                    chartDoacaoMensal.ChartAreas[0]
                        .AxisX.Interval = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar gráfico mensal: "
                    + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // =========================================================
        // GRÁFICO ANUAL
        // =========================================================
        private void CarregarGraficoAnual()
        {
            string query = @"
        SELECT
            YEAR(dataDeEntrada) AS ano,

            COALESCE(
                SUM(
                    CASE
                        WHEN quantidade > 0
                        THEN quantidade
                        ELSE 0
                    END
                ), 0
            ) AS quantidade,

            COALESCE(
                SUM(
                    CASE
                        WHEN quantidade > 0
                        THEN quantidade * peso
                        ELSE 0
                    END
                ), 0
            ) AS peso

        FROM tbProdutos

        GROUP BY
            YEAR(dataDeEntrada)

        ORDER BY
            ano;";

            try
            {
                using (var conn = DataBaseConnection.OpenConnection())
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    chartAnual.Series.Clear();
                    chartAnual.Titles.Clear();

                    Series serie = new Series("Peso recebido");

                    serie.ChartType = SeriesChartType.Column;
                    serie.IsValueShownAsLabel = true;
                    serie.LabelFormat = "N2";

                    while (reader.Read())
                    {
                        int ano =
                            Convert.ToInt32(reader["ano"]);

                        decimal peso =
                            Convert.ToDecimal(reader["peso"]);

                        decimal pesoKg =
                            peso / 1000m;

                        DataPoint ponto =
                            new DataPoint();

                        ponto.SetValueY(pesoKg);

                        ponto.AxisLabel =
                            ano.ToString();

                        ponto.Label =
                            FormatarPeso(peso);

                        serie.Points.Add(ponto);
                    }

                    chartAnual.Series.Add(serie);

                    chartAnual.ChartAreas[0]
                        .AxisY.Title = "Peso recebido";

                    chartAnual.ChartAreas[0]
                        .AxisX.Title = "Ano";

                    chartAnual.ChartAreas[0]
                        .AxisY.LabelStyle.Format = "N2";

                    chartAnual.ChartAreas[0]
                        .AxisX.Interval = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar gráfico anual: "
                    + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region COMPARATIVOS

        private void AtualizarComparativos()
        {
            try
            {
                DateTime hoje = DateTime.Today;

                // =====================================================
                // MÊS ATUAL
                // =====================================================

                DateTime inicioMesAtual =
                    new DateTime(
                        hoje.Year,
                        hoje.Month,
                        1);

                DateTime fimMesAtual =
                    hoje.AddDays(1);

                // =====================================================
                // MÊS ANTERIOR
                // =====================================================

                DateTime inicioMesAnterior =
                    inicioMesAtual.AddMonths(-1);

                int ultimoDiaMesAnterior =
                    DateTime.DaysInMonth(
                        inicioMesAnterior.Year,
                        inicioMesAnterior.Month);

                int diaMesAnterior =
                    Math.Min(
                        hoje.Day,
                        ultimoDiaMesAnterior);

                DateTime fimMesAnterior =
                    new DateTime(
                        inicioMesAnterior.Year,
                        inicioMesAnterior.Month,
                        diaMesAnterior)
                    .AddDays(1);

                // =====================================================
                // MESMO PERÍODO DO ANO ANTERIOR
                // =====================================================

                DateTime inicioAnoAnterior =
                    inicioMesAtual.AddYears(-1);

                int ultimoDiaAnoAnterior =
                    DateTime.DaysInMonth(
                        inicioAnoAnterior.Year,
                        inicioAnoAnterior.Month);

                int diaAnoAnterior =
                    Math.Min(
                        hoje.Day,
                        ultimoDiaAnoAnterior);

                DateTime fimAnoAnterior =
                    new DateTime(
                        inicioAnoAnterior.Year,
                        inicioAnoAnterior.Month,
                        diaAnoAnterior)
                    .AddDays(1);

                decimal pesoAtual =
                    ObterPesoPeriodo(
                        inicioMesAtual,
                        fimMesAtual);

                decimal pesoAnterior =
                    ObterPesoPeriodo(
                        inicioMesAnterior,
                        fimMesAnterior);

                decimal pesoAnoAnterior =
                    ObterPesoPeriodo(
                        inicioAnoAnterior,
                        fimAnoAnterior);

                // =====================================================
                // COMPARATIVO MÊS ANTERIOR
                // =====================================================

                if (pesoAnterior > 0)
                {
                    decimal percentual =
                        ((pesoAtual - pesoAnterior)
                        / pesoAnterior) * 100;

                    string sinal =
                        percentual > 0 ? "+" : "";

                    lblComparativoMes.Text =
                        sinal +
                        percentual.ToString("N1") +
                        "% vs mês anterior";
                }
                else
                {
                    lblComparativoMes.Text =
                        "Sem dados mês anterior";
                }

                // =====================================================
                // COMPARATIVO ANO ANTERIOR
                // =====================================================

                if (pesoAnoAnterior > 0)
                {
                    decimal percentual =
                        ((pesoAtual - pesoAnoAnterior)
                        / pesoAnoAnterior) * 100;

                    string sinal =
                        percentual > 0 ? "+" : "";

                    lblComparativoAno.Text =
                        sinal +
                        percentual.ToString("N1") +
                        "% vs ano anterior";
                }
                else
                {
                    lblComparativoAno.Text =
                        "Sem dados ano anterior";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao calcular comparativos: "
                    + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private decimal ObterPesoPeriodo(
            DateTime inicio,
            DateTime fim)
        {
            string query = @"
                SELECT
                    COALESCE(
                        SUM(
                            CASE
                                WHEN quantidade > 0
                                THEN quantidade * peso
                                ELSE 0
                            END
                        ), 0
                    )
                FROM tbProdutos

                WHERE dataDeEntrada >= @inicio
                  AND dataDeEntrada < @fim;";

            using (var conn =
                DataBaseConnection.OpenConnection())
            using (var cmd =
                new MySqlCommand(query, conn))
            {
                cmd.Parameters.Add(
                    "@inicio",
                    MySqlDbType.DateTime)
                    .Value = inicio;

                cmd.Parameters.Add(
                    "@fim",
                    MySqlDbType.DateTime)
                    .Value = fim;

                return Convert.ToDecimal(
                    cmd.ExecuteScalar());
            }
        }

        #endregion

        #region MÊS ATUAL

        private void AtualizarLabelMesAtual()
        {
            lblMesAtualDataReceiver.Text =
                DateTime.Now.ToString("MMMM");

            lblMesAtualDataReceiver.ForeColor =
                Color.Orange;

            lblMesAtualDataReceiver.Visible = true;
        }

        #endregion

        #region NAVEGAÇÃO

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir =
                new frmMenuPrincipal(codUsuLogado);

            abrir.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmGerenciarProdutos abrir =
                new frmGerenciarProdutos(codUsuLogado);

            abrir.Show();
            this.Hide();
        }

        #endregion
    }
}

