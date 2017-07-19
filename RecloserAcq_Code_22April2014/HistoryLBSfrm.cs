using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecloserAcq.Device;
using System.IO;
using RecloserAcq.OracleDAL;
namespace RecloserAcq
{
    public partial class HistoryLBSfrm : Form
    {
        private string _devicefile  = string.Empty;
        
        public HistoryLBSfrm(string DeviceFile)
        {
            InitializeComponent();
            _devicefile = DeviceFile;
        }
        List<LBS> _list;
        public HistoryLBSfrm(List<LBS> list)
        {
            InitializeComponent();
            _list = list;
        }

       

        private void HistoryLBSfrm_Load(object sender, EventArgs e)
        {
            
            
            cboDevice.DataSource = _list;
            

            cboDevice.DisplayMember = "Name";
            cboDevice.ValueMember = "Id";
            dteFrom.Value = DateTime.Now.AddDays(-1);
            dteTo.Value = DateTime.Now;
            grdResults.AutoGenerateColumns = true;
        }

        private void btnExcel_Click_1(object sender, EventArgs e)
        {
            ExportGridToExcel(cboDevice.Text + "History", grdResults);
        }
        public void ExportGridToExcel(string ExportingFileName, DataGridView grdSearchResult)
        {
            if (grdSearchResult.Rows.Count <= 0)
            {
                MessageBox.Show("Grid has no records to export.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (grdSearchResult.Rows.Count >= 65536)
            {
                DialogResult dlgRes = MessageBox.Show(string.Format("The grid contains more data then Excel can handle, only the first {0} rows will be exported.  Do you want to continue?", 65536), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dlgRes != DialogResult.Yes)
                {
                    return;
                }
            }
            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel csv (*.csv)|.csv";
            dlgSurveyExcel.FileName = ExportingFileName + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");
            dlgSurveyExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dlgResSaveFile = dlgSurveyExcel.ShowDialog();
            if (dlgResSaveFile == DialogResult.Cancel)
            {
                return;
            }
            Application.DoEvents();
            string strFields = "DeviceId \t Name \t Location \t Opt  \t OperationTime  ";
            string strvalues;
            StreamWriter fs = new StreamWriter(dlgSurveyExcel.FileName, true,Encoding.Unicode);
            fs.WriteLine(strFields);        
            foreach (DataRow row in grdSearchResult.Rows)
            {
                strvalues = row["DeviceId"].ToString() + " \t " + row["Name"].ToString() + " \t " + row["Location"].ToString() + " \t " + row["Opt"].ToString() + " \t " + row["OperationTime"].ToString() ;
                fs.WriteLine(strvalues);

            }
            fs.Flush();
            fs.Close();
            //UltraGridExcelExporter GridToToExcel = new UltraGridExcelExporter();
            //GridToToExcel.FileLimitBehaviour = FileLimitBehaviour.TruncateData;
            //GridToToExcel.InitializeColumn += new InitializeColumnEventHandler(GridToToExcel_InitializeColumn);
            //GridToToExcel.Export(grdSearchResult, dlgSurveyExcel.FileName);
        }

        private void btnFind_Click_1(object sender, EventArgs e)
        {
            var id = cboDevice.SelectedValue;
            var from = dteFrom.Value;
            var to = dteTo.Value;
            if (id == null)
            {
                MessageBox.Show("Please select device to find");
                return;
            }
            if (from == null)
            {
                MessageBox.Show("Please select from date time");
                return;
            }
            if (to == null)
            {
                MessageBox.Show("Please select to date time");
                return;
            }
            grdResults.DataSource = DBController.Instance.SearchLBS(id, from, to);
        }
    }
}
