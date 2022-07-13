using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class Inventory
    {
        private int idInventory;
        private string ivnStorage;
        private int ivnCcapacity;
        private string ivnCreatedBy;
        private string ivnCreatedDate;
        private string ivnUpdateBy;
        private string ivnUpdateDate;
        private string invStatus;
        private string invSisaCapacity;

        public string InvSisaCapacity
        {
            get { return invSisaCapacity; }
            set { invSisaCapacity = value; }
        }

        public int IdInventory
        {
            get { return idInventory; }
            set { idInventory = value; }
        }
        public string IvnStorage
        {
            get { return ivnStorage; }
            set { ivnStorage = value; }
        }
        public int IvnCcapacity
        {
            get { return ivnCcapacity; }
            set { ivnCcapacity = value; }
        }
        public string IvnCreatedBy
        {
            get { return ivnCreatedBy; }
            set { ivnCreatedBy = value; }
        }
        public string IvnCreatedDate
        {
            get { return ivnCreatedDate; }
            set { ivnCreatedDate = value; }
        }
        public string IvnUpdateBy
        {
            get { return ivnUpdateBy; }
            set { ivnUpdateBy = value; }
        }
        public string IvnUpdateDate
        {
            get { return ivnUpdateDate; }
            set { ivnUpdateDate = value; }
        }
        public string InvStatus
        {
            get { return invStatus; }
            set { invStatus = value; }
        }
    }
}