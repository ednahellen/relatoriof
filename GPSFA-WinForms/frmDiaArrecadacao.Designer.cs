namespace GPSFA_WinForms
{
    partial class frmTipoDeArrecadacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTipoDeArrecadacao));
            this.lblTipoDeArrecadacao = new System.Windows.Forms.Label();
            this.gpbCamposArrecadacao = new System.Windows.Forms.GroupBox();
            this.cbbTipoDeArrecadacao = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gpbCamposArrecadacao.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTipoDeArrecadacao
            // 
            this.lblTipoDeArrecadacao.AutoSize = true;
            this.lblTipoDeArrecadacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoDeArrecadacao.ForeColor = System.Drawing.Color.Black;
            this.lblTipoDeArrecadacao.Location = new System.Drawing.Point(16, 20);
            this.lblTipoDeArrecadacao.Name = "lblTipoDeArrecadacao";
            this.lblTipoDeArrecadacao.Size = new System.Drawing.Size(400, 29);
            this.lblTipoDeArrecadacao.TabIndex = 3;
            this.lblTipoDeArrecadacao.Text = "Selecione o tipo de arrecadação:";
            // 
            // gpbCamposArrecadacao
            // 
            this.gpbCamposArrecadacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.gpbCamposArrecadacao.Controls.Add(this.cbbTipoDeArrecadacao);
            this.gpbCamposArrecadacao.Controls.Add(this.btnCancelar);
            this.gpbCamposArrecadacao.Controls.Add(this.lblTipoDeArrecadacao);
            this.gpbCamposArrecadacao.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpbCamposArrecadacao.Location = new System.Drawing.Point(12, 11);
            this.gpbCamposArrecadacao.Name = "gpbCamposArrecadacao";
            this.gpbCamposArrecadacao.Size = new System.Drawing.Size(737, 121);
            this.gpbCamposArrecadacao.TabIndex = 15;
            this.gpbCamposArrecadacao.TabStop = false;
            // 
            // cbbTipoDeArrecadacao
            // 
            this.cbbTipoDeArrecadacao.FormattingEnabled = true;
            this.cbbTipoDeArrecadacao.Location = new System.Drawing.Point(16, 51);
            this.cbbTipoDeArrecadacao.Name = "cbbTipoDeArrecadacao";
            this.cbbTipoDeArrecadacao.Size = new System.Drawing.Size(564, 35);
            this.cbbTipoDeArrecadacao.TabIndex = 23;
            this.cbbTipoDeArrecadacao.SelectedIndexChanged += new System.EventHandler(this.cbbTipoDeArrecadacao_SelectedIndexChanged);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelar.Image")));
            this.btnCancelar.Location = new System.Drawing.Point(596, 49);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(130, 40);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmTipoDeArrecadacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 142);
            this.Controls.Add(this.gpbCamposArrecadacao);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmTipoDeArrecadacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista São Francisco de Assis - Tipo de Arrecação";
            this.gpbCamposArrecadacao.ResumeLayout(false);
            this.gpbCamposArrecadacao.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblTipoDeArrecadacao;
        private System.Windows.Forms.GroupBox gpbCamposArrecadacao;
        private System.Windows.Forms.ComboBox cbbTipoDeArrecadacao;
        private System.Windows.Forms.Button btnCancelar;
    }
}