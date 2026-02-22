using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Add this using directive
using finaluserandstaff;

namespace cms
{
    public partial class Form1 : Form
    {
        private GameRates gameRatesControl;
        private UserManagementControl userManagementControl;
        private UserControl2 dashboardControl;
        private SETTINGS settingsControl; // Add settings control reference
        private Activitylogs activityLogsControl; // Add Activitylogs control reference
        private bool isSigningOut = false; // Add this flag

        // Add property to store logged in user role
        public string LoggedInUserRole { get; set; }

        // Helper class to store original colors
        private class LabelColors
        {
            public Color ForeColor { get; set; }
            public Color BackColor { get; set; }
        }

        // Dictionary to store original colors for each label
        private Dictionary<Label, LabelColors> originalLabelColors = new Dictionary<Label, LabelColors>();

        public Form1()
        {
            InitializeComponent();

            // Set all menu labels to yellow color
            SetMenuLabelsToYellow();

            // Set initial state - highlight Dashboard by default
            HighlightMenuItem(dash);

            // Show dashboard by default when form loads
            ShowDashboard();

            // AFTER InitializeComponent(), connect the Sign Out label
            AttachSignOutEvent();

            // Add hover effects for menu labels
            AttachMenuHoverEffects();

            // Set the welcome message with logged in user role
            SetWelcomeMessage();
        }

        // New method to set the welcome message based on logged in user
        private void SetWelcomeMessage()
        {
            if (loggedName != null)
            {
                // Check if we have a role passed from Form2
                if (!string.IsNullOrEmpty(LoggedInUserRole))
                {
                    loggedName.Text = $"Welcome, {LoggedInUserRole}!";
                }
                else
                {
                    // Default fallback
                    loggedName.Text = "Welcome, Admin!";
                }

                // Optional: Change color based on role
                if (LoggedInUserRole == "Super Admin")
                {
                    loggedName.ForeColor = Color.Gold; // Special color for Super Admin
                }
            }
        }

        // Method to pass login info from Form2
        public void SetLoggedInUser(string username)
        {
            if (username.ToUpper() == "SUPERADMIN")
            {
                LoggedInUserRole = "Super Admin";
            }
            else
            {
                LoggedInUserRole = "Admin";
            }

            SetWelcomeMessage();
        }

        private void AttachSignOutEvent()
        {
            // Find ALL label5 controls in the form
            var allLabels5 = this.Controls.Find("label5", true).OfType<Label>().ToList();

            if (allLabels5.Count > 0)
            {
                foreach (Label label5 in allLabels5)
                {
                    label5.Click += label5_Click;
                    label5.Cursor = Cursors.Hand;

                    // Make it look clickable
                    label5.ForeColor = Color.FromArgb(40, 41, 34);
                    label5.Font = new System.Drawing.Font(label5.Font, FontStyle.Bold);

                    // Also add mouse enter/leave events for hover effect
                    label5.MouseEnter += SignOut_MouseEnter;
                    label5.MouseLeave += SignOut_MouseLeave;
                }
            }
            else
            {
                // If no label5 found, check if there's a different name for the sign out label
                MessageBox.Show("Sign Out label not found. Please check if label5 exists in the designer.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AttachMenuHoverEffects()
        {
            // Add hover effects for all menu labels
            Label[] menuLabels = { dash, label1, label2, label3 };

            foreach (var label in menuLabels)
            {
                if (label != null)
                {
                    label.MouseEnter += MenuLabel_MouseEnter;
                    label.MouseLeave += MenuLabel_MouseLeave;
                    label.Cursor = Cursors.Hand; // Make them look clickable
                }
            }
        }

        // Fixed label5 click event - This should close Form1 and open Form2
        private void label5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to sign out?",
                "Sign Out Verification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Set flag to indicate we're signing out (not closing the app)
                    isSigningOut = true;

                    // Show new login form FIRST
                    Form2 loginForm = new Form2();

                    // IMPORTANT: Hide current form first
                    this.Hide();

                    // Show the login form
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // Get the username from Form2
                        string username = loginForm.GetLoggedInUsername();

                        // Update the welcome message
                        SetLoggedInUser(username);

                        // Show this form again
                        this.Show();
                    }
                    else
                    {
                        // Login was cancelled or failed - exit application
                        Application.Exit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening login form: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isSigningOut = false; // Reset flag
                    this.Show(); // Show current form again
                }
            }
        }

