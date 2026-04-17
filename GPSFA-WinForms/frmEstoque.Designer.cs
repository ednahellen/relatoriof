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
            this.btnImportar = new System.Windows.Forms.Button();
            this.lblTitleEstoque = new System.Windows.Forms.Label();
            this.btnExportar = new System.Windows.Forms.Button();
            this.gpbFiltrosDoRelatorio = new System.Windows.Forms.GroupBox();
            this.btnCadastrar = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnLimparFiltros = new System.Windows.Forms.Button();
            this.btnAplicarFiltros = new System.Windows.Forms.Button();
            this.pnlFiltrosDeBusca = new System.Windows.Forms.Panel();
            this.cbxprodutoSelecionado = new System.Windows.Forms.ComboBox();
            this.btnProdutosPrincipais = new System.Windows.Forms.Button();
            this.lblProduto = new System.Windows.Forms.Label();
            this.btnSincronizar = new System.Windows.Forms.Button();
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
            this.tabPage1.Location = new System.Drawing.Point(4, 30);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1309, 423);
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
            this.tabPageSaidas.Location = new System.Drawing.Point(4, 30);
            this.tabPageSaidas.Name = "tabPageSaidas";
            this.tabPageSaidas.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSaidas.Size = new System.Drawing.Size(1309, 423);
            this.tabPageSaidas.TabIndex = 1;
            this.tabPageSaidas.Text = "Histórico de Saídas";
            this.tabPageSaidas.UseVisualStyleBackColor = true;
            // 
            // tabPageRegistrarSaida
            // 
            this.tabPageRegistrarSaida.Location = new System.Drawing.Point(4, 30);
            this.tabPageRegistrarSaida.Name = "tabPageRegistrarSaida";
            this.tabPageRegistrarSaida.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegistrarSaida.Size = new System.Drawing.Size(1309, 423);
            this.tabPageRegistrarSaida.TabIndex = 2;
            this.tabPageRegistrarSaida.Text = "Registrar Saida";
            this.tabPageRegistrarSaida.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.panel1.Controls.Add(this.btnProdutosPrincipais);
            this.panel1.Controls.Add(this.btnImportar);
            this.panel1.Controls.Add(this.lblTitleEstoque);
            this.panel1.Controls.Add(this.btnExportar);
            this.panel1.Controls.Add(this.gpbFiltrosDoRelatorio);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.panel1.Size = new System.Drawing.Size(1323, 146);
            this.panel1.TabIndex = 18;
            // 
            // btnImportar
            // 
            this.btnImportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnImportar.Location = new System.Drawing.Point(951, 14);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(152, 43);
            this.btnImportar.TabIndex = 81;
            this.btnImportar.Text = "Importar";
            this.btnImportar.UseVisualStyleBackColor = false;
            // 
            // lblTitleEstoque
            // 
            this.lblTitleEstoque.AutoSize = true;
            this.lblTitleEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleEstoque.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitleEstoque.Location = new System.Drawing.Point(8, 12);
            this.lblTitleEstoque.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTitleEstoque.Name = "lblTitleEstoque";
            this.lblTitleEstoque.Size = new System.Drawing.Size(186, 20);
            this.lblTitleEstoque.TabIndex = 11;
            this.lblTitleEstoque.Text = "Controle de Alimentos";
            // 
            // btnExportar
            // 
            this.btnExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnExportar.Location = new System.Drawing.Point(1125, 12);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(152, 43);
            this.btnExportar.TabIndex = 5;
            this.btnExportar.Text = "Imprimir";
            this.btnExportar.UseVisualStyleBackColor = false;
            // 
            // gpbFiltrosDoRelatorio
            // 
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnSincronizar);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblProduto);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.cbxprodutoSelecionado);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnCadastrar);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnMenu);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnSair);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.button3);
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
            // btnCadastrar
            // 
            this.btnCadastrar.Location = new System.Drawing.Point(948, 24);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(152, 43);
            this.btnCadastrar.TabIndex = 82;
            this.btnCadastrar.Text = "Cadastrar";
            this.btnCadastrar.UseVisualStyleBackColor = true;
            this.btnCadastrar.Click += new System.EventHandler(this.btnCadastrar_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(11)))), ((int)(((byte)(13)))));
            this.btnMenu.FlatAppearance.BorderSize = 2;
            this.btnMenu.ForeColor = System.Drawing.Color.White;
            this.btnMenu.Location = new System.Drawing.Point(1121, 23);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(152, 43);
            this.btnMenu.TabIndex = 10;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = false;
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
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1408, 81);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(142, 41);
            this.button4.TabIndex = 8;
            this.button4.Text = "&Aplicar Filtros";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
            this.panel4.Controls.Add(this.btnLimparFiltros);
            this.panel4.Controls.Add(this.btnAplicarFiltros);
            this.panel4.Location = new System.Drawing.Point(3, 147);
            this.panel4.Margin = new System.Windows.Forms.Padding(5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1309, 92);
            this.panel4.TabIndex = 13;
            // 
            // btnLimparFiltros
            // 
            this.btnLimparFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnLimparFiltros.ForeColor = System.Drawing.Color.White;
            this.btnLimparFiltros.Location = new System.Drawing.Point(443, 18);
            this.btnLimparFiltros.Name = "btnLimparFiltros";
            this.btnLimparFiltros.Size = new System.Drawing.Size(168, 54);
            this.btnLimparFiltros.TabIndex = 80;
            this.btnLimparFiltros.Text = "Limpar Filtros";
            this.btnLimparFiltros.UseVisualStyleBackColor = false;
            this.btnLimparFiltros.Click += new System.EventHandler(this.btnLimparFiltros_Click);
            // 
            // btnAplicarFiltros
            // 
            this.btnAplicarFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnAplicarFiltros.ForeColor = System.Drawing.Color.White;
            this.btnAplicarFiltros.Location = new System.Drawing.Point(182, 19);
            this.btnAplicarFiltros.Name = "btnAplicarFiltros";
            this.btnAplicarFiltros.Size = new System.Drawing.Size(170, 52);
            this.btnAplicarFiltros.TabIndex = 79;
            this.btnAplicarFiltros.Text = "Aplicar Filtros";
            this.btnAplicarFiltros.UseVisualStyleBackColor = false;
            this.btnAplicarFiltros.Click += new System.EventHandler(this.btnAplicarFiltros_Click);
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
            // cbxprodutoSelecionado
            // 
            this.cbxprodutoSelecionado.FormattingEnabled = true;
            this.cbxprodutoSelecionado.Location = new System.Drawing.Point(166, 27);
            this.cbxprodutoSelecionado.Name = "cbxprodutoSelecionado";
            this.cbxprodutoSelecionado.Size = new System.Drawing.Size(513, 29);
            this.cbxprodutoSelecionado.TabIndex = 83;
            // 
            // btnProdutosPrincipais
            // 
            this.btnProdutosPrincipais.Location = new System.Drawing.Point(697, 17);
            this.btnProdutosPrincipais.Name = "btnProdutosPrincipais";
            this.btnProdutosPrincipais.Size = new System.Drawing.Size(235, 37);
            this.btnProdutosPrincipais.TabIndex = 82;
            this.btnProdutosPrincipais.Text = "Produtos Principais";
            this.btnProdutosPrincipais.UseVisualStyleBackColor = true;
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblProduto.Location = new System.Drawing.Point(16, 31);
            this.lblProduto.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(146, 21);
            this.lblProduto.TabIndex = 84;
            this.lblProduto.Text = "Código ou Nome:";
            // 
            // btnSincronizar
            // 
            this.btnSincronizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSincronizar.Location = new System.Drawing.Point(776, 23);
            this.btnSincronizar.Name = "btnSincronizar";
            this.btnSincronizar.Size = new System.Drawing.Size(152, 43);
            this.btnSincronizar.TabIndex = 85;
            this.btnSincronizar.Text = "Sincronizar";
            this.btnSincronizar.UseVisualStyleBackColor = false;
            this.btnSincronizar.Click += new System.EventHandler(this.btnSincronizar_Click);
            // 
            // frmEstoque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
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
            this.pnlFiltrosDeBusca.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitleEstoque;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.GroupBox gpbFiltrosDoRelatorio;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnlFiltrosDeBusca;
        private System.Windows.Forms.Button btnAplicarFiltros;
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
        private System.Windows.Forms.Button btnCadastrar;
        private System.Windows.Forms.Button btnProdutosPrincipais;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.ComboBox cbxprodutoSelecionado;
        private System.Windows.Forms.Button btnSincronizar;
    }
}