namespace cms
{
    partial class SETTINGS
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxSystem = new System.Windows.Forms.GroupBox();
            this.numericUpDownSessionTimeout = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxAutoBackup = new System.Windows.Forms.CheckBox();
            this.checkBoxNotifications = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTheme = new System.Windows.Forms.ComboBox();
            this.tabPageDatabase = new System.Windows.Forms.TabPage();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnSaveDatabase = new System.Windows.Forms.Button();
            this.groupBoxBackup = new System.Windows.Forms.GroupBox();
            this.btnRestoreDatabase = new System.Windows.Forms.Button();
            this.btnBackupDatabase = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBackupPath = new System.Windows.Forms.TextBox();
            this.groupBoxConnection = new System.Windows.Forms.GroupBox();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPageSecurity = new System.Windows.Forms.TabPage();
            this.groupBoxPasswordPolicy = new System.Windows.Forms.GroupBox();
            this.numericUpDownPasswordHistory = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPasswordExpiry = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxAttempts = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMinLength = new System.Windows.Forms.NumericUpDown();
            this.checkBoxRequireSpecialChar = new System.Windows.Forms.CheckBox();
            this.checkBoxRequireNumber = new System.Windows.Forms.CheckBox();
            this.checkBoxRequireUppercase = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBoxSession = new System.Windows.Forms.GroupBox();
            this.checkBoxForceLogout = new System.Windows.Forms.CheckBox();
            this.tabPageNotifications = new System.Windows.Forms.TabPage();
            this.groupBoxEmail = new System.Windows.Forms.GroupBox();
            this.btnTestEmail = new System.Windows.Forms.Button();
            this.btnSaveEmail = new System.Windows.Forms.Button();
            this.txtEmailPassword = new System.Windows.Forms.TextBox();
            this.txtEmailUsername = new System.Windows.Forms.TextBox();
            this.txtSmtpServer = new System.Windows.Forms.TextBox();
            this.txtSmtpPort = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBoxNotificationTypes = new System.Windows.Forms.GroupBox();
            this.checkBoxEmailSalesReport = new System.Windows.Forms.CheckBox();
            this.checkBoxEmailLowInventory = new System.Windows.Forms.CheckBox();
            this.checkBoxEmailNewUser = new System.Windows.Forms.CheckBox();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.groupBoxAbout = new System.Windows.Forms.GroupBox();
            this.linkLabelWebsite = new System.Windows.Forms.LinkLabel();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBoxSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSessionTimeout)).BeginInit();
            this.tabPageDatabase.SuspendLayout();
            this.groupBoxBackup.SuspendLayout();
            this.groupBoxConnection.SuspendLayout();
            this.tabPageSecurity.SuspendLayout();
            this.groupBoxPasswordPolicy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPasswordHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPasswordExpiry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxAttempts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinLength)).BeginInit();
            this.groupBoxSession.SuspendLayout();
            this.tabPageNotifications.SuspendLayout();
            this.groupBoxEmail.SuspendLayout();
            this.groupBoxNotificationTypes.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            this.groupBoxAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageDatabase);
            this.tabControl1.Controls.Add(this.tabPageSecurity);
            this.tabControl1.Controls.Add(this.tabPageNotifications);
            this.tabControl1.Controls.Add(this.tabPageAbout);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1727, 739);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tabPageGeneral.Controls.Add(this.groupBoxSystem);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 32);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(20);
            this.tabPageGeneral.Size = new System.Drawing.Size(1719, 703);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.Click += new System.EventHandler(this.tabPageGeneral_Click);
            // 
            // groupBoxSystem
            // 
            this.groupBoxSystem.BackColor = System.Drawing.Color.White;
            this.groupBoxSystem.Controls.Add(this.numericUpDownSessionTimeout);
            this.groupBoxSystem.Controls.Add(this.label3);
            this.groupBoxSystem.Controls.Add(this.checkBoxAutoBackup);
            this.groupBoxSystem.Controls.Add(this.checkBoxNotifications);
            this.groupBoxSystem.Controls.Add(this.label2);
            this.groupBoxSystem.Controls.Add(this.comboBoxLanguage);
            this.groupBoxSystem.Controls.Add(this.label1);
            this.groupBoxSystem.Controls.Add(this.comboBoxTheme);
            this.groupBoxSystem.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSystem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxSystem.Location = new System.Drawing.Point(30, 30);
            this.groupBoxSystem.Name = "groupBoxSystem";
            this.groupBoxSystem.Size = new System.Drawing.Size(650, 350);
            this.groupBoxSystem.TabIndex = 0;
            this.groupBoxSystem.TabStop = false;
            this.groupBoxSystem.Text = "System Settings";
            this.groupBoxSystem.Enter += new System.EventHandler(this.groupBoxSystem_Enter);
            // 
            // numericUpDownSessionTimeout
            // 
            this.numericUpDownSessionTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.numericUpDownSessionTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownSessionTimeout.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownSessionTimeout.Location = new System.Drawing.Point(250, 160);
            this.numericUpDownSessionTimeout.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numericUpDownSessionTimeout.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownSessionTimeout.Name = "numericUpDownSessionTimeout";
            this.numericUpDownSessionTimeout.Size = new System.Drawing.Size(120, 30);
            this.numericUpDownSessionTimeout.TabIndex = 7;
            this.numericUpDownSessionTimeout.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownSessionTimeout.ValueChanged += new System.EventHandler(this.numericUpDownSessionTimeout_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label3.Location = new System.Drawing.Point(30, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Session Timeout:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // checkBoxAutoBackup
            // 
            this.checkBoxAutoBackup.AutoSize = true;
            this.checkBoxAutoBackup.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAutoBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxAutoBackup.Location = new System.Drawing.Point(250, 250);
            this.checkBoxAutoBackup.Name = "checkBoxAutoBackup";
            this.checkBoxAutoBackup.Size = new System.Drawing.Size(129, 27);
            this.checkBoxAutoBackup.TabIndex = 5;
            this.checkBoxAutoBackup.Text = "Auto Backup";
            this.checkBoxAutoBackup.UseVisualStyleBackColor = true;
            this.checkBoxAutoBackup.CheckedChanged += new System.EventHandler(this.checkBoxAutoBackup_CheckedChanged);
            // 
            // checkBoxNotifications
            // 
            this.checkBoxNotifications.AutoSize = true;
            this.checkBoxNotifications.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNotifications.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxNotifications.Location = new System.Drawing.Point(250, 220);
            this.checkBoxNotifications.Name = "checkBoxNotifications";
            this.checkBoxNotifications.Size = new System.Drawing.Size(184, 27);
            this.checkBoxNotifications.TabIndex = 4;
            this.checkBoxNotifications.Text = "Enable Notifications";
            this.checkBoxNotifications.UseVisualStyleBackColor = true;
            this.checkBoxNotifications.CheckedChanged += new System.EventHandler(this.checkBoxNotifications_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label2.Location = new System.Drawing.Point(30, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Language:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxLanguage.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Items.AddRange(new object[] {
            "English",
            "Filipino"});
            this.comboBoxLanguage.Location = new System.Drawing.Point(250, 97);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(250, 31);
            this.comboBoxLanguage.TabIndex = 2;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguage_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label1.Location = new System.Drawing.Point(30, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Theme:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.comboBoxTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxTheme.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTheme.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.comboBoxTheme.FormattingEnabled = true;
            this.comboBoxTheme.Items.AddRange(new object[] {
            "Light",
            "Dark",
            "Blue",
            "Green"});
            this.comboBoxTheme.Location = new System.Drawing.Point(250, 37);
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.Size = new System.Drawing.Size(250, 31);
            this.comboBoxTheme.TabIndex = 0;
            this.comboBoxTheme.SelectedIndexChanged += new System.EventHandler(this.comboBoxTheme_SelectedIndexChanged_1);
            // 
            // tabPageDatabase
            // 
            this.tabPageDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tabPageDatabase.Controls.Add(this.btnTestConnection);
            this.tabPageDatabase.Controls.Add(this.btnSaveDatabase);
            this.tabPageDatabase.Controls.Add(this.groupBoxBackup);
            this.tabPageDatabase.Controls.Add(this.groupBoxConnection);
            this.tabPageDatabase.Location = new System.Drawing.Point(4, 32);
            this.tabPageDatabase.Name = "tabPageDatabase";
            this.tabPageDatabase.Padding = new System.Windows.Forms.Padding(20);
            this.tabPageDatabase.Size = new System.Drawing.Size(1719, 703);
            this.tabPageDatabase.TabIndex = 1;
            this.tabPageDatabase.Text = "Database";
            this.tabPageDatabase.Click += new System.EventHandler(this.tabPageDatabase_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(91)))), ((int)(((byte)(86)))));
            this.btnTestConnection.FlatAppearance.BorderSize = 0;
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnection.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestConnection.ForeColor = System.Drawing.Color.White;
            this.btnTestConnection.Location = new System.Drawing.Point(410, 320);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(200, 45);
            this.btnTestConnection.TabIndex = 3;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click_1);
            // 
            // btnSaveDatabase
            // 
            this.btnSaveDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.btnSaveDatabase.FlatAppearance.BorderSize = 0;
            this.btnSaveDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDatabase.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDatabase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.btnSaveDatabase.Location = new System.Drawing.Point(180, 320);
            this.btnSaveDatabase.Name = "btnSaveDatabase";
            this.btnSaveDatabase.Size = new System.Drawing.Size(200, 45);
            this.btnSaveDatabase.TabIndex = 2;
            this.btnSaveDatabase.Text = "Save Settings";
            this.btnSaveDatabase.UseVisualStyleBackColor = false;
            this.btnSaveDatabase.Click += new System.EventHandler(this.btnSaveDatabase_Click_1);
            // 
            // groupBoxBackup
            // 
            this.groupBoxBackup.BackColor = System.Drawing.Color.White;
            this.groupBoxBackup.Controls.Add(this.btnRestoreDatabase);
            this.groupBoxBackup.Controls.Add(this.btnBackupDatabase);
            this.groupBoxBackup.Controls.Add(this.label8);
            this.groupBoxBackup.Controls.Add(this.txtBackupPath);
            this.groupBoxBackup.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxBackup.Location = new System.Drawing.Point(630, 30);
            this.groupBoxBackup.Name = "groupBoxBackup";
            this.groupBoxBackup.Size = new System.Drawing.Size(600, 280);
            this.groupBoxBackup.TabIndex = 1;
            this.groupBoxBackup.TabStop = false;
            this.groupBoxBackup.Text = "Backup & Restore";
            this.groupBoxBackup.Enter += new System.EventHandler(this.groupBoxBackup_Enter);
            // 
            // btnRestoreDatabase
            // 
            this.btnRestoreDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnRestoreDatabase.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnRestoreDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreDatabase.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreDatabase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnRestoreDatabase.Location = new System.Drawing.Point(320, 190);
            this.btnRestoreDatabase.Name = "btnRestoreDatabase";
            this.btnRestoreDatabase.Size = new System.Drawing.Size(220, 45);
            this.btnRestoreDatabase.TabIndex = 3;
            this.btnRestoreDatabase.Text = "Restore Database";
            this.btnRestoreDatabase.UseVisualStyleBackColor = false;
            this.btnRestoreDatabase.Click += new System.EventHandler(this.btnRestoreDatabase_Click_1);
            // 
            // btnBackupDatabase
            // 
            this.btnBackupDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnBackupDatabase.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnBackupDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackupDatabase.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackupDatabase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnBackupDatabase.Location = new System.Drawing.Point(60, 190);
            this.btnBackupDatabase.Name = "btnBackupDatabase";
            this.btnBackupDatabase.Size = new System.Drawing.Size(220, 45);
            this.btnBackupDatabase.TabIndex = 2;
            this.btnBackupDatabase.Text = "Backup Database";
            this.btnBackupDatabase.UseVisualStyleBackColor = false;
            this.btnBackupDatabase.Click += new System.EventHandler(this.btnBackupDatabase_Click_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label8.Location = new System.Drawing.Point(30, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 25);
            this.label8.TabIndex = 1;
            this.label8.Text = "Backup Path:";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // txtBackupPath
            // 
            this.txtBackupPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtBackupPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBackupPath.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBackupPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtBackupPath.Location = new System.Drawing.Point(180, 57);
            this.txtBackupPath.Name = "txtBackupPath";
            this.txtBackupPath.Size = new System.Drawing.Size(380, 30);
            this.txtBackupPath.TabIndex = 0;
            this.txtBackupPath.TextChanged += new System.EventHandler(this.txtBackupPath_TextChanged);
            // 
            // groupBoxConnection
            // 
            this.groupBoxConnection.BackColor = System.Drawing.Color.White;
            this.groupBoxConnection.Controls.Add(this.txtDatabaseName);
            this.groupBoxConnection.Controls.Add(this.txtPassword);
            this.groupBoxConnection.Controls.Add(this.txtUsername);
            this.groupBoxConnection.Controls.Add(this.txtServer);
            this.groupBoxConnection.Controls.Add(this.label7);
            this.groupBoxConnection.Controls.Add(this.label6);
            this.groupBoxConnection.Controls.Add(this.label5);
            this.groupBoxConnection.Controls.Add(this.label4);
            this.groupBoxConnection.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxConnection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxConnection.Location = new System.Drawing.Point(30, 30);
            this.groupBoxConnection.Name = "groupBoxConnection";
            this.groupBoxConnection.Size = new System.Drawing.Size(580, 280);
            this.groupBoxConnection.TabIndex = 0;
            this.groupBoxConnection.TabStop = false;
            this.groupBoxConnection.Text = "Database Connection";
            this.groupBoxConnection.Enter += new System.EventHandler(this.groupBoxConnection_Enter);
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDatabaseName.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDatabaseName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtDatabaseName.Location = new System.Drawing.Point(220, 180);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(320, 30);
            this.txtDatabaseName.TabIndex = 7;
            this.txtDatabaseName.TextChanged += new System.EventHandler(this.txtDatabaseName_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtPassword.Location = new System.Drawing.Point(220, 140);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(320, 30);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtUsername.Location = new System.Drawing.Point(220, 100);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(320, 30);
            this.txtUsername.TabIndex = 5;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // txtServer
            // 
            this.txtServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServer.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtServer.Location = new System.Drawing.Point(220, 60);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(320, 30);
            this.txtServer.TabIndex = 4;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label7.Location = new System.Drawing.Point(30, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 25);
            this.label7.TabIndex = 3;
            this.label7.Text = "Database Name:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label6.Location = new System.Drawing.Point(30, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 25);
            this.label6.TabIndex = 2;
            this.label6.Text = "Password:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label5.Location = new System.Drawing.Point(30, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Username:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label4.Location = new System.Drawing.Point(30, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Server:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // tabPageSecurity
            // 
            this.tabPageSecurity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tabPageSecurity.Controls.Add(this.groupBoxPasswordPolicy);
            this.tabPageSecurity.Controls.Add(this.groupBoxSession);
            this.tabPageSecurity.Location = new System.Drawing.Point(4, 32);
            this.tabPageSecurity.Name = "tabPageSecurity";
            this.tabPageSecurity.Padding = new System.Windows.Forms.Padding(20);
            this.tabPageSecurity.Size = new System.Drawing.Size(1719, 703);
            this.tabPageSecurity.TabIndex = 2;
            this.tabPageSecurity.Text = "Security";
            this.tabPageSecurity.Click += new System.EventHandler(this.tabPageSecurity_Click);
            // 
            // groupBoxPasswordPolicy
            // 
            this.groupBoxPasswordPolicy.BackColor = System.Drawing.Color.White;
            this.groupBoxPasswordPolicy.Controls.Add(this.numericUpDownPasswordHistory);
            this.groupBoxPasswordPolicy.Controls.Add(this.numericUpDownPasswordExpiry);
            this.groupBoxPasswordPolicy.Controls.Add(this.numericUpDownMaxAttempts);
            this.groupBoxPasswordPolicy.Controls.Add(this.numericUpDownMinLength);
            this.groupBoxPasswordPolicy.Controls.Add(this.checkBoxRequireSpecialChar);
            this.groupBoxPasswordPolicy.Controls.Add(this.checkBoxRequireNumber);
            this.groupBoxPasswordPolicy.Controls.Add(this.checkBoxRequireUppercase);
            this.groupBoxPasswordPolicy.Controls.Add(this.label13);
            this.groupBoxPasswordPolicy.Controls.Add(this.label12);
            this.groupBoxPasswordPolicy.Controls.Add(this.label11);
            this.groupBoxPasswordPolicy.Controls.Add(this.label10);
            this.groupBoxPasswordPolicy.Controls.Add(this.label9);
            this.groupBoxPasswordPolicy.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPasswordPolicy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxPasswordPolicy.Location = new System.Drawing.Point(30, 30);
            this.groupBoxPasswordPolicy.Name = "groupBoxPasswordPolicy";
            this.groupBoxPasswordPolicy.Size = new System.Drawing.Size(650, 380);
            this.groupBoxPasswordPolicy.TabIndex = 1;
            this.groupBoxPasswordPolicy.TabStop = false;
            this.groupBoxPasswordPolicy.Text = "Password Policy";
            this.groupBoxPasswordPolicy.Enter += new System.EventHandler(this.groupBoxPasswordPolicy_Enter);
            // 
            // numericUpDownPasswordHistory
            // 
            this.numericUpDownPasswordHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.numericUpDownPasswordHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPasswordHistory.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPasswordHistory.Location = new System.Drawing.Point(280, 280);
            this.numericUpDownPasswordHistory.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDownPasswordHistory.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPasswordHistory.Name = "numericUpDownPasswordHistory";
            this.numericUpDownPasswordHistory.Size = new System.Drawing.Size(120, 30);
            this.numericUpDownPasswordHistory.TabIndex = 12;
            this.numericUpDownPasswordHistory.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownPasswordHistory.ValueChanged += new System.EventHandler(this.numericUpDownPasswordHistory_ValueChanged);
            // 
            // numericUpDownPasswordExpiry
            // 
            this.numericUpDownPasswordExpiry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.numericUpDownPasswordExpiry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPasswordExpiry.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPasswordExpiry.Location = new System.Drawing.Point(280, 240);
            this.numericUpDownPasswordExpiry.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numericUpDownPasswordExpiry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPasswordExpiry.Name = "numericUpDownPasswordExpiry";
            this.numericUpDownPasswordExpiry.Size = new System.Drawing.Size(120, 30);
            this.numericUpDownPasswordExpiry.TabIndex = 11;
            this.numericUpDownPasswordExpiry.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownPasswordExpiry.ValueChanged += new System.EventHandler(this.numericUpDownPasswordExpiry_ValueChanged);
            // 
            // numericUpDownMaxAttempts
            // 
            this.numericUpDownMaxAttempts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.numericUpDownMaxAttempts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownMaxAttempts.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownMaxAttempts.Location = new System.Drawing.Point(280, 200);
            this.numericUpDownMaxAttempts.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMaxAttempts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMaxAttempts.Name = "numericUpDownMaxAttempts";
            this.numericUpDownMaxAttempts.Size = new System.Drawing.Size(120, 30);
            this.numericUpDownMaxAttempts.TabIndex = 10;
            this.numericUpDownMaxAttempts.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownMaxAttempts.ValueChanged += new System.EventHandler(this.numericUpDownMaxAttempts_ValueChanged);
            // 
            // numericUpDownMinLength
            // 
            this.numericUpDownMinLength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.numericUpDownMinLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownMinLength.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownMinLength.Location = new System.Drawing.Point(280, 160);
            this.numericUpDownMinLength.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownMinLength.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownMinLength.Name = "numericUpDownMinLength";
            this.numericUpDownMinLength.Size = new System.Drawing.Size(120, 30);
            this.numericUpDownMinLength.TabIndex = 9;
            this.numericUpDownMinLength.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDownMinLength.ValueChanged += new System.EventHandler(this.numericUpDownMinLength_ValueChanged);
            // 
            // checkBoxRequireSpecialChar
            // 
            this.checkBoxRequireSpecialChar.AutoSize = true;
            this.checkBoxRequireSpecialChar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRequireSpecialChar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxRequireSpecialChar.Location = new System.Drawing.Point(40, 130);
            this.checkBoxRequireSpecialChar.Name = "checkBoxRequireSpecialChar";
            this.checkBoxRequireSpecialChar.Size = new System.Drawing.Size(296, 27);
            this.checkBoxRequireSpecialChar.TabIndex = 8;
            this.checkBoxRequireSpecialChar.Text = "Require Special Character (!@#$%)";
            this.checkBoxRequireSpecialChar.UseVisualStyleBackColor = true;
            this.checkBoxRequireSpecialChar.CheckedChanged += new System.EventHandler(this.checkBoxRequireSpecialChar_CheckedChanged);
            // 
            // checkBoxRequireNumber
            // 
            this.checkBoxRequireNumber.AutoSize = true;
            this.checkBoxRequireNumber.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRequireNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxRequireNumber.Location = new System.Drawing.Point(40, 100);
            this.checkBoxRequireNumber.Name = "checkBoxRequireNumber";
            this.checkBoxRequireNumber.Size = new System.Drawing.Size(158, 27);
            this.checkBoxRequireNumber.TabIndex = 7;
            this.checkBoxRequireNumber.Text = "Require Number";
            this.checkBoxRequireNumber.UseVisualStyleBackColor = true;
            this.checkBoxRequireNumber.CheckedChanged += new System.EventHandler(this.checkBoxRequireNumber_CheckedChanged);
            // 
            // checkBoxRequireUppercase
            // 
            this.checkBoxRequireUppercase.AutoSize = true;
            this.checkBoxRequireUppercase.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRequireUppercase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxRequireUppercase.Location = new System.Drawing.Point(40, 70);
            this.checkBoxRequireUppercase.Name = "checkBoxRequireUppercase";
            this.checkBoxRequireUppercase.Size = new System.Drawing.Size(175, 27);
            this.checkBoxRequireUppercase.TabIndex = 6;
            this.checkBoxRequireUppercase.Text = "Require Uppercase";
            this.checkBoxRequireUppercase.UseVisualStyleBackColor = true;
            this.checkBoxRequireUppercase.CheckedChanged += new System.EventHandler(this.checkBoxRequireUppercase_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label13.Location = new System.Drawing.Point(40, 282);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(199, 25);
            this.label13.TabIndex = 5;
            this.label13.Text = "Password History (Last):";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label12.Location = new System.Drawing.Point(40, 242);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(197, 25);
            this.label12.TabIndex = 4;
            this.label12.Text = "Password Expiry (Days):";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label11.Location = new System.Drawing.Point(40, 202);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(177, 25);
            this.label11.TabIndex = 3;
            this.label11.Text = "Max Login Attempts:";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label10.Location = new System.Drawing.Point(40, 162);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(151, 25);
            this.label10.TabIndex = 2;
            this.label10.Text = "Minimum Length:";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(20, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(214, 25);
            this.label9.TabIndex = 1;
            this.label9.Text = "Password Requirements";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // groupBoxSession
            // 
            this.groupBoxSession.BackColor = System.Drawing.Color.White;
            this.groupBoxSession.Controls.Add(this.checkBoxForceLogout);
            this.groupBoxSession.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSession.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxSession.Location = new System.Drawing.Point(750, 30);
            this.groupBoxSession.Name = "groupBoxSession";
            this.groupBoxSession.Size = new System.Drawing.Size(450, 180);
            this.groupBoxSession.TabIndex = 0;
            this.groupBoxSession.TabStop = false;
            this.groupBoxSession.Text = "Session Security";
            this.groupBoxSession.Enter += new System.EventHandler(this.groupBoxSession_Enter);
            // 
            // checkBoxForceLogout
            // 
            this.checkBoxForceLogout.AutoSize = true;
            this.checkBoxForceLogout.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxForceLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxForceLogout.Location = new System.Drawing.Point(40, 70);
            this.checkBoxForceLogout.Name = "checkBoxForceLogout";
            this.checkBoxForceLogout.Size = new System.Drawing.Size(296, 27);
            this.checkBoxForceLogout.TabIndex = 0;
            this.checkBoxForceLogout.Text = "Force Logout on Password Change";
            this.checkBoxForceLogout.UseVisualStyleBackColor = true;
            this.checkBoxForceLogout.CheckedChanged += new System.EventHandler(this.checkBoxForceLogout_CheckedChanged);
            // 
            // tabPageNotifications
            // 
            this.tabPageNotifications.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tabPageNotifications.Controls.Add(this.groupBoxEmail);
            this.tabPageNotifications.Controls.Add(this.groupBoxNotificationTypes);
            this.tabPageNotifications.Location = new System.Drawing.Point(4, 32);
            this.tabPageNotifications.Name = "tabPageNotifications";
            this.tabPageNotifications.Padding = new System.Windows.Forms.Padding(20);
            this.tabPageNotifications.Size = new System.Drawing.Size(1719, 703);
            this.tabPageNotifications.TabIndex = 4;
            this.tabPageNotifications.Text = "Notifications";
            this.tabPageNotifications.Click += new System.EventHandler(this.tabPageNotifications_Click);
            // 
            // groupBoxEmail
            // 
            this.groupBoxEmail.BackColor = System.Drawing.Color.White;
            this.groupBoxEmail.Controls.Add(this.btnTestEmail);
            this.groupBoxEmail.Controls.Add(this.btnSaveEmail);
            this.groupBoxEmail.Controls.Add(this.txtEmailPassword);
            this.groupBoxEmail.Controls.Add(this.txtEmailUsername);
            this.groupBoxEmail.Controls.Add(this.txtSmtpServer);
            this.groupBoxEmail.Controls.Add(this.txtSmtpPort);
            this.groupBoxEmail.Controls.Add(this.label20);
            this.groupBoxEmail.Controls.Add(this.label19);
            this.groupBoxEmail.Controls.Add(this.label18);
            this.groupBoxEmail.Controls.Add(this.label17);
            this.groupBoxEmail.Controls.Add(this.label16);
            this.groupBoxEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxEmail.Location = new System.Drawing.Point(30, 30);
            this.groupBoxEmail.Name = "groupBoxEmail";
            this.groupBoxEmail.Size = new System.Drawing.Size(650, 380);
            this.groupBoxEmail.TabIndex = 1;
            this.groupBoxEmail.TabStop = false;
            this.groupBoxEmail.Text = "Email Configuration";
            this.groupBoxEmail.Enter += new System.EventHandler(this.groupBoxEmail_Enter);
            // 
            // btnTestEmail
            // 
            this.btnTestEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(91)))), ((int)(((byte)(86)))));
            this.btnTestEmail.FlatAppearance.BorderSize = 0;
            this.btnTestEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestEmail.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestEmail.ForeColor = System.Drawing.Color.White;
            this.btnTestEmail.Location = new System.Drawing.Point(350, 300);
            this.btnTestEmail.Name = "btnTestEmail";
            this.btnTestEmail.Size = new System.Drawing.Size(220, 45);
            this.btnTestEmail.TabIndex = 10;
            this.btnTestEmail.Text = "Test Email";
            this.btnTestEmail.UseVisualStyleBackColor = false;
            this.btnTestEmail.Click += new System.EventHandler(this.btnTestEmail_Click_1);
            // 
            // btnSaveEmail
            // 
            this.btnSaveEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.btnSaveEmail.FlatAppearance.BorderSize = 0;
            this.btnSaveEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveEmail.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.btnSaveEmail.Location = new System.Drawing.Point(80, 300);
            this.btnSaveEmail.Name = "btnSaveEmail";
            this.btnSaveEmail.Size = new System.Drawing.Size(220, 45);
            this.btnSaveEmail.TabIndex = 9;
            this.btnSaveEmail.Text = "Save Email Settings";
            this.btnSaveEmail.UseVisualStyleBackColor = false;
            this.btnSaveEmail.Click += new System.EventHandler(this.btnSaveEmail_Click_1);
            // 
            // txtEmailPassword
            // 
            this.txtEmailPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtEmailPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailPassword.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtEmailPassword.Location = new System.Drawing.Point(230, 240);
            this.txtEmailPassword.Name = "txtEmailPassword";
            this.txtEmailPassword.PasswordChar = '*';
            this.txtEmailPassword.Size = new System.Drawing.Size(340, 30);
            this.txtEmailPassword.TabIndex = 8;
            this.txtEmailPassword.TextChanged += new System.EventHandler(this.txtEmailPassword_TextChanged);
            // 
            // txtEmailUsername
            // 
            this.txtEmailUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtEmailUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailUsername.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtEmailUsername.Location = new System.Drawing.Point(230, 200);
            this.txtEmailUsername.Name = "txtEmailUsername";
            this.txtEmailUsername.Size = new System.Drawing.Size(340, 30);
            this.txtEmailUsername.TabIndex = 7;
            this.txtEmailUsername.TextChanged += new System.EventHandler(this.txtEmailUsername_TextChanged);
            // 
            // txtSmtpServer
            // 
            this.txtSmtpServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtSmtpServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSmtpServer.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSmtpServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtSmtpServer.Location = new System.Drawing.Point(230, 120);
            this.txtSmtpServer.Name = "txtSmtpServer";
            this.txtSmtpServer.Size = new System.Drawing.Size(340, 30);
            this.txtSmtpServer.TabIndex = 6;
            this.txtSmtpServer.TextChanged += new System.EventHandler(this.txtSmtpServer_TextChanged);
            // 
            // txtSmtpPort
            // 
            this.txtSmtpPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtSmtpPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSmtpPort.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSmtpPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtSmtpPort.Location = new System.Drawing.Point(230, 160);
            this.txtSmtpPort.Name = "txtSmtpPort";
            this.txtSmtpPort.Size = new System.Drawing.Size(120, 30);
            this.txtSmtpPort.TabIndex = 5;
            this.txtSmtpPort.Text = "587";
            this.txtSmtpPort.TextChanged += new System.EventHandler(this.txtSmtpPort_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label20.Location = new System.Drawing.Point(40, 243);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(91, 25);
            this.label20.TabIndex = 4;
            this.label20.Text = "Password:";
            this.label20.Click += new System.EventHandler(this.label20_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label19.Location = new System.Drawing.Point(40, 203);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(95, 25);
            this.label19.TabIndex = 3;
            this.label19.Text = "Username:";
            this.label19.Click += new System.EventHandler(this.label19_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label18.Location = new System.Drawing.Point(40, 163);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(98, 25);
            this.label18.TabIndex = 2;
            this.label18.Text = "SMTP Port:";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label17.Location = new System.Drawing.Point(40, 123);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(115, 25);
            this.label17.TabIndex = 1;
            this.label17.Text = "SMTP Server:";
            this.label17.Click += new System.EventHandler(this.label17_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(40, 70);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(243, 25);
            this.label16.TabIndex = 0;
            this.label16.Text = "Email Server Configuration";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // groupBoxNotificationTypes
            // 
            this.groupBoxNotificationTypes.BackColor = System.Drawing.Color.White;
            this.groupBoxNotificationTypes.Controls.Add(this.checkBoxEmailSalesReport);
            this.groupBoxNotificationTypes.Controls.Add(this.checkBoxEmailLowInventory);
            this.groupBoxNotificationTypes.Controls.Add(this.checkBoxEmailNewUser);
            this.groupBoxNotificationTypes.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxNotificationTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxNotificationTypes.Location = new System.Drawing.Point(750, 30);
            this.groupBoxNotificationTypes.Name = "groupBoxNotificationTypes";
            this.groupBoxNotificationTypes.Size = new System.Drawing.Size(450, 220);
            this.groupBoxNotificationTypes.TabIndex = 0;
            this.groupBoxNotificationTypes.TabStop = false;
            this.groupBoxNotificationTypes.Text = "Notification Types";
            this.groupBoxNotificationTypes.Enter += new System.EventHandler(this.groupBoxNotificationTypes_Enter);
            // 
            // checkBoxEmailSalesReport
            // 
            this.checkBoxEmailSalesReport.AutoSize = true;
            this.checkBoxEmailSalesReport.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEmailSalesReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxEmailSalesReport.Location = new System.Drawing.Point(40, 130);
            this.checkBoxEmailSalesReport.Name = "checkBoxEmailSalesReport";
            this.checkBoxEmailSalesReport.Size = new System.Drawing.Size(214, 27);
            this.checkBoxEmailSalesReport.TabIndex = 2;
            this.checkBoxEmailSalesReport.Text = "Email Daily Sales Report";
            this.checkBoxEmailSalesReport.UseVisualStyleBackColor = true;
            this.checkBoxEmailSalesReport.CheckedChanged += new System.EventHandler(this.checkBoxEmailSalesReport_CheckedChanged);
            // 
            // checkBoxEmailLowInventory
            // 
            this.checkBoxEmailLowInventory.AutoSize = true;
            this.checkBoxEmailLowInventory.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEmailLowInventory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxEmailLowInventory.Location = new System.Drawing.Point(40, 100);
            this.checkBoxEmailLowInventory.Name = "checkBoxEmailLowInventory";
            this.checkBoxEmailLowInventory.Size = new System.Drawing.Size(233, 27);
            this.checkBoxEmailLowInventory.TabIndex = 1;
            this.checkBoxEmailLowInventory.Text = "Email Low Inventory Alerts";
            this.checkBoxEmailLowInventory.UseVisualStyleBackColor = true;
            this.checkBoxEmailLowInventory.CheckedChanged += new System.EventHandler(this.checkBoxEmailLowInventory_CheckedChanged);
            // 
            // checkBoxEmailNewUser
            // 
            this.checkBoxEmailNewUser.AutoSize = true;
            this.checkBoxEmailNewUser.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEmailNewUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.checkBoxEmailNewUser.Location = new System.Drawing.Point(40, 70);
            this.checkBoxEmailNewUser.Name = "checkBoxEmailNewUser";
            this.checkBoxEmailNewUser.Size = new System.Drawing.Size(199, 27);
            this.checkBoxEmailNewUser.TabIndex = 0;
            this.checkBoxEmailNewUser.Text = "Email New User Alerts";
            this.checkBoxEmailNewUser.UseVisualStyleBackColor = true;
            this.checkBoxEmailNewUser.CheckedChanged += new System.EventHandler(this.checkBoxEmailNewUser_CheckedChanged);
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tabPageAbout.Controls.Add(this.groupBoxAbout);
            this.tabPageAbout.Location = new System.Drawing.Point(4, 32);
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.Padding = new System.Windows.Forms.Padding(20);
            this.tabPageAbout.Size = new System.Drawing.Size(1719, 703);
            this.tabPageAbout.TabIndex = 5;
            this.tabPageAbout.Text = "About";
            this.tabPageAbout.Click += new System.EventHandler(this.tabPageAbout_Click);
            // 
            // groupBoxAbout
            // 
            this.groupBoxAbout.BackColor = System.Drawing.Color.White;
            this.groupBoxAbout.Controls.Add(this.linkLabelWebsite);
            this.groupBoxAbout.Controls.Add(this.label24);
            this.groupBoxAbout.Controls.Add(this.label23);
            this.groupBoxAbout.Controls.Add(this.label22);
            this.groupBoxAbout.Controls.Add(this.label21);
            this.groupBoxAbout.Controls.Add(this.pictureBoxLogo);
            this.groupBoxAbout.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBoxAbout.Location = new System.Drawing.Point(30, 30);
            this.groupBoxAbout.Name = "groupBoxAbout";
            this.groupBoxAbout.Size = new System.Drawing.Size(650, 420);
            this.groupBoxAbout.TabIndex = 0;
            this.groupBoxAbout.TabStop = false;
            this.groupBoxAbout.Text = "About CMS";
            this.groupBoxAbout.Enter += new System.EventHandler(this.groupBoxAbout_Enter);
            // 
            // linkLabelWebsite
            // 
            this.linkLabelWebsite.AutoSize = true;
            this.linkLabelWebsite.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelWebsite.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.linkLabelWebsite.Location = new System.Drawing.Point(280, 280);
            this.linkLabelWebsite.Name = "linkLabelWebsite";
            this.linkLabelWebsite.Size = new System.Drawing.Size(171, 25);
            this.linkLabelWebsite.TabIndex = 5;
            this.linkLabelWebsite.TabStop = true;
            this.linkLabelWebsite.Text = "www.matchpoint.ph";
            this.linkLabelWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelWebsite_LinkClicked);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label24.Location = new System.Drawing.Point(280, 240);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(103, 25);
            this.label24.TabIndex = 4;
            this.label24.Text = "Version: 2.0";
            this.label24.Click += new System.EventHandler(this.label24_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label23.Location = new System.Drawing.Point(280, 200);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(257, 25);
            this.label23.TabIndex = 3;
            this.label23.Text = "© 2026 MatchPoint Game Hub";
            this.label23.Click += new System.EventHandler(this.label23_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.label22.Location = new System.Drawing.Point(280, 160);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(207, 25);
            this.label22.TabIndex = 2;
            this.label22.Text = "Game Hub Management";
            this.label22.Click += new System.EventHandler(this.label22_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.label21.Location = new System.Drawing.Point(280, 120);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(139, 31);
            this.label21.TabIndex = 1;
            this.label21.Text = "MatchPoint";
            this.label21.Click += new System.EventHandler(this.label21_Click);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.White;
            this.pictureBoxLogo.Location = new System.Drawing.Point(70, 120);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(180, 180);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Click += new System.EventHandler(this.pictureBoxLogo_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.btnSaveAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSaveAll.FlatAppearance.BorderSize = 0;
            this.btnSaveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAll.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.btnSaveAll.Location = new System.Drawing.Point(0, 739);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(1727, 102);
            this.btnSaveAll.TabIndex = 1;
            this.btnSaveAll.Text = "Save All Settings";
            this.btnSaveAll.UseVisualStyleBackColor = false;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click_1);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnReset.Location = new System.Drawing.Point(0, 841);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(1727, 98);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset to Defaults";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click_1);
            // 
            // panelContainer
            // 
            this.panelContainer.Controls.Add(this.tabControl1);
            this.panelContainer.Controls.Add(this.btnSaveAll);
            this.panelContainer.Controls.Add(this.btnReset);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1727, 939);
            this.panelContainer.TabIndex = 3;
            // 
            // SETTINGS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelContainer);
            this.Name = "SETTINGS";
            this.Size = new System.Drawing.Size(1727, 939);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.groupBoxSystem.ResumeLayout(false);
            this.groupBoxSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSessionTimeout)).EndInit();
            this.tabPageDatabase.ResumeLayout(false);
            this.groupBoxBackup.ResumeLayout(false);
            this.groupBoxBackup.PerformLayout();
            this.groupBoxConnection.ResumeLayout(false);
            this.groupBoxConnection.PerformLayout();
            this.tabPageSecurity.ResumeLayout(false);
            this.groupBoxPasswordPolicy.ResumeLayout(false);
            this.groupBoxPasswordPolicy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPasswordHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPasswordExpiry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxAttempts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinLength)).EndInit();
            this.groupBoxSession.ResumeLayout(false);
            this.groupBoxSession.PerformLayout();
            this.tabPageNotifications.ResumeLayout(false);
            this.groupBoxEmail.ResumeLayout(false);
            this.groupBoxEmail.PerformLayout();
            this.groupBoxNotificationTypes.ResumeLayout(false);
            this.groupBoxNotificationTypes.PerformLayout();
            this.tabPageAbout.ResumeLayout(false);
            this.groupBoxAbout.ResumeLayout(false);
            this.groupBoxAbout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageDatabase;
        private System.Windows.Forms.TabPage tabPageSecurity;
        private System.Windows.Forms.TabPage tabPageNotifications;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.GroupBox groupBoxConnection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.GroupBox groupBoxBackup;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnSaveDatabase;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBackupPath;
        private System.Windows.Forms.Button btnRestoreDatabase;
        private System.Windows.Forms.Button btnBackupDatabase;
        private System.Windows.Forms.GroupBox groupBoxPasswordPolicy;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxRequireUppercase;
        private System.Windows.Forms.CheckBox checkBoxRequireSpecialChar;
        private System.Windows.Forms.CheckBox checkBoxRequireNumber;
        private System.Windows.Forms.GroupBox groupBoxSession;
        private System.Windows.Forms.CheckBox checkBoxForceLogout;
        private System.Windows.Forms.NumericUpDown numericUpDownPasswordHistory;
        private System.Windows.Forms.NumericUpDown numericUpDownPasswordExpiry;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxAttempts;
        private System.Windows.Forms.NumericUpDown numericUpDownMinLength;
        private System.Windows.Forms.GroupBox groupBoxEmail;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtEmailPassword;
        private System.Windows.Forms.TextBox txtEmailUsername;
        private System.Windows.Forms.TextBox txtSmtpServer;
        private System.Windows.Forms.TextBox txtSmtpPort;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnTestEmail;
        private System.Windows.Forms.Button btnSaveEmail;
        private System.Windows.Forms.GroupBox groupBoxNotificationTypes;
        private System.Windows.Forms.CheckBox checkBoxEmailSalesReport;
        private System.Windows.Forms.CheckBox checkBoxEmailLowInventory;
        private System.Windows.Forms.CheckBox checkBoxEmailNewUser;
        private System.Windows.Forms.GroupBox groupBoxAbout;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.LinkLabel linkLabelWebsite;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.GroupBox groupBoxSystem;
        private System.Windows.Forms.NumericUpDown numericUpDownSessionTimeout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxAutoBackup;
        private System.Windows.Forms.CheckBox checkBoxNotifications;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxTheme;
    }
}