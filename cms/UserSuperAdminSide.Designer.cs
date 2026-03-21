namespace cms
{
    partial class UserSuperAdminSide
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSuperAdminSide));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.userheader = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cardTotalUsers = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalUsers = new System.Windows.Forms.Label();
            this.picTotalIcon = new System.Windows.Forms.PictureBox();
            this.cardActiveAdmins = new System.Windows.Forms.Panel();
            this.lblActiveValue = new System.Windows.Forms.Label();
            this.lblActiveAdmins = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cardInactive = new System.Windows.Forms.Panel();
            this.lblInactiveValue = new System.Windows.Forms.Label();
            this.lblInactiveAcc = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.cardOnline = new System.Windows.Forms.Panel();
            this.lblOnlineValue = new System.Windows.Forms.Label();
            this.lblOnlineNow = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.lblUserList = new System.Windows.Forms.Label();
            this.pnlTableHeader = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.IDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullnameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsernameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LastLoginColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.cardTotalUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTotalIcon)).BeginInit();
            this.cardActiveAdmins.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.cardInactive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.cardOnline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.pnlTableHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // userheader
            // 
            this.userheader.AutoSize = true;
            this.userheader.Font = new System.Drawing.Font("Segoe UI Black", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userheader.Location = new System.Drawing.Point(3, 13);
            this.userheader.Name = "userheader";
            this.userheader.Size = new System.Drawing.Size(308, 38);
            this.userheader.TabIndex = 0;
            this.userheader.Text = "USER MANAGEMENT";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.cardTotalUsers);
            this.flowLayoutPanel1.Controls.Add(this.cardActiveAdmins);
            this.flowLayoutPanel1.Controls.Add(this.cardInactive);
            this.flowLayoutPanel1.Controls.Add(this.cardOnline);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 54);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1137, 100);
            this.flowLayoutPanel1.TabIndex = 1;
           
            // 
            // cardTotalUsers
            // 
            this.cardTotalUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalUsers.Controls.Add(this.lblTotalValue);
            this.cardTotalUsers.Controls.Add(this.lblTotalUsers);
            this.cardTotalUsers.Controls.Add(this.picTotalIcon);
            this.cardTotalUsers.Location = new System.Drawing.Point(23, 13);
            this.cardTotalUsers.Name = "cardTotalUsers";
            this.cardTotalUsers.Size = new System.Drawing.Size(235, 74);
            this.cardTotalUsers.TabIndex = 0;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalValue.Location = new System.Drawing.Point(71, 22);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(58, 45);
            this.lblTotalValue.TabIndex = 2;
            this.lblTotalValue.Text = "15";
            // 
            // lblTotalUsers
            // 
            this.lblTotalUsers.AutoSize = true;
            this.lblTotalUsers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalUsers.Location = new System.Drawing.Point(76, 6);
            this.lblTotalUsers.Name = "lblTotalUsers";
            this.lblTotalUsers.Size = new System.Drawing.Size(81, 20);
            this.lblTotalUsers.TabIndex = 1;
            this.lblTotalUsers.Text = "Total Users";
            // 
            // picTotalIcon
            // 
            this.picTotalIcon.Image = ((System.Drawing.Image)(resources.GetObject("picTotalIcon.Image")));
            this.picTotalIcon.Location = new System.Drawing.Point(8, 3);
            this.picTotalIcon.Name = "picTotalIcon";
            this.picTotalIcon.Size = new System.Drawing.Size(63, 66);
            this.picTotalIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTotalIcon.TabIndex = 0;
            this.picTotalIcon.TabStop = false;
            //this.picTotalIcon.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // cardActiveAdmins
            // 
            this.cardActiveAdmins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardActiveAdmins.Controls.Add(this.lblActiveValue);
            this.cardActiveAdmins.Controls.Add(this.lblActiveAdmins);
            this.cardActiveAdmins.Controls.Add(this.pictureBox1);
            this.cardActiveAdmins.Location = new System.Drawing.Point(264, 13);
            this.cardActiveAdmins.Name = "cardActiveAdmins";
            this.cardActiveAdmins.Size = new System.Drawing.Size(235, 74);
            this.cardActiveAdmins.TabIndex = 3;
            // 
            // lblActiveValue
            // 
            this.lblActiveValue.AutoSize = true;
            this.lblActiveValue.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveValue.Location = new System.Drawing.Point(71, 22);
            this.lblActiveValue.Name = "lblActiveValue";
            this.lblActiveValue.Size = new System.Drawing.Size(39, 45);
            this.lblActiveValue.TabIndex = 2;
            this.lblActiveValue.Text = "5";
            // 
            // lblActiveAdmins
            // 
            this.lblActiveAdmins.AutoSize = true;
            this.lblActiveAdmins.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveAdmins.Location = new System.Drawing.Point(76, 6);
            this.lblActiveAdmins.Name = "lblActiveAdmins";
            this.lblActiveAdmins.Size = new System.Drawing.Size(104, 20);
            this.lblActiveAdmins.TabIndex = 1;
            this.lblActiveAdmins.Text = "Active Admins";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(63, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // cardInactive
            // 
            this.cardInactive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardInactive.Controls.Add(this.lblInactiveValue);
            this.cardInactive.Controls.Add(this.lblInactiveAcc);
            this.cardInactive.Controls.Add(this.pictureBox2);
            this.cardInactive.Location = new System.Drawing.Point(505, 13);
            this.cardInactive.Name = "cardInactive";
            this.cardInactive.Size = new System.Drawing.Size(235, 74);
            this.cardInactive.TabIndex = 4;
            // 
            // lblInactiveValue
            // 
            this.lblInactiveValue.AutoSize = true;
            this.lblInactiveValue.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInactiveValue.Location = new System.Drawing.Point(71, 22);
            this.lblInactiveValue.Name = "lblInactiveValue";
            this.lblInactiveValue.Size = new System.Drawing.Size(39, 45);
            this.lblInactiveValue.TabIndex = 2;
            this.lblInactiveValue.Text = "2";
            // 
            // lblInactiveAcc
            // 
            this.lblInactiveAcc.AutoSize = true;
            this.lblInactiveAcc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInactiveAcc.Location = new System.Drawing.Point(76, 6);
            this.lblInactiveAcc.Name = "lblInactiveAcc";
            this.lblInactiveAcc.Size = new System.Drawing.Size(124, 20);
            this.lblInactiveAcc.TabIndex = 1;
            this.lblInactiveAcc.Text = "Inactive Accounts";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(8, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(63, 66);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // cardOnline
            // 
            this.cardOnline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardOnline.Controls.Add(this.lblOnlineValue);
            this.cardOnline.Controls.Add(this.lblOnlineNow);
            this.cardOnline.Controls.Add(this.pictureBox3);
            this.cardOnline.Location = new System.Drawing.Point(746, 13);
            this.cardOnline.Name = "cardOnline";
            this.cardOnline.Size = new System.Drawing.Size(235, 74);
            this.cardOnline.TabIndex = 5;
            // 
            // lblOnlineValue
            // 
            this.lblOnlineValue.AutoSize = true;
            this.lblOnlineValue.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnlineValue.Location = new System.Drawing.Point(71, 22);
            this.lblOnlineValue.Name = "lblOnlineValue";
            this.lblOnlineValue.Size = new System.Drawing.Size(39, 45);
            this.lblOnlineValue.TabIndex = 2;
            this.lblOnlineValue.Text = "4";
            // 
            // lblOnlineNow
            // 
            this.lblOnlineNow.AutoSize = true;
            this.lblOnlineNow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnlineNow.Location = new System.Drawing.Point(76, 6);
            this.lblOnlineNow.Name = "lblOnlineNow";
            this.lblOnlineNow.Size = new System.Drawing.Size(87, 20);
            this.lblOnlineNow.TabIndex = 1;
            this.lblOnlineNow.Text = "Online Now";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(8, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(63, 66);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.SystemColors.Control;
            this.btnExport.Location = new System.Drawing.Point(892, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(99, 43);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.BackColor = System.Drawing.Color.ForestGreen;
            this.btnManageUsers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageUsers.ForeColor = System.Drawing.Color.White;
            this.btnManageUsers.Location = new System.Drawing.Point(997, 5);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(150, 42);
            this.btnManageUsers.TabIndex = 4;
            this.btnManageUsers.Text = "Manage Users";
            this.btnManageUsers.UseVisualStyleBackColor = false;
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtSearchBox.Location = new System.Drawing.Point(882, 7);
            this.txtSearchBox.Multiline = true;
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(245, 29);
            this.txtSearchBox.TabIndex = 6;
            this.txtSearchBox.Text = "Search keyword...";
            //this.txtSearchBox.TextChanged += new System.EventHandler(this.txtSearchBox_TextChanged_1);
            // 
            // lblUserList
            // 
            this.lblUserList.AutoSize = true;
            this.lblUserList.Font = new System.Drawing.Font("Segoe UI Black", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserList.Location = new System.Drawing.Point(3, 5);
            this.lblUserList.Name = "lblUserList";
            this.lblUserList.Size = new System.Drawing.Size(140, 31);
            this.lblUserList.TabIndex = 5;
            this.lblUserList.Text = "USER LISTS";
            //this.lblUserList.Click += new System.EventHandler(this.lblUserList_Click_1);
            // 
            // pnlTableHeader
            // 
            this.pnlTableHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlTableHeader.Controls.Add(this.lblUserList);
            this.pnlTableHeader.Controls.Add(this.txtSearchBox);
            this.pnlTableHeader.Location = new System.Drawing.Point(10, 162);
            this.pnlTableHeader.Name = "pnlTableHeader";
            this.pnlTableHeader.Size = new System.Drawing.Size(1136, 39);
            this.pnlTableHeader.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.dgvUsers);
            this.panel1.Location = new System.Drawing.Point(10, 210);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1137, 473);
            this.panel1.TabIndex = 8;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToOrderColumns = true;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDColumn,
            this.fullnameColumn,
            this.UsernameColumn,
            this.RoleColumn,
            this.statusColumn,
            this.LastLoginColumn});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgvUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.RowTemplate.Height = 24;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(1137, 473);
            this.dgvUsers.TabIndex = 0;
            //this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            // 
            // IDColumn
            // 
            this.IDColumn.HeaderText = "#ID";
            this.IDColumn.MinimumWidth = 6;
            this.IDColumn.Name = "IDColumn";
            this.IDColumn.ReadOnly = true;
            // 
            // fullnameColumn
            // 
            this.fullnameColumn.HeaderText = "FULL NAME";
            this.fullnameColumn.MinimumWidth = 6;
            this.fullnameColumn.Name = "fullnameColumn";
            this.fullnameColumn.ReadOnly = true;
            // 
            // UsernameColumn
            // 
            this.UsernameColumn.HeaderText = "USERNAME";
            this.UsernameColumn.MinimumWidth = 6;
            this.UsernameColumn.Name = "UsernameColumn";
            this.UsernameColumn.ReadOnly = true;
            // 
            // RoleColumn
            // 
            this.RoleColumn.HeaderText = "ROLE";
            this.RoleColumn.MinimumWidth = 6;
            this.RoleColumn.Name = "RoleColumn";
            this.RoleColumn.ReadOnly = true;
            // 
            // statusColumn
            // 
            this.statusColumn.HeaderText = "STATUS";
            this.statusColumn.Items.AddRange(new object[] {
            "ACTIVE",
            "INACTIVE"});
            this.statusColumn.MinimumWidth = 6;
            this.statusColumn.Name = "statusColumn";
            this.statusColumn.ReadOnly = true;
            this.statusColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.statusColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LastLoginColumn
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.Format = "g";
            dataGridViewCellStyle4.NullValue = null;
            this.LastLoginColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.LastLoginColumn.HeaderText = "LAST LOG-IN";
            this.LastLoginColumn.MinimumWidth = 6;
            this.LastLoginColumn.Name = "LastLoginColumn";
            this.LastLoginColumn.ReadOnly = true;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(902, 12);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(29, 28);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            //this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // UserSuperAdminSide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlTableHeader);
            this.Controls.Add(this.btnManageUsers);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.userheader);
            this.Margin = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.Name = "UserSuperAdminSide";
            this.Size = new System.Drawing.Size(1164, 709);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.cardTotalUsers.ResumeLayout(false);
            this.cardTotalUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTotalIcon)).EndInit();
            this.cardActiveAdmins.ResumeLayout(false);
            this.cardActiveAdmins.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.cardInactive.ResumeLayout(false);
            this.cardInactive.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.cardOnline.ResumeLayout(false);
            this.cardOnline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.pnlTableHeader.ResumeLayout(false);
            this.pnlTableHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label userheader;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel cardTotalUsers;
        private System.Windows.Forms.PictureBox picTotalIcon;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Panel cardActiveAdmins;
        private System.Windows.Forms.Label lblActiveValue;
        private System.Windows.Forms.Label lblActiveAdmins;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel cardInactive;
        private System.Windows.Forms.Label lblInactiveValue;
        private System.Windows.Forms.Label lblInactiveAcc;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel cardOnline;
        private System.Windows.Forms.Label lblOnlineValue;
        private System.Windows.Forms.Label lblOnlineNow;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.TextBox txtSearchBox;
        private System.Windows.Forms.Label lblUserList;
        private System.Windows.Forms.Panel pnlTableHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullnameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsernameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoleColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn statusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastLoginColumn;
    }
}
