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
            Application.Run(new frmLogin());
=======
            Application.Run(new frmMenuPrincipal());
>>>>>>> 964de8756bda6058d9ea79c39df30203330316a4
        }
    }
}
