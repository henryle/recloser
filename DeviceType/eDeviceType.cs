
namespace RecloserAcq.Device
{
    public enum eDeviceType
    {
        CooperFxb, Nulec, TuBu, Recloser351R, Elster1700, Nulec_U, RecloserADVC, RecloserADVC45, RecloserUSeries, RecloserADVCTCPIP, RecloserVP, LBS
    }
    public enum eAlertVal
    {
        None,Operation, Imag_A0, Imag_B0, Imag_C0, Imag_AMax, Imag_BMax, Imag_CMax, Current_IA0, Current_IB0, Current_IC0,
        Current_IAMax, Current_IBMax, Current_ICMax, Current_IAMin, Current_IBMin, Current_ICMin,
        Curent_AmpPercent, vA_B_src, vB_C_src, vC_A_src, vA_B_load, vB_C_load, vC_A_load,
        Open
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
