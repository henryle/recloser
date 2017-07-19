using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TcpComm;
using System.Xml.Serialization;
using System.Timers;
using FA_Accounting.Common;
using System.Threading;
using RecloserAcq.OracleDAL;
//using RecloserAcq.Comm;

namespace RecloserAcq.Device
{
    [System.Xml.Serialization.XmlInclude(typeof(CooperFXB))]
    [System.Xml.Serialization.XmlInclude(typeof(Nulec))]
    [System.Xml.Serialization.XmlInclude(typeof(Nulec_U))]
    [System.Xml.Serialization.XmlInclude(typeof(TuBu))]
    [System.Xml.Serialization.XmlInclude(typeof(Elster1700))]
    [System.Xml.Serialization.XmlInclude(typeof(Recloser351R))]
    [System.Xml.Serialization.XmlInclude(typeof(RecloserADVC))]
    [System.Xml.Serialization.XmlInclude(typeof(RecloserADVC45))]
    [System.Xml.Serialization.XmlInclude(typeof(RecloserUSeries))]
    [System.Xml.Serialization.XmlInclude(typeof(RecloserADVCTCPIP))]
    [System.Xml.Serialization.XmlInclude(typeof(RecloserVP))]
    [System.Xml.Serialization.XmlInclude(typeof(LBS))]
    [System.Xml.Serialization.XmlInclude(typeof(RecloserBase))]
    public class RecloserBase:INotifyPropertyChanged, ICommDevice
    {
        public event EventHandler OnDataUpdated;
        //public static int MaxAmp = 300;
        //public static int MaxZeroDuration = 3;
        //public static int MaxDuration = 3;
        public int Testing = 0;
        protected System.Timers.Timer timerBetweenEachRequest = new System.Timers.Timer();
        protected System.Timers.Timer timerBetweenEachPoll = new System.Timers.Timer();
        protected System.Timers.Timer timerSaveHistory = new System.Timers.Timer();
        protected System.Timers.Timer timercheckalert = new System.Timers.Timer();
        protected System.Timers.Timer timerSuspend = new System.Timers.Timer();
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void Notify(string prop, bool markDirty = true)
        {
            if(markDirty)
                _isDirty = true;

            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        } 
        #endregion

        #region General info
        private int _operations;
        [XmlIgnore]
        public int Operations
        {
            get { return _operations; }
            set
            {
                if (_operations != value)
                {
                    if (value == _operations +1)
                    {
                        /*try
                        {
                            SendAmpRequest();

                            LogService.WriteInfo("Bao dong operation", "");
                        }
                        catch 
                        { }*/
                        FA_Accounting.Common.LogService.WriteInfo("Bao dong operation", "");
                        _operations = value;
                        _alertVal = (int)eAlertVal.Operation;
                        SaveData();
                        Notify("AlertVal");
                    }

                    _operations = value;

                    Notify("Operations");
                }
                else
                {
                    if (_alertVal == (int)eAlertVal.Operation)
                    {
                        _alertVal = (int)eAlertVal.None;
                        
                        Notify("AlertVal");
                    }
                }
            }
        }
        public bool isnew = false;
        private string _battery_1;
        [XmlIgnore]
        public string Battery_1
        {
            get { return _battery_1; }
            set
            {
                if (_battery_1 != value)
                {
                    _battery_1 = value;
                    Notify("Battery_1");
                }
            }
        }

        private string _battery_2;
        [XmlIgnore]
        public string Battery_2
        {
            get { return _battery_2; }
            set 
            {
                if (_battery_2 != value)
                {
                    _battery_2 = value;
                    Notify("Battery_2");
                }
            }
        }
        #endregion

        #region Current
        protected DateTime? _startZeroA;
        protected DateTime? _startZeroB;
        protected DateTime? _startZeroC;
        protected int _preAlertAmp12;
        protected int _maxampduration;
        
        public virtual int Maxampduration
        {
            get { return _maxampduration; }
            set
            {
                if (_maxampduration != value)
                {
                    _maxampduration = value;

                }
            }
        }
        protected int _minamp;
        
