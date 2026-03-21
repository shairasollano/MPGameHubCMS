using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cms
{
    public partial class lastsuper : UserControl
    {
        // User Data Class
        public class UserData
        {
            public string ID { get; set; }
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Role { get; set; }
            public string Status { get; set; }
            public DateTime LastLogin { get; set; }
            public string Email { get; set; }
        }

        // Data storage
        private List<UserData> allUsers = new List<UserData>();
        private List<UserData> displayedUsers = new List<UserData>();

        public lastsuper()
        {
            InitializeComponent();
            LoadSampleData();
            SetupEvents();
            RefreshUserList();
            UpdateStatistics();
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
                Email = "lisa@matchpoint.com"
            });

            // Staff (15 members)
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
                    Email = $"staff{i}@matchpoint.com"
                });
            }
        }

        private void SetupEvents()
        {
            // Search box - assuming you have txtSearch in your designer
            if (txtSearch != null)
                txtSearch.TextChanged += TxtSearch_TextChanged;

            // DataGridView events
            if (dgvUsers != null)
            {
                dgvUsers.CellValueChanged += DgvUsers_CellValueChanged;
                dgvUsers.CurrentCellDirtyStateChanged += DgvUsers_CurrentCellDirtyStateChanged;
                dgvUsers.CellFormatting += DgvUsers_CellFormatting;
            }

            // Buttons - assuming you have these in your designer
            if (btnExport != null)
                btnExport.Click += BtnExport_Click;

            if (btnManageUsers != null)
                btnManageUsers.Click += BtnManageUsers_Click;
        }

        private void RefreshUserList()
        {
            if (dgvUsers == null) return;

            dgvUsers.Rows.Clear();

            // Apply search filter
            string searchText = txtSearch?.Text.Trim().ToLower() ?? "";
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

            // Update user list count if you have a label for it
            // if (lblUserList != null)
            // {
            //     lblUserList.Text = $"User List ({displayedUsers.Count} of {allUsers.Count})";
            // }
        }

        private void UpdateStatistics()
        {
            int totalUsers = allUsers.Count;
            int activeAdmins = allUsers.Count(u => u.Status == "ACTIVE" &&
                (u.Role == "ADMIN" || u.Role == "SUPER ADMIN"));
            int inactiveAccounts = allUsers.Count(u => u.Status == "INACTIVE");
            int onlineNow = allUsers.Count(u => u.Status == "ACTIVE" &&
                u.LastLogin.Date == DateTime.Now.Date);

            // Update the VALUE labels (not the title labels)
            if (lblTotalValue != null) lblTotalValue.Text = $"{totalUsers} users";
            if (lblActiveValue != null) lblActiveValue.Text = $"{activeAdmins} users";
            if (lblInactiveValue != null) lblInactiveValue.Text = $"{inactiveAccounts} users";
            if (lblOnlineValue != null) lblOnlineValue.Text = $"{onlineNow} users";
        }

        // Event Handlers
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        private void DgvUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dgvUsers.Columns[e.ColumnIndex].Name;

                if (columnName == "StatusColumn")
                {
                    var user = dgvUsers.Rows[e.RowIndex].Tag as UserData;
                    if (user != null)
                    {
                        string newStatus = dgvUsers.Rows[e.RowIndex].Cells["StatusColumn"].Value?.ToString();
                        user.Status = newStatus;
                        UpdateStatistics();
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

                    // Status column styling
                    if (dgvUsers.Columns[e.ColumnIndex].Name == "StatusColumn")
                    {
                        if (user.Status == "ACTIVE")
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new System.Drawing.Font(dgvUsers.Font, FontStyle.Bold);
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // TODO: Add export functionality
            MessageBox.Show("Export feature coming soon!", "Export",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnManageUsers_Click(object sender, EventArgs e)
        {
            // TODO: Open Manage Users form
            MessageBox.Show("Manage Users feature coming soon!", "Manage Users",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Public methods for other forms
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

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Keep this empty or add your logic
        }
    }
}