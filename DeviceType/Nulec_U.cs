﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using TcpComm;

namespace RecloserAcq.Device
{    
    public class Nulec_U: Nulec
    {
        


        


        #region Request
        private const string Closestr = "1B 32 01 2A 0A 53 45 54 0A 43 42 0A 31 0A 7E 0A " +
                                    "44 43 36 41 03 1B 32 01 2A 0A 52 45 51 0A 43 42 " + 
                                    "0A 7E 0A 41 44 43 46 03";
        private const string Openstr = "1B 32 01 2A 0A 53 45 54 0A 43 42 0A 30 0A 7E 0A " +
                                    "41 41 44 45 03 1B 32 01 2A 0A 52 45 51 0A 43 42 " +
                                    "0A 7E 0A 41 44 43 46 03";
        private const string Settimestr = "1B 32 01 2A 0A 53 45 54 0A 43 4B 0A 32 31 0A 31 0A"; // 0A 7E 0A 2A 0A 52 45 51 0A 43 4B 0A 7E 0A";
        //public const string Request_01 =    "1B 32 01 2A 0A 52 45 51 0A 4D 41 23 0A 7E 0A 2A " +
        //                                    "0A 52 45 51 0A 4D 42 23 0A 7E 0A 2A 0A 52 45 51 " +
        //                                    "0A 4D 43 23 0A 7E 0A 2A 0A 52 45 51 0A 4D 45 23 " +
        //                                    "0A 7E 0A 31 34 41 30 03";
        
        //public const string Request_02 =    "1B 32 01 2A 0A 52 45 51 0A 4D 41 56 23 0A 7E 0A " +
        //                                    "2A 0A 52 45 51 0A 4D 42 56 23 0A 7E 0A 2A 0A 52 " +
        //                                    "45 51 0A 4D 43 56 23 0A 7E 0A 2A 0A 52 45 51 0A " +
        //                                    "4C 4C 23 0A 7E 0A 2A 0A 52 45 51 0A 42 4F 0A 7E " +
        //                                    "0A 38 37 32 37 03";

        //public const string Request_03 =    "1B 32 01 2A 0A 52 45 51 0A 4C 54 23 0A 7E 0A 30 32 42 44 03";

        //public const string Request_04 =    "1B 32 01 2A 0A 52 45 51 0A 44 49 0A 7E 0A 2A 0A " +
        //                                    "52 45 51 0A 4D 44 56 23 0A 7E 0A 2A 0A 52 45 51 " +
        //                                    "0A 4D 45 56 23 0A 7E 0A 2A 0A 52 45 51 0A 4D 46 " +
        //                                    "56 23 0A 7E 0A 2A 0A 52 45 51 0A 4D 47 56 23 0A " +
        //                                    "7E 0A 43 34 35 39 03";

        //public const string Request_05 =    "1B 32 01 2A 0A 52 45 51 0A 4D 48 56 23 0A 7E 0A " +
        //                                    "2A 0A 52 45 51 0A 4D 49 56 23 0A 7E 0A 2A 0A 52 " +
        //                                    "45 51 0A 4D 4A 56 23 0A 7E 0A 2A 0A 52 45 51 0A " +
        //                                    "4D 4B 56 23 0A 7E 0A 2A 0A 52 45 51 0A 4D 4C 56 " +
        //                                    "23 0A 7E 0A 45 31 30 36 03";

        //public const string Request_06 =    "1B 32 01 2A 0A 52 45 51 0A 44 49 44 2D 32 38 31 " +
        //                                    "0A 7E 0A 2A 0A 52 45 51 0A 44 49 44 2D 33 34 35 " +
        //                                    "0A 7E 0A 2A 0A 52 45 51 0A 4B 57 23 0A 7E 0A 2A " +
        //                                    "0A 52 45 51 0A 50 57 46 54 23 0A 7E 0A 2A 0A 52 " +
        //                                    "45 51 0A 46 53 35 30 36 30 0A 7E 0A 30 37 30 30 03";

