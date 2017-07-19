using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using TcpComm;

namespace RecloserAcq.Device
{    
    public class Elster1700: RecloserBase
    {

        [System.Runtime.InteropServices.DllImport("Encrypt.dll")]
        public static extern int ENCRYPT(byte[] szEncryptPwd, byte[] szPassword, byte[] szKey);

        #region Constants
		              
	    #endregion

        #region Receive Pattern
        // Current
        // .*.MA#.6296,100.~.*.MB#.7141,100.~.*.MC#.6952,100.~.*.ME#.845,100.~.880C.
        public const string Pattern_Current = @"(?<Type>M[ABCE]#)\n(?<Value>\d{3,5}),(?<Factor>\d{3})";

        // Power
        // .*.DID-281.0.~.*.DID-345.1.~.*.KW#.2592737,1000.~.*.PWFT#.97,100.~.*.FS5060.0.~.1766.					           
        // .*.UOF12#.50130,1000.~.*.MN.4.0.~.*.MK#.2661267,1000.~.*.MR#.582373,1000.~.*.U1.1.~.DC75.
        public const string Pattern_Power = @"(?<Type>KW#|PWFT#|MK#|MR#)\n(?<Value>\d{2,7}),(?<Factor>\d{3,4})";

        // Date time & Status
        // .*.CK.21.1.09:30:56 05/06/12.~.*.AR.1.~.*.CL.0.~.*.+B-CL.0.~.*.EE.1.~.EFAC.
        public const string Pattern_Status = @"(?<Type>SE|SA|EE|CB)\n(?<Value>0|1)";

        //"CK\n21\n1\n09:30:55 05/06/12"
        public const string Pattern_DateTime = @"CK\n21\n1\n(?<Time>\d{2}:\d{2}:\d{2})\s(?<Date>\d{2}/\d{2}/\d{2})";

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

      
        
        #region Power
        //// MK#
        //private int _apparentPower;
        //[XmlIgnore]
        //public int ApparentPower
        //{
        //    get { return _apparentPower; }
        //    set
        //    {
        //        if (_apparentPower != value)
        //        {
        //            _apparentPower = value;
        //            Notify("ApparentPower");
        //        }
        //    }
        //}

        // MR#
        //private int _reactivePower;
        //[XmlIgnore]
        //public int ReactivePower
        //{
        //    get { return _reactivePower; }
        //    set
        //    {
        //        if (_reactivePower != value)
        //        {
        //            _reactivePower = value;
        //            Notify("ReactivePower");
        //        }
        //    }
        //}

        //// kw# 
        //private int _realPower;
        //[XmlIgnore]
        //public int RealPower
        //{
        //    get { return _realPower; }
        //    set
        //    {
        //        if (_realPower != value)
        //        {
        //            _realPower = value;
        //            Notify("RealPower");
        //        }
        //    }
        //}

        // PWFTH#
        //private float _powerFactor;
        //[XmlIgnore]
        //public float PowerFactor
        //{
        //    get { return _powerFactor; }
        //    set
        //    {
        //        if (_powerFactor != value)
        //        {
        //            _powerFactor = value;
        //            Notify("PowerFactor");
        //        }
        //    }
        //}

