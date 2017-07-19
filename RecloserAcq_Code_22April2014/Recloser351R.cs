using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using TcpComm;
using System.Threading;

namespace RecloserAcq.Device
{    
    public class Recloser351R: RecloserBase
    {
        


        #region Constants
		public const string MA = "MA#";     // A Phase Amps
        public const string MB = "MB#";     // B Phase Amps
        public const string MC = "MC#";     // C phase Amps
        public const string ME = "ME#";     // Earth Amps
        public const string CK = "CK";      // Date & Time
        public const string AR = "AR";      // Auto Reclose
        public const string SA = "SA";      // SEF Availability
        public const string BTV = "BTV#";   // battery Voltage
        public const string SV = "SV";      // Swichgear
        public const string SR = "SR";      // Switchgear Serial No
        public const string OP = "OP";      // Operations Counter
        public const string UW = "UW#";     // I Contact Life
        public const string VW = "VW#";     // II Contact Life
        public const string WW = "WW#";     // III Contact Life
        public const string RC = "RC";      // Rated Current
        public const string IC = "IC";      // Rated Interruption
        public const string RA = "RA";      // Rated Voltage
        public const string MGV = "MGV#";   // A-B Phase ( load )
        public const string MHV = "MHV#";   // B-C Phase ( load )
        public const string MIV = "MIV#";   // C-A Phase ( load )
        public const string kw = "kw#";     // Real Power
        public const string PWFTH = "PWFT#";  // Power Factor
        public const string UOF12 = "UOF12#";  // Measured Frequency
        public const string MK = "MK#";     // Apparent Power
        public const string MR = "MR#";     // Reactive Power
        public const string PS = "PS#";     // Phase Trip current
        public const string ES = "ES#";     // Earth trip current
        public const string SS = "SS#";     // SEF ( Protection )
        public const string NS = "NS#";     // NPS trip current
        public const string PERT = "PERT";    // Fault reset time
        public const string PTCM = "PTCM#";   // Phase Threshold Multi
        public const string ETCM = "ETCM#";   // Earth Threshold Multi
        public const string NTCM = "NTCM#";   // NPS Threshold Multi                
	    #endregion

        //private List<String> dataReceiveList = new List<string>();
        public bool acceptSendingRequest = false;
        #region Receive Pattern
        // hỏi password 
        public const string Pattern_Password = "Password";
        public const string Pattern_Closed = "Asserted";
        public const string Pattern_Open = "Deasserted";
        // Current
        // .*.MA#.6296,100.~.*.MB#.7141,100.~.*.MC#.6952,100.~.*.ME#.845,100.~.880C.
        public const string Pattern_Current = @"(?<Type>M[ABCE]#)\n(?<Value>\d{3,5}),(?<Factor>\d{3})";

        // Power
        // .*.DID-281.0.~.*.DID-345.1.~.*.KW#.2592737,1000.~.*.PWFT#.97,100.~.*.FS5060.0.~.1766.					           
        // .*.UOF12#.50130,1000.~.*.MN.4.0.~.*.MK#.2661267,1000.~.*.MR#.582373,1000.~.*.U1.1.~.DC75.
        public const string Pattern_Power = @"(?<Type>KW#|PWFT#|MK#|MR#)\n(?<Value>\d{2,7}),(?<Factor>\d{3,4})";

        // Date time & Status
        // .*.CK.21.1.09:30:56 05/06/12.~.*.AR.1.~.*.CL.0.~.*.+B-CL.0.~.*.EE.1.~.EFAC.
        //public const string Pattern_Status = @"(?<Type>SE|SA|EE|CB)\n(?<Value>0|1)";

        //"CK\n21\n1\n09:30:55 05/06/12"
        //public const string Pattern_DateTime = @"CK\n21\n1\n(?<Time>\d{2}:\d{2}:\d{2})\s(?<Date>\d{2}/\d{2}/\d{2})";
        // Recloser351 
        public const string Pattern_Date = @"(\d{2,2}/\d{2,2}/\d{2,2})";
        public const string Pattern_Time =@"(\d{2,2}:\d{2,2}:\d{2,2})";
        // end Recloser351
        
        // Battery
        //.*.MF.0.~.*.CS.10.1.528-19.01.~.*.BTS.1.~.*.WSOSTM.0.~.*.MIL.0.~.EA7C.
        public const string Pattern_Battery = @"BTS\n(?<Battery>0|1)";

        // Battery voltage
        // .*.FO.0.~.*.CF.0.~.*.CK.21.1.09:31:12 05/06/12.~.*.PR.5.~.*.BTV#.2720,100.~.7605.
        public const string Pattern_Battery_Voltage = @"BTV#\n(?<Value>\d{2,4}),(?<Factor>\d{3})";

        // Operation
        // .*.OP.103.~.*.UW#.9854,100.~.*.VW#.9827,100.~.*.WW#.9868,100.~.009C.
        public const string Pattern_Operation = @"OP\n(?<Operation>\d+)";

        // .*.MAV#.129712,10.~.*.MBV#.130182,10.~.*.MCV#.131010,10.~.*.LL.2000.~.*.BO.0.~.CCF7.
        // @"(?<Type>M[ABCEFGHIJKL]V#)\n(?<Value>\d{6}),(?<Factor>\d{2})"

        // .*.DI.1.~.*.MDV#.0,10.~.*.MEV#.0,10.~.*.MFV#.0,10.~.*.MGV#.225090,10.~.0830.

        // .*.MHV#.226182,10.~.*.MIV#.225850,10.~.*.MJV#.0,10.~.*.MKV#.0,10.~.*.MLV#.0,10.~.285E.

        // .*.V1.1.~.*.W1.1.~.*.U2.0.~.*.V2.0.~.*.W2.0.~.EF61.

        // .*.DID-344.0.~.D1C9.

