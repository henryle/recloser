using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using RecloserAcq.Device;
using System.Data;
using System.IO;
using System.Windows.Forms;
using FA_Accounting.Common;
namespace RecloserAcq.DAL
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

            
        

        private string _connectionString = "Data Source=(local);Initial Catalog=Recloser;User ID=sa;Password=password00";
        //SqlConnection _sqlConn;
        public DBController()
        {
            
            //_sqlConn = new SqlConnection(connectionString);
            
        }
     /*   public void WriteRow(RecloserBase obj)
        {

            string dvpath = RecloserAcq.Properties.Settings.Default.dvpath.Trim();
            if (dvpath.LastIndexOf('\\') != dvpath.Length - 1)
            {
                dvpath += "\\";
            }
            string filePath = RecloserAcq.Properties.Settings.Default.dvpath + obj.Port + ".csv";
            string strFields = "TestField\r\n";
            string strvalues = "TestValue";
            try
            {
                strFields = "DeviceId,Type,Date,Alert,Amp12,Amp34,Amp56,AmpEarth,Operations,Battery_1,Battery_2,Status_Open,Status_Close,Status_Lockout";
                strvalues = obj.Id.ToString() + "," +
                    obj.DeviceType.ToString() + "," +
                    obj.LastUpdated.ToString() + "," +
                    obj.AlertVal.ToString() + "," +
                    obj.Amp12.ToString() + "," +
                    obj.Amp34.ToString() + "," +
                    obj.Amp56.ToString() + "," +
                    obj.AmpEarth.ToString() + "," +
                    obj.Operations.ToString() + "," +
                    (obj.Battery_1 == null ? " " : obj.Battery_1.ToString()) + "," +
                    (obj.Battery_2 == null ? " " : obj.Battery_2.ToString()) + "," +
                    obj.Status_Open.ToString() + "," +
                    obj.Status_Close.ToString() + "," +
                    obj.Status_Lockout.ToString();
                if (obj is CooperFXB)
                {
                    strFields = strFields + ",Target12,Target34,Target56,EarthTarget,Status_Target12,Status_Target34,Status_Target56,Status_EarthTarget\r\n";
                    var coo = obj as CooperFXB;
                    strvalues = strvalues + "," + coo.Target12.ToString() + "," +
                    coo.Target34.ToString() + "," +
                    coo.Target56.ToString() + "," +
                    coo.EarthTarget.ToString() + "," +
                    coo.Status_Target12.ToString() + "," +
                    coo.Status_Target34.ToString() + "," +
                    coo.Status_Target56.ToString() + "," +
                    coo.Status_EarthTarget.ToString();
                }
                else if (obj is Nulec)
                {
                    strFields = strFields + ",ApparentPower,ReactivePower,RealPower,PowerFactor,DeviceTime\r\n";
                    var nuc = obj as Nulec;
                    strvalues = strvalues + "," + nuc.ApparentPower.ToString() + "," +
                    nuc.ReactivePower.ToString() + "," +
                    nuc.RealPower.ToString() + "," +
                    nuc.PowerFactor.ToString() + "," +
                    ((nuc.DeviceTime.HasValue && nuc.DeviceTime.Value > DateTime.MinValue) ? nuc.DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "");
                }
                try
                {
                    using (FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] FieldLine = Encoding.ASCII.GetBytes(strFields);
                        byte[] ValueLine = Encoding.ASCII.GetBytes(strvalues);
                        fs.Write(FieldLine, 0, FieldLine.Length);
                        fs.Write(ValueLine, 0, ValueLine.Length);
                        fs.Close();
                    }
                }
                catch (Exception e)
                {
                    LogService.WriteError("WriteRow", e.Message);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("WriteRow", ex.Message);
            }
        }*/


        public void SaveReceive(DateTime date, int port, string data)
        {
            string str = string.Format("INSERT INTO ReceiveLog(Date, Port, Data) " +
               "VALUES(@Date, @Port, @Data)");
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            
            
            try
            {
                
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                    
                    using (SqlCommand sqlCommand = new SqlCommand(str, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;

                        var param = sqlCommand.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = date;
                        param = sqlCommand.Parameters.Add("@Port", System.Data.SqlDbType.Int);
                        param.Value = port;
                        param = sqlCommand.Parameters.Add("@Data", System.Data.SqlDbType.VarChar, 500);
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

        public void SaveRecloser(RecloserBase obj)
        {
            if (obj.LastUpdated == DateTime.MinValue) return;

            //string str = string.Format("INSERT INTO StatusInfo(DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18) " +
            //   "VALUES(@DeviceId, @Type, @Date, @Alert, @Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10, @Field11, @Field12, @Field13, @Field14, @Field15, @Field16, @Field17, @Field18)");
            string str = "InsertStatusInfo";
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 50);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("@Alert", System.Data.SqlDbType.Bit);
                        param.Value = obj.Alert;

                        param = cmd.Parameters.Add("@Field1", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field2", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field3", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field4", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field5", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field6", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field7", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field8", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field9", System.Data.SqlDbType.VarChar, 50);

                        param = cmd.Parameters.Add("@Field10", System.Data.SqlDbType.VarChar, 50);


                        cmd.Parameters["@Field1"].Value = obj.Amp12.ToString();
                        cmd.Parameters["@Field2"].Value = obj.Amp34.ToString();
                        cmd.Parameters["@Field3"].Value = obj.Amp56.ToString();
                        cmd.Parameters["@Field4"].Value = obj.AmpEarth.ToString();
                        cmd.Parameters["@Field5"].Value = obj.Operations.ToString();
                        cmd.Parameters["@Field6"].Value = obj.Battery_1 ?? "";
                        cmd.Parameters["@Field7"].Value = obj.Battery_2 ?? "";
                        cmd.Parameters["@Field8"].Value = obj.Status_Open.ToString();
                        cmd.Parameters["@Field9"].Value = obj.Status_Close.ToString();
                        cmd.Parameters["@Field10"].Value = obj.Status_Lockout.ToString();

                        if (obj is CooperFXB)
                        {
                            var coo = obj as CooperFXB;
                            param = cmd.Parameters.Add("@Field11", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.Target12.ToString();
                            param = cmd.Parameters.Add("@Field12", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.Target34.ToString();
                            param = cmd.Parameters.Add("@Field13", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.Target56.ToString();
                            param = cmd.Parameters.Add("@Field14", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.EarthTarget.ToString();
                            param = cmd.Parameters.Add("@Field15", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.Status_Target12.ToString();
                            param = cmd.Parameters.Add("@Field16", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.Status_Target34.ToString();
                            param = cmd.Parameters.Add("@Field17", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.Status_Target56.ToString();
                            param = cmd.Parameters.Add("@Field18", System.Data.SqlDbType.VarChar, 50);
                            param.Value = coo.Status_EarthTarget.ToString();
                        }
                        else if (obj is Nulec)
                        {
                            var nuc = obj as Nulec;
                            param = cmd.Parameters.Add("@Field11", System.Data.SqlDbType.VarChar, 50);
                            param.Value = nuc.ApparentPower.ToString();
                            param = cmd.Parameters.Add("@Field12", System.Data.SqlDbType.VarChar, 50);
                            param.Value = nuc.ReactivePower.ToString();
                            param = cmd.Parameters.Add("@Field13", System.Data.SqlDbType.VarChar, 50);
                            param.Value = nuc.RealPower.ToString();
                            param = cmd.Parameters.Add("@Field14", System.Data.SqlDbType.VarChar, 50);
                            param.Value = nuc.PowerFactor.ToString();
                            param = cmd.Parameters.Add("@Field15", System.Data.SqlDbType.VarChar, 50);

                            param.Value = ((nuc.DeviceTime.HasValue && nuc.DeviceTime.Value > DateTime.MinValue) ? nuc.DeviceTime.Value.ToString("dd/MM/yyyy HH:mm:ss.fff") : "");

                            param = cmd.Parameters.Add("@Field16", System.Data.SqlDbType.VarChar, 50);
                            param.Value = "False";
                            param = cmd.Parameters.Add("@Field17", System.Data.SqlDbType.VarChar, 50);
                            param.Value = "False";
                            param = cmd.Parameters.Add("@Field18", System.Data.SqlDbType.VarChar, 50);
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
                LogService.WriteError("Save Datanew" + obj.Name, ex.Message);
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void SaveRecloserDisCon(RecloserBase obj)
        {


            string str = string.Format("INSERT INTO StatusInfo(DeviceId, Type, Date, Alert, Status) " +
               "VALUES(@DeviceId, @Type, @Date, @Alert, @Status)");
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 50);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("@Alert", System.Data.SqlDbType.Bit);
                        param.Value = obj.Alert;
                        param = cmd.Parameters.Add("@Status", System.Data.SqlDbType.VarChar, 50);
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

            string str = string.Format("INSERT INTO RecloserSel(DeviceId, Date, Alert, [MW_A],	[MW_B],	[MW_C],	[MW_3P],"+	
            "[Q_MVAR_A],	[Q_MVAR_B],	[Q_MVAR_C],	[Q_MVAR_3P],	[PF_A],	[PF_B],	[PF_C],	[PF_3P],	[voltsValue_MAG_A],	" + 
            "[voltsValue_MAG_B],	[voltsValue_MAG_C],	[voltsValue_MAG_S],	[vang_A],	[vang_B],	[vang_C],	[vang_S],	[imag_A],	"+
            "[imag_B],	[imag_N],	[imag_G],	[imag_C])" +
             " VALUES (@DeviceId,  @Date, @Alert, @MW_A,	@MW_B,	@MW_C,	@MW_3P,	@Q_MVAR_A,"+	
             "@Q_MVAR_B,	@Q_MVAR_C,	@Q_MVAR_3P,	@PF_A,	@PF_B,	@PF_C,	@PF_3P,	@voltsValue_MAG_A,"+
             "@voltsValue_MAG_B,	@voltsValue_MAG_C,	@voltsValue_MAG_S,	@vang_A,	@vang_B,	@vang_C,	"+
             "@vang_S,	@imag_A,	@imag_B,	@imag_N,	@imag_G,	@imag_C)");
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                    
                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 10);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("@Alert", System.Data.SqlDbType.Bit);
                        param.Value = obj.Alert;

                        param = cmd.Parameters.Add("@MW_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@MW_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@MW_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@MW_3P", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Q_MVAR_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Q_MVAR_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Q_MVAR_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Q_MVAR_3P", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_3P", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@voltsValue_MAG_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@voltsValue_MAG_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@voltsValue_MAG_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@voltsValue_MAG_S", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@vang_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@vang_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@vang_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@vang_S", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@imag_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@imag_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@imag_N", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@imag_G", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@imag_C", System.Data.SqlDbType.VarChar, 10);

                        cmd.Parameters["@MW_A"].Value = obj.MW_A.ToString();
                        cmd.Parameters["@MW_B"].Value = obj.MW_B.ToString();
                        cmd.Parameters["@MW_C"].Value = obj.MW_C.ToString();
                        cmd.Parameters["@MW_3P"].Value = obj.MW_3P.ToString();
                        cmd.Parameters["@Q_MVAR_A"].Value = obj.Q_MVAR_A.ToString();
                        cmd.Parameters["@Q_MVAR_B"].Value = obj.Q_MVAR_B.ToString();
                        cmd.Parameters["@Q_MVAR_C"].Value = obj.Q_MVAR_C.ToString();
                        cmd.Parameters["@Q_MVAR_3P"].Value = obj.Q_MVAR_3P.ToString();
                        cmd.Parameters["@PF_A"].Value = obj.PF_A.ToString();
                        cmd.Parameters["@PF_B"].Value = obj.PF_B.ToString();
                        cmd.Parameters["@PF_C"].Value = obj.PF_C.ToString();
                        cmd.Parameters["@PF_3P"].Value = obj.PF_3P.ToString();
                        cmd.Parameters["@voltsValue_MAG_A"].Value = obj.VoltsValue_MAG_A.ToString();
                        cmd.Parameters["@voltsValue_MAG_B"].Value = obj.VoltsValue_MAG_B.ToString();
                        cmd.Parameters["@voltsValue_MAG_C"].Value = obj.VoltsValue_MAG_C.ToString();
                        cmd.Parameters["@voltsValue_MAG_S"].Value = obj.VoltsValue_MAG_S.ToString();
                        cmd.Parameters["@vang_A"].Value = obj.Vang_A.ToString();
                        cmd.Parameters["@vang_B"].Value = obj.Vang_B.ToString();
                        cmd.Parameters["@vang_C"].Value = obj.Vang_C.ToString();
                        cmd.Parameters["@vang_S"].Value = obj.Vang_S.ToString();
                        cmd.Parameters["@imag_A"].Value = obj.Imag_A.ToString();
                        cmd.Parameters["@imag_B"].Value = obj.Imag_B.ToString();
                        cmd.Parameters["@imag_N"].Value = obj.Imag_N.ToString();
                        cmd.Parameters["@imag_G"].Value = obj.Imag_G.ToString();
                        cmd.Parameters["@imag_C"].Value = obj.Imag_C.ToString();

                        //cmd.Parameters["@Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["@Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["@Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.Text;

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

            string str = string.Format("INSERT INTO Elster(DeviceId, Date, Alert, [Volt_A],	[Volt_B],	[Volt_C],	[Volt_Total]," +
            "[Ample_A],	[Ample_B],	[Ample_C],	[Ample_Total],	[PF_A],	[PF_B],	[PF_C],	[PF_Total],	[AP_A],	" +
            "[AP_B],	[AP_C],	[AP_Total],	[ReAP_A],	[ReAP_B],	[ReAP_C],	[ReAP_Total])" +
             " VALUES (@DeviceId,  @Date, @Alert, @Volt_A,	@Volt_B,	@Volt_C,	@Volt_Total,	@Ample_A," +
             "@Ample_B,	@Ample_C,	@Ample_Total,	@PF_A,	@PF_B,	@PF_C,	@PF_Total,	@AP_A," +
             "@AP_B,	@AP_C,	@AP_Total,	@ReAP_A,	@ReAP_B,	@ReAP_C,	" +
             "@ReAP_Total)");
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 10);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("@Alert", System.Data.SqlDbType.Bit);
                        param.Value = obj.Alert;

                        param = cmd.Parameters.Add("@Volt_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Volt_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Volt_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Volt_Total", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Ample_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Ample_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Ample_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Ample_Total", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PF_Total", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@AP_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@AP_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@AP_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@AP_Total", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@ReAP_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@ReAP_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@ReAP_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@ReAP_Total", System.Data.SqlDbType.VarChar, 10);
                        

                        cmd.Parameters["@Volt_A"].Value = obj.Volt_A.ToString();
                        cmd.Parameters["@Volt_B"].Value = obj.Volt_B.ToString();
                        cmd.Parameters["@Volt_C"].Value = obj.Volt_C.ToString();
                        cmd.Parameters["@Volt_Total"].Value = obj.Volt_Total.ToString();
                        cmd.Parameters["@Ample_A"].Value = obj.Ample_A.ToString();
                        cmd.Parameters["@Ample_B"].Value = obj.Ample_B.ToString();
                        cmd.Parameters["@Ample_C"].Value = obj.Ample_C.ToString();
                        cmd.Parameters["@Ample_Total"].Value = obj.Ample_Total.ToString();
                        cmd.Parameters["@PF_A"].Value = obj.PowerFactor_A.ToString();
                        cmd.Parameters["@PF_B"].Value = obj.PowerFactor_B.ToString();
                        cmd.Parameters["@PF_C"].Value = obj.PowerFactor_C.ToString();
                        cmd.Parameters["@PF_Total"].Value = obj.PowerFactor_Total.ToString();
                        cmd.Parameters["@AP_A"].Value = obj.ActivePower_A.ToString();
                        cmd.Parameters["@AP_B"].Value = obj.ActivePower_B.ToString();
                        cmd.Parameters["@AP_C"].Value = obj.ActivePower_C.ToString();
                        cmd.Parameters["@AP_Total"].Value = obj.ActivePower_Total.ToString();
                        cmd.Parameters["@ReAP_A"].Value = obj.ReActivePower_A.ToString();
                        cmd.Parameters["@ReAP_B"].Value = obj.ReActivePower_B.ToString();
                        cmd.Parameters["@ReAP_C"].Value = obj.ReActivePower_C.ToString();
                        cmd.Parameters["@ReAP_Total"].Value = obj.ReActivePower_Total.ToString();
                        

                        //cmd.Parameters["@Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["@Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["@Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.Text;

                        //sqlConnection.Open();
                        var result = cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
                //SqlConnection sqlConn = new SqlConnection(_connectionString);





            }
            catch (Exception ex)
            {
                LogService.WriteError("SaveElster", ex.Message);
            }
            finally
            {
                //_sqlConn.Close();
            }
        }
        public void SaveTubu(TuBu obj,DateTime operationtime, bool open,bool? auto)
        {
           // if (obj.LastUpdated == DateTime.MinValue) return;

            string str = string.Format("INSERT INTO TuBu (DeviceId, Name,Location, OperationTime, Operation, Auto) " +
               "VALUES(@DeviceId, @Name, @Location,@OperationTime, @Operation, @Auto)");
            //SqlConnection sqlConn = new SqlConnection(_connectionString);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                    
                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 50);
                        param.Value = obj.Name;

                        param = cmd.Parameters.Add("@Location", System.Data.SqlDbType.VarChar, 50);
                        param.Value = obj.Location;

                        param = cmd.Parameters.Add("@OperationTime", System.Data.SqlDbType.DateTime);
                        param.Value = operationtime.ToString("yyyy-MM-dd HH:mm:ss");
                        //sqlcmd.Parameters.AddWithValue("@enterdate", dtEnterDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        param = cmd.Parameters.Add("@Operation", System.Data.SqlDbType.Bit);
                        param.Value = open ? 1 : 0;
                        param = cmd.Parameters.Add("@Auto", System.Data.SqlDbType.Bit);
                        if (auto == null)
                        { param.Value = DBNull.Value; }
                        else
                        { param.Value = auto; }
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
                //_sqlConn.Close();
            }
        }
        public DataTable Search(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
                
                string str = "Select DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18 From StatusInfo WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To ";
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                        
                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var para = cmd.Parameters.Add("@DeviceId", SqlDbType.Int);
                        para.Value = id;
                        para = cmd.Parameters.Add("@From", SqlDbType.DateTime);
                        para.Value = from;
                        para = cmd.Parameters.Add("@To", SqlDbType.DateTime);
                        para.Value = to;
                        SqlConnection.ClearAllPools();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public DataTable SearchRecloser351(object id, object from, object to)
        {
            DataTable tb = new DataTable();
            try
            {
                //DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18
                string str = " Select DeviceId, Date, Alert, [MW_A] ,	[MW_B] ,	[MW_C] ,	[MW_3P] ," +
            "[Q_MVAR_A] ,	[Q_MVAR_B] ,	[Q_MVAR_C] ,	[Q_MVAR_3P] ,	[PF_A] ,	[PF_B] ,	[PF_C] ,	[PF_3P] ,	[voltsValue_MAG_A] ,	" +
            "[voltsValue_MAG_B] ,	[voltsValue_MAG_C] ,	[voltsValue_MAG_S] ,	[vang_A] ,	[vang_B] ,	[vang_C] ,	[vang_S],	[imag_A],	" +
            "[imag_B],	[imag_N],	[imag_G],	[imag_C] " +
                " From RecloserSel WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To  ";
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                    
                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var para = cmd.Parameters.Add("@DeviceId", SqlDbType.Int);
                        para.Value = id;
                        para = cmd.Parameters.Add("@From", SqlDbType.DateTime);
                        para.Value = from;
                        para = cmd.Parameters.Add("@To", SqlDbType.DateTime);
                        para.Value = to;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        SqlConnection.ClearAllPools();
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                    }
                }
                
                
                
                
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
                //DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18
                string str = " Select DeviceId, Date, Alert,[Volt_A],	[Volt_B],	[Volt_C],	[Volt_Total]," +
            "[Ample_A],	[Ample_B],	[Ample_C],	[Ample_Total],	[PF_A],	[PF_B],	[PF_C],	[PF_Total],	[AP_A],	" +
            "[AP_B],	[AP_C],	[AP_Total],	[ReAP_A],	[ReAP_B],	[ReAP_C],	[ReAP_Total] " +
                " From Elster WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To  ";
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var para = cmd.Parameters.Add("@DeviceId", SqlDbType.Int);
                        para.Value = id;
                        para = cmd.Parameters.Add("@From", SqlDbType.DateTime);
                        para.Value = from;
                        para = cmd.Parameters.Add("@To", SqlDbType.DateTime);
                        para.Value = to;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        SqlConnection.ClearAllPools();
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                    }
                }




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
                //DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18
                string str = " Select [DeviceId] , 	[Date] ,	[Alert] ,	[Current_IA] ,	[Current_IB] ,	[Current_IC] , " +
	"[Current_IE] ,[ApparentPower],	[Apparent_S_A] ,	[Apparent_S_B] ,	[Apparent_S_C] ,[RealPower],	[Real_P_A] ,	[Real_P_B] ,	[Real_P_C] ," +
	"[ReactivePower],[Reactive_Q_A] ,	[Reactive_Q_B] ,	[Reactive_Q_C] ,[Cosphi],	[Cosphi_A] ,	[Cosphi_B] ,	[Cosphi_C] ,	[Total] ,"+
	"[Forward] ,	[Reverse] ,	[BatterryVol] ,	[VA_GND_src] ,	[VB_GND_src] ,	[VC_GND_src] ,	[VA_GND_load] ,	[VB_GND_load] ,"+
	"[VC_GND_load] ,	[VA_B_src] ,	[VB_C_src] ,	[VC_A_src] ,	[VA_B_load] ,	[VB_C_load] ,	[VC_A_load],[Operations]  " +
     " From RecloserADVC WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To  ";
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var para = cmd.Parameters.Add("@DeviceId", SqlDbType.Int);
                        para.Value = id;
                        para = cmd.Parameters.Add("@From", SqlDbType.DateTime);
                        para.Value = from;
                        para = cmd.Parameters.Add("@To", SqlDbType.DateTime);
                        para.Value = to;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        SqlConnection.ClearAllPools();
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                    }
                }




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
                //DeviceId, Type, Date, Alert, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18
                string str = " Select [DeviceId] , 	[Date] ,	[Alert] ,	[Current_IA] ,	[Current_IB] ,	[Current_IC] , " +
    "[Current_IE] ,	[Apparent_S_A] ,	[Apparent_S_B] ,	[Apparent_S_C] ,	[Real_P_A] ,	[Real_P_B] ,	[Real_P_C] ," +
    "[Reactive_Q_A] ,	[Reactive_Q_B] ,	[Reactive_Q_C] ,	[Cosphi_A] ,	[Cosphi_B] ,	[Cosphi_C] ,	[Total] ," +
    "[Forward] ,	[Reverse] ,	[BatterryVol] ,	[VA_GND_src] ,	[VB_GND_src] ,	[VC_GND_src] ,	[VA_GND_load] ,	[VB_GND_load] ," +
    "[VC_GND_load] ,	[VA_B_src] ,	[VB_C_src] ,	[VC_A_src] ,	[VA_B_load] ,	[VB_C_load] ,	[VC_A_load],[Operations]  " +
     " From RecloserVP WHERE DeviceId = @DeviceId AND Date >= @From AND Date <= @To  ";
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var para = cmd.Parameters.Add("@DeviceId", SqlDbType.Int);
                        para.Value = id;
                        para = cmd.Parameters.Add("@From", SqlDbType.DateTime);
                        para.Value = from;
                        para = cmd.Parameters.Add("@To", SqlDbType.DateTime);
                        para.Value = to;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        SqlConnection.ClearAllPools();
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        sqlConnection.Close();
                    }
                }




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
                
                string str = "Select  Name,Location,OperationTime,Case Operation when 1 then 'Open' else 'Close' end as Opt, Case Auto when 1 then 'Auto' When 0 then 'Click' else 'Manual' End as Mod From TuBu WHERE DeviceId = @DeviceId AND OperationTime >= @From AND OperationTime <= @To ";
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.
                    //string cmdText = "";
                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        cmd.CommandType = CommandType.Text;
                        var para = cmd.Parameters.Add("@DeviceId", SqlDbType.Int);
                        para.Value = id;
                        para = cmd.Parameters.Add("@From", SqlDbType.DateTime);
                        para.Value = from;
                        para = cmd.Parameters.Add("@To", SqlDbType.DateTime);
                        para.Value = to;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        SqlConnection.ClearAllPools();

                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }
                        da.Fill(tb);
                        
                        sqlConnection.Close();
                    }
                }
                
                
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
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 10);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("@Alert", System.Data.SqlDbType.Bit);
                        param.Value = obj.Alert;


                        param = cmd.Parameters.Add("@Current_IA", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IB", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IC", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IE", System.Data.SqlDbType.VarChar, 10);





                        param = cmd.Parameters.Add("@Apparent_S_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Total", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Forward", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reverse", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@BatterryVol", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_B_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_C_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_A_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_B_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_C_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_A_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Operations", System.Data.SqlDbType.VarChar, 10);
                        cmd.Parameters["@Current_IA"].Value = obj.Current_IA.ToString();
                        cmd.Parameters["@Current_IB"].Value = obj.Current_IB.ToString();
                        cmd.Parameters["@Current_IC"].Value = obj.Current_IC.ToString();
                        cmd.Parameters["@Current_IE"].Value = obj.Current_IE.ToString();
                        cmd.Parameters["@Apparent_S_A"].Value = obj.Apparent_S_A.ToString();
                        cmd.Parameters["@Apparent_S_B"].Value = obj.Apparent_S_B.ToString();
                        cmd.Parameters["@Apparent_S_C"].Value = obj.Apparent_S_C.ToString();
                        cmd.Parameters["@Real_P_A"].Value = obj.Real_P_A.ToString();
                        cmd.Parameters["@Real_P_B"].Value = obj.Real_P_B.ToString();
                        cmd.Parameters["@Real_P_C"].Value = obj.Real_P_C.ToString();
                        cmd.Parameters["@Reactive_Q_A"].Value = obj.Reactive_Q_A.ToString();
                        cmd.Parameters["@Reactive_Q_B"].Value = obj.Reactive_Q_B.ToString();
                        cmd.Parameters["@Reactive_Q_C"].Value = obj.Reactive_Q_C.ToString();
                        cmd.Parameters["@Cosphi_A"].Value = obj.Cosphi_A.ToString();
                        cmd.Parameters["@Cosphi_B"].Value = obj.Cosphi_B.ToString();
                        cmd.Parameters["@Cosphi_C"].Value = obj.Cosphi_C.ToString();
                        cmd.Parameters["@Total"].Value = obj.Total.ToString();
                        cmd.Parameters["@Forward"].Value = obj.Forward.ToString();
                        cmd.Parameters["@Reverse"].Value = obj.Reverse.ToString();
                        cmd.Parameters["@BatterryVol"].Value = obj.BatterryVol.ToString();
                        cmd.Parameters["@VA_GND_src"].Value = obj.VA_GND_src.ToString();
                        cmd.Parameters["@VB_GND_src"].Value = obj.VB_GND_src.ToString();
                        cmd.Parameters["@VC_GND_src"].Value = obj.VC_GND_src.ToString();
                        cmd.Parameters["@VA_GND_load"].Value = obj.VA_GND_load.ToString();
                        cmd.Parameters["@VB_GND_load"].Value = obj.VB_GND_load.ToString();
                        cmd.Parameters["@VC_GND_load"].Value = obj.VC_GND_load.ToString();
                        cmd.Parameters["@VA_B_src"].Value = obj.VA_B_src.ToString();
                        cmd.Parameters["@VB_C_src"].Value = obj.VB_C_src.ToString();
                        cmd.Parameters["@VC_A_src"].Value = obj.VC_A_src.ToString();
                        cmd.Parameters["@VA_B_load"].Value = obj.VA_B_load.ToString();
                        cmd.Parameters["@VB_C_load"].Value = obj.VB_C_load.ToString();
                        cmd.Parameters["@VC_A_load"].Value = obj.VC_A_load.ToString();
                        cmd.Parameters["@Operations"].Value = obj.Operations.ToString();

                        //cmd.Parameters["@Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["@Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["@Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.Text;

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
            string str = string.Format("INSERT INTO RecloserADVC (DeviceId, Date, Alert, " +
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
	" @VC_A_load,@Operations)");
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 10);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("@Alert", System.Data.SqlDbType.Bit);
                        param.Value = obj.Alert;


                        param = cmd.Parameters.Add("@Current_IA", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IB", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IC", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IE", System.Data.SqlDbType.VarChar, 10);
                        //ApparentPower, RealPower, ReactivePower, PowerFactor
                        param = cmd.Parameters.Add("@ApparentPower", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@RealPower", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@ReactivePower", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@PowerFactor", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Total", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Forward", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reverse", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@BatterryVol", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_B_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_C_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_A_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_B_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_C_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_A_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Operations", System.Data.SqlDbType.VarChar, 10);
                        cmd.Parameters["@Current_IA"].Value = obj.Current_IA.ToString();
                        cmd.Parameters["@Current_IB"].Value = obj.Current_IB.ToString();
                        cmd.Parameters["@Current_IC"].Value = obj.Current_IC.ToString();
                        cmd.Parameters["@Current_IE"].Value = obj.Current_IE.ToString();
                        cmd.Parameters["@Apparent_S_A"].Value = obj.Apparent_S_A.ToString();
                        cmd.Parameters["@Apparent_S_B"].Value = obj.Apparent_S_B.ToString();
                        cmd.Parameters["@Apparent_S_C"].Value = obj.Apparent_S_C.ToString();
                        cmd.Parameters["@Real_P_A"].Value = obj.Real_P_A.ToString();
                        cmd.Parameters["@Real_P_B"].Value = obj.Real_P_B.ToString();
                        cmd.Parameters["@Real_P_C"].Value = obj.Real_P_C.ToString();
                        cmd.Parameters["@Reactive_Q_A"].Value = obj.Reactive_Q_A.ToString();
                        cmd.Parameters["@Reactive_Q_B"].Value = obj.Reactive_Q_B.ToString();
                        cmd.Parameters["@Reactive_Q_C"].Value = obj.Reactive_Q_C.ToString();
                        cmd.Parameters["@Cosphi_A"].Value = obj.Cosphi_A.ToString();
                        cmd.Parameters["@Cosphi_B"].Value = obj.Cosphi_B.ToString();
                        cmd.Parameters["@Cosphi_C"].Value = obj.Cosphi_C.ToString();
                        cmd.Parameters["@Total"].Value = obj.Total.ToString();
                        cmd.Parameters["@Forward"].Value = obj.Forward.ToString();
                        cmd.Parameters["@Reverse"].Value = obj.Reverse.ToString();
                        cmd.Parameters["@BatterryVol"].Value = obj.BatterryVol.ToString();
                        cmd.Parameters["@VA_GND_src"].Value = obj.VA_GND_src.ToString();
                        cmd.Parameters["@VB_GND_src"].Value = obj.VB_GND_src.ToString();
                        cmd.Parameters["@VC_GND_src"].Value = obj.VC_GND_src.ToString();
                        cmd.Parameters["@VA_GND_load"].Value = obj.VA_GND_load.ToString();
                        cmd.Parameters["@VB_GND_load"].Value = obj.VB_GND_load.ToString();
                        cmd.Parameters["@VC_GND_load"].Value = obj.VC_GND_load.ToString();
                        cmd.Parameters["@VA_B_src"].Value = obj.VA_B_src.ToString();
                        cmd.Parameters["@VB_C_src"].Value = obj.VB_C_src.ToString();
                        cmd.Parameters["@VC_A_src"].Value = obj.VC_A_src.ToString();
                        cmd.Parameters["@VA_B_load"].Value = obj.VA_B_load.ToString();
                        cmd.Parameters["@VB_C_load"].Value = obj.VB_C_load.ToString();
                        cmd.Parameters["@VC_A_load"].Value = obj.VC_A_load.ToString();
                        cmd.Parameters["@Operations"].Value = obj.Operations.ToString();
                        //ApparentPower, RealPower, ReactivePower, PowerFactor
                        cmd.Parameters["@ApparentPower"].Value = obj.ApparentPower.ToString();
                        cmd.Parameters["@RealPower"].Value = obj.RealPower.ToString();
                        cmd.Parameters["@ReactivePower"].Value = obj.ReactivePower.ToString();
                        cmd.Parameters["@PowerFactor"].Value = obj.PowerFactor.ToString();
                        //cmd.Parameters["@Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["@Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["@Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.Text;

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
        /*public void SaveRecloserADVC45(RecloserADVC45 obj)
        {
            // VP sua lai connectionstring

            // if (obj.LastUpdated == DateTime.MinValue) return;

            string str = string.Format("INSERT INTO RecloserADVC (DeviceId, Date, Alert, " +
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
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    // Need to define cmdText.

                    using (SqlCommand cmd = new SqlCommand(str, sqlConnection))
                    {
                        var param = cmd.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        param.Value = obj.Id;
                        param = cmd.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 10);
                        param.Value = obj.DeviceType.ToString();
                        param = cmd.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
                        param.Value = obj.LastUpdated;
                        param = cmd.Parameters.Add("@Alert", System.Data.SqlDbType.Bit);
                        param.Value = obj.Alert;


                        param = cmd.Parameters.Add("@Current_IA", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IB", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IC", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Current_IE", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Apparent_S_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Real_P_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reactive_Q_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_A", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_B", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Cosphi_C", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Total", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Forward", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Reverse", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@BatterryVol", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_GND_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_GND_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_B_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_C_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_A_src", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VA_B_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VB_C_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@VC_A_load", System.Data.SqlDbType.VarChar, 10);
                        param = cmd.Parameters.Add("@Operations", System.Data.SqlDbType.VarChar, 10);
                        cmd.Parameters["@Current_IA"].Value = obj.Current_IA.ToString();
                        cmd.Parameters["@Current_IB"].Value = obj.Current_IB.ToString();
                        cmd.Parameters["@Current_IC"].Value = obj.Current_IC.ToString();
                        cmd.Parameters["@Current_IE"].Value = obj.Current_IE.ToString();
                        cmd.Parameters["@Apparent_S_A"].Value = obj.Apparent_S_A.ToString();
                        cmd.Parameters["@Apparent_S_B"].Value = obj.Apparent_S_B.ToString();
                        cmd.Parameters["@Apparent_S_C"].Value = obj.Apparent_S_C.ToString();
                        cmd.Parameters["@Real_P_A"].Value = obj.Real_P_A.ToString();
                        cmd.Parameters["@Real_P_B"].Value = obj.Real_P_B.ToString();
                        cmd.Parameters["@Real_P_C"].Value = obj.Real_P_C.ToString();
                        cmd.Parameters["@Reactive_Q_A"].Value = obj.Reactive_Q_A.ToString();
                        cmd.Parameters["@Reactive_Q_B"].Value = obj.Reactive_Q_B.ToString();
                        cmd.Parameters["@Reactive_Q_C"].Value = obj.Reactive_Q_C.ToString();
                        cmd.Parameters["@Cosphi_A"].Value = obj.Cosphi_A.ToString();
                        cmd.Parameters["@Cosphi_B"].Value = obj.Cosphi_B.ToString();
                        cmd.Parameters["@Cosphi_C"].Value = obj.Cosphi_C.ToString();
                        cmd.Parameters["@Total"].Value = obj.Total.ToString();
                        cmd.Parameters["@Forward"].Value = obj.Forward.ToString();
                        cmd.Parameters["@Reverse"].Value = obj.Reverse.ToString();
                        cmd.Parameters["@BatterryVol"].Value = obj.BatterryVol.ToString();
                        cmd.Parameters["@VA_GND_src"].Value = obj.VA_GND_src.ToString();
                        cmd.Parameters["@VB_GND_src"].Value = obj.VB_GND_src.ToString();
                        cmd.Parameters["@VC_GND_src"].Value = obj.VC_GND_src.ToString();
                        cmd.Parameters["@VA_GND_load"].Value = obj.VA_GND_load.ToString();
                        cmd.Parameters["@VB_GND_load"].Value = obj.VB_GND_load.ToString();
                        cmd.Parameters["@VC_GND_load"].Value = obj.VC_GND_load.ToString();
                        cmd.Parameters["@VA_B_src"].Value = obj.VA_B_src.ToString();
                        cmd.Parameters["@VB_C_src"].Value = obj.VB_C_src.ToString();
                        cmd.Parameters["@VC_A_src"].Value = obj.VC_A_src.ToString();
                        cmd.Parameters["@VA_B_load"].Value = obj.VA_B_load.ToString();
                        cmd.Parameters["@VB_C_load"].Value = obj.VB_C_load.ToString();
                        cmd.Parameters["@VC_A_load"].Value = obj.VC_A_load.ToString();
                        cmd.Parameters["@Operations"].Value = obj.Operations.ToString();

                        //cmd.Parameters["@Field8"].Value = obj.Status_Open.ToString();
                        //cmd.Parameters["@Field9"].Value = obj.Status_Close.ToString();
                        //cmd.Parameters["@Field10"].Value = obj.Status_Lockout.ToString();
                        cmd.CommandType = CommandType.Text;

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
        }*/
    }
}
