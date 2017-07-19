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
    public partial class frmDongCatEvent : Form
    {
        private string _devicefile = string.Empty;
        public DeviceEvent dvevent;
        private int _groupid;
        public frmDongCatEvent(string DeviceFile)
        {
            InitializeComponent();
            _devicefile = DeviceFile;
        }
        public frmDongCatEvent(int groupid)
        {
            InitializeComponent();
            _groupid = groupid;
        }
        private void frmDongCatEvent_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_devicefile))
            {
                cbNut.DataSource = DeviceStatic.GetDevices(_groupid).OfType<TuBu>().ToList<TuBu>();
            }
            else
            {
                cbNut.DataSource = DeviceStatic.GetDevices(_devicefile).OfType<TuBu>().ToList<TuBu>();//Program.GetDevices().OfType<TuBu>();
            }
            cbNut.DisplayMember = "Name";
            cbNut.ValueMember = "Id";
            
        }

        private void rdOneTime_CheckedChanged(object sender, EventArgs e)
        {
            if (rdOneTime.Checked)
            {
                panelweekday.Visible = false;
                paneldtActive.Visible = false;
                paneldtExpire.Visible = false;
                dtTrigger.CustomFormat = "dd/MM/yyyy HH:mm:ss";
                panelrepeat.Visible = true;
            }
        }

        private void rdEveryday_CheckedChanged(object sender, EventArgs e)
        {
            if (rdEveryday.Checked)
            {
                panelweekday.Visible = false;
                paneldtActive.Visible = true;
                paneldtExpire.Visible = true;
                dtTrigger.CustomFormat = "HH:mm:ss";
                panelrepeat.Visible = false;
            }
        }

        private void rdEveryWeek_CheckedChanged(object sender, EventArgs e)
        {
            if (rdEveryWeek.Checked)
            {
                panelweekday.Visible = true;
                paneldtActive.Visible = true;
                paneldtExpire.Visible = true;
                dtTrigger.CustomFormat = "dd/MM/yyyy HH:mm:ss";
                panelrepeat.Visible = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(dtExpire.Value <= DateTime.Now)
            {
                 MessageBox.Show("Vui long chọn thời gian hết hiệu lực");
                        dtExpire.Focus();
                return;
            }

            try
            {
                
                dvevent = new DeviceEvent();
                dvevent.NameOfEvent = txtName.Text;
                dvevent.Weekday = "";
                dvevent.DtActive = DateTime.MinValue;
                //int id = (int)cbNut.SelectedValue;
                string cmd;
                if (cbcommand.SelectedIndex == 0)
                {
                    cmd = "Dong";
                }
                else if (cbcommand.SelectedIndex == 1)
                {
                    cmd = "Mo";
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn lệnh");
                    return;
                }
                dvevent.Command = cmd;
                dvevent.DeviceId = (int)cbNut.SelectedValue;
                dvevent.DtExpire = dtExpire.Value;
                DateTime dtExe;
                if (rdOneTime.Checked)
                {
                    dtExe = dtTrigger.Value;
                    if ((dtExe- DateTime.Now).TotalMinutes<5)
                    {
                        MessageBox.Show("Vui lòng chọn thời gian xa hơn");
                        dtTrigger.Focus();
                        return;
                    }
                    // Save id,cmd,ttype,dtExe
                    dvevent.Type = "onetime";
                    dvevent.hourRepeat = (int)nmHourRepeat.Value;
                    dvevent.DtNextRun = dtTrigger.Value;
                }
                else if (rdEveryday.Checked) {
                    dvevent.Type = "daily";
                    if(dtActive.Value <= DateTime.Now){ 
                        MessageBox.Show("Vui long chọn thời gian hiệu lực");
                        dtActive.Focus();
                        return;
                    }
                    dvevent.DtActive = dtActive.Value;
                    dvevent.DtNextRun = new DateTime(dtActive.Value.Year, dtActive.Value.Month, dtActive.Value.Day, dtTrigger.Value.Hour, dtTrigger.Value.Minute, dtTrigger.Value.Second);
                }
                else if (rdEveryWeek.Checked) {
                    if (dtActive.Value <= DateTime.Now)
                    {
                        MessageBox.Show("Vui long chọn thời gian hiệu lực");
                        dtActive.Focus();
                        return;
                    }
                    dvevent.DtActive = dtActive.Value;
                    dvevent.Type = "weekly";
                    string strweekday = (chSun.Checked ? "Sunday," : "") + (chMonday.Checked ? "Monday," : "") + (chTue.Checked ? "Tuesday," : "") + (chWed.Checked ? "Wednesday," : "") + (chThu.Checked ? "Thursday," : "") + (chFri.Checked ? "Friday," : "") + (chSat.Checked ? "Saturday," : "");
                    if (string.IsNullOrEmpty(strweekday))
                    {
                        MessageBox.Show("Vui lòng chọn ít nhất một ngày trong tuần");
                        panelweekday.Focus();
                        return;
                    }
                    dvevent.Weekday = strweekday.Remove(strweekday.Length - 1, 1);
                    DateTime dttemp = new DateTime(dtActive.Value.Year, dtActive.Value.Month, dtActive.Value.Day, dtTrigger.Value.Hour, dtTrigger.Value.Minute, dtTrigger.Value.Second);
                    // assume it just run the day before the active day and reset nextrun
                    
                    dvevent.SetFirstRun(dttemp);
                }
                if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("tubu") == false)
                {
                    return;
                }
                dvevent.InsertNew();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                //LogService.WriteError("Save Event", ex.ToString());
                MessageBox.Show("Save Error", ex.ToString());
            }
        }
    }
}
