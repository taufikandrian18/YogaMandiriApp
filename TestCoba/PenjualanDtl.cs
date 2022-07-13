using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class PenjualanDtl
    {
        private int idPOPenjualanDtl;
        private int idPOPenjualanMst;
        private int idBarang;
        private decimal totalHargaPOPenjualanDtl;
        private int jumlahBoxPOPenjualanDtl;
        private string penjualanDtlCreatedBy;
        private string penjualanDtlCreatedDate;
        private string penjualanDtlUpdatedBy;
        private string penjualanDtlUpdatedDate;
        private string namaBarangPOPenjualanDtl;
        private string qtyBarangPOPenjualanDtl;
        private string hargaPOPenjualanDtl;

        public string HargaPOPenjualanDtl
        {
            get { return hargaPOPenjualanDtl; }
            set { hargaPOPenjualanDtl = value; }
        }

        public string QtyBarangPOPenjualanDtl
        {
            get { return qtyBarangPOPenjualanDtl; }
            set { qtyBarangPOPenjualanDtl = value; }
        }

        public string NamaBarangPOPenjualanDtl
        {
            get { return namaBarangPOPenjualanDtl; }
            set { namaBarangPOPenjualanDtl = value; }
        }

        public string PenjualanDtlUpdatedDate
        {
            get { return penjualanDtlUpdatedDate; }
            set { penjualanDtlUpdatedDate = value; }
        }

        public string PenjualanDtlUpdatedBy
        {
            get { return penjualanDtlUpdatedBy; }
            set { penjualanDtlUpdatedBy = value; }
        }

        public string PenjualanDtlCreatedDate
        {
            get { return penjualanDtlCreatedDate; }
            set { penjualanDtlCreatedDate = value; }
        }

        public string PenjualanDtlCreatedBy
        {
            get { return penjualanDtlCreatedBy; }
            set { penjualanDtlCreatedBy = value; }
        }

        public int JumlahBoxPOPenjualanDtl
        {
            get { return jumlahBoxPOPenjualanDtl; }
            set { jumlahBoxPOPenjualanDtl = value; }
        }

        public decimal TotalHargaPOPenjualanDtl
        {
            get { return totalHargaPOPenjualanDtl; }
            set { totalHargaPOPenjualanDtl = value; }
        }

        public int IdBarang
        {
            get { return idBarang; }
            set { idBarang = value; }
        }

        public int IdPOPenjualanMst
        {
            get { return idPOPenjualanMst; }
            set { idPOPenjualanMst = value; }
        }

        public int IdPOPenjualanDtl
        {
            get { return idPOPenjualanDtl; }
            set { idPOPenjualanDtl = value; }
        }
    }
}