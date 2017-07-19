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
    public partial class Elster1700Ctrl : UserControl
    {
        private List<Elster1700> _list;
        public Elster1700Ctrl()
        {
            
            InitializeComponent();
            dgv.AutoGenerateColumns = false;
        }
        public void SetDataSource(List<Elster1700> ds)
        {
            this._list = ds;
            dgvBindingSource.DataSource = ds;
            dgv.Refresh();
            dgv.Update();
        }
        public void Refresh(RecloserBase rc)
        {
            //dgv.Refresh();
            //var row = dgv.Rows.Where(r => r.ListObject == rc).FirstOrDefault();
            //if (row != null)
            //    row.Refresh();
            var row = dgv.Rows.Cast<DataGridViewRow>().Where(r => r.DataBoundItem == rc).FirstOrDefault();
            if (row != null)
            {    //row.Refresh();
                dgv.InvalidateRow(row.Index);//myDataGridView.CurrentRow.Index)
                
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("elster") == false)
                return;
            Elster1700 rc = (Elster1700)dgvBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Open") == true)
            {

                rc.CommandOpen(false);



            }
        }
        private bool Validate_SendCommand(string devicename, string command)
        {
            //require password
            if (System.Windows.Forms.MessageBox.Show("Bạn có chắc bạn muốn " + command + " " + devicename + "?", "Xac nhan", System.Windows.Forms.MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("elster") == false)
                return;
            Elster1700 rc = (Elster1700)dgvBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Close") == true)
            {

                rc.CommandClose(false);
                


            }
        }
        
        private void btnSynTime_Click(object sender, EventArgs e)
        {
            Elster1700 rc = (Elster1700)dgvBindingSource.Current;
            //rc.Listener.Restart();
            //System.Threading.Thread.Sleep(1000);  
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("elster") == false)
                return;


            if (Validate_SendCommand(rc.Location, "Syn Time") == true)
            {

                rc.SetTime();

            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            HistoryElster frm = new HistoryElster(_list);
            frm.Show();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

            Elster1700 rc = (Elster1700)dgvBindingSource.Current;
            //if (rc.Listener.IsConnected)
            //{
            //    rc.LogOff();
            //    rc.Listener.StopListening();
                
            //    return;
            //}
            //rc.Listener.StartListening();
            if (rc.Listener.IsConnected)
            {
                rc.CommStatus = "Connected";
            }
            //byte[] b1 = new byte[] { 0x2F, 0x3F, 0x30, 0x30, 0x31, 0x21, 0x0D, 0x0A};
            //byte[] b2 = new byte[] { 0x06, 0x30, 0x35, 0x31, 0x0D, 0x0A };
            //rc.Listener.Send(b1);
            //System.Threading.Thread.Sleep(1000);
            //rc.Listener.Send(b2);
            rc.sendConnectCommand();
            //rc.IfAcceptConnectIsTrue();
            //rc.IfGetSeed();
            //rc.IfSendPasswordOK();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Elster1700 rc = (Elster1700)dgvBindingSource.Current;
            if (rc.Listener.IsConnected)
            {
                rc.LogOff();
                rc.Listener.StopListening();

                return;
            }
            rc.Listener.StartListening();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Elster1700 rc = (Elster1700)dgvBindingSource.Current;
            if (rc.Listener.IsConnected)
            {
                rc.LogOff();
                rc.Listener.StopListening();

                return;
            }
        }

        private void Elster1700Ctrl_Load(object sender, EventArgs e)
        {
            //dgv.AutoGenerateColumns = false;
        }
    }
}
