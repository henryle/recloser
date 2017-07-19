using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecloserAcq.Device;
namespace RecloserAcq
{
    public partial class MapTuBuCtrl : UserControl
    {
        private RecloserBase _rc;
        private List<RecloserBase> _tubulist;
        public MapTuBuCtrl(RecloserBase rc, List<RecloserBase> tubulist)
        {
            _rc = rc;
            _tubulist = tubulist;
            InitializeComponent();
            
        }

        private void MapTuBuCtrl_Load(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = _tubulist;
            lstTubu.DataSource = bs;
            lstTubu.DisplayMember = "Location";
            lstTubu.ValueMember = "Id";

            //BindingSource bs1 = new BindingSource();
            //bs1.DataSource = _tubulist;
            //lstClose.DataSource = bs1;
            lstClose.DisplayMember = "Location";
            lstClose.ValueMember = "Id";
            
            //BindingSource bs2 = new BindingSource();
            //bs2.DataSource = _tubulist;
            //lstOpen.DataSource = bs2;
            lstOpen.DisplayMember = "Location";
            lstOpen.ValueMember = "Id";
            
            //lstClose.Items.Clear();
            //lstOpen.Items.Clear();
            string[] stra = _rc.TuBuClose.Split(',');
            for (int i = 0; i < stra.Count() ; i++)
            {
                try
                {
                    int id = Convert.ToInt32(stra[i]);
                    lstClose.Items.Add(_tubulist.Find(k => k.Id == id));
                }
                catch { }
            }
            string[] str1a = _rc.TuBuOpen.Split(',');
            for (int i = 0; i < str1a.Count() ; i++)
            {
                try
                {
                    int id = Convert.ToInt32(str1a[i]);
                    lstOpen.Items.Add(_tubulist.Find(k => k.Id == id));
                }
                catch { }
            }
            
            chkCosphi.Checked = _rc.controlbyNo_Cosphi_Q == eControlType.Cosphi;
            chkQ.Checked = _rc.controlbyNo_Cosphi_Q == eControlType.Q;
            txtMaxClose.Text = _rc.MaxClose.ToString();
            txtMinOpen.Text = _rc.MinOpen.ToString();

            txtMaxQClose.Text = _rc.MaxQClose.ToString();
            txtMinQOpen.Text = _rc.MinQOpen.ToString();
            txtSecondWait.Text = _rc.SecondWait.ToString();
        }
        /// <summary>
        /// When click Save on Dieu Khien Tu Bu configuration
        /// </summary>
        public void Ret()
        {
            if (chkCosphi.Checked)
            {
                _rc.controlbyNo_Cosphi_Q = eControlType.Cosphi;
            }
            else if (chkQ.Checked)
            {
                _rc.controlbyNo_Cosphi_Q = eControlType.Q;
            }
            else
            {
                _rc.controlbyNo_Cosphi_Q = eControlType.No;
                return;
            }
            _rc.MaxClose = (float)Convert.ToDecimal(txtMaxClose.Text);
            _rc.MinOpen = (float)Convert.ToDecimal(txtMinOpen.Text);
            _rc.MinQOpen = (float)Convert.ToDecimal(txtMinQOpen.Text);
            _rc.MaxQClose = (float)Convert.ToDecimal(txtMaxQClose.Text);
           
            _rc.SecondWait = Convert.ToInt32(txtSecondWait.Text);
            //_rc.TuBuClose = 

            StringBuilder builder = new StringBuilder();
            bool first = true;

            foreach (RecloserBase rctmp in lstClose.Items)
            {
                if (first == true)
                {
                    first = false;
                }
                else
                {
                    builder.Append(',');
                }
                builder.Append(rctmp.Id.ToString());
            }
            _rc.TuBuClose = builder.ToString();
            builder.Clear();
            first = true;
            foreach (RecloserBase rctmp in lstOpen.Items)
            {
                if (first == true)
                {
                    first = false;
                }
                else
                {
                    builder.Append(',');
                }

                builder.Append(rctmp.Id.ToString());
                
            }
            _rc.TuBuOpen = builder.ToString();
            if (_rc.controlbyNo_Cosphi_Q == eControlType.Q && _rc.MaxQClose == 0)
            {
                MessageBox.Show("Incorrect setting");
                
                _rc.controlbyNo_Cosphi_Q = eControlType.No;
            }
            if (_rc.controlbyNo_Cosphi_Q == eControlType.Cosphi && _rc.MaxClose == 0)
            {
                MessageBox.Show("Incorrect setting");
                
                _rc.controlbyNo_Cosphi_Q = eControlType.No;
            }

        }
        private void btnRight_Click(object sender, EventArgs e)
        {
            RecloserBase rc = (RecloserBase)lstTubu.SelectedItem;
            lstClose.Items.Add(rc);
            
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            lstClose.Items.RemoveAt(lstClose.SelectedIndex);
        }

        private void btnRight1_Click(object sender, EventArgs e)
        {
            RecloserBase rc = (RecloserBase)lstTubu.SelectedItem;
            lstOpen.Items.Add(rc);
        }

        private void btnLeft2_Click(object sender, EventArgs e)
        {
            lstOpen.Items.RemoveAt(lstOpen.SelectedIndex);
        }

        private void chkQ_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkQ.Checked)
            {
                return;
            }
            else
            {
                if (chkCosphi.Checked)
                {
                    chkCosphi.Checked = false;
                }
            }
        }

        private void chkCosphi_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkCosphi.Checked)
            {
                return;
            }
            else
            {
                if (chkQ.Checked)
                {
                    chkQ.Checked = false;
                }
            }
        }
    }
}
