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
    public partial class Form1 : Form
    {
        
        private List<System.Windows.Forms.TabPage> tabPagelist = new List<TabPage>();
        private List<MapTuBuCtrl> mapTuBuCtrlList = new List<MapTuBuCtrl>();
        
        private List<RecloserBase> _list;
        private string _deviceFile;
        
        public Form1(List<RecloserBase> list,string deviceFile)
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            _deviceFile = deviceFile;
            this.tabControl1.SuspendLayout();

            this.SuspendLayout();
            TabPage tabpagetmp;
                        
            _list = list;
            InitializeComponent();
            this.tabControl1.SuspendLayout();
            int tabindex = 0;
            foreach (RecloserBase rc in _list)
            {
                if (rc.DeviceType != eDeviceType.TuBu )
                {

                    
                    tabpagetmp = new System.Windows.Forms.TabPage();
                    //tabPagelist.Add(tabpagetmp);
                    tabpagetmp.SuspendLayout();
                    MapTuBuCtrl maptubutmp;
                    maptubutmp = new RecloserAcq.MapTuBuCtrl(rc,_list.OfType<TuBu>().ToList<RecloserBase>());
                    //mapTuBuCtrlList.Add(maptubutmp);
                    tabpagetmp.Controls.Add(maptubutmp);
                    tabpagetmp.Location = new System.Drawing.Point(4, 22);
                    tabpagetmp.Name = "tabPage" + tabindex.ToString();
                    tabpagetmp.Padding = new System.Windows.Forms.Padding(3);
                    tabpagetmp.Size = new System.Drawing.Size(543, 342);
                    tabpagetmp.TabIndex = tabindex;
                    tabpagetmp.Text = rc.Name;
                    tabpagetmp.UseVisualStyleBackColor = true;
                    tabindex++;
                    this.tabControl1.Controls.Add(tabpagetmp);
                    tabpagetmp.ResumeLayout(false);        
                    
                }
            }
            
            this.tabControl1.Location = new System.Drawing.Point(12, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(551, 368);
            this.tabControl1.TabIndex = 0;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 410);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Đóng cắt tự động";
            tabControl1.ResumeLayout(false);
            
            this.ResumeLayout(false);
            
            
            // 
            // tabPage1
            // 
            
            
            // 
            // mapTuBuCtrl1
            // 
            //this.mapTuBuCtrl1.Location = new System.Drawing.Point(6, 6);
            //this.mapTuBuCtrl1.Name = "mapTuBuCtrl1";
            //this.mapTuBuCtrl1.Size = new System.Drawing.Size(522, 321);
            //this.mapTuBuCtrl1.TabIndex = 0;

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (TabPage p in this.tabControl1.TabPages)
            {
                foreach (Control ctr in p.Controls)
                {
                    if (ctr.GetType() == typeof(MapTuBuCtrl))
                    {
                        ((MapTuBuCtrl)ctr).Ret();

                    }
                }
            }
            //Program.SaveDevices(this._list);
            DeviceStatic.SaveDevices(this._list, _deviceFile);                
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
