using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data;
using RecloserAcq.OracleDAL;
//using System.Windows.Forms;

namespace RecloserAcq.Device
{
    public static class DeviceStatic
    {
        public static void SaveDevices(List<RecloserBase> list, string DeviceFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<RecloserBase>));
            using (var stream = File.CreateText(DeviceFile))
            {
                ser.Serialize(stream, list);
                stream.Close();
            }
        }
        public static Int32 getGroup(Int32 DeviceId)
        {
            return DBController.Instance.GetGroupId(DeviceId); 
        }
        public static void SaveDevices(List<RecloserBase> list)
        {
            foreach (RecloserBase rc in list)
            {
                rc.SaveDevice();
            }
        }
        public static  List<RecloserBase> GetDevices(string DeviceFile)
        {
            List<RecloserBase> list = null;
            if (File.Exists(DeviceFile))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<RecloserBase>));
                using (var stream = File.OpenRead(DeviceFile))
                {
                    var devices = ser.Deserialize(stream) as List<RecloserBase>;
                    list = devices;
                }
            }
            else
            {
                list = new List<RecloserBase>();
                list.Add(new CooperFXB(7000) { Name = "Cooper 1" });
                list.Add(new Nulec(7001) { Name = "Nulec 1" });
            }

            return list;
        }
          //DeviceStatic.LogOperation(userid, username, deviceid, devicename, groupname, 0,false);//0:close,1:open
        public static void LogOperation(int userid, string username, int deviceid, string devicename, string groupname, int action, bool auto)
        {
            DBController.Instance.LogOperation(userid, username, deviceid, devicename,  groupname, action, auto);
        }
        public static List<RecloserBase> GetDevices(int groupid)
        {
            List<RecloserBase> list = null;
            list = new List<RecloserBase>();
            DataTable dt = DBController.Instance.GetDevices(groupid);
            bool addedRecloserADVC = false;
            foreach(DataRow row in dt.Rows)
            {
                RecloserBase rb ;
               
                int port = Convert.ToInt32(row["Port"]);
                if(row["DeviceType"].ToString() == "TuBu"){
                    rb = new TuBu();
                }
                else if(row["DeviceType"].ToString() == "LBS"){
                    rb = new LBS();
                }
                else if(row["DeviceType"].ToString() == "Recloser351R"){
                    rb = new Recloser351R(port);   
                }
                else if(row["DeviceType"].ToString() == "CooperFxb"){
                    rb = new CooperFXB(port);
                }
                else if(row["DeviceType"].ToString() == "Nulec"){
                    rb= new Nulec(port);
                }
                else if (row["DeviceType"].ToString() == "RecloserUSeries")
                {
                    rb = new RecloserUSeries(port);
                    if (addedRecloserADVC == false)
                    {
                        // Other type device here 
                        RecloserBase rbtmp = new RecloserADVC();
                        list.Add(rb);
                        addedRecloserADVC = true;
                    }
                }
                else if(row["DeviceType"].ToString() == "Elster1700"){
                    rb = new Elster1700(port);
                   
                }
                else if (row["DeviceType"].ToString() == "RecloserADVC" )
                {
                    rb = new RecloserADVC(port);
                    addedRecloserADVC = true;

                }
                else if (row["DeviceType"].ToString() == "RecloserADVC45")
                {
                    rb = new RecloserADVC45(port);
                    if (addedRecloserADVC == false)
                    {
                        // Other type device here 
                        RecloserBase rbtmp = new RecloserADVC();
                        list.Add(rb);
                        addedRecloserADVC = true;
                    }
                }
                else if (row["DeviceType"].ToString() == "RecloserADVCTCPIP")
                {
                    rb = new RecloserADVCTCPIP(port);
                    if (addedRecloserADVC == false)
                    {
                        // Other type device here 
                        RecloserBase rbtmp = new RecloserADVC();
                        list.Add(rb);
                        addedRecloserADVC = true;
                    }
                }
                else
                {
                    rb = new RecloserBase();
                }
                // Other type device here 
                rb.Name = row["Name"].ToString();
                rb.Location = row["Location"].ToString();
                rb.Port = Convert.ToInt32(row["Port"]);
                rb.Serveraddress = row["ServerAddress"].ToString();
                rb.GroupName = row["GroupName"].ToString();
                rb.GroupId = Convert.ToInt16(row["GroupId"]);
                rb.Enable = Convert.ToBoolean(row["Enable"]);
                rb.Latencybetweenpoll = Convert.ToInt32(row["LatencyBetweenPoll"]);
                rb.Latencybetweenrequest = Convert.ToInt32(row["LATENCYBETWEENREQUEST"]);
                rb.LatencySaveHistory = Convert.ToInt32(row["LATENCYSAVEHISTORY"]);
                rb.Maxampduration = Convert.ToInt32(row["Maxampduration"]);
                rb.MinAmp = Convert.ToInt32(row["MinAmp"]);
                rb.MaxAmp = Convert.ToInt32(row["MaxAmp"]);
                rb.Id = Convert.ToInt32(row["ID"]);
                rb.Password = row["Password"].ToString();
                rb.Client = Convert.ToBoolean(row["Client"]);
                rb.DeviceAddress = row["deviceaddress"].ToString();
                rb.BaudRate = Convert.ToInt32(row["baudrate"]);
                rb.SecondWait = row["secondwait"]==DBNull.Value ? 0: Convert.ToInt32(row["secondwait"]);
                rb.VolStandard = row["voltstandard"] == DBNull.Value ? 0 : Convert.ToInt32(row["voltstandard"]);
                rb.Volpercent = row["voltpercent"] == DBNull.Value ? 0 : Convert.ToInt32(row["voltpercent"]);
                rb.AmpPercent = row["amppercent"] == DBNull.Value ? 0 : Convert.ToInt32(row["amppercent"]);
                list.Add(rb);
                
            }
            if (list.Count() == 0)
            {
                list.Add(new CooperFXB(7000) { Name = "Cooper 1" });
                list.Add(new Nulec(7001) { Name = "Nulec 1" });
            }

            return list;
        }
        

        public static string[] getNameGroup()
        {

            DataTable tb = DBController.Instance.getNameGroup();
            string[] strs = new string[tb.Rows.Count];
            int i = 0;
            foreach (DataRow row in tb.Rows)
            {
                strs[i] = row["groupname"].ToString();
                i++;
            }
            return strs;
        }
        public static void UpdateRole(RoleObj obj)
        {
            DBController.Instance.UpdateRole(obj);
        }
        public static List<RoleObj> getroles()
        {
            DataTable tb = DBController.Instance.getroles();
            List<RoleObj> list = new List<RoleObj>();
            foreach (DataRow row in tb.Rows)
            {
                //SELECT Users.ID, Users.name as Username,role.name as Role, roleid, password FROM Users left join Role  on Users.roleId = role.id;
                RoleObj obj = new RoleObj();
                obj.name = row["name"].ToString();
                obj.id = Convert.ToInt32(row["id"]);

                obj.permsyntime = Convert.ToUInt16(row["perm1"] == DBNull.Value ? 0 : row["perm1"]);
                obj.permseeHistory = Convert.ToUInt16(row["perm2"] == DBNull.Value ? 0 : row["perm2"]);
                obj.permCloseRecloser = Convert.ToUInt16(row["perm3"] == DBNull.Value ? 0 : row["perm3"]);
                obj.permopenRecloser = Convert.ToUInt16(row["perm4"] == DBNull.Value ? 0 : row["perm4"]);
                obj.permoperateTubu = Convert.ToUInt16(row["perm5"] == DBNull.Value ? 0 : row["perm5"]);
                obj.permUser = Convert.ToUInt16(row["perm6"] == DBNull.Value ? 0 : row["perm6"]);
                obj.permConfigCommon = Convert.ToUInt16(row["perm7"] == DBNull.Value ? 0 : row["perm7"]);
                obj.permNhom1 = Convert.ToUInt16(row["perm8"] == DBNull.Value ? 0 : row["perm8"]); //permNhom1 : permUser
                obj.permNhom2 = Convert.ToUInt16(row["perm9"] == DBNull.Value ? 0 : row["perm9"]); //permConfigCommon -> permNhom2
                obj.permNhom3 = Convert.ToUInt16(row["perm10"] == DBNull.Value ? 0 : row["perm10"]);
                obj.permNhom4 = Convert.ToUInt16(row["perm11"] == DBNull.Value ? 0 : row["perm11"]);
                obj.permNhom5 = Convert.ToUInt16(row["perm12"] == DBNull.Value ? 0 : row["perm12"]);
                obj.permNhom6 = Convert.ToUInt16(row["perm13"] == DBNull.Value ? 0 : row["perm13"]);
                obj.permNhom7 = Convert.ToUInt16(row["perm14"] == DBNull.Value ? 0 : row["perm14"]);
                obj.permNhom8 = Convert.ToUInt16(row["perm15"] == DBNull.Value ? 0 : row["perm15"]);
                obj.permNhom9 = Convert.ToUInt16(row["perm16"] == DBNull.Value ? 0 : row["perm16"]);
                obj.permNhom10 = Convert.ToUInt16(row["perm17"] == DBNull.Value ? 0 : row["perm17"]);
                obj.permNhom11 = Convert.ToUInt16(row["perm18"] == DBNull.Value ? 0 : row["perm18"]);
                obj.permNhom12 = Convert.ToUInt16(row["perm19"] == DBNull.Value ? 0 : row["perm19"]);
                obj.permNhom13 = Convert.ToUInt16(row["perm20"] == DBNull.Value ? 0 : row["perm20"]);
                obj.permNhom14 = Convert.ToUInt16(row["perm21"] == DBNull.Value ? 0 : row["perm21"]);
                obj.permNhom15 = Convert.ToUInt16(row["perm22"] == DBNull.Value ? 0 : row["perm22"]);
                obj.permNhom16 = Convert.ToUInt16(row["perm23"] == DBNull.Value ? 0 : row["perm23"]);
                obj.permNhom17 = Convert.ToUInt16(row["perm24"] == DBNull.Value ? 0 : row["perm24"]);
                obj.permNhom18 = Convert.ToUInt16(row["perm25"] == DBNull.Value ? 0 : row["perm25"]);
                obj.permNhom19 = Convert.ToUInt16(row["perm26"] == DBNull.Value ? 0 : row["perm26"]);
               

                list.Add(obj);
            }
            return list;
        }

        public static List<UserObj> getUserList()
        {
            DataTable tb = DBController.Instance.getUserList();
            List<UserObj> list  = new List<UserObj>(); 
            foreach (DataRow row in tb.Rows)
            {
                //SELECT Users.ID, Users.name as Username,role.name as Role, roleid, password FROM Users left join Role  on Users.roleId = role.id;
                UserObj obj = new UserObj();
                obj.name = row["Username"].ToString();
                obj.id = Convert.ToInt32(row["id"]);
                obj.RoleId = Convert.ToInt32(row["roleid"]);
                obj.RoleName = row["role"].ToString();
                obj.password = row["password"].ToString();
                list.Add(obj);
            }
            return list;
        }
        public static void DeleteUser(int userid)
        {
            DBController.Instance.DeleteUser(userid);
        }
        public static void UpdateUser(UserObj user)
        {
            DBController.Instance.UpdateUser(user);
        }
        public static void InsertUser(UserObj user)
        {
            DBController.Instance.InsertUser(user);
        }
        public static UserObj getUserLogin(string username, string encryptpassword)
        {
            return DBController.Instance.getUserLogin(username, encryptpassword);
        }
        public static UserObj getUserInfo(int userId)
        {
            return DBController.Instance.getUserInfo(userId);
        }
        public static List<RecloserBase> GetDevicesSchedule(string DeviceFile)
        {
            List<RecloserBase> list = null;
            if (File.Exists(DeviceFile))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<RecloserBase>));
                using (var stream = File.OpenRead(DeviceFile))
                {
                    var devices = ser.Deserialize(stream) as List<RecloserBase>;
                    list = devices;
                }
            }
            else
            {
                list = new List<RecloserBase>();
                list.Add(new CooperFXB(7000) { Name = "Cooper 1" });
                list.Add(new Nulec(7001) { Name = "Nulec 1" });
            }

            return list;
        }
        public static void DeleteDeviceSchedule(DeviceEvent de)
        {
            DBController.Instance.DeleteSchedule(de);
        }
        public static void SaveGroup(GroupClass obj)
        {
            DBController.Instance.SaveGroup(obj);
        }
        public static void InsertGroup(GroupClass obj)
        {
            DBController.Instance.InsertGroup(obj);
        }
        public static List<DeviceEvent> GetADeviceSchedule(int DeviceID)
        {
            List<DeviceEvent> list = null;
            list = new List<DeviceEvent>();
            DataTable dt = DBController.Instance.GetADeviceSchedule(DeviceID);
            foreach (DataRow row in dt.Rows)
            {
                DeviceEvent de;


                de = new DeviceEvent();

                // Other type device here 
                de.NameOfEvent = row["eventname"].ToString();
                de.hourRepeat = Convert.ToInt16(row["hourRepeat"]);
                de.Command = row["command"].ToString();
                de.DtNextRun = Convert.ToDateTime(row["dtnextrun"]);
                if (row["dtPrevRun"] != DBNull.Value)
                {
                    de.DtPrevRun = Convert.ToDateTime(row["dtPrevRun"]);
                }
                de.Id = Convert.ToInt32(row["Id"]);
                de.Weekday = row["weekdays"].ToString();
                de.Type = row["type"].ToString();
                de.DtExpire = Convert.ToDateTime(row["dtExpire"]);
                de.DtActive = Convert.ToDateTime(row["DtActive"]);
                de.DeviceId = Convert.ToInt16(row["DeviceId"]);
                de.DeviceName = row["Name"].ToString();
                list.Add(de);

            }


            return list;
        }
        public static List<DeviceEvent> GetDevicesSchedule(int groupid)
        {
            List<DeviceEvent> list = null;
            list = new List<DeviceEvent>();
            DataTable dt = DBController.Instance.GetDevicesSchedule(groupid);
            foreach (DataRow row in dt.Rows)
            {
                DeviceEvent de;


                de = new DeviceEvent();
              
                // Other type device here 
                de.NameOfEvent = row["eventname"].ToString();
                de.hourRepeat = Convert.ToInt16(row["hourRepeat"]);
                de.Command = row["command"].ToString();
                de.DtNextRun = Convert.ToDateTime(row["dtnextrun"]);
                if (row["dtPrevRun"] != DBNull.Value)
                {
                    de.DtPrevRun = Convert.ToDateTime(row["dtPrevRun"]);
                }
                de.Id = Convert.ToInt32(row["Id"]);
                de.Weekday = row["weekdays"].ToString();
                de.Type = row["type"].ToString();
                de.DtExpire = Convert.ToDateTime(row["dtExpire"]);
                de.DtActive = Convert.ToDateTime(row["DtActive"]);
                de.DeviceId = Convert.ToInt16(row["DeviceId"]);
                de.DeviceName = row["Name"].ToString();
                list.Add(de);

            }
            

            return list;
        }
        public static bool IsPasswordValidated(string devicetype)
        {
            ValidatePassword vp = new ValidatePassword(true, devicetype);
            if (vp.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                //this.Close();
                return false;
            }
            return true;
        }
        public static bool ValidPassword(string pass, string type)
        {
            
            int indexofType = pass.LastIndexOf(type);
            if (indexofType < 0)
            {
                
                return false;
            }
            string expectedpassword = pass.Substring(0, indexofType);
            return FA_Accounting.Common.Password.ValidatePassword(expectedpassword);
            
        }
    }
}
