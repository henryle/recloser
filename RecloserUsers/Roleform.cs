using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using RecloserAcq.Device;

namespace RecloserAcq
{
    public partial class Roleform : Form
    {
        private string _connectionstr;
        public string ConnectionStr
        {
            set
            {
                _connectionstr = value;
            }
        }
        
        //SqlCommandBuilder cmdBuilder;
        //int preIndex;
        string[] permarr = new string[7]{
                "Đồng bộ thời gian","Xem lịch sử",
                "Đóng Recloser","Mở Recloser","Đóng mở tụ bù","Quản lý người dùng","Cấu hình thiết bị"
        };
        string[] groupname;
        //DataTable roletbl;
        public Roleform(string connectionstr)
        {
            InitializeComponent();
            _connectionstr = connectionstr;
        }
        public Roleform()
        {
            InitializeComponent();
            groupname = DeviceStatic.getNameGroup();
        }
        List<RoleObj> listrole;
        RoleObj currole;
        bool loaded = false;
        private void Roleform_Load(object sender, EventArgs e)
        {
            //string strConnection111;
            //strConnection111 = "Data Source=(local);Initial Catalog=FAAccounting;User ID=sa;Password=Goodman99sa";//FA_Accounting.Common.SqlConnectionObject.ConnectionString;
            
            //SqlConnection connectionrole = new SqlConnection(_connectionstr);
            //SqlConnection connectionrole = new SqlConnection(FA_Accounting.Properties.Settings.Default.SqlConnectionString);
            
            //roletbl.AcceptChanges();
            
            //dgvUser.Columns[CONST_PASSWORD].
            listrole = DeviceStatic.getroles();
            bindingSource1.DataSource = listrole;
            lstRole.DataSource = bindingSource1;
            lstRole.DisplayMember = "name";
            lstRole.ValueMember = "id";
            loaded = true;
            /*cbPerm1.SelectedIndex = currole.permsyntime;
            cbPerm2.SelectedIndex = currole.permseeHistory;
            cbPerm3.SelectedIndex = currole.permCloseRecloser;
            cbPerm4.SelectedIndex = currole.permopenRecloser;
            cbPerm5.SelectedIndex = currole.permoperateTubu;
            cbPerm6.SelectedIndex = currole.permUser;
            cbPerm7.SelectedIndex = currole.permConfigCommon;*/
            label1.Text = permarr[0];
            label2.Text = permarr[1];
            label3.Text = permarr[2];
            label4.Text = permarr[3];
            label5.Text = permarr[4];
            label6.Text = permarr[5];
            label7.Text = permarr[6];
            try
            {
                label8.Text = groupname[1];
                label9.Text = groupname[2];
                label10.Text = groupname[3];
                label11.Text = groupname[4];
                label12.Text = groupname[5];
                label13.Text = groupname[6];
                label14.Text = groupname[7];
                label15.Text = groupname[8];
                label16.Text = groupname[9];
                label17.Text = groupname[10];
                label18.Text = groupname[11];
                label19.Text = groupname[12];
                label20.Text = groupname[13];
                label21.Text = groupname[14];
                label22.Text = groupname[15];
                label23.Text = groupname[16];
                label24.Text = groupname[17];
                label25.Text = groupname[18];
                label26.Text = groupname[19];
                //label27.Text = groupname[19];
            }catch(Exception ){
            }
            Dictionary<int, string> choices = new Dictionary<int, string>() { { 0, "Không có quyền" }, { 1, "Chỉ đọc" }, { 2, "Toàn quyền " } }; //, { 3, "Chỉ Edit (Đọc, sửa)" }, { 4, "Đọc, Sửa, Thêm" } };
            
            //tbtest.Rows.Add(new object[]{0});
            //tbtest.Rows.Add(new object[] { 1 });
            //tbtest.Rows.Add(new object[] { 2 });
            //roletbl.Rows[0]["perm1"] = 3;
            //roletbl.AcceptChanges();
            //int i = adapter.Update(roletbl);
            string[] strs = new string[] { "Không có quyền", "Chỉ xem", "Toàn quyền " };
            foreach (Control ctr in this.Controls)
            {
                if (ctr is ComboBox)
                {
                    ((ComboBox)ctr).Items.Clear();
                    foreach (string st in strs)
                    {
                        
                        ((ComboBox)ctr).Items.Add(st);
                    }
                }
            }
            

            //cbPerm1.DataSource = new BindingSource(choices, null);//tbtest;;
            
            
            currole = (RoleObj)bindingSource1.Current;
            //List<string> src = new List<string> { "kkkk", "ccc", "kkkddd" };
            //cbPerm1.DataSource = src;
            //cbPerm1.DisplayMember = "Value";//"items";
            //cbPerm1.ValueMember = "Key";
            
            
            //cbPerm1.BindingContext = this.BindingContext;
            //label9.Text = permarr[8];
            //lstRole.SelectedIndex = 0;
            //preIndex = 0;
            
        }
        private void role2combobox()
        {
            if (currole == null)
                return;

            cbPerm1.SelectedIndex = currole.permsyntime  ;
            cbPerm2.SelectedIndex =currole.permseeHistory ;
            cbPerm4.SelectedIndex=currole.permopenRecloser ;
            cbPerm5.SelectedIndex=currole.permoperateTubu ;
            cbPerm6.SelectedIndex=currole.permUser ;
            cbPerm3.SelectedIndex=currole.permCloseRecloser ;
            cbPerm7.SelectedIndex=currole.permConfigCommon ;
            cbPerm8.SelectedIndex=currole.permNhom1 ;
            cbPerm9.SelectedIndex=currole.permNhom2 ;
            cbPerm10.SelectedIndex=currole.permNhom3 ;
            cbPerm11.SelectedIndex=currole.permNhom4 ;
            cbPerm12.SelectedIndex=currole.permNhom5 ;
            cbPerm13.SelectedIndex=currole.permNhom6 ;
            cbPerm14.SelectedIndex=currole.permNhom7 ;
            cbPerm15.SelectedIndex=currole.permNhom8 ;
            cbPerm16.SelectedIndex=currole.permNhom9 ;
            cbPerm17.SelectedIndex = currole.permNhom10;
            cbPerm18.SelectedIndex = currole.permNhom11;
            cbPerm19.SelectedIndex = currole.permNhom12;
            cbPerm20.SelectedIndex = currole.permNhom13;
            cbPerm21.SelectedIndex = currole.permNhom14;
            cbPerm22.SelectedIndex = currole.permNhom15;
            cbPerm23.SelectedIndex = currole.permNhom16;
            cbPerm24.SelectedIndex = currole.permNhom17;
            cbPerm25.SelectedIndex = currole.permNhom18;
            cbPerm26.SelectedIndex = currole.permNhom19;
        }
        private void combox2role()
        {
            if (currole == null)
            {
                return;
            }
            currole.permsyntime = (UInt16)cbPerm1.SelectedIndex;
            currole.permseeHistory = (UInt16)cbPerm2.SelectedIndex;
            currole.permopenRecloser = (UInt16)cbPerm4.SelectedIndex;
            currole.permoperateTubu = (UInt16)cbPerm5.SelectedIndex;
            currole.permUser = (UInt16)cbPerm6.SelectedIndex;
            currole.permCloseRecloser = (UInt16)cbPerm3.SelectedIndex;
            currole.permConfigCommon = (UInt16)cbPerm7.SelectedIndex;
            currole.permNhom1 = (UInt16)cbPerm8.SelectedIndex;
            currole.permNhom2 = (UInt16)cbPerm9.SelectedIndex;
            currole.permNhom3 = (UInt16)cbPerm10.SelectedIndex;
            currole.permNhom4 = (UInt16)cbPerm11.SelectedIndex;
            currole.permNhom5 = (UInt16)cbPerm12.SelectedIndex;
            currole.permNhom6 = (UInt16)cbPerm13.SelectedIndex;
            currole.permNhom7 = (UInt16)cbPerm14.SelectedIndex;
            currole.permNhom8 = (UInt16)cbPerm15.SelectedIndex;
            currole.permNhom9 = (UInt16)cbPerm16.SelectedIndex;
            currole.permNhom10 = (UInt16)cbPerm17.SelectedIndex;
            currole.permNhom11 = (UInt16)cbPerm18.SelectedIndex;
            currole.permNhom12 = (UInt16)cbPerm19.SelectedIndex;
            currole.permNhom13 = (UInt16)cbPerm20.SelectedIndex;
            currole.permNhom14 = (UInt16)cbPerm21.SelectedIndex;
            currole.permNhom15 = (UInt16)cbPerm22.SelectedIndex;
            currole.permNhom16 = (UInt16)cbPerm23.SelectedIndex;
            currole.permNhom17 = (UInt16)cbPerm24.SelectedIndex;
            currole.permNhom18 = (UInt16)cbPerm25.SelectedIndex;
            currole.permNhom19 = (UInt16)cbPerm26.SelectedIndex;

        }
        private void lstRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                combox2role();
                currole = (RoleObj)bindingSource1.Current;
                role2combobox();
            }
            /*roletbl.Rows[preIndex].EndEdit();
            string item = lstRole.SelectedItem.ToString();
            roletbl.DefaultView.RowFilter = "Name ='" + item + "'";
            //string item = lstRole.SelectedItem.ToString();
            //DataRow[] rows = roletbl.Select("Name ='" + item + "'");
            //if (rows != null && rows.Length > 0)
            //{

            //    DataRow row = rows[0];
            //    cbPerm1.SelectedIndex = Convert.ToInt16(row["perm1"]);
            //    cbPerm2.SelectedIndex = Convert.ToInt16(row["perm2"]);
            //    cbPerm3.SelectedIndex = Convert.ToInt16(row["perm3"]);
            //    cbPerm4.SelectedIndex = Convert.ToInt16(row["perm4"]);
            //    cbPerm5.SelectedIndex = Convert.ToInt16(row["perm5"]);
            //    cbPerm6.SelectedIndex = Convert.ToInt16(row["perm6"]);
            //    cbPerm7.SelectedIndex = Convert.ToInt16(row["perm7"]);
            //    cbPerm8.SelectedIndex = Convert.ToInt16(row["perm8"]);
            //    //cbPerm9.SelectedIndex = Convert.ToInt16(row["perm9"]);
            //}
            preIndex = lstRole.SelectedIndex;*/
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            //adapter.AcceptChangesDuringUpdate=true;
            //string str = cmdBuilder.GetUpdateCommand().CommandText;
            //string str1 = roletbl.Rows[0]["perm1"].ToString();
            //roletbl.Rows[0]["perm1"] = cbPerm1.SelectedIndex;
            //roletbl.Rows[preIndex].EndEdit();
            try
            {
                combox2role();
                foreach (RoleObj obj in listrole)
                {
                    obj.Update();
                }
                MessageBox.Show("User privileges saved successfully");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void lstRole_Validating(object sender, CancelEventArgs e)
        {
            int i = cbPerm1.SelectedIndex;

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

      
    }
}
