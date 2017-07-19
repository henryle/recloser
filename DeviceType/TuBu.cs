using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using TcpComm;
using System.Timers;
using RecloserAcq.OracleDAL;
namespace RecloserAcq.Device
{
    public class TuBu : RecloserBase
    {
        // R: 52
        // I: 49
        // T: 54
        // W: 57
        // O: 4F
        // P: 50
        int CommandFlag = 0; // 0 do nothing 1: must close 2: must open
        bool AutoFlag = false;
        protected System.Timers.Timer timerClose = new System.Timers.Timer();
        
        public List<DeviceEvent> EventScheduleList;
        public void GetEventSchedule()
        {
            EventScheduleList = DeviceStatic.GetADeviceSchedule(this.Id);
        }
        private DateTime? _deviceTime;
        public DateTime? DeviceTime
        {
            get { return _deviceTime; }
            set
            {
                if (_deviceTime != value)
                {
                    _deviceTime = value;
                    Notify("DeviceTime");
                }
            }
        }
        private DateTime? _latestOpenTime = null;
        public DateTime? LatestOpenTime
        {
            get { return _latestOpenTime; }
            set
            {
                if (_latestOpenTime != value)
                {
                    _latestOpenTime = value;
                    Notify("LatestOpenTime");
                }
            }
        }
        private DateTime? _latestCloseTime;
        public DateTime? LatestCloseTime
        {
            get { return _latestCloseTime; }
            set
            {
                if (_latestCloseTime != value)
                {
                    _latestCloseTime = value;
                    Notify("LatestCloseTime");
                }
            }
        }
        private DateTime? _latestManualCloseTime;
        public DateTime? LatestManualCloseTime
        {
            get { return _latestManualCloseTime; }
            set
            {
                if (_latestManualCloseTime != value && _latestManualCloseTime <DateTime.Now)
                {
                    _latestManualCloseTime = value;
                    Notify("LatestManualCloseTime");
                }
            }
        }
        private DateTime? _latestManualOpenTime;
        public DateTime? LatestManualOpenTime
        {
            get { return _latestManualOpenTime; }
            set
            {
                if (_latestManualOpenTime != value && _latestManualOpenTime < DateTime.Now)
                {
                    _latestManualOpenTime = value;
                    Notify("LatestManualOpenTime");
                }
            }
        }
        public TuBu(int port)
            : base(port)
        {
            _buffersize = 128;
            _deviceTime = null;
            timerClose.Elapsed += new ElapsedEventHandler(OnTimerClose);
        }

