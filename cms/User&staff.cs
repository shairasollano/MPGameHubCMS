using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace cms
{
    // =========================== DATABASE MODELS ===========================
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    // =========================== DATABASE CONTEXT ===========================
    public class MatchpointDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection string for local SQL Server
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=matchpoint_db;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }

    // =========================== DATABASE HELPER CLASS ===========================
    public class DatabaseHelper
    {
        private static MatchpointDbContext _context;

        public static void InitializeDatabase()
        {
            try
            {
                _context = new MatchpointDbContext();

                // Create database if it doesn't exist
                _context.Database.EnsureCreated();

                // Check if users table is empty
                if (!_context.Users.Any())
                {
                    CreateStaticAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CreateStaticAccounts()
        {
            var staticUsers = new List<User>
            {
                new User
                {
                    UserId = "USR001",
                    Username = "admin",
                    Password = "admin123",
                    Role = "ADMIN",
                    Status = "ACTIVE",
                    CreatedDate = DateTime.Now
                },
                new User
                {
                    UserId = "USR002",
                    Username = "manager",
                    Password = "manager123",
                    Role = "MANAGER",
                    Status = "ACTIVE",
                    CreatedDate = DateTime.Now
                },
                new User
                {
                    UserId = "USR003",
                    Username = "staff",
                    Password = "staff123",
                    Role = "STAFF",
                    Status = "ACTIVE",
                    CreatedDate = DateTime.Now
                }
            };

            _context.Users.AddRange(staticUsers);
            _context.SaveChanges();
        }

        public static List<User> GetAllUsers()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<User>();
            }
        }

        public static bool AddUser(string username, string role, string password = "12345")
        {
            try
            {
                // Check if username already exists
                if (_context.Users.Any(u => u.Username == username))
                {
                    MessageBox.Show("Username already exists!", "Duplicate User",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Generate new UserId
                int nextId = _context.Users.Count() + 1;
                string userId = $"USR{nextId:D3}";

                var newUser = new User
                {
                    UserId = userId,
                    Username = username,
                    Password = password,
                    Role = role,
                    Status = "ACTIVE",
                    CreatedDate = DateTime.Now
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool UpdateUser(string oldUsername, string newUsername, string role)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == oldUsername);
                if (user != null)
                {
                    // Check if new username already exists (if changed)
                    if (oldUsername != newUsername && _context.Users.Any(u => u.Username == newUsername))
                    {
                        MessageBox.Show("Username already exists!", "Duplicate User",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    user.Username = newUsername;
                    user.Role = role;
                    user.LastModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool DeleteUser(string username)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool UpdateUserStatus(string username, string status)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    user.Status = status;
                    user.LastModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating status: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool ChangePassword(string username, string newPassword)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    user.Password = newPassword;
                    user.LastModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing password: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    // =========================== USER_STAFF FORM (UPDATED) ===========================
    public partial class User_staff : Form
    {
        public User_staff()
        {
            InitializeComponent();
            // Add event handlers for status changes
            this.datagrd.CellValueChanged += new DataGridViewCellEventHandler(this.STATUS_SelectedIndexChanged);
            this.datagrd.CurrentCellDirtyStateChanged += new EventHandler(this.datagrd_CurrentCellDirtyStateChanged);
        }

        private void RefreshUserGrid()
        {
            datagrd.Rows.Clear();
            var users = DatabaseHelper.GetAllUsers();

            foreach (var user in users)
            {
                int rowIndex = datagrd.Rows.Add(user.UserId, user.Username, user.Role, user.Status);

                // Style for admin account
                if (user.Username == "admin")
                {
                    datagrd.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Red;
                    datagrd.Rows[rowIndex].DefaultCellStyle.Font = new Font(datagrd.Font, FontStyle.Bold);
                }
            }
        }

        private void Manage_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(btnManage, 0, btnManage.Height);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUser addWindow = new frmAddUser();
            addWindow.ShowDialog();
            RefreshUserGrid(); // Refresh after adding
        }

        private void uPDATEUSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagrd.SelectedRows.Count > 0)
            {
                string selectedUser = datagrd.SelectedRows[0].Cells[1].Value.ToString();
                frmUpdateUser updateWindow = new frmUpdateUser();

                updateWindow.txtUpdateUsername.Text = selectedUser;
                updateWindow.cmbRole.Text = datagrd.SelectedRows[0].Cells[2].Value.ToString();

                if (selectedUser == "admin")
                {
                    updateWindow.cmbRole.Enabled = false;
                }

                updateWindow.ShowDialog();
                RefreshUserGrid(); // Refresh after update
            }
        }

        private void dELETEUSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagrd.SelectedRows.Count > 0)
            {
                string selectedUser = datagrd.SelectedRows[0].Cells[1].Value.ToString();

                if (selectedUser == "admin")
                {
                    MessageBox.Show("Security Violation: You cannot delete your own administrative account!",
                        "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete {selectedUser}?",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    if (DatabaseHelper.DeleteUser(selectedUser))
                    {
                        MessageBox.Show("User deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshUserGrid();
                    }
                }
            }
        }

        private void cHANGEPASSWORDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagrd.SelectedRows.Count > 0)
            {
                string selectedUser = datagrd.SelectedRows[0].Cells[1].Value.ToString();
                frmChangePassword changePasswordWindow = new frmChangePassword(selectedUser);
                changePasswordWindow.ShowDialog();
            }
        }

        private void mainform_Load(object sender, EventArgs e)
        {
            // Initialize database and load users
            DatabaseHelper.InitializeDatabase();
            RefreshUserGrid();
        }

        private void STATUS_SelectedIndexChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 3) // Status column index
            {
                string username = datagrd.Rows[e.RowIndex].Cells[1].Value.ToString();
                string newStatus = datagrd.Rows[e.RowIndex].Cells[3].Value.ToString();

                if (username != "admin") // Don't allow status change for admin
                {
                    DatabaseHelper.UpdateUserStatus(username, newStatus);
                }
                else
                {
                    MessageBox.Show("Cannot change status of admin account!", "Action Denied",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    RefreshUserGrid(); // Revert the change
                }
            }
        }

        private void datagrd_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (datagrd.IsCurrentCellDirty)
            {
                datagrd.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }

    // =========================== FRMUPDATEUSER (UPDATED) ===========================
    public partial class frmUpdateUser : Form
    {
        private string oldUsername;

        public frmUpdateUser()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string oldUsername = txtUpdateUsername.Text; // Store old username
            string newUsername = txtUpdateUsername.Text;
            string newRole = cmbRole.Text;

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Username cannot be empty!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DatabaseHelper.UpdateUser(oldUsername, newUsername, newRole))
            {
                MessageBox.Show("User updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    // =========================== FRMADDUSER (UPDATED) ===========================
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
            cmbRole.SelectedIndex = 0; // Select MANAGER by default
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string role = cmbRole.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string defaultPassword = "12345";

            if (DatabaseHelper.AddUser(username, role, defaultPassword))
            {
                MessageBox.Show($"User {username} created successfully!\nDefault password is: {defaultPassword}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    // =========================== FRMCHANGEPASSWORD (NEW FORM) ===========================
    public partial class frmChangePassword : Form
    {
        private string username;

        public frmChangePassword(string user)
        {
            InitializeComponent();
            username = user;
            lblUsername.Text = $"Changing password for: {username}";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Password cannot be empty!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DatabaseHelper.ChangePassword(username, txtNewPassword.Text))
            {
                MessageBox.Show("Password changed successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Add this partial class for frmChangePassword designer
    public partial class frmChangePassword
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
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
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(97, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "CHANGE PASSWORD";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.Maroon;
            this.lblUsername.Location = new System.Drawing.Point(48, 84);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(0, 23);
            this.lblUsername.TabIndex = 1;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtNewPassword.Location = new System.Drawing.Point(52, 164);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(329, 34);
            this.txtNewPassword.TabIndex = 2;
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtConfirmPassword.Location = new System.Drawing.Point(52, 243);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(329, 34);
            this.txtConfirmPassword.TabIndex = 3;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Green;
            this.btnConfirm.Location = new System.Drawing.Point(61, 313);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(141, 55);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "CONFIRM";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(217, 313);
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
            this.label2.Location = new System.Drawing.Point(48, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "NEW PASSWORD";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(48, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "CONFIRM PASSWORD";
            // 
            // frmChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(432, 400);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "frmChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}