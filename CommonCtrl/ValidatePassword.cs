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
    public partial class ValidatePassword : Form
    {
        public bool commandvalidate = true;
        public string _type;
        // type: tubu , nulec , cooper , 351 , elster
        /// <summary>
        /// FALSE IF IT'S LOG IN VALIDATE PASSWORD
        /// TRUE IF IT'S COMMAND VALIDATE PASSWORD
        /// </summary>
        /// <param name="cmd"></param>
        public ValidatePassword(bool cmd, string devicetype ="")
        {
            commandvalidate = cmd;
            _type = devicetype;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Password.getpassword();
            if (commandvalidate == false)
            {
                if (Password.ValidateMainPassword(txtOldPassword.Text.Trim()))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Password isn't correct");
                    return;
                }
            }
            else
            {
                string enterpassword = txtOldPassword.Text.Trim();
                int indexofType = enterpassword.LastIndexOf(_type);
                if (indexofType < 0)
                {
                    MessageBox.Show("Sai password");
                    return;
                }
                string expectedpassword = enterpassword.Substring(0, indexofType);
                if (Password.ValidatePassword(expectedpassword))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Password isn't correct");
                    return;
                }
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return;
        }
    }
}


