using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecloserAcq.Device;

namespace RecloserAcq
{
    public partial class DevicesBtn : Button
    {
        private string _deviceFilePath = string.Empty;
        private frmDeviceStatus _frm;
        public int GroupID;
        //dvcBtn.Click += new System.EventHandler(this.devicesBtn1_Click);
        public string DeviceFilePath
        {
            get { return _deviceFilePath; }
            set
            {
                if (_deviceFilePath != value && !string.IsNullOrEmpty(value))
                {
                    _deviceFilePath = value;

                }
            }
        }
        public RecloserBase getRecloser(int Id)
        {
            if (_frm == null)
            {
                return null;
            }
            return _frm.getRecloser(Id);
        }
        public DevicesBtn()
        {
            InitializeComponent();
        }
        
        private void DevicesBtn_Click(object sender, EventArgs e)
        {
            //_frm = new frmDeviceStatus("devicefile4.xml");
            //_frm.Show();
            if (_frm == null || _frm.IsDisposed)
            {
                if (string.IsNullOrEmpty(_deviceFilePath))
                {
                    _frm = new frmDeviceStatus(GroupID);
                }
                else
                {
                    _frm = new frmDeviceStatus(_deviceFilePath);
                }
                _frm.Text = this.Text;
                _frm.Show();
            }
            else
            {
                
                _frm.Focus();
            }
            
        }
    }
}
