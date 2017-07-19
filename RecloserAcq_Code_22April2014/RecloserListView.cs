using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using RecloserAcq.Device;

namespace RecloserAcq
{
    public partial class RecloserListView : UserControl
    {
        public List<RecloserBase> DataSource
        {
            get 
            { 
                return recloserBaseBindingSource.DataSource as List<RecloserBase>; 
            }

            set 
            {
                if (value != null)
                {
                    recloserBaseBindingSource.DataSource = value;
                }
            }
        }

        public RecloserListView()
        {
            InitializeComponent();
        }
        public void UpdateGrid()
        {
            ultraGrid1.Update();
            ultraGrid1.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            var vl = new ValueList();
            vl.ValueListItems.Add(eDeviceType.CooperFxb, "Cooper");
            vl.ValueListItems.Add(eDeviceType.Nulec, "Nulec");
            vl.ValueListItems.Add(eDeviceType.TuBu, "TuBu");
            var band = e.Layout.Bands[0];
            band.Columns["DeviceType"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            band.Columns["DeviceType"].ValueList = vl;
        }

        private void ultraGrid1_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key == "AlertFile")
            {
                var rc = e.Cell.Row.ListObject as RecloserBase;
                if (rc == null) return;

                var dlg = new OpenFileDialog();
                dlg.DefaultExt = "wav";
                dlg.Filter = "Wav files (*.wav)|*.wav";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    rc.AlertFile = dlg.FileName;
                    e.Cell.Refresh();
                }
            }
        }
        public void DeleteSelected()
        {
            
            RecloserBase rc = (RecloserBase)recloserBaseBindingSource.Current;
            rc.DateDeleted = DateTime.MinValue;
            rc.SaveDevice();
            ultraGrid1.DeleteSelectedRows(true);// ultraGrid1.Selected
            
        }
    }
}