        public virtual int MinAmp
        {
            get { return _minamp; }
            set
            {
                if (_minamp != value)
                {
                    _minamp = value;

                }
            }
        }
        protected int _volStandard;
        public virtual int VolStandard
        {
            get { return _volStandard; }
            set
            {
                if (_volStandard != value)
                {
                    _volStandard = value;
                    Notify("VolStandard");    
                }
                
            }
        }
        protected int _volpercent;
        public virtual int Volpercent
        {
            get { return _volpercent; }
            set
            {
                if (_volpercent != value)
                {
                    _volpercent = value;
                    Notify("Volpercent");
                }
            }
        }
        protected int _maxamp;
        public virtual int MaxAmp
        {
            get { return _maxamp; }
            set
            {
                if (_maxamp != value)
                {
                    _maxamp = value;

                }
            }
        }
        protected int _amp12;

        [XmlIgnore]
        public virtual int Amp12
        {
            get { return _amp12; }
            set 
            {
                if (_amp12 != value)
                {
                    _amp12 = value;
                   
                    Notify("Amp12");
                }
            }
        }

        protected int _amp34;
        [XmlIgnore]
        public virtual int Amp34
        {
            get { return _amp34; }
            set
            {
                if (_amp34 != value)
                {
                    _amp34 = value;
                    Notify("Amp34");
                }
            }
        }

        protected int _amp56;
        [XmlIgnore]
        public virtual int Amp56
        {
            get { return _amp56; }
            set
            {
                if (_amp56 != value)
                {
                    _amp56 = value;
                    Notify("Amp56");
                }
            }
        }

        protected int _ampEarth;
        [XmlIgnore]
        public virtual int AmpEarth
        {
            get { return _ampEarth; }
            set
            {
                if (_ampEarth != value)
                {
                    _ampEarth = value;
                    Notify("AmpEarth");
                }
            }
        }
        // kw# 
        protected double _realPower;
        [XmlIgnore]
        public double RealPower
        {
            get { return _realPower; }
            set
            {
                if (_realPower != value)
                {
                    _realPower = value;
                    Notify("RealPower");
                }
            }
        }
        // PWFTH# PWFT# WFT#
        protected double _powerFactor;
        [XmlIgnore]
        public double PowerFactor
        {
            get { return _powerFactor; }
            set
            {
                if (_powerFactor != value)
                {
                    _powerFactor = value;
                    Notify("PowerFactor");
                }
            }
        }
        #endregion
        #region Power
        // MK#
        protected double _apparentPower;
        [XmlIgnore]
        public double ApparentPower
        {
            get { return _apparentPower; }
            set
            {
                if (_apparentPower != value)
                {
                    _apparentPower = value;
                    Notify("ApparentPower");
                }
            }
        }

        // MR#
        protected double _reactivePower;
        [XmlIgnore]
        public double ReactivePower
        {
            get { return _reactivePower; }
            set
            {
                if (_reactivePower != value)
                {
                    _reactivePower = value;
                    Notify("ReactivePower");
                }
            }
        }
        #endregion

        #region Recloser status
        //public int alerting = 0; // 1,2 : has something wrong, need to alert ->_alertVal
        //1. Color 
        //2. Alert Nulec and Cooper
        //3. Alert Reclosersel
        //4. Auto close tubu

        //1. 0 -> 0 again (this time , alert or not , play sound or not) , max , max again
        //2. when it's in alert state, -> ampere 'back to normal, -> stop alert 
        protected int _alertVal;
        public bool DisableAlert = false;
        protected virtual void setAlert(int val, ref DateTime? start)
        {
            if (_alertVal == val)
            {
                return;
            }
            if (start == null)
            {
                start = DateTime.Now;
            }
            else
            {
                if ((DateTime.Now - start.Value).TotalSeconds > Maxampduration && Maxampduration!=0)
                {
                    _alertVal = val;
                    
                    Notify("AlertVal");
                    SaveData();
                }
            }
        }
        protected DateTime latestClosedt;
        protected DateTime latestOpendt;
        private bool _status_Open;
        [XmlIgnore]
        public bool Status_Open
        {
            get { return _status_Open; }
            set
            {
                if (_status_Open != value)
                {
                    
                    _status_Open = value;
                    if (_status_Open == true && _status_Close == true)
                    {
                        latestOpendt = DateTime.Now;
                        //if((latestOpendt - latestClosedt).TotalSeconds > Maxampduration

                        // alert and save 

                        _alertVal = (int)eAlertVal.Open;
                        SaveData();
                        //LogService.WriteInfo("eeeeeeeeeeee", "fffffffffffffff");
                        Notify("AlertVal");

                    }
                    

                    Notify("Status_Open");
                }
            }
        }

