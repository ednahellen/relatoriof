namespace GPSFA_WinForms
{
    partial class frmDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboard));
            this.tbctrlDashboardAnual = new System.Windows.Forms.TabControl();
            this.tbpgDashboardAlimentos = new System.Windows.Forms.TabPage();
            this.lblComparativoAno = new System.Windows.Forms.Label();
            this.txtCompativoAno = new System.Windows.Forms.TextBox();
            this.lblComparativoMes = new System.Windows.Forms.Label();
            this.txtCompativoMes = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.lbTotalPeso = new System.Windows.Forms.TextBox();
            this.lblPeso = new System.Windows.Forms.Label();
            this.lblTotalQuantidade = new System.Windows.Forms.Label();
            this.lblMesAtualDataReceiver = new System.Windows.Forms.Label();
            this.lblTotalItens = new System.Windows.Forms.Label();
            this.chartProdutos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpgDashboadMensal = new System.Windows.Forms.TabPage();
            this.chartDoacaoMensal = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chartAnual = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlDadosProdutos = new System.Windows.Forms.Panel();
            this.btnCadastrar = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.lblTitleDashboard = new System.Windows.Forms.Label();
            this.tbctrlDashboardAnual.SuspendLayout();
            this.tbpgDashboardAlimentos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartProdutos)).BeginInit();
            this.tbpgDashboadMensal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDoacaoMensal)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartAnual)).BeginInit();
            this.pnlDadosProdutos.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbctrlDashboardAnual
            // 
            this.tbctrlDashboardAnual.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbctrlDashboardAnual.Controls.Add(this.tbpgDashboardAlimentos);
            this.tbctrlDashboardAnual.Controls.Add(this.tbpgDashboadMensal);
            this.tbctrlDashboardAnual.Controls.Add(this.tabPage1);
            this.tbctrlDashboardAnual.Location = new System.Drawing.Point(12, 75);
            this.tbctrlDashboardAnual.Name = "tbctrlDashboardAnual";
            this.tbctrlDashboardAnual.SelectedIndex = 0;
            this.tbctrlDashboardAnual.Size = new System.Drawing.Size(1317, 595);
            this.tbctrlDashboardAnual.TabIndex = 0;
            // 
            // tbpgDashboardAlimentos
            // 
            this.tbpgDashboardAlimentos.Controls.Add(this.lblComparativoAno);
            this.tbpgDashboardAlimentos.Controls.Add(this.txtCompativoAno);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblComparativoMes);
            this.tbpgDashboardAlimentos.Controls.Add(this.txtCompativoMes);
            this.tbpgDashboardAlimentos.Controls.Add(this.textBox6);
            this.tbpgDashboardAlimentos.Controls.Add(this.textBox5);
            this.tbpgDashboardAlimentos.Controls.Add(this.textBox3);
            this.tbpgDashboardAlimentos.Controls.Add(this.lbTotalPeso);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblPeso);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalQuantidade);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblMesAtualDataReceiver);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalItens);
            this.tbpgDashboardAlimentos.Controls.Add(this.chartProdutos);
            this.tbpgDashboardAlimentos.Location = new System.Drawing.Point(4, 29);
            this.tbpgDashboardAlimentos.Name = "tbpgDashboardAlimentos";
            this.tbpgDashboardAlimentos.Padding = new System.Windows.Forms.Padding(3);
            this.tbpgDashboardAlimentos.Size = new System.Drawing.Size(1309, 562);
            this.tbpgDashboardAlimentos.TabIndex = 0;
            this.tbpgDashboardAlimentos.Text = "Alimentos mais recebidos";
            this.tbpgDashboardAlimentos.UseVisualStyleBackColor = true;
            // 
            // lblComparativoAno
            // 
            this.lblComparativoAno.AutoSize = true;
            this.lblComparativoAno.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblComparativoAno.Location = new System.Drawing.Point(881, 393);
            this.lblComparativoAno.Name = "lblComparativoAno";
            this.lblComparativoAno.Size = new System.Drawing.Size(18, 20);
            this.lblComparativoAno.TabIndex = 27;
            this.lblComparativoAno.Text = "0";
            // 
            // txtCompativoAno
            // 
            this.txtCompativoAno.Location = new System.Drawing.Point(872, 364);
            this.txtCompativoAno.Name = "txtCompativoAno";
            this.txtCompativoAno.Size = new System.Drawing.Size(402, 26);
            this.txtCompativoAno.TabIndex = 26;
            this.txtCompativoAno.Text = "Comparativo em relação ao Ano anterior:";
            // 
            // lblComparativoMes
            // 
            this.lblComparativoMes.AutoSize = true;
            this.lblComparativoMes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblComparativoMes.Location = new System.Drawing.Point(880, 328);
            this.lblComparativoMes.Name = "lblComparativoMes";
            this.lblComparativoMes.Size = new System.Drawing.Size(18, 20);
            this.lblComparativoMes.TabIndex = 25;
            this.lblComparativoMes.Text = "0";
            // 
            // txtCompativoMes
            // 
            this.txtCompativoMes.Location = new System.Drawing.Point(872, 299);
            this.txtCompativoMes.Name = "txtCompativoMes";
            this.txtCompativoMes.Size = new System.Drawing.Size(366, 26);
            this.txtCompativoMes.TabIndex = 24;
            this.txtCompativoMes.Text = "Comparativo em relação ao Mês anterior:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(872, 33);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(166, 26);
            this.textBox6.TabIndex = 23;
            this.textBox6.Text = "Mês atual:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(872, 103);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(206, 26);
            this.textBox5.TabIndex = 22;
            this.textBox5.Text = "Total de itens:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(872, 170);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(206, 26);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = "Total de Quantidade:";
            // 
            // lbTotalPeso
            // 
            this.lbTotalPeso.Location = new System.Drawing.Point(871, 236);
            this.lbTotalPeso.Name = "lbTotalPeso";
            this.lbTotalPeso.Size = new System.Drawing.Size(207, 26);
            this.lbTotalPeso.TabIndex = 19;
            this.lbTotalPeso.Text = "Total de Peso:";
            // 
            // lblPeso
            // 
            this.lblPeso.AutoSize = true;
            this.lblPeso.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPeso.Location = new System.Drawing.Point(880, 265);
            this.lblPeso.Name = "lblPeso";
            this.lblPeso.Size = new System.Drawing.Size(18, 20);
            this.lblPeso.TabIndex = 15;
            this.lblPeso.Text = "0";
            // 
            // lblTotalQuantidade
            // 
            this.lblTotalQuantidade.AutoSize = true;
            this.lblTotalQuantidade.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalQuantidade.Location = new System.Drawing.Point(881, 199);
            this.lblTotalQuantidade.Name = "lblTotalQuantidade";
            this.lblTotalQuantidade.Size = new System.Drawing.Size(18, 20);
            this.lblTotalQuantidade.TabIndex = 13;
            this.lblTotalQuantidade.Text = "0";
            // 
            // lblMesAtualDataReceiver
            // 
            this.lblMesAtualDataReceiver.AutoSize = true;
            this.lblMesAtualDataReceiver.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMesAtualDataReceiver.Location = new System.Drawing.Point(882, 62);
            this.lblMesAtualDataReceiver.Name = "lblMesAtualDataReceiver";
            this.lblMesAtualDataReceiver.Size = new System.Drawing.Size(39, 20);
            this.lblMesAtualDataReceiver.TabIndex = 11;
            this.lblMesAtualDataReceiver.Text = "Mês";
            // 
            // lblTotalItens
            // 
            this.lblTotalItens.AutoSize = true;
            this.lblTotalItens.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalItens.Location = new System.Drawing.Point(881, 132);
            this.lblTotalItens.Name = "lblTotalItens";
            this.lblTotalItens.Size = new System.Drawing.Size(18, 20);
            this.lblTotalItens.TabIndex = 8;
            this.lblTotalItens.Text = "0";
            // 
            // chartProdutos
            // 
            this.chartProdutos.BackColor = System.Drawing.Color.Lavender;
            this.chartProdutos.BorderlineColor = System.Drawing.Color.Teal;
            this.chartProdutos.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chartProdutos.BorderlineWidth = 0;
            chartArea1.Name = "ChartArea1";
            this.chartProdutos.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartProdutos.Legends.Add(legend1);
            this.chartProdutos.Location = new System.Drawing.Point(7, 7);
            this.chartProdutos.Margin = new System.Windows.Forms.Padding(4);
            this.chartProdutos.Name = "chartProdutos";
            this.chartProdutos.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartProdutos.Series.Add(series1);
            this.chartProdutos.Size = new System.Drawing.Size(846, 547);
            this.chartProdutos.TabIndex = 0;
            this.chartProdutos.Text = "Produtos";
            // 
            // tbpgDashboadMensal
            // 
            this.tbpgDashboadMensal.Controls.Add(this.chartDoacaoMensal);
            this.tbpgDashboadMensal.Location = new System.Drawing.Point(4, 29);
            this.tbpgDashboadMensal.Name = "tbpgDashboadMensal";
            this.tbpgDashboadMensal.Padding = new System.Windows.Forms.Padding(3);
            this.tbpgDashboadMensal.Size = new System.Drawing.Size(1309, 562);
            this.tbpgDashboadMensal.TabIndex = 1;
            this.tbpgDashboadMensal.Text = "Mensais";
            this.tbpgDashboadMensal.UseVisualStyleBackColor = true;
            // 
            // chartDoacaoMensal
            // 
            this.chartDoacaoMensal.BackColor = System.Drawing.Color.Lavender;
            this.chartDoacaoMensal.BorderlineColor = System.Drawing.Color.Teal;
            this.chartDoacaoMensal.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chartDoacaoMensal.BorderlineWidth = 0;
            chartArea2.Name = "ChartArea1";
            this.chartDoacaoMensal.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartDoacaoMensal.Legends.Add(legend2);
            this.chartDoacaoMensal.Location = new System.Drawing.Point(7, 7);
            this.chartDoacaoMensal.Margin = new System.Windows.Forms.Padding(4);
            this.chartDoacaoMensal.Name = "chartDoacaoMensal";
            this.chartDoacaoMensal.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartDoacaoMensal.Series.Add(series2);
            this.chartDoacaoMensal.Size = new System.Drawing.Size(793, 548);
            this.chartDoacaoMensal.TabIndex = 1;
            this.chartDoacaoMensal.Text = "Mensal";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chartAnual);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1309, 562);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Anuais";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chartAnual
            // 
            this.chartAnual.BackColor = System.Drawing.Color.Lavender;
            this.chartAnual.BorderlineColor = System.Drawing.Color.Teal;
            this.chartAnual.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chartAnual.BorderlineWidth = 0;
            chartArea3.Name = "ChartArea1";
            this.chartAnual.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartAnual.Legends.Add(legend3);
            this.chartAnual.Location = new System.Drawing.Point(7, 7);
            this.chartAnual.Margin = new System.Windows.Forms.Padding(4);
            this.chartAnual.Name = "chartAnual";
            this.chartAnual.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartAnual.Series.Add(series3);
            this.chartAnual.Size = new System.Drawing.Size(793, 548);
            this.chartAnual.TabIndex = 2;
            this.chartAnual.Text = "Mensal";
            // 
            // pnlDadosProdutos
            // 
            this.pnlDadosProdutos.AutoSize = true;
            this.pnlDadosProdutos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.pnlDadosProdutos.Controls.Add(this.btnCadastrar);
            this.pnlDadosProdutos.Controls.Add(this.btnMenu);
            this.pnlDadosProdutos.Controls.Add(this.lblTitleDashboard);
            this.pnlDadosProdutos.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDadosProdutos.Location = new System.Drawing.Point(0, 0);
            this.pnlDadosProdutos.Name = "pnlDadosProdutos";
            this.pnlDadosProdutos.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlDadosProdutos.Size = new System.Drawing.Size(1341, 53);
            this.pnlDadosProdutos.TabIndex = 1;
            // 
            // btnCadastrar
            // 
            this.btnCadastrar.Location = new System.Drawing.Point(491, 7);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(164, 33);
            this.btnCadastrar.TabIndex = 22;
            this.btnCadastrar.Text = "Cadastrar";
            this.btnCadastrar.UseVisualStyleBackColor = true;
            this.btnCadastrar.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Location = new System.Drawing.Point(682, 7);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(164, 33);
            this.btnMenu.TabIndex = 21;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // lblTitleDashboard
            // 
            this.lblTitleDashboard.AutoSize = true;
            this.lblTitleDashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleDashboard.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitleDashboard.Location = new System.Drawing.Point(12, 9);
            this.lblTitleDashboard.Name = "lblTitleDashboard";
            this.lblTitleDashboard.Size = new System.Drawing.Size(97, 20);
            this.lblTitleDashboard.TabIndex = 12;
            this.lblTitleDashboard.Text = "Dashboard";
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(1341, 682);
            this.Controls.Add(this.tbctrlDashboardAnual);
            this.Controls.Add(this.pnlDadosProdutos);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Dashboard";
            this.Load += new System.EventHandler(this.frmDashboard_Load);
            this.tbctrlDashboardAnual.ResumeLayout(false);
            this.tbpgDashboardAlimentos.ResumeLayout(false);
            this.tbpgDashboardAlimentos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartProdutos)).EndInit();
            this.tbpgDashboadMensal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDoacaoMensal)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartAnual)).EndInit();
            this.pnlDadosProdutos.ResumeLayout(false);
            this.pnlDadosProdutos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbctrlDashboardAnual;
        private System.Windows.Forms.TabPage tbpgDashboardAlimentos;
        private System.Windows.Forms.TabPage tbpgDashboadMensal;
        private System.Windows.Forms.Panel pnlDadosProdutos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartProdutos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoacaoMensal;
        private System.Windows.Forms.Label lblTitleDashboard;
        private System.Windows.Forms.Label lblMesAtualDataReceiver;
        private System.Windows.Forms.Label lblTotalItens;
        private System.Windows.Forms.Label lblTotalQuantidade;
        private System.Windows.Forms.Label lblPeso;
        private System.Windows.Forms.Button btnCadastrar;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAnual;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox lbTotalPeso;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox txtCompativoMes;
        private System.Windows.Forms.Label lblComparativoMes;
        private System.Windows.Forms.Label lblComparativoAno;
        private System.Windows.Forms.TextBox txtCompativoAno;
    }
}