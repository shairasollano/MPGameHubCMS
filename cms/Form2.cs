using cms;
using KGHCashierPOS;
using Mysqlx.Crud;
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

        public Form2()
        {
            InitializeComponent();
            SetupForm();
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

        // ⭐ UPDATED - Database Authentication
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
                // ⭐ ONLY call AuthenticateUser - it handles all logging internally
                LoginResult result = AuthenticationRepository.AuthenticateUser(username, password);

                this.Cursor = Cursors.Default;
                btnSignIn.Enabled = true;

                if (result.Success)
                {
                    // ⭐ Just debug output - NO database logging here
                    System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                    System.Diagnostics.Debug.WriteLine($"Login Successful:");
                    System.Diagnostics.Debug.WriteLine($"  User: {UserSession.Username}");
                    System.Diagnostics.Debug.WriteLine($"  Role: {UserSession.RoleName}");
                    System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");

                    this.Hide();

                    // ⭐ Route to appropriate form - NO logging here
                    switch (result.RoleId)
                    {
                        case 1: // Super Admin
                            Form1 superAdminForm = new Form1();
                            superAdminForm.Closed += (s, args) =>
                            {
                                AuthenticationRepository.LogoutUser();
                                this.Close();
                            };
                            superAdminForm.Show();
                            break;

                        case 2: // Admin
                            Form1 adminForm = new Form1();
                            adminForm.Closed += (s, args) =>
                            {
                                AuthenticationRepository.LogoutUser();
                                this.Close();
                            };
                            adminForm.Show();
                            break;

                        case 3: // Cashier
                            KGHCashierPOS.CashierForm cashierForm = new KGHCashierPOS.CashierForm();
                            cashierForm.Closed += (s, args) =>
                            {
                                AuthenticationRepository.LogoutUser();
                                this.Close();
                            };
                            cashierForm.Show();
                            break;

                        case 4: // Customer
                            KGHCashierPOS.OrderForm orderForm = new KGHCashierPOS.OrderForm();
                            orderForm.Closed += (s, args) =>
                            {
                                AuthenticationRepository.LogoutUser();
                                this.Close();
                            };
                            orderForm.Show();
                            break;

                        default:
                            MessageBox.Show("Invalid user role.", "Access Denied",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Show();
                            ClearLoginFields();
                            break;
                    }
                }
                else
                {
                    // Failed login - already logged in AuthenticateUser
                    MessageBox.Show(result.Message, "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearLoginFields();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                btnSignIn.Enabled = true;
                MessageBox.Show($"Login error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                "Test Credentials (from database):\n" +
                "Super Admin: superadmin / super123\n" +
                "Admin: admin / admin123\n" +
                "Cashier: cashier / cashier123\n" +
                "Kiosk: kiosk / kiosk123",
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

        private void Form2_Load(object sender, EventArgs e) { }
    }
}