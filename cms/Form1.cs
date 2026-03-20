using finaluserandstaff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class Form1 : Form
    {
        // Existing controls
        private GameRates gameRatesControl;
        private UserManagementControl userManagementControl;
        private UserControl2 dashboardControl;
        private SETTINGS settingsControl;
        private Activitylogs activityLogsControl;
        private bool isSigningOut = false;

        // New controls for Game Rates & Equipment tabs
        private TabControl gameTabControl;
        private TabPage tabRates;
        private TabPage tabEquipment;
        private Panel gameContentPanel;

        // ADD THIS: GameEquipment control reference
        private GameEquipment gameEquipmentControl;

        public string LoggedInUserRole { get; set; }

        // Helper class to store original colors
        private class LabelColors
        {
            public Color ForeColor { get; set; }
            public Color BackColor { get; set; }
        }

        private Dictionary<Label, LabelColors> originalLabelColors = new Dictionary<Label, LabelColors>();

        public Form1()
        {
            InitializeComponent();
            InitializeGameTabs(); // Initialize ONCE at startup

            // Set all menu labels to yellow color
            SetMenuLabelsToYellow();

            // Set initial state - highlight Dashboard by default
            HighlightMenuItem(dash);

            // Show dashboard by default when form loads
            ShowDashboard();

            // Connect the Sign Out label
            AttachSignOutEvent();

            // Add hover effects for menu labels
            AttachMenuHoverEffects();

            // Set the welcome message with logged in user role
            SetWelcomeMessage();
        }

        // Initialize the tab control ONCE at startup - NOT recreated each time
        private void InitializeGameTabs()
        {
            // Create the tab control (only once)
            gameTabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Visible = false
            };

            // Create Rates tab
            tabRates = new TabPage("Game Rates");

            // Create Equipment tab - with placeholder (will be replaced when clicked)
            tabEquipment = new TabPage("Game Equipment");

            // Add a click event to load equipment when tab is selected
            tabEquipment.Enter += TabEquipment_Enter;

            // Initial placeholder
            Label eqPlaceholder = new Label
            {
                Text = "Click to load Game Equipment",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray
            };
            tabEquipment.Controls.Add(eqPlaceholder);

            // Add tabs to tab control
            gameTabControl.TabPages.Add(tabRates);
            gameTabControl.TabPages.Add(tabEquipment);
        }

        // NEW: Event handler for when Equipment tab is selected
        private void TabEquipment_Enter(object sender, EventArgs e)
        {
            // Load GameEquipment control only when tab is clicked
            LoadGameEquipment();
        }

        // NEW: Method to load GameEquipment control
        private void LoadGameEquipment()
        {
            // Clear the tab first
            tabEquipment.Controls.Clear();

            // Create fresh GameEquipment control
            gameEquipmentControl = new GameEquipment();
            gameEquipmentControl.Dock = DockStyle.Fill;

            // Add to tab
            tabEquipment.Controls.Add(gameEquipmentControl);
        }

        private void SetWelcomeMessage()
        {
            if (loggedName != null)
            {
                if (!string.IsNullOrEmpty(LoggedInUserRole))
                {
                    loggedName.Text = $"Welcome, {LoggedInUserRole}!";
                }
                else
                {
                    loggedName.Text = "Welcome, Admin!";
                }

                if (LoggedInUserRole == "Super Admin")
                {
                    loggedName.ForeColor = Color.Gold;
                }
            }
        }

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
            var allLabels5 = this.Controls.Find("label5", true).OfType<Label>().ToList();

            if (allLabels5.Count > 0)
            {
                foreach (Label label5 in allLabels5)
                {
                    label5.Click += label5_Click;
                    label5.Cursor = Cursors.Hand;
                    label5.ForeColor = Color.FromArgb(40, 41, 34);
                    label5.Font = new System.Drawing.Font(label5.Font, FontStyle.Bold);
                    label5.MouseEnter += SignOut_MouseEnter;
                    label5.MouseLeave += SignOut_MouseLeave;
                }
            }
        }

        private void AttachMenuHoverEffects()
        {
            Label[] menuLabels = { dash, label1, label2, label3 };

            foreach (var label in menuLabels)
            {
                if (label != null)
                {
                    label.MouseEnter += MenuLabel_MouseEnter;
                    label.MouseLeave += MenuLabel_MouseLeave;
                    label.Cursor = Cursors.Hand;
                }
            }
        }

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
                    isSigningOut = true;
                    Form2 loginForm = new Form2();
                    this.Hide();

                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        string username = loginForm.GetLoggedInUsername();
                        SetLoggedInUser(username);
                        this.Show();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening login form: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isSigningOut = false;
                    this.Show();
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
            ShowGameRatesAndEquipment();
            HighlightMenuItem(label2);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ShowActivityLogs();
            HighlightMenuItem(label3);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            ShowSettings();
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

        // Shows both Game Rates and Equipment in tabs
        private void ShowGameRatesAndEquipment()
        {
            ClearPanel2();

            // Create a fresh GameRates control
            gameRatesControl = new GameRates();
            gameRatesControl.Dock = DockStyle.Fill;

            // Clear the Rates tab and add the new control
            tabRates.Controls.Clear();
            tabRates.Controls.Add(gameRatesControl);

            // Reset Equipment tab to placeholder (will load when clicked)
            tabEquipment.Controls.Clear();
            Label eqPlaceholder = new Label
            {
                Text = "Click to load Game Equipment",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray
            };
            tabEquipment.Controls.Add(eqPlaceholder);

            // Create container panel for the tab control (if needed)
            if (gameContentPanel == null || gameContentPanel.IsDisposed)
            {
                gameContentPanel = new Panel
                {
                    Dock = DockStyle.Fill
                };
            }
            else
            {
                gameContentPanel.Controls.Clear();
            }

            // Add the tab control to the container (ONCE)
            if (!gameContentPanel.Controls.Contains(gameTabControl))
            {
                gameContentPanel.Controls.Add(gameTabControl);
            }

            gameTabControl.Visible = true;

            // Add the container to panel2
            panel2.Controls.Add(gameContentPanel);
            panel2.Visible = true;
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

        // ClearPanel2 - DON'T dispose the tab control, just remove it from view
        private void ClearPanel2()
        {
            // Dispose existing controls (but NOT the tab control)
            if (dashboardControl != null && !dashboardControl.IsDisposed)
            {
                dashboardControl.Dispose();
                dashboardControl = null;
            }

            if (userManagementControl != null && !userManagementControl.IsDisposed)
            {
                userManagementControl.Dispose();
                userManagementControl = null;
            }

            if (gameRatesControl != null && !gameRatesControl.IsDisposed)
            {
                gameRatesControl.Dispose();
                gameRatesControl = null;
            }

            // ADD THIS: Dispose GameEquipment control
            if (gameEquipmentControl != null && !gameEquipmentControl.IsDisposed)
            {
                gameEquipmentControl.Dispose();
                gameEquipmentControl = null;
            }

            if (settingsControl != null && !settingsControl.IsDisposed)
            {
                settingsControl.Dispose();
                settingsControl = null;
            }

            if (activityLogsControl != null && !activityLogsControl.IsDisposed)
            {
                activityLogsControl.Dispose();
                activityLogsControl = null;
            }

            // Clear the Rates tab (but keep the tab control itself)
            if (tabRates != null && !tabRates.IsDisposed)
            {
                tabRates.Controls.Clear();
            }

            // Clear Equipment tab (will be reloaded when clicked)
            if (tabEquipment != null && !tabEquipment.IsDisposed)
            {
                tabEquipment.Controls.Clear();
            }

            // Remove the tab control from view but DON'T dispose it
            if (gameContentPanel != null && !gameContentPanel.IsDisposed)
            {
                gameContentPanel.Controls.Clear(); // Remove tab control from view
                gameContentPanel.Dispose();
                gameContentPanel = null;
            }

            // Hide the tab control
            if (gameTabControl != null && !gameTabControl.IsDisposed)
            {
                gameTabControl.Visible = false;
            }

            // Clear panel2
            panel2.Controls.Clear();
        }

        // ==============================================
        // MENU LABELS YELLOW COLOR SETUP
        // ==============================================

        private void SetMenuLabelsToYellow()
        {
            Color yellowColor = Color.FromArgb(228, 186, 94);

            if (dash != null)
            {
                dash.ForeColor = yellowColor;
                originalLabelColors[dash] = new LabelColors { ForeColor = yellowColor, BackColor = dash.BackColor };
            }

            if (label1 != null)
            {
                label1.ForeColor = yellowColor;
                originalLabelColors[label1] = new LabelColors { ForeColor = yellowColor, BackColor = label1.BackColor };
            }

            if (label2 != null)
            {
                label2.ForeColor = yellowColor;
                originalLabelColors[label2] = new LabelColors { ForeColor = yellowColor, BackColor = label2.BackColor };
            }

            if (label3 != null)
            {
                label3.ForeColor = yellowColor;
                originalLabelColors[label3] = new LabelColors { ForeColor = yellowColor, BackColor = label3.BackColor };
            }
        }

        // ==============================================
        // MENU HIGHLIGHTING METHODS
        // ==============================================

        private void HighlightMenuItem(Label clickedLabel)
        {
            StoreOriginalColors();
            ResetMenuColors();

            if (clickedLabel != null)
            {
                clickedLabel.BackColor = Color.FromArgb(40, 50, 60);
                clickedLabel.ForeColor = Color.White;
            }
        }

        private void StoreOriginalColors()
        {
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
            Color yellowColor = Color.FromArgb(228, 186, 94);
            Color defaultBackColor = Color.Transparent;

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

        private void MenuLabel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                if (label.BackColor != Color.FromArgb(40, 50, 60))
                {
                    if (!originalLabelColors.ContainsKey(label))
                    {
                        originalLabelColors[label] = new LabelColors
                        {
                            ForeColor = label.ForeColor,
                            BackColor = label.BackColor
                        };
                    }

                    label.ForeColor = Color.White;
                    label.BackColor = Color.FromArgb(60, 70, 80);
                }
            }
        }

        private void MenuLabel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                if (label.BackColor != Color.FromArgb(40, 50, 60))
                {
                    if (originalLabelColors.ContainsKey(label))
                    {
                        label.ForeColor = originalLabelColors[label].ForeColor;
                        label.BackColor = originalLabelColors[label].BackColor;
                    }
                    else
                    {
                        label.ForeColor = Color.FromArgb(228, 186, 94);
                        label.BackColor = Color.Transparent;
                    }
                }
            }
        }

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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSigningOut)
            {
                return;
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
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

        private void Form1_Load(object sender, EventArgs e)
        {
            // Your existing Form1_Load code
        }

        private void date_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dddd, MMMM dd, yyyy");

            MessageBox.Show($"Current date and time:\n{formattedDate}\n{currentDate.ToString("hh:mm:ss tt")}",
                            "Date Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            if (sender is Label dateLabel)
            {
                dateLabel.Text = formattedDate;
                dateLabel.ForeColor = Color.Blue;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Your existing label8_Click code
        }
    }
}