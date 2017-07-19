
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecloserAcq.Device;
using RecloserAcq.Properties;
using TcpComm;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using System.Reflection;
using System.Media;
using System.IO;
using RecloserAcq.OracleDAL;
using System.Xml.Serialization;
using System.Timers;
using System.Runtime.InteropServices;
namespace RecloserAcq
{
    public partial class frmDeviceStatus : Form
    {
        #region Members
        Settings _setting = RecloserAcq.Properties.Settings.Default;
        public string _DeviceFile = String.Empty;
        public int GroupID = -1;
        private List<RecloserBase> _list;
        private DateTime? _lastPollTime;
       
        

        SoundPlayer _sndPlayer;        
        private bool _isPlaying = false;

        Queue<RecloserBase> _alertQueue;

        private Action<RecloserBase> _saveRecloserData;
        private Action<byte[], string, bool> _addCommLog;
        private Action<DateTime, int, string> _saveCommData;
        
        private Func<bool> _flash;
        public bool AutoStartPoll { get; set; }

        #endregion
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;

        // Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        // Do the flashing - this does not involve a raincoat.
        public bool FlashWindow()
        {
            
            IntPtr hWnd = this.Handle;
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }
        #region Ctor()
        public frmDeviceStatus(string devicefilepathxml)
        {
            InitializeComponent();
            _DeviceFile = devicefilepathxml;
            _saveRecloserData = new Action<RecloserBase>(SaveData);
            _addCommLog = new Action<byte[], string, bool>(AddCommLog);
            _flash = new Func<bool>(FlashWindow);
            _saveCommData = new Action<DateTime, int, string>(SaveCommData);
            
            _list = new List<RecloserBase>();
            _lastPollTime = null;
            _sndPlayer = new SoundPlayer();
            _alertQueue = new Queue<RecloserBase>();
            //_saveCounter = 0;
            //fileconfig = "formconfig" + this.GroupID.ToString() + ".xml";
            //_formprop = PropertyFormFunc.getObj(fileconfig);
            //timerSaveHistory.Interval = _setting.SaveDataInterval * 1000;
            this.AutoStartPoll = false;
            splitAdvcandNew.Orientation = Orientation.Horizontal;
            splitContainer1.Orientation = Orientation.Horizontal;
            splitContainer2.Orientation = Orientation.Horizontal;
            splitContainer3.Orientation = Orientation.Horizontal;
            splitContainer6.Orientation = Orientation.Horizontal;
            splitCooperNulec.Orientation = Orientation.Horizontal;
            splitTuBuRecloserSel.Orientation = Orientation.Horizontal;
            splitElsterAndNew.Orientation = Orientation.Vertical;
            
            grdCooper.Dock = DockStyle.Fill;
            commLogCtrl1.Dock = DockStyle.Fill;
            grdNulec.Dock = DockStyle.Fill;
            grdTuBu.Dock = DockStyle.Fill;
            grdLBS.Dock = DockStyle.Fill;
            elster1700Ctrl1.Dock = DockStyle.Fill;
            recloser351RListCtrl1.Dock = DockStyle.Fill;
            dgvrecloserAdvc.Dock = DockStyle.Fill;
        }
        public frmDeviceStatus(int groupid)
        {
            InitializeComponent();
            GroupID = groupid;
            _saveRecloserData = new Action<RecloserBase>(SaveData);
            _addCommLog = new Action<byte[], string, bool>(AddCommLog);
            _saveCommData = new Action<DateTime, int, string>(SaveCommData);
            _flash = new Func<bool>(FlashWindow);
            _list = new List<RecloserBase>();
            _lastPollTime = null;
            _sndPlayer = new SoundPlayer();
            _alertQueue = new Queue<RecloserBase>();
            //_saveCounter = 0;
            //fileconfig = "formconfig" + this.GroupID.ToString() + ".xml";
            //_formprop = PropertyFormFunc.getObj(fileconfig);
            //timerSaveHistory.Interval = _setting.SaveDataInterval * 1000;
            this.AutoStartPoll = false;
            splitAdvcandNew.Orientation = Orientation.Horizontal;
            splitContainer1.Orientation = Orientation.Horizontal;
            splitContainer2.Orientation = Orientation.Horizontal;
            splitContainer3.Orientation = Orientation.Horizontal;
            splitContainer6.Orientation = Orientation.Horizontal;
            splitCooperNulec.Orientation = Orientation.Horizontal;
            splitElsterAndNew.Orientation = Orientation.Horizontal;
            splitTuBuRecloserSel.Orientation = Orientation.Horizontal;
            grdCooper.Dock = DockStyle.Fill;
            commLogCtrl1.Dock = DockStyle.Fill;
            grdNulec.Dock = DockStyle.Fill;
            grdTuBu.Dock = DockStyle.Fill;
            grdLBS.Dock = DockStyle.Fill;
            elster1700Ctrl1.Dock = DockStyle.Fill;
            recloser351RListCtrl1.Dock = DockStyle.Fill;
            dgvrecloserAdvc.Dock = DockStyle.Fill;
            
        } 
        #endregion
        public bool testNzero = false;
        public bool testCzero = false;
        public bool testNmax = false;
        public bool testCmax = false;
        #region Event handlers
        System.Timers.Timer tmplay; 
        private void frmDeviceStatus_Load(object sender, EventArgs e)
        {
            SetBtnPermission();
            tmplay = new System.Timers.Timer(3000);

            tmplay.Elapsed += new ElapsedEventHandler(OnTimePlayEvent);
            tmplay.Enabled = false;
            
            //ValidatePassword vp = new ValidatePassword(false);

            //if (vp.ShowDialog() == DialogResult.Cancel)
            //{
            //    this.Close();
            //    return;
            //}
            //fileconfig = "formconfig" +  this.GroupID.ToString() + ".xml";

            if (this.GroupID != 0)// 0 is all group
            {
                this.dgvrecloserAdvc.DisplayLayout.Bands[0].Columns["Location"].Hidden = true;
            }
            LoadDevices();

            // Update GUI
            UpdatePollStatus();           
//#if DEBUG
//            this.btntestC.Visible = true;
//            this.btntestN.Visible = true;
//            this.btntestCMax.Visible = true;
//            this.btntestNMax.Visible = true;
//            this.btnCo5.Visible = true;
//            this.btnN5.Visible = true;
//            btRestart.Visible = true;
//#endif
        }

