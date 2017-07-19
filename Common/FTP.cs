using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using FA_Accounting.Common;

namespace FA_Accounting.Common
{
    public static class FTP 
    {
        public static string ServerIP { get; set; }
        public static string UserID { get; set; }
        public static string Password { get; set; }
        private static bool initialized = false;
        public static string secret = "Goodman99";
        public static void Init(string serverIP, string userID, string password)
        {
            ServerIP = serverIP;
            UserID = userID;
            Password = password;
            initialized = true;
        }
        
        public static void Init()
        {
            string serverIP = string.Empty; 
            string userID = string.Empty;
            string password = string.Empty;
            //SqlConnectionObject.GetFTPSetting(ref serverIP, ref userID, ref password);
            
            ServerIP = Config.Instance.ftpserver;
            UserID = Config.Instance.ftpuser;
            Password = Encrypt.DecryptString(Config.Instance.ftppassword,FTP.secret);
            
            if (string.IsNullOrEmpty(ServerIP))
            {
                LogService.WriteError("class SynATC init", " FtpIp is not setup");
            }
            initialized = true;
        }

        public static bool CheckConnection()
        {
            string uriFile = "ftp://" + ServerIP + "/";
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(uriFile);
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.KeepAlive = false;
                //reqFTP.UsePassive = true;
                reqFTP.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
                WebResponse response = reqFTP.GetResponse();
            }
            catch (Exception ex)
            {
                LogService.WriteError("CheckConnection failed", uriFile, ex);
                return false;
            }
            return true;
        }
        public static bool CheckConnection(ref string result)
        {
            string uriFile = "ftp://" + ServerIP + "/";
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(uriFile);
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
                WebResponse response = reqFTP.GetResponse();
            }
            catch (Exception ex)
            {
                LogService.WriteError("CheckConnection failed", uriFile, ex);
                LogService.WriteInfo("Test form Name", "test message");
                result = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// Method to upload the specified file to the root of FTP Server
        /// </summary>
        /// <param name="fileName">file full name to be uploaded</param>
        public static bool Upload(string fileName)
        {
            return Upload(fileName, "",DateTime.Today.ToString(GlobalVariables.DateFormat));
        }

        /// <summary>
        /// Method to upload the specified file to the specified FTP Server
        /// </summary>
        /// <param name="ftpFolder">ftp folder to be uploaded</param>
        /// <param name="filePath">file full name to be uploaded</param>
        public static bool Upload(string filePath, string ftpFolder, string CreatedDate)
        {
            string uriFile = string.Empty;
            if (!initialized)
            {
                Init();
            }
            try
            {
                if (!ExistsDir(ftpFolder))
                {
                    MakeDir(ftpFolder);
                }
                string filename = Path.GetFileName(filePath); //filePath.Substring(filePath.LastIndexOf('\\') + 1);
                //string tableDir = filename.Substring(0,filename.IndexOf('_'));
                if (!ExistsDir(ftpFolder + "/" + CreatedDate))
                {
                    MakeDir(ftpFolder + "/" + CreatedDate);
                }
                FileInfo fileInf = new FileInfo(filePath);
                uriFile = "ftp://" + ServerIP + "/" + (ftpFolder == string.Empty ? "" : ftpFolder + "/") + (CreatedDate == string.Empty ? "" : CreatedDate + "/") + fileInf.Name;
                FtpWebRequest reqFTP;

                // Create FtpWebRequest object from the Uri provided
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));

                // set timeout default is 100
                reqFTP.Timeout = DatabaseSetting.Instance.FTPRequestTimeout * 1000;

                // Provide the WebPermission Credintials
                reqFTP.Credentials = new NetworkCredential(UserID, Password);

                // By default KeepAlive is true, where the control connection is not closed after a command is executed.
                reqFTP.KeepAlive = false;

                // Specify the command to be executed.
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // Specify the data transfer type.
                reqFTP.UseBinary = true;

                // Notify the server about the size of the uploaded file
                reqFTP.ContentLength = fileInf.Length;

                // The buffer size is set to 2kb
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
                FileStream fs = fileInf.OpenRead();
            
                // Stream to which the file to be upload is written
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream
                strm.Close();
                fs.Close();
                //LogService.WriteInfo("FTP Upload success", uriFile);
            }
            catch (Exception ex)
            {
                LogService.WriteError("Upload", filePath, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method to download the specified file to the specified FTP Server
        /// </summary>
        /// <param name="filename">file full name to be downloaded</param>
        public static bool Download(string localFilePath, string fileName)
        {
            if (fileName != null)
                return Download(localFilePath, fileName);
            return true;
        }
        /// <summary>
        /// Method to download the specified file to the specified FTP Server
        /// </summary>
        /// <param name="filename">file full name to be downloaded</param>
        public static bool Download(string localFilePath, string fileName, string ftpFolder)
        {
            string uriFile = "ftp://" + ServerIP + "/" + (ftpFolder == string.Empty ? "" : ftpFolder + "/") + fileName;
            uriFile = uriFile.Replace("\\", "/");
            FtpWebRequest reqFTP;
            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
                FileStream outputStream = new FileStream(Path.Combine(localFilePath, fileName), FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));
                // set timeout default is 100
                reqFTP.Timeout = DatabaseSetting.Instance.FTPRequestTimeout * 1000;
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                LogService.WriteInfo("FTP Download success", uriFile);
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP Download failed", uriFile, ex);
                return false;
            }
            return true;
        }

