using finaluserandstaff;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace cms.lastsuper
{
    public partial class frmManageUsers : Form
    {
        private UserManagementControl parentControl;
        private UserManagementControl.UserData selectedUser;
        private UserManagementControl.UserData currentAdmin;
        private string[] assignableRoles;

        public frmManageUsers(UserManagementControl control)
        {
            parentControl = control;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Manage Staff - MatchPoint Gaming Hub";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Get current logged-in admin
            currentAdmin = parentControl.GetCurrentLoggedInUser();

            // Get assignable roles based on current user's role
            assignableRoles = parentControl.GetAssignableRolesForCurrentUser().ToArray();

            // Change button text based on role
            btnAddUser.Text = "+ ADD USER";
            btnEditUser.Text = "✏️ EDIT USER";
            btnDeactivateUser.Text = "🚫 DEACTIVATE";
            btnReactivateUser.Text = "🔄 REACTIVATE";

            // Load users (only users with lower role than current user)
            LoadUsersIntoComboBox();

            // Wire up button events
            btnAddUser.Click += (s, e) => ShowAddUserDialog();
            btnEditUser.Click += (s, e) => ShowEditUserDialog();
            btnDeactivateUser.Click += (s, e) => DeactivateUser();
            btnReactivateUser.Click += (s, e) => ReactivateUser();

            // Gray out buttons based on selected user status
            UpdateButtonStates();
        }

        private void LoadUsersIntoComboBox()
        {
            if (parentControl != null)
            {
                // Get current user's role level
                int currentLevel = GetRoleLevel(currentAdmin.Role);

                // Only show users with roles lower than current user
                var users = parentControl.GetAllUsers()
                    .Where(u => GetRoleLevel(u.Role) > currentLevel) // Users with lower role (higher number)
                    .OrderBy(u => GetRoleLevel(u.Role))
                    .ThenBy(u => u.UserId)
                    .ToList();

                cmbUsers.Items.Clear();
                foreach (var user in users)
                {
                    string roleIcon = GetRoleIcon(user.Role);
                    cmbUsers.Items.Add($"{roleIcon} {user.UserId} - {user.FullName} ({user.Role})");
                }
                if (cmbUsers.Items.Count > 0)
                {
                    cmbUsers.SelectedIndex = 0;
                }
                else
                {
                    lblUserInfo.Text = "𝐔𝐒𝐄𝐑 𝐈𝐍𝐅𝐎\n\nNo users found with lower role than yours";
                }
            }
        }

        private int GetRoleLevel(string role)
        {
            switch (role.ToUpper())
            {
                case "SUPER ADMIN": return 1;
                case "ADMIN": return 2;
                case "MANAGER": return 3;
                case "STAFF": return 4;
                case "CASHIER": return 5;
                case "CUSTOMER": return 6;
                default: return 99;
            }
        }

        private string GetRoleIcon(string role)
        {
            switch (role.ToUpper())
            {
                case "SUPER ADMIN": return "👑";
                case "ADMIN": return "⚙️";
                case "MANAGER": return "📋";
                case "STAFF": return "👤";
                case "CASHIER": return "💰";
                case "CUSTOMER": return "👥";
                default: return "❓";
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUsers.SelectedIndex >= 0 && parentControl != null)
            {
                // Get users with lower role than current user
                int currentLevel = GetRoleLevel(currentAdmin.Role);
                var manageableUsers = parentControl.GetAllUsers()
                    .Where(u => GetRoleLevel(u.Role) > currentLevel)
                    .OrderBy(u => GetRoleLevel(u.Role))
                    .ThenBy(u => u.UserId)
                    .ToList();

                if (cmbUsers.SelectedIndex < manageableUsers.Count)
                {
                    selectedUser = manageableUsers[cmbUsers.SelectedIndex];

                    // Style the label
                    lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Regular);
                    lblUserInfo.BackColor = Color.White;
                    lblUserInfo.BorderStyle = BorderStyle.None;
                    lblUserInfo.Padding = new Padding(5);

                    string statusDisplay = selectedUser.Status == "ACTIVE" ? "🟢 ACTIVE" : "🔴 INACTIVE";
                    string roleIcon = GetRoleIcon(selectedUser.Role);

                    // Using Unicode bold characters for labels
                    lblUserInfo.Text =
                        $"𝐔𝐒𝐄𝐑 𝐈𝐍𝐅𝐎\n\n" +
                        $"𝐈𝐃          : {selectedUser.UserId}\n" +
                        $"𝐔𝐬𝐞𝐫𝐧𝐚𝐦𝐞    : {selectedUser.Username}\n" +
                        $"𝐅𝐮𝐥𝐥 𝐍𝐚𝐦𝐞 : {selectedUser.FullName}\n" +
                        $"𝐑𝐨𝐥𝐞       : {roleIcon} {selectedUser.Role}\n" +
                        $"𝐒𝐭𝐚𝐭𝐮𝐬     : {statusDisplay}\n" +
                        $"𝐂𝐫𝐞𝐚𝐭𝐞𝐝    : {selectedUser.CreatedDate:MMM dd, yyyy HH:mm}";

                    // Update button states based on selected user
                    UpdateButtonStates();
                }
            }
            else
            {
                selectedUser = null;
                lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 11);
                lblUserInfo.BackColor = Color.White;
                lblUserInfo.BorderStyle = BorderStyle.None;
                lblUserInfo.Text = "𝐔𝐒𝐄𝐑 𝐈𝐍𝐅𝐎\n\nSelect a user from the dropdown above";
            }
        }

        private void UpdateButtonStates()
        {
            if (selectedUser != null)
            {
                // Gray out Deactivate if user is INACTIVE
                btnDeactivateUser.Enabled = selectedUser.Status == "ACTIVE";
                btnDeactivateUser.BackColor = selectedUser.Status == "ACTIVE" ? Color.FromArgb(220, 53, 69) : Color.Gray;

                // Gray out Reactivate if user is ACTIVE
                btnReactivateUser.Enabled = selectedUser.Status == "INACTIVE";
                btnReactivateUser.BackColor = selectedUser.Status == "INACTIVE" ? Color.FromArgb(255, 193, 7) : Color.Gray;
            }
        }

        private void ShowAddUserDialog()
        {
            // Check if user can add users (has assignable roles)
            if (assignableRoles.Length == 0)
            {
                MessageBox.Show("You don't have permission to add new users. Your role doesn't allow creating users with lower roles.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to add new users.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form addForm = new Form();
            addForm.Text = "Add New User";
            addForm.Size = new Size(600, 650);
            addForm.StartPosition = FormStartPosition.CenterParent;
            addForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            addForm.BackColor = Color.White;

            // Username
            Label lblUsername = new Label { Text = "Username:", Location = new Point(30, 30), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtUsername = new TextBox { Location = new Point(150, 27), Size = new Size(350, 27), Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtUsername, 25);

            // Last Name
            Label lblLastName = new Label { Text = "Last Name:", Location = new Point(30, 70), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtLastName = new TextBox { Location = new Point(150, 67), Size = new Size(350, 27), Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtLastName, 25);

            // First Name
            Label lblFirstName = new Label { Text = "First Name:", Location = new Point(30, 110), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtFirstName = new TextBox { Location = new Point(150, 107), Size = new Size(350, 27), Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtFirstName, 25);

            // Middle Initial
            Label lblMiddleInitial = new Label { Text = "M.I.:", Location = new Point(30, 150), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtMiddleInitial = new TextBox { Location = new Point(150, 147), Size = new Size(60, 27), Font = new System.Drawing.Font("Segoe UI", 10), MaxLength = 1 };

            // Role Selection - Only show roles lower than current user
            Label lblRole = new Label { Text = "Role:", Location = new Point(30, 190), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            ComboBox cmbRole = new ComboBox
            {
                Location = new Point(150, 187),
                Size = new Size(200, 27),
                Font = new System.Drawing.Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Add assignable roles with icons
            foreach (var role in assignableRoles)
            {
                string roleIcon = GetRoleIcon(role);
                cmbRole.Items.Add($"{roleIcon} {role}");
            }
            if (cmbRole.Items.Count > 0) cmbRole.SelectedIndex = 0;

            // Note about password
            Label lblPasswordNote = new Label
            {
                Text = "⚠️ Default password will be set to: MatchPoint123!\n   User must change password on first login.",
                Location = new Point(30, 240),
                Size = new Size(470, 45),
                Font = new System.Drawing.Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Buttons
            Button btnSave = new Button
            {
                Text = "CREATE USER",
                BackColor = Color.FromArgb(46, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, 320),
                Size = new Size(140, 40),
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(310, 320),
                Size = new Size(140, 40),
                Font = new System.Drawing.Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, ev) => addForm.Close();

            // Real-time validation for name fields
            void AddNameValidation(TextBox textBox, Label label)
            {
                textBox.TextChanged += (s, ev) =>
                {
                    if (!string.IsNullOrEmpty(textBox.Text) && !IsValidName(textBox.Text))
                    {
                        textBox.BackColor = Color.LightPink;
                        label.ForeColor = Color.Red;
                    }
                    else
                    {
                        textBox.BackColor = Color.White;
                        label.ForeColor = Color.Black;
                    }
                };
            }

            AddNameValidation(txtLastName, lblLastName);
            AddNameValidation(txtFirstName, lblFirstName);
            AddNameValidation(txtMiddleInitial, lblMiddleInitial);

            btnSave.Click += (s, ev) =>
            {
                // Validate Username
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (txtUsername.Text.Length > 25)
                {
                    MessageBox.Show("Username cannot exceed 25 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                // Validate Last Name
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    MessageBox.Show("Last Name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLastName.Focus();
                    return;
                }

                if (!IsValidName(txtLastName.Text))
                {
                    MessageBox.Show("Last Name can only contain letters, spaces, hyphens, and apostrophes.",
                        "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLastName.Focus();
                    return;
                }

                // Validate First Name
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("First Name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFirstName.Focus();
                    return;
                }

                if (!IsValidName(txtFirstName.Text))
                {
                    MessageBox.Show("First Name can only contain letters, spaces, hyphens, and apostrophes.",
                        "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFirstName.Focus();
                    return;
                }

                // Validate Middle Initial (optional)
                string middleInitial = txtMiddleInitial.Text.Trim();
                if (!string.IsNullOrEmpty(middleInitial))
                {
                    if (middleInitial.Length > 1)
                    {
                        MessageBox.Show("Middle Initial must be a single character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMiddleInitial.Focus();
                        return;
                    }
                    if (!char.IsLetter(middleInitial[0]))
                    {
                        MessageBox.Show("Middle Initial must be a letter.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMiddleInitial.Focus();
                        return;
                    }
                    middleInitial = middleInitial.ToUpper() + ".";
                }

                // Check for duplicate username
                var existingUsers = parentControl.GetAllUsers();
                if (existingUsers.Any(u => u.Username.Equals(txtUsername.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"Username '{txtUsername.Text}' already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                // Get selected role (remove icon)
                string selectedRole = cmbRole.SelectedItem.ToString();
                int spaceIndex = selectedRole.IndexOf(' ');
                if (spaceIndex > 0) selectedRole = selectedRole.Substring(spaceIndex + 1);

                // Build Full Name
                string fullName = $"{txtLastName.Text.Trim()}, {txtFirstName.Text.Trim()}";
                if (!string.IsNullOrEmpty(middleInitial))
                {
                    fullName += $" {middleInitial}";
                }

                string defaultPassword = "MatchPoint123!";

                var newUser = new UserManagementControl.UserData
                {
                    UserId = GenerateNewUserId(selectedRole),
                    Username = txtUsername.Text.Trim(),
                    FullName = fullName,
                    Role = selectedRole,
                    Status = "ACTIVE",
                    Password = defaultPassword,
                    CreatedDate = DateTime.Now
                };

                parentControl.AddUser(newUser);

                MessageBox.Show($"User {newUser.Username} created successfully!\n\n" +
                    $"ID: {newUser.UserId}\n" +
                    $"Name: {fullName}\n" +
                    $"Role: {selectedRole}\n" +
                    $"Default Password: {defaultPassword}\n\n" +
                    $"Please inform the user to change their password on first login.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                addForm.Close();
                LoadUsersIntoComboBox();
            };

            addForm.Controls.AddRange(new Control[] {
                lblUsername, txtUsername,
                lblLastName, txtLastName,
                lblFirstName, txtFirstName,
                lblMiddleInitial, txtMiddleInitial,
                lblRole, cmbRole,
                lblPasswordNote,
                btnSave, btnCancel
            });

            addForm.ShowDialog();
        }

        private void ShowEditUserDialog()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if user can edit this user
            if (GetRoleLevel(selectedUser.Role) <= GetRoleLevel(currentAdmin.Role))
            {
                MessageBox.Show($"You cannot edit {selectedUser.Role} users. You can only edit users with lower role than yours.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to edit user.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form editForm = new Form();
            editForm.Text = $"Edit User - {selectedUser.Username}";
            editForm.Size = new Size(600, 650);
            editForm.StartPosition = FormStartPosition.CenterParent;
            editForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            editForm.BackColor = Color.White;

            // Parse existing full name
            string lastName = "";
            string firstName = "";
            string middleInitial = "";

            string fullName = selectedUser.FullName;
            if (fullName.Contains(","))
            {
                var parts = fullName.Split(',');
                lastName = parts[0].Trim();

                string remaining = parts[1].Trim();
                int lastSpaceIndex = remaining.LastIndexOf(' ');
                if (lastSpaceIndex > 0 && remaining.EndsWith("."))
                {
                    firstName = remaining.Substring(0, lastSpaceIndex).Trim();
                    middleInitial = remaining.Substring(lastSpaceIndex + 1).Trim().Replace(".", "");
                }
                else
                {
                    firstName = remaining;
                    middleInitial = "";
                }
            }

            string originalLastName = lastName;
            string originalFirstName = firstName;
            string originalMiddleInitial = middleInitial;
            bool passwordReset = false;
            bool isSaved = false;

            // User ID (read-only)
            Label lblIDTitle = new Label
            {
                Text = "User ID:",
                Location = new Point(30, 20),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
            };
            Label lblID = new Label
            {
                Text = selectedUser.UserId,
                Location = new Point(150, 20),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            // Username (read-only)
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(30, 60),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            TextBox txtUsername = new TextBox
            {
                Location = new Point(150, 57),
                Size = new Size(350, 27),
                Text = selectedUser.Username,
                ReadOnly = true,
                BackColor = Color.LightGray,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            // Last Name
            Label lblLastName = new Label
            {
                Text = "Last Name:",
                Location = new Point(30, 100),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            TextBox txtLastName = new TextBox
            {
                Location = new Point(150, 97),
                Size = new Size(350, 27),
                Text = lastName,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            LimitTextBoxLength(txtLastName, 25);

            // First Name
            Label lblFirstName = new Label
            {
                Text = "First Name:",
                Location = new Point(30, 140),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            TextBox txtFirstName = new TextBox
            {
                Location = new Point(150, 137),
                Size = new Size(350, 27),
                Text = firstName,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            LimitTextBoxLength(txtFirstName, 25);

            // Middle Initial
            Label lblMiddleInitial = new Label
            {
                Text = "M.I.:",
                Location = new Point(30, 180),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            TextBox txtMiddleInitial = new TextBox
            {
                Location = new Point(150, 177),
                Size = new Size(60, 27),
                Text = middleInitial,
                Font = new System.Drawing.Font("Segoe UI", 10),
                MaxLength = 1
            };

            // Role (read-only display - cannot change role in edit)
            Label lblRole = new Label
            {
                Text = "Role:",
                Location = new Point(30, 220),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            Label lblRoleValue = new Label
            {
                Text = $"{GetRoleIcon(selectedUser.Role)} {selectedUser.Role}",
                Location = new Point(150, 220),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(79, 70, 229)
            };

            // Status (read-only display)
            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(30, 260),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            Label lblStatusValue = new Label
            {
                Text = selectedUser.Status == "ACTIVE" ? "🟢 ACTIVE" : "🔴 INACTIVE",
                Location = new Point(150, 260),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, selectedUser.Status == "ACTIVE" ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = selectedUser.Status == "ACTIVE" ? Color.Green : Color.Red
            };

            // Separator
            Label lblSeparator = new Label
            {
                Text = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
                Location = new Point(30, 300),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            // Reset Password section
            Label lblResetTitle = new Label
            {
                Text = "SECURITY",
                Location = new Point(30, 330),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            Button btnResetPassword = new Button
            {
                Text = "🔒 RESET PASSWORD",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, 360),
                Size = new Size(200, 45),
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            // Save Changes button
            Button btnSaveChanges = new Button
            {
                Text = "💾 SAVE CHANGES",
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, 440),
                Size = new Size(140, 40),
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(310, 440),
                Size = new Size(140, 40),
                Font = new System.Drawing.Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };

            // Real-time validation for name fields
            void AddNameValidation(TextBox textBox, Label label)
            {
                textBox.TextChanged += (s, ev) =>
                {
                    if (!string.IsNullOrEmpty(textBox.Text) && !IsValidName(textBox.Text))
                    {
                        textBox.BackColor = Color.LightPink;
                        label.ForeColor = Color.Red;
                    }
                    else
                    {
                        textBox.BackColor = Color.White;
                        label.ForeColor = Color.Black;
                    }
                };
            }

            AddNameValidation(txtLastName, lblLastName);
            AddNameValidation(txtFirstName, lblFirstName);
            AddNameValidation(txtMiddleInitial, lblMiddleInitial);

            // Reset Password Button Click
            btnResetPassword.Click += (s, ev) =>
            {
                DialogResult result = MessageBox.Show($"Reset password for {selectedUser.Username}?\n\nNew password will be set to: MatchPoint123!\n\nThis action cannot be undone.",
                    "Confirm Password Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string defaultPassword = "MatchPoint123!";
                    selectedUser.Password = defaultPassword;
                    passwordReset = true;

                    btnResetPassword.BackColor = Color.Green;
                    btnResetPassword.Text = "✓ PASSWORD RESET";
                    btnResetPassword.Enabled = false;
                }
            };

            // Function to check if there are unsaved changes
            bool HasUnsavedChanges()
            {
                bool lastNameChanged = txtLastName.Text.Trim() != originalLastName;
                bool firstNameChanged = txtFirstName.Text.Trim() != originalFirstName;
                bool middleInitialChanged = txtMiddleInitial.Text.Trim() != originalMiddleInitial;
                bool passwordChanged = passwordReset;
                return lastNameChanged || firstNameChanged || middleInitialChanged || passwordChanged;
            }

            // Function to build full name
            string BuildFullName()
            {
                string full = $"{txtLastName.Text.Trim()}, {txtFirstName.Text.Trim()}";
                string mi = txtMiddleInitial.Text.Trim();
                if (!string.IsNullOrEmpty(mi))
                {
                    full += $" {mi.ToUpper()}.";
                }
                return full;
            }

            // Function to validate and save changes
            bool ValidateAndSave()
            {
                // Validate Last Name
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLastName.Focus();
                    return false;
                }
                if (!IsValidName(txtLastName.Text))
                {
                    MessageBox.Show("Last Name can only contain letters, spaces, hyphens, and apostrophes.",
                        "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLastName.Focus();
                    return false;
                }

                // Validate First Name
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFirstName.Focus();
                    return false;
                }
                if (!IsValidName(txtFirstName.Text))
                {
                    MessageBox.Show("First Name can only contain letters, spaces, hyphens, and apostrophes.",
                        "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFirstName.Focus();
                    return false;
                }

                // Validate Middle Initial (optional)
                string mi = txtMiddleInitial.Text.Trim();
                if (!string.IsNullOrEmpty(mi))
                {
                    if (mi.Length > 1)
                    {
                        MessageBox.Show("Middle Initial must be a single character.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMiddleInitial.Focus();
                        return false;
                    }
                    if (!char.IsLetter(mi[0]))
                    {
                        MessageBox.Show("Middle Initial must be a letter.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMiddleInitial.Focus();
                        return false;
                    }
                }

                // Save changes
                selectedUser.FullName = BuildFullName();
                parentControl.UpdateUser(selectedUser);

                if (passwordReset)
                {
                    MessageBox.Show($"User {selectedUser.Username} updated successfully!\n\nPassword has been reset to: MatchPoint123!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"User {selectedUser.Username} updated successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                isSaved = true;
                return true;
            }

            // Save Changes Button Click
            btnSaveChanges.Click += (s, ev) =>
            {
                if (ValidateAndSave())
                {
                    editForm.Close();
                    LoadUsersIntoComboBox();
                }
            };

            // Cancel Button Click with unsaved changes warning
            btnCancel.Click += (s, ev) =>
            {
                if (HasUnsavedChanges() && !isSaved)
                {
                    DialogResult result = MessageBox.Show("You have unsaved changes.\n\nDo you want to save your changes before closing?",
                        "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        if (ValidateAndSave())
                        {
                            editForm.Close();
                            LoadUsersIntoComboBox();
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        editForm.Close();
                    }
                }
                else
                {
                    editForm.Close();
                }
            };

            // Form Closing event (X button)
            editForm.FormClosing += (s, ev) =>
            {
                if (HasUnsavedChanges() && !isSaved)
                {
                    DialogResult result = MessageBox.Show("You have unsaved changes.\n\nDo you want to save your changes before closing?",
                        "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        if (ValidateAndSave())
                        {
                            LoadUsersIntoComboBox();
                        }
                        else
                        {
                            ev.Cancel = true;
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        // Close without saving
                    }
                    else
                    {
                        ev.Cancel = true;
                    }
                }
            };

            editForm.Controls.AddRange(new Control[] {
                lblIDTitle, lblID,
                lblUsername, txtUsername,
                lblLastName, txtLastName,
                lblFirstName, txtFirstName,
                lblMiddleInitial, txtMiddleInitial,
                lblRole, lblRoleValue,
                lblStatus, lblStatusValue,
                lblSeparator,
                lblResetTitle, btnResetPassword,
                btnSaveChanges, btnCancel
            });

            editForm.ShowDialog();
        }

        private void DeactivateUser()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prevent deactivating yourself
            if (selectedUser.UserId == currentAdmin.UserId)
            {
                MessageBox.Show("You cannot deactivate your own account!", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check permission
            if (GetRoleLevel(selectedUser.Role) <= GetRoleLevel(currentAdmin.Role))
            {
                MessageBox.Show($"You cannot deactivate {selectedUser.Role} users. You can only manage users with lower role than yours.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to deactivate user.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.Status == "INACTIVE")
            {
                MessageBox.Show("User is already deactivated.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show($"Deactivate {selectedUser.Username}?\n\nUser will not be able to log in.",
                "Confirm Deactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                selectedUser.Status = "INACTIVE";
                parentControl.UpdateUser(selectedUser);
                MessageBox.Show($"User {selectedUser.Username} deactivated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersIntoComboBox();
            }
        }

        private void ReactivateUser()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check permission
            if (GetRoleLevel(selectedUser.Role) <= GetRoleLevel(currentAdmin.Role))
            {
                MessageBox.Show($"You cannot reactivate {selectedUser.Role} users. You can only manage users with lower role than yours.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to reactivate user.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.Status == "ACTIVE")
            {
                MessageBox.Show("User is already active.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show($"Reactivate {selectedUser.Username}?\n\nUser will regain access.",
                "Confirm Reactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                selectedUser.Status = "ACTIVE";
                parentControl.UpdateUser(selectedUser);
                MessageBox.Show($"User {selectedUser.Username} reactivated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersIntoComboBox();
            }
        }

        private string GenerateNewUserId(string role)
        {
            var users = parentControl.GetAllUsers();
            int maxNumber = 0;
            string prefix = "";

            switch (role.ToUpper())
            {
                case "SUPER ADMIN": prefix = "SA"; break;
                case "ADMIN": prefix = "AD"; break;
                case "MANAGER": prefix = "MG"; break;
                case "STAFF": prefix = "ST"; break;
                case "CASHIER": prefix = "CA"; break;
                case "CUSTOMER": prefix = "CU"; break;
                default: prefix = "US"; break;
            }

            foreach (var user in users.Where(u => u.UserId.StartsWith(prefix)))
            {
                string numberPart = user.UserId.Substring(2);
                if (int.TryParse(numberPart, out int num) && num > maxNumber)
                    maxNumber = num;
            }
            return $"{prefix}{(maxNumber + 1):D3}";
        }

        private void LimitTextBoxLength(TextBox textBox, int maxLength)
        {
            textBox.MaxLength = maxLength;
            textBox.KeyPress += (s, e) =>
            {
                if (textBox.Text.Length >= maxLength && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        private bool IsValidName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s\-']+$");
        }

        private bool ConfirmAdminPassword()
        {
            Form confirmForm = new Form();
            confirmForm.Text = "Security Verification";
            confirmForm.Size = new Size(450, 260);
            confirmForm.StartPosition = FormStartPosition.CenterParent;
            confirmForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            confirmForm.MaximizeBox = false;
            confirmForm.BackColor = Color.White;

            Label lblMessage = new Label
            {
                Text = "Please enter your password to authorize this action:",
                Location = new Point(20, 20),
                Size = new Size(390, 45),
                Font = new System.Drawing.Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(20, 80),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
            };

            TextBox txtPassword = new TextBox
            {
                Location = new Point(120, 77),
                Size = new Size(280, 30),
                PasswordChar = '*',
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            CheckBox chkShowPass = new CheckBox
            {
                Text = "Show Password",
                Location = new Point(120, 115),
                AutoSize = true
            };
            chkShowPass.CheckedChanged += (s, e) =>
            {
                txtPassword.PasswordChar = chkShowPass.Checked ? '\0' : '*';
            };

            Button btnConfirm = new Button
            {
                Text = "CONFIRM",
                BackColor = Color.FromArgb(46, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(120, 160),
                Size = new Size(110, 40)
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(240, 160),
                Size = new Size(110, 40)
            };

            bool isAuthorized = false;

            btnConfirm.Click += (s, e) =>
            {
                var currentUser = parentControl.GetCurrentLoggedInUser();

                if (currentUser != null && currentUser.Password == txtPassword.Text)
                {
                    isAuthorized = true;
                    confirmForm.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect password. Access denied.", "Authorization Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            };

            btnCancel.Click += (s, e) => confirmForm.Close();

            confirmForm.Controls.AddRange(new Control[] {
                lblMessage, lblPassword, txtPassword, chkShowPass, btnConfirm, btnCancel
            });

            confirmForm.ShowDialog();
            return isAuthorized;
        }
    }
}