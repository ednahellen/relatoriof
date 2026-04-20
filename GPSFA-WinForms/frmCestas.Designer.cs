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
            this.btnDistribuir = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
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
            this.gpbGerirCestas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensDaCesta)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbGerirCestas
            // 
            this.gpbGerirCestas.Controls.Add(this.btnDistribuir);
            this.gpbGerirCestas.Controls.Add(this.btnExportar);
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
            this.gpbGerirCestas.Size = new System.Drawing.Size(1135, 556);
            this.gpbGerirCestas.TabIndex = 0;
            this.gpbGerirCestas.TabStop = false;
            // 
            // btnDistribuir
            // 
            this.btnDistribuir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnDistribuir.Location = new System.Drawing.Point(786, 47);
            this.btnDistribuir.Name = "btnDistribuir";
            this.btnDistribuir.Size = new System.Drawing.Size(152, 43);
            this.btnDistribuir.TabIndex = 83;
            this.btnDistribuir.Text = "Distribuir";
            this.btnDistribuir.UseVisualStyleBackColor = false;
            // 
            // btnExportar
            // 
            this.btnExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnExportar.Location = new System.Drawing.Point(949, 44);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(152, 43);
            this.btnExportar.TabIndex = 82;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.UseVisualStyleBackColor = false;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
            this.btnVoltar.Location = new System.Drawing.Point(925, 472);
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
            this.dgvItensDaCesta.Location = new System.Drawing.Point(18, 115);
            this.dgvItensDaCesta.Name = "dgvItensDaCesta";
            this.dgvItensDaCesta.RowHeadersWidth = 51;
            this.dgvItensDaCesta.Size = new System.Drawing.Size(1111, 337);
            this.dgvItensDaCesta.TabIndex = 3;
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
            // frmCestas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(1158, 580);
            this.Controls.Add(this.gpbGerirCestas);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "frmCestas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Gerenciar Cestas";
            this.Load += new System.EventHandler(this.frmCestas_Load);
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
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnDistribuir;
    }
}