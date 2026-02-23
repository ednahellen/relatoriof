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
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalPeso = new System.Windows.Forms.Label();
            this.lblTotalQuantidade = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalEmQuilosDataReceiver = new System.Windows.Forms.Label();
            this.lblMesAtualDataReceiver = new System.Windows.Forms.Label();
            this.lblTotalItens = new System.Windows.Forms.Label();
            this.lblTotalEmQuilos = new System.Windows.Forms.Label();
            this.lblTotalDeItens = new System.Windows.Forms.Label();
            this.lblMesAtual = new System.Windows.Forms.Label();
            this.chartProdutos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpgDashboadMensal = new System.Windows.Forms.TabPage();
            this.chartDoacaoMensal = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlDadosProdutos = new System.Windows.Forms.Panel();
            this.btnCadastrar = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.lblTitleDashboard = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalProdutos = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chartAnual = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbctrlDashboardAnual.SuspendLayout();
            this.tbpgDashboardAlimentos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartProdutos)).BeginInit();
            this.tbpgDashboadMensal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDoacaoMensal)).BeginInit();
            this.pnlDadosProdutos.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartAnual)).BeginInit();
            this.SuspendLayout();
            // 
            // tbctrlDashboardAnual
            // 
            this.tbctrlDashboardAnual.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbctrlDashboardAnual.Controls.Add(this.tbpgDashboardAlimentos);
            this.tbctrlDashboardAnual.Controls.Add(this.tbpgDashboadMensal);
            this.tbctrlDashboardAnual.Controls.Add(this.tabPage1);
            this.tbctrlDashboardAnual.Location = new System.Drawing.Point(12, 45);
            this.tbctrlDashboardAnual.Name = "tbctrlDashboardAnual";
            this.tbctrlDashboardAnual.SelectedIndex = 0;
            this.tbctrlDashboardAnual.Size = new System.Drawing.Size(1624, 713);
            this.tbctrlDashboardAnual.TabIndex = 0;
            // 
            // tbpgDashboardAlimentos
            // 
            this.tbpgDashboardAlimentos.Controls.Add(this.label3);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalProdutos);
            this.tbpgDashboardAlimentos.Controls.Add(this.label1);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalPeso);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalQuantidade);
            this.tbpgDashboardAlimentos.Controls.Add(this.label2);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalEmQuilosDataReceiver);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblMesAtualDataReceiver);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalItens);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalEmQuilos);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblTotalDeItens);
            this.tbpgDashboardAlimentos.Controls.Add(this.lblMesAtual);
            this.tbpgDashboardAlimentos.Controls.Add(this.chartProdutos);
            this.tbpgDashboardAlimentos.Location = new System.Drawing.Point(4, 29);
            this.tbpgDashboardAlimentos.Name = "tbpgDashboardAlimentos";
            this.tbpgDashboardAlimentos.Padding = new System.Windows.Forms.Padding(3);
            this.tbpgDashboardAlimentos.Size = new System.Drawing.Size(1616, 680);
            this.tbpgDashboardAlimentos.TabIndex = 0;
            this.tbpgDashboardAlimentos.Text = "Alimentos mais recebidos";
            this.tbpgDashboardAlimentos.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(1098, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "0";
            // 
            // lblTotalPeso
            // 
            this.lblTotalPeso.AutoSize = true;
            this.lblTotalPeso.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPeso.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalPeso.Location = new System.Drawing.Point(881, 189);
            this.lblTotalPeso.Name = "lblTotalPeso";
            this.lblTotalPeso.Size = new System.Drawing.Size(104, 24);
            this.lblTotalPeso.TabIndex = 14;
            this.lblTotalPeso.Text = "Total Peso:";
            // 
            // lblTotalQuantidade
            // 
            this.lblTotalQuantidade.AutoSize = true;
            this.lblTotalQuantidade.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalQuantidade.Location = new System.Drawing.Point(1097, 153);
            this.lblTotalQuantidade.Name = "lblTotalQuantidade";
            this.lblTotalQuantidade.Size = new System.Drawing.Size(18, 20);
            this.lblTotalQuantidade.TabIndex = 13;
            this.lblTotalQuantidade.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(880, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = "Total Quantidade:";
            // 
            // lblTotalEmQuilosDataReceiver
            // 
            this.lblTotalEmQuilosDataReceiver.AutoSize = true;
            this.lblTotalEmQuilosDataReceiver.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalEmQuilosDataReceiver.Location = new System.Drawing.Point(1076, 113);
            this.lblTotalEmQuilosDataReceiver.Name = "lblTotalEmQuilosDataReceiver";
            this.lblTotalEmQuilosDataReceiver.Size = new System.Drawing.Size(18, 20);
            this.lblTotalEmQuilosDataReceiver.TabIndex = 9;
            this.lblTotalEmQuilosDataReceiver.Text = "0";
            // 
            // lblMesAtualDataReceiver
            // 
            this.lblMesAtualDataReceiver.AutoSize = true;
            this.lblMesAtualDataReceiver.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMesAtualDataReceiver.Location = new System.Drawing.Point(1033, 44);
            this.lblMesAtualDataReceiver.Name = "lblMesAtualDataReceiver";
            this.lblMesAtualDataReceiver.Size = new System.Drawing.Size(39, 20);
            this.lblMesAtualDataReceiver.TabIndex = 11;
            this.lblMesAtualDataReceiver.Text = "Mês";
            // 
            // lblTotalItens
            // 
            this.lblTotalItens.AutoSize = true;
            this.lblTotalItens.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalItens.Location = new System.Drawing.Point(1063, 75);
            this.lblTotalItens.Name = "lblTotalItens";
            this.lblTotalItens.Size = new System.Drawing.Size(18, 20);
            this.lblTotalItens.TabIndex = 8;
            this.lblTotalItens.Text = "0";
            // 
            // lblTotalEmQuilos
            // 
            this.lblTotalEmQuilos.AutoSize = true;
            this.lblTotalEmQuilos.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalEmQuilos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalEmQuilos.Location = new System.Drawing.Point(880, 109);
            this.lblTotalEmQuilos.Name = "lblTotalEmQuilos";
            this.lblTotalEmQuilos.Size = new System.Drawing.Size(147, 24);
            this.lblTotalEmQuilos.TabIndex = 7;
            this.lblTotalEmQuilos.Text = "Total em Quilos:";
            // 
            // lblTotalDeItens
            // 
            this.lblTotalDeItens.AutoSize = true;
            this.lblTotalDeItens.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDeItens.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalDeItens.Location = new System.Drawing.Point(880, 75);
            this.lblTotalDeItens.Name = "lblTotalDeItens";
            this.lblTotalDeItens.Size = new System.Drawing.Size(127, 24);
            this.lblTotalDeItens.TabIndex = 6;
            this.lblTotalDeItens.Text = "Total de Itens:";
            // 
            // lblMesAtual
            // 
            this.lblMesAtual.AutoSize = true;
            this.lblMesAtual.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMesAtual.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMesAtual.Location = new System.Drawing.Point(880, 41);
            this.lblMesAtual.Name = "lblMesAtual";
            this.lblMesAtual.Size = new System.Drawing.Size(95, 24);
            this.lblMesAtual.TabIndex = 10;
            this.lblMesAtual.Text = "Mês atual:";
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
            this.chartProdutos.Location = new System.Drawing.Point(4, 0);
            this.chartProdutos.Margin = new System.Windows.Forms.Padding(4);
            this.chartProdutos.Name = "chartProdutos";
            this.chartProdutos.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartProdutos.Series.Add(series1);
            this.chartProdutos.Size = new System.Drawing.Size(853, 668);
            this.chartProdutos.TabIndex = 0;
            this.chartProdutos.Text = "Produtos";
            // 
            // tbpgDashboadMensal
            // 
            this.tbpgDashboadMensal.Controls.Add(this.chartDoacaoMensal);
            this.tbpgDashboadMensal.Location = new System.Drawing.Point(4, 29);
            this.tbpgDashboadMensal.Name = "tbpgDashboadMensal";
            this.tbpgDashboadMensal.Padding = new System.Windows.Forms.Padding(3);
            this.tbpgDashboadMensal.Size = new System.Drawing.Size(1616, 680);
            this.tbpgDashboadMensal.TabIndex = 1;
            this.tbpgDashboadMensal.Text = "Alimentos mensais";
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
            this.chartDoacaoMensal.Location = new System.Drawing.Point(0, 4);
            this.chartDoacaoMensal.Margin = new System.Windows.Forms.Padding(4);
            this.chartDoacaoMensal.Name = "chartDoacaoMensal";
            this.chartDoacaoMensal.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartDoacaoMensal.Series.Add(series2);
            this.chartDoacaoMensal.Size = new System.Drawing.Size(800, 661);
            this.chartDoacaoMensal.TabIndex = 1;
            this.chartDoacaoMensal.Text = "Mensal";
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
            this.pnlDadosProdutos.Size = new System.Drawing.Size(1647, 53);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(1098, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "0";
            // 
            // lblTotalProdutos
            // 
            this.lblTotalProdutos.AutoSize = true;
            this.lblTotalProdutos.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalProdutos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalProdutos.Location = new System.Drawing.Point(881, 233);
            this.lblTotalProdutos.Name = "lblTotalProdutos";
            this.lblTotalProdutos.Size = new System.Drawing.Size(136, 24);
            this.lblTotalProdutos.TabIndex = 16;
            this.lblTotalProdutos.Text = "Total Produtos:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chartAnual);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1616, 680);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "tabPage1";
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
            this.chartAnual.Location = new System.Drawing.Point(0, 2);
            this.chartAnual.Margin = new System.Windows.Forms.Padding(4);
            this.chartAnual.Name = "chartAnual";
            this.chartAnual.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartAnual.Series.Add(series3);
            this.chartAnual.Size = new System.Drawing.Size(800, 661);
            this.chartAnual.TabIndex = 2;
            this.chartAnual.Text = "Mensal";
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(1647, 770);
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
            this.pnlDadosProdutos.ResumeLayout(false);
            this.pnlDadosProdutos.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartAnual)).EndInit();
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
        private System.Windows.Forms.Label lblTotalDeItens;
        private System.Windows.Forms.Label lblMesAtual;
        private System.Windows.Forms.Label lblTotalQuantidade;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalPeso;
        private System.Windows.Forms.Label lblTotalEmQuilosDataReceiver;
        private System.Windows.Forms.Label lblTotalEmQuilos;
        private System.Windows.Forms.Button btnCadastrar;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTotalProdutos;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAnual;
    }
}