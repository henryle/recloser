using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using FA_Accounting.Common;

namespace RecloserAcq.Device
{
    public class RecloserUSeries : RecloserADVC
    {
         public RecloserUSeries(int port)
             : base(port)
         {
             _deviceTime = null;
         }

         public RecloserUSeries(): this(0)            
         { }
        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.RecloserUSeries;
            }
        }
        public override string Pattern_Power
        {
            get
            {
                return @"(?<Type>KW|MK|MR)\n(?<Value>-?\d{2,7})";
            }
            set { }
        }
        public string Pattern_PWFT = @"PWFT\n(?<Value>\d{1}\.\d{2})";
        public override string Pattern_Current
        {
            get
            {
                return @"(?<Type>M[ABCE])\n(?<Value>\d{2,6})";
            }
            
        }
        public override string Pattern_Voltage
        {
            get
            {
                return @"(?<Type>MAV|MBV|MCV|MEV|MFV|MDV|MGV|MHV|MIV|MJV|MKV|MLV)\n(?<Value>-?\d{1,7})";
            }
            set { }
        }
        protected override bool GetCurrent(string s)
        {
            var re = new Regex(Pattern_Current);
            var match = re.Match(s);
            if (!match.Success)
            {
                return false;
            }
            var type = match.Groups["Type"].Value;
            double v = Convert.ToDouble(match.Groups["Value"].Value);
            //double f = Convert.ToDouble(match.Groups["Factor"].Value);
            //double c = v / f;

            switch (type)
            {
                case "MA":

                    this.Current_IA = v;  //set it = 0 to test alert or set it > maxamp to test alert

                    break;

                case "MB":
                    this.Current_IB = v ;
                    break;

                case "MC":
                    this.Current_IC = v ;
                    break;

                case "ME":
                    this.Current_IE = v ;
                    break;
            }
            return true;
        }
        protected override bool GetPower(string s)
        {
            var re = new Regex(Pattern_Power);
            var match = re.Match(s);



            if (match.Success)
            {
                var type = match.Groups["Type"].Value;
                double v = Convert.ToDouble(match.Groups["Value"].Value);

                //double c = v / f;

                switch (type)
                {


                    // Power
                    // .*.DID-281.0.~.*.DID-345.1.~.*.KW#.2592737,1000.~.*.PWFT#.97,100.~.*.FS5060.0.~.1766.					           
                    // .*.UOF12#.50130,1000.~.*.MN.4.0.~.*.MK#.2661267,1000.~.*.MR#.582373,1000.~.*.U1.1.~.DC75.
                    case "KW":
                        this.RealPower = v;
                        break;

                    case "MK":
                        this.ApparentPower = v;
                        break;

                    case "MR":
                        this.ReactivePower = v;
                        break;

                }

                return true;
            }
            else
            {
                return GetPWFT(s);
            }
            
        }
        protected bool GetPWFT(string s)
        {
            var re = new Regex(Pattern_PWFT);
            var match = re.Match(s);



            if (match.Success)
            {
                
                 this.PowerFactor  = Convert.ToDouble(match.Groups["Value"].Value);

                //double c = v / f;

                

                return true;
            }

            return false;
        }
        protected override bool GetVoltage(string s)
        {
            var re = new Regex(Pattern_Voltage);
            var match = re.Match(s);



            if (match.Success)
            {
                var type = match.Groups["Type"].Value;
                double v = Convert.ToDouble(match.Groups["Value"].Value);
                //double f = Convert.ToDouble(match.Groups["Factor"].Value);
                //double c = v / f;

                switch (type)
                {
                    case "MAV":

                        this.VA_GND_load = v; //set it = 0 to test alert or set it > maxamp to test alert

                        break;

                    case "MBV":
                        this.VB_GND_load = v;
                        break;

                    case "MCV":
                        this.VC_GND_load = v;
                        break;

                    case "MDV":
                        this.VA_GND_src = v;
                        break;
                    case "MEV":
                        this.VB_GND_src = v;
                        break;
                    case "MFV":
                        this.VC_GND_src = v;
                        break;
                    case "MGV":
                        this.VA_B_load = v;
                        break;
                    case "MHV":
                        this.VB_C_load = v;
                        break;
                    case "MIV":
                        this.VC_A_load = v;
                        break;
                    case "MJV":
                        this.VA_B_src = v;
                        break;
                    case "MKV":
                        this.VB_C_src = v;
                        break;
                    case "MLV":
                        this.VC_A_src = v;
                        break;
                }

                return true;
            }

            return false;

        }
        public override bool sendConnectCommand()
        {
            try
            {
                timerBetweenEachPoll.Stop();
                timerBetweenEachRequest.Stop();
                //1B 32 01 2A 0A 52 45 51 0A 4F 53 0A 7E 0A 34 42 46 37 03
                //hex_login_init1: 0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x4F , 0x53 , 0x0A , 0x7E , 0x0A , 0x34 , 0x42 , 0x46 , 0x37 , 0x03 , 0x1B , 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x4F , 0x53 , 0x0A , 0x7E , 0x0A , 0x34 , 0x42 , 0x46 , 0x37 , 0x03
                
                //Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x4F, 0x53, 0x0A, 0x7E, 0x0A, 0x34, 0x42, 0x46, 0x37, 0x03, 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x4F, 0x53, 0x0A, 0x7E, 0x0A, 0x34, 0x42, 0x46, 0x37, 0x03 });
                //sleep(2);
                Listener.Send(new byte[] {0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x4F , 0x53 , 0x0A , 0x7E , 0x0A , 0x34 , 0x42 , 0x46 , 0x37 , 0x03});
                sleep(2);
                //2: 1B 32 01 2A 0A 52 45 51 0A 44 49 44 2D 33 36 30   .2.*.REQ.DID-360
                //0A 7E 0A 45 39 46 30 03 
                //3: 1B 32 01 2A 0A 52 45 51 0A 43 42 0A 7E 0A 41 44   .2.*.REQ.CB.~.AD
                //43 46 03 
                //4: 1B 32 01 2A 0A 52 45 51 0A 41 33 0A 7E 0A 44 44   .2.*.REQ.A3.~.DD
                //38 44 03  
                //5: 1B 32 01 2A 0A 52 45 51 0A 53 52 0A 7E 0A 2A 0A   .2.*.REQ.SR.~.*.
                 //52 45 51 0A 41 43 52 41 43 54 0A 7E 0A 2A 0A 52   REQ.ACRACT.~.*.R
                 //45 51 0A 41 43 52 43 4F 4E 0A 7E 0A 39 46 38 41   EQ.ACRCON.~.9F8A
                 //03
                //6: 1B 32 01 2A 0A 52 45 51 0A 46 4F 0A 7E 0A 2A 0A   .2.*.REQ.FO.~.*.
                 // 52 45 51 0A 43 46 0A 7E 0A 39 35 30 43 03  
                //7: 1B 32 01 2A 0A 52 45 51 0A 53 4F 53 0A 7E 0A 2A   .2.*.REQ.SOS.~.*
                 //0A 52 45 51 0A 53 45 46 0A 7E 0A 2A 0A 52 45 51   .REQ.SEF.~.*.REQ
                 //0A 4C 4F 50 0A 7E 0A 2A 0A 52 45 51 0A 50 58 0A   .LOP.~.*.REQ.PX.
                 //7E 0A 34 32 31 42 03
                //8: 1B 32 01 2A 0A 52 45 51 0A 43 41 0A 7E 0A 2A 0A   .2.*.REQ.CA.~.*.
                //52 45 51 0A 43 53 0A 7E 0A 45 42 33 36 03
                //9: 01 2A 0A 54 4E 0A 32 30 0A 31 0A 20 0A 7E 0A 42   .*.TN.20.1. .~.B
                //34 42 36 03
                // hex_login_init2: 0x1B, 0x32, 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x44 , 0x49 , 0x44 , 0x2D , 0x33 , 0x36 , 0x31  , 0x0A , 0x7E , 0x0A , 0x39 , 0x46 , 0x34 , 0x34 , 0x03
                /*Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x44, 0x49, 0x44, 0x2D, 0x33, 0x36, 0x31, 0x0A, 0x7E, 0x0A, 0x39, 0x46, 0x34, 0x34, 0x03 });
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
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x53, 0x4F, 0x53, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x53, 0x45, 0x46, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x4C, 0x4F, 0x50, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x50, 0x58, 0x0A, 0x7E, 0x0A, 0x34, 0x32, 0x31, 0x42, 0x03 });
                sleep(2);
                Listener.Send(new byte[] { 0x1B, 0x32, 0x01, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x53, 0x4F, 0x53, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x53, 0x45, 0x46, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x4C, 0x4F, 0x50, 0x0A, 0x7E, 0x0A, 0x2A, 0x0A, 0x52, 0x45, 0x51, 0x0A, 0x50, 0x58, 0x0A, 0x7E, 0x0A, 0x34, 0x32, 0x31, 0x42, 0x03 });
                sleep(2);*/
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("RecloserUSeries_sendConnectCommand", ex.ToString());
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

        /***   copy from RecloserADVC45 */
    }
}
