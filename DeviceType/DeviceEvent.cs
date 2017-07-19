using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecloserAcq.OracleDAL;
namespace RecloserAcq.Device
{
    public class DeviceEvent
    {
        private int _id;



        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }// Id of the event
        private int _deviceId;
        public int DeviceId
        {
            
            get { return _deviceId; }
            set { _deviceId = value; }
        }
        private string _deviceName;
        public string DeviceName
        {
            
            get { return _deviceName; }
            set { _deviceName = value; }
        }
        private string _command;
        public string Command
        {
            // two values: "Dong" hoac "Mo" 
            get { return _command; }
            set { _command = value; }
        }
        private string _type;
        public string Type
        {
            //"onetime", "daily", "weekly"
            get { return _type; }
            set { _type = value; }
        }
        private string _weekday ;
        public string Weekday{
            get{
                return _weekday;
            }
            set
            {
                _weekday = value;
                if (!String.IsNullOrEmpty(value))
                {
                    string[] strs = value.Split(',');

                    wdays = new DayOfWeek[strs.Count()];
                    for (int i = 0; i < strs.Count(); i++)
                    {
                        wdays[i] = ConvertDay(strs[i]);
                    }
                }
            }
        } // value: eg. "0,1,4,6" ="CN,Monday,Wednesday,Saturday"  // use. DateTime.DayofWeek
        private DayOfWeek[] wdays;
        private DateTime _dtactive;
        public DateTime DtActive
        {
            get { return _dtactive; }
            set { _dtactive = value; }
        }
        private DateTime _dtExpire;
        public DateTime DtExpire
        {
            get { return _dtExpire; }
            set { _dtExpire = value; }
        }
        private DateTime _dtNextRun;
        public DateTime DtNextRun
        {
            get { return _dtNextRun; }
            set { _dtNextRun = value; }
        }
        private DateTime _dtPrevRun;
        public DateTime DtPrevRun
        {
            get { return _dtPrevRun; }
            set { _dtPrevRun = value; }
        }
        private string _nameofevent;
        public string NameOfEvent
        {
            get { return _nameofevent; }
            set { _nameofevent = value; }
        }
        private int _hourrepeat;
        public int hourRepeat
        {
            get { return _hourrepeat; }
            set { _hourrepeat = value; }
        }
        public DeviceEvent()
        {
        }
        private DayOfWeek ConvertDay(string strday)
        {
            switch (strday)
            {
                case "Sunday":
                    return DayOfWeek.Sunday;
                case "Monday":
                    return DayOfWeek.Monday;
                case "Tuesday":
                    return DayOfWeek.Tuesday;
                case "Wednesday":
                    return DayOfWeek.Wednesday;
                case "Thursday":
                    return DayOfWeek.Thursday;
                case "Friday":
                    return DayOfWeek.Friday;
                case "Saturday":
                    return DayOfWeek.Saturday;
                default:
                    throw new Exception("day is invalid");
            }
            
        }
        public void InsertNew()
        {
            DBController.Instance.SaveSchedule(this);
        }
        public void UpdateSchedule()
        {
            DBController.Instance.UpdateSchedule(this);
        }
        /// <summary>
        /// Call this function only right after event is run
        /// NextRun is also the date this function is called
        /// </summary>
        public void ResetNextRun()
        {
            if (Type == "onetime")
            {
                if (hourRepeat > 0)
                {
                    DtNextRun = DtNextRun.AddHours(hourRepeat);
                }
                else
                {
                    DtNextRun = DateTime.MaxValue;
                }
            }
            else if (Type == "daily")
            {
                DtNextRun = DtNextRun.AddDays(1);
            }
            else if (Type == "weekly")
            {
                //string[] wdays = this.Weekday.Split(',');
                DayOfWeek today = DateTime.Now.DayOfWeek;
                DayOfWeek nextday;
                for(int i =0;i<wdays.Count();i++)
                {
                    if (wdays[i] == today && i < wdays.Count()-1)
                    {
                        nextday = wdays[i + 1];
                        DtNextRun = DtNextRun.AddDays(nextday - today);
                        break;
                    }
                    else if (i == wdays.Count()-1)
                    {
                        nextday = wdays[0];
                        DtNextRun = DtNextRun.AddDays(7 - (int)today + (int)nextday);
                    }
                }
                
                
                //1. still at least one run time in the week
                // 2. next run time is next week
            }
            else
            {
                throw new Exception("Type is not valid");
            }
            //save to database when NextRun is changed
            this.UpdateSchedule();
        }
        /// <summary>
        ///  for type = weekly
        /// </summary>
        public void SetFirstRun(DateTime dtprActive)
        {
            if (this.Type != "weekly")
            {
                return;
            }
            DayOfWeek atvday = dtprActive.DayOfWeek;
            DayOfWeek nextday;
            for (int i = 0; i < wdays.Count(); i++)
            {
                if (wdays[i] == atvday )
                {
                    
                    DtNextRun = dtprActive;
                    break;
                }
                else if (wdays[i] > atvday)
                {
                    nextday = wdays[i];
                    DtNextRun = dtprActive.AddDays(nextday - atvday);
                    break;
                }
                else if (i == wdays.Count() - 1)
                {
                    nextday = wdays[0];
                    DtNextRun = dtprActive.AddDays(7 - (int)atvday + (int)nextday);
                }
            }
        }
    }
}
