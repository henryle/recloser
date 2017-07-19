namespace RecloserAcq
{
    partial class Elster1700Ctrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Elster1700Ctrl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.dgvBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ElsterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationGrid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VoltA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VoltB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VoltC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmpA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmpB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmpC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReactivePowerA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReactivePowerB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReactivePowerC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReactivePower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivePowerA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivePowerB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivePowerC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivePower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PowerFactorA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PowerFactorB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PowerFactorC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PowerFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnDisconnect);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.btnHistory);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 114);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2060, 34);
            this.panel1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 17);
            this.label2.TabIndex = 42;
            this.label2.Text = "Điều khiển ELSTER: ";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(1032, 4);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 32);
            this.btnDisconnect.TabIndex = 49;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Visible = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(949, 4);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 32);
            this.btnConnect.TabIndex = 48;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Visible = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(871, 4);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(71, 32);
            this.btnTest.TabIndex = 47;
            this.btnTest.Text = "Get";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(731, 4);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(119, 32);
            this.button5.TabIndex = 46;
            this.button5.Text = "Reset Modem";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // btnHistory
            // 
            this.btnHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnHistory.Image")));
            this.btnHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHistory.Location = new System.Drawing.Point(604, 4);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(119, 32);
            this.btnHistory.TabIndex = 45;
            this.btnHistory.Text = "History...  ";
            this.btnHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Enabled = false;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpen.Location = new System.Drawing.Point(353, 4);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(119, 32);
            this.btnOpen.TabIndex = 44;
            this.btnOpen.Text = "OPEN    ";
            this.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Enabled = false;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(219, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(119, 32);
            this.btnClose.TabIndex = 43;
            this.btnClose.Text = "CLOSE    ";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            // 
            // dgv
            // 
            this.dgv.AutoGenerateColumns = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ElsterName,
            this.LocationGrid,
            this.Port,
            this.LastActive,
            this.Status,
            this.VoltA,
            this.VoltB,
            this.VoltC,
            this.Volt,
            this.AmpA,
            this.AmpB,
            this.AmpC,
            this.Amp,
            this.ReactivePowerA,
            this.ReactivePowerB,
            this.ReactivePowerC,
            this.ReactivePower,
            this.ActivePowerA,
            this.ActivePowerB,
            this.ActivePowerC,
            this.ActivePower,
            this.PowerFactorA,
            this.PowerFactorB,
            this.PowerFactorC,
            this.PowerFactor,
            this.deviceTimeDataGridViewTextBoxColumn});
            this.dgv.DataSource = this.dgvBindingSource;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersWidth = 25;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.Size = new System.Drawing.Size(2060, 114);
            this.dgv.TabIndex = 5;
            // 
            // dgvBindingSource
            // 
            this.dgvBindingSource.DataSource = typeof(RecloserAcq.Device.Elster1700);
            // 
            // ElsterName
            // 
            this.ElsterName.DataPropertyName = "Name";
            this.ElsterName.HeaderText = "Name";
            this.ElsterName.Name = "ElsterName";
            this.ElsterName.ReadOnly = true;
            this.ElsterName.Width = 70;
            // 
            // LocationGrid
            // 
            this.LocationGrid.DataPropertyName = "Location";
            this.LocationGrid.HeaderText = "Location";
            this.LocationGrid.Name = "LocationGrid";
            this.LocationGrid.ReadOnly = true;
            this.LocationGrid.Width = 50;
            // 
            // Port
            // 
            this.Port.DataPropertyName = "Port";
            this.Port.HeaderText = "Port";
            this.Port.Name = "Port";
            this.Port.ReadOnly = true;
            this.Port.Width = 55;
            // 
            // LastActive
            // 
            this.LastActive.DataPropertyName = "LastUpdated";
            this.LastActive.HeaderText = "Last Active";
            this.LastActive.Name = "LastActive";
            this.LastActive.ReadOnly = true;
            this.LastActive.Width = 80;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "CommStatus";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 50;
            // 
            // VoltA
            // 
            this.VoltA.DataPropertyName = "Volt_A";
            this.VoltA.HeaderText = "Volt A";
            this.VoltA.Name = "VoltA";
            this.VoltA.ReadOnly = true;
            this.VoltA.Width = 60;
            // 
            // VoltB
            // 
            this.VoltB.DataPropertyName = "Volt_B";
            this.VoltB.HeaderText = "Volt B";
            this.VoltB.Name = "VoltB";
            this.VoltB.ReadOnly = true;
            this.VoltB.Width = 60;
            // 
            // VoltC
            // 
            this.VoltC.DataPropertyName = "Volt_C";
            this.VoltC.HeaderText = "Volt C";
            this.VoltC.Name = "VoltC";
            this.VoltC.ReadOnly = true;
            this.VoltC.Width = 60;
            // 
            // Volt
            // 
            this.Volt.DataPropertyName = "Volt_Total";
            this.Volt.HeaderText = "Volt";
            this.Volt.Name = "Volt";
            this.Volt.ReadOnly = true;
            this.Volt.Width = 50;
            // 
            // AmpA
            // 
            this.AmpA.DataPropertyName = "Ample_A";
            this.AmpA.HeaderText = "Amp A";
            this.AmpA.Name = "AmpA";
            this.AmpA.ReadOnly = true;
            this.AmpA.Width = 65;
            // 
            // AmpB
            // 
            this.AmpB.DataPropertyName = "Ample_B";
            this.AmpB.HeaderText = "Amp B";
            this.AmpB.Name = "AmpB";
            this.AmpB.ReadOnly = true;
            this.AmpB.Width = 65;
            // 
            // AmpC
            // 
            this.AmpC.DataPropertyName = "Ample_C";
            this.AmpC.HeaderText = "Amp C";
            this.AmpC.Name = "AmpC";
            this.AmpC.ReadOnly = true;
            this.AmpC.Width = 65;
            // 
            // Amp
            // 
            this.Amp.DataPropertyName = "Ample_Total";
            this.Amp.HeaderText = "Amp";
            this.Amp.Name = "Amp";
            this.Amp.ReadOnly = true;
            this.Amp.Width = 60;
            // 
            // ReactivePowerA
            // 
            this.ReactivePowerA.DataPropertyName = "ReActivePower_A";
            this.ReactivePowerA.HeaderText = "ReAP A";
            this.ReactivePowerA.Name = "ReactivePowerA";
            this.ReactivePowerA.ReadOnly = true;
            this.ReactivePowerA.Width = 50;
            // 
            // ReactivePowerB
            // 
            this.ReactivePowerB.DataPropertyName = "ReActivePower_B";
            this.ReactivePowerB.HeaderText = "ReAP B";
            this.ReactivePowerB.Name = "ReactivePowerB";
            this.ReactivePowerB.ReadOnly = true;
            this.ReactivePowerB.Width = 50;
            // 
            // ReactivePowerC
            // 
            this.ReactivePowerC.DataPropertyName = "ReActivePower_C";
            this.ReactivePowerC.HeaderText = "ReAP C";
            this.ReactivePowerC.Name = "ReactivePowerC";
            this.ReactivePowerC.ReadOnly = true;
            this.ReactivePowerC.Width = 50;
            // 
            // ReactivePower
            // 
            this.ReactivePower.DataPropertyName = "ReActivePower_Total";
            this.ReactivePower.HeaderText = "ReAP";
            this.ReactivePower.Name = "ReactivePower";
            this.ReactivePower.ReadOnly = true;
            this.ReactivePower.Width = 50;
            // 
            // ActivePowerA
            // 
            this.ActivePowerA.DataPropertyName = "ActivePower_A";
            this.ActivePowerA.HeaderText = "AP A";
            this.ActivePowerA.Name = "ActivePowerA";
            this.ActivePowerA.ReadOnly = true;
            this.ActivePowerA.Width = 55;
            // 
            // ActivePowerB
            // 
            this.ActivePowerB.DataPropertyName = "ActivePower_B";
            this.ActivePowerB.HeaderText = "AP B";
            this.ActivePowerB.Name = "ActivePowerB";
            this.ActivePowerB.ReadOnly = true;
            this.ActivePowerB.Width = 55;
            // 
            // ActivePowerC
            // 
            this.ActivePowerC.DataPropertyName = "ActivePower_C";
            this.ActivePowerC.HeaderText = "AP C";
            this.ActivePowerC.Name = "ActivePowerC";
            this.ActivePowerC.ReadOnly = true;
            this.ActivePowerC.Width = 55;
            // 
            // ActivePower
            // 
            this.ActivePower.DataPropertyName = "ActivePower_Total";
            this.ActivePower.HeaderText = "Active Power";
            this.ActivePower.Name = "ActivePower";
            this.ActivePower.ReadOnly = true;
            this.ActivePower.Width = 55;
            // 
            // PowerFactorA
            // 
            this.PowerFactorA.DataPropertyName = "PowerFactor_A";
            this.PowerFactorA.HeaderText = "PF A";
            this.PowerFactorA.Name = "PowerFactorA";
            this.PowerFactorA.ReadOnly = true;
            this.PowerFactorA.Width = 55;
            // 
            // PowerFactorB
            // 
            this.PowerFactorB.DataPropertyName = "PowerFactor_B";
            this.PowerFactorB.HeaderText = "PF B";
            this.PowerFactorB.Name = "PowerFactorB";
            this.PowerFactorB.ReadOnly = true;
            this.PowerFactorB.Width = 55;
            // 
            // PowerFactorC
            // 
            this.PowerFactorC.DataPropertyName = "PowerFactor_C";
            this.PowerFactorC.HeaderText = "PF C";
            this.PowerFactorC.Name = "PowerFactorC";
            this.PowerFactorC.ReadOnly = true;
            this.PowerFactorC.Width = 55;
            // 
            // PowerFactor
            // 
            this.PowerFactor.DataPropertyName = "PowerFactor_Total";
            this.PowerFactor.HeaderText = "Power Factor";
            this.PowerFactor.Name = "PowerFactor";
            this.PowerFactor.ReadOnly = true;
            this.PowerFactor.Width = 55;
            // 
            // deviceTimeDataGridViewTextBoxColumn
            // 
            this.deviceTimeDataGridViewTextBoxColumn.DataPropertyName = "DeviceTime";
            this.deviceTimeDataGridViewTextBoxColumn.HeaderText = "DeviceTime";
            this.deviceTimeDataGridViewTextBoxColumn.Name = "deviceTimeDataGridViewTextBoxColumn";
            this.deviceTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Elster1700Ctrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Elster1700Ctrl";
            this.Size = new System.Drawing.Size(2060, 148);
            this.Load += new System.EventHandler(this.Elster1700Ctrl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource dgvBindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnClose;
      
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElsterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Port;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn VoltA;
        private System.Windows.Forms.DataGridViewTextBoxColumn VoltB;
        private System.Windows.Forms.DataGridViewTextBoxColumn VoltC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volt;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmpA;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmpB;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmpC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReactivePowerA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReactivePowerB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReactivePowerC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReactivePower;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivePowerA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivePowerB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivePowerC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivePower;
        private System.Windows.Forms.DataGridViewTextBoxColumn PowerFactorA;
        private System.Windows.Forms.DataGridViewTextBoxColumn PowerFactorB;
        private System.Windows.Forms.DataGridViewTextBoxColumn PowerFactorC;
        private System.Windows.Forms.DataGridViewTextBoxColumn PowerFactor;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceTimeDataGridViewTextBoxColumn;
    }
}
