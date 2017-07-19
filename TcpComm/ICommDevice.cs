using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpComm
{
    public interface ICommDevice
    {
        int Port { get; }
        void UpdateData(byte[] data);
        byte[] DoParse(RingBuffer buffer); 
        string Name { get; }
    }
}
