namespace RecloserAcq
{
    partial class frmDongCatEvent
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbNut = new System.Windows.Forms.ComboBox();
            this.cbcommand = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdOneTime = new System.Windows.Forms.RadioButton();
            this.rdEveryday = new System.Windows.Forms.RadioButton();
            this.rdEveryWeek = new System.Windows.Forms.RadioButton();
            this.chMonday = new System.Windows.Forms.CheckBox();
            this.chTue = new System.Windows.Forms.CheckBox();
            this.chWed = new System.Windows.Forms.CheckBox();
            this.chThu = new System.Windows.Forms.CheckBox();
            this.chFri = new System.Windows.Forms.CheckBox();
            this.chSat = new System.Windows.Forms.CheckBox();
            this.chSun = new System.Windows.Forms.CheckBox();
            this.dtTrigger = new System.Windows.Forms.DateTimePicker();
            this.dtActive = new System.Windows.Forms.DateTimePicker();
            this.dtExpire = new System.Windows.Forms.DateTimePicker();
            this.panelweekday = new System.Windows.Forms.Panel();
            this.paneldtActive = new System.Windows.Forms.Panel();
            this.paneldtExpire = new System.Windows.Forms.Panel();
            this.panelrepeat = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.nmHourRepeat = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panelweekday.SuspendLayout();
            this.paneldtActive.SuspendLayout();
            this.paneldtExpire.SuspendLayout();
            this.panelrepeat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmHourRepeat)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(537, 420);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(648, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nút";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Đóng/Mở";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Thực hiện";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Ngày";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Thời gian thực hiện";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "Hiệu lực tới";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Ngày bắt đầu hiệu lực";
            // 
            // cbNut
            // 
            this.cbNut.FormattingEnabled = true;
            this.cbNut.Location = new System.Drawing.Point(130, 40);
            this.cbNut.Name = "cbNut";
            this.cbNut.Size = new System.Drawing.Size(121, 24);
            this.cbNut.TabIndex = 9;
            // 
            // cbcommand
            // 
            this.cbcommand.FormattingEnabled = true;
            this.cbcommand.Items.AddRange(new object[] {
            "Đóng",
            "Mở"});
            this.cbcommand.Location = new System.Drawing.Point(130, 73);
            this.cbcommand.Name = "cbcommand";
            this.cbcommand.Size = new System.Drawing.Size(121, 24);
            this.cbcommand.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rdEveryWeek);
            this.panel1.Controls.Add(this.rdEveryday);
            this.panel1.Controls.Add(this.rdOneTime);
            this.panel1.Location = new System.Drawing.Point(130, 103);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 44);
            this.panel1.TabIndex = 11;
            // 
            // rdOneTime
            // 
            this.rdOneTime.AutoSize = true;
            this.rdOneTime.Checked = true;
            this.rdOneTime.Location = new System.Drawing.Point(10, 14);
            this.rdOneTime.Name = "rdOneTime";
            this.rdOneTime.Size = new System.Drawing.Size(75, 21);
            this.rdOneTime.TabIndex = 0;
            this.rdOneTime.TabStop = true;
            this.rdOneTime.Text = "Một lần";
            this.rdOneTime.UseVisualStyleBackColor = true;
            this.rdOneTime.CheckedChanged += new System.EventHandler(this.rdOneTime_CheckedChanged);
            // 
            // rdEveryday
            // 
            this.rdEveryday.AutoSize = true;
            this.rdEveryday.Location = new System.Drawing.Point(106, 14);
            this.rdEveryday.Name = "rdEveryday";
            this.rdEveryday.Size = new System.Drawing.Size(86, 21);
            this.rdEveryday.TabIndex = 1;
            this.rdEveryday.TabStop = true;
            this.rdEveryday.Text = "Mỗi ngày";
            this.rdEveryday.UseVisualStyleBackColor = true;
            this.rdEveryday.CheckedChanged += new System.EventHandler(this.rdEveryday_CheckedChanged);
            // 
            // rdEveryWeek
            // 
            this.rdEveryWeek.AutoSize = true;
            this.rdEveryWeek.Location = new System.Drawing.Point(219, 14);
            this.rdEveryWeek.Name = "rdEveryWeek";
            this.rdEveryWeek.Size = new System.Drawing.Size(83, 21);
            this.rdEveryWeek.TabIndex = 2;
            this.rdEveryWeek.TabStop = true;
            this.rdEveryWeek.Text = "Mỗi tuần";
            this.rdEveryWeek.UseVisualStyleBackColor = true;
            this.rdEveryWeek.CheckedChanged += new System.EventHandler(this.rdEveryWeek_CheckedChanged);
            // 
            // chMonday
            // 
            this.chMonday.AutoSize = true;
            this.chMonday.Location = new System.Drawing.Point(171, 12);
            this.chMonday.Name = "chMonday";
            this.chMonday.Size = new System.Drawing.Size(67, 21);
            this.chMonday.TabIndex = 12;
            this.chMonday.Text = "Thứ 2";
            this.chMonday.UseVisualStyleBackColor = true;
            // 
            // chTue
            // 
            this.chTue.AutoSize = true;
            this.chTue.Location = new System.Drawing.Point(249, 12);
            this.chTue.Name = "chTue";
            this.chTue.Size = new System.Drawing.Size(67, 21);
            this.chTue.TabIndex = 13;
            this.chTue.Text = "Thứ 3";
            this.chTue.UseVisualStyleBackColor = true;
            // 
            // chWed
            // 
            this.chWed.AutoSize = true;
            this.chWed.Location = new System.Drawing.Point(327, 12);
            this.chWed.Name = "chWed";
            this.chWed.Size = new System.Drawing.Size(67, 21);
            this.chWed.TabIndex = 14;
            this.chWed.Text = "Thứ 4";
            this.chWed.UseVisualStyleBackColor = true;
            // 
            // chThu
            // 
            this.chThu.AutoSize = true;
            this.chThu.Location = new System.Drawing.Point(405, 12);
            this.chThu.Name = "chThu";
            this.chThu.Size = new System.Drawing.Size(67, 21);
            this.chThu.TabIndex = 15;
            this.chThu.Text = "Thứ 5";
            this.chThu.UseVisualStyleBackColor = true;
            // 
            // chFri
            // 
            this.chFri.AutoSize = true;
            this.chFri.Location = new System.Drawing.Point(483, 12);
            this.chFri.Name = "chFri";
            this.chFri.Size = new System.Drawing.Size(67, 21);
            this.chFri.TabIndex = 16;
            this.chFri.Text = "Thứ 6";
            this.chFri.UseVisualStyleBackColor = true;
            // 
            // chSat
            // 
            this.chSat.AutoSize = true;
            this.chSat.Location = new System.Drawing.Point(561, 12);
            this.chSat.Name = "chSat";
            this.chSat.Size = new System.Drawing.Size(67, 21);
            this.chSat.TabIndex = 17;
            this.chSat.Text = "Thứ 7";
            this.chSat.UseVisualStyleBackColor = true;
            // 
            // chSun
            // 
            this.chSun.AutoSize = true;
            this.chSun.Location = new System.Drawing.Point(107, 11);
            this.chSun.Name = "chSun";
            this.chSun.Size = new System.Drawing.Size(49, 21);
            this.chSun.TabIndex = 18;
            this.chSun.Text = "CN";
            this.chSun.UseVisualStyleBackColor = true;
            // 
            // dtTrigger
            // 
            this.dtTrigger.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtTrigger.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTrigger.Location = new System.Drawing.Point(191, 164);
            this.dtTrigger.Name = "dtTrigger";
            this.dtTrigger.Size = new System.Drawing.Size(200, 22);
            this.dtTrigger.TabIndex = 19;
            // 
            // dtActive
            // 
            this.dtActive.CustomFormat = "dd/MM/yyyy";
            this.dtActive.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtActive.Location = new System.Drawing.Point(166, 8);
            this.dtActive.Name = "dtActive";
            this.dtActive.Size = new System.Drawing.Size(132, 22);
            this.dtActive.TabIndex = 20;
            // 
            // dtExpire
            // 
            this.dtExpire.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtExpire.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtExpire.Location = new System.Drawing.Point(166, 3);
            this.dtExpire.Name = "dtExpire";
            this.dtExpire.Size = new System.Drawing.Size(200, 22);
            this.dtExpire.TabIndex = 21;
            // 
            // panelweekday
            // 
            this.panelweekday.Controls.Add(this.chSat);
            this.panelweekday.Controls.Add(this.label4);
            this.panelweekday.Controls.Add(this.chMonday);
            this.panelweekday.Controls.Add(this.chTue);
            this.panelweekday.Controls.Add(this.chSun);
            this.panelweekday.Controls.Add(this.chWed);
            this.panelweekday.Controls.Add(this.chThu);
            this.panelweekday.Controls.Add(this.chFri);
            this.panelweekday.Location = new System.Drawing.Point(12, 192);
            this.panelweekday.Name = "panelweekday";
            this.panelweekday.Size = new System.Drawing.Size(659, 45);
            this.panelweekday.TabIndex = 22;
            // 
            // paneldtActive
            // 
            this.paneldtActive.Controls.Add(this.dtActive);
            this.paneldtActive.Controls.Add(this.label7);
            this.paneldtActive.Location = new System.Drawing.Point(25, 243);
            this.paneldtActive.Name = "paneldtActive";
            this.paneldtActive.Size = new System.Drawing.Size(438, 41);
            this.paneldtActive.TabIndex = 23;
            // 
            // paneldtExpire
            // 
            this.paneldtExpire.Controls.Add(this.dtExpire);
            this.paneldtExpire.Controls.Add(this.label6);
            this.paneldtExpire.Location = new System.Drawing.Point(25, 305);
            this.paneldtExpire.Name = "paneldtExpire";
            this.paneldtExpire.Size = new System.Drawing.Size(486, 46);
            this.paneldtExpire.TabIndex = 24;
            // 
            // panelrepeat
            // 
            this.panelrepeat.Controls.Add(this.label9);
            this.panelrepeat.Controls.Add(this.nmHourRepeat);
            this.panelrepeat.Controls.Add(this.label8);
            this.panelrepeat.Location = new System.Drawing.Point(25, 357);
            this.panelrepeat.Name = "panelrepeat";
            this.panelrepeat.Size = new System.Drawing.Size(488, 49);
            this.panelrepeat.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 17);
            this.label8.TabIndex = 22;
            this.label8.Text = "Lặp lại mỗi:";
            // 
            // nmHourRepeat
            // 
            this.nmHourRepeat.Location = new System.Drawing.Point(95, 15);
            this.nmHourRepeat.Name = "nmHourRepeat";
            this.nmHourRepeat.Size = new System.Drawing.Size(120, 22);
            this.nmHourRepeat.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(221, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 17);
            this.label9.TabIndex = 24;
            this.label9.Text = "(giờ)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(39, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 17);
            this.label10.TabIndex = 26;
            this.label10.Text = "Tên gợi nhớ";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(130, 9);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(261, 22);
            this.txtName.TabIndex = 27;
            // 
            // frmDongCatEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 455);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panelrepeat);
            this.Controls.Add(this.paneldtExpire);
            this.Controls.Add(this.paneldtActive);
            this.Controls.Add(this.panelweekday);
            this.Controls.Add(this.dtTrigger);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbcommand);
            this.Controls.Add(this.cbNut);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.Name = "frmDongCatEvent";
            this.Text = "Tạo sự kiện đóng / cắt";
            this.Load += new System.EventHandler(this.frmDongCatEvent_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelweekday.ResumeLayout(false);
            this.panelweekday.PerformLayout();
            this.paneldtActive.ResumeLayout(false);
            this.paneldtActive.PerformLayout();
            this.paneldtExpire.ResumeLayout(false);
            this.paneldtExpire.PerformLayout();
            this.panelrepeat.ResumeLayout(false);
            this.panelrepeat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmHourRepeat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbNut;
        private System.Windows.Forms.ComboBox cbcommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdEveryWeek;
        private System.Windows.Forms.RadioButton rdEveryday;
        private System.Windows.Forms.RadioButton rdOneTime;
        private System.Windows.Forms.CheckBox chMonday;
        private System.Windows.Forms.CheckBox chTue;
        private System.Windows.Forms.CheckBox chWed;
        private System.Windows.Forms.CheckBox chThu;
        private System.Windows.Forms.CheckBox chFri;
        private System.Windows.Forms.CheckBox chSat;
        private System.Windows.Forms.CheckBox chSun;
        private System.Windows.Forms.DateTimePicker dtTrigger;
        private System.Windows.Forms.DateTimePicker dtActive;
        private System.Windows.Forms.DateTimePicker dtExpire;
        private System.Windows.Forms.Panel panelweekday;
        private System.Windows.Forms.Panel paneldtActive;
        private System.Windows.Forms.Panel paneldtExpire;
        private System.Windows.Forms.Panel panelrepeat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nmHourRepeat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtName;
    }
}