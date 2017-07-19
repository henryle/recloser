using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using FA_Accounting.Common;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using FA_Accounting.Common;
using RecloserAcq.Device;

namespace RecloserAcq
{
    public partial class UserLogin : Form
    {
        public int UserId {get; set;}
        public Int16 ky=0;
        public UserObj user;
        //private string savedpsw;
        //private string savedusername;
        const int CONST_ID = 0;
        const int CONST_LOGGED = 1;
        const int CONST_PERM1 = 2;
            const int CONST_PERM2 = 3;
                const int CONST_PERM3 = 4;
                    const int CONST_PERM4 = 5;
                        const int CONST_PERM5 = 6;
                            const int CONST_PERM6 = 7;
                                const int CONST_PERM7 = 8;
        const int CONST_PERM8 = 9;
        const int CONST_PERM9 = 10;
        const int CONST_PERM10 = 11;
        const int CONST_COMP = 12;
        const int CONST_LASTLOGIN = 13;
        //SqlConnection connectionUser;
        //SqlCommand sqlcmd;
        private string _strcon = string.Empty;

        public UserLogin()
        {
            //_strcon = connstring;
            InitializeComponent();
            //connectionUser = new SqlConnection(_strcon);
            //connectionUser = new SqlConnection(FA_Accounting.Properties.Settings.Default.SqlConnectionString);
            //string sqlstring = "select [Users].id, logged, perm1, perm2, perm3, perm4, perm5, perm6, perm7, perm8, computer, lastlogin from [Users] left outer join role on [Users].roleid = role.id where [Users].name = '" + username + "' and Password = '" + pwd + "'";
            
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string pwd = Encrypt.EncodePassword(txtPwd.Text);
            string username = txtUser.Text.Trim();
            this.user = DeviceStatic.getUserLogin(username, pwd);
            if (user == null)
            {
                MessageBox.Show("Tên hoặc mật khẩu không đúng");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
           /* if (connectionUser.State != ConnectionState.Open)
            {
                connectionUser.Open();
            }
            string pwd = Encrypt.EncodePassword(txtPwd.Text);
            string username = txtUser.Text.Trim();


            sqlcmd.Parameters["@username"].Value = username;
            sqlcmd.Parameters["@password"].Value = pwd;
            //DataTable ret = SqlConnectionObject.ReturnDataTable(sqlstring,connectionUser);
            
            //SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlcmd);
            
            SqlDataReader dr = sqlcmd.ExecuteReader(CommandBehavior.SingleRow);
            //sqlAdapter.Fill(ret);
            
            //if (ret != null && ret.Rows.Count>0)
            if (dr.Read())
            {
                //bool logged = Convert.ToBoolean(dr[CONST_LOGGED]);
                user = new UserObj();
                //user.id = Convert.ToInt32(dr[CONST_ID]);
                user.id = Convert.ToInt32(dr[CONST_ID]);
                ////bool logged = Convert.ToBoolean(ret.Rows[0][CONST_LOGGED]);
                //user.perm1 = Convert.ToUInt16(ret.Rows[0][CONST_PERM1]);
                //perm1, perm2, perm3, perm4, perm5, perm6, perm7, perm8, perm9,perm10
                user.perm1 = Convert.ToUInt16(dr["perm1"]);
                user.perm2 = Convert.ToUInt16(dr["perm2"]);
                user.perm3 = Convert.ToUInt16(dr["perm3"]);
                user.perm4 = Convert.ToUInt16(dr["perm4"]);
                user.perm5 = Convert.ToUInt16(dr["perm5"]);
                user.perm6 = Convert.ToUInt16(dr["perm6"]);
                user.perm7 = Convert.ToUInt16(dr["perm7"]);
                user.permUser = Convert.ToUInt16(dr[CONST_PERM8]);
                user.permConfigCommon = Convert.ToUInt16(dr[CONST_PERM9]);
                //user.permConfigDatabase = Convert.ToUInt16(dr[CONST_PERM10]);
                //user.lastlogin = Convert.ToDateTime(dr[CONST_LASTLOGIN]);
                //user.computer = dr[CONST_COMP].ToString();
                user.fullname = dr["fullname"].ToString();
                //string curcomp = System.Environment.MachineName;
                //string logedcomp = dr[CONST_COMP].ToString();
                dr.Close();
               
                
                // do next
                
                //LogService.WriteInfo("User Login", user);
                user.name = username;
                // log to a file 
                // if this user account currently loged in from another computer
                // if this user acocunt currently loged in in the same computer
                
                
                    // save chkRempass, name , encrypted password in application config file
                
                
                //string sqlupdate = "update [Users] set logged = 1, computer ='" + curcomp + "', lastlogin = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"' where id = " + user.id;
                //SqlCommand sqlupdate = new SqlCommand("sp_updateuserloggedin", connectionUser);
                //sqlupdate.Parameters.AddWithValue("@curcomp", curcomp);
                //sqlupdate.Parameters.AddWithValue("@userid", user.id);
                //sqlupdate.CommandType = CommandType.StoredProcedure;

                //sqlupdate.ExecuteNonQuery();
                if (this.checkBox1.Checked)
                {//doi password
                    UserChgpass frm = new UserChgpass(this.user.name,_strcon);
                    frm.ShowDialog();
                }
                this.DialogResult = DialogResult.OK;
                
            }
            else
            {
                MessageBox.Show("Tên hoặc mật khẩu không đúng");
                //this.DialogResult = DialogResult.Cancel;
            }
            dr.Close(); // oldcode Sql server
            */
        }

        
        private void UserLogin_Load(object sender, EventArgs e)
        {
            // get chkRempass 
            
            
            /*string sqlstring = "sp_getuser";
           
            sqlcmd = new SqlCommand(sqlstring, connectionUser);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar));
            sqlcmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar));*/
            
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            
        }

        
    }
}
