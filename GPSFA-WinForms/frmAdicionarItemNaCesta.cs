using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    public partial class frmAdicionarItemNaCesta : Form
    {
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);
        
        public frmAdicionarItemNaCesta()
        {
            InitializeComponent();
        }

        public frmAdicionarItemNaCesta(int codUsu)
        {
            codUsuLogado = codUsu;
            //externalWindowRef = windowRef;
            InitializeComponent();
            carregarProdutosCbb();
        }

        int codUsuLogado;
        string externalWindowRef;

        public int CodProdutoSelecionado { get; private set; }
        public string NomeProdutoSelecionado { get; private set; }
        public int QuantidadeSelecionada { get; private set; }

        private void carregarProdutosCbb()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "SELECT descricao FROM tbLista ORDER BY descricao ASC;";
            comm.CommandType = CommandType.Text;

            comm.Connection = DataBaseConnection.OpenConnection();

            MySqlDataReader DR = comm.ExecuteReader();

            while (DR.Read())
            {
                cbbProdutos.Items.Add(DR.GetString(0));
            }

            DataBaseConnection.CloseConnection();
        }

        // Evento de pressionar teclas na caixa de texto de quantidade - limita a entrada de dados a números
        private void txtQtdCestas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool QuantidadeValida()
        {
            if (!int.TryParse(txtQuantidade.Text, out int quantidade))
                return false;

            return quantidade > 0;
        }

        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            if (cbbProdutos.SelectedItem == null || txtQuantidade.Text.Equals("") || !QuantidadeValida())
            {
                MessageBox.Show("Selecione um produto e defina sua quantidade por cesta para continuar", "Mensagm do sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                NomeProdutoSelecionado = cbbProdutos.SelectedItem.ToString();
                QuantidadeSelecionada = Convert.ToInt32(txtQuantidade.Text);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
