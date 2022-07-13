using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class Customer
    {
        private int idCustomer;
        private string customerName;
        private string customerAddress;
        private string customerPhone;
        private string customerEmail;
        private string customerStatus;
        private string customerCreatedBy;
        private string customerCreatedDate;
        private string customerUpdatedBy;
        private string customerUpdatedDate;

        public string CustomerUpdatedDate
        {
            get { return customerUpdatedDate; }
            set { customerUpdatedDate = value; }
        }

        public string CustomerUpdatedBy
        {
            get { return customerUpdatedBy; }
            set { customerUpdatedBy = value; }
        }

        public string SupplierCreatedDate
        {
            get { return customerCreatedDate; }
            set { customerCreatedDate = value; }
        }

        public string CustomerCreatedBy
        {
            get { return customerCreatedBy; }
            set { customerCreatedBy = value; }
        }

        public string CustomerStatus
        {
            get { return customerStatus; }
            set { customerStatus = value; }
        }

        public string CustomerEmail
        {
            get { return customerEmail; }
            set { customerEmail = value; }
        }

        public string CustomerrPhone
        {
            get { return customerPhone; }
            set { customerPhone = value; }
        }

        public string CustomerAddress
        {
            get { return customerAddress; }
            set { customerAddress = value; }
        }


        public string CupplierName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public int IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; }
        }
    }
}