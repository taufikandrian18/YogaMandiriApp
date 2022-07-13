using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class PembelianMst
    {
        private int idPembelian;
        private string tglPembelian;
        private string jenisTagihan;
        private string createdBy;
        private string createdDate;
        private string updatedBy;
        private string updatedDate;
        private int idSupplier;
        private string noInvoicePembelian;
        private string tglJatuhTmpPembelian;
        private decimal totalTagihanPembelian;
        private string noRekPembayaran;
        private int idInventory;
        private string statusInvoice;

        public string StatusInvoice
        {
            get { return statusInvoice; }
            set { statusInvoice = value; }
        }

        public int IdInventory
        {
            get { return idInventory; }
            set { idInventory = value; }
        }

        public string NoRekPembayaran
        {
            get { return noRekPembayaran; }
            set { noRekPembayaran = value; }
        }

        public decimal TotalTagihanPembelian
        {
            get { return totalTagihanPembelian; }
            set { totalTagihanPembelian = value; }
        }

        public string TglJatuhTmpPembelian
        {
            get { return tglJatuhTmpPembelian; }
            set { tglJatuhTmpPembelian = value; }
        }

        public string NoInvoicePembelian
        {
            get { return noInvoicePembelian; }
            set { noInvoicePembelian = value; }
        }

        public int IdSupplier
        {
            get { return idSupplier; }
            set { idSupplier = value; }
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

        public string TglPembelian
        {
            get { return tglPembelian; }
            set { tglPembelian = value; }
        }

        public int IdPembelian
        {
            get { return idPembelian; }
            set { idPembelian = value; }
        }
    }
}