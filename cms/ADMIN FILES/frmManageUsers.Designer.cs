namespace cms.lastsuper
{
    partial class frmManageUsers
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
            this.lblUserInfo = new System.Windows.Forms.RichTextBox();
            this.btnReactivateUser = new System.Windows.Forms.Button();
            this.btnDeactivateUser = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserInfo.Location = new System.Drawing.Point(34, 78);
            this.lblUserInfo.Margin = new System.Windows.Forms.Padding(2);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(417, 146);
            this.lblUserInfo.TabIndex = 27;
            this.lblUserInfo.Text = "";
            // 
            // btnReactivateUser
            // 
            this.btnReactivateUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnReactivateUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReactivateUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnReactivateUser.Location = new System.Drawing.Point(34, 297);
            this.btnReactivateUser.Margin = new System.Windows.Forms.Padding(2);
            this.btnReactivateUser.Name = "btnReactivateUser";
            this.btnReactivateUser.Size = new System.Drawing.Size(200, 41);
            this.btnReactivateUser.TabIndex = 24;
            this.btnReactivateUser.Text = "🔄 REACTIVATE USER";
            this.btnReactivateUser.UseVisualStyleBackColor = false;
            // 
            // btnDeactivateUser
            // 
            this.btnDeactivateUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeactivateUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeactivateUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnDeactivateUser.ForeColor = System.Drawing.Color.White;
            this.btnDeactivateUser.Location = new System.Drawing.Point(246, 297);
            this.btnDeactivateUser.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeactivateUser.Name = "btnDeactivateUser";
            this.btnDeactivateUser.Size = new System.Drawing.Size(205, 41);
            this.btnDeactivateUser.TabIndex = 23;
            this.btnDeactivateUser.Text = "🚫 DEACTIVATE USER";
            this.btnDeactivateUser.UseVisualStyleBackColor = false;
            // 
            // btnEditUser
            // 
            this.btnEditUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnEditUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditUser.ForeColor = System.Drawing.Color.White;
            this.btnEditUser.Location = new System.Drawing.Point(246, 238);
            this.btnEditUser.Margin = new System.Windows.Forms.Padding(2);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(205, 41);
            this.btnEditUser.TabIndex = 21;
            this.btnEditUser.Text = "✏️ EDIT USER";
            this.btnEditUser.UseVisualStyleBackColor = false;
            // 
            // btnAddUser
            // 
            this.btnAddUser.BackColor = System.Drawing.Color.SeaGreen;
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(34, 238);
            this.btnAddUser.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(200, 41);
            this.btnAddUser.TabIndex = 20;
            this.btnAddUser.Text = "+ ADD NEW USER";
            this.btnAddUser.UseVisualStyleBackColor = false;
            // 
            // cmbUsers
            // 
            this.cmbUsers.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsers.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUsers.ForeColor = System.Drawing.SystemColors.MenuText;
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(34, 29);
            this.cmbUsers.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(417, 33);
            this.cmbUsers.TabIndex = 19;
            this.cmbUsers.SelectedIndexChanged += new System.EventHandler(this.cmbUsers_SelectedIndexChanged);
            // 
            // frmManageUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(483, 380);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.btnReactivateUser);
            this.Controls.Add(this.btnDeactivateUser);
            this.Controls.Add(this.btnEditUser);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.cmbUsers);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmManageUsers";
            this.Text = "frmManageUsers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox lblUserInfo;
        private System.Windows.Forms.Button btnReactivateUser;
        private System.Windows.Forms.Button btnDeactivateUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.ComboBox cmbUsers;
    }
}