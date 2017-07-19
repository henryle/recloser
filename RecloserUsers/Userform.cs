using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using RecloserUsers;
using RecloserAcq.Device;
//using FA_Accounting.Common;
namespace RecloserAcq
{
    public partial class Userform : Form
    {
        private string _strcon;
        
        
        //bool changed = false;
        const int CONST_ID = 0;
        const int CONST_NAME = 1;
        const int CONST_PASSWORD = 4;
        const int CONST_ROLEID = 3;
        const int CONST_ROLE = 2;
        const int CONST_LOGGED = 5;
        const int CONST_LASTLOGIN = 6;
        const int CONST_COMPUTER = 7;
        private UserObj _user;
        List<UserObj> listofUsers;
        public Userform(string constr,UserObj user)
        {
            InitializeComponent();
            _strcon = constr;
            _user = user;
            dgvUser.AutoGenerateColumns = false;
        }

        private void User_Load(object sender, EventArgs e)
        {
            //string strConnection111;
            //strConnection111 = "Data Source=(local);Initial Catalog=FAAccounting;User ID=sa;Password=Goodman99sa";//FA_Accounting.Common.SqlConnectionObject.ConnectionString;
            
            listofUsers = DeviceStatic.getUserList();
            //SqlConnection connectionuser = new SqlConnection(FA_Accounting.Properties.Settings.Default.SqlConnectionString);

            
            //dgvUser.DataSource = listofUsers;
            userObjBindingSource.DataSource = listofUsers;
            dgvUser.ReadOnly = true;
            //dgvUser.Columns[CONST_ID].ReadOnly = true;
            //dgvUser.Columns[CONST_ROLE].ReadOnly = true;
            //dgvUser.Columns[CONST_NAME].ReadOnly = true;
            //dgvUser.Columns[CONST_LOGGED].ReadOnly = true;
            //dgvUser.Columns[CONST_COMPUTER].ReadOnly = true;
            //dgvUser.Columns[CONST_LASTLOGIN].ReadOnly = true;
       
       //     dgvUser.Columns[CONST_ROLEID].Visible = false;
       //     dgvUser.Columns[CONST_PASSWORD].Visible = false;
            //dgvUser.Columns[CONST_PASSWORD].
            btnEditRole.Enabled = (_user.permUser == 2);
            btncedituser.Enabled = (_user.permUser == 2); 
            btnDelete.Enabled = (_user.permUser == 2);
            btnInsert.Enabled = (_user.permUser == 2);
        }

        private void btnchangepass_Click(object sender, EventArgs e)
        {
            UserObj selecteduser = (UserObj )userObjBindingSource.Current;
            if (_user.name != selecteduser.name && _user.permUser != 2)
            {
                MessageBox.Show("Bạn không có quyền edit user khác, vui lòng chọn username của bạn trong danh sách để edit", "Recloser", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int userid = selecteduser.id;
            string username = selecteduser.name;
            int roleid = selecteduser.RoleId;
            //EditUser ed = new EditUser(userid,username,roleid ,_user.permUser);
            EditUser ed = new EditUser(selecteduser);
            if (ed.ShowDialog() == DialogResult.OK)
            {
                listofUsers = DeviceStatic.getUserList();
                dgvUser.Refresh();
            }
        }

        private void btnEditRole_Click(object sender, EventArgs e)
        {
            Roleform rl = new Roleform();
            rl.Show();

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            EditUser ed = new EditUser();
            if (ed.ShowDialog() == DialogResult.OK)
            {
                userObjBindingSource.Add(ed.user);
                dgvUser.Update();
                dgvUser.Refresh();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            UserObj obj = (UserObj)userObjBindingSource.Current;
            userObjBindingSource.Remove(obj);


            DeviceStatic.DeleteUser(obj.id);
            
            dgvUser.Update();
            dgvUser.Refresh();
        }
    }
}
