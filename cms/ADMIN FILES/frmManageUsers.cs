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
                // Only show STAFF users
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
                    lblUserInfo.Text = "USER INFO\n\nNo staff members found";
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

                    lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
                    lblUserInfo.BackColor = Color.White;
                    lblUserInfo.BorderStyle = BorderStyle.None;
                    lblUserInfo.Padding = new Padding(5);

                    string statusDisplay = selectedUser.Status == "ACTIVE" ? "🟢 ACTIVE" : "🔴 INACTIVE";

                    lblUserInfo.Text =
                        $"USER INFO\n" +
                        $"ID          : {selectedUser.ID}\n" +
                        $"Username    : {selectedUser.Username}\n" +
                        $"Full Name   : {selectedUser.FullName}\n" +
                        $"Role        : {selectedUser.Role}\n" +
                        $"Status      : {statusDisplay}\n" +
                        $"Last Login  : {selectedUser.LastLogin:MMM dd, yyyy HH:mm}";

                    UpdateButtonStates();
                }
            }
            else
            {
                selectedUser = null;
                lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 11);
                lblUserInfo.BackColor = Color.White;
                lblUserInfo.BorderStyle = BorderStyle.None;
                lblUserInfo.Text = "USER INFO\n\nSelect a user from the dropdown above";
            }
        }

        private void UpdateButtonStates()
        {
            if (selectedUser != null)
            {
                btnDeactivateUser.Enabled = selectedUser.Status == "ACTIVE";
                btnDeactivateUser.BackColor = selectedUser.Status == "ACTIVE" ? Color.FromArgb(220, 53, 69) : Color.Gray;

                btnReactivateUser.Enabled = selectedUser.Status == "INACTIVE";
                btnReactivateUser.BackColor = selectedUser.Status == "INACTIVE" ? Color.FromArgb(255, 193, 7) : Color.Gray;
            }
        }

        private void ShowAddUserDialog()
        {
            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to add new staff.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form addForm = new Form();
            addForm.Text = "Add New Staff";
            addForm.Size = new Size(550, 600);
            addForm.StartPosition = FormStartPosition.CenterParent;
            addForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            addForm.BackColor = Color.White;

            // Username
            Label lblUsername = new Label { Text = "Username:", Location = new Point(30, 30), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtUsername = new TextBox { Location = new Point(150, 27), Size = new Size(350, 27), Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtUsername, 25);
            Label lblUsernameError = new Label { Text = "", Location = new Point(150, 55), Size = new Size(350, 20), ForeColor = Color.Red, Font = new System.Drawing.Font("Segoe UI", 8), Visible = false };

            // Last Name
            Label lblLastName = new Label { Text = "Last Name:", Location = new Point(30, 80), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtLastName = new TextBox { Location = new Point(150, 77), Size = new Size(350, 27), Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtLastName, 25);
            Label lblLastNameError = new Label { Text = "Last Name is required", Location = new Point(150, 105), Size = new Size(350, 20), ForeColor = Color.Red, Font = new System.Drawing.Font("Segoe UI", 8), Visible = false };

            // First Name
            Label lblFirstName = new Label { Text = "First Name:", Location = new Point(30, 130), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtFirstName = new TextBox { Location = new Point(150, 127), Size = new Size(350, 27), Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtFirstName, 25);
            Label lblFirstNameError = new Label { Text = "First Name is required", Location = new Point(150, 155), Size = new Size(350, 20), ForeColor = Color.Red, Font = new System.Drawing.Font("Segoe UI", 8), Visible = false };

            // Middle Initial
            Label lblMiddleInitial = new Label { Text = "M.I.:", Location = new Point(30, 180), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtMiddleInitial = new TextBox { Location = new Point(150, 177), Size = new Size(60, 27), Font = new System.Drawing.Font("Segoe UI", 10), MaxLength = 1 };
            Label lblMiddleError = new Label { Text = "M.I. is required (single letter)", Location = new Point(150, 205), Size = new Size(350, 20), ForeColor = Color.Red, Font = new System.Drawing.Font("Segoe UI", 8), Visible = false };

            // Password note
            Label lblPasswordNote = new Label
            {
                Text = "⚠️ Default password will be set to: MatchPoint123!\n   User must change password on first login.",
                Location = new Point(30, 240),
                Size = new Size(470, 45),
                Font = new System.Drawing.Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Button btnSave = new Button
            {
                Text = "CREATE STAFF",
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

            // Username validation
            void ValidateUsername()
            {
                string username = txtUsername.Text.Trim();
                bool hasUpper = username.Any(char.IsUpper);
                bool hasLower = username.Any(char.IsLower);
                bool hasSpecial = username.Any(ch => !char.IsLetterOrDigit(ch));
                bool hasLetters = username.Any(char.IsLetter);

                if (string.IsNullOrEmpty(username))
                {
                    lblUsernameError.Text = "Username is required";
                    lblUsernameError.Visible = true;
                    txtUsername.BackColor = Color.LightPink;
                }
                else if (!hasUpper)
                {
                    lblUsernameError.Text = "Username must contain at least 1 uppercase letter";
                    lblUsernameError.Visible = true;
                    txtUsername.BackColor = Color.LightPink;
                }
                else if (!hasLower)
                {
                    lblUsernameError.Text = "Username must contain at least 1 lowercase letter";
                    lblUsernameError.Visible = true;
                    txtUsername.BackColor = Color.LightPink;
                }
                else if (!hasSpecial)
                {
                    lblUsernameError.Text = "Username must contain at least 1 special character (!@#$%^&*)";
                    lblUsernameError.Visible = true;
                    txtUsername.BackColor = Color.LightPink;
                }
                else if (!hasLetters)
                {
                    lblUsernameError.Text = "Username must contain letters";
                    lblUsernameError.Visible = true;
                    txtUsername.BackColor = Color.LightPink;
                }
                else
                {
                    lblUsernameError.Visible = false;
                    txtUsername.BackColor = Color.White;
                }
            }

            void ValidateAllFields()
            {
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    lblLastNameError.Visible = true;
                    txtLastName.BackColor = Color.LightPink;
                }
                else if (!IsValidName(txtLastName.Text))
                {
                    lblLastNameError.Text = "Only letters, spaces, hyphens, and apostrophes allowed";
                    lblLastNameError.Visible = true;
                    txtLastName.BackColor = Color.LightPink;
                }
                else
                {
                    lblLastNameError.Visible = false;
                    txtLastName.BackColor = Color.White;
                }

                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    lblFirstNameError.Visible = true;
                    txtFirstName.BackColor = Color.LightPink;
                }
                else if (!IsValidName(txtFirstName.Text))
                {
                    lblFirstNameError.Text = "Only letters, spaces, hyphens, and apostrophes allowed";
                    lblFirstNameError.Visible = true;
                    txtFirstName.BackColor = Color.LightPink;
                }
                else
                {
                    lblFirstNameError.Visible = false;
                    txtFirstName.BackColor = Color.White;
                }

                string mi = txtMiddleInitial.Text.Trim();
                if (string.IsNullOrEmpty(mi))
                {
                    lblMiddleError.Text = "Middle Initial is required";
                    lblMiddleError.Visible = true;
                    txtMiddleInitial.BackColor = Color.LightPink;
                }
                else if (mi.Length > 1)
                {
                    lblMiddleError.Text = "Must be a single character";
                    lblMiddleError.Visible = true;
                    txtMiddleInitial.BackColor = Color.LightPink;
                }
                else if (!char.IsLetter(mi[0]))
                {
                    lblMiddleError.Text = "Must be a letter";
                    lblMiddleError.Visible = true;
                    txtMiddleInitial.BackColor = Color.LightPink;
                }
                else
                {
                    lblMiddleError.Visible = false;
                    txtMiddleInitial.BackColor = Color.White;
                }

                ValidateUsername();
            }

            txtUsername.TextChanged += (s, ev) => ValidateAllFields();
            txtLastName.TextChanged += (s, ev) => ValidateAllFields();
            txtFirstName.TextChanged += (s, ev) => ValidateAllFields();
            txtMiddleInitial.TextChanged += (s, ev) => ValidateAllFields();

            btnSave.Click += (s, ev) =>
            {
                ValidateAllFields();

                if (lblUsernameError.Visible || lblLastNameError.Visible || lblFirstNameError.Visible || lblMiddleError.Visible)
                {
                    MessageBox.Show("Please correct all errors before saving.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                var existingUsers = parentControl.GetAllUsers();
                if (existingUsers.Any(u => u.Username.Equals(txtUsername.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"Username '{txtUsername.Text}' already exists.", "Duplicate",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                string middleInitial = txtMiddleInitial.Text.Trim().ToUpper() + ".";
                string fullName = $"{txtLastName.Text.Trim()}, {txtFirstName.Text.Trim()}";
                if (!string.IsNullOrEmpty(middleInitial))
                {
                    fullName += $" {middleInitial}";
                }

                string defaultPassword = "MatchPoint123!";

                var newUser = new UserManagementControl.UserData
                {
                    ID = GenerateNewID(),
                    Username = txtUsername.Text.Trim(),
                    FullName = fullName,
                    Role = "STAFF",
                    Status = "ACTIVE",
                    Password = defaultPassword,
                    LastLogin = DateTime.Now
                };

                parentControl.AddUser(newUser);

                MessageBox.Show($"Staff {newUser.Username} created successfully!\n\n" +
                    $"ID: {newUser.ID}\n" +
                    $"Name: {fullName}\n" +
                    $"Default Password: {defaultPassword}\n\n" +
                    $"Please inform the user to change their password on first login.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                addForm.Close();
                LoadUsersIntoComboBox();
            };

            addForm.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblUsernameError,
                lblLastName, txtLastName, lblLastNameError,
                lblFirstName, txtFirstName, lblFirstNameError,
                lblMiddleInitial, txtMiddleInitial, lblMiddleError,
                lblPasswordNote,
                btnSave, btnCancel
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

            if (!ConfirmAdminPassword())
            {
                MessageBox.Show("Authorization required to edit staff.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form editForm = new Form();
            editForm.Text = $"Edit Staff - {selectedUser.Username}";
            editForm.Size = new Size(550, 550);
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
                }
            }

            string originalLastName = lastName;
            string originalFirstName = firstName;
            string originalMiddleInitial = middleInitial;
            bool passwordReset = false;
            bool isSaved = false;

            // User ID
            Label lblIDTitle = new Label { Text = "User ID:", Location = new Point(30, 20), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold) };
            Label lblID = new Label { Text = selectedUser.ID, Location = new Point(150, 20), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };

            // Username (read-only)
            Label lblUsername = new Label { Text = "Username:", Location = new Point(30, 60), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtUsername = new TextBox { Location = new Point(150, 57), Size = new Size(350, 27), Text = selectedUser.Username, ReadOnly = true, BackColor = Color.LightGray, Font = new System.Drawing.Font("Segoe UI", 10) };

            // Last Name
            Label lblLastName = new Label { Text = "Last Name:", Location = new Point(30, 100), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtLastName = new TextBox { Location = new Point(150, 97), Size = new Size(350, 27), Text = lastName, Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtLastName, 25);
            Label lblLastNameError = new Label { Text = "Last Name is required", Location = new Point(150, 125), Size = new Size(350, 20), ForeColor = Color.Red, Font = new System.Drawing.Font("Segoe UI", 8), Visible = false };

            // First Name
            Label lblFirstName = new Label { Text = "First Name:", Location = new Point(30, 150), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtFirstName = new TextBox { Location = new Point(150, 147), Size = new Size(350, 27), Text = firstName, Font = new System.Drawing.Font("Segoe UI", 10) };
            LimitTextBoxLength(txtFirstName, 25);
            Label lblFirstNameError = new Label { Text = "First Name is required", Location = new Point(150, 175), Size = new Size(350, 20), ForeColor = Color.Red, Font = new System.Drawing.Font("Segoe UI", 8), Visible = false };

            // Middle Initial
            Label lblMiddleInitial = new Label { Text = "M.I.:", Location = new Point(30, 200), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            TextBox txtMiddleInitial = new TextBox { Location = new Point(150, 197), Size = new Size(60, 27), Text = middleInitial, Font = new System.Drawing.Font("Segoe UI", 10), MaxLength = 1 };
            Label lblMiddleError = new Label { Text = "M.I. is required (single letter)", Location = new Point(150, 225), Size = new Size(350, 20), ForeColor = Color.Red, Font = new System.Drawing.Font("Segoe UI", 8), Visible = false };

            // Status
            Label lblStatus = new Label { Text = "Status:", Location = new Point(30, 260), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            Label lblStatusValue = new Label
            {
                Text = selectedUser.Status == "ACTIVE" ? "🟢 ACTIVE" : "🔴 INACTIVE",
                Location = new Point(150, 260),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, selectedUser.Status == "ACTIVE" ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = selectedUser.Status == "ACTIVE" ? Color.Green : Color.Red
            };

            Label lblSeparator = new Label
            {
                Text = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
                Location = new Point(30, 300),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

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

            void ValidateNameFields()
            {
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    lblLastNameError.Visible = true;
                    txtLastName.BackColor = Color.LightPink;
                }
                else if (!IsValidName(txtLastName.Text))
                {
                    lblLastNameError.Text = "Only letters, spaces, hyphens, and apostrophes allowed";
                    lblLastNameError.Visible = true;
                    txtLastName.BackColor = Color.LightPink;
                }
                else
                {
                    lblLastNameError.Visible = false;
                    txtLastName.BackColor = Color.White;
                }

                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    lblFirstNameError.Visible = true;
                    txtFirstName.BackColor = Color.LightPink;
                }
                else if (!IsValidName(txtFirstName.Text))
                {
                    lblFirstNameError.Text = "Only letters, spaces, hyphens, and apostrophes allowed";
                    lblFirstNameError.Visible = true;
                    txtFirstName.BackColor = Color.LightPink;
                }
                else
                {
                    lblFirstNameError.Visible = false;
                    txtFirstName.BackColor = Color.White;
                }

                string mi = txtMiddleInitial.Text.Trim();
                if (string.IsNullOrEmpty(mi))
                {
                    lblMiddleError.Text = "Middle Initial is required";
                    lblMiddleError.Visible = true;
                    txtMiddleInitial.BackColor = Color.LightPink;
                }
                else if (mi.Length > 1)
                {
                    lblMiddleError.Text = "Must be a single character";
                    lblMiddleError.Visible = true;
                    txtMiddleInitial.BackColor = Color.LightPink;
                }
                else if (!char.IsLetter(mi[0]))
                {
                    lblMiddleError.Text = "Must be a letter";
                    lblMiddleError.Visible = true;
                    txtMiddleInitial.BackColor = Color.LightPink;
                }
                else
                {
                    lblMiddleError.Visible = false;
                    txtMiddleInitial.BackColor = Color.White;
                }
            }

            txtLastName.TextChanged += (s, ev) => ValidateNameFields();
            txtFirstName.TextChanged += (s, ev) => ValidateNameFields();
            txtMiddleInitial.TextChanged += (s, ev) => ValidateNameFields();

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

            bool HasUnsavedChanges()
            {
                return txtLastName.Text.Trim() != originalLastName ||
                       txtFirstName.Text.Trim() != originalFirstName ||
                       txtMiddleInitial.Text.Trim() != originalMiddleInitial ||
                       passwordReset;
            }

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

            bool ValidateAndSave()
            {
                ValidateNameFields();

                if (lblLastNameError.Visible || lblFirstNameError.Visible || lblMiddleError.Visible)
                {
                    MessageBox.Show("Please correct all errors before saving.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                selectedUser.FullName = BuildFullName();
                parentControl.UpdateUser(selectedUser);

                if (passwordReset)
                {
                    MessageBox.Show($"Staff {selectedUser.Username} updated successfully!\n\nPassword has been reset to: MatchPoint123!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Staff {selectedUser.Username} updated successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                isSaved = true;
                return true;
            }

            btnSaveChanges.Click += (s, ev) =>
            {
                if (ValidateAndSave())
                {
                    editForm.Close();
                    LoadUsersIntoComboBox();
                }
            };

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
                lblLastName, txtLastName, lblLastNameError,
                lblFirstName, txtFirstName, lblFirstNameError,
                lblMiddleInitial, txtMiddleInitial, lblMiddleError,
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
                MessageBox.Show("Please select a staff member first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.ID == currentAdmin.ID)
            {
                MessageBox.Show("You cannot deactivate your own account!", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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