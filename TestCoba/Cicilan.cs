using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class Cicilan
    {
        private int idCicilan;
        private int idPOPenjualanMst;
        private decimal nilaiPembayaran;

        public int IdCicilan { get => idCicilan; set => idCicilan = value; }
        public int IdPOPenjualanMst { get => idPOPenjualanMst; set => idPOPenjualanMst = value; }
        public decimal NilaiPembayaran { get => nilaiPembayaran; set => nilaiPembayaran = value; }
    }
}