        public TuBu()
            : this(0)
        {
            _deviceTime = null;
            timerClose.Elapsed += new ElapsedEventHandler(OnTimerClose);
        }
        private void OnTimerClose(object source, ElapsedEventArgs e)
        {
            if (CommandFlag != 0)
            {
                CommandFlag = 0;
                Close(this.AutoFlag);
                timerClose.Enabled = false;
                timerClose.Stop();
            }

        }
        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.TuBu;
            }
        }

       
        public override void UpdateData(byte[] data)
        {
            // after Parser, the data now is from nByteSend to EndBlock (0x03), len is = nByteSend
            // include nByteSend byte
            int i = 0;
            int len = data.Length;
            int blocklen = data[i + 2];
            while (i < len - 4 && blocklen != 0)
            {
                blocklen = data[i + 2];

                // copy to buff
                var buff = new byte[blocklen];
                //Array.Copy(data, start, buff, 0, len);
                System.Buffer.BlockCopy(data, i, buff, 0, blocklen);

                //var s = System.Text.Encoding.ASCII.GetString(buff); Tu Bu goi ve byte (hex) 

                GetValue(buff);
                i += blocklen;
                this.LastUpdated = DateTime.Now;
                
                // continue
            }
            base.UpdateData(data);
            CheckEvent();
            
            WriteRow();
        }
        
        private void CheckEvent()
        {
            foreach (DeviceEvent ev in EventScheduleList)
            {
                if (EventIsTime(ev))
                {
                    if (ev.Command == "Mo")
                    {
                        this.CommandOpen(true);
                    }
                    else if (ev.Command == "Dong")
                    {
                        this.CommandClose(true);
                    }
                    ev.DtPrevRun = DateTime.Now;
                    ev.ResetNextRun();
                }
            }
        }
        private bool EventIsTime(DeviceEvent ev)
        {
            if(ev.DtExpire<= DateTime.Now )
            {
                return false;
            }
            if (ev.DtActive > DateTime.Now)
            {
                return false;
            }
            if (DateTime.Now >= ev.DtNextRun && (DateTime.Now-ev.DtNextRun).TotalMinutes<12)
            {
                return true;
            }
            return false;
        }
        public override void WriteRow()
        {
             //string dvpath = RecloserAcq.Properties.Settings.Default.dvpath.Trim();
           
            string filePath = _devicepath + Port + ".csv";
            string strFields = "TestField\r\n";
            string strvalues = "TestValue";
            try
            {
                strFields = "DeviceId,Type,Date,Alert,Mo,Dong,DeviceTime,LatestClose,LatestOpen,LatestManualCloseTime,LatestManualOpenTime \r\n";

                strvalues = Id.ToString() + "," +
                    DeviceType.ToString() + "," +
                    LastUpdated.ToString() + "," +
                    "0," +
                    Status_Open.ToString() + "," +
                    Status_Close.ToString() + "," +
                    ((DeviceTime.HasValue && DeviceTime.Value > DateTime.MinValue) ? DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "") + "," +
                    ((LatestCloseTime.HasValue && LatestCloseTime.Value > DateTime.MinValue) ? LatestCloseTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "") + "," +
                    ((LatestOpenTime.HasValue && LatestOpenTime.Value > DateTime.MinValue) ? LatestOpenTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "") + "," +
                    ((LatestManualCloseTime.HasValue && LatestManualCloseTime.Value > DateTime.MinValue) ? LatestManualCloseTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "") + "," +
                    ((LatestManualOpenTime.HasValue && LatestManualOpenTime.Value > DateTime.MinValue) ? LatestManualOpenTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "");
                    
                
                

                try
                {
                    using (FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] FieldLine = Encoding.ASCII.GetBytes(strFields);
                        byte[] ValueLine = Encoding.ASCII.GetBytes(strvalues);
                        fs.Write(FieldLine, 0, FieldLine.Length);
                        fs.Write(ValueLine, 0, ValueLine.Length);
                        fs.Close();
                    }
                }
                catch 
                {
                    
                }
            }
            catch 
            {
                
            }
        }
        private void GetValue(byte[] s)
        {
            try
            {
                //var found = GetCurrentandPower(s); Di
                //var found = GetStatus(s);
                //if (!found)
                //    GetStatus(s);


                /*var  found = GetOpenDateTime(s);
                if (!found)
                    found = GetCloseDateTime(s);
                if (!found)
                    found = GetDeviceTime(s);
                if (!found)
                    found = GetManualOperationTime(s);*/
                GetDateTime(s);
                
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.Logger.Error(string.Format("TuBu.GetValue({0}) error", s), ex);
            }
        }



       
        protected override void SaveData()
        {
            // donothing as it's Save on getting Close / Open time
            // reset time
            this.SetTime();
            
        }
        /// <summary>
        /// get Operation (click/manual) datetime and device time
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool GetDateTime(byte[] s)
        {

            // DateTime
            // public const string Pattern_IOR = @"0x49 0x4F 0x0C 0x52 0x00 0x01 0x000x000x000x000x000x00 "; //Di
            int i = 0;
            int indexoftime = -1;
            while (indexoftime == -1 && i < s.Length - 9)
            {

                if (s[i] == 0x49 && s[i + 1] == 0x4F && s[i + 2] == 0x0C && s[i + 3] == 0x52)
                {

                    indexoftime = i + 6;
                }
                else if (s[i] == 0x52 && s[i + 1] == 0x54 && (s[i + 2] == 0x0A || s[i + 2] == 0x0B) && s[i + 3] == 0x52)
                {
                    indexoftime = i + 4;
                }
                i++;
            }

            if (indexoftime >= 0)
            {
                i--;
                //DateTime dt = new DateTime((int)s[indexoftime + 2], (int)s[indexoftime + 1], (int)s[indexoftime], (int)s[indexoftime + 3], (int)s[indexoftime + 4], (int)s[indexoftime + 5]);
                DateTime dt = new DateTime(Convert.ToInt16(Ultility.ByteToHex(s[indexoftime + 2])) + 2000, Convert.ToInt16(Ultility.ByteToHex(s[indexoftime + 1])), Convert.ToInt16(Ultility.ByteToHex(s[indexoftime])), Convert.ToInt16(Ultility.ByteToHex(s[indexoftime + 3])), Convert.ToInt16(Ultility.ByteToHex(s[indexoftime + 4])), Convert.ToInt16(Ultility.ByteToHex(s[indexoftime + 5])));
                if (indexoftime == i + 4)
                {
                    _deviceTime = dt;
                    return true;
                }
                if (s[i + 4] == 0x10)
                {
                    if (s[i + 5] == 0x01)
                    {
                        // close click
                        _latestCloseTime = dt;
                    }
                    else if (s[i + 5] == 0x4D)
                    {
                        // close manually
                        if (_latestManualCloseTime != dt && dt<DateTime.Now)
                        {

                            DBController.Instance.SaveTubu(this, (DateTime)dt, false, false);
                            _latestManualCloseTime = dt;
                        }
                        
                    }
                    else
                    {
                        // end while
                        return false;
                    }
                }
                else if (s[i + 4] == 0x11)
                {
                    if (s[i + 5] == 0x01)
                    {
                        // open click
                        _latestOpenTime = dt;
                    }
                    else if (s[i + 5] == 0x4D)
                    {
                        // open manually
                        if (_latestManualOpenTime != dt && dt <DateTime.Now)
                        {

                            DBController.Instance.SaveTubu(this, (DateTime)dt, true, false);
                            _latestManualOpenTime = dt;
                        }
                        
                        
                    }
                    else
                    {
                        //end while
                        return false;
                    }
                }
                else
                {
                    //end while
                    return false;
                }
                DateTime tmpManualOpen = (DateTime)(_latestManualOpenTime == null ? DateTime.MinValue : _latestManualOpenTime);
                DateTime tmpOpen = (DateTime)(_latestOpenTime == null ? DateTime.MinValue : _latestOpenTime);
                DateTime tmpManualClose = (DateTime)(_latestManualCloseTime == null ? DateTime.MinValue : _latestManualCloseTime);
                DateTime tmpClose = (DateTime)(_latestCloseTime == null ? DateTime.MinValue : _latestCloseTime);
                this.Status_Open = (tmpOpen > tmpManualOpen ? tmpOpen : tmpManualOpen) > (tmpClose > tmpManualClose ? tmpClose : tmpManualClose);
                //this.Status_Open = tmpOpen > tmpClose;
                this.Status_Close = !this.Status_Open;
                if (CommandFlag == 1 && _latestOpenTime != null)
                {
                    CommandFlag = 0;
                    this.timerBetweenEachPoll.Stop();
                    this.timerBetweenEachRequest.Stop();

                    this.timerBetweenEachPoll.Interval = this.Latencybetweenpoll;
                    this.timerBetweenEachRequest.Interval = this.Latencybetweenrequest;
                    
                    this.timerBetweenEachPoll.Start();
                    this.timerBetweenEachRequest.Start();
                    SendCloseWithFlag(AutoFlag);

                    
                }
                
                return true;
            }
            return false;
        }
       

        protected override void StartSendingRequests()
        {
            //SetTime();
            if (CommandFlag == 1)
            {
                //CommandFlag = 0;
                timerClose.Interval = 5 * 60 * 1000;
                timerClose.Enabled = true;
                timerClose.Start();
            }
            this.pollingcount++;

            if (pollingcount % 2 == 0)
            {
                if (!this.CheckConnectionStatus())
                {
                    return;
                }
            }
            // each subclass will have override StartSendingRequest
            // at first they do some connection request, password etc before sending the first request
            timerBetweenEachRequest.Enabled = true;
            timerBetweenEachRequest.Start();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();

            _iRequest = 0;
        }
        protected override bool CheckConnectionStatus()
        {

            if ((DateTime.Now - this.LastUpdated).TotalMilliseconds >= 240000) //4 minutes
            {
                pollingcount = 0;
                this.Stop();
                this.Start();

                FA_Accounting.Common.LogService.WriteInfo("Reset connection " + this.Name + " at " + this.Location + " port:" + this.Port.ToString(), "");
                return false;
            }
            return true;
        }
       

       


     
        public override void CommandClose(bool auto)
        {
            // Get the last time when it open 
            //reset LatestOpentime 
            SetTime();
            try
            {
                if (!Listener.IsConnected)
                {
                    Listener.Restart();
                    System.Threading.Thread.Sleep(1000);
                }
                _latestOpenTime = null;
                _latestCloseTime = null;
                _latestManualCloseTime = null;
                //RequestOpenCloseTime();
                //System.Threading.Thread.Sleep(100);  // wait for _latestOpenTime to be updated
                //RequestOpenCloseTime();
                CommandFlag = 1;
                this.timerBetweenEachRequest.Stop();
                this.timerBetweenEachPoll.Stop();
                this.timerBetweenEachRequest.Interval = 2000;
                this.timerBetweenEachPoll.Interval = 3000;
                
                AutoFlag = auto;
                
                this.timerBetweenEachPoll.Start();
                //this.timerBetweenEachRequest.Start();
                if (AutoFlag == true)
                {
                    FA_Accounting.Common.LogService.WriteInfo("Đóng Tụ Bù", "Flag set on để đóng tụ bù " + this.Location + " port:" + this.Port);
                }
                base.CommandClose(auto);
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Close Tubu", ex.Message, ex);
            }
        }
        private void SendCloseWithFlag(bool auto)
        {
            DateTime? tmplastClose = this._latestCloseTime;
            int passsecond = 0;
            DateTime now = DateTime.Now;
            DateTime? tmpOpentime;
            if (_latestOpenTime == null)
            {
                tmpOpentime = _latestManualOpenTime;
            }
            else
            {
                if (_latestManualOpenTime == null)
                {
                    tmpOpentime = _latestOpenTime;
                }
                else
                {
                    tmpOpentime = (_latestOpenTime > _latestManualOpenTime ? _latestOpenTime : _latestManualOpenTime);
                }
            }
            string morenote = "";
            if (tmpOpentime != null)
            {
                //passsecond = (now - (DateTime)tmpOpentime).Seconds;
                passsecond = (int)(now - (DateTime)tmpOpentime).TotalSeconds;
                if (passsecond < 0 && _deviceTime != null)
                {
                    passsecond = (int)((DateTime)_deviceTime - (DateTime)tmpOpentime).TotalSeconds;
                    morenote = "Cần đồng bộ thời gian  cho tụ bù " + Location;
                    if (passsecond < 0)
                    {
                        passsecond = 300; // Cần đồng bộ thời gian  cho tụ bù 
                    }
                }

                //else
                //{
                //    passsecond = 300;
                //}
            }
            else
            {
                passsecond = 300;
            }
            if (passsecond <= 300 && passsecond > 0)
            {
                CountDownFrm ctform = new CountDownFrm("Tụ Bù " + this.Location + " sẽ đóng trong ", passsecond, morenote);
                if (ctform.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }
            //Mo RL0 RLON
            //1+7+3(crc+end) = 11 =0x0B
            // $1B$32$01$2A$0A$0BIO$07W$10$00$00$F6$58$03
            // 1B 32 01 2A 0A 0B 49 4F 07 57 10 00 00 F6 58 03
            //Byte0 : 4 bit thấp là thứ tự Relay cần tác động RLx = 0—1 .
            //4 bit cao : 0=OFF tác động ngắt/1=ON tác động đóng

            //System.Threading.Thread.Sleep(2000);
            Close(auto);
            //CommandFlag = 0; // only set this = 0 khi da xac nhan duoc trang thai la dong
            //RequestOpenCloseTime();
        }

        private void Close(bool auto)
        {
            try
            {
                byte[] btran = new byte[16] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x0B, 0x49, 0x4F, 0x07, 0x57, 0x10, 0x00, 0x00, 0xF6, 0x58, 0x03 };

                // check the state (open/close) , if it's open, get the time when it was open called lastopentime
                // if now - lastopentime <5minutes -> wait lan mo gan nhat
                // if it's close 

                Listener.Send(btran);
                //Listener.Send(Ultility.FromHex(Closestr));

                //Listener.Send(btran);




                if (this.Status_Open)//(tmplastClose == null || this._latestCloseTime > tmplastClose)
                {
                    DBController.Instance.SaveTubu(this, DateTime.Now, false, auto);

                }
                _latestCloseTime = DateTime.Now; // khong can biet mo thanh cong hay khong, set open de ngan ngua truong hop dong lai lien
                this.Status_Close = true;
                this.Status_Open = false;
                this.CommandFlag = 0;
                this.WriteRow();
                System.Windows.Forms.MessageBox.Show("Lệnh đóng đã được gởi tới tụ bụ " + this.Location);
            }
            catch (Exception)
            {
            }
        }
        public override void CommandOpen(bool auto)
        {
            //Write RL1  ON
            //1+7+3(crc+end) = 11 =0x0B
            //$1B$32$01$2A$0A$0BIO$07W$11$00$00$A7$98$03
            //0x1B ,0x32 ,0x01 ,0x2A ,0x0A ,0x0B, 0x49, 0x4F ,0x07, 0x57 ,0x11 ,0x00 ,0x00 ,0xA7 ,0x98 ,0x03
            //Byte0 : 4 bit thấp là thứ tự Relay cần tác động RLx = 0—1 .
            //4 bit cao : 0=OFF tác động ngắt/1=ON tác động đóng
            SetTime();
            try
            {
                if (!Listener.IsConnected)
                {
                    Listener.Restart();
                    System.Threading.Thread.Sleep(1000);
                }
                //RequestOpenCloseTime();

                DateTime? tmplastOpen = this._latestOpenTime;
                byte[] btran = new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x0B, 0x49, 0x4F, 0x07, 0x57, 0x11, 0x00, 0x00, 0xA7, 0x98, 0x03 };
                this.timerBetweenEachPoll.Stop();
                this.timerBetweenEachRequest.Stop();

                
                Listener.Send(btran);
                
                this.timerBetweenEachPoll.Start();
                //this.timerBetweenEachRequest.Start();




                if (!this.Status_Open)//(tmplastOpen == null || this._latestOpenTime > tmplastOpen)
                {
                    DBController.Instance.SaveTubu(this, DateTime.Now, true, auto);

                }
                _latestOpenTime = DateTime.Now; // khong can biet mo thanh cong hay khong, set open de ngan ngua truong hop dong lai lien
                this.Status_Open = true;
                this.Status_Close = false;
                this.CommandFlag = 0;
                if (AutoFlag == true)
                {
                    FA_Accounting.Common.LogService.WriteInfo("Mở Tụ Bù", "Lệnh mở tụ bù tự động" + this.Location + " port:" + this.Port);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Lệnh mở đã được gởi tới tụ bụ " + this.Location);
                }
                //RequestOpenCloseTime();
                //System.Threading.Thread.Sleep(1000);
                //    Listener.Send(btran);
                //System.Threading.Thread.Sleep(1000);
                //    Listener.Send(btran);
                //System.Threading.Thread.Sleep(1000);
                //    Listener.Send(btran);
                //System.Threading.Thread.Sleep(1000);
                //    Listener.Send(btran);
                //System.Threading.Thread.Sleep(1000);
                //    Listener.Send(btran);
                //    System.Threading.Thread.Sleep(1000);
                this.WriteRow();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Open Tubu", ex.Message, ex);
            }
        }
        public void RequestLastTimeOpen()
        {
            //Read point time Relay 1 ON
            //1+12+12+3(crc+end) = 28 =0x1C
            //$1B$32$01$2A$0A$10IO$0CR$01$00$00$00$00$00$00$00$03 
            byte[] btran = new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x10, 0x49, 0x4F, 0x0C, 0x52, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x71, 0x1B, 0x03 };
            Listener.Send(btran);
            //System.Threading.Thread.Sleep(10);
        }
        public void RequestLastTimeClose()
        {
            //Read point time Relay 0 ON
            //1+12+3(crc+end) = 16 =0x10
            // 1B 32 01 2A 0A 10 49 4F 0C 52 00 00 00 00 00 00 00 00 B0 D7 03
            byte[] btran = new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x10, 0x49, 0x4F, 0x0C, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB0, 0xD7, 0x03 };
            //Read point time Relay 1 ON
            //1+12+3(crc+end) = 16 =0x10

            //can test kq tra ve 
            // 1B 32 01 2A 0A 10 49 4F 0C 52 01 00 00 00 00 00 00 00 71 1B 03
            //byte[] btran = new byte[]{0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x10, 0x49, 0x4F, 0x0C, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB0, 0xD7, 0x03};

            //Get Status
            //1(len) +5(data) + 3(crc+end) = 9
            // 1B 32 01 2A 0A 09GS 05R 00 7B 71 03 
            // 1B 32 01 2A 0A 09 47 53 05 52 00 7B 71 03 
            //byte[] btran = new byte[14] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x09, 0x47, 0x53, 0x05, 0x52, 0x00, 0x7B, 0x71, 0x03 };

            Listener.Send(btran);
            System.Threading.Thread.Sleep(10);
            //Listener.ReceiveBuffer

        }
        public void RequestOpenCloseTime()
        {
            //1B 32 01 2A 0A 1CIO 0CR 00 00 00 00 00 00 00 00IO 0CR 01 00 00 00 00 00 00 00 93 80 03
            //1B 32 01 2A 0A 1C 49 4F 0C 52 00 00 00 00 00 00 00 00 49 4F 0C 52 01 00 00 00 00 00 00 00 93 80 03
            byte[] btran = new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x1C, 0x49, 0x4F, 0x0C, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x49, 0x4F, 0x0C, 0x52, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x93, 0x80, 0x03 };
            Listener.Send(btran);
            
        }
        public override void CommandReclose()
        {

        }
        public override void SetTime()
        {
            //1(len) +10(data) + 3(crc+end) = 14=0x0E
            //$1B$32$01$2A$0A$0ERT$0AW$01$02$13$04$05$06$79$B1$03 //Write Time
            //$1B$32$01$2A$0A$0ERT$0AR$00$00$00$00$00$00$92$B7$03//Read Time
            //1B 32 01 2A 0A 0E 52 54 0A 57 {01 02 13 04 05 06} 79 B1 03
            //dd/mm/yy/hh/mm/ss
            try
            {
                DateTime now = DateTime.Now;
                byte[] btran = new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x0E, 0x52, 0x54, 0x0A, 0x57, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 };

                byte[] bdata = new byte[] { 0x52, 0x54, 0x0A, 0x57, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                //bdata[4] = Ultility.FromHexToByte(now.Day.ToString());
                //bdata[5] = Ultility.FromHexToByte(now.Month.ToString());
                //bdata[6] = Ultility.FromHexToByte((now.Year - 2000).ToString());
                //bdata[7] = Ultility.FromHexToByte(now.Hour.ToString());
                //bdata[8] = Ultility.FromHexToByte(now.Minute.ToString());
                //bdata[9] = Ultility.FromHexToByte(now.Second.ToString());
                bdata[4] = Ultility.FromHexToByte(now.Day.ToString());
                bdata[5] = Ultility.FromHexToByte(now.Month.ToString());
                bdata[6] = Ultility.FromHexToByte((now.Year - 2000).ToString());
                bdata[7] = Ultility.FromHexToByte(now.Hour.ToString());
                bdata[8] = Ultility.FromHexToByte(now.Minute.ToString());
                bdata[9] = Ultility.FromHexToByte(now.Second.ToString());
                byte bdatasize = 10;
                ushort r = CheckSumTB.crc16_checkBAO(bdata, (byte)(bdatasize + 4));
                byte[] bCrc = BitConverter.GetBytes(r);
                System.Buffer.BlockCopy(bdata, 4, btran, 10, 6);

                btran[16] = bCrc[0];
                btran[17] = bCrc[1];
                btran[18] = 0x03;



                Listener.Send(btran);
                //System.Threading.Thread.Sleep(10);
                // check if it successful or not and show message or return sth and show message at the frmDeviceStatus
            }
            catch (Exception )
            {
            }
            
            
        }
        public override void SetTimePulse(ushort timepulse, byte relay)
        {
            //bufsize :nBytetoSend (không tính độ dài của header, có tính độ dài của footer =1 + CRC =2 + do dai cua nByteToSend =1)
            //1B 32 01 2A 0A 0B TP 07 W 01 14 00 AA FF 03//Write Tpulse of RL1 0x0014 = 20 <== 2s
            //1B 32 01 2A 0A 0B 54 50 07  57        01     14 00   AA FF  03    //Write Tpulse of RL1 0x0014 = 20 <== 2s
            //{header      } n  {cmd} {n} {R/W}  {Relay}  {time}  {crc}  {end}
            byte[] bufsize = new byte[1] { 11 }; //0xOB
            //bufsize[0] = 11; //0xOB

            //default tran bytes:
            byte[] btran = new byte[16] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x0B, 0x54, 0x50, 0x07, 0x57, 0x01, 0x14, 0x00, 0xAA, 0xFF, 0x03 };

            byte[] bheader = Ultility.FromHex("1B 32 01 2A 0A");

            //set command : TP
            //byte[] bcmd = new byte[2] { 0x54, 0x50 };
            btran[6] = 0x54;
            btran[7] = 0x50;
            // set no of byte data:
            //byte[] bnobyte = new byte[1] { 0x07 };
            btran[8] = 0x07;
            // set mode Read/Write 
            //byte[] bmode = new byte[1] { 0x57 };
            btran[9] = 0x57; // W

            //set relay 0/1
            //byte[] brelay = new byte[1];
            //brelay[0] = relay;
            btran[10] = relay;

            // set time pulse:
            byte[] btimepulse = BitConverter.GetBytes(timepulse);
            btran[11] = btimepulse[0];
            btran[12] = btimepulse[1];

            byte[] binputdata = new byte[7];
            //System.Buffer.BlockCopy(bcmd, 0, binputdata, 0, 2);
            //System.Buffer.BlockCopy(bnobyte, 0, binputdata, 2, 1);
            //System.Buffer.BlockCopy(bmode, 0, binputdata, 3, 1);
            //System.Buffer.BlockCopy(brelay, 0, binputdata, 4, 1);
            //System.Buffer.BlockCopy(btimepulse, 0, binputdata, 5, 2);
            System.Buffer.BlockCopy(btran, 6, binputdata, 0, 7);

            ushort r = CheckSumTB.crc16_checkBAO(binputdata, bufsize[0]);

            //r |= (ushort)(r << 8);
            //r |= (ushort)(r >> 8);
            byte[] bCrc = BitConverter.GetBytes(r);

            btran[13] = bCrc[0];
            btran[14] = bCrc[1];
            btran[15] = 3; // 0x03;

            Listener.Send(btran);
            
        }
        private readonly byte Answer_Start = 0x01;
        public readonly byte Answer_End = 0x03;
        public override byte[] DoParse(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(Answer_Start);
            if (start != -1 && ringBuffer.Count > start + 1)
            {
                if (ringBuffer[start + 1] == 0x2A && ringBuffer.Count > start + 3 && ringBuffer[start + 2] == 0x0A)
                {

                    int end = ringBuffer.IndexOf(Answer_End);
                    int len = ringBuffer[start + 3] - 1;
                    //&& end - start - 2 == ringBuffer[start+3] 
                    if (end != -1 && ringBuffer.Count >= len)
                    {
                        start += 4;
                        // Found
                        //len = end - start + 1; //len = nByteSend

                        // Move the tail index to start read byte
                        var unusedBytes = new byte[start];
                        ringBuffer.Read(unusedBytes, 0, start);

                        var buff = new byte[len];
                        //ringBuffer.Read(buff, start, len);
                        ringBuffer.Read(buff, 0, len);
                        ringBuffer.Reset();
                        bDataReturned = true;
                        return buff;
                    }
                }
            }

            return null;
        }
    }
}
