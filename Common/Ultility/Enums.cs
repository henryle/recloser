using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FA_Accounting.Common
{
    public enum eGradientType { Panel, Button, Wizard };
    public enum eErrroLevel { Hint, Warning, Error };
    public enum ePermission
    {
        Perm1 = 0, Perm2 = 1, Perm3 = 2
    }
    public enum eAuthenticateMode
    {
        Manual = 0, Trusted = 1, Fixed = 2
    }

    /// <summary>
    /// Text auto trimming mode
    /// </summary>
    public enum eAutoTrimMode
    {
        None = 0, Both = 1, Left = 2, Right = 3
    }

    /// <summary>
    /// Text auto correct case mode
    /// </summary>
    public enum eAutoCorrectCaseMode
    {
        None = 0, Title = 1, Upper = 2, Lower = 3
    }

    /// <summary>
    /// Application authentication mode
    /// </summary>
    //public enum eAuthenticateMode
    //{
    //    Manual = 0, Trusted = 1, Fixed = 2
    //}

    public enum eContactTypeMode 
    {
        GeneralContact = 0, Doctor = 1, OutsideDoctor = 2, Staff = 3, Patient = 4 
    }
    
    /// <summary>
    /// Admin key mode
    /// </summary>
    public enum eAdminKeyMode 
    { 
        Global = 0, Lookup = 1 , AppointmentSetting = 2
    }

    /// <summary>
    /// Appointment NotificationStatus
    /// 0 none; 1 Patient notified SMS, phone, post, email; 2 Patient Replied Confirmed to come; 3 Patient replied cancel appt
    /// </summary>
    public enum eApptNotificationStatus
    { 
        None = 0, Notify = 1, Confirm = 2, Cancel = 3
    }

    /// <summary>
    /// Appointment Status
    /// None 0; InWatingroom 1; Left 2; Missing 3; Billed 4
    /// </summary>
    //public enum eApptStatus
    //{ 
    //    None = 0, Arrived = 1, Left = 2, Missing = 3, Billed = 4
    //}

    /// <summary>
    /// SMS send 0, receipt 1
    /// </summary>
    public enum eSMSDirection
    { 
        Send = 0, Receipt = 1
    }    
}