        // Method to DeleteFile the root of FTP Server
        public static bool DeleteFile(string fileName)
        { 
            return DeleteFile(fileName, "");
        }

        // Method to DeleteFile specified folder FTP Server
        public static bool DeleteFile(string fileName, string ftpFolder)
        {
            string uriFile = "ftp://" + ServerIP + "/" + (ftpFolder == string.Empty ? "" : ftpFolder + "/") + fileName;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));

                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP DeleteFile failed", uriFile, ex);
                return false;
            }
            return true;
        }

        // Method to DeleteFile the root of FTP Server
        public static bool DeleteFolder(string folderName)
        {
            string uriFile = "ftp://" + ServerIP + "/" + folderName;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));

                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP DeleteFolder failed", uriFile, ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// return the files in detail like this: 
        /// "drwxr-xr-x 1 ftp ftp              0 Oct 17 22:51 ATC"
        /// "-rw-r--r-- 1 ftp ftp           1344 Sep 30 11:31 Card_pjtDCSync_30092011113133.txt"
        /// </summary>
        /// <returns></returns>
        public static string[] GetFilesDetailList()
        {
            string uriFile = "ftp://" + ServerIP + "/";
            string[] downloadFiles;
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                int i = result.ToString().LastIndexOf('\n');
                if (i >= 0)
                {
                    result.Remove(i, 1);
                    downloadFiles = result.ToString().Split('\n');
                }
                else
                {
                    downloadFiles = null;
                }
                reader.Close();
                response.Close();
                return downloadFiles;
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP GetFilesDetailList failed", uriFile, ex);
                downloadFiles = null;
                return downloadFiles;
            }
        }
        public static string[] GetFileList(string ftpFolder, string dirName)
        { 
            return GetFileList(ftpFolder + "/" + dirName);
        }

        /// <summary>
        /// get files , return null if no file
        /// </summary>
        /// <param name="ftpFolder"></param>
        /// <returns></returns>
        public static string[] GetFileList(string ftpFolder)
        {
            // dau co can tao folder cho nay 
            //CreateFolderIfExists(ftpFolder);

            string uriFile = "ftp://" + ServerIP + "/" + ftpFolder;
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                int i = result.ToString().LastIndexOf('\n');
                if (i >= 0)
                {
                    result.Remove(i, 1);
                    downloadFiles = result.ToString().Split('\n');
                }
                else
                {
                    downloadFiles = null;
                }
                reader.Close();
                response.Close();
                return downloadFiles;
            }
            catch 
            {
                //LogService.WriteError("FTP GetFileList failed", uriFile, ex);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        private static void CreateFolderIfExists(string ftpFolder)
        {
            try
            {
                string tmpFolder = string.Empty;
                string[] arrFolder = ftpFolder.Split(new char[1] { '/' });
                foreach (var folder in arrFolder)
                {
                    tmpFolder = (tmpFolder == string.Empty ? "" : tmpFolder + "/") + folder;
                    if (!ExistsDir(tmpFolder))
                    {
                        MakeDir(tmpFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP CreateFolderIfExists failed", ftpFolder, ex);
            }
        }

        public static long GetFileSize(string fileName)
        {
            string uriFile = "ftp://" + ServerIP + "/" + fileName;
            FtpWebRequest reqFTP;
            long fileSize = 0;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP GetFileSize failed", uriFile, ex);
            }
            return fileSize;
        }

        public static bool Rename(string fileName, string newFileName)
        { 
            return Rename(fileName, "", newFileName);
        }

        public static bool Rename(string fileName, string ftpFolder, string newFileName)
        {
            string uriFile = "ftp://" + ServerIP + "/" + (ftpFolder == string.Empty ? "" : ftpFolder + "/") + fileName;
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFileName;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP GetFileSize failed", uriFile, ex);
                return false;
            }
            return true;
        }
        public static bool ExistsDir(string dirName)
        {
            return ExistsDir(dirName, "");
        }

        public static bool ExistsDir(string dirName, string ftpFolder)
        {
            try
            {                
                string uriFile = "ftp://" + ServerIP + "/" + (ftpFolder == string.Empty ? "" : ftpFolder + "/") + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uriFile));
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory; 
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
                return true;
            } 
        }


        public static bool MakeDir(string dirName)
        {
            return MakeDir(dirName, "");
        }

        public static bool MakeDir(string dirName, string ftpFolder)
        {
            // dirName = name of the directory to create.
            string uriFile = "ftp://" + ServerIP + "/" + (ftpFolder == string.Empty ? "" : ftpFolder + "/") + dirName;
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uriFile));
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogService.WriteError("FTP MakeDir failed", uriFile, ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// location used to be a branch name, it is name of location here
        /// upload to location\filecreateddate 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="location"></param>
        /// <returns></returns>
       /* public static bool ZipAndUploadFtp(string filename, string location)
        {
            bool ret = false;
            DateTime fileCreatedDate = File.GetCreationTime(filename);
            string strfileCreatedDate = fileCreatedDate.ToString(GlobalVariables.DateFormat);
            string localBackupPath = Path.Combine(GlobalVariables.LocalUploadBackupPath, strfileCreatedDate);
            try
            {
                
                // zip file then upload to FTP
                string fileZip = GeneralUtils.ArchiveFile(filename, localBackupPath);
                if (fileZip != string.Empty)
                {
                    if (FTP.Upload(fileZip, location, strfileCreatedDate) == true)
                    {
                        //if (DatabaseSetting.Instance.DeleteFileAfterSyn == 1)
                        //    if (File.Exists(filename)) File.Delete(filename);
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("ZipAndUploadFTP", filename, ex);
            }
            return ret;
        }*/
        /// <summary>
        /// location used to be a branch name, it is name of location here
        /// upload to location\subfolder 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool ZipAndUploadFtp(string filename, string location, string subfolder)
        {
            bool ret = false;
            
            FileInfo fi = new FileInfo(filename);
            if (fi.Length <= 0)
            {
                return true;
            }
            DateTime fileCreatedDate = fi.LastWriteTime;
            //string strfileCreatedDate = fileCreatedDate.ToString(GlobalVariables.DateFormat);
            
            string localBackupPath = Path.Combine(GlobalVariables.LocalUploadBackupPath, subfolder);
            try
            {

                // zip file then upload to FTP
                //string fileZip = GeneralUtils.GetZipFilePath(filename);
                //string filebackup = GeneralUtils.GetZipFilePath(filename,localBackupPath);
                string fileZip = GeneralUtils.ArchiveFile(filename);
                if (fileZip==string.Empty)
                {
                    ret = false;
                }
                else if (fileZip == "Khongtimthayfile")
                {
                    ret = true;
                }
                else if (fileZip == "Khongzipduocfile")
                {
                    ret = true;
                }
                else
                {
                    if (FTP.Upload(fileZip, location, subfolder) == true)
                    {
                        //if (DatabaseSetting.Instance.DeleteFileAfterSyn == 1)
                        //    if (File.Exists(filename)) File.Delete(filename);
                        //File.Move(fileZip, filebackup);
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("ZipAndUploadFTP", filename, ex);
            }
            return ret;
        }
        public static List<string> UploadFolder(string folderpath, DateTime filetime)
        {
            List<string> failedfiles = new List<string>();
            

            DirectoryInfo di = new DirectoryInfo(folderpath);
            FileInfo[] rgFiles = di.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                DateTime fileCreatedDate = fi.LastWriteTime;
                if (fileCreatedDate > filetime)
                {
                    failedfiles.Add(fi.FullName);
                }
            }
            int i = 0;
            while (i < failedfiles.Count) 
            {
                try
                {
                    //file upload
                    bool success = true;
                    string subfolder;

                    //FTP.Upload(fi.FullName, txtLocation.Text, fileCreatedDate.ToString(GlobalVariables.DateFormat));
                    //if (fi.Directory.FullName != di.FullName)
                    FileInfo f = new FileInfo(failedfiles[i]);

                    if (f.Directory.FullName != di.FullName)//failedfiles[i].Substring(0,failedfiles[i].LastIndexOf('\\')) != di.FullName)
                    {
                        subfolder = f.Directory.Name;
                    }
                    else
                    {
                        subfolder = f.CreationTime.ToString(GlobalVariables.DateFormat);
                    }

                    //string localBackupPath = Path.Combine(GlobalVariables.LocalUploadBackupPath, subfolder);
                    //if file hasn't been uploaded
                    if (!File.Exists(GeneralUtils.GetZipFilePath(f.FullName)))
                    {
                        success = FTP.ZipAndUploadFtp(f.FullName, Config.Instance.location, subfolder);
                    }
                    //end file uploade
                    if (success == true)
                    {
                        //copy file to retry folder
                        //string retryfolder = Path.Combine(GlobalVariables.LocalUploadFolder, "Retry");
                        //string retrypath = Path.Combine(GlobalVariables.LocalRetryPath, subfolder);
                        //if (!Directory.Exists(retrypath))
                        //{
                        //    Directory.CreateDirectory(retrypath);
                        //}
                        //fi.CopyTo(retrypath + "\\" + fi.Name);
                        failedfiles.RemoveAt(i);

                    }
                    else
                    {
                        i++;
                    }

                }
                catch (Exception ex)
                {
                    LogService.WriteError("UploadFolder", folderpath, ex);

                }
            } 
            return failedfiles;
        }
    }
}