        //public const string Request_07 =    "1B 32 01 2A 0A 52 45 51 0A 55 4F 46 31 32 23 0A " +
        //                                    "7E 0A 2A 0A 52 45 51 0A 4D 4E 23 0A 7E 0A 2A 0A " +
        //                                    "52 45 51 0A 4D 4B 23 0A 7E 0A 2A 0A 52 45 51 0A " +
        //                                    "4D 52 23 0A 7E 0A 2A 0A 52 45 51 0A 55 31 0A 7E " +
        //                                    "0A 41 30 43 39 03";

        //public const string Request_08 =    "1B 32 01 2A 0A 52 45 51 0A 56 31 0A 7E 0A 2A 0A " +
        //                                    "52 45 51 0A 57 31 0A 7E 0A 2A 0A 52 45 51 0A 55 " +
        //                                    "32 0A 7E 0A 2A 0A 52 45 51 0A 56 32 0A 7E 0A 2A " +
        //                                    "0A 52 45 51 0A 57 32 0A 7E 0A 34 39 33 36 03";

        //public const string Request_09 =    "1B 32 01 2A 0A 52 45 51 0A 44 49 44 2D 33 34 34 " +
        //                                    "0A 7E 0A 36 37 38 32 03";


        //public const string Request_10 =    "1B 32 01 2A 0A 52 45 51 0A 54 4E 0A 7E 0A 2A 0A " +
        //                                    "52 45 51 0A 44 49 44 2D 33 37 31 0A 7E 0A 2A 0A " +
        //                                    "52 45 51 0A 44 49 44 2D 32 39 31 0A 7E 0A 2A 0A " +
        //                                    "52 45 51 0A 44 49 44 2D 33 38 36 0A 7E 0A 36 31 " +
        //                                    "36 33 03";

        //public const string Request_11 =    "1B 32 01 2A 0A 52 45 51 0A 41 50 47 4F 41 0A 7E " +
        //                                    "0A 2A 0A 52 45 51 0A 41 50 47 41 4C 0A 7E 0A 2A " +
        //                                    "0A 52 45 51 0A 4F 53 0A 7E 0A 2A 0A 52 45 51 0A " +
        //                                    "53 56 0A 7E 0A 2A 0A 52 45 51 0A 43 42 0A 7E 0A " +
        //                                    "31 33 41 32 03";
        #endregion
       

        
        

        public Nulec_U(int port)
            : base(port)
        {
            DeviceTime = null;
        }

