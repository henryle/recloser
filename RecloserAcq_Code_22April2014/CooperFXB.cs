using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using TcpComm;

namespace RecloserAcq.Device
{
    public class CooperFXB: RecloserBase
    {
        #region Ctor()
        public CooperFXB(int port)
            : base(port)
        {
        }

        public CooperFXB()
            : base()
        { 
        }
        #endregion

        #region Properties
        // Counter
        private int _target12;
        [XmlIgnore]
        public int Target12
        {
            get { return _target12; }
            set
            {
                if (_target12 != value)
                {
                    _target12 = value;
                    Notify("Target12");
                }
            }
        }

        private int _target34;
        [XmlIgnore]
        public int Target34
        {
            get { return _target34; }
            set
            {
                if (_target34 != value)
                {
                    _target34 = value;
                    Notify("Target34");
                }
            }
        }

        private int _target56;
        [XmlIgnore]
        public int Target56
        {
            get { return _target56; }
            set
            {
                if (_target56 != value)
                {
                    _target56 = value;
                    Notify("Target56");
                }
            }
        }

        private int _earthTarget;
        [XmlIgnore]
        public int EarthTarget
        {
            get { return _earthTarget; }
            set
            {
                if (_earthTarget != value)
                {
                    _earthTarget = value;
                    Notify("EarthTarget");
                }
            }
        }        
        #endregion

        #region Alert Status
        //private bool _status_AboveMinTrip;
        //public bool Status_AboveMinTrip
        //{
        //    get { return _status_AboveMinTrip; }
        //    set
        //    {
        //        if (_status_AboveMinTrip != value)
        //        {
        //            _status_AboveMinTrip = value;
        //            Notify("Status_AboveMinTrip");
        //        }
        //    }
        //}

        //private bool _status_Malfunction;
        //public bool Status_Malfunction
        //{
        //    get { return _status_Malfunction; }
        //    set
        //    {
        //        if (_status_Malfunction != value)
        //        {
        //            _status_Malfunction = value;
        //            Notify("Status_Malfunction");
        //        }
        //    }
        //}

        private bool _status_Target12;
        [XmlIgnore]
        public bool Status_Target12
        {
            get { return _status_Target12; }
            set
            {
                if (_status_Target12 != value)
                {
                    _status_Target12 = value;
                    Notify("Status_Target12");
                }
            }
        }

        private bool _status_Target34;
        [XmlIgnore]
        public bool Status_Target34
        {
            get { return _status_Target34; }
            set
            {
                if (_status_Target34 != value)
                {
                    _status_Target34 = value;
                    Notify("Status_Target34");
                }
            }
        }

        private bool _status_Target56;
        [XmlIgnore]
        public bool Status_Target56
        {
            get { return _status_Target56; }
            set
            {
                if (_status_Target56 != value)
                {
                    _status_Target56 = value;
                    Notify("Status_Target56");
                }
            }
        }

        private bool _status_EarthTarget;
        [XmlIgnore]
        public bool Status_EarthTarget
        {
            get { return _status_EarthTarget; }
            set
            {
                if (_status_EarthTarget != value)
                {
                    _status_EarthTarget = value;
                    Notify("Status_EarthTarget");
                }
            }
        }

        //private bool _status_SEFTarget;
        //public bool Status_SEFTarget
        //{
        //    get { return _status_SEFTarget; }
        //    set
        //    {
        //        if (_status_SEFTarget != value)
        //        {
        //            _status_SEFTarget = value;
        //            Notify("Status_SEFTarget");
        //        }
        //    }
        //} 
        #endregion