        private void LoadDevices()
        {
            // Load alert setting for Recloser
            //RecloserBase.MaxAmp = _setting.MaxCurrent;
            //RecloserBase.MaxDuration = _setting.CurrentDuration;

            // Filtering unused devices
            if (!String.IsNullOrEmpty(_DeviceFile))
            {
                _list = DeviceStatic.GetDevices(_DeviceFile).Where(r => r.Enable).ToList();
            }
            else
            {
                _list = DeviceStatic.GetDevices(GroupID).Where(r => r.Enable).ToList();
            }
            //_list = De .GetDevices(_DeviceFile).Where(r => r.Enable).ToList();
            foreach (var cp in _list)
            {
                cp.Listener.OnDataReceived += new EventHandler<DataTransferEventArg>(Listener_OnDataReceived);
                cp.Listener.OnDataSent += new EventHandler<DataTransferEventArg>(Listener_OnDataSent);
                cp.Listener.OnStatusChanged += new EventHandler<StatusChangedEventArgs>(Listener_OnStatusChanged);
                //cp.Listener.DoParse = Parser.DoParse;

                cp.PropertyChanged += new PropertyChangedEventHandler(Recloser_PropertyChanged);
                cp.OnDataUpdated += new EventHandler(Recloser_OnDataUpdated);
                cp.DevicePath = _setting.dvpath;
            }
            if (_list.OfType<CooperFXB>().Count() == 0)
            {
                //chkCooper.Checked = false;
                cooperToolStripMenuItem.Checked = false;
            }
            if (_list.OfType<Nulec>().Count() == 0 && _list.OfType<Nulec_U>().Count()==0)
            {
                //chkNulec.Checked = false;
                nulecToolStripMenuItem.Checked = false;
            }
            if (_list.OfType<TuBu>().Count() == 0)
            {
                //chkTuBu.Checked = false;
                toolstripcc.Checked = false;
            }
            else
            {
                toolstripcc.Checked = true;
                foreach (TuBu tb in _list.OfType<TuBu>())
                {
                    tb.GetEventSchedule();
                }
            }
            if (_list.OfType<Elster1700>().Count() == 0)
            {
                //chkElster1700.Checked = false;
                eToolStripMenuItem.Checked = false;
            }
            if (_list.OfType<Recloser351R>().Count() == 0)
            {
                //chkRecloser351R.Checked = false;
                recloserSelToolStripMenuItem.Checked = false;
            }
            if (_list.OfType<RecloserADVC>().Count() == 0 )
            {
                recloserADVCToolStripMenuItem.Checked = false;
            }
            if (_list.OfType<LBS>().Count() == 0)
            {
                recloserNew2ToolStripMenuItem.Checked = false;
            }
            comLogToolStripMenuItem.Checked = false;
            recloserNew1ToolStripMenuItem.Checked = false;
            
            /*if (_formprop != null)
            {
                splitContainer1.SplitterDistance = _formprop.DistanceSplit1;
                splitContainer2.SplitterDistance = _formprop.DistanceSplit2;
                splitContainer3.SplitterDistance = _formprop.DistanceSplit3;
                splitContainer6.SplitterDistance = _formprop.DistanceSplit6;
                splitAdvcandNew.SplitterDistance = _formprop.DistanceAdvc;
                splitCooperNulec.SplitterDistance = _formprop.DistanceCooper;
                splitElsterAndNew.SplitterDistance = _formprop.DistanceElster;
                splitTuBuRecloserSel.SplitterDistance = _formprop.DistanceTubu;
            }
            else
            {
                _formprop = new PropertyFormClass();
            }*/
            //check splitContainer2.Panel2
            if (toolstripcc.Checked == false && recloserSelToolStripMenuItem.Checked == false)
            {
                splitContainer2.Panel2Collapsed = true;
                // no need to check splitContainer2.panel1 because splitContainer2.Panel1 and Panel2 = splitContainer1.Panel1
                //check splitContainer1.Panel1 
                if (cooperToolStripMenuItem.Checked == false && nulecToolStripMenuItem.Checked == false)
                {
                    splitContainer1.Panel1Collapsed = true;
                    // no need to check splitContainer2.panel1 because splitContainer2.Panel1 and Panel2 = splitContainer1.Panel1
                }
            }
            if (recloserNew1ToolStripMenuItem.Checked == false && eToolStripMenuItem.Checked == false)
            {
                splitContainer6.Panel1Collapsed = true;
                if (recloserNew2ToolStripMenuItem.Checked == false && recloserADVCToolStripMenuItem.Checked == false)
                {
                    splitContainer3.Panel1Collapsed = true;
                    if (comLogToolStripMenuItem.Checked == false)
                    {
                        splitContainer1.Panel2Collapsed = true;
                    }
                }
                
            }
            if (recloserNew2ToolStripMenuItem.Checked == false && recloserADVCToolStripMenuItem.Checked == false)
            {
                splitContainer6.Panel2Collapsed = true;
            }
            recloserBaseBindingSource.DataSource = _list.OfType<CooperFXB>();
            nulecBindingSource.DataSource = _list.OfType<Nulec>();
            BindingsourceTubu.DataSource = _list.OfType<TuBu>();
            elsterbindingsource.DataSource = _list.OfType<Elster1700>();
            Recloser351RbindingSource.DataSource = _list.OfType<Recloser351R>();
            dgvBindingSource.DataSource = _list.OfType<RecloserADVC>();
            lBSBindingSource.DataSource = _list.OfType<LBS>();
        }
        //private string fileconfig;
        private void frmDeviceStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (timer.Enabled)
            /*_formprop.DistanceSplit1 =splitContainer1.SplitterDistance  ;
            _formprop.DistanceSplit2 = splitContainer2.SplitterDistance  ;
            _formprop.DistanceSplit3 = splitContainer3.SplitterDistance  ;
            _formprop.DistanceSplit6 = splitContainer6.SplitterDistance;
            _formprop.DistanceAdvc = splitAdvcandNew.SplitterDistance  ;
            _formprop.DistanceCooper = splitCooperNulec.SplitterDistance  ;
            _formprop.DistanceElster = splitElsterAndNew.SplitterDistance;
            _formprop.DistanceTubu = splitTuBuRecloserSel.SplitterDistance  ;
            PropertyFormFunc.saveObj(fileconfig, _formprop);*/
            try
            {
                EndPoll();
            }catch
            {}
        }

        private void Listener_OnDataSent(object sender, DataTransferEventArg e)
        {
            if (this.Disposing) return;
            try
            {
                this.Invoke(_addCommLog, e.Data, e.Port.ToString(), false);
            }
            catch{}
        }

        private void Listener_OnDataReceived(object sender, DataTransferEventArg e)
        {
            if (this.Disposing) return;
            if(comLogToolStripMenuItem.Checked)//if (chkShowLog.Checked)
            {
                this.Invoke(_addCommLog, e.Data, e.Port.ToString(), true);
            
                _saveCommData.BeginInvoke(DateTime.Now, e.Port, Ultility.ToHexText(e.Data), null, null);
            }
        }

