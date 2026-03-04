namespace GPSFA_WinForms
{
    partial class frmAdicionarItemNaCesta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdicionarItemNaCesta));
            this.gpbAdicionarItemNaCesta = new System.Windows.Forms.GroupBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtQuantidade = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdicionarItem = new System.Windows.Forms.Button();
            this.lblNome = new System.Windows.Forms.Label();
            this.cbbProdutos = new System.Windows.Forms.ComboBox();
            this.gpbAdicionarItemNaCesta.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbAdicionarItemNaCesta
            // 
            this.gpbAdicionarItemNaCesta.Controls.Add(this.btnCancelar);
            this.gpbAdicionarItemNaCesta.Controls.Add(this.txtQuantidade);
            this.gpbAdicionarItemNaCesta.Controls.Add(this.label1);
            this.gpbAdicionarItemNaCesta.Controls.Add(this.btnAdicionarItem);
            this.gpbAdicionarItemNaCesta.Controls.Add(this.lblNome);
            this.gpbAdicionarItemNaCesta.Controls.Add(this.cbbProdutos);
            this.gpbAdicionarItemNaCesta.Location = new System.Drawing.Point(12, 12);
            this.gpbAdicionarItemNaCesta.Name = "gpbAdicionarItemNaCesta";
            this.gpbAdicionarItemNaCesta.Size = new System.Drawing.Size(481, 317);
            this.gpbAdicionarItemNaCesta.TabIndex = 0;
            this.gpbAdicionarItemNaCesta.TabStop = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelar.Image")));
            this.btnCancelar.Location = new System.Drawing.Point(253, 233);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(190, 47);
            this.btnCancelar.TabIndex = 84;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtQuantidade
            // 
            this.txtQuantidade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantidade.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantidade.Location = new System.Drawing.Point(32, 168);
            this.txtQuantidade.MaxLength = 100;
            this.txtQuantidade.Name = "txtQuantidade";
            this.txtQuantidade.Size = new System.Drawing.Size(119, 39);
            this.txtQuantidade.TabIndex = 83;
            this.txtQuantidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQtdCestas_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(27, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 29);
            this.label1.TabIndex = 82;
            this.label1.Text = "Quantidade por cesta";
            // 
            // btnAdicionarItem
            // 
            this.btnAdicionarItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdicionarItem.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAdicionarItem.Image")));
            this.btnAdicionarItem.Location = new System.Drawing.Point(32, 233);
            this.btnAdicionarItem.Name = "btnAdicionarItem";
            this.btnAdicionarItem.Size = new System.Drawing.Size(190, 47);
            this.btnAdicionarItem.TabIndex = 81;
            this.btnAdicionarItem.Text = "&Adicionar a cesta";
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
            this.lblNome.Location = new System.Drawing.Point(27, 44);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(270, 29);
            this.lblNome.TabIndex = 80;
            this.lblNome.Text = "Selecione um produto";
            // 
            // cbbProdutos
            // 
            this.cbbProdutos.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbProdutos.FormattingEnabled = true;
            this.cbbProdutos.Location = new System.Drawing.Point(32, 76);
            this.cbbProdutos.Name = "cbbProdutos";
            this.cbbProdutos.Size = new System.Drawing.Size(411, 39);
            this.cbbProdutos.TabIndex = 78;
            // 
            // frmAdicionarItemNaCesta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(504, 341);
            this.Controls.Add(this.gpbAdicionarItemNaCesta);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdicionarItemNaCesta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Adicionar Item na Cesta";
            this.gpbAdicionarItemNaCesta.ResumeLayout(false);
            this.gpbAdicionarItemNaCesta.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbAdicionarItemNaCesta;
        private System.Windows.Forms.Button btnAdicionarItem;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.ComboBox cbbProdutos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtQuantidade;
    }
}