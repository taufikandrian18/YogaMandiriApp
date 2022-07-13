using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class BiayaDtl
    {
        private int _idBiayaDtl;
        private string _namaBiayaDtl;
        private decimal _hargaBiayaDtl;
        private int _idBiayaMst;
        private string _createdBy;
        private string _createdDate;
        private string _updatedBy;
        private string _updatedDate;

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

        public int IdBiayaMst
        {
            get { return _idBiayaMst; }
            set { _idBiayaMst = value; }
        }

        public decimal HargaBiayaDtl
        {
            get { return _hargaBiayaDtl; }
            set { _hargaBiayaDtl = value; }
        }

        public string NamaBiayaDtl
        {
            get { return _namaBiayaDtl; }
            set { _namaBiayaDtl = value; }
        }

        public int IdBiayaDtl
        {
            get { return _idBiayaDtl; }
            set { _idBiayaDtl = value; }
        }
    }
}