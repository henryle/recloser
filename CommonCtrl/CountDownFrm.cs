using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecloserAcq
{
    public partial class CountDownFrm : Form
    {
        int countdownsecond;
        bool ok = true;
        public CountDownFrm(string text, int countdownsecond)
        {
            
            InitializeComponent();
            this.countdownsecond = countdownsecond;
            label1.Text = text;
            lbSecond.Text = countdownsecond.ToString();
        }
        public CountDownFrm(string text, int countdownsecond,string text1)
        {

            InitializeComponent();
            this.countdownsecond = countdownsecond;
            label1.Text = text;
            lbSecond.Text = countdownsecond.ToString();
            label3.Text = text1;
        }
        private void btnAbort_Click(object sender, EventArgs e)
        {
            this.ok = false;
            this.Close();
            
            return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (countdownsecond > 0)
            {
                countdownsecond -= 1;
                lbSecond.Text = countdownsecond.ToString();
                this.Refresh();
            }
            else
            {
                this.ok = true;
                this.DialogResult = DialogResult.OK;
                
                this.Close();
                
                return;
            }
        }

        private void CountDownFrm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void CountDownFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.ok && this.DialogResult!=DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