        // ==============================================
        // MENU FUNCTIONALITY
        // ==============================================

        private void dash_Click(object sender, EventArgs e)
        {
            ShowDashboard();
            HighlightMenuItem(dash);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            ShowUserManagement();
            HighlightMenuItem(label1);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ShowGameRates();
            HighlightMenuItem(label2);
        }

        // Activity Log click handler - UPDATED
        private void label3_Click(object sender, EventArgs e)
        {
            ShowActivityLogs();
            HighlightMenuItem(label3);
        }

        // Settings click handler - UPDATED
        private void label6_Click(object sender, EventArgs e)
        {
            ShowSettings();

            // Settings is not in the main menu, so we don't highlight it
            // But we can reset all menu highlights
            ResetMenuColors();
        }

        // ==============================================
        // SHOW CONTROL METHODS
        // ==============================================

        private void ShowDashboard()
        {
            ClearPanel2();

            dashboardControl = new UserControl2();
            dashboardControl.Dock = DockStyle.Fill;

            panel2.Controls.Add(dashboardControl);
            panel2.Visible = true;
        }

        private void ShowUserManagement()
        {
            ClearPanel2();

            userManagementControl = new UserManagementControl();
            userManagementControl.Dock = DockStyle.Fill;

            panel2.Controls.Add(userManagementControl);
            panel2.Visible = true;
        }

        private void ShowGameRates()
        {
            ClearPanel2();

            try
            {
                gameRatesControl = new GameRates();
                gameRatesControl.Dock = DockStyle.Fill;

                panel2.Controls.Add(gameRatesControl);
                panel2.Visible = true;
            }
            catch (Exception)
            {
                Label placeholder = new Label();
                placeholder.Text = "GAME RATES\n\nFeature coming soon...";
                placeholder.Font = new System.Drawing.Font("Segoe UI", 16, FontStyle.Bold);
                placeholder.Dock = DockStyle.Fill;
                placeholder.TextAlign = ContentAlignment.MiddleCenter;
                placeholder.ForeColor = Color.Gray;
                panel2.Controls.Add(placeholder);
            }
        }

        private void ShowActivityLogs()
        {
            ClearPanel2();

            try
            {
                activityLogsControl = new Activitylogs();
                activityLogsControl.Dock = DockStyle.Fill;

                panel2.Controls.Add(activityLogsControl);
                panel2.Visible = true;
            }
            catch (Exception ex)
            {
                // Fallback in case of error
                MessageBox.Show($"Error loading Activity Logs: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Label placeholder = new Label();
                placeholder.Text = "ACTIVITY LOG\n\nError loading activity logs.\n" + ex.Message;
                placeholder.Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold);
                placeholder.Dock = DockStyle.Fill;
                placeholder.TextAlign = ContentAlignment.MiddleCenter;
                placeholder.ForeColor = Color.Red;
                panel2.Controls.Add(placeholder);
            }
        }

        private void ShowSettings()
        {
            ClearPanel2();

            settingsControl = new SETTINGS();
            settingsControl.Dock = DockStyle.Fill;

            panel2.Controls.Add(settingsControl);
            panel2.Visible = true;
        }

        private void ClearPanel2()
        {
            // Dispose existing controls if needed
            if (dashboardControl != null)
            {
                dashboardControl.Dispose();
                dashboardControl = null;
            }

            if (userManagementControl != null)
            {
                userManagementControl.Dispose();
                userManagementControl = null;
            }

            if (gameRatesControl != null)
            {
                gameRatesControl.Dispose();
                gameRatesControl = null;
            }

            if (settingsControl != null)
            {
                settingsControl.Dispose();
                settingsControl = null;
            }

            if (activityLogsControl != null)
            {
                activityLogsControl.Dispose();
                activityLogsControl = null;
            }

            panel2.Controls.Clear();
        }

