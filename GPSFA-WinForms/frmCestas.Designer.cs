namespace GPSFA_WinForms
{
    partial class frmCestas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCestas));
            this.gpbGerirCestas = new System.Windows.Forms.GroupBox();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.dgvItensDaCesta = new System.Windows.Forms.DataGridView();
            this.btnMontar = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnAdicionarItem = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQtdCestas = new System.Windows.Forms.TextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.btnModeloDeCesta = new System.Windows.Forms.Button();
            this.cbbModeloDeCesta = new System.Windows.Forms.ComboBox();
            this.CodList = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Produto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtdePorCesta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstoqueAtual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalNecessario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantoFalta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gpbGerirCestas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensDaCesta)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbGerirCestas
            // 
            this.gpbGerirCestas.Controls.Add(this.btnVoltar);
            this.gpbGerirCestas.Controls.Add(this.dgvItensDaCesta);
            this.gpbGerirCestas.Controls.Add(this.btnMontar);
            this.gpbGerirCestas.Controls.Add(this.btnLimpar);
            this.gpbGerirCestas.Controls.Add(this.btnAdicionarItem);
            this.gpbGerirCestas.Controls.Add(this.label1);
            this.gpbGerirCestas.Controls.Add(this.txtQtdCestas);
            this.gpbGerirCestas.Controls.Add(this.lblNome);
            this.gpbGerirCestas.Controls.Add(this.btnModeloDeCesta);
            this.gpbGerirCestas.Controls.Add(this.cbbModeloDeCesta);
            this.gpbGerirCestas.Location = new System.Drawing.Point(12, 12);
            this.gpbGerirCestas.Name = "gpbGerirCestas";
            this.gpbGerirCestas.Size = new System.Drawing.Size(1284, 556);
            this.gpbGerirCestas.TabIndex = 0;
            this.gpbGerirCestas.TabStop = false;
            // 
            // btnVoltar
            // 
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
            this.btnVoltar.Location = new System.Drawing.Point(1077, 472);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(190, 63);
            this.btnVoltar.TabIndex = 7;
            this.btnVoltar.Text = "&Voltar";
            this.btnVoltar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVoltar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // dgvItensDaCesta
            // 
            this.dgvItensDaCesta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItensDaCesta.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CodList,
            this.Produto,
            this.QtdePorCesta,
            this.EstoqueAtual,
            this.TotalNecessario,
            this.Status,
            this.QuantoFalta});
            this.dgvItensDaCesta.Location = new System.Drawing.Point(18, 118);
            this.dgvItensDaCesta.Name = "dgvItensDaCesta";
            this.dgvItensDaCesta.Size = new System.Drawing.Size(1249, 332);
            this.dgvItensDaCesta.TabIndex = 3;
            this.dgvItensDaCesta.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItensDaCesta_CellContentClick);
            this.dgvItensDaCesta.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItensDaCesta_CellEndEdit);
            this.dgvItensDaCesta.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvItensDaCesta_EditingControlShowing);
            // 
            // btnMontar
            // 
            this.btnMontar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMontar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMontar.Image = ((System.Drawing.Image)(resources.GetObject("btnMontar.Image")));
            this.btnMontar.Location = new System.Drawing.Point(213, 472);
            this.btnMontar.Name = "btnMontar";
            this.btnMontar.Size = new System.Drawing.Size(190, 63);
            this.btnMontar.TabIndex = 5;
            this.btnMontar.Text = "&Montar cestas";
            this.btnMontar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMontar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMontar.UseVisualStyleBackColor = true;
            this.btnMontar.Click += new System.EventHandler(this.btnMontar_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.Image = ((System.Drawing.Image)(resources.GetObject("btnLimpar.Image")));
            this.btnLimpar.Location = new System.Drawing.Point(409, 472);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(190, 63);
            this.btnLimpar.TabIndex = 6;
            this.btnLimpar.Text = "&Limpar";
            this.btnLimpar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLimpar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnAdicionarItem
            // 
            this.btnAdicionarItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdicionarItem.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAdicionarItem.Image")));
            this.btnAdicionarItem.Location = new System.Drawing.Point(18, 472);
            this.btnAdicionarItem.Name = "btnAdicionarItem";
            this.btnAdicionarItem.Size = new System.Drawing.Size(190, 63);
            this.btnAdicionarItem.TabIndex = 4;
            this.btnAdicionarItem.Text = "&Adicionar item";
            this.btnAdicionarItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdicionarItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdicionarItem.UseVisualStyleBackColor = true;
            this.btnAdicionarItem.Click += new System.EventHandler(this.btnAdicionarItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(491, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 29);
            this.label1.TabIndex = 67;
            this.label1.Text = "Quantidade de cestas";
            // 
            // txtQtdCestas
            // 
            this.txtQtdCestas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQtdCestas.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQtdCestas.Location = new System.Drawing.Point(496, 63);
            this.txtQtdCestas.MaxLength = 5;
            this.txtQtdCestas.Name = "txtQtdCestas";
            this.txtQtdCestas.Size = new System.Drawing.Size(103, 39);
            this.txtQtdCestas.TabIndex = 2;
            this.txtQtdCestas.TextChanged += new System.EventHandler(this.txtQtdCestas_TextChanged);
            this.txtQtdCestas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQtdCestas_KeyPress);
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNome.ForeColor = System.Drawing.Color.Black;
            this.lblNome.Location = new System.Drawing.Point(13, 30);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(268, 29);
            this.lblNome.TabIndex = 65;
            this.lblNome.Text = "Usar modelo de cesta";
            // 
            // btnModeloDeCesta
            // 
            this.btnModeloDeCesta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModeloDeCesta.FlatAppearance.BorderSize = 0;
            this.btnModeloDeCesta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeloDeCesta.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModeloDeCesta.Image = ((System.Drawing.Image)(resources.GetObject("btnModeloDeCesta.Image")));
            this.btnModeloDeCesta.Location = new System.Drawing.Point(409, 62);
            this.btnModeloDeCesta.Name = "btnModeloDeCesta";
            this.btnModeloDeCesta.Size = new System.Drawing.Size(48, 39);
            this.btnModeloDeCesta.TabIndex = 1;
            this.btnModeloDeCesta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnModeloDeCesta.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModeloDeCesta.UseVisualStyleBackColor = true;
            this.btnModeloDeCesta.Click += new System.EventHandler(this.btnModeloDeCesta_Click);
            // 
            // cbbModeloDeCesta
            // 
            this.cbbModeloDeCesta.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbModeloDeCesta.FormattingEnabled = true;
            this.cbbModeloDeCesta.Location = new System.Drawing.Point(18, 62);
            this.cbbModeloDeCesta.Name = "cbbModeloDeCesta";
            this.cbbModeloDeCesta.Size = new System.Drawing.Size(385, 39);
            this.cbbModeloDeCesta.TabIndex = 0;
            this.cbbModeloDeCesta.SelectedIndexChanged += new System.EventHandler(this.cbbModeloDeCesta_SelectedIndexChanged);
            // 
            // CodList
            // 
            this.CodList.HeaderText = "Codigo";
            this.CodList.Name = "CodList";
            this.CodList.ReadOnly = true;
            // 
            // Produto
            // 
            this.Produto.HeaderText = "Produto";
            this.Produto.MaxInputLength = 100;
            this.Produto.Name = "Produto";
            this.Produto.ReadOnly = true;
            this.Produto.Width = 115;
            // 
            // QtdePorCesta
            // 
            this.QtdePorCesta.HeaderText = "Qtde por cesta";
            this.QtdePorCesta.MaxInputLength = 10;
            this.QtdePorCesta.Name = "QtdePorCesta";
            this.QtdePorCesta.Width = 178;
            // 
            // EstoqueAtual
            // 
            this.EstoqueAtual.HeaderText = "Estoque atual";
            this.EstoqueAtual.MaxInputLength = 10;
            this.EstoqueAtual.Name = "EstoqueAtual";
            this.EstoqueAtual.ReadOnly = true;
            this.EstoqueAtual.Width = 153;
            // 
            // TotalNecessario
            // 
            this.TotalNecessario.HeaderText = "Total necessário";
            this.TotalNecessario.MaxInputLength = 10;
            this.TotalNecessario.Name = "TotalNecessario";
            this.TotalNecessario.ReadOnly = true;
            this.TotalNecessario.Width = 176;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MaxInputLength = 20;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 95;
            // 
            // QuantoFalta
            // 
            this.QuantoFalta.HeaderText = "Quanto falta";
            this.QuantoFalta.MaxInputLength = 10;
            this.QuantoFalta.Name = "QuantoFalta";
            this.QuantoFalta.ReadOnly = true;
            // 
            // frmCestas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(1308, 580);
            this.Controls.Add(this.gpbGerirCestas);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "frmCestas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Gerenciar Cestas";
            this.gpbGerirCestas.ResumeLayout(false);
            this.gpbGerirCestas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensDaCesta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbGerirCestas;
        private System.Windows.Forms.ComboBox cbbModeloDeCesta;
        private System.Windows.Forms.Button btnModeloDeCesta;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQtdCestas;
        private System.Windows.Forms.DataGridView dgvItensDaCesta;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnAdicionarItem;
        private System.Windows.Forms.Button btnMontar;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Produto;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtdePorCesta;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstoqueAtual;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalNecessario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantoFalta;
    }
}