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
    public partial class UserManagementControl : UserControl
    {
        // UI Components
        private Panel panelHeader;
        private Panel panel2;
        private DataGridView datagrd;
        private Button btnManage;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem uPDATEUSERToolStripMenuItem;
        private ToolStripMenuItem dELETEUSERToolStripMenuItem;
        private ToolStripMenuItem cHANGEPASSWORDToolStripMenuItem;
        private Label labelTitle;

        // DataGridView Columns
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn NAME;
        private DataGridViewTextBoxColumn ROLE;
        private Button btnEdit;
        private Button btnFilter;
        private ContextMenuStrip filterContextMenu;
        private ToolStripMenuItem ALL;
        private ToolStripMenuItem MANAGER;
        private ToolStripMenuItem STAFF;
        private TextBox txtSearch;
        private DataGridViewComboBoxColumn STATUS;

        public UserManagementControl()
        {
            InitializeComponent();
            this.datagrd.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.datagrd_CellBeginEdit);
        }

        private void datagrd_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Check if the user is clicking the STATUS column
            if (datagrd.Columns[e.ColumnIndex].Name == "STATUS")
            {
                string targetUser = datagrd.Rows[e.RowIndex].Cells["NAME"].Value.ToString();
                string currentUser = "admin01";

                // RULE: Only admin01 can change admin01's status
                // SUBSTITUTION: If target != current, Cancel = true
                if (targetUser != currentUser)
                {
                    MessageBox.Show("Security Restriction: You can only change your own status!", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    e.Cancel = true; // This stops the dropdown from opening
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean up components if needed
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnManage = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.datagrd = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dELETEUSERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uPDATEUSERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cHANGEPASSWORDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ALL = new System.Windows.Forms.ToolStripMenuItem();
            this.MANAGER = new System.Windows.Forms.ToolStripMenuItem();
            this.STAFF = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeader.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagrd)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.filterContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.panelHeader.Controls.Add(this.txtSearch);
            this.panelHeader.Controls.Add(this.btnFilter);
            this.panelHeader.Controls.Add(this.btnEdit);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Controls.Add(this.btnManage);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1124, 80);
            this.panelHeader.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI Semilight", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.Black;
            this.txtSearch.Location = new System.Drawing.Point(477, 30);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(253, 23);
            this.txtSearch.TabIndex = 10;
            this.txtSearch.Text = "search here";
            this.txtSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSearch_MouseClick);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(743, 20);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(116, 40);
            this.btnFilter.TabIndex = 3;
            this.btnFilter.Text = "FILTER";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.btnEdit.Location = new System.Drawing.Point(736, 20);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(180, 40);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "EDIT MY PROFILE";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.labelTitle.Location = new System.Drawing.Point(15, 27);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(464, 28);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "USER AND STAFF MANAGEMENT | ADMIN SIDE";
            // 
            // btnManage
            // 
            this.btnManage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(91)))), ((int)(((byte)(86)))));
            this.btnManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManage.ForeColor = System.Drawing.Color.White;
            this.btnManage.Location = new System.Drawing.Point(924, 20);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(180, 40);
            this.btnManage.TabIndex = 1;
            this.btnManage.Text = "MANAGE USERS";
            this.btnManage.UseVisualStyleBackColor = false;
            this.btnManage.Click += new System.EventHandler(this.Manage_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.datagrd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 80);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(20);
            this.panel2.Size = new System.Drawing.Size(1124, 420);
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
            this.datagrd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagrd.GridColor = System.Drawing.Color.IndianRed;
            this.datagrd.Location = new System.Drawing.Point(20, 20);
            this.datagrd.MultiSelect = false;
            this.datagrd.Name = "datagrd";
            this.datagrd.RowHeadersWidth = 51;
            this.datagrd.RowTemplate.Height = 24;
            this.datagrd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagrd.Size = new System.Drawing.Size(1084, 380);
            this.datagrd.TabIndex = 0;
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.dELETEUSERToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(187, 52);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(186, 24);
            this.toolStripMenuItem1.Text = "ADD NEW USER";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // dELETEUSERToolStripMenuItem
            // 
            this.dELETEUSERToolStripMenuItem.Name = "dELETEUSERToolStripMenuItem";
            this.dELETEUSERToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.dELETEUSERToolStripMenuItem.Text = "DELETE USER";
            this.dELETEUSERToolStripMenuItem.Click += new System.EventHandler(this.dELETEUSERToolStripMenuItem_Click);
            // 
            // uPDATEUSERToolStripMenuItem
            // 
            this.uPDATEUSERToolStripMenuItem.Name = "uPDATEUSERToolStripMenuItem";
            this.uPDATEUSERToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // cHANGEPASSWORDToolStripMenuItem
            // 
            this.cHANGEPASSWORDToolStripMenuItem.Name = "cHANGEPASSWORDToolStripMenuItem";
            this.cHANGEPASSWORDToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // filterContextMenu
            // 
            this.filterContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.filterContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ALL,
            this.MANAGER,
            this.STAFF});
            this.filterContextMenu.Name = "filterContextMenu";
            this.filterContextMenu.Size = new System.Drawing.Size(150, 76);
            this.filterContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.filterContextMenu_Opening);
            // 
            // ALL
            // 
            this.ALL.Name = "ALL";
            this.ALL.Size = new System.Drawing.Size(149, 24);
            this.ALL.Text = "ALL";
            this.ALL.Click += new System.EventHandler(this.ALL_Click);
            // 
            // MANAGER
            // 
            this.MANAGER.Name = "MANAGER";
            this.MANAGER.Size = new System.Drawing.Size(149, 24);
            this.MANAGER.Text = "MANAGER";
            this.MANAGER.Click += new System.EventHandler(this.MANAGER_Click);
            // 
            // STAFF
            // 
            this.STAFF.Name = "STAFF";
            this.STAFF.Size = new System.Drawing.Size(149, 24);
            this.STAFF.Text = "STAFF";
            this.STAFF.Click += new System.EventHandler(this.STAFF_Click);
            // 
            // UserManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserManagementControl";
            this.Size = new System.Drawing.Size(1124, 500);
            this.Load += new System.EventHandler(this.UserManagementControl_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagrd)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.filterContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.IContainer components;

        // Event Handlers
        private void Manage_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(btnManage, 0, btnManage.Height);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowAddUserForm();
        }




        private void dELETEUSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagrd.SelectedRows.Count > 0)
            {
                string selectedUser = datagrd.SelectedRows[0].Cells["NAME"].Value.ToString();

                if (selectedUser == "admin01")
                {
                    MessageBox.Show("Security Violation: You cannot delete your own administrative account!",
                        "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete user: {selectedUser}?",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    datagrd.Rows.RemoveAt(datagrd.SelectedRows[0].Index);
                    MessageBox.Show("User deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void UserManagementControl_Load(object sender, EventArgs e)
        {
            LoadSampleData();
            this.ActiveControl = labelTitle;
        }

        private void LoadSampleData()
        {
            datagrd.Rows.Clear();

            int rowIndex = datagrd.Rows.Add("00001", "admin01", "MANAGER", "ACTIVE");
            datagrd.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Red;
            datagrd.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font(datagrd.Font, System.Drawing.FontStyle.Bold);

            for (int i = 2; i <= 20; i++)
            {
                string id = i.ToString("D5");
                string username = $"user{i:00}";
                string role = (i % 3 == 0) ? "STAFF" : "MANAGER";
                string status = (i % 4 == 0) ? "INACTIVE" : "ACTIVE";

                datagrd.Rows.Add(id, username, role, status);
            }
        }

        // ===== UPDATE USER FORM (Hardcoded) =====
        private void ShowUpdateUserForm(string username, string currentRole, int rowIndex)
        {
            Form updateForm = new Form();
            updateForm.Text = "Update User";
            updateForm.Size = new Size(450, 400);
            updateForm.StartPosition = FormStartPosition.CenterParent;
            updateForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            updateForm.MaximizeBox = false;
            updateForm.MinimizeBox = false;
            updateForm.BackColor = Color.MistyRose;

            // Username TextBox
            TextBox txtUsername = new TextBox();
            txtUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            txtUsername.Location = new Point(50, 120);
            txtUsername.Size = new Size(350, 38);
            txtUsername.Text = username;

            // --- APPLY READONLY HERE ---
            txtUsername.ReadOnly = true; // This locks the text so it can't be changed
            txtUsername.BackColor = Color.LightGray; // Makes it look disabled to the user

            // Maroon Header Panel
            Panel headerPanel = new Panel();
            headerPanel.BackColor = Color.Maroon;
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 71;

            Label lblHeader = new Label();
            lblHeader.Text = "UPDATE FORM";
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.AutoSize = true;
            lblHeader.Location = new Point(120, 20);

            headerPanel.Controls.Add(lblHeader);

            // Username Label
            Label lblUsername = new Label();
            lblUsername.Text = "ENTER NEW USERNAME";
            lblUsername.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            lblUsername.ForeColor = Color.Maroon;
            lblUsername.Location = new Point(50, 90);
            lblUsername.AutoSize = true;

            // Username TextBox

            txtUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            txtUsername.Location = new Point(50, 120);
            txtUsername.Size = new Size(350, 38);
            txtUsername.Text = username;

            // Role ComboBox
            ComboBox cmbRole = new ComboBox();
            cmbRole.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            cmbRole.ForeColor = Color.Maroon;
            cmbRole.Location = new Point(50, 200);
            cmbRole.Size = new Size(350, 31);
            cmbRole.Items.AddRange(new object[] { "MANAGER", "STAFF" });
            cmbRole.Text = currentRole;

            // Buttons
            Button btnConfirm = new Button();
            btnConfirm.Text = "CONFIRM";
            btnConfirm.BackColor = Color.Green;
            btnConfirm.ForeColor = Color.White;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            btnConfirm.Location = new Point(70, 260);
            btnConfirm.Size = new Size(140, 55);
            btnConfirm.Click += (s, eArgs) =>
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbRole.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                datagrd.Rows[rowIndex].Cells["NAME"].Value = txtUsername.Text;
                datagrd.Rows[rowIndex].Cells["ROLE"].Value = cmbRole.Text;

                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateForm.DialogResult = DialogResult.OK;
                updateForm.Close();
            };

            Button btnCancel = new Button();
            btnCancel.Text = "CANCEL";
            btnCancel.BackColor = Color.Red;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            btnCancel.Location = new Point(230, 260);
            btnCancel.Size = new Size(140, 55);
            btnCancel.Click += (s, eArgs) =>
            {
                updateForm.DialogResult = DialogResult.Cancel;
                updateForm.Close();
            };


            updateForm.Controls.AddRange(new Control[]
            {
                headerPanel, lblUsername, txtUsername, cmbRole, btnConfirm, btnCancel
            });

            updateForm.ShowDialog();
        }

        // ===== ADD USER FORM =====
        private void ShowAddUserForm()
        {
            Form addForm = new Form();
            addForm.Text = "Add New User";
            addForm.Size = new Size(400, 350);
            addForm.StartPosition = FormStartPosition.CenterParent;
            addForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            addForm.MaximizeBox = false;
            addForm.MinimizeBox = false;
            addForm.BackColor = Color.MistyRose;

            Label lblTitle = new Label();
            lblTitle.Text = "ADD NEW USER";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = Color.Maroon;
            lblTitle.Location = new Point(120, 20);
            lblTitle.AutoSize = true;

            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(50, 70);
            lblUsername.AutoSize = true;

            TextBox txtUsername = new TextBox();
            txtUsername.Location = new Point(150, 67);
            txtUsername.Size = new Size(200, 27);

            Label lblRole = new Label();
            lblRole.Text = "Role:";
            lblRole.Location = new Point(50, 110);
            lblRole.AutoSize = true;

            ComboBox cmbRole = new ComboBox();
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Location = new Point(150, 107);
            cmbRole.Size = new Size(200, 28);
            cmbRole.Items.AddRange(new object[] { "MANAGER", "STAFF" });
            cmbRole.SelectedIndex = 0;

            Label lblStatus = new Label();
            lblStatus.Text = "Status:";
            lblStatus.Location = new Point(50, 150);
            lblStatus.AutoSize = true;

            ComboBox cmbStatus = new ComboBox();
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Location = new Point(150, 147);
            cmbStatus.Size = new Size(200, 28);
            cmbStatus.Items.AddRange(new object[] { "ACTIVE", "INACTIVE" });
            cmbStatus.SelectedIndex = 0;

            Button btnAdd = new Button();
            btnAdd.Text = "ADD USER";
            btnAdd.BackColor = Color.Green;
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(80, 200);
            btnAdd.Size = new Size(120, 40);
            btnAdd.Click += (s, e) =>
            {
                string newUsername = txtUsername.Text.Trim();


                if (string.IsNullOrWhiteSpace(newUsername))
                {
                    MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                foreach (DataGridViewRow row in datagrd.Rows)
                {

                    if (row.Cells["NAME"].Value != null)
                    {
                        string existingUser = row.Cells["NAME"].Value.ToString();


                        if (existingUser.Equals(newUsername, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show($"The username '{newUsername}' already exists in the system!",
                                            "Duplicate Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }


                int newIdCount = datagrd.Rows.Count + 1;
                string formattedId = newIdCount.ToString("D5");
                datagrd.Rows.Add(formattedId, newUsername, cmbRole.Text, cmbStatus.Text);

                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addForm.Close();
            };

            Button btnCancel = new Button();
            btnCancel.Text = "CANCEL";
            btnCancel.BackColor = Color.Red;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(220, 200);
            btnCancel.Size = new Size(120, 40);
            btnCancel.Click += (s, e) => addForm.Close();

            addForm.Controls.AddRange(new Control[]
            {
                lblTitle, lblUsername, txtUsername, lblRole, cmbRole,
                lblStatus, cmbStatus, btnAdd, btnCancel
            });

            addForm.ShowDialog();
        }


        // ===== CHANGE PASSWORD FORM =====
        private void ShowChangePasswordForm(string username)
        {
            Form passForm = new Form();
            passForm.Text = "Change Password";
            passForm.Size = new Size(350, 250);
            passForm.StartPosition = FormStartPosition.CenterParent;
            passForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            passForm.MaximizeBox = false;
            passForm.MinimizeBox = false;

            Label lblUser = new Label();
            lblUser.Text = $"User: {username}";
            lblUser.Location = new Point(20, 20);
            lblUser.AutoSize = true;

            Label lblNewPass = new Label();
            lblNewPass.Text = "New Password:";
            lblNewPass.Location = new Point(20, 60);
            lblNewPass.AutoSize = true;

            TextBox txtNewPass = new TextBox();
            txtNewPass.Location = new Point(140, 57);
            txtNewPass.Size = new Size(160, 27);
            txtNewPass.PasswordChar = '*';

            Label lblConfirmPass = new Label();
            lblConfirmPass.Text = "Confirm Password:";
            lblConfirmPass.Location = new Point(20, 100);
            lblConfirmPass.AutoSize = true;

            TextBox txtConfirmPass = new TextBox();
            txtConfirmPass.Location = new Point(140, 97);
            txtConfirmPass.Size = new Size(160, 27);
            txtConfirmPass.PasswordChar = '*';

            Button btnChange = new Button();
            btnChange.Text = "CHANGE";
            btnChange.BackColor = Color.Green;
            btnChange.ForeColor = Color.White;
            btnChange.Location = new Point(60, 150);
            btnChange.Size = new Size(100, 40);
            btnChange.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNewPass.Text))
                {
                    MessageBox.Show("Please enter a new password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtNewPass.Text != txtConfirmPass.Text)
                {
                    MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show($"Password for {username} has been changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                passForm.DialogResult = DialogResult.OK;
                passForm.Close();
            };

            Button btnCancel = new Button();
            btnCancel.Text = "CANCEL";
            btnCancel.BackColor = Color.Red;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(180, 150);
            btnCancel.Size = new Size(100, 40);
            btnCancel.Click += (s, e) =>
            {
                passForm.DialogResult = DialogResult.Cancel;
                passForm.Close();
            };

            passForm.Controls.AddRange(new Control[]
            {
                lblUser, lblNewPass, txtNewPass, lblConfirmPass, txtConfirmPass, btnChange, btnCancel
            });

            passForm.ShowDialog();
        }

        private void ShowMyProfileEditForm(string username)
        {
            Form profileForm = new Form();
            profileForm.Text = "Edit My Profile";
            profileForm.Size = new Size(350, 420); // Height increased to fit greeting
            profileForm.StartPosition = FormStartPosition.CenterParent;
            profileForm.FormBorderStyle = FormBorderStyle.FixedDialog;

            // --- GREETING TEXTBOX (THE NEW INPUT/HEADER) ---
            TextBox txtGreeting = new TextBox();
            txtGreeting.Text = $"LOGGED IN AS: {username.ToUpper()}";
            txtGreeting.Location = new Point(20, 20);
            txtGreeting.Size = new Size(280, 30);
            txtGreeting.ReadOnly = true; // Prevents accidental typing
            txtGreeting.BackColor = Color.WhiteSmoke;
            txtGreeting.TextAlign = HorizontalAlignment.Center;
            txtGreeting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);


            Label lblNew = new Label() { Text = "New Password:", Location = new Point(20, 75), AutoSize = true };
            TextBox txtNewPass = new TextBox() { Location = new Point(20, 100), Size = new Size(280, 27), PasswordChar = '*' };

            Label lblConfirm = new Label() { Text = "Confirm Password:", Location = new Point(20, 135), AutoSize = true };
            TextBox txtConfirmPass = new TextBox() { Location = new Point(20, 160), Size = new Size(280, 27), PasswordChar = '*' };

            CheckBox chkShowPass = new CheckBox() { Text = "Show Password", Location = new Point(20, 200), AutoSize = true };


            chkShowPass.CheckedChanged += (s, e) =>
            {
                char passwordChar = chkShowPass.Checked ? '\0' : '*';
                txtNewPass.PasswordChar = passwordChar;
                txtConfirmPass.PasswordChar = passwordChar;
            };

            Button btnUpdate = new Button() { Text = "UPDATE", Location = new Point(20, 250), Size = new Size(130, 40), BackColor = Color.LimeGreen };
            Button btnCancel = new Button() { Text = "CANCEL", Location = new Point(170, 250), Size = new Size(130, 40), BackColor = Color.IndianRed };

            btnUpdate.Click += (s, e) =>
            {
                if (txtNewPass.Text != txtConfirmPass.Text || string.IsNullOrWhiteSpace(txtNewPass.Text))
                {
                    MessageBox.Show("Passwords must match and cannot be empty!");
                    return;
                }


                cms.Activitylogs logs = new cms.Activitylogs();
                logs.AddLogEntry(username, "Self-Update", "Admin updated their own password credentials", "Info");

                MessageBox.Show("Profile updated successfully!");
                profileForm.Close();
            };

            btnCancel.Click += (s, e) => profileForm.Close();


            profileForm.Controls.AddRange(new Control[] { txtGreeting, lblNew, txtNewPass, lblConfirm, txtConfirmPass, chkShowPass, btnUpdate, btnCancel });
            profileForm.ShowDialog();
        }


        private void FilterRows(string role)
        {

            datagrd.CurrentCell = null;


            this.datagrd.SuspendLayout();

            try
            {
                foreach (DataGridViewRow row in datagrd.Rows)
                {

                    if (row.IsNewRow) continue;

                    if (role == "ALL")
                    {
                        row.Visible = true;
                    }
                    else
                    {

                        if (row.Cells["ROLE"].Value != null)
                        {
                            string rowRole = row.Cells["ROLE"].Value.ToString().ToUpper().Trim();


                            row.Visible = (rowRole == role.ToUpper());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Filter Error: " + ex.Message);
            }
            finally
            {

                this.datagrd.ResumeLayout();
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            filterContextMenu.Show(btnFilter, 0, btnFilter.Height);
        }


        private void ALL_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in datagrd.Rows)
            {
                row.Visible = true;
            }
        }

        private void MANAGER_Click(object sender, EventArgs e)
        {
            FilterRowsByRole("MANAGER");
        }

        private void STAFF_Click(object sender, EventArgs e)
        {
            FilterRowsByRole("STAFF");
        }

        private void FilterRowsByRole(string role)
        {

            datagrd.CurrentCell = null;

            foreach (DataGridViewRow row in datagrd.Rows)
            {
                if (row.Cells["ROLE"].Value != null)
                {
                    row.Visible = (row.Cells["ROLE"].Value.ToString() == role);
                }
            }
        }

        private void filterContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string myAdminUser = "admin01";
            ShowMyProfileEditForm(myAdminUser);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            if (txtSearch.Text == "search here" || txtSearch.ForeColor == Color.DimGray)
            {
                foreach (DataGridViewRow r in datagrd.Rows) r.Visible = true;
                return;
            }

            string searchText = txtSearch.Text.Trim().ToLower();
            datagrd.CurrentCell = null;

            foreach (DataGridViewRow row in datagrd.Rows)
            {
                if (row.Cells["NAME"].Value != null)
                {
                    string username = row.Cells["NAME"].Value.ToString().ToLower();
                    row.Visible = username.Contains(searchText);
                }
            }
        }


        private void txtSearch_Enter(object sender, EventArgs e)
        {

            if (txtSearch.Text == "search here")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "search here";
                txtSearch.ForeColor = Color.DimGray;
            }
        }

        private void txtSearch_MouseClick(object sender, MouseEventArgs e)
        {

            txtSearch_Enter(sender, e);
        }
    }
}