using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FA_Accounting.Common;

namespace RecloserAcq
{
    public partial class ChangePassword : Form
    {
        public bool commandvalidate = true;
        public ChangePassword(bool cmd)
        {
            commandvalidate = cmd;
            InitializeComponent();

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (commandvalidate == true)
            {
                if (Password.ValidatePassword(txtOldPassword.Text.Trim()) == true)
                {
                    if (txtNewPassword.Text.Trim() == txtConfirmnew.Text.Trim())
                    {
                        Password.SaveNewCommandPassword(txtNewPassword.Text.Trim());
                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Confirmed password isn't matched");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Old Password is not correct");
                }
            }
            else
            {
                if (Password.ValidateMainPassword(txtOldPassword.Text.Trim()) == true)
                {
                    if (txtNewPassword.Text.Trim() == txtConfirmnew.Text.Trim())
                    {
                        Password.SaveNewMainPassword(txtNewPassword.Text.Trim());
                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Confirmed password isn't matched");
                        return;
                    }
                }
                MessageBox.Show("Old Password is not correct");
            }

        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            Password.getpassword();
        }
    }
}