        #region Constants
        private const string Closestr = "00 07 91 02 01 00 FF 66";
        private const string ReClosestr = "00 07 91 01 01 00 FF 67";
        private const string Openstr = "00 07 91 00 01 00 FF 68";
        private const string Settimestr = "00 07 91 00 01 00 FF 68";                                    
        public static readonly byte[] Request_Battery =     new byte[] { 0x00, 0x05, 0x00, 0x0C, 0x02, 0xED };
        public static readonly byte[] Request_Current =     new byte[] { 0x00, 0x05, 0x20, 0x01, 0x04, 0xD6 };
        public static readonly byte[] Request_Status_1 =    new byte[] { 0x00, 0x05, 0x20, 0x0D, 0x07, 0xC7 };
        public static readonly byte[] Request_Status_2 =    new byte[] { 0x00, 0x05, 0x30, 0x00, 0x02, 0xC9 };

        public static readonly byte[] Response_Fail =       new byte[] { 0x00, 0x03, 0x02, 0xFB };
        public static readonly byte[] Response_OK =         new byte[] { 0x00, 0x03, 0x00, 0xFD };

        const int DataLength_Battery = 8;
        const int DataLength_Current = 0x0D;
        const int DataLength_Status_1 = 0x14;
        const int DataLength_Status_2 = 8;
        #endregion

