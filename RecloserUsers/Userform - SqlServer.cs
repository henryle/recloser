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
//using FA_Accounting.Common;
namespace RecloserAcq
{
    public partial class Userform : Form
    {
        private string _strcon;
        SqlDataAdapter adapter;
        DataTable usertbl;
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
        public Userform(string constr,UserObj user)
        {
            InitializeComponent();
            _strcon = constr;
            _user = user;
        }

        private void User_Load(object sender, EventArgs e)
        {
            //string strConnection111;
            //strConnection111 = "Data Source=(local);Initial Catalog=FAAccounting;User ID=sa;Password=Goodman99sa";//FA_Accounting.Common.SqlConnectionObject.ConnectionString;
            SqlConnection connectionuser = new SqlConnection(_strcon);
            
            //SqlConnection connectionuser = new SqlConnection(FA_Accounting.Properties.Settings.Default.SqlConnectionString);

            string queryString = "SELECT [Users].ID, [Users].name as Username,role.name as Role, roleid, password FROM [Users] left join [Role]  on [Users].roleId = role.id ";
            adapter = new SqlDataAdapter(queryString, connectionuser);
            #region createUpdateCommand
            /*SqlCommand DAUpdateCmd;
            DAUpdateCmd = new SqlCommand("Update USER set Name = @pName, where Id = @pId", connectionuser);
            DAUpdateCmd.Parameters.Add(new SqlParameter("@pName", SqlDbType.VarChar));
            DAUpdateCmd.Parameters["@pName"].SourceVersion = DataRowVersion.Current;
            DAUpdateCmd.Parameters["@pName"].SourceColumn = "Name";

            DAUpdateCmd.Parameters.Add(new SqlParameter("@pAddress", SqlDbType.VarChar));
            DAUpdateCmd.Parameters["@pAddress"].SourceVersion = DataRowVersion.Current;
            DAUpdateCmd.Parameters["@pAddress"].SourceColumn = "Address";

            DAUpdateCmd.Parameters.Add(new SqlParameter("@pTk", SqlDbType.VarChar));
            DAUpdateCmd.Parameters["@pTk"].SourceVersion = DataRowVersion.Current;
            DAUpdateCmd.Parameters["@pTk"].SourceColumn = "tk";

            DAUpdateCmd.Parameters.Add(new SqlParameter("@pTaxcode", SqlDbType.VarChar));
            DAUpdateCmd.Parameters["@pTaxcode"].SourceVersion = DataRowVersion.Current;
            DAUpdateCmd.Parameters["@pTaxcode"].SourceColumn = "Taxcode";



            DAUpdateCmd.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
            DAUpdateCmd.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
            DAUpdateCmd.Parameters["@pId"].SourceColumn = "id";

            //Assign the SqlCommand to the UpdateCommand property of the SqlDataAdapter.
            adapter.UpdateCommand = DAUpdateCmd;*/
            #endregion
            #region createInsertCommand
            /*SqlCommand DAInsertCmd;
            DAInsertCmd = new SqlCommand("Insert into CRM (Code,Name,Address,tk,taxcode) values (@pCode, @pName, @pAddress, @pTk, @pTaxcode)", connectionuser);

            DAInsertCmd.Parameters.Add(new SqlParameter("@pCode", SqlDbType.VarChar));
            DAInsertCmd.Parameters["@pCode"].SourceVersion = DataRowVersion.Current;
            DAInsertCmd.Parameters["@pCode"].SourceColumn = "Code";

            DAInsertCmd.Parameters.Add(new SqlParameter("@pName", SqlDbType.VarChar));
            DAInsertCmd.Parameters["@pName"].SourceVersion = DataRowVersion.Current;
            DAInsertCmd.Parameters["@pName"].SourceColumn = "Name";

            DAInsertCmd.Parameters.Add(new SqlParameter("@pAddress", SqlDbType.VarChar));
            DAInsertCmd.Parameters["@pAddress"].SourceVersion = DataRowVersion.Current;
            DAInsertCmd.Parameters["@pAddress"].SourceColumn = "Address";

            DAInsertCmd.Parameters.Add(new SqlParameter("@pTk", SqlDbType.VarChar));
            DAInsertCmd.Parameters["@pTk"].SourceVersion = DataRowVersion.Current;
            DAInsertCmd.Parameters["@pTk"].SourceColumn = "tk";

            DAInsertCmd.Parameters.Add(new SqlParameter("@pTaxcode", SqlDbType.VarChar));
            DAInsertCmd.Parameters["@pTaxcode"].SourceVersion = DataRowVersion.Current;
            DAInsertCmd.Parameters["@pTaxcode"].SourceColumn = "Taxcode";

            adapter.InsertCommand = DAInsertCmd;*/
            #endregion
            #region create delete command
            SqlCommand DADeleteCmd;
            DADeleteCmd = new SqlCommand("Delete [Users] where id = @pId", connectionuser);

            //DADeleteCmd.Parameters.Add(new SqlParameter("@pCode", SqlDbType.VarChar));
            //DADeleteCmd.Parameters["@pCode"].SourceVersion = DataRowVersion.Current;
            //DADeleteCmd.Parameters["@pCode"].SourceColumn = "Code";
            DADeleteCmd.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
            DADeleteCmd.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
            DADeleteCmd.Parameters["@pId"].SourceColumn = "id";
            adapter.DeleteCommand = DADeleteCmd;
            #endregion
            usertbl = new DataTable();
            adapter.Fill(usertbl);
            
            dgvUser.DataSource = usertbl;
            dgvUser.Columns[CONST_ID].Width = 40;
            dgvUser.Columns[CONST_NAME].Width = 80;
            //dgvUser.Columns[CONST_PASSWORD].Width = 255;
            dgvUser.Columns[CONST_ROLE].Width = 80;
            dgvUser.ReadOnly = true;
            //dgvUser.Columns[CONST_ID].ReadOnly = true;
            //dgvUser.Columns[CONST_ROLE].ReadOnly = true;
            //dgvUser.Columns[CONST_NAME].ReadOnly = true;
            //dgvUser.Columns[CONST_LOGGED].ReadOnly = true;
            //dgvUser.Columns[CONST_COMPUTER].ReadOnly = true;
            //dgvUser.Columns[CONST_LASTLOGIN].ReadOnly = true;
            

            dgvUser.Columns[CONST_ROLEID].Visible = false;
            dgvUser.Columns[CONST_PASSWORD].Visible = false;
            //dgvUser.Columns[CONST_PASSWORD].
            btnEditRole.Enabled = (_user.perm6 == 2);
            btncedituser.Enabled = (_user.perm6 == 2); 
            btnDelete.Enabled = (_user.perm6 == 2);
            btnInsert.Enabled = (_user.perm6 == 2);
        }

