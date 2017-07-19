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
    public partial class Recloser351RListCtrl : UserControl
    {
        private List<Recloser351R> _list;
        public Recloser351RListCtrl()
        {
            
            InitializeComponent();
            
//#if DEBUG 
//            btnTest.Visible = true;
//            btnTest2.Visible = true;
//#endif 
        }
        public void SetDataSource(List<Recloser351R> ds)  //IEnumerable<Recloser351R> ds)
        {
            this._list = ds;
            dgvBindingSource.DataSource = ds;
            dgv.Refresh();
            dgv.Update();
        }
        //public void SetDataSource1(IEnumerable<Recloser351R> ds)
        //{
        //    dgv.DataSource = ds;
        //    dgv.Refresh();
        //    dgv.Update();
        //}
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

            // type: tubu , nulec , cooper , 351 , elster
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("351") == false)
                return;
            Recloser351R rc = (Recloser351R)dgvBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Open") == true)
            {

                rc.CommandOpen(false);



            }
        }
       
        public DataGridViewRow GetRow(Recloser351R rc)
        {
            //dgv.Rows. Rows.Where(r => r.ListObject == recloser).FirstOrDefault();
            int searchValue = rc.Port;
            try
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if ((int)row.Cells["Port"].Value == searchValue)
                    {
                        return row;

                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("GetRow-------------------------------------------351", ex.Message);
            }
            return null;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            // type: tubu , nulec , cooper , 351 , elster
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("351") == false)
                return;
            Recloser351R rc = (Recloser351R)dgvBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Close") == true)
            {

                rc.CommandClose(false);



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
        private void btnSynTime_Click(object sender, EventArgs e)
        {
            Recloser351R rc = (Recloser351R)dgvBindingSource.Current;
            //rc.Listener.Restart();
            //System.Threading.Thread.Sleep(1000);  
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("351") == false)
                return;


            if (Validate_SendCommand(rc.Location, "Syn Time") == true)
            {

                rc.SetTime();

            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Recloser351R rc = (Recloser351R)dgvBindingSource.Current;
            //rc.Listener.Restart();
            //System.Threading.Thread.Sleep(1000);  
            


            

            rc.sendConnectCommand();

            //LogService.WriteInfo("acceptSendingRequest=" + rc.acceptSendingRequest, "");
        }

        private void Recloser351RListCtrl_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            Recloser351R rc = (Recloser351R)dgvBindingSource.Current;
            //rc.Listener.Restart();
            //System.Threading.Thread.Sleep(1000);  





            rc.sendTestMET();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                RecloserBase rc = (RecloserBase)dgvBindingSource.Current;

                rc.resetModem();

            }
            catch (Exception )
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HistoryRecloserSel frm = new HistoryRecloserSel(_list);
            frm.Show();
        }

        private void btnEnableAlertCooper_Click(object sender, EventArgs e)
        {
            RecloserBase rc = (RecloserBase)dgvBindingSource.Current;
            rc.DisableAlert = false;
        }

        private void btnDAlertCooper_Click(object sender, EventArgs e)
        {
            RecloserBase rc = (RecloserBase)dgvBindingSource.Current;
            rc.DisableAlert = true;
        }
    }
}