        private bool _status_Lockout;
        [XmlIgnore]
        public bool Status_Lockout
        {
            get { return _status_Lockout; }
            set
            {
                if (_status_Lockout != value)
                {
                    _status_Lockout = value;
                    Notify("Status_Lockout");
                }
            }
        }

        private bool _status_Close;
        [XmlIgnore]
        public bool Status_Close
        {
            get { return _status_Close; }
            set
            {
                if (_status_Close != value)
                {
                    _status_Close = value;
                    
                    if (_alertVal == (int)eAlertVal.Open)
                    {
                        _alertVal = (int)eAlertVal.None;
                        //LogService.WriteInfo("hhhhhhhhhhh", "BBBBBBBB");
                        Notify("AlertVal");
                    }
                    
                    Notify("Status_Close");
                    SaveData();

                }
            }
        } 
        #endregion        

        #region Identity Recloser base attributes
        protected bool acceptSendingRequest = false; //only used for ADVC
        protected int pollingcount = 0;
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public DateTime? DateDeleted = null;
        public eControlType controlbyNo_Cosphi_Q = eControlType.No; // 0:no , 1: Cosphi 2: Q
        private float _maxQclose;
        public float MaxQClose
        {
            get { return _maxQclose; }
            set
            {
                _maxQclose = value;
            }
        }
        private float _minQopen;
        [XmlIgnore]
        public float MinQOpen
        {
            get { return _minQopen; }
            set
            {
                _minQopen = value;
            }
        }
        private float _maxclose;
        [XmlIgnore]
        public float MaxClose
        {
            get { return _maxclose; }
            set
            {
                _maxclose = value;
            }
        }
        private float _minopen;
        [XmlIgnore]
        public float MinOpen
        {
            get { return _minopen; }
            set
            {
                _minopen = value;
            }
        }
        private int _secondwait;
        [XmlIgnore]
        public int SecondWait
        {
            get { return _secondwait; }
            set
            {
                _secondwait = value;
            }
        }
        private int _port;
        public int Port
        {
            get { return _port; }
            set 
            {
                _port = value;                
            }
        }
        private int _baudRate;
        public int BaudRate
        {
            get { return _baudRate; }
            set
            {
                if (_baudRate != value)
                {
                    _baudRate = value;
                    Notify("BaudRate");
                }
            }
        }
        private string _deviceAddress;
        public string DeviceAddress
        {
            get { return _deviceAddress; }
            set
            {
                if (_deviceAddress != value)
                {
                    _deviceAddress = value;
                    Notify("DeviceAddress");
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                Notify("Password");
            }
        }
        protected string _serveraddress;

        public string Serveraddress
        {
            get { return _serveraddress; }
            set
            {
                if (_serveraddress != value)
                {
                    _serveraddress = value;


                }
            }
        }
        protected bool _client;

        
        public bool Client
        {
            get { return _client; }
            set
            {
                if (_client != value)
                {
                    _client = value;

                    
                }
                if (_client == true)
                {
                    _listener = new TcpClient(this, _serveraddress, _port,_buffersize);
                }
                else
                {
                    _listener = new TcpServer(this,_buffersize);
                }
                _listener.OnStatusChanged += new EventHandler<StatusChangedEventArgs>(listener_OnStatusChanged);
                timerBetweenEachRequest.Elapsed += new ElapsedEventHandler(OnTimedBetweenEachRequestEvent);
                timercheckalert.Elapsed += new ElapsedEventHandler(OnTimedcheckalert);
                timerBetweenEachPoll.Elapsed += new ElapsedEventHandler(OnTimedBetweenEachPollEvent);
                timerSaveHistory.Elapsed += new ElapsedEventHandler(OnTimedSaveHistory);
                timerSuspend.Elapsed += new ElapsedEventHandler(OnTimedSuspend);
            }
        }
        
        private int _latencybetweenrequest; //parameter in milisecond
        
        public int Latencybetweenrequest
        {
            get { return _latencybetweenrequest; }
            set
            {
                if (value <= 0)
                {
                    return;
                }
               
               _latencybetweenrequest = value;
               
                Notify("Latencybetweenrequest");
            }
        }
        private int _latencySaveHistory;
        public int LatencySaveHistory
        {
            get { return _latencySaveHistory; }
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _latencySaveHistory = value;

               Notify("LatencySaveHistory");
            }
        }
        private int _latencybetweenpoll; //parameter in milisecond
        public int Latencybetweenpoll
        {
            get { return _latencybetweenpoll; }
            set
            {
                if (value <= 0)
                {
                    return;
                }
                _latencybetweenpoll = value;
                Notify("Latencybetweenpoll");
            }
        }
        