        void Listener_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            var msg = string.Empty;
            var listener = sender as TcpServer;
            if (listener != null)
                msg = string.Format("{0}:{1} {2}", listener.Device.Name, listener.Device.Port, e.Status);
            else
                msg = e.Status;
            if (comLogToolStripMenuItem.Checked)//if (chkShowLog.Checked)
            {
                commLogCtrl1.Invoke(new Action<string>(commLogCtrl1.AddText), msg);
                //LogService.Logger.Debug(msg);
            }
        }

        /*private void timer_Tick(object sender, EventArgs e)
        {
            try
            {      
                var now = DateTime.Now;
                // No need auto-restart now
                //if (now.Hour == 0 && now.Minute == 0 && now.Second <= 1)
                //{
                //    LogService.Logger.Debug("Application self restart.");
                //    btRestart_Click(btRestart, EventArgs.Empty);
                //    return;
                //}

                _counter++;
                //timer.Stop();
                if (_counter >= _setting.PollingInterval)
                {
                    _counter = 0;
                    PollDevices();
                    //timer.Stop();
                }
                                
                foreach (var rc in _list.OfType<Nulec>())
                {
        // var recloser = sender as Recloser351R;
                    if (rc.CheckCosphi() == eAlertOpenClose.Open)
                    {
                        int i=0;
                        bool done = false;
                        while(i < rc.TuBuOpenpub.Count() && done==false)
                        {
                            try
                            {
                                int id = rc.TuBuOpenpub[i];
                                RecloserBase tmpTb = _list.Find(k => k.Id == id);
                                if (tmpTb!=null && !tmpTb.Status_Open)
                                {
                                    //((TuBu)_list[i]).CommandOpen(true);
                                    ((TuBu)tmpTb).CommandOpen(true);
                                    done = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                LogService.WriteError("frmDeviceStatus AutoOpen", ex.Message, ex);
                               
                            }
                            i++;
                        }
                    }
                    else if (rc.CheckCosphi() == eAlertOpenClose.Close)
                    {
                        int i = 0;
                        bool done = false;
                        while (i < rc.TuBuClosepub.Count() && done == false)
                        {
                            try
                            {
                                int id = rc.TuBuClosepub[i];
                                RecloserBase tmpTb = _list.Find(k => k.Id == id);
                                if (tmpTb != null && !tmpTb.Status_Close)
                                {
                                    //((TuBu)_list[i]).CommandOpen(true);
                                    ((TuBu)tmpTb).CommandClose(true);
                                    done = true;
                                }
                                
                            }
                            catch (Exception ex)
                            {
                                LogService.WriteError("frmDeviceStatus AutoClose", ex.Message, ex);

                            }
                            i++;
                        }
                    }
                    
                }


                //if (!_isPlaying)
                //{
                //    // Alert sound
                //    lock (_alertQueue)
                //    {
                //        foreach (var rc in _list)
                //        {
                //            if (rc.Alert && rc.AlertSoundStatus == eAlertSoundStatus.None)
                //                _alertQueue.Enqueue(rc);
                //        }
                //    }

                //    if (_alertQueue.Count > 0)
                //    {
                //        var play = new Action(PlayAlertSound);
                //        play.BeginInvoke(OnPlayAlertSoundCompleted, null);
                //        // if the above code is called, then _isplaying only false if users click Stop Alert
                //        // because of playLooping 
                //    }
                //}

                // save values
                _saveCounter++;
                if (_saveCounter > _setting.SaveDataInterval)
                {
                    _saveCounter = 0;

                    foreach (var rc in _list.Where(r => r.Listener.IsConnected && r.IsDirty))
                    { 
                        _saveRecloserData.BeginInvoke(rc, null, null);
                    }

                    
                }
            }
            catch (Exception ex)
            {
                LogService.Logger.Error("Timer", ex);
            }
            //timer.Enabled = true;
        }*/

        private bool _isPolling = false;
        private void PollDevices()
        {
            
            if (_isPolling)
                return;
            else
                _isPolling = true;

            var now  = DateTime.Now;
            var timeout = _setting.PollingInterval * 6;

            foreach (var rc in _list)
            {
                if (rc.Listener.IsConnected &&
                        (now - rc.LastUpdated).TotalSeconds > timeout &&
                        (now - rc.Listener.StartTime).TotalSeconds > timeout)
                {
                    LogService.Logger.Info(string.Format("Connection timeout on {0}", rc.Name));

                    rc.Listener.Restart();

                    //if(rc is Nulec)
                    //    ((Nulec)rc).RefeshAlertTime();
                }
                else if (rc.Enable)
                {
                    
                    Action<int> poll = new Action<int>(rc.Poll);
                    poll.BeginInvoke(RecloserAcq.Properties.Settings.Default.LatencyBetweenRequest, null, null);                    
                }
            }

            _isPolling = false;            
            _lastPollTime = DateTime.Now;
            this.Invoke(new Action(UpdatePollStatus));
        }

        private void btStartPoll_Click(object sender, EventArgs e)
        {
            StartPoll();
        }
        public void StartPoll()
        {
           
                foreach (var rc in _list)
                {
                     try
                    {
                        rc.Start();
                     }
                     catch (Exception ex)
                     {
                         LogService.Logger.Error("StartPoll", ex);
                         MessageBox.Show(ex.Message, "StartPoll", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                }
                //_counter = 0;
                //_saveCounter = 0;
                //timer.Interval = 1000;
                //timer.Enabled = true;
                _lastPollTime = null;
                UpdatePollStatus();
                startPollToolStripMenuItem1.Text = "End Poll";
                int connectedcount = 0;
                int notconnectedcount = 0;
                foreach(RecloserBase rc in _list)
                {
                    if (rc.Listener.success)
                    {
                        connectedcount++;
                    }
                    else
                    {
                        notconnectedcount++;
                    }
                }
                lbPollStatus.Text += " " + connectedcount.ToString() + " connected," + notconnectedcount.ToString() + " not connected";
            //btStartPoll.Enabled = false;
                //btEndPoll.Enabled = true;
                // Init current value for nulec only
                //if (_setting.CurrentDuration > 0)
                //{
                //    foreach (var rc in _list.OfType<Nulec>())
                //    {
                //        rc.RefeshAlertTime();
                //    }
                //}
            
            
        }
        private void btEndPoll_Click(object sender, EventArgs e)
        {
            EndPoll();

        }
        public void EndPoll()
        {
            try
            {
                //timer.Enabled = false;
                foreach (var rc in _list)
                {
                    try
                    {
                        rc.StopPolling();
                        rc.Listener.StopListening();
                        rc.Listener.ReceiveBuffer.Flush();
                    }
                    catch (Exception ex)
                    {
                        LogService.WriteError(ex.Message,ex.ToString());
                    }
                    
                }
                    

                UpdatePollStatus();
                //startPollToolStripMenuItem.Text = "Start Poll";
                startPollToolStripMenuItem1.Text = "Start Poll";
            }
            catch (Exception ex)
            {
                LogService.Logger.Error("EndPoll", ex);
                MessageBox.Show(ex.Message, "EndPoll", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void grdDevice_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            var cell = e.Row.Cells["PollButton"];
            cell.Value = "Poll";
        }

        private void grdDevice_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key == "PollButton")
            {
                var rc = e.Cell.Row.ListObject as RecloserBase;
                if (rc != null)
                {
                    Action<int> poll = new Action<int>(rc.Poll);
                    poll.BeginInvoke(RecloserAcq.Properties.Settings.Default.LatencyBetweenRequest, null, null);                    
                }
            }
        }

        private void grdCooper_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            var band = e.Layout.Bands[0];

            // Alert column
            UltraControlContainerEditor container = new UltraControlContainerEditor();
            var ctrl = new AlertCtrl();

            container.RenderingControl = ctrl;
            container.RenderingControlPropertyName = "Alert";

            var col = band.Columns["Alert"];
            col.EditorComponent = container;

            band.Layout.UseFixedHeaders = true;
            band.Groups["General"].Header.Fixed = true;
        }

        private void grdCooper_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            if (e.Row != null && e.Row.ListObject is RecloserBase)
            {
                var rc = e.Row.ListObject as RecloserBase;

                if (rc.Alert && rc.AlertSoundStatus == eAlertSoundStatus.Playing)
                    rc.AlertSoundStatus = eAlertSoundStatus.Stopped;
            }
        }

        private void SetAlertRecloser(RecloserBase rec, UltraGrid grd)
        {
            try
            {

                if (rec.DisableAlert)
                    return;
                var row = grd.Rows.Where(r => r.ListObject == rec).FirstOrDefault();
                if (row == null) return;
                if (rec.Alert == true)
                {
                    row.Appearance.ForeColor = Color.Red;
                    PlayAlertSound();
                }
                else
                {
                    row.Appearance.ForeColor = Color.Black;
                }
                row.Refresh();
            }
            catch (Exception ex)
            {
                LogService.WriteError("Alert error", ex.ToString());
            }
        }
        private void SetAlertNulec(object sender)
        {
            try
            {
                var nl = sender as Nulec;
                if (nl.DisableAlert)
                    return;
                var row = grdNulec.Rows.Where(r => r.ListObject == nl).FirstOrDefault();
                if (row == null) return;
                //UltraGridCell cell = row.Cells[e.PropertyName];
                //int? v = nl.GetInternalValue(e.PropertyName);
                //cell = row.Cells[e.PropertyName];

                if (nl.Alert == true)
                {
                    row.Appearance.ForeColor = Color.Red;
                    PlayAlertSound();
                }
                else
                {
                    row.Appearance.ForeColor = Color.Black;
                }
                row.Refresh();
            }
            catch (Exception ex)
            {
                LogService.WriteError("Alert error", ex.ToString());
            }

            
        }
        private void SetAlertCooperFXB(object sender)
        {
            try
            {
                var recloser = sender as CooperFXB;
                if (recloser.DisableAlert)
                    return;
                var row = grdCooper.Rows.Where(r => r.ListObject == recloser).FirstOrDefault();
                if (row == null) return;
                //UltraGridCell cell = row.Cells[e.PropertyName];
                //int? v = nl.GetInternalValue(e.PropertyName);
                //cell = row.Cells[e.PropertyName];

                if (recloser.Alert == true)
                {
                    row.Appearance.ForeColor = Color.Red;
                    PlayAlertSound();
                }
                else
                {
                    row.Appearance.ForeColor = Color.Black;
                }
                row.Refresh();
            }
            catch (Exception ex)
            {
                LogService.WriteError("Alert error", ex.ToString());
            }

        }
        private void OpenTuBuAuto(object sender)
        {
            
            var recloser = sender as RecloserBase;
                    
            int i=0;
            bool done = false;
            while(i < recloser.TuBuOpenpub.Count() && done==false)
            {
                try
                {
                    int id = recloser.TuBuOpenpub[i];
                    RecloserBase tmpTb = _list.Find(k => k.Id == id);
                    if (tmpTb!=null && !tmpTb.Status_Open)
                    {
                        //((TuBu)_list[i]).CommandOpen(true);
                        ((TuBu)tmpTb).CommandOpen(true);
                        done = true;
                    }
                }
                catch (Exception ex)
                {
                    LogService.WriteError("frmDeviceStatus AutoOpen", ex.Message, ex);
                               
                }
                i++;
            }
                    
        }
        private void CloseTuBuAuto(object sender)
        {
            var recloser = sender as RecloserBase;
            {
                int i = 0;
                bool done = false;
                while (i < recloser.TuBuClosepub.Count() && done == false)
                {
                    try
                    {
                        int id = recloser.TuBuClosepub[i];
                        RecloserBase tmpTb = _list.Find(k => k.Id == id);
                        if (tmpTb != null && !tmpTb.Status_Close)
                        {
                            //((TuBu)_list[i]).CommandOpen(true);
                            ((TuBu)tmpTb).CommandClose(true);
                            done = true;
                        }
                                
                    }
                    catch (Exception ex)
                    {
                        LogService.WriteError("frmDeviceStatus AutoClose", ex.Message, ex);

                    }
                    i++;
                }
            }
        }

        private void SetAlertRecloserADVC(object sender)
        {
            try
            {
                var recloser = sender as RecloserADVC;
                if (recloser.DisableAlert)
                    return;
                var row = dgvrecloserAdvc.Rows.Where(r => r.ListObject == recloser).FirstOrDefault();
                if (row == null) return;
                //UltraGridCell cell = row.Cells[e.PropertyName];
                //int? v = nl.GetInternalValue(e.PropertyName);
                //cell = row.Cells[e.PropertyName];
                //var value = recloser.Alert;
                if (recloser.Alert == true)
                {

                    row.Appearance.ForeColor = Color.Red;
                    PlayAlertSound();
                }
                else
                {
                    row.Appearance.ForeColor = Color.Black;
                }
                row.Refresh();
            }
            catch (Exception ex)
            {
                LogService.WriteError("Alert error", ex.ToString());
            }

        }
        private void SetAlertRecloser351R(object sender)
        {
            try
            {
                var recloser = sender as Recloser351R;
                if (recloser.DisableAlert)
                    return;
                var row = recloser351RListCtrl1.Rows.Where(r => r.ListObject == recloser).FirstOrDefault();
                if (row == null) return;
                //UltraGridCell cell = row.Cells[e.PropertyName];
                //int? v = nl.GetInternalValue(e.PropertyName);
                //cell = row.Cells[e.PropertyName];
                //var value = recloser.Alert;
                if (recloser.Alert == true)
                {

                    row.Appearance.ForeColor = Color.Red;
                    PlayAlertSound();
                }
                else
                {
                    row.Appearance.ForeColor = Color.Black;
                }
                row.Refresh();
            }
            catch (Exception ex)
            {
                LogService.WriteError("Alert error", ex.ToString());
            }
        }
        bool bmessage = false;
        RecloserBase curalertRecloser;
        
        void Recloser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "AlertVal")
                {


                    if (sender is Nulec)
                    {
                        SetAlertRecloser((Nulec)sender, grdNulec);
                    }
                    if (sender is CooperFXB)
                    {
                        SetAlertRecloser((CooperFXB)sender, grdCooper);
                    }
                    if (sender is Recloser351R)
                    {
                        SetAlertRecloser((Recloser351R)sender, recloser351RListCtrl1);
                    }
                    if (sender is RecloserADVC)
                    {
                        SetAlertRecloser((RecloserADVC)sender, dgvrecloserAdvc);
                    }
                    curalertRecloser = (RecloserBase)sender;
                    
                    
                    if (curalertRecloser.Alert == true && curalertRecloser.DisableAlert != true && !(sender is TuBu) && !bmessage)
                    {
                        /*this.TopMost = true;
                        this.Activate();
                        
                        MessageBox.Show("Sự cố tại nút " + rb.Name + ", " + rb.Location + ", " + rb.GroupName,);*/
                        this.Invoke(_flash);
                        bmessage = true;
                        string message = "Sự cố tại nút " + curalertRecloser.Name + ", " + curalertRecloser.Location + ", " + curalertRecloser.GroupName;
                        //Invoke(new Action<string>(commLogCtrl1.AddText), msg);
                        this.Invoke( new Action<string>(ShowAlertMessage),message);
                    }
                }
                else if (e.PropertyName == "OpenTuBu")
                {
                    OpenTuBuAuto(sender);
                }
                else if(e.PropertyName=="CloseTuBu")
                {
                    CloseTuBuAuto(sender);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("PropertyChanged---------------------------------------", ex.Message);
            }
            //code cu
            /*
             * if (e.PropertyName.StartsWith("Phase") && sender is Nulec)
            {
                var nl = sender as Nulec;
                var row = grdNulec.Rows.Where(r => r.ListObject == nl).FirstOrDefault();
                if (row == null) return;

                UltraGridCell cell = row.Cells[e.PropertyName];

                int? v = nl.GetInternalValue(e.PropertyName);

                if (v != null && (v.Value == 0 || v.Value > Nulec.MaxAmp))
                {
                    if (v.Value == 0)
                    { cell.Appearance.ForeColor = Color.Red; }
                    else
                    { cell.Appearance.ForeColor = Color.Blue; }

                    cell.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                }
                else
                {
                    cell.Appearance.ForeColor = Color.Black;
                    cell.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
                }
                cell.Refresh();
            }            
             */
        }

        private void ShowAlertMessage(string message)
        {
            
            lbPollStatus.Text = message;

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string caption = "Event";
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(this, message, caption, buttons);
            if (result == DialogResult.OK)
            {
                TriggerclickDisableAlert();
                curalertRecloser.resetAlert();

            }
            //rb.DisableAlert = true;
        }
        public bool Validate_SendCommand(string devicename, string command)
        {
            //require password
            if (System.Windows.Forms.MessageBox.Show("Bạn có chắc bạn muốn " + command + " " + devicename + "?", "Xac nhan", System.Windows.Forms.MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
        
        void Recloser_OnDataUpdated(object sender, EventArgs e)
        {
            UltraGrid grid = null;
            
            var rc = sender as RecloserBase;
            if (rc == null) return;

            if (rc is Nulec)
                grid = grdNulec;
            else if (rc is CooperFXB)
                grid = grdCooper;
            else if (rc is TuBu)
                grid = grdTuBu;
            else if (rc is LBS)
            {
                grid = grdLBS;
            }
            else if (rc is Recloser351R) // rc is Recloser351RListCtrl
            {
                // Di New device
                //recloser351RListCtrl1.Refresh(rc);
                //var row = recloser351RListCtrl1.Rows.Where(r => r.ListObject == rc).FirstOrDefault();
                //if (row != null)
                //    row.Refresh();
                grid = recloser351RListCtrl1;
            }
            else if (rc is Elster1700)  // rc is Elster1700ListCtrl
            {
                // Di New device
                //elster1700Ctrl1.Refresh(rc);
                grid = elster1700Ctrl1;
            }
            else if (rc is RecloserADVC)
            {
                grid = dgvrecloserAdvc;
            }
            if (grid != null)
            {
                var row = grid.Rows.Where(r => r.ListObject == rc).FirstOrDefault();
                if (row != null)
                {
                    row.Refresh();
                
                }
            }
        }

        

        #endregion

        #region Helpers
        

        private void UpdatePollStatus()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Polling status: {0}", "");
            
            if (_lastPollTime.HasValue)
                sb.AppendFormat(". Last request time: {0}", _lastPollTime.Value.ToString("HH:mm:ss"));

            lbPollStatus.Text = sb.ToString();

            //btStartPoll.Enabled = !timer.Enabled;
            //btEndPoll.Enabled = timer.Enabled;
        }

        private void AddCommLog(byte[] data, string port, bool rcv)
        {
            if (comLogToolStripMenuItem.Checked && data != null)
                this.commLogCtrl1.AddLog(data, port, rcv);                
        }
        private void playsoundtest()
        {
            _sndPlayer.SoundLocation = @"C:\Users\Administrator\Desktop\TestRecloser\Windows Exclamation.wav";
            _sndPlayer.Load();
            _sndPlayer.Play();
        }
        private void OnTimePlayEvent(object source, ElapsedEventArgs e)
        {
            if ((DateTime.Now - dtstartplay).TotalSeconds >= RecloserAcq.Properties.Settings.Default.soundduration && playcount<=0 )
            {
                tmplay.Enabled = false;
                tmplay.Stop();
                _sndPlayer.Stop();
                _isPlaying = false;
            }
            else
            {
                playcount--;
                PlayFile();
            }
        }
        private void PlayFile()
        {
            //if (_isPlaying == true) { return; }
            if (_enablealert)
            {
                _sndPlayer.Play();

            }
        }
        DateTime dtstartplay;
        int playcount;
        private void PlayAlertSound()
        {
            dtstartplay = DateTime.Now;
            playcount = RecloserAcq.Properties.Settings.Default.playcount;
            if(_isPlaying==true)
            {return;}

                

                
                //var file = _alertQueue.Dequeue().AlertFile;
            var file = _setting.alertsoundfile;
            if (!string.IsNullOrEmpty(file) && File.Exists(file))
            {
                try
                {
                    _sndPlayer.SoundLocation = file;
                    PlayFile();
                    tmplay.Enabled = true;
                    tmplay.Start();


                }
                catch (Exception ex)
                {
                    LogService.Logger.Error("PlayAlertSound", ex);
                }
                finally
                {
                    _isPlaying = false;
                }
            }
        }
        
        //private void PlayAlertSound()
        //{
        //    if (_enablealert == false)
        //    {
        //        return;
        //    }
        //    lock (_alertQueue)
        //    {
        //        if (_alertQueue.Count == 0 || _isPlaying) return;

        //        _isPlaying = true;

        //        while (_alertQueue.Count > 0)
        //        {
        //            var file = _alertQueue.Dequeue().AlertFile;
        //            if (!string.IsNullOrEmpty(file) && File.Exists(file))
        //            {
        //                try
        //                {
        //                    _sndPlayer.SoundLocation = file;
        //                    _sndPlayer.Load();
        //                    _sndPlayer.PlayLooping();
                            
        //                }
        //                catch(Exception ex)
        //                {
        //                    LogService.Logger.Error("PlayAlertSound", ex);
        //                }
        //            }
        //        }                
        //    }
        //}

        void OnPlayAlertSoundCompleted(IAsyncResult ar)
        { 
            if(ar.IsCompleted)
                _isPlaying = false; 
        }
        //private void OnTimedSaveHistory(object source, ElapsedEventArgs e)
        //{
        //    foreach (var rc in _list.Where(r => r.Listener.IsConnected && r.IsDirty))
        //    {
        //        //_saveRecloserData.BeginInvoke(rc, null, null);
        //        SaveData(rc);
        //    }
        //}
        private void SaveData(RecloserBase rc)
        {
            lock (rc)
            {
                lock (DBController.Instance)
                {
                    DBController.Instance.SaveRecloser(rc);
                    rc.AcceptChanges();
                }
            }
        }

        private void SaveCommData(DateTime date, int port, string data)
        {
            lock (DBController.Instance)
            {
                DBController.Instance.SaveReceive(date, port, data);
            }
        }
        #endregion        
        protected bool _enablealert = true;
        
        private void btntestN_Click(object sender, EventArgs e)
        {
            foreach (var rc in _list.OfType<Nulec>())
            {
                rc.settestzero();
            }
            //testNzero = !testNzero;
            //btntestN.Text = (testNzero == false ? "Tst N Zero" : "Stop N Zero");
            
            //testNmax = false;
            //btntestNMax.Text = "Tst N Max";
        }

        private void btntestC_Click(object sender, EventArgs e)
        {
            foreach (var rc in _list.OfType<CooperFXB>())
            {
                rc.settestzero();
            }
            //testCzero = !testCzero;
            
            //btntestC.Text = (testCzero == false ? "Tst C Zero" : "Stop C Zero");

            //testCmax = false;//(testCzero == true ? false : testCmax);
            //btntestCMax.Text = "Tst C Max";//(testCmax == false ? "Tst C Max" : "Stop C Max");
            
        }

        private void btntestCMax_Click(object sender, EventArgs e)
        {
            foreach (var rc in _list.OfType<CooperFXB>())
            {
                rc.settestmax();
            }
            //testCmax = !testCmax;
            //btntestCMax.Text = (testCmax == false ? "Tst C Max" : "Stop C Max");

            //testCzero = false;
            //btntestC.Text = "Tst C Zero";
            
        }

        private void btntestNMax_Click(object sender, EventArgs e)
        {
            
            foreach (var rc in _list.OfType<Nulec>())
            {
                rc.settestmax();
            }
            //testNmax = !testNmax;
            //btntestNMax.Text = (testNmax == false ? "Tst N Max" : "Stop N Max");
            //testNzero = false;
            //btntestN.Text = "Tst N Zero";
        }

        private void btnCo5_Click(object sender, EventArgs e)
        {
            foreach (var rc in _list.OfType<CooperFXB>())
            {
                rc.Testing = 0;
                rc.Amp12 = 5;
            }
        }

        private void btnN5_Click(object sender, EventArgs e)
        {
            foreach (var rc in _list.OfType<Nulec>())
            {
                rc.Testing = 0;
                rc.Amp12 = 5;
            }
        }

        private void frmDeviceStatus_Shown(object sender, EventArgs e)
        {
            if (this.AutoStartPoll)
                StartPoll();
        }

        private void btRestart_Click(object sender, EventArgs e)
        {
            try
            {
                //timer.Enabled = false;
                this.Close();
                //System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogService.Logger.Error("Restart error", ex);
            }
            finally
            {
                Application.Exit();
                Program.Restart();
            }
        }
        

        private void btnSendCmdNu_Click(object sender, EventArgs e)
        {
            ValidatePassword vp = new ValidatePassword(true);
            if (vp.ShowDialog() == DialogResult.Cancel)
            {
                //this.Close();
                return;
            }
            int i = grdNulec.ActiveRow.Band.Index;
            Nulec rc = (Nulec)nulecBindingSource.Current;
            if (Validate_SendCommand(rc.Location, cbCommandNu.Text) == true)
            {
                //Đóng
                //Mở
                //Set Time
                if (cbCommandNu.Text == "Đóng")
                {
                    rc.CommandClose(false);
                }
                else if (cbCommandNu.Text == "Mở")
                {
                    rc.CommandOpen(false);
                }
                else if (cbCommandNu.Text == "Set Time")
                {
                    rc.SetTime();
                }
            }
            //     foreach (var rc in _list.OfType<Nulec>())
            

        }

        private void btnSendCmdCo_Click(object sender, EventArgs e)
        {
            ValidatePassword vp = new ValidatePassword(true);
            if (vp.ShowDialog() == DialogResult.Cancel)
            {
                //this.Close();
                return;
            }
            
            int i = grdCooper.ActiveRow.Band.Index;
            
            CooperFXB rc = (CooperFXB)recloserBaseBindingSource.Current;
            if (Validate_SendCommand(rc.Location, cbCommandCo.Text) == true)
            {
                //Đóng (Close)
                //Mở Đóng (Reclose)
                //Mở (Open - Lock out)
                //Set Time
                if (cbCommandCo.Text == "Đóng (Close)")
                {
                    rc.CommandClose(false);
                }
                else if (cbCommandCo.Text == "Mở Đóng (Reclose)")
                {
                    rc.CommandReclose();
                }
                else if (cbCommandCo.Text == "Mở (Open - Lock out)")
                {
                    rc.CommandOpen(false);
                }
                else if (cbCommandCo.Text == "Set Time")
                {
                    rc.SetTime();
                }
            }
        }
        
        //private void chkNulec_CheckedChanged(object sender, EventArgs e)
        //{
        //    //panelNulec.Visible = chkNulec.Checked;
        //    if (chkNulec.Checked == false)
        //    {
        //        hideNulec();
        //    }
        //    else
        //    {
        //        showNulec();
        //    }
        //}
        
        //private void chkTuBu_CheckedChanged(object sender, EventArgs e)
        //{
        //    //panelTubu.Visible = chkTuBu.Checked;
        //    if (chkTuBu.Checked == false)
        //    {
        //        hideTubu();
        //    }
        //    else
        //    {
        //        showTubu();
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            ValidatePassword vp = new ValidatePassword(true);
            if (vp.ShowDialog() == DialogResult.Cancel)
            {
                //this.Close();
                return;
            }

            int i = grdTuBu.ActiveRow.Band.Index;
            TuBu rc = (TuBu)BindingsourceTubu.Current;
            if (Validate_SendCommand(rc.Location, cbcmdTuBu.Text) == true)
            {
                //Đóng (Close)
                //Mở Đóng (Reclose)
                //Mở (Open - Lock out)
                //Set Time
                if (cbcmdTuBu.Text == "Đóng (Close)")
                {
                    rc.CommandClose(false);
                }
                else if (cbcmdTuBu.Text == "Mở Đóng (Reclose)")
                {
                    rc.CommandReclose();
                }
                else if (cbcmdTuBu.Text == "Mở (Open - Lock out)")
                {
                    rc.CommandOpen(false);
                }
                else if (cbcmdTuBu.Text == "Set Time")
                {
                    rc.SetTime();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ValidatePassword vp = new ValidatePassword(true);
            if (vp.ShowDialog() == DialogResult.Cancel)
            {
                //this.Close();
                return;
            }

            int i = grdTuBu.ActiveRow.Band.Index;
            TuBu rc = (TuBu)BindingsourceTubu.Current;
            if (Validate_SendCommand(rc.Location, "Set time pulse") == true)
            {
                
                rc.SetTimePulse((ushort)nmTimepulse.Value,1);
                
            }
        }

        private void nmTimepulse_ValueChanged(object sender, EventArgs e)
        {

        }
        //private bool IsPasswordValidated(string devicetype)
        //{
        //    ValidatePassword vp = new ValidatePassword(true, devicetype);
        //    if (vp.ShowDialog() == DialogResult.Cancel)
        //    {
        //        //this.Close();
        //        return false;
        //    }
        //    return true;
        //}
        private void btnTuBuSynTime_Click(object sender, EventArgs e)
        {
            int i = grdTuBu.ActiveRow.Band.Index;
            TuBu rc = (TuBu)BindingsourceTubu.Current;
            //rc.Listener.Restart();
            //System.Threading.Thread.Sleep(1000);  
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("tubu") == false)
                return;


            if (Validate_SendCommand(rc.Location, "Syn Time") == true)
            {
                
                rc.SetTime();
                
            }
        }

        private void btnTuBuClose_Click(object sender, EventArgs e)
        {
            int i = grdTuBu.ActiveRow.Band.Index;
            TuBu rc = (TuBu)BindingsourceTubu.Current;


            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("tubu") == false)
                return;


            if (Validate_SendCommand(rc.Location, "Close") == true)
            {
                FA_Accounting.Common.LogService.WriteInfo("Click Close TuBu id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandClose(false);
                
                

            }
            //System.Threading.Thread.Sleep(3000);
            //EndPoll();
            //StartPoll();
        }

        private void btnTBOpen_Click(object sender, EventArgs e)
        {
            int i = grdTuBu.ActiveRow.Band.Index;
            TuBu rc = (TuBu)BindingsourceTubu.Current;



            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("tubu") == false)
                return;


            if (Validate_SendCommand(rc.Location, "Open") == true)
            {

                FA_Accounting.Common.LogService.WriteInfo("Click Open Tu Bu id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandOpen(false);
                
            }
            //System.Threading.Thread.Sleep(3000);
            //EndPoll();
            //StartPoll();
        }

        private void btnNulecSynTime_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloser") == false)
                return;
            int i = grdNulec.ActiveRow.Band.Index;
            Nulec rc = (Nulec)nulecBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Syn Time") == true)
            {
                //Đóng
                //Mở
                //Set Time
                rc.SetTime();
                
            }
        }

        private void btnNulecClose_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloser") == false)
                return;
            int i = grdNulec.ActiveRow.Band.Index;
            Nulec rc = (Nulec)nulecBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Close") == true)
            {
                //Đóng
                //Mở
                //Set Time
                rc.CommandClose(false);

            }
        }

        private void btnNulecOpen_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloser") == false)
                return;
            int i = grdNulec.ActiveRow.Band.Index;
            Nulec rc = (Nulec)nulecBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Open") == true)
            {
                //Đóng
                //Mở
                //Set Time
                FA_Accounting.Common.LogService.WriteInfo("Click Open Nulec id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandOpen(false);

            }
        }

        private void btnCooperSynTime_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloser") == false)
                return;

            int i = grdCooper.ActiveRow.Band.Index;

            CooperFXB rc = (CooperFXB)recloserBaseBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Syn Time") == true)
            {
                //Đóng (Close)
                //Mở Đóng (Reclose)
                //Mở (Open - Lock out)
                //Set Time
                rc.SetTime();
            }
        }

        private void btnCooperClose_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloser") == false)
                return;

            int i = grdCooper.ActiveRow.Band.Index;

            CooperFXB rc = (CooperFXB)recloserBaseBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Close") == true)
            {
                //Đóng (Close)
                //Mở Đóng (Reclose)
                //Mở (Open - Lock out)
                //Set Time
                rc.CommandClose(false);
            }
        }

        private void btnCooperOpen_Click(object sender, EventArgs e)
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloser") == false)
                return;

            int i = grdCooper.ActiveRow.Band.Index;

            CooperFXB rc = (CooperFXB)recloserBaseBindingSource.Current;
            if (Validate_SendCommand(rc.Location, "Open") == true)
            {
                //Đóng (Close)
                //Mở Đóng (Reclose)
                //Mở (Open - Lock out)
                //Set Time
                FA_Accounting.Common.LogService.WriteInfo("Click Open Cooper id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandOpen(false);
            }
        }

        private void btnsimulateCosphy_Click(object sender, EventArgs e)
        {
            int i = grdNulec.ActiveRow.Band.Index;
            Nulec rc = (Nulec)nulecBindingSource.Current;
            rc.PowerFactor = 2;
        }

       

        private void btnauto_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1(_list,this._DeviceFile);
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HistoryTuBufrm frm;
            List<TuBu> tmplist = (List<TuBu>)_list.OfType<TuBu>().ToList();
            frm = new HistoryTuBufrm(tmplist);
            
            frm.Show();
        }
        //Poll Tubu
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (_isPolling)
                return;
            else
                _isPolling = true;

            var now = DateTime.Now;
            var timeout = _setting.PollingInterval * 6;

            foreach (var rc in _list)
            {
                if (rc is TuBu)
                {
                    if (rc.Listener.IsConnected &&
                            (now - rc.LastUpdated).TotalSeconds > timeout &&
                            (now - rc.Listener.StartTime).TotalSeconds > timeout)
                    {
                        LogService.Logger.Info(string.Format("Connection timeout on {0}", rc.Name));

                        rc.Listener.Restart();

                        //if(rc is Nulec)
                        //    ((Nulec)rc).RefeshAlertTime();
                    }
                    else if (rc.Enable)
                    {
                        Action<int> poll = new Action<int>(rc.Poll);
                        poll.BeginInvoke(RecloserAcq.Properties.Settings.Default.LatencyBetweenRequest, null, null);
                    }
                }
            }

            _isPolling = false;
            _lastPollTime = DateTime.Now;
            this.Invoke(new Action(UpdatePollStatus));
        }
        

        public void SaveDevices(List<RecloserBase> list)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<RecloserBase>));
            using (var stream = File.CreateText(_DeviceFile))
            {
                ser.Serialize(stream, list);
                stream.Close();
            }
        }

        //private void chkShowLog_CheckedChanged(object sender, EventArgs e)
        //{
        //    //panelNulec.Visible = chkNulec.Checked;
        //    if (chkShowLog.Checked == false)
        //    {
        //        panelLog.Hide();
        //        //splitmain2.Panel2.Hide();
        //        //splitmain2.SplitterDistance = 0;
        //        //splitmain2.Panel2Collapsed = true;
        //        //splitmain2.Panel1Collapsed = false;
        //    }
        //    else
        //    {
        //        panelLog.Show();
        //        //splitmain2.Panel2.Show();
        //        //splitmain2.SplitterDistance = 100;
        //        //splitmain2.Panel2Collapsed = false;
        //        //splitmain2.Panel1Collapsed = true;
        //    }
        //}
        
        private void btnResetModem_Click(object sender, EventArgs e)
        {
            try
            {
                RecloserBase rc = (RecloserBase)recloserBaseBindingSource.Current;

                rc.resetModem();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ResetModem",ex.Message);
            }
        }

        private void btnSendCmdCo_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnResetMd2_Click(object sender, EventArgs e)
        {
            try
            {
                RecloserBase rc = (RecloserBase)nulecBindingSource.Current;

                rc.resetModem();

            }
            catch (Exception ex)
            {
                MessageBox.Show("resetModem2",ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                RecloserBase rc = (RecloserBase)BindingsourceTubu.Current;

                rc.resetModem();

            }
            catch (Exception ex)
            {
                MessageBox.Show("resettubu",ex.Message);
            }
        }

        private void btnDAlertCooper_Click(object sender, EventArgs e)
        {
            CooperFXB rc = (CooperFXB)recloserBaseBindingSource.Current;
            rc.DisableAlert = true;

        }

        private void btnEnableAlertCooper_Click(object sender, EventArgs e)
        {
            CooperFXB rc = (CooperFXB)recloserBaseBindingSource.Current;
            rc.DisableAlert = false;
        }

        private void btnDAlertNulec_Click(object sender, EventArgs e)
        {
            Nulec rc = (Nulec)nulecBindingSource.Current;
            rc.DisableAlert = true;
        }

        private void btnEnableAlertNulec_Click(object sender, EventArgs e)
        {
            Nulec rc = (Nulec)nulecBindingSource.Current;
            rc.DisableAlert = false;
        }

        private void btnSendCmdNu_Click_1(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Nulec rc = (Nulec)nulecBindingSource.Current;
            rc.Amp12 = 0;
        }
        bool bPollstarted = false;
        private void startPollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //startPollToolStripMenuItem1
            if (bPollstarted)
            {
                EndPoll();
                
            }
            else
            {
                StartPoll();
               
            }
            bPollstarted = !bPollstarted;
            
        }
        

        

        private void stopAlertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this._enablealert == true)
            {
                _sndPlayer.Stop();
                this.stopAlertToolStripMenuItem.Text = "Enable Alert";
                this.stopAlertToolStripMenuItem.ForeColor = Color.Red;
                _isPlaying = false;
                this._enablealert = false;
                //reset alert of all device
                foreach (var rc in _list)
                {
                    rc.resetAlert();
                    rc.DisableAlert = true;
                }
            }
            else
            {
                this._enablealert = true;
                this.stopAlertToolStripMenuItem.Text = "Stop Alert";
                this.stopAlertToolStripMenuItem.ForeColor = Color.Black;
                foreach (var rc in _list)
                {
                    rc.resetAlert();
                    rc.DisableAlert = false;
                }
            }
        }

        private void editRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new frmRequest();
            f.ShowDialog(this);
        }

        private void cooperToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<RecloserBase> list = this._list.OfType<CooperFXB>().ToList<RecloserBase>();
            var frm = new frmHistorySearch(list);
            frm.ShowDialog();
        }

        private void nulecToolStripMenuItem1_Click(object sender, EventArgs e)
        {
                
            List<RecloserBase> list = this._list.OfType<Nulec>().ToList<RecloserBase>();
            var frm = new frmHistorySearch(list);
            frm.ShowDialog();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_DeviceFile))
            {
                var f = new frmConfig(GroupID);
                f.ShowDialog(this);
            }
            else
            {
                var f = new frmConfig(_DeviceFile);
                f.ShowDialog(this);
            }
            
            
        }

        private void capacitorScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmcompensationcapacitor frm;
            if (string.IsNullOrEmpty(_DeviceFile))
            {
                frm = new frmcompensationcapacitor(this.GroupID);
            }
            else
            {
                frm = new frmcompensationcapacitor(this._DeviceFile);
            }
            frm.ShowDialog();
            if (frm.changed == true)
            {
                this.Close();
            }
        }

        private void compensationCapacitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryTuBufrm frm;
            List<TuBu> tmplist = (List<TuBu>)_list.OfType<TuBu>().ToList();
            frm = new HistoryTuBufrm(tmplist);

            frm.Show();
        }

        private void recloserSelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<Recloser351R> tmplist = (List<Recloser351R>)_list.OfType<Recloser351R>().ToList();
            HistoryRecloserSel frm = new HistoryRecloserSel(tmplist);
            frm.Show();
        }

        private void meterElsterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Elster1700> tmplist = (List<Elster1700>)_list.OfType<Elster1700>().ToList();
            HistoryElster frm = new HistoryElster(tmplist);
            frm.Show();
        }

        private void startPollToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            if (bPollstarted)
            {
                EndPoll();
                
            }
            else
            {
                StartPoll();
               
            }
            bPollstarted = !bPollstarted;

        }
        #region Display and resize splits
        //PropertyFormClass _formprop = new PropertyFormClass();
        //view or hide Nulec panel

        private void hideElster()
        {
            splitElsterAndNew.Panel1.Hide();
        }
        private void showElster()
        {
            splitContainer1.Panel2.Show();
            splitContainer3.Panel1.Show();
            splitContainer6.Panel1.Show();
            splitElsterAndNew.Panel1.Show();
            //splitElsterAndNew.SplitterDistance = _formprop.DistanceElster;
        }
        private void chkElster1700_CheckedChanged(object sender, EventArgs e)
        {
            //panelNulec.Visible = chkNulec.Checked;
            if (eToolStripMenuItem.Checked == false)
            {
                hideElster();
                //splitContainer1.SplitterDistance = 0;
            }
            else
            {
                showElster();
            }
        }
        private void hideRecloser351()
        {

            splitTuBuRecloserSel.Panel2.Hide();
            if (toolstripcc.Checked == false)
            {
                splitContainer2.Panel2.Hide();
            }
            
        }
        private void showRecloser351()
        {
            splitContainer1.Panel1.Show();
            splitContainer2.Panel2.Show();
            splitTuBuRecloserSel.Panel2.Show();


            //splitTuBuRecloserSel.SplitterDistance = _formprop.DistanceTubu;
        }
        private void chkRecloser351R_CheckedChanged(object sender, EventArgs e)
        {

            if (recloserSelToolStripMenuItem.Checked == false)
            {
                hideRecloser351();




                //splitContainer1.SplitterDistance = 0;
            }
            else
            {
                //splitmain.Panel1.Show();
                showRecloser351();
            }
        }

        private void nulecToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (nulecToolStripMenuItem.Checked == false)
            {
                hideNulec();
            }
            else
            {
                showNulec();
            }
        }

       



        private void recloserSelToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (recloserSelToolStripMenuItem.Checked)
            {
                showRecloser351();
            }
            else
            {
                hideRecloser351();
            }
        }

        private void eToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (eToolStripMenuItem.Checked)
            {
                showElster();
            }
            else
            {
                hideElster();
            }
        }

       
        private void showTubu()
        {
            splitTuBuRecloserSel.Panel1.Show();
            //splitTuBuRecloserSel.SplitterDistance = _formprop.DistanceTubu;
        }
        private void hideTubu()
        {
            splitTuBuRecloserSel.Panel1.Hide();
            if (recloserSelToolStripMenuItem.Checked == false)
            {
                splitContainer2.Panel2.Hide();
            }
            //splitTuBuRecloserSel.SplitterDistance = 0;
        }
        private void toolstripcc_CheckedChanged_1(object sender, EventArgs e)
        {
            if (toolstripcc.Checked)
            {
                showTubu();
            }
            else
            {
                hideTubu();
            }

        }
        //view or hide Nulec panel
       
        

        private void cooperToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (cooperToolStripMenuItem.Checked == false)
            {
                hideCooper();
            }
            else
            {
                showCooper();
            }
        }

        private void recloserADVCToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (recloserADVCToolStripMenuItem.Checked == false)
            {
                hideADVC();
            }
            else
            {
                showADVC();
            }
        }
        private void hideADVC()
        {
            splitAdvcandNew.Panel1.Hide();
            splitAdvcandNew.Panel1Collapsed = true;
            /*splitAdvcandNew.Panel1Collapsed = true;
            if (splitAdvcandNew.Panel2Collapsed == true)
            {
                splitContainer6.Panel2Collapsed = true;
            }*/
            
            //splitCooperNulec.SplitterDistance = 34;
        }
        private void showADVC()
        {
            splitAdvcandNew.Panel1Collapsed = false;
            splitAdvcandNew.SplitterDistance = 200;

        }
        private void hideCooper()
        {
            splitCooperNulec.Panel1.Hide();
            splitCooperNulec.Panel1Collapsed = true;
            if (nulecToolStripMenuItem.Checked == false)
            {
                splitContainer2.Panel1Collapsed = false;
            }
            //splitCooperNulec.SplitterDistance = 34;
        }
        private void showCooper()
        {
            splitContainer1.Panel1.Show();
            splitContainer2.Panel1.Show();
            splitCooperNulec.Panel1.Show();
            //splitCooperNulec.SplitterDistance = _formprop.DistanceCooper;
           
        }
        
        private void showNulec()
        {
            splitContainer1.Panel1.Show();
            splitContainer2.Panel1.Show();
            splitCooperNulec.Panel1.Show();
            splitCooperNulec.Panel2Collapsed = false;
            //splitCooperNulec.SplitterDistance = _formprop.DistanceCooper;
        }
        private void hideNulec()
        {
            
            splitCooperNulec.Panel2Collapsed = true;
            if (cooperToolStripMenuItem.Checked == false)
            {
                splitContainer2.Panel1Collapsed = true;
            }
            //splitCooperNulec.SplitterDistance = _formprop.DistanceCooper;
        }
        private void comLogToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (comLogToolStripMenuItem.Checked == false)
            {
                splitContainer3.Panel2.Hide();
                splitContainer3.Panel2Collapsed = true;
            }
            else
            {
                
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel2.Show();
                splitContainer3.Panel2Collapsed = false;
                splitContainer3.Panel2.Show();

            }
        }

        private void recloserNew2ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (recloserNew2ToolStripMenuItem.Checked == false)
            {

                splitAdvcandNew.Panel2Collapsed = true;
            }
            else
            {
                splitAdvcandNew.Panel2.Show();
                splitAdvcandNew.Panel2Collapsed = false;
            }
        }

        private void recloserNew1ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (recloserNew1ToolStripMenuItem.Checked == false)
            {
                
                splitElsterAndNew.Panel2Collapsed = true;
            }
            else
            {
                splitElsterAndNew.Panel2.Show();
                splitElsterAndNew.Panel2Collapsed = false;
            }
        }
        private void hidereclosernew1()
        {
            
            splitElsterAndNew.Panel2.Hide();
            if (eToolStripMenuItem.Checked == false)
            {
                splitContainer6.Panel1.Hide();
            }
        }
        private void showreclosernew1()
        {
            splitContainer1.Panel2.Show();
            splitContainer3.Panel1.Show();
            splitContainer6.Panel1.Show();
           // splitElsterAndNew.Panel2Collapsed = false;

        }
        private void hidereclosernew2()
        {
            splitAdvcandNew.Panel2.Hide();
            if (recloserADVCToolStripMenuItem.Checked == false)
            {
                splitContainer6.Panel2.Hide();
            }
            
        }
        private void showreclosernew2()
        {
            splitContainer1.Panel2.Show();
            splitContainer3.Panel1.Show();
            splitContainer6.Panel2.Show();
            splitAdvcandNew.Panel2Collapsed = false;
        }
