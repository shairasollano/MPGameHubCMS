namespace cms
{
    partial class GameEquipment
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
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerControlsPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.filterCombo = new System.Windows.Forms.ComboBox();
            this.btnAddEquipment = new System.Windows.Forms.Button();
            this.btnManageCategories = new System.Windows.Forms.Button();
            this.btnViewLogs = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.equipmentFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.managementOverlay = new System.Windows.Forms.Panel();
            this.managementTabs = new System.Windows.Forms.TabControl();
            this.tabCategories = new System.Windows.Forms.TabPage();
            this.categoriesPanel = new System.Windows.Forms.Panel();
            this.categoriesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.tabConditions = new System.Windows.Forms.TabPage();
            this.conditionsPanel = new System.Windows.Forms.Panel();
            this.conditionsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCloseManagement = new System.Windows.Forms.Button();

            this.mainContainer.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.headerControlsPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.managementOverlay.SuspendLayout();
            this.managementTabs.SuspendLayout();
            this.tabCategories.SuspendLayout();
            this.categoriesPanel.SuspendLayout();
            this.tabConditions.SuspendLayout();
            this.conditionsPanel.SuspendLayout();
            this.SuspendLayout();

            // Modern color scheme
            System.Drawing.Color bgColor = System.Drawing.Color.FromArgb(248, 250, 252);
            System.Drawing.Color primaryColor = System.Drawing.Color.FromArgb(79, 70, 229);
            System.Drawing.Color successColor = System.Drawing.Color.FromArgb(16, 185, 129);
            System.Drawing.Color dangerColor = System.Drawing.Color.FromArgb(239, 68, 68);
            System.Drawing.Color warningColor = System.Drawing.Color.FromArgb(245, 158, 11);
            System.Drawing.Color infoColor = System.Drawing.Color.FromArgb(59, 130, 246);
            System.Drawing.Color textColor = System.Drawing.Color.FromArgb(17, 24, 39);
            System.Drawing.Color cardBgColor = System.Drawing.Color.White;
            System.Drawing.Color hoverColor = System.Drawing.Color.FromArgb(249, 250, 251);
            System.Drawing.Color borderColor = System.Drawing.Color.FromArgb(229, 231, 235);

            // mainContainer
            this.mainContainer.BackColor = bgColor;
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.headerPanel, 0, 0);
            this.mainContainer.Controls.Add(this.contentPanel, 0, 1);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 0);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(24);
            this.mainContainer.RowCount = 2;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Size = new System.Drawing.Size(1920, 1080);
            this.mainContainer.TabIndex = 0;

            // headerPanel
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.headerPanel.Controls.Add(this.headerControlsPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(24, 24);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16);
            this.headerPanel.Size = new System.Drawing.Size(1872, 64);
            this.headerPanel.TabIndex = 0;

            // headerControlsPanel
            this.headerControlsPanel.BackColor = System.Drawing.Color.White;
            this.headerControlsPanel.Controls.Add(this.lblTitle);
            this.headerControlsPanel.Controls.Add(this.filterCombo);
            this.headerControlsPanel.Controls.Add(this.btnAddEquipment);
            this.headerControlsPanel.Controls.Add(this.btnManageCategories);
            this.headerControlsPanel.Controls.Add(this.btnViewLogs);
            this.headerControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerControlsPanel.Location = new System.Drawing.Point(16, 16);
            this.headerControlsPanel.Name = "headerControlsPanel";
            this.headerControlsPanel.Size = new System.Drawing.Size(1840, 32);
            this.headerControlsPanel.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = textColor;
            this.lblTitle.Location = new System.Drawing.Point(0, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(232, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Game Equipment";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // filterCombo
            this.filterCombo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.filterCombo.BackColor = System.Drawing.Color.White;
            this.filterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterCombo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.filterCombo.ForeColor = textColor;
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Items.AddRange(new object[] {
            "All Equipment",
            "Available Only",
            "Unavailable Only",
            "Needs Maintenance"});
            this.filterCombo.Location = new System.Drawing.Point(1150, 4);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(180, 25);
            this.filterCombo.TabIndex = 1;
            // REMOVED: this.filterCombo.SelectedIndexChanged += new System.EventHandler(this.filterCombo_SelectedIndexChanged);

            // btnAddEquipment
            this.btnAddEquipment.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddEquipment.BackColor = successColor;
            this.btnAddEquipment.FlatAppearance.BorderSize = 0;
            this.btnAddEquipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddEquipment.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddEquipment.ForeColor = System.Drawing.Color.White;
            this.btnAddEquipment.Location = new System.Drawing.Point(1340, 0);
            this.btnAddEquipment.Name = "btnAddEquipment";
            this.btnAddEquipment.Size = new System.Drawing.Size(150, 32);
            this.btnAddEquipment.TabIndex = 2;
            this.btnAddEquipment.Text = "+ Add Equipment";
            this.btnAddEquipment.UseVisualStyleBackColor = false;
            // REMOVED: this.btnAddEquipment.Click += new System.EventHandler(this.btnAddEquipment_Click);

            // btnManageCategories
            this.btnManageCategories.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnManageCategories.BackColor = primaryColor;
            this.btnManageCategories.FlatAppearance.BorderSize = 0;
            this.btnManageCategories.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageCategories.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnManageCategories.ForeColor = System.Drawing.Color.White;
            this.btnManageCategories.Location = new System.Drawing.Point(1500, 0);
            this.btnManageCategories.Name = "btnManageCategories";
            this.btnManageCategories.Size = new System.Drawing.Size(170, 32);
            this.btnManageCategories.TabIndex = 3;
            this.btnManageCategories.Text = "Manage Categories";
            this.btnManageCategories.UseVisualStyleBackColor = false;
            // REMOVED: this.btnManageCategories.Click += new System.EventHandler(this.btnManageCategories_Click);

            // btnViewLogs
            this.btnViewLogs.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnViewLogs.BackColor = infoColor;
            this.btnViewLogs.FlatAppearance.BorderSize = 0;
            this.btnViewLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewLogs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewLogs.ForeColor = System.Drawing.Color.White;
            this.btnViewLogs.Location = new System.Drawing.Point(1680, 0);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.Size = new System.Drawing.Size(130, 32);
            this.btnViewLogs.TabIndex = 4;
            this.btnViewLogs.Text = "View Logs";
            this.btnViewLogs.UseVisualStyleBackColor = false;
            // REMOVED: this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);

            // contentPanel
            this.contentPanel.BackColor = System.Drawing.Color.Transparent;
            this.contentPanel.Controls.Add(this.equipmentFlowPanel);
            this.contentPanel.Controls.Add(this.managementOverlay);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(24, 120);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0);
            this.contentPanel.Size = new System.Drawing.Size(1872, 936);
            this.contentPanel.TabIndex = 1;

            // equipmentFlowPanel
            this.equipmentFlowPanel.AutoScroll = true;
            this.equipmentFlowPanel.BackColor = System.Drawing.Color.Transparent;
            this.equipmentFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.equipmentFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.equipmentFlowPanel.Name = "equipmentFlowPanel";
            this.equipmentFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.equipmentFlowPanel.Size = new System.Drawing.Size(1872, 936);
            this.equipmentFlowPanel.TabIndex = 0;

            // managementOverlay
            this.managementOverlay.BackColor = System.Drawing.Color.White;
            this.managementOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.managementOverlay.Controls.Add(this.managementTabs);
            this.managementOverlay.Controls.Add(this.btnCloseManagement);
            this.managementOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementOverlay.Location = new System.Drawing.Point(0, 0);
            this.managementOverlay.Name = "managementOverlay";
            this.managementOverlay.Padding = new System.Windows.Forms.Padding(24, 24, 24, 64);
            this.managementOverlay.Size = new System.Drawing.Size(1872, 936);
            this.managementOverlay.TabIndex = 1;
            this.managementOverlay.Visible = false;

            // managementTabs
            this.managementTabs.Controls.Add(this.tabCategories);
            this.managementTabs.Controls.Add(this.tabConditions);
            this.managementTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementTabs.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.managementTabs.Location = new System.Drawing.Point(24, 24);
            this.managementTabs.Name = "managementTabs";
            this.managementTabs.SelectedIndex = 0;
            this.managementTabs.Size = new System.Drawing.Size(1822, 846);
            this.managementTabs.TabIndex = 0;
            // REMOVED: this.managementTabs.SelectedIndexChanged += new System.EventHandler(this.ManagementTabs_SelectedIndexChanged);

            // ============== CATEGORIES TAB ==============
            this.tabCategories.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.tabCategories.Controls.Add(this.categoriesPanel);
            this.tabCategories.Location = new System.Drawing.Point(4, 29);
            this.tabCategories.Name = "tabCategories";
            this.tabCategories.Padding = new System.Windows.Forms.Padding(20);
            this.tabCategories.Size = new System.Drawing.Size(1814, 813);
            this.tabCategories.TabIndex = 0;
            this.tabCategories.Text = "Categories";

            // categoriesPanel
            this.categoriesPanel.BackColor = System.Drawing.Color.Transparent;
            this.categoriesPanel.Controls.Add(this.categoriesFlowPanel);
            this.categoriesPanel.Controls.Add(this.btnAddCategory);
            this.categoriesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoriesPanel.Location = new System.Drawing.Point(20, 20);
            this.categoriesPanel.Name = "categoriesPanel";
            this.categoriesPanel.Size = new System.Drawing.Size(1774, 773);
            this.categoriesPanel.TabIndex = 0;

            // categoriesFlowPanel
            this.categoriesFlowPanel.AutoScroll = true;
            this.categoriesFlowPanel.BackColor = System.Drawing.Color.Transparent;
            this.categoriesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoriesFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.categoriesFlowPanel.Name = "categoriesFlowPanel";
            this.categoriesFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.categoriesFlowPanel.Size = new System.Drawing.Size(1774, 713);
            this.categoriesFlowPanel.TabIndex = 0;

            // btnAddCategory
            this.btnAddCategory.BackColor = successColor;
            this.btnAddCategory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddCategory.FlatAppearance.BorderSize = 0;
            this.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddCategory.ForeColor = System.Drawing.Color.White;
            this.btnAddCategory.Location = new System.Drawing.Point(0, 713);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(1774, 60);
            this.btnAddCategory.TabIndex = 1;
            this.btnAddCategory.Text = "+ Add New Category";
            this.btnAddCategory.UseVisualStyleBackColor = false;
            // REMOVED: this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);

            // Add controls to categoriesPanel
            this.categoriesPanel.Controls.Add(this.categoriesFlowPanel);
            this.categoriesPanel.Controls.Add(this.btnAddCategory);

            // ============== CONDITIONS TAB ==============
            this.tabConditions.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.tabConditions.Controls.Add(this.conditionsPanel);
            this.tabConditions.Location = new System.Drawing.Point(4, 29);
            this.tabConditions.Name = "tabConditions";
            this.tabConditions.Padding = new System.Windows.Forms.Padding(20);
            this.tabConditions.Size = new System.Drawing.Size(1814, 813);
            this.tabConditions.TabIndex = 1;
            this.tabConditions.Text = "Conditions";

            // conditionsPanel
            this.conditionsPanel.BackColor = System.Drawing.Color.Transparent;
            this.conditionsPanel.Controls.Add(this.conditionsFlowPanel);
            this.conditionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionsPanel.Location = new System.Drawing.Point(20, 20);
            this.conditionsPanel.Name = "conditionsPanel";
            this.conditionsPanel.Size = new System.Drawing.Size(1774, 773);
            this.conditionsPanel.TabIndex = 0;

            // conditionsFlowPanel
            this.conditionsFlowPanel.AutoScroll = true;
            this.conditionsFlowPanel.BackColor = System.Drawing.Color.Transparent;
            this.conditionsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionsFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.conditionsFlowPanel.Name = "conditionsFlowPanel";
            this.conditionsFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.conditionsFlowPanel.Size = new System.Drawing.Size(1774, 773);
            this.conditionsFlowPanel.TabIndex = 0;

            // Add controls to conditionsPanel
            this.conditionsPanel.Controls.Add(this.conditionsFlowPanel);

            // btnCloseManagement
            this.btnCloseManagement.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCloseManagement.BackColor = dangerColor;
            this.btnCloseManagement.FlatAppearance.BorderSize = 0;
            this.btnCloseManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseManagement.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnCloseManagement.ForeColor = System.Drawing.Color.White;
            this.btnCloseManagement.Location = new System.Drawing.Point(900, 876);
            this.btnCloseManagement.Name = "btnCloseManagement";
            this.btnCloseManagement.Size = new System.Drawing.Size(120, 40);
            this.btnCloseManagement.TabIndex = 3;
            this.btnCloseManagement.Text = "Close";
            this.btnCloseManagement.UseVisualStyleBackColor = false;
            // REMOVED: this.btnCloseManagement.Click += new System.EventHandler(this.btnCloseManagement_Click);

            // GameEquipment
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.mainContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "GameEquipment";
            this.Size = new System.Drawing.Size(1920, 1080);
            this.mainContainer.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerControlsPanel.ResumeLayout(false);
            this.headerControlsPanel.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            this.managementOverlay.ResumeLayout(false);
            this.managementTabs.ResumeLayout(false);
            this.tabCategories.ResumeLayout(false);
            this.categoriesPanel.ResumeLayout(false);
            this.tabConditions.ResumeLayout(false);
            this.conditionsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // Control declarations
        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel headerControlsPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox filterCombo;
        private System.Windows.Forms.Button btnAddEquipment;
        private System.Windows.Forms.Button btnManageCategories;
        private System.Windows.Forms.Button btnViewLogs;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.FlowLayoutPanel equipmentFlowPanel;
        private System.Windows.Forms.Panel managementOverlay;
        private System.Windows.Forms.TabControl managementTabs;
        private System.Windows.Forms.TabPage tabCategories;
        private System.Windows.Forms.Panel categoriesPanel;
        private System.Windows.Forms.FlowLayoutPanel categoriesFlowPanel;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.TabPage tabConditions;
        private System.Windows.Forms.Panel conditionsPanel;
        private System.Windows.Forms.FlowLayoutPanel conditionsFlowPanel;
        private System.Windows.Forms.Button btnCloseManagement;
    }
}