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
    public partial class HistoryElster : Form
    {
        //private string _devicefile;
        List<Elster1700> _list;
        public HistoryElster(List<Elster1700> _list)
        {
            InitializeComponent();
            this._list = _list;
            //_devicefile = DeviceFile;
        }




        private void HistoryElster_Load(object sender, EventArgs e)
        {

            cboDevice.DataSource = _list;//DeviceStatic.GetDevices(_devicefile).OfType<Elster>().ToList<Elster>();//Program.GetDevices().OfType<TuBu>();
            //List<TuBu> lst = 

            cboDevice.DisplayMember = "Name";
            cboDevice.ValueMember = "Id";
            dteFrom.Value = DateTime.Now.AddDays(-1);
            dteTo.Value = DateTime.Now;
            grdResults.AutoGenerateColumns = true;
        }

        private void btnExcel_Click_1(object sender, EventArgs e)
        {
            try
            {
                ExportGridToExcel(cboDevice.Text + "History", grdResults);
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Export Excel Elster ", ex.Message);
            }

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
            string strFields = "[Date]      \t	[Alert]      \t	[Volt_A]      \t	[Volt_B]      \t	[Volt_C]      \t	[Volt_Total]      \t	[Ample_A]      \t	[Ample_B]      \t	[Ample_C]      \t	[Ample_Total]      \t	[PF_A]      \t	[PF_B]      \t	[PF_C]      \t	[PF_Total]      \t	[ActivePower_A]      \t	[ActivePower_B]      \t	[ActivePower_C]      \t	[ActivePower_Total]      \t	[ReActivePower_A]      \t	[ReActivePower_B]      \t	[ReActivePower_C]      \t	[ReActivePower_Total]     ";
            string strvalues;
            StreamWriter fs = new StreamWriter(dlgSurveyExcel.FileName, true,Encoding.Unicode);
            fs.WriteLine(strFields);        
            foreach (DataGridViewRow row in grdSearchResult.Rows)
            {
                strvalues = row.Cells["DateRec"].Value.ToString() + " \t " + row.Cells["Alert"].Value.ToString() + " \t " +
                    row.Cells["Volt_A"].Value.ToString() + " \t "
                        + row.Cells["Volt_B"].Value.ToString() + " \t "
                        + row.Cells["Volt_C"].Value.ToString() + " \t "
                        + row.Cells["Volt_Total"].Value.ToString() + " \t "
                        + row.Cells["Ample_A"].Value.ToString() + " \t "
                        + row.Cells["Ample_B"].Value.ToString() + " \t "
                        + row.Cells["Ample_C"].Value.ToString() + " \t "
                        + row.Cells["Ample_Total"].Value.ToString() + " \t "
                        + row.Cells["PF_A"].Value.ToString() + " \t "
                        + row.Cells["PF_B"].Value.ToString() + " \t "
                        + row.Cells["PF_C"].Value.ToString() + " \t "
                        + row.Cells["PF_Total"].Value.ToString() + " \t "
                        + row.Cells["AP_A"].Value.ToString() + " \t "
                        + row.Cells["AP_B"].Value.ToString() + " \t "
                        + row.Cells["AP_C"].Value.ToString() + " \t "
                        + row.Cells["AP_Total"].Value.ToString() + " \t "
                        + row.Cells["ReAP_A"].Value.ToString() + " \t "
                        + row.Cells["ReAP_B"].Value.ToString() + " \t "
                        + row.Cells["ReAP_C"].Value.ToString() + " \t "
                        + row.Cells["ReAP_Total"].Value.ToString();

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
            grdResults.DataSource = DBController.Instance.SearchElster(id, from, to);
        }
    }
}
