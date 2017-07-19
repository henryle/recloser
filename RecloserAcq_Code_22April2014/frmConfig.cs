using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecloserAcq.Properties;
using System.Xml.Serialization;
using RecloserAcq.Device;
using System.IO;

namespace RecloserAcq
{
    public partial class frmConfig : Form
    {
        Settings _setting = RecloserAcq.Properties.Settings.Default;
        private string _deviceFile = string.Empty;
        private int _groupid;
        public frmConfig(string DeviceFile)
        {
            InitializeComponent();
            _deviceFile = DeviceFile;
        }
        public frmConfig(int groupid)
        {
            InitializeComponent();
            _groupid = groupid;
        }
        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                //_setting.PollingInterval = (int)pollingIntervalNumericUpDown.Value;
               // _setting.LatencyBetweenRequest = (int)latencyBetweenRequestNumericUpDown.Value;
                _setting.CurrentDuration = (int)numCurrentDuration.Value;
                _setting.alertsoundfile = txtAlertSoundFile.Text;
                _setting.soundduration = (int)nmsoundduration.Value;
                _setting.MaxCurrent = (int)numMaxCurrent.Value;
                //_setting.SaveDataInterval = (int)numSaveDataInterval.Value;
                _setting.playcount = (int)nmplaycount.Value; 
                _setting.dvpath = txtdvpath.Text.Trim();
                _setting.Save();
                if (string.IsNullOrEmpty(_deviceFile))
                {
                    DeviceStatic.SaveDevices(recloserListView1.DataSource);
                }
                else
                {
                    DeviceStatic.SaveDevices(recloserListView1.DataSource, _deviceFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("frmconfig _ ok click",ex.Message);
            }

            this.Close();
        }
        
        private void frmConfig_Load(object sender, EventArgs e)
        {
            try
            {
                //pollingIntervalNumericUpDown.Value = _setting.PollingInterval;
                //latencyBetweenRequestNumericUpDown.Value = _setting.LatencyBetweenRequest;
                numCurrentDuration.Value = _setting.CurrentDuration;
                numMaxCurrent.Value = _setting.MaxCurrent;
                txtAlertSoundFile.Text = _setting.alertsoundfile;
                //numSaveDataInterval.Value = _setting.SaveDataInterval;
                nmsoundduration.Value = _setting.soundduration;
                nmplaycount.Value = _setting.playcount;
                txtdvpath.Text = _setting.dvpath;
                //recloserListView1.DataSource = DeviceStatic.GetDevices(_deviceFile);                
                recloserListView1.DataSource = DeviceStatic.GetDevices(_groupid);                //DBController.Instance.GetDevices                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Form config load",ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _setting.Reload();
            this.Close();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            TuBu rc = new TuBu();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            
            rc.Name = "Tu Bu";
            rc.GroupId = this._groupid;
            rc.isnew = true;
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 150000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnAddNulec_Click(object sender, EventArgs e)
        {
            Nulec rc = new Nulec();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;

            rc.Name = "Nulec";
            rc.Port = rc.Id;
            rc.GroupId = this._groupid;
            rc.isnew = true;
            rc.Latencybetweenpoll = 3000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnAddCooper_Click(object sender, EventArgs e)
        {
            CooperFXB rc = new CooperFXB();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            rc.isnew = true;
            rc.Name = "Cooper";
            rc.Port = rc.Id;
            rc.GroupId = this._groupid;
            rc.Latencybetweenpoll = 3000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnAddRecloserSel_Click(object sender, EventArgs e)
        {
            Recloser351R rc = new Recloser351R();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            rc.isnew = true;
            rc.Name = "RecloserSEL";
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 60000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            rc.GroupId = this._groupid;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnAddDKDT_Click(object sender, EventArgs e)
        {
            Elster1700 rc = new Elster1700();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            rc.isnew = true;
            rc.GroupId = this._groupid;
            rc.Name = "Điện Kế ĐT";
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 60000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = (float)1;
            rc.MinOpen = (float)-1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            recloserListView1.DeleteSelected();
        }

        private void btnAddRecloserADVC_Click(object sender, EventArgs e)
        {
            RecloserADVC rc = new RecloserADVC();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            rc.isnew = true;
            rc.GroupId = this._groupid;
            rc.Name = "ADVC";
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 3000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnaddRelADVC45_Click(object sender, EventArgs e)
        {
            RecloserADVC45 rc = new RecloserADVC45();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            rc.isnew = true;
            rc.GroupId = this._groupid;
            rc.Name = "ADVC45";
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 3000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (Program.user.permUser != 2)
            {
                MessageBox.Show("Bạn không được cấp quyền");
                return;
            }
            Userform userfrm = new Userform(RecloserAcq.Properties.Settings.Default.SQLConnection,Program.user);

            userfrm.StartPosition = FormStartPosition.CenterScreen;
            userfrm.Show();
        }

        private void btnAddNulecUseries_Click(object sender, EventArgs e)
        {
            RecloserUSeries rc = new RecloserUSeries();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            rc.isnew = true;
            rc.GroupId = this._groupid;
            rc.Name = "Nulect Useries";
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 3000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

        private void btnAdvcTcpIp_Click(object sender, EventArgs e)
        {
            RecloserADVCTCPIP rc = new RecloserADVCTCPIP();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;
            rc.isnew = true;
            rc.GroupId = this._groupid;
            rc.Name = "ADVCTCPIP";
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 3000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();

        }

        private void btnAddLBS_Click(object sender, EventArgs e)
        {
            LBS rc = new LBS();
            int idmax = recloserListView1.DataSource.Max(x => x.Id);
            rc.Id = idmax + 1;

            rc.Name = "LBS";
            rc.GroupId = this._groupid;
            rc.isnew = true;
            rc.Port = rc.Id;
            rc.Latencybetweenpoll = 150000;
            rc.Latencybetweenrequest = 2000;
            rc.LatencySaveHistory = 300000;
            rc.MaxClose = 1;
            rc.MinOpen = -1;
            recloserListView1.DataSource.Add(rc);
            //recloserListView1.DataSource = listrc;
            //recloserListView1.Refresh();
            recloserListView1.UpdateGrid();
            recloserListView1.Refresh();
        }

       
    }
}
