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
        private SALES salesControl;
        private bool isSigningOut = false;

        // New controls for Game Rates & Equipment tabs
        private TabControl gameTabControl;
        private TabPage tabRates;
        private TabPage tabEquipment;
        private Panel gameContentPanel;

        // GameEquipment control reference
        private GameEquipment gameEquipmentControl;

        // User info properties
        public string LoggedInUserRole { get; set; } = "ADMIN";
        public string LoggedInUsername { get; set; } = "admin";

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
            InitializeGameTabs();

            // Set all menu labels to yellow color
            SetMenuLabelsToYellow();

            // Set initial state - highlight Dashboard by default
            HighlightMenuItem(dash);

            // Show dashboard by default when form loads
            ShowDashboard();

            // Connect the Sign Out event to pictureBox2
            AttachSignOutEvent();

            // Connect Settings event to pictureBox3
            AttachSettingsEvent();

            // Add hover effects and click events for menu labels
            AttachMenuHoverEffects();
            AttachMenuClickEvents();

            // Add hover effects for pictureBox2 (Sign Out) and pictureBox3 (Settings)
            AttachPictureBoxHoverEffects();

            // Set the welcome message
            SetWelcomeMessage();

            // Apply role-based access control
            ApplyRoleBasedAccess();

            // Hide the old label5 and label6
            HideOldControls();

            // Reposition icons to top right corner
            RepositionTopIcons();

            // Initialize Activity Logs
            InitializeActivityLogs();
        }

        // Initialize Activity Logs and set current user
        private void InitializeActivityLogs()
        {
            try
            {
                Activitylogs.EnsureInitialized();

                // Set global logger with current user info
                if (!string.IsNullOrEmpty(LoggedInUsername))
                {
                    GlobalLogger.CurrentUsername = LoggedInUsername;
                    GlobalLogger.CurrentUserRole = LoggedInUserRole ?? "ADMIN";

                    // Log that admin form was loaded
                    GlobalLogger.LogInfo("System", $"Admin/Manager form loaded by {LoggedInUsername}");
                }

                System.Diagnostics.Debug.WriteLine("Activity Logs initialized in Form1!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialize Activity Logs: {ex.Message}");
            }
        }

        // Method to set user info (called from login)
        public void SetCurrentUser(string username, string role)
        {
            LoggedInUsername = username;
            LoggedInUserRole = role;

            // Update global logger
            GlobalLogger.CurrentUsername = username;
            GlobalLogger.CurrentUserRole = role ?? "ADMIN";

            SetWelcomeMessage();
            ApplyRoleBasedAccess();

            // Log the session start
            try
            {
                GlobalLogger.LogInfo("System", $"User session started: {username} ({role})");
            }
            catch { }
        }

        // Apply role-based access control
        private void ApplyRoleBasedAccess()
        {
            // Convert role to uppercase for comparison
            string role = (LoggedInUserRole ?? "").ToUpper();

            // Admin and Manager have full access
            bool isAdminOrManager = (role == "ADMIN" || role == "MANAGER");

            // Cashier have limited access
            bool isCashier = (role == "CASHIER");

            // Hide/show picture boxes based on role
            if (pictureBox4 != null) pictureBox4.Visible = isAdminOrManager;
            if (pictureBox5 != null) pictureBox5.Visible = isAdminOrManager;
            if (pictureBox6 != null) pictureBox6.Visible = isAdminOrManager;
            if (pictureBox7 != null) pictureBox7.Visible = isAdminOrManager;
            if (pictureBox8 != null) pictureBox8.Visible = isAdminOrManager;

            if (isCashier)
            {
                // Hide or disable admin-only menus for cashier
                if (label1 != null) // User Management
                {
                    label1.Visible = false;
                    label1.Enabled = false;
                }

                if (label3 != null) // Activity Logs
                {
                    label3.Visible = false;
                    label3.Enabled = false;
                }

                // Hide settings pictureBox for cashier
                if (pictureBox3 != null)
                {
                    pictureBox3.Visible = false;
                    pictureBox3.Enabled = false;
                }

                // Cashier can only see Dashboard and Game Rates/Equipment
                ShowDashboard();
                HighlightMenuItem(dash);

                // Log role-based restriction
                try
                {
                    GlobalLogger.LogInfo("System", $"Cashier access: Limited view applied for {LoggedInUsername}");
                }
                catch { }
            }
            else if (isAdminOrManager)
            {
                // Show all menus for admin/manager
                if (label1 != null)
                {
                    label1.Visible = true;
                    label1.Enabled = true;
                }

                if (label3 != null)
                {
                    label3.Visible = true;
                    label3.Enabled = true;
                }

                // Show settings pictureBox for admin/manager
                if (pictureBox3 != null)
                {
                    pictureBox3.Visible = true;
                    pictureBox3.Enabled = true;
                }
            }
        }

        // Hide the old label5 (Sign Out) and label6 (Settings)
        private void HideOldControls()
        {
            if (label5 != null)
            {
                label5.Visible = false;
                label5.Enabled = false;
            }

            if (label6 != null)
            {
                label6.Visible = false;
                label6.Enabled = false;
            }
        }

        // Reposition the icons to top right corner
        private void RepositionTopIcons()
        {
            // Move pictureBox2 (Sign Out) to top right
            if (pictureBox2 != null)
            {
                pictureBox2.Location = new Point(this.ClientSize.Width - 50, 15);
                pictureBox2.Size = new Size(35, 35);
                pictureBox2.BackColor = Color.Transparent;
                pictureBox2.Cursor = Cursors.Hand;
            }

            // Move pictureBox3 (Settings) to left of sign out
            if (pictureBox3 != null)
            {
                pictureBox3.Location = new Point(this.ClientSize.Width - 95, 15);
                pictureBox3.Size = new Size(35, 35);
                pictureBox3.BackColor = Color.Transparent;
                pictureBox3.Cursor = Cursors.Hand;
            }
        }

        // Initialize the tab control ONCE at startup
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

        private void TabEquipment_Enter(object sender, EventArgs e)
        {
            LoadGameEquipment();

            // Log equipment tab access
            try
            {
                GlobalLogger.LogInfo("GameEquipment", $"Equipment tab accessed by {LoggedInUsername}");
            }
            catch { }
        }

        private void LoadGameEquipment()
        {
            tabEquipment.Controls.Clear();
            gameEquipmentControl = new GameEquipment();
            gameEquipmentControl.Dock = DockStyle.Fill;
            tabEquipment.Controls.Add(gameEquipmentControl);
        }

        private void SetWelcomeMessage()
        {
            if (loggedName != null)
            {
                if (!string.IsNullOrEmpty(LoggedInUsername))
                {
                    string roleDisplay = !string.IsNullOrEmpty(LoggedInUserRole) ?
                        $"{LoggedInUserRole}" : "User";

                    loggedName.Text = $"Welcome, {LoggedInUsername} ({roleDisplay})!";
                }
                else
                {
                    loggedName.Text = "Welcome, Admin!";
                }

                // Set color based on role
                string role = (LoggedInUserRole ?? "").ToUpper();

                if (role == "ADMIN")
                {
                    loggedName.ForeColor = Color.Gold;
                }
                else if (role == "MANAGER")
                {
                    loggedName.ForeColor = Color.FromArgb(100, 200, 200);
                }
                else if (role == "CASHIER")
                {
                    loggedName.ForeColor = Color.FromArgb(100, 200, 100);
                }
                else
                {
                    loggedName.ForeColor = Color.FromArgb(228, 186, 94);
                }
            }

            if (label9 != null)
            {
                // Update role display
                if (!string.IsNullOrEmpty(LoggedInUserRole))
                {
                    label9.Text = LoggedInUserRole;
                }
            }
        }

        private void AttachSignOutEvent()
        {
            // Attach click event to pictureBox2 for Sign Out
            if (pictureBox2 != null)
            {
                pictureBox2.Click += PictureBox2_Click;
                pictureBox2.Cursor = Cursors.Hand;
            }
        }

        private void AttachSettingsEvent()
        {
            // Attach click event to pictureBox3 for Settings
            if (pictureBox3 != null)
            {
                pictureBox3.Click += PictureBox3_Click;
                pictureBox3.Cursor = Cursors.Hand;
            }
        }

        private void AttachPictureBoxHoverEffects()
        {
            // Hover effects for pictureBox2 (Sign Out)
            if (pictureBox2 != null)
            {
                pictureBox2.MouseEnter += (s, e) => pictureBox2.BackColor = Color.FromArgb(60, 70, 80);
                pictureBox2.MouseLeave += (s, e) => pictureBox2.BackColor = Color.Transparent;
            }

            // Hover effects for pictureBox3 (Settings)
            if (pictureBox3 != null)
            {
                pictureBox3.MouseEnter += (s, e) => pictureBox3.BackColor = Color.FromArgb(60, 70, 80);
                pictureBox3.MouseLeave += (s, e) => pictureBox3.BackColor = Color.Transparent;
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            // Sign Out
            DialogResult result = MessageBox.Show("Are you sure you want to sign out?",
                "Sign Out Verification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Log sign out
                    try
                    {
                        GlobalLogger.Log("Logout", $"User '{LoggedInUsername}' signed out", "Info", "System");
                        Activitylogs.Instance?.AddLogEntry(LoggedInUsername, "Logout", $"User '{LoggedInUsername}' logged out of the system", "Info", "System");
                    }
                    catch { }

                    isSigningOut = true;

                    // Close the current form
                    this.Close();

                    // Create a new login form
                    Form2 loginForm = new Form2();
                    loginForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error signing out: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isSigningOut = false;
                }
            }
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            // Check if user has permission to access Settings
            string role = (LoggedInUserRole ?? "").ToUpper();
            if (role == "CASHIER")
            {
                MessageBox.Show("Access Denied! Cashier cannot access Settings.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Log access denied
                try
                {
                    GlobalLogger.LogWarning("System", $"Access denied: {LoggedInUsername} attempted to access Settings");
                }
                catch { }
                return;
            }

            // Log settings access
            try
            {
                GlobalLogger.LogInfo("System", $"{LoggedInUsername} opened Settings");
            }
            catch { }

            ShowSettings();
            HighlightMenuItem(null);
        }

        private void AttachMenuHoverEffects()
        {
            Label[] menuLabels = { dash, label1, label2, label3, salesBtn };

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

        private void AttachMenuClickEvents()
        {
            if (dash != null) dash.Click += dash_Click;
            if (label1 != null) label1.Click += label1_Click_1;
            if (label2 != null) label2.Click += label2_Click;
            if (label3 != null) label3.Click += label3_Click;
            if (salesBtn != null) salesBtn.Click += salesBtn_Click;
        }

        // ==============================================
        // MENU FUNCTIONALITY
        // ==============================================

        private void dash_Click(object sender, EventArgs e)
        {
            ShowDashboard();
            HighlightMenuItem(dash);
            dash.Refresh();

            // Log dashboard access
            try
            {
                GlobalLogger.LogInfo("Dashboard", $"{LoggedInUsername} viewed dashboard");
            }
            catch { }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            string role = (LoggedInUserRole ?? "").ToUpper();
            if (role == "CASHIER")
            {
                MessageBox.Show("Access Denied! Cashier cannot access User Management.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Log access denied
                try
                {
                    GlobalLogger.LogWarning("Users", $"Access denied: {LoggedInUsername} attempted to access User Management");
                }
                catch { }
                return;
            }

            ShowUserManagement();
            HighlightMenuItem(label1);
            label1.Refresh();

            // Log user management access
            try
            {
                GlobalLogger.LogInfo("Users", $"{LoggedInUsername} opened User Management");
            }
            catch { }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ShowGameRatesAndEquipment();
            HighlightMenuItem(label2);
            label2.Refresh();

            // Log game rates & equipment access
            try
            {
                GlobalLogger.LogInfo("GameRates", $"{LoggedInUsername} opened Game Rates & Equipment");
            }
            catch { }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string role = (LoggedInUserRole ?? "").ToUpper();
            if (role == "CASHIER")
            {
                MessageBox.Show("Access Denied! Cashier cannot access Activity Logs.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Log access denied
                try
                {
                    GlobalLogger.LogWarning("ActivityLogs", $"Access denied: {LoggedInUsername} attempted to access Activity Logs");
                }
                catch { }
                return;
            }

            ShowActivityLogs();
            HighlightMenuItem(label3);
            label3.Refresh();

            // Log activity logs access
            try
            {
                GlobalLogger.LogInfo("ActivityLogs", $"{LoggedInUsername} opened Activity Logs");
            }
            catch { }
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

        private void ShowGameRatesAndEquipment()
        {
            ClearPanel2();

            gameRatesControl = new GameRates();
            gameRatesControl.Dock = DockStyle.Fill;

            tabRates.Controls.Clear();
            tabRates.Controls.Add(gameRatesControl);

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

            if (gameContentPanel == null || gameContentPanel.IsDisposed)
            {
                gameContentPanel = new Panel { Dock = DockStyle.Fill };
            }
            else
            {
                gameContentPanel.Controls.Clear();
            }

            if (!gameContentPanel.Controls.Contains(gameTabControl))
            {
                gameContentPanel.Controls.Add(gameTabControl);
            }

            gameTabControl.Visible = true;
            panel2.Controls.Add(gameContentPanel);
            panel2.Visible = true;
        }

        private void ShowActivityLogs()
        {
            ClearPanel2();
            try
            {
                // Use the singleton instance to maintain state
                activityLogsControl = Activitylogs.Instance;
                activityLogsControl.Dock = DockStyle.Fill;
                panel2.Controls.Add(activityLogsControl);
                panel2.Visible = true;

                // Refresh to show latest logs
                activityLogsControl.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Activity Logs: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Log error
                try
                {
                    GlobalLogger.LogError("ActivityLogs", $"Error loading logs: {ex.Message}");
                }
                catch { }

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

        private void ShowSales()
        {
            ClearPanel2();
            salesControl = new SALES();
            salesControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(salesControl);
            panel2.Visible = true;

            // Log sales access
            try
            {
                GlobalLogger.LogInfo("Sales", $"{LoggedInUsername} opened Sales report");
            }
            catch { }
        }

        private void ClearPanel2()
        {
            if (dashboardControl != null && !dashboardControl.IsDisposed) { dashboardControl.Dispose(); dashboardControl = null; }
            if (userManagementControl != null && !userManagementControl.IsDisposed) { userManagementControl.Dispose(); userManagementControl = null; }
            if (gameRatesControl != null && !gameRatesControl.IsDisposed) { gameRatesControl.Dispose(); gameRatesControl = null; }
            if (gameEquipmentControl != null && !gameEquipmentControl.IsDisposed) { gameEquipmentControl.Dispose(); gameEquipmentControl = null; }
            if (settingsControl != null && !settingsControl.IsDisposed) { settingsControl.Dispose(); settingsControl = null; }
            if (activityLogsControl != null && !activityLogsControl.IsDisposed) { activityLogsControl.Dispose(); activityLogsControl = null; }
            if (salesControl != null && !salesControl.IsDisposed) { salesControl.Dispose(); salesControl = null; }

            if (tabRates != null && !tabRates.IsDisposed) { tabRates.Controls.Clear(); }
            if (tabEquipment != null && !tabEquipment.IsDisposed) { tabEquipment.Controls.Clear(); }
            if (gameContentPanel != null && !gameContentPanel.IsDisposed) { gameContentPanel.Controls.Clear(); gameContentPanel.Dispose(); gameContentPanel = null; }
            if (gameTabControl != null && !gameTabControl.IsDisposed) { gameTabControl.Visible = false; }
            panel2.Controls.Clear();
        }

        // ==============================================
        // MENU STYLING METHODS
        // ==============================================

        private void SetMenuLabelsToYellow()
        {
            Color yellowColor = Color.FromArgb(228, 186, 94);
            Label[] menuLabels = { dash, label1, label2, label3, salesBtn };

            foreach (var label in menuLabels)
            {
                if (label != null)
                {
                    label.ForeColor = yellowColor;
                    label.BackColor = Color.Transparent;
                    // Store the ORIGINAL colors (not the highlighted ones)
                    if (!originalLabelColors.ContainsKey(label))
                    {
                        originalLabelColors[label] = new LabelColors
                        {
                            ForeColor = yellowColor,
                            BackColor = Color.Transparent
                        };
                    }
                }
            }
        }

        private void HighlightMenuItem(Label clickedLabel)
        {
            // Reset all menu items to their ORIGINAL colors (not highlighted state)
            ResetAllMenuItemsToOriginal();

            // Then highlight only the clicked one
            if (clickedLabel != null)
            {
                clickedLabel.BackColor = Color.FromArgb(40, 50, 60);
                clickedLabel.ForeColor = Color.White;
            }
        }

        private void ResetAllMenuItemsToOriginal()
        {
            Label[] menuLabels = { dash, label1, label2, label3, salesBtn };

            foreach (var label in menuLabels)
            {
                if (label != null && originalLabelColors.ContainsKey(label))
                {
                    // Always reset to the ORIGINAL colors stored
                    label.ForeColor = originalLabelColors[label].ForeColor;
                    label.BackColor = originalLabelColors[label].BackColor;
                }
            }
        }

        private void StoreOriginalColors()
        {
            // This method is kept for compatibility but does nothing
            // Colors are already stored in SetMenuLabelsToYellow()
        }

        private void ResetMenuColors()
        {
            // This method is kept for backward compatibility
            ResetAllMenuItemsToOriginal();
        }

        // ==============================================
        // HOVER EFFECT METHODS
        // ==============================================

        private void MenuLabel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                // Check if this label is currently highlighted (has dark background)
                bool isHighlighted = (label.BackColor == Color.FromArgb(40, 50, 60));

                // Only apply hover effect if this label is NOT the currently highlighted one
                if (!isHighlighted)
                {
                    // Store original colors if not already stored (should already be stored)
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
                // Check if this label is currently highlighted (has dark background)
                bool isHighlighted = (label.BackColor == Color.FromArgb(40, 50, 60));

                // Only revert if this label is NOT the currently highlighted one
                if (!isHighlighted)
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

        // ==============================================
        // FORM EVENT HANDLERS
        // ==============================================

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSigningOut) return;

            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Do you want to exit the application?",
                    "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Log application exit
                    try
                    {
                        GlobalLogger.LogInfo("System", $"Application closed by {LoggedInUsername}");
                    }
                    catch { }
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
            // Reposition icons when form loads
            RepositionTopIcons();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Reposition icons when form is resized
            RepositionTopIcons();
        }

        private void date_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dddd, MMMM dd, yyyy");
            MessageBox.Show($"Current date and time:\n{formattedDate}\n{currentDate.ToString("hh:mm:ss tt")}",
                            "Date Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label8_Click(object sender, EventArgs e) { }

        private void salesBtn_Click(object sender, EventArgs e)
        {
            ShowSales();
            HighlightMenuItem(salesBtn);
            salesBtn.Refresh();

            // Log sales access
            try
            {
                GlobalLogger.LogInfo("Sales", $"{LoggedInUsername} opened Sales report");
            }
            catch { }
        }

        private void panel2_Paint(object sender, PaintEventArgs e) { }

        // ==============================================
        // PASSWORD VALIDATION METHODS
        // ==============================================

        /// <summary>
        /// Validates a password against the current security settings
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if password meets requirements, false otherwise</returns>
        public bool ValidatePassword(string password)
        {
            try
            {
                // Create a temporary settings instance to get current policies
                SETTINGS tempSettings = new SETTINGS();
                return tempSettings.ValidatePassword(password);
            }
            catch (Exception ex)
            {
                GlobalLogger.LogError("PasswordValidation", ex.Message, "Error validating password");
                return false;
            }
        }

        /// <summary>
        /// Gets the current password requirements as a formatted string
        /// </summary>
        /// <returns>Password requirements string</returns>
        public string GetPasswordRequirements()
        {
            try
            {
                SETTINGS tempSettings = new SETTINGS();
                return tempSettings.GetPasswordRequirements();
            }
            catch (Exception ex)
            {
                GlobalLogger.LogError("PasswordValidation", ex.Message, "Error getting password requirements");
                return "Minimum 8 characters, at least one uppercase, one number, and one special character";
            }
        }

        /// <summary>
        /// Checks password strength and returns detailed feedback
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <returns>Password strength feedback string</returns>
        public string CheckPasswordStrength(string password)
        {
            SETTINGS tempSettings = new SETTINGS();
            var settings = tempSettings.GetCurrentSecuritySettings();
            List<string> issues = new List<string>();

            if (password.Length < settings.MinPasswordLength)
                issues.Add($"At least {settings.MinPasswordLength} characters");

            if (settings.RequireUppercase && !password.Any(char.IsUpper))
                issues.Add("At least one uppercase letter");

            if (settings.RequireNumber && !password.Any(char.IsDigit))
                issues.Add("At least one number");

            if (settings.RequireSpecialChar && !password.Any(ch => !char.IsLetterOrDigit(ch)))
                issues.Add("At least one special character (!@#$%^&*)");

            if (issues.Count == 0)
                return "✓ Strong password!";
            else
                return $"Password requirements: {string.Join(", ", issues)}";
        }

        /// <summary>
        /// Use this method when creating or updating a user's password
        /// </summary>
        public bool IsPasswordValid(string password, out string errorMessage)
        {
            if (ValidatePassword(password))
            {
                errorMessage = "";
                return true;
            }
            else
            {
                errorMessage = $"Password does not meet requirements: {GetPasswordRequirements()}";
                return false;
            }
        }
    }
}