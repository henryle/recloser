using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecloserAcq.OracleDAL;
using TcpComm;
using System.IO;

namespace RecloserAcq.Device
{
    public class LBS: RecloserBase
    {
        const byte LBS_ON_OFF_MASK = (byte)0x03; //bit 0,1
        const byte LBS_STATUS_ON_FB = (byte)0x01; //bit0
        const byte LBS_STATUS_OFF_FB = (byte)0x02; //bit1

        const byte LBS_AF_VAL = (byte)0x03;
        const byte LBS_NF_VAL = (byte)0x00;
        const byte LBS_CLOSE_VAL = (byte)0x01;
        const byte LBS_TRIP_VAL = (byte)0x02;


        const byte LBS_STATUS_MOTOR_MASK = (byte)0x0C; //bit 2 ,3 --> set in execute cmd
        const byte LBS_STATUS_MOTOR_OK = (byte)0x00;
        const byte LBS_STATUS_MOTOR_ERR_NO_LOAD = (byte)0x04;
        const byte LBS_STATUS_MOTOR_ERR_TIME_OUT = (byte)0x08;
        const byte LBS_STATUS_MOTOR_LOAD_ERR_TIME_OUT = (byte)0x0C;


        const byte LBS_STATUS_MOTOR_SW_MASK = (byte)0x30; //bit 4 ,5
        const byte LBS_STATUS_MOTOR_SW_ON = (byte)0x10;
        const byte LBS_STATUS_MOTOR_SW_OFF = (byte)0x20;	
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
        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        
        private string _motorstatus;
        public string MotorStatus
        {
            get
            {
                return _motorstatus;
            }
            set
            {
                _motorstatus = value;
            }
        }
        public LBS(int port)
            : base(port)
        {
            _buffersize = 128;
            _deviceTime = null;
        }

        public LBS()
            : this(0)
        {
            _deviceTime = null;
            
        }
        private byte[] ParseHex(string hex)
        {
            int offset = hex.StartsWith("0x") ? 2 : 0;
            if ((hex.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid length: " + hex.Length);
            }

            var ret = new byte[(hex.Length - offset) / 2];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)((ParseNybble(hex[offset]) << 4)
                                 | ParseNybble(hex[offset + 1]));
                offset += 2;
            }
            return ret;
        }
        private int ParseNybble(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }

