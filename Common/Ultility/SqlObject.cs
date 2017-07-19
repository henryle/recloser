using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using log4net;
namespace FA_Accounting.Common
{
    public static class SqlConnectionObject
    {
        //public static string ConnectionString = "Data Source=(local);Initial Catalog=FAAccTest;User ID=sa;Password=Goodman99sa;MultipleActiveResultSets=True";
        public static string ConnectionString = @"Data Source=SERVER\SQLEXPRESS;Initial Catalog=Recloser;User ID=sa;Password=password00";
        //public static string ConnectionString = @"Data Source=(local);Initial Catalog=RecloserST;User ID=sa;Password=Goodman99sa";
        public static string InitialCatalog;
        public static SqlConnection connection;
        public static SqlTransaction transaction;

        public static string sCreateDatabaseDataPathFile;
        public static string sCreateTablePathFile;
        public static string sConvertDataPathFile;
        public static string sUpdateDataPathFile;

        public static string BuildConnectionString(string datasource, string catalog, eAuthenticateMode authenticateMode, string username, string password)
        {
            SqlConnectionStringBuilder cstBuilder = new SqlConnectionStringBuilder();
            cstBuilder.DataSource = datasource;
            cstBuilder.InitialCatalog = catalog;
            if (authenticateMode == eAuthenticateMode.Trusted)
            {
                cstBuilder.IntegratedSecurity = true;
                cstBuilder.UserID = "";
                cstBuilder.Password = "";
            }
            else
            {
                cstBuilder.IntegratedSecurity = false;
                cstBuilder.UserID = username;
                cstBuilder.Password = password;
            }
            ConnectionString = cstBuilder.ConnectionString;
            return cstBuilder.ConnectionString;
        }

        public static bool CheckConnection(string datasource, string catalog, eAuthenticateMode authenticateMode, string username, string password)
        {
            string cnnString = BuildConnectionString(datasource, catalog, authenticateMode, username, password);
            SqlConnection sqlConn = new SqlConnection(cnnString);
            try
            {
                sqlConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CheckConnection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public static void OpenConectionTransaction()
        {
            connection = new SqlConnection(ConnectionString);
            connection.Open();

            transaction = null;
        }

        public static SqlConnection OpenConection()
        {
            if (String.IsNullOrEmpty(ConnectionString))
                SqlConnectionObject.BuildConnectionString(DatabaseSetting.Instance.DataSource, DatabaseSetting.Instance.Database, DatabaseSetting.Instance.AuthenticateMode, DatabaseSetting.Instance.SqlUsername, DatabaseSetting.Instance.SqlPassword);
            SqlConnection aConnection;
            aConnection = new SqlConnection(ConnectionString);
            aConnection.Open();
            return aConnection;
        }

        public static SqlConnection OpenConection(string connString)
        {
            SqlConnection aConnection;
            aConnection = new SqlConnection(connString);
            aConnection.Open();
            return aConnection;
        }

        public static void CloseConectionTransaction()
        {
            if (transaction != null)
            {
                Rollback();
            }
            connection.Close();
        }

        public static void StartNewTransaction(SqlConnection aConnection)
        {
            transaction = aConnection.BeginTransaction(IsolationLevel.ReadUncommitted, "TRANSACTION");
        }

        public static void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
        }
        public static void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }

        public static object ReturnSingleField(string aSqlStr)
        {
            try
            {
                SqlCommand sqlCom;
                SqlConnection _connection;
                _connection = OpenConection();
                sqlCom = new SqlCommand(aSqlStr, _connection);
                object retVal = sqlCom.ExecuteScalar();
                if (retVal == DBNull.Value)
                    retVal = null;
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                return retVal;
            }
            catch (Exception ex)
            {
                LogService.WriteError("ReturnSingleField", aSqlStr, ex);
                return null;
            }
        }

        public static object ReturnSingleField(string aSqlStr, int aTimeOut)
        {
            try
            {
                SqlCommand sqlCom;
                SqlConnection _connection;
                _connection = OpenConection();
                sqlCom = new SqlCommand(aSqlStr, _connection);

                sqlCom.CommandTimeout = aTimeOut;
                object retVal = sqlCom.ExecuteScalar();
                if (retVal == DBNull.Value)
                    retVal = null;
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                return retVal;
            }
            catch (Exception ex)
            {
                LogService.WriteError("ReturnSingleField", aSqlStr, ex);
                return null;
            }
        }

        public static object ReturnSingleField(string aSqlStr, SqlConnection aConnection)
        {
            try
            {
                SqlCommand sqlCom;
                sqlCom = new SqlCommand(aSqlStr, aConnection);
                if (transaction != null)
                    sqlCom.Transaction = transaction;

                sqlCom.CommandTimeout = 0;
                if (aConnection.State == ConnectionState.Closed)
                {
                    aConnection.Open();
                }
                object retVal = sqlCom.ExecuteScalar();
                if (retVal == DBNull.Value)
                    retVal = null;
                return retVal;
            }
            catch (Exception ex)
            {
                LogService.WriteError("ReturnSingleField", aSqlStr, ex);
                return null;
            }
        }