        public virtual eDeviceType DeviceType
        {
            get { return eDeviceType.CooperFxb; }
        }

        private int[] _tubuopen;
        [XmlIgnore]
        public int[] TuBuOpenpub
        {
            get
            {
                return _tubuopen;
            }
        }
        private int _amppercent;
        public int AmpPercent
        {
            get { return _amppercent; }
            set
            {
                
                if (_amppercent != value)
                {
                    _amppercent = value;
                }
            }
        }
        [XmlIgnore]
        public string TuBuOpen
        {
            get {
                if (_tubuopen == null)
                    return "";
                StringBuilder builder = new StringBuilder();
                bool first = true;
                foreach (int id in _tubuopen)
                {
                    if (first == true)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(',');
                    }
                    builder.Append(id.ToString());
                    
                }
                return builder.ToString();
            }
            set
            {
                string[] str = value.Split(',');
                _tubuopen = new int[str.Count()];
                int i = 0;
                foreach (string strvalue in str)
                {
                    try
                    {
                        _tubuopen[i] = Convert.ToInt32(strvalue);
                        i++;
                    }
                    catch
                    {
                    }
                }
                Notify("TuBuOpen");
            }
        }
        private int[] _tubuclose;
        [XmlIgnore]
        public int[] TuBuClosepub
        {
            get
            {
                return _tubuopen;
            }
        }
        
