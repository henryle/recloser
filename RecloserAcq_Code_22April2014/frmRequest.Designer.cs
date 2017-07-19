namespace RecloserAcq
{
    partial class frmRequest
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label deviceTypeLabel;
            System.Windows.Forms.Label nameLabel;
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Request", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Text");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeviceType");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Used");
            this.deviceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.textTextBox = new System.Windows.Forms.TextBox();
            this.usedCheckBox = new System.Windows.Forms.CheckBox();
            this.requestUltraGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txAscii = new System.Windows.Forms.TextBox();
            this.btNew = new System.Windows.Forms.Button();
            this.btCheck = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.currentRequestSource = new System.Windows.Forms.BindingSource(this.components);
            this.requestBindingSource = new System.Windows.Forms.BindingSource(this.components);
            deviceTypeLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.requestUltraGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentRequestSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requestBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // deviceTypeLabel
            // 
            deviceTypeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            deviceTypeLabel.AutoSize = true;
            deviceTypeLabel.Location = new System.Drawing.Point(44, 8);
            deviceTypeLabel.Name = "deviceTypeLabel";
            deviceTypeLabel.Size = new System.Drawing.Size(34, 13);
            deviceTypeLabel.TabIndex = 1;
            deviceTypeLabel.Text = "Type:";
            // 
            // nameLabel
            // 
            nameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(169, 8);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(38, 13);
            nameLabel.TabIndex = 3;
            nameLabel.Text = "Name:";
            // 
            // deviceTypeComboBox
            // 
            this.deviceTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceTypeComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.currentRequestSource, "DeviceType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.deviceTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceTypeComboBox.FormattingEnabled = true;
            this.deviceTypeComboBox.Location = new System.Drawing.Point(84, 4);
            this.deviceTypeComboBox.Name = "deviceTypeComboBox";
            this.deviceTypeComboBox.Size = new System.Drawing.Size(79, 21);
            this.deviceTypeComboBox.TabIndex = 2;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.currentRequestSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nameTextBox.Location = new System.Drawing.Point(213, 4);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(191, 20);
            this.nameTextBox.TabIndex = 4;
            // 
            // textTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textTextBox, 7);
            this.textTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.currentRequestSource, "Text", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textTextBox.Location = new System.Drawing.Point(3, 32);
            this.textTextBox.Multiline = true;
            this.textTextBox.Name = "textTextBox";
            this.textTextBox.Size = new System.Drawing.Size(652, 243);
            this.textTextBox.TabIndex = 6;
            this.textTextBox.TextChanged += new System.EventHandler(this.textTextBox_TextChanged);
            // 
            // usedCheckBox
            // 
            this.usedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.usedCheckBox.AutoSize = true;
            this.usedCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.currentRequestSource, "Used", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.usedCheckBox.Location = new System.Drawing.Point(410, 6);
            this.usedCheckBox.Name = "usedCheckBox";
            this.usedCheckBox.Size = new System.Drawing.Size(51, 17);
            this.usedCheckBox.TabIndex = 8;
            this.usedCheckBox.Text = "Used";
            this.usedCheckBox.UseVisualStyleBackColor = true;
            // 
            // requestUltraGrid
            // 
            this.requestUltraGrid.DataMember = null;
            this.requestUltraGrid.DataSource = this.requestBindingSource;
            this.requestUltraGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn5.Width = 119;
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 0;
            ultraGridColumn7.Width = 72;
            ultraGridColumn8.Header.VisiblePosition = 3;
            ultraGridColumn8.Width = 45;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8});
            this.requestUltraGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.requestUltraGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.requestUltraGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.requestUltraGrid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            this.requestUltraGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.requestUltraGrid.Dock = System.Windows.Forms.DockStyle.Left;
            this.requestUltraGrid.Location = new System.Drawing.Point(0, 0);
            this.requestUltraGrid.Name = "requestUltraGrid";
            this.requestUltraGrid.Size = new System.Drawing.Size(257, 474);
            this.requestUltraGrid.TabIndex = 8;
            this.requestUltraGrid.Text = "ultraGrid1";
            this.requestUltraGrid.AfterRowActivate += new System.EventHandler(this.requestUltraGrid_AfterRowActivate);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(257, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(658, 474);
            this.panel1.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(deviceTypeLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txAscii, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.deviceTypeComboBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btNew, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btCheck, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.textTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(nameLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.nameTextBox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.usedCheckBox, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btCancel, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.btSave, 5, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(658, 474);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // txAscii
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txAscii, 7);
            this.txAscii.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txAscii.Location = new System.Drawing.Point(3, 281);
            this.txAscii.Multiline = true;
            this.txAscii.Name = "txAscii";
            this.txAscii.ReadOnly = true;
            this.txAscii.Size = new System.Drawing.Size(652, 160);
            this.txAscii.TabIndex = 13;
            // 
            // btNew
            // 
            this.btNew.Location = new System.Drawing.Point(3, 447);
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(75, 23);
            this.btNew.TabIndex = 11;
            this.btNew.Text = "&New";
            this.btNew.UseVisualStyleBackColor = true;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // btCheck
            // 
            this.btCheck.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btCheck.Location = new System.Drawing.Point(467, 3);
            this.btCheck.Name = "btCheck";
            this.btCheck.Size = new System.Drawing.Size(75, 23);
            this.btCheck.TabIndex = 12;
            this.btCheck.Text = "&Refine";
            this.btCheck.UseVisualStyleBackColor = true;
            this.btCheck.Click += new System.EventHandler(this.btCheck_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.Location = new System.Drawing.Point(580, 447);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 10;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(467, 447);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 9;
            this.btSave.Text = "&Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // currentRequestSource
            // 
            this.currentRequestSource.DataSource = typeof(RecloserAcq.Request);
            // 
            // requestBindingSource
            // 
            this.requestBindingSource.DataSource = typeof(RecloserAcq.Request);
            // 
            // frmRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 474);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.requestUltraGrid);
            this.Name = "frmRequest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Request";
            this.Load += new System.EventHandler(this.frmRequest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.requestUltraGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentRequestSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requestBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource requestBindingSource;
        private System.Windows.Forms.ComboBox deviceTypeComboBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox textTextBox;
        private System.Windows.Forms.CheckBox usedCheckBox;
        private Infragistics.Win.UltraWinGrid.UltraGrid requestUltraGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.BindingSource currentRequestSource;
        private System.Windows.Forms.Button btCheck;
        private System.Windows.Forms.TextBox txAscii;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}