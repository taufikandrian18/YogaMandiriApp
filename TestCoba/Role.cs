using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class Role
    {
        private int idEmp;
        private string nameEmp;
        private string userEmp;
        private string passEmp;
        private string addrEmp;
        private string phoneEmp;
        private string statusEmp;
        private string emailEmp;
        private int idRole;
        private string createdBy;
        private string createdDate;
        private string updatedBy;
        private string updatedDate;

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

        public int IdRole
        {
            get { return idRole; }
            set { idRole = value; }
        }

        public string EmailEmp
        {
            get { return emailEmp; }
            set { emailEmp = value; }
        }

        public string StatusEmp
        {
            get { return statusEmp; }
            set { statusEmp = value; }
        }

        public string PhoneEmp
        {
            get { return phoneEmp; }
            set { phoneEmp = value; }
        }

        public string AddrEmp
        {
            get { return addrEmp; }
            set { addrEmp = value; }
        }

        public string PassEmp
        {
            get { return passEmp; }
            set { passEmp = value; }
        }

        public string UserEmp
        {
            get { return userEmp; }
            set { userEmp = value; }
        }

        public string NameEmp
        {
            get { return nameEmp; }
            set { nameEmp = value; }
        }

        public int IdEmp
        {
            get { return idEmp; }
            set { idEmp = value; }
        }
    }
}