using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace cms
{
    public partial class Form2 : Form
    {
        // Hardcoded credentials - Staff and Admin credentials
        private const string STAFF_USERNAME = "staff";
        private const string STAFF_PASSWORD = "staff123";
        private const string ADMIN_USERNAME = "admin";
        private const string ADMIN_PASSWORD = "admin123";

        // Track password visibility state
        private bool isPasswordVisible = false;
        private string passwordText = ""; // Store actual password text

        // Add this field to store logged in username and role
        private string loggedInUsername = "";
        private string loggedInRole = "";

        public Form2()
        {
            InitializeComponent();

            // Setup the form after designer initialization
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "CMS - Login";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Wire up the button click event
            signInBtn.Click += signInBtn_Click;

            // Set Enter key to trigger login
            this.AcceptButton = signInBtn;

            // Auto-focus on username field
            usernameType.Focus();

            // Initialize password field
            passwordType.Text = "";
            passwordText = "";

            // Change "Forgot Password?" to show test credentials - Updated to show correct credentials
            label3.Text = "Staff: staff / staff123 | Admin: admin / admin123";
            label3.ForeColor = Color.Gray;
            label3.Font = new System.Drawing.Font(label3.Font, System.Drawing.FontStyle.Italic);

            // Setup show password icon click event
            showPassIcon.Click += showPassIcon_Click;
            showPassIcon.Cursor = Cursors.Hand;

            // Handle text changes for password field
            passwordType.TextChanged += passwordType_TextChanged;

            // Handle key events
            passwordType.KeyPress += passwordType_KeyPress;
            passwordType.KeyDown += passwordType_KeyDown;
        }

        // Add this method to expose the logged-in username
        public string GetLoggedInUsername()
        {
            return loggedInUsername;
        }

        // Add this method to expose the logged-in role
        public string GetLoggedInRole()
        {
            return loggedInRole;
        }

        private void passwordType_TextChanged(object sender, EventArgs e)
        {
            // When in hidden mode, we need to track the actual password
            if (!isPasswordVisible)
            {
                // Get the current cursor position
                int cursorPosition = passwordType.SelectionStart;

                // Temporarily remove event handler to avoid recursion
                passwordType.TextChanged -= passwordType_TextChanged;

                // If the text changed and we're in hidden mode, the user might have pasted or something
                // For simplicity, we'll just maintain the passwordText based on the displayed asterisks
                // This is not perfect but works for basic typing

                // Re-attach event handler
                passwordType.TextChanged += passwordType_TextChanged;
            }
        }

        private void passwordType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isPasswordVisible && !char.IsControl(e.KeyChar))
            {
                // Handle regular character input
                int cursorPos = passwordType.SelectionStart;

                // Insert character at cursor position
                passwordText = passwordText.Insert(cursorPos, e.KeyChar.ToString());

                // Show asterisk
                passwordType.Text = passwordType.Text + "•"; // Simplified for now

                // Move cursor to end
                passwordType.SelectionStart = passwordType.Text.Length;

                // Prevent the character from being added normally
                e.Handled = true;

                // Debug - show that passwordText is being updated
                Console.WriteLine("Password text: " + passwordText);
            }
        }

        private void passwordType_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isPasswordVisible)
            {
                if (e.KeyCode == Keys.Back)
                {
                    // Handle backspace
                    if (passwordText.Length > 0)
                    {
                        passwordText = passwordText.Substring(0, passwordText.Length - 1);

                        // Update display
                        passwordType.Text = new string('•', passwordText.Length);
                        passwordType.SelectionStart = passwordType.Text.Length;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    // Handle delete - simplified, just ignore for now
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    signInBtn_Click(signInBtn, new EventArgs());
                    e.Handled = true;
                }
            }
        }

        private void showPassIcon_Click(object sender, EventArgs e)
        {
            // Toggle password visibility
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Show actual password
                passwordType.Text = passwordText;
                showPassIcon.Text = "👁️"; // Open eye
                showPassIcon.ForeColor = Color.Blue;
            }
            else
            {
                // Show asterisks
                passwordType.Text = new string('•', passwordText.Length);
                showPassIcon.Text = "👁️‍🗨️"; // Closed eye
                showPassIcon.ForeColor = Color.Gray;
            }

            // Move cursor to end
            passwordType.SelectionStart = passwordType.Text.Length;

            // Debug
            Console.WriteLine("Toggle visibility: " + isPasswordVisible + ", Password: " + passwordText);
        }

        private void signInBtn_Click(object sender, EventArgs e)
        {
            string username = usernameType.Text.Trim();
            string password = passwordText; // Always use passwordText which stores the actual password

            // Debug
            Console.WriteLine("Login attempt - Username: " + username + ", Password: " + password);

            // Check credentials - Staff login (case insensitive username)
            if (username.ToLower() == STAFF_USERNAME && password == STAFF_PASSWORD)
            {
                loggedInUsername = username;
                loggedInRole = "Staff";

                // Show POS coming soon message for staff
                MessageBox.Show("POS is coming soon!",
                    "Point of Sale System",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Don't close the form - stay on login screen
                // Clear password field for next attempt
                passwordType.Text = "";
                passwordText = "";
                usernameType.Focus();
                usernameType.SelectAll();

                return;
            }
            // Check credentials - Admin login (case insensitive username)
            else if (username.ToLower() == ADMIN_USERNAME && password == ADMIN_PASSWORD)
            {
                // Store which user logged in
                loggedInUsername = username;
                loggedInRole = "Admin";

                // Login successful - set DialogResult to OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Show error message
                MessageBox.Show("Invalid username or password!",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Clear password field
                passwordType.Text = "";
                passwordText = "";
                usernameType.Focus();
                usernameType.SelectAll();
            }
        }

        private void usernameType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                passwordType.Focus();
                e.Handled = true;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Only show exit confirmation if login wasn't successful
            if (e.CloseReason == CloseReason.UserClosing && this.DialogResult != DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit?",
                    "Exit Application",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Your picture box click handler code here
        }

        private void usernameType_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void signInBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }
    }
}