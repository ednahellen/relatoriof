namespace GPSFA_WinForms
{
    partial class frmRelatorios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRelatorios));
            this.pnlFiltrosDeBusca = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.lblTitleRelatórios = new System.Windows.Forms.Label();
            this.gpbFiltrosDoRelatorio = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbxStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxProduto = new System.Windows.Forms.ComboBox();
            this.cbbUsuario = new System.Windows.Forms.ComboBox();
            this.btnLimparFiltros = new System.Windows.Forms.Button();
            this.dtpDataFinalPeriodo = new System.Windows.Forms.DateTimePicker();
            this.lblE = new System.Windows.Forms.Label();
            this.lblDataInicial = new System.Windows.Forms.Label();
            this.dtpDataInicialPeriodo = new System.Windows.Forms.DateTimePicker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvRelatorios = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlRegistroSaida = new System.Windows.Forms.Panel();
            this.btnRegistrarSaida = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDestinoSaida = new System.Windows.Forms.TextBox();
            this.dtpDataSaida = new System.Windows.Forms.DateTimePicker();
            this.nudQuantidadeSaida = new System.Windows.Forms.NumericUpDown();
            this.cmbProdutoSaida = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAtualizarSaidas = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pnlFiltrosDeBusca.SuspendLayout();
            this.gpbFiltrosDoRelatorio.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorios)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.pnlRegistroSaida.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidadeSaida)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFiltrosDeBusca
            // 
            this.pnlFiltrosDeBusca.AutoSize = true;
            this.pnlFiltrosDeBusca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.pnlFiltrosDeBusca.Controls.Add(this.button1);
            this.pnlFiltrosDeBusca.Controls.Add(this.btnPesquisar);
            this.pnlFiltrosDeBusca.Controls.Add(this.btnMenu);
            this.pnlFiltrosDeBusca.Controls.Add(this.btnExportarExcel);
            this.pnlFiltrosDeBusca.Controls.Add(this.lblTitleRelatórios);
            this.pnlFiltrosDeBusca.Controls.Add(this.gpbFiltrosDoRelatorio);
            this.pnlFiltrosDeBusca.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltrosDeBusca.Location = new System.Drawing.Point(0, 0);
            this.pnlFiltrosDeBusca.Name = "pnlFiltrosDeBusca";
            this.pnlFiltrosDeBusca.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlFiltrosDeBusca.Size = new System.Drawing.Size(1163, 159);
            this.pnlFiltrosDeBusca.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(127, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 33);
            this.button1.TabIndex = 19;
            this.button1.Text = "Cadastrar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisar.Image = ((System.Drawing.Image)(resources.GetObject("btnPesquisar.Image")));
            this.btnPesquisar.Location = new System.Drawing.Point(976, 7);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(152, 43);
            this.btnPesquisar.TabIndex = 80;
            this.btnPesquisar.Text = "&Pesquisar";
            this.btnPesquisar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPesquisar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPesquisar.UseVisualStyleBackColor = true;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Location = new System.Drawing.Point(308, 12);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(164, 33);
            this.btnMenu.TabIndex = 18;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.Location = new System.Drawing.Point(754, 12);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(164, 33);
            this.btnExportarExcel.TabIndex = 17;
            this.btnExportarExcel.Text = "Exportar Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = true;
            // 
            // lblTitleRelatórios
            // 
            this.lblTitleRelatórios.AutoSize = true;
            this.lblTitleRelatórios.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleRelatórios.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitleRelatórios.Location = new System.Drawing.Point(12, 9);
            this.lblTitleRelatórios.Name = "lblTitleRelatórios";
            this.lblTitleRelatórios.Size = new System.Drawing.Size(114, 27);
            this.lblTitleRelatórios.TabIndex = 11;
            this.lblTitleRelatórios.Text = "Relatórios";
            // 
            // gpbFiltrosDoRelatorio
            // 
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblStatus);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.cbxStatus);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.label2);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.label1);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.cbxProduto);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.cbbUsuario);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnLimparFiltros);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.dtpDataFinalPeriodo);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblE);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblDataInicial);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.dtpDataInicialPeriodo);
            this.gpbFiltrosDoRelatorio.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpbFiltrosDoRelatorio.Location = new System.Drawing.Point(12, 51);
            this.gpbFiltrosDoRelatorio.Name = "gpbFiltrosDoRelatorio";
            this.gpbFiltrosDoRelatorio.Padding = new System.Windows.Forms.Padding(0);
            this.gpbFiltrosDoRelatorio.Size = new System.Drawing.Size(1139, 95);
            this.gpbFiltrosDoRelatorio.TabIndex = 16;
            this.gpbFiltrosDoRelatorio.TabStop = false;
            this.gpbFiltrosDoRelatorio.Text = "Filtros";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.Window;
            this.lblStatus.Location = new System.Drawing.Point(772, 25);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(70, 27);
            this.lblStatus.TabIndex = 86;
            this.lblStatus.Text = "Status";
            // 
            // cbxStatus
            // 
            this.cbxStatus.FormattingEnabled = true;
            this.cbxStatus.Location = new System.Drawing.Point(776, 54);
            this.cbxStatus.Name = "cbxStatus";
            this.cbxStatus.Size = new System.Drawing.Size(213, 35);
            this.cbxStatus.TabIndex = 85;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(526, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 27);
            this.label2.TabIndex = 84;
            this.label2.Text = "Produto";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(292, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 27);
            this.label1.TabIndex = 83;
            this.label1.Text = "Usuário";
            // 
            // cbxProduto
            // 
            this.cbxProduto.FormattingEnabled = true;
            this.cbxProduto.Location = new System.Drawing.Point(530, 54);
            this.cbxProduto.Name = "cbxProduto";
            this.cbxProduto.Size = new System.Drawing.Size(232, 35);
            this.cbxProduto.TabIndex = 81;
            // 
            // cbbUsuario
            // 
            this.cbbUsuario.FormattingEnabled = true;
            this.cbbUsuario.Location = new System.Drawing.Point(296, 54);
            this.cbbUsuario.Name = "cbbUsuario";
            this.cbbUsuario.Size = new System.Drawing.Size(223, 35);
            this.cbbUsuario.TabIndex = 81;
            // 
            // btnLimparFiltros
            // 
            this.btnLimparFiltros.Location = new System.Drawing.Point(1008, 54);
            this.btnLimparFiltros.Name = "btnLimparFiltros";
            this.btnLimparFiltros.Size = new System.Drawing.Size(120, 33);
            this.btnLimparFiltros.TabIndex = 9;
            this.btnLimparFiltros.Text = "&Limpar filtros";
            this.btnLimparFiltros.UseVisualStyleBackColor = true;
            // 
            // dtpDataFinalPeriodo
            // 
            this.dtpDataFinalPeriodo.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDataFinalPeriodo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataFinalPeriodo.Location = new System.Drawing.Point(156, 54);
            this.dtpDataFinalPeriodo.Name = "dtpDataFinalPeriodo";
            this.dtpDataFinalPeriodo.Size = new System.Drawing.Size(123, 34);
            this.dtpDataFinalPeriodo.TabIndex = 2;
            // 
            // lblE
            // 
            this.lblE.AutoSize = true;
            this.lblE.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblE.ForeColor = System.Drawing.SystemColors.Window;
            this.lblE.Location = new System.Drawing.Point(152, 27);
            this.lblE.Name = "lblE";
            this.lblE.Size = new System.Drawing.Size(47, 27);
            this.lblE.TabIndex = 5;
            this.lblE.Text = "Fim";
            // 
            // lblDataInicial
            // 
            this.lblDataInicial.AutoSize = true;
            this.lblDataInicial.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataInicial.ForeColor = System.Drawing.SystemColors.Window;
            this.lblDataInicial.Location = new System.Drawing.Point(14, 27);
            this.lblDataInicial.Name = "lblDataInicial";
            this.lblDataInicial.Size = new System.Drawing.Size(63, 27);
            this.lblDataInicial.TabIndex = 4;
            this.lblDataInicial.Text = "Início";
            // 
            // dtpDataInicialPeriodo
            // 
            this.dtpDataInicialPeriodo.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDataInicialPeriodo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataInicialPeriodo.Location = new System.Drawing.Point(18, 54);
            this.dtpDataInicialPeriodo.Name = "dtpDataInicialPeriodo";
            this.dtpDataInicialPeriodo.Size = new System.Drawing.Size(123, 34);
            this.dtpDataInicialPeriodo.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 165);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1163, 460);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvRelatorios);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1155, 422);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "📊 Relatório de Estoque";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvRelatorios
            // 
            this.dgvRelatorios.AllowUserToOrderColumns = true;
            this.dgvRelatorios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRelatorios.Location = new System.Drawing.Point(3, 3);
            this.dgvRelatorios.Name = "dgvRelatorios";
            this.dgvRelatorios.RowHeadersWidth = 51;
            this.dgvRelatorios.Size = new System.Drawing.Size(1149, 416);
            this.dgvRelatorios.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pnlRegistroSaida);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1155, 422);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "📝 Registrar Saída";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pnlRegistroSaida
            // 
            this.pnlRegistroSaida.Controls.Add(this.btnRegistrarSaida);
            this.pnlRegistroSaida.Controls.Add(this.label6);
            this.pnlRegistroSaida.Controls.Add(this.txtDestinoSaida);
            this.pnlRegistroSaida.Controls.Add(this.dtpDataSaida);
            this.pnlRegistroSaida.Controls.Add(this.nudQuantidadeSaida);
            this.pnlRegistroSaida.Controls.Add(this.cmbProdutoSaida);
            this.pnlRegistroSaida.Controls.Add(this.label5);
            this.pnlRegistroSaida.Controls.Add(this.label4);
            this.pnlRegistroSaida.Controls.Add(this.label3);
            this.pnlRegistroSaida.Controls.Add(this.btnAtualizarSaidas);
            this.pnlRegistroSaida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRegistroSaida.Location = new System.Drawing.Point(3, 3);
            this.pnlRegistroSaida.Name = "pnlRegistroSaida";
            this.pnlRegistroSaida.Size = new System.Drawing.Size(1149, 416);
            this.pnlRegistroSaida.TabIndex = 0;
            // 
            // btnRegistrarSaida
            // 
            this.btnRegistrarSaida.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnRegistrarSaida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnRegistrarSaida.ForeColor = System.Drawing.Color.White;
            this.btnRegistrarSaida.Location = new System.Drawing.Point(168, 250);
            this.btnRegistrarSaida.Name = "btnRegistrarSaida";
            this.btnRegistrarSaida.Size = new System.Drawing.Size(200, 40);
            this.btnRegistrarSaida.TabIndex = 29;
            this.btnRegistrarSaida.Text = "REGISTRAR SAÍDA";
            this.btnRegistrarSaida.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(49, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 25);
            this.label6.TabIndex = 28;
            this.label6.Text = "Destino:";
            // 
            // txtDestinoSaida
            // 
            this.txtDestinoSaida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtDestinoSaida.Location = new System.Drawing.Point(168, 188);
            this.txtDestinoSaida.Name = "txtDestinoSaida";
            this.txtDestinoSaida.Size = new System.Drawing.Size(350, 30);
            this.txtDestinoSaida.TabIndex = 27;
            // 
            // dtpDataSaida
            // 
            this.dtpDataSaida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpDataSaida.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataSaida.Location = new System.Drawing.Point(216, 138);
            this.dtpDataSaida.Name = "dtpDataSaida";
            this.dtpDataSaida.Size = new System.Drawing.Size(139, 30);
            this.dtpDataSaida.TabIndex = 26;
            // 
            // nudQuantidadeSaida
            // 
            this.nudQuantidadeSaida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nudQuantidadeSaida.Location = new System.Drawing.Point(199, 88);
            this.nudQuantidadeSaida.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantidadeSaida.Name = "nudQuantidadeSaida";
            this.nudQuantidadeSaida.Size = new System.Drawing.Size(120, 30);
            this.nudQuantidadeSaida.TabIndex = 25;
            this.nudQuantidadeSaida.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbProdutoSaida
            // 
            this.cmbProdutoSaida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbProdutoSaida.FormattingEnabled = true;
            this.cmbProdutoSaida.Location = new System.Drawing.Point(168, 37);
            this.cmbProdutoSaida.Name = "cmbProdutoSaida";
            this.cmbProdutoSaida.Size = new System.Drawing.Size(350, 33);
            this.cmbProdutoSaida.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(49, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 25);
            this.label5.TabIndex = 23;
            this.label5.Text = "Data de Saída:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(49, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 25);
            this.label4.TabIndex = 22;
            this.label4.Text = "Quantidade:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(49, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 25);
            this.label3.TabIndex = 21;
            this.label3.Text = "Produto:";
            // 
            // btnAtualizarSaidas
            // 
            this.btnAtualizarSaidas.Location = new System.Drawing.Point(53, 320);
            this.btnAtualizarSaidas.Name = "btnAtualizarSaidas";
            this.btnAtualizarSaidas.Size = new System.Drawing.Size(164, 33);
            this.btnAtualizarSaidas.TabIndex = 20;
            this.btnAtualizarSaidas.Text = "Atualizar Lista";
            this.btnAtualizarSaidas.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1155, 431);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "📋 Lista de Saídas";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // frmRelatorios
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(1163, 625);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnlFiltrosDeBusca);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmRelatorios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Relatórios";
            this.pnlFiltrosDeBusca.ResumeLayout(false);
            this.pnlFiltrosDeBusca.PerformLayout();
            this.gpbFiltrosDoRelatorio.ResumeLayout(false);
            this.gpbFiltrosDoRelatorio.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorios)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.pnlRegistroSaida.ResumeLayout(false);
            this.pnlRegistroSaida.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidadeSaida)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Painéis e containers
        private System.Windows.Forms.Panel pnlFiltrosDeBusca;
        private System.Windows.Forms.GroupBox gpbFiltrosDoRelatorio;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel pnlRegistroSaida;

        // Controles de filtro
        private System.Windows.Forms.DateTimePicker dtpDataInicialPeriodo;
        private System.Windows.Forms.DateTimePicker dtpDataFinalPeriodo;
        private System.Windows.Forms.Label lblDataInicial;
        private System.Windows.Forms.Label lblE;
        private System.Windows.Forms.ComboBox cbbUsuario;
        private System.Windows.Forms.ComboBox cbxProduto;
        private System.Windows.Forms.ComboBox cbxStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;

        // Botões
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.Button btnLimparFiltros;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button button1;

        // Labels
        private System.Windows.Forms.Label lblTitleRelatórios;

        // DataGridViews
        private System.Windows.Forms.DataGridView dgvRelatorios;

        // Controles da tabPage2 (Registro de Saída)
        private System.Windows.Forms.ComboBox cmbProdutoSaida;
        private System.Windows.Forms.NumericUpDown nudQuantidadeSaida;
        private System.Windows.Forms.DateTimePicker dtpDataSaida;
        private System.Windows.Forms.TextBox txtDestinoSaida;
        private System.Windows.Forms.Button btnRegistrarSaida;
        private System.Windows.Forms.Button btnAtualizarSaidas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;

    }
}