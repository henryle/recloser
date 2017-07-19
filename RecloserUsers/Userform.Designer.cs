namespace RecloserAcq
{
    partial class Userform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Userform));
            this.dgvUser = new System.Windows.Forms.DataGridView();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btncedituser = new System.Windows.Forms.Button();
            this.btnEditRole = new System.Windows.Forms.Button();
            this.userObjBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.clusername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userObjBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUser
            // 
            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.AllowUserToDeleteRows = false;
            this.dgvUser.AutoGenerateColumns = false;
            this.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clusername,
            this.clRole});
            this.dgvUser.DataSource = this.userObjBindingSource;
            this.dgvUser.Location = new System.Drawing.Point(16, 48);
            this.dgvUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvUser.MultiSelect = false;
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.ReadOnly = true;
            this.dgvUser.RowHeadersWidth = 24;
            this.dgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUser.Size = new System.Drawing.Size(701, 299);
            this.dgvUser.TabIndex = 0;
            // 
            // btnInsert
            // 
            this.btnInsert.Image = ((System.Drawing.Image)(resources.GetObject("btnInsert.Image")));
            this.btnInsert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInsert.Location = new System.Drawing.Point(389, 379);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(160, 57);
            this.btnInsert.TabIndex = 7;
            this.btnInsert.Text = "New User   ";
            this.btnInsert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(557, 379);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(160, 57);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete User  ";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btncedituser
            // 
            this.btncedituser.Image = ((System.Drawing.Image)(resources.GetObject("btncedituser.Image")));
            this.btncedituser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncedituser.Location = new System.Drawing.Point(221, 379);
            this.btncedituser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btncedituser.Name = "btncedituser";
            this.btncedituser.Size = new System.Drawing.Size(160, 57);
            this.btncedituser.TabIndex = 8;
            this.btncedituser.Text = "Edit User         ";
            this.btncedituser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btncedituser.UseVisualStyleBackColor = true;
            this.btncedituser.Click += new System.EventHandler(this.btnchangepass_Click);
            // 
            // btnEditRole
            // 
            this.btnEditRole.Image = ((System.Drawing.Image)(resources.GetObject("btnEditRole.Image")));
            this.btnEditRole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditRole.Location = new System.Drawing.Point(53, 379);
            this.btnEditRole.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEditRole.Name = "btnEditRole";
            this.btnEditRole.Size = new System.Drawing.Size(160, 57);
            this.btnEditRole.TabIndex = 9;
            this.btnEditRole.Text = "Edit Role     ";
            this.btnEditRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEditRole.UseVisualStyleBackColor = true;
            this.btnEditRole.Click += new System.EventHandler(this.btnEditRole_Click);
            // 
            // userObjBindingSource
            // 
            this.userObjBindingSource.DataSource = typeof(RecloserAcq.UserObj);
            // 
            // clusername
            // 
            this.clusername.DataPropertyName = "name";
            this.clusername.HeaderText = "Username";
            this.clusername.Name = "clusername";
            this.clusername.ReadOnly = true;
            // 
            // clRole
            // 
            this.clRole.DataPropertyName = "RoleName";
            this.clRole.HeaderText = "Role";
            this.clRole.Name = "clRole";
            this.clRole.ReadOnly = true;
            // 
            // Userform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 463);
            this.Controls.Add(this.btnEditRole);
            this.Controls.Add(this.btncedituser);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dgvUser);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Userform";
            this.Text = "User";
            this.Load += new System.EventHandler(this.User_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userObjBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btncedituser;
        private System.Windows.Forms.Button btnEditRole;
        private System.Windows.Forms.BindingSource userObjBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn clusername;
        private System.Windows.Forms.DataGridViewTextBoxColumn clRole;
    }
}