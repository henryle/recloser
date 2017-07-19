using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecloserAcq.Device;

namespace RecloserAcq
{
    public partial class frmcompensationcapacitor : Form
    {
        private string _devicefile=string.Empty;
        private int _groupid;
        public frmcompensationcapacitor(string DeviceFile)
        {
            InitializeComponent();
            _devicefile = DeviceFile;
        }
        public frmcompensationcapacitor(int groupid)
        {
            InitializeComponent();
            _groupid = groupid;
        }
        public bool changed = false;
        private void btnNew_Click(object sender, EventArgs e)
        {
            using (frmDongCatEvent frm = new frmDongCatEvent(this._groupid))
            {
                DialogResult rs = frm.ShowDialog();
                if (rs == DialogResult.OK)
                {
                    changed = true;
                    bindingSource1.Add(frm.dvevent);
                    grSchedule.Update();
                    grSchedule.Refresh();
                    
                }
            }
        }

        private void frmcompensationcapacitor_Load(object sender, EventArgs e)
        {
            loadschedule();
               
        }

        private void loadschedule()
        {
            if (string.IsNullOrEmpty(_devicefile))
            {
                bindingSource1.DataSource = DeviceStatic.GetDevicesSchedule(_groupid);
            }
            else
            {
                bindingSource1.DataSource = DeviceStatic.GetDevicesSchedule(_devicefile);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DeviceEvent dve = (DeviceEvent )bindingSource1.Current;
            DeviceStatic.DeleteDeviceSchedule(dve);
            this.changed = true;
            bindingSource1.Remove(dve);
            grSchedule.Update();
            grSchedule.Refresh();
            
        }
    }
}
