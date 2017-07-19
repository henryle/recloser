using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using RecloserAcq.Device;
using System.Data;
using System.IO;
using System.Windows.Forms;
using FA_Accounting.Common;
namespace RecloserAcq.OracleDAL
{
    public class DBController
    {
          #region Singleton

        private static object _syncObj = new object();
        private static DBController _instance;


        public static DBController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObj)
                    {
                        _instance = new DBController();
                    }
                }

                return _instance;
            }
        }

        #endregion
        public string ConnectionString
        {
            set
            {
                _connectionString = value;
            }
        }

            
        

        /*private string _connectionString = "Data Source=(DESCRIPTION="
             + "(ADDRESS=(PROTOCOL=TCP)(HOST=ORASRVR)(PORT=1521))"
             + "(CONNECT_DATA=(SERVICE_NAME=ORCL)));"
             + "User Id=hr;Password=hr;";*/
        private string _connectionString = DeviceType.Properties.Settings.Default.oracleconnectionstring;
            /*"User Id=mimic01;Password=mimic01mm1;Data Source=(DESCRIPTION =" +
    "(ADDRESS_LIST ="+
    "  (ADDRESS = (PROTOCOL = TCP)(HOST = 10.175.88.100)(PORT = 1521))" +
    ")"+
    "(CONNECT_DATA ="+
      "(SERVICE_NAME = MIMIC1)"+
    ")"+
  ")";*/
        //SqlConnection _sqlConn;
        public DBController()
        {
            
            //_sqlConn = new SqlConnection(connectionString);
            
        }

        public void LogOperation(int userid, string username, int deviceid, string devicename, string groupname, int action, bool auto)
        {
            string str = "sp_logoperation";
            try
            {

                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand sqlCommand = new OracleCommand(str, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                       //p_userid integer, p_username varchar2, p_deviceid integer, p_devicename varchar2, p_groupname varchar2, p_action integer,  p_auto integer)

                        var param = sqlCommand.Parameters.Add("p_userid", OracleDbType.Int16);
                        param.Value = userid;
                        param = sqlCommand.Parameters.Add("p_username", OracleDbType.Varchar2);
                        param.Value = username;
                        param = sqlCommand.Parameters.Add("p_deviceid", OracleDbType.Int32);
                        param.Value = deviceid;
                        param = sqlCommand.Parameters.Add("p_devicename", OracleDbType.Varchar2);
                        param.Value = devicename;
                        param = sqlCommand.Parameters.Add("p_groupname", OracleDbType.Varchar2);
                        param.Value = groupname;
                        param = sqlCommand.Parameters.Add("p_action", OracleDbType.Int32);
                        param.Value = action;
                        param = sqlCommand.Parameters.Add("p_auto", OracleDbType.Int32);
                        param.Value = auto;
                        
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        var result = sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }


            }

            catch (Exception ex)
            {
                LogService.WriteError("Log Operation", ex.Message);
            }
            finally
            {
                //_sqlConn.Close();
            }

        }
        public DataTable searchOperationLog(DateTime from, DateTime to, int id = 0)
        {
            //strvalues = row["DeviceId"].ToString() + " \t " + row["Name"].ToString() + " \t " + row["Location"].ToString() + " \t " + row["Opt"].ToString() + " \t " + row["Mod"].ToString()
              //      + " \t" + row["userid"] + " \t" + row["username"];
            
            //string str = "sp_searchoperationlog";
            try
            {
                /*string str = "Select DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18 From StatusInfo WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To ";*/
                string str = "sp_searchoperationlog";
                return ExecSearch(id, from, to, str);
            }
            catch (Exception ex)
            {
                LogService.WriteError("Search Operation", ex.ToString());

            }
            finally
            {
                //_sqlConn.Close();
            }
            return null;

        }
        public void SaveReceive(DateTime date, int port, string data)
        {
            //string str = string.Format("INSERT INTO ReceiveLog(DateRec, Port, Data) " +
            //   "VALUES(@Date, @Port, @Data)");
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            string str = "sp_insReceiveLog";
            
            try
            {

                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand sqlCommand = new OracleCommand(str, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        var param = sqlCommand.Parameters.Add("p_DateRec", OracleDbType.Date);
                        param.Value = date;
                        param = sqlCommand.Parameters.Add("p_Port", OracleDbType.Int32);
                        param.Value = port;
                        param = sqlCommand.Parameters.Add("p_Data", OracleDbType.Varchar2,500);
                        param.Value = data;
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        var result = sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveReceiv", ex.Message);
            }
            
        }
        //spSelectSchedule spDeleteSchedule
        public void DeleteSchedule(DeviceEvent obj)
        {
            

            string str = "spDeleteSchedule";

            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_RowId", OracleDbType.Int32);
                        param.Value = obj.Id;
                        

                        // sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                LogService.WriteError("Delete Schedule", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void SaveSchedule(DeviceEvent obj)
        {
            if (obj.DtExpire < DateTime.Now) return;

            string str = "spInsertSchedule";

            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_DeviceId", OracleDbType.Int32);
                        param.Value = obj.DeviceId;
                        param = cmd.Parameters.Add("p_HourRepeat", OracleDbType.Int32);
                        param.Value = obj.hourRepeat;
                        param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 50);
                        param.Value = obj.Type.ToString();
                        param = cmd.Parameters.Add("p_DtExpire", OracleDbType.Date);
                        param.Value = obj.DtExpire;
                        param = cmd.Parameters.Add("p_DtActive", OracleDbType.Date);
                        param.Value = obj.DtActive;
                        param = cmd.Parameters.Add("p_weekday", OracleDbType.Varchar2,50);
                        param.Value = obj.Weekday;
                        param = cmd.Parameters.Add("p_EventName", OracleDbType.Varchar2, 100);
                        param.Value = obj.NameOfEvent;
                        param = cmd.Parameters.Add("p_Command", OracleDbType.Varchar2,10);
                        param.Value = obj.Command;
                        param = cmd.Parameters.Add("p_DtNextRun", OracleDbType.Date);
                        param.Value = obj.DtNextRun;
                        
                        
                        // sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                LogService.WriteError("Insert Schedule" , ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void UpdateSchedule(DeviceEvent obj)
        {
            

            string str = "spUpdateSchedule";

            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_RowId", OracleDbType.Int32);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("p_DeviceId", OracleDbType.Int32);
                        param.Value = obj.DeviceId;
                        param = cmd.Parameters.Add("p_HourRepeat", OracleDbType.Int32);
                        param.Value = obj.hourRepeat;
                        param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 50);
                        param.Value = obj.Type.ToString();
                        param = cmd.Parameters.Add("p_DtExpire", OracleDbType.Date);
                        param.Value = obj.DtExpire;
                        param = cmd.Parameters.Add("p_DtActive", OracleDbType.Date);
                        param.Value = obj.DtActive;
                        param = cmd.Parameters.Add("p_weekday", OracleDbType.Varchar2, 50);
                        param.Value = obj.Weekday;
                        param = cmd.Parameters.Add("p_EventName", OracleDbType.Varchar2, 100);
                        param.Value = obj.NameOfEvent;
                        param = cmd.Parameters.Add("p_Command", OracleDbType.Varchar2, 10);
                        param.Value = obj.Command;
                        param = cmd.Parameters.Add("p_DtNextRun", OracleDbType.Date);
                        param.Value = obj.DtNextRun;


                        // sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                LogService.WriteError("Update Next runtime Schedule", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void SaveRecloser(RecloserBase obj)
        {
            if (obj.LastUpdated == DateTime.MinValue) return;

            //string str = string.Format("INSERT INTO StatusInfo(DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18) " +
            //   "VALUES(@DeviceId, @Type, @Date, @Alert, @Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10, @Field11, @Field12, @Field13, @Field14, @Field15, @Field16, @Field17, @Field18)");
            string str = "InsertStatusInfo";
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 50);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("p_DateRec", OracleDbType.Date);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("p_Alert", OracleDbType.Int32);
                        param.Value = obj.Alert;

                        param = cmd.Parameters.Add("p_Field1", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field2", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field3", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field4", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field5", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field6", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field7", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field8", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field9", OracleDbType.Varchar2, 50);

                        param = cmd.Parameters.Add("p_Field10", OracleDbType.Varchar2, 50);


                        cmd.Parameters["p_Field1"].Value = obj.Amp12.ToString();
                        cmd.Parameters["p_Field2"].Value = obj.Amp34.ToString();
                        cmd.Parameters["p_Field3"].Value = obj.Amp56.ToString();
                        cmd.Parameters["p_Field4"].Value = obj.AmpEarth.ToString();
                        cmd.Parameters["p_Field5"].Value = obj.Operations.ToString();
                        cmd.Parameters["p_Field6"].Value = obj.Battery_1 ?? "";
                        cmd.Parameters["p_Field7"].Value = obj.Battery_2 ?? "";
                        cmd.Parameters["p_Field8"].Value = obj.Status_Open.ToString();
                        cmd.Parameters["p_Field9"].Value = obj.Status_Close.ToString();
                        cmd.Parameters["p_Field10"].Value = obj.Status_Lockout.ToString();

                        if (obj is CooperFXB)
                        {
                            var coo = obj as CooperFXB;
                            param = cmd.Parameters.Add("p_Field11", OracleDbType.Varchar2, 50);
                            param.Value = coo.Target12.ToString();
                            param = cmd.Parameters.Add("p_Field12", OracleDbType.Varchar2, 50);
                            param.Value = coo.Target34.ToString();
                            param = cmd.Parameters.Add("p_Field13", OracleDbType.Varchar2, 50);
                            param.Value = coo.Target56.ToString();
                            param = cmd.Parameters.Add("p_Field14", OracleDbType.Varchar2, 50);
                            param.Value = coo.EarthTarget.ToString();
                            param = cmd.Parameters.Add("p_Field15", OracleDbType.Varchar2, 50);
                            param.Value = coo.Status_Target12.ToString();
                            param = cmd.Parameters.Add("p_Field16", OracleDbType.Varchar2, 50);
                            param.Value = coo.Status_Target34.ToString();
                            param = cmd.Parameters.Add("p_Field17", OracleDbType.Varchar2, 50);
                            param.Value = coo.Status_Target56.ToString();
                            param = cmd.Parameters.Add("p_Field18", OracleDbType.Varchar2, 50);
                            param.Value = coo.Status_EarthTarget.ToString();
                        }
                        else if (obj is Nulec)
                        {
                            var nuc = obj as Nulec;
                            param = cmd.Parameters.Add("p_Field11", OracleDbType.Varchar2, 50);
                            param.Value = nuc.ApparentPower.ToString();
                            param = cmd.Parameters.Add("p_Field12", OracleDbType.Varchar2, 50);
                            param.Value = nuc.ReactivePower.ToString();
                            param = cmd.Parameters.Add("p_Field13", OracleDbType.Varchar2, 50);
                            param.Value = nuc.RealPower.ToString();
                            param = cmd.Parameters.Add("p_Field14", OracleDbType.Varchar2, 50);
                            param.Value = nuc.PowerFactor.ToString();
                            param = cmd.Parameters.Add("p_Field15", OracleDbType.Varchar2, 50);

                            param.Value = ((nuc.DeviceTime.HasValue && nuc.DeviceTime.Value > DateTime.MinValue) ? nuc.DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "");

                            param = cmd.Parameters.Add("p_Field16", OracleDbType.Varchar2, 50);
                            param.Value = "False";
                            param = cmd.Parameters.Add("p_Field17", OracleDbType.Varchar2, 50);
                            param.Value = "False";
                            param = cmd.Parameters.Add("p_Field18", OracleDbType.Varchar2, 50);
                            param.Value = "False";
                        }
                        else
                        {
                            return;
                        }
                       // sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                LogService.WriteError("Save Datanew " + obj.Name +" " + _connectionString, ex.Message);
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void SaveRecloserDisCon(RecloserBase obj)
        {


            //string str = string.Format("INSERT INTO StatusInfo(DeviceId, Type, Date, Alert, Status) " +
            //   "VALUES(@DeviceId, @Type, @Date, @Alert, @Status)");
            string str = "sp_SaveRecloserDisCon";
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int16);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 50);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("p_DateRec", OracleDbType.Date);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("p_Alert", OracleDbType.Int16);
                        param.Value = obj.Alert;
                        param = cmd.Parameters.Add("p_Status", OracleDbType.Varchar2, 50);
                        param.Value = obj.CommStatus;


                        // sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                LogService.WriteError("Save Data341" + obj.Name, ex.Message);
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void SaveRecloser351(Recloser351R obj)
        {
            // if (obj.LastUpdated == DateTime.MinValue) return;

            /*string str = string.Format("INSERT INTO RecloserSel(DeviceId, Date, Alert, [MW_A],	[MW_B],	[MW_C],	[MW_3P],"+	
            "[Q_MVAR_A],	[Q_MVAR_B],	[Q_MVAR_C],	[Q_MVAR_3P],	[PF_A],	[PF_B],	[PF_C],	[PF_3P],	[voltsValue_MAG_A],	" + 
            "[voltsValue_MAG_B],	[voltsValue_MAG_C],	[voltsValue_MAG_S],	[vang_A],	[vang_B],	[vang_C],	[vang_S],	[imag_A],	"+
            "[imag_B],	[imag_N],	[imag_G],	[imag_C])" +
             " VALUES (@DeviceId,  @Date, @Alert, @MW_A,	@MW_B,	@MW_C,	@MW_3P,	@Q_MVAR_A,"+	
             "p_Q_MVAR_B,	@Q_MVAR_C,	@Q_MVAR_3P,	@PF_A,	@PF_B,	@PF_C,	@PF_3P,	@voltsValue_MAG_A,"+
             "p_voltsValue_MAG_B,	@voltsValue_MAG_C,	@voltsValue_MAG_S,	@vang_A,	@vang_B,	@vang_C,	"+
             "p_vang_S,	@imag_A,	@imag_B,	@imag_N,	@imag_G,	@imag_C)");*/
            string str = "sp_SaveRecloser351";
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                    
                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        /*param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 10);
                        param.Value = obj.DeviceType.ToString();*/
                        param = cmd.Parameters.Add("p_DateRec", OracleDbType.Date);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("p_Alert", OracleDbType.Int16);
                        param.Value = obj.Alert;

                        param = cmd.Parameters.Add("p_MW_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_MW_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_MW_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_MW_3P", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Q_MVAR_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Q_MVAR_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Q_MVAR_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Q_MVAR_3P", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_3P", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_voltsValue_MAG_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_voltsValue_MAG_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_voltsValue_MAG_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_voltsValue_MAG_S", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_vang_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_vang_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_vang_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_vang_S", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_imag_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_imag_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_imag_N", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_imag_G", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_imag_C", OracleDbType.Varchar2, 10);

                        cmd.Parameters["p_MW_A"].Value = obj.MW_A.ToString();
                        cmd.Parameters["p_MW_B"].Value = obj.MW_B.ToString();
                        cmd.Parameters["p_MW_C"].Value = obj.MW_C.ToString();
                        cmd.Parameters["p_MW_3P"].Value = obj.MW_3P.ToString();
                        cmd.Parameters["p_Q_MVAR_A"].Value = obj.Q_MVAR_A.ToString();
                        cmd.Parameters["p_Q_MVAR_B"].Value = obj.Q_MVAR_B.ToString();
                        cmd.Parameters["p_Q_MVAR_C"].Value = obj.Q_MVAR_C.ToString();
                        cmd.Parameters["p_Q_MVAR_3P"].Value = obj.Q_MVAR_3P.ToString();
                        cmd.Parameters["p_PF_A"].Value = obj.PF_A.ToString();
                        cmd.Parameters["p_PF_B"].Value = obj.PF_B.ToString();
                        cmd.Parameters["p_PF_C"].Value = obj.PF_C.ToString();
                        cmd.Parameters["p_PF_3P"].Value = obj.PF_3P.ToString();
                        cmd.Parameters["p_voltsValue_MAG_A"].Value = obj.VoltsValue_MAG_A.ToString();
                        cmd.Parameters["p_voltsValue_MAG_B"].Value = obj.VoltsValue_MAG_B.ToString();
                        cmd.Parameters["p_voltsValue_MAG_C"].Value = obj.VoltsValue_MAG_C.ToString();
                        cmd.Parameters["p_voltsValue_MAG_S"].Value = obj.VoltsValue_MAG_S.ToString();
                        cmd.Parameters["p_vang_A"].Value = obj.Vang_A.ToString();
                        cmd.Parameters["p_vang_B"].Value = obj.Vang_B.ToString();
                        cmd.Parameters["p_vang_C"].Value = obj.Vang_C.ToString();
                        cmd.Parameters["p_vang_S"].Value = obj.Vang_S.ToString();
                        cmd.Parameters["p_imag_A"].Value = obj.Imag_A.ToString();
                        cmd.Parameters["p_imag_B"].Value = obj.Imag_B.ToString();
                        cmd.Parameters["p_imag_N"].Value = obj.Imag_N.ToString();
                        cmd.Parameters["p_imag_G"].Value = obj.Imag_G.ToString();
                        cmd.Parameters["p_imag_C"].Value = obj.Imag_C.ToString();

                        //cmd.Parameters["p_Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["p_Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["p_Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
                //SqlConnection sqlConn = new SqlConnection(_connectionString);
                
                



            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveRecloser351", ex.Message);
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void SaveElster(Elster1700 obj)
        {
            // if (obj.LastUpdated == DateTime.MinValue) return;

            string str = "sp_SaveElster";/* string.Format("INSERT INTO Elster(DeviceId, Date, Alert, [Volt_A],	[Volt_B],	[Volt_C],	[Volt_Total]," +
             "[Ample_A],	[Ample_B],	[Ample_C],	[Ample_Total],	[PF_A],	[PF_B],	[PF_C],	[PF_Total],	[AP_A],	" +
             "[AP_B],	[AP_C],	[AP_Total],	[ReAP_A],	[ReAP_B],	[ReAP_C],	[ReAP_Total])" +
              " VALUES (@DeviceId,  @Date, @Alert, @Volt_A,	@Volt_B,	@Volt_C,	@Volt_Total,	@Ample_A," +
              "p_Ample_B,	@Ample_C,	@Ample_Total,	@PF_A,	@PF_B,	@PF_C,	@PF_Total,	@AP_A," +
              "p_AP_B,	@AP_C,	@AP_Total,	@ReAP_A,	@ReAP_B,	@ReAP_C,	" +
              "p_ReAP_Total)");*/
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        /*param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 10);
                        param.Value = obj.DeviceType.ToString();*/
                        param = cmd.Parameters.Add("p_DateRec", OracleDbType.Date);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("p_Alert", OracleDbType.Decimal);
                        param.Value = obj.Alert;

                        param = cmd.Parameters.Add("p_Volt_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Volt_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Volt_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Volt_Total", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Ample_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Ample_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Ample_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Ample_Total", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PF_Total", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_AP_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_AP_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_AP_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_AP_Total", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_ReAP_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_ReAP_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_ReAP_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_ReAP_Total", OracleDbType.Varchar2, 10);
                        

                        cmd.Parameters["p_Volt_A"].Value = obj.Volt_A.ToString();
                        cmd.Parameters["p_Volt_B"].Value = obj.Volt_B.ToString();
                        cmd.Parameters["p_Volt_C"].Value = obj.Volt_C.ToString();
                        cmd.Parameters["p_Volt_Total"].Value = obj.Volt_Total.ToString();
                        cmd.Parameters["p_Ample_A"].Value = obj.Ample_A.ToString();
                        cmd.Parameters["p_Ample_B"].Value = obj.Ample_B.ToString();
                        cmd.Parameters["p_Ample_C"].Value = obj.Ample_C.ToString();
                        cmd.Parameters["p_Ample_Total"].Value = obj.Ample_Total.ToString(); 
                        cmd.Parameters["p_PF_A"].Value = obj.PowerFactor_A.ToString();
                        cmd.Parameters["p_PF_B"].Value = obj.PowerFactor_B.ToString();
                        cmd.Parameters["p_PF_C"].Value = obj.PowerFactor_C.ToString();
                        cmd.Parameters["p_PF_Total"].Value = obj.PowerFactor_Total.ToString();
                        cmd.Parameters["p_AP_A"].Value = obj.ActivePower_A.ToString();
                        cmd.Parameters["p_AP_B"].Value = obj.ActivePower_B.ToString();
                        cmd.Parameters["p_AP_C"].Value = obj.ActivePower_C.ToString();
                        cmd.Parameters["p_AP_Total"].Value = obj.ActivePower_Total.ToString();
                        cmd.Parameters["p_ReAP_A"].Value = obj.ReActivePower_A.ToString();
                        cmd.Parameters["p_ReAP_B"].Value = obj.ReActivePower_B.ToString();
                        cmd.Parameters["p_ReAP_C"].Value = obj.ReActivePower_C.ToString();
                        cmd.Parameters["p_ReAP_Total"].Value = obj.ReActivePower_Total.ToString();
                        

                        //cmd.Parameters["p_Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["p_Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["p_Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
                //SqlConnection sqlConn = new SqlConnection(_connectionString);




            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveElster 1", ex.ToString() + " Total:" + obj.Volt_Total.ToString());
            }
            finally
            {
                //_sqlConn.Close();
                
            }
        }
        public void SaveTubu(TuBu obj,DateTime operationtime, bool open,bool? auto)
        {
           // if (obj.LastUpdated == DateTime.MinValue) return;

            string str = "sp_SaveTubu"; /* string.Format("INSERT INTO TuBu (DeviceId, Name,Location, OperationTime, Operation, Auto) " +
                "VALUES(@DeviceId, @Name, @Location,@OperationTime, @Operation, @Auto)");*/
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                    
                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("p_Name", OracleDbType.Varchar2, 50);
                        param.Value = obj.Name;

                        param = cmd.Parameters.Add("p_Location", OracleDbType.Varchar2, 50);
                        param.Value = obj.Location;

                        param = cmd.Parameters.Add("p_OperationTime", OracleDbType.Date);
                        param.Value = operationtime;//.ToString("yyyy-MM-dd HH:mm:ss");
                        //sqlcmd.Parameters.AddWithValue("p_enterdate", dtEnterDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        param = cmd.Parameters.Add("p_Operation", OracleDbType.Int16);
                        param.Value = open ? 1 : 0;
                        param = cmd.Parameters.Add("p_AutoMode", OracleDbType.Int16);
                        if (auto == null)
                        { param.Value = DBNull.Value; }
                        else
                        { param.Value = Convert.ToInt16(auto); }
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }


                
            }
            
            catch (Exception ex)
            {
                LogService.WriteError("SaveTubu",ex.Message);
            }
            finally
            {
                
            }
        }
        public void SaveLBS(LBS obj, DateTime operationtime, bool open, bool? auto)
        {
            

            string str = "sp_SaveLBS"; /* string.Format("INSERT INTO TuBu (DeviceId, Name,Location, OperationTime, Operation, Auto) " +
                "VALUES(@DeviceId, @Name, @Location,@OperationTime, @Operation, @Auto)");*/
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("p_Name", OracleDbType.Varchar2, 50);
                        param.Value = obj.Name;

                        param = cmd.Parameters.Add("p_Location", OracleDbType.Varchar2, 50);
                        param.Value = obj.Location;

                        param = cmd.Parameters.Add("p_OperationTime", OracleDbType.Date);
                        param.Value = operationtime;//.ToString("yyyy-MM-dd HH:mm:ss");
                        //sqlcmd.Parameters.AddWithValue("p_enterdate", dtEnterDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        param = cmd.Parameters.Add("p_Operation", OracleDbType.Int16);
                        param.Value = open ? 1 : 0;
                       
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }



            }

            catch (Exception ex)
            {
                LogService.WriteError("SaveLBS", ex.Message);
            }
            finally
            {

            }
        }
        public void SaveDevice(RecloserBase obj)
        {
            string str = "sp_SaveDevice";
            if (obj.isnew)
            {
                str = "sp_InsertNewDevice";
            }
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("p_Port", OracleDbType.Int32);
                        param.Value = obj.Port;
                        param = cmd.Parameters.Add("p_Name", OracleDbType.Varchar2, 50);
                        param.Value = obj.Name;
                        param = cmd.Parameters.Add("p_datedeleted", OracleDbType.Date);
                        param.Value = obj.DateDeleted;
                        param = cmd.Parameters.Add("p_Location", OracleDbType.Varchar2, 50);
                        param.Value = obj.Location;
                        param = cmd.Parameters.Add("p_DeviceType", OracleDbType.Varchar2, 50);
                        param.Value = obj.DeviceType;
                        param = cmd.Parameters.Add("p_Client", OracleDbType.Int16);
                        param.Value = obj.Client;
                        param = cmd.Parameters.Add("p_Enable", OracleDbType.Int16);
                        param.Value = obj.Enable;
                        param = cmd.Parameters.Add("p_ServerAddress", OracleDbType.Varchar2, 50);
                        param.Value = obj.Serveraddress;
                        param = cmd.Parameters.Add("p_Latencybetweenrequest", OracleDbType.Int32);
                        param.Value = obj.Latencybetweenrequest;
                        param = cmd.Parameters.Add("p_Latencybetweenpoll", OracleDbType.Int32);
                        param.Value = obj.Latencybetweenpoll;
                        param = cmd.Parameters.Add("p_LatencySaveHistory", OracleDbType.Int32);
                        param.Value = obj.LatencySaveHistory;
                        param = cmd.Parameters.Add("p_Maxampduration", OracleDbType.Int32);
                        param.Value = obj.Maxampduration;
                        param = cmd.Parameters.Add("p_MaxAmp", OracleDbType.Int32);
                        param.Value = obj.MaxAmp;
                        param = cmd.Parameters.Add("p_MinAmp", OracleDbType.Int32);
                        param.Value = obj.MinAmp;
                        param = cmd.Parameters.Add("p_GroupId", OracleDbType.Int32);
                        param.Value = obj.GroupId;
                        param = cmd.Parameters.Add("p_password", OracleDbType.Varchar2);
                        param.Value = obj.Password;
                        param = cmd.Parameters.Add("p_deviceaddress", OracleDbType.Varchar2);
                        param.Value = obj.DeviceAddress;
                        param = cmd.Parameters.Add("p_baudrate", OracleDbType.Int32);
                        param.Value = obj.BaudRate;
                        param = cmd.Parameters.Add("p_voltstandard", OracleDbType.Int32);
                        param.Value = obj.VolStandard;
                        param = cmd.Parameters.Add("p_voltpercent", OracleDbType.Int32);
                        param.Value = obj.Volpercent;
                        param = cmd.Parameters.Add("p_secondwait", OracleDbType.Int32);

                        param.Value = obj.SecondWait;
                        param = cmd.Parameters.Add("p_amppercent", OracleDbType.Int32);

                        param.Value = obj.AmpPercent;
                        
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }



            }

            catch (Exception ex)
            {
                LogService.WriteError("Save Device", ex.Message);
                MessageBox.Show(ex.Message, "Save Error: " + obj.Name  );

            }
            finally
            {

            }
        }
        public void SaveGroup(GroupClass obj)
        {
            string str = "sp_SaveGroup";
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.ID;
                        
                        param = cmd.Parameters.Add("p_GroupName", OracleDbType.Varchar2, 50);
                        param.Value = obj.GroupName;
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                LogService.WriteError("Save Group", ex.Message);
            }
            finally
            {

            }
        }
        public void InsertGroup(GroupClass obj)
        {
            string str = "sp_insertgroup";
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        var param = cmd.Parameters.Add("p_GroupName", OracleDbType.Varchar2, 50);
                        param.Value = obj.GroupName;
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }



            }

            catch (Exception ex)
            {
                LogService.WriteError("Insert Group", ex.Message);
            }
            finally
            {

            }
        }
        public DataTable Search(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
                /*string str = "Select DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18 From StatusInfo WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To ";*/
                string str = "sp_searchNulecCooper";
                return ExecSearch(id, from, to, str);
            }
            catch (Exception ex)
            {
                LogService.WriteError("Search History", ex.ToString());
                
            }         
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }

        private DataTable ExecSearch(object id, object from, object to,  string strcmd)
        {
            DataTable tb = new DataTable();
            using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                // Need to define cmdText.

                using (OracleCommand cmd = new OracleCommand(strcmd, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    var para = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                    para.Value = id;
                    para = cmd.Parameters.Add("p_From", OracleDbType.Date);
                    para.Value = from;
                    para = cmd.Parameters.Add("p_To", OracleDbType.Date);
                    para.Value = to;
                    OracleParameter oraP = new OracleParameter();
                    oraP.OracleDbType = OracleDbType.RefCursor;
                    oraP.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(oraP);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    da.Fill(tb);
                    //var result = cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    return tb;
                }
            }
        }

        public DataTable GetADeviceSchedule(int DeviceId)
        {
            DataTable tb = new DataTable();
            try
            {
                string str = "sp_getADeviceSchedule";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
   
                        var para = cmd.Parameters.Add("p_DeviceId", OracleDbType.Int32);
                        para.Value = DeviceId;
                        
                        OracleParameter oraP = new OracleParameter();
                        oraP.OracleDbType = OracleDbType.RefCursor;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get Devices", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }
        public DataTable GetDevicesSchedule(object groupid)
        {
            DataTable tb = new DataTable();
            try
            {
                string str = "sp_getDevicesSchedule";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
   
                        var para = cmd.Parameters.Add("p_GroupId", OracleDbType.Int32);
                        para.Value = groupid;
                        
                        OracleParameter oraP = new OracleParameter();
                        oraP.OracleDbType = OracleDbType.RefCursor;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get Devices Schedule", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }
        public int GetGroupId(int DeviceId)
        {

            try
            {
                string str = "sp_getGroupId";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var para = cmd.Parameters.Add("p_DeviceId", OracleDbType.Int32);
                        para.Value = DeviceId;
                        
                        OracleParameter oraP = new OracleParameter();
                        oraP.ParameterName = "p_GroupId";
                        oraP.OracleDbType = OracleDbType.Int32;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                        return Convert.ToInt32(cmd.Parameters["p_GroupId"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get GroupId ", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return -1;
        }
        public DataTable GetDevices(object groupid)
        {
            DataTable tb = new DataTable();
            try
            {
                string str = "sp_getDevices";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var para = cmd.Parameters.Add("p_GroupId", OracleDbType.Int32);
                        para.Value = groupid;
                        
                        OracleParameter oraP = new OracleParameter();
                        oraP.OracleDbType = OracleDbType.RefCursor;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get Devices", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }
        public DataTable GetGroups()
        {
            DataTable tb = new DataTable();
            try
            {
                string str = "spGetGroups";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        OracleParameter oraP = new OracleParameter();
                        oraP.OracleDbType = OracleDbType.RefCursor;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get Groups", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }
        public DataTable getNameGroup()
        {
            DataTable tb = new DataTable();
            try
            {
                string str = "sp_getnamegroup";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter oraP = new OracleParameter();
                        oraP.OracleDbType = OracleDbType.RefCursor;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get Group Name List", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }
        public void UpdateRole(RoleObj obj)
        {
            string str = "sp_updaterole";
            //p_name nvarchar2,
            //p_password nvarchar2,
            //p_roleid integer
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var para = cmd.Parameters.Add("p_id", OracleDbType.Varchar2);
                        para.Value = obj.id;
                        para = cmd.Parameters.Add("p_name", OracleDbType.Varchar2);
                        para.Value = obj.name;
                        para = cmd.Parameters.Add("p_perm1", OracleDbType.Int16);
                        para.Value = obj.permsyntime;
                        para = cmd.Parameters.Add("p_perm2", OracleDbType.Int16);
                        para.Value = obj.permseeHistory;
                        para = cmd.Parameters.Add("p_perm3", OracleDbType.Int16);
                        para.Value = obj.permCloseRecloser;
                        para = cmd.Parameters.Add("p_perm4", OracleDbType.Int16);
                        para.Value = obj.permopenRecloser;
                        para = cmd.Parameters.Add("p_perm5", OracleDbType.Int16);
                        para.Value = obj.permoperateTubu;
                        para = cmd.Parameters.Add("p_perm6", OracleDbType.Int16);
                        para.Value = obj.permUser;
                        para = cmd.Parameters.Add("p_perm7", OracleDbType.Int16);
                        para.Value = obj.permConfigCommon;
                        para = cmd.Parameters.Add("p_perm8", OracleDbType.Int16);
                        para.Value = obj.permNhom1;
                        para = cmd.Parameters.Add("p_perm9", OracleDbType.Int16);
                        para.Value = obj.permNhom2;
                        para = cmd.Parameters.Add("p_perm10", OracleDbType.Int16);
                        para.Value = obj.permNhom3;
                        para = cmd.Parameters.Add("p_perm11", OracleDbType.Int16);
                        para.Value = obj.permNhom4;
                        para = cmd.Parameters.Add("p_perm12", OracleDbType.Int16);
                        para.Value = obj.permNhom5;
                        para = cmd.Parameters.Add("p_perm13", OracleDbType.Int16);
                        para.Value = obj.permNhom6;
                        para = cmd.Parameters.Add("p_perm14", OracleDbType.Int16);
                        para.Value = obj.permNhom7;
                        para = cmd.Parameters.Add("p_perm15", OracleDbType.Int16);
                        para.Value = obj.permNhom8;
                        para = cmd.Parameters.Add("p_perm16", OracleDbType.Int16);
                        para.Value = obj.permNhom9;
                        para = cmd.Parameters.Add("p_perm17", OracleDbType.Int16);
                        para.Value = obj.permNhom10;
                        para = cmd.Parameters.Add("p_perm18", OracleDbType.Int16);
                        para.Value = obj.permNhom11;
                        para = cmd.Parameters.Add("p_perm19", OracleDbType.Int16);
                        para.Value = obj.permNhom12;
                        para = cmd.Parameters.Add("p_perm20", OracleDbType.Int16);
                        para.Value = obj.permNhom13;
                        para = cmd.Parameters.Add("p_perm21", OracleDbType.Int16);
                        para.Value = obj.permNhom14;
                        para = cmd.Parameters.Add("p_perm22", OracleDbType.Int16);
                        para.Value = obj.permNhom15;
                        para = cmd.Parameters.Add("p_perm23", OracleDbType.Int16);
                        para.Value = obj.permNhom16;
                        para = cmd.Parameters.Add("p_perm24", OracleDbType.Int16);
                        para.Value = obj.permNhom17;
                        para = cmd.Parameters.Add("p_perm25", OracleDbType.Int16);
                        para.Value = obj.permNhom18;
                        para = cmd.Parameters.Add("p_perm26", OracleDbType.Int16);
                        para.Value = obj.permNhom19;
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        cmd.ExecuteNonQuery();
                        sqlConnection.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Update role", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            
        }
        public DataTable getroles()
        {
            DataTable tb = new DataTable();
            try
            {
                string str = "sp_getroles";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter oraP = new OracleParameter();
                        oraP.OracleDbType = OracleDbType.RefCursor;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get User List", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }
        public DataTable getUserList()
        {
            
            DataTable tb = new DataTable();
            try
            {
                string str = "sp_getuserlist";
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter oraP = new OracleParameter();
                        oraP.OracleDbType = OracleDbType.RefCursor;
                        oraP.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(oraP);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Get User List", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
            return tb;
        }
        public void InsertUser(UserObj user)
        {
            string str = "sp_insertuser";
            //p_name nvarchar2,
            //p_password nvarchar2,
            //p_roleid integer
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var para = cmd.Parameters.Add("p_name", OracleDbType.Varchar2);
                        para.Value = user.name;
                        para = cmd.Parameters.Add("password", OracleDbType.Varchar2);
                        para.Value =user.password ;
                        para = cmd.Parameters.Add("p_roleid", OracleDbType.Int32);
                        para.Value = user.RoleId;
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        cmd.ExecuteNonQuery();
                        sqlConnection.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Insert User", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }

        }
        //        public void UpdateRole(RoleObj role)
        //        {
        //            sp_updaterole (
        //p_id integer,
        //p_name varchar2,
        //p_perm1 integer,
        //p_perm2 integer,
        //p_perm3 integer,
        //p_perm4 integer,
        //p_perm5 integer,
        //p_perm6 integer,
        //p_perm7 integer,
        //p_perm8 integer,
        //p_perm9 integer,
        //p_perm10 integer,
        //p_perm11 integer,
        //p_perm12 integer,
        //p_perm13 integer,
        //p_perm14 integer,
        //p_perm15 integer,
        //p_perm16 integer,
        //p_perm17 integer,
        //p_perm18 integer,
        //p_perm19 integer,
        //p_perm20 integer,
        //p_perm21 integer,
        //p_perm22 integer,
        //p_perm23 integer,
        //p_perm24 integer,
        //p_perm25 integer,
        //p_perm26 integer
        //)    
        //        }
        public void UpdateUser(UserObj user)
        {
            string str = "sp_updateuser";
            //p_id integer,
            //p_name varchar2,
            //p_password varchar2,
            //p_roleid integer
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var para = cmd.Parameters.Add("p_id", OracleDbType.Int32);
                        para.Value = user.id;
                        para = cmd.Parameters.Add("p_name", OracleDbType.Varchar2);
                        para.Value = user.name;
                        para = cmd.Parameters.Add("password", OracleDbType.Varchar2);
                        para.Value = user.password;
                        para = cmd.Parameters.Add("p_roleid", OracleDbType.Int32);
                        para.Value = user.RoleId;
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        cmd.ExecuteNonQuery();
                        sqlConnection.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Update User", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }

        }
        public void DeleteUser(int userId)
        {
            string str = "sp_deleteuser";//p_id integer
            try{
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var para = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        para.Value = userId;
                        
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("Delete User", ex.ToString());
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public UserObj getUserLogin(string username, string encryptpassword)
        {
            string str = "sp_getuser";
            using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                // Need to define cmdText.
                DataTable tb = new DataTable();
                using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var para = cmd.Parameters.Add("p_username", OracleDbType.Varchar2,20);
                    para.Value = username;
                    para = cmd.Parameters.Add("p_password", OracleDbType.Varchar2, 50);
                    para.Value = encryptpassword;
                    OracleParameter oraP = new OracleParameter();
                    oraP.OracleDbType = OracleDbType.RefCursor;
                    oraP.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(oraP);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    da.Fill(tb);
                    sqlConnection.Close();
                    if (tb.Rows.Count > 0)
                    {
                        UserObj user = new UserObj();
                        user.id = Convert.ToInt32(tb.Rows[0]["Id"]);
                        ////bool logged = Convert.ToBoolean(ret.Rows[0][CONST_LOGGED]);
                        //user.perm1 = Convert.ToUInt16(ret.Rows[0][CONST_PERM1]);
                        //perm1, perm2, perm3, perm4, perm5, perm6, perm7, perm8, perm9,perm10
                        user.permsyntime = Convert.ToUInt16(tb.Rows[0]["perm1"]);
                        user.permseeHistory = Convert.ToUInt16(tb.Rows[0]["perm2"]);
                        user.permCloseRecloser = Convert.ToUInt16(tb.Rows[0]["perm3"]);
                        user.permopenRecloser = Convert.ToUInt16(tb.Rows[0]["perm4"]);
                        user.permoperateTubu = Convert.ToUInt16(tb.Rows[0]["perm5"]);
                        user.permUser = Convert.ToUInt16(tb.Rows[0]["perm6"]);
                        user.permConfigCommon = Convert.ToUInt16(tb.Rows[0]["perm7"]);
                        user.permNhom1 = Convert.ToUInt16(tb.Rows[0]["perm8"]); //permNhom1 : permUser
                        user.permNhom2 = Convert.ToUInt16(tb.Rows[0]["perm9"]); //permConfigCommon -> permNhom2
                        user.permNhom3 = Convert.ToUInt16(tb.Rows[0]["perm10"]);
                        user.permNhom4 = Convert.ToUInt16(tb.Rows[0]["perm11"]);
                        user.permNhom5 = Convert.ToUInt16(tb.Rows[0]["perm12"]);
                        user.permNhom6 = Convert.ToUInt16(tb.Rows[0]["perm13"]);
                        user.permNhom7 = Convert.ToUInt16(tb.Rows[0]["perm14"]);
                        user.permNhom8 = Convert.ToUInt16(tb.Rows[0]["perm15"]);
                        user.permNhom9 = Convert.ToUInt16(tb.Rows[0]["perm16"]);
                        user.permNhom10 = Convert.ToUInt16(tb.Rows[0]["perm17"]);
                        user.permNhom11 = Convert.ToUInt16(tb.Rows[0]["perm18"]);
                        user.permNhom12 = Convert.ToUInt16(tb.Rows[0]["perm19"]);
                        user.permNhom13 = Convert.ToUInt16(tb.Rows[0]["perm20"]);
                        user.permNhom14 = Convert.ToUInt16(tb.Rows[0]["perm21"]);
                        user.permNhom15 = Convert.ToUInt16(tb.Rows[0]["perm22"]);
                        user.permNhom16 = Convert.ToUInt16(tb.Rows[0]["perm23"]);
                        user.permNhom17 = Convert.ToUInt16(tb.Rows[0]["perm24"]);
                        user.permNhom18 = Convert.ToUInt16(tb.Rows[0]["perm25"]);
                        user.permNhom19 = Convert.ToUInt16(tb.Rows[0]["perm26"]);
                        //user.permConfigDatabase = Convert.ToUInt16(dr[CONST_PERM10]);
                        //user.lastlogin = Convert.ToDateTime(dr[CONST_LASTLOGIN]);
                        //user.computer = dr[CONST_COMP].ToString();
                        user.RoleName = tb.Rows[0]["rolename"].ToString();
                        user.RoleId = Convert.ToInt32(tb.Rows[0]["roleid"]);
                        //user.fullname = tb.Rows[0]["fullname"].ToString();
                        user.name = username;
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            
        }
        public UserObj getUserInfo(int Userid)
        {
            string str = "sp_getuserviaId";
            using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                // Need to define cmdText.
                DataTable tb = new DataTable();
                using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var para = cmd.Parameters.Add("p_id", OracleDbType.Int32);
                    para.Value = Userid;
                    
                    OracleParameter oraP = new OracleParameter();
                    oraP.OracleDbType = OracleDbType.RefCursor;
                    oraP.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(oraP);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    da.Fill(tb);
                    sqlConnection.Close();
                    if (tb.Rows.Count > 0)
                    {
                        UserObj user = new UserObj();
                        user.id = Convert.ToInt32(tb.Rows[0]["Id"]);
                        ////bool logged = Convert.ToBoolean(ret.Rows[0][CONST_LOGGED]);
                        //user.perm1 = Convert.ToUInt16(ret.Rows[0][CONST_PERM1]);
                        //perm1, perm2, perm3, perm4, perm5, perm6, perm7, perm8, perm9,perm10
                        user.permsyntime = Convert.ToUInt16(tb.Rows[0]["perm1"]);
                        user.permseeHistory = Convert.ToUInt16(tb.Rows[0]["perm2"]);
                        user.permCloseRecloser = Convert.ToUInt16(tb.Rows[0]["perm3"]);
                        user.permopenRecloser = Convert.ToUInt16(tb.Rows[0]["perm4"]);
                        user.permoperateTubu = Convert.ToUInt16(tb.Rows[0]["perm5"]);
                        user.permUser = Convert.ToUInt16(tb.Rows[0]["perm6"]);
                        user.permConfigCommon = Convert.ToUInt16(tb.Rows[0]["perm7"]);
                        user.permNhom1 = Convert.ToUInt16(tb.Rows[0]["perm8"]); //permNhom1 : permUser
                        user.permNhom2 = Convert.ToUInt16(tb.Rows[0]["perm9"]); //permConfigCommon -> permNhom2
                        user.permNhom3 = Convert.ToUInt16(tb.Rows[0]["perm10"]);
                        user.permNhom4 = Convert.ToUInt16(tb.Rows[0]["perm11"]);
                        user.permNhom5 = Convert.ToUInt16(tb.Rows[0]["perm12"]);
                        user.permNhom6 = Convert.ToUInt16(tb.Rows[0]["perm13"]);
                        user.permNhom7 = Convert.ToUInt16(tb.Rows[0]["perm14"]);
                        user.permNhom8 = Convert.ToUInt16(tb.Rows[0]["perm15"]);
                        user.permNhom9 = Convert.ToUInt16(tb.Rows[0]["perm16"]);
                        user.permNhom10 = Convert.ToUInt16(tb.Rows[0]["perm17"]);
                        user.permNhom11 = Convert.ToUInt16(tb.Rows[0]["perm18"]);
                        user.permNhom12 = Convert.ToUInt16(tb.Rows[0]["perm19"]);
                        user.permNhom13 = Convert.ToUInt16(tb.Rows[0]["perm20"]);
                        user.permNhom14 = Convert.ToUInt16(tb.Rows[0]["perm21"]);
                        user.permNhom15 = Convert.ToUInt16(tb.Rows[0]["perm22"]);
                        user.permNhom16 = Convert.ToUInt16(tb.Rows[0]["perm23"]);
                        user.permNhom17 = Convert.ToUInt16(tb.Rows[0]["perm24"]);
                        user.permNhom18 = Convert.ToUInt16(tb.Rows[0]["perm25"]);
                        user.permNhom19 = Convert.ToUInt16(tb.Rows[0]["perm26"]);
                        //user.permConfigDatabase = Convert.ToUInt16(dr[CONST_PERM10]);
                        //user.lastlogin = Convert.ToDateTime(dr[CONST_LASTLOGIN]);
                        //user.computer = dr[CONST_COMP].ToString();
                        
                        user.name = tb.Rows[0]["username"].ToString(); ;
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
        public DataTable SearchRecloser351(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
               /* //DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18
                string str = " Select DeviceId, Date, Alert, [MW_A] ,	[MW_B] ,	[MW_C] ,	[MW_3P] ," +
            "[Q_MVAR_A] ,	[Q_MVAR_B] ,	[Q_MVAR_C] ,	[Q_MVAR_3P] ,	[PF_A] ,	[PF_B] ,	[PF_C] ,	[PF_3P] ,	[voltsValue_MAG_A] ,	" +
            "[voltsValue_MAG_B] ,	[voltsValue_MAG_C] ,	[voltsValue_MAG_S] ,	[vang_A] ,	[vang_B] ,	[vang_C] ,	[vang_S],	[imag_A],	" +
            "[imag_B],	[imag_N],	[imag_G],	[imag_C] " +
                " From RecloserSel WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To  ";*/
                string str = "sp_searchRecloser351R";
                return ExecSearch(id, from, to, str);
                
            }
            catch (Exception ex)
            {
                LogService.WriteError("Search Recloser351", ex.Message);
            }
            finally
            {
                
            }
            return tb;
        }
        public DataTable SearchElster(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
                string str = "sp_searchElster";
                return ExecSearch(id, from, to, str);
            }
            catch (Exception ex)
            {
                LogService.WriteError("Search Elster", ex.Message);
            }
            finally
            {

            }
            return tb;
        }
        public DataTable SearchRecloserADVC(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
              /*  //DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18
                string str = " Select [DeviceId] , 	[Date] ,	[Alert] ,	[Current_IA] ,	[Current_IB] ,	[Current_IC] , " +
	"[Current_IE] ,[ApparentPower],	[Apparent_S_A] ,	[Apparent_S_B] ,	[Apparent_S_C] ,[RealPower],	[Real_P_A] ,	[Real_P_B] ,	[Real_P_C] ," +
	"[ReactivePower],[Reactive_Q_A] ,	[Reactive_Q_B] ,	[Reactive_Q_C] ,[Cosphi],	[Cosphi_A] ,	[Cosphi_B] ,	[Cosphi_C] ,	[Total] ,"+
	"[Forward] ,	[Reverse] ,	[BatterryVol] ,	[VA_GND_src] ,	[VB_GND_src] ,	[VC_GND_src] ,	[VA_GND_load] ,	[VB_GND_load] ,"+
	"[VC_GND_load] ,	[VA_B_src] ,	[VB_C_src] ,	[VC_A_src] ,	[VA_B_load] ,	[VB_C_load] ,	[VC_A_load],[Operations]  " +
     " From RecloserADVC WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To  ";*/
                string str = "sp_searchRecloserADVC";
                return ExecSearch(id, from, to, str);
            }
            catch (Exception ex)
            {
                LogService.WriteError("Search ADVC", ex.Message);
            }
            finally
            {

            }
            return tb;
        }
        public DataTable SearchRecloserVP(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
              /*  //DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18
                string str = " Select [DeviceId] , 	[Date] ,	[Alert] ,	[Current_IA] ,	[Current_IB] ,	[Current_IC] , " +
    "[Current_IE] ,	[Apparent_S_A] ,	[Apparent_S_B] ,	[Apparent_S_C] ,	[Real_P_A] ,	[Real_P_B] ,	[Real_P_C] ," +
    "[Reactive_Q_A] ,	[Reactive_Q_B] ,	[Reactive_Q_C] ,	[Cosphi_A] ,	[Cosphi_B] ,	[Cosphi_C] ,	[Total] ," +
    "[Forward] ,	[Reverse] ,	[BatterryVol] ,	[VA_GND_src] ,	[VB_GND_src] ,	[VC_GND_src] ,	[VA_GND_load] ,	[VB_GND_load] ," +
    "[VC_GND_load] ,	[VA_B_src] ,	[VB_C_src] ,	[VC_A_src] ,	[VA_B_load] ,	[VB_C_load] ,	[VC_A_load],[Operations]  " +
     " From RecloserVP WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To  ";*/
                string str = "sp_searchRecloserVP";
                return ExecSearch(id, from, to, str);

            }
            catch (Exception ex)
            {
                LogService.WriteError("Search RecloserVP", ex.Message);
            }
            finally
            {

            }
            return tb;
        }

        public DataTable SearchTuBu(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
                
               string str = "sp_searchTubu";
                   //"Select  Name,Location,OperationTime,Case Operation when 1 then 'Open' else 'Close' end as Opt, Case Auto when 1 then 'Auto' When 0 then 'Click' else 'Manual' End as Mod From TuBu WHERE DeviceId = @DeviceId AND OperationTime >= @From AND OperationTime <= @To ";
               return ExecSearch(id, from, to, str);
                
            }
            catch (Exception ex)
            {
                LogService.WriteError("Search Tubu", ex.Message);
            }
            finally
            {
                
            }
            return tb;
        }
        public DataTable SearchLBS(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {

                string str = "sp_searchlbs";
                //"Select  Name,Location,OperationTime,Case Operation when 1 then 'Open' else 'Close' end as Opt, Case Auto when 1 then 'Auto' When 0 then 'Click' else 'Manual' End as Mod From TuBu WHERE DeviceId = @DeviceId AND OperationTime >= @From AND OperationTime <= @To ";
                return ExecSearch(id, from, to, str);

            }
            catch (Exception ex)
            {
                LogService.WriteError("Search LBS", ex.Message);
            }
            finally
            {

            }
            return tb;
        }
        public void SaveRecloserVP(RecloserVP obj)
        {
            // VP sua lai connectionstring

            // if (obj.LastUpdated == DateTime.MinValue) return;

            string str = string.Format("INSERT INTO RecloserVP (DeviceId, Date, Alert, " +
               " Current_IA ,	Current_IB ,	Current_IC ,	Current_IE ,	Apparent_S_A ,	Apparent_S_B ," +
	" Apparent_S_C ,	Real_P_A ,	Real_P_B ,	Real_P_C ,	Reactive_Q_A ,	Reactive_Q_B ,	Reactive_Q_C ," +
	" Cosphi_A ,	Cosphi_B ,Cosphi_C ,	Total ,	Forward ,	Reverse ,	BatterryVol ,	VA_GND_src ,	VB_GND_src ," +
	" VC_GND_src ,	VA_GND_load ,	VB_GND_load ,	VC_GND_load ,	VA_B_src ,	VB_C_src ,	VC_A_src ,	VA_B_load ,	VB_C_load ," +
	" VC_A_load, Operations )" +
             " VALUES (@DeviceId,  @Date, @Alert, " + 
             " @Current_IA ,	@Current_IB ,	@Current_IC ,	@Current_IE ,	@Apparent_S_A ,	@Apparent_S_B , " +
	" @Apparent_S_C ,	@Real_P_A ,	@Real_P_B ,	@Real_P_C ,	@Reactive_Q_A ,	@Reactive_Q_B ,	@Reactive_Q_C ,	@Cosphi_A ," +
	" @Cosphi_B ,	@Cosphi_C ,	@Total ,	@Forward ,	@Reverse ,	@BatterryVol ,	@VA_GND_src ,	@VB_GND_src ," +
	" @VC_GND_src ,	@VA_GND_load ,	@VB_GND_load ,	@VC_GND_load ,	@VA_B_src ,	@VB_C_src ,	@VC_A_src ,	@VA_B_load ,	@VB_C_load ," +
	" @VC_A_load,@Operations)");
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 10);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("p_DateRec", OracleDbType.Date);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("p_Alert", OracleDbType.Int16);
                        param.Value = obj.Alert;


                        param = cmd.Parameters.Add("p_Current_IA", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Current_IB", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Current_IC", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Current_IE", OracleDbType.Varchar2, 10);





                        param = cmd.Parameters.Add("p_Apparent_S_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Apparent_S_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Apparent_S_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Real_P_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Real_P_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Real_P_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reactive_Q_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reactive_Q_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reactive_Q_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Cosphi_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Cosphi_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Cosphi_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Total", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Forward", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reverse", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_BatterryVol", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_GND_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_GND_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_GND_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_GND_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_GND_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_GND_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_B_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_C_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_A_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_B_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_C_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_A_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Operations", OracleDbType.Varchar2, 10);
                        cmd.Parameters["p_Current_IA"].Value = obj.Current_IA.ToString();
                        cmd.Parameters["p_Current_IB"].Value = obj.Current_IB.ToString();
                        cmd.Parameters["p_Current_IC"].Value = obj.Current_IC.ToString();
                        cmd.Parameters["p_Current_IE"].Value = obj.Current_IE.ToString();
                        cmd.Parameters["p_Apparent_S_A"].Value = obj.Apparent_S_A.ToString();
                        cmd.Parameters["p_Apparent_S_B"].Value = obj.Apparent_S_B.ToString();
                        cmd.Parameters["p_Apparent_S_C"].Value = obj.Apparent_S_C.ToString();
                        cmd.Parameters["p_Real_P_A"].Value = obj.Real_P_A.ToString();
                        cmd.Parameters["p_Real_P_B"].Value = obj.Real_P_B.ToString();
                        cmd.Parameters["p_Real_P_C"].Value = obj.Real_P_C.ToString();
                        cmd.Parameters["p_Reactive_Q_A"].Value = obj.Reactive_Q_A.ToString();
                        cmd.Parameters["p_Reactive_Q_B"].Value = obj.Reactive_Q_B.ToString();
                        cmd.Parameters["p_Reactive_Q_C"].Value = obj.Reactive_Q_C.ToString();
                        cmd.Parameters["p_Cosphi_A"].Value = obj.Cosphi_A.ToString();
                        cmd.Parameters["p_Cosphi_B"].Value = obj.Cosphi_B.ToString();
                        cmd.Parameters["p_Cosphi_C"].Value = obj.Cosphi_C.ToString();
                        cmd.Parameters["p_Total"].Value = obj.Total.ToString();
                        cmd.Parameters["p_Forward"].Value = obj.Forward.ToString();
                        cmd.Parameters["p_Reverse"].Value = obj.Reverse.ToString();
                        cmd.Parameters["p_BatterryVol"].Value = obj.BatterryVol.ToString();
                        cmd.Parameters["p_VA_GND_src"].Value = obj.VA_GND_src.ToString();
                        cmd.Parameters["p_VB_GND_src"].Value = obj.VB_GND_src.ToString();
                        cmd.Parameters["p_VC_GND_src"].Value = obj.VC_GND_src.ToString();
                        cmd.Parameters["p_VA_GND_load"].Value = obj.VA_GND_load.ToString();
                        cmd.Parameters["p_VB_GND_load"].Value = obj.VB_GND_load.ToString();
                        cmd.Parameters["p_VC_GND_load"].Value = obj.VC_GND_load.ToString();
                        cmd.Parameters["p_VA_B_src"].Value = obj.VA_B_src.ToString();
                        cmd.Parameters["p_VB_C_src"].Value = obj.VB_C_src.ToString();
                        cmd.Parameters["p_VC_A_src"].Value = obj.VC_A_src.ToString();
                        cmd.Parameters["p_VA_B_load"].Value = obj.VA_B_load.ToString();
                        cmd.Parameters["p_VB_C_load"].Value = obj.VB_C_load.ToString();
                        cmd.Parameters["p_VC_A_load"].Value = obj.VC_A_load.ToString();
                        cmd.Parameters["p_Operations"].Value = obj.Operations.ToString();

                        //cmd.Parameters["p_Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["p_Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["p_Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
                //SqlConnection sqlConn = new SqlConnection(_connectionString);





            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveRecloserVP", ex.Message + " " + obj.LastUpdated);
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
    

        public void SaveRecloserADVC(RecloserADVC obj)
        {
            // VP sua lai connectionstring

            // if (obj.LastUpdated == DateTime.MinValue) return;
            //them ApparentPower, RealPower, ReactivePower, PowerFactor
            /*string str = string.Format("INSERT INTO RecloserADVC (DeviceId, Date, Alert, " +
               " Current_IA ,	Current_IB ,	Current_IC ,	Current_IE ,ApparentPower,	Apparent_S_A ,	Apparent_S_B ," +
    " Apparent_S_C ,RealPower,	Real_P_A ,	Real_P_B ,	Real_P_C ,	ReactivePower,Reactive_Q_A ,	Reactive_Q_B ,	Reactive_Q_C ," +
    "Cosphi, Cosphi_A ,	Cosphi_B ,Cosphi_C ,	Total ,	Forward ,	Reverse ,	BatterryVol ,	VA_GND_src ,	VB_GND_src ," +
	" VC_GND_src ,	VA_GND_load ,	VB_GND_load ,	VC_GND_load ,	VA_B_src ,	VB_C_src ,	VC_A_src ,	VA_B_load ,	VB_C_load ," +
	" VC_A_load, Operations )" +
             " VALUES (@DeviceId,  @Date, @Alert, " +
             " @Current_IA ,	@Current_IB ,	@Current_IC ,	@Current_IE ,@ApparentPower,	@Apparent_S_A ,	@Apparent_S_B , " +
    " @Apparent_S_C ,	@RealPower,@Real_P_A ,	@Real_P_B ,	@Real_P_C ,@ReactivePower,	@Reactive_Q_A ,	@Reactive_Q_B ,	@Reactive_Q_C ,	@PowerFactor,@Cosphi_A ," +
	" @Cosphi_B ,	@Cosphi_C ,	@Total ,	@Forward ,	@Reverse ,	@BatterryVol ,	@VA_GND_src ,	@VB_GND_src ," +
	" @VC_GND_src ,	@VA_GND_load ,	@VB_GND_load ,	@VC_GND_load ,	@VA_B_src ,	@VB_C_src ,	@VC_A_src ,	@VA_B_load ,	@VB_C_load ," +
	" @VC_A_load,@Operations)");*/
            string str = "sp_saverecloseradvc";
            try
            {
                using (OracleConnection sqlConnection = new OracleConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (OracleCommand cmd = new OracleCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("p_Id", OracleDbType.Int32);
                        param.Value = obj.Id;
                        /*param = cmd.Parameters.Add("p_Type", OracleDbType.Varchar2, 10);
                        param.Value = obj.DeviceType.ToString();*/
                        param = cmd.Parameters.Add("p_DateRec", OracleDbType.Date);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("p_Alert", OracleDbType.Int16);
                        param.Value = obj.Alert;


                        param = cmd.Parameters.Add("p_Current_IA", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Current_IB", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Current_IC", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Current_IE", OracleDbType.Varchar2, 10);
                        //ApparentPower, RealPower, ReactivePower, PowerFactor
                        param = cmd.Parameters.Add("p_ApparentPower", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Apparent_S_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Apparent_S_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Apparent_S_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_RealPower", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Real_P_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Real_P_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Real_P_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_ReactivePower", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reactive_Q_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reactive_Q_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reactive_Q_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_PowerFactor", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Cosphi_A", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Cosphi_B", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Cosphi_C", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Total", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Forward", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Reverse", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_BatterryVol", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_GND_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_GND_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_GND_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_GND_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_GND_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_GND_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_B_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_C_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_A_src", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VA_B_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VB_C_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_VC_A_load", OracleDbType.Varchar2, 10);
                        param = cmd.Parameters.Add("p_Operations", OracleDbType.Varchar2, 10);
                        cmd.Parameters["p_Current_IA"].Value = obj.Current_IA.ToString();
                        cmd.Parameters["p_Current_IB"].Value = obj.Current_IB.ToString();
                        cmd.Parameters["p_Current_IC"].Value = obj.Current_IC.ToString();
                        cmd.Parameters["p_Current_IE"].Value = obj.Current_IE.ToString();
                        cmd.Parameters["p_Apparent_S_A"].Value = obj.Apparent_S_A.ToString();
                        cmd.Parameters["p_Apparent_S_B"].Value = obj.Apparent_S_B.ToString();
                        cmd.Parameters["p_Apparent_S_C"].Value = obj.Apparent_S_C.ToString();
                        cmd.Parameters["p_Real_P_A"].Value = obj.Real_P_A.ToString();
                        cmd.Parameters["p_Real_P_B"].Value = obj.Real_P_B.ToString();
                        cmd.Parameters["p_Real_P_C"].Value = obj.Real_P_C.ToString();
                        cmd.Parameters["p_Reactive_Q_A"].Value = obj.Reactive_Q_A.ToString();
                        cmd.Parameters["p_Reactive_Q_B"].Value = obj.Reactive_Q_B.ToString();
                        cmd.Parameters["p_Reactive_Q_C"].Value = obj.Reactive_Q_C.ToString();
                        cmd.Parameters["p_Cosphi_A"].Value = obj.Cosphi_A.ToString();
                        cmd.Parameters["p_Cosphi_B"].Value = obj.Cosphi_B.ToString();
                        cmd.Parameters["p_Cosphi_C"].Value = obj.Cosphi_C.ToString();
                        cmd.Parameters["p_Total"].Value = obj.Total.ToString();
                        cmd.Parameters["p_Forward"].Value = obj.Forward.ToString();
                        cmd.Parameters["p_Reverse"].Value = obj.Reverse.ToString();
                        cmd.Parameters["p_BatterryVol"].Value = obj.BatterryVol.ToString();
                        cmd.Parameters["p_VA_GND_src"].Value = obj.VA_GND_src.ToString();
                        cmd.Parameters["p_VB_GND_src"].Value = obj.VB_GND_src.ToString();
                        cmd.Parameters["p_VC_GND_src"].Value = obj.VC_GND_src.ToString();
                        cmd.Parameters["p_VA_GND_load"].Value = obj.VA_GND_load.ToString();
                        cmd.Parameters["p_VB_GND_load"].Value = obj.VB_GND_load.ToString();
                        cmd.Parameters["p_VC_GND_load"].Value = obj.VC_GND_load.ToString();
                        cmd.Parameters["p_VA_B_src"].Value = obj.VA_B_src.ToString();
                        cmd.Parameters["p_VB_C_src"].Value = obj.VB_C_src.ToString();
                        cmd.Parameters["p_VC_A_src"].Value = obj.VC_A_src.ToString();
                        cmd.Parameters["p_VA_B_load"].Value = obj.VA_B_load.ToString();
                        cmd.Parameters["p_VB_C_load"].Value = obj.VB_C_load.ToString();
                        cmd.Parameters["p_VC_A_load"].Value = obj.VC_A_load.ToString();
                        cmd.Parameters["p_Operations"].Value = obj.Operations.ToString();
                        //ApparentPower, RealPower, ReactivePower, PowerFactor
                        cmd.Parameters["p_ApparentPower"].Value = obj.ApparentPower.ToString();
                        cmd.Parameters["p_RealPower"].Value = obj.RealPower.ToString();
                        cmd.Parameters["p_ReactivePower"].Value = obj.ReactivePower.ToString();
                        cmd.Parameters["p_PowerFactor"].Value = obj.PowerFactor.ToString();
                        //cmd.Parameters["p_Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["p_Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["p_Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.BindByName = true;
                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
                //SqlConnection sqlConn = new SqlConnection(_connectionString);





            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveRecloserADVC", ex.Message + " " + obj.LastUpdated);
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
       
    }
}
