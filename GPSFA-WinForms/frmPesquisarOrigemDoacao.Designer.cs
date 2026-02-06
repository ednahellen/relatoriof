namespace GPSFA_WinForms
{
    partial class frmPesquisarOrigemDoacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPesquisarOrigemDoacao));
            this.gpbOrigemDoacoes = new System.Windows.Forms.GroupBox();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.ltbPesquisarOrigem = new System.Windows.Forms.ListBox();
            this.lblResultado = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.btnPesquisarOrigem = new System.Windows.Forms.Button();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.gpbOrigemDoacoes.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbOrigemDoacoes
            // 
            this.gpbOrigemDoacoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.gpbOrigemDoacoes.Controls.Add(this.btnLimpar);
            this.gpbOrigemDoacoes.Controls.Add(this.btnVoltar);
            this.gpbOrigemDoacoes.Controls.Add(this.ltbPesquisarOrigem);
            this.gpbOrigemDoacoes.Controls.Add(this.lblResultado);
            this.gpbOrigemDoacoes.Controls.Add(this.txtDescricao);
            this.gpbOrigemDoacoes.Controls.Add(this.btnPesquisarOrigem);
            this.gpbOrigemDoacoes.Controls.Add(this.lblDescricao);
            this.gpbOrigemDoacoes.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpbOrigemDoacoes.Location = new System.Drawing.Point(12, 12);
            this.gpbOrigemDoacoes.Name = "gpbOrigemDoacoes";
            this.gpbOrigemDoacoes.Size = new System.Drawing.Size(737, 285);
            this.gpbOrigemDoacoes.TabIndex = 15;
            this.gpbOrigemDoacoes.TabStop = false;
            // 
            // btnLimpar
            // 
            this.btnLimpar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.Image = ((System.Drawing.Image)(resources.GetObject("btnLimpar.Image")));
            this.btnLimpar.Location = new System.Drawing.Point(596, 132);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(130, 40);
            this.btnLimpar.TabIndex = 5;
            this.btnLimpar.Text = "&Limpar";
            this.btnLimpar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLimpar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Image = ((System.Drawing.Image)(resources.GetObject("btnVoltar.Image")));
            this.btnVoltar.Location = new System.Drawing.Point(596, 220);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(130, 40);
            this.btnVoltar.TabIndex = 8;
            this.btnVoltar.Text = "&Voltar";
            this.btnVoltar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVoltar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // ltbPesquisarOrigem
            // 
            this.ltbPesquisarOrigem.Enabled = false;
            this.ltbPesquisarOrigem.Font = new System.Drawing.Font("Microsoft YaHei", 18F);
            this.ltbPesquisarOrigem.FormattingEnabled = true;
            this.ltbPesquisarOrigem.ItemHeight = 31;
            this.ltbPesquisarOrigem.Location = new System.Drawing.Point(16, 132);
            this.ltbPesquisarOrigem.Name = "ltbPesquisarOrigem";
            this.ltbPesquisarOrigem.Size = new System.Drawing.Size(564, 128);
            this.ltbPesquisarOrigem.TabIndex = 22;
            this.ltbPesquisarOrigem.SelectedIndexChanged += new System.EventHandler(this.ltbPesquisarOrigem_SelectedIndexChanged);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultado.ForeColor = System.Drawing.Color.Black;
            this.lblResultado.Location = new System.Drawing.Point(16, 100);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(131, 29);
            this.lblResultado.TabIndex = 21;
            this.lblResultado.Text = "Resultado";
            // 
            // txtDescricao
            // 
            this.txtDescricao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescricao.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescricao.Location = new System.Drawing.Point(16, 50);
            this.txtDescricao.MaxLength = 20;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(564, 39);
            this.txtDescricao.TabIndex = 1;
            this.txtDescricao.TextChanged += new System.EventHandler(this.txtDescricao_TextChanged);
            // 
            // btnPesquisarOrigem
            // 
            this.btnPesquisarOrigem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisarOrigem.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisarOrigem.Image = ((System.Drawing.Image)(resources.GetObject("btnPesquisarOrigem.Image")));
            this.btnPesquisarOrigem.Location = new System.Drawing.Point(596, 51);
            this.btnPesquisarOrigem.Name = "btnPesquisarOrigem";
            this.btnPesquisarOrigem.Size = new System.Drawing.Size(130, 40);
            this.btnPesquisarOrigem.TabIndex = 2;
            this.btnPesquisarOrigem.Text = "&Pesquisar";
            this.btnPesquisarOrigem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPesquisarOrigem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPesquisarOrigem.UseVisualStyleBackColor = true;
            this.btnPesquisarOrigem.Click += new System.EventHandler(this.btnPesquisarOrigem_Click);
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricao.ForeColor = System.Drawing.Color.Black;
            this.lblDescricao.Location = new System.Drawing.Point(16, 20);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(130, 29);
            this.lblDescricao.TabIndex = 3;
            this.lblDescricao.Text = "Descrição";
            // 
            // frmPesquisarOrigemDoacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 304);
            this.Controls.Add(this.gpbOrigemDoacoes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmPesquisarOrigemDoacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Pesquisar Origem Doação";
            this.Load += new System.EventHandler(this.frmPesquisarOrigemDoacao_Load);
            this.gpbOrigemDoacoes.ResumeLayout(false);
            this.gpbOrigemDoacoes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbOrigemDoacoes;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.ListBox ltbPesquisarOrigem;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Button btnPesquisarOrigem;
        private System.Windows.Forms.Label lblDescricao;
    }
}