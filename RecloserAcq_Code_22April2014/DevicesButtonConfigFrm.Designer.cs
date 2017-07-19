namespace RecloserAcq
{
    partial class DevicesButtonConfigFrm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("GroupClass", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("GroupName");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnChangeLogin = new System.Windows.Forms.Button();
            this.btnchangepassword = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.dgv = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(640, 358);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 28);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save && close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnChangeLogin
            // 
            this.btnChangeLogin.Location = new System.Drawing.Point(255, 358);
            this.btnChangeLogin.Margin = new System.Windows.Forms.Padding(4);
            this.btnChangeLogin.Name = "btnChangeLogin";
            this.btnChangeLogin.Size = new System.Drawing.Size(219, 28);
            this.btnChangeLogin.TabIndex = 17;
            this.btnChangeLogin.Text = "Change Login Password...";
            this.btnChangeLogin.UseVisualStyleBackColor = true;
            this.btnChangeLogin.Click += new System.EventHandler(this.btnChangeLogin_Click);
            // 
            // btnchangepassword
            // 
            this.btnchangepassword.Location = new System.Drawing.Point(11, 358);
            this.btnchangepassword.Margin = new System.Windows.Forms.Padding(4);
            this.btnchangepassword.Name = "btnchangepassword";
            this.btnchangepassword.Size = new System.Drawing.Size(217, 28);
            this.btnchangepassword.TabIndex = 16;
            this.btnchangepassword.Text = "Change Command Password...";
            this.btnchangepassword.UseVisualStyleBackColor = true;
            this.btnchangepassword.Click += new System.EventHandler(this.btnchangepassword_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(491, 358);
            this.btnNew.Margin = new System.Windows.Forms.Padding(4);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(132, 28);
            this.btnNew.TabIndex = 18;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // dgv
            // 
            this.dgv.DataMember = null;
            this.dgv.DataSource = this.bindingSource1;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.dgv.DisplayLayout.Appearance = appearance1;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            this.dgv.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.dgv.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dgv.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.dgv.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.dgv.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.dgv.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.dgv.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            this.dgv.DisplayLayout.MaxColScrollRegions = 1;
            this.dgv.DisplayLayout.MaxRowScrollRegions = 1;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dgv.DisplayLayout.Override.ActiveCellAppearance = appearance7;
            appearance10.BackColor = System.Drawing.SystemColors.Highlight;
            appearance10.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            this.dgv.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.dgv.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            this.dgv.DisplayLayout.Override.CardAreaAppearance = appearance12;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.dgv.DisplayLayout.Override.CellAppearance = appearance8;
            this.dgv.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.dgv.DisplayLayout.Override.CellPadding = 0;
            appearance6.BackColor = System.Drawing.SystemColors.Control;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.dgv.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            appearance5.TextHAlignAsString = "Left";
            this.dgv.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.dgv.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.dgv.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.dgv.DisplayLayout.Override.RowAppearance = appearance11;
            this.dgv.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dgv.DisplayLayout.Override.TemplateAddRowAppearance = appearance9;
            this.dgv.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.dgv.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.dgv.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.dgv.Location = new System.Drawing.Point(11, 15);
            this.dgv.Margin = new System.Windows.Forms.Padding(4);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(735, 320);
            this.dgv.TabIndex = 19;
            this.dgv.Text = "ultraGrid1";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(RecloserAcq.Device.GroupClass);
            // 
            // DevicesButtonConfigFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 399);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnChangeLogin);
            this.Controls.Add(this.btnchangepassword);
            this.Controls.Add(this.btnSave);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DevicesButtonConfigFrm";
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.DevicesButtonConfigFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnChangeLogin;
        private System.Windows.Forms.Button btnchangepassword;
        private System.Windows.Forms.Button btnNew;
        private Infragistics.Win.UltraWinGrid.UltraGrid dgv;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}