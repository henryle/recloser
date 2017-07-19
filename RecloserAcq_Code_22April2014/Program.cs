using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using RecloserAcq.Device;
using System.Diagnostics;

namespace RecloserAcq
{
    static class Program
    {
        public static UserObj user;
        public static string DienLuc = "bentre";
        //public const string DeviceFile = "DeviceFile.xml";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool autoStartPoll = false;
            if (args.Count() > 0)
            {
                autoStartPoll = args[0] == "AutoStart";
            }
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (login() == false)
               return;
            //Application.Run(new frmDeviceStatus() { AutoStartPoll = autoStartPoll });
            Application.Run(new mainForm());
            //Application.Run(new Form1());
        }
        static bool login()
        {
            bool retry = true;
            while (retry)
            {
                try
                {
                    //UserLogin ulogin = new UserLogin(RecloserAcq.Properties.Settings.Default.SQLConnection);
                    UserLogin ulogin = new UserLogin();
                    Program.user = new UserObj();
                    if (ulogin.ShowDialog() == DialogResult.Cancel)
                    {
                        
                        return false;
                    }
                    else
                    {
                        
                        //this.userlgin = new UserObj();
                        //this.user = ulogin.user;
                        Program.user = ulogin.user;
                        //DatabaseSetting.Instance.UserId = ulogin.UserId;
                        return true;
                    }
                }
                catch (Exception )
                {
                    if (MessageBox.Show("Log in failed, vui lòng liên hệ IT", "Recloser", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                    {

                        retry = false;
                        return false;
                    }

                }
            }
            return false;
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            RecloserAcq.LogService.Logger.Error("Application error", e.Exception);
        }

        

        public static void Restart()
        {
            var appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecloserAcq.exe");
            Process.Start(appPath, "AutoStart");
        }
        //public static List<RecloserBase> GetDevices(string devicefile)
        //{
        //    List<RecloserBase> list = null;
        //    if (File.Exists(devicefile))
        //    {
        //        XmlSerializer ser = new XmlSerializer(typeof(List<RecloserBase>));
        //        using (var stream = File.OpenRead(devicefile))
        //        {
        //            var devices = ser.Deserialize(stream) as List<RecloserBase>;
        //            list = devices;
        //        }
        //    }
        //    else
        //    {
        //        list = new List<RecloserBase>();
        //        list.Add(new CooperFXB(7000) { Name = "Cooper 1" });
        //        list.Add(new Nulec(7001) { Name = "Nulec 1" });
        //    }

        //    return list;
        //}
        //const string RequestFileName = "Request.xml";
        //private static List<Request> _requestList;
        //public static List<Request> RequestList
        //{
        //    get
        //    {
        //        if (_requestList == null)
        //            LoadRequest();

        //        return _requestList;
        //    }
        //}

        //public static void LoadRequest()
        //{
        //    try
        //    {
        //        XmlSerializer ser = new XmlSerializer(typeof(List<Request>));
        //        using (var stream = File.OpenRead(RequestFileName))
        //        {
        //            _requestList = ser.Deserialize(stream) as List<Request>;
        //        }
        //    }
        //    catch
        //    {
        //        _requestList = new List<Request>();
        //    }
        //}

        //public static void SaveRequest()
        //{
        //    try
        //    {
        //        XmlSerializer ser = new XmlSerializer(typeof(List<Request>));
        //        using (var stream = File.CreateText(RequestFileName))
        //        {
        //            ser.Serialize(stream, _requestList);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

    }
}
