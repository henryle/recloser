namespace RecloserAcq
{
    partial class frmConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label9;
            this.label1 = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.recloserListView1 = new RecloserAcq.RecloserListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numMaxCurrent = new System.Windows.Forms.NumericUpDown();
            this.numCurrentDuration = new System.Windows.Forms.NumericUpDown();
            this.btnchangepassword = new System.Windows.Forms.Button();
            this.btnChangeLogin = new System.Windows.Forms.Button();
            this.txtdvpath = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAddNulec = new System.Windows.Forms.Button();
            this.btnAddCooper = new System.Windows.Forms.Button();
            this.btnAddRecloserSel = new System.Windows.Forms.Button();
            this.btnAddDKDT = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.nmsoundduration = new System.Windows.Forms.NumericUpDown();
            this.txtAlertSoundFile = new System.Windows.Forms.TextBox();
            this.btnaddRelADVC45 = new System.Windows.Forms.Button();
            this.btnAddRecloserADVC = new System.Windows.Forms.Button();
            this.btnAddNulecUseries = new System.Windows.Forms.Button();
            this.btnAdvcTcpIp = new System.Windows.Forms.Button();
            this.nmplaycount = new System.Windows.Forms.NumericUpDown();
            this.btnAddLBS = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmsoundduration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmplaycount)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(28, 20);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(50, 13);
            label4.TabIndex = 4;
            label4.Text = "Duration:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(139, 20);
            label5.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(47, 13);
            label5.TabIndex = 8;
            label5.Text = "seconds";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(28, 46);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(162, 13);
            label6.TabIndex = 9;
            label6.Text = "Notify when current greater than:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(251, 46);
            label7.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(14, 13);
            label7.TabIndex = 11;
            label7.Text = "A";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(787, 372);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(82, 13);
            label10.TabIndex = 11;
            label10.Text = "Device log files:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(520, 66);
            label8.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(47, 13);
            label8.TabIndex = 31;
            label8.Text = "seconds";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(362, 66);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(84, 13);
            label3.TabIndex = 32;
            label3.Text = "Sound Duration:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(392, 41);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(52, 13);
            label2.TabIndex = 29;
            label2.Text = "Alarm file:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(356, 86);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(88, 13);
            label9.TabIndex = 38;
            label9.Text = "Play alert sound :";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Cyan;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(930, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Communication settings";
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(764, 410);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 8;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(845, 410);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.recloserListView1);
            this.groupBox1.Location = new System.Drawing.Point(3, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(914, 265);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Devices";
            // 
            // recloserListView1
            // 
            this.recloserListView1.DataSource = null;
            this.recloserListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recloserListView1.Location = new System.Drawing.Point(3, 16);
            this.recloserListView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.recloserListView1.Name = "recloserListView1";
            this.recloserListView1.Size = new System.Drawing.Size(908, 246);
            this.recloserListView1.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(label7);
            this.groupBox2.Controls.Add(this.numMaxCurrent);
            this.groupBox2.Controls.Add(label6);
            this.groupBox2.Controls.Add(label5);
            this.groupBox2.Controls.Add(this.numCurrentDuration);
            this.groupBox2.Controls.Add(label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 72);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recloser";
            // 
            // numMaxCurrent
            // 
            this.numMaxCurrent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMaxCurrent.Location = new System.Drawing.Point(196, 44);
            this.numMaxCurrent.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            0});
            this.numMaxCurrent.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMaxCurrent.Name = "numMaxCurrent";
            this.numMaxCurrent.Size = new System.Drawing.Size(56, 20);
            this.numMaxCurrent.TabIndex = 10;
            this.numMaxCurrent.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numCurrentDuration
            // 
            this.numCurrentDuration.Location = new System.Drawing.Point(84, 18);
            this.numCurrentDuration.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numCurrentDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCurrentDuration.Name = "numCurrentDuration";
            this.numCurrentDuration.Size = new System.Drawing.Size(56, 20);
            this.numCurrentDuration.TabIndex = 7;
            this.numCurrentDuration.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnchangepassword
            // 
            this.btnchangepassword.Location = new System.Drawing.Point(14, 410);
            this.btnchangepassword.Name = "btnchangepassword";
            this.btnchangepassword.Size = new System.Drawing.Size(163, 23);
            this.btnchangepassword.TabIndex = 14;
            this.btnchangepassword.Text = "Change Command Password...";
            this.btnchangepassword.UseVisualStyleBackColor = true;
            this.btnchangepassword.Click += new System.EventHandler(this.btnchangepassword_Click);
            // 
            // btnChangeLogin
            // 
            this.btnChangeLogin.Location = new System.Drawing.Point(199, 410);
            this.btnChangeLogin.Name = "btnChangeLogin";
            this.btnChangeLogin.Size = new System.Drawing.Size(164, 23);
            this.btnChangeLogin.TabIndex = 15;
            this.btnChangeLogin.Text = "Change Login Password...";
            this.btnChangeLogin.UseVisualStyleBackColor = true;
            this.btnChangeLogin.Click += new System.EventHandler(this.btnChangeLogin_Click);
            // 
            // txtdvpath
            // 
            this.txtdvpath.Location = new System.Drawing.Point(790, 388);
            this.txtdvpath.Name = "txtdvpath";
            this.txtdvpath.Size = new System.Drawing.Size(128, 20);
            this.txtdvpath.TabIndex = 16;
            this.txtdvpath.Text = "C:\\Recloser\\Devices\\";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 384);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(71, 23);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.Text = "Add TuBu ";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddNulec
            // 
            this.btnAddNulec.Location = new System.Drawing.Point(89, 384);
            this.btnAddNulec.Name = "btnAddNulec";
            this.btnAddNulec.Size = new System.Drawing.Size(71, 23);
            this.btnAddNulec.TabIndex = 18;
            this.btnAddNulec.Text = "Add Nulec";
            this.btnAddNulec.UseVisualStyleBackColor = true;
            this.btnAddNulec.Click += new System.EventHandler(this.btnAddNulec_Click);
            // 
            // btnAddCooper
            // 
            this.btnAddCooper.Location = new System.Drawing.Point(165, 384);
            this.btnAddCooper.Name = "btnAddCooper";
            this.btnAddCooper.Size = new System.Drawing.Size(71, 23);
            this.btnAddCooper.TabIndex = 19;
            this.btnAddCooper.Text = "Add Cooper ";
            this.btnAddCooper.UseVisualStyleBackColor = true;
            this.btnAddCooper.Click += new System.EventHandler(this.btnAddCooper_Click);
            // 
            // btnAddRecloserSel
            // 
            this.btnAddRecloserSel.Location = new System.Drawing.Point(242, 384);
            this.btnAddRecloserSel.Name = "btnAddRecloserSel";
            this.btnAddRecloserSel.Size = new System.Drawing.Size(101, 23);
            this.btnAddRecloserSel.TabIndex = 22;
            this.btnAddRecloserSel.Text = "Add RecloserSel";
            this.btnAddRecloserSel.UseVisualStyleBackColor = true;
            this.btnAddRecloserSel.Click += new System.EventHandler(this.btnAddRecloserSel_Click);
            // 
            // btnAddDKDT
            // 
            this.btnAddDKDT.Location = new System.Drawing.Point(349, 384);
            this.btnAddDKDT.Name = "btnAddDKDT";
            this.btnAddDKDT.Size = new System.Drawing.Size(71, 23);
            this.btnAddDKDT.TabIndex = 23;
            this.btnAddDKDT.Text = "Add DKDT";
            this.btnAddDKDT.UseVisualStyleBackColor = true;
            this.btnAddDKDT.Click += new System.EventHandler(this.btnAddDKDT_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(637, 384);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 23);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "Delete ";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUsers
            // 
            this.btnUsers.Location = new System.Drawing.Point(729, 36);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(75, 23);
            this.btnUsers.TabIndex = 28;
            this.btnUsers.Text = "Users ...";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // nmsoundduration
            // 
            this.nmsoundduration.Location = new System.Drawing.Point(454, 64);
            this.nmsoundduration.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nmsoundduration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmsoundduration.Name = "nmsoundduration";
            this.nmsoundduration.Size = new System.Drawing.Size(56, 20);
            this.nmsoundduration.TabIndex = 33;
            this.nmsoundduration.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // txtAlertSoundFile
            // 
            this.txtAlertSoundFile.Location = new System.Drawing.Point(454, 38);
            this.txtAlertSoundFile.Name = "txtAlertSoundFile";
            this.txtAlertSoundFile.Size = new System.Drawing.Size(223, 20);
            this.txtAlertSoundFile.TabIndex = 30;
            this.txtAlertSoundFile.Text = "Fire.wav";
            // 
            // btnaddRelADVC45
            // 
            this.btnaddRelADVC45.Location = new System.Drawing.Point(374, 410);
            this.btnaddRelADVC45.Name = "btnaddRelADVC45";
            this.btnaddRelADVC45.Size = new System.Drawing.Size(113, 23);
            this.btnaddRelADVC45.TabIndex = 35;
            this.btnaddRelADVC45.Text = "Add RTU ADVC 45";
            this.btnaddRelADVC45.UseVisualStyleBackColor = true;
            this.btnaddRelADVC45.Click += new System.EventHandler(this.btnaddRelADVC45_Click);
            // 
            // btnAddRecloserADVC
            // 
            this.btnAddRecloserADVC.Location = new System.Drawing.Point(426, 384);
            this.btnAddRecloserADVC.Name = "btnAddRecloserADVC";
            this.btnAddRecloserADVC.Size = new System.Drawing.Size(98, 23);
            this.btnAddRecloserADVC.TabIndex = 34;
            this.btnAddRecloserADVC.Text = "Add RTU ADVC";
            this.btnAddRecloserADVC.UseVisualStyleBackColor = true;
            this.btnAddRecloserADVC.Click += new System.EventHandler(this.btnAddRecloserADVC_Click);
            // 
            // btnAddNulecUseries
            // 
            this.btnAddNulecUseries.Location = new System.Drawing.Point(493, 410);
            this.btnAddNulecUseries.Name = "btnAddNulecUseries";
            this.btnAddNulecUseries.Size = new System.Drawing.Size(113, 23);
            this.btnAddNulecUseries.TabIndex = 36;
            this.btnAddNulecUseries.Text = "Add Nulec USeries";
            this.btnAddNulecUseries.UseVisualStyleBackColor = true;
            this.btnAddNulecUseries.Click += new System.EventHandler(this.btnAddNulecUseries_Click);
            // 
            // btnAdvcTcpIp
            // 
            this.btnAdvcTcpIp.Location = new System.Drawing.Point(612, 410);
            this.btnAdvcTcpIp.Name = "btnAdvcTcpIp";
            this.btnAdvcTcpIp.Size = new System.Drawing.Size(113, 23);
            this.btnAdvcTcpIp.TabIndex = 37;
            this.btnAdvcTcpIp.Text = "Add ADVCTCPIP";
            this.btnAdvcTcpIp.UseVisualStyleBackColor = true;
            this.btnAdvcTcpIp.Click += new System.EventHandler(this.btnAdvcTcpIp_Click);
            // 
            // nmplaycount
            // 
            this.nmplaycount.Location = new System.Drawing.Point(454, 84);
            this.nmplaycount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmplaycount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmplaycount.Name = "nmplaycount";
            this.nmplaycount.Size = new System.Drawing.Size(56, 20);
            this.nmplaycount.TabIndex = 39;
            this.nmplaycount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnAddLBS
            // 
            this.btnAddLBS.Location = new System.Drawing.Point(530, 384);
            this.btnAddLBS.Name = "btnAddLBS";
            this.btnAddLBS.Size = new System.Drawing.Size(102, 23);
            this.btnAddLBS.TabIndex = 40;
            this.btnAddLBS.Text = "Add LBS";
            this.btnAddLBS.UseVisualStyleBackColor = true;
            this.btnAddLBS.Click += new System.EventHandler(this.btnAddLBS_Click);
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 450);
            this.Controls.Add(this.btnAddLBS);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.nmplaycount);
            this.Controls.Add(label9);
            this.Controls.Add(this.btnAdvcTcpIp);
            this.Controls.Add(this.btnAddNulecUseries);
            this.Controls.Add(this.btnaddRelADVC45);
            this.Controls.Add(this.btnAddRecloserADVC);
            this.Controls.Add(label8);
            this.Controls.Add(this.nmsoundduration);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.txtAlertSoundFile);
            this.Controls.Add(this.btnUsers);
            this.Controls.Add(this.btnAddDKDT);
            this.Controls.Add(this.btnAddRecloserSel);
            this.Controls.Add(this.btnAddCooper);
            this.Controls.Add(this.btnAddNulec);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(label10);
            this.Controls.Add(this.txtdvpath);
            this.Controls.Add(this.btnChangeLogin);
            this.Controls.Add(this.btnchangepassword);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmsoundduration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmplaycount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button button2;
        private RecloserListView recloserListView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numMaxCurrent;
        private System.Windows.Forms.NumericUpDown numCurrentDuration;
        private System.Windows.Forms.Button btnchangepassword;
        private System.Windows.Forms.Button btnChangeLogin;
        private System.Windows.Forms.TextBox txtdvpath;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAddNulec;
        private System.Windows.Forms.Button btnAddCooper;
        private System.Windows.Forms.Button btnAddRecloserSel;
        private System.Windows.Forms.Button btnAddDKDT;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.NumericUpDown nmsoundduration;
        private System.Windows.Forms.TextBox txtAlertSoundFile;
        private System.Windows.Forms.Button btnaddRelADVC45;
        private System.Windows.Forms.Button btnAddRecloserADVC;
        private System.Windows.Forms.Button btnAddNulecUseries;
        private System.Windows.Forms.Button btnAdvcTcpIp;
        private System.Windows.Forms.NumericUpDown nmplaycount;
        private System.Windows.Forms.Button btnAddLBS;
    }
}