        // ==============================================
        // MENU LABELS YELLOW COLOR SETUP
        // ==============================================

        private void SetMenuLabelsToYellow()
        {
            Color yellowColor = Color.FromArgb(228, 186, 94); // Your yellow color

            if (dash != null)
            {
                dash.ForeColor = yellowColor;
                originalLabelColors[dash] = new LabelColors
                {
                    ForeColor = yellowColor,
                    BackColor = dash.BackColor
                };
            }

            if (label1 != null)
            {
                label1.ForeColor = yellowColor;
                originalLabelColors[label1] = new LabelColors
                {
                    ForeColor = yellowColor,
                    BackColor = label1.BackColor
                };
            }

            if (label2 != null)
            {
                label2.ForeColor = yellowColor;
                originalLabelColors[label2] = new LabelColors
                {
                    ForeColor = yellowColor,
                    BackColor = label2.BackColor
                };
            }

            if (label3 != null)
            {
                label3.ForeColor = yellowColor;
                originalLabelColors[label3] = new LabelColors
                {
                    ForeColor = yellowColor,
                    BackColor = label3.BackColor
                };
            }
        }

        // ==============================================
        // MENU HIGHLIGHTING METHODS
        // ==============================================

        private void HighlightMenuItem(Label clickedLabel)
        {
            // Store original colors for all labels before resetting
            StoreOriginalColors();

            // Reset all menu items to default colors first
            ResetMenuColors();

            // Highlight the clicked menu item with full width
            if (clickedLabel != null)
            {
                clickedLabel.BackColor = Color.FromArgb(40, 50, 60); // Dark blue highlight color
                clickedLabel.ForeColor = Color.White;
            }
        }

        private void StoreOriginalColors()
        {
            // Store original colors for menu labels
            Label[] menuLabels = { dash, label1, label2, label3 };

            foreach (var label in menuLabels)
            {
                if (label != null && !originalLabelColors.ContainsKey(label))
                {
                    originalLabelColors[label] = new LabelColors
                    {
                        ForeColor = label.ForeColor,
                        BackColor = label.BackColor
                    };
                }
            }
        }

        private void ResetMenuColors()
        {
            Color yellowColor = Color.FromArgb(228, 186, 94); // Yellow color
            Color defaultBackColor = Color.Transparent;

            // Reset all menu labels
            if (dash != null)
            {
                if (originalLabelColors.ContainsKey(dash))
                {
                    dash.ForeColor = originalLabelColors[dash].ForeColor;
                    dash.BackColor = originalLabelColors[dash].BackColor;
                }
                else
                {
                    dash.ForeColor = yellowColor;
                    dash.BackColor = defaultBackColor;
                }
            }

            if (label1 != null)
            {
                if (originalLabelColors.ContainsKey(label1))
                {
                    label1.ForeColor = originalLabelColors[label1].ForeColor;
                    label1.BackColor = originalLabelColors[label1].BackColor;
                }
                else
                {
                    label1.ForeColor = yellowColor;
                    label1.BackColor = defaultBackColor;
                }
            }

            if (label2 != null)
            {
                if (originalLabelColors.ContainsKey(label2))
                {
                    label2.ForeColor = originalLabelColors[label2].ForeColor;
                    label2.BackColor = originalLabelColors[label2].BackColor;
                }
                else
                {
                    label2.ForeColor = yellowColor;
                    label2.BackColor = defaultBackColor;
                }
            }

            if (label3 != null)
            {
                if (originalLabelColors.ContainsKey(label3))
                {
                    label3.ForeColor = originalLabelColors[label3].ForeColor;
                    label3.BackColor = originalLabelColors[label3].BackColor;
                }
                else
                {
                    label3.ForeColor = yellowColor;
                    label3.BackColor = defaultBackColor;
                }
            }
        }

        // ==============================================
        // HOVER EFFECT METHODS
        // ==============================================

