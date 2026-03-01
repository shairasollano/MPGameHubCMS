namespace cms
{
    partial class GameRates
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.dataPanel = new System.Windows.Forms.Panel();
            this.dgvGameRates = new System.Windows.Forms.DataGridView();
            this.colStatus = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCourtType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGameType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cboFilterStatus = new System.Windows.Forms.ComboBox();
            this.btnManage = new System.Windows.Forms.Button();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.panelManagement = new System.Windows.Forms.Panel();
            this.tabControlManagement = new System.Windows.Forms.TabControl();
            this.tabPageCourts = new System.Windows.Forms.TabPage();
            this.dgvCourts = new System.Windows.Forms.DataGridView();
            this.colCourtId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCourtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCourtDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCourtActions = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelCourtButtons = new System.Windows.Forms.Panel();
            this.btnAddCourt = new System.Windows.Forms.Button();
            this.btnCloseCourts = new System.Windows.Forms.Button();
            this.tabPageGameTypes = new System.Windows.Forms.TabPage();
            this.dgvGameTypes = new System.Windows.Forms.DataGridView();
            this.colGameTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGameTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGameTypeDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGameTypeActions = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelGameTypeButtons = new System.Windows.Forms.Panel();
            this.btnAddGameType = new System.Windows.Forms.Button();
            this.btnCloseGameTypes = new System.Windows.Forms.Button();
            this.mainTableLayout.SuspendLayout();
            this.dataPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGameRates)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panelManagement.SuspendLayout();
            this.tabControlManagement.SuspendLayout();
            this.tabPageCourts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourts)).BeginInit();
            this.panelCourtButtons.SuspendLayout();
            this.tabPageGameTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGameTypes)).BeginInit();
            this.panelGameTypeButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayout
            // 
            this.mainTableLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.mainTableLayout.ColumnCount = 1;
            this.mainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayout.Controls.Add(this.dataPanel, 0, 0);
            this.mainTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayout.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayout.Name = "mainTableLayout";
            this.mainTableLayout.RowCount = 1;
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayout.Size = new System.Drawing.Size(1920, 1080);
            this.mainTableLayout.TabIndex = 0;
            // 
            // dataPanel
            // 
            this.dataPanel.BackColor = System.Drawing.Color.Transparent;
            this.dataPanel.Controls.Add(this.dgvGameRates);
            this.dataPanel.Controls.Add(this.panelHeader);
            this.dataPanel.Controls.Add(this.panelManagement);
            this.dataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPanel.Location = new System.Drawing.Point(0, 0);
            this.dataPanel.Margin = new System.Windows.Forms.Padding(0);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Size = new System.Drawing.Size(1920, 1080);
            this.dataPanel.TabIndex = 0;
            // 
            // dgvGameRates
            // 
            this.dgvGameRates.AllowUserToAddRows = false;
            this.dgvGameRates.AllowUserToDeleteRows = false;
            this.dgvGameRates.AllowUserToResizeRows = false;
            this.dgvGameRates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGameRates.BackgroundColor = System.Drawing.Color.White;
            this.dgvGameRates.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGameRates.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGameRates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvGameRates.ColumnHeadersHeight = 50;
            this.dgvGameRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGameRates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStatus,
            this.colName,
            this.colCourtType,
            this.colGameType,
            this.colRate,
            this.colDescription,
            this.colEdit});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGameRates.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvGameRates.EnableHeadersVisualStyles = false;
            this.dgvGameRates.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.dgvGameRates.Location = new System.Drawing.Point(0, 70);
            this.dgvGameRates.Name = "dgvGameRates";
            this.dgvGameRates.ReadOnly = true;
            this.dgvGameRates.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvGameRates.RowHeadersVisible = false;
            this.dgvGameRates.RowHeadersWidth = 51;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle13.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.dgvGameRates.RowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvGameRates.RowTemplate.Height = 50;
            this.dgvGameRates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGameRates.Size = new System.Drawing.Size(1920, 650);
            this.dgvGameRates.TabIndex = 1;
            // 
            // colStatus
            // 
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 80;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Text = "Enable";
            this.colStatus.Width = 80;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.FillWeight = 18F;
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 150;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colCourtType
            // 
            this.colCourtType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCourtType.FillWeight = 12F;
            this.colCourtType.HeaderText = "Court Type";
            this.colCourtType.MinimumWidth = 120;
            this.colCourtType.Name = "colCourtType";
            this.colCourtType.ReadOnly = true;
            // 
            // colGameType
            // 
            this.colGameType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGameType.FillWeight = 12F;
            this.colGameType.HeaderText = "Game Type";
            this.colGameType.MinimumWidth = 120;
            this.colGameType.Name = "colGameType";
            this.colGameType.ReadOnly = true;
            // 
            // colRate
            // 
            this.colRate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRate.FillWeight = 12F;
            this.colRate.HeaderText = "Rate per hour";
            this.colRate.MinimumWidth = 100;
            this.colRate.Name = "colRate";
            this.colRate.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.FillWeight = 30F;
            this.colDescription.HeaderText = "Description";
            this.colDescription.MinimumWidth = 200;
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colEdit
            // 
            this.colEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEdit.FillWeight = 5F;
            this.colEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colEdit.HeaderText = "";
            this.colEdit.MinimumWidth = 80;
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 80;
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblFilterStatus);
            this.panelHeader.Controls.Add(this.cboFilterStatus);
            this.panelHeader.Controls.Add(this.btnManage);
            this.panelHeader.Controls.Add(this.btnAddNew);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1920, 70);
            this.panelHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(145, 32);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Game Rates";
            // 
            // lblFilterStatus
            // 
            this.lblFilterStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFilterStatus.ForeColor = System.Drawing.Color.White;
            this.lblFilterStatus.Location = new System.Drawing.Point(1137, 28);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(106, 23);
            this.lblFilterStatus.TabIndex = 5;
            this.lblFilterStatus.Text = "Show Status:";
            // 
            // cboFilterStatus
            // 
            this.cboFilterStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cboFilterStatus.BackColor = System.Drawing.Color.White;
            this.cboFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboFilterStatus.FormattingEnabled = true;
            this.cboFilterStatus.Items.AddRange(new object[] {
            "All",
            "Enabled Only",
            "Disabled Only"});
            this.cboFilterStatus.Location = new System.Drawing.Point(1263, 23);
            this.cboFilterStatus.Name = "cboFilterStatus";
            this.cboFilterStatus.Size = new System.Drawing.Size(180, 31);
            this.cboFilterStatus.TabIndex = 4;
            // 
            // btnManage
            // 
            this.btnManage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnManage.FlatAppearance.BorderSize = 0;
            this.btnManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManage.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManage.ForeColor = System.Drawing.Color.White;
            this.btnManage.Location = new System.Drawing.Point(1471, 15);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(200, 44);
            this.btnManage.TabIndex = 3;
            this.btnManage.Text = "Manage Courts";
            this.btnManage.UseVisualStyleBackColor = false;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click_1);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddNew.FlatAppearance.BorderSize = 0;
            this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNew.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.ForeColor = System.Drawing.Color.White;
            this.btnAddNew.Location = new System.Drawing.Point(1689, 15);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(200, 44);
            this.btnAddNew.TabIndex = 0;
            this.btnAddNew.Text = "Add New Rate";
            this.btnAddNew.UseVisualStyleBackColor = false;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click_1);
            // 
            // panelManagement
            // 
            this.panelManagement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelManagement.Controls.Add(this.tabControlManagement);
            this.panelManagement.Location = new System.Drawing.Point(0, 720);
            this.panelManagement.Margin = new System.Windows.Forms.Padding(0);
            this.panelManagement.Name = "panelManagement";
            this.panelManagement.Size = new System.Drawing.Size(1920, 360);
            this.panelManagement.TabIndex = 3;
            this.panelManagement.Visible = false;
            // 
            // tabControlManagement
            // 
            this.tabControlManagement.Controls.Add(this.tabPageCourts);
            this.tabControlManagement.Controls.Add(this.tabPageGameTypes);
            this.tabControlManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlManagement.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControlManagement.Location = new System.Drawing.Point(0, 0);
            this.tabControlManagement.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlManagement.Name = "tabControlManagement";
            this.tabControlManagement.Padding = new System.Drawing.Point(10, 10);
            this.tabControlManagement.SelectedIndex = 0;
            this.tabControlManagement.Size = new System.Drawing.Size(1920, 360);
            this.tabControlManagement.TabIndex = 0;
            // 
            // tabPageCourts
            // 
            this.tabPageCourts.BackColor = System.Drawing.Color.White;
            this.tabPageCourts.Controls.Add(this.dgvCourts);
            this.tabPageCourts.Controls.Add(this.panelCourtButtons);
            this.tabPageCourts.Location = new System.Drawing.Point(4, 46);
            this.tabPageCourts.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageCourts.Name = "tabPageCourts";
            this.tabPageCourts.Size = new System.Drawing.Size(1912, 310);
            this.tabPageCourts.TabIndex = 0;
            this.tabPageCourts.Text = "Manage Courts";
            this.tabPageCourts.UseVisualStyleBackColor = true;
            // 
            // dgvCourts
            // 
            this.dgvCourts.AllowUserToAddRows = false;
            this.dgvCourts.AllowUserToDeleteRows = false;
            this.dgvCourts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCourts.BackgroundColor = System.Drawing.Color.White;
            this.dgvCourts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCourts.ColumnHeadersHeight = 40;
            this.dgvCourts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCourtId,
            this.colCourtName,
            this.colCourtDescription,
            this.colCourtActions});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCourts.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvCourts.Location = new System.Drawing.Point(0, 0);
            this.dgvCourts.Name = "dgvCourts";
            this.dgvCourts.ReadOnly = true;
            this.dgvCourts.RowHeadersVisible = false;
            this.dgvCourts.RowHeadersWidth = 51;
            this.dgvCourts.RowTemplate.Height = 40;
            this.dgvCourts.Size = new System.Drawing.Size(1912, 260);
            this.dgvCourts.TabIndex = 0;
            // 
            // colCourtId
            // 
            this.colCourtId.HeaderText = "ID";
            this.colCourtId.MinimumWidth = 6;
            this.colCourtId.Name = "colCourtId";
            this.colCourtId.ReadOnly = true;
            this.colCourtId.Width = 50;
            // 
            // colCourtName
            // 
            this.colCourtName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCourtName.HeaderText = "Court Name";
            this.colCourtName.MinimumWidth = 6;
            this.colCourtName.Name = "colCourtName";
            this.colCourtName.ReadOnly = true;
            // 
            // colCourtDescription
            // 
            this.colCourtDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCourtDescription.HeaderText = "Description";
            this.colCourtDescription.MinimumWidth = 6;
            this.colCourtDescription.Name = "colCourtDescription";
            this.colCourtDescription.ReadOnly = true;
            // 
            // colCourtActions
            // 
            this.colCourtActions.HeaderText = "Actions";
            this.colCourtActions.MinimumWidth = 6;
            this.colCourtActions.Name = "colCourtActions";
            this.colCourtActions.ReadOnly = true;
            this.colCourtActions.Text = "Delete";
            this.colCourtActions.UseColumnTextForButtonValue = true;
            this.colCourtActions.Width = 120;
            // 
            // panelCourtButtons
            // 
            this.panelCourtButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCourtButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelCourtButtons.Controls.Add(this.btnAddCourt);
            this.panelCourtButtons.Controls.Add(this.btnCloseCourts);
            this.panelCourtButtons.Location = new System.Drawing.Point(0, 260);
            this.panelCourtButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelCourtButtons.Name = "panelCourtButtons";
            this.panelCourtButtons.Size = new System.Drawing.Size(1912, 60);
            this.panelCourtButtons.TabIndex = 1;
            // 
            // btnAddCourt
            // 
            this.btnAddCourt.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddCourt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddCourt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCourt.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAddCourt.ForeColor = System.Drawing.Color.White;
            this.btnAddCourt.Location = new System.Drawing.Point(1550, 10);
            this.btnAddCourt.Name = "btnAddCourt";
            this.btnAddCourt.Size = new System.Drawing.Size(150, 40);
            this.btnAddCourt.TabIndex = 0;
            this.btnAddCourt.Text = "Add New Court";
            this.btnAddCourt.UseVisualStyleBackColor = false;
            // 
            // btnCloseCourts
            // 
            this.btnCloseCourts.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCloseCourts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCloseCourts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseCourts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCloseCourts.ForeColor = System.Drawing.Color.White;
            this.btnCloseCourts.Location = new System.Drawing.Point(1720, 10);
            this.btnCloseCourts.Name = "btnCloseCourts";
            this.btnCloseCourts.Size = new System.Drawing.Size(150, 40);
            this.btnCloseCourts.TabIndex = 1;
            this.btnCloseCourts.Text = "Close";
            this.btnCloseCourts.UseVisualStyleBackColor = false;
            // 
            // tabPageGameTypes
            // 
            this.tabPageGameTypes.BackColor = System.Drawing.Color.White;
            this.tabPageGameTypes.Controls.Add(this.dgvGameTypes);
            this.tabPageGameTypes.Controls.Add(this.panelGameTypeButtons);
            this.tabPageGameTypes.Location = new System.Drawing.Point(4, 46);
            this.tabPageGameTypes.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageGameTypes.Name = "tabPageGameTypes";
            this.tabPageGameTypes.Size = new System.Drawing.Size(1912, 310);
            this.tabPageGameTypes.TabIndex = 1;
            this.tabPageGameTypes.Text = "Manage Game Types";
            this.tabPageGameTypes.UseVisualStyleBackColor = true;
            // 
            // dgvGameTypes
            // 
            this.dgvGameTypes.AllowUserToAddRows = false;
            this.dgvGameTypes.AllowUserToDeleteRows = false;
            this.dgvGameTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGameTypes.BackgroundColor = System.Drawing.Color.White;
            this.dgvGameTypes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGameTypes.ColumnHeadersHeight = 40;
            this.dgvGameTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGameTypeId,
            this.colGameTypeName,
            this.colGameTypeDescription,
            this.colGameTypeActions});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGameTypes.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvGameTypes.Location = new System.Drawing.Point(0, 0);
            this.dgvGameTypes.Name = "dgvGameTypes";
            this.dgvGameTypes.ReadOnly = true;
            this.dgvGameTypes.RowHeadersVisible = false;
            this.dgvGameTypes.RowHeadersWidth = 51;
            this.dgvGameTypes.RowTemplate.Height = 40;
            this.dgvGameTypes.Size = new System.Drawing.Size(1912, 260);
            this.dgvGameTypes.TabIndex = 1;
            // 
            // colGameTypeId
            // 
            this.colGameTypeId.HeaderText = "ID";
            this.colGameTypeId.MinimumWidth = 6;
            this.colGameTypeId.Name = "colGameTypeId";
            this.colGameTypeId.ReadOnly = true;
            this.colGameTypeId.Width = 50;
            // 
            // colGameTypeName
            // 
            this.colGameTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGameTypeName.HeaderText = "Game Type";
            this.colGameTypeName.MinimumWidth = 6;
            this.colGameTypeName.Name = "colGameTypeName";
            this.colGameTypeName.ReadOnly = true;
            // 
            // colGameTypeDescription
            // 
            this.colGameTypeDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGameTypeDescription.HeaderText = "Description";
            this.colGameTypeDescription.MinimumWidth = 6;
            this.colGameTypeDescription.Name = "colGameTypeDescription";
            this.colGameTypeDescription.ReadOnly = true;
            // 
            // colGameTypeActions
            // 
            this.colGameTypeActions.HeaderText = "Actions";
            this.colGameTypeActions.MinimumWidth = 6;
            this.colGameTypeActions.Name = "colGameTypeActions";
            this.colGameTypeActions.ReadOnly = true;
            this.colGameTypeActions.Text = "Delete";
            this.colGameTypeActions.UseColumnTextForButtonValue = true;
            this.colGameTypeActions.Width = 120;
            // 
            // panelGameTypeButtons
            // 
            this.panelGameTypeButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGameTypeButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelGameTypeButtons.Controls.Add(this.btnAddGameType);
            this.panelGameTypeButtons.Controls.Add(this.btnCloseGameTypes);
            this.panelGameTypeButtons.Location = new System.Drawing.Point(0, 260);
            this.panelGameTypeButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelGameTypeButtons.Name = "panelGameTypeButtons";
            this.panelGameTypeButtons.Size = new System.Drawing.Size(1912, 60);
            this.panelGameTypeButtons.TabIndex = 2;
            // 
            // btnAddGameType
            // 
            this.btnAddGameType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddGameType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddGameType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGameType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAddGameType.ForeColor = System.Drawing.Color.White;
            this.btnAddGameType.Location = new System.Drawing.Point(1550, 10);
            this.btnAddGameType.Name = "btnAddGameType";
            this.btnAddGameType.Size = new System.Drawing.Size(150, 40);
            this.btnAddGameType.TabIndex = 1;
            this.btnAddGameType.Text = "Add New Type";
            this.btnAddGameType.UseVisualStyleBackColor = false;
            // 
            // btnCloseGameTypes
            // 
            this.btnCloseGameTypes.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCloseGameTypes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCloseGameTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseGameTypes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCloseGameTypes.ForeColor = System.Drawing.Color.White;
            this.btnCloseGameTypes.Location = new System.Drawing.Point(1720, 10);
            this.btnCloseGameTypes.Name = "btnCloseGameTypes";
            this.btnCloseGameTypes.Size = new System.Drawing.Size(150, 40);
            this.btnCloseGameTypes.TabIndex = 2;
            this.btnCloseGameTypes.Text = "Close";
            this.btnCloseGameTypes.UseVisualStyleBackColor = false;
            // 
            // GameRates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.mainTableLayout);
            this.Name = "GameRates";
            this.Size = new System.Drawing.Size(1920, 1080);
            this.mainTableLayout.ResumeLayout(false);
            this.dataPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGameRates)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelManagement.ResumeLayout(false);
            this.tabControlManagement.ResumeLayout(false);
            this.tabPageCourts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourts)).EndInit();
            this.panelCourtButtons.ResumeLayout(false);
            this.tabPageGameTypes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGameTypes)).EndInit();
            this.panelGameTypeButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayout;
        private System.Windows.Forms.Panel dataPanel;
        private System.Windows.Forms.DataGridView dgvGameRates;
        private System.Windows.Forms.DataGridViewButtonColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCourtType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGameType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cboFilterStatus;
        private System.Windows.Forms.Button btnManage;

        // Management Panel Controls
        private System.Windows.Forms.Panel panelManagement;
        private System.Windows.Forms.TabControl tabControlManagement;
        private System.Windows.Forms.TabPage tabPageCourts;
        private System.Windows.Forms.DataGridView dgvCourts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCourtId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCourtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCourtDescription;
        private System.Windows.Forms.DataGridViewButtonColumn colCourtActions;
        private System.Windows.Forms.Panel panelCourtButtons;
        private System.Windows.Forms.Button btnAddCourt;
        private System.Windows.Forms.Button btnCloseCourts;
        private System.Windows.Forms.TabPage tabPageGameTypes;
        private System.Windows.Forms.DataGridView dgvGameTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGameTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGameTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGameTypeDescription;
        private System.Windows.Forms.DataGridViewButtonColumn colGameTypeActions;
        private System.Windows.Forms.Panel panelGameTypeButtons;
        private System.Windows.Forms.Button btnAddGameType;
        private System.Windows.Forms.Button btnCloseGameTypes;
    }
}