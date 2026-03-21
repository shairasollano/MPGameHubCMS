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

        public frmManageUsers(UserManagementControl control)
        {
            parentControl = control;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Manage Users - MatchPoint Gaming Hub";

            // ADD THIS LINE - Wire up the ComboBox event
            this.cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;

            // Load users
            LoadUsersIntoComboBox();

            // Wire up button events
            btnAddUser.Click += (s, e) => ShowAddUserDialog();
            btnEditUser.Click += (s, e) => ShowEditUserDialog();
            btnDeleteUser.Click += (s, e) => DeleteUser();
            btnDeactivateUser.Click += (s, e) => DeactivateUser();
            btnReactivateUser.Click += (s, e) => ReactivateUser();
            btnChangeRole.Click += (s, e) => ChangeUserRole();
            btnResetPassword.Click += (s, e) => ResetPassword();
        }

        private void LoadUsersIntoComboBox()
        {
            if (parentControl != null)
            {
                var users = parentControl.GetAllUsers();
                cmbUsers.Items.Clear();
                foreach (var user in users)
                {
                    cmbUsers.Items.Add($"{user.ID} - {user.FullName} ({user.Role})");
                }
                if (cmbUsers.Items.Count > 0)
                {
                    cmbUsers.SelectedIndex = 0;
                    // FORCE the label to update
                    cmbUsers_SelectedIndexChanged(this, EventArgs.Empty);
                }
                else
                {
                    lblUserInfo.Text = "𝐔𝐒𝐄𝐑 𝐈𝐍𝐅𝐎\n\nNo users found";
                }
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUsers.SelectedIndex >= 0 && parentControl != null)
            {
                var users = parentControl.GetAllUsers();
                if (cmbUsers.SelectedIndex < users.Count)
                {
                    selectedUser = users[cmbUsers.SelectedIndex];

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

        private void ShowAddUserDialog()
        {
            // Authorization check
            if (!ConfirmSuperAdminPassword())
            {
                MessageBox.Show("Authorization required to add new users.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form addForm = new Form();
            addForm.Text = "Add New User";
            addForm.Size = new Size(500, 520);
            addForm.StartPosition = FormStartPosition.CenterParent;
            addForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            addForm.BackColor = Color.White;

            Label lblUsername = new Label { Text = "Username:", Location = new Point(30, 40), AutoSize = true };
            TextBox txtUsername = new TextBox { Location = new Point(150, 37), Size = new Size(300, 27) };
            LimitTextBoxLength(txtUsername, 25);

            Label lblFullName = new Label { Text = "Full Name:", Location = new Point(30, 80), AutoSize = true };
            TextBox txtFullName = new TextBox { Location = new Point(150, 77), Size = new Size(300, 27) };
            LimitTextBoxLength(txtFullName, 25);

            Label lblRole = new Label { Text = "Role:", Location = new Point(30, 120), AutoSize = true };
            ComboBox cmbRole = new ComboBox
            {
                Location = new Point(150, 117),
                Size = new Size(300, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new object[] { "SUPER ADMIN", "ADMIN", "STAFF" });
            cmbRole.SelectedIndex = 2;

            Label lblPassword = new Label { Text = "Password:", Location = new Point(30, 160), AutoSize = true };
            TextBox txtPassword = new TextBox { Location = new Point(150, 157), Size = new Size(300, 27), PasswordChar = '*' };
            LimitTextBoxLength(txtPassword, 25);

            Label lblConfirm = new Label { Text = "Confirm:", Location = new Point(30, 200), AutoSize = true };
            TextBox txtConfirm = new TextBox { Location = new Point(150, 197), Size = new Size(300, 27), PasswordChar = '*' };
            LimitTextBoxLength(txtConfirm, 25);

            CheckBox chkShowPass = new CheckBox
            {
                Text = "Show Password",
                Location = new Point(150, 230),
                AutoSize = true
            };
            chkShowPass.CheckedChanged += (s, ev) =>
            {
                txtPassword.PasswordChar = chkShowPass.Checked ? '\0' : '*';
                txtConfirm.PasswordChar = chkShowPass.Checked ? '\0' : '*';
            };

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

            Button btnSave = new Button
            {
                Text = "CREATE USER",
                BackColor = Color.FromArgb(46, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, 280),
                Size = new Size(140, 40)
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(310, 280),
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

                if (txtUsername.Text.Length > 25)
                {
                    MessageBox.Show("Username cannot exceed 25 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Full Name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtFullName.Text.Length > 25)
                {
                    MessageBox.Show("Full Name cannot exceed 25 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!IsValidName(txtFullName.Text))
                {
                    MessageBox.Show("Full Name can only contain letters, numbers, spaces, hyphens, and apostrophes.",
                        "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var existingUsers = parentControl.GetAllUsers();
                if (existingUsers.Any(u => u.Username.Equals(txtUsername.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"Username '{txtUsername.Text}' already exists. Please choose a different username.",
                        "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                string selectedRole = cmbRole.Text;

                var newUser = new UserManagementControl.UserData
                {
                    ID = GenerateNewID(selectedRole),
                    Username = txtUsername.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Role = selectedRole,
                    Status = "ACTIVE",
                    Password = txtPassword.Text,
                    LastLogin = DateTime.Now
                };

                parentControl.AddUser(newUser);
                MessageBox.Show($"User {newUser.Username} created successfully!\nID: {newUser.ID}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                addForm.Close();
                LoadUsersIntoComboBox();
            };

            addForm.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblFullName, txtFullName,
                lblRole, cmbRole,
                lblPassword, txtPassword, lblConfirm, txtConfirm,
                chkShowPass, btnSave, btnCancel
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

            // Authorization check
            if (!ConfirmSuperAdminPassword())
            {
                MessageBox.Show("Authorization required to edit users.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form editForm = new Form();
            editForm.Text = $"Edit User - {selectedUser.Username}";
            editForm.Size = new Size(500, 400);
            editForm.StartPosition = FormStartPosition.CenterParent;
            editForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            editForm.BackColor = Color.White;

            Label lblUsername = new Label { Text = "Username:", Location = new Point(30, 40), AutoSize = true };
            TextBox txtUsername = new TextBox { Location = new Point(150, 37), Size = new Size(300, 27), Text = selectedUser.Username, ReadOnly = true, BackColor = Color.LightGray };

            Label lblFullName = new Label { Text = "Full Name:", Location = new Point(30, 80), AutoSize = true };
            TextBox txtFullName = new TextBox { Location = new Point(150, 77), Size = new Size(300, 27), Text = selectedUser.FullName };
            LimitTextBoxLength(txtFullName, 25);

            Label lblRole = new Label { Text = "Role:", Location = new Point(30, 120), AutoSize = true };
            ComboBox cmbRole = new ComboBox
            {
                Location = new Point(150, 117),
                Size = new Size(300, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new object[] { "SUPER ADMIN", "ADMIN", "STAFF" });
            cmbRole.SelectedItem = selectedUser.Role;

            Button btnSave = new Button
            {
                Text = "UPDATE USER",
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, 200),
                Size = new Size(140, 40)
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(310, 200),
                Size = new Size(140, 40)
            };
            btnCancel.Click += (s, ev) => editForm.Close();

            btnSave.Click += (s, ev) =>
            {
                if (!IsValidName(txtFullName.Text))
                {
                    MessageBox.Show("Full Name can only contain letters, numbers, spaces, hyphens, and apostrophes.",
                        "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedUser.FullName = txtFullName.Text.Trim();
                selectedUser.Role = cmbRole.Text;

                parentControl.UpdateUser(selectedUser);
                MessageBox.Show($"User {selectedUser.Username} updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                editForm.Close();
                LoadUsersIntoComboBox();
            };

            editForm.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblFullName, txtFullName,
                lblRole, cmbRole, btnSave, btnCancel
            });

            editForm.ShowDialog();
        }

        private void DeleteUser()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmSuperAdminPassword())
            {
                MessageBox.Show("Authorization required to delete users.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure you want to delete {selectedUser.Username}?\n\nThis action cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                parentControl.DeleteUser(selectedUser.ID);
                MessageBox.Show($"User {selectedUser.Username} deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersIntoComboBox();
            }
        }

        private void DeactivateUser()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmSuperAdminPassword())
            {
                MessageBox.Show("Authorization required to deactivate users.", "Access Denied",
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

            // Authorization check
            if (!ConfirmSuperAdminPassword())
            {
                MessageBox.Show("Authorization required to reactivate users.", "Access Denied",
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

        private void ChangeUserRole()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmSuperAdminPassword())
            {
                MessageBox.Show("Authorization required to change user roles.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form roleForm = new Form();
            roleForm.Text = $"Change Role - {selectedUser.Username}";
            roleForm.Size = new Size(400, 250);
            roleForm.StartPosition = FormStartPosition.CenterParent;
            roleForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            roleForm.BackColor = Color.White;

            Label lblCurrent = new Label
            {
                Text = $"Current Role: {selectedUser.Role}",
                Location = new Point(30, 40),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblNewRole = new Label { Text = "New Role:", Location = new Point(30, 90), AutoSize = true };
            ComboBox cmbNewRole = new ComboBox
            {
                Location = new Point(120, 87),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbNewRole.Items.AddRange(new object[] { "SUPER ADMIN", "ADMIN", "STAFF" });
            cmbNewRole.SelectedItem = selectedUser.Role;

            Button btnChange = new Button
            {
                Text = "CHANGE ROLE",
                BackColor = Color.FromArgb(111, 66, 193),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(120, 140),
                Size = new Size(140, 40)
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(270, 140),
                Size = new Size(100, 40)
            };
            btnCancel.Click += (s, ev) => roleForm.Close();

            btnChange.Click += (s, ev) =>
            {
                string oldRole = selectedUser.Role;
                selectedUser.Role = cmbNewRole.Text;
                parentControl.UpdateUser(selectedUser);
                MessageBox.Show($"Role changed from {oldRole} to {selectedUser.Role}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                roleForm.Close();
                LoadUsersIntoComboBox();
            };

            roleForm.Controls.AddRange(new Control[] { lblCurrent, lblNewRole, cmbNewRole, btnChange, btnCancel });
            roleForm.ShowDialog();
        }

        private void ResetPassword()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authorization check
            if (!ConfirmSuperAdminPassword())
            {
                MessageBox.Show("Authorization required to reset passwords.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string defaultPassword = "MatchPoint123!";

            DialogResult result = MessageBox.Show($"Reset password for {selectedUser.Username}?\n\nNew password will be: {defaultPassword}",
                "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                selectedUser.Password = defaultPassword;
                parentControl.UpdateUser(selectedUser);
                MessageBox.Show($"Password for {selectedUser.Username} has been reset to:\n{defaultPassword}\n\nPlease inform the user to change it on next login.",
                    "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GenerateNewID(string role)
        {
            var users = parentControl.GetAllUsers();
            int maxNumber = 0;

            string prefix;
            switch (role.ToUpper())
            {
                case "SUPER ADMIN":
                    prefix = "SA";
                    break;
                case "ADMIN":
                    prefix = "AD";
                    break;
                case "STAFF":
                    prefix = "ST";
                    break;
                default:
                    prefix = "US";
                    break;
            }

            foreach (var user in users)
            {
                if (user.ID.StartsWith(prefix))
                {
                    string numberPart = user.ID.Substring(2);
                    if (int.TryParse(numberPart, out int num))
                    {
                        if (num > maxNumber)
                        {
                            maxNumber = num;
                        }
                    }
                }
            }

            return $"{prefix}{(maxNumber + 1):D3}";
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            // Load is handled in constructor
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

        private bool ConfirmSuperAdminPassword()
        {
            Form confirmForm = new Form();
            confirmForm.Text = "Security Verification";
            confirmForm.Size = new Size(450, 260);  // Made wider and taller
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
                Size = new Size(280, 30),  // Wider textbox
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
                Size = new Size(110, 40)  // Taller button
            };

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(240, 160),
                Size = new Size(110, 40)  // Taller button
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

        private void btnResetPassword_Click(object sender, EventArgs e)
        {

        }
    }
}