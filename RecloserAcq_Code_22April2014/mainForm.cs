using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecloserAcq.Device;
using System.Xml;
using System.IO;
using RecloserAcq.OracleDAL;
using TcpComm;


namespace RecloserAcq
{
    public partial class mainForm : Form
    {
        
        private string _devicebtnfilepath = Application.StartupPath + "\\DeviceFiles\\devices.xml";
        List<DevicesBtn> _listbtns  = new List<DevicesBtn>();
        int port;
        
        webcommand listeningdev;
        public mainForm()
        {
            InitializeComponent();
            try
            {
                port = Convert.ToInt32(RecloserAcq.Properties.Settings.Default.Port);
                listeningdev = new webcommand(port);
                listeningdev.Client = false;
            }
            catch (Exception ex)
            {
                LogService.WriteError("main form ini", ex.ToString());
            }
            
            pictureBox1.ImageLocation = "banner.png";
            
        }
        private RecloserBase getRecloser(int Id)
        {
            RecloserBase rb;
            int groupid = 0;
            groupid = DeviceStatic.getGroup(Id);
            try
            {
                DevicesBtn group = _listbtns.Where(b => b.GroupID == groupid).FirstOrDefault();
                rb = group.getRecloser(Id);
                if (rb == null)
                {
                    group = _listbtns.Where(b => b.GroupID == 0).FirstOrDefault();
                    rb = group.getRecloser(Id);
                }
                //phai cho them bGroupID == 0 nua 
                return rb;
            }
            catch (Exception ex)
            {
                LogService.WriteError("Rerecloser Mainform", ex.ToString());
                return null;
            }
        }
        private void ExecCommand(int DeviceId,string Commandstr,string password,string UserId)
        {
            RecloserBase rb = getRecloser(DeviceId);
            if (rb != null )/// only apply this function for Tubu
            {
                //if ((rb.DeviceType == eDeviceType.TuBu && Program.DienLuc == "bentre") || Program.DienLuc!="bentre")
                //{ y/c moi cua Bentre , cho phép dong cat Tu Bu va Recloser Trên web
                    UserObj usercommand = DeviceStatic.getUserInfo(Convert.ToInt32(UserId));
                    if (rb.DeviceType == eDeviceType.TuBu && usercommand.permoperateTubu<2)
                    {
                        string msg = usercommand.name + " chưa được cấp quyền thao tác Tụ bụ\r\n";
                        listeningdev.Listener.Send(Ultility.FromString(msg));
                        return;
                        //check user permission: in the allowed group, 2 operate allowed action
                    }
                    if (rb.DeviceType != eDeviceType.TuBu && usercommand.permopenRecloser < 2 && Commandstr == "MO")
                    {
                        string msg = usercommand.name + " chưa được cấp quyền thao tác open recloser\r\n";
                        listeningdev.Listener.Send(Ultility.FromString(msg));
                        return;
                        //check user permission: in the allowed group, 2 operate allowed action
                    }
                    if (rb.DeviceType != eDeviceType.TuBu && usercommand.permCloseRecloser < 2 && Commandstr == "DONG")
                    {
                        string msg = usercommand.name + " chưa được cấp quyền thao tác close recloser\r\n";
                        listeningdev.Listener.Send(Ultility.FromString(msg));
                        return;
                        //check user permission: in the allowed group, 2 operate allowed action
                    }
                    if ((rb.DeviceType == eDeviceType.TuBu &&!DeviceStatic.ValidPassword(password, "tubu"))
                        ||(rb.DeviceType != eDeviceType.TuBu && !DeviceStatic.ValidPassword(password, "recloser")))
                    {
                        string msg = "Password sai\r\n";
                        listeningdev.Listener.Send(Ultility.FromString(msg));
                        return;
                    }

                    if (Commandstr == "DONG")
                    {
                        rb.CommandClose(false);
                        string msg = "Đóng đã được thực hiện\r\n";
                        listeningdev.Listener.Send(Ultility.FromString(msg));

                        LogService.WriteInfo("Webcommand", "Dong is executed  at deviceId " + rb.Id + " by " + listeningdev.UserID);
                    }
                    else if (Commandstr == "MO")
                    {
                        rb.CommandOpen(false);
                        string msg = "Mở đã được thực hiện\r\n";
                        listeningdev.Listener.Send(Ultility.FromString(msg));
                        LogService.WriteInfo("Webcommand", "Mo is executed at deviceId " + rb.Id + " by " + listeningdev.UserID);
                    }
                    else
                    {
                        string msg = "Lệnh không rõ\r\n";
                        listeningdev.Listener.Send(Ultility.FromString(msg));

                        LogService.WriteInfo("Webcommand", "Lệnh không rõ");
                    }
                //}
                //else // xem lai y/c moi cua Ben Tre
                //{
                //    string msg = "Ngoài tụ bù ra, các recloser không đóng cắt trên web\r\n";
                //    listeningdev.Listener.Send(Ultility.FromString(msg));

                //    LogService.WriteInfo("Webcommand", "Thao tác recloser từ chối");
                //}
            }
            else
            {
                string msg = "Device ID không tìm thấy\r\n";
                listeningdev.Listener.Send(Ultility.FromString(msg));
            }
        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            // load devices buttons in DeviceFiles\\devices.xml 
            /*DevicesButtonConfigFrm frm = new DevicesButtonConfigFrm(_devicebtnfilepath);
            frm.DtDeviceGroup = GetDeviceBtn(_devicebtnfilepath);
            frm.ShowDialog();
            LoadButtons();*/
            using (DevicesButtonConfigFrm frm = new DevicesButtonConfigFrm())
            {

                frm.GroupList = GetListGroup();
                frm.ShowDialog();
                LoadButtonsGroup();
            }
        }

