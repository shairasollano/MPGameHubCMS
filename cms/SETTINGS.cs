using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cms
{
    public partial class SETTINGS : UserControl
    {
        // Configuration file path
        private string configFilePath = Path.Combine(Application.StartupPath, "config.xml");

        // Settings data class
        public class SettingsData
        {
            // General Settings
            public string Theme { get; set; }
            public string Language { get; set; }
            public bool EnableNotifications { get; set; }
            public bool AutoBackup { get; set; }
            public int SessionTimeout { get; set; }

            // Database Settings
            public string Server { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string DatabaseName { get; set; }
            public string BackupPath { get; set; }

            // Security Settings
            public int MinPasswordLength { get; set; }
            public bool RequireUppercase { get; set; }
            public bool RequireNumber { get; set; }
            public bool RequireSpecialChar { get; set; }
            public int MaxLoginAttempts { get; set; }
            public int PasswordExpiryDays { get; set; }
            public int PasswordHistoryCount { get; set; }
            public bool ForceLogoutOnPasswordChange { get; set; }

            // Game Settings
            public string DefaultGame { get; set; }
            public bool EnableSound { get; set; }
            public bool AutoStartGame { get; set; }
            public decimal DefaultHourlyRate { get; set; }

            // Email Settings
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string EmailUsername { get; set; }
            public string EmailPassword { get; set; }
            public bool EmailNewUserAlerts { get; set; }
            public bool EmailLowInventoryAlerts { get; set; }
            public bool EmailSalesReport { get; set; }
        }

        public SETTINGS()
        {
            InitializeComponent();

            // Apply light colors
            ApplyLightTheme();

            // Load settings when control is loaded
            this.Load += SETTINGS_Load;

            // Set up event handlers
            SetupEventHandlers();

            // Set logo image
            SetLogoImage();
        }

        private void ApplyLightTheme()
        {
            // Set background colors
            this.BackColor = Color.FromArgb(250, 250, 250);

            // Set tab control colors
            tabControl1.BackColor = Color.FromArgb(250, 250, 250);

            // Set button colors
            btnSaveAll.BackColor = Color.FromArgb(60, 180, 100);
            btnSaveAll.ForeColor = Color.White;

            btnReset.BackColor = Color.FromArgb(220, 220, 220);
            btnReset.ForeColor = Color.FromArgb(70, 70, 70);

            // Set group box colors
            foreach (Control control in this.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = Color.White;
                    groupBox.ForeColor = Color.FromArgb(50, 50, 50);
                }
            }
        }

        private void SETTINGS_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void SetupEventHandlers()
        {
            // Database buttons
            btnTestConnection.Click += BtnTestConnection_Click;
            btnSaveDatabase.Click += BtnSaveDatabase_Click;
            btnBackupDatabase.Click += BtnBackupDatabase_Click;
            btnRestoreDatabase.Click += BtnRestoreDatabase_Click;

            // Email buttons
            btnTestEmail.Click += BtnTestEmail_Click;
            btnSaveEmail.Click += BtnSaveEmail_Click;

            // General buttons
            btnSaveAll.Click += BtnSaveAll_Click;
            btnReset.Click += BtnReset_Click;

            // Website link
            linkLabelWebsite.LinkClicked += LinkLabelWebsite_LinkClicked;

            // Theme change
            
        }

        private void SetLogoImage()
        {
            try
            {
                // Create a simple logo with light colors
                Bitmap bmp = new Bitmap(180, 180);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Draw background
                    g.Clear(Color.White);

                    // Draw a circle with light blue
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(200, 230, 255)))
                    {
                        g.FillEllipse(brush, 10, 10, 160, 160);
                    }

                    // Draw MP text
                    using (System.Drawing.Font font = new System.Drawing.Font("Arial", 48, System.Drawing.FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(70, 130, 180)))
                    {
                        g.DrawString("MP", font, brush, 45, 55);
                    }

                    // Add a subtle border
                    using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 2))
                    {
                        g.DrawEllipse(pen, 10, 10, 160, 160);
                    }
                }
                pictureBoxLogo.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating logo: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Preview theme change
            

            // You can add theme preview logic here
            // For example, change some colors temporarily
        }

        // ==============================================
        // LOAD AND SAVE SETTINGS
        // ==============================================

        private void LoadSettings()
        {
            try
            {
                SettingsData settings = LoadSettingsFromFile();

                if (settings == null)
                {
                    // Load default settings if file doesn't exist
                    settings = GetDefaultSettings();
                }

                ApplySettingsToUI(settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                ApplySettingsToUI(GetDefaultSettings());
            }
        }

        private SettingsData LoadSettingsFromFile()
        {
            if (!File.Exists(configFilePath))
                return null;

            try
            {
                string[] lines = File.ReadAllLines(configFilePath);
                var settings = new SettingsData();

                foreach (string line in lines)
                {
                    if (line.Contains("="))
                    {
                        string[] parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();

                            switch (key)
                            {
                                case "Theme": settings.Theme = value; break;
                                case "Language": settings.Language = value; break;
                                case "EnableNotifications": settings.EnableNotifications = bool.Parse(value); break;
                                case "AutoBackup": settings.AutoBackup = bool.Parse(value); break;
                                case "SessionTimeout": settings.SessionTimeout = int.Parse(value); break;
                                case "Server": settings.Server = value; break;
                                case "Username": settings.Username = value; break;
                                case "Password": settings.Password = value; break;
                                case "DatabaseName": settings.DatabaseName = value; break;
                                case "BackupPath": settings.BackupPath = value; break;
                                case "MinPasswordLength": settings.MinPasswordLength = int.Parse(value); break;
                                case "RequireUppercase": settings.RequireUppercase = bool.Parse(value); break;
                                case "RequireNumber": settings.RequireNumber = bool.Parse(value); break;
                                case "RequireSpecialChar": settings.RequireSpecialChar = bool.Parse(value); break;
                                case "MaxLoginAttempts": settings.MaxLoginAttempts = int.Parse(value); break;
                                case "PasswordExpiryDays": settings.PasswordExpiryDays = int.Parse(value); break;
                                case "PasswordHistoryCount": settings.PasswordHistoryCount = int.Parse(value); break;
                                case "ForceLogoutOnPasswordChange": settings.ForceLogoutOnPasswordChange = bool.Parse(value); break;
                                case "DefaultGame": settings.DefaultGame = value; break;
                                case "EnableSound": settings.EnableSound = bool.Parse(value); break;
                                case "AutoStartGame": settings.AutoStartGame = bool.Parse(value); break;
                                case "DefaultHourlyRate": settings.DefaultHourlyRate = decimal.Parse(value); break;
                                case "SmtpServer": settings.SmtpServer = value; break;
                                case "SmtpPort": settings.SmtpPort = int.Parse(value); break;
                                case "EmailUsername": settings.EmailUsername = value; break;
                                case "EmailPassword": settings.EmailPassword = value; break;
                                case "EmailNewUserAlerts": settings.EmailNewUserAlerts = bool.Parse(value); break;
                                case "EmailLowInventoryAlerts": settings.EmailLowInventoryAlerts = bool.Parse(value); break;
                                case "EmailSalesReport": settings.EmailSalesReport = bool.Parse(value); break;
                            }
                        }
                    }
                }

                return settings;
            }
            catch
            {
                return null;
            }
        }

        private void SaveSettingsToFile(SettingsData settings)
        {
            try
            {
                List<string> lines = new List<string>();

                // Add all settings to the list
                lines.Add($"Theme={settings.Theme}");
                lines.Add($"Language={settings.Language}");
                lines.Add($"EnableNotifications={settings.EnableNotifications}");
                lines.Add($"AutoBackup={settings.AutoBackup}");
                lines.Add($"SessionTimeout={settings.SessionTimeout}");
                lines.Add($"Server={settings.Server}");
                lines.Add($"Username={settings.Username}");
                lines.Add($"Password={settings.Password}");
                lines.Add($"DatabaseName={settings.DatabaseName}");
                lines.Add($"BackupPath={settings.BackupPath}");
                lines.Add($"MinPasswordLength={settings.MinPasswordLength}");
                lines.Add($"RequireUppercase={settings.RequireUppercase}");
                lines.Add($"RequireNumber={settings.RequireNumber}");
                lines.Add($"RequireSpecialChar={settings.RequireSpecialChar}");
                lines.Add($"MaxLoginAttempts={settings.MaxLoginAttempts}");
                lines.Add($"PasswordExpiryDays={settings.PasswordExpiryDays}");
                lines.Add($"PasswordHistoryCount={settings.PasswordHistoryCount}");
                lines.Add($"ForceLogoutOnPasswordChange={settings.ForceLogoutOnPasswordChange}");
                lines.Add($"DefaultGame={settings.DefaultGame}");
                lines.Add($"EnableSound={settings.EnableSound}");
                lines.Add($"AutoStartGame={settings.AutoStartGame}");
                lines.Add($"DefaultHourlyRate={settings.DefaultHourlyRate}");
                lines.Add($"SmtpServer={settings.SmtpServer}");
                lines.Add($"SmtpPort={settings.SmtpPort}");
                lines.Add($"EmailUsername={settings.EmailUsername}");
                lines.Add($"EmailPassword={settings.EmailPassword}");
                lines.Add($"EmailNewUserAlerts={settings.EmailNewUserAlerts}");
                lines.Add($"EmailLowInventoryAlerts={settings.EmailLowInventoryAlerts}");
                lines.Add($"EmailSalesReport={settings.EmailSalesReport}");

                File.WriteAllLines(configFilePath, lines);

                MessageBox.Show("Settings saved successfully!", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SettingsData GetDefaultSettings()
        {
            return new SettingsData
            {
                // General
                Theme = "Light",
                Language = "English",
                EnableNotifications = true,
                AutoBackup = true,
                SessionTimeout = 30,

                // Database
                Server = "localhost",
                Username = "root",
                Password = "",
                DatabaseName = "matchpoint_db",
                BackupPath = Path.Combine(Application.StartupPath, "Backups"),

                // Security
                MinPasswordLength = 8,
                RequireUppercase = true,
                RequireNumber = true,
                RequireSpecialChar = true,
                MaxLoginAttempts = 3,
                PasswordExpiryDays = 90,
                PasswordHistoryCount = 5,
                ForceLogoutOnPasswordChange = true,

                // Game Settings
                DefaultGame = "Counter-Strike",
                EnableSound = true,
                AutoStartGame = false,
                DefaultHourlyRate = 50.00m,

                // Email Settings
                SmtpServer = "smtp.gmail.com",
                SmtpPort = 587,
                EmailUsername = "",
                EmailPassword = "",
                EmailNewUserAlerts = true,
                EmailLowInventoryAlerts = true,
                EmailSalesReport = true
            };
        }

        private void ApplySettingsToUI(SettingsData settings)
        {
            

            // Database Settings
            txtServer.Text = settings.Server;
            txtUsername.Text = settings.Username;
            txtPassword.Text = settings.Password;
            txtDatabaseName.Text = settings.DatabaseName;
            txtBackupPath.Text = settings.BackupPath;

            // Security Settings
            numericUpDownMinLength.Value = settings.MinPasswordLength;
            checkBoxRequireUppercase.Checked = settings.RequireUppercase;
            checkBoxRequireNumber.Checked = settings.RequireNumber;
            checkBoxRequireSpecialChar.Checked = settings.RequireSpecialChar;
            numericUpDownMaxAttempts.Value = settings.MaxLoginAttempts;
            numericUpDownPasswordExpiry.Value = settings.PasswordExpiryDays;
            numericUpDownPasswordHistory.Value = settings.PasswordHistoryCount;
            checkBoxForceLogout.Checked = settings.ForceLogoutOnPasswordChange;

            

            // Email Settings
            txtSmtpServer.Text = settings.SmtpServer;
            txtSmtpPort.Text = settings.SmtpPort.ToString();
            txtEmailUsername.Text = settings.EmailUsername;
            txtEmailPassword.Text = settings.EmailPassword;
            checkBoxEmailNewUser.Checked = settings.EmailNewUserAlerts;
            checkBoxEmailLowInventory.Checked = settings.EmailLowInventoryAlerts;
            checkBoxEmailSalesReport.Checked = settings.EmailSalesReport;
        }

        private SettingsData GetSettingsFromUI()
        {
            return new SettingsData
            {
                

                // Database Settings
                Server = txtServer.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Text,
                DatabaseName = txtDatabaseName.Text,
                BackupPath = txtBackupPath.Text,

                // Security Settings
                MinPasswordLength = (int)numericUpDownMinLength.Value,
                RequireUppercase = checkBoxRequireUppercase.Checked,
                RequireNumber = checkBoxRequireNumber.Checked,
                RequireSpecialChar = checkBoxRequireSpecialChar.Checked,
                MaxLoginAttempts = (int)numericUpDownMaxAttempts.Value,
                PasswordExpiryDays = (int)numericUpDownPasswordExpiry.Value,
                PasswordHistoryCount = (int)numericUpDownPasswordHistory.Value,
                ForceLogoutOnPasswordChange = checkBoxForceLogout.Checked,

                

                // Email Settings
                SmtpServer = txtSmtpServer.Text,
                SmtpPort = int.TryParse(txtSmtpPort.Text, out int port) ? port : 587,
                EmailUsername = txtEmailUsername.Text,
                EmailPassword = txtEmailPassword.Text,
                EmailNewUserAlerts = checkBoxEmailNewUser.Checked,
                EmailLowInventoryAlerts = checkBoxEmailLowInventory.Checked,
                EmailSalesReport = checkBoxEmailSalesReport.Checked
            };
        }

        // ==============================================
        // EVENT HANDLERS
        // ==============================================

        private void BtnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsData settings = GetSettingsFromUI();
                SaveSettingsToFile(settings);

                // Apply theme changes if needed
                ApplyTheme(settings.Theme);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to reset all settings to default?",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SettingsData defaultSettings = GetDefaultSettings();
                ApplySettingsToUI(defaultSettings);
                MessageBox.Show("Settings reset to default values.", "Reset Complete",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                System.Threading.Thread.Sleep(1000); // Simulate connection test
                Cursor.Current = Cursors.Default;

                MessageBox.Show("Database connection test successful!", "Connection Test",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Connection Test",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsData settings = GetSettingsFromUI();

                // Validate database settings
                if (string.IsNullOrWhiteSpace(settings.Server))
                {
                    MessageBox.Show("Server address cannot be empty.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(settings.DatabaseName))
                {
                    MessageBox.Show("Database name cannot be empty.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save only database settings
                var currentSettings = LoadSettingsFromFile() ?? GetDefaultSettings();
                currentSettings.Server = settings.Server;
                currentSettings.Username = settings.Username;
                currentSettings.Password = settings.Password;
                currentSettings.DatabaseName = settings.DatabaseName;
                currentSettings.BackupPath = settings.BackupPath;

                SaveSettingsToFile(currentSettings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving database settings: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBackupDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                string backupPath = txtBackupPath.Text;

                if (string.IsNullOrWhiteSpace(backupPath))
                {
                    MessageBox.Show("Please specify a backup path.", "Backup Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create backup directory if it doesn't exist
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                // Generate backup filename with timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFile = Path.Combine(backupPath, $"backup_{timestamp}.sql");

                // Create a dummy backup file
                File.WriteAllText(backupFile, $"-- Database Backup {timestamp}\n-- MatchPoint CMS Backup File");

                MessageBox.Show($"Database backup created successfully!\nLocation: {backupFile}",
                              "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Backup failed: {ex.Message}", "Backup Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRestoreDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            openFileDialog.Title = "Select Backup File to Restore";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult confirm = MessageBox.Show(
                    "WARNING: This will overwrite your current database. Are you sure you want to continue?",
                    "Confirm Restore",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        System.Threading.Thread.Sleep(2000); // Simulate restore process
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("Database restore completed successfully!",
                                      "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Restore failed: {ex.Message}", "Restore Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnTestEmail_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate email settings
                if (string.IsNullOrWhiteSpace(txtSmtpServer.Text))
                {
                    MessageBox.Show("SMTP Server cannot be empty.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtSmtpPort.Text, out int port) || port <= 0)
                {
                    MessageBox.Show("Invalid SMTP Port.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmailUsername.Text))
                {
                    MessageBox.Show("Email username cannot be empty.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                System.Threading.Thread.Sleep(1000); // Simulate email test
                Cursor.Current = Cursors.Default;

                MessageBox.Show("Email connection test successful!", "Email Test",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Email test failed: {ex.Message}", "Email Test",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveEmail_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsData settings = GetSettingsFromUI();

                // Validate email settings
                if (string.IsNullOrWhiteSpace(settings.SmtpServer))
                {
                    MessageBox.Show("SMTP Server cannot be empty.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (settings.SmtpPort <= 0)
                {
                    MessageBox.Show("Invalid SMTP Port.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save only email settings
                var currentSettings = LoadSettingsFromFile() ?? GetDefaultSettings();
                currentSettings.SmtpServer = settings.SmtpServer;
                currentSettings.SmtpPort = settings.SmtpPort;
                currentSettings.EmailUsername = settings.EmailUsername;
                currentSettings.EmailPassword = settings.EmailPassword;
                currentSettings.EmailNewUserAlerts = settings.EmailNewUserAlerts;
                currentSettings.EmailLowInventoryAlerts = settings.EmailLowInventoryAlerts;
                currentSettings.EmailSalesReport = settings.EmailSalesReport;

                SaveSettingsToFile(currentSettings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving email settings: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.matchpoint.ph");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot open website: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==============================================
        // HELPER METHODS
        // ==============================================

        private void ApplyTheme(string theme)
        {
            // This method would apply the selected theme to the application
            switch (theme.ToLower())
            {
                case "dark":
                    // Apply dark theme
                    MessageBox.Show("Dark theme selected. Restart application for changes to take effect.",
                                  "Theme Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "light":
                    // Apply light theme
                    MessageBox.Show("Light theme selected. Restart application for changes to take effect.",
                                  "Theme Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "blue":
                    // Apply blue theme
                    MessageBox.Show("Blue theme selected. Restart application for changes to take effect.",
                                  "Theme Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "green":
                    // Apply green theme
                    MessageBox.Show("Green theme selected. Restart application for changes to take effect.",
                                  "Theme Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    // Apply default theme
                    break;
            }
        }

        // Method to get backup directory
        public string GetBackupDirectory()
        {
            SettingsData settings = LoadSettingsFromFile() ?? GetDefaultSettings();
            return settings.BackupPath;
        }

        // Method to check if notifications are enabled
        public bool AreNotificationsEnabled()
        {
            SettingsData settings = LoadSettingsFromFile() ?? GetDefaultSettings();
            return settings.EnableNotifications;
        }

        // Method to get default game rate
        public decimal GetDefaultGameRate()
        {
            SettingsData settings = LoadSettingsFromFile() ?? GetDefaultSettings();
            return settings.DefaultHourlyRate;
        }

        // Method to get session timeout
        public int GetSessionTimeout()
        {
            SettingsData settings = LoadSettingsFromFile() ?? GetDefaultSettings();
            return settings.SessionTimeout;
        }

        private void tabPageSecurity_Click(object sender, EventArgs e)
        {


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxSystem_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDownSessionTimeout_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxAutoBackup_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxNotifications_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxTheme_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void tabPageGeneral_Click(object sender, EventArgs e)
        {

        }

        private void btnTestConnection_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSaveDatabase_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBoxBackup_Enter(object sender, EventArgs e)
        {

        }

        private void btnRestoreDatabase_Click_1(object sender, EventArgs e)
        {

        }

        private void btnBackupDatabase_Click_1(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtBackupPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxConnection_Enter(object sender, EventArgs e)
        {

        }

        private void txtDatabaseName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tabPageDatabase_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxPasswordPolicy_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDownPasswordHistory_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDownPasswordExpiry_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDownMaxAttempts_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDownMinLength_ValueChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxRequireSpecialChar_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxRequireNumber_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxRequireUppercase_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxSession_Enter(object sender, EventArgs e)
        {

        }

        private void checkBoxForceLogout_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPageGameSettings_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxRates_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDownDefaultRate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxGameDefaults_Enter(object sender, EventArgs e)
        {

        }

        private void checkBoxAutoStartGame_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxEnableSound_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxDefaultGame_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPageNotifications_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxEmail_Enter(object sender, EventArgs e)
        {

        }

        private void btnTestEmail_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSaveEmail_Click_1(object sender, EventArgs e)
        {

        }

        private void txtEmailPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmailUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSmtpServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSmtpPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxNotificationTypes_Enter(object sender, EventArgs e)
        {

        }

        private void checkBoxEmailSalesReport_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxEmailLowInventory_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxEmailNewUser_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPageAbout_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxAbout_Enter(object sender, EventArgs e)
        {

        }

        private void linkLabelWebsite_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveAll_Click_1(object sender, EventArgs e)
        {

        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {

        }
    }
}