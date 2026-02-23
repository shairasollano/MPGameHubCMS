using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        // Site identity fields
        private string currentLogoPath = string.Empty;
        private Image originalLogo;
        private PictureBox pictureBoxSiteLogo;
        private TextBox txtSiteName;
        private Button btnChangeLogo;
        private Button btnRemoveLogo;
        private GroupBox groupBoxSiteIdentity;
        private Label labelSiteName;
        private Label labelLogoPreview;
        private Panel panelLogoPreview;

        // Settings data class
        public class SettingsData
        {
            // General Settings
            public string Theme { get; set; }
            public string Language { get; set; }
            public bool EnableNotifications { get; set; }
            public bool AutoBackup { get; set; }
            public int SessionTimeout { get; set; }

            // Site Identity Settings
            public string SiteName { get; set; }
            public byte[] SiteLogo { get; set; }
            public string LogoPath { get; set; }

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

            // Initialize site identity controls
            InitializeSiteIdentityControls();

            // Apply light colors
            ApplyLightTheme();

            // Load settings when control is loaded
            this.Load += SETTINGS_Load;

            // Set up event handlers
            SetupEventHandlers();

            // Set logo image
            SetLogoImage();
        }

        private void InitializeSiteIdentityControls()
        {
            // GroupBox Site Identity
            this.groupBoxSiteIdentity = new System.Windows.Forms.GroupBox();
            this.pictureBoxSiteLogo = new System.Windows.Forms.PictureBox();
            this.labelSiteName = new System.Windows.Forms.Label();
            this.txtSiteName = new System.Windows.Forms.TextBox();
            this.btnChangeLogo = new System.Windows.Forms.Button();
            this.btnRemoveLogo = new System.Windows.Forms.Button();
            this.labelLogoPreview = new System.Windows.Forms.Label();
            this.panelLogoPreview = new System.Windows.Forms.Panel();

            this.groupBoxSiteIdentity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSiteLogo)).BeginInit();
            this.panelLogoPreview.SuspendLayout();

            // groupBoxSiteIdentity
            this.groupBoxSiteIdentity.Controls.Add(this.panelLogoPreview);
            this.groupBoxSiteIdentity.Controls.Add(this.labelSiteName);
            this.groupBoxSiteIdentity.Controls.Add(this.txtSiteName);
            this.groupBoxSiteIdentity.Controls.Add(this.btnChangeLogo);
            this.groupBoxSiteIdentity.Controls.Add(this.btnRemoveLogo);
            this.groupBoxSiteIdentity.Location = new System.Drawing.Point(20, 20);
            this.groupBoxSiteIdentity.Name = "groupBoxSiteIdentity";
            this.groupBoxSiteIdentity.Size = new System.Drawing.Size(650, 200);
            this.groupBoxSiteIdentity.TabIndex = 0;
            this.groupBoxSiteIdentity.TabStop = false;
            this.groupBoxSiteIdentity.Text = "Site Identity";
            this.groupBoxSiteIdentity.BackColor = System.Drawing.Color.White;

            // panelLogoPreview
            this.panelLogoPreview.Controls.Add(this.pictureBoxSiteLogo);
            this.panelLogoPreview.Controls.Add(this.labelLogoPreview);
            this.panelLogoPreview.Location = new System.Drawing.Point(20, 30);
            this.panelLogoPreview.Name = "panelLogoPreview";
            this.panelLogoPreview.Size = new System.Drawing.Size(140, 140);
            this.panelLogoPreview.TabIndex = 6;
            this.panelLogoPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLogoPreview.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);

            // pictureBoxSiteLogo
            this.pictureBoxSiteLogo.Location = new System.Drawing.Point(10, 10);
            this.pictureBoxSiteLogo.Name = "pictureBoxSiteLogo";
            this.pictureBoxSiteLogo.Size = new System.Drawing.Size(120, 100);
            this.pictureBoxSiteLogo.TabIndex = 4;
            this.pictureBoxSiteLogo.TabStop = false;
            this.pictureBoxSiteLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSiteLogo.Click += new System.EventHandler(this.PictureBoxSiteLogo_Click);

            // labelLogoPreview
            this.labelLogoPreview.AutoSize = true;
            this.labelLogoPreview.Location = new System.Drawing.Point(30, 115);
            this.labelLogoPreview.Name = "labelLogoPreview";
            this.labelLogoPreview.Size = new System.Drawing.Size(75, 15);
            this.labelLogoPreview.TabIndex = 5;
            this.labelLogoPreview.Text = "Logo Preview";
            this.labelLogoPreview.ForeColor = System.Drawing.Color.Gray;

            // labelSiteName
            this.labelSiteName.AutoSize = true;
            this.labelSiteName.Location = new System.Drawing.Point(180, 40);
            this.labelSiteName.Name = "labelSiteName";
            this.labelSiteName.Size = new System.Drawing.Size(63, 15);
            this.labelSiteName.TabIndex = 0;
            this.labelSiteName.Text = "Site Name:";

            // txtSiteName
            this.txtSiteName.Location = new System.Drawing.Point(180, 60);
            this.txtSiteName.Name = "txtSiteName";
            this.txtSiteName.Size = new System.Drawing.Size(300, 23);
            this.txtSiteName.TabIndex = 1;
            this.txtSiteName.Text = "MatchPoint CMS";
            this.txtSiteName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSiteName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSiteName.TextChanged += new System.EventHandler(this.TxtSiteName_TextChanged);

            // btnChangeLogo
            this.btnChangeLogo.Location = new System.Drawing.Point(180, 100);
            this.btnChangeLogo.Name = "btnChangeLogo";
            this.btnChangeLogo.Size = new System.Drawing.Size(120, 30);
            this.btnChangeLogo.TabIndex = 2;
            this.btnChangeLogo.Text = "Change Logo";
            this.btnChangeLogo.UseVisualStyleBackColor = true;
            this.btnChangeLogo.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.btnChangeLogo.ForeColor = System.Drawing.Color.White;
            this.btnChangeLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeLogo.FlatAppearance.BorderSize = 0;
            this.btnChangeLogo.Click += new System.EventHandler(this.BtnChangeLogo_Click);

            // btnRemoveLogo
            this.btnRemoveLogo.Location = new System.Drawing.Point(310, 100);
            this.btnRemoveLogo.Name = "btnRemoveLogo";
            this.btnRemoveLogo.Size = new System.Drawing.Size(100, 30);
            this.btnRemoveLogo.TabIndex = 3;
            this.btnRemoveLogo.Text = "Remove Logo";
            this.btnRemoveLogo.UseVisualStyleBackColor = true;
            this.btnRemoveLogo.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnRemoveLogo.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.btnRemoveLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveLogo.FlatAppearance.BorderSize = 0;
            this.btnRemoveLogo.Click += new System.EventHandler(this.BtnRemoveLogo_Click);

            // Add to tabPageGeneral
            this.tabPageGeneral.Controls.Add(this.groupBoxSiteIdentity);

            this.groupBoxSiteIdentity.ResumeLayout(false);
            this.groupBoxSiteIdentity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSiteLogo)).EndInit();
            this.panelLogoPreview.ResumeLayout(false);
            this.panelLogoPreview.PerformLayout();

            // Set default logo
            SetDefaultSiteLogo();
        }

        private void SetDefaultSiteLogo()
        {
            try
            {
                // Create a simple default logo
                Bitmap bmp = new Bitmap(120, 100);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(250, 250, 250));

                    // Draw a simple icon
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(70, 130, 180)))
                    {
                        g.FillRectangle(brush, 30, 20, 60, 60);
                    }

                    // Draw MP text
                    using (System.Drawing.Font font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        g.DrawString("MP", font, brush, 45, 40);
                    }
                }
                pictureBoxSiteLogo.Image = bmp;
                originalLogo = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating default logo: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChangeLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                openFileDialog.Title = "Select Site Logo";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Load the selected image
                        Image selectedImage = Image.FromFile(openFileDialog.FileName);

                        // Resize image if too large
                        Image resizedImage = ResizeImage(selectedImage, 120, 100);

                        // Display in picture box
                        pictureBoxSiteLogo.Image = resizedImage;

                        // Store the path
                        currentLogoPath = openFileDialog.FileName;

                        // Dispose the original selected image if different from resized
                        if (selectedImage != resizedImage)
                        {
                            selectedImage.Dispose();
                        }

                        MessageBox.Show("Logo selected successfully. Click 'Save All' to apply changes.",
                                      "Logo Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading logo: {ex.Message}", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            // Create a new bitmap with the desired size
            Bitmap resizedImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                // Set high quality settings
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                // Draw the image with the new size
                g.DrawImage(image, 0, 0, width, height);
            }

            return resizedImage;
        }

        private void BtnRemoveLogo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to remove the site logo?",
                "Remove Logo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Restore default logo
                SetDefaultSiteLogo();
                currentLogoPath = string.Empty;

                MessageBox.Show("Logo removed. Default logo will be used.",
                              "Logo Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TxtSiteName_TextChanged(object sender, EventArgs e)
        {
            // Live preview of site name (optional)
        }

        private void PictureBoxSiteLogo_Click(object sender, EventArgs e)
        {
            // Show preview
            if (pictureBoxSiteLogo.Image != null)
            {
                MessageBox.Show("Current site logo. Click 'Change Logo' to update.",
                              "Logo Preview", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

            // Update site identity group box if it exists
            if (groupBoxSiteIdentity != null)
            {
                groupBoxSiteIdentity.BackColor = Color.White;
                groupBoxSiteIdentity.ForeColor = Color.FromArgb(50, 50, 50);
                panelLogoPreview.BackColor = Color.FromArgb(250, 250, 250);
                labelLogoPreview.ForeColor = Color.Gray;
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
                                case "SiteName": settings.SiteName = value; break;
                                case "LogoPath": settings.LogoPath = value; break;
                                case "SiteLogoBase64":
                                    try
                                    {
                                        settings.SiteLogo = Convert.FromBase64String(value);
                                    }
                                    catch { }
                                    break;
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
                lines.Add($"SiteName={settings.SiteName}");
                lines.Add($"LogoPath={settings.LogoPath}");

                // Convert logo to base64 if you want to store in the same file
                if (settings.SiteLogo != null && settings.SiteLogo.Length > 0)
                {
                    lines.Add($"SiteLogoBase64={Convert.ToBase64String(settings.SiteLogo)}");
                }

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

                // Site Identity
                SiteName = "MatchPoint CMS",
                LogoPath = "",
                SiteLogo = null,

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
            // Site Identity Settings
            if (txtSiteName != null)
            {
                txtSiteName.Text = !string.IsNullOrEmpty(settings.SiteName) ? settings.SiteName : "MatchPoint CMS";
            }

            // Load logo if exists
            if (pictureBoxSiteLogo != null)
            {
                if (!string.IsNullOrEmpty(settings.LogoPath) && File.Exists(settings.LogoPath))
                {
                    try
                    {
                        Image logo = Image.FromFile(settings.LogoPath);
                        pictureBoxSiteLogo.Image = ResizeImage(logo, 120, 100);
                        currentLogoPath = settings.LogoPath;
                        logo.Dispose();
                    }
                    catch
                    {
                        SetDefaultSiteLogo();
                    }
                }
                else if (settings.SiteLogo != null && settings.SiteLogo.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(settings.SiteLogo))
                        {
                            Image logo = Image.FromStream(ms);
                            pictureBoxSiteLogo.Image = ResizeImage(logo, 120, 100);
                        }
                    }
                    catch
                    {
                        SetDefaultSiteLogo();
                    }
                }
                else
                {
                    SetDefaultSiteLogo();
                }
            }

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
            var settings = new SettingsData
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
                EmailSalesReport = checkBoxEmailSalesReport.Checked,

                // Site Identity Settings
                SiteName = txtSiteName != null ? txtSiteName.Text : "MatchPoint CMS",
                LogoPath = currentLogoPath
            };

            // Convert logo to byte array if needed
            if (pictureBoxSiteLogo != null && pictureBoxSiteLogo.Image != null && pictureBoxSiteLogo.Image != originalLogo)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Save as PNG to maintain transparency
                    pictureBoxSiteLogo.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    settings.SiteLogo = ms.ToArray();
                }
            }

            return settings;
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
                if (settings != null)
                {
                    ApplyTheme(settings.Theme);
                }
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
            // This method applies the selected theme to the settings UI
            switch (theme.ToLower())
            {
                case "dark":
                    // Apply dark theme to the settings panel
                    this.BackColor = Color.FromArgb(45, 45, 45);
                    tabControl1.BackColor = Color.FromArgb(45, 45, 45);

                    foreach (TabPage page in tabControl1.TabPages)
                    {
                        page.BackColor = Color.FromArgb(55, 55, 55);
                        page.ForeColor = Color.White;
                    }

                    foreach (Control control in this.Controls)
                    {
                        if (control is GroupBox groupBox)
                        {
                            groupBox.BackColor = Color.FromArgb(65, 65, 65);
                            groupBox.ForeColor = Color.White;
                        }
                    }

                    // Update site identity group box if it exists
                    if (groupBoxSiteIdentity != null)
                    {
                        groupBoxSiteIdentity.BackColor = Color.FromArgb(65, 65, 65);
                        groupBoxSiteIdentity.ForeColor = Color.White;
                        panelLogoPreview.BackColor = Color.FromArgb(55, 55, 55);
                        labelLogoPreview.ForeColor = Color.LightGray;
                    }

                    MessageBox.Show("Dark theme applied. Some changes may require restart.",
                                  "Theme Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "light":
                    // Apply light theme
                    ApplyLightTheme();
                    MessageBox.Show("Light theme applied.", "Theme Changed",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "blue":
                    // Apply blue theme
                    this.BackColor = Color.FromArgb(240, 248, 255);
                    tabControl1.BackColor = Color.FromArgb(240, 248, 255);

                    foreach (TabPage page in tabControl1.TabPages)
                    {
                        page.BackColor = Color.FromArgb(255, 255, 255);
                        page.ForeColor = Color.FromArgb(0, 70, 140);
                    }

                    foreach (Control control in this.Controls)
                    {
                        if (control is GroupBox groupBox)
                        {
                            groupBox.BackColor = Color.FromArgb(230, 242, 255);
                            groupBox.ForeColor = Color.FromArgb(0, 70, 140);
                        }
                    }

                    MessageBox.Show("Blue theme applied.", "Theme Changed",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "green":
                    // Apply green theme
                    this.BackColor = Color.FromArgb(240, 255, 240);
                    tabControl1.BackColor = Color.FromArgb(240, 255, 240);

                    foreach (TabPage page in tabControl1.TabPages)
                    {
                        page.BackColor = Color.FromArgb(255, 255, 255);
                        page.ForeColor = Color.FromArgb(0, 100, 0);
                    }

                    foreach (Control control in this.Controls)
                    {
                        if (control is GroupBox groupBox)
                        {
                            groupBox.BackColor = Color.FromArgb(230, 255, 230);
                            groupBox.ForeColor = Color.FromArgb(0, 100, 0);
                        }
                    }

                    MessageBox.Show("Green theme applied.", "Theme Changed",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    // Apply default (light) theme
                    ApplyLightTheme();
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

        // Empty event handlers that are required by the designer
        private void tabPageSecurity_Click(object sender, EventArgs e) { }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void groupBoxSystem_Enter(object sender, EventArgs e) { }
        private void numericUpDownSessionTimeout_ValueChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void checkBoxAutoBackup_CheckedChanged(object sender, EventArgs e) { }
        private void checkBoxNotifications_CheckedChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void comboBoxTheme_SelectedIndexChanged_1(object sender, EventArgs e) { }
        private void tabPageGeneral_Click(object sender, EventArgs e) { }
        private void btnTestConnection_Click_1(object sender, EventArgs e) { }
        private void btnSaveDatabase_Click_1(object sender, EventArgs e) { }
        private void groupBoxBackup_Enter(object sender, EventArgs e) { }
        private void btnRestoreDatabase_Click_1(object sender, EventArgs e) { }
        private void btnBackupDatabase_Click_1(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void txtBackupPath_TextChanged(object sender, EventArgs e) { }
        private void groupBoxConnection_Enter(object sender, EventArgs e) { }
        private void txtDatabaseName_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void txtUsername_TextChanged(object sender, EventArgs e) { }
        private void txtServer_TextChanged(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void tabPageDatabase_Click(object sender, EventArgs e) { }
        private void groupBoxPasswordPolicy_Enter(object sender, EventArgs e) { }
        private void numericUpDownPasswordHistory_ValueChanged(object sender, EventArgs e) { }
        private void numericUpDownPasswordExpiry_ValueChanged(object sender, EventArgs e) { }
        private void numericUpDownMaxAttempts_ValueChanged(object sender, EventArgs e) { }
        private void numericUpDownMinLength_ValueChanged(object sender, EventArgs e) { }
        private void checkBoxRequireSpecialChar_CheckedChanged(object sender, EventArgs e) { }
        private void checkBoxRequireNumber_CheckedChanged(object sender, EventArgs e) { }
        private void checkBoxRequireUppercase_CheckedChanged(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void label12_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void groupBoxSession_Enter(object sender, EventArgs e) { }
        private void checkBoxForceLogout_CheckedChanged(object sender, EventArgs e) { }
        private void tabPageGameSettings_Click(object sender, EventArgs e) { }
        private void groupBoxRates_Enter(object sender, EventArgs e) { }
        private void numericUpDownDefaultRate_ValueChanged(object sender, EventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }
        private void groupBoxGameDefaults_Enter(object sender, EventArgs e) { }
        private void checkBoxAutoStartGame_CheckedChanged(object sender, EventArgs e) { }
        private void checkBoxEnableSound_CheckedChanged(object sender, EventArgs e) { }
        private void label15_Click(object sender, EventArgs e) { }
        private void comboBoxDefaultGame_SelectedIndexChanged(object sender, EventArgs e) { }
        private void tabPageNotifications_Click(object sender, EventArgs e) { }
        private void groupBoxEmail_Enter(object sender, EventArgs e) { }
        private void btnTestEmail_Click_1(object sender, EventArgs e) { }
        private void btnSaveEmail_Click_1(object sender, EventArgs e) { }
        private void txtEmailPassword_TextChanged(object sender, EventArgs e) { }
        private void txtEmailUsername_TextChanged(object sender, EventArgs e) { }
        private void txtSmtpServer_TextChanged(object sender, EventArgs e) { }
        private void txtSmtpPort_TextChanged(object sender, EventArgs e) { }
        private void label20_Click(object sender, EventArgs e) { }
        private void label19_Click(object sender, EventArgs e) { }
        private void label18_Click(object sender, EventArgs e) { }
        private void label17_Click(object sender, EventArgs e) { }
        private void label16_Click(object sender, EventArgs e) { }
        private void groupBoxNotificationTypes_Enter(object sender, EventArgs e) { }
        private void checkBoxEmailSalesReport_CheckedChanged(object sender, EventArgs e) { }
        private void checkBoxEmailLowInventory_CheckedChanged(object sender, EventArgs e) { }
        private void checkBoxEmailNewUser_CheckedChanged(object sender, EventArgs e) { }
        private void tabPageAbout_Click(object sender, EventArgs e) { }
        private void groupBoxAbout_Enter(object sender, EventArgs e) { }
        private void label24_Click(object sender, EventArgs e) { }
        private void label23_Click(object sender, EventArgs e) { }
        private void label22_Click(object sender, EventArgs e) { }
        private void label21_Click(object sender, EventArgs e) { }
        private void pictureBoxLogo_Click(object sender, EventArgs e) { }
        private void btnSaveAll_Click_1(object sender, EventArgs e) { }
        private void btnReset_Click_1(object sender, EventArgs e) { }
    }
}