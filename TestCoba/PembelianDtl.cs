using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class PembelianDtl
    {
        private int idPembelianDtl;
        private int idPembelianMst;
        private int idBarang;
        private string hargaPembelianDtl;
        private decimal totalHargaPembelianDtl;
        private int jumlahBoxPembelianDtl;
        private string qtyBarangPembelianDtl;
        private string namaBarangPembelianDtl;

        public string NamaBarangPembelianDtl
        {
            get { return namaBarangPembelianDtl; }
            set { namaBarangPembelianDtl = value; }
        }

        public string QtyBarangPembelianDtl
        {
            get { return qtyBarangPembelianDtl; }
            set { qtyBarangPembelianDtl = value; }
        }


        public int JumlahBoxPembelianDtl
        {
            get { return jumlahBoxPembelianDtl; }
            set { jumlahBoxPembelianDtl = value; }
        }


        public decimal TotalHargaPembelianDtl
        {
            get { return totalHargaPembelianDtl; }
            set { totalHargaPembelianDtl = value; }
        }

        public string HargaPembelianDtl
        {
            get { return hargaPembelianDtl; }
            set { hargaPembelianDtl = value; }
        }

        public int IdBarang
        {
            get { return idBarang; }
            set { idBarang = value; }
        }

        public int IdPembelianMst
        {
            get { return idPembelianMst; }
            set { idPembelianMst = value; }
        }

        public int IdPembelianDtl
        {
            get { return idPembelianDtl; }
            set { idPembelianDtl = value; }
        }
    }
}