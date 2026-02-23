namespace GPSFA_WinForms
{
    partial class frmMenuPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenuPrincipal));
            this.pnlForm = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGerenciarProdutos = new System.Windows.Forms.Button();
            this.btnVoluntarios = new System.Windows.Forms.Button();
            this.btnRelatorios = new System.Windows.Forms.Button();
            this.btnAlimentos = new System.Windows.Forms.Button();
            this.pnlDadosProdutos = new System.Windows.Forms.Panel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.pnlForm.Controls.Add(this.button1);
            this.pnlForm.Controls.Add(this.btnGerenciarProdutos);
            this.pnlForm.Controls.Add(this.btnVoluntarios);
            this.pnlForm.Controls.Add(this.btnRelatorios);
            this.pnlForm.Controls.Add(this.btnAlimentos);
            this.pnlForm.Controls.Add(this.pnlDadosProdutos);
            this.pnlForm.Controls.Add(this.btnDashboard);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(943, 507);
            this.pnlForm.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(11)))), ((int)(((byte)(13)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(783, 449);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 46);
            this.button1.TabIndex = 11;
            this.button1.Text = "&Sair";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnGerenciarProdutos
            // 
            this.btnGerenciarProdutos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnGerenciarProdutos.FlatAppearance.BorderSize = 0;
            this.btnGerenciarProdutos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGerenciarProdutos.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGerenciarProdutos.ForeColor = System.Drawing.Color.White;
            this.btnGerenciarProdutos.Image = ((System.Drawing.Image)(resources.GetObject("btnGerenciarProdutos.Image")));
            this.btnGerenciarProdutos.Location = new System.Drawing.Point(252, 69);
            this.btnGerenciarProdutos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGerenciarProdutos.Name = "btnGerenciarProdutos";
            this.btnGerenciarProdutos.Size = new System.Drawing.Size(220, 200);
            this.btnGerenciarProdutos.TabIndex = 9;
            this.btnGerenciarProdutos.Text = "Gerenciar  Produtos";
            this.btnGerenciarProdutos.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGerenciarProdutos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGerenciarProdutos.UseVisualStyleBackColor = false;
            this.btnGerenciarProdutos.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnVoluntarios
            // 
            this.btnVoluntarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnVoluntarios.FlatAppearance.BorderSize = 0;
            this.btnVoluntarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoluntarios.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoluntarios.ForeColor = System.Drawing.Color.White;
            this.btnVoluntarios.Image = ((System.Drawing.Image)(resources.GetObject("btnVoluntarios.Image")));
            this.btnVoluntarios.Location = new System.Drawing.Point(480, 69);
            this.btnVoluntarios.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnVoluntarios.Name = "btnVoluntarios";
            this.btnVoluntarios.Size = new System.Drawing.Size(220, 200);
            this.btnVoluntarios.TabIndex = 2;
            this.btnVoluntarios.Text = "Voluntários";
            this.btnVoluntarios.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVoluntarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVoluntarios.UseVisualStyleBackColor = false;
            this.btnVoluntarios.Click += new System.EventHandler(this.btnVoluntarios_Click);
            // 
            // btnRelatorios
            // 
            this.btnRelatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnRelatorios.FlatAppearance.BorderSize = 0;
            this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorios.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelatorios.ForeColor = System.Drawing.Color.White;
            this.btnRelatorios.Image = ((System.Drawing.Image)(resources.GetObject("btnRelatorios.Image")));
            this.btnRelatorios.Location = new System.Drawing.Point(252, 279);
            this.btnRelatorios.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRelatorios.Name = "btnRelatorios";
            this.btnRelatorios.Size = new System.Drawing.Size(220, 200);
            this.btnRelatorios.TabIndex = 3;
            this.btnRelatorios.Text = "Relatórios";
            this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRelatorios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRelatorios.UseVisualStyleBackColor = false;
            this.btnRelatorios.Click += new System.EventHandler(this.btnRelatorios_Click);
            // 
            // btnAlimentos
            // 
            this.btnAlimentos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnAlimentos.FlatAppearance.BorderSize = 0;
            this.btnAlimentos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlimentos.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlimentos.ForeColor = System.Drawing.Color.White;
            this.btnAlimentos.Image = ((System.Drawing.Image)(resources.GetObject("btnAlimentos.Image")));
            this.btnAlimentos.Location = new System.Drawing.Point(24, 69);
            this.btnAlimentos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAlimentos.Name = "btnAlimentos";
            this.btnAlimentos.Size = new System.Drawing.Size(220, 200);
            this.btnAlimentos.TabIndex = 8;
            this.btnAlimentos.Text = "Estoque";
            this.btnAlimentos.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAlimentos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAlimentos.UseVisualStyleBackColor = false;
            this.btnAlimentos.Click += new System.EventHandler(this.btnEstoque_Click);
            // 
            // pnlDadosProdutos
            // 
            this.pnlDadosProdutos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.pnlDadosProdutos.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDadosProdutos.Location = new System.Drawing.Point(0, 0);
            this.pnlDadosProdutos.Name = "pnlDadosProdutos";
            this.pnlDadosProdutos.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlDadosProdutos.Size = new System.Drawing.Size(943, 48);
            this.pnlDadosProdutos.TabIndex = 2;
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Image = ((System.Drawing.Image)(resources.GetObject("btnDashboard.Image")));
            this.btnDashboard.Location = new System.Drawing.Point(24, 279);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(220, 200);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // frmMenuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(943, 507);
            this.Controls.Add(this.pnlForm);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmMenuPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupo Socorrista Francisco de Assis - Menu Principal";
            this.Load += new System.EventHandler(this.frmMenuPrincipal_Load);
            this.pnlForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Button btnGerenciarProdutos;
        private System.Windows.Forms.Button btnVoluntarios;
        private System.Windows.Forms.Button btnRelatorios;
        private System.Windows.Forms.Button btnAlimentos;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Panel pnlDadosProdutos;
        private System.Windows.Forms.Button button1;
    }
}