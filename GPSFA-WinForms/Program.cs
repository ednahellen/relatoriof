using Projeto_Socorrista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSFA_WinForms
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
<<<<<<< HEAD
            Application.Run(new frmOrigemDoacao());
=======
            Application.Run(new frmMenuPrincipal());
>>>>>>> dc32c7218e4572ed7061238d5db8060bc574e6f6
        }
    }
}
