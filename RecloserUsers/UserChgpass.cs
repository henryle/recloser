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

namespace RecloserAcq
{
    public partial class UserChgpass : Form
    {

        private string _strcon = string.Empty;

        
        
        
     
        SqlConnection connectionUser;
        SqlCommand sqlcmd;
        string username;
        public UserChgpass(string username,string connectionstr)
        {
            InitializeComponent();
            _strcon = connectionstr;
            connectionUser = new SqlConnection(_strcon);
            //connectionUser = new SqlConnection(FA_Accounting.Properties.Settings.Default.SqlConnectionString);
            //string sqlstring = "select [Users].id, logged, perm1, perm2, perm3, perm4, perm5, perm6, perm7, perm8, computer, lastlogin from [Users] left outer join role on [Users].roleid = role.id where [Users].name = '" + username + "' and Password = '" + pwd + "'";
            this.username = username;
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtconfirm.Text!=txtPwd.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không đúng");
                return;
            }
            try
            {
                if (connectionUser.State != ConnectionState.Open)
                {
                    connectionUser.Open();
                }
                string pwd = Encrypt.EncodePassword(txtPwd.Text);



                sqlcmd.Parameters["@username"].Value = this.username;
                sqlcmd.Parameters["@password"].Value = pwd;
                //DataTable ret = SqlConnectionObject.ReturnDataTable(sqlstring,connectionUser);

                //SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlcmd);

                sqlcmd.ExecuteNonQuery();
                //sqlAdapter.Fill(ret);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.DialogResult = DialogResult.OK;
                return;
            }
                
             
                
            
            MessageBox.Show("Đổi password thành công!");
                //this.DialogResult = DialogResult.Cancel;
            this.Close();
            
        }

        
        private void UserChgpass_Load(object sender, EventArgs e)
        {
            // get chkRempass 
            
            
            string sqlstring = "sp_changepassword";
           
            sqlcmd = new SqlCommand(sqlstring, connectionUser);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar));
            sqlcmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar));
            
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
    }
}
