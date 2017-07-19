using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace FA_Accounting.Common
{
    public class Config
    {
        public string ftpserver;
        public string ftpuser;
        public string ftppassword;
        public string accesspassword = string.Empty;
        public int interval;
        public int deletedays;
        public string path;
        public int function;
        public string location;
        public DateTime uploadfromtime;
        private static Config _instance;

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Config();
                }

                return _instance;
            }
        }
        public Config()
        {
            Reload();
            
        }
        public void Reload()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalVariables.configfile);
            using (StreamReader readFile = new StreamReader(path, Encoding.UTF8))
            {
                string line;
                string value;
                string varname;
                while ((line = readFile.ReadLine()) != null)
                {
                    value = line.Substring(line.IndexOf('=') + 1);
                    varname = line.Substring(0, line.IndexOf('='));
                    if (varname == GlobalVariables.HOST)
                    {
                        ftpserver = value;
                    }
                    else if (varname == GlobalVariables.USER)
                    {
                        ftpuser = value;
                    }
                    else if (varname == GlobalVariables.PSW)
                    {
                        ftppassword = value;

                    }
                    else if (varname == GlobalVariables.ACCESSPSW)
                    {
                        accesspassword = value;

                    }
                    else if (varname == GlobalVariables.INTERVAL)
                    {
                        interval = Convert.ToInt16(value);
                    }
                    else if (varname == GlobalVariables.DELETEDAYS)
                    {
                        deletedays = Convert.ToInt16(value);
                    }
                    else if (varname == GlobalVariables.PATH)
                    {
                        this.path = value;
                    }
                    else if (varname == GlobalVariables.FUNCTION)
                    {
                        function = Convert.ToInt16(value);
                    }
                    else if (varname == GlobalVariables.LOCATION)
                    {
                        location = value;
                    }
                    else if (varname == GlobalVariables.UPLOADFROM)
                    {
                        uploadfromtime = Convert.ToDateTime(value);
                    }


                }
            }
        }
        public void Save()
        {
            //string encryptedpwd = (savedpsw == txtPwd.Text ? savedpsw : Encrypt.EncryptString(txtPwd.Text, FTP.secret));
            //string encryptedaccesspwd = (savedmatkhau == txtMatkhau.Text ? savedmatkhau : Encrypt.EncryptString(txtMatkhau.Text, FTP.secret));
            try
            {

                StreamWriter file = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalVariables.configfile), false);

                string strline;
                strline = GlobalVariables.HOST + "=" + this.ftpserver;
                file.WriteLine(strline);
                strline = GlobalVariables.USER + "=" + this.ftpuser;
                file.WriteLine(strline);
                strline = GlobalVariables.PSW + "=" + this.ftppassword;
                file.WriteLine(strline);
                strline = GlobalVariables.INTERVAL + "=" + this.interval.ToString();
                file.WriteLine(strline);
                strline = GlobalVariables.DELETEDAYS + "=" + this.deletedays.ToString();
                file.WriteLine(strline);
                strline = GlobalVariables.PATH + "=" + this.path;
                file.WriteLine(strline);
                strline = GlobalVariables.FUNCTION + "=" + this.function.ToString();
                file.WriteLine(strline);
                strline = GlobalVariables.LOCATION + "=" + this.location;
                file.WriteLine(strline);
                strline = GlobalVariables.UPLOADFROM + "=" + this.uploadfromtime;
                file.WriteLine(strline);
                strline = GlobalVariables.ACCESSPSW + "=" + this.accesspassword;
                file.WriteLine(strline);

                file.Close();
                

            }
            catch (Exception ex)
            {
                LogService.WriteError("Config.Save()", "", ex);
            }
        }
    }
}
