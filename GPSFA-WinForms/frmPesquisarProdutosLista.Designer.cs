namespace GPSFA_WinForms
{
    partial class frmPesquisarProdutosLista
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPesquisarProdutosLista));
            this.gpbListaVoluntarios = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.dgvListaDeProdutos = new System.Windows.Forms.DataGridView();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.gpbListaVoluntarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDeProdutos)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbListaVoluntarios
            // 
            this.gpbListaVoluntarios.Controls.Add(this.btnVoltar);
            this.gpbListaVoluntarios.Controls.Add(this.btnLimpar);
            this.gpbListaVoluntarios.Controls.Add(this.btnPesquisar);
            this.gpbListaVoluntarios.Controls.Add(this.label1);
            this.gpbListaVoluntarios.Controls.Add(this.txtDescricao);
            this.gpbListaVoluntarios.Controls.Add(this.lblDescricao);
            this.gpbListaVoluntarios.Controls.Add(this.dgvListaDeProdutos);
            this.gpbListaVoluntarios.Location = new System.Drawing.Point(11, 11);
            this.gpbListaVoluntarios.Margin = new System.Windows.Forms.Padding(2);
            this.gpbListaVoluntarios.Name = "gpbListaVoluntarios";
            this.gpbListaVoluntarios.Padding = new System.Windows.Forms.Padding(2);
            this.gpbListaVoluntarios.Size = new System.Drawing.Size(823, 452);
            this.gpbListaVoluntarios.TabIndex = 1;
            this.gpbListaVoluntarios.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(10, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 29);
            this.label1.TabIndex = 9;
            this.label1.Text = "Lista de produtos";
            // 
            // txtDescricao
            // 
            this.txtDescricao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescricao.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescricao.Location = new System.Drawing.Point(15, 53);
            this.txtDescricao.MaxLength = 20;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(392, 39);
            this.txtDescricao.TabIndex = 7;
            this.txtDescricao.TextChanged += new System.EventHandler(this.txtDescricao_TextChanged);
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricao.ForeColor = System.Drawing.Color.Black;
            this.lblDescricao.Location = new System.Drawing.Point(10, 21);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(130, 29);
            this.lblDescricao.TabIndex = 8;
            this.lblDescricao.Text = "Descrição";
            // 
            // dgvListaDeProdutos
            // 
            this.dgvListaDeProdutos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaDeProdutos.Location = new System.Drawing.Point(4, 161);
            this.dgvListaDeProdutos.Margin = new System.Windows.Forms.Padding(2);
            this.dgvListaDeProdutos.Name = "dgvListaDeProdutos";
            this.dgvListaDeProdutos.RowHeadersWidth = 51;
            this.dgvListaDeProdutos.RowTemplate.Height = 24;
            this.dgvListaDeProdutos.Size = new System.Drawing.Size(815, 287);
            this.dgvListaDeProdutos.TabIndex = 6;
            this.dgvListaDeProdutos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListaDeProdutos_CellContentClick);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
            this.btnVoltar.Location = new System.Drawing.Point(685, 52);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(133, 40);
            this.btnVoltar.TabIndex = 23;
            this.btnVoltar.Text = "&Voltar";
            this.btnVoltar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVoltar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.Image = ((System.Drawing.Image)(resources.GetObject("btnLimpar.Image")));
            this.btnLimpar.Location = new System.Drawing.Point(549, 52);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(130, 40);
            this.btnLimpar.TabIndex = 24;
            this.btnLimpar.Text = "&Limpar";
            this.btnLimpar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLimpar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLimpar.UseVisualStyleBackColor = true;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisar.Image = ((System.Drawing.Image)(resources.GetObject("btnPesquisar.Image")));
            this.btnPesquisar.Location = new System.Drawing.Point(413, 52);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(130, 40);
            this.btnPesquisar.TabIndex = 23;
            this.btnPesquisar.Text = "&Pesquisar";
            this.btnPesquisar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPesquisar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPesquisar.UseVisualStyleBackColor = true;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // frmPesquisarProdutosLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(843, 475);
            this.Controls.Add(this.gpbListaVoluntarios);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPesquisarProdutosLista";
            this.Text = "Grupo Socorrista São Francisco de Assis - Pesquisar Produtos da Lista";
            this.Load += new System.EventHandler(this.frmPesquisarProdutosLista_Load);
            this.gpbListaVoluntarios.ResumeLayout(false);
            this.gpbListaVoluntarios.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaDeProdutos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbListaVoluntarios;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.DataGridView dgvListaDeProdutos;
    }
}