            throw new ArgumentException("Invalid hex digit: " + c);
        }
        private Boolean CheckValidCalendar(byte idx,byte[] dataCMDbuff)
        {
            //byte idx=10;
            int val = 0;
            //dd
            val = dataCMDbuff[idx++];
            if ((val & 0x0F) > 0x09)
            {
                return false;
            }

            if ((val & 0xF0) > 0x30)
            {
                return false;
            }
            //mm
            val = dataCMDbuff[idx++];
            if ((val & 0x0F) > 0x09)
            {
                return false;
            }

            if ((val & 0xF0) > 0x10)
            {

                return false;
            }
            else
            {
                if (((val & 0x0F) > 0x02) && ((val & 0xF0) == 0x10))
                {
                    return false;
                }

            }

            //yy
            val = dataCMDbuff[idx++];
            if ((val & 0x0F) > 0x09)
            {
                return false;
            }

            if ((val & 0xF0) > 0x90)
            {
                return false;
            }

            //hh
            val = dataCMDbuff[idx++];
            if ((val & 0x0F) > 0x09)
            {
                return false;
            }

            if ((val & 0xF0) > 0x20)
            {
                return false;
            }
            else
            {

                if (((val & 0xF0) == 0x20) && ((val & 0x0F) > 0x03))
                {
                    return false;
                }

            }

            //mm
            val = dataCMDbuff[idx++];
            if ((val & 0x0F) > 0x09)
            {
                return false;
            }

            if ((val & 0xF0) > 0x50)
            {
                return false;
            }

            //ss
            val = dataCMDbuff[idx++];
            if ((val & 0x0F) > 0x09)
            {
                return false;
            }

            if ((val & 0xF0) > 0x50)
            {
                return false;
            }

            //day of week
            val = dataCMDbuff[idx++];
            if ((val & 0x0F) > 0x07)
            {
                return false;
            }
            if ((val & 0x0F) == 0x00)
            {
                return false;
            }

            if ((val & 0xF0) > 0x00)
            {
                return false;
            }
            return true;


        }
        public override void SetTime()
        {
            
            byte[] dataCMDbuffchksum = new byte[255];
            byte[] dataCMDbuff = new byte[20];
            try
            {
                DateTime now = DateTime.Now;
                
                dataCMDbuffchksum[0] = 0x52;/*R*/
                dataCMDbuffchksum[1] = 0x54;/*T*/;
                dataCMDbuffchksum[2] = 0x0B;//len =11
                dataCMDbuffchksum[3] = 0x57;/*W*/
                var buff = ParseHex(now.ToString("ddMMyy"));
                dataCMDbuffchksum[4] = buff[0];
                dataCMDbuffchksum[5] = buff[1];
                dataCMDbuffchksum[6] = buff[2];

                buff = ParseHex(now.ToString("HHmmss"));
                
                dataCMDbuffchksum[7] = buff[0];
                dataCMDbuffchksum[8] = buff[1];
                dataCMDbuffchksum[9] = buff[2];

                buff = ParseHex(((int)now.DayOfWeek + 1).ToString().PadLeft(2, '0'));
                dataCMDbuffchksum[10] = buff[0];

                int checksum = CheckSumTB.crc16_check(dataCMDbuffchksum, 0x0F);
                var buffcrc = BitConverter.GetBytes(checksum);

                dataCMDbuff[0] = 0x01B;
                dataCMDbuff[1] = 0x032;
                dataCMDbuff[2] = 0x001;
                dataCMDbuff[3] = 0x02A;
                dataCMDbuff[4] = 0x00A;
                dataCMDbuff[5] = 0x00F; /*total len = data len+4*/
                dataCMDbuff[6] = 0x52;/*R*/
                dataCMDbuff[7] = 0x54;/*T*/;
                dataCMDbuff[8] = 0x0B;//len =11
                dataCMDbuff[9] = 0x57;/*W*/
                //System.Buffer.BlockCopy(dataCMDbuffchksum, 4, dataCMDbuff, 10, 7);
                dataCMDbuff[10] = dataCMDbuffchksum[4];
                dataCMDbuff[11] = dataCMDbuffchksum[5];
                dataCMDbuff[12] = dataCMDbuffchksum[6];
                dataCMDbuff[13] = dataCMDbuffchksum[7];
                dataCMDbuff[14] = dataCMDbuffchksum[8];
                dataCMDbuff[15] = dataCMDbuffchksum[9];
                dataCMDbuff[16] = dataCMDbuffchksum[10];
                dataCMDbuff[17] = buffcrc[0];
                dataCMDbuff[18] = buffcrc[1];
                dataCMDbuff[19] = 0x003;

                if (CheckValidCalendar(10,dataCMDbuff) == false)
                {
                    throw new Exception("date format is not valid");
                }
                Listener.Send(dataCMDbuff);

            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Send request Đóng LBS
        /// </summary>
        private void Close()
        {
            try
            {
                sleep(1.5);
                byte[] dataCMDbuffchksum = new byte[255];
                byte[] dataCMDbuff = new byte[16];
                dataCMDbuffchksum[0] = 0x49;/*I*/
                dataCMDbuffchksum[1] = 0x4F;/*O*/
                dataCMDbuffchksum[2] = 0x007;
                dataCMDbuffchksum[3] = 0x57;/*W*/
                dataCMDbuffchksum[4] = 0x010;
                dataCMDbuffchksum[5] = 0x000;
                dataCMDbuffchksum[6] = 0x000;
                dataCMDbuffchksum[7] = 0x0F6;
                dataCMDbuffchksum[8] = 0x058;
                dataCMDbuffchksum[9] = 0x003;

                int checksum = CheckSumTB.crc16_check(dataCMDbuffchksum, 0x0B);
                var buffcrc = BitConverter.GetBytes(checksum);
                
                dataCMDbuff[0] = 0x01B;
                dataCMDbuff[1] = 0x032;
                dataCMDbuff[2] = 0x001;
                dataCMDbuff[3] = 0x02A;
                dataCMDbuff[4] = 0x00A;
                dataCMDbuff[5] = 0x00B;

                dataCMDbuff[6] = 0x49;
                dataCMDbuff[7] = 0x4F;
                dataCMDbuff[8] = 0x007;
                dataCMDbuff[9] = 0x57;
                dataCMDbuff[10] = 0x010;
                dataCMDbuff[11] = 0x000;
                dataCMDbuff[12] = 0x000;

                dataCMDbuff[13] = buffcrc[0];
                dataCMDbuff[14] = buffcrc[1];

                dataCMDbuff[15] = 0x003;
                Listener.Send(dataCMDbuff);

                System.Threading.Thread.Sleep(200); 

                this.Status_Close = true;
                this.Status_Open = false;
                
                this.WriteRow();
                System.Windows.Forms.MessageBox.Show("Lệnh đóng đã được gởi tới LBS " + this.Location);
            }
            catch (Exception)
            {
            }
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
                else
                {
                    this.Close();
                    this.Stop();
                    this.Start();
                }
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Close LBS", ex.Message, ex);
            }
            base.CommandClose(auto);
        }
        
        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.LBS;
            }
        }
        public override void CommandOpen(bool auto = false)
        {
            
            SetTime();
            try
            {
                if (!Listener.IsConnected)
                {
                    Listener.Restart();
                    System.Threading.Thread.Sleep(1000);
                }
                byte[] dataCMDbuffchksum = new byte[255];
                byte[] dataCMDbuff = new byte[16];
                //RequestOpenCloseTime();
                dataCMDbuffchksum[0] = 0x49;/*I*/
                dataCMDbuffchksum[1] = 0x4F;/*O*/
                dataCMDbuffchksum[2] = 0x007;
                dataCMDbuffchksum[3] = 0x57;/*W*/
                dataCMDbuffchksum[4] = 0x011;
                dataCMDbuffchksum[5] = 0x000;
                dataCMDbuffchksum[6] = 0x000;
                dataCMDbuffchksum[7] = 0x0F6;
                dataCMDbuffchksum[8] = 0x058;
                dataCMDbuffchksum[9] = 0x003;

                int checksum = CheckSumTB.crc16_check(dataCMDbuffchksum, 0x0B);
                var buffcrc = BitConverter.GetBytes(checksum);
                
                dataCMDbuff[0] = 0x01B;
                dataCMDbuff[1] = 0x032;
                dataCMDbuff[2] = 0x001;
                dataCMDbuff[3] = 0x02A;
                dataCMDbuff[4] = 0x00A;
                dataCMDbuff[5] = 0x00B;

                dataCMDbuff[6] = 0x49;
                dataCMDbuff[7] = 0x4F;
                dataCMDbuff[8] = 0x007;
                dataCMDbuff[9] = 0x57;
                dataCMDbuff[10] = 0x011;
                dataCMDbuff[11] = 0x000;
                dataCMDbuff[12] = 0x000;

                dataCMDbuff[13] = buffcrc[0];
                dataCMDbuff[14] = buffcrc[1];

                dataCMDbuff[15] = 0x003;
                Listener.Send(dataCMDbuff);


                System.Threading.Thread.Sleep(300);


                this.Status_Close = false;
                this.Status_Open = true;
                

                this.Stop();
                this.Start();
                System.Windows.Forms.MessageBox.Show("Lệnh mở đã được gởi tới LBS " + this.Location);

                this.WriteRow();
                base.CommandOpen();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Open LBS", ex.Message, ex);
            }
        }
        
        public override void UpdateData(byte[] data)
        {
            // after Parser, the data now is from nByteSend to EndBlock (0x03), len is = nByteSend
            // include nByteSend byte
            if (currentRequest.Name == "ReadStatus")
            {
                GetStatus(data);
            }
            if (currentRequest.Name == "ReadDate")
            {
                GetDateTime(data);
            }
            if (bGetLastOpt)
            {
                GetLastOperation(data);
            }
            base.UpdateData(data);
            
            WriteRow();
        }
        private bool bGetLastOpt = false;
        private void RequestLastOperation()
        {
            byte[] cmd = new byte[] { 0x1b, 0x032, 0x01, 0x2a, 0x0a, 0x0a, 0x52, 0x48, 0x06, 0x52, 0x01, 0x00, 0x65, 0x30, 0x03 };
            Listener.Send(cmd);
            bDataReturned = false;
            bGetLastOpt = true;
        }
        //private bool _lastopt;
        //public bool LastOpt
        //{
        //    get
        //    {
        //        return _lastopt;
        //    }
        //    set
        //    {
        //        _lastopt = value;
        //    }
        //}
        private void GetLastOperation(byte[] data)
        {
            byte status = data[9];//lay byte status va check bit 0 1 2 3
            byte val = (byte)(status & LBS_ON_OFF_MASK);
            switch (val)
            {
                case LBS_CLOSE_VAL:
                    //res_lbs_hw_fb = "lbsAF: lbs trip&close the same time"; //Bi loi cam bien -->
                    // = "lbsAF: lbs trip&close the same time"; //Bi loi cam bien -->
                   
                    this.Status_Close = true;
                    this.Status_Open = false;
                    
                    this.Status = "Closed";
                    break;
                   
                
                
                case LBS_TRIP_VAL:
                    
                    Status_Close = false;
                    Status_Open = true;
                    
                    this.Status = "Opened";
                    break;

            }
            
            LastUpdated = DateTime.Now;
        }

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
                    
                    LastUpdated = DateTime.Now;
                    return true;
                }
                
                return true;
            }
            return false;
        }	
        private void GetStatus(byte[] data)
        {
            
            byte status = data[8];//lay byte status va check bit 0 1 2 3
            byte val = (byte)(status & LBS_ON_OFF_MASK); //mask bit 0 va bit 1 --> check sensor hành trình
            switch (val)
            {
                case LBS_AF_VAL:
                    //res_lbs_hw_fb = "lbsAF: lbs trip&close the same time"; //Bi loi cam bien -->
                    // = "lbsAF: lbs trip&close the same time"; //Bi loi cam bien -->
                    Status = "Error";
                    
                    break;
                case LBS_NF_VAL:
                    //res_lbs_hw_fb = "lbsNF: lbs no connect";// motor o giua hanh trinh ON va OFF
                    if (this.Status_Close)
                    {
                        Status = "Closed";
                    }
                    else
                    {
                        Status = "Opened";
                    }

                    
                    break;
                case LBS_CLOSE_VAL:
                    this.Status_Close = true;
                    this.Status_Open = false;
                    
                    Status = "Closed";
                    break;
                case LBS_TRIP_VAL:
                    this.Status_Close = false; //Status_Close is set before Status_Open
                    this.Status_Open = true;
                    Status = "Opened";
                    
                    break;

            }

            val = (byte)(status & LBS_STATUS_MOTOR_MASK);//mask bit 2 va bit 3 --> check trang thai motor
            switch (val)
            {
                case LBS_STATUS_MOTOR_OK:
                    //res_lbs_motor_status = "MoOK: Motor OK";
                    MotorStatus = "OK";
                    
                    break;
                case LBS_STATUS_MOTOR_ERR_NO_LOAD:
                    MotorStatus = "Motor not connect to Load";
                    
                    break;
                case LBS_STATUS_MOTOR_ERR_TIME_OUT:
                    MotorStatus= "Motor timeout (failed limit FB)";
                    
                    break;
                case LBS_STATUS_MOTOR_LOAD_ERR_TIME_OUT:
                    MotorStatus = "Motor Load timeout(motor stuck)";
                    
                    break;

            }

            //val = (byte)(status & LBS_STATUS_MOTOR_SW_MASK);
            //switch (val)
            //{
            //    case LBS_STATUS_MOTOR_SW_ON:
            //        res_lbs_sw_on_off = "SwC: Switch closed postiton";
            //        break;
            //    case LBS_STATUS_MOTOR_SW_OFF:
            //        res_lbs_sw_on_off = "SwT: Switch trip postiton";
            //        break;

            //    default:
            //        res_lbs_sw_on_off = "SwE: Switch err";
            //        break;

            //}

            LastUpdated = DateTime.Now;
           
        }
      
        public override void WriteRow()
        {
             //string dvpath = RecloserAcq.Properties.Settings.Default.dvpath.Trim();
           
            string filePath = _devicepath + Port + ".csv";
            string strFields = "TestField\r\n";
            string strvalues = "TestValue";
            try
            {
                strFields = "DeviceId,Type,Date,Alert,Mo,Dong,Status,MotorStatus,DeviceTime \r\n";

                strvalues = Id.ToString() + "," +
                    DeviceType.ToString() + "," +
                    LastUpdated.ToString() + "," +
                    "0," +
                    Status_Open.ToString() + "," +
                    Status_Close.ToString() + "," +
                    Status.ToString() + "," +
                    MotorStatus.ToString() + "," +
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
                catch 
                {
                    
                }
            }
            catch 
            {
                
            }
        }
        



    
        protected override void SaveData()
        {
            // this is only Called when status_open /close is changed
            DBController.Instance.SaveLBS(this,DateTime.Now, this.Status_Open==true, false);
            
        }


        public override void StartPoll()
        {
            RequestLastOperation();
            base.StartPoll();
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
        private readonly byte[] Answer_Header = new byte[] { 0x01, 0x2A, 0x0A };
        
        public override byte[] DoParse(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(Answer_Header, 3);
            if (start != -1 && ringBuffer.Count > start + 1)
            {
                int len = ringBuffer[start + 3];

                if (ringBuffer.Count >= len)
                {
                    var buff = new byte[len];
                    ringBuffer.Read(buff, start, len);
                    ringBuffer.Reset();
                    bDataReturned = true;
                    return buff;
                }

            }

            return null;
        }
    }
}
