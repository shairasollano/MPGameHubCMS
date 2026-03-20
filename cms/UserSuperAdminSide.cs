using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cms
{
    public partial class UserSuperAdminSide : UserControl
    {
        // User data class
        public class UserData
        {
            public string ID { get; set; }
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Role { get; set; }
            public string Status { get; set; }
            public DateTime LastLogin { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // Store all users
        private List<UserData> allUsers = new List<UserData>();
        private List<UserData> displayedUsers = new List<UserData>();

        public UserSuperAdminSide()
        {
            InitializeComponent();


            // Setup DataGridView
            SetupDataGridView();

            // Load sample data
            LoadSampleData();

            // Setup events
            SetupEvents();

            // Display data
            RefreshUserList();
            UpdateStatistics();
        }

        private void SetupDataGridView()
        {
            // Basic properties
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeRows = false;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.ReadOnly = true;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.GridColor = Color.FromArgb(233, 236, 239);

            // Column headers style
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(73, 80, 87);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            dgvUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvUsers.ColumnHeadersHeight = 40;

            // Row style
            dgvUsers.RowTemplate.Height = 45;
            dgvUsers.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9);
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(228, 186, 94);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(26, 26, 26);

            // Add columns if not already in designer
            if (dgvUsers.Columns.Count == 0)
            {
                // ID Column
                DataGridViewTextBoxColumn idCol = new DataGridViewTextBoxColumn
                {
                    Name = "IDColumn",
                    HeaderText = "ID",
                    Width = 80,
                    ReadOnly = true
                };
                dgvUsers.Columns.Add(idCol);

                // Username Column
                DataGridViewTextBoxColumn usernameCol = new DataGridViewTextBoxColumn
                {
                    Name = "UsernameColumn",
                    HeaderText = "USERNAME",
                    Width = 130,
                    ReadOnly = true
                };
                dgvUsers.Columns.Add(usernameCol);

                // Full Name Column
                DataGridViewTextBoxColumn fullNameCol = new DataGridViewTextBoxColumn
                {
                    Name = "FullNameColumn",
                    HeaderText = "FULL NAME",
                    Width = 160,
                    ReadOnly = true
                };
                dgvUsers.Columns.Add(fullNameCol);

                // Role Column
                DataGridViewTextBoxColumn roleCol = new DataGridViewTextBoxColumn
                {
                    Name = "RoleColumn",
                    HeaderText = "ROLE",
                    Width = 110,
                    ReadOnly = true
                };
                dgvUsers.Columns.Add(roleCol);

                // Status Column (ComboBox for editing)
                DataGridViewComboBoxColumn statusCol = new DataGridViewComboBoxColumn
                {
                    Name = "StatusColumn",
                    HeaderText = "STATUS",
                    Width = 100
                };
                statusCol.Items.AddRange(new object[] { "ACTIVE", "INACTIVE" });
                dgvUsers.Columns.Add(statusCol);

                // Last Login Column
                DataGridViewTextBoxColumn lastLoginCol = new DataGridViewTextBoxColumn
                {
                    Name = "LastLoginColumn",
                    HeaderText = "LAST LOGIN",
                    Width = 140,
                    ReadOnly = true
                };
                dgvUsers.Columns.Add(lastLoginCol);
            }
        }

        private void LoadSampleData()
        {
            allUsers.Clear();

            // Super Admin
            allUsers.Add(new UserData
            {
                ID = "SA001",
                Username = "jessica_p",
                FullName = "Jessica Parker",
                Role = "SUPER ADMIN",
                Status = "ACTIVE",
                LastLogin = DateTime.Now.AddHours(-2),
                CreatedDate = DateTime.Now.AddDays(-30),
                Email = "jessica@matchpoint.com"
            });

            // Admins
            allUsers.Add(new UserData
            {
                ID = "AD001",
                Username = "mike_t",
                FullName = "Mike Thompson",
                Role = "ADMIN",
                Status = "ACTIVE",
                LastLogin = DateTime.Now.AddHours(-5),
                CreatedDate = DateTime.Now.AddDays(-25),
                Email = "mike@matchpoint.com"
            });

            allUsers.Add(new UserData
            {
                ID = "AD002",
                Username = "lisa_w",
                FullName = "Lisa Wong",
                Role = "ADMIN",
                Status = "ACTIVE",
                LastLogin = DateTime.Now.AddHours(-1),
                CreatedDate = DateTime.Now.AddDays(-20),
                Email = "lisa@matchpoint.com"
            });

            // Staff (15 staff members)
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
                    CreatedDate = DateTime.Now.AddDays(-i),
                    Email = $"staff{i}@matchpoint.com"
                });
            }
        }

        private void SetupEvents()
        {
            // Search box
            txtSearchBox.TextChanged += TxtSearchBox_TextChanged;

            // DataGridView cell value changed (for status updates)
            dgvUsers.CellValueChanged += DgvUsers_CellValueChanged;
            dgvUsers.CurrentCellDirtyStateChanged += DgvUsers_CurrentCellDirtyStateChanged;
            dgvUsers.CellFormatting += DgvUsers_CellFormatting;
        }

        private void RefreshUserList()
        {
            dgvUsers.Rows.Clear();

            // Apply search filter
            string searchText = txtSearchBox.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(searchText) || searchText == "search keywords...")
            {
                displayedUsers = new List<UserData>(allUsers);
            }
            else
            {
                displayedUsers = allUsers.Where(u =>
                    u.Username.ToLower().Contains(searchText) ||
                    u.FullName.ToLower().Contains(searchText) ||
                    u.ID.ToLower().Contains(searchText) ||
                    u.Role.ToLower().Contains(searchText)
                ).ToList();
            }

            // Add rows to DataGridView
            foreach (var user in displayedUsers)
            {
                int rowIndex = dgvUsers.Rows.Add(
                    user.ID,
                    user.Username,
                    user.FullName,
                    user.Role,
                    user.Status,
                    user.LastLogin.ToString("MMM dd, yyyy HH:mm")
                );

                // Store user object in row tag
                dgvUsers.Rows[rowIndex].Tag = user;
            }

            // Update user list count
            UpdateUserListCount();
        }

        private void UpdateUserListCount()
        {
            int totalUsers = allUsers.Count;
            int visibleUsers = displayedUsers.Count;
            lblUserList.Text = $"User List ({visibleUsers} of {totalUsers})";
        }

        private void UpdateStatistics()
        {
            int totalUsers = allUsers.Count;
            int activeAdmins = allUsers.Count(u => u.Status == "ACTIVE" &&
                (u.Role == "ADMIN" || u.Role == "SUPER ADMIN"));
            int inactiveAccounts = allUsers.Count(u => u.Status == "INACTIVE");
            int onlineNow = allUsers.Count(u => u.Status == "ACTIVE" &&
                u.LastLogin.Date == DateTime.Now.Date);

            lblTotalUsers.Text = $"{totalUsers} users";
            lblActiveAdmins.Text = $"{activeAdmins} users";
            lblInactiveAcc.Text = $"{inactiveAccounts} users";
            lblOnlineNow.Text = $"{onlineNow} users";
        }

        // Event Handlers
        private void TxtSearchBox_TextChanged(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        private void DgvUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // When status is changed in the grid
            if (e.RowIndex >= 0 && dgvUsers.Columns[e.ColumnIndex].Name == "StatusColumn")
            {
                var user = dgvUsers.Rows[e.RowIndex].Tag as UserData;
                if (user != null)
                {
                    string newStatus = dgvUsers.Rows[e.RowIndex].Cells["StatusColumn"].Value?.ToString();
                    user.Status = newStatus;

                    // Update statistics
                    UpdateStatistics();
                }
            }
        }

        private void DgvUsers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Commit changes immediately when using ComboBox
            if (dgvUsers.IsCurrentCellDirty)
            {
                dgvUsers.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Style rows based on role and status
            if (e.RowIndex >= 0)
            {
                var row = dgvUsers.Rows[e.RowIndex];
                var user = row.Tag as UserData;

                if (user != null)
                {
                    // Role-based styling
                    if (user.Role == "SUPER ADMIN")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(40, 41, 34);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(228, 186, 94);
                    }
                    else if (user.Role == "ADMIN")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                    }

                    // Status-based styling for Status column
                    if (dgvUsers.Columns[e.ColumnIndex].Name == "StatusColumn")
                    {
                        if (user.Status == "ACTIVE")
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold);
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        // Public methods for other forms to use
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
            allUsers.Add(newUser);
            RefreshUserList();
            UpdateStatistics();
        }

        public void UpdateUser(UserData updatedUser)
        {
            var index = allUsers.FindIndex(u => u.ID == updatedUser.ID);
            if (index >= 0)
            {
                allUsers[index] = updatedUser;
                RefreshUserList();
                UpdateStatistics();
            }
        }

        public void DeleteUser(string userId)
        {
            var user = allUsers.FirstOrDefault(u => u.ID == userId);
            if (user != null)
            {
                allUsers.Remove(user);
                RefreshUserList();
                UpdateStatistics();
            }
        }
    }
}