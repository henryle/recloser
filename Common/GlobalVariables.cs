using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FA_Accounting.Common
{
    public static class GlobalVariables
    {
        #region other constants        
        public const string Separator = "##";
        public static DateTime dtstartusing = new DateTime(2013,1,1);
        //public const string UploadFileExt = ".txt";
        public static Encoding SynEncoding = Encoding.UTF8 ;
        public const string BackupFolder = "Backup";
        public const string retryfile = "retry.txt"; //also log for failed file
        public const string configfile = "FtpBT.config";
        public const string sqlDateFormat = "yyyy/MM/dd";
        public const string DateFormat = "yyyyMMdd";
        
        public const string PSW = "pass";
        public const string USER = "user";
        public const string HOST = "host";
        public const string INTERVAL = "interval";
        public const string DELETEDAYS = "deletedays";
        public const string PATH = "path";
        public const string FUNCTION = "function";
        public const string LOCATION = "location";
        public const string UPLOADFROM = "uploadfrom";
        public const string ZIPPassword = "Goodman99";
        public const string ACCESSPSW = "Matkhau";

        public const string sqlDateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public const string LocalUploadFolder = "UPLOAD";
        public const string LocalRetryFolder = "Retry";
        public const string LocalDownloadFolder = "DOWNLOAD";
        public static string LocalUploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LocalUploadFolder);
        public static string LocalRetryPath = Path.Combine(LocalUploadPath, LocalRetryFolder);
        public static string LocalUploadBackupPath = Path.Combine(LocalUploadPath, "Uploaded");
        public static string LocalDownloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LocalDownloadFolder);
        public static string LocalDownloadBackupPath = Path.Combine(LocalDownloadPath, "Downloaded"); 
        #endregion
        
     

       

    }
}