        [XmlIgnore]
        public string TuBuClose
        {
            get {
                if (_tubuclose == null)
                    return "";
                StringBuilder builder = new StringBuilder();
                bool first = true;
                foreach (int id in _tubuclose)
                {
                    if (first == true)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(',');
                    }
                    builder.Append(id.ToString());
                    
                }
                return builder.ToString();
            }
            set
            {
                string[] str = value.Split(',');
                _tubuclose = new int[str.Count()];
                int i = 0;
                foreach (string strvalue in str)
                {
                    try
                    {
                        _tubuclose[i] = Convert.ToInt32(strvalue);
                        i++;
                    }
                    catch(Exception )
                    {
                    }
                }
                Notify("TuBuClose");
            }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                Notify("Name");
            }
        }
        private int _groupid;
        public int GroupId
        {
            get { return _groupid; }
            set
            {
                _groupid = value;
            }
        }
        private string _groupname;
        public string GroupName
        {
            get { return _groupname; }
            set
            {
                _groupname = value;
            }
        }
        private string _location;
        public string Location
        {
            get { return _location; }
            set 
            { 
                _location = value;
                //Notify("Location");
            }
        }

        private string _alertFile;
        [XmlIgnore]
        public string AlertFile
        {
            get { return _alertFile; }
            set 
            { 
                _alertFile = value;
                //Notify("AlertFile");
            }
        }

        private DateTime _lastUpdated = DateTime.MinValue;
        [XmlIgnore]
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set 
            { 
                _lastUpdated = value;
                Notify("LastUpdated", false);
            }
        }

        private bool _enable;
        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                //Notify("Enable");
            }
        }

        private string _commStatus;
        [XmlIgnore]
        public string CommStatus 
        {
            get { return _commStatus; }
            set
            {
                if (_commStatus != value)
                {
                    _commStatus = value;
                    Notify("CommStatus", false);
                }
            }
        }
        #endregion

        protected List<Request> _request;
        protected int _iRequest;
        protected bool _isDirty;
        [XmlIgnore]
        public bool IsDirty
        {
            get { return _isDirty; }            
        }

        public void AcceptChanges()
        {
            _isDirty = false;
        }
        private TcpBase _listener;
        [XmlIgnore]
        public TcpBase Listener
        {
            get
            {
                if (_listener == null)
                {
                    this.Client = false; //set default mode as Server to init Listener
                }
                return _listener;
            }
        }
        //private TcpServer _listener;
        //[XmlIgnore]
        //public TcpServer Listener
        //{
        //    get { return _listener; }
        //}

        public RecloserBase():this(0)
        { 
        }
        protected int _buffersize;
        public RecloserBase(int port)
        {
            _port = port;
            
            _isDirty = false;            
            _enable = true;
            _request = GetRequest();
            _iRequest = 0;
            this.AlertSoundStatus = eAlertSoundStatus.None;

            this.timerSuspend.Interval = 5000; //suspend 5s
            
            //GC.KeepAlive(timer1);
        }
        private string _version;
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
        
        private void OnTimedBetweenEachPollEvent(object source, ElapsedEventArgs e)
        {
            FirePoll();
            
        }
        protected void FirePoll()
        {
            try
            {
                bDataReturned = true;
                if (_listener.IsConnected)
                {
                    StartSendingRequests();
                }
                else
                {
                    
                    Listener.StartListening();
                    Listener.ReceiveBuffer.Reset();
                    //rec.StartPoll();
                }
            }
            catch(Exception ) {
                //FA_Accounting.Common.LogService.WriteError("FirePoll", ex.ToString());
            }
        }
        private void OnTimedSaveHistory(object source, ElapsedEventArgs e)
        {
            try
            {
                SaveData();
            }
            catch { }
        }
        /// <summary>
        /// Execute when suspending time is over
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedSuspend(object source, ElapsedEventArgs e)
        {
            Start();
            timerSuspend.Enabled = false;
            
        }
        
       
        /// <summary>
        /// Stop listening socket , stop all timer 
        /// </summary>
        protected void Stop()
        {
            try
            {
                acceptSendingRequest = false;
                
                StopPolling();
                Listener.StopListening();
                Listener.ReceiveBuffer.Flush();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Recloserbase Stop", ex.ToString());
            }
        }
        /// <summary>
        /// Start listening socket , start all timer 
        /// </summary>
        public void Start()
        {
            try
            {
                Listener.StartListening();
                Listener.ReceiveBuffer.Reset();
                StartPoll();
            }
            catch (Exception ex)
            {
                FA_Accounting.Common.LogService.WriteError("Recloserbase Start", ex.ToString());
            }
        }
        protected virtual void SaveData()
        {
            try
            {
                if (this.Listener.IsConnected)
                {
                    if (this.LastUpdated != DateTime.MinValue)
                    {
                        DBController.Instance.SaveRecloser(this);
                    }
                    
                }
                else
                {
                    DBController.Instance.SaveRecloserDisCon(this);
                }
            }
            catch(Exception ex) {
                FA_Accounting.Common.LogService.WriteError("SaveData", ex.ToString());
            }
        }
        public void StartPollforAlert()
        {
            timercheckalert.Interval = 5000;
            timercheckalert.Enabled = true;
            this.Listener.Alertprogram = true;
                 
        }
        bool firsttimepolling = true;
        public virtual void StartPoll()
        {
            //when initially start timerBetweenEachPoll= 2s
            /*if (!this.Listener.success)
            {
                return;
            }*/
            
            timerBetweenEachPoll.Interval = 9000;
            
            /*if (this.Latencybetweenpoll == 0)
            {
                timerBetweenEachPoll.Interval = 100;
            }
            else
            {
                timerBetweenEachPoll.Interval = this.Latencybetweenpoll;
            }*/

            timerBetweenEachRequest.Interval = this.Latencybetweenrequest;
            timerSaveHistory.Interval = this.LatencySaveHistory == 0 ? 600000 : this.LatencySaveHistory;
            timerSaveHistory.Enabled = true;
            timerBetweenEachRequest.Enabled = false;
            timerBetweenEachRequest.Stop();
            //Thread athread = new Thread(SendingRequestIfConnected);
            //athread.Start();

            bDataReturned = true;
            timerBetweenEachPoll.Enabled = true;
            timerBetweenEachPoll.Start();
            
            //test();
        }
        /// <summary>
        /// first called when OnTimedBetweenEachPollEvent occurs
        /// </summary>
        protected virtual void StartSendingRequests()
        {
            this.pollingcount++;
            
            if (pollingcount % 3 == 0)
            {
                if (!this.CheckConnectionStatus())
                {
                    return;
                }
            }
            // each subclass will have override StartSendingRequest
            // at first they do some connection request, password etc before sending the first request
            timerBetweenEachRequest.Enabled = true;
            timerBetweenEachRequest.Start();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();
            
            _iRequest = 0;
        }
        protected void test()
        {
            var item = _request[_iRequest];
            _iRequest++;
            if (item.Used)
            {
                this.Listener.Send(item.Getbytes());
                // recheck Di
                //System.Threading.Thread.Sleep(2000); // thu *1000
            }
        }
        public bool shutdownalert = false;
        private void OnTimedcheckalert(object source, ElapsedEventArgs e)
        {
            if (shutdownalert == true)
            {
                this.timercheckalert.Enabled = false;
                return;
            }
            checkspecialalert();
        }
        protected void checkspecialalert()
        {
            if (!_listener.IsConnected)
            {
                Listener.StartListening();
                Listener.ReceiveBuffer.Reset();
                
            }
            if (handshaked == false)
            {
                try
                {
                    timercheckalert.Enabled = false;
                    SendHandshake();
                    handshaked = true;
                    timercheckalert.Enabled = true; 
                }
                catch(Exception ex){
                    LogService.WriteError("Send Handshake", ex.ToString());
                }
            }
            CheckingInsAlert();
        }
        protected bool handshaked = false;
        public bool Alertprogram = false;
        protected virtual void SendHandshake()
        {
        }
        protected virtual void CheckingInsAlert()
        {
        }
        protected Request currentRequest = new Request();
        protected bool bDataReturned = true;
        int countRequestEvent = 0;
        private void OnTimedBetweenEachRequestEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                if (_request != null && _request.Count > 0)
                {
                    if (_iRequest >= _request.Count)
                    {
                        WaitForNextPoll();
                        return;
                    }
                    try
                    {
                        if (firsttimepolling && !string.IsNullOrEmpty(this.StartPackage))
                        {
                            this.Listener.Send(TcpComm.Ultility.FromHex(this.StartPackage));
                            firsttimepolling = false;
                            //System.Threading.Thread.Sleep(latency);
                        }
                        else
                        {
                            if (!bDataReturned) // don't send a new request until dataupdate return value from previous request
                            {
                                countRequestEvent++;
                                if (countRequestEvent > _request.Count +1)
                                {
                                    countRequestEvent = 0;
                                    WaitForNextPoll();
                                }
                                return;
                            }
                            if (_iRequest >= _request.Count)
                            {
                                WaitForNextPoll();
                                return;
                            }
                            currentRequest = _request[_iRequest];
                            _iRequest++;
                            //if (item.Used)
                            //{
                            this.Listener.Send(currentRequest.Getbytes());
                            bDataReturned = false;
                            // recheck Di
                            //System.Threading.Thread.Sleep(2000); // thu *1000
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        LogService.Logger.Error(string.Format("Recloser {0}, port {1} Poll error", this.DeviceType, this.Port), ex);
                    }

                }
            }
            catch { }
        }
        protected void WaitForNextPoll()
        {
            _iRequest = 0;
            timerBetweenEachRequest.Enabled = false;
            timerBetweenEachRequest.Stop();
            if (firsttimepolling == true)
            {
                if (this.Latencybetweenpoll == 0)
                {
                    timerBetweenEachPoll.Interval = 10000;
                }
                else
                {
                    timerBetweenEachPoll.Interval = this.Latencybetweenpoll;
                }
                firsttimepolling = false;
            }
            timerBetweenEachPoll.Enabled = true;
            
            timerBetweenEachPoll.Start();

        }
        public void StopPolling()
        {
            _iRequest = 0;
            firsttimepolling = true;
            timerBetweenEachRequest.Enabled = false;
            
            timerBetweenEachRequest.Stop();
            timerBetweenEachPoll.Enabled = false;
            timerBetweenEachPoll.Stop();

            timerSaveHistory.Enabled = false;
            timerSaveHistory.Stop();
        }
        private void listener_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.CommStatus = e.Status;
        }

        public virtual void UpdateData(byte[] data)
        {
            if (this.OnDataUpdated != null && data != null)
                this.OnDataUpdated(this, EventArgs.Empty);
        }
        public virtual byte[] DoParse(RingBuffer ringBuffer)
        {
            return null;
        }
        public virtual void CommandClose(bool auto)
        {
            FA_Accounting.Common.LogService.WriteError("CommandClose arg auto called to id: " + this.Id.ToString() + " name: " + this.Name + " device: " + this.DeviceType + " auto:" + auto, "");
        }
        public virtual void CommandClose()
        {
            FA_Accounting.Common.LogService.WriteError("CommandClose called to id: " + this.Id.ToString() + " name: " + this.Name + " device: " + this.DeviceType,"");
        }
        public virtual void CommandOpen(bool auto)
        {
            FA_Accounting.Common.LogService.WriteError("CommandOpen arg auto called to id: " + this.Id.ToString() + " name: " + this.Name + " device: " + this.DeviceType + " auto:" + auto, "");
        }
        public virtual void CommandOpen()
        {
            FA_Accounting.Common.LogService.WriteError("CommandOpen called to id: " + this.Id.ToString() + " name: " + this.Name + " device: " + this.DeviceType , "");
        }
        public virtual void CommandReclose()
        {
            FA_Accounting.Common.LogService.WriteError("CommandReclose called to id: " + this.Id.ToString() + " name: " + this.Name + " device: " + this.DeviceType, "");
        }
        public virtual void SetTime()
        {
        }
        public virtual void SetTimePulse(ushort timepulse, byte relay)
        { 
        }
        public virtual void CheckAlert()
        {
            if (!this.Alert)
                AlertSoundStatus = eAlertSoundStatus.None;
        }
        public virtual eAlertOpenClose CheckCosphi()
        {
            return eAlertOpenClose.None;
        }
        protected virtual List<Request> GetRequest()
        {
            return RequestManager.Instance.RequestList.Where(r => r.DeviceType == this.DeviceType && r.Used).ToList();
        }
    
        //public virtual void Pollold(int latency = 10)
        //{
        //    if (_request != null && _request.Count > 0)
        //    {
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(this.StartPackage))
        //            {
        //                var s = this.StartPackage.Trim();
        //                if (s != string.Empty)
        //                {
        //                    this.Listener.Send(Ultility.FromHex(s));
        //                    System.Threading.Thread.Sleep(latency);
        //                }
        //            }

        //            foreach (var item in _request)
        //            {
        //                if (item.Used)
        //                {
        //                    this.Listener.Send(item.Getbytes());
        //                    System.Threading.Thread.Sleep(2000); // thu *1000
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogService.Logger.Error(string.Format("Recloser {0}, port {1} Poll error", this.DeviceType, this.Port), ex);
        //        }
        //    }
        //}
        public virtual void Poll(int latency = 10)
        {
            if (_latencybetweenrequest == 0)
            {
                this.timerBetweenEachRequest.Interval = latency ;
            }
            else
            {
                this.timerBetweenEachRequest.Interval = _latencybetweenrequest ;
            }
            StartSendingRequests();
            
        }
        [XmlIgnore]
        public virtual int AlertVal
        {
            get { return _alertVal; }
        }
        [XmlIgnore]
        public virtual bool Alert
        {
            get { return _alertVal != 0; }
        }
        
        public virtual void resetAlert()
        {
            _alertVal = 0;
            _startZeroA = null;
            _startZeroB = null;
            _startZeroC = null;
        }

        private eAlertSoundStatus _alertSoundStatus;
        [XmlIgnore]
        public eAlertSoundStatus AlertSoundStatus
        {
            get { return _alertSoundStatus; }
            set
            {
                if (_alertSoundStatus != value)
                {
                    _alertSoundStatus = value;
                    Notify("AlertSoundStatus", false); 
                }
            }
        }
        
        //public virtual bool IsAlert(int amp, DateTime? start)
        //{
        //    if (amp == 0 && start.HasValue && (DateTime.Now - start.Value).TotalSeconds > MaxZeroDuration)
        //    {
        //        _alertVal = 1;
        //        return true;
        //    }
        //    if (amp > MaxAmp){
        //        _alertVal = 2;
        //        return true;
        //    }
        //    return false;
        //}
        public void settestzero()
        {
            Testing = 1; //(Testing == 1 ? 0 : 1);
            Amp12 = 0;// (Testing == 1 ? 0 : 5);
        }
        public void settestmax()
        {

            Testing = 2;// (Testing == 2 ? 0 : 2);
            Amp12 = MaxAmp + 1;// (Testing == 2 ? MaxAmp + 1 : 5);
        }
        protected string _devicepath = string.Empty;
        public string DevicePath
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                if (value.LastIndexOf('\\') != value.Length - 1)
                {
                    
                    _devicepath = value + "\\";
                }
                else
                {
                    _devicepath = value;
                }
                
            }
        }
            
        public virtual void WriteRow()
        {
        }

        private string _startPackage;

        public string StartPackage
        {
            get { return _startPackage; }
            set 
            { 
                _startPackage = value;
                Notify("StartPackage");
            }
        }
        
        public void resetModem()
        {
            
                string ip = "10.175.8" + this.Port.ToString().Substring(0, 1) + "." + this.Port.ToString().Substring(1, 3);
                string url = "http://" + ip + "/cgi/login.cgi?Username=admin&Password=system";

                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(url);

                request.GetResponse();

                //http://10.175.88.116/cgi/login.cgi?Username=admin&Password=system
                url = "http://" + ip + "/cgi/reset.cgi?back=Reset&reset=true";
                request = System.Net.HttpWebRequest.Create(url);

                request.GetResponse();
                //http://10.175.88.101/cgi/reset.cgi?back=Reset&reset=ture

                //http://10.175.88.101/cgi/uart.cgi?mode=0&set_baudrate=4800&set_cb=8&set_pt=0&set_sb=0&set_hf=0&D1V=00&D2V=FF&D3V=5&Modify=Update
            

        }
        protected void sleep(double second)
        {
            DateTime current = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - current).TotalMilliseconds >= (1000 * second))
                    break;
            }

        }
        public void SaveDevice()
        {
            DBController.Instance.SaveDevice(this);
        }
        public virtual void SendAmpRequest()
        {
        }
        /// <summary>
        /// Check if long time (2*Poll interval) has no update date
        /// then stop socket, timers and start suspending 
        /// </summary>
        protected virtual bool CheckConnectionStatus()
        {

            if ((DateTime.Now - this.LastUpdated).TotalMilliseconds >= (timerBetweenEachPoll.Interval < 120000 ? 120000 : timerBetweenEachPoll.Interval))//&& this.LastUpdated != DateTime.MinValue) 
            {
                this.timerSuspend.Enabled = true;
                pollingcount = 0;
                this.Stop();
                FA_Accounting.Common.LogService.WriteInfo("Reset connection " + this.Name + " at " + this.Location + " port:" + this.Port.ToString(), "");
                return false;
            }
            return true;
        }
    }

}