        // .*.TN.20.1. .~.*.DID-371.0.~.*.DID-291.1.~.*.DID-386.1.~.AFD4.

        // .*.APGOA.0.~.*.APGAL.0.~.*.OS.0.~.*.SV.1.~.*.CB.1.~.1757. 

        // .*.TN.20.1. .~.*.DID-371.0.~.*.DID-291.1.~.*.DID-386.1.~.AFD4.
        #endregion

        #region Request
        private const string Closestr = "1B 32 01 2A 0A 53 45 54 0A 43 42 0A 31 0A 7E 0A " +
                                    "44 43 36 41 03 1B 32 01 2A 0A 52 45 51 0A 43 42 " + 
                                    "0A 7E 0A 41 44 43 46 03";
        private const string Openstr = "1B 32 01 2A 0A 53 45 54 0A 43 42 0A 30 0A 7E 0A " +
                                    "41 41 44 45 03 1B 32 01 2A 0A 52 45 51 0A 43 42 " +
                                    "0A 7E 0A 41 44 43 46 03";
        private const string Settimestr = "1B 32 01 2A 0A 53 45 54 0A 43 4B 0A 32 31 0A 31 0A"; // 0A 7E 0A 2A 0A 52 45 51 0A 43 4B 0A 7E 0A";
        
        #endregion
        
        

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

        public Recloser351R(int port)
            : base(port)
        {
            _deviceTime = null;
        }

