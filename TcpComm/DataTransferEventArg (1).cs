using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpComm
{
    public class DataTransferEventArg:EventArgs
    {
        public int Port { get; private set; }
        public byte[] Data { get; private set; }

        public DataTransferEventArg(byte[] data, int port)
            : base()
        {
            this.Data = data;
            this.Port = port;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Port, Ultility.ToHexText(Data));
        }
    }
}
