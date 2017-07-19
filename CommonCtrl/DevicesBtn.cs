using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecloserAcq
{
    public partial class DevicesBtn : Button
    {
        private string _deviceFilePath;
        private frmDeviceStatus _frm;
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
        public DevicesBtn()
        {
            InitializeComponent();
        }

        private void DevicesBtn_Click(object sender, EventArgs e)
        {
            if (_frm == null || _frm.IsDisposed)
            {
                _frm = new frmDeviceStatus(_deviceFilePath);
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