        public Nulec_U()
            : this(0)            
        { }

        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.Nulec_U;
            }
        }

        public override void UpdateData(byte[] data)
        { 
            int i = 0;
            while(i < data.Length - 1)
            {
                // Find start datablock
                if (data[i] == Parser.Nulec_StartBlock[0] && data[i + 1] == Parser.Nulec_StartBlock[1])
                {
                    int j = i + 2;
                    while (j < data.Length - 2)
                    {
                        if (data[j] == Parser.Nulec_EndBlock[0] &&
                            data[j + 1] == Parser.Nulec_EndBlock[1] &&
                            data[j + 2] == Parser.Nulec_EndBlock[2])
                        {
                            // get buff then convert to ascii and parse
                            int start = i + 2;
                            int end = j - 1;
                            var len = end - start + 1;

                            // copy to buff
                            var buff = new byte[len];
                            Array.Copy(data, start, buff, 0, len);
                            var s = System.Text.Encoding.ASCII.GetString(buff);
                            GetValue(s);

                            this.LastUpdated = DateTime.Now;
                            //bDataReturned = true;
                            // continue
                            i = j + 3;
                            break;
                        }
                        else
                            j += 1;
                    }
                }
                else
                {
                    i += 1;
                }
            }

            base.UpdateData(data);
            WriteRow();
        }
        
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
                FA_Accounting.Common.LogService.Logger.Error(string.Format("Nulec.GetValue({0}) error", s), ex);
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
               

                //if(DateTime.TryParseExact(date + " " + time, "dd/MM/yy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dt))
                this.DeviceTime = DateTime.ParseExact(date + " " + time, "MM/dd/yy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }

            return false;
        }
        // end

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
            if (_powerFactor > this.MaxClose)
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
                if (_powerFactor < this.MinOpen)
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
            
        }
        public override void SetTime()
        {
            DateTime now = DateTime.Now;
            string timenow = now.ToString("HH:mm:ss_dd/MM/yy");
            string strhexnow = Ultility.StringToHex(timenow);
            //string timenowhex = Ultility.ToHexText(timenow);
            string settimeformat = 
            "1B 32 01 2A 0A 53 45 54 0A 43 4B 0A 32 31 0A 31 0A " + strhexnow
            + " 0A 7E 0A 2A 0A 52 45 51 0A 43 4B 0A 7E 0A 36 30 30 44 03 1B 32 01 2A 0A 53 45 54 0A 43 4B 0A 32 31 0A 31 0A " + strhexnow
            + " 0A 7E 0A 2A 0A 52 45 51 0A 43 4B 0A 7E 0A 36 30 30 44 03 1B 32 01 2A 0A 53 45 54 0A 43 4B 0A 32 31 0A 31 0A " + strhexnow
            + " 0A 7E 0A 2A 0A 52 45 51 0A 43 4B 0A 7E 0A 36 30 30 44 03 ";
            //"31 34 3A 33 36 3A 32 32 5F 31 36 2F 30 38 2F 31 33
            // truong hop y/c them RQ. 
            //->> string settimeformat = now.ToString("1B 32 01 2A 0A 53 45 54 0A 43 4B 0A 32 31 0A 31 0A hh:mm:ss_dd/MM/yy 0A 7E 0A 2A 0A 52 45 51 0A 43 4B 0A 7E 0A");
          //string settimeformat = now.ToString("1B 32 01 2A 0A 53 45 54 0A 43 4B 0A 32 31 0A 31 0A hh:mm:ss_dd/MM/yy 0A 7E 0A");
            //note 0A 7E 0A  gia tri ket thuc data block

            Listener.Send(Ultility.FromHex(settimeformat));
            //System.Threading.Thread.Sleep(10);
        }
        public override void WriteRow()
        {

            string filePath = _devicepath + Port + ".csv";
            string strFields = "TestField\r\n";
            string strvalues = "TestValue";
            try
            {
                strFields = "DeviceId,Type,Date,Alert,Amp12,Amp34,Amp56,AmpEarth,Operations,Battery_1,Battery_2,Status_Open,Status_Close,Status_Lockout";
                strvalues = Id.ToString() + "," +
                    DeviceType.ToString() + "," +
                    LastUpdated.ToString() + "," +
                    AlertVal.ToString() + "," +
                    Amp12.ToString() + "," +
                    Amp34.ToString() + "," +
                    Amp56.ToString() + "," +
                    AmpEarth.ToString() + "," +
                    Operations.ToString() + "," +
                    (Battery_1 == null ? " " : Battery_1.ToString()) + "," +
                    (Battery_2 == null ? " " : Battery_2.ToString()) + "," +
                    Status_Open.ToString() + "," +
                    Status_Close.ToString() + "," +
                    Status_Lockout.ToString();
                
                
                    strFields = strFields + ",ApparentPower,ReactivePower,RealPower,PowerFactor,DeviceTime\r\n";
                    
                    strvalues = strvalues + "," + ApparentPower.ToString() + "," +
                    ReactivePower.ToString() + "," +
                    RealPower.ToString() + "," +
                    PowerFactor.ToString() + "," +
                    ((DeviceTime.HasValue && DeviceTime.Value > DateTime.MinValue) ? DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "");
                
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
                    FA_Accounting.Common.LogService.WriteError("Write Row Nulec U", e.Message);
                }
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Write Row Nulec U", ex.Message);
            }
        }
    }
}