        #region Helpers
        public override void UpdateData(byte[] data)
        {
            try
            {
                switch (data.Length - 1)
                {
                    case DataLength_Current:
                        UpdateCurrent(data);
                        break;

                    case DataLength_Status_1:
                        UpdateStatus_1(data);
                        break;

                    case DataLength_Status_2:
                        if (data[5] == 0 && data[6] == 0 && data[7] >= 0x3A)
                            UpdateStatus_2(data);
                        else
                            UpdateBattery(data);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogService.Logger.Error(string.Format("Cooper UpdateData error"), ex);
            }

            this.LastUpdated = DateTime.Now;
            base.UpdateData(data);
            WriteRow();
        }        

        private void UpdateStatus_2(byte[] data)
        {
            if (data.Length - 1 != DataLength_Status_2)
                LogService.WriteError("Cooper UpdateStatus 2","Length of status 1 data is not valid");

            var cStatus = data[3];
            var rStatus = data[4];

            // Parse recloser status, right to left
            // bit 0: Open
            // bit 1: Lock out
            // bit 2: Close
            // bit 3: Unused            
            // bit 4: Malfunction
            // bit 5: Above Min Trip

            this.Status_Open = (rStatus & 1) != 0;
            this.Status_Lockout = (rStatus & 1 << 1) != 0;
            this.Status_Close = (rStatus & 1 << 2) != 0;

            //this.Status_Malfunction = (rStatus & 1 << 4) != 0;
            //this.Status_AboveMinTrip = (rStatus & 1 << 5) != 0;
        }

        private void UpdateStatus_1(byte[] data)
        {
            if (data.Length - 1 != DataLength_Status_1)
                LogService.WriteError("Update Status 1","Length of status 2 data is not valid");

            byte status = data[4];
            // Parse recloser status, right to left
            // bit 0: Earth target
            // bit 1: 1-2 target
            // bit 2: 3-4 target
            // bit 3: 5-6 target
            this.Status_EarthTarget = (status & 1) != 0;
            this.Status_Target12 = (status & 1 << 1) != 0;
            this.Status_Target34 = (status & 1 << 2) != 0;
            this.Status_Target56 = (status & 1 << 3) != 0;
            Notify("Alert", false);

            // Target
            this.EarthTarget = data[6] * 256 + data[7];
            this.Target12 = data[8] * 256 + data[9];
            // skip 1 byte
            this.Target34 = data[11] * 256 + data[12];
            this.Target56 = data[13] * 256 + data[14];
            // skip 1 byte
            //this.SEFTarget = data[16] * 256 + data[17];
            this.Operations = data[18] * 256 + data[19];
        }

        private void UpdateBattery(byte[] data)
        {
            if (data.Length - 1 != DataLength_Battery)
                LogService.WriteError("Update Battery 1","Length of battery data is not valid");

            float uload = (float)(data[3] * 256 + data[4]) / 1000;
            float load = (float)(data[6] * 256 + data[7]) / 1000;

            this.Battery_1 = uload.ToString("##.##");
            this.Battery_2 = load.ToString("##.##");
        }

        private void UpdateCurrent(byte[] data)
        {
            if (data.Length - 1 != DataLength_Current)
                LogService.WriteError("Update curency","Length of current data is not valid");

            this.AmpEarth = data[3] * 256 + data[4];
#if DEBUG
            if (Testing == 1)
            {
                this.Amp12 = 0;
            }
            else if (Testing == 2)
            {
                this.Amp12 = MaxAmp;
            }
            else
            {
                this.Amp12 = data[6] * 256 + data[7];   //set it = 0 to test alert or set it > maxamp to test alert
            }
#else
            this.Amp12 = data[6] * 256 + data[7];   //set it = 0 to test alert or set it > maxamp to test alert
#endif

            this.Amp34 = data[8] * 256 + data[9];
            this.Amp56 = data[11] * 256 + data[12];
        }
        [XmlIgnore]
        public override int Amp12
        {
            get { return _amp12; }
            set
            {
                _amp12 = value;
                if (value == 0)
                {
                    // if just start up , _amp12 is 0 null 
                    // _amp12 still 0 and start time count
                    // else 
                    ///  if(_amp12 != value) keeps its value ->>> _amp12 keeps its value and starttime count to alert 
                    ///  else do not _start time count
                    if (_alertVal == 0)
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                }
                else if (value > MaxAmp)
                {
                    
                    setAlert(2, ref _startZeroA);
                }
                else
                {

                    if (_alertVal != 0)
                    {
                        _alertVal = 0;
                        Notify("AlertVal");
                        _startZeroA = null;
                    }
                }
            }
        }
        [XmlIgnore]
        public override int Amp34
        {
            get { return _amp34; }
            set {
                //if (_amp34 != value)
                //{
                //    return;
                //}
                _amp34 = value;
                if (value == 0)
                {
                    // if just start up , _amp34 is 0 null 
                    // _amp12 still 0 and start time count
                    // else 
                    ///  if(_amp34 != value) keeps its value ->>> _amp34 keeps its value and starttime count to alert 
                    ///  else do not _start time count
                    //setAlert(1, ref _startZeroB);
                    if (_alertVal == 0)
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                }
                else if (value > MaxAmp)
                {
                    //if (_amp34 != value)
                    //{
                      
                        //Notify("Amp34");
                        //if (_alertVal == 0)
                        //{
                        //    _alertVal = 1;
                        //    Notify("AlertVal");
                        //}
                        // starttime count
                    //}// else starttime count already started ->> do nothing
                    setAlert(2, ref _startZeroB);
                }
                else
                {
                    if (_alertVal != 0)
                    {
                        _alertVal = 0;
                        _startZeroB = null;
                        Notify("AlertVal");
                    }
                        //Notify("Amp34");
                    //_alertVal = 0; do not set to 0, only reset manually to 0 after going alerted. 
                }
            }
        }
        [XmlIgnore]
        public override int Amp56
        {
            get { return _amp56; }
            set
            {
                _amp56 = value;
                if (value == 0)
                {
                    // if just start up , _amp12 is 0 null 
                    // _amp12 still 0 and start time count
                    // else 
                    ///  if(_amp12 != value) keeps its value ->>> _amp12 keeps its value and starttime count to alert 
                    ///  else do not _start time count
                    if (_alertVal == 0)
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                }
                else if (value > MaxAmp)
                {
                    
                    setAlert(2, ref _startZeroC);
                }
                else
                {

                    if (_alertVal != 0)
                    {
                        _alertVal = 0;
                        _startZeroC = null;
                        Notify("AlertVal");
                        
                    }
                    
                    //_alertVal = 0; do not set to 0, only reset manually to 0 after going alerted. 
                }
            }
        }
        #endregion 
        /// <summary>
        /// Set alert value or reset the count time
        /// </summary>
        /// <param name="val"></param>
        /// <param name="start"></param>
        //public override void setAlert(int val, ref DateTime? start )
        //{
        //    if (_alertVal == val)
        //    {
        //        return;
        //    }
        //    if (start == null)
        //    {
        //        start = DateTime.Now;
        //    }
        //    else
        //    {
        //        if ((DateTime.Now - start.Value).TotalSeconds > MaxZeroDuration)
        //        {
        //            _alertVal = val;
        //        }
        //    }
        //}
        //public override bool Alert
        //{
        //    get
        //    {
        //        //return this.Listener.IsConnected && this.Status_EarthTarget || 
        //        //    this.Status_Target12 || this.Status_Target34 || this.Status_Target56;
        //        //if (this.Listener.IsConnected)
        //        //{
        //        //    var al = IsAlert(_amp12, _startZeroA);

        //        //    if (!al) al = IsAlert(_amp34, _startZeroB);

        //        //    if (!al) al = IsAlert(_amp56, _startZeroC);

        //        //    return al;
        //        //}
        //        //else
        //        //    return false;
        //        return (_alertVal != 0);
        //    }
        //}
        public override void CommandClose(bool auto)
        {
            //Listener.Send(Ultility.FromHex(Closestr));
            //System.Threading.Thread.Sleep(10);
        }
        public override void CommandOpen(bool auto)
        {
            Listener.Send(Ultility.FromHex(Openstr));
            //System.Threading.Thread.Sleep(10);
        }
        public override void CommandReclose()
        {
            Listener.Send(Ultility.FromHex(ReClosestr));
            //System.Threading.Thread.Sleep(10);
        }
        public override void SetTime()
        {
            Listener.Send(Ultility.FromHex(Settimestr));
            //System.Threading.Thread.Sleep(10);
        }
        public override void WriteRow()
        {
            string dvpath = RecloserAcq.Properties.Settings.Default.dvpath.Trim();
            if (dvpath.LastIndexOf('\\') != dvpath.Length - 1)
            {
                dvpath += "\\";
            }
            string filePath = RecloserAcq.Properties.Settings.Default.dvpath + Port + ".csv";
            string strFields = "TestField\r\n";
            string strvalues = "TestValue";
            try
            {
                /* TUBU
                 * strFields = "DeviceId,Type,LastUpdate,Alert,Mo,Dong,DeviceTime,Latest Close ,	" + 
                "Latest Open,Latest Manual Close Time,	Latest Manual Open Time \r\n";
                 */
                strFields = "DeviceId,Type,Date,Alert,Mo,Dong,DeviceTime,Operation,Target12,Target34,Target56,EarthTarget," +
                    " Amp12,Amp34,Amp56,AmpEarth, Status_Target12,Status_Target34,Status_Target56,Status_EarthTarget," +
                "Battery_Unloaded,Battery_Loaded \r\n";

                strvalues = DeviceID.ToString() + "," +
                    
                    DeviceType.ToString() + "," +
                    LastUpdated.ToString() + "," +
                    AlertVal.ToString() + "," +
                    Status_Open.ToString() + "," +
                    Status_Close.ToString() + ",0," +
                    Operations.ToString() + "," +
                    Target12.ToString() + "," +
                    Target34.ToString() + "," +
                    Target56.ToString() + "," +
                    EarthTarget.ToString() + "," +
                    Amp12.ToString() + "," +
                    Amp34.ToString() + "," +
                    Amp56.ToString() + "," +
                    AmpEarth.ToString() + "," +
                    Status_Target12.ToString() + "," +
                    Status_Target34.ToString() + "," +
                    Status_Target56.ToString() + "," +
                    Status_EarthTarget.ToString() + "," + 
                (Battery_1 == null ? " " : Battery_1.ToString()) + "," +
                (Battery_2 == null ? " " : Battery_2.ToString());
                    
                                        
                    
                    
                    
                
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
                catch (Exception e)
                {
                    LogService.WriteError("Write Row Cooper", e.Message) ;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Write Row Cooper", ex.Message);
            }
        }
    }
}
