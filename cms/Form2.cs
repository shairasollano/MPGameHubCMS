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

        // Password toggle icon
        private Label showPassIcon;

        // Timer for fade effect
        private Timer fadeTimer;

        // MySQL Connection String
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";

        public Form2()
        {
            InitializeComponent();
            SetupForm();

            // Initialize database
            InitializeDatabase();
        }

        private void InitializeDatabase()
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

                    // Create users table if it doesn't exist
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS users (
                            Id INT PRIMARY KEY AUTO_INCREMENT,
                            UserId VARCHAR(20) NOT NULL UNIQUE,
                            Username VARCHAR(50) NOT NULL UNIQUE,
                            Password VARCHAR(100) NOT NULL,
                            Role VARCHAR(20) NOT NULL,
                            Status VARCHAR(10) NOT NULL DEFAULT 'ACTIVE',
                            CreatedDate DATETIME NOT NULL,
                            LastModifiedDate DATETIME
                        )";

                    using (MySqlCommand cmd = new MySqlCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Check if users table is empty and seed static accounts
                    string checkUsersQuery = "SELECT COUNT(*) FROM users";
                    using (MySqlCommand cmd = new MySqlCommand(checkUsersQuery, conn))
                    {
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (userCount == 0)
                        {
                            SeedStaticAccounts(conn);
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

        private void SeedStaticAccounts(MySqlConnection conn)
        {
            string insertQuery = @"
                INSERT INTO users (UserId, Username, Password, Role, Status, CreatedDate) 
                VALUES (@UserId, @Username, @Password, @Role, @Status, @CreatedDate)";

            var staticUsers = new[]
            {
                new { UserId = "USR001", Username = "admin", Password = "admin123", Role = "ADMIN" },
                new { UserId = "USR002", Username = "manager", Password = "manager123", Role = "MANAGER" },
                new { UserId = "USR003", Username = "staff", Password = "staff123", Role = "STAFF" },
                new { UserId = "USR004", Username = "customer", Password = "customer123", Role = "CUSTOMER" }
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

        private void SetupForm()
        {
            this.Text = "MatchPoint - Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // Create password toggle icon
            CreateShowPasswordIcon();

            // Wire up events
            btnSignIn.Click += btnSignIn_Click;
            btnExit.Click += btnExit_Click;
            lblForgotPassword.Click += lblForgotPassword_Click;

            // Set Enter key to trigger login
            this.AcceptButton = btnSignIn;

            // Setup textboxes with placeholder text
            SetupTextBoxes();

            // Initialize password field
            passwordText = "";

            // Handle key events
            txtPassword.KeyPress += txtPassword_KeyPress;
            txtPassword.KeyDown += txtPassword_KeyDown;
            txtUsername.KeyDown += txtUsername_KeyDown;

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
                    fadeTimer.Dispose();
                    fadeTimer = null;
                }
            }
        }

        private void CreateShowPasswordIcon()
        {
            showPassIcon = new Label
            {
                Text = "👁️",
                Location = new Point(txtPassword.Right - 35, txtPassword.Top + 8),
                Size = new Size(30, 25),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(120, 120, 120)
            };
            showPassIcon.Click += showPassIcon_Click;
            this.panelLogin.Controls.Add(showPassIcon);
            showPassIcon.BringToFront();
        }

        private void SetupTextBoxes()
        {
            // Setup username textbox with placeholder
            txtUsername.Text = "Enter your username";
            txtUsername.ForeColor = Color.FromArgb(120, 120, 120);
            txtUsername.GotFocus += RemoveUsernamePlaceholder;
            txtUsername.LostFocus += AddUsernamePlaceholder;

            // Setup password textbox with placeholder
            txtPassword.Text = "Enter your password";
            txtPassword.ForeColor = Color.FromArgb(120, 120, 120);
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.GotFocus += RemovePasswordPlaceholder;
            txtPassword.LostFocus += AddPasswordPlaceholder;
        }

        private void RemoveUsernamePlaceholder(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Enter your username")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.White;
            }
        }

        private void AddUsernamePlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Enter your username";
                txtUsername.ForeColor = Color.FromArgb(120, 120, 120);
            }
        }

        private void RemovePasswordPlaceholder(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Enter your password")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.White;
                txtPassword.UseSystemPasswordChar = true;
                if (showPassIcon != null)
                    showPassIcon.ForeColor = Color.FromArgb(120, 120, 120);
            }
        }

        private void AddPasswordPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Enter your password";
                txtPassword.ForeColor = Color.FromArgb(120, 120, 120);
                txtPassword.UseSystemPasswordChar = false;
                isPasswordVisible = false;
                passwordText = "";
                if (showPassIcon != null)
                {
                    showPassIcon.Text = "👁️";
                    showPassIcon.ForeColor = Color.FromArgb(120, 120, 120);
                }
            }
        }

        private void SetupHoverEffects()
        {
            // Sign In button hover effects
            btnSignIn.MouseEnter += (s, e) => btnSignIn.BackColor = Color.FromArgb(248, 206, 114);
            btnSignIn.MouseLeave += (s, e) => btnSignIn.BackColor = Color.FromArgb(228, 186, 94);

            // Exit button hover
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

            // Forgot password link hover
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

            // Password toggle icon hover
            if (showPassIcon != null)
            {
                showPassIcon.MouseEnter += (s, e) => showPassIcon.ForeColor = Color.FromArgb(228, 186, 94);
                showPassIcon.MouseLeave += (s, e) => showPassIcon.ForeColor = Color.FromArgb(120, 120, 120);
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isPasswordVisible && txtPassword.Text != "Enter your password" && !char.IsControl(e.KeyChar))
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
            if (!isPasswordVisible && txtPassword.Text != "Enter your password")
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
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void showPassIcon_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Enter your password")
                return;

            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                txtPassword.Text = passwordText;
                txtPassword.UseSystemPasswordChar = false;
                if (showPassIcon != null)
                {
                    showPassIcon.Text = "👁️‍🗨️";
                    showPassIcon.ForeColor = Color.FromArgb(228, 186, 94);
                }
            }
            else
            {
                txtPassword.Text = new string('•', passwordText.Length);
                txtPassword.UseSystemPasswordChar = true;
                if (showPassIcon != null)
                {
                    showPassIcon.Text = "👁️";
                    showPassIcon.ForeColor = Color.FromArgb(120, 120, 120);
                }
            }

            txtPassword.SelectionStart = txtPassword.Text.Length;
        }

        // ⭐ AUTHENTICATION WITH MySQL
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = isPasswordVisible ? txtPassword.Text : passwordText;

            if (username == "Enter your username") username = "";
            if (txtPassword.Text == "Enter your password") password = "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            btnSignIn.Enabled = false;

            try
            {
                // Authenticate user from MySQL database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

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

                                this.Cursor = Cursors.Default;
                                btnSignIn.Enabled = true;

                                this.Hide();

                                // ⭐ ROUTING LOGIC - Admin/Manager to Form1, Staff/Cashier to CashierForm
                                string role = userRole.ToUpper();

                                if (role == "ADMIN" || role == "MANAGER")
                                {
                                    // Admin or Manager goes to Form1 (User Management with Game Rates, Equipment, etc.)
                                    Form1 adminForm = new Form1();

                                    // Set user info in Form1
                                    adminForm.LoggedInUsername = username;
                                    adminForm.LoggedInUserRole = userRole;
                                    adminForm.SetCurrentUser(username, userRole);

                                    adminForm.Closed += (s, args) =>
                                    {
                                        this.Close();
                                    };
                                    adminForm.Show();
                                }
                                else if (role == "STAFF" || role == "CASHIER")
                                {
                                    // Staff/Cashier goes to CashierForm
                                    KGHCashierPOS.CashierForm cashierForm = new KGHCashierPOS.CashierForm();

                                    // Set user info in CashierForm
                                    cashierForm.SetCurrentUser(username, userRole);

                                    cashierForm.Closed += (s, args) =>
                                    {
                                        this.Close();
                                    };
                                    cashierForm.Show();
                                }
                                else if (role == "CUSTOMER")
                                {
                                    // Customer goes to OrderForm
                                    // Note: Ensure OrderForm exists in your project or add the correct namespace
                                    OrderForm customerForm = new OrderForm();

                                    // Optional: Pass user info if OrderForm needs it
                                    // customerForm.SetCurrentUser(username, userRole);

                                    customerForm.FormClosed += (s, args) =>
                                    {
                                        this.Close(); // Fully close app when order form is closed
                                    };

                                    customerForm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid user role. Please contact administrator.",
                                        "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Show();
                                    ClearLoginFields();
                                }
                            }
                            else
                            {
                                // Check if user exists but is inactive
                                string inactiveQuery = "SELECT * FROM users WHERE Username = @username AND Password = @password";
                                using (MySqlCommand inactiveCmd = new MySqlCommand(inactiveQuery, conn))
                                {
                                    inactiveCmd.Parameters.AddWithValue("@username", username);
                                    inactiveCmd.Parameters.AddWithValue("@password", password);

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
                                                MessageBox.Show("Invalid username or password. Please try again.",
                                                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Invalid username or password. Please try again.",
                                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }

                                this.Cursor = Cursors.Default;
                                btnSignIn.Enabled = true;
                                ClearLoginFields();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                btnSignIn.Enabled = true;
                MessageBox.Show($"Login error:\n{ex.Message}\n\nPlease make sure XAMPP is running and MySQL is started.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearLoginFields()
        {
            passwordText = "";
            txtPassword.Text = "Enter your password";
            txtPassword.ForeColor = Color.FromArgb(120, 120, 120);
            txtPassword.UseSystemPasswordChar = false;
            isPasswordVisible = false;
            if (showPassIcon != null)
            {
                showPassIcon.Text = "👁️";
                showPassIcon.ForeColor = Color.FromArgb(120, 120, 120);
            }

            txtUsername.Focus();
            if (txtUsername.Text != "Enter your username")
            {
                txtUsername.SelectAll();
            }
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Please contact your system administrator to reset your password.\n\n" +
                "📧 Email: admin@matchpoint.com\n" +
                "📞 Phone: (02) 8123-4567\n\n" +
                "Test Credentials:\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                "👑 ADMIN (Management Access):\n" +
                "   Username: admin\n" +
                "   Password: admin123\n\n" +
                "👔 MANAGER (Management Access):\n" +
                "   Username: manager\n" +
                "   Password: manager123\n\n" +
                "💼 STAFF (Cashier Access):\n" +
                "   Username: staff\n" +
                "   Password: staff123\n\n" +
                "Note: New users created will have default password: 12345\n\n" +
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

                if (showPassIcon != null && txtPassword != null)
                {
                    showPassIcon.Location = new Point(txtPassword.Right - 35, txtPassword.Top + 8);
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Database already initialized in constructor
        }
    }
}
