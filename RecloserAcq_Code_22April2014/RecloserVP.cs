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
    public class RecloserVP: RecloserBase
    {
        


     

        //private List<String> dataReceiveList = new List<string>();
        public bool acceptSendingRequest = false;
        #region Receive Pattern
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
        // hỏi password 
       
        // Current
        // .*.MA#.6296,100.~.*.MB#.7141,100.~.*.MC#.6952,100.~.*.ME#.845,100.~.880C.
        public const string Pattern_Current = @"(?<Type>M[ABCE]#)\n(?<Value>\d{2,6}),(?<Factor>\d{3,4})";
        

        // Power
        // .*.DID-281.0.~.*.DID-345.1.~.*.KW#.2592737,1000.~.*.PWFT#.97,100.~.*.FS5060.0.~.1766.					           
        // .*.UOF12#.50130,1000.~.*.MN.4.0.~.*.MK#.2661267,1000.~.*.MR#.582373,1000.~.*.U1.1.~.DC75.
        public const string Pattern_Power_noABC = @"(?<Type>KW#|PWFT#|MK#|MR#)\n(?<Value>\d{2,7}),(?<Factor>\d{3,4})";
        public const string Pattern_Power = @"(?<Type>KW#|KW[ABC]#|WFT#|PWFT#|PWFT[ABC]#|MK#|MK[ABC]#|MR#|MR[ABC]#)\n(?<Value>-?\d{2,7}),(?<Factor>\d{3,4})";
        public const string Pattern_TotalPower = @"(?<Type>KWH|KWHF|KWHR)\n(?<Value>\d{2,9})";

        // Date time & Status
        // .*.CK.21.1.09:30:56 05/06/12.~.*.AR.1.~.*.CL.0.~.*.+B-CL.0.~.*.EE.1.~.EFAC.
        //public const string Pattern_Status = @"(?<Type>SE|SA|EE|CB)\n(?<Value>0|1)";

        //"CK\n21\n1\n09:30:55 05/06/12"
        //public const string Pattern_DateTime = @"CK\n21\n1\n(?<Time>\d{2}:\d{2}:\d{2})\s(?<Date>\d{2}/\d{2}/\d{2})";
        // RecloserVP 
        public const string Pattern_Date = @"(\d{2,2}/\d{2,2}/\d{2,2})";
        public const string Pattern_Time =@"(\d{2,2}:\d{2,2}:\d{2,2})";
        // end RecloserVP
        
        // Battery
        //.*.MF.0.~.*.CS.10.1.528-19.01.~.*.BTS.1.~.*.WSOSTM.0.~.*.MIL.0.~.EA7C.
        public const string Pattern_Battery = @"BTS\n(?<Battery>0|1)";
        

        // Battery voltage
        // .*.FO.0.~.*.CF.0.~.*.CK.21.1.09:31:12 05/06/12.~.*.PR.5.~.*.BTV#.2720,100.~.7605.
        public const string Pattern_Battery_Voltage = @"BTV#\n(?<Value>\d{2,4}),(?<Factor>\d{3})";

        
        //public const string Pattern_Operation = @"OP\n(?<Operation>\d+)";

        public const string Pattern_Status = @"(?<Type>SE|SA|EE|CB)\n(?<Value>0|1)";

        //"CK\n21\n1\n09:30:55 05/06/12"
        public const string Pattern_DateTime = @"CK\n21\n1\n(?<Time>\d{2}:\d{2}:\d{2})\s(?<Date>\d{2}/\d{2}/\d{2})";

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

        public RecloserVP(int port)
            : base(port)
        {
            _deviceTime = null;
        }

        public RecloserVP():this(0)            
        { }

        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.RecloserVP;
            }
        }
        
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
            
            this.LastUpdated = DateTime.Now;
            
        }
        protected override void SaveData()
        {
            if (this.Listener.IsConnected)
            {
                RecloserAcq.DAL.DBController.Instance.SaveRecloserVP(this);
                if ((DateTime.Now - LastUpdated).TotalMilliseconds >= this.Latencybetweenpoll * 2)
                {
                    acceptSendingRequest = false;
                    RePoll();
                }
            }
        }
        public override void UpdateData(byte[] data)
        {
            int i = 0;
            while (i < data.Length - 1)
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
        public const string Pattern_Operation = @"OP\n(?<Operation>\d+)";
        private bool getOperation(string s)
        {// Operation
            var re = new Regex(Pattern_Operation);
           var match = re.Match(s);
            if (match.Success)
            {
                this.Operations = Convert.ToInt32(match.Groups["Operation"].Value);
                return true;
            }
            return false;
        }
        private void GetValue(string s)
        {
            try
            {
                var found = GetStatus(s);
                    
                if (!found)
                    GetTotalPower(s);
                if (!found)
                    GetCurrentandPower(s);
                if (!found)
                    GetVoltage(s);
                if (!found)
                    getOperation(s);
                if (!found)
                    GetOtherValue(s);
                if (hastoCalculateVoltage == true) //_vA_B_load == 0 && _vB_C_load == 0 && VC_A_load == 0 && _vA_B_src == 0 && _vB_C_src == 0 && VC_A_src == 0)
                {
                    GetMeasure(); // get voltage by formulars
                }
                else if (hastoCalculateVoltage == false)
                {
                }    
                else //(hastoCalculateVoltage == null)
                {
                    if (_iRequest == _request.Count && _vA_B_load == 0 && _vB_C_load == 0 && VC_A_load == 0 && _vA_B_src == 0 && _vB_C_src == 0 && VC_A_src == 0)
                    {
                        hastoCalculateVoltage = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogService.Logger.Error(string.Format("RecloserVP.GetValue({0}) error", s), ex);
            }
        }
        private bool? hastoCalculateVoltage = null;
        private void GetMeasure()
        {
            double[] volt_phase_G = new double[3];//VA VB VC
            volt_phase_G[0] = volt_phase_G[1] = volt_phase_G[2] = 0;
            //txtResult.Text = "IA = " + Convert.ToString(ampe_measure_log[0]) + new_line +
            //                     "IB = " + Convert.ToString(ampe_measure_log[1]) + new_line +
            //                     "IC = " + Convert.ToString(ampe_measure_log[2]) + new_line +
            //                     "IE = " + Convert.ToString(ampe_measure_log[3]);
            //string res_cs_S = "apparentPWR(S):" + new_line +
            //                 "S_A = " + Convert.ToString(apparent_Power_S[0]) + new_line +
            //                 "S_B = " + Convert.ToString(apparent_Power_S[1]) + new_line +
            //                 "S_C = " + Convert.ToString(apparent_Power_S[2]) + new_line;
            if (_current_IA != 0)
            {
                volt_phase_G[0] = _apparent_S_A / _current_IA;
            }
            if (_current_IB != 0)
            {
                volt_phase_G[1] = _apparent_S_B / _current_IB;
            }
            if (_current_IC != 0)
            {
                volt_phase_G[2] = _apparent_S_C / _current_IC;
            }

            
            double Vab = 1000 * Math.Sqrt(volt_phase_G[0] * volt_phase_G[0] + volt_phase_G[1] * volt_phase_G[1] + volt_phase_G[0] * volt_phase_G[1]);
            double Vbc = 1000 * Math.Sqrt(volt_phase_G[1] * volt_phase_G[1] + volt_phase_G[2] * volt_phase_G[2] + volt_phase_G[1] * volt_phase_G[2]);
            double Vca = 1000 * Math.Sqrt(volt_phase_G[2] * volt_phase_G[2] + volt_phase_G[0] * volt_phase_G[0] + volt_phase_G[2] * volt_phase_G[0]);

            VA_B_src = VA_B_load = Math.Round(Vab);
            VB_C_src = VB_C_load = Math.Round(Vbc);
            VC_A_src = VC_A_load = Math.Round(Vca);
            VA_GND_src = VA_GND_load = volt_phase_G[0];
            VB_GND_src = VB_GND_load = volt_phase_G[1];
            VC_GND_src = VC_GND_load = volt_phase_G[2];
            //volt_phase_phase = "Volt Phase-Phase:" + new_line +
            //                   "VAB(volt)   = " + Convert.ToString(Math.Round(Vab)) + new_line +
            //                   "VBC(volt)   = " + Convert.ToString(Math.Round(Vbc)) + new_line +
            //                   "VCA(volt)   = " + Convert.ToString(Math.Round(Vca)) + new_line;
            //txtResult.Text = res_cs_S + res_cs_P + res_cs_Q + res_cos_phi + sumarize_kw + res_I_amp + volt_phase_phase;
        }
        
        private bool GetVoltage(string s)
        {
            int pos_cmd = s.IndexOf("A7\x0A", 0);
            //if (pos_cmd < 0)
            //{
            //    pos_cmd = s.IndexOf("A6\x0A", 0);
            //}
            if (pos_cmd >= 0)
            {
                s = s + "\x0A";
                try
                {
                    int start_data = s.IndexOf("-", pos_cmd + 4);//pass qua 0x0A gan A7
                    int end_cmd = s.IndexOf("\x0A", pos_cmd + 4);//pass qua 0x0A gan A7
                    if ((end_cmd >= start_data) && (start_data >= pos_cmd + 4))
                    {
                        string res_data = s.Substring(start_data + 1, end_cmd - start_data - 2);
                        if (res_data.Length > (64 + 24))
                        {
                            int phase_gnd_pos = 40;
                            int phase_phase_pos = phase_gnd_pos + 24;

                            string str_volt = res_data.Substring(phase_gnd_pos, 48);
                            byte[] buff_volt = TcpComm.Ultility.hexStringToByteArray(str_volt);

                            VA_GND_src = buff_volt[0] * 256 + buff_volt[1];
                            VB_GND_src = buff_volt[2] * 256 + buff_volt[3];
                            VC_GND_src = buff_volt[4] * 256 + buff_volt[5];

                            VA_GND_load = buff_volt[6] * 256 + buff_volt[7];
                            VB_GND_load = buff_volt[8] * 256 + buff_volt[9];
                            VC_GND_load = buff_volt[10] * 256 + buff_volt[11];


                            VA_B_src = buff_volt[12] * 256 + buff_volt[13];
                            VB_C_src = buff_volt[14] * 256 + buff_volt[15];
                            VC_A_src = buff_volt[16] * 256 + buff_volt[17];

                            VA_B_load = buff_volt[18] * 256 + buff_volt[19];
                            VB_C_load = buff_volt[20] * 256 + buff_volt[21];
                            VC_A_load = buff_volt[22] * 256 + buff_volt[23];

                            hastoCalculateVoltage = false;

                            return true;
                        }

                    }
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.WriteError("Get Recloser VP Voltage", ex.Message);
                    return false;
                }

            }
            //else
            //{

            //}
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
                        this.Status_Open = val; // Status_open set before Status_Close
                        this.Status_Close = !val;
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
                this.BatterryVol = bv;//.ToString("##.##"); ;
                return true;
            }

            //// Operation
            //re = new Regex(Pattern_Operation);
            //match = re.Match(s);
            //if (match.Success)
            //{
            //    this.Operations = Convert.ToInt32(match.Groups["Operation"].Value);
            //    return true;
            //}

            // DateTime
            re = new Regex(Pattern_DateTime);
            match = re.Match(s);
            if (match.Success)
            {
                // update device datetime
                var time = match.Groups["Time"].Value;
                var date = match.Groups["Date"].Value;
                DateTime dt;

                if (DateTime.TryParseExact(date + " " + time, "dd/MM/yy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dt))
                    this.DeviceTime = dt;
                return true;
            }

            return false;
        }
        //private bool GetDateTime(string s)
        //{
        //    // DateTime
        //    try
        //    {
        //        Match matchDate = Regex.Match(s, Pattern_Date);
        //        Match matchTime = Regex.Match(s, Pattern_Time);
        //        string strdate = this._deviceTime == null ? "01/01/2014" : this._deviceTime.Value.ToString("MM/dd/yy");
        //        string strtime = this._deviceTime == null ? "00:00:00" : this._deviceTime.Value.ToString("HH:mm:ss");
        //        // Here we check the Match instance.
        //        if (matchDate.Success)
        //        {
        //            // Finally, we get the Group value and display it.
        //            //It is important to note that the indexing of the Groups collection on Match objects starts at 1.
        //            strdate = matchDate.Groups[1].Value;
        //            //date = //DateTime.ParseExact(key, "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);

        //        }
        //        if (matchTime.Success)
        //        {
        //            // Finally, we get the Group value and display it.
        //            strtime = matchTime.Groups[1].Value;
        //            //this._deviceTime = DateTime.ParseExact(key, "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);

        //        }
        //        this._deviceTime = DateTime.ParseExact(strdate + " " + strtime, "MM/dd/yy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                
        //    }
        //    catch(Exception ex)
        //    {
        //    }

        //    return false;
        //}
        // end

       
        #region new field for RecloserVP 
           
        
        private double _current_IA;
        [XmlIgnore]
        public double Current_IA
        {
            get { return _current_IA; }
            set
            {
                _current_IA = value;
                if (value == 0) // khong set gia tri am pe khi gia tri ampe = 0 , de giu gia tri truoc khi co su co
                {
                    if (_alertVal == 0)
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }

                }
                else if ((value > MaxAmp && MaxAmp != 0) || (value < MinAmp && MinAmp!=0) )
                {
                    setAlert(2, ref _startZeroA);
                }
                else
                {
                    //if (AmpPercent!=0 && ((Math.Abs(value - _current_IB) / ((value + _current_IB) / 2) >= (AmpPercent / 100) && _current_IB != 0)
                    //    || (Math.Abs(value - _current_IC) / ((value + _current_IC) / 2) >= (AmpPercent / 100) && _current_IC != 0)))
                        
                    //if ((value >= _current_IB * (_amppercent/100) && _current_IB != 0) 
                    //    || value <= _current_IB / 2 
                    //    || (value >= _current_IC * 2 && _current_IC!=0) 
                    //    || value <= _current_IC / 2)
                    double p = (double)AmpPercent / 100;
                    if (AmpPercent != 0 && _current_IB!=0 && _current_IC!=0 && ((Math.Max(Math.Max(value, _current_IB), _current_IC) - Math.Min(Math.Min(value, _current_IB), _current_IC)) * 3 / (value + _current_IB + _current_IC) >= p))
                    {
                        //if (_alertVal == 0)
                        //{
                            LogService.WriteInfo("Bao dong pha", "");
                            _alertVal = 1;
                            SaveData();
                            Notify("AlertVal");

                        //}
                        //else
                        //{

                        //}
                    }
                    else if (_alertVal != 0)
                    {
                        _alertVal = 0;
                        Notify("AlertVal");
                        _startZeroA = null;
                    }
                }
            }
        }
       
        private double _current_IB;
        [XmlIgnore]
        public double Current_IB
        {
            get { return _current_IB; }
            set
            {
                _current_IB = value;
                if (value == 0)
                {
                    if (_alertVal == 0) // khong set gia tri am pe khi gia tri ampe = 0 , de giu gia tri truoc khi co su co
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }

                }
                else if ((value > MaxAmp && MaxAmp != 0) || (value < MinAmp && MinAmp != 0))
                {
                    setAlert(2, ref _startZeroB);
                }
                else
                {
                    //((Max(IA,IB,IC) - Min(IA,IB,IC) )/ ((IA+IB+IC)/3 ))*100
                    //if (AmpPercent!=0 && ((Math.Abs(value - _current_IA) / ((value + _current_IA) / 2) >= (AmpPercent / 100) && _current_IA != 0)
                    //    || (Math.Abs(value - _current_IC) / ((value + _current_IC) / 2) >= (AmpPercent / 100) && _current_IC != 0) ))
                    double p = (double)AmpPercent/100;
                    if (AmpPercent != 0 && _current_IA!=0 && _current_IC!=0 &&  ((Math.Max(Math.Max(value,_current_IA),_current_IC)-Math.Min(Math.Min(value,_current_IA),_current_IC))*3/(value + _current_IA +  _current_IC)>= p))
                    {
                        //if (_alertVal == 0)
                        //{
                        LogService.WriteInfo("Bao dong pha B", "");
                            _alertVal = 1;
                            SaveData();
                            Notify("AlertVal");

                        //}
                        //else
                        //{

                        //}
                    }
                    else if (_alertVal != 0)
                    {
                        _alertVal = 0;
                        Notify("AlertVal");
                        _startZeroB = null;
                    }
                }
            }
        }
          private double _current_IC;
        [XmlIgnore]
        public double Current_IC
        {
            get { return _current_IC; }
            set
            {
                _current_IC = value;
                if (value == 0)
                {
                    if (_alertVal == 0) // khong set gia tri am pe khi gia tri ampe = 0 , de giu gia tri truoc khi co su co
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }

                }
                else if ((value > MaxAmp && MaxAmp!=0)|| (value < MinAmp && MinAmp!=0))
                {
                    setAlert(2, ref _startZeroC);
                }
                else
                {
                    // Cong thuc tinh phan tram chenh lech: 
                    // (Math.abs(X-Y)*2/(X+Y))*100%
                    //if (AmpPercent!=0 && ((Math.Abs(value - _current_IB) / ((value + _current_IB) / 2) >= (AmpPercent / 100) && _current_IB != 0)
                    //    || (Math.Abs(value - _current_IA) / ((value + _current_IA) / 2) >= (AmpPercent / 100) && _current_IA != 0)))
                    double p = (double)AmpPercent / 100;
                    if (AmpPercent != 0 && _current_IB != 0 && _current_IA != 0 && ((Math.Max(Math.Max(value, _current_IA), _current_IB) - Math.Min(Math.Min(value, _current_IA), _current_IB)) * 3 / (value + _current_IA + _current_IB) >= p))
                    {
                        //if (_alertVal == 0)
                        //{
                        LogService.WriteInfo("Bao dong pha C", "");
                            _alertVal = 1;
                            SaveData();
                            Notify("AlertVal");

                        //}
                        //else
                        //{

                        //}
                    }
                    else if (_alertVal != 0)
                    {
                        _alertVal = 0;
                        Notify("AlertVal");
                        _startZeroC = null;
                    }
                }
            }
        }
        private double _current_IE;
        [XmlIgnore]
        public double Current_IE
        {
            get { return _current_IE; }
            set
            {
                if (_current_IE != value)
                {
                    _current_IE = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _apparent_S_A;
        [XmlIgnore]
        public double Apparent_S_A 
        {
            get { return _apparent_S_A; }
            set
            {
                if (_apparent_S_A != value)
                {
                    _apparent_S_A = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _apparent_S_B;
        [XmlIgnore]
        public double Apparent_S_B 
        {
            get { return _apparent_S_B; }
            set
            {
                if (_apparent_S_B != value)
                {
                    _apparent_S_B = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _apparent_S_C;
        [XmlIgnore]
        public double Apparent_S_C 
        {
            get { return _apparent_S_C; }
            set
            {
                if (_apparent_S_C != value)
                {
                    _apparent_S_C = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _real_P_A;
        [XmlIgnore]
        public double Real_P_A
        {
            get { return _real_P_A; }
            set
            {
                if (_real_P_A != value)
                {
                    _real_P_A = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _real_P_B;
        [XmlIgnore]
        public double Real_P_B
        {
            get { return _real_P_B; }
            set
            {
                if (_real_P_B != value)
                {
                    _real_P_B = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _real_P_C;
        [XmlIgnore]
        public double Real_P_C
        {
            get { return _real_P_C; }
            set
            {
                if (_real_P_C != value)
                {
                    _real_P_C = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _reactive_Q_A;
        [XmlIgnore]
        public double Reactive_Q_A
        {
            get { return _reactive_Q_A; }
            set
            {
                if (_reactive_Q_A != value)
                {
                    _reactive_Q_A = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _reactive_Q_B;
        [XmlIgnore]
        public double Reactive_Q_B
        {
            get { return _reactive_Q_B; }
            set
            {
                if (_reactive_Q_B != value)
                {
                    _reactive_Q_B = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _reactive_Q_C;
        [XmlIgnore]
        public double Reactive_Q_C
        {
            get { return _reactive_Q_C; }
            set
            {
                if (_reactive_Q_C != value)
                {
                    _reactive_Q_C = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _cosphi_A;
        [XmlIgnore]
        public double Cosphi_A
        {
            get { return _cosphi_A; }
            set
            {
                if (_cosphi_A != value)
                {
                    _cosphi_A = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _cosphi_B;
        [XmlIgnore]
        public double Cosphi_B
        {
            get { return _cosphi_B; }
            set
            {
                if (_cosphi_B != value)
                {
                    _cosphi_B = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _cosphi_C;
        [XmlIgnore]
        public double Cosphi_C
        {
            get { return _cosphi_C; }
            set
            {
                if (_cosphi_C != value)
                {
                    _cosphi_C = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _total;
        [XmlIgnore]
        public double Total
        {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    _total = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _forward;
        [XmlIgnore]
        public double Forward
        {
            get { return _forward; }
            set
            {
                if (_forward != value)
                {
                    _forward = value;
                    //Notify("Volt_A");
                }
            }
        }
        
        private double _reverse;
        [XmlIgnore]
        public double Reverse
        {
            get { return _reverse; }
            set
            {
                if (_reverse != value)
                {
                    _reverse = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _batterryVol;
        [XmlIgnore]
        public double BatterryVol
        {
            get { return _batterryVol; }
            set
            {
                if (_batterryVol != value)
                {
                    _batterryVol = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _vA_GND_src;
        [XmlIgnore]
        public double VA_GND_src
        {
            get { return _vA_GND_src; }
            set
            {
                if (_vA_GND_src != value)
                {
                    _vA_GND_src = value;
                   
                    //Notify("Volt_A");
                }
            }
        }
        private double _vB_GND_src;
        [XmlIgnore]
        public double VB_GND_src
        {
            get { return _vB_GND_src; }
            set
            {
                if (_vB_GND_src != value)
                {
                    _vB_GND_src = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _vC_GND_src;
        [XmlIgnore]
        public double VC_GND_src
        {
            get { return _vC_GND_src; }
            set
            {
                if (_vC_GND_src != value)
                {
                    _vC_GND_src = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _vA_GND_load;
        [XmlIgnore]
        public double VA_GND_load
        {
            get { return _vA_GND_load; }
            set
            {
                if (_vA_GND_load != value)
                {
                    _vA_GND_load = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _vB_GND_load;
        [XmlIgnore]
        public double VB_GND_load
        {
            get { return _vB_GND_load; }
            set
            {
                if (_vB_GND_load != value)
                {
                    _vB_GND_load = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _vC_GND_load;
        [XmlIgnore]
        public double VC_GND_load
        {
            get { return _vC_GND_load; }
            set
            {
                if (_vC_GND_load != value)
                {
                    _vC_GND_load = value;
                    //Notify("Volt_A");
                }
            }
        }
        private double _vA_B_src  ;
        [XmlIgnore]
        public double VA_B_src  
        {
            get { return _vA_B_src  ; }
            set
            {
                if (_vA_B_src != value)
                {
                    _vA_B_src = value;
                    if (value != 0 && _volStandard != 0 && _volpercent != 0 && (value < (_volStandard - (_volStandard * _volpercent / 100)) || value > (_volStandard + (_volStandard * _volpercent / 100))))
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                   
                    //Notify("Volt_A");
                }
            }
        }
        private double _vB_C_src  ;
        [XmlIgnore]
        public double VB_C_src  
        {
            get { return _vB_C_src  ; }
            set
            {
                if (_vB_C_src != value)
                {
                    _vB_C_src = value;
                    if (value != 0 && _volStandard != 0 && _volpercent != 0 && (value < (_volStandard - (_volStandard * _volpercent / 100)) || value > (_volStandard + (_volStandard * _volpercent / 100))))
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                   
                    //Notify("Volt_A");
                }
            }
        }
        private double _vC_A_src  ;
        [XmlIgnore]
        public double VC_A_src  
        {
            get { return _vC_A_src  ; }
            set
            {
                if (_vC_A_src != value)
                {
                    _vC_A_src = value;
                    
                    if (value != 0 && _volStandard != 0 && _volpercent != 0 && (value < (_volStandard - (_volStandard * _volpercent / 100)) || value > (_volStandard + (_volStandard * _volpercent / 100))))
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                    //Notify("Volt_A");
                }
            }
        }
        private double _vA_B_load;
        [XmlIgnore]
        public double VA_B_load
        {
            get { return _vA_B_load; }
            set
            {
                if (_vA_B_load != value)
                {
                    _vA_B_load = value;
                    
                    if (value != 0 && _volStandard != 0 && _volpercent != 0 && (value < (_volStandard - (_volStandard * _volpercent / 100)) || value > (_volStandard + (_volStandard * _volpercent / 100))))
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                    //Notify("Volt_A");
                }
            }
        }
        private double _vB_C_load;
        [XmlIgnore]
        public double VB_C_load
        {
            get { return _vB_C_load; }
            set
            {
                if (_vB_C_load != value)
                {
                    _vB_C_load = value;
                    if (value != 0 && _volStandard != 0 && _volpercent != 0 && (value < (_volStandard - (_volStandard * _volpercent / 100)) || value > (_volStandard + (_volStandard * _volpercent / 100))))
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                    //Notify("Volt_A");
                }
            }
        }
        private double _vC_A_load;
        [XmlIgnore]
        public double VC_A_load
        {
            get { return _vC_A_load; }
            set
            {
                if (_vC_A_load != value)
                {
                    _vC_A_load = value;
                    if (value != 0 && _volStandard != 0 && _volpercent != 0 && (value < (_volStandard - (_volStandard * _volpercent / 100)) || value > (_volStandard + (_volStandard * _volpercent / 100))))
                    {
                        _alertVal = 1;
                        SaveData();
                        Notify("AlertVal");
                    }
                    //Notify("Volt_A");
                }
            }
        }

        
        #endregion new fields for RecloserVP
       
        
       

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
            return eAlertOpenClose.None;
        }

        public eAlertOpenClose CheckQ()
        {
            
            return eAlertOpenClose.None;
        }

       
        public override void CommandClose(bool auto)
        {
           //VP xong
            timerBetweenEachRequest.Enabled = false;
            timerBetweenEachRequest.Stop();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();

            try
            {
                //string hex_close = "1B 32 01 2A 0A 53 45 54 0A 43 42 0A 31 0A 7E 0A 44 43 36 41 03 1B 32 01 2A 0A 52 45 51 0A 43 42 0A 7E 0A 41 44 43 46 03";
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x53, 0x45, 0x54, 0x0A, 0x43, 0x42, 0x0A, 0x31, 0x0A, 0x7E, 0x0A, 0x44, 0x43, 0x36, 0x41, 0x03, 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x43, 0x42, 0x0A, 0x7E, 0x0A, 0x41, 0x44, 0x43, 0x46, 0x03 });
                //System.Threading.Thread.Sleep(10);
                //Listener.Send(new byte[] { (byte)'Y', 0x0D });
            }
            catch (Exception ex)
            {
                
            }
            timerBetweenEachRequest.Enabled = true;
            timerBetweenEachRequest.Start();
        }
        public override void CommandOpen(bool auto)
        {
            //VP xong
            
           
            timerBetweenEachRequest.Enabled = false;
            timerBetweenEachRequest.Stop();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();
            try
            {
                //string hex_open = "1B 32 01 2A 0A 53 45 54 0A 43 42 0A 30 0A 7E 0A 41 41 44 45 03 1B 32 01 2A 0A 52 45 51 0A 43 42 0A 7E 0A 41 44 43 46 03";
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x53, 0x45, 0x54, 0x0A, 0x43, 0x42, 0x0A, 0x30, 0x0A, 0x7E, 0x0A, 0x41, 0x41, 0x44, 0x45, 0x03, 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x43, 0x42, 0x0A, 0x7E, 0x0A, 0x41, 0x44, 0x43, 0x46, 0x03 });
                //System.Threading.Thread.Sleep(10);
                
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
        
        public override void WriteRow()
        {
            //VP Xong
            string dvpath = RecloserAcq.Properties.Settings.Default.dvpath.Trim();
            if (dvpath.LastIndexOf('\\') != dvpath.Length - 1)
            {
                dvpath += "\\";
            }
            string filePath = RecloserAcq.Properties.Settings.Default.dvpath + Port + ".csv";
            string strFields = "";
            string strvalues = "";
            try
            {

                strFields = "DeviceId,Type,Date,Alert,Mo,Dong,DeviceTime,Current_IA," +
	"Current_IB,Current_IC,Current_IE,Apparent_S_A,Apparent_S_B,Apparent_S_C,Real_P_A," +
	"Real_P_B,Real_P_C,Reactive_Q_A,Reactive_Q_B,Reactive_Q_C,Cosphi_A,Cosphi_B,Cosphi_C," +
	"Total ,Forward,Reverse,BatterryVol,VA_GND_src,VB_GND_src,VC_GND_src,VA_GND_load," +
    "VB_GND_load,VC_GND_load,VA_B_src,VB_C_src,VC_A_src,VA_B_load,VB_C_load,VC_A_load,Operation \r\n";
                strvalues = DeviceID.ToString() + "," +
                    DeviceType.ToString() + "," +
                    LastUpdated.ToString() + "," +
                    AlertVal.ToString() + "," +
                    Status_Open.ToString() + "," +
                    Status_Close.ToString() + "," +
                ((DeviceTime.HasValue && DeviceTime.Value > DateTime.MinValue) ? DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "") + "," +
                _current_IA.ToString() + "," + _current_IB.ToString() + "," + _current_IC.ToString() + "," + _current_IE.ToString() + "," +
                _apparent_S_A.ToString() + "," + _apparent_S_B.ToString() + "," + _apparent_S_C.ToString() + "," +
                _real_P_A.ToString() + "," + _real_P_B.ToString() + "," + _real_P_C.ToString() + "," +
                _reactive_Q_A.ToString() + "," + _reactive_Q_B.ToString() + "," + _reactive_Q_C.ToString() + "," +
                _cosphi_A.ToString() + "," + _cosphi_B.ToString() + "," + _cosphi_C.ToString() + "," +
                _total.ToString() + "," + _forward.ToString() + "," + _reverse.ToString() + "," + _batterryVol.ToString() + "," +
                _vA_GND_src.ToString() + "," + _vB_GND_src.ToString() + "," + _vC_GND_src.ToString() + "," +
                _vA_GND_load.ToString() + "," + _vB_GND_load.ToString() + "," + _vC_GND_load.ToString() + "," +
                _vA_B_src.ToString() + "," + _vB_C_src.ToString() + "," + _vC_A_src.ToString() + "," +
                _vA_B_load.ToString() + "," + _vB_C_load.ToString() + "," + _vC_A_load.ToString() + "," + Operations.ToString();
                
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
                    LogService.WriteError("WriteRow RecloserVP 1", e.Message); 
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("WriteRow RecloserVP", ex.Message);
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
     
     
            }
            catch (Exception ex)
            {
                LogService.WriteError("RecloserVP_StartSendingRequest", ex.ToString());

            }
            finally
            {
                //timerBetweenEachRequest.Start();
                //timerBetweenEachPoll.Start();

            }
     
            base.StartSendingRequests(); //-->
     
        }
        
     
        
        public bool sendConnectCommand()
        {
            try
            {
                timerBetweenEachPoll.Stop();
                timerBetweenEachRequest.Stop();

                //hex_login_init1: 0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x4F , 0x53 , 0x0A , 0x7E , 0x0A , 0x34 , 0x42 , 0x46 , 0x37 , 0x03 , 0x1B , 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x4F , 0x53 , 0x0A , 0x7E , 0x0A , 0x34 , 0x42 , 0x46 , 0x37 , 0x03
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x4F, 0x53, 0x0A, 0x7E, 0x0A, 0x34, 0x42, 0x46, 0x37, 0x03, 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x4F, 0x53, 0x0A, 0x7E, 0x0A, 0x34, 0x42, 0x46, 0x37, 0x03 });
                sleep(2);
                // hex_login_init2: 0x1B, 0x32, 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x44 , 0x49 , 0x44 , 0x2D , 0x33 , 0x36 , 0x31  , 0x0A , 0x7E , 0x0A , 0x39 , 0x46 , 0x34 , 0x34 , 0x03
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x44, 0x49, 0x44, 0x2D, 0x33, 0x36, 0x31, 0x0A, 0x7E, 0x0A, 0x39, 0x46, 0x34, 0x34, 0x03 });
                sleep(2);
                
                // hex_login_init3: 0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x44 , 0x49 , 0x44 , 0x2D , 0x33 , 0x36 , 0x30 , 0x0A , 0x7E , 0x0A , 0x45 , 0x39 , 0x46 , 0x30 , 0x03
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x44, 0x49, 0x44, 0x2D, 0x33, 0x36, 0x30, 0x0A, 0x7E, 0x0A, 0x45, 0x39, 0x46, 0x30, 0x03 });
                sleep(2);

                //hex_lgoin4:0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x49 , 0x4E , 0x43 , 0x4F , 0x4E , 0x0A , 0x7E , 0x0A , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x43 , 0x53 , 0x0A , 0x7E , 0x0A , 0x30 , 0x32 , 0x41 , 0x38 , 0x03

                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x49, 0x4E, 0x43, 0x4F, 0x4E, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x43, 0x53, 0x0A, 0x7E, 0x0A, 0x30, 0x32, 0x41, 0x38, 0x03 });
                sleep(2);
                
                // hex_login5 0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x41 , 0x33 , 0x0A , 0x7E , 0x0A , 0x44 , 0x44 , 0x38 , 0x44 , 0x03
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x41, 0x33, 0x0A, 0x7E, 0x0A, 0x44, 0x44, 0x38, 0x44, 0x03 });
                sleep(2);
                // hex_login 6 0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x53 , 0x52 , 0x0A , 0x7E , 0x0A , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x41 , 0x43 , 0x52 , 0x41 , 0x43 , 0x54 , 0x0A , 0x7E , 0x0A , 0x2A , 0x0A , 0x52  , 0x45 , 0x51 , 0x0A , 0x4D , 0x45 , 0x4D , 0x0A , 0x7E , 0x0A , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x50 , 0x52 , 0x54 , 0x0A , 0x7E , 0x0A , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x43 , 0x56 , 0x54 , 0x4D , 0x41 , 0x50 , 0x0A , 0x7E , 0x0A , 0x31 , 0x41 , 0x45 , 0x36 , 0x03

                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x53, 0x52, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x41, 0x43, 0x52, 0x41, 0x43, 0x54, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x4D, 0x45, 0x4D, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x50, 0x52, 0x54, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x43, 0x56, 0x54, 0x4D, 0x41, 0x50, 0x0A, 0x7E, 0x0A, 0x31, 0x41, 0x45, 0x36, 0x03 });
                sleep(2);
                             
                
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("RecloserVP_sendConnectCommand", ex.ToString());
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
        private bool GetTotalPower(string s)
        {
            var re = new Regex(Pattern_TotalPower);
            var    match = re.Match(s);
            if (match.Success)
            {
                var type = match.Groups["Type"].Value;
                int v = Convert.ToInt32(match.Groups["Value"].Value);
                switch (type)
                {
                    case "KWH":

                        this.Total = v;
                        break;
                    case "KWHF":
                        this.Forward = v;
                        break;
                    case "KWHR":
                        this.Reverse = v;
                        break;
                }
                return true;
            }
            return false;
                
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
                double v = Convert.ToInt32(match.Groups["Value"].Value);
                double f = Convert.ToInt32(match.Groups["Factor"].Value);
                double c = v / f;

                switch (type)
                {
                    case "MA#":

                        this.Current_IA = v / f; //set it = 0 to test alert or set it > maxamp to test alert

                        break;

                    case "MB#":
                        this.Current_IB = v / f;
                        break;

                    case "MC#":
                        this.Current_IC = v / f;
                        break;

                    case "ME#":
                        this.Current_IE = v / f;
                        break;

                    // Power
                    // .*.DID-281.0.~.*.DID-345.1.~.*.KW#.2592737,1000.~.*.PWFT#.97,100.~.*.FS5060.0.~.1766.					           
                    // .*.UOF12#.50130,1000.~.*.MN.4.0.~.*.MK#.2661267,1000.~.*.MR#.582373,1000.~.*.U1.1.~.DC75.
                    
                    case "KWA#":
                        this.Real_P_A = v / f;
                        break;

                    case "KWB#":
                        this.Real_P_B = v / f;
                        break;
                    case "KWC#":
                        this.Real_P_C = v / f;
                        break;

                    case "PWFTA#":
                        this.Cosphi_A = (float)v / f;
                        break;
                    case "PWFTB#":
                        this.Cosphi_B = (float)v / f;
                        break;
                    case "PWFTC#":
                        this.Cosphi_C = (float)v / f;
                        break;
                    case "MKA#":
                        this.Apparent_S_A = v / f;
                        break;
                    case "MKB#":
                        this.Apparent_S_B = v / f;
                        break;
                    case "MKC#":
                        this.Apparent_S_C = v / f;
                        break;
                    case "MRA#":
                        this.Reactive_Q_A = v / f;
                        break;
                    case "MRB#":
                        this.Reactive_Q_B = v / f;
                        break;
                    case "MRC#":
                        this.Reactive_Q_C = v / f;
                        break;
                }

                return true;
            }

            return false;
        }
    }
}
