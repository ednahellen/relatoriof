namespace GPSFA_WinForms
{
    partial class frmPesquisarVoluntario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPesquisarVoluntario));
            this.gpbCamposMedidas = new System.Windows.Forms.GroupBox();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.ltbPesquisarVoluntarios = new System.Windows.Forms.ListBox();
            this.lblResultado = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.btnPesquisarVoluntario = new System.Windows.Forms.Button();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.gpbCamposMedidas.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbCamposMedidas
            // 
            this.gpbCamposMedidas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.gpbCamposMedidas.Controls.Add(this.btnLimpar);
            this.gpbCamposMedidas.Controls.Add(this.btnVoltar);
            this.gpbCamposMedidas.Controls.Add(this.ltbPesquisarVoluntarios);
            this.gpbCamposMedidas.Controls.Add(this.lblResultado);
            this.gpbCamposMedidas.Controls.Add(this.txtDescricao);
            this.gpbCamposMedidas.Controls.Add(this.btnPesquisarVoluntario);
            this.gpbCamposMedidas.Controls.Add(this.lblDescricao);
            this.gpbCamposMedidas.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpbCamposMedidas.Location = new System.Drawing.Point(12, 12);
            this.gpbCamposMedidas.Name = "gpbCamposMedidas";
            this.gpbCamposMedidas.Size = new System.Drawing.Size(839, 310);
            this.gpbCamposMedidas.TabIndex = 15;
            this.gpbCamposMedidas.TabStop = false;
            // 
            // btnLimpar
            // 
            this.btnLimpar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.Image = ((System.Drawing.Image)(resources.GetObject("btnLimpar.Image")));
            this.btnLimpar.Location = new System.Drawing.Point(674, 143);
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
            this.btnVoltar.Location = new System.Drawing.Point(674, 231);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(130, 40);
            this.btnVoltar.TabIndex = 8;
            this.btnVoltar.Text = "&Voltar";
            this.btnVoltar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVoltar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // ltbPesquisarVoluntarios
            // 
            this.ltbPesquisarVoluntarios.Enabled = false;
            this.ltbPesquisarVoluntarios.Font = new System.Drawing.Font("Microsoft YaHei", 18F);
            this.ltbPesquisarVoluntarios.FormattingEnabled = true;
            this.ltbPesquisarVoluntarios.ItemHeight = 31;
            this.ltbPesquisarVoluntarios.Location = new System.Drawing.Point(16, 143);
            this.ltbPesquisarVoluntarios.Name = "ltbPesquisarVoluntarios";
            this.ltbPesquisarVoluntarios.Size = new System.Drawing.Size(641, 159);
            this.ltbPesquisarVoluntarios.TabIndex = 22;
            this.ltbPesquisarVoluntarios.SelectedIndexChanged += new System.EventHandler(this.ltbPesquisarVoluntarios_SelectedIndexChanged);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultado.ForeColor = System.Drawing.Color.Black;
            this.lblResultado.Location = new System.Drawing.Point(11, 111);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(131, 29);
            this.lblResultado.TabIndex = 21;
            this.lblResultado.Text = "Resultado";
            // 
            // txtDescricao
            // 
            this.txtDescricao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescricao.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescricao.Location = new System.Drawing.Point(16, 61);
            this.txtDescricao.MaxLength = 20;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(641, 39);
            this.txtDescricao.TabIndex = 1;
            this.txtDescricao.TextChanged += new System.EventHandler(this.txtDescricao_TextChanged);
            // 
            // btnPesquisarVoluntario
            // 
            this.btnPesquisarVoluntario.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisarVoluntario.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisarVoluntario.Image = ((System.Drawing.Image)(resources.GetObject("btnPesquisarVoluntario.Image")));
            this.btnPesquisarVoluntario.Location = new System.Drawing.Point(674, 60);
            this.btnPesquisarVoluntario.Name = "btnPesquisarVoluntario";
            this.btnPesquisarVoluntario.Size = new System.Drawing.Size(130, 40);
            this.btnPesquisarVoluntario.TabIndex = 2;
            this.btnPesquisarVoluntario.Text = "&Pesquisar";
            this.btnPesquisarVoluntario.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPesquisarVoluntario.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPesquisarVoluntario.UseVisualStyleBackColor = true;
            this.btnPesquisarVoluntario.Click += new System.EventHandler(this.btnPesquisarVoluntario_Click);
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricao.ForeColor = System.Drawing.Color.Black;
            this.lblDescricao.Location = new System.Drawing.Point(11, 29);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(130, 29);
            this.lblDescricao.TabIndex = 3;
            this.lblDescricao.Text = "Descrição";
            // 
            // frmPesquisarVoluntario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(863, 335);
            this.Controls.Add(this.gpbCamposMedidas);
            this.Name = "frmPesquisarVoluntario";
            this.Text = "frmPesquisarVoluntario";
            this.Load += new System.EventHandler(this.frmPesquisarVoluntario_Load);
            this.gpbCamposMedidas.ResumeLayout(false);
            this.gpbCamposMedidas.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbCamposMedidas;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.ListBox ltbPesquisarVoluntarios;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Button btnPesquisarVoluntario;
        private System.Windows.Forms.Label lblDescricao;
    }
}