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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.headerButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.refreshButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.filterTable = new System.Windows.Forms.TableLayoutPanel();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.userLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
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
            this.gameRateChangesCard = new System.Windows.Forms.Panel();
            this.gameRateChangesIcon = new System.Windows.Forms.Label();
            this.gameRateChangesValue = new System.Windows.Forms.Label();
            this.gameRateChangesLabel = new System.Windows.Forms.Label();
            this.equipmentChangesCard = new System.Windows.Forms.Panel();
            this.equipmentChangesIcon = new System.Windows.Forms.Label();
            this.equipmentChangesValue = new System.Windows.Forms.Label();
            this.equipmentChangesLabel = new System.Windows.Forms.Label();
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
            this.totalLogsCard.SuspendLayout();
            this.todayLogsCard.SuspendLayout();
            this.distinctUsersCard.SuspendLayout();
            this.activeTodayCard.SuspendLayout();
            this.gameRateChangesCard.SuspendLayout();
            this.equipmentChangesCard.SuspendLayout();
            this.infoCard.SuspendLayout();
            this.warningCard.SuspendLayout();
            this.errorCard.SuspendLayout();
            this.criticalCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
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
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.headerButtonPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(15, 355);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(15);
            this.headerPanel.Size = new System.Drawing.Size(1370, 70);
            this.headerPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.titleLabel.Location = new System.Drawing.Point(15, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(225, 46);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Activity Logs";
            // 
            // headerButtonPanel
            // 
            this.headerButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerButtonPanel.AutoSize = true;
            this.headerButtonPanel.Controls.Add(this.refreshButton);
            this.headerButtonPanel.Controls.Add(this.exportButton);
            this.headerButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.headerButtonPanel.Location = new System.Drawing.Point(1080, 15);
            this.headerButtonPanel.Name = "headerButtonPanel";
            this.headerButtonPanel.Size = new System.Drawing.Size(265, 46);
            this.headerButtonPanel.TabIndex = 1;
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.White;
            this.refreshButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.refreshButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.refreshButton.Location = new System.Drawing.Point(142, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(120, 40);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "↻ Refresh";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.exportButton.FlatAppearance.BorderSize = 0;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.exportButton.ForeColor = System.Drawing.Color.White;
            this.exportButton.Location = new System.Drawing.Point(16, 3);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(120, 40);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "📊 Export";
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // filterPanel
            // 
            this.filterPanel.BackColor = System.Drawing.Color.White;
            this.filterPanel.Controls.Add(this.filterGroupBox);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(15, 225);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Padding = new System.Windows.Forms.Padding(15);
            this.filterPanel.Size = new System.Drawing.Size(1370, 130);
            this.filterPanel.TabIndex = 1;
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Controls.Add(this.filterTable);
            this.filterGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.filterGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.filterGroupBox.Location = new System.Drawing.Point(15, 15);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(1340, 100);
            this.filterGroupBox.TabIndex = 0;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Filters";
            // 
            // filterTable
            // 
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
            this.filterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 284F));
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
            this.filterTable.Location = new System.Drawing.Point(3, 26);
            this.filterTable.Name = "filterTable";
            this.filterTable.Padding = new System.Windows.Forms.Padding(10);
            this.filterTable.RowCount = 2;
            this.filterTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.filterTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.filterTable.Size = new System.Drawing.Size(1334, 71);
            this.filterTable.TabIndex = 0;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.startDateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.startDateLabel.Location = new System.Drawing.Point(13, 10);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(45, 23);
            this.startDateLabel.TabIndex = 0;
            this.startDateLabel.Text = "Start";
            this.startDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // startDatePicker
            // 
            this.startDatePicker.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDatePicker.Location = new System.Drawing.Point(83, 13);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(140, 30);
            this.startDatePicker.TabIndex = 1;
            // 
            // endDateLabel
            // 
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.endDateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.endDateLabel.Location = new System.Drawing.Point(233, 10);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(39, 23);
            this.endDateLabel.TabIndex = 2;
            this.endDateLabel.Text = "End";
            this.endDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endDatePicker
            // 
            this.endDatePicker.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDatePicker.Location = new System.Drawing.Point(303, 13);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(140, 30);
            this.endDatePicker.TabIndex = 3;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.userLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.userLabel.Location = new System.Drawing.Point(453, 10);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(44, 23);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userTextBox
            // 
            this.userTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.userTextBox.ForeColor = System.Drawing.Color.Gray;
            this.userTextBox.Location = new System.Drawing.Point(523, 13);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(190, 30);
            this.userTextBox.TabIndex = 5;
            this.userTextBox.Text = "Enter username...";
            this.userTextBox.Enter += new System.EventHandler(this.UserTextBox_Enter);
            this.userTextBox.Leave += new System.EventHandler(this.UserTextBox_Leave);
            // 
            // activityLabel
            // 
            this.activityLabel.AutoSize = true;
            this.activityLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.activityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.activityLabel.Location = new System.Drawing.Point(13, 45);
            this.activityLabel.Name = "activityLabel";
            this.activityLabel.Size = new System.Drawing.Size(57, 35);
            this.activityLabel.TabIndex = 6;
            this.activityLabel.Text = "Activity";
            this.activityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // activityCombo
            // 
            this.activityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activityCombo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.activityCombo.Location = new System.Drawing.Point(83, 48);
            this.activityCombo.Name = "activityCombo";
            this.activityCombo.Size = new System.Drawing.Size(140, 31);
            this.activityCombo.TabIndex = 7;
            // 
            // severityLabel
            // 
            this.severityLabel.AutoSize = true;
            this.severityLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.severityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.severityLabel.Location = new System.Drawing.Point(233, 45);
            this.severityLabel.Name = "severityLabel";
            this.severityLabel.Size = new System.Drawing.Size(61, 35);
            this.severityLabel.TabIndex = 8;
            this.severityLabel.Text = "Severity";
            this.severityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // severityCombo
            // 
            this.severityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.severityCombo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.severityCombo.Location = new System.Drawing.Point(303, 48);
            this.severityCombo.Name = "severityCombo";
            this.severityCombo.Size = new System.Drawing.Size(140, 31);
            this.severityCombo.TabIndex = 9;
            // 
            // moduleLabel
            // 
            this.moduleLabel.AutoSize = true;
            this.moduleLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.moduleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.moduleLabel.Location = new System.Drawing.Point(453, 45);
            this.moduleLabel.Name = "moduleLabel";
            this.moduleLabel.Size = new System.Drawing.Size(59, 35);
            this.moduleLabel.TabIndex = 10;
            this.moduleLabel.Text = "Module";
            this.moduleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // moduleCombo
            // 
            this.moduleCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.moduleCombo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.moduleCombo.Location = new System.Drawing.Point(523, 48);
            this.moduleCombo.Name = "moduleCombo";
            this.moduleCombo.Size = new System.Drawing.Size(190, 31);
            this.moduleCombo.TabIndex = 11;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(943, 48);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(90, 29);
            this.searchButton.TabIndex = 12;
            this.searchButton.Text = "Apply";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.BackColor = System.Drawing.Color.White;
            this.clearButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.clearButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.clearButton.Location = new System.Drawing.Point(1043, 48);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(90, 29);
            this.clearButton.TabIndex = 13;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // actionPanel
            // 
            this.actionPanel.BackColor = System.Drawing.Color.White;
            this.actionPanel.Controls.Add(this.actionButtonPanel);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionPanel.Location = new System.Drawing.Point(15, 165);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Padding = new System.Windows.Forms.Padding(15);
            this.actionPanel.Size = new System.Drawing.Size(1370, 60);
            this.actionPanel.TabIndex = 2;
            // 
            // actionButtonPanel
            // 
            this.actionButtonPanel.AutoSize = true;
            this.actionButtonPanel.Controls.Add(this.deleteSelectedButton);
            this.actionButtonPanel.Controls.Add(this.deleteAllButton);
            this.actionButtonPanel.Location = new System.Drawing.Point(15, 15);
            this.actionButtonPanel.Name = "actionButtonPanel";
            this.actionButtonPanel.Size = new System.Drawing.Size(290, 41);
            this.actionButtonPanel.TabIndex = 0;
            // 
            // deleteSelectedButton
            // 
            this.deleteSelectedButton.BackColor = System.Drawing.Color.White;
            this.deleteSelectedButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.deleteSelectedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteSelectedButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.deleteSelectedButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.deleteSelectedButton.Location = new System.Drawing.Point(3, 3);
            this.deleteSelectedButton.Name = "deleteSelectedButton";
            this.deleteSelectedButton.Size = new System.Drawing.Size(140, 35);
            this.deleteSelectedButton.TabIndex = 0;
            this.deleteSelectedButton.Text = "Delete Selected";
            this.deleteSelectedButton.UseVisualStyleBackColor = false;
            this.deleteSelectedButton.Click += new System.EventHandler(this.deleteSelectedButton_Click);
            // 
            // deleteAllButton
            // 
            this.deleteAllButton.BackColor = System.Drawing.Color.White;
            this.deleteAllButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.deleteAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteAllButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.deleteAllButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.deleteAllButton.Location = new System.Drawing.Point(149, 3);
            this.deleteAllButton.Name = "deleteAllButton";
            this.deleteAllButton.Size = new System.Drawing.Size(120, 35);
            this.deleteAllButton.TabIndex = 1;
            this.deleteAllButton.Text = "Delete All";
            this.deleteAllButton.UseVisualStyleBackColor = false;
            this.deleteAllButton.Click += new System.EventHandler(this.deleteAllButton_Click);
            // 
            // statsPanel
            // 
            this.statsPanel.BackColor = System.Drawing.Color.White;
            this.statsPanel.Controls.Add(this.statsGroupBox);
            this.statsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsPanel.Location = new System.Drawing.Point(15, 15);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Padding = new System.Windows.Forms.Padding(15);
            this.statsPanel.Size = new System.Drawing.Size(1370, 150);
            this.statsPanel.TabIndex = 3;
            // 
            // statsGroupBox
            // 
            this.statsGroupBox.Controls.Add(this.statsFlowLayout);
            this.statsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statsGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.statsGroupBox.Location = new System.Drawing.Point(15, 15);
            this.statsGroupBox.Name = "statsGroupBox";
            this.statsGroupBox.Size = new System.Drawing.Size(1340, 120);
            this.statsGroupBox.TabIndex = 0;
            this.statsGroupBox.TabStop = false;
            this.statsGroupBox.Text = "Statistics";
            // 
            // statsFlowLayout
            // 
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
            this.statsFlowLayout.Location = new System.Drawing.Point(3, 26);
            this.statsFlowLayout.Name = "statsFlowLayout";
            this.statsFlowLayout.Padding = new System.Windows.Forms.Padding(10);
            this.statsFlowLayout.Size = new System.Drawing.Size(1334, 91);
            this.statsFlowLayout.TabIndex = 0;
            // 
            // totalLogsCard
            // 
            this.totalLogsCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.totalLogsCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.totalLogsCard.Controls.Add(this.totalLogsIcon);
            this.totalLogsCard.Controls.Add(this.totalLogsValue);
            this.totalLogsCard.Controls.Add(this.totalLogsLabel);
            this.totalLogsCard.Location = new System.Drawing.Point(13, 13);
            this.totalLogsCard.Name = "totalLogsCard";
            this.totalLogsCard.Size = new System.Drawing.Size(150, 70);
            this.totalLogsCard.TabIndex = 0;
            // 
            // totalLogsIcon
            // 
            this.totalLogsIcon.AutoSize = true;
            this.totalLogsIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.totalLogsIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.totalLogsIcon.Location = new System.Drawing.Point(10, 10);
            this.totalLogsIcon.Name = "totalLogsIcon";
            this.totalLogsIcon.Size = new System.Drawing.Size(52, 36);
            this.totalLogsIcon.TabIndex = 2;
            this.totalLogsIcon.Text = "📊";
            // 
            // totalLogsValue
            // 
            this.totalLogsValue.AutoSize = true;
            this.totalLogsValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.totalLogsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.totalLogsValue.Location = new System.Drawing.Point(50, 8);
            this.totalLogsValue.Name = "totalLogsValue";
            this.totalLogsValue.Size = new System.Drawing.Size(33, 37);
            this.totalLogsValue.TabIndex = 1;
            this.totalLogsValue.Text = "0";
            // 
            // totalLogsLabel
            // 
            this.totalLogsLabel.AutoSize = true;
            this.totalLogsLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.totalLogsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.totalLogsLabel.Location = new System.Drawing.Point(50, 38);
            this.totalLogsLabel.Name = "totalLogsLabel";
            this.totalLogsLabel.Size = new System.Drawing.Size(71, 19);
            this.totalLogsLabel.TabIndex = 0;
            this.totalLogsLabel.Text = "Total Logs";
            // 
            // todayLogsCard
            // 
            this.todayLogsCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.todayLogsCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.todayLogsCard.Controls.Add(this.todayLogsIcon);
            this.todayLogsCard.Controls.Add(this.todayLogsValue);
            this.todayLogsCard.Controls.Add(this.todayLogsLabel);
            this.todayLogsCard.Location = new System.Drawing.Point(169, 13);
            this.todayLogsCard.Name = "todayLogsCard";
            this.todayLogsCard.Size = new System.Drawing.Size(150, 70);
            this.todayLogsCard.TabIndex = 1;
            // 
            // todayLogsIcon
            // 
            this.todayLogsIcon.AutoSize = true;
            this.todayLogsIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.todayLogsIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.todayLogsIcon.Location = new System.Drawing.Point(10, 10);
            this.todayLogsIcon.Name = "todayLogsIcon";
            this.todayLogsIcon.Size = new System.Drawing.Size(52, 36);
            this.todayLogsIcon.TabIndex = 2;
            this.todayLogsIcon.Text = "📅";
            // 
            // todayLogsValue
            // 
            this.todayLogsValue.AutoSize = true;
            this.todayLogsValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.todayLogsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.todayLogsValue.Location = new System.Drawing.Point(50, 8);
            this.todayLogsValue.Name = "todayLogsValue";
            this.todayLogsValue.Size = new System.Drawing.Size(33, 37);
            this.todayLogsValue.TabIndex = 1;
            this.todayLogsValue.Text = "0";
            // 
            // todayLogsLabel
            // 
            this.todayLogsLabel.AutoSize = true;
            this.todayLogsLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.todayLogsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.todayLogsLabel.Location = new System.Drawing.Point(50, 38);
            this.todayLogsLabel.Name = "todayLogsLabel";
            this.todayLogsLabel.Size = new System.Drawing.Size(87, 19);
            this.todayLogsLabel.TabIndex = 0;
            this.todayLogsLabel.Text = "Today\'s Logs";
            // 
            // distinctUsersCard
            // 
            this.distinctUsersCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.distinctUsersCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.distinctUsersCard.Controls.Add(this.distinctUsersIcon);
            this.distinctUsersCard.Controls.Add(this.distinctUsersValue);
            this.distinctUsersCard.Controls.Add(this.distinctUsersLabel);
            this.distinctUsersCard.Location = new System.Drawing.Point(325, 13);
            this.distinctUsersCard.Name = "distinctUsersCard";
            this.distinctUsersCard.Size = new System.Drawing.Size(150, 70);
            this.distinctUsersCard.TabIndex = 2;
            // 
            // distinctUsersIcon
            // 
            this.distinctUsersIcon.AutoSize = true;
            this.distinctUsersIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.distinctUsersIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.distinctUsersIcon.Location = new System.Drawing.Point(10, 10);
            this.distinctUsersIcon.Name = "distinctUsersIcon";
            this.distinctUsersIcon.Size = new System.Drawing.Size(52, 36);
            this.distinctUsersIcon.TabIndex = 2;
            this.distinctUsersIcon.Text = "👥";
            // 
            // distinctUsersValue
            // 
            this.distinctUsersValue.AutoSize = true;
            this.distinctUsersValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.distinctUsersValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.distinctUsersValue.Location = new System.Drawing.Point(50, 8);
            this.distinctUsersValue.Name = "distinctUsersValue";
            this.distinctUsersValue.Size = new System.Drawing.Size(33, 37);
            this.distinctUsersValue.TabIndex = 1;
            this.distinctUsersValue.Text = "0";
            // 
            // distinctUsersLabel
            // 
            this.distinctUsersLabel.AutoSize = true;
            this.distinctUsersLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.distinctUsersLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.distinctUsersLabel.Location = new System.Drawing.Point(50, 38);
            this.distinctUsersLabel.Name = "distinctUsersLabel";
            this.distinctUsersLabel.Size = new System.Drawing.Size(93, 19);
            this.distinctUsersLabel.TabIndex = 0;
            this.distinctUsersLabel.Text = "Distinct Users";
            // 
            // activeTodayCard
            // 
            this.activeTodayCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.activeTodayCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activeTodayCard.Controls.Add(this.activeTodayIcon);
            this.activeTodayCard.Controls.Add(this.activeTodayValue);
            this.activeTodayCard.Controls.Add(this.activeTodayLabel);
            this.activeTodayCard.Location = new System.Drawing.Point(481, 13);
            this.activeTodayCard.Name = "activeTodayCard";
            this.activeTodayCard.Size = new System.Drawing.Size(150, 70);
            this.activeTodayCard.TabIndex = 3;
            // 
            // activeTodayIcon
            // 
            this.activeTodayIcon.AutoSize = true;
            this.activeTodayIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.activeTodayIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.activeTodayIcon.Location = new System.Drawing.Point(10, 10);
            this.activeTodayIcon.Name = "activeTodayIcon";
            this.activeTodayIcon.Size = new System.Drawing.Size(52, 36);
            this.activeTodayIcon.TabIndex = 2;
            this.activeTodayIcon.Text = "⚡";
            // 
            // activeTodayValue
            // 
            this.activeTodayValue.AutoSize = true;
            this.activeTodayValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.activeTodayValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.activeTodayValue.Location = new System.Drawing.Point(50, 8);
            this.activeTodayValue.Name = "activeTodayValue";
            this.activeTodayValue.Size = new System.Drawing.Size(33, 37);
            this.activeTodayValue.TabIndex = 1;
            this.activeTodayValue.Text = "0";
            // 
            // activeTodayLabel
            // 
            this.activeTodayLabel.AutoSize = true;
            this.activeTodayLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.activeTodayLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.activeTodayLabel.Location = new System.Drawing.Point(50, 38);
            this.activeTodayLabel.Name = "activeTodayLabel";
            this.activeTodayLabel.Size = new System.Drawing.Size(86, 19);
            this.activeTodayLabel.TabIndex = 0;
            this.activeTodayLabel.Text = "Active Today";
            // 
            // gameRateChangesCard
            // 
            this.gameRateChangesCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.gameRateChangesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameRateChangesCard.Controls.Add(this.gameRateChangesIcon);
            this.gameRateChangesCard.Controls.Add(this.gameRateChangesValue);
            this.gameRateChangesCard.Controls.Add(this.gameRateChangesLabel);
            this.gameRateChangesCard.Location = new System.Drawing.Point(637, 13);
            this.gameRateChangesCard.Name = "gameRateChangesCard";
            this.gameRateChangesCard.Size = new System.Drawing.Size(150, 70);
            this.gameRateChangesCard.TabIndex = 4;
            // 
            // gameRateChangesIcon
            // 
            this.gameRateChangesIcon.AutoSize = true;
            this.gameRateChangesIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.gameRateChangesIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.gameRateChangesIcon.Location = new System.Drawing.Point(10, 10);
            this.gameRateChangesIcon.Name = "gameRateChangesIcon";
            this.gameRateChangesIcon.Size = new System.Drawing.Size(52, 36);
            this.gameRateChangesIcon.TabIndex = 2;
            this.gameRateChangesIcon.Text = "💰";
            // 
            // gameRateChangesValue
            // 
            this.gameRateChangesValue.AutoSize = true;
            this.gameRateChangesValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.gameRateChangesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.gameRateChangesValue.Location = new System.Drawing.Point(50, 8);
            this.gameRateChangesValue.Name = "gameRateChangesValue";
            this.gameRateChangesValue.Size = new System.Drawing.Size(33, 37);
            this.gameRateChangesValue.TabIndex = 1;
            this.gameRateChangesValue.Text = "0";
            // 
            // gameRateChangesLabel
            // 
            this.gameRateChangesLabel.AutoSize = true;
            this.gameRateChangesLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.gameRateChangesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.gameRateChangesLabel.Location = new System.Drawing.Point(50, 38);
            this.gameRateChangesLabel.Name = "gameRateChangesLabel";
            this.gameRateChangesLabel.Size = new System.Drawing.Size(71, 19);
            this.gameRateChangesLabel.TabIndex = 0;
            this.gameRateChangesLabel.Text = "Rate Chgs";
            // 
            // equipmentChangesCard
            // 
            this.equipmentChangesCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.equipmentChangesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equipmentChangesCard.Controls.Add(this.equipmentChangesIcon);
            this.equipmentChangesCard.Controls.Add(this.equipmentChangesValue);
            this.equipmentChangesCard.Controls.Add(this.equipmentChangesLabel);
            this.equipmentChangesCard.Location = new System.Drawing.Point(793, 13);
            this.equipmentChangesCard.Name = "equipmentChangesCard";
            this.equipmentChangesCard.Size = new System.Drawing.Size(150, 70);
            this.equipmentChangesCard.TabIndex = 5;
            // 
            // equipmentChangesIcon
            // 
            this.equipmentChangesIcon.AutoSize = true;
            this.equipmentChangesIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.equipmentChangesIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.equipmentChangesIcon.Location = new System.Drawing.Point(10, 10);
            this.equipmentChangesIcon.Name = "equipmentChangesIcon";
            this.equipmentChangesIcon.Size = new System.Drawing.Size(52, 36);
            this.equipmentChangesIcon.TabIndex = 2;
            this.equipmentChangesIcon.Text = "🏸";
            // 
            // equipmentChangesValue
            // 
            this.equipmentChangesValue.AutoSize = true;
            this.equipmentChangesValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.equipmentChangesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.equipmentChangesValue.Location = new System.Drawing.Point(50, 8);
            this.equipmentChangesValue.Name = "equipmentChangesValue";
            this.equipmentChangesValue.Size = new System.Drawing.Size(33, 37);
            this.equipmentChangesValue.TabIndex = 1;
            this.equipmentChangesValue.Text = "0";
            // 
            // equipmentChangesLabel
            // 
            this.equipmentChangesLabel.AutoSize = true;
            this.equipmentChangesLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.equipmentChangesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.equipmentChangesLabel.Location = new System.Drawing.Point(50, 38);
            this.equipmentChangesLabel.Name = "equipmentChangesLabel";
            this.equipmentChangesLabel.Size = new System.Drawing.Size(78, 19);
            this.equipmentChangesLabel.TabIndex = 0;
            this.equipmentChangesLabel.Text = "Equip Chgs";
            // 
            // infoCard
            // 
            this.infoCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.infoCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoCard.Controls.Add(this.infoIcon);
            this.infoCard.Controls.Add(this.infoValue);
            this.infoCard.Controls.Add(this.infoLabel);
            this.infoCard.Location = new System.Drawing.Point(949, 13);
            this.infoCard.Name = "infoCard";
            this.infoCard.Size = new System.Drawing.Size(100, 70);
            this.infoCard.TabIndex = 6;
            // 
            // infoIcon
            // 
            this.infoIcon.AutoSize = true;
            this.infoIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F);
            this.infoIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.infoIcon.Location = new System.Drawing.Point(10, 8);
            this.infoIcon.Name = "infoIcon";
            this.infoIcon.Size = new System.Drawing.Size(47, 32);
            this.infoIcon.TabIndex = 2;
            this.infoIcon.Text = "ℹ️";
            // 
            // infoValue
            // 
            this.infoValue.AutoSize = true;
            this.infoValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.infoValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.infoValue.Location = new System.Drawing.Point(10, 33);
            this.infoValue.Name = "infoValue";
            this.infoValue.Size = new System.Drawing.Size(28, 32);
            this.infoValue.TabIndex = 1;
            this.infoValue.Text = "0";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.infoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.infoLabel.Location = new System.Drawing.Point(45, 42);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(33, 19);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Info";
            // 
            // warningCard
            // 
            this.warningCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.warningCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.warningCard.Controls.Add(this.warningIcon);
            this.warningCard.Controls.Add(this.warningValue);
            this.warningCard.Controls.Add(this.warningLabel);
            this.warningCard.Location = new System.Drawing.Point(1055, 13);
            this.warningCard.Name = "warningCard";
            this.warningCard.Size = new System.Drawing.Size(100, 70);
            this.warningCard.TabIndex = 7;
            // 
            // warningIcon
            // 
            this.warningIcon.AutoSize = true;
            this.warningIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F);
            this.warningIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.warningIcon.Location = new System.Drawing.Point(10, 8);
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(47, 32);
            this.warningIcon.TabIndex = 2;
            this.warningIcon.Text = "⚠️";
            // 
            // warningValue
            // 
            this.warningValue.AutoSize = true;
            this.warningValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.warningValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.warningValue.Location = new System.Drawing.Point(10, 33);
            this.warningValue.Name = "warningValue";
            this.warningValue.Size = new System.Drawing.Size(28, 32);
            this.warningValue.TabIndex = 1;
            this.warningValue.Text = "0";
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.warningLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.warningLabel.Location = new System.Drawing.Point(45, 42);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(60, 19);
            this.warningLabel.TabIndex = 0;
            this.warningLabel.Text = "Warning";
            // 
            // errorCard
            // 
            this.errorCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.errorCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorCard.Controls.Add(this.errorIcon);
            this.errorCard.Controls.Add(this.errorValue);
            this.errorCard.Controls.Add(this.errorLabel);
            this.errorCard.Location = new System.Drawing.Point(1161, 13);
            this.errorCard.Name = "errorCard";
            this.errorCard.Size = new System.Drawing.Size(100, 70);
            this.errorCard.TabIndex = 8;
            // 
            // errorIcon
            // 
            this.errorIcon.AutoSize = true;
            this.errorIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F);
            this.errorIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.errorIcon.Location = new System.Drawing.Point(10, 8);
            this.errorIcon.Name = "errorIcon";
            this.errorIcon.Size = new System.Drawing.Size(47, 32);
            this.errorIcon.TabIndex = 2;
            this.errorIcon.Text = "❌";
            // 
            // errorValue
            // 
            this.errorValue.AutoSize = true;
            this.errorValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.errorValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.errorValue.Location = new System.Drawing.Point(10, 33);
            this.errorValue.Name = "errorValue";
            this.errorValue.Size = new System.Drawing.Size(28, 32);
            this.errorValue.TabIndex = 1;
            this.errorValue.Text = "0";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.errorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.errorLabel.Location = new System.Drawing.Point(45, 42);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(39, 19);
            this.errorLabel.TabIndex = 0;
            this.errorLabel.Text = "Error";
            // 
            // criticalCard
            // 
            this.criticalCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.criticalCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.criticalCard.Controls.Add(this.criticalIcon);
            this.criticalCard.Controls.Add(this.criticalValue);
            this.criticalCard.Controls.Add(this.criticalLabel);
            this.criticalCard.Location = new System.Drawing.Point(13, 89);
            this.criticalCard.Name = "criticalCard";
            this.criticalCard.Size = new System.Drawing.Size(100, 70);
            this.criticalCard.TabIndex = 9;
            // 
            // criticalIcon
            // 
            this.criticalIcon.AutoSize = true;
            this.criticalIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F);
            this.criticalIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(92)))), ((int)(((byte)(246)))));
            this.criticalIcon.Location = new System.Drawing.Point(10, 8);
            this.criticalIcon.Name = "criticalIcon";
            this.criticalIcon.Size = new System.Drawing.Size(47, 32);
            this.criticalIcon.TabIndex = 2;
            this.criticalIcon.Text = "🔥";
            // 
            // criticalValue
            // 
            this.criticalValue.AutoSize = true;
            this.criticalValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.criticalValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.criticalValue.Location = new System.Drawing.Point(10, 33);
            this.criticalValue.Name = "criticalValue";
            this.criticalValue.Size = new System.Drawing.Size(28, 32);
            this.criticalValue.TabIndex = 1;
            this.criticalValue.Text = "0";
            // 
            // criticalLabel
            // 
            this.criticalLabel.AutoSize = true;
            this.criticalLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.criticalLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.criticalLabel.Location = new System.Drawing.Point(45, 42);
            this.criticalLabel.Name = "criticalLabel";
            this.criticalLabel.Size = new System.Drawing.Size(50, 19);
            this.criticalLabel.TabIndex = 0;
            this.criticalLabel.Text = "Critical";
            // 
            // logsDataGridView
            // 
            this.logsDataGridView.AllowUserToAddRows = false;
            this.logsDataGridView.AllowUserToDeleteRows = false;
            this.logsDataGridView.AllowUserToResizeRows = false;
            this.logsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logsDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.logsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.logsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.logsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.logsDataGridView.ColumnHeadersHeight = 45;
            this.logsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.logsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconColumn,
            this.timestampColumn,
            this.userColumn,
            this.activityColumn,
            this.descriptionColumn,
            this.severityColumn,
            this.moduleColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.logsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.logsDataGridView.EnableHeadersVisualStyles = false;
            this.logsDataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.logsDataGridView.Location = new System.Drawing.Point(15, 440);
            this.logsDataGridView.Name = "logsDataGridView";
            this.logsDataGridView.ReadOnly = true;
            this.logsDataGridView.RowHeadersVisible = false;
            this.logsDataGridView.RowHeadersWidth = 51;
            this.logsDataGridView.RowTemplate.Height = 40;
            this.logsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.logsDataGridView.Size = new System.Drawing.Size(1370, 345);
            this.logsDataGridView.TabIndex = 4;
            // 
            // iconColumn
            // 
            this.iconColumn.HeaderText = "";
            this.iconColumn.MinimumWidth = 6;
            this.iconColumn.Name = "iconColumn";
            this.iconColumn.ReadOnly = true;
            this.iconColumn.Width = 40;
            // 
            // timestampColumn
            // 
            this.timestampColumn.HeaderText = "Timestamp";
            this.timestampColumn.MinimumWidth = 6;
            this.timestampColumn.Name = "timestampColumn";
            this.timestampColumn.ReadOnly = true;
            this.timestampColumn.Width = 150;
            // 
            // userColumn
            // 
            this.userColumn.HeaderText = "User";
            this.userColumn.MinimumWidth = 6;
            this.userColumn.Name = "userColumn";
            this.userColumn.ReadOnly = true;
            this.userColumn.Width = 120;
            // 
            // activityColumn
            // 
            this.activityColumn.HeaderText = "Activity";
            this.activityColumn.MinimumWidth = 6;
            this.activityColumn.Name = "activityColumn";
            this.activityColumn.ReadOnly = true;
            this.activityColumn.Width = 140;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.MinimumWidth = 6;
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.ReadOnly = true;
            // 
            // severityColumn
            // 
            this.severityColumn.HeaderText = "Severity";
            this.severityColumn.MinimumWidth = 6;
            this.severityColumn.Name = "severityColumn";
            this.severityColumn.ReadOnly = true;
            this.severityColumn.Width = 125;
            // 
            // moduleColumn
            // 
            this.moduleColumn.HeaderText = "Module";
            this.moduleColumn.MinimumWidth = 6;
            this.moduleColumn.Name = "moduleColumn";
            this.moduleColumn.ReadOnly = true;
            this.moduleColumn.Width = 125;
            // 
            // Activitylogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "Activitylogs";
            this.Size = new System.Drawing.Size(1400, 800);
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
            this.totalLogsCard.ResumeLayout(false);
            this.totalLogsCard.PerformLayout();
            this.todayLogsCard.ResumeLayout(false);
            this.todayLogsCard.PerformLayout();
            this.distinctUsersCard.ResumeLayout(false);
            this.distinctUsersCard.PerformLayout();
            this.activeTodayCard.ResumeLayout(false);
            this.activeTodayCard.PerformLayout();
            this.gameRateChangesCard.ResumeLayout(false);
            this.gameRateChangesCard.PerformLayout();
            this.equipmentChangesCard.ResumeLayout(false);
            this.equipmentChangesCard.PerformLayout();
            this.infoCard.ResumeLayout(false);
            this.infoCard.PerformLayout();
            this.warningCard.ResumeLayout(false);
            this.warningCard.PerformLayout();
            this.errorCard.ResumeLayout(false);
            this.errorCard.PerformLayout();
            this.criticalCard.ResumeLayout(false);
            this.criticalCard.PerformLayout();
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