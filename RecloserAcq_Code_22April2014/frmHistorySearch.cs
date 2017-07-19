using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using RecloserAcq.Device;
using RecloserAcq.OracleDAL;
namespace RecloserAcq
{
    public partial class frmHistorySearch : Form
    {
        //private string _DeviceFile;
        List<RecloserBase> _list;
        
        public frmHistorySearch(List<RecloserBase> list)
        {
            InitializeComponent();
            _list = list;
        }
        private void frmHistorySearch_Load(object sender, EventArgs e)
        {
            
            cboDevice.DataSource = _list;
           // cboDevice.DisplayMember = "Name";
            cboDevice.ValueMember = "ID";
            
            dteFrom.Value = DateTime.Now.AddHours(-1);
            dteTo.Value = DateTime.Now;
            for (int i = 0; i < cboDevice.Rows.Band.Columns.Count; i++)
            {
                cboDevice.Rows.Band.Columns[i].Hidden = true;
            }
            cboDevice.Rows.Band.Columns["Location"].Hidden = false;
            cboDevice.Rows.Band.Columns["Name"].Hidden = false;     
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            var id = cboDevice.Value;
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
            grdResults.DataSource = DBController.Instance.Search(id, from, to);
            var band =grdResults.DisplayLayout.Bands[0];
            try
            {
                band.Columns["ID"].Hidden = true;
            }
            catch(Exception ex)
            {
                LogService.WriteError("Search NulecCooper", ex.ToString());
            }
            LogService.WriteInfo("Column", band.Columns[0].ToString() + " " + band.Columns[1].ToString() + " " + band.Columns[2].ToString());
            band.Columns["DateRec"].MaskInput = "dd/mm/yyyy hh:mm:ss";
            band.Columns["DateRec"].Width = 120;
            
            var device = cboDevice.SelectedRow.ListObject as RecloserAcq.Device.RecloserBase;
            //band.Columns["Counterclose"].Header.Caption = "Counter";
            if (device.DeviceType == Device.eDeviceType.CooperFxb)
            {
                band.Columns["Field1"].Header.Caption = "Amp12";
                
                band.Columns["Field2"].Header.Caption = "Amp34";
                band.Columns["Field3"].Header.Caption = "Amp56";
                band.Columns["Field4"].Header.Caption = "Earth";
                band.Columns["Field5"].Header.Caption = "Operations";
                band.Columns["Field6"].Header.Caption = "Batery Unload";
                band.Columns["Field7"].Header.Caption = "Batery Load";
                band.Columns["Field8"].Header.Caption = "Open";
                band.Columns["Field9"].Header.Caption = "Close";
                band.Columns["Field10"].Header.Caption = "Lockout";
                band.Columns["Field11"].Header.Caption = "Target12";
                band.Columns["Field12"].Header.Caption = "Target34";
                band.Columns["Field13"].Header.Caption = "Target56";
                band.Columns["Field14"].Header.Caption = "TargetEarth";
                band.Columns["Field15"].Header.Caption = "Status 12";
                band.Columns["Field16"].Header.Caption = "Status 34";
                band.Columns["Field17"].Header.Caption = "Status 56";
                band.Columns["Field18"].Header.Caption = "Status Earth";
            }
            else if (device.DeviceType == Device.eDeviceType.Nulec)
            {
                band.Columns["Field1"].Header.Caption = "Phase A";
                band.Columns["Field2"].Header.Caption = "Phase B";
                
                band.Columns["Field3"].Header.Caption = "Phase C";
                band.Columns["Field4"].Header.Caption = "Earth";
                band.Columns["Field5"].Header.Caption = "Operations";
                band.Columns["Field6"].Header.Caption = "Batery";
                band.Columns["Field7"].Header.Caption = "Batery V";
                band.Columns["Field8"].Header.Caption = "Open";
                band.Columns["Field9"].Hidden = true;
                band.Columns["Field10"].Hidden = true;
                band.Columns["Field11"].Header.Caption = "Apparent Power";
                band.Columns["Field12"].Header.Caption = "Reactive Power";
                band.Columns["Field13"].Header.Caption = "Real Power";
                band.Columns["Field14"].Header.Caption = "Power Factor";
                band.Columns["Field15"].Header.Caption = "Device Time";
                band.Columns["Field16"].Hidden = true;
                band.Columns["Field17"].Hidden = true;
                band.Columns["Field18"].Hidden = true;
            }
        }

        public void ExportGridToExcel(string ExportingFileName, UltraGrid grdSearchResult)
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
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = ExportingFileName + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");
            dlgSurveyExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dlgResSaveFile = dlgSurveyExcel.ShowDialog();
            if (dlgResSaveFile == DialogResult.Cancel)
            {
                return;
            }
            Application.DoEvents();
            UltraGridExcelExporter GridToToExcel = new UltraGridExcelExporter();
            GridToToExcel.FileLimitBehaviour = FileLimitBehaviour.TruncateData;
            GridToToExcel.InitializeColumn += new InitializeColumnEventHandler(GridToToExcel_InitializeColumn);
            GridToToExcel.Export(grdSearchResult, dlgSurveyExcel.FileName);
        }
        void GridToToExcel_InitializeColumn(object sender, InitializeColumnEventArgs e)
        {
            try
            {
                if (e.Column.DataType == typeof(System.DateTime?) && e.Column.Format != null)
                {
                    e.ExcelFormatStr = e.Column.Format.Replace("tt", "AM/PM");
                }
                else
                {
                    e.ExcelFormatStr = e.Column.Format;
                }
            }
            catch (Exception )
            {
                //Abaki.Common.Ultility.LogService.Logge
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

            try
            {
                ExportGridToExcel(cboDevice.Text + "History", grdResults);
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Export Excel frmHistorySearch ", ex.Message);
            }

        }
    }
}
