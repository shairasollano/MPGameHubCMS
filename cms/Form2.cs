using cms;
using KGHCashierPOS;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class Form2 : Form
    {
        // Track password visibility state
        private bool isPasswordVisible = false;
        private string passwordText = "";
        private bool isClosing = false;
        private bool isLoggingIn = false;
        private Timer fadeTimer;

        // MySQL Connection String - Using matchpoint_db
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";

        public Form2()
        {
            InitializeComponent();
            SetupForm();

            // Initialize database connection check only (don't recreate tables)
            CheckDatabaseConnection();
        }

        private void CheckDatabaseConnection()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("Database connection successful!");
                }
            }
            catch (MySqlException mysqlEx)
            {
                MessageBox.Show($"MySQL Error: {mysqlEx.Message}\n\n" +
                    "Please make sure:\n" +
                    "1. XAMPP is running\n" +
                    "2. MySQL is started in XAMPP Control Panel\n" +
                    "3. MySQL is running on port 3306",
                    "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}\n\n" +
                    "Please make sure XAMPP is running and MySQL is started.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupForm()
        {
            this.Text = "MatchPoint - Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // Wire up form events for proper cleanup
            this.FormClosing += Form2_FormClosing;
            this.FormClosed += Form2_FormClosed;
            this.Load += Form2_Load;

            // Configure show password button
            SetupShowPasswordButton();

            // Wire up events
            if (btnSignIn != null) btnSignIn.Click += btnSignIn_Click;
            if (btnExit != null) btnExit.Click += btnExit_Click;
            if (lblForgotPassword != null) lblForgotPassword.Click += lblForgotPassword_Click;

            // Set Enter key to trigger login
            this.AcceptButton = btnSignIn;

            // Setup textboxes with placeholder text
            SetupTextBoxes();

            // Initialize password field
            passwordText = "";

            // Handle key events
            if (txtPassword != null)
            {
                txtPassword.KeyPress += txtPassword_KeyPress;
                txtPassword.KeyDown += txtPassword_KeyDown;
            }
            if (txtUsername != null) txtUsername.KeyDown += txtUsername_KeyDown;

            // Setup hover effects
            SetupHoverEffects();

            // Add fade-in effect
            this.Opacity = 0;
            fadeTimer = new Timer();
            fadeTimer.Interval = 20;
            fadeTimer.Tick += FadeTimer_Tick;
            fadeTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.05;
            }
            else
            {
                if (fadeTimer != null)
                {
                    fadeTimer.Stop();
                    fadeTimer.Tick -= FadeTimer_Tick;
                    fadeTimer.Dispose();
                    fadeTimer = null;
                }
            }
        }

        private void SetupShowPasswordButton()
        {
            // Configure the existing btnShowPassword from designer
            if (btnShowPassword != null)
            {
                btnShowPassword.Visible = true;
                btnShowPassword.BackColor = Color.Transparent;
                btnShowPassword.FlatStyle = FlatStyle.Flat;
                btnShowPassword.FlatAppearance.BorderSize = 0;
                btnShowPassword.Font = new Font("Segoe UI", 12F);
                btnShowPassword.ForeColor = Color.FromArgb(180, 180, 180);
                btnShowPassword.Text = "👁";
                btnShowPassword.Cursor = Cursors.Hand;
                btnShowPassword.Click += ShowPasswordButton_Click;
                btnShowPassword.MouseEnter += (s, e) => btnShowPassword.ForeColor = Color.FromArgb(228, 186, 94);
                btnShowPassword.MouseLeave += (s, e) => btnShowPassword.ForeColor = Color.FromArgb(180, 180, 180);
            }
        }

        private void SetupTextBoxes()
        {
            // Setup username textbox with placeholder
            if (txtUsername != null)
            {
                txtUsername.Text = "Enter your username";
                txtUsername.ForeColor = Color.FromArgb(120, 120, 120);
                txtUsername.GotFocus += RemoveUsernamePlaceholder;
                txtUsername.LostFocus += AddUsernamePlaceholder;
            }

            // Setup password textbox with placeholder
            if (txtPassword != null)
            {
                txtPassword.Text = "Enter your password";
                txtPassword.ForeColor = Color.FromArgb(120, 120, 120);
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.GotFocus += RemovePasswordPlaceholder;
                txtPassword.LostFocus += AddPasswordPlaceholder;
            }
        }

        private void RemoveUsernamePlaceholder(object sender, EventArgs e)
        {
            if (txtUsername != null && txtUsername.Text == "Enter your username")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.White;
            }
        }

        private void AddUsernamePlaceholder(object sender, EventArgs e)
        {
            if (txtUsername != null && string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Enter your username";
                txtUsername.ForeColor = Color.FromArgb(120, 120, 120);
            }
        }

        private void RemovePasswordPlaceholder(object sender, EventArgs e)
        {
            if (txtPassword != null && txtPassword.Text == "Enter your password")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.White;
                txtPassword.UseSystemPasswordChar = true;
                if (btnShowPassword != null)
                    btnShowPassword.ForeColor = Color.FromArgb(180, 180, 180);
            }
        }

        private void AddPasswordPlaceholder(object sender, EventArgs e)
        {
            if (txtPassword != null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Enter your password";
                txtPassword.ForeColor = Color.FromArgb(120, 120, 120);
                txtPassword.UseSystemPasswordChar = false;
                isPasswordVisible = false;
                passwordText = "";
                if (btnShowPassword != null)
                {
                    btnShowPassword.Text = "👁";
                    btnShowPassword.ForeColor = Color.FromArgb(180, 180, 180);
                }
            }
        }

        private void SetupHoverEffects()
        {
            // Sign In button hover effects
            if (btnSignIn != null)
            {
                btnSignIn.MouseEnter += (s, e) => btnSignIn.BackColor = Color.FromArgb(248, 206, 114);
                btnSignIn.MouseLeave += (s, e) => btnSignIn.BackColor = Color.FromArgb(228, 186, 94);
            }

            // Exit button hover
            if (btnExit != null)
            {
                btnExit.MouseEnter += (s, e) =>
                {
                    btnExit.ForeColor = Color.FromArgb(180, 180, 180);
                    btnExit.Font = new Font(btnExit.Font, FontStyle.Underline);
                };
                btnExit.MouseLeave += (s, e) =>
                {
                    btnExit.ForeColor = Color.FromArgb(120, 120, 120);
                    btnExit.Font = new Font(btnExit.Font, FontStyle.Regular);
                };
            }

            // Forgot password link hover
            if (lblForgotPassword != null)
            {
                lblForgotPassword.MouseEnter += (s, e) =>
                {
                    lblForgotPassword.ForeColor = Color.FromArgb(228, 186, 94);
                    lblForgotPassword.Font = new Font(lblForgotPassword.Font, FontStyle.Underline);
                };
                lblForgotPassword.MouseLeave += (s, e) =>
                {
                    lblForgotPassword.ForeColor = Color.FromArgb(140, 140, 140);
                    lblForgotPassword.Font = new Font(lblForgotPassword.Font, FontStyle.Regular);
                };
            }
        }

        private void ShowPasswordButton_Click(object sender, EventArgs e)
        {
            if (txtPassword == null || txtPassword.Text == "Enter your password")
                return;

            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Show password
                txtPassword.Text = passwordText;
                txtPassword.UseSystemPasswordChar = false;
                if (btnShowPassword != null)
                {
                    btnShowPassword.Text = "👁‍🗨";
                    btnShowPassword.ForeColor = Color.FromArgb(228, 186, 94);
                }
            }
            else
            {
                // Hide password
                txtPassword.Text = new string('•', passwordText.Length);
                txtPassword.UseSystemPasswordChar = true;
                if (btnShowPassword != null)
                {
                    btnShowPassword.Text = "👁";
                    btnShowPassword.ForeColor = Color.FromArgb(180, 180, 180);
                }
            }

            if (txtPassword != null) txtPassword.SelectionStart = txtPassword.Text.Length;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtPassword != null && !isPasswordVisible && txtPassword.Text != "Enter your password" && !char.IsControl(e.KeyChar))
            {
                int cursorPos = txtPassword.SelectionStart;
                passwordText = passwordText.Insert(cursorPos, e.KeyChar.ToString());
                txtPassword.Text = new string('•', passwordText.Length);
                txtPassword.SelectionStart = cursorPos + 1;
                e.Handled = true;
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPassword != null && !isPasswordVisible && txtPassword.Text != "Enter your password")
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (passwordText.Length > 0 && txtPassword.SelectionStart > 0)
                    {
                        int cursorPos = txtPassword.SelectionStart;
                        passwordText = passwordText.Remove(cursorPos - 1, 1);
                        txtPassword.Text = new string('•', passwordText.Length);
                        txtPassword.SelectionStart = cursorPos - 1;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (passwordText.Length > 0 && txtPassword.SelectionStart < passwordText.Length)
                    {
                        int cursorPos = txtPassword.SelectionStart;
                        passwordText = passwordText.Remove(cursorPos, 1);
                        txtPassword.Text = new string('•', passwordText.Length);
                        txtPassword.SelectionStart = cursorPos;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    btnSignIn_Click(btnSignIn, new EventArgs());
                    e.Handled = true;
                }
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtPassword != null)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        // ⭐ AUTHENTICATION WITH MySQL - Gets username and password from users table
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (isLoggingIn) return;

            string username = txtUsername != null ? txtUsername.Text.Trim() : "";
            string password = isPasswordVisible ? (txtPassword != null ? txtPassword.Text : "") : passwordText;

            if (username == "Enter your username") username = "";
            if (txtPassword != null && txtPassword.Text == "Enter your password") password = "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isLoggingIn = true;
            this.Cursor = Cursors.WaitCursor;
            if (btnSignIn != null) btnSignIn.Enabled = false;

            try
            {
                // Authenticate user from MySQL database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    // Query to check username and password
                    string query = "SELECT * FROM users WHERE Username = @username AND Password = @password AND Status = 'ACTIVE'";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Login successful
                                string userId = reader["UserId"].ToString();
                                string userRole = reader["Role"].ToString();
                                string userStatus = reader["Status"].ToString();

                                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                                System.Diagnostics.Debug.WriteLine($"Login Successful:");
                                System.Diagnostics.Debug.WriteLine($"  User: {username}");
                                System.Diagnostics.Debug.WriteLine($"  Role: {userRole}");
                                System.Diagnostics.Debug.WriteLine($"  Status: {userStatus}");
                                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");

                                // ==============================================
                                // SET GLOBAL LOGGER USER INFO
                                // ==============================================
                                GlobalLogger.CurrentUsername = username;
                                GlobalLogger.CurrentUserRole = userRole;
                                GlobalLogger.CurrentUserId = userId;

                                // Log the login activity
                                try
                                {
                                    GlobalLogger.Log("Login", $"User '{username}' logged in as {userRole}", "Info", "System");
                                    Activitylogs.Instance?.AddLogEntry(username, "Login", $"User '{username}' logged into the system", "Info", "System");
                                }
                                catch (Exception logEx)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Login logging failed: {logEx.Message}");
                                }

                                this.Cursor = Cursors.Default;
                                if (btnSignIn != null) btnSignIn.Enabled = true;

                                // Hide login form
                                this.Hide();

                                // ⭐ ROUTING LOGIC based on role
                                string role = userRole.ToUpper();

                                try
                                {
                                    if (role == "ADMIN" || role == "MANAGER")
                                    {
                                        // Admin/Manager goes to Form1
                                        Form1 adminForm = new Form1();
                                        adminForm.LoggedInUsername = username;
                                        adminForm.LoggedInUserRole = userRole;
                                        adminForm.SetCurrentUser(username, userRole);
                                        adminForm.FormClosed += (s, args) =>
                                        {
                                            Application.Exit();
                                        };
                                        adminForm.Show();
                                    }
                                    else if (role == "STAFF")
                                    {
                                        // Staff goes to CashierForm (POS)
                                        KGHCashierPOS.CashierForm staffForm = new KGHCashierPOS.CashierForm();
                                        staffForm.SetLoginForm(this);
                                        staffForm.SetCurrentUser(username, userRole);
                                        staffForm.FormClosed += (s, args) =>
                                        {
                                            Application.Exit();
                                        };
                                        staffForm.Show();

                                        // Log successful staff login
                                        try
                                        {
                                            GlobalLogger.LogInfo("Staff", $"Staff {username} logged in successfully");
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid user role. Please contact administrator.",
                                            "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        this.Show();
                                        ClearLoginFields();
                                        isLoggingIn = false;
                                        return;
                                    }
                                }
                                catch (Exception formEx)
                                {
                                    MessageBox.Show($"Error opening form: {formEx.Message}\n\n" +
                                        "Please check if the form is properly configured.",
                                        "Form Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Show();
                                    ClearLoginFields();
                                    isLoggingIn = false;
                                    return;
                                }
                            }
                            else
                            {
                                // Login failed - log failed attempt
                                try
                                {
                                    GlobalLogger.LogWarning("System", $"Failed login attempt for username: {username}");
                                }
                                catch { }

                                // Check if user exists but is inactive
                                string inactiveQuery = "SELECT * FROM users WHERE Username = @username";
                                using (MySqlCommand inactiveCmd = new MySqlCommand(inactiveQuery, conn))
                                {
                                    inactiveCmd.Parameters.AddWithValue("@username", username);

                                    using (MySqlDataReader inactiveReader = inactiveCmd.ExecuteReader())
                                    {
                                        if (inactiveReader.Read())
                                        {
                                            string status = inactiveReader["Status"].ToString();
                                            if (status != "ACTIVE")
                                            {
                                                MessageBox.Show("Your account is inactive. Please contact administrator.",
                                                    "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            }
                                            else
                                            {
                                                MessageBox.Show("Invalid password. Please try again.",
                                                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Username not found. Please check your username.",
                                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }

                                this.Cursor = Cursors.Default;
                                if (btnSignIn != null) btnSignIn.Enabled = true;
                                ClearLoginFields();
                                isLoggingIn = false;
                            }
                        }
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                this.Cursor = Cursors.Default;
                if (btnSignIn != null) btnSignIn.Enabled = true;
                isLoggingIn = false;

                // Log database error
                try
                {
                    GlobalLogger.LogError("Database", $"MySQL Error: {mysqlEx.Message}");
                }
                catch { }

                MessageBox.Show($"MySQL Error: {mysqlEx.Message}\n\n" +
                    "Please make sure:\n" +
                    "1. XAMPP is running\n" +
                    "2. MySQL is started in XAMPP Control Panel\n" +
                    "3. MySQL is running on port 3306",
                    "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                if (btnSignIn != null) btnSignIn.Enabled = true;
                isLoggingIn = false;

                // Log general error
                try
                {
                    GlobalLogger.LogError("Login", $"Login error: {ex.Message}");
                }
                catch { }

                MessageBox.Show($"Login error:\n{ex.Message}\n\nPlease make sure XAMPP is running and MySQL is started.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearLoginFields()
        {
            passwordText = "";
            if (txtPassword != null)
            {
                txtPassword.Text = "Enter your password";
                txtPassword.ForeColor = Color.FromArgb(120, 120, 120);
                txtPassword.UseSystemPasswordChar = false;
            }
            isPasswordVisible = false;
            if (btnShowPassword != null)
            {
                btnShowPassword.Text = "👁";
                btnShowPassword.ForeColor = Color.FromArgb(180, 180, 180);
            }

            if (txtUsername != null)
            {
                txtUsername.Focus();
                if (txtUsername.Text != "Enter your username")
                {
                    txtUsername.SelectAll();
                }
            }
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            // Log that user viewed forgot password info
            try
            {
                GlobalLogger.LogInfo("System", "User viewed forgot password information");
            }
            catch { }

            MessageBox.Show(
                "Please contact your system administrator to reset your password.\n\n" +
                "📧 Email: admin@matchpoint.com\n" +
                "📞 Phone: (02) 8123-4567\n\n" +
                "Test Credentials:\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                "👑 ADMIN (Full Management Access):\n" +
                "   Username: admin\n" +
                "   Password: admin123\n\n" +
                "👔 MANAGER (Limited Management Access):\n" +
                "   Username: manager\n" +
                "   Password: manager123\n\n" +
                "👤 STAFF (POS Access):\n" +
                "   Username: staff\n" +
                "   Password: staff123\n\n" +
                "⚠️ Make sure XAMPP is running with MySQL started.",
                "Forgot Password",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Log application exit
                try
                {
                    GlobalLogger.LogInfo("System", "Application closed by user");
                }
                catch { }

                isClosing = true;
                Application.Exit();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (panelLogin != null)
            {
                panelLogin.Left = (this.ClientSize.Width - panelLogin.Width) / 2;
                panelLogin.Top = (this.ClientSize.Height - panelLogin.Height) / 2;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Initialize Activity Logs early
            try
            {
                Activitylogs.EnsureInitialized();
                System.Diagnostics.Debug.WriteLine("Activity Logs initialized successfully!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialize Activity Logs: {ex.Message}");
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosing) return;

            // Don't close if we're in the middle of logging in
            if (isLoggingIn)
            {
                e.Cancel = true;
                return;
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            CleanupEventHandlers();
        }

        private void CleanupEventHandlers()
        {
            try
            {
                isClosing = true;

                // Stop and dispose timer
                if (fadeTimer != null)
                {
                    fadeTimer.Stop();
                    fadeTimer.Tick -= FadeTimer_Tick;
                    fadeTimer.Dispose();
                    fadeTimer = null;
                }

                // Remove form events
                this.FormClosing -= Form2_FormClosing;
                this.FormClosed -= Form2_FormClosed;
                this.Load -= Form2_Load;

                // Clean up controls
                CleanupControls(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cleanup error: {ex.Message}");
            }
        }

        private void CleanupControls(Control parent)
        {
            if (parent == null) return;

            try
            {
                // Remove event handlers from the parent control
                parent.Click -= null;
                parent.MouseClick -= null;
                parent.MouseEnter -= null;
                parent.MouseLeave -= null;
                parent.MouseMove -= null;
                parent.Paint -= null;

                // Handle specific control types
                if (parent is Button button)
                {
                    button.Click -= null;
                    button.MouseEnter -= null;
                    button.MouseLeave -= null;
                }
                else if (parent is TextBox textBox)
                {
                    textBox.TextChanged -= null;
                    textBox.KeyPress -= null;
                    textBox.KeyDown -= null;
                    textBox.GotFocus -= null;
                    textBox.LostFocus -= null;
                }
                else if (parent is Label label)
                {
                    label.Click -= null;
                    label.MouseEnter -= null;
                    label.MouseLeave -= null;
                }
                else if (parent is PictureBox pictureBox)
                {
                    pictureBox.Click -= null;
                    pictureBox.MouseEnter -= null;
                    pictureBox.MouseLeave -= null;
                }

                // Recursively clean up child controls
                foreach (Control child in parent.Controls)
                {
                    CleanupControls(child);
                }
            }
            catch { }
        }

        private void pictureBoxBg_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}