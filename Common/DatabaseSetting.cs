using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Win32;

namespace FA_Accounting.Common
{
    //[SettingsProvider(typeof(RegistrySettingsProvider))]
    public class DatabaseSetting : ApplicationSettingsBase
    {
        #region Properties

        [ApplicationScopedSetting]
        [DefaultSettingValue("(local)")]
        public string DataSource
        {
            get { return (string)this["DataSource"]; }
            set { this["DataSource"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("")]
        public string Database
        {
            get { return (string)this["Database"]; }
            set { this["Database"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("sa")]
        public string SqlUsername
        {
            get { return (string)this["SqlUsername"]; }
            set { this["SqlUsername"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("")]
        public string SqlPassword
        {
            get { return (string)this["SqlPassword"]; }
            set { this["SqlPassword"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("Fixed")]
        public eAuthenticateMode AuthenticateMode
        {
            get { return (eAuthenticateMode)this["AuthenticateMode"]; }
            set { this["AuthenticateMode"] = value; }
        }
        
        [ApplicationScopedSetting]
        [DefaultSettingValue("2")]
        public int UserId
        {
            get { return (int)this["UserId"]; }
            set { this["UserId"] = value; }
        }
        [Browsable(false)]
        [ApplicationScopedSetting]
        [DefaultSettingValue("30")]
        public int ConnectionTimeout
        {
            get { return (int)this["ConnectionTimeout"]; }
            set { this["ConnectionTimeout"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("")]
        public string Location
        {
            get { return (string)this["Location"]; }
            set { this["Location"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("600")] // 10 minutes = 600 secs
        public int TimerInterval
        {
            get { return (int)this["TimerInterval"]; }
            set { this["TimerInterval"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("10")] // 10 secs
        public int IntervalSecond
        {
            get { return (int)this["IntervalSecond"]; }
            set { this["IntervalSecond"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("5")] // 5 times
        public int RetryMax
        {
            get { return (int)this["RetryMax"]; }
            set { this["RetryMax"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("100")] // 100 secs
        public int FTPRequestTimeout
        {
            get { return (int)this["FTPRequestTimeout"]; }
            set { this["FTPRequestTimeout"] = value; }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("1")] // true
        public int DeleteFileAfterSyn
        {
            get { return (int)this["DeleteFileAfterSyn"]; }
            set { this["DeleteFileAfterSyn"] = value; }
        }


        #endregion

        #region Singleton Pattern
        public DatabaseSetting()
            : base()
        {
        }

        private static DatabaseSetting _instance;

        public static DatabaseSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseSetting();
                }

                return _instance;
            }
        }
        #endregion

        public static bool CheckConnection(string dataSource, eAuthenticateMode authenticateMode, string initialCatalog, string userId, string password)
        {
            bool success = false;
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.InitialCatalog = initialCatalog;
                builder.DataSource = dataSource;
                builder.IntegratedSecurity = false;
                if (authenticateMode == eAuthenticateMode.Trusted) //trusted windows authenticaion
                {
                    builder.IntegratedSecurity = true;
                    builder.UserID = "";
                    builder.Password = "";
                }
                else
                {
                    builder.IntegratedSecurity = false;
                    builder.UserID = userId;
                    builder.Password = password;
                }

                SqlConnection cn = new SqlConnection(builder.ConnectionString);
                cn.Open();
                success = true;
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error logging in. " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return success;
        }

        public string ConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = this.DataSource;
            builder.InitialCatalog = this.Database;

            if (AuthenticateMode == eAuthenticateMode.Trusted) //trusted windows authenticaion
            {
                builder.IntegratedSecurity = true;
                builder.UserID = "";
                builder.Password = "";
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = this.SqlUsername;
                builder.Password = this.SqlPassword;
            }

            builder.MultipleActiveResultSets = true;
            builder.ConnectTimeout = this.ConnectionTimeout;

            return builder.ConnectionString;
        }

    }
}
