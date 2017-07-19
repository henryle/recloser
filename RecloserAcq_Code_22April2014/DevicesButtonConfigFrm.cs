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
    public partial class DevicesButtonConfigFrm : Form
    {
        private string _deviceFilePath = string.Empty;//Application.StartupPath + "\\DeviceFiles\\devices.xml";
        public DevicesButtonConfigFrm(string filepath)
        {
            InitializeComponent();
            _deviceFilePath = filepath;
        }
        public DevicesButtonConfigFrm()
        {
            InitializeComponent();
        }
        private DataTable _dtDeviceGroup;
        public List<GroupClass> GroupList;
        public DataTable DtDeviceGroup
        {
            set
            {
                if(value!=null)
                _dtDeviceGroup = value;
            }
        }
        private void DevicesButtonConfigFrm_Load(object sender, EventArgs e)
        {
            if (_dtDeviceGroup != null)
            {
                dgv.DataSource = _dtDeviceGroup;
            }
            else
            {
                bindingSource1.DataSource = GroupList;
                
            }
            //dgv.Columns[1].Width = 400;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_deviceFilePath))
            {
                DataTable dt = (DataTable)dgv.DataSource;

                dt.WriteXml(_deviceFilePath);
                this.Close();
            }
            else
            {
                foreach (GroupClass gr in GroupList)
                {
                    gr.SaveData();
                }
                this.Close();
            }
        }

        private void btnchangepassword_Click(object sender, EventArgs e)
        {
            ChangePassword cp = new ChangePassword(true);
            cp.ShowDialog();
        }

        private void btnChangeLogin_Click(object sender, EventArgs e)
        {
            ChangePassword cp = new ChangePassword(false);
            cp.ShowDialog();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            GroupList.Add(new GroupClass(true));
            
            dgv.Update();
            dgv.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
            //dgv.Refresh();
        }
    }
}
