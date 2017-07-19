using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using RecloserAcq.Device;
using TcpComm;

namespace RecloserAcq
{
    public partial class frmRequest : Form
    {
        public Request CurrentRequest 
        { 
            get { return currentRequestSource.DataSource as Request;}
            set { currentRequestSource.DataSource = value;}
        }

        public BindingList<Request> DataSource 
        {
            get { return requestBindingSource.DataSource as BindingList<Request>;}
            set { requestBindingSource.DataSource=value;} 
        }

        public frmRequest()
        {
            InitializeComponent();

            deviceTypeComboBox.DataSource = Enum.GetNames(typeof(eDeviceType));
        }

        private void requestUltraGrid_AfterRowActivate(object sender, EventArgs e)
        {
            //if(!DataSource.Contains(CurrentRequest))
            //{
            //    if (MessageBox.Show("Ban co muon them request hien tai vao danh sach", "Hoi", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        Save();
            //    }
            //}
            if (requestUltraGrid.ActiveRow != null && requestUltraGrid.ActiveRow.IsDataRow)
            {
                CurrentRequest = requestUltraGrid.ActiveRow.ListObject as Request;
            }
        }

        private void btNew_Click(object sender, EventArgs e)
        {
            CurrentRequest = new Request();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            btSave.Focus();
            Application.DoEvents();

            if (!Ultility.IsHexString(textTextBox.Text))
                MessageBox.Show("Invalid hex data. Please refine before saving.");
            else
                Save();
        }

        public void Save()
        {
            if (!DataSource.Contains(CurrentRequest))
            {
                DataSource.Add(CurrentRequest);
               
            }
            //Call save data
            RequestManager.Instance.SaveRequest();
        }

        private void frmRequest_Load(object sender, EventArgs e)
        {
            //Load Data

            DataSource = new BindingList<Request>(RequestManager.Instance.RequestList);
        }

        private void btCheck_Click(object sender, EventArgs e)
        {
            var s = textTextBox.Text;
            s = Regex.Replace(s, @"[^0-9a-fA-F\s]+", " ");
            s = Regex.Replace(s, @"[\r\n]", " ");
            s = Regex.Replace(s, @"\s{2,}", " ");
            textTextBox.Text = s;
                
        }

        private void textTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Ultility.IsHexString(textTextBox.Text))
            {
                var data = Ultility.FromHex(textTextBox.Text);
                txAscii.Text = Ultility.GetAsciiString(data);
            }
            else
                txAscii.Text = string.Empty;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
