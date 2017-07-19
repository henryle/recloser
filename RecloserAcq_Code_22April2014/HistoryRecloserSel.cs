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
    public partial class HistoryRecloserSel : Form
    {
        //private string _devicefile;
        List<Recloser351R> _list;
        public HistoryRecloserSel(List<Recloser351R> _list)
        {
            InitializeComponent();
            this._list = _list;
            //_devicefile = DeviceFile;
        }




        private void HistoryRecloserSel_Load(object sender, EventArgs e)
        {

            cboDevice.DataSource = _list;//DeviceStatic.GetDevices(_devicefile).OfType<Recloser351R>().ToList<Recloser351R>();//Program.GetDevices().OfType<TuBu>();
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
                FA_Accounting.Common.LogService.WriteError("Export Excel Recloser351 ", ex.Message);
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
            string strFields =  "Date" + "\t" + "Alert" + "\t" + "MW_A" + "\t" + "MW_B" + "\t" + "MW_C" + "\t" + "MW_3P" + "\t" + "Q_MVAR_A" + "\t" + "Q_MVAR_B" + "\t" + "Q_MVAR_C" + "\t" + "Q_MVAR_3P" + "\t" + "PF_A" + "\t" + "PF_B" + "\t" + "PF_C" + "\t" + "PF_3P" + "\t" + "voltsValue_MAG_A" + "\t" + "voltsValue_MAG_B" + "\t" + "voltsValue_MAG_C" + "\t" + "voltsValue_MAG_S" + "\t" + "vang_A" + "\t" + "vang_B" + "\t" + "vang_C" + "\t" + "vang_S" + "\t" + "imag_A" + "\t" + "imag_B" + "\t" + "imag_N" + "\t" + "imag_G" + "\t" + "imag_C";
            string strvalues;
            StreamWriter fs = new StreamWriter(dlgSurveyExcel.FileName, true, Encoding.Unicode);
            fs.WriteLine(strFields);
            foreach (DataGridViewRow row in grdSearchResult.Rows)
            {
                strvalues = row.Cells["DateRec"].Value.ToString() + " \t " + row.Cells["Alert"].Value.ToString() + " \t " + row.Cells["MW_A"].Value.ToString() + " \t " + row.Cells["MW_B"].Value.ToString() + " \t " + row.Cells["MW_C"].Value.ToString() + " \t " + row.Cells["MW_3P"].Value.ToString() + " \t " + row.Cells["Q_MVAR_A"].Value.ToString() + " \t " + row.Cells["Q_MVAR_B"].Value.ToString() + " \t " + row.Cells["Q_MVAR_C"].Value.ToString() + " \t " + row.Cells["Q_MVAR_3P"].Value.ToString() + " \t " + row.Cells["PF_A"].Value.ToString() + " \t " + row.Cells["PF_B"].Value.ToString() + " \t " + row.Cells["PF_C"].Value.ToString() + " \t " + row.Cells["PF_3P"].Value.ToString() + " \t " + row.Cells["voltsValue_MAG_A"].Value.ToString() + " \t " + row.Cells["voltsValue_MAG_B"].Value.ToString() + " \t " + row.Cells["voltsValue_MAG_C"].Value.ToString() + " \t " + row.Cells["voltsValue_MAG_S"].Value.ToString() + " \t " + row.Cells["vang_A"].Value.ToString() + " \t " + row.Cells["vang_B"].Value.ToString() + " \t " + row.Cells["vang_C"].Value.ToString() + " \t " + row.Cells["vang_S"].Value.ToString() + " \t " + row.Cells["imag_A"].Value.ToString() + " \t " + row.Cells["imag_B"].Value.ToString() + " \t " + row.Cells["imag_N"].Value.ToString() + " \t " + row.Cells["imag_G"].Value.ToString() + " \t " + row.Cells["imag_C"].Value.ToString();

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
            grdResults.DataSource = DBController.Instance.SearchRecloser351(id, from, to);
        }
    }
}
