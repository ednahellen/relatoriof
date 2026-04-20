using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSFA_WinForms
{
    public class AjusteEstoque
    {
        public int CodList { get; set; }
        public string Produto { get; set; }
        public int SaldoAtual { get; set; }
        public int QuantidadePlanilha { get; set; }
        public int Diferenca { get; set; }
        public string Tipo => Diferenca > 0 ? "ENTRADA" : "SAIDA";
    }
}
