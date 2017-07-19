using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecloserAcq.Device
{
    public enum eDeviceType
    {
        CooperFxb, Nulec, TuBu, Recloser351R, Elster1700, Nulec_U, RecloserVP
    }
    public enum eControlType
    {
        No,Cosphi,Q
    }
    public enum eAlertSoundStatus
    { 
        None, Playing, Stopped
    }
    public enum eAlertOpenClose
    {
        None,Open,Close
    }
}
