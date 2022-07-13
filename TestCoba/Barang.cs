using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class Barang
    {
        private Supplier supObj;
        private Inventory invObj;

        private int idBarang;
        private string namaBarang;
        private decimal barangQty;
        private string barangCreatedBy;
        private string barangCreatedDate;
        private string barangUpdateBy;
        private string barangUpdateDate;
        private string barangSatuan;
        private int idSupplier;
        private string barangStatus;
        private string barangTglBeli;
        private int idInventory;
        private int barangJmlBox;
        private string barangItemCode;
        private string barangCategory;

        public string BarangCategory
        {
            get { return barangCategory; }
            set { barangCategory = value; }
        }

        public string BarangItemCode
        {
            get { return barangItemCode; }
            set { barangItemCode = value; }
        }

        public int BarangJmlBox
        {
            get { return barangJmlBox; }
            set { barangJmlBox = value; }
        }

        public int IdInventory
        {
            get { return idInventory; }
            set { idInventory = value; }
        }

        public Inventory InvObj
        {
            get { return invObj; }
            set { invObj = value; }
        }

        public string BarangTglBeli
        {
            get { return barangTglBeli; }
            set { barangTglBeli = value; }
        }

        public string BarangStatus
        {
            get { return barangStatus; }
            set { barangStatus = value; }
        }


        public Supplier SupObj
        {
            get { return supObj; }
            set { supObj = value; }
        }

        public int IdSupplier
        {
            get { return idSupplier; }
            set { idSupplier = value; }
        }

        public string BarangSatuan
        {
            get { return barangSatuan; }
            set { barangSatuan = value; }
        }

        public string BarangUpdateDate
        {
            get { return barangUpdateDate; }
            set { barangUpdateDate = value; }
        }

        public string BarangUpdateBy
        {
            get { return barangUpdateBy; }
            set { barangUpdateBy = value; }
        }

        public string BarangCreatedDate
        {
            get { return barangCreatedDate; }
            set { barangCreatedDate = value; }
        }

        public string BarangCreatedBy
        {
            get { return barangCreatedBy; }
            set { barangCreatedBy = value; }
        }
        public decimal BarangQty
        {
            get { return barangQty; }
            set { barangQty = value; }
        }

        public string NamaBarang
        {
            get { return namaBarang; }
            set { namaBarang = value; }
        }

        public int IdBarang
        {
            get { return idBarang; }
            set { idBarang = value; }
        }
    }
}