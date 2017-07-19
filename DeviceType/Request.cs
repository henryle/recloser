using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecloserAcq.Device;
using TcpComm;

namespace RecloserAcq
{
    public class Request
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public eDeviceType DeviceType { get; set; }
        public bool Used { get; set; }

        public byte[] Getbytes()
        {
            try
            {
                return Ultility.FromHex(this.Text);
            }
            catch
            {
                return null;
            }
        }
    }
}