        private void dvcList1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            //ValidatePassword vp = new ValidatePassword(false);
            /// remove comment when go live
            //if (vp.ShowDialog() == DialogResult.Cancel)
            //{
            //    this.Close();
            //    return;
            //}

            //load button and device files 
            // load devices buttons in DeviceFiles\\devices.xml 
            listeningdev.Listener.OnDataSent += new EventHandler<DataTransferEventArg>(Listener_OnDataSent);
            try
            {
                LoadButtonsGroup();
                listeningdev.Listener.OnDataReceived += new EventHandler<DataTransferEventArg>(Listener_OnDataReceived);
                //listeningdev.Listener.OnDataSent += new EventHandler<DataTransferEventArg>(Listener_OnDataSent);
                listeningdev.Listener.OnStatusChanged += new EventHandler<StatusChangedEventArgs>(Listener_OnStatusChanged);
                listeningdev.OnDataUpdated += new EventHandler(onreceivcommand);
                //listeningdev.Listener.DoParse = Parser.DoParse;


                listeningdev.Latencybetweenrequest = 0;
                listeningdev.Latencybetweenpoll = 0;
                listeningdev.Listener.StartListening();
                listeningdev.Listener.ReceiveBuffer.Reset();
            }
            catch (Exception ex)
            {
                LogService.WriteError("Main form Load", ex.ToString());
            }
            //listeningdev.StartPoll();
            //LoadButtons();
        }
        private void Listener_OnDataSent(object sender, DataTransferEventArg e)
        {
            if (this.Disposing) return;
            try
            {
                //this.Invoke(_addCommLog, e.Data, e.Port.ToString(), false);
                FA_Accounting.Common.LogService.WriteInfo(e.Data.ToString(), e.Port.ToString());
            }
            catch { }
        }
        private void onreceivcommand(object sender, EventArgs e)
        {
            try
            {
                var rc = sender as webcommand;
                int DevId = rc.IDControl;
                string UserId = rc.UserID;
                string commandstr = rc.commandstr;
                string password = rc.commandpassword;
                //LogService.WriteInfo("receivecommand", "DevID:" + DevId.ToString() + ",UserId:" + UserId + ",command:" + commandstr + ",password:" + password);
                //checkvalid password
                ExecCommand(DevId, commandstr, password,UserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        private void Listener_OnDataReceived(object sender, DataTransferEventArg e)
        {
            LogService.WriteInfo("Webcommand", FA_Accounting.Common.Ultility.ToHexText(e.Data));
        }
        void Listener_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            LogService.WriteInfo("Webcommand", e.Status);
        }
        public static DataTable GetDeviceBtn(string DeviceFile)
        {
            List<DevicesBtn> list = null;
            if (File.Exists(DeviceFile))
            {
                XmlReader xmlFile = XmlReader.Create(DeviceFile, new XmlReaderSettings());
                DataSet dataSet = new DataSet();
                //Read xml to dataset
                dataSet.ReadXml(xmlFile);
                //Pass empdetails table to datagridview datasource

                //Close xml reader
                xmlFile.Close();
                return dataSet.Tables["DevicesBtn"];
            }
            else
            {
                list = new List<DevicesBtn>();
                list.Add(new DevicesBtn() { Text = "KCN 1" });
                list.Add(new DevicesBtn() { Text = "KCN 2" });
                list.Add(new DevicesBtn() { Text = "KCN 3" });

                return null;
            }


        }
        public static DataTable GetDeviceBtnGroup()
        {
            
            DataTable dt = DBController.Instance.GetGroups();

            return dt;
            


        }
        public static List<GroupClass> GetListGroup()
        {
            List<GroupClass> list = new List<GroupClass>();
            DataTable dt = DBController.Instance.GetGroups();
            foreach (DataRow row in dt.Rows)
            {
                GroupClass dvcBtn = new GroupClass();
                dvcBtn.GroupName = row["GroupName"].ToString();
                dvcBtn.ID = Convert.ToInt16(row["ID"]);
                list.Add(dvcBtn);
            }
            return list;

        }
     /*   private void LoadButtons()
        {
            // remove buttons of DevicesBtn
            int count = this.Controls.Count;
            int dis = 0;
            for(int j =0;j<count;j++)
            {
                if (this.Controls[j-dis].GetType() == typeof(DevicesBtn))
                {
                    this.Controls.Remove(this.Controls[j-dis]);
                    dis++;

                }
            }

            DataTable tmp = GetDeviceBtn(_devicebtnfilepath);
            int i = 0;
            foreach (DataRow row in tmp.Rows)
            {
                DevicesBtn dvcBtn = new DevicesBtn();
                dvcBtn.Text = row["Text"].ToString();
                dvcBtn.DeviceFilePath = row["DeviceFile"].ToString();
                dvcBtn.Location = new System.Drawing.Point(9 + (i / 4) * 108, 39 + i * 44);
                dvcBtn.Name = "devicesBtn" + i;
                dvcBtn.Size = new System.Drawing.Size(103, 39);
                dvcBtn.TabIndex = i;
                //dvcBtn.Click += new System.EventHandler(this.devicesBtn1_Click);
                i++;
                this.Controls.Add(dvcBtn);
                
            }
            this.ResumeLayout(false);
        }*/
        private void LoadButtonsGroup()
        {
            // remove buttons of DevicesBtn
            int count = this.Controls.Count;
            int dis = 0;
            for (int j = 0; j < count; j++)
            {
                if (this.Controls[j - dis].GetType() == typeof(DevicesBtn))
                {
                    this.Controls.Remove(this.Controls[j - dis]);
                    dis++;

                }
            }

            DataTable tmp = GetDeviceBtnGroup();
            int i = 0;
            foreach (DataRow row in tmp.Rows)
            {
                DevicesBtn dvcBtn = new DevicesBtn();
                dvcBtn.Text = row["GroupName"].ToString();
                dvcBtn.GroupID = Convert.ToInt16(row["ID"]);
                dvcBtn.Location = new System.Drawing.Point(49 + (i / 4) * 108, 89 + ((i%4)+1) * 44);
                dvcBtn.Name = "devicesBtn" + i;
                dvcBtn.Size = new System.Drawing.Size(103, 39);
                dvcBtn.TabIndex = i;
                //dvcBtn.Click += new System.EventHandler(this.devicesBtn1_Click);
                i++;
                int groupid = Convert.ToInt16(row["ID"]);
                dvcBtn.Enabled = IsGroupAccessible(groupid);
                this.Controls.Add(dvcBtn);
                _listbtns.Add(dvcBtn);
            }
            this.ResumeLayout(false);
        }
        private bool IsGroupAccessible(int groupid)
        {
            switch (groupid)
            {
                case 0:
                    return Program.user.RoleId == 0;
                case 1:
                    return Program.user.permNhom1 > 0;
                    
                case 2:
                    return Program.user.permNhom2 > 0;
                    
                case 3:
                    return Program.user.permNhom3 > 0;
                    
                case 4:
                    return Program.user.permNhom4 > 0;
                    
                case 5:
                    return Program.user.permNhom5 > 0;
                    
                case 6:
                    return Program.user.permNhom6 > 0;
                    
                case 7:
                    return Program.user.permNhom7 > 0;
                    
                case 8:
                    return Program.user.permNhom8 > 0;
                    
                case 9:
                    return Program.user.permNhom9 > 0;
                    
                case 10:
                    return Program.user.permNhom10 > 0;
                    
                case 11:
                    return Program.user.permNhom11 > 0;
                    
                case 12:
                    return Program.user.permNhom12 > 0;
                    
                case 13:
                    return Program.user.permNhom13 > 0;
                    
                case 14:
                    return Program.user.permNhom14 > 0;
                    
                case 15:
                    return Program.user.permNhom15 > 0;
                    
                case 16:
                    return Program.user.permNhom16 > 0;
                    
                case 17:
                    return Program.user.permNhom17 > 0;
                    
                case 18:
                    return Program.user.permNhom18 > 0;
                    
                case 19:
                    return Program.user.permNhom19 > 0;
                    
                
                default: return false;
            }
        }
        private void devicesBtn1_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            stoplistening();
            this.Close();
        }
        private void stoplistening()
        {
            listeningdev.StopPolling();
            listeningdev.Listener.StopListening();
            listeningdev.Listener.ReceiveBuffer.Flush();
            
        }
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stoplistening();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (Program.user.permUser != 2)
            {
                MessageBox.Show("Bạn không được cấp quyền");
                return;
            }
            Userform userfrm = new Userform(RecloserAcq.Properties.Settings.Default.SQLConnection, Program.user);

            userfrm.StartPosition = FormStartPosition.CenterScreen;
            userfrm.Show();
        }
    }
}
