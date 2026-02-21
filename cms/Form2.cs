using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cms
{
    public partial class Form2 : Form
    {
        // Hardcoded credentials
        private const string HARDCODED_USERNAME = "ADMIN";
        private const string HARDCODED_PASSWORD = "ADMIN123";

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
            button1.Click += button1_Click;

            // Set Enter key to trigger login
            this.AcceptButton = button1;

            // Auto-focus on username field
            richTextBox1.Focus();

            // Change "Forgot Password?" to show test credentials
            label3.Text = "Test: ADMIN / ADMIN123";
            label3.ForeColor = Color.Gray;
            label3.Font = new System.Drawing.Font(label3.Font, System.Drawing.FontStyle.Italic);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = richTextBox1.Text.Trim();
            string password = richTextBox2.Text;

            // Check hardcoded credentials
            if (username.ToUpper() == HARDCODED_USERNAME && password == HARDCODED_PASSWORD)
            {
                // Login successful - set DialogResult to OK
                this.DialogResult = DialogResult.OK;
                this.Close(); // This will return to Program.cs
            }
            else
            {
                // Show error message
                MessageBox.Show("Invalid username or password!",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Clear password field
                richTextBox2.Text = "";
                richTextBox1.Focus();
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                richTextBox2.Focus();
                e.Handled = true;
            }
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(button1, new EventArgs());
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
    }
}