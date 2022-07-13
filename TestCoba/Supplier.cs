using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class Supplier
    {
        private int idSupplier;
        private string supplierName;
        private string supplierAddress;
        private string supplierPhone;
        private string supplierEmail;
        private string supplierStatus;
        private string supplierCreatedBy;
        private string supplierCreatedDate;
        private string supplierUpdatedBy;
        private string supplierUpdatedDate;

        public string SupplierUpdatedDate
        {
            get { return supplierUpdatedDate; }
            set { supplierUpdatedDate = value; }
        }

        public string SupplierUpdatedBy
        {
            get { return supplierUpdatedBy; }
            set { supplierUpdatedBy = value; }
        }

        public string SupplierCreatedDate
        {
            get { return supplierCreatedDate; }
            set { supplierCreatedDate = value; }
        }

        public string SupplierCreatedBy
        {
            get { return supplierCreatedBy; }
            set { supplierCreatedBy = value; }
        }

        public string SupplierStatus
        {
            get { return supplierStatus; }
            set { supplierStatus = value; }
        }

        public string SupplierEmail
        {
            get { return supplierEmail; }
            set { supplierEmail = value; }
        }

        public string SupplierPhone
        {
            get { return supplierPhone; }
            set { supplierPhone = value; }
        }

        public string SupplierAddress
        {
            get { return supplierAddress; }
            set { supplierAddress = value; }
        }


        public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        public int IdSupplier
        {
            get { return idSupplier; }
            set { idSupplier = value; }
        }
    }
}