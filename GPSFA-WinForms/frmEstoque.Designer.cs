namespace Projeto_Socorrista
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
            this.dgvEstoque = new System.Windows.Forms.DataGridView();
            this.dataLimiteSaida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.peso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.produto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.lblCodOrNome = new System.Windows.Forms.Label();
            this.txtNomeOrCod = new System.Windows.Forms.TextBox();
            this.btnCarregaTodosProdutos = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbxModoExibicao = new System.Windows.Forms.ComboBox();
            this.gpbFiltrosDoRelatorio = new System.Windows.Forms.GroupBox();
            this.cbxStatus = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblValidadeAte = new System.Windows.Forms.Label();
            this.dtpDataValidade = new System.Windows.Forms.DateTimePicker();
            this.btnAplicarFiltros = new System.Windows.Forms.Button();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cbxCategoria = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnAplicarModoExibicao = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.btnLimparFiltros = new System.Windows.Forms.Button();
            this.lblTitleEstoque = new System.Windows.Forms.Label();
            this.pnlFiltrosDeBusca = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gpbFiltrosDoRelatorio.SuspendLayout();
            this.pnlFiltrosDeBusca.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.dgvEstoque);
            this.panel2.Location = new System.Drawing.Point(0, 5);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1323, 1409);
            this.panel2.TabIndex = 20;
            // 
            // dgvEstoque
            // 
            this.dgvEstoque.AllowUserToAddRows = false;
            this.dgvEstoque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEstoque.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo,
            this.produto,
            this.quantidade,
            this.unidade,
            this.peso,
            this.status,
            this.dataLimiteSaida});
            this.dgvEstoque.Location = new System.Drawing.Point(3, 236);
            this.dgvEstoque.Margin = new System.Windows.Forms.Padding(5);
            this.dgvEstoque.Name = "dgvEstoque";
            this.dgvEstoque.ReadOnly = true;
            this.dgvEstoque.Size = new System.Drawing.Size(1309, 431);
            this.dgvEstoque.TabIndex = 5;
            this.dgvEstoque.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEstoque_CellContentClick);
            this.dgvEstoque.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvEstoque_Paint);
            // 
            // dataLimiteSaida
            // 
            this.dataLimiteSaida.HeaderText = "Limite de Saída ";
            this.dataLimiteSaida.Name = "dataLimiteSaida";
            this.dataLimiteSaida.ReadOnly = true;
            // 
            // status
            // 
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // peso
            // 
            this.peso.HeaderText = "Peso";
            this.peso.Name = "peso";
            this.peso.ReadOnly = true;
            // 
            // unidade
            // 
            this.unidade.HeaderText = "Unidade";
            this.unidade.Name = "unidade";
            this.unidade.ReadOnly = true;
            // 
            // quantidade
            // 
            this.quantidade.HeaderText = "Quantidade";
            this.quantidade.Name = "quantidade";
            this.quantidade.ReadOnly = true;
            // 
            // produto
            // 
            this.produto.HeaderText = "Produto";
            this.produto.Name = "produto";
            this.produto.ReadOnly = true;
            // 
            // codigo
            // 
            this.codigo.HeaderText = "Código";
            this.codigo.Name = "codigo";
            this.codigo.ReadOnly = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
            this.panel4.Controls.Add(this.btnCarregaTodosProdutos);
            this.panel4.Controls.Add(this.txtNomeOrCod);
            this.panel4.Controls.Add(this.lblCodOrNome);
            this.panel4.Controls.Add(this.btnPesquisar);
            this.panel4.Location = new System.Drawing.Point(3, 147);
            this.panel4.Margin = new System.Windows.Forms.Padding(5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1309, 92);
            this.panel4.TabIndex = 13;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnPesquisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnPesquisar.FlatAppearance.BorderSize = 0;
            this.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPesquisar.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.btnPesquisar.ForeColor = System.Drawing.Color.White;
            this.btnPesquisar.Location = new System.Drawing.Point(897, 27);
            this.btnPesquisar.Margin = new System.Windows.Forms.Padding(5);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(164, 33);
            this.btnPesquisar.TabIndex = 12;
            this.btnPesquisar.Text = "Pesquisar";
            this.btnPesquisar.UseVisualStyleBackColor = false;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // lblCodOrNome
            // 
            this.lblCodOrNome.AutoSize = true;
            this.lblCodOrNome.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblCodOrNome.Location = new System.Drawing.Point(24, 1);
            this.lblCodOrNome.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCodOrNome.Name = "lblCodOrNome";
            this.lblCodOrNome.Size = new System.Drawing.Size(146, 21);
            this.lblCodOrNome.TabIndex = 2;
            this.lblCodOrNome.Text = "Código ou Nome:";
            // 
            // txtNomeOrCod
            // 
            this.txtNomeOrCod.BackColor = System.Drawing.Color.White;
            this.txtNomeOrCod.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.txtNomeOrCod.Location = new System.Drawing.Point(28, 27);
            this.txtNomeOrCod.Margin = new System.Windows.Forms.Padding(5);
            this.txtNomeOrCod.Multiline = true;
            this.txtNomeOrCod.Name = "txtNomeOrCod";
            this.txtNomeOrCod.Size = new System.Drawing.Size(839, 40);
            this.txtNomeOrCod.TabIndex = 11;
            this.txtNomeOrCod.TextChanged += new System.EventHandler(this.txtNomeOrCod_TextChanged);
            this.txtNomeOrCod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNomeOrCod_KeyDown);
            // 
            // btnCarregaTodosProdutos
            // 
            this.btnCarregaTodosProdutos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnCarregaTodosProdutos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCarregaTodosProdutos.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCarregaTodosProdutos.FlatAppearance.BorderSize = 0;
            this.btnCarregaTodosProdutos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCarregaTodosProdutos.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.btnCarregaTodosProdutos.ForeColor = System.Drawing.Color.White;
            this.btnCarregaTodosProdutos.Location = new System.Drawing.Point(1097, 27);
            this.btnCarregaTodosProdutos.Margin = new System.Windows.Forms.Padding(5);
            this.btnCarregaTodosProdutos.Name = "btnCarregaTodosProdutos";
            this.btnCarregaTodosProdutos.Size = new System.Drawing.Size(164, 33);
            this.btnCarregaTodosProdutos.TabIndex = 13;
            this.btnCarregaTodosProdutos.Text = "Todos";
            this.btnCarregaTodosProdutos.UseVisualStyleBackColor = false;
            this.btnCarregaTodosProdutos.Click += new System.EventHandler(this.btnCarregaTodosProdutos_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.panel1.Controls.Add(this.lblTitleEstoque);
            this.panel1.Controls.Add(this.btnLimparFiltros);
            this.panel1.Controls.Add(this.btnVoltar);
            this.panel1.Controls.Add(this.btnExportarExcel);
            this.panel1.Controls.Add(this.btnAplicarModoExibicao);
            this.panel1.Controls.Add(this.gpbFiltrosDoRelatorio);
            this.panel1.Controls.Add(this.cbxModoExibicao);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.panel1.Size = new System.Drawing.Size(1323, 146);
            this.panel1.TabIndex = 18;
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
            this.cbxModoExibicao.Location = new System.Drawing.Point(220, 16);
            this.cbxModoExibicao.Margin = new System.Windows.Forms.Padding(5);
            this.cbxModoExibicao.Name = "cbxModoExibicao";
            this.cbxModoExibicao.Size = new System.Drawing.Size(186, 29);
            this.cbxModoExibicao.TabIndex = 1;
            // 
            // gpbFiltrosDoRelatorio
            // 
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnMenu);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnSair);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.button3);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.cbxCategoria);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblCategoria);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.btnAplicarFiltros);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.dtpDataValidade);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblValidadeAte);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.lblStatus);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.button4);
            this.gpbFiltrosDoRelatorio.Controls.Add(this.cbxStatus);
            this.gpbFiltrosDoRelatorio.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpbFiltrosDoRelatorio.Location = new System.Drawing.Point(3, 58);
            this.gpbFiltrosDoRelatorio.Name = "gpbFiltrosDoRelatorio";
            this.gpbFiltrosDoRelatorio.Padding = new System.Windows.Forms.Padding(0);
            this.gpbFiltrosDoRelatorio.Size = new System.Drawing.Size(1314, 75);
            this.gpbFiltrosDoRelatorio.TabIndex = 16;
            this.gpbFiltrosDoRelatorio.TabStop = false;
            this.gpbFiltrosDoRelatorio.Text = "Filtros";
            // 
            // cbxStatus
            // 
            this.cbxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStatus.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxStatus.FormattingEnabled = true;
            this.cbxStatus.Location = new System.Drawing.Point(95, 33);
            this.cbxStatus.Margin = new System.Windows.Forms.Padding(5);
            this.cbxStatus.Name = "cbxStatus";
            this.cbxStatus.Size = new System.Drawing.Size(186, 29);
            this.cbxStatus.TabIndex = 6;
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
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(21, 39);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(61, 21);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status:";
            // 
            // lblValidadeAte
            // 
            this.lblValidadeAte.AutoSize = true;
            this.lblValidadeAte.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblValidadeAte.ForeColor = System.Drawing.Color.White;
            this.lblValidadeAte.Location = new System.Drawing.Point(585, 33);
            this.lblValidadeAte.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblValidadeAte.Name = "lblValidadeAte";
            this.lblValidadeAte.Size = new System.Drawing.Size(109, 21);
            this.lblValidadeAte.TabIndex = 7;
            this.lblValidadeAte.Text = "Validade até:";
            // 
            // dtpDataValidade
            // 
            this.dtpDataValidade.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDataValidade.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDataValidade.Location = new System.Drawing.Point(695, 27);
            this.dtpDataValidade.Margin = new System.Windows.Forms.Padding(5);
            this.dtpDataValidade.Name = "dtpDataValidade";
            this.dtpDataValidade.Size = new System.Drawing.Size(186, 29);
            this.dtpDataValidade.TabIndex = 8;
            // 
            // btnAplicarFiltros
            // 
            this.btnAplicarFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnAplicarFiltros.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAplicarFiltros.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAplicarFiltros.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.btnAplicarFiltros.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAplicarFiltros.Location = new System.Drawing.Point(905, 23);
            this.btnAplicarFiltros.Margin = new System.Windows.Forms.Padding(5);
            this.btnAplicarFiltros.Name = "btnAplicarFiltros";
            this.btnAplicarFiltros.Size = new System.Drawing.Size(164, 33);
            this.btnAplicarFiltros.TabIndex = 9;
            this.btnAplicarFiltros.Text = "Aplicar filtros";
            this.btnAplicarFiltros.UseVisualStyleBackColor = false;
            this.btnAplicarFiltros.Click += new System.EventHandler(this.btnAplicarFiltros_Click);
            // 
            // lblCategoria
            // 
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblCategoria.ForeColor = System.Drawing.Color.White;
            this.lblCategoria.Location = new System.Drawing.Point(291, 36);
            this.lblCategoria.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(88, 21);
            this.lblCategoria.TabIndex = 5;
            this.lblCategoria.Text = "Categoria:";
            // 
            // cbxCategoria
            // 
            this.cbxCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCategoria.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCategoria.FormattingEnabled = true;
            this.cbxCategoria.Location = new System.Drawing.Point(389, 27);
            this.cbxCategoria.Margin = new System.Windows.Forms.Padding(5);
            this.cbxCategoria.Name = "cbxCategoria";
            this.cbxCategoria.Size = new System.Drawing.Size(186, 29);
            this.cbxCategoria.TabIndex = 7;
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
            // btnMenu
            // 
            this.btnMenu.FlatAppearance.BorderSize = 2;
            this.btnMenu.Location = new System.Drawing.Point(1077, 21);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(164, 33);
            this.btnMenu.TabIndex = 10;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnAplicarModoExibicao
            // 
            this.btnAplicarModoExibicao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnAplicarModoExibicao.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAplicarModoExibicao.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAplicarModoExibicao.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAplicarModoExibicao.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.btnAplicarModoExibicao.ForeColor = System.Drawing.Color.White;
            this.btnAplicarModoExibicao.Location = new System.Drawing.Point(428, 16);
            this.btnAplicarModoExibicao.Margin = new System.Windows.Forms.Padding(5);
            this.btnAplicarModoExibicao.Name = "btnAplicarModoExibicao";
            this.btnAplicarModoExibicao.Size = new System.Drawing.Size(164, 33);
            this.btnAplicarModoExibicao.TabIndex = 2;
            this.btnAplicarModoExibicao.Text = "Aplicar modo exibição";
            this.btnAplicarModoExibicao.UseVisualStyleBackColor = false;
            this.btnAplicarModoExibicao.Click += new System.EventHandler(this.btnAplicarModoExibicao_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.Location = new System.Drawing.Point(1146, 12);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(164, 33);
            this.btnExportarExcel.TabIndex = 5;
            this.btnExportarExcel.Text = "Imprimir";
            this.btnExportarExcel.UseVisualStyleBackColor = true;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
            this.btnVoltar.Location = new System.Drawing.Point(976, 13);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(164, 33);
            this.btnVoltar.TabIndex = 4;
            this.btnVoltar.Text = "&Voltar";
            this.btnVoltar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVoltar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // btnLimparFiltros
            // 
            this.btnLimparFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnLimparFiltros.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimparFiltros.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnLimparFiltros.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLimparFiltros.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.btnLimparFiltros.ForeColor = System.Drawing.Color.White;
            this.btnLimparFiltros.Location = new System.Drawing.Point(804, 15);
            this.btnLimparFiltros.Margin = new System.Windows.Forms.Padding(5);
            this.btnLimparFiltros.Name = "btnLimparFiltros";
            this.btnLimparFiltros.Size = new System.Drawing.Size(164, 33);
            this.btnLimparFiltros.TabIndex = 3;
            this.btnLimparFiltros.Text = "Limpar Filtros";
            this.btnLimparFiltros.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLimparFiltros.UseVisualStyleBackColor = false;
            this.btnLimparFiltros.Click += new System.EventHandler(this.btnLimparFiltros_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gpbFiltrosDoRelatorio.ResumeLayout(false);
            this.gpbFiltrosDoRelatorio.PerformLayout();
            this.pnlFiltrosDeBusca.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitleEstoque;
        private System.Windows.Forms.Button btnLimparFiltros;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnAplicarModoExibicao;
        private System.Windows.Forms.GroupBox gpbFiltrosDoRelatorio;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox cbxCategoria;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.Button btnAplicarFiltros;
        private System.Windows.Forms.DateTimePicker dtpDataValidade;
        private System.Windows.Forms.Label lblValidadeAte;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cbxStatus;
        private System.Windows.Forms.ComboBox cbxModoExibicao;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnCarregaTodosProdutos;
        private System.Windows.Forms.TextBox txtNomeOrCod;
        private System.Windows.Forms.Label lblCodOrNome;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.DataGridView dgvEstoque;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn produto;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn unidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn peso;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataLimiteSaida;
        private System.Windows.Forms.Panel pnlFiltrosDeBusca;
    }
}