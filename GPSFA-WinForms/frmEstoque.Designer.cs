namespace GPSFA_WinForms
{
    partial class frmEstoque
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEstoque));
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbEstoque = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvEstoque = new System.Windows.Forms.DataGridView();
            this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Saída = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Peso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Validade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cadastrado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageSaidas = new System.Windows.Forms.TabPage();
            this.tabPageRegistrarSaida = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnProdutosPrincipais = new System.Windows.Forms.Button();
            this.btnAplicarModo = new System.Windows.Forms.Button();
            this.lblTitleEstoque = new System.Windows.Forms.Label();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.gpbFiltrosDoRelatorio = new System.Windows.Forms.GroupBox();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnLimparFiltros = new System.Windows.Forms.Button();
            this.btnAplicarFiltros = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dtpDataValidade = new System.Windows.Forms.DateTimePicker();
            this.lblValidadeAte = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.cbxModoExibicao = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbxprodutoSelecionado = new System.Windows.Forms.ComboBox();
            this.lblProduto = new System.Windows.Forms.Label();
            this.pnlFiltrosDeBusca = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.tbEstoque.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).BeginInit();
            this.panel1.SuspendLayout();
            this.gpbFiltrosDoRelatorio.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlFiltrosDeBusca.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.panel2.Controls.Add(this.tbEstoque);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Location = new System.Drawing.Point(0, 5);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1323, 1409);
            this.panel2.TabIndex = 20;
            // 
            // tbEstoque
            // 
            this.tbEstoque.Controls.Add(this.tabPage1);
            this.tbEstoque.Controls.Add(this.tabPageSaidas);
            this.tbEstoque.Controls.Add(this.tabPageRegistrarSaida);
            this.tbEstoque.Location = new System.Drawing.Point(0, 238);
            this.tbEstoque.Name = "tbEstoque";
            this.tbEstoque.SelectedIndex = 0;
            this.tbEstoque.Size = new System.Drawing.Size(1317, 457);
            this.tbEstoque.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvEstoque);
            this.tabPage1.Location = new System.Drawing.Point(4, 36);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1309, 417);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Estoque";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvEstoque
            // 
            this.dgvEstoque.AllowUserToAddRows = false;
            this.dgvEstoque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEstoque.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Data,
            this.Saída,
            this.Quantidade,
            this.Peso,
            this.Validade,
            this.Cadastrado});
            this.dgvEstoque.Location = new System.Drawing.Point(2, 0);
            this.dgvEstoque.Margin = new System.Windows.Forms.Padding(5);
            this.dgvEstoque.Name = "dgvEstoque";
            this.dgvEstoque.ReadOnly = true;
            this.dgvEstoque.RowHeadersWidth = 51;
            this.dgvEstoque.Size = new System.Drawing.Size(1309, 409);
            this.dgvEstoque.TabIndex = 6;
            // 
            // Data
            // 
            this.Data.HeaderText = "Data Entrada";
            this.Data.MinimumWidth = 6;
            this.Data.Name = "Data";
            this.Data.ReadOnly = true;
            this.Data.Width = 125;
            // 
            // Saída
            // 
            this.Saída.HeaderText = "Data Saída";
            this.Saída.MinimumWidth = 6;
            this.Saída.Name = "Saída";
            this.Saída.ReadOnly = true;
            this.Saída.Width = 125;
            // 
            // Quantidade
            // 
            this.Quantidade.HeaderText = "Quantidade";
            this.Quantidade.MinimumWidth = 6;
            this.Quantidade.Name = "Quantidade";
            this.Quantidade.ReadOnly = true;
            this.Quantidade.Width = 125;
            // 
            // Peso
            // 
            this.Peso.HeaderText = "Peso";
            this.Peso.MinimumWidth = 6;
            this.Peso.Name = "Peso";
            this.Peso.ReadOnly = true;
            this.Peso.Width = 125;
            // 
            // Validade
            // 
            this.Validade.HeaderText = "Data Validade";
            this.Validade.MinimumWidth = 6;
            this.Validade.Name = "Validade";
            this.Validade.ReadOnly = true;
            this.Validade.Width = 125;
            // 
            // Cadastrado
            // 
            this.Cadastrado.HeaderText = "Cadastrado";
            this.Cadastrado.MinimumWidth = 6;
            this.Cadastrado.Name = "Cadastrado";
            this.Cadastrado.ReadOnly = true;
            this.Cadastrado.Width = 125;
            // 
            // tabPageSaidas
            // 
            this.tabPageSaidas.Location = new System.Drawing.Point(4, 36);
            this.tabPageSaidas.Name = "tabPageSaidas";
            this.tabPageSaidas.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSaidas.Size = new System.Drawing.Size(1309, 417);
            this.tabPageSaidas.TabIndex = 1;
            this.tabPageSaidas.Text = "Histórico de Saídas";
            this.tabPageSaidas.UseVisualStyleBackColor = true;
            // 
            // tabPageRegistrarSaida
            // 
            this.tabPageRegistrarSaida.Location = new System.Drawing.Point(4, 36);
            this.tabPageRegistrarSaida.Name = "tabPageRegistrarSaida";
            this.tabPageRegistrarSaida.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegistrarSaida.Size = new System.Drawing.Size(1309, 417);
            this.tabPageRegistrarSaida.TabIndex = 2;
            this.tabPageRegistrarSaida.Text = "Registrar Saida";
            this.tabPageRegistrarSaida.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.panel1.Controls.Add(this.btnProdutosPrincipais);
            this.panel1.Controls.Add(this.btnAplicarModo);
            this.panel1.Controls.Add(this.lblTitleEstoque);
            this.panel1.Controls.Add(this.btnExportarExcel);
            this.panel1.Controls.Add(this.gpbFiltrosDoRelatorio);
            this.panel1.Controls.Add(this.cbxModoExibicao);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.panel1.Size = new System.Drawing.Size(1323, 146);
            this.panel1.TabIndex = 18;
            // 
            // btnProdutosPrincipais
            // 
            this.btnProdutosPrincipais.Location = new System.Drawing.Point(836, 12);
            this.btnProdutosPrincipais.Name = "btnProdutosPrincipais";
            this.btnProdutosPrincipais.Size = new System.Drawing.Size(164, 33);
            this.btnProdutosPrincipais.TabIndex = 21;
            this.btnProdutosPrincipais.Text = "Produtos Principais";
            this.btnProdutosPrincipais.UseVisualStyleBackColor = true;
            // 
            // btnAplicarModo
            // 
            this.btnAplicarModo.Location = new System.Drawing.Point(450, 12);
            this.btnAplicarModo.Name = "btnAplicarModo";
            this.btnAplicarModo.Size = new System.Drawing.Size(164, 33);
            this.btnAplicarModo.TabIndex = 19;
            this.btnAplicarModo.Text = "Aplicar Modo";
            this.btnAplicarModo.UseVisualStyleBackColor = true;
            // 
            // lblTitleEstoque
            // 
            this.lblTitleEstoque.AutoSize = true;
            this.lblTitleEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleEstoque.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitleEstoque.Location = new System.Drawing.Point(8, 12);
            this.lblTitleEstoque.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTitleEstoque.Name = "lblTitleEstoque";
            this.lblTitleEstoque.Size = new System.Drawing.Size(225, 25);
            this.lblTitleEstoque.TabIndex = 11;
            this.lblTitleEstoque.Text = "Controle de Alimentos";
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.Location = new System.Drawing.Point(1125, 12);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(164, 33);
            this.btnExportarExcel.TabIndex = 5;
            this.btnExportarExcel.Text = "Imprimir";
            this.btnExportarExcel.UseVisualStyleBackColor = true;
            // 
            // gpbFiltrosDoRelatorio
            // 
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnImportar);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnLimparFiltros);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnAplicarFiltros);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnMenu);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnSair);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.button3);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.dtpDataValidade);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblValidadeAte);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.button4);
            this.gpbFiltrosDoRelatorio.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpbFiltrosDoRelatorio.Location = new System.Drawing.Point(3, 58);
            this.gpbFiltrosDoRelatorio.Name = "gpbFiltrosDoRelatorio";
            this.gpbFiltrosDoRelatorio.Padding = new System.Windows.Forms.Padding(0);
            this.gpbFiltrosDoRelatorio.Size = new System.Drawing.Size(1314, 75);
            this.gpbFiltrosDoRelatorio.TabIndex = 16;
            this.gpbFiltrosDoRelatorio.TabStop = false;
            this.gpbFiltrosDoRelatorio.Text = "Filtros";
            // 
            // btnImportar
            // 
            this.btnImportar.Location = new System.Drawing.Point(856, 24);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(164, 33);
            this.btnImportar.TabIndex = 81;
            this.btnImportar.Text = "Importar";
            this.btnImportar.UseVisualStyleBackColor = true;
            // 
            // btnLimparFiltros
            // 
            this.btnLimparFiltros.Location = new System.Drawing.Point(660, 25);
            this.btnLimparFiltros.Name = "btnLimparFiltros";
            this.btnLimparFiltros.Size = new System.Drawing.Size(164, 33);
            this.btnLimparFiltros.TabIndex = 80;
            this.btnLimparFiltros.Text = "Limpar Filtros";
            this.btnLimparFiltros.UseVisualStyleBackColor = true;
            // 
            // btnAplicarFiltros
            // 
            this.btnAplicarFiltros.Location = new System.Drawing.Point(460, 23);
            this.btnAplicarFiltros.Name = "btnAplicarFiltros";
            this.btnAplicarFiltros.Size = new System.Drawing.Size(164, 33);
            this.btnAplicarFiltros.TabIndex = 79;
            this.btnAplicarFiltros.Text = "Aplicar Filtros";
            this.btnAplicarFiltros.UseVisualStyleBackColor = true;
            // 
            // btnMenu
            // 
            this.btnMenu.FlatAppearance.BorderSize = 2;
            this.btnMenu.Location = new System.Drawing.Point(1121, 23);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(164, 33);
            this.btnMenu.TabIndex = 10;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click_1);
            // 
            // btnSair
            // 
            this.btnSair.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.Location = new System.Drawing.Point(1711, 31);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(126, 41);
            this.btnSair.TabIndex = 78;
            this.btnSair.Text = "&Voltar";
            this.btnSair.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1556, 81);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(142, 41);
            this.button3.TabIndex = 9;
            this.button3.Text = "&Limpar filtros";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // dtpDataValidade
            // 
            this.dtpDataValidade.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDataValidade.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDataValidade.Location = new System.Drawing.Point(189, 23);
            this.dtpDataValidade.Margin = new System.Windows.Forms.Padding(5);
            this.dtpDataValidade.Name = "dtpDataValidade";
            this.dtpDataValidade.Size = new System.Drawing.Size(186, 34);
            this.dtpDataValidade.TabIndex = 8;
            // 
            // lblValidadeAte
            // 
            this.lblValidadeAte.AutoSize = true;
            this.lblValidadeAte.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblValidadeAte.ForeColor = System.Drawing.Color.White;
            this.lblValidadeAte.Location = new System.Drawing.Point(40, 33);
            this.lblValidadeAte.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblValidadeAte.Name = "lblValidadeAte";
            this.lblValidadeAte.Size = new System.Drawing.Size(135, 27);
            this.lblValidadeAte.TabIndex = 7;
            this.lblValidadeAte.Text = "Validade até:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1408, 81);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(142, 41);
            this.button4.TabIndex = 8;
            this.button4.Text = "&Aplicar Filtros";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // cbxModoExibicao
            // 
            this.cbxModoExibicao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxModoExibicao.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxModoExibicao.FormattingEnabled = true;
            this.cbxModoExibicao.Items.AddRange(new object[] {
            "Selecione...",
            "Modo agrupado",
            "Modo detalhado"});
            this.cbxModoExibicao.Location = new System.Drawing.Point(243, 12);
            this.cbxModoExibicao.Margin = new System.Windows.Forms.Padding(5);
            this.cbxModoExibicao.Name = "cbxModoExibicao";
            this.cbxModoExibicao.Size = new System.Drawing.Size(186, 35);
            this.cbxModoExibicao.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
            this.panel4.Controls.Add(this.cbxprodutoSelecionado);
            this.panel4.Controls.Add(this.lblProduto);
            this.panel4.Location = new System.Drawing.Point(3, 147);
            this.panel4.Margin = new System.Windows.Forms.Padding(5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1309, 92);
            this.panel4.TabIndex = 13;
            // 
            // cbxprodutoSelecionado
            // 
            this.cbxprodutoSelecionado.FormattingEnabled = true;
            this.cbxprodutoSelecionado.Location = new System.Drawing.Point(17, 31);
            this.cbxprodutoSelecionado.Name = "cbxprodutoSelecionado";
            this.cbxprodutoSelecionado.Size = new System.Drawing.Size(513, 35);
            this.cbxprodutoSelecionado.TabIndex = 14;
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblProduto.Location = new System.Drawing.Point(24, 1);
            this.lblProduto.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(183, 27);
            this.lblProduto.TabIndex = 2;
            this.lblProduto.Text = "Código ou Nome:";
            // 
            // pnlFiltrosDeBusca
            // 
            this.pnlFiltrosDeBusca.AutoSize = true;
            this.pnlFiltrosDeBusca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.pnlFiltrosDeBusca.Controls.Add(this.panel2);
            this.pnlFiltrosDeBusca.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltrosDeBusca.Location = new System.Drawing.Point(0, 0);
            this.pnlFiltrosDeBusca.Margin = new System.Windows.Forms.Padding(5);
            this.pnlFiltrosDeBusca.Name = "pnlFiltrosDeBusca";
            this.pnlFiltrosDeBusca.Padding = new System.Windows.Forms.Padding(17, 0, 17, 16);
            this.pnlFiltrosDeBusca.Size = new System.Drawing.Size(1337, 1435);
            this.pnlFiltrosDeBusca.TabIndex = 15;
            // 
            // frmEstoque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1337, 712);
            this.Controls.Add(this.pnlFiltrosDeBusca);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEstoque";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Formulário de estoque ";
            this.Load += new System.EventHandler(this.frmEstoque_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tbEstoque.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gpbFiltrosDoRelatorio.ResumeLayout(false);
            this.gpbFiltrosDoRelatorio.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlFiltrosDeBusca.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitleEstoque;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.GroupBox gpbFiltrosDoRelatorio;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DateTimePicker dtpDataValidade;
        private System.Windows.Forms.Label lblValidadeAte;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cbxModoExibicao;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.Panel pnlFiltrosDeBusca;
        private System.Windows.Forms.Button btnAplicarModo;
        private System.Windows.Forms.Button btnAplicarFiltros;
        private System.Windows.Forms.Button btnProdutosPrincipais;
        private System.Windows.Forms.ComboBox cbxprodutoSelecionado;
        private System.Windows.Forms.Button btnLimparFiltros;
        private System.Windows.Forms.TabControl tbEstoque;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvEstoque;
        private System.Windows.Forms.DataGridViewTextBoxColumn Data;
        private System.Windows.Forms.DataGridViewTextBoxColumn Saída;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Peso;
        private System.Windows.Forms.DataGridViewTextBoxColumn Validade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cadastrado;
        private System.Windows.Forms.TabPage tabPageSaidas;
        private System.Windows.Forms.TabPage tabPageRegistrarSaida;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageEstoque;
        private System.Windows.Forms.Button btnImportar;
    }
}