using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpComm
{
    public class StatusChangedEventArgs : EventArgs
    {
        public string Status { get; private set; }

        public StatusChangedEventArgs(string status)
            : base()
        {
            this.Status = status;
        }
    }
}
