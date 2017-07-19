using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FA_Accounting.Common;
using RecloserAcq;
using RecloserAcq.Device;

namespace RecloserUsers
{
    public partial class EditUser : Form
    {
        
        //int alloweditrole; //Program.user.permUser
        bool newuser = false;
        public UserObj user = new UserObj();
        public EditUser()
        {
            InitializeComponent();
            newuser = true;

        }
        public EditUser(UserObj puser)
        {
            InitializeComponent();
            user = puser;
            newuser = false;
            //this.alloweditrole = Program.;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try{
                if (txtpassword.Text != txtconfirmpass.Text)
                {
                    MessageBox.Show("Passwords do not match","RecloserAcq", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                user.name = this.txtusername.Text.Trim() ;
                user.RoleId = (int)cbrole.SelectedValue;
                user.RoleName = cbrole.Text;
                //roleid = " + this.cbrole.SelectedIndex + ", password ='" + Encrypt.EncodePassword(txtpassword.Text) + "' WHere id = " + userid;
                if (!string.IsNullOrEmpty(txtpassword.Text))
                {
                    user.password = Encrypt.EncodePassword(txtpassword.Text);
                    //sqlupdate = "Update [Users] set name ='" + this.txtusername.Text + "'" + ",roleid = " + this.cbrole.SelectedIndex + " WHere id = " + userid;   
                }
                else
                {
                    if (newuser)
                    {
                        MessageBox.Show("Password is empty");
                        return;
                    }
                }
                if (newuser)
                {
                    //DeviceStatic.DeleteUser
                    DeviceStatic.InsertUser(user);
                    //sqlupdate = "Insert into [Users] (name,roleid,password) values ('" + txtusername.Text + "'," + cbrole.SelectedIndex + ",'" + Encrypt.EncodePassword(txtpassword.Text) + "')";
                }
                else
                {
                

                    DeviceStatic.UpdateUser(user);
                    
                
                }
            
                this.DialogResult = DialogResult.OK;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        List<RoleObj> listroles;
        private void EditUser_Load(object sender, EventArgs e)
        {
            
            listroles  = DeviceStatic.getroles();
            
                BindingSource dtbinding = new BindingSource();
                dtbinding.DataSource = listroles;
                cbrole.DataSource = dtbinding;
                cbrole.DisplayMember = "name";
                //cbrole.DataBindings.Add("SelectedIndex", user.RoleId, null);
                cbrole.ValueMember = "id";
                cbrole.SelectedValue = user.RoleId;

            txtusername.Text = user.name;
            //cbrole.Enabled = alloweditrole == 2;
            if (newuser)
            {
                lb5.Visible = false;
                cbrole.Enabled = true;
            }
        }
    }
}