        public static DataTable ReturnDataTable(string aSqlStr)
        {
            try
            {
                //return the complete dataset object
                SqlCommand sqlCom;
                SqlConnection _connection;
                _connection = OpenConection();
                if (_connection != null)
                {
                    sqlCom = new SqlCommand(aSqlStr, _connection);

                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCom);
                    DataTable dt = new DataTable();
                    sqlAdapter.Fill(dt);
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();

                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogService.WriteError("ReturnDataTable", aSqlStr, ex);
                return null;
            }
        }

        public static DataTable ReturnDataTable(string aSqlStr, SqlConnection aConnection)
        {
            try
            {
                //return the complete dataset object
                SqlCommand sqlCom;
                if (aConnection != null)
                {
                    sqlCom = new SqlCommand(aSqlStr, aConnection);
                    if (transaction != null)
                        sqlCom.Transaction = transaction;

                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCom);
                    DataTable dt = new DataTable();
                    sqlAdapter.Fill(dt);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogService.WriteError("ReturnDataTable", aSqlStr, ex);
                return null;
            }
        }
        public static bool RunNonQuery(string aSqlStr, int aTimeOut)
        {
            try
            {
                SqlConnection _connection;
                _connection = OpenConection();
                SqlCommand sqlCom = new SqlCommand(aSqlStr, _connection);

                if (transaction != null)
                    sqlCom.Transaction = transaction;

                sqlCom.CommandTimeout = aTimeOut;
                sqlCom.ExecuteNonQuery();
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("RunNonQuery", aSqlStr, ex);
                return false;
            }
        }

        public static bool RunNonQuery(string aSqlStr)
        {
            try
            {
                SqlConnection _connection;
                _connection = OpenConection();
                SqlCommand sqlCom = new SqlCommand(aSqlStr, _connection);

                if (transaction != null)
                    sqlCom.Transaction = transaction;

                sqlCom.CommandTimeout = 0;
                sqlCom.ExecuteNonQuery();
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("RunNonQuery", aSqlStr, ex);
                return false;
            }
        }

        public static bool RunNonQuery(string aSqlStr, SqlConnection aConnection)
        {
            try
            {
                SqlCommand sqlCom = new SqlCommand(aSqlStr, aConnection);

                if (transaction != null)
                    sqlCom.Transaction = transaction;

                sqlCom.CommandTimeout = 0;
                sqlCom.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("RunNonQuery", aSqlStr, ex);
                return false;
            }
        }

        public static void GetFTPSetting(ref string ServerIP, ref string UserID, ref string Password)
        {
            ServerIP = string.Empty;
            UserID = string.Empty;
            Password = string.Empty;
            string aSqlStr = "Select URL, Username, Password From FTPConfig";
            try
            {
                DataTable dt = ReturnDataTable(aSqlStr);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    if (row != null)
                    {
                        ServerIP = row[0].ToString();
                        UserID = row[1].ToString();
                        Password = row[2].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("GetFTPSetting", aSqlStr, ex);
            }
        }
        public static void SaveFTPSetting(string ServerIP, string UserID, string Password)
        {
            string aSqlStr = string.Format("Update FTPConfig SET URL= '{0}', Username= '{1}', Password= '{2}'", ServerIP, UserID, Password);
            try
            {
                RunNonQuery(aSqlStr);
            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveFTPSetting", aSqlStr, ex);
            }
        }

        public static void GetTimerSetting(ref int uploadTimer, ref int downloadTimer)
        {
            string aSqlStr = "Select UploadTimer, DownloadTimer From SyncTimer";
            try
            {
                DataTable dt = ReturnDataTable(aSqlStr);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    if (row != null)
                    {
                        if (row[0] != null) uploadTimer = int.Parse(row[0].ToString());
                        if (row[1] != null) downloadTimer = int.Parse(row[1].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("GetTimerSetting", aSqlStr, ex);
            }
        }

        public static void SaveTimerSetting(DateTime dtUploadTimer, DateTime dtDownloadTimer)
        {
            string aSqlStr = string.Empty;
            try
            {
                int uploadTimer = (int)(TimeSpan.Parse(dtUploadTimer.ToString("HH:mm")).TotalMinutes);
                int downloadTimer = (int)TimeSpan.Parse(dtDownloadTimer.ToString("HH:mm")).TotalMinutes;
                aSqlStr = string.Format("Update SyncTimer SET UploadTimer= {0}, DownloadTimer= {1}", uploadTimer, downloadTimer);
                RunNonQuery(aSqlStr);
            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveTimerSetting", aSqlStr, ex);
            }
        }

    }
}
