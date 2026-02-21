using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finaluserandstaff
{
    // =========================== MAINFORM ===========================
    public partial class mainform : Form
    {
        public mainform()
        {
            InitializeComponent();
        }

        private void Manage_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(btnManage, 0, btnManage.Height);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUser addWindow = new frmAddUser();
            addWindow.ShowDialog();
        }

        private void uPDATEUSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagrd.SelectedRows.Count > 0)
            {
                string selectedUser = datagrd.SelectedRows[0].Cells[1].Value.ToString();
                frmUpdateUser updateWindow = new frmUpdateUser();

                updateWindow.txtUpdateUsername.Text = selectedUser;
                updateWindow.cmbRole.Text = datagrd.SelectedRows[0].Cells[2].Value.ToString();

                if (selectedUser == "admin01")
                {
                    updateWindow.cmbRole.Enabled = false;
                }

                updateWindow.ShowDialog();
            }
        }

        private void dELETEUSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagrd.SelectedRows.Count > 0)
            {
                string selectedUser = datagrd.SelectedRows[0].Cells[1].Value.ToString();

                if (selectedUser == "admin01")
                {
                    MessageBox.Show("Security Violation: You cannot delete your own administrative account!",
                        "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete {selectedUser}?",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    datagrd.Rows.RemoveAt(datagrd.SelectedRows[0].Index);
                }
            }
        }

        private void mainform_Load(object sender, EventArgs e)
        {
            int rowIndex = datagrd.Rows.Add("00001", "admin01", "MANAGER", "ACTIVE");
            datagrd.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Red;
            datagrd.Rows[rowIndex].DefaultCellStyle.Font = new Font(datagrd.Font, FontStyle.Bold);
        }
    }

    public partial class mainform
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
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.datagrd = new System.Windows.Forms.DataGridView();
            this.btnManage = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.uPDATEUSERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dELETEUSERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cHANGEPASSWORDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagrd)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.Black;
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(147, 726);
            this.pnlMenu.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Maroon;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(144, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1118, 76);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Snow;
            this.panel2.Controls.Add(this.datagrd);
            this.panel2.Location = new System.Drawing.Point(171, 145);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(20);
            this.panel2.Size = new System.Drawing.Size(1056, 495);
            this.panel2.TabIndex = 1;
            // 
            // datagrd
            // 
            this.datagrd.AllowUserToAddRows = false;
            this.datagrd.AllowUserToDeleteRows = false;
            this.datagrd.AllowUserToResizeRows = false;
            this.datagrd.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.datagrd.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.datagrd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagrd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NAME,
            this.ROLE,
            this.STATUS});
            this.datagrd.GridColor = System.Drawing.Color.IndianRed;
            this.datagrd.Location = new System.Drawing.Point(16, 14);
            this.datagrd.MultiSelect = false;
            this.datagrd.Name = "datagrd";
            this.datagrd.RowHeadersWidth = 51;
            this.datagrd.RowTemplate.Height = 24;
            this.datagrd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagrd.Size = new System.Drawing.Size(1024, 463);
            this.datagrd.TabIndex = 0;
            // 
            // btnManage
            // 
            this.btnManage.BackColor = System.Drawing.Color.IndianRed;
            this.btnManage.ForeColor = System.Drawing.Color.White;
            this.btnManage.Location = new System.Drawing.Point(997, 97);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(229, 37);
            this.btnManage.TabIndex = 2;
            this.btnManage.Text = "MANAGE USERS";
            this.btnManage.UseVisualStyleBackColor = false;
            this.btnManage.Click += new System.EventHandler(this.Manage_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.uPDATEUSERToolStripMenuItem,
            this.dELETEUSERToolStripMenuItem,
            this.cHANGEPASSWORDToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(220, 100);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(219, 24);
            this.toolStripMenuItem1.Text = "ADD NEW USER";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // uPDATEUSERToolStripMenuItem
            // 
            this.uPDATEUSERToolStripMenuItem.Name = "uPDATEUSERToolStripMenuItem";
            this.uPDATEUSERToolStripMenuItem.Size = new System.Drawing.Size(219, 24);
            this.uPDATEUSERToolStripMenuItem.Text = "UPDATE USER";
            this.uPDATEUSERToolStripMenuItem.Click += new System.EventHandler(this.uPDATEUSERToolStripMenuItem_Click);
            // 
            // dELETEUSERToolStripMenuItem
            // 
            this.dELETEUSERToolStripMenuItem.Name = "dELETEUSERToolStripMenuItem";
            this.dELETEUSERToolStripMenuItem.Size = new System.Drawing.Size(219, 24);
            this.dELETEUSERToolStripMenuItem.Text = "DELETE USER";
            this.dELETEUSERToolStripMenuItem.Click += new System.EventHandler(this.dELETEUSERToolStripMenuItem_Click);
            // 
            // cHANGEPASSWORDToolStripMenuItem
            // 
            this.cHANGEPASSWORDToolStripMenuItem.Name = "cHANGEPASSWORDToolStripMenuItem";
            this.cHANGEPASSWORDToolStripMenuItem.Size = new System.Drawing.Size(219, 24);
            this.cHANGEPASSWORDToolStripMenuItem.Text = "CHANGE PASSWORD";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "USER AND STAFF MANAGEMENT | ADMIN SIDE";
            // 
            // ID
            // 
            this.ID.HeaderText = "#ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // NAME
            // 
            this.NAME.HeaderText = "USERNAME";
            this.NAME.MinimumWidth = 6;
            this.NAME.Name = "NAME";
            this.NAME.ReadOnly = true;
            // 
            // ROLE
            // 
            this.ROLE.HeaderText = "ROLE";
            this.ROLE.MinimumWidth = 6;
            this.ROLE.Name = "ROLE";
            this.ROLE.ReadOnly = true;
            // 
            // STATUS
            // 
            this.STATUS.HeaderText = "STATUS";
            this.STATUS.Items.AddRange(new object[] {
            "ACTIVE",
            "INACTIVE"});
            this.STATUS.MinimumWidth = 6;
            this.STATUS.Name = "STATUS";
            this.STATUS.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.STATUS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1256, 726);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mainform";
            this.Load += new System.EventHandler(this.mainform_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagrd)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnManage;
        public System.Windows.Forms.DataGridView datagrd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem uPDATEUSERToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dELETEUSERToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cHANGEPASSWORDToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROLE;
        private System.Windows.Forms.DataGridViewComboBoxColumn STATUS;
    }

    // =========================== FRMUPDATEUSER ===========================
    public partial class frmUpdateUser : Form
    {
        public frmUpdateUser()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            mainform main = (mainform)Application.OpenForms["mainform"];

            if (main != null && main.datagrd.SelectedRows.Count > 0)
            {
                main.datagrd.SelectedRows[0].Cells[1].Value = txtUpdateUsername.Text;
                main.datagrd.SelectedRows[0].Cells[2].Value = cmbRole.Text;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }

    public partial class frmUpdateUser
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtUpdateUsername = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Maroon;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 71);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "UPDATE FORM";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.Font = new System.Windows.Forms.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRole.ForeColor = System.Drawing.Color.Maroon;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "MANAGER",
            "STAFF",
            "BOTH"});
            this.cmbRole.Location = new System.Drawing.Point(52, 203);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(329, 31);
            this.cmbRole.TabIndex = 2;
            this.cmbRole.Text = "SELECT ROLE..";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Green;
            this.btnConfirm.Location = new System.Drawing.Point(61, 251);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(141, 55);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "CONFIRM";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtUpdateUsername
            // 
            this.txtUpdateUsername.Font = new System.Windows.Forms.Font("Segoe UI Semibold", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdateUsername.Location = new System.Drawing.Point(52, 154);
            this.txtUpdateUsername.Name = "txtUpdateUsername";
            this.txtUpdateUsername.Size = new System.Drawing.Size(329, 38);
            this.txtUpdateUsername.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(217, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 55);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(48, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "ENTER NEW USERNAME";
            // 
            // frmUpdateUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(432, 382);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtUpdateUsername);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmUpdateUser";
            this.Text = "Update User";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox cmbRole;
        public System.Windows.Forms.TextBox txtUpdateUsername;
        private System.Windows.Forms.Label label2;
    }

    // =========================== FRMADDUSER ===========================
    public partial class frmAddUser : Form
    {
        public frmAddUser()
        {
            InitializeComponent();
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("MANAGER");
            cmbRole.Items.Add("STAFF");
            cmbRole.SelectedIndex = 1; // Automatically selects 'STAFF'
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var main = Application.OpenForms["mainform"] as mainform;

            if (main != null)
            {
                string id = "#" + new Random().Next(1000, 9999);
                string user = txtUsername.Text;
                string role = cmbRole.Text;
                string status = "ACTIVE";

                string defaultPassword = "12345";

                main.datagrd.Rows.Add(id, user, role, status);

                MessageBox.Show($"User {user} created! Default password is: {defaultPassword}");
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public partial class frmAddUser
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Maroon;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 71);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "ADD USER FORM";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.Font = new System.Windows.Forms.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRole.ForeColor = System.Drawing.Color.Maroon;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "MANAGER",
            "STAFF",
            "BOTH"});
            this.cmbRole.Location = new System.Drawing.Point(52, 203);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(329, 31);
            this.cmbRole.TabIndex = 2;
            this.cmbRole.Text = "SELECT ROLE..";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Green;
            this.btnConfirm.Location = new System.Drawing.Point(61, 251);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(141, 55);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "CONFIRM";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(52, 154);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(329, 38);
            this.txtUsername.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(217, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 55);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(48, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "ENTER NEW USERNAME";
            // 
            // frmAddUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(432, 382);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.panel1);
            this.Font = new System.Windows.Forms.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAddUser";
            this.Text = "Add User";
            this.Load += new System.EventHandler(this.frmAddUser_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox cmbRole;
        public System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
    }
}