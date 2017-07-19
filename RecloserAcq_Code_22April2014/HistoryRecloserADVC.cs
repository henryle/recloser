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
    public partial class HistoryRecloserADVC : Form
    {
        //private string _devicefile;
        List<RecloserADVC> _list;
        public HistoryRecloserADVC(List<RecloserADVC> _list)
        {
            InitializeComponent();
            this._list = _list;
            //_devicefile = DeviceFile;
        }




        private void HistoryRecloserADVC_Load(object sender, EventArgs e)
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
                FA_Accounting.Common.LogService.WriteError("Export Excel RecloserADVC ", ex.Message);
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
            //Application.DoEvents();
            string strFields = "[DeviceId]      ,	[Date]      ,	[Alert]      " +
               " ,  [Current_IA]   ,  	[Current_IB]   ,  	[Current_IC]   ,  	[Current_IE]   ,  	[Apparent_S_A]   ,  " +
    " [Apparent_S_B]   ,  	[Apparent_S_C]   ,  	[Real_P_A]   ,  	[Real_P_B]   ,  	[Real_P_C]   ,  " +
    " [Reactive_Q_A]   ,  	[Reactive_Q_B]   ,  	[Reactive_Q_C]   ,  	[Cosphi_A]   ,  	[Cosphi_B]   , " +
    " [Cosphi_C]   ,  	[Total]   ,  	[Forward]   ,  	[Reverse]   ,  	[BatterryVol]   ,  " +
    " [VA_GND_src]   ,  	[VB_GND_src]   ,  	[VC_GND_src]   ,  	[VA_GND_load]   ,  	[VB_GND_load]   ,  " +
    " [VC_GND_load]   ,  	[VA_B_src]   ,  	[VB_C_src]   ,  	[VC_A_src]   ,  	[VA_B_load]   ,  " +
    " [VB_C_load]   ,  	[VC_A_load]    \r\n";
            string strvalues;
            //using (StreamWriter fs = new StreamWriter(dlgSurveyExcel.FileName, true, Encoding.Unicode))
            using (FileStream fs = File.Open(dlgSurveyExcel.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] FieldLine = Encoding.ASCII.GetBytes(strFields);

                fs.Write(FieldLine, 0, FieldLine.Length);





                foreach (DataGridViewRow row in grdSearchResult.Rows)
                {

                    strvalues = row.Cells["Id"].Value.ToString() + " , " + row.Cells["DateRec"].Value.ToString() + " , " + row.Cells["Alert"].Value.ToString() + " , " +
                        row.Cells["Current_IA"].Value.ToString() + " , "
                        + row.Cells["Current_IB"].Value.ToString() + " , "
                        + row.Cells["Current_IC"].Value.ToString() + " , "
                        + row.Cells["Current_IE"].Value.ToString() + " , "
                        + row.Cells["Apparent_S_A"].Value.ToString() + " , "
                        + row.Cells["Apparent_S_B"].Value.ToString() + " , "
                        + row.Cells["Apparent_S_C"].Value.ToString() + " , "
                        + row.Cells["Real_P_A"].Value.ToString() + " , "
                        + row.Cells["Real_P_B"].Value.ToString() + " , "
                        + row.Cells["Real_P_C"].Value.ToString() + " , "
                        + row.Cells["Reactive_Q_A"].Value.ToString() + " , "
                        + row.Cells["Reactive_Q_B"].Value.ToString() + " , "
                        + row.Cells["Reactive_Q_C"].Value.ToString() + " , "
                        + row.Cells["Cosphi_A"].Value.ToString() + " , "
                        + row.Cells["Cosphi_B"].Value.ToString() + " , "
                        + row.Cells["Cosphi_C"].Value.ToString() + " , "
                        + row.Cells["Total"].Value.ToString() + " , "
                        + row.Cells["Forward"].Value.ToString() + " , "
                        + row.Cells["Reverse"].Value.ToString() + " , "
                        + row.Cells["BatterryVol"].Value.ToString() + " , "
                        + row.Cells["VA_GND_src"].Value.ToString() + " , "
                        + row.Cells["VB_GND_src"].Value.ToString() + " , "
                        + row.Cells["VC_GND_src"].Value.ToString() + " , "
                        + row.Cells["VA_GND_load"].Value.ToString() + " , "
                        + row.Cells["VB_GND_load"].Value.ToString() + " , "
                        + row.Cells["VC_GND_load"].Value.ToString() + " , "
                        + row.Cells["VA_B_src"].Value.ToString() + " , "
                        + row.Cells["VB_C_src"].Value.ToString() + " , "
                        + row.Cells["VC_A_src"].Value.ToString() + " , "
                        + row.Cells["VA_B_load"].Value.ToString() + " , "
                        + row.Cells["VB_C_load"].Value.ToString() + " , "
                        + row.Cells["VC_A_load"].Value.ToString() + " \r\n";

                    //fs.WriteLine(strvalues);
                    byte[] ValueLine = Encoding.ASCII.GetBytes(strvalues);
                    fs.Write(ValueLine, 0, ValueLine.Length);


                }
                fs.Flush();
                fs.Close();
            }
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
            grdResults.DataSource = DBController.Instance.SearchRecloserVP(id, from, to);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            int i = 0;
            while(i<grdResults.Rows.Count)
            {
                DataGridViewRow row = grdResults.Rows[i];
                decimal dmax = Math.Max(Math.Max(Convert.ToDecimal(row.Cells["current_IA"].Value), Convert.ToDecimal(row.Cells["current_IB"].Value)), Convert.ToDecimal(row.Cells["current_IC"].Value));
                decimal dmin = Math.Min(Math.Min(Convert.ToDecimal(row.Cells["current_IA"].Value), Convert.ToDecimal(row.Cells["current_IB"].Value)), Convert.ToDecimal(row.Cells["current_IC"].Value));

                if ((dmax - dmin) / dmax <= (decimal)0.1)
                {
                    grdResults.Rows.Remove(row);
                }
                else
                {
                    i++;
                }
            }
            //grdResults.Refresh();
        }
    }
}
