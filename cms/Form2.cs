using System;
using System.Drawing;
using System.Windows.Forms;

namespace cms
{
    public partial class Form2 : Form
    {
        private bool isPasswordVisible = false;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Set default focus to username field
            txtUsername.Focus();

            // Add Enter key event handlers
            txtUsername.KeyPress += TxtUsername_KeyPress;
            txtPassword.KeyPress += TxtPassword_KeyPress;
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If Enter key is pressed, move focus to password field
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If Enter key is pressed, trigger login
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSignIn.PerformClick();
                e.Handled = true;
            }
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            // Clear previous error message
            ClearError();

            // Validate username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Please enter username");
                txtUsername.Focus();
                return;
            }

            // Validate password
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Please enter password");
                txtPassword.Focus();
                return;
            }

            // Authentication logic - Replace with your actual database validation
            if (ValidateCredentials(txtUsername.Text.Trim(), txtPassword.Text))
            {
                // Login successful - proceed to main form
                PerformLogin();
            }
            else
            {
                ShowError("Invalid username or password");
                txtPassword.Clear();
                txtPassword.Focus();

                // Reset password visibility toggle if password was visible
                if (isPasswordVisible)
                {
                    btnShowPassword_Click(null, null);
                }
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            // TODO: Replace this with your actual authentication logic
            // Example: Check against database, Active Directory, etc.

            // Temporary demo credentials
            if (username == "admin" && password == "admin123")
            {
                return true;
            }

            if (username == "user" && password == "user123")
            {
                return true;
            }

            // Add your actual validation logic here
            // return DatabaseHelper.ValidateUser(username, password);

            return false;
        }

        private void PerformLogin()
        {
            // Store login information if needed
            string loggedInUser = txtUsername.Text.Trim();

            // Optional: Save to session or settings
            // Properties.Settings.Default.LastLoggedInUser = loggedInUser;
            // Properties.Settings.Default.Save();

            // Hide login form
            this.Hide();

            // TODO: Open your main application form
            // Replace with your actual main form
            Form1 mainForm = new Form1();
            mainForm.Show();

            // Close login form when main form closes (optional)
            mainForm.FormClosed += (s, args) => this.Close();
        }

        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Show password
                txtPassword.UseSystemPasswordChar = false;
                btnShowPassword.Text = "👁‍🗨";
                btnShowPassword.ForeColor = Color.FromArgb(228, 186, 94);
                btnShowPassword.BackColor = Color.FromArgb(60, 61, 58);
            }
            else
            {
                // Hide password
                txtPassword.UseSystemPasswordChar = true;
                btnShowPassword.Text = "👁";
                btnShowPassword.ForeColor = Color.FromArgb(180, 180, 180);
                btnShowPassword.BackColor = Color.FromArgb(40, 41, 38);
            }

            // Maintain focus on password field
            txtPassword.Focus();
            txtPassword.SelectionStart = txtPassword.Text.Length;
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            // Implement forgot password functionality
            DialogResult result = MessageBox.Show(
                "Password recovery will be sent to your registered email address.\n\nWould you like to proceed?",
                "Forgot Password",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // TODO: Implement password recovery logic
                MessageBox.Show("Please contact your system administrator for password assistance.",
                    "Password Recovery",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit the application?",
                "Exit Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ShowError(string message)
        {
            lblErrorMessage.Text = message;
            lblErrorMessage.Visible = true;
            lblErrorMessage.ForeColor = Color.FromArgb(255, 100, 100);

            // Auto-clear error after 5 seconds
            Timer errorTimer = new Timer();
            errorTimer.Interval = 5000;
            errorTimer.Tick += (s, ev) =>
            {
                ClearError();
                errorTimer.Stop();
                errorTimer.Dispose();
            };
            errorTimer.Start();
        }

        private void ClearError()
        {
            lblErrorMessage.Text = "";
            lblErrorMessage.Visible = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ClearError();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Add hover effects for buttons
            btnSignIn.MouseEnter += (s, ev) =>
            {
                btnSignIn.BackColor = Color.FromArgb(248, 206, 114);
                btnSignIn.Cursor = Cursors.Hand;
            };

            btnSignIn.MouseLeave += (s, ev) =>
            {
                btnSignIn.BackColor = Color.FromArgb(228, 186, 94);
                btnSignIn.Cursor = Cursors.Default;
            };

            btnShowPassword.MouseEnter += (s, ev) =>
            {
                btnShowPassword.Cursor = Cursors.Hand;
                if (!isPasswordVisible)
                {
                    btnShowPassword.BackColor = Color.FromArgb(60, 61, 58);
                }
            };

            btnShowPassword.MouseLeave += (s, ev) =>
            {
                if (!isPasswordVisible)
                {
                    btnShowPassword.BackColor = Color.FromArgb(40, 41, 38);
                }
            };

            lblForgotPassword.MouseEnter += (s, ev) =>
            {
                lblForgotPassword.ForeColor = Color.FromArgb(228, 186, 94);
                lblForgotPassword.Cursor = Cursors.Hand;
            };

            lblForgotPassword.MouseLeave += (s, ev) =>
            {
                lblForgotPassword.ForeColor = Color.FromArgb(140, 140, 140);
                lblForgotPassword.Cursor = Cursors.Default;
            };

            btnExit.MouseEnter += (s, ev) =>
            {
                btnExit.ForeColor = Color.FromArgb(228, 186, 94);
                btnExit.Cursor = Cursors.Hand;
            };

            btnExit.MouseLeave += (s, ev) =>
            {
                btnExit.ForeColor = Color.FromArgb(120, 120, 120);
                btnExit.Cursor = Cursors.Default;
            };
        }

        public void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            ClearError();

            if (isPasswordVisible)
            {
                btnShowPassword_Click(null, null);
            }

            txtUsername.Focus();
        }

        public void SetTestCredentials(string username, string password)
        {
            txtUsername.Text = username;
            txtPassword.Text = password;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to exit?",
                    "Exit Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}