        // HOVER EFFECTS FOR MENU LABELS (dash, label1, label2, label3)
        private void MenuLabel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                // Only change color if not currently highlighted
                if (label.BackColor != Color.FromArgb(40, 50, 60))
                {
                    // Store original colors for this specific label
                    if (!originalLabelColors.ContainsKey(label))
                    {
                        originalLabelColors[label] = new LabelColors
                        {
                            ForeColor = label.ForeColor,
                            BackColor = label.BackColor
                        };
                    }

                    // Change to hover colors
                    label.ForeColor = Color.White;
                    label.BackColor = Color.FromArgb(60, 70, 80); // Slightly lighter dark gray for hover
                }
            }
        }

        private void MenuLabel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                // Only restore original colors if not highlighted
                if (label.BackColor != Color.FromArgb(40, 50, 60))
                {
                    // Restore to original colors
                    if (originalLabelColors.ContainsKey(label))
                    {
                        label.ForeColor = originalLabelColors[label].ForeColor;
                        label.BackColor = originalLabelColors[label].BackColor;
                    }
                    else
                    {
                        // Fallback to default colors
                        label.ForeColor = Color.FromArgb(228, 186, 94); // Yellow
                        label.BackColor = Color.Transparent;
                    }
                }
            }
        }

        // HOVER EFFECTS FOR SIGN OUT LABEL
        private void SignOut_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                label.ForeColor = Color.Red;
            }
        }

        private void SignOut_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                label.ForeColor = Color.FromArgb(40, 41, 34);
            }
        }

        // HOVER EFFECTS FOR SETTINGS LABEL
        private void Settings_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                label.ForeColor = Color.Blue;
            }
        }

        private void Settings_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                label.ForeColor = Color.FromArgb(40, 41, 34);
            }
        }

        // ==============================================
        // FORM EVENT HANDLERS
        // ==============================================

        // Handle Form1 closing to ensure proper application exit
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if we're signing out - if yes, just close without asking
            if (isSigningOut)
            {
                return; // Just close the form, don't ask about exiting application
            }

            // Only ask about exiting if user is closing the form directly (not signing out)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Ask if user wants to exit the entire application
                DialogResult result = MessageBox.Show("Do you want to exit the application?",
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

        // ==============================================
        // ADDITIONAL HELPER METHODS
        // ==============================================

        // Method to get current active control (useful for debugging)
        public string GetCurrentControl()
        {
            if (panel2.Controls.Count > 0)
            {
                return panel2.Controls[0].GetType().Name;
            }
            return "None";
        }

        // Method to refresh all controls (if needed)
        public void RefreshAllControls()
        {
            if (dashboardControl != null && dashboardControl.Visible)
            {
                // Refresh dashboard if needed
            }

            if (userManagementControl != null && userManagementControl.Visible)
            {
                userManagementControl.Refresh(); // If you have a refresh method
            }

            if (gameRatesControl != null && gameRatesControl.Visible)
            {
                gameRatesControl.Refresh(); // If you have a refresh method
            }

            if (settingsControl != null && settingsControl.Visible)
            {
                // Settings might need to reload from file
            }

            if (activityLogsControl != null && activityLogsControl.Visible)
            {
                // Activity logs might need to refresh data
                activityLogsControl.Refresh();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void date_Click(object sender, EventArgs e)
        {
            // Get current date and time
            DateTime currentDate = DateTime.Now;

            // Format options:
            string formattedDate = currentDate.ToString("dddd, MMMM dd, yyyy");
            // or: currentDate.ToString("MM/dd/yyyy")
            // or: currentDate.ToString("dd-MMM-yyyy")
            // or: currentDate.ToString("yyyy-MM-dd")

            // Show the date
            MessageBox.Show($"Current date and time:\n{formattedDate}\n{currentDate.ToString("hh:mm:ss tt")}",
                            "Date Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            // If you have a label or textbox to display the date, update it:
            // For example, if you have a label named "dateLabel":
            // dateLabel.Text = formattedDate;

            // If the sender is a control that shows the date, update it:
            if (sender is Label dateLabel)
            {
                dateLabel.Text = formattedDate;
                dateLabel.ForeColor = Color.Blue; // Optional: change color to indicate it's updated
            }
            else if (sender is Button dateButton)
            {
                dateButton.Text = formattedDate;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}