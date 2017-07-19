namespace RecloserAcq
{
    partial class frmcompensationcapacitor
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("DeviceEvent", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Id");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeviceId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeviceName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Command");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Weekday");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DtActive");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DtExpire");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DtNextRun");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DtPrevRun");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NameOfEvent");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("hourRepeat");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            this.btnNew = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grSchedule = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(962, 10);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdDelete);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 555);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1237, 45);
            this.panel1.TabIndex = 1;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Location = new System.Drawing.Point(1082, 10);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(75, 23);
            this.cmdDelete.TabIndex = 1;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grSchedule);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1237, 555);
            this.panel2.TabIndex = 2;
            // 
            // grSchedule
            // 
            this.grSchedule.DataMember = null;
            this.grSchedule.DataSource = this.bindingSource1;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grSchedule.DisplayLayout.Appearance = appearance13;
            ultraGridColumn25.Header.VisiblePosition = 0;
            ultraGridColumn26.Header.VisiblePosition = 1;
            ultraGridColumn27.Header.VisiblePosition = 2;
            ultraGridColumn28.Header.VisiblePosition = 5;
            ultraGridColumn29.Header.VisiblePosition = 6;
            ultraGridColumn30.Header.VisiblePosition = 4;
            ultraGridColumn31.Format = "dd/MM/yy HH:mm:ss";
            ultraGridColumn31.Header.Caption = "Active Date";
            ultraGridColumn31.Header.VisiblePosition = 7;
            ultraGridColumn31.Width = 150;
            ultraGridColumn32.Format = "dd/MM/yy HH:mm:ss";
            ultraGridColumn32.Header.Caption = "Expire Date";
            ultraGridColumn32.Header.VisiblePosition = 8;
            ultraGridColumn32.Width = 150;
            ultraGridColumn33.Format = "dd/MM/yy HH:mm:ss";
            ultraGridColumn33.Header.Caption = "Next Run";
            ultraGridColumn33.Header.VisiblePosition = 9;
            ultraGridColumn33.Width = 150;
            ultraGridColumn34.Format = "dd/MM/yy HH:mm:ss";
            ultraGridColumn34.Header.Caption = "Prev Run";
            ultraGridColumn34.Header.VisiblePosition = 10;
            ultraGridColumn34.Width = 150;
            ultraGridColumn35.Header.Caption = "Name";
            ultraGridColumn35.Header.VisiblePosition = 3;
            ultraGridColumn36.Header.Caption = "Repeat in (hours)";
            ultraGridColumn36.Header.VisiblePosition = 11;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn25,
            ultraGridColumn26,
            ultraGridColumn27,
            ultraGridColumn28,
            ultraGridColumn29,
            ultraGridColumn30,
            ultraGridColumn31,
            ultraGridColumn32,
            ultraGridColumn33,
            ultraGridColumn34,
            ultraGridColumn35,
            ultraGridColumn36});
            this.grSchedule.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grSchedule.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grSchedule.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.grSchedule.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grSchedule.DisplayLayout.GroupByBox.BandLabelAppearance = appearance16;
            this.grSchedule.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance15.BackColor2 = System.Drawing.SystemColors.Control;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grSchedule.DisplayLayout.GroupByBox.PromptAppearance = appearance15;
            this.grSchedule.DisplayLayout.MaxColScrollRegions = 1;
            this.grSchedule.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grSchedule.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance22.BackColor = System.Drawing.SystemColors.Highlight;
            appearance22.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grSchedule.DisplayLayout.Override.ActiveRowAppearance = appearance22;
            this.grSchedule.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grSchedule.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grSchedule.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grSchedule.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grSchedule.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            this.grSchedule.DisplayLayout.Override.CardAreaAppearance = appearance24;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grSchedule.DisplayLayout.Override.CellAppearance = appearance20;
            this.grSchedule.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grSchedule.DisplayLayout.Override.CellPadding = 0;
            appearance18.BackColor = System.Drawing.SystemColors.Control;
            appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance18.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance18.BorderColor = System.Drawing.SystemColors.Window;
            this.grSchedule.DisplayLayout.Override.GroupByRowAppearance = appearance18;
            appearance17.TextHAlignAsString = "Left";
            this.grSchedule.DisplayLayout.Override.HeaderAppearance = appearance17;
            this.grSchedule.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grSchedule.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.grSchedule.DisplayLayout.Override.RowAppearance = appearance23;
            this.grSchedule.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance21.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grSchedule.DisplayLayout.Override.TemplateAddRowAppearance = appearance21;
            this.grSchedule.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grSchedule.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grSchedule.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grSchedule.Location = new System.Drawing.Point(0, 0);
            this.grSchedule.Name = "grSchedule";
            this.grSchedule.Size = new System.Drawing.Size(1237, 555);
            this.grSchedule.TabIndex = 0;
            this.grSchedule.Text = "ultraGrid1";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(RecloserAcq.Device.DeviceEvent);
            // 
            // frmcompensationcapacitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 600);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmcompensationcapacitor";
            this.Text = "Lịch Đóng Cắt Tụ Bù";
            this.Load += new System.EventHandler(this.frmcompensationcapacitor_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grSchedule;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button cmdDelete;
    }
}