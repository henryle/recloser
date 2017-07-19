using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpComm;


namespace RecloserAcq.Device
{
    public class webcommand : RecloserBase
    {
        public webcommand(int port)
            : base(port)
        {
            _buffersize = 128;
        }

        public webcommand()
            : base()
        { 
        }
        public int IDControl = -11111111;
        public string commandstr = string.Empty;
        public string UserID = string.Empty;
        public string commandpassword = string.Empty;
        public override void UpdateData(byte[] data)
        {
            
            //UserID_DeviceID_DONG_Password\r\n
            try
            {
                string reply = Ultility.GetAsciiString(data);
                string[] strs = reply.Split('_');
                IDControl = Convert.ToInt32(strs[2]);
                commandstr = strs[3];
                UserID = strs[1];
                commandpassword = strs[4];
                FA_Accounting.Common.LogService.WriteInfo("UpdateData", strs[1] + strs[2] + strs[3] + strs[4]);
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteInfo("Webcommand",ex.ToString());
            }
            
            base.UpdateData(data);
            
        }
        public override byte[] DoParse(RingBuffer ringBuffer)
        {
            byte[] endbytes = System.Text.Encoding.UTF8.GetBytes("_END");
            byte[] startbytes = System.Text.Encoding.UTF8.GetBytes("START_");
            //int start = ringBuffer.IndexOf(startbytes, 6);
            int end = ringBuffer.IndexOf(endbytes, 4);
            if (end > 0)
            {
                var buff = new byte[end + 4];
                //ringBuffer.Read(buff, start, len);
                ringBuffer.Read(buff, 0, end + 4);
                return buff;
            }
            else
            {
                return null;
            }
        }
    }
}