        public Recloser351R():this(0)            
        { }

        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.Recloser351R;
            }
        }
        public bool AccAns_askPassword = false;
        private List<string> rawDataList = null;
        private void parse(string s)
        {
            if (rawDataList != null)
            {
                rawDataList.Clear();
            }
            else
            {
                rawDataList = new List<string>();
            }
            
            foreach (var c in s.Replace("\r", "").Split('\n'))
            {
                rawDataList.Add(c);
            }

            
        }
        private void setReceive()
        {
            //var feederValue = new FeederValue(rawDataList[2]);
            var ampleValue_MAG = new AmpleValue(rawDataList[1]);
            var ampleValue_ANG = new AmpleValue(rawDataList[2]);
            var voltsValue_MAG = new VoltsValue(rawDataList[5]);
            var voltsValue_ANG = new VoltsValue(rawDataList[6]);
            var modifiedValue_W = new ModifiedValue(rawDataList[9]);
            var modifiedValue_VAR = new ModifiedValue(rawDataList[10]);
            var modifiedValue_PF = new ModifiedValue(rawDataList[11]);
            //var groupValue_MAG = new GroupValue(rawDataList[12]);
            //var groupValue_ANG = new GroupValue(rawDataList[13]);
            //var frequencyValue = new FrequencyValue(rawDataList[17]);
            this._MW_A = modifiedValue_W.A;
            this._MW_B = modifiedValue_W.B;
            this._MW_C = modifiedValue_W.C;
            this._MW_3P = modifiedValue_W._3P;
            this._Q_MVAR_A = modifiedValue_VAR.A;
            this._Q_MVAR_B = modifiedValue_VAR.B;
            this._Q_MVAR_C = modifiedValue_VAR.C;
            this._Q_MVAR_3P = modifiedValue_VAR._3P;
            this._PF_A = modifiedValue_PF.A;
            this._PF_B = modifiedValue_PF.B;
            this._PF_C = modifiedValue_PF.C;
            this._PF_3P = modifiedValue_PF._3P;

            this._voltsValue_MAG_A = voltsValue_MAG.A;
            this._voltsValue_MAG_B = voltsValue_MAG.B;
            this._voltsValue_MAG_C = voltsValue_MAG.C;
            this._voltsValue_MAG_S = voltsValue_MAG.S;
            this._vang_A = voltsValue_ANG.A;
            this._vang_B = voltsValue_ANG.B;
            this._vang_C = voltsValue_ANG.C;
            this._vang_S = voltsValue_ANG.S;

            this._imag_A = ampleValue_MAG.A;
            this._imag_B = ampleValue_MAG.B;
            this._imag_C = ampleValue_MAG.C;
            this._imag_N = ampleValue_MAG.N;
            this._imag_G = ampleValue_MAG.G;

            this._iang_A = ampleValue_ANG.A;
            this._iang_B = ampleValue_ANG.B;
            this._iang_C = ampleValue_ANG.C;
            this._iang_N = ampleValue_ANG.N;
            this._iang_G = ampleValue_ANG.G;
            this.LastUpdated = DateTime.Now;
            
        }
        protected override void SaveData()
        {
            if (this.Listener.IsConnected)
            {
                RecloserAcq.DAL.DBController.Instance.SaveRecloser351(this);
                if ((DateTime.Now - LastUpdated).TotalMilliseconds >= this.Latencybetweenpoll * 2)
                {
                    acceptSendingRequest = false;
                    RePoll();
                }
            }
        }
        public override void UpdateData(byte[] data)
        {
            //String receive = null;
            var s = System.Text.Encoding.ASCII.GetString(data);
            if (s.IndexOf("Invalid Access Level") >= 0) //Invalid Access Level: 496e76616c696420416363657373204c6576656c
            {
                acceptSendingRequest = false;
                this.CommStatus = "Invalid Access Level";
                return;
            }
            GetDateTime(s);
            GetOtherValue(s);
                
            // and remaining values before this below code
            
            if(s.IndexOf("=>")>=0 )
            {
                int i = s.IndexOf("A         B         C         N         G") ;
                if (i>0)
                {
                    //int iFeeder2101 = dataReceiveList[3].IndexOf("FEEDER 2101");
                    //int ilast = dataReceiveList[3].IndexOf("FEEDER 2101");
                    //dataReceiveList[3] = dataReceiveList[3].Substring(iFeeder2101+11, ilast - iFeeder2101 - 11);
                    s = s.Substring(i);
                    parse(s);
                    setReceive();
                }

            }
            if (this.CommStatus == "Invalid Access Level")
            {
                this.CommStatus = "Polling";
            }
            
            
            base.UpdateData(data);
            WriteRow();
        }
        
     

      

        private bool GetStatus(string s)
        {
            if(s.IndexOf("Asserted") > 0)
            {
                this.Status_Close = true;
                this.Status_Open = false;
            }
            else if (s.IndexOf("Deasserted") > 0)
            {
                this.Status_Open = true;
                this.Status_Close = false;
                
            }
            else{
                return false;
            }
            return true;
            
        }

        private bool GetOtherValue(string s)
        {
            try
            {
                // Recloser351R
                // có hỏi Password
                if (Regex.IsMatch(s, Pattern_Password, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    AccAns_askPassword = true;
                    //return true;
                }
                if (Regex.IsMatch(s, Pattern_Open, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    Status_Open = true;
                    Status_Close = false;
                    return true;
                }
                else if (Regex.IsMatch(s, Pattern_Closed, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    Status_Open = false;
                    Status_Close = true;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            
           
        }
        private bool GetDateTime(string s)
        {
            // DateTime
            try
            {
                Match matchDate = Regex.Match(s, Pattern_Date);
                Match matchTime = Regex.Match(s, Pattern_Time);
                string strdate = this._deviceTime == null ? "01/01/2014" : this._deviceTime.Value.ToString("MM/dd/yy");
                string strtime = this._deviceTime == null ? "00:00:00" : this._deviceTime.Value.ToString("HH:mm:ss");
                // Here we check the Match instance.
                if (matchDate.Success)
                {
                    // Finally, we get the Group value and display it.
                    //It is important to note that the indexing of the Groups collection on Match objects starts at 1.
                    strdate = matchDate.Groups[1].Value;
                    //date = //DateTime.ParseExact(key, "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);

                }
                if (matchTime.Success)
                {
                    // Finally, we get the Group value and display it.
                    strtime = matchTime.Groups[1].Value;
                    //this._deviceTime = DateTime.ParseExact(key, "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);

                }
                this._deviceTime = DateTime.ParseExact(strdate + " " + strtime, "MM/dd/yy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                //Regex re = new Regex(Pattern_Date);
                //Regex reTime = new Regex(Pattern_Time);
                //var match = re.Match(s);
                //var matchtime = reTime.Match(s);
                //if (match.Success)
                //{
                //    // update device datetime
                //    //var time = match.Groups["Time"].Value;
                //    var date = match.Groups["Date"].Value;
                //    DateTime dt;

                //    if (DateTime.TryParseExact(date + " " + time, "dd/MM/yy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dt))
                //        this.DeviceTime = dt;
                //}
            }
            catch(Exception ex)
            {
            }

            return false;
        }
        // end

       
        #region new field for Recloser351 

        
        private double _voltsValue_MAG_A ;
        [XmlIgnore]
        public double VoltsValue_MAG_A
        {
            get { return _voltsValue_MAG_A; }
            set
            {
                if (_voltsValue_MAG_A != value)
                {
                    _voltsValue_MAG_A = value;
                    //Notify("VoltsValue_MAG_A");
                }
            }
        }
        private double _voltsValue_MAG_B;
        [XmlIgnore]
        public double VoltsValue_MAG_B
        {
            get { return _voltsValue_MAG_B; }
            set
            {
                if (_voltsValue_MAG_B != value)
                {
                    _voltsValue_MAG_B = value;
                    //Notify("VoltsValue_MAG_B");
                }
            }
        }
        private double _voltsValue_MAG_C;
        [XmlIgnore]
        public double VoltsValue_MAG_C
        {
            get { return _voltsValue_MAG_C; }
            set
            {
                if (_voltsValue_MAG_C != value)
                {
                    _voltsValue_MAG_C = value;
                    //Notify("VoltsValue_MAG_C");
                }
            }
        }
        private double _voltsValue_MAG_S;
        [XmlIgnore]
        public double VoltsValue_MAG_S
        {
            get { return _voltsValue_MAG_S; }
            set
            {
                if (_voltsValue_MAG_S != value)
                {
                    _voltsValue_MAG_S = value;
                    //Notify("VoltsValue_MAG_N");
                }
            }
        }
        private double _vang_A;
        [XmlIgnore]
        public double Vang_A
        {
            get { return _vang_A; }
            set
            {
                if (_vang_A != value)
                {
                    _vang_A = value;
                    //Notify("Vang_A");
                }
            }
        }
        private double _vang_B;
        [XmlIgnore]
        public double Vang_B
        {
            get { return _vang_B; }
            set
            {
                if (_vang_B != value)
                {
                    _vang_B = value;
                    //Notify("Vang_B");
                }
            }
        }
        private double _vang_C;
        [XmlIgnore]
        public double Vang_C
        {
            get { return _vang_C; }
            set
            {
                if (_vang_C != value)
                {
                    _vang_C = value;
                    //Notify("Vang_C");
                }
            }
        }
        private double _vang_S;
        [XmlIgnore]
        public double Vang_S
        {
            get { return _vang_S; }
            set
            {
                if (_vang_S != value)
                {
                    _vang_S = value;
                    //Notify("Vang_N");
                }
            }
        }
        private double _imag_A;
        [XmlIgnore]
        public double Imag_A
        {
            get { return _imag_A; }
            set
            {
                _imag_A = value;
                if (value == 0)
                {
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
        private double _imag_B;
        [XmlIgnore]
        public double Imag_B
        {
            get { return _imag_B; }
            set
            {
                _imag_B = value;
                if (value == 0)
                {
                    if (_alertVal == 0)
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                }
                else if (value > MaxAmp)
                {
                    setAlert(2, ref _startZeroB);
                }
                else
                {
                    if (_alertVal != 0)
                    {
                        _alertVal = 0;
                        Notify("AlertVal");
                        _startZeroB = null;
                    }
                }
            }
        }
        private double _imag_C;
        [XmlIgnore]
        public double Imag_C
        {
            get { return _imag_C; }
            set
            {
                _imag_C = value;
                if (value == 0)
                {
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
                        Notify("AlertVal");
                        _startZeroC = null;
                    }
                }
            }
        }
        private double _imag_N;
        [XmlIgnore]
        public double Imag_N
        {
            get { return _imag_N; }
            set
            {
                if (_imag_N != value)
                {
                    _imag_N = value;
                    //Notify("Imag_N");
                }
            }
        }
        private double _imag_G;
        [XmlIgnore]
        public double Imag_G
        {
            get { return _imag_G; }
            set
            {
                if (_imag_G != value)
                {
                    _imag_G = value;
                    //Notify("Imag_G");
                }
            }
        }
        private double _iang_A;
        [XmlIgnore]
        public double Iang_A
        {
            get { return _iang_A; }
            set
            {
                if (_iang_A != value)
                {
                    _iang_A = value;
                    //Notify("Iang_A");
                }
            }
        }
        private double _iang_B;
        [XmlIgnore]
        public double Iang_B
        {
            get { return _iang_B; }
            set
            {
                if (_iang_B != value)
                {
                    _iang_B = value;
                    //Notify("Iang_B");
                }
            }
        }
        private double _iang_C;
        [XmlIgnore]
        public double Iang_C
        {
            get { return _iang_C; }
            set
            {
                if (_iang_C != value)
                {
                    _iang_C = value;
                    //Notify("Iang_C");
                }
            }
        }
        private double _iang_N;
        [XmlIgnore]
        public double Iang_N
        {
            get { return _iang_N; }
            set
            {
                if (_iang_N != value)
                {
                    _iang_N = value;
                    //Notify("Iang_N");
                }
            }
        }
        private double _iang_G;
        [XmlIgnore]
        public double Iang_G
        {
            get { return _iang_G; }
            set
            {
                if (_iang_G != value)
                {
                    _iang_G = value;
                    //Notify("Iang_G");
                }
            }
        }
        private double _Q_MVAR_A;
        [XmlIgnore]
        public double Q_MVAR_A
        {
            get { return _Q_MVAR_A; }
            set
            {
                if (_Q_MVAR_A != value)
                {
                    _Q_MVAR_A = value;
                    //Notify("Q_MVAR_A");
                }
            }
        }
        private double _Q_MVAR_B;
        [XmlIgnore]
        public double Q_MVAR_B
        {
            get { return _Q_MVAR_B; }
            set
            {
                if (_Q_MVAR_B != value)
                {
                    _Q_MVAR_B = value;
                    //Notify("Q_MVAR_B");
                }
            }
        }
        private double _Q_MVAR_C;
        [XmlIgnore]
        public double Q_MVAR_C
        {
            get { return _Q_MVAR_C; }
            set
            {
                if (_Q_MVAR_C != value)
                {
                    _Q_MVAR_C = value;
                    //Notify("Q_MVAR_C");
                }
            }
        }
        private double _Q_MVAR_3P;
        [XmlIgnore]
        public double Q_MVAR_3P
        {
            get { return _Q_MVAR_3P; }
            set
            {
                if (_Q_MVAR_3P != value)
                {
                    _Q_MVAR_3P = value;
                    //Notify("Q_MVAR_N");
                }
                if (controlbyNo_Cosphi_Q == eControlType.Q)
                {
                    eAlertOpenClose et = CheckQ();
                    if (et == eAlertOpenClose.Close)
                    {
                        Notify("CloseTuBu");
                    }
                    else if (et == eAlertOpenClose.Open)
                    {
                        Notify("OpenTuBu");
                    }
                }
            }
        }
        private double _MW_A;
        [XmlIgnore]
        public double MW_A
        {
            get { return _MW_A; }
            set
            {
                if (_MW_A != value)
                {
                    _MW_A = value;
                    //Notify("MW_A");
                }
            }
        }
        private double _MW_B;
        [XmlIgnore]
        public double MW_B
        {
            get { return _MW_B; }
            set
            {
                if (_MW_B != value)
                {
                    _MW_B = value;
                    //Notify("MW_B");
                }
            }
        }
        private double _MW_C;
        [XmlIgnore]
        public double MW_C
        {
            get { return _MW_C; }
            set
            {
                if (_MW_C != value)
                {
                    _MW_C = value;
                    //Notify("MW_C");
                }
            }
        }
        private double _MW_3P;
        [XmlIgnore]
        public double MW_3P
        {
            get { return _MW_3P; }
            set
            {
                if (_MW_3P != value)
                {
                    _MW_3P = value;
                    //Notify("MW_N");
                }
            }
        }
        private double _PF_A;
        [XmlIgnore]
        public double PF_A
        {
            get { return _PF_A; }
            set
            {
                if (_PF_A != value)
                {
                    _PF_A = value;
                    //Notify("PF_A");
                }
            }
        }
        private double _PF_B;
        [XmlIgnore]
        public double PF_B
        {
            get { return _PF_B; }
            set
            {
                if (_PF_B != value)
                {
                    _PF_B = value;
                    //Notify("PF_B");
                }
            }
        }
        private double _PF_C;
        [XmlIgnore]
        public double PF_C
        {
            get { return _PF_C; }
            set
            {
                if (_PF_C != value)
                {
                    _PF_C = value;
                    //Notify("PF_C");
                }
            }
        }
        private double _PF_3P;
        [XmlIgnore]
        public double PF_3P
        {
            get { return _PF_3P; }
            set
            {
                
                if (_PF_3P != value)
                {
                    _PF_3P = value;
                    //Notify("PF_N");
                }
                if (controlbyNo_Cosphi_Q == eControlType.Cosphi)
                {
                    eAlertOpenClose et = CheckCosphi();
                    if ( et == eAlertOpenClose.Close)
                    {
                        Notify("CloseTuBu");
                    }
                    else if (et == eAlertOpenClose.Open)
                    {
                        Notify("OpenTuBu");
                    }
                }
                
            }
        }
        // khong dung nhung thong so tu day xuong
        /*
        private double _MAG_A;
        [XmlIgnore]
        public double MAG_A
        {
            get { return _MAG_A; }
            set
            {
                if (_MAG_A != value)
                {
                    _MAG_A = value;
                    Notify("MAG_A");
                }
            }
        }
        private double _MAG_B;
        [XmlIgnore]
        public double MAG_B
        {
            get { return _MAG_B; }
            set
            {
                if (_MAG_B != value)
                {
                    _MAG_B = value;
                    Notify("MAG_B");
                }
            }
        }
        private double _MAG_C;
        [XmlIgnore]
        public double MAG_C
        {
            get { return _MAG_C; }
            set
            {
                if (_MAG_C != value)
                {
                    _MAG_C = value;
                    Notify("MAG_C");
                }
            }
        }
        private double _MAG_N;
        [XmlIgnore]
        public double MAG_N
        {
            get { return _MAG_N; }
            set
            {
                if (_MAG_N != value)
                {
                    _MAG_N = value;
                    Notify("MAG_N");
                }
            }
        }
        private double _MAG_G;
        [XmlIgnore]
        public double MAG_G
        {
            get { return _MAG_G; }
            set
            {
                if (_MAG_G != value)
                {
                    _MAG_G = value;
                    Notify("MAG_G");
                }
            }
        }
        private double _MAG_H;
        [XmlIgnore]
        public double MAG_H
        {
            get { return _MAG_H; }
            set
            {
                if (_MAG_H != value)
                {
                    _MAG_H = value;
                    Notify("MAG_H");
                }
            }
        }
        private double _ANG_A;
        [XmlIgnore]
        public double ANG_A
        {
            get { return _ANG_A; }
            set
            {
                if (_ANG_A != value)
                {
                    _ANG_A = value;
                    Notify("ANG_A");
                }
            }
        }
        private double _ANG_B;
        [XmlIgnore]
        public double ANG_B
        {
            get { return _ANG_B; }
            set
            {
                if (_ANG_B != value)
                {
                    _ANG_B = value;
                    Notify("ANG_B");
                }
            }
        }
        private double _ANG_C;
        [XmlIgnore]
        public double ANG_C
        {
            get { return _ANG_C; }
            set
            {
                if (_ANG_C != value)
                {
                    _ANG_C = value;
                    Notify("ANG_C");
                }
            }
        }
        private double _ANG_N;
        [XmlIgnore]
        public double ANG_N
        {
            get { return _ANG_N; }
            set
            {
                if (_ANG_N != value)
                {
                    _ANG_N = value;
                    Notify("ANG_N");
                }
            }
        }
        private double _ANG_G;
        [XmlIgnore]
        public double ANG_G
        {
            get { return _ANG_G; }
            set
            {
                if (_ANG_G != value)
                {
                    _ANG_G = value;
                    Notify("ANG_G");
                }
            }
        }
        private double _ANG_H;
        [XmlIgnore]
        public double ANG_H
        {
            get { return _ANG_H; }
            set
            {
                if (_ANG_H != value)
                {
                    _ANG_H = value;
                    Notify("ANG_H");
                }
            }
        }*/
        //private double _voltsValue_MAG_;
        //[XmlIgnore]
        //public double VoltsValue_MAG_
        //{
        //    get { return _voltsValue_MAG_; }
        //    set
        //    {
        //        if (_voltsValue_MAG_ != value)
        //        {
        //            _voltsValue_MAG_ = value;
        //            Notify("VoltsValue_MAG_");
        //        }
        //    }
        //}
        #endregion new fields for Recloser351
       
        
       

        public override void CheckAlert()
        {
            Notify("Alert", false);
            base.CheckAlert();
        }
        private DateTime tmpDateTimeMax = DateTime.MinValue;
        private DateTime tmpDateTimeMin = DateTime.MinValue;
        private int secondcountmaxclose =0;
        private int secondcountminopen = 0;
        //CheckCosphi is called every one second
        // have to check cosphi every one second, cannot check cosphi when cosphi value change because
        // the timer SecondWait must count when cosphi reach max or min value

        public override eAlertOpenClose CheckCosphi()
        {
            //_powerFactor
            try
            {
                if (_PF_3P > this.MaxClose)
                {
                    secondcountminopen = 0;
                    tmpDateTimeMin = DateTime.MinValue;
                    if (tmpDateTimeMax == DateTime.MinValue) //secondcoun..==0
                    {
                        tmpDateTimeMax = DateTime.Now;
                    }
                    else
                    {
                        secondcountmaxclose = (int)(DateTime.Now - tmpDateTimeMax).TotalSeconds;
                    }
                    if (secondcountmaxclose >= SecondWait)
                    {
                        secondcountmaxclose = 0;
                        tmpDateTimeMax = DateTime.MinValue;
                        return eAlertOpenClose.Close;
                    }

                }
                else if (_PF_3P < this.MinOpen)
                {
                    secondcountmaxclose = 0;
                    tmpDateTimeMax = DateTime.MinValue;

                    if (tmpDateTimeMin == DateTime.MinValue)
                    {
                        tmpDateTimeMin = DateTime.Now;
                    }
                    else
                    {
                        secondcountminopen = (int)(DateTime.Now - tmpDateTimeMin).TotalSeconds;
                    }
                    if (secondcountminopen >= SecondWait)
                    {
                        secondcountminopen = 0;
                        tmpDateTimeMin = DateTime.MinValue;
                        return eAlertOpenClose.Open;
                    }



                }
                else  // _powerFactor(Cosphi) lies in the middle
                {
                    secondcountminopen = 0;
                    secondcountmaxclose = 0;
                    tmpDateTimeMin = DateTime.MinValue;
                    tmpDateTimeMax = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CheckCosphi", ex.Message);
                secondcountminopen = 0;
                secondcountmaxclose = 0;
                tmpDateTimeMin = DateTime.MinValue;
                tmpDateTimeMax = DateTime.MinValue;
                return eAlertOpenClose.None;
            }



            return eAlertOpenClose.None;
        }

        public eAlertOpenClose CheckQ()
        {
            //Q
            try
            {
                if (_Q_MVAR_3P > this.MaxClose)
                {
                    secondcountminopen = 0;
                    tmpDateTimeMin = DateTime.MinValue;
                    if (tmpDateTimeMax == DateTime.MinValue) //secondcoun..==0
                    {
                        tmpDateTimeMax = DateTime.Now;
                    }
                    else
                    {
                        secondcountmaxclose = (int)(DateTime.Now - tmpDateTimeMax).TotalSeconds;
                    }
                    if (secondcountmaxclose >= SecondWait)
                    {
                        secondcountmaxclose = 0;
                        tmpDateTimeMax = DateTime.MinValue;
                        return eAlertOpenClose.Close;
                    }

                }
                else if (_Q_MVAR_3P < this.MinOpen)
                {
                    secondcountmaxclose = 0;
                    tmpDateTimeMax = DateTime.MinValue;

                    if (tmpDateTimeMin == DateTime.MinValue)
                    {
                        tmpDateTimeMin = DateTime.Now;
                    }
                    else
                    {
                        secondcountminopen = (int)(DateTime.Now - tmpDateTimeMin).TotalSeconds;
                    }
                    if (secondcountminopen >= SecondWait)
                    {
                        secondcountminopen = 0;
                        tmpDateTimeMin = DateTime.MinValue;
                        return eAlertOpenClose.Open;
                    }



                }
                else  // Q lies in the middle
                {
                    secondcountminopen = 0;
                    secondcountmaxclose = 0;
                    tmpDateTimeMin = DateTime.MinValue;
                    tmpDateTimeMax = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CheckQ", ex.Message);
                secondcountminopen = 0;
                secondcountmaxclose = 0;
                tmpDateTimeMin = DateTime.MinValue;
                tmpDateTimeMax = DateTime.MinValue;
                return eAlertOpenClose.None;
            }



            return eAlertOpenClose.None;
        }

       
        public override void CommandClose(bool auto)
        {
            //button1.Enabled = false;
            //serialPort.Write(new byte[] { (byte)'C', (byte)'L', (byte)'O', 0x0D }, 0, 4);
            //Thread.Sleep(500);
            //String CLOans = serialPort.ReadExisting();
            //txtReceive.Text = CLOans;
            //if (CLOans.IndexOf("(Y/N) ?") > 0)/*Close Breaker  (Y/N) ?*/
            //{
            //    serialPort.Write(new byte[] { (byte)'Y', 0x0D }, 0, 2);
            //    Thread.Sleep(500);
            //    String Areyousure = serialPort.ReadExisting();
            //    txtReceive.Text = Areyousure;
            //    if (Areyousure.IndexOf("(Y/N) ?") > 0)/*Are you sure (Y/N) ?*/
            //    {
            //        serialPort.Write(new byte[] { (byte)'Y', 0x0D }, 0, 2);
            //    }

            //}
            //serialPort.ReadExisting();
            //button1.Enabled = true;
            timerBetweenEachRequest.Enabled = false;
            timerBetweenEachRequest.Stop();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();

            try
            {
                Listener.Send(new byte[] { (byte)'C', (byte)'L', (byte)'O', 0x0D });
                //System.Threading.Thread.Sleep(10);
                Listener.Send(new byte[] { (byte)'Y', 0x0D });
            }
            catch (Exception ex)
            {
            }
            timerBetweenEachRequest.Enabled = true;
            timerBetweenEachRequest.Start();
        }
        public override void CommandOpen(bool auto)
        {
            
            
            //button2.Enabled = false;
            //serialPort.Write(new byte[] { (byte)'O', (byte)'P', (byte)'E', 0x0D }, 0, 4);
            //Thread.Sleep(500);
            //String OPEans = serialPort.ReadExisting();
            //txtReceive.Text = OPEans;
            //if (OPEans.IndexOf("(Y/N) ?") > 0)/*Open Breaker  (Y/N) ?*/
            //{
            //    serialPort.Write(new byte[] { (byte)'Y', 0x0D }, 0, 2);
            //    Thread.Sleep(500);
            //    String OPEAreyousure = serialPort.ReadExisting();
            //    txtReceive.Text = OPEAreyousure;
            //    if (OPEAreyousure.IndexOf("(Y/N) ?") > 0)/*Are you sure (Y/N) ?*/
            //    {
            //        serialPort.Write(new byte[] { (byte)'Y', 0x0D }, 0, 2);
            //    }

            //}
            //button2.Enabled = true;
            timerBetweenEachRequest.Enabled = false;
            timerBetweenEachRequest.Stop();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();
            try
            {
                Listener.Send(new byte[] { (byte)'O', (byte)'P', (byte)'E', 0x0D });
                //System.Threading.Thread.Sleep(10);
                Listener.Send(new byte[] { (byte)'Y', 0x0D });
            }
            catch (Exception ex)
            {
            }
            timerBetweenEachRequest.Enabled = true;
            timerBetweenEachRequest.Start();
        }
        public override void CommandReclose()
        {
            
        }
        public override void SetTime()
        {
            //set time

            String Ti = DateTime.Now.ToLongTimeString();
            //serialPort.Write(new byte[] { (byte)'T', (byte)'I', (byte)'M', 0x20 }, 0, 4);
            this.Listener.Send(new byte[] { (byte)'T', (byte)'I', (byte)'M', 0x20 });
            //serialPort.Write(Ti.Trim());
            this.Listener.Send(Ultility.FromString(Ti));
            //serialPort.Write(new byte[] { 0x0D }, 0, 1);
            this.Listener.Send(new byte[] { 0x0D });
            //Thread.Sleep(500);
            //String TiAnsSet = serialPort.ReadExisting();

            //txtReceive.Text = TiAnsSet;
            
            // set date
            
            //String Da = txtDate.Text;
            String Da = DateTime.Now.ToString("MM/dd/yy");
            //serialPort.Write(new byte[] { (byte)'D', (byte)'A', (byte)'T', 0x20 }, 0, 4);
            this.Listener.Send(new byte[] { (byte)'D', (byte)'A', (byte)'T', 0x20 });
            //serialPort.Write(Da.Trim());
            this.Listener.Send(Ultility.FromString(Da));
            //serialPort.Write(new byte[] { 0x0D }, 0, 1);
            this.Listener.Send(new byte[] { 0x0D });
            //Thread.Sleep(500);
            //String DaAns = serialPort.ReadExisting();

            //txtReceive.Text = DaAns;
            
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
                //DeviceId,Type,LastUpdate,Alert,Mo,Dong,DeviceTime,
                strFields = "DeviceId,Type,Date,Alert,Mo,Dong,DeviceTime,VMAG_A,VMAG_B,VMAG_C,VMAG_S,VangA,VangB,VangC,VangS," +
                    "ImagA,ImagB,ImagC,ImagN,ImagG,Iang_A,Iang_B,Iang_C,Iang_N,Iang_G,Q_MVAR_A,Q_MVAR_B,Q_MVAR_C,Q_MVAR_3P" +
                    ",MW_A,MW_B,MW_C,MW_3P,PF_A,PF_B,PF_C,PF_3P \r\n";
                strvalues = DeviceID.ToString() + "," +
                    DeviceType.ToString() + "," +
                    LastUpdated.ToString() + "," +
                    AlertVal.ToString() + "," +
                     Status_Open.ToString() + "," +
                    Status_Close.ToString() + "," +
                    ((DeviceTime.HasValue && DeviceTime.Value > DateTime.MinValue) ? DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "") + "," +
                    _voltsValue_MAG_A.ToString() + "," +
                    _voltsValue_MAG_B.ToString() + "," +
                    _voltsValue_MAG_C.ToString() + "," +
                    _voltsValue_MAG_S.ToString() + "," +
                    _vang_A.ToString() + "," +
                    _vang_B.ToString() + "," +
                    _vang_C.ToString() + "," +
                    _vang_S.ToString() + "," +
                    _imag_A.ToString() + "," +
                    _imag_B.ToString() + "," +
                    _imag_C.ToString() + "," +
                    _imag_N.ToString() + "," +
                    _imag_G.ToString() + "," +
                    _iang_A.ToString() + "," +
                    _iang_B.ToString() + "," +
                    _iang_C.ToString() + "," +
                    _iang_N.ToString() + "," +
                    _iang_G.ToString() + "," +
                    _Q_MVAR_A.ToString() + "," +
                    _Q_MVAR_B.ToString() + "," +
                    _Q_MVAR_C.ToString() + "," +
                    _Q_MVAR_3P.ToString() + "," +
                    _MW_A.ToString() + "," +
                    _MW_B.ToString() + "," +
                    _MW_C.ToString() + "," +
                    _MW_3P.ToString() + "," +
                    _PF_A.ToString() + "," +
                    _PF_B.ToString() + "," +
                    _PF_C.ToString() + "," +
                    _PF_3P.ToString();
                   
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
                    LogService.WriteError("WriteRow Recloser351 1", e.Message); 
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("WriteRow Recloser351", ex.Message);
            }
        }
        protected override void StartSendingRequests()
        {
            // send connect command and send login 
            // then let timerRquest start 
            
            try
            {
                if (acceptSendingRequest == false)
                {
                    acceptSendingRequest = sendConnectCommand();
                }
                //LogService.WriteInfo("acceptSendingRequest=" + acceptSendingRequest, "");
                //timerBetweenEachRequest.Enabled = false;
                //timerBetweenEachRequest.Stop();
                //timerBetweenEachPoll.Enabled = false;
                //timerBetweenEachPoll.Stop();

                //if (acceptSendingRequest)
                //{
                //    timerBetweenEachRequest.Enabled = true;
                //    timerBetweenEachRequest.Start();
                //    timerBetweenEachPoll.Enabled = false;
                //    timerBetweenEachPoll.Stop();
                //}
                //else
                //{
                //    timerBetweenEachRequest.Enabled = false;
                //    timerBetweenEachRequest.Stop();
                //    timerBetweenEachPoll.Enabled = true;
                //    timerBetweenEachPoll.Start();
                //    this.CommStatus = "failed authentication";
                //    return;
                //}
                //or send //MET1
                // then send TIM
                // then send Status
                //BeginInvoke(new MethodInvoker(ShowResult));
            }
            catch (Exception ex)
            {
                LogService.WriteError("Recloser351R_StartSendingRequest", ex.ToString());

            }
            finally
            {
                //timerBetweenEachRequest.Start();
                //timerBetweenEachPoll.Start();

            }
            //dataReceiveList.Clear();
            //dataReceiveList.Add("0");
            //dataReceiveList.Add("1");
            //dataReceiveList.Add("2");
            base.StartSendingRequests(); //-->
            //timerBetweenEachRequest.Enabled = true;
            //timerBetweenEachRequest.Start();
            //timerBetweenEachPoll.Enabled = false;
            //timerBetweenEachPoll.Stop();
            //_iRequest = 0;

            //WaitForNextPoll();
        }
        
      /*  protected void SendRequests()
        {
            if (_request != null && _request.Count > 0)
            {
                if (_iRequest >= _request.Count)
                {
                    WaitForNextPoll();
                    return;
                }
                try
                {
                    if (!string.IsNullOrEmpty(this.StartPackage))
                    {
                        var s = this.StartPackage.Trim();
                        if (s != string.Empty)
                        {
                            this.Listener.Send(Ultility.FromHex(s));
                            //System.Threading.Thread.Sleep(latency);
                        }
                    }

                    var item = _request[_iRequest];
                    _iRequest++;
                    if (item.Used)
                    {
                        this.Listener.Send(item.Getbytes());
                        // recheck Di
                        //System.Threading.Thread.Sleep(2000); // thu *1000
                    }

                }
                catch (Exception ex)
                {
                    LogService.Logger.Error(string.Format("Recloser {0}, port {1} Poll error", this.DeviceType, this.Port), ex);
                }

            }
        }*/
        
        public bool sendConnectCommand()
        {
            try
            {
                timerBetweenEachPoll.Stop();
                timerBetweenEachRequest.Stop();

                //gui nhieu lan duoc
                //serialPort.Write(new byte[] { 0x18, 0x18, 0x18 }, 0, 3);
                Listener.Send(new byte[] { 0x18, 0x18, 0x18 });
                sleep(2);
                // Thread.Sleep(200);
                //dataReceiveList.Add(readTo("=>"));
                //string scan = serialPort.ReadExisting();
                //serialPort.Write(new byte[] { 0x18, 0x18, 0x18 }, 0, 3);
                Listener.Send(new byte[] { 0x18, 0x18, 0x18 });
                sleep(2);
                //Thread.Sleep(200);
                //dataReceiveList.Add(readTo("=>"));
                //String Devco = serialPort.ReadExisting();

                //len CR LF chi gui 1 lan sau scan
                //serialPort.Write(new byte[] { 0x0D, 0x4A }, 0, 2);
                Listener.Send(new byte[] { 0x0D, 0x4A });
                sleep(2);
                //Thread.Sleep(200);
                //dataReceiveList.Add(readTo("=>"));
                //Devco += serialPort.ReadExisting();

                //serialPort.Write(new byte[] { 0x18, 0x18, 0x18, 0x0D }, 0, 4);
                Listener.Send(new byte[] { 0x18, 0x18, 0x18, 0x0D });
                sleep(2);
                //Thread.Sleep(200);
                //dataReceiveList.Add(readTo("=>"));
                //Devco += serialPort.ReadExisting();

                //login ACC
                //serialPort.Write(new byte[] { 0x41, 0x43, 0x43, 0x0d }, 0, 4);
                Listener.Send(new byte[] { 0x41, 0x43, 0x43, 0x0d });
                sleep(2);
                //Thread.Sleep(200);
                //dataReceiveList.Add(readTo("=>"));
                //String AccAns = serialPort.ReadExisting();

                //if (AccAns_askPassword)/*co hoi pass*/
                //{
                //send Pass Level1 : OTTER
                //serialPort.Write(new byte[] { 0x4F, 0x54, 0x54, 0x45, 0x52, 0x0D }, 0, 6);
                Listener.Send(new byte[] { 0x4F, 0x54, 0x54, 0x45, 0x52, 0x0D });
                sleep(2);
                //Thread.Sleep(500);
                //dataReceiveList.Add(readTo("=>"));
                AccAns_askPassword = false;
                //String AccpassOK = serialPort.ReadExisting();
                //}


                //login 2AC
                //serialPort.Write(new byte[] { 0x32, 0x41, 0x43, 0x0D }, 0, 4);
                Listener.Send(new byte[] { 0x32, 0x41, 0x43, 0x0D });
                sleep(2);
                //Thread.Sleep(500);
                //dataReceiveList.Add(readTo("=>"));
                //String Ans2AccReq = serialPort.ReadExisting();
                //if (Ans2AccReq.IndexOf("Password") > 0)/*co hoi pass*/
                //if (AccAns_askPassword)
                //{
                //send Pass Level2 : TAIL
                //serialPort.Write(new byte[] { 0x54, 0x41, 0x49, 0x4C, 0x0D }, 0, 5);
                Listener.Send(new byte[] { 0x54, 0x41, 0x49, 0x4C, 0x0D });
                sleep(2);
                //Thread.Sleep(500);
                //dataReceiveList.Add(readTo("=>"));
                //String Ans2ACReg = serialPort.ReadExisting();
                //}
                return true;
                //txtReceive.Text = "Ready";

                //MET1 
                //      serialPort.Write(new byte[] { 0x4D, 0x45, 0x54,0x31, 0x0D }, 0, 5);
                //       Thread.Sleep(2500);
                //       String Met1 = serialPort.ReadExisting();
                //      int i = 1;

                //serialPort.Write("MET 1\r");
            }
            catch (Exception ex)
            {
                LogService.WriteError("Recloser351_sendConnectCommand", ex.ToString());
                return false;
            }

            finally
            {
                timerBetweenEachRequest.Enabled = true;
                timerBetweenEachRequest.Start();
                timerBetweenEachPoll.Enabled = false;
                timerBetweenEachPoll.Stop();
                _iRequest = 0;
            }



        }
        public void sendTestMET()
        {
            this.Listener.Send(new byte[] { 0x4D, 0x45, 0x54, 0x31, 0x0D });
            
        }
    }
}
