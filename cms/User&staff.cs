using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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

    // =========================== DATABASE HELPER CLASS WITH AUTO CREATE TABLE ===========================
    public class DatabaseHelper
    {
        private static string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";

        public static void InitializeDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Create database if it doesn't exist
                    string createDbQuery = "CREATE DATABASE IF NOT EXISTS matchpoint_db";
                    using (MySqlCommand cmd = new MySqlCommand(createDbQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Switch to matchpoint_db
                    conn.ChangeDatabase("matchpoint_db");

                    // Check if users table exists
                    string checkTableQuery = "SHOW TABLES LIKE 'users'";
                    using (MySqlCommand cmd = new MySqlCommand(checkTableQuery, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            // Create users table with all required columns
                            string createTableQuery = @"
                                CREATE TABLE users (
                                    Id INT PRIMARY KEY AUTO_INCREMENT,
                                    UserId VARCHAR(20) NOT NULL UNIQUE,
                                    Username VARCHAR(50) NOT NULL UNIQUE,
                                    Password VARCHAR(100) NOT NULL,
                                    Role VARCHAR(20) NOT NULL,
                                    Status VARCHAR(10) NOT NULL DEFAULT 'ACTIVE',
                                    CreatedDate DATETIME NOT NULL,
                                    LastModifiedDate DATETIME
                                )";

                            using (MySqlCommand createCmd = new MySqlCommand(createTableQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }

                            // Insert static accounts
                            CreateStaticAccounts(conn);
                        }
                        else
                        {
                            // Check if table is empty
                            string checkEmptyQuery = "SELECT COUNT(*) FROM users";
                            using (MySqlCommand countCmd = new MySqlCommand(checkEmptyQuery, conn))
                            {
                                int userCount = Convert.ToInt32(countCmd.ExecuteScalar());
                                if (userCount == 0)
                                {
                                    CreateStaticAccounts(conn);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error: {ex.Message}\n\n" +
                    "Please make sure XAMPP is running and MySQL is started.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CreateStaticAccounts(MySqlConnection conn)
        {
            string insertQuery = @"
                INSERT INTO users (UserId, Username, Password, Role, Status, CreatedDate) 
                VALUES (@UserId, @Username, @Password, @Role, @Status, @CreatedDate)";

            var staticUsers = new[]
            {
                new { UserId = "USR001", Username = "admin", Password = "admin123", Role = "ADMIN" },
                new { UserId = "USR002", Username = "manager", Password = "manager123", Role = "MANAGER" },
                new { UserId = "USR003", Username = "staff", Password = "staff123", Role = "STAFF" }
            };

            foreach (var user in staticUsers)
            {
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@Status", "ACTIVE");
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string query = "SELECT * FROM users ORDER BY Id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    UserId = reader["UserId"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                    LastModifiedDate = reader["LastModifiedDate"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["LastModifiedDate"]) : (DateTime?)null
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return users;
        }

        public static bool AddUser(string username, string role, string password = "12345")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @username";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists!", "Duplicate User",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    // Get next UserId
                    string idQuery = "SELECT COUNT(*) FROM users";
                    using (MySqlCommand idCmd = new MySqlCommand(idQuery, conn))
                    {
                        int nextId = Convert.ToInt32(idCmd.ExecuteScalar()) + 1;
                        string userId = $"USR{nextId:D3}";

                        string insertQuery = @"
                            INSERT INTO users (UserId, Username, Password, Role, Status, CreatedDate) 
                            VALUES (@UserId, @Username, @Password, @Role, @Status, @CreatedDate)";

                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@UserId", userId);
                            insertCmd.Parameters.AddWithValue("@Username", username);
                            insertCmd.Parameters.AddWithValue("@Password", password);
                            insertCmd.Parameters.AddWithValue("@Role", role);
                            insertCmd.Parameters.AddWithValue("@Status", "ACTIVE");
                            insertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

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
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    // Check if new username already exists (if changed)
                    if (oldUsername != newUsername)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @username";
                        using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@username", newUsername);
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (count > 0)
                            {
                                MessageBox.Show("Username already exists!", "Duplicate User",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }

                    string updateQuery = @"
                        UPDATE users 
                        SET Username = @newUsername, Role = @role, LastModifiedDate = @lastModified 
                        WHERE Username = @oldUsername";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@newUsername", newUsername);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@lastModified", DateTime.Now);
                        cmd.Parameters.AddWithValue("@oldUsername", oldUsername);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
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
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string deleteQuery = "DELETE FROM users WHERE Username = @username";
                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
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
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string updateQuery = @"
                        UPDATE users 
                        SET Status = @status, LastModifiedDate = @lastModified 
                        WHERE Username = @username";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@lastModified", DateTime.Now);
                        cmd.Parameters.AddWithValue("@username", username);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
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
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string updateQuery = @"
                        UPDATE users 
                        SET Password = @password, LastModifiedDate = @lastModified 
                        WHERE Username = @username";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@password", newPassword);
                        cmd.Parameters.AddWithValue("@lastModified", DateTime.Now);
                        cmd.Parameters.AddWithValue("@username", username);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing password: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    // =========================== USER_STAFF FORM ===========================
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
            RefreshUserGrid();
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
                RefreshUserGrid();
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
            if (e.RowIndex >= 0 && e.ColumnIndex == 3)
            {
                string username = datagrd.Rows[e.RowIndex].Cells[1].Value.ToString();
                string newStatus = datagrd.Rows[e.RowIndex].Cells[3].Value.ToString();

                if (username != "admin")
                {
                    DatabaseHelper.UpdateUserStatus(username, newStatus);
                }
                else
                {
                    MessageBox.Show("Cannot change status of admin account!", "Action Denied",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    RefreshUserGrid();
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

    // Keep the rest of your forms (frmUpdateUser, frmAddUser, frmChangePassword) as they are
    // They already use DatabaseHelper methods which now work with MySQL
}