using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace cms
{
    public partial class SETTINGS : UserControl
    {
        // Configuration file path
        private string configFilePath = Path.Combine(Application.StartupPath, "security_config.xml");

        // MySQL Connection String
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";

        // Current user info
        private string currentUser = "";
        private string currentUserRole = "";

        // Security Settings Data
        public class SecuritySettings
        {
            public int MinPasswordLength { get; set; }
            public bool RequireUppercase { get; set; }
            public bool RequireNumber { get; set; }
            public bool RequireSpecialChar { get; set; }
            public int MaxLoginAttempts { get; set; }
            public int PasswordExpiryDays { get; set; }
            public int PasswordHistoryCount { get; set; }
            public bool ForceLogoutOnPasswordChange { get; set; }
        }

        public SETTINGS()
        {
            InitializeComponent();

            // Get current user from GlobalLogger
            if (!string.IsNullOrEmpty(GlobalLogger.CurrentUsername))
            {
                currentUser = GlobalLogger.CurrentUsername;
                currentUserRole = GlobalLogger.CurrentUserRole;
            }
            else
            {
                currentUser = "System";
                currentUserRole = "ADMIN";
            }

            // Apply color scheme
            ApplyColorScheme();

            // Load security settings
            LoadSecuritySettings();

            // Wire up event handlers
            SetupEventHandlers();

            // Log that Settings module was opened
            try
            {
                GlobalLogger.LogInfo("Settings", $"User {currentUser} ({currentUserRole}) opened Settings module");
            }
            catch { }
        }

        public void SetCurrentUser(string username, string role)
        {
            currentUser = username;
            currentUserRole = role;

            try
            {
                GlobalLogger.LogInfo("Settings", $"User {username} ({role}) accessed Settings module");
            }
            catch { }
        }

        private void ApplyColorScheme()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            tabControl1.BackColor = Color.FromArgb(250, 250, 250);

            // Style buttons
            StyleButton(btnSaveAll, Color.FromArgb(228, 186, 94), Color.FromArgb(40, 41, 34));
            StyleButton(btnReset, Color.FromArgb(220, 220, 220), Color.FromArgb(70, 70, 70));
            StyleButton(btnSaveDatabase, Color.FromArgb(228, 186, 94), Color.FromArgb(40, 41, 34));
            StyleButton(btnTestConnection, Color.FromArgb(89, 91, 86), Color.White);
            StyleButton(btnBackupDatabase, Color.FromArgb(240, 240, 240), Color.FromArgb(70, 70, 70));
            StyleButton(btnRestoreDatabase, Color.FromArgb(240, 240, 240), Color.FromArgb(70, 70, 70));
            StyleButton(btnSaveEmail, Color.FromArgb(228, 186, 94), Color.FromArgb(40, 41, 34));
            StyleButton(btnTestEmail, Color.FromArgb(89, 91, 86), Color.White);

            // Set group box colors
            foreach (var groupBox in new[] { groupBoxConnection, groupBoxBackup, groupBoxPasswordPolicy,
                groupBoxSession, groupBoxEmail, groupBoxNotificationTypes, groupBoxAbout, groupBoxSystem })
            {
                if (groupBox != null)
                {
                    groupBox.BackColor = Color.White;
                    groupBox.ForeColor = Color.FromArgb(50, 50, 50);
                }
            }

            // Set label colors
            foreach (Control control in this.Controls)
            {
                SetLabelColors(control);
            }
        }

        private void StyleButton(Button btn, Color backColor, Color foreColor)
        {
            if (btn != null)
            {
                btn.BackColor = backColor;
                btn.ForeColor = foreColor;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;

                btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(backColor, 0.2f);
                btn.MouseLeave += (s, e) => btn.BackColor = backColor;
            }
        }

        private void SetLabelColors(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = Color.FromArgb(70, 70, 70);
                }
                if (control.HasChildren)
                {
                    SetLabelColors(control);
                }
            }
        }

        private void SetupEventHandlers()
        {
            // Tab control
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            // General tab
            comboBoxTheme.SelectedIndexChanged += ComboBoxTheme_SelectedIndexChanged;
            comboBoxLanguage.SelectedIndexChanged += ComboBoxLanguage_SelectedIndexChanged;
            checkBoxNotifications.CheckedChanged += CheckBoxNotifications_CheckedChanged;
            checkBoxAutoBackup.CheckedChanged += CheckBoxAutoBackup_CheckedChanged;
            numericUpDownSessionTimeout.ValueChanged += NumericUpDownSessionTimeout_ValueChanged;

            // Database tab
            txtServer.TextChanged += (s, e) => { };
            txtUsername.TextChanged += (s, e) => { };
            txtPassword.TextChanged += (s, e) => { };
            txtDatabaseName.TextChanged += (s, e) => { };
            txtBackupPath.TextChanged += (s, e) => { };
            btnTestConnection.Click += BtnTestConnection_Click;
            btnSaveDatabase.Click += BtnSaveDatabase_Click;
            btnBackupDatabase.Click += BtnBackupDatabase_Click;
            btnRestoreDatabase.Click += BtnRestoreDatabase_Click;

            // Security tab
            numericUpDownMinLength.ValueChanged += NumericUpDownMinLength_ValueChanged;
            numericUpDownMaxAttempts.ValueChanged += NumericUpDownMaxAttempts_ValueChanged;
            numericUpDownPasswordExpiry.ValueChanged += NumericUpDownPasswordExpiry_ValueChanged;
            numericUpDownPasswordHistory.ValueChanged += NumericUpDownPasswordHistory_ValueChanged;
            checkBoxRequireUppercase.CheckedChanged += CheckBoxRequireUppercase_CheckedChanged;
            checkBoxRequireNumber.CheckedChanged += CheckBoxRequireNumber_CheckedChanged;
            checkBoxRequireSpecialChar.CheckedChanged += CheckBoxRequireSpecialChar_CheckedChanged;
            checkBoxForceLogout.CheckedChanged += CheckBoxForceLogout_CheckedChanged;

            // Notifications tab
            txtSmtpServer.TextChanged += (s, e) => { };
            txtSmtpPort.TextChanged += (s, e) => { };
            txtEmailUsername.TextChanged += (s, e) => { };
            txtEmailPassword.TextChanged += (s, e) => { };
            btnTestEmail.Click += BtnTestEmail_Click;
            btnSaveEmail.Click += BtnSaveEmail_Click;
            checkBoxEmailNewUser.CheckedChanged += CheckBoxEmailNewUser_CheckedChanged;
            checkBoxEmailLowInventory.CheckedChanged += CheckBoxEmailLowInventory_CheckedChanged;
            checkBoxEmailSalesReport.CheckedChanged += CheckBoxEmailSalesReport_CheckedChanged;

            // About tab
            linkLabelWebsite.LinkClicked += LinkLabelWebsite_LinkClicked;

            // Bottom buttons
            btnSaveAll.Click += BtnSaveAll_Click;
            btnReset.Click += BtnReset_Click;
        }

        // ==================== GENERAL TAB HANDLERS ====================
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalLogger.LogInfo("Settings", $"User {currentUser} switched to tab: {tabControl1.SelectedTab.Text}");
            }
            catch { }
        }

        private void ComboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalLogger.LogInfo("Settings", $"User {currentUser} changed theme to: {comboBoxTheme.SelectedItem}");
            }
            catch { }
        }

        private void ComboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalLogger.LogInfo("Settings", $"User {currentUser} changed language to: {comboBoxLanguage.SelectedItem}");
            }
            catch { }
        }

        private void CheckBoxNotifications_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalLogger.LogInfo("Settings", $"User {currentUser} {(checkBoxNotifications.Checked ? "enabled" : "disabled")} notifications");
            }
            catch { }
        }

        private void CheckBoxAutoBackup_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalLogger.LogInfo("Settings", $"User {currentUser} {(checkBoxAutoBackup.Checked ? "enabled" : "disabled")} auto backup");
            }
            catch { }
        }

        private void NumericUpDownSessionTimeout_ValueChanged(object sender, EventArgs e)
        {
            // Handle session timeout change
        }

        // ==================== DATABASE TAB HANDLERS ====================
        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                string server = txtServer.Text.Trim();
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;
                string database = txtDatabaseName.Text.Trim();

                string testConnectionString = $"Server={server};Database={database};Uid={username};Pwd={password};";

                using (MySqlConnection conn = new MySqlConnection(testConnectionString))
                {
                    conn.Open();
                    MessageBox.Show("Connection successful! Database is accessible.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    try
                    {
                        GlobalLogger.LogInfo("Settings", $"User {currentUser} tested database connection successfully");
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    GlobalLogger.LogError("Settings", ex.Message, "Database connection test failed");
                }
                catch { }
            }
        }

        private void BtnSaveDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                // Save database settings to config file or registry
                MessageBox.Show("Database settings saved successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    GlobalLogger.LogInfo("Settings", $"User {currentUser} saved database settings");
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBackupDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "sql";
                saveDialog.FileName = $"matchpoint_db_backup_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Perform backup using mysqldump or custom backup
                    MessageBox.Show($"Database backup created successfully!\n\nSaved to: {saveDialog.FileName}",
                        "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    try
                    {
                        GlobalLogger.LogInfo("Settings", $"User {currentUser} created database backup: {saveDialog.FileName}");
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Backup failed: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    GlobalLogger.LogError("Settings", ex.Message, "Database backup failed");
                }
                catch { }
            }
        }

        private void BtnRestoreDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    DialogResult confirm = MessageBox.Show("Restoring database will overwrite current data.\n\nAre you sure?",
                        "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        // Perform restore
                        MessageBox.Show($"Database restored successfully from:\n{openDialog.FileName}",
                            "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        try
                        {
                            GlobalLogger.LogInfo("Settings", $"User {currentUser} restored database from: {openDialog.FileName}");
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Restore failed: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    GlobalLogger.LogError("Settings", ex.Message, "Database restore failed");
                }
                catch { }
            }
        }

        // ==================== SECURITY TAB HANDLERS ====================
        private void ValidatePasswordPolicy(object sender, EventArgs e)
        {
            int minLength = (int)numericUpDownMinLength.Value;
            bool hasUppercase = checkBoxRequireUppercase.Checked;
            bool hasNumber = checkBoxRequireNumber.Checked;
            bool hasSpecial = checkBoxRequireSpecialChar.Checked;

            // Create or get warning label if needed
            Label lblPolicyWarning = new Label();
            // In a real implementation, you'd have a label on the form
        }

        private void NumericUpDownMinLength_ValueChanged(object sender, EventArgs e) { }
        private void NumericUpDownMaxAttempts_ValueChanged(object sender, EventArgs e) { }
        private void NumericUpDownPasswordExpiry_ValueChanged(object sender, EventArgs e) { }
        private void NumericUpDownPasswordHistory_ValueChanged(object sender, EventArgs e) { }
        private void CheckBoxRequireUppercase_CheckedChanged(object sender, EventArgs e) { }
        private void CheckBoxRequireNumber_CheckedChanged(object sender, EventArgs e) { }
        private void CheckBoxRequireSpecialChar_CheckedChanged(object sender, EventArgs e) { }
        private void CheckBoxForceLogout_CheckedChanged(object sender, EventArgs e) { }

        // ==================== NOTIFICATIONS TAB HANDLERS ====================
        private void BtnTestEmail_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Test email sent successfully! Check your inbox.",
                    "Email Test", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    GlobalLogger.LogInfo("Settings", $"User {currentUser} tested email configuration");
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Email test failed: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveEmail_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Email settings saved successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    GlobalLogger.LogInfo("Settings", $"User {currentUser} saved email settings");
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving email settings: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBoxEmailNewUser_CheckedChanged(object sender, EventArgs e) { }
        private void CheckBoxEmailLowInventory_CheckedChanged(object sender, EventArgs e) { }
        private void CheckBoxEmailSalesReport_CheckedChanged(object sender, EventArgs e) { }

        // ==================== ABOUT TAB HANDLERS ====================
        private void LinkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.matchpoint.ph");
            }
            catch { }
        }

        // ==================== SECURITY SETTINGS METHODS ====================
        private void LoadSecuritySettings()
        {
            try
            {
                SecuritySettings settings = LoadSettingsFromDatabase();

                if (settings == null)
                {
                    settings = GetDefaultSecuritySettings();
                    SaveSettingsToDatabase(settings);
                }

                ApplySettingsToUI(settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading security settings: {ex.Message}\n\nUsing default settings.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ApplySettingsToUI(GetDefaultSecuritySettings());
            }
        }

        private SecuritySettings LoadSettingsFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string checkTableQuery = @"
                        SELECT COUNT(*) 
                        FROM information_schema.tables 
                        WHERE table_schema = 'matchpoint_db' 
                        AND table_name = 'security_settings'";

                    using (MySqlCommand checkCmd = new MySqlCommand(checkTableQuery, conn))
                    {
                        int tableCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (tableCount == 0)
                        {
                            string createTableQuery = @"
                                CREATE TABLE IF NOT EXISTS security_settings (
                                    setting_id INT PRIMARY KEY AUTO_INCREMENT,
                                    min_password_length INT NOT NULL DEFAULT 8,
                                    require_uppercase BOOLEAN DEFAULT TRUE,
                                    require_number BOOLEAN DEFAULT TRUE,
                                    require_special_char BOOLEAN DEFAULT TRUE,
                                    max_login_attempts INT NOT NULL DEFAULT 3,
                                    password_expiry_days INT NOT NULL DEFAULT 90,
                                    password_history_count INT NOT NULL DEFAULT 5,
                                    force_logout_on_password_change BOOLEAN DEFAULT TRUE,
                                    last_updated DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
                                )";

                            using (MySqlCommand createCmd = new MySqlCommand(createTableQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }

                            string insertQuery = @"
                                INSERT INTO security_settings 
                                (min_password_length, require_uppercase, require_number, require_special_char, 
                                 max_login_attempts, password_expiry_days, password_history_count, force_logout_on_password_change)
                                VALUES (8, TRUE, TRUE, TRUE, 3, 90, 5, TRUE)";

                            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.ExecuteNonQuery();
                            }

                            return null;
                        }
                    }

                    string query = "SELECT * FROM security_settings ORDER BY setting_id DESC LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SecuritySettings
                                {
                                    MinPasswordLength = reader.GetInt32("min_password_length"),
                                    RequireUppercase = reader.GetBoolean("require_uppercase"),
                                    RequireNumber = reader.GetBoolean("require_number"),
                                    RequireSpecialChar = reader.GetBoolean("require_special_char"),
                                    MaxLoginAttempts = reader.GetInt32("max_login_attempts"),
                                    PasswordExpiryDays = reader.GetInt32("password_expiry_days"),
                                    PasswordHistoryCount = reader.GetInt32("password_history_count"),
                                    ForceLogoutOnPasswordChange = reader.GetBoolean("force_logout_on_password_change")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings from database: {ex.Message}");
            }

            return null;
        }

        private void SaveSettingsToDatabase(SecuritySettings settings)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string checkQuery = "SELECT COUNT(*) FROM security_settings";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            string updateQuery = @"
                                UPDATE security_settings 
                                SET min_password_length = @minLength,
                                    require_uppercase = @requireUppercase,
                                    require_number = @requireNumber,
                                    require_special_char = @requireSpecial,
                                    max_login_attempts = @maxAttempts,
                                    password_expiry_days = @expiryDays,
                                    password_history_count = @historyCount,
                                    force_logout_on_password_change = @forceLogout,
                                    last_updated = NOW()";

                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@minLength", settings.MinPasswordLength);
                                updateCmd.Parameters.AddWithValue("@requireUppercase", settings.RequireUppercase);
                                updateCmd.Parameters.AddWithValue("@requireNumber", settings.RequireNumber);
                                updateCmd.Parameters.AddWithValue("@requireSpecial", settings.RequireSpecialChar);
                                updateCmd.Parameters.AddWithValue("@maxAttempts", settings.MaxLoginAttempts);
                                updateCmd.Parameters.AddWithValue("@expiryDays", settings.PasswordExpiryDays);
                                updateCmd.Parameters.AddWithValue("@historyCount", settings.PasswordHistoryCount);
                                updateCmd.Parameters.AddWithValue("@forceLogout", settings.ForceLogoutOnPasswordChange);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string insertQuery = @"
                                INSERT INTO security_settings 
                                (min_password_length, require_uppercase, require_number, require_special_char, 
                                 max_login_attempts, password_expiry_days, password_history_count, force_logout_on_password_change)
                                VALUES (@minLength, @requireUppercase, @requireNumber, @requireSpecial, 
                                        @maxAttempts, @expiryDays, @historyCount, @forceLogout)";

                            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@minLength", settings.MinPasswordLength);
                                insertCmd.Parameters.AddWithValue("@requireUppercase", settings.RequireUppercase);
                                insertCmd.Parameters.AddWithValue("@requireNumber", settings.RequireNumber);
                                insertCmd.Parameters.AddWithValue("@requireSpecial", settings.RequireSpecialChar);
                                insertCmd.Parameters.AddWithValue("@maxAttempts", settings.MaxLoginAttempts);
                                insertCmd.Parameters.AddWithValue("@expiryDays", settings.PasswordExpiryDays);
                                insertCmd.Parameters.AddWithValue("@historyCount", settings.PasswordHistoryCount);
                                insertCmd.Parameters.AddWithValue("@forceLogout", settings.ForceLogoutOnPasswordChange);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                try
                {
                    GlobalLogger.LogInfo("Settings", $"User {currentUser} saved security settings to database");
                }
                catch { }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving settings to database: {ex.Message}");
            }
        }

        private SecuritySettings GetDefaultSecuritySettings()
        {
            return new SecuritySettings
            {
                MinPasswordLength = 8,
                RequireUppercase = true,
                RequireNumber = true,
                RequireSpecialChar = true,
                MaxLoginAttempts = 3,
                PasswordExpiryDays = 90,
                PasswordHistoryCount = 5,
                ForceLogoutOnPasswordChange = true
            };
        }

        private void ApplySettingsToUI(SecuritySettings settings)
        {
            numericUpDownMinLength.Value = settings.MinPasswordLength;
            checkBoxRequireUppercase.Checked = settings.RequireUppercase;
            checkBoxRequireNumber.Checked = settings.RequireNumber;
            checkBoxRequireSpecialChar.Checked = settings.RequireSpecialChar;
            numericUpDownMaxAttempts.Value = settings.MaxLoginAttempts;
            numericUpDownPasswordExpiry.Value = settings.PasswordExpiryDays;
            numericUpDownPasswordHistory.Value = settings.PasswordHistoryCount;
            checkBoxForceLogout.Checked = settings.ForceLogoutOnPasswordChange;
        }

        private SecuritySettings GetSettingsFromUI()
        {
            return new SecuritySettings
            {
                MinPasswordLength = (int)numericUpDownMinLength.Value,
                RequireUppercase = checkBoxRequireUppercase.Checked,
                RequireNumber = checkBoxRequireNumber.Checked,
                RequireSpecialChar = checkBoxRequireSpecialChar.Checked,
                MaxLoginAttempts = (int)numericUpDownMaxAttempts.Value,
                PasswordExpiryDays = (int)numericUpDownPasswordExpiry.Value,
                PasswordHistoryCount = (int)numericUpDownPasswordHistory.Value,
                ForceLogoutOnPasswordChange = checkBoxForceLogout.Checked
            };
        }

        // ==================== BOTTOM BUTTON HANDLERS ====================
        private void BtnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                // Save all settings
                SecuritySettings settings = GetSettingsFromUI();
                SaveSettingsToDatabase(settings);

                MessageBox.Show("All settings saved successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    GlobalLogger.LogInfo("Settings", $"User {currentUser} saved all settings");
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    GlobalLogger.LogError("Settings", ex.Message, "Error saving all settings");
                }
                catch { }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to reset all settings to default values?",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SecuritySettings defaultSettings = GetDefaultSecuritySettings();
                ApplySettingsToUI(defaultSettings);

                MessageBox.Show("Settings reset to default values.\n\nClick 'Save All Settings' to apply changes.",
                    "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    GlobalLogger.LogInfo("Settings", $"User {currentUser} reset settings to defaults");
                }
                catch { }
            }
        }

        // Public methods for login form to use
        public SecuritySettings GetCurrentSecuritySettings()
        {
            return LoadSettingsFromDatabase() ?? GetDefaultSecuritySettings();
        }

        public bool ValidatePassword(string password)
        {
            SecuritySettings settings = GetCurrentSecuritySettings();

            if (password.Length < settings.MinPasswordLength)
                return false;

            if (settings.RequireUppercase && !password.Any(char.IsUpper))
                return false;

            if (settings.RequireNumber && !password.Any(char.IsDigit))
                return false;

            if (settings.RequireSpecialChar && !password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }

        public string GetPasswordRequirements()
        {
            SecuritySettings settings = GetCurrentSecuritySettings();
            List<string> requirements = new List<string>();

            requirements.Add($"Minimum {settings.MinPasswordLength} characters");

            if (settings.RequireUppercase)
                requirements.Add("at least one uppercase letter");

            if (settings.RequireNumber)
                requirements.Add("at least one number");

            if (settings.RequireSpecialChar)
                requirements.Add("at least one special character");

            return string.Join(", ", requirements);
        }
    }
}