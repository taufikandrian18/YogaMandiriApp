using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class BiayaMst
    {
        private int _idBiayaMst;
        private string _jenisBiayaMst;
        private string _tanggalBiayaMst;
        private string _akunBiayaMst;
        private string _createdBy;
        private string _createdDate;
        private string _updatedBy;
        private string _updatedDate;
        private decimal _totalBiayaMst;

        public decimal TotalBiayaMst
        {
            get { return _totalBiayaMst; }
            set { _totalBiayaMst = value; }
        }

        public string UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }

        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { _updatedBy = value; }
        }

        public string CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public string AkunBiayaMst
        {
            get { return _akunBiayaMst; }
            set { _akunBiayaMst = value; }
        }

        public string TanggalBiayaMst
        {
            get { return _tanggalBiayaMst; }
            set { _tanggalBiayaMst = value; }
        }

        public string JenisBiayaMst
        {
            get { return _jenisBiayaMst; }
            set { _jenisBiayaMst = value; }
        }

        public int IdBiayaMst
        {
            get { return _idBiayaMst; }
            set { _idBiayaMst = value; }
        }
    }
}