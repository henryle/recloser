using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;

namespace RecloserAcq
{
    public static class LogService
    {
        #region Members
        private static readonly ILog logger = LogManager.GetLogger(typeof(LogService));

        public static ILog Logger
        {
            get { return logger; }
        }

        #endregion

        static string _logPathFile = string.Empty;
        public static string LogFile
        {
            get { return _logPathFile.Substring(0, _logPathFile.LastIndexOf(@"\") + 1) + DateTime.Now.ToString("yyyyMMdd"); }
            set { _logPathFile = value; }
        }

        #region Constructors
        static LogService()
        {
            LoadLogService(@"Log\");
        }
        public static void LogServiceConfig(string logPath)
        {
            if (logPath != string.Empty)
            {
                if (logPath.EndsWith(@"\") == false) logPath = logPath + @"\";
                LoadLogService(logPath);
            }
        }

        private static void LoadLogService(string logPath)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders(); /*Remove any other appenders*/

            RollingFileAppender fileAppender = new RollingFileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.StaticLogFileName = false;
            fileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            fileAppender.DatePattern = "yyyyMMdd";
            fileAppender.File = logPath;
            PatternLayout pl = new PatternLayout();
            pl.ConversionPattern = @"%date [%thread] %-5level %logger %ndc - %message%newline"; // "%d [%2%t] %-5p [%-10c] %m%n%n";
            pl.ActivateOptions();
            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(fileAppender);

            _logPathFile = fileAppender.File;
        }

        #endregion


        public static void WriteInfo(string formName, string msg)
        {
            if (logger != null)
                logger.Debug(string.Format("{0} - {1}", formName, msg));
        }

        public static void WriteError(string formName, string msg, Exception ex = null)
        {
            if (logger != null)
            {
                var log = string.Format("{0} - {1}", formName, msg);
                if (ex == null)
                    logger.Error(log);
                else
                    logger.Error(log, ex);
            }
        }

    }
}