        private void btnchangepass_Click(object sender, EventArgs e)
        {
            if (_user.name != dgvUser.CurrentRow.Cells[CONST_NAME].Value.ToString() && _user.permUser != 2)
            {
                MessageBox.Show("Bạn không có quyền edit user khác, vui lòng chọn username của bạn trong danh sách để edit", "FA-Accounting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int userid = Convert.ToInt16(dgvUser.CurrentRow.Cells[CONST_ID].Value);
            string username = dgvUser.CurrentRow.Cells[CONST_NAME].Value.ToString();
            int roleid = Convert.ToInt16(dgvUser.CurrentRow.Cells[CONST_ROLEID].Value);
            EditUser ed = new EditUser(userid,username,roleid ,_user.permUser);
            if (ed.ShowDialog() == DialogResult.OK)
            {
                usertbl.Clear();
                adapter.Fill(usertbl);
            }
        }

        private void btnEditRole_Click(object sender, EventArgs e)
        {
            Roleform rl = new Roleform(_strcon);
            rl.Show();

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            EditUser ed = new EditUser();
            if (ed.ShowDialog() == DialogResult.OK)
            {
                usertbl.Clear();
                adapter.Fill(usertbl);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dgvUser.Rows.Remove(dgvUser.CurrentRow);
            adapter.Update(usertbl);
        }
    }
}