        #endregion
        #region New Fields for Elster 1700
        private int _baudRate;
        public int BaudRate
        {
            get { return _baudRate; }
            set
            {
                if (_baudRate != value)
                {
                    _baudRate = value;
                    Notify("BaudRate");
                }
            }
        }
        private double _volt_A;
        [XmlIgnore]
        public double Volt_A
        {
            get { return _volt_A; }
            set
            {
                if (_volt_A != value)
                {
                    _volt_A = value;
                    Notify("Volt_A");
                }
            }
        }
        private double _volt_B;
        [XmlIgnore]
        public double Volt_B
        {
            get { return _volt_B; }
            set
            {
                if (_volt_B != value)
                {
                    _volt_B = value;
                    Notify("Volt_B");
                }
            }
        }
        private double _volt_C;
        [XmlIgnore]
        public double Volt_C
        {
            get { return _volt_C; }
            set
            {
                if (_volt_C != value)
                {
                    _volt_C = value;
                    Notify("Volt_C");
                }
            }
        }
        private double _volt_Total;
        [XmlIgnore]
        public double Volt_Total
        {
            get { return _volt_Total; }
            set
            {
                if (_volt_Total != value)
                {
                    _volt_Total = value;
                    Notify("Volt_Total");
                }
            }
        }
        private double _ample_Total;
        [XmlIgnore]
        public double Ample_Total
        {
            get { return _ample_Total; }
            set
            {
                if (_ample_Total != value)
                {
                    _ample_Total = value;
                    Notify("Ample_Total");
                }
            }
        }
        private double _ample_C;
        [XmlIgnore]
        public double Ample_C
        {
            get { return _ample_C; }
            set
            {
                if (_ample_C != value)
                {
                    _ample_C = value;
                    Notify("Ample_C");
                }
            }
        }
        private double _ample_B;
        [XmlIgnore]
        public double Ample_B
        {
            get { return _ample_B; }
            set
            {
                if (_ample_B != value)
                {
                    _ample_B = value;
                    Notify("Ample_B");
                }
            }
        }
        private double _ample_A;
        [XmlIgnore]
        public double Ample_A
        {
            get { return _ample_A; }
            set
            {
                if (_ample_A != value)
                {
                    _ample_A = value;
                    Notify("Ample_A");
                }
            }
        }
        private double _reActivePower_A;
        [XmlIgnore]
        public double ReActivePower_A
        {
            get { return _reActivePower_A; }
            set
            {
                if (_reActivePower_A != value)
                {
                    _reActivePower_A = value;
                    Notify("ReActivePower_A");
                }
            }
        }
        private double _reActivePower_B;
        [XmlIgnore]
        public double ReActivePower_B
        {
            get { return _reActivePower_B; }
            set
            {
                if (_reActivePower_B != value)
                {
                    _reActivePower_B = value;
                    Notify("ReActivePower_B");
                }
            }
        }
        private double _reActivePower_C;
        [XmlIgnore]
        public double ReActivePower_C
        {
            get { return _reActivePower_C; }
            set
            {
                if (_reActivePower_C != value)
                {
                    _reActivePower_C = value;
                    Notify("ReActivePower_C");
                }
            }
        }
        private double _reActivePower_Total;
        [XmlIgnore]
        public double ReActivePower_Total
        {
            get { return _reActivePower_Total; }
            set
            {
                if (_reActivePower_Total != value)
                {
                    _reActivePower_Total = value;
                   // Notify("ReActivePower_Total");
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
        private double _activePower_A;
        [XmlIgnore]
        public double ActivePower_A
        {
            get { return _activePower_A; }
            set
            {
                if (_activePower_A != value)
                {
                    _activePower_A = value;
                    Notify("ActivePower_A");
                }
            }
        }
        private double _activePower_B;
        [XmlIgnore]
        public double ActivePower_B
        {
            get { return _activePower_B; }
            set
            {
                if (_activePower_B != value)
                {
                    _activePower_B = value;
                    Notify("ActivePower_B");
                }
            }
        }
        private double _activePower_C;
        [XmlIgnore]
        public double ActivePower_C
        {
            get { return _activePower_C; }
            set
            {
                if (_activePower_C != value)
                {
                    _activePower_C = value;
                    Notify("ActivePower_C");
                }
            }
        }
        private double _activePower_Total;
        [XmlIgnore]
        public double ActivePower_Total
        {
            get { return _activePower_Total; }
            set
            {
                if (_activePower_Total != value)
                {
                    _activePower_Total = value;
                   // Notify("ActivePower_Total");
                }
            }
        }
        private double _powerFactor_A;
        [XmlIgnore]
        public double PowerFactor_A
        {
            get { return _powerFactor_A; }
            set
            {
                if (_powerFactor_A != value)
                {
                    _powerFactor_A = value;
                    //Notify("PowerFactor_A");
                }
            }
        }
        private double _powerFactor_B;
        [XmlIgnore]
        public double PowerFactor_B
        {
            get { return _powerFactor_B; }
            set
            {
                if (_powerFactor_B != value)
                {
                    _powerFactor_B = value;
                    //Notify("PowerFactor_B");
                }
            }
        }
        private double _powerFactor_C;
        [XmlIgnore]
        public double PowerFactor_C
        {
            get { return _powerFactor_C; }
            set
            {
                if (_powerFactor_C != value)
                {
                    _powerFactor_C = value;
                    //Notify("PowerFactor_C");
                }
            }
        }
        private double _powerFactor_Total;
        [XmlIgnore]
        public double PowerFactor_Total
        {
            //Cosphi
            get { return _powerFactor_Total; }
            set
            {
                if (_powerFactor_Total != value)
                {
                    _powerFactor_Total = value;
                    //Notify("PowerFactor_Total");
                }
                if (controlbyNo_Cosphi_Q == eControlType.Cosphi)
                {
                    eAlertOpenClose et = CheckCosphi();
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
        //private readonly List<String> replyList = new List<string>();
        private DateTime tmpDateTimeMax = DateTime.MinValue;
        private DateTime tmpDateTimeMin = DateTime.MinValue;
        
        
        public eAlertOpenClose CheckQ()
        {
            //Q
            try
            {
                
                if (_reActivePower_Total  > this.MaxClose)
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
                else if (_reActivePower_Total < this.MinOpen)
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
        public Elster1700(int port)
            : base(port)
        {
            _deviceTime = null;
            
            
        }

        public Elster1700():this(0)            
        { }

        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.Elster1700;
            }
        }
        private bool _acceptSendingRequest = false;
        
        public bool incorrectpassword = true;
        private string seed = string.Empty;
        public void GetSeed(string str)
        {
            //string pattern = "P0*(*)";
            //"<SOH>P0<STX>(66FF9688E27B0858)<ETX>"
            int i =str.IndexOf("<SOH>P0<STX>(");
            string strseed = string.Empty;
            if (i >= 0)
            {
                strseed  = str.Substring(i + 13, 16);
            }
            if (strseed != string.Empty)
            {
                seed = strseed;
                IfGetSeed();
            }
        }
        //private List<string> strItemList = new List<string>();
        private int indexItem;
        public void GetItem(string str)
        {
            
            //sample string :  <STX>(1AAA4A00)<ETX><STX>(1AAACA00)<ETX>p<STX>(1AAACA80)<ETX>x<STX>(9AAACA80)<ETX>p<STX>(00000000641875000000007249260000000073739700FF0000000000)<ETX>
            int i = str.LastIndexOf('(');
            int j = str.LastIndexOf(')');
            if (j - i -1 == 56)
            {
                //string data = str.Substring(i + 1, 56);
                //if (indexItem == 0) 
                int tmpi = str.IndexOf("9AAACA80");
                int tmpi1;
                int tmpi2;
                if(tmpi>=0)
                {
                    tmpi1 = str.IndexOf('(', tmpi);
                    tmpi2 = str.IndexOf(')', tmpi1);
                    if (tmpi2 >= tmpi1 + 54)
                    {
                        string data = str.Substring(tmpi1 + 1, 56);
                        var ampe = ParseAmple(data);
                        Ample_A = ampe.PhaseA;
                        Ample_B = ampe.PhaseB;
                        Ample_C = ampe.PhaseC;
                        Ample_Total = ampe.Total;
                    }
                }
                tmpi = str.IndexOf("9BABCB80");
                if(tmpi>=0)//indexItem==1)
                {
                    tmpi1 = str.IndexOf('(', tmpi);
                    tmpi2 = str.IndexOf(')', tmpi1);
                    if (tmpi2 >= tmpi1 + 54)
                    {
                        string data = str.Substring(tmpi1 + 1, 56);
                        var volt = ParseVolts(data);
                        Volt_A = volt.PhaseA;
                        Volt_B = volt.PhaseB;
                        Volt_C = volt.PhaseC;
                        Volt_Total = volt.Total;
                    }
                }
                tmpi = str.IndexOf("9CACCC8C");
                if(tmpi>=0)//indexItem==1)
                {
                    tmpi1 = str.IndexOf('(', tmpi);
                    tmpi2 = str.IndexOf(')', tmpi1);
                    if (tmpi2 >= tmpi1 + 54)
                    {
                        string data = str.Substring(tmpi1 + 1, 56);

                        var activepower = ParseActivePower(data);
                        ActivePower_A = activepower.PhaseA;
                        ActivePower_B = activepower.PhaseB;
                        ActivePower_C = activepower.PhaseC;
                        ActivePower_Total = activepower.Total;
                    }
                }
                tmpi = str.IndexOf("9DADCD8D");
                if(tmpi>=0)//indexItem==1)
                {
                    tmpi1 = str.IndexOf('(', tmpi);
                    tmpi2 = str.IndexOf(')', tmpi1);
                    if (tmpi2 >= tmpi1 + 54)
                    {
                        string data = str.Substring(tmpi1 + 1, 56);
                        var reactivepower = ParseReActivePower(data);
                        ReActivePower_A = reactivepower.PhaseA;
                        ReActivePower_B = reactivepower.PhaseB;
                        ReActivePower_C = reactivepower.PhaseC;
                        ReActivePower_Total = reactivepower.Total;
                    }
                }
                tmpi = str.IndexOf("93A3C383");
                if(tmpi>=0)//indexItem==1)
                {
                    tmpi1 = str.IndexOf('(', tmpi);
                    tmpi2 = str.IndexOf(')', tmpi1);
                    if (tmpi2 >= tmpi1 + 54)
                    {
                        string data = str.Substring(tmpi1 + 1, 56);


                        var powerfactor = ParsePowerFactor(data);
                        PowerFactor_A = powerfactor.PhaseA;
                        PowerFactor_B = powerfactor.PhaseB;
                        PowerFactor_C = powerfactor.PhaseC;
                        PowerFactor_Total = powerfactor.Total;
                    }
                }
                //indexItem++;
                this.LastUpdated = DateTime.Now;
            }
            
            
        }
        private ParseObject ParseReActivePower(String data)
        {
            var parse = Parse(data);
            parse.Name = "Reactive Power (kvar)";

            return parse;
        }

        private ParseObject ParseActivePower(String data)
        {
            var parse = Parse(data);
            parse.Name = "Active Power (kW)";

            return parse;
        }

        private ParseObject ParsePowerFactor(String data)
        {
            var parse = Parse(data);
            parse.Name = "Power Factor";

            return parse;
        }
        private ParseObject ParseAmple(String data)
        {
            var parse = Parse(data);
            parse.Name = "Ample";

            return parse;
        }
        private ParseObject ParseVolts(String data)
        {
            var parse = Parse(data);
            parse.Name = "Volts";

            return parse;
        }
        private ParseObject Parse(String data)
        {
            var parse = new ParseObject();

            parse.PhaseA = ParseValue(data, 0);
            parse.PhaseB = ParseValue(data, 14);
            parse.PhaseC = ParseValue(data, 28);
            parse.Total = ParseValue(data, 42);

            return parse;
        }
        private double ParseValue(String data, int start)
        {
            if (data[start + 2] == 'F' && data[start + 3] == 'F')
            {
                return double.NaN;
            }

            start = start + 2;

            var temp = data.Substring(start, 8);
            temp += "." + data.Substring(start + 8, 4);

            var value = Convert.ToDouble(temp);

            return value;
        }
        public override void UpdateData(byte[] data)
        {
            var reply = Ultility.GetAsciiString(data);
            
            var parse =
               reply.Replace("\x15", "<NAK>")
                    .Replace("\x06", "<ACK>")
                    .Replace("\x01", "<SOH>")
                    .Replace("\x02", "<STX>")
                    .Replace("\x03", "<ETX>")
                    .Replace("\x0D", "<CR>")
                    .Replace("\x0A", "<LF>");
           // replyList.Add(parse);
            //LogService.WriteInfo("ReplyList:", parse);
            GetSeed(parse);
            GetItem(parse);    
            
            //if (justsendConnectCommand == true)
            //{
            //    justsendConnectCommand = false;
            //    if (data[0] == 21) // 0x15 = NAK 
            //    {
            //        // denied 
            //        _acceptSendingRequest = false;
            //        this.CommStatus = "Access Denied";
            //        return;
            //    }
            //    else
            //    {
            //        _acceptSendingRequest = true;
            //        this.CommStatus = "Polling";
            //        IfAcceptConnectIsTrue();
            //        return;
            //        //if data.Count<byte> >
            //        //no return, continue to update data
            //    }
            //}
            if (justsendPassword == true)
            {
                justsendPassword = false;
                //if (data[0] != 6) // != ACK 
                //{
                //    // denied 
                //    incorrectpassword = true;
                //    this.CommStatus = "Incorrect Password";
                //    return;
                //}
                //else
                //{
                //    incorrectpassword = false;
                    this.CommStatus = "Polling";
                    IfSendPasswordOK();
                    //if data.Count<byte> >
                    //no return, continue to update data
                //}
            }
            
            if (this.CommStatus == "Access Denied")
            {
                this.CommStatus = "Polling";
            }
            try
            {
                WriteRow();
                //base.UpdateData(data);

            }
            catch (Exception ex)
            {
                LogService.WriteError("updatedata", ex.ToString());
            }
        }
        #region old function 
        /*
        private void GetValue(string s)
        {
            try
            {
                var found = GetCurrentandPower(s);

                if (!found)
                    GetStatus(s);

                if (!found)
                    GetOtherValue(s);
            }
            catch (Exception ex)
            {
                LogService.Logger.Error(string.Format("Elster1700.GetValue({0}) error", s), ex);
            }
        }

        private bool GetCurrentandPower(string s)
        {
            var re = new Regex(Pattern_Current);
            var match = re.Match(s);

            if (!match.Success)
            {
                re = new Regex(Pattern_Power);
                match = re.Match(s);
            }

            if (match.Success)
            {
                var type = match.Groups["Type"].Value;
                int v = Convert.ToInt32(match.Groups["Value"].Value);
                int f = Convert.ToInt32(match.Groups["Factor"].Value);
                int c = v / f;

                switch (type)
                {
                    case "MA#":
#if DEBUG
                        if (Testing == 1)
                        {
                            this.Amp12 = 0;
                        }
                        else if (Testing == 2)
                        {
                            this.Amp12 = MaxAmp +1;
                        }
                        else
                        {
                            this.Amp12 = v / f; //set it = 0 to test alert or set it > maxamp to test alert
                        }
#else
                            this.Amp12 = v / f; //set it = 0 to test alert or set it > maxamp to test alert
#endif 
                        break;

                    case "MB#":
                        this.Amp34 = v / f;
                        break;

                    case "MC#":
                        this.Amp56 = v / f;
                        break;

                    case "ME#":
                        this.AmpEarth = v / f;
                        break;

                    // Power
                    // .*.DID-281.0.~.*.DID-345.1.~.*.KW#.2592737,1000.~.*.PWFT#.97,100.~.*.FS5060.0.~.1766.					           
                    // .*.UOF12#.50130,1000.~.*.MN.4.0.~.*.MK#.2661267,1000.~.*.MR#.582373,1000.~.*.U1.1.~.DC75.
                    case "KW#":
                        this.RealPower = v / f;
                        break;

                    case "PWFT#":
                        this.PowerFactor = (float)v / f;
                        break;

                    case "MK#":
                        this.ApparentPower = v / f;
                        break;

                    case "MR#":
                        this.ReactivePower = v / f;
                        break;
                }

                return true;
            }

            return false;
        }

        private bool GetStatus(string s)
        {
            var re = new Regex(Pattern_Status);
            var match = re.Match(s);

            if (match.Success)
            {
                var type = match.Groups["Type"].Value;
                //var val = match.Groups["Value"].Value == "1" ? true : false;
                var val = match.Groups["Value"].Value == "1" ? false : true;
                switch (type)
                {
                    case "EE":
                        //this.Status_EarthTarget = val;
                        break;

                    case "SA":
                        break;

                    case "SE":
                        //this.Status_SEFTarget = val;
                        break;

                    case "CB":
                        this.Status_Open = val;
                        break;
                }

                return true;
            }

            return false;
        }

        private bool GetOtherValue(string s)
        {
            // Battery
            var re = new Regex(Pattern_Battery);
            var match = re.Match(s);
            if (match.Success)
            {
                this.Battery_1 = match.Groups["Battery"].Value == "1" ? "Normal" : "Fail";
                return true;
            }

            // Battery Voltage
            re = new Regex(Pattern_Battery_Voltage);
            match = re.Match(s);
            if (match.Success)
            {
                int v = Convert.ToInt32(match.Groups["Value"].Value);
                int f = Convert.ToInt32(match.Groups["Factor"].Value);
                float bv = (float)v / f;
                this.Battery_2 = bv.ToString("##.##"); ;
                return true;
            }

            // Operation
            re = new Regex(Pattern_Operation);
            match = re.Match(s);
            if (match.Success)
            {
                this.Operations = Convert.ToInt32(match.Groups["Operation"].Value);
                return true;
            }

            // DateTime
            re = new Regex(Pattern_DateTime);
            match = re.Match(s);
            if (match.Success)
            {
                // update device datetime
                var time = match.Groups["Time"].Value;
                var date = match.Groups["Date"].Value;
                DateTime dt;

                if(DateTime.TryParseExact(date + " " + time, "dd/MM/yy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dt))
                    this.DeviceTime = dt;
            }

            return false;
        }
        // end
        */
#endregion 
        #region Current        
        [XmlIgnore]
        public override int Amp12
        {
            get { return _amp12; }
            set
            {
                if (value == 0)
                {
                    // if just start up , _amp12 is 0 null 
                    // _amp12 still 0 and start time count
                    // else 
                    ///  if(_amp12 != value) keeps its value ->>> _amp12 keeps its value and starttime count to alert 
                    ///  else do not _start time count
                    setAlert(1, ref _startZeroA);
                }
                else if (value > MaxAmp)
                {
                    if (_amp12 != value)
                    {
                        _amp12 = value;
                        Notify("Amp12");
                        this.Phase_A = _amp12;
                        // starttime count
                    }// else starttime count already started ->> do nothing
                    setAlert(2, ref _startZeroA);
                }
                else
                {

                    if (_amp12 != value)
                    {
                        _amp12 = value;
                        Notify("Amp12");
                        this.Phase_A = _amp12;
                    }
                    _startZeroA = null;
                    //_alertVal = 0; do not set to 0, only reset manually to 0 after going alerted. 
                }
                /*if (_amp12 != value)
                {
                    _amp12 = value;
                    Notify("Amp12");

                    if (_amp12 == 0)
                    {
                        _startZeroA = DateTime.Now;
                    }
                    else
                    {
                        _startZeroA = null;
                        if (_amp12 <= MaxAmp)
                            this.Phase_A = _amp12;
                    }
                }*/
            }
        }
        
        [XmlIgnore]
        public override int Amp34
        {
            get { return _amp34; }
            set
            {
                if (value == 0)
                {
                    // if just start up , _amp34 is 0 null 
                    // _amp34 still 0 and start time count
                    // else 
                    ///  if(_amp34 != value) keeps its value ->>> _amp34 keeps its value and starttime count to alert 
                    ///  else do not _start time count
                    setAlert(1, ref _startZeroB);
                }
                else if (value > MaxAmp)
                {
                    if (_amp34 != value)
                    {
                        _amp34 = value;
                        Notify("Amp34");
                        this.Phase_B = _amp34;
                        // starttime count
                    }// else starttime count already started ->> do nothing
                    setAlert(2, ref _startZeroB);
                }
                else
                {

                    if (_amp34 != value)
                    {
                        _amp34 = value;
                        Notify("Amp34");
                        this.Phase_B = _amp34;
                    }
                    _startZeroB = null;
                    //_alertVal = 0; do not set to 0, only reset manually to 0 after going alerted. 
                }
                /*if (_amp34 != value)
                {
                    _amp34 = value;
                    Notify("Amp34");

                    if (_amp34 == 0)
                    {
                        _startZeroB = DateTime.Now;
                    }
                    else
                    {
                        _startZeroB = null;
                        if (_amp34 <= MaxAmp)
                            this.Phase_B = _amp34;
                    }
                }*/
            }
        }
                
        [XmlIgnore]
        public override int Amp56
        {
            get { return _amp56; }
            set
            {
                if (value == 0)
                {
                    // if just start up , _amp56 is 0 null 
                    // _amp56 still 0 and start time count
                    // else 
                    ///  if(_amp56 != value) keeps its value ->>> _amp56 keeps its value and starttime count to alert 
                    ///  else do not _start time count
                    setAlert(1, ref _startZeroC);
                }
                else if (value > MaxAmp)
                {
                    if (_amp56 != value)
                    {
                        _amp56 = value;
                        Notify("Amp56");
                        this.Phase_C = _amp56;
                        // starttime count
                    }// else starttime count already started ->> do nothing
                    setAlert(2, ref _startZeroC);
                }
                else
                {

                    if (_amp56 != value)
                    {
                        _amp56 = value;
                        Notify("Amp56");
                        this.Phase_C = _amp56;
                    }
                    _startZeroC = null;
                    //_alertVal = 0; do not set to 0, only reset manually to 0 after going alerted. 
                }
                /*if (_amp56 != value)
                {
                    _amp56 = value;
                    Notify("Amp56");

                    if (_amp56 == 0)
                    {
                        _startZeroC = DateTime.Now;
                    }
                    else
                    {
                        _startZeroC = null;
                        if (_amp56 <= MaxAmp)
                            this.Phase_C = _amp56;
                    }
                }*/
            }
        }                
        #endregion

        #region Last normal value of current
        
        private int _phase_A;
        [XmlIgnore]
        public int Phase_A
        {
            get { return _phase_A; }
            set
            {
                if (_phase_A != value)
                {
                    _phase_A = value;
                    Notify("Phase_A");
                }
            }
        }

        
        private int _phase_B;
        [XmlIgnore]
        public int Phase_B
        {
            get { return _phase_B; }
            set
            {
                if (_phase_B != value)
                {
                    _phase_B = value;
                    Notify("Phase_B");
                }
            }
        }

        
        private int _phase_C;
        [XmlIgnore]
        public int Phase_C
        {
            get { return _phase_C; }
            set
            {
                if (_phase_C != value)
                {
                    _phase_C = value;
                    Notify("Phase_C");
                }
            }
        }        
        #endregion
        
        //public override int AlertVal
        //{
        //    get
        //    {
        //        //if (this.Alert == false) return 0;
        //        return _alertVal;

        //    }
        //}
        //public override bool Alert
        //{
        //    get
        //    {
        //        if (this.Listener.IsConnected)
        //        {
        //            var al = IsAlert(_amp12, _startZeroA);

        //            if (!al) al = IsAlert(_amp34, _startZeroB);

        //            if (!al) al = IsAlert(_amp56, _startZeroC);

        //            return al;
        //        }
        //        else
        //            return false;
        //    }
        //}

        public override void CheckAlert()
        {
            Notify("Alert", false);
            base.CheckAlert();
        }
        private int secondcountmaxclose =0;
        private int secondcountminopen = 0;
        //CheckCosphi is called every one second
        // have to check cosphi every one second, cannot check cosphi when cosphi value change because
        // the timer SecondWait must count when cosphi reach max or min value
        public override eAlertOpenClose CheckCosphi()
        {
            //_powerFactor
            if (_powerFactor_Total > this.MaxClose)
            {
                secondcountminopen = 0;
                if (secondcountmaxclose >= SecondWait)
                {
                    secondcountmaxclose = 0;
                    return eAlertOpenClose.Close;
                }
                else
                {
                    secondcountmaxclose++;
                }
            }
            else 
            {
                secondcountmaxclose = 0;
                if (_powerFactor_Total < this.MinOpen)
                {
                    if (secondcountminopen >= SecondWait)
                    {
                        secondcountminopen = 0;
                        return eAlertOpenClose.Open;
                    }
                    else
                    {
                        secondcountminopen++;
                    }
                }
                else
                {
                    secondcountminopen = 0;
                }
            }

            return eAlertOpenClose.None;
        }
        

     

        //public void RefeshAlertTime()
        //{
        //    Amp12 = 0;
        //    Amp34 = 0;
        //    Amp56 = 0;

        //    _startZeroA = DateTime.Now;
        //    _startZeroB = DateTime.Now;
        //    _startZeroC = DateTime.Now;
        //}

    
       
       
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
                strFields = "DeviceId,Type,Date,Alert,Mo,Dong,DeviceTime,Volt_A,	Volt_B,	Volt_C,	Volt_Total," +
                    "Ample_A,	Ample_B,	Ample_C,	Ample_Total,	ReAP_A,	ReAP_B,	ReAP_C,	ReAP_Total,	" +
                    "AP_A,	AP_B,	AP_C,	AP_Total,PF_A,	PF_B,	PF_C,	PF_Total	\r\n";
                strvalues = DeviceID.ToString() + "," +
                    DeviceType.ToString() + "," +
                    LastUpdated.ToString() + "," +
                    AlertVal.ToString() + ",0,0," +
                     ((DeviceTime.HasValue && DeviceTime.Value > DateTime.MinValue) ? DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "") + "," +
                    _volt_A.ToString() + "," +
                    _volt_B.ToString() + "," +
                    _volt_C.ToString() + "," +
                    _volt_Total.ToString() + "," +
                    _ample_A.ToString() + "," +
                    _ample_B.ToString() + "," +
                    _ample_C.ToString() + "," +
                    _ample_Total.ToString() + "," +
                     _reActivePower_A.ToString() + "," +
                    _reActivePower_B.ToString() + "," +
                    _reActivePower_C.ToString() + "," +
                    _reActivePower_Total.ToString() + "," +
                _activePower_A.ToString() + "," +
                _activePower_B.ToString() + "," +
                _activePower_C.ToString() + "," +
                _activePower_Total.ToString() + "," +
                _powerFactor_A.ToString() + "," +
                _powerFactor_B.ToString() + "," +
                _powerFactor_C.ToString() + "," +
                _powerFactor_Total.ToString();
                
                
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
                    ;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("WriteRow Elster",ex.Message);
            }
        }
        protected override void StartSendingRequests()
        {
            timerBetweenEachRequest.Enabled = false;
            timerBetweenEachRequest.Stop();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();
            try
            {
                //replyList.Clear();
                //strItemList.Clear();
                indexItem = 0;
                _acceptSendingRequest = false;
                Listener.ReceiveBuffer.Reset();
                incorrectpassword = true;

                sendConnectCommand();

                
                
                    
            
                    
                    //end SendExChange byte[] seed, byte[] password
                
                
                //Logon();

                //ExChange();

                //ReadData();

                //LogOff();

                ////Save();

                //ParseOnline();

                //BeginInvoke(new MethodInvoker(ShowResult));
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (timerBetweenEachPoll.Interval < 3000 * 60)
                {
                    timerBetweenEachPoll.Interval = 3000*60;
                }
                timerBetweenEachPoll.Enabled = true;
                timerBetweenEachPoll.Start();
            }
            // each poll , device will send all request, no need for request timer
            // this device not use request xml
            
           

        }
        public void IfAcceptConnectIsTrue()
        {
            //if acceptSendingRequest still == false at this stage: -> 
                // do nothing and Wait for the next Polling
                // if acceptSendingRequest became true but is not updated at this stage
                // -->
                //DateTime apoint = DateTime.Now;
                //while (acceptSendingRequest == false && ((DateTime.Now - apoint).TotalMilliseconds <= 1000))
                //{
                //    //do waiting
                //}
                //if (_acceptSendingRequest == true)
                //{
                    SendBaudRate(); // send ACK and password confirm
                    SendExChangeRequest();
                    
                    //if (reply[0] != 6)
                    //{
                    //    throw new Exception("Incorrect Password");
                    //}
                    //apoint = DateTime.Now;
                    //while (incorrectpassword == true && ((DateTime.Now - apoint).TotalMilliseconds <= 1000))
                    //{
                    //    // donothing
                    //}
                    
                //}
        }
        private void SendAfterAskSeed()
        {
            //buff = ("\x01" + "P2" + "\x02" + "(0000000000000000)" + "\x03" + "b").ToCharArray();
            var buff = Ultility.FromString("\x01" + "P2" + "\x02" + "(0000000000000000)" + "\x03" + "b");
            //_serialPort.Write(buff, 0, buff.Length);
            this.Listener.Send(buff);
            //Read(_serialPort, 3000);
            //Wait();
            sleep(0.5);
        }
        public void IfGetSeed()
        {
            //var seedReply = replyList[1];
            IfAcceptConnectIsTrue();
            SendAfterAskSeed();
            //SendExChange(seedReply, txtPassword.Text);
            //private void SendExChange(string seedReply, string password)
            //seed = GetSeed(seedReply);
            var seedBytes = Encoding.ASCII.GetBytes(seed);
            var passwordBytes = Encoding.ASCII.GetBytes(this.Password);
            //SendExChange(seedBytes, passwordBytes);
            //private void SendExChange(byte[] seed, byte[] password)
            //var data = GetExChangeCommand(seed, password);
            //private String GetExChangeCommand(byte[] seed, byte[] password)

            var output = new byte[17];

            ENCRYPT(output, seedBytes, passwordBytes);

            var exChangeData = ("P2" + (char)2 + "(" + Encoding.ASCII.GetString(output, 0, 16) + ")" + (char)3);

            var checksum = CalculateChecksum(Encoding.ASCII.GetBytes(exChangeData));

            string data = (char)1 + exChangeData + (char)checksum;

            var buff = Ultility.FromString(data);

            //_serialPort.Write(buff, 0, buff.Length);
            this.Listener.Send(buff);
            //end SendExchange(text,text)
            sleep(0.5);
            //var reply = Read(_serialPort, 3000);
            justsendPassword = true;
        }
        public void IfSendPasswordOK()
        {
            //if (incorrectpassword == false)
            //{
                //ReadData
                Read("1A2A4A00");

                Read("1B2B4B00");

                Read("1C2C4C0C");

                Read("1D2D4D0D");

                Read("1E2E4E0E");

                Read("13234303");

                Read("18284800");

                Read("19294900");

                Read("07000000");

                LogOff();
                //for (int i = 0; i < replyList.Count; i++)
                //{
                //    LogService.WriteInfo("ReplyList" + i, replyList[i]);
                //}
            //}
        }
        private bool justsendConnectCommand = false;
        private bool justsendPassword = false;
        public void sendConnectCommand()
        {
             //private void SendRequest()
            //{
            //var buff = ("/?" + txtDeviceID.Text + "!\r\n").ToCharArray();
            //var buff = ("/?" + _serialID + "!\r\n");
            byte[] buff = Ultility.FromString("/?" + DeviceAddress + "!\r\n");
            //_serialPort.Write(buff, 0, buff.Length);
            justsendConnectCommand = true;
            //acceptSendingRequest = true;
            this.Listener.Send(buff);
            //var reply = Read(_serialPort, 3000);
            //if (reply[0] == 21)
            //{
            //    throw new Exception("Connect is deny");
            //}
            sleep(0.5); // in other code: sleep 1/4 second
            SendBaudRate();
            //Wait();
            //if (acceptSendingRequest == false)
            //{
            //    return false;
            //}
            ////justsendConnectCommand = false;
            
            //return acceptSendingRequest;
            //}
        }
        /// <summary>
        /// Should be sent secondly
        /// </summary>
        private void SendBaudRate()
        {
            byte[] buff;
            //buff = Ultility.FromString("\x06" + "001" + "\r\n"); 
            // Send ACK with BaudRate to the Device after request accepted
            // <ACK>0Z1<ACK><LF>
            //Z dai dien cho toc do baud
            /*
             with z = proposal of the communication baud rate
            · z=0: 300Baud
            · z=1: 600Baud
            · z=2 1200Baud
            · z=3: 2400Baud
            · z=4: 4800Baud
            · z=5: 9600Baud
            · z=6: 19200Baud
             */
            //var baudRate = int.Parse(txtBaudRate.Text);
            string ackBaudRate = "";//0Z1
            switch (_baudRate)
            {

                case 300:/*Z=0*/
                    ackBaudRate = "001";
                    break;
                case 600:/*Z=1*/
                    ackBaudRate = "011";
                    break;
                case 1200:/*Z=2*/
                    ackBaudRate = "021";
                    break;
                case 2400:/*Z=3*/
                    ackBaudRate = "031";
                    break;
                case 4800:/*Z=4*/
                    ackBaudRate = "041";
                    break;
                case 9600:/*Z=5*/
                    ackBaudRate = "051";
                    break;
                case 19200:/*Z=6*/
                    ackBaudRate = "061";
                    break;
                default:
                    throw new Exception(_baudRate + "Invalid baudrate! Not support" );
                    
            }

            // buff = ("\x06" + "001" + "\r\n").ToCharArray();//baud 300
            buff = Ultility.FromString("\x06" + ackBaudRate + "\r\n");

            this.Listener.Send(buff);
            //Read(_serialPort, 3000);
            //Wait();
            sleep(0.5);

           
            
            
        }
        private void SendExChangeRequest()
        {
            var buff = Ultility.FromString("\x01" + "R1" + "\x02" + "798001(10)" + "\x03" + "e");
            this.Listener.Send(buff);
            sleep(0.5);
            //_serialPort.Write(buff, 0, buff.Length);
            //Read(_serialPort, 3000);
            //Wait();

            buff = Ultility.FromString("\x01" + "R1" + "\x02" + "795001(08)" + "\x03" + "a");
            this.Listener.Send(buff);
            sleep(0.5);
            //_serialPort.Write(buff, 0, buff.Length);
            //Read(_serialPort, 3000);
            //Wait();
        }

        
        private byte CalculateChecksum(byte[] data)
        {
            return CalculateChecksum(data, 0, data.Length);
        }
        private byte CalculateChecksum(byte[] data, int index, int count)
        {
            if (count == -1)
            {
                count = data.Length - index;
            }
            byte result = 0;
            for (var pos = index; pos != index + count; ++pos)
            {
                result ^= data[pos];
            }
            return result;
        }
        public void LogOff()
        {
            var buff = Ultility.FromString("\x01" + "B0" + "\x03" + "q");
            //_serialPort.Write(buff, 0, buff.Length);
            this.Listener.Send(buff);
        }
        //private string GetSeed(string seedReply)
        //{
        //    var start = seedReply.IndexOf('(');
        //    var end = seedReply.LastIndexOf(')');

        //    return seedReply.Substring(start + 1, end - start - 1);
        //}
        private void Read(String address)
        {
            var cmd = "W1" + "\x02" + "605001(" + address + ")" + "\x03";
            var result = CalculateChecksum(Encoding.ASCII.GetBytes(cmd));
            var buff = Ultility.FromString("\x01" + cmd + (char)result);
            //_serialPort.Write(buff, 0, buff.Length);
            this.Listener.Send(buff);
            //Read(_serialPort, 3000);
            //Wait();
            sleep(0.5);
            for (var i = 0; i < 4; i++)
            {
                buff = Ultility.FromString("\x01" + "R1" + "\x02" + "605001(04)" + "\x03" + "e");
                //_serialPort.Write(buff, 0, buff.Length);
                //Read(_serialPort, 3000);
                this.Listener.Send(buff);
                sleep(0.5);
            }

            buff = Ultility.FromString("\x01" + "R1" + "\x02" + "606001(1C)" + "\x03" + "\x10");
            this.Listener.Send(buff);
            sleep(0.5);
            //Wait();
        }
        protected override void SaveData()
        {
            if (this.Listener.IsConnected)
            {
                RecloserAcq.DAL.DBController.Instance.SaveElster(this);
                if ((DateTime.Now - LastUpdated).TotalMilliseconds >= this.Latencybetweenpoll * 2)
                {
                    RePoll();
                }
            }
        }
    }
}
