namespace GPSFA_WinForms
{
    partial class frmModelosDeCestas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModelosDeCestas));
            this.gpbGerirCestas = new System.Windows.Forms.GroupBox();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.dgvItensDaCesta = new System.Windows.Forms.DataGridView();
            this.CodList = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Produto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtdePorCesta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnMontar = new System.Windows.Forms.Button();
            this.btnAdicionarItem = new System.Windows.Forms.Button();
            this.lblNome = new System.Windows.Forms.Label();
            this.gpbGerirCestas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensDaCesta)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbGerirCestas
            // 
            this.gpbGerirCestas.Controls.Add(this.btnVoltar);
            this.gpbGerirCestas.Controls.Add(this.dgvItensDaCesta);
            this.gpbGerirCestas.Controls.Add(this.btnMontar);
            this.gpbGerirCestas.Controls.Add(this.btnAdicionarItem);
            this.gpbGerirCestas.Controls.Add(this.lblNome);
            this.gpbGerirCestas.Location = new System.Drawing.Point(12, 12);
            this.gpbGerirCestas.Name = "gpbGerirCestas";
            this.gpbGerirCestas.Size = new System.Drawing.Size(751, 467);
            this.gpbGerirCestas.TabIndex = 2;
            this.gpbGerirCestas.TabStop = false;
            // 
            // btnVoltar
            // 
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
            this.btnVoltar.Location = new System.Drawing.Point(633, 19);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(112, 40);
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
            this.QtdePorCesta});
            this.dgvItensDaCesta.Location = new System.Drawing.Point(13, 73);
            this.dgvItensDaCesta.Name = "dgvItensDaCesta";
            this.dgvItensDaCesta.Size = new System.Drawing.Size(598, 300);
            this.dgvItensDaCesta.TabIndex = 3;
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
            // btnMontar
            // 
            this.btnMontar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMontar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMontar.Image = ((System.Drawing.Image)(resources.GetObject("btnMontar.Image")));
            this.btnMontar.Location = new System.Drawing.Point(209, 389);
            this.btnMontar.Name = "btnMontar";
            this.btnMontar.Size = new System.Drawing.Size(190, 63);
            this.btnMontar.TabIndex = 5;
            this.btnMontar.Text = "&Salvar modelo";
            this.btnMontar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMontar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMontar.UseVisualStyleBackColor = true;
            this.btnMontar.Click += new System.EventHandler(this.btnMontar_Click);
            // 
            // btnAdicionarItem
            // 
            this.btnAdicionarItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdicionarItem.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAdicionarItem.Image")));
            this.btnAdicionarItem.Location = new System.Drawing.Point(13, 389);
            this.btnAdicionarItem.Name = "btnAdicionarItem";
            this.btnAdicionarItem.Size = new System.Drawing.Size(190, 63);
            this.btnAdicionarItem.TabIndex = 4;
            this.btnAdicionarItem.Text = "&Adicionar item";
            this.btnAdicionarItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdicionarItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdicionarItem.UseVisualStyleBackColor = true;
            this.btnAdicionarItem.Click += new System.EventHandler(this.btnAdicionarItem_Click);
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNome.ForeColor = System.Drawing.Color.Black;
            this.lblNome.Location = new System.Drawing.Point(8, 30);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(283, 29);
            this.lblNome.TabIndex = 65;
            this.lblNome.Text = "Editar modelo de cesta";
            // 
            // frmModelosDeCestas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(792, 492);
            this.Controls.Add(this.gpbGerirCestas);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "frmModelosDeCestas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Modelo de cestas";
            this.gpbGerirCestas.ResumeLayout(false);
            this.gpbGerirCestas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensDaCesta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbGerirCestas;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.DataGridView dgvItensDaCesta;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Produto;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtdePorCesta;
        private System.Windows.Forms.Button btnMontar;
        private System.Windows.Forms.Button btnAdicionarItem;
        private System.Windows.Forms.Label lblNome;
    }
}