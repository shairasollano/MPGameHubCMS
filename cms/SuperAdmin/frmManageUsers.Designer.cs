namespace cms.SuperAdmin
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
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnDeactivateUser = new System.Windows.Forms.Button();
            this.btnReactivateUser = new System.Windows.Forms.Button();
            this.btnChangeRole = new System.Windows.Forms.Button();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.lblUserInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // cmbUsers
            // 
            this.cmbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsers.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUsers.ForeColor = System.Drawing.SystemColors.MenuText;
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(45, 34);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(549, 33);
            this.cmbUsers.TabIndex = 0;
            this.cmbUsers.SelectedIndexChanged += new System.EventHandler(this.cmbUsers_SelectedIndexChanged);
            // 
            // btnAddUser
            // 
            this.btnAddUser.BackColor = System.Drawing.Color.SeaGreen;
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(41, 305);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(267, 50);
            this.btnAddUser.TabIndex = 2;
            this.btnAddUser.Text = "+ ADD NEW USER";
            this.btnAddUser.UseVisualStyleBackColor = false;
            // 
            // btnEditUser
            // 
            this.btnEditUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnEditUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditUser.ForeColor = System.Drawing.Color.White;
            this.btnEditUser.Location = new System.Drawing.Point(323, 305);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(267, 50);
            this.btnEditUser.TabIndex = 3;
            this.btnEditUser.Text = "✏️ EDIT USER";
            this.btnEditUser.UseVisualStyleBackColor = false;
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeleteUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnDeleteUser.ForeColor = System.Drawing.Color.White;
            this.btnDeleteUser.Location = new System.Drawing.Point(41, 361);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(267, 50);
            this.btnDeleteUser.TabIndex = 4;
            this.btnDeleteUser.Text = "🗑 DELETE USER";
            this.btnDeleteUser.UseVisualStyleBackColor = false;
            // 
            // btnDeactivateUser
            // 
            this.btnDeactivateUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeactivateUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnDeactivateUser.ForeColor = System.Drawing.Color.White;
            this.btnDeactivateUser.Location = new System.Drawing.Point(323, 424);
            this.btnDeactivateUser.Name = "btnDeactivateUser";
            this.btnDeactivateUser.Size = new System.Drawing.Size(267, 50);
            this.btnDeactivateUser.TabIndex = 5;
            this.btnDeactivateUser.Text = "🚫 DEACTIVATE USER";
            this.btnDeactivateUser.UseVisualStyleBackColor = false;
            // 
            // btnReactivateUser
            // 
            this.btnReactivateUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnReactivateUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnReactivateUser.Location = new System.Drawing.Point(41, 424);
            this.btnReactivateUser.Name = "btnReactivateUser";
            this.btnReactivateUser.Size = new System.Drawing.Size(267, 50);
            this.btnReactivateUser.TabIndex = 6;
            this.btnReactivateUser.Text = "🔄 REACTIVATE USER";
            this.btnReactivateUser.UseVisualStyleBackColor = false;
            // 
            // btnChangeRole
            // 
            this.btnChangeRole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(66)))), ((int)(((byte)(193)))));
            this.btnChangeRole.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnChangeRole.ForeColor = System.Drawing.Color.White;
            this.btnChangeRole.Location = new System.Drawing.Point(323, 361);
            this.btnChangeRole.Name = "btnChangeRole";
            this.btnChangeRole.Size = new System.Drawing.Size(267, 50);
            this.btnChangeRole.TabIndex = 7;
            this.btnChangeRole.Text = "👑 CHANGE USER ROLE\t";
            this.btnChangeRole.UseVisualStyleBackColor = false;
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnResetPassword.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnResetPassword.ForeColor = System.Drawing.Color.White;
            this.btnResetPassword.Location = new System.Drawing.Point(172, 480);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(267, 50);
            this.btnResetPassword.TabIndex = 8;
            this.btnResetPassword.Text = "🔒 RESET PASSWORD";
            this.btnResetPassword.UseVisualStyleBackColor = false;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Location = new System.Drawing.Point(47, 84);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(542, 204);
            this.lblUserInfo.TabIndex = 9;
            this.lblUserInfo.Text = "";
            // 
            // frmManageUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(644, 568);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.btnChangeRole);
            this.Controls.Add(this.btnReactivateUser);
            this.Controls.Add(this.btnDeactivateUser);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.btnEditUser);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.cmbUsers);
            this.Name = "frmManageUsers";
            this.Text = "frmManageUsers";
            this.Load += new System.EventHandler(this.frmManageUsers_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnDeactivateUser;
        private System.Windows.Forms.Button btnReactivateUser;
        private System.Windows.Forms.Button btnChangeRole;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.RichTextBox lblUserInfo;
    }
}