#endregion

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCooperOpen.PerformClick();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }


        public RecloserBase getRecloser(int Id)
        {
           
           return _list.Where(r => r.Id == Id).FirstOrDefault();
           
        }
        private void SetBtnPermission()
        {
            //schedule
            capacitorScheduleToolStripMenuItem.Enabled = (Program.user.RoleName == "Superadmin" && Program.user.RoleId==0);
            // Recloser
            bool bopenrec = (Program.user.permopenRecloser > 1);
            bool bcloserec = (Program.user.permCloseRecloser > 1 && Program.DienLuc!="bentre");
            btnCloseAdvc.Enabled = bcloserec;
            btnNulecClose.Enabled = bcloserec;
            btnCooperClose.Enabled = bcloserec;
            btnrecloserSelClose.Enabled = bcloserec;
            closeToolStripMenuItem.Enabled = bcloserec; //toolstrip for Cooper 
            closeToolStripMenuItem1.Enabled = bcloserec;//toolstrip for Nulec
            recloserSelcloseToolStrip.Enabled = bcloserec; // Sel
            recloserAdvccloseToolStrip.Enabled = bcloserec; // Advc
            

            openToolStripMenuItem.Enabled = bopenrec; //toolstrip for Cooper Open
            openToolStripMenuItem1.Enabled = bopenrec; //toolstrip for Nulec Open
            recloserSelopenToolStrip.Enabled = bopenrec;
            recloserAdvcopenToolStrip.Enabled = bopenrec;
            btnOpenAdvc.Enabled = bopenrec;
            btnNulecOpen.Enabled = bopenrec;
            btnCooperOpen.Enabled = bopenrec;
            btnRecloserSelOpen.Enabled = bopenrec;

            // Tubu
            bool boperatetubu = (Program.user.permoperateTubu > 1);
            closeToolStripMenuItem2.Enabled = boperatetubu;
            openToolStripMenuItem2.Enabled = boperatetubu;
            btnTuBuClose.Enabled = boperatetubu;
            btnTBOpen.Enabled = boperatetubu;
        }

        private void recloserADVCHistory_Click(object sender, EventArgs e)
        {
            HistoryRecloserADVC frm = new HistoryRecloserADVC(_list.OfType<RecloserADVC>().ToList<RecloserADVC>());
            frm.Show();
        }

        private void btnOpenAdvc_Click(object sender, EventArgs e)
        {
            OpenRecloserADVCEvent();
        }

        private void btnCloseAdvc_Click(object sender, EventArgs e)
        {
            CloseRecloserADVCEvent();
        }

        private void recloserAdvcopenToolStrip_Click(object sender, EventArgs e)
        {
            OpenRecloserADVCEvent();
        }

        private void OpenRecloserADVCEvent()
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloservp") == false)
                return;
            RecloserADVC rc = (RecloserADVC)dgvBindingSource.Current;
            if (Validate_SendCommand(rc.Location + " " + rc.Name, "Open") == true)
            {
                FA_Accounting.Common.LogService.WriteInfo("Click Open Nulec id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandOpen(false);
                int userid = Program.user.id;
                string username = Program.user.name;
                int deviceid = rc.Id;
                string devicename = rc.Name;
                string groupname = rc.GroupName;
                DeviceStatic.LogOperation(userid, username, deviceid, devicename, groupname, 1, false);//0:close,1:open


            }
        }
        private void OpenLBSEvent()
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("lbs") == false)
                return;
            LBS rc = (LBS)lBSBindingSource.Current;
            if (Validate_SendCommand(rc.Location + " " + rc.Name, "Open") == true)
            {
                FA_Accounting.Common.LogService.WriteInfo("Click Open LBS id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandOpen(false);
                int userid = Program.user.id;
                string username = Program.user.name;
                int deviceid = rc.Id;
                string devicename = rc.Name;
                string groupname = rc.GroupName;
                DeviceStatic.LogOperation(userid, username, deviceid, devicename, groupname, 1, false);//0:close,1:open


            }
        }
        private void CloseLBSEvent()
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("lbs") == false)
                return;
            LBS rc = (LBS)lBSBindingSource.Current;
            if (Validate_SendCommand(rc.Location + " " + rc.Name, "Close") == true)
            {
                FA_Accounting.Common.LogService.WriteInfo("Click Close LBS id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandClose(false);
                int userid = Program.user.id;
                string username = Program.user.name;
                int deviceid = rc.Id;
                string devicename = rc.Name;
                string groupname = rc.GroupName;
                DeviceStatic.LogOperation(userid, username, deviceid, devicename, groupname, 0, false);//0:close,1:open


            }
        }
        private void recloserAdvccloseToolStrip_Click(object sender, EventArgs e)
        {
            CloseRecloserADVCEvent();
        }

        private void CloseRecloserADVCEvent()
        {
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("recloservp") == false)
                return;
            RecloserADVC rc = (RecloserADVC)dgvBindingSource.Current;
            if (Validate_SendCommand(rc.Location + " " + rc.Name, "Close") == true)
            {
                FA_Accounting.Common.LogService.WriteInfo("Click Close ADVC id " + rc.Id + " name: " + rc.Name, "");
                rc.CommandClose(false);
                int userid = Program.user.id;
                string username = Program.user.name;
                int deviceid = rc.Id;
                string devicename = rc.Name;
                string groupname = rc.GroupName;
                DeviceStatic.LogOperation(userid, username, deviceid, devicename, groupname, 0, false);//0:close,1:open


            }
        }

        private void operationLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formOperationLog frm;
            List<RecloserBase> tmplist = (List<RecloserBase>)_list.OfType<RecloserBase>().ToList();
            frm = new formOperationLog(tmplist);

            frm.Show();
        }

        private void stopAlertToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //CooperFXB rc = (CooperFXB)recloserBaseBindingSource.Current;
            TriggerclickDisableAlert();
        }
        private void TriggerclickDisableAlert()
        {
            if (stopAlertToolStripMenuItem1.Text == "Stop Alert")
            {
                stopAlertToolStripMenuItem1.Text = "Enable Alert";
                _enablealert = false;
                bmessage = false;
                if (curalertRecloser != null)
                {
                    curalertRecloser.DisableAlert = true;
                }
                try
                {
                    _sndPlayer.Stop();
                }
                catch (Exception ) { }
                _isPlaying = false;
                

            }
            else
            {
                stopAlertToolStripMenuItem1.Text = "Stop Alert";
                _enablealert = true;
                foreach (RecloserBase rb in _list)
                {
                    rb.DisableAlert = false;
                }
            }

        }

        private void btnEndpoll_Click(object sender, EventArgs e)
        {
            try
            {
                RecloserADVC rec = (RecloserADVC)dgvBindingSource.Current;
                if (btnEndpoll.Text == "End Poll")
                {
                    btnEndpoll.Text = "Start Poll";

                    rec.StopPolling();
                    rec.Listener.StopListening();
                    rec.Listener.ReceiveBuffer.Flush();

                }
                else
                {
                    btnEndpoll.Text = "End Poll";
                    rec.Start();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Start-End poll button click",ex.ToString());
            }
        }

        private void dgvrecloserAdvc_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnEndpoll.Visible = true;
            try{
                RecloserADVC rec = (RecloserADVC)dgvBindingSource.Current;
                if (rec.Listener.IsConnected)
                {
                    btnEndpoll.Text = "End Poll";
                }
                else
                {
                    btnEndpoll.Text = "Start Poll";
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("After selectrow ",ex.ToString());
            }
        }

        private void btnEndPollNulec_Click(object sender, EventArgs e)
        {
            try
            {
                Nulec rec = (Nulec)nulecBindingSource.Current;
                if (btnEndPollNulec.Text == "End Poll")
                {
                    btnEndPollNulec.Text = "Start Poll";

                    rec.StopPolling();
                    rec.Listener.StopListening();
                    rec.Listener.ReceiveBuffer.Flush();

                }
                else
                {
                    btnEndPollNulec.Text = "End Poll";
                    rec.Start();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Nulec Start-End poll button click", ex.ToString());
            }
        }

        private void grdNulec_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnEndPollNulec.Visible = true;
            try
            {
                Nulec rec = (Nulec)nulecBindingSource.Current;
                if (rec.Listener.IsConnected)
                {
                    btnEndPollNulec.Text = "End Poll";
                }
                else
                {
                    btnEndPollNulec.Text = "Start Poll";
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Nulec After selectrow ", ex.ToString());
            }
        }

        private void btnEndpollCooper_Click(object sender, EventArgs e)
        {
            try
            {
                CooperFXB rec = (CooperFXB)recloserBaseBindingSource.Current;
                if (btnEndpollCooper.Text == "End Poll")
                {
                    btnEndpollCooper.Text = "Start Poll";

                    rec.StopPolling();
                    rec.Listener.StopListening();
                    rec.Listener.ReceiveBuffer.Flush();

                }
                else
                {
                    btnEndpollCooper.Text = "End Poll";
                    rec.Start();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Cooper Start-End poll button click", ex.ToString());
            }
        }

        private void btnEndpolltubu_Click(object sender, EventArgs e)
        {
            try
            {
                TuBu rec = (TuBu)BindingsourceTubu.Current;
                if (btnEndpolltubu.Text == "End Poll")
                {
                    btnEndpolltubu.Text = "Start Poll";

                    rec.StopPolling();
                    rec.Listener.StopListening();
                    rec.Listener.ReceiveBuffer.Flush();

                }
                else
                {
                    btnEndpolltubu.Text = "End Poll";
                    rec.Start();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Tubu Start-End poll button click", ex.ToString());
            }
        }

        private void btnEndpoll351_Click(object sender, EventArgs e)
        {
            try
            {
                Recloser351R rec = (Recloser351R)Recloser351RbindingSource.Current;
                if (btnEndpoll351.Text == "End Poll")
                {
                    btnEndpoll351.Text = "Start Poll";

                    rec.StopPolling();
                    rec.Listener.StopListening();
                    rec.Listener.ReceiveBuffer.Flush();

                }
                else
                {
                    btnEndpoll351.Text = "End Poll";
                    rec.Start();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Recloser351 Start-End poll button click", ex.ToString());
            }
        }

        private void grdCooper_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnEndpollCooper.Visible = true;
            try
            {
                RecloserBase rec = (RecloserBase)recloserBaseBindingSource.Current;
                if (rec.Listener.IsConnected)
                {
                    btnEndpollCooper.Text = "End Poll";
                }
                else
                {
                    btnEndpollCooper.Text = "Start Poll";
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Cooper After selectrow ", ex.ToString());
            }
        }

        private void recloser351RListCtrl1_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnEndpoll351.Visible = true;
            try
            {
                RecloserBase rec = (RecloserBase)Recloser351RbindingSource.Current;
                if (rec.Listener.IsConnected)
                {
                    btnEndpoll351.Text = "End Poll";
                }
                else
                {
                    btnEndpoll351.Text = "Start Poll";
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("351 After selectrow ", ex.ToString());
            }
        }

        private void grdTuBu_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnEndpolltubu.Visible = true;
            try
            {
                RecloserBase rec = (RecloserBase)BindingsourceTubu.Current;
                if (rec.Listener.IsConnected)
                {
                    btnEndpolltubu.Text = "End Poll";
                }
                else
                {
                    btnEndpolltubu.Text = "Start Poll";
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("Tubu After selectrow ", ex.ToString());
            }
        }

        private void btnEnableAlertNulec_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSelSynTime_Click(object sender, EventArgs e)
        {
            
            Recloser351R rc = (Recloser351R)Recloser351RbindingSource.Current;
            //rc.Listener.Restart();
            //System.Threading.Thread.Sleep(1000);  
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("351") == false)
                return;


            if (Validate_SendCommand(rc.Location, "Syn Time") == true)
            {

                rc.SetTime();

            }

           


           

        }

        private void lbsToolstrip_open_Click(object sender, EventArgs e)
        {
            OpenLBSEvent();
        }

        private void lbsToolstrip_close_Click(object sender, EventArgs e)
        {
            CloseLBSEvent();
        }

        private void btnOpenLBS_Click(object sender, EventArgs e)
        {
            OpenLBSEvent();
        }

        private void btnCloseLBS_Click(object sender, EventArgs e)
        {
            CloseLBSEvent();
        }

        private void btnPollLBS_Click(object sender, EventArgs e)
        {
            try
            {
                LBS rec = (LBS)lBSBindingSource.Current;
                if (btnPollLBS.Text == "End Poll")
                {
                    btnPollLBS.Text = "Start Poll";

                    rec.StopPolling();
                    rec.Listener.StopListening();
                    rec.Listener.ReceiveBuffer.Flush();

                }
                else
                {
                    btnPollLBS.Text = "End Poll";
                    rec.Start();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("LBS Start-End poll button click", ex.ToString());
            }
        }

        private void btnSynLBS_Click(object sender, EventArgs e)
        {
            
            LBS rc = (LBS)lBSBindingSource.Current;
            
            if (RecloserAcq.Device.DeviceStatic.IsPasswordValidated("lbs") == false)
                return;
            if (Validate_SendCommand(rc.Location, "Syn Time") == true)
            {

                rc.SetTime();

            }
        }

        private void btnLBSHistory_Click(object sender, EventArgs e)
        {
            HistoryLBSfrm frm;
            List<LBS> tmplist = (List<LBS>)_list.OfType<LBS>().ToList();
            frm = new HistoryLBSfrm(tmplist);

            frm.Show();
        }

        private void grdLBS_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            
            btnEndpolltubu.Visible = true;
            try
            {
                RecloserBase rec = (RecloserBase)lBSBindingSource.Current; 
                if (rec.Listener.IsConnected)
                {
                    btnPollLBS.Text = "End Poll";
                }
                else
                {
                    btnPollLBS.Text = "Start Poll";
                }
            }
            catch (Exception ex)
            {
                LogService.WriteInfo("LBS After selectrow ", ex.ToString());
            }
        }

        private void btnCooperClose_Click_1(object sender, EventArgs e)
        {

        }
        
    }
}


/*
 * using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//add this namespace
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
public partial class Form5 : Form
{
public Form5()
{
InitializeComponent();
}
//Use DLLImport for user32.dll

[DllImport("user32.dll")]
static extern void FlashWindow(IntPtr a, bool b);
//IntPtr structure:
//It is used to represent a pointer or a handle.


//call the function FlashWindow() passing the Handle property of the 
//window to be 'flashed' and boolean value set to true.
void dd(Form w)
{
FlashWindow(w.Handle, true);
}
private void Form5_Load(object sender, EventArgs e)
{
//initialize w variable
Form k = this;
dd(k);

}
//

//Output:the form caption bar will blink once. To keep it continuously
//blnking , use a Timer.



}


}
*/