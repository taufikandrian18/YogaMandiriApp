using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class PenjualanMst
    {
        private int idPOPenjualanMst;
        private string tanggalPOPenjualan;
        private string jenisTagihan;
        private string createdBy;
        private string createdDate;
        private string updatedBy;
        private string updatedDate;
        private string noInvoicePenjualan;
        private string tanggalPOJatuhTmpPenjualan;
        private decimal totalTagihanPenjualan;
        private string noRekeningPembayaran;
        private int idInventory;
        private int idSupplier;
        private int idCustomer;
        private string statusInvoicePenjualan;

        public string StatusInvoicePenjualan
        {
            get { return statusInvoicePenjualan; }
            set { statusInvoicePenjualan = value; }
        }

        public int IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; }
        }

        public int IdSupplier
        {
            get { return idSupplier; }
            set { idSupplier = value; }
        }

        public int IdInventory
        {
            get { return idInventory; }
            set { idInventory = value; }
        }

        public string NoRekeningPembayaran
        {
            get { return noRekeningPembayaran; }
            set { noRekeningPembayaran = value; }
        }

        public decimal TotalTagihanPenjualan
        {
            get { return totalTagihanPenjualan; }
            set { totalTagihanPenjualan = value; }
        }

        public string TanggalPOJatuhTmpPenjualan
        {
            get { return tanggalPOJatuhTmpPenjualan; }
            set { tanggalPOJatuhTmpPenjualan = value; }
        }

        public string NoInvoicePenjualan
        {
            get { return noInvoicePenjualan; }
            set { noInvoicePenjualan = value; }
        }

        public string UpdatedDate
        {
            get { return updatedDate; }
            set { updatedDate = value; }
        }

        public string UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        public string CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public string JenisTagihan
        {
            get { return jenisTagihan; }
            set { jenisTagihan = value; }
        }

        public string TanggalPOPenjualan
        {
            get { return tanggalPOPenjualan; }
            set { tanggalPOPenjualan = value; }
        }

        public int IdPOPenjualanMst
        {
            get { return idPOPenjualanMst; }
            set { idPOPenjualanMst = value; }
        }
    }
}