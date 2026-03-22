using finaluserandstaff;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cms.lastsuper
{
    public partial class frmManageUsers : Form
    {
        private UserManagementControl parentControl;
        private UserManagementControl.UserData selectedUser;
        private UserManagementControl.UserData currentAdmin;

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

            

            // Change button text
            btnAddUser.Text = "+ ADD STAFF";
            btnEditUser.Text = "✏️ EDIT STAFF";
            btnDeactivateUser.Text = "🚫 DEACTIVATE";
            btnReactivateUser.Text = "🔄 REACTIVATE";

            // Load users (STAFF only)
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
                // Only show STAFF users (not other Admins)
                var users = parentControl.GetAllUsers().Where(u => u.Role == "STAFF").ToList();
                cmbUsers.Items.Clear();
                foreach (var user in users)
                {
                    cmbUsers.Items.Add($"{user.ID} - {user.FullName}");
                }
                if (cmbUsers.Items.Count > 0)
                {
                    cmbUsers.SelectedIndex = 0;
                }
                else
                {
                    lblUserInfo.Text = "𝐔𝐒𝐄𝐑 𝐈𝐍𝐅𝐎\n\nNo staff members found";
                }
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUsers.SelectedIndex >= 0 && parentControl != null)
            {
                var staffUsers = parentControl.GetAllUsers().Where(u => u.Role == "STAFF").ToList();
                if (cmbUsers.SelectedIndex < staffUsers.Count)
                {
                    selectedUser = staffUsers[cmbUsers.SelectedIndex];

                    // Style the label
                    lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Regular);
                    lblUserInfo.BackColor = Color.White;
                    lblUserInfo.BorderStyle = BorderStyle.None;
                    lblUserInfo.Padding = new Padding(5);

                    string statusDisplay = selectedUser.Status == "ACTIVE" ? "🟢 ACTIVE" : "🔴 INACTIVE";

                    // Using Unicode bold characters for labels
                    lblUserInfo.Text =
                        $"𝐔𝐒𝐄𝐑 𝐈𝐍𝐅𝐎\n\n" +
                        $"𝐈𝐃          : {selectedUser.ID}\n" +
                        $"𝐔𝐬𝐞𝐫𝐧𝐚𝐦𝐞    : {selectedUser.Username}\n" +
                        $"𝐅𝐮𝐥𝐥 𝐍𝐚𝐦𝐞 : {selectedUser.FullName}\n" +
                        $"𝐑𝐨𝐥𝐞       : {selectedUser.Role}\n" +
                        $"𝐒𝐭𝐚𝐭𝐮𝐬     : {statusDisplay}\n" +
                        $"𝐋𝐚𝐬𝐭 𝐋𝐨𝐠𝐢𝐧 : {selectedUser.LastLogin:MMM dd, yyyy HH:mm}";

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
            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to add new staff.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form addForm = new Form();
            addForm.Text = "Add New Staff";
            addForm.Size = new Size(500, 480);
            addForm.StartPosition = FormStartPosition.CenterParent;
            addForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            addForm.BackColor = Color.White;

            Label lblUsername = new Label { Text = "Username:", Location = new Point(30, 40), AutoSize = true };
            TextBox txtUsername = new TextBox { Location = new Point(150, 37), Size = new Size(300, 27) };
            LimitTextBoxLength(txtUsername, 25);

            Label lblFullName = new Label { Text = "Full Name:", Location = new Point(30, 80), AutoSize = true };
            TextBox txtFullName = new TextBox { Location = new Point(150, 77), Size = new Size(300, 27) };
            LimitTextBoxLength(txtFullName, 25);

            Label lblPassword = new Label { Text = "Password:", Location = new Point(30, 120), AutoSize = true };
            TextBox txtPassword = new TextBox { Location = new Point(150, 117), Size = new Size(300, 27), PasswordChar = '*' };
            LimitTextBoxLength(txtPassword, 25);

            Label lblConfirm = new Label { Text = "Confirm:", Location = new Point(30, 160), AutoSize = true };
            TextBox txtConfirm = new TextBox { Location = new Point(150, 157), Size = new Size(300, 27), PasswordChar = '*' };
            LimitTextBoxLength(txtConfirm, 25);

            CheckBox chkShowPass = new CheckBox
            {
                Text = "Show Password",
                Location = new Point(150, 190),
                AutoSize = true
            };
            chkShowPass.CheckedChanged += (s, ev) =>
            {
                txtPassword.PasswordChar = chkShowPass.Checked ? '\0' : '*';
                txtConfirm.PasswordChar = chkShowPass.Checked ? '\0' : '*';
            };

            Button btnSave = new Button
            {
                Text = "CREATE STAFF",
                BackColor = Color.FromArgb(46, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, 240),
                Size = new Size(140, 40)
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(310, 240),
                Size = new Size(140, 40)
            };
            btnCancel.Click += (s, ev) => addForm.Close();

            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtPassword.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtPassword.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var existingUsers = parentControl.GetAllUsers();
                if (existingUsers.Any(u => u.Username.Equals(txtUsername.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"Username '{txtUsername.Text}' already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newUser = new UserManagementControl.UserData
                {
                    ID = GenerateNewID(),
                    Username = txtUsername.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Role = "STAFF",
                    Status = "ACTIVE",
                    Password = txtPassword.Text,
                    LastLogin = DateTime.Now
                };

                parentControl.AddUser(newUser);
                MessageBox.Show($"Staff {newUser.Username} created successfully!\nID: {newUser.ID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addForm.Close();
                LoadUsersIntoComboBox();
            };

            addForm.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblFullName, txtFullName,
                lblPassword, txtPassword, lblConfirm, txtConfirm,
                chkShowPass, btnSave, btnCancel
            });

            addForm.ShowDialog();
        }

        private void ShowEditUserDialog()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a staff member first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to edit staff.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form editForm = new Form();
            editForm.Text = $"Edit Staff - {selectedUser.Username}";
            editForm.Size = new Size(500, 480);
            editForm.StartPosition = FormStartPosition.CenterParent;
            editForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            editForm.BackColor = Color.White;

            // Store original values to detect changes
            string originalFullName = selectedUser.FullName;
            string originalPassword = selectedUser.Password;
            bool passwordReset = false;
            bool isSaved = false; // Flag to track if changes were saved

            // User ID (read-only)
            Label lblIDTitle = new Label
            {
                Text = "User ID:",
                Location = new Point(30, 30),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
            };
            Label lblID = new Label
            {
                Text = selectedUser.ID,
                Location = new Point(150, 30),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            // Username (read-only)
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(30, 70),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            TextBox txtUsername = new TextBox
            {
                Location = new Point(150, 67),
                Size = new Size(300, 27),
                Text = selectedUser.Username,
                ReadOnly = true,
                BackColor = Color.LightGray,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            // Full Name (editable)
            Label lblFullName = new Label
            {
                Text = "Full Name:",
                Location = new Point(30, 110),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            TextBox txtFullName = new TextBox
            {
                Location = new Point(150, 107),
                Size = new Size(300, 27),
                Text = selectedUser.FullName,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            LimitTextBoxLength(txtFullName, 25);

            // Status (read-only display)
            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(30, 150),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            Label lblStatusValue = new Label
            {
                Text = selectedUser.Status == "ACTIVE" ? "🟢 ACTIVE" : "🔴 INACTIVE",
                Location = new Point(150, 150),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, selectedUser.Status == "ACTIVE" ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = selectedUser.Status == "ACTIVE" ? Color.Green : Color.Red
            };

            // Separator
            Label lblSeparator = new Label
            {
                Text = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
                Location = new Point(30, 190),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            // Reset Password section
            Label lblResetTitle = new Label
            {
                Text = "SECURITY",
                Location = new Point(30, 220),
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
                Location = new Point(150, 250),
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
                Location = new Point(150, 330),
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
                Location = new Point(310, 330),
                Size = new Size(140, 40),
                Font = new System.Drawing.Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };

            // Real-time validation for Full Name (no special characters)
            txtFullName.TextChanged += (s, ev) =>
            {
                if (!string.IsNullOrEmpty(txtFullName.Text) && !IsValidName(txtFullName.Text))
                {
                    txtFullName.BackColor = Color.LightPink;
                    lblFullName.ForeColor = Color.Red;
                    lblFullName.Text = "Full Name: (No special characters!)";
                }
                else
                {
                    txtFullName.BackColor = Color.White;
                    lblFullName.ForeColor = Color.Black;
                    lblFullName.Text = "Full Name:";
                }
            };

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
                bool fullNameChanged = txtFullName.Text.Trim() != originalFullName;
                bool passwordChanged = passwordReset;
                return fullNameChanged || passwordChanged;
            }

            // Function to validate and save changes
            bool ValidateAndSave()
            {
                // Validate Full Name
                if (!IsValidName(txtFullName.Text))
                {
                    MessageBox.Show("Full Name can only contain letters, numbers, spaces, hyphens, and apostrophes.",
                        "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Full Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Save changes
                selectedUser.FullName = txtFullName.Text.Trim();
                parentControl.UpdateUser(selectedUser);

                if (passwordReset)
                {
                    MessageBox.Show($"Staff {selectedUser.Username} updated successfully!\n\nPassword has been reset to: MatchPoint123!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtFullName.Text.Trim() != originalFullName)
                {
                    MessageBox.Show($"Staff {selectedUser.Username} updated successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                isSaved = true; // Mark as saved
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
                    // Cancel: do nothing, stay on form
                }
                else
                {
                    editForm.Close();
                }
            };

            // Form Closing event (X button)
            editForm.FormClosing += (s, ev) =>
            {
                // Only show warning if there are unsaved changes AND not already saved
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
                    else // Cancel
                    {
                        ev.Cancel = true;
                    }
                }
            };

            editForm.Controls.AddRange(new Control[] {
        lblIDTitle, lblID, lblUsername, txtUsername,
        lblFullName, txtFullName, lblStatus, lblStatusValue,
        lblSeparator, lblResetTitle, btnResetPassword,
        btnSaveChanges, btnCancel
    });

            editForm.ShowDialog();
        }

        private void DeactivateUser()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a staff member first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prevent deactivating yourself
            if (selectedUser.ID == currentAdmin.ID)
            {
                MessageBox.Show("You cannot deactivate your own account!", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to deactivate staff.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.Status == "INACTIVE")
            {
                MessageBox.Show("Staff member is already deactivated.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show($"Deactivate {selectedUser.Username}?\n\nUser will not be able to log in.",
                "Confirm Deactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                selectedUser.Status = "INACTIVE";
                parentControl.UpdateUser(selectedUser);
                MessageBox.Show($"Staff {selectedUser.Username} deactivated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersIntoComboBox();
            }
        }

        private void ReactivateUser()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a staff member first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to reactivate staff.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.Status == "ACTIVE")
            {
                MessageBox.Show("Staff member is already active.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show($"Reactivate {selectedUser.Username}?\n\nUser will regain access.",
                "Confirm Reactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                selectedUser.Status = "ACTIVE";
                parentControl.UpdateUser(selectedUser);
                MessageBox.Show($"Staff {selectedUser.Username} reactivated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersIntoComboBox();
            }
        }

        private string GenerateNewID()
        {
            var users = parentControl.GetAllUsers();
            int maxNumber = 0;
            string prefix = "ST";

            foreach (var user in users.Where(u => u.ID.StartsWith(prefix)))
            {
                string numberPart = user.ID.Substring(2);
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
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9\s\-']+$");
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