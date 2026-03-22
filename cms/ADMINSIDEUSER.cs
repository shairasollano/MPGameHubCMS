
using MySql.Data.MySqlClient;
using cms.lastsuper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace finaluserandstaff
{
    public partial class UserManagementControl : UserControl
    {
        // User Data Class
        public class UserData
        {
            public int DbId { get; set; }
            public string ID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public string Role { get; set; }
            public string Status { get; set; }
            public DateTime LastLogin { get; set; }
            public string Email { get; set; }
        }

        // Role Enum
        public enum UserRole
        {
            Staff = 1,
            Admin = 2,
            SuperAdmin = 3
        }

        // Data storage
        private List<UserData> allUsers = new List<UserData>();
        private List<UserData> displayedUsers = new List<UserData>();

        // Current logged-in user
        private UserData currentLoggedInUser;

        // Database connection string
        private string connectionString = "Server=localhost;Database=cms;Uid=root;Pwd=;";

        // UI Controls
        private Label lblTotalUsers;
        private Panel cardActiveAdmins;
        private Label lblActiveValue;
        private Label lblActiveAdmins;
        private PictureBox pictureBox1;
        private PictureBox picTotalIcon;
        private PictureBox pictureBox4;
        private Label lblInactiveValue;
        private Label lblInactiveAcc;
        private Panel pnlTableHeader;
        private Label lblUserList;
        private TextBox txtSearch;
        private PictureBox pictureBox2;
        private Panel cardOnline;
        private Label lblOnlineValue;
        private Label lblOnlineNow;
        private PictureBox pictureBox3;
        private Button btnManageUsers;
        private Button btnExport;
        private Panel cardInactive;
        private Panel datagridpanel;
        private DataGridView dgvUsers;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel cardTotalUsers;
        private Label lblTotalValue;
        private DataGridViewTextBoxColumn IDColumn;
        private DataGridViewTextBoxColumn fullnameColumn;
        private DataGridViewTextBoxColumn UsernameColumn;
        private DataGridViewTextBoxColumn RoleColumn;
        private DataGridViewComboBoxColumn statusColumn;
        private DataGridViewTextBoxColumn LastLoginColumn;
        private Label userheader;

        public UserManagementControl()
        {
            InitializeComponent();

            // Set current user as ADMIN
            currentLoggedInUser = new UserData
            {
                ID = "AD001",
                Username = "admin",
                FullName = "Administrator",
                Role = "ADMIN",
                Status = "ACTIVE",
                Password = "admin123"
            };

            // Set card titles
            if (lblTotalUsers != null) lblTotalUsers.Text = "Total Users";
            if (lblActiveAdmins != null) lblActiveAdmins.Text = "Active Admins";
            if (lblInactiveAcc != null) lblInactiveAcc.Text = "Inactive Accounts";
            if (lblOnlineNow != null) lblOnlineNow.Text = "Active Users";

            LoadSampleData();
            SetupEvents();
            RefreshUserList();
            UpdateStatistics();
            ApplyRolePermissions();
            HighlightCurrentUser();
        }

        // ==================== DATABASE METHODS ====================

        // Update the LoadUsersFromDatabase query
        private List<UserData> LoadUsersFromDatabase()
        {
            List<UserData> users = new List<UserData>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT ID, Username, password, full_name, ROLE, STATUS, last_login, custom_id 
                            FROM users 
                            ORDER BY FIELD(ROLE, 'SUPER ADMIN', 'ADMIN', 'STAFF'), custom_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserData
                            {
                                ID = reader["custom_id"]?.ToString() ?? reader["ID"]?.ToString() ?? "",
                                Username = reader["Username"]?.ToString() ?? "",
                                Password = reader["password"]?.ToString() ?? "",
                                FullName = reader["full_name"]?.ToString() ?? "",
                                Role = reader["ROLE"]?.ToString() ?? "STAFF",
                                Status = reader["STATUS"]?.ToString() ?? "ACTIVE",
                                LastLogin = reader["last_login"] != DBNull.Value ? Convert.ToDateTime(reader["last_login"]) : DateTime.Now
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}\n\nMake sure MySQL is running in XAMPP",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return users;
        }
        private void SaveUserToDatabase(UserData user, bool isNew)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (isNew)
                    {
                        string query = @"INSERT INTO users (ID, Username, password, full_name, ROLE, STATUS, last_login, custom_id) 
                                VALUES (@ID, @Username, @password, @full_name, @ROLE, @STATUS, @last_login, @custom_id)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", user.ID);
                            cmd.Parameters.AddWithValue("@Username", user.Username);
                            cmd.Parameters.AddWithValue("@password", user.Password);
                            cmd.Parameters.AddWithValue("@full_name", user.FullName);
                            cmd.Parameters.AddWithValue("@ROLE", user.Role);
                            cmd.Parameters.AddWithValue("@STATUS", user.Status);
                            cmd.Parameters.AddWithValue("@last_login", user.LastLogin);
                            cmd.Parameters.AddWithValue("@custom_id", user.ID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string query = @"UPDATE users 
                                SET full_name = @full_name, ROLE = @ROLE, STATUS = @STATUS
                                WHERE custom_id = @custom_id";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@full_name", user.FullName);
                            cmd.Parameters.AddWithValue("@ROLE", user.Role);
                            cmd.Parameters.AddWithValue("@STATUS", user.Status);
                            cmd.Parameters.AddWithValue("@custom_id", user.ID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteUserFromDatabase(string customId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM users WHERE custom_id = @custom_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@custom_id", customId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateNewID(string role)
        {
            string prefix = role == "SUPER ADMIN" ? "SA" : role == "ADMIN" ? "AD" : "ST";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"SELECT custom_id FROM users WHERE custom_id LIKE '{prefix}%' ORDER BY custom_id DESC LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                            return $"{prefix}001";

                        string lastID = result.ToString();
                        if (lastID.Length >= 3 && int.TryParse(lastID.Substring(2), out int number))
                        {
                            return $"{prefix}{(number + 1):D3}";
                        }
                        return $"{prefix}001";
                    }
                }
            }
            catch
            {
                return $"{prefix}001";
            }
        }

        // ==================== DATA METHODS ====================

        private void LoadSampleData()
        {
            // Load from database
            allUsers = LoadUsersFromDatabase();

            // If database is empty, create sample data
            if (allUsers.Count == 0)
            {
                // Super Admin
                allUsers.Add(new UserData
                {
                    ID = "SA001",
                    Username = "superadmin",
                    FullName = "Super Administrator",
                    Role = "SUPER ADMIN",
                    Status = "ACTIVE",
                    LastLogin = DateTime.Now,
                    Password = "super123"
                });

                // Admin
                allUsers.Add(new UserData
                {
                    ID = "AD001",
                    Username = "admin",
                    FullName = "System Administrator",
                    Role = "ADMIN",
                    Status = "ACTIVE",
                    LastLogin = DateTime.Now,
                    Password = "admin123"
                });

                // Cashier
                allUsers.Add(new UserData
                {
                    ID = "CA001",
                    Username = "cashier",
                    FullName = "Cashier User",
                    Role = "CASHIER",
                    Status = "ACTIVE",
                    LastLogin = DateTime.Now,
                    Password = "cashier123"
                });

                // Customer (Kiosk)
                allUsers.Add(new UserData
                {
                    ID = "CU001",
                    Username = "kiosk",
                    FullName = "Kiosk Customer",
                    Role = "CUSTOMER",
                    Status = "ACTIVE",
                    LastLogin = DateTime.Now,
                    Password = "kiosk123"
                });

                // Staff members
                for (int i = 1; i <= 15; i++)
                {
                    allUsers.Add(new UserData
                    {
                        ID = $"ST{i:D3}",
                        Username = $"staff{i:00}",
                        FullName = $"Staff Member {i}",
                        Role = "STAFF",
                        Status = i % 3 == 0 ? "INACTIVE" : "ACTIVE",
                        LastLogin = DateTime.Now.AddHours(-i),
                        Password = "staff123"
                    });
                }

                // Save all to database
                foreach (var user in allUsers)
                {
                    SaveUserToDatabase(user, true);
                }
            }

            allUsers = SortUsersByRole(allUsers);
        }

        private void SetupEvents()
        {
            // Search box
            if (txtSearch != null)
            {
                txtSearch.TextChanged += TxtSearch_TextChanged;
                txtSearch.Enter += TxtSearch_Enter;
                txtSearch.Leave += TxtSearch_Leave;

                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search keyword...";
                    txtSearch.ForeColor = Color.Gray;
                    txtSearch.Font = new System.Drawing.Font(txtSearch.Font, FontStyle.Italic);
                }
            }

            // DataGridView events
            if (dgvUsers != null)
            {
                dgvUsers.CellValueChanged += DgvUsers_CellValueChanged;
                dgvUsers.CurrentCellDirtyStateChanged += DgvUsers_CurrentCellDirtyStateChanged;
                dgvUsers.CellFormatting += DgvUsers_CellFormatting;
                dgvUsers.CellBeginEdit += DgvUsers_CellBeginEdit;
                dgvUsers.EditingControlShowing += DgvUsers_EditingControlShowing;
            }

            // Buttons
            if (btnExport != null)
                btnExport.Click += BtnExport_Click;

            if (btnManageUsers != null)
                btnManageUsers.Click += BtnManageUsers_Click;
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search keyword..." && txtSearch.ForeColor == Color.Gray)
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
                txtSearch.Font = new System.Drawing.Font(txtSearch.Font, FontStyle.Regular);
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search keyword...";
                txtSearch.ForeColor = Color.Gray;
                txtSearch.Font = new System.Drawing.Font(txtSearch.Font, FontStyle.Italic);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search keyword..." || txtSearch.ForeColor == Color.Gray)
            {
                displayedUsers = new List<UserData>(allUsers);
                RefreshUserList();
                return;
            }
            RefreshUserList();
        }

        private void ApplyRolePermissions()
        {
            UserRole currentRole = GetRoleFromString(currentLoggedInUser.Role);

            if (btnManageUsers != null)
            {
                btnManageUsers.Text = currentRole == UserRole.SuperAdmin ? "MANAGE USERS" : "MANAGE STAFF";
            }
        }

        private UserRole GetRoleFromString(string role)
        {
            switch (role.ToUpper())
            {
                case "SUPER ADMIN":
                    return UserRole.SuperAdmin;
                case "ADMIN":
                    return UserRole.Admin;
                default:
                    return UserRole.Staff;
            }
        }

        private bool CanModifyUser(UserData targetUser)
        {
            UserRole currentRole = GetRoleFromString(currentLoggedInUser.Role);
            UserRole targetRole = GetRoleFromString(targetUser.Role);

            if (currentRole == UserRole.SuperAdmin)
            {
                return true;
            }

            if (currentRole == UserRole.Admin && targetRole == UserRole.Staff)
            {
                return true;
            }

            return false;
        }

        private void RefreshUserList()
        {
            if (dgvUsers == null) return;

            dgvUsers.Rows.Clear();

            string searchText = txtSearch?.Text.Trim().ToLower() ?? "";
            bool isSearching = !string.IsNullOrWhiteSpace(searchText) && searchText != "search keyword...";

            var filtered = allUsers.AsEnumerable();

            if (isSearching)
            {
                filtered = filtered.Where(u =>
                    u.ID.ToLower().Contains(searchText) ||
                    u.Username.ToLower().Contains(searchText) ||
                    u.FullName.ToLower().Contains(searchText) ||
                    u.Role.ToLower().Contains(searchText) ||
                    u.Status.ToLower().Contains(searchText) ||
                    u.LastLogin.ToString("MMM dd, yyyy HH:mm").ToLower().Contains(searchText) ||
                    u.LastLogin.ToString("MMM dd, yyyy").ToLower().Contains(searchText) ||
                    u.LastLogin.ToString("HH:mm").ToLower().Contains(searchText)
                );
            }

            displayedUsers = SortUsersByRole(filtered.ToList());

            foreach (var user in displayedUsers)
            {
                int rowIndex = dgvUsers.Rows.Add(
                    user.ID,
                    user.FullName,
                    user.Username,
                    user.Role,
                    user.Status,
                    user.LastLogin.ToString("MMM dd, yyyy HH:mm")
                );
                dgvUsers.Rows[rowIndex].Tag = user;
            }

            if (lblUserList != null)
            {
                lblUserList.Text = $"User List ({displayedUsers.Count} of {allUsers.Count})";
            }

            UpdateStatistics();
            HighlightCurrentUser();
        }

        private void UpdateStatistics()
        {
            int totalUsers = displayedUsers.Count;
            int activeAdmins = displayedUsers.Count(u => u.Status == "ACTIVE" && u.Role == "ADMIN");
            int inactiveAccounts = displayedUsers.Count(u => u.Status == "INACTIVE");
            int activeUsers = displayedUsers.Count(u => u.Status == "ACTIVE");

            if (lblTotalValue != null) lblTotalValue.Text = $"{totalUsers} users";
            if (lblActiveValue != null) lblActiveValue.Text = $"{activeAdmins} admins";
            if (lblInactiveValue != null) lblInactiveValue.Text = $"{inactiveAccounts} accounts";
            if (lblOnlineValue != null) lblOnlineValue.Text = $"{activeUsers} users";
        }

        private void HighlightCurrentUser()
        {
            if (dgvUsers == null || currentLoggedInUser == null) return;

            for (int i = 0; i < dgvUsers.Rows.Count; i++)
            {
                var user = dgvUsers.Rows[i].Tag as UserData;
                if (user != null && user.ID == currentLoggedInUser.ID)
                {
                    dgvUsers.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 200);
                    dgvUsers.Rows[i].DefaultCellStyle.Font = new System.Drawing.Font(dgvUsers.Font, FontStyle.Bold);
                    dgvUsers.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromArgb(228, 186, 94);
                    break;
                }
            }
        }

        private void DgvUsers_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvUsers.Columns[e.ColumnIndex].Name == "statusColumn")
            {
                var user = dgvUsers.Rows[e.RowIndex].Tag as UserData;
                if (user != null)
                {
                    if (!CanModifyUser(user))
                    {
                        MessageBox.Show($"You cannot change the status of {user.Role} users!",
                            "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void DgvUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dgvUsers.Columns[e.ColumnIndex].Name;

                if (columnName == "statusColumn")
                {
                    var user = dgvUsers.Rows[e.RowIndex].Tag as UserData;
                    if (user != null)
                    {
                        string newStatus = dgvUsers.Rows[e.RowIndex].Cells["statusColumn"].Value?.ToString();
                        if (!string.IsNullOrEmpty(newStatus) && user.Status != newStatus)
                        {
                            user.Status = newStatus;
                            SaveUserToDatabase(user, false);
                            MessageBox.Show($"Status changed to {newStatus} for {user.Username}",
                                "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshUserList();
                        }
                    }
                }
            }
        }

        private void DgvUsers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvUsers.IsCurrentCellDirty)
            {
                dgvUsers.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvUsers.Rows[e.RowIndex];
                var user = row.Tag as UserData;

                if (user != null)
                {
                    bool isCurrentUser = (user.ID == currentLoggedInUser?.ID);

                    if (isCurrentUser)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 200);
                        row.DefaultCellStyle.Font = new System.Drawing.Font(dgvUsers.Font, FontStyle.Bold);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(228, 186, 94);
                    }
                    else
                    {
                        if (user.Role == "SUPER ADMIN")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(40, 41, 34);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(228, 186, 94);
                            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 61, 54);
                            row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(228, 186, 94);
                        }
                        else if (user.Role == "ADMIN")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
                            row.DefaultCellStyle.SelectionForeColor = Color.Black;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.White;
                            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
                            row.DefaultCellStyle.SelectionForeColor = Color.Black;
                        }
                    }
                }

                if (dgvUsers.Columns[e.ColumnIndex].Name == "statusColumn")
                {
                    var statusUser = dgvUsers.Rows[e.RowIndex].Tag as UserData;
                    if (statusUser != null)
                    {
                        if (statusUser.Status == "ACTIVE")
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new System.Drawing.Font(dgvUsers.Font, FontStyle.Bold);
                            e.CellStyle.SelectionForeColor = Color.Green;
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Red;
                            e.CellStyle.Font = new System.Drawing.Font(dgvUsers.Font, FontStyle.Bold);
                            e.CellStyle.SelectionForeColor = Color.Red;
                        }
                        e.CellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
                    }
                }
            }
        }

        private void DgvUsers_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvUsers.CurrentCell.ColumnIndex == dgvUsers.Columns["statusColumn"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                    comboBox.DrawItem += (s, ev) =>
                    {
                        if (ev.Index < 0) return;
                        string itemText = comboBox.Items[ev.Index].ToString();
                        ev.DrawBackground();
                        Brush textBrush = itemText == "ACTIVE" ? Brushes.Green : Brushes.Red;
                        if ((ev.State & DrawItemState.Selected) == DrawItemState.Selected)
                        {
                            ev.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(220, 220, 220)), ev.Bounds);
                        }
                        ev.Graphics.DrawString(itemText, comboBox.Font, textBrush, ev.Bounds.X, ev.Bounds.Y);
                        ev.DrawFocusRectangle();
                    };
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV Files (*.csv)|*.csv|Excel Files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 1;
                saveDialog.FileName = $"UserExport_{DateTime.Now:yyyyMMdd_HHmmss}";
                saveDialog.Title = "Export User Data";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (saveDialog.FileName.EndsWith(".csv"))
                        {
                            ExportToCSV(saveDialog.FileName);
                        }
                        else
                        {
                            ExportToExcel(saveDialog.FileName);
                        }
                        MessageBox.Show($"Export successful!\nSaved to: {saveDialog.FileName}",
                            "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportToCSV(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("\"ID\",\"FULL NAME\",\"USERNAME\",\"ROLE\",\"STATUS\",\"LAST LOGIN\"");
                foreach (var user in displayedUsers)
                {
                    writer.WriteLine($"\"{user.ID}\",\"{user.FullName}\",\"{user.Username}\"," +
                        $"\"{user.Role}\",\"{user.Status}\",\"{user.LastLogin:MMM dd, yyyy HH:mm}\"");
                }
            }
        }

        private void ExportToExcel(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<html><head><meta charset='UTF-8'></head><body>");
                writer.WriteLine("<h2>User Export - MatchPoint Gaming Hub</h2>");
                writer.WriteLine($"<p>Exported on: {DateTime.Now:MMMM dd, yyyy HH:mm}</p>");
                writer.WriteLine("<table border='1' cellpadding='5' cellspacing='0'>");
                writer.WriteLine("<tr style='background-color:#F8F9FA;'>");
                writer.WriteLine("<th>ID</th><th>FULL NAME</th><th>USERNAME</th><th>ROLE</th><th>STATUS</th><th>LAST LOGIN</th>");
                writer.WriteLine("</tr>");
                foreach (var user in displayedUsers)
                {
                    string rowColor = user.Role == "SUPER ADMIN" ? "#E4BA5E" : "white";
                    writer.WriteLine($"<tr style='background-color:{rowColor};'>");
                    writer.WriteLine($"<td>{user.ID}</td>");
                    writer.WriteLine($"<td>{user.FullName}</td>");
                    writer.WriteLine($"<td>{user.Username}</td>");
                    writer.WriteLine($"<td>{user.Role}</td>");
                    writer.WriteLine($"<td style='color:{(user.Status == "ACTIVE" ? "green" : "red")};font-weight:bold;'>{user.Status}</td>");
                    writer.WriteLine($"<td>{user.LastLogin:MMM dd, yyyy HH:mm}</td>");
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine($"<p>Total Users: {displayedUsers.Count}</p>");
                writer.WriteLine("</body></html>");
            }
        }

        private List<UserData> SortUsersByRole(List<UserData> users)
        {
            return users.OrderBy(u =>
                u.Role == "SUPER ADMIN" ? 1 :
                u.Role == "ADMIN" ? 2 :
                3
            ).ThenBy(u => u.ID).ToList();
        }

        public UserData GetCurrentLoggedInUser()
        {
            return currentLoggedInUser;
        }

        private void BtnManageUsers_Click(object sender, EventArgs e)
        {
            using (frmManageUsers manageForm = new frmManageUsers(this))
            {
                manageForm.ShowDialog();
            }
            RefreshUserList();
            UpdateStatistics();
        }

        public void RefreshData()
        {
            RefreshUserList();
            UpdateStatistics();
        }

        public List<UserData> GetAllUsers()
        {
            return allUsers;
        }

        public void AddUser(UserData newUser)
        {
            if (string.IsNullOrEmpty(newUser.ID))
            {
                newUser.ID = GenerateNewID(newUser.Role);
            }
            SaveUserToDatabase(newUser, true);
            allUsers = LoadUsersFromDatabase();
            allUsers = SortUsersByRole(allUsers);
            RefreshUserList();
            UpdateStatistics();
            HighlightNewUser(newUser.ID);
        }

        private void HighlightNewUser(string userId)
        {
            if (dgvUsers == null) return;
            for (int i = 0; i < dgvUsers.Rows.Count; i++)
            {
                var user = dgvUsers.Rows[i].Tag as UserData;
                if (user != null && user.ID == userId)
                {
                    dgvUsers.ClearSelection();
                    dgvUsers.Rows[i].Selected = true;
                    dgvUsers.FirstDisplayedScrollingRowIndex = i;
                    FlashRow(i);
                    break;
                }
            }
        }

        private System.Windows.Forms.Timer flashTimer;
        private int flashRowIndex;
        private int flashCount;

        private void FlashRow(int rowIndex)
        {
            flashRowIndex = rowIndex;
            flashCount = 0;
            if (flashTimer == null)
            {
                flashTimer = new System.Windows.Forms.Timer();
                flashTimer.Interval = 200;
                flashTimer.Tick += FlashTimer_Tick;
            }
            flashTimer.Start();
        }

        private void FlashTimer_Tick(object sender, EventArgs e)
        {
            if (flashRowIndex >= 0 && flashRowIndex < dgvUsers.Rows.Count)
            {
                var row = dgvUsers.Rows[flashRowIndex];
                var user = row.Tag as UserData;
                if (flashCount % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 200);
                }
                else
                {
                    if (user?.Role == "SUPER ADMIN")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(40, 41, 34);
                    }
                    else if (user?.Role == "ADMIN")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
                flashCount++;
                if (flashCount >= 10)
                {
                    flashTimer.Stop();
                    if (user?.Role == "SUPER ADMIN")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(40, 41, 34);
                    }
                    else if (user?.Role == "ADMIN")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            else
            {
                flashTimer.Stop();
            }
        }

        public void UpdateUser(UserData updatedUser)
        {
            SaveUserToDatabase(updatedUser, false);
            allUsers = LoadUsersFromDatabase();
            allUsers = SortUsersByRole(allUsers);
            RefreshUserList();
            UpdateStatistics();
        }

        public void DeleteUser(string userId)
        {
            DeleteUserFromDatabase(userId);
            allUsers = LoadUsersFromDatabase();
            allUsers = SortUsersByRole(allUsers);
            RefreshUserList();
            UpdateStatistics();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            TxtSearch_TextChanged(sender, e);
        }

        private void UserManagementControl_Load(object sender, EventArgs e)
        {
        }

       
private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagementControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTotalUsers = new System.Windows.Forms.Label();
            this.cardActiveAdmins = new System.Windows.Forms.Panel();
            this.lblActiveValue = new System.Windows.Forms.Label();
            this.lblActiveAdmins = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picTotalIcon = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.lblInactiveValue = new System.Windows.Forms.Label();
            this.lblInactiveAcc = new System.Windows.Forms.Label();
            this.pnlTableHeader = new System.Windows.Forms.Panel();
            this.lblUserList = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.cardOnline = new System.Windows.Forms.Panel();
            this.lblOnlineValue = new System.Windows.Forms.Label();
            this.lblOnlineNow = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.cardInactive = new System.Windows.Forms.Panel();
            this.datagridpanel = new System.Windows.Forms.Panel();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cardTotalUsers = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.userheader = new System.Windows.Forms.Label();
            this.IDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullnameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsernameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LastLoginColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardActiveAdmins.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTotalIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.pnlTableHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.cardOnline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.cardInactive.SuspendLayout();
            this.datagridpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.cardTotalUsers.SuspendLayout();
            this.SuspendLayout();
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
            // cardActiveAdmins
            // 
            this.cardActiveAdmins.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
            // picTotalIcon
            // 
            this.picTotalIcon.Image = ((System.Drawing.Image)(resources.GetObject("picTotalIcon.Image")));
            this.picTotalIcon.Location = new System.Drawing.Point(8, 3);
            this.picTotalIcon.Name = "picTotalIcon";
            this.picTotalIcon.Size = new System.Drawing.Size(63, 66);
            this.picTotalIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTotalIcon.TabIndex = 0;
            this.picTotalIcon.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(962, 11);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(29, 28);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 19;
            this.pictureBox4.TabStop = false;
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
            // pnlTableHeader
            // 
            this.pnlTableHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlTableHeader.Controls.Add(this.lblUserList);
            this.pnlTableHeader.Controls.Add(this.txtSearch);
            this.pnlTableHeader.Location = new System.Drawing.Point(13, 161);
            this.pnlTableHeader.Name = "pnlTableHeader";
            this.pnlTableHeader.Size = new System.Drawing.Size(1206, 39);
            this.pnlTableHeader.TabIndex = 21;
            // 
            // lblUserList
            // 
            this.lblUserList.AutoSize = true;
            this.lblUserList.Font = new System.Drawing.Font("Segoe UI Black", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserList.ForeColor = System.Drawing.Color.DimGray;
            this.lblUserList.Location = new System.Drawing.Point(3, 5);
            this.lblUserList.Name = "lblUserList";
            this.lblUserList.Size = new System.Drawing.Size(140, 31);
            this.lblUserList.TabIndex = 5;
            this.lblUserList.Text = "USER LISTS";
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtSearch.Location = new System.Drawing.Point(939, 7);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 29);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged_1);
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
            this.cardOnline.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
            // btnManageUsers
            // 
            this.btnManageUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnManageUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageUsers.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageUsers.ForeColor = System.Drawing.Color.White;
            this.btnManageUsers.Location = new System.Drawing.Point(1069, 4);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(150, 42);
            this.btnManageUsers.TabIndex = 20;
            this.btnManageUsers.Text = "Manage Users";
            this.btnManageUsers.UseVisualStyleBackColor = false;
            this.btnManageUsers.Click += new System.EventHandler(this.BtnManageUsers_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.SystemColors.Control;
            this.btnExport.Location = new System.Drawing.Point(952, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(111, 43);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // cardInactive
            // 
            this.cardInactive.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cardInactive.Controls.Add(this.lblInactiveValue);
            this.cardInactive.Controls.Add(this.lblInactiveAcc);
            this.cardInactive.Controls.Add(this.pictureBox2);
            this.cardInactive.Location = new System.Drawing.Point(505, 13);
            this.cardInactive.Name = "cardInactive";
            this.cardInactive.Size = new System.Drawing.Size(235, 74);
            this.cardInactive.TabIndex = 4;
            // 
            // datagridpanel
            // 
            this.datagridpanel.BackColor = System.Drawing.Color.Transparent;
            this.datagridpanel.Controls.Add(this.dgvUsers);
            this.datagridpanel.Location = new System.Drawing.Point(13, 209);
            this.datagridpanel.Name = "datagridpanel";
            this.datagridpanel.Size = new System.Drawing.Size(1206, 516);
            this.datagridpanel.TabIndex = 22;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvUsers.ColumnHeadersHeight = 40;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDColumn,
            this.fullnameColumn,
            this.UsernameColumn,
            this.RoleColumn,
            this.statusColumn,
            this.LastLoginColumn});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgvUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvUsers.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvUsers.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.dgvUsers.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.dgvUsers.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.dgvUsers.RowTemplate.Height = 25;
            this.dgvUsers.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(1206, 516);
            this.dgvUsers.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.cardTotalUsers);
            this.flowLayoutPanel1.Controls.Add(this.cardActiveAdmins);
            this.flowLayoutPanel1.Controls.Add(this.cardInactive);
            this.flowLayoutPanel1.Controls.Add(this.cardOnline);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 53);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1206, 100);
            this.flowLayoutPanel1.TabIndex = 17;
            // 
            // cardTotalUsers
            // 
            this.cardTotalUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
            // userheader
            // 
            this.userheader.AutoSize = true;
            this.userheader.Font = new System.Drawing.Font("Segoe UI Black", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userheader.Location = new System.Drawing.Point(6, 12);
            this.userheader.Name = "userheader";
            this.userheader.Size = new System.Drawing.Size(308, 38);
            this.userheader.TabIndex = 16;
            this.userheader.Text = "USER MANAGEMENT";
            // 
            // IDColumn
            // 
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            this.IDColumn.DefaultCellStyle = dataGridViewCellStyle11;
            this.IDColumn.HeaderText = "#ID";
            this.IDColumn.MinimumWidth = 6;
            this.IDColumn.Name = "IDColumn";
            this.IDColumn.ReadOnly = true;
            this.IDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // fullnameColumn
            // 
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fullnameColumn.DefaultCellStyle = dataGridViewCellStyle12;
            this.fullnameColumn.HeaderText = "FULL NAME";
            this.fullnameColumn.MinimumWidth = 6;
            this.fullnameColumn.Name = "fullnameColumn";
            this.fullnameColumn.ReadOnly = true;
            this.fullnameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UsernameColumn
            // 
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameColumn.DefaultCellStyle = dataGridViewCellStyle13;
            this.UsernameColumn.HeaderText = "USERNAME";
            this.UsernameColumn.MinimumWidth = 6;
            this.UsernameColumn.Name = "UsernameColumn";
            this.UsernameColumn.ReadOnly = true;
            this.UsernameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RoleColumn
            // 
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RoleColumn.DefaultCellStyle = dataGridViewCellStyle14;
            this.RoleColumn.HeaderText = "ROLE";
            this.RoleColumn.MinimumWidth = 6;
            this.RoleColumn.Name = "RoleColumn";
            this.RoleColumn.ReadOnly = true;
            this.RoleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // statusColumn
            // 
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(200)))));
            this.statusColumn.DefaultCellStyle = dataGridViewCellStyle15;
            this.statusColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.statusColumn.HeaderText = "STATUS";
            this.statusColumn.Items.AddRange(new object[] {
            "ACTIVE",
            "INACTIVE"});
            this.statusColumn.MinimumWidth = 6;
            this.statusColumn.Name = "statusColumn";
            this.statusColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // LastLoginColumn
            // 
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.Format = "g";
            dataGridViewCellStyle16.NullValue = null;
            this.LastLoginColumn.DefaultCellStyle = dataGridViewCellStyle16;
            this.LastLoginColumn.HeaderText = "LAST LOG-IN";
            this.LastLoginColumn.MinimumWidth = 6;
            this.LastLoginColumn.Name = "LastLoginColumn";
            this.LastLoginColumn.ReadOnly = true;
            this.LastLoginColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UserManagementControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pnlTableHeader);
            this.Controls.Add(this.btnManageUsers);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.datagridpanel);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.userheader);
            this.Name = "UserManagementControl";
            this.Size = new System.Drawing.Size(1257, 745);
            this.Load += new System.EventHandler(this.UserManagementControl_Load);
            this.cardActiveAdmins.ResumeLayout(false);
            this.cardActiveAdmins.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTotalIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.pnlTableHeader.ResumeLayout(false);
            this.pnlTableHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.cardOnline.ResumeLayout(false);
            this.cardOnline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.cardInactive.ResumeLayout(false);
            this.cardInactive.PerformLayout();
            this.datagridpanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.cardTotalUsers.ResumeLayout(false);
            this.cardTotalUsers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }



    }
}