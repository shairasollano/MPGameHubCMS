namespace cms
{
    partial class Activitylogs
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            // ===== COLOR SCHEME =====
            System.Drawing.Color primaryColor = System.Drawing.Color.FromArgb(79, 70, 229);
            System.Drawing.Color primaryLight = System.Drawing.Color.FromArgb(224, 231, 255);
            System.Drawing.Color successColor = System.Drawing.Color.FromArgb(16, 185, 129);
            System.Drawing.Color dangerColor = System.Drawing.Color.FromArgb(239, 68, 68);
            System.Drawing.Color warningColor = System.Drawing.Color.FromArgb(245, 158, 11);
            System.Drawing.Color infoColor = System.Drawing.Color.FromArgb(59, 130, 246);
            System.Drawing.Color purpleColor = System.Drawing.Color.FromArgb(139, 92, 246);
            System.Drawing.Color gray50 = System.Drawing.Color.FromArgb(249, 250, 251);
            System.Drawing.Color gray100 = System.Drawing.Color.FromArgb(243, 244, 246);
            System.Drawing.Color gray200 = System.Drawing.Color.FromArgb(229, 231, 235);
            System.Drawing.Color gray300 = System.Drawing.Color.FromArgb(209, 213, 219);
            System.Drawing.Color gray400 = System.Drawing.Color.FromArgb(156, 163, 175);
            System.Drawing.Color gray500 = System.Drawing.Color.FromArgb(107, 114, 128);
            System.Drawing.Color gray600 = System.Drawing.Color.FromArgb(75, 85, 99);
            System.Drawing.Color gray700 = System.Drawing.Color.FromArgb(55, 65, 81);
            System.Drawing.Color white = System.Drawing.Color.White;

            // ===== MAIN CONTAINER =====
            this.mainPanel = new System.Windows.Forms.Panel();

            // ===== HEADER SECTION =====
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.headerButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.refreshButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();

            this.filterPanel = new System.Windows.Forms.Panel();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.filterTable = new System.Windows.Forms.TableLayoutPanel();

            // Row 1
            this.startDateLabel = new System.Windows.Forms.Label();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.userLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();

            // Row 2
            this.activityLabel = new System.Windows.Forms.Label();
            this.activityCombo = new System.Windows.Forms.ComboBox();
            this.severityLabel = new System.Windows.Forms.Label();
            this.severityCombo = new System.Windows.Forms.ComboBox();
            this.moduleLabel = new System.Windows.Forms.Label();
            this.moduleCombo = new System.Windows.Forms.ComboBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();

            this.actionPanel = new System.Windows.Forms.Panel();
            this.actionButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.deleteSelectedButton = new System.Windows.Forms.Button();
            this.deleteAllButton = new System.Windows.Forms.Button();

            this.statsPanel = new System.Windows.Forms.Panel();
            this.statsGroupBox = new System.Windows.Forms.GroupBox();
            this.statsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();

            // Overview Stats Cards
            this.totalLogsCard = new System.Windows.Forms.Panel();
            this.totalLogsIcon = new System.Windows.Forms.Label();
            this.totalLogsValue = new System.Windows.Forms.Label();
            this.totalLogsLabel = new System.Windows.Forms.Label();

            this.todayLogsCard = new System.Windows.Forms.Panel();
            this.todayLogsIcon = new System.Windows.Forms.Label();
            this.todayLogsValue = new System.Windows.Forms.Label();
            this.todayLogsLabel = new System.Windows.Forms.Label();

            this.distinctUsersCard = new System.Windows.Forms.Panel();
            this.distinctUsersIcon = new System.Windows.Forms.Label();
            this.distinctUsersValue = new System.Windows.Forms.Label();
            this.distinctUsersLabel = new System.Windows.Forms.Label();

            this.activeTodayCard = new System.Windows.Forms.Panel();
            this.activeTodayIcon = new System.Windows.Forms.Label();
            this.activeTodayValue = new System.Windows.Forms.Label();
            this.activeTodayLabel = new System.Windows.Forms.Label();

            // Module Stats Cards
            this.gameRateChangesCard = new System.Windows.Forms.Panel();
            this.gameRateChangesIcon = new System.Windows.Forms.Label();
            this.gameRateChangesValue = new System.Windows.Forms.Label();
            this.gameRateChangesLabel = new System.Windows.Forms.Label();

            this.equipmentChangesCard = new System.Windows.Forms.Panel();
            this.equipmentChangesIcon = new System.Windows.Forms.Label();
            this.equipmentChangesValue = new System.Windows.Forms.Label();
            this.equipmentChangesLabel = new System.Windows.Forms.Label();

            // Severity Stats Cards
            this.infoCard = new System.Windows.Forms.Panel();
            this.infoIcon = new System.Windows.Forms.Label();
            this.infoValue = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();

            this.warningCard = new System.Windows.Forms.Panel();
            this.warningIcon = new System.Windows.Forms.Label();
            this.warningValue = new System.Windows.Forms.Label();
            this.warningLabel = new System.Windows.Forms.Label();

            this.errorCard = new System.Windows.Forms.Panel();
            this.errorIcon = new System.Windows.Forms.Label();
            this.errorValue = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();

            this.criticalCard = new System.Windows.Forms.Panel();
            this.criticalIcon = new System.Windows.Forms.Label();
            this.criticalValue = new System.Windows.Forms.Label();
            this.criticalLabel = new System.Windows.Forms.Label();

            this.logsDataGridView = new System.Windows.Forms.DataGridView();
            this.iconColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timestampColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.severityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // ===== SUSPEND LAYOUT =====
            this.mainPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.headerButtonPanel.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.filterGroupBox.SuspendLayout();
            this.filterTable.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.actionButtonPanel.SuspendLayout();
            this.statsPanel.SuspendLayout();
            this.statsGroupBox.SuspendLayout();
            this.statsFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).BeginInit();

            // ===== MAIN PANEL =====
            this.mainPanel.BackColor = gray50;
            this.mainPanel.Controls.Add(this.headerPanel);
            this.mainPanel.Controls.Add(this.filterPanel);
            this.mainPanel.Controls.Add(this.actionPanel);
            this.mainPanel.Controls.Add(this.statsPanel);
            this.mainPanel.Controls.Add(this.logsDataGridView);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(15);
            this.mainPanel.Size = new System.Drawing.Size(1400, 800);
            this.mainPanel.TabIndex = 0;

            // ===== HEADER PANEL (like Game Rates) =====
            this.headerPanel.BackColor = white;
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.headerButtonPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(15, 15);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(15);
            this.headerPanel.Size = new System.Drawing.Size(1370, 70);
            this.headerPanel.TabIndex = 0;

            // Title Label - Large header like Game Rates
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.ForeColor = gray700;
            this.titleLabel.Location = new System.Drawing.Point(15, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(185, 37);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Activity Logs";

            // Header Button Panel (right-aligned)
            this.headerButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerButtonPanel.AutoSize = true;
            this.headerButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.headerButtonPanel.Location = new System.Drawing.Point(1080, 15);
            this.headerButtonPanel.Name = "headerButtonPanel";
            this.headerButtonPanel.Size = new System.Drawing.Size(265, 45);
            this.headerButtonPanel.TabIndex = 1;

            // Refresh Button
            this.refreshButton.BackColor = white;
            this.refreshButton.FlatAppearance.BorderColor = gray200;
            this.refreshButton.FlatAppearance.BorderSize = 1;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.refreshButton.ForeColor = gray600;
            this.refreshButton.Location = new System.Drawing.Point(135, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(120, 40);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "↻ Refresh";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);

            // Export Button
            this.exportButton.BackColor = successColor;
            this.exportButton.FlatAppearance.BorderSize = 0;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exportButton.ForeColor = white;
            this.exportButton.Location = new System.Drawing.Point(5, 3);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(120, 40);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "📊 Export";
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);

            this.headerButtonPanel.Controls.Add(this.refreshButton);
            this.headerButtonPanel.Controls.Add(this.exportButton);

            // ===== FILTER PANEL =====
            this.filterPanel.BackColor = white;
            this.filterPanel.Controls.Add(this.filterGroupBox);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(15, 85);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Padding = new System.Windows.Forms.Padding(15);
            this.filterPanel.Size = new System.Drawing.Size(1370, 130);
            this.filterPanel.TabIndex = 1;

            // Filter GroupBox
            this.filterGroupBox.Controls.Add(this.filterTable);
            this.filterGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.filterGroupBox.ForeColor = gray700;
            this.filterGroupBox.Location = new System.Drawing.Point(15, 15);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(1340, 100);
            this.filterGroupBox.TabIndex = 0;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Filters";

            // Filter Table - Updated to include Module column
            this.filterTable.ColumnCount = 10;
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));

            this.filterTable.Controls.Add(this.startDateLabel, 0, 0);
            this.filterTable.Controls.Add(this.startDatePicker, 1, 0);
            this.filterTable.Controls.Add(this.endDateLabel, 2, 0);
            this.filterTable.Controls.Add(this.endDatePicker, 3, 0);
            this.filterTable.Controls.Add(this.userLabel, 4, 0);
            this.filterTable.Controls.Add(this.userTextBox, 5, 0);
            this.filterTable.Controls.Add(this.activityLabel, 0, 1);
            this.filterTable.Controls.Add(this.activityCombo, 1, 1);
            this.filterTable.Controls.Add(this.severityLabel, 2, 1);
            this.filterTable.Controls.Add(this.severityCombo, 3, 1);
            this.filterTable.Controls.Add(this.moduleLabel, 4, 1);
            this.filterTable.Controls.Add(this.moduleCombo, 5, 1);
            this.filterTable.Controls.Add(this.searchButton, 8, 1);
            this.filterTable.Controls.Add(this.clearButton, 9, 1);

            this.filterTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterTable.Location = new System.Drawing.Point(3, 22);
            this.filterTable.Name = "filterTable";
            this.filterTable.Padding = new System.Windows.Forms.Padding(10);
            this.filterTable.RowCount = 2;
            this.filterTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.filterTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.filterTable.Size = new System.Drawing.Size(1334, 75);
            this.filterTable.TabIndex = 0;

            // Start Date Label
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.startDateLabel.ForeColor = gray600;
            this.startDateLabel.Location = new System.Drawing.Point(13, 13);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(32, 19);
            this.startDateLabel.TabIndex = 0;
            this.startDateLabel.Text = "Start";
            this.startDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Start Date Picker
            this.startDatePicker.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDatePicker.Location = new System.Drawing.Point(83, 13);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(140, 25);
            this.startDatePicker.TabIndex = 1;

            // End Date Label
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.endDateLabel.ForeColor = gray600;
            this.endDateLabel.Location = new System.Drawing.Point(233, 13);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(29, 19);
            this.endDateLabel.TabIndex = 2;
            this.endDateLabel.Text = "End";
            this.endDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // End Date Picker
            this.endDatePicker.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDatePicker.Location = new System.Drawing.Point(303, 13);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(140, 25);
            this.endDatePicker.TabIndex = 3;

            // User Label
            this.userLabel.AutoSize = true;
            this.userLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.userLabel.ForeColor = gray600;
            this.userLabel.Location = new System.Drawing.Point(453, 13);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(35, 19);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // User TextBox
            this.userTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.userTextBox.Location = new System.Drawing.Point(523, 13);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(190, 25);
            this.userTextBox.TabIndex = 5;
            this.userTextBox.Text = "Enter username...";
            this.userTextBox.ForeColor = System.Drawing.Color.Gray;
            this.userTextBox.Enter += new System.EventHandler(this.UserTextBox_Enter);
            this.userTextBox.Leave += new System.EventHandler(this.UserTextBox_Leave);

            // Activity Label
            this.activityLabel.AutoSize = true;
            this.activityLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.activityLabel.ForeColor = gray600;
            this.activityLabel.Location = new System.Drawing.Point(13, 48);
            this.activityLabel.Name = "activityLabel";
            this.activityLabel.Size = new System.Drawing.Size(54, 19);
            this.activityLabel.TabIndex = 6;
            this.activityLabel.Text = "Activity";
            this.activityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Activity Combo
            this.activityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activityCombo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.activityCombo.Location = new System.Drawing.Point(83, 48);
            this.activityCombo.Name = "activityCombo";
            this.activityCombo.Size = new System.Drawing.Size(140, 25);
            this.activityCombo.TabIndex = 7;

            // Severity Label
            this.severityLabel.AutoSize = true;
            this.severityLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.severityLabel.ForeColor = gray600;
            this.severityLabel.Location = new System.Drawing.Point(233, 48);
            this.severityLabel.Name = "severityLabel";
            this.severityLabel.Size = new System.Drawing.Size(55, 19);
            this.severityLabel.TabIndex = 8;
            this.severityLabel.Text = "Severity";
            this.severityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Severity Combo
            this.severityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.severityCombo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.severityCombo.Location = new System.Drawing.Point(303, 48);
            this.severityCombo.Name = "severityCombo";
            this.severityCombo.Size = new System.Drawing.Size(140, 25);
            this.severityCombo.TabIndex = 9;

            // Module Label
            this.moduleLabel.AutoSize = true;
            this.moduleLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.moduleLabel.ForeColor = gray600;
            this.moduleLabel.Location = new System.Drawing.Point(453, 48);
            this.moduleLabel.Name = "moduleLabel";
            this.moduleLabel.Size = new System.Drawing.Size(53, 19);
            this.moduleLabel.TabIndex = 10;
            this.moduleLabel.Text = "Module";
            this.moduleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Module Combo
            this.moduleCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.moduleCombo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.moduleCombo.Location = new System.Drawing.Point(523, 48);
            this.moduleCombo.Name = "moduleCombo";
            this.moduleCombo.Size = new System.Drawing.Size(190, 25);
            this.moduleCombo.TabIndex = 11;

            // Search Button
            this.searchButton.BackColor = primaryColor;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.searchButton.ForeColor = white;
            this.searchButton.Location = new System.Drawing.Point(973, 48);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(90, 30);
            this.searchButton.TabIndex = 12;
            this.searchButton.Text = "Apply";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);

            // Clear Button
            this.clearButton.BackColor = white;
            this.clearButton.FlatAppearance.BorderColor = gray200;
            this.clearButton.FlatAppearance.BorderSize = 1;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.clearButton.ForeColor = gray600;
            this.clearButton.Location = new System.Drawing.Point(1073, 48);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(90, 30);
            this.clearButton.TabIndex = 13;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);

            // ===== ACTION PANEL =====
            this.actionPanel.BackColor = white;
            this.actionPanel.Controls.Add(this.actionButtonPanel);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionPanel.Location = new System.Drawing.Point(15, 215);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Padding = new System.Windows.Forms.Padding(15);
            this.actionPanel.Size = new System.Drawing.Size(1370, 60);
            this.actionPanel.TabIndex = 2;

            // Action Button Panel
            this.actionButtonPanel.AutoSize = true;
            this.actionButtonPanel.Location = new System.Drawing.Point(15, 15);
            this.actionButtonPanel.Name = "actionButtonPanel";
            this.actionButtonPanel.Size = new System.Drawing.Size(290, 40);
            this.actionButtonPanel.TabIndex = 0;

            // Delete Selected Button
            this.deleteSelectedButton.BackColor = white;
            this.deleteSelectedButton.FlatAppearance.BorderColor = warningColor;
            this.deleteSelectedButton.FlatAppearance.BorderSize = 1;
            this.deleteSelectedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteSelectedButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.deleteSelectedButton.ForeColor = warningColor;
            this.deleteSelectedButton.Location = new System.Drawing.Point(3, 3);
            this.deleteSelectedButton.Name = "deleteSelectedButton";
            this.deleteSelectedButton.Size = new System.Drawing.Size(140, 35);
            this.deleteSelectedButton.TabIndex = 0;
            this.deleteSelectedButton.Text = "Delete Selected";
            this.deleteSelectedButton.UseVisualStyleBackColor = false;
            this.deleteSelectedButton.Click += new System.EventHandler(this.deleteSelectedButton_Click);

            // Delete All Button
            this.deleteAllButton.BackColor = white;
            this.deleteAllButton.FlatAppearance.BorderColor = dangerColor;
            this.deleteAllButton.FlatAppearance.BorderSize = 1;
            this.deleteAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteAllButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.deleteAllButton.ForeColor = dangerColor;
            this.deleteAllButton.Location = new System.Drawing.Point(149, 3);
            this.deleteAllButton.Name = "deleteAllButton";
            this.deleteAllButton.Size = new System.Drawing.Size(120, 35);
            this.deleteAllButton.TabIndex = 1;
            this.deleteAllButton.Text = "Delete All";
            this.deleteAllButton.UseVisualStyleBackColor = false;
            this.deleteAllButton.Click += new System.EventHandler(this.deleteAllButton_Click);

            this.actionButtonPanel.Controls.Add(this.deleteSelectedButton);
            this.actionButtonPanel.Controls.Add(this.deleteAllButton);

            // ===== STATS PANEL =====
            this.statsPanel.BackColor = white;
            this.statsPanel.Controls.Add(this.statsGroupBox);
            this.statsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsPanel.Location = new System.Drawing.Point(15, 275);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Padding = new System.Windows.Forms.Padding(15);
            this.statsPanel.Size = new System.Drawing.Size(1370, 150);
            this.statsPanel.TabIndex = 3;

            // Stats GroupBox
            this.statsGroupBox.Controls.Add(this.statsFlowLayout);
            this.statsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.statsGroupBox.ForeColor = gray700;
            this.statsGroupBox.Location = new System.Drawing.Point(15, 15);
            this.statsGroupBox.Name = "statsGroupBox";
            this.statsGroupBox.Size = new System.Drawing.Size(1340, 120);
            this.statsGroupBox.TabIndex = 0;
            this.statsGroupBox.TabStop = false;
            this.statsGroupBox.Text = "Statistics";

            // Stats Flow Layout
            this.statsFlowLayout.AutoScroll = true;
            this.statsFlowLayout.Controls.Add(this.totalLogsCard);
            this.statsFlowLayout.Controls.Add(this.todayLogsCard);
            this.statsFlowLayout.Controls.Add(this.distinctUsersCard);
            this.statsFlowLayout.Controls.Add(this.activeTodayCard);
            this.statsFlowLayout.Controls.Add(this.gameRateChangesCard);
            this.statsFlowLayout.Controls.Add(this.equipmentChangesCard);
            this.statsFlowLayout.Controls.Add(this.infoCard);
            this.statsFlowLayout.Controls.Add(this.warningCard);
            this.statsFlowLayout.Controls.Add(this.errorCard);
            this.statsFlowLayout.Controls.Add(this.criticalCard);
            this.statsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsFlowLayout.Location = new System.Drawing.Point(3, 22);
            this.statsFlowLayout.Name = "statsFlowLayout";
            this.statsFlowLayout.Padding = new System.Windows.Forms.Padding(10);
            this.statsFlowLayout.Size = new System.Drawing.Size(1334, 95);
            this.statsFlowLayout.TabIndex = 0;

            // Total Logs Card
            this.totalLogsCard.BackColor = gray50;
            this.totalLogsCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.totalLogsCard.Controls.Add(this.totalLogsIcon);
            this.totalLogsCard.Controls.Add(this.totalLogsValue);
            this.totalLogsCard.Controls.Add(this.totalLogsLabel);
            this.totalLogsCard.Location = new System.Drawing.Point(13, 13);
            this.totalLogsCard.Name = "totalLogsCard";
            this.totalLogsCard.Size = new System.Drawing.Size(150, 70);
            this.totalLogsCard.TabIndex = 0;

            this.totalLogsIcon.AutoSize = true;
            this.totalLogsIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.totalLogsIcon.ForeColor = primaryColor;
            this.totalLogsIcon.Location = new System.Drawing.Point(10, 10);
            this.totalLogsIcon.Name = "totalLogsIcon";
            this.totalLogsIcon.Size = new System.Drawing.Size(34, 30);
            this.totalLogsIcon.TabIndex = 2;
            this.totalLogsIcon.Text = "📊";

            this.totalLogsValue.AutoSize = true;
            this.totalLogsValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.totalLogsValue.ForeColor = gray700;
            this.totalLogsValue.Location = new System.Drawing.Point(50, 8);
            this.totalLogsValue.Name = "totalLogsValue";
            this.totalLogsValue.Size = new System.Drawing.Size(27, 30);
            this.totalLogsValue.TabIndex = 1;
            this.totalLogsValue.Text = "0";

            this.totalLogsLabel.AutoSize = true;
            this.totalLogsLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.totalLogsLabel.ForeColor = gray500;
            this.totalLogsLabel.Location = new System.Drawing.Point(50, 38);
            this.totalLogsLabel.Name = "totalLogsLabel";
            this.totalLogsLabel.Size = new System.Drawing.Size(55, 13);
            this.totalLogsLabel.TabIndex = 0;
            this.totalLogsLabel.Text = "Total Logs";

            // Today Logs Card
            this.todayLogsCard.BackColor = gray50;
            this.todayLogsCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.todayLogsCard.Controls.Add(this.todayLogsIcon);
            this.todayLogsCard.Controls.Add(this.todayLogsValue);
            this.todayLogsCard.Controls.Add(this.todayLogsLabel);
            this.todayLogsCard.Location = new System.Drawing.Point(179, 13);
            this.todayLogsCard.Name = "todayLogsCard";
            this.todayLogsCard.Size = new System.Drawing.Size(150, 70);
            this.todayLogsCard.TabIndex = 1;

            this.todayLogsIcon.AutoSize = true;
            this.todayLogsIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.todayLogsIcon.ForeColor = successColor;
            this.todayLogsIcon.Location = new System.Drawing.Point(10, 10);
            this.todayLogsIcon.Name = "todayLogsIcon";
            this.todayLogsIcon.Size = new System.Drawing.Size(34, 30);
            this.todayLogsIcon.TabIndex = 2;
            this.todayLogsIcon.Text = "📅";

            this.todayLogsValue.AutoSize = true;
            this.todayLogsValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.todayLogsValue.ForeColor = gray700;
            this.todayLogsValue.Location = new System.Drawing.Point(50, 8);
            this.todayLogsValue.Name = "todayLogsValue";
            this.todayLogsValue.Size = new System.Drawing.Size(27, 30);
            this.todayLogsValue.TabIndex = 1;
            this.todayLogsValue.Text = "0";

            this.todayLogsLabel.AutoSize = true;
            this.todayLogsLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.todayLogsLabel.ForeColor = gray500;
            this.todayLogsLabel.Location = new System.Drawing.Point(50, 38);
            this.todayLogsLabel.Name = "todayLogsLabel";
            this.todayLogsLabel.Size = new System.Drawing.Size(61, 13);
            this.todayLogsLabel.TabIndex = 0;
            this.todayLogsLabel.Text = "Today's Logs";

            // Distinct Users Card
            this.distinctUsersCard.BackColor = gray50;
            this.distinctUsersCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.distinctUsersCard.Controls.Add(this.distinctUsersIcon);
            this.distinctUsersCard.Controls.Add(this.distinctUsersValue);
            this.distinctUsersCard.Controls.Add(this.distinctUsersLabel);
            this.distinctUsersCard.Location = new System.Drawing.Point(345, 13);
            this.distinctUsersCard.Name = "distinctUsersCard";
            this.distinctUsersCard.Size = new System.Drawing.Size(150, 70);
            this.distinctUsersCard.TabIndex = 2;

            this.distinctUsersIcon.AutoSize = true;
            this.distinctUsersIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.distinctUsersIcon.ForeColor = infoColor;
            this.distinctUsersIcon.Location = new System.Drawing.Point(10, 10);
            this.distinctUsersIcon.Name = "distinctUsersIcon";
            this.distinctUsersIcon.Size = new System.Drawing.Size(34, 30);
            this.distinctUsersIcon.TabIndex = 2;
            this.distinctUsersIcon.Text = "👥";

            this.distinctUsersValue.AutoSize = true;
            this.distinctUsersValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.distinctUsersValue.ForeColor = gray700;
            this.distinctUsersValue.Location = new System.Drawing.Point(50, 8);
            this.distinctUsersValue.Name = "distinctUsersValue";
            this.distinctUsersValue.Size = new System.Drawing.Size(27, 30);
            this.distinctUsersValue.TabIndex = 1;
            this.distinctUsersValue.Text = "0";

            this.distinctUsersLabel.AutoSize = true;
            this.distinctUsersLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.distinctUsersLabel.ForeColor = gray500;
            this.distinctUsersLabel.Location = new System.Drawing.Point(50, 38);
            this.distinctUsersLabel.Name = "distinctUsersLabel";
            this.distinctUsersLabel.Size = new System.Drawing.Size(71, 13);
            this.distinctUsersLabel.TabIndex = 0;
            this.distinctUsersLabel.Text = "Distinct Users";

            // Active Today Card
            this.activeTodayCard.BackColor = gray50;
            this.activeTodayCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activeTodayCard.Controls.Add(this.activeTodayIcon);
            this.activeTodayCard.Controls.Add(this.activeTodayValue);
            this.activeTodayCard.Controls.Add(this.activeTodayLabel);
            this.activeTodayCard.Location = new System.Drawing.Point(511, 13);
            this.activeTodayCard.Name = "activeTodayCard";
            this.activeTodayCard.Size = new System.Drawing.Size(150, 70);
            this.activeTodayCard.TabIndex = 3;

            this.activeTodayIcon.AutoSize = true;
            this.activeTodayIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.activeTodayIcon.ForeColor = warningColor;
            this.activeTodayIcon.Location = new System.Drawing.Point(10, 10);
            this.activeTodayIcon.Name = "activeTodayIcon";
            this.activeTodayIcon.Size = new System.Drawing.Size(34, 30);
            this.activeTodayIcon.TabIndex = 2;
            this.activeTodayIcon.Text = "⚡";

            this.activeTodayValue.AutoSize = true;
            this.activeTodayValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.activeTodayValue.ForeColor = gray700;
            this.activeTodayValue.Location = new System.Drawing.Point(50, 8);
            this.activeTodayValue.Name = "activeTodayValue";
            this.activeTodayValue.Size = new System.Drawing.Size(27, 30);
            this.activeTodayValue.TabIndex = 1;
            this.activeTodayValue.Text = "0";

            this.activeTodayLabel.AutoSize = true;
            this.activeTodayLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.activeTodayLabel.ForeColor = gray500;
            this.activeTodayLabel.Location = new System.Drawing.Point(50, 38);
            this.activeTodayLabel.Name = "activeTodayLabel";
            this.activeTodayLabel.Size = new System.Drawing.Size(66, 13);
            this.activeTodayLabel.TabIndex = 0;
            this.activeTodayLabel.Text = "Active Today";

            // Game Rate Changes Card
            this.gameRateChangesCard.BackColor = gray50;
            this.gameRateChangesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameRateChangesCard.Controls.Add(this.gameRateChangesIcon);
            this.gameRateChangesCard.Controls.Add(this.gameRateChangesValue);
            this.gameRateChangesCard.Controls.Add(this.gameRateChangesLabel);
            this.gameRateChangesCard.Location = new System.Drawing.Point(677, 13);
            this.gameRateChangesCard.Name = "gameRateChangesCard";
            this.gameRateChangesCard.Size = new System.Drawing.Size(150, 70);
            this.gameRateChangesCard.TabIndex = 4;

            this.gameRateChangesIcon.AutoSize = true;
            this.gameRateChangesIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameRateChangesIcon.ForeColor = primaryColor;
            this.gameRateChangesIcon.Location = new System.Drawing.Point(10, 10);
            this.gameRateChangesIcon.Name = "gameRateChangesIcon";
            this.gameRateChangesIcon.Size = new System.Drawing.Size(34, 30);
            this.gameRateChangesIcon.TabIndex = 2;
            this.gameRateChangesIcon.Text = "💰";

            this.gameRateChangesValue.AutoSize = true;
            this.gameRateChangesValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gameRateChangesValue.ForeColor = gray700;
            this.gameRateChangesValue.Location = new System.Drawing.Point(50, 8);
            this.gameRateChangesValue.Name = "gameRateChangesValue";
            this.gameRateChangesValue.Size = new System.Drawing.Size(27, 30);
            this.gameRateChangesValue.TabIndex = 1;
            this.gameRateChangesValue.Text = "0";

            this.gameRateChangesLabel.AutoSize = true;
            this.gameRateChangesLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameRateChangesLabel.ForeColor = gray500;
            this.gameRateChangesLabel.Location = new System.Drawing.Point(50, 38);
            this.gameRateChangesLabel.Name = "gameRateChangesLabel";
            this.gameRateChangesLabel.Size = new System.Drawing.Size(51, 13);
            this.gameRateChangesLabel.TabIndex = 0;
            this.gameRateChangesLabel.Text = "Rate Chgs";

            // Equipment Changes Card
            this.equipmentChangesCard.BackColor = gray50;
            this.equipmentChangesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equipmentChangesCard.Controls.Add(this.equipmentChangesIcon);
            this.equipmentChangesCard.Controls.Add(this.equipmentChangesValue);
            this.equipmentChangesCard.Controls.Add(this.equipmentChangesLabel);
            this.equipmentChangesCard.Location = new System.Drawing.Point(843, 13);
            this.equipmentChangesCard.Name = "equipmentChangesCard";
            this.equipmentChangesCard.Size = new System.Drawing.Size(150, 70);
            this.equipmentChangesCard.TabIndex = 5;

            this.equipmentChangesIcon.AutoSize = true;
            this.equipmentChangesIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.equipmentChangesIcon.ForeColor = successColor;
            this.equipmentChangesIcon.Location = new System.Drawing.Point(10, 10);
            this.equipmentChangesIcon.Name = "equipmentChangesIcon";
            this.equipmentChangesIcon.Size = new System.Drawing.Size(34, 30);
            this.equipmentChangesIcon.TabIndex = 2;
            this.equipmentChangesIcon.Text = "🏸";

            this.equipmentChangesValue.AutoSize = true;
            this.equipmentChangesValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.equipmentChangesValue.ForeColor = gray700;
            this.equipmentChangesValue.Location = new System.Drawing.Point(50, 8);
            this.equipmentChangesValue.Name = "equipmentChangesValue";
            this.equipmentChangesValue.Size = new System.Drawing.Size(27, 30);
            this.equipmentChangesValue.TabIndex = 1;
            this.equipmentChangesValue.Text = "0";

            this.equipmentChangesLabel.AutoSize = true;
            this.equipmentChangesLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.equipmentChangesLabel.ForeColor = gray500;
            this.equipmentChangesLabel.Location = new System.Drawing.Point(50, 38);
            this.equipmentChangesLabel.Name = "equipmentChangesLabel";
            this.equipmentChangesLabel.Size = new System.Drawing.Size(63, 13);
            this.equipmentChangesLabel.TabIndex = 0;
            this.equipmentChangesLabel.Text = "Equip Chgs";

            // Info Card
            this.infoCard.BackColor = gray50;
            this.infoCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoCard.Controls.Add(this.infoIcon);
            this.infoCard.Controls.Add(this.infoValue);
            this.infoCard.Controls.Add(this.infoLabel);
            this.infoCard.Location = new System.Drawing.Point(1009, 13);
            this.infoCard.Name = "infoCard";
            this.infoCard.Size = new System.Drawing.Size(100, 70);
            this.infoCard.TabIndex = 6;

            this.infoIcon.AutoSize = true;
            this.infoIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.infoIcon.ForeColor = infoColor;
            this.infoIcon.Location = new System.Drawing.Point(10, 8);
            this.infoIcon.Name = "infoIcon";
            this.infoIcon.Size = new System.Drawing.Size(29, 25);
            this.infoIcon.TabIndex = 2;
            this.infoIcon.Text = "ℹ️";

            this.infoValue.AutoSize = true;
            this.infoValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.infoValue.ForeColor = gray700;
            this.infoValue.Location = new System.Drawing.Point(10, 33);
            this.infoValue.Name = "infoValue";
            this.infoValue.Size = new System.Drawing.Size(23, 25);
            this.infoValue.TabIndex = 1;
            this.infoValue.Text = "0";

            this.infoLabel.AutoSize = true;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.infoLabel.ForeColor = gray500;
            this.infoLabel.Location = new System.Drawing.Point(45, 42);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(25, 13);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Info";

            // Warning Card
            this.warningCard.BackColor = gray50;
            this.warningCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.warningCard.Controls.Add(this.warningIcon);
            this.warningCard.Controls.Add(this.warningValue);
            this.warningCard.Controls.Add(this.warningLabel);
            this.warningCard.Location = new System.Drawing.Point(1125, 13);
            this.warningCard.Name = "warningCard";
            this.warningCard.Size = new System.Drawing.Size(100, 70);
            this.warningCard.TabIndex = 7;

            this.warningIcon.AutoSize = true;
            this.warningIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.warningIcon.ForeColor = warningColor;
            this.warningIcon.Location = new System.Drawing.Point(10, 8);
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(29, 25);
            this.warningIcon.TabIndex = 2;
            this.warningIcon.Text = "⚠️";

            this.warningValue.AutoSize = true;
            this.warningValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.warningValue.ForeColor = gray700;
            this.warningValue.Location = new System.Drawing.Point(10, 33);
            this.warningValue.Name = "warningValue";
            this.warningValue.Size = new System.Drawing.Size(23, 25);
            this.warningValue.TabIndex = 1;
            this.warningValue.Text = "0";

            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.warningLabel.ForeColor = gray500;
            this.warningLabel.Location = new System.Drawing.Point(45, 42);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(45, 13);
            this.warningLabel.TabIndex = 0;
            this.warningLabel.Text = "Warning";

            // Error Card
            this.errorCard.BackColor = gray50;
            this.errorCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorCard.Controls.Add(this.errorIcon);
            this.errorCard.Controls.Add(this.errorValue);
            this.errorCard.Controls.Add(this.errorLabel);
            this.errorCard.Location = new System.Drawing.Point(1241, 13);
            this.errorCard.Name = "errorCard";
            this.errorCard.Size = new System.Drawing.Size(100, 70);
            this.errorCard.TabIndex = 8;

            this.errorIcon.AutoSize = true;
            this.errorIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.errorIcon.ForeColor = dangerColor;
            this.errorIcon.Location = new System.Drawing.Point(10, 8);
            this.errorIcon.Name = "errorIcon";
            this.errorIcon.Size = new System.Drawing.Size(29, 25);
            this.errorIcon.TabIndex = 2;
            this.errorIcon.Text = "❌";

            this.errorValue.AutoSize = true;
            this.errorValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.errorValue.ForeColor = gray700;
            this.errorValue.Location = new System.Drawing.Point(10, 33);
            this.errorValue.Name = "errorValue";
            this.errorValue.Size = new System.Drawing.Size(23, 25);
            this.errorValue.TabIndex = 1;
            this.errorValue.Text = "0";

            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.errorLabel.ForeColor = gray500;
            this.errorLabel.Location = new System.Drawing.Point(45, 42);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(29, 13);
            this.errorLabel.TabIndex = 0;
            this.errorLabel.Text = "Error";

            // Critical Card
            this.criticalCard.BackColor = gray50;
            this.criticalCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.criticalCard.Controls.Add(this.criticalIcon);
            this.criticalCard.Controls.Add(this.criticalValue);
            this.criticalCard.Controls.Add(this.criticalLabel);
            this.criticalCard.Location = new System.Drawing.Point(1357, 13);
            this.criticalCard.Name = "criticalCard";
            this.criticalCard.Size = new System.Drawing.Size(100, 70);
            this.criticalCard.TabIndex = 9;

            this.criticalIcon.AutoSize = true;
            this.criticalIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.criticalIcon.ForeColor = purpleColor;
            this.criticalIcon.Location = new System.Drawing.Point(10, 8);
            this.criticalIcon.Name = "criticalIcon";
            this.criticalIcon.Size = new System.Drawing.Size(29, 25);
            this.criticalIcon.TabIndex = 2;
            this.criticalIcon.Text = "🔥";

            this.criticalValue.AutoSize = true;
            this.criticalValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.criticalValue.ForeColor = gray700;
            this.criticalValue.Location = new System.Drawing.Point(10, 33);
            this.criticalValue.Name = "criticalValue";
            this.criticalValue.Size = new System.Drawing.Size(23, 25);
            this.criticalValue.TabIndex = 1;
            this.criticalValue.Text = "0";

            this.criticalLabel.AutoSize = true;
            this.criticalLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.criticalLabel.ForeColor = gray500;
            this.criticalLabel.Location = new System.Drawing.Point(45, 42);
            this.criticalLabel.Name = "criticalLabel";
            this.criticalLabel.Size = new System.Drawing.Size(39, 13);
            this.criticalLabel.TabIndex = 0;
            this.criticalLabel.Text = "Critical";

            // ===== DATA GRID VIEW =====
            this.logsDataGridView.AllowUserToAddRows = false;
            this.logsDataGridView.AllowUserToDeleteRows = false;
            this.logsDataGridView.AllowUserToResizeRows = false;
            this.logsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logsDataGridView.BackgroundColor = white;
            this.logsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.logsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.logsDataGridView.ColumnHeadersHeight = 45;
            this.logsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = gray50;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = gray600;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            dataGridViewCellStyle1.SelectionBackColor = gray50;
            dataGridViewCellStyle1.SelectionForeColor = gray600;
            this.logsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;

            this.logsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.iconColumn,
                this.timestampColumn,
                this.userColumn,
                this.activityColumn,
                this.descriptionColumn,
                this.severityColumn,
                this.moduleColumn});

            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = white;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = gray700;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            dataGridViewCellStyle2.SelectionBackColor = primaryLight;
            dataGridViewCellStyle2.SelectionForeColor = gray700;
            this.logsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.logsDataGridView.EnableHeadersVisualStyles = false;
            this.logsDataGridView.GridColor = gray100;
            this.logsDataGridView.Location = new System.Drawing.Point(15, 440);
            this.logsDataGridView.Name = "logsDataGridView";
            this.logsDataGridView.ReadOnly = true;
            this.logsDataGridView.RowHeadersVisible = false;
            this.logsDataGridView.RowTemplate.Height = 40;
            this.logsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.logsDataGridView.Size = new System.Drawing.Size(1370, 345);
            this.logsDataGridView.TabIndex = 4;

            // Icon Column
            this.iconColumn.HeaderText = "";
            this.iconColumn.Name = "iconColumn";
            this.iconColumn.ReadOnly = true;
            this.iconColumn.Width = 40;

            // Timestamp Column
            this.timestampColumn.HeaderText = "Timestamp";
            this.timestampColumn.Name = "timestampColumn";
            this.timestampColumn.ReadOnly = true;
            this.timestampColumn.Width = 150;

            // User Column
            this.userColumn.HeaderText = "User";
            this.userColumn.Name = "userColumn";
            this.userColumn.ReadOnly = true;
            this.userColumn.Width = 120;

            // Activity Column
            this.activityColumn.HeaderText = "Activity";
            this.activityColumn.Name = "activityColumn";
            this.activityColumn.ReadOnly = true;
            this.activityColumn.Width = 140;

            // Description Column
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.ReadOnly = true;
            this.descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            // Severity Column
            this.severityColumn.HeaderText = "Severity";
            this.severityColumn.Name = "severityColumn";
            this.severityColumn.ReadOnly = true;
            this.severityColumn.Width = 100;

            // Module Column
            this.moduleColumn.HeaderText = "Module";
            this.moduleColumn.Name = "moduleColumn";
            this.moduleColumn.ReadOnly = true;
            this.moduleColumn.Width = 100;

            // ===== ACTIVITYLOGS CONTROL =====
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = gray50;
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "Activitylogs";
            this.Size = new System.Drawing.Size(1400, 800);

            // ===== RESUME LAYOUT =====
            this.mainPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.headerButtonPanel.ResumeLayout(false);
            this.filterPanel.ResumeLayout(false);
            this.filterGroupBox.ResumeLayout(false);
            this.filterTable.ResumeLayout(false);
            this.filterTable.PerformLayout();
            this.actionPanel.ResumeLayout(false);
            this.actionPanel.PerformLayout();
            this.actionButtonPanel.ResumeLayout(false);
            this.statsPanel.ResumeLayout(false);
            this.statsGroupBox.ResumeLayout(false);
            this.statsFlowLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        // Control declarations
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.FlowLayoutPanel headerButtonPanel;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button exportButton;

        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.GroupBox filterGroupBox;
        private System.Windows.Forms.TableLayoutPanel filterTable;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.Label activityLabel;
        private System.Windows.Forms.ComboBox activityCombo;
        private System.Windows.Forms.Label severityLabel;
        private System.Windows.Forms.ComboBox severityCombo;
        private System.Windows.Forms.Label moduleLabel;
        private System.Windows.Forms.ComboBox moduleCombo;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button clearButton;

        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.FlowLayoutPanel actionButtonPanel;
        private System.Windows.Forms.Button deleteSelectedButton;
        private System.Windows.Forms.Button deleteAllButton;

        private System.Windows.Forms.Panel statsPanel;
        private System.Windows.Forms.GroupBox statsGroupBox;
        private System.Windows.Forms.FlowLayoutPanel statsFlowLayout;

        // Stats Cards
        private System.Windows.Forms.Panel totalLogsCard;
        private System.Windows.Forms.Label totalLogsIcon;
        private System.Windows.Forms.Label totalLogsValue;
        private System.Windows.Forms.Label totalLogsLabel;

        private System.Windows.Forms.Panel todayLogsCard;
        private System.Windows.Forms.Label todayLogsIcon;
        private System.Windows.Forms.Label todayLogsValue;
        private System.Windows.Forms.Label todayLogsLabel;

        private System.Windows.Forms.Panel distinctUsersCard;
        private System.Windows.Forms.Label distinctUsersIcon;
        private System.Windows.Forms.Label distinctUsersValue;
        private System.Windows.Forms.Label distinctUsersLabel;

        private System.Windows.Forms.Panel activeTodayCard;
        private System.Windows.Forms.Label activeTodayIcon;
        private System.Windows.Forms.Label activeTodayValue;
        private System.Windows.Forms.Label activeTodayLabel;

        private System.Windows.Forms.Panel gameRateChangesCard;
        private System.Windows.Forms.Label gameRateChangesIcon;
        private System.Windows.Forms.Label gameRateChangesValue;
        private System.Windows.Forms.Label gameRateChangesLabel;

        private System.Windows.Forms.Panel equipmentChangesCard;
        private System.Windows.Forms.Label equipmentChangesIcon;
        private System.Windows.Forms.Label equipmentChangesValue;
        private System.Windows.Forms.Label equipmentChangesLabel;

        private System.Windows.Forms.Panel infoCard;
        private System.Windows.Forms.Label infoIcon;
        private System.Windows.Forms.Label infoValue;
        private System.Windows.Forms.Label infoLabel;

        private System.Windows.Forms.Panel warningCard;
        private System.Windows.Forms.Label warningIcon;
        private System.Windows.Forms.Label warningValue;
        private System.Windows.Forms.Label warningLabel;

        private System.Windows.Forms.Panel errorCard;
        private System.Windows.Forms.Label errorIcon;
        private System.Windows.Forms.Label errorValue;
        private System.Windows.Forms.Label errorLabel;

        private System.Windows.Forms.Panel criticalCard;
        private System.Windows.Forms.Label criticalIcon;
        private System.Windows.Forms.Label criticalValue;
        private System.Windows.Forms.Label criticalLabel;

        private System.Windows.Forms.DataGridView logsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn iconColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestampColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn activityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn severityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduleColumn;
    }
}