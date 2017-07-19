using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecloserAcq.Device
{
    public class GroupClass
    {
        private int _id =0;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public bool newobj = false;
        private string _groupname = string.Empty;
        public string GroupName
        {
            get
            {
                return _groupname;
            }
            set
            {
                _groupname = value;
            }
        }
        public GroupClass() { }
        public GroupClass(bool newobj) {
            this.newobj = newobj;
        }
        public void SaveData()
        {
            if (newobj)
            {
                DeviceStatic.InsertGroup(this);
            }
            else
            {
                DeviceStatic.SaveGroup(this);
            }
        }
    }
}
