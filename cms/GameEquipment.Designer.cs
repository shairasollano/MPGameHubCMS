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
            // 
            // mainContainer
            // 
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
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
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.headerControlsPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(24, 24);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16);
            this.headerPanel.Size = new System.Drawing.Size(1872, 64);
            this.headerPanel.TabIndex = 0;
            // 
            // headerControlsPanel
            // 
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
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(294, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Game Equipment";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // filterCombo
            // 
            this.filterCombo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.filterCombo.BackColor = System.Drawing.Color.White;
            this.filterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterCombo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.filterCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Items.AddRange(new object[] {
            "All Equipment",
            "Available Only",
            "Unavailable Only",
            "Needs Maintenance"});
            this.filterCombo.Location = new System.Drawing.Point(1150, 4);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(180, 31);
            this.filterCombo.TabIndex = 1;
            // 
            // btnAddEquipment
            // 
            this.btnAddEquipment.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddEquipment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
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
            this.btnAddEquipment.Click += new System.EventHandler(this.btnAddEquipment_Click);
            // 
            // btnManageCategories
            // 
            this.btnManageCategories.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnManageCategories.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
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
            this.btnManageCategories.Click += new System.EventHandler(this.btnManageCategories_Click);
            // 
            // btnViewLogs
            // 
            this.btnViewLogs.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnViewLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
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
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.Transparent;
            this.contentPanel.Controls.Add(this.equipmentFlowPanel);
            this.contentPanel.Controls.Add(this.managementOverlay);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(24, 104);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(1872, 952);
            this.contentPanel.TabIndex = 1;
            // 
            // equipmentFlowPanel
            // 
            this.equipmentFlowPanel.AutoScroll = true;
            this.equipmentFlowPanel.BackColor = System.Drawing.Color.Transparent;
            this.equipmentFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.equipmentFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.equipmentFlowPanel.Name = "equipmentFlowPanel";
            this.equipmentFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.equipmentFlowPanel.Size = new System.Drawing.Size(1872, 952);
            this.equipmentFlowPanel.TabIndex = 0;
            // 
            // managementOverlay
            // 
            this.managementOverlay.BackColor = System.Drawing.Color.White;
            this.managementOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.managementOverlay.Controls.Add(this.managementTabs);
            this.managementOverlay.Controls.Add(this.btnCloseManagement);
            this.managementOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementOverlay.Location = new System.Drawing.Point(0, 0);
            this.managementOverlay.Name = "managementOverlay";
            this.managementOverlay.Padding = new System.Windows.Forms.Padding(24, 24, 24, 64);
            this.managementOverlay.Size = new System.Drawing.Size(1872, 952);
            this.managementOverlay.TabIndex = 1;
            this.managementOverlay.Visible = false;
            // 
            // managementTabs
            // 
            this.managementTabs.Controls.Add(this.tabCategories);
            this.managementTabs.Controls.Add(this.tabConditions);
            this.managementTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementTabs.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.managementTabs.Location = new System.Drawing.Point(24, 24);
            this.managementTabs.Name = "managementTabs";
            this.managementTabs.SelectedIndex = 0;
            this.managementTabs.Size = new System.Drawing.Size(1822, 862);
            this.managementTabs.TabIndex = 0;
            // 
            // tabCategories
            // 
            this.tabCategories.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.tabCategories.Controls.Add(this.categoriesPanel);
            this.tabCategories.Location = new System.Drawing.Point(4, 34);
            this.tabCategories.Name = "tabCategories";
            this.tabCategories.Padding = new System.Windows.Forms.Padding(20);
            this.tabCategories.Size = new System.Drawing.Size(1814, 824);
            this.tabCategories.TabIndex = 0;
            this.tabCategories.Text = "Categories";
            // 
            // categoriesPanel
            // 
            this.categoriesPanel.BackColor = System.Drawing.Color.Transparent;
            this.categoriesPanel.Controls.Add(this.categoriesFlowPanel);
            this.categoriesPanel.Controls.Add(this.btnAddCategory);
            this.categoriesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoriesPanel.Location = new System.Drawing.Point(20, 20);
            this.categoriesPanel.Name = "categoriesPanel";
            this.categoriesPanel.Size = new System.Drawing.Size(1774, 784);
            this.categoriesPanel.TabIndex = 0;
            // 
            // categoriesFlowPanel
            // 
            this.categoriesFlowPanel.AutoScroll = true;
            this.categoriesFlowPanel.BackColor = System.Drawing.Color.Transparent;
            this.categoriesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoriesFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.categoriesFlowPanel.Name = "categoriesFlowPanel";
            this.categoriesFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.categoriesFlowPanel.Size = new System.Drawing.Size(1774, 724);
            this.categoriesFlowPanel.TabIndex = 0;
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnAddCategory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddCategory.FlatAppearance.BorderSize = 0;
            this.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddCategory.ForeColor = System.Drawing.Color.White;
            this.btnAddCategory.Location = new System.Drawing.Point(0, 724);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(1774, 60);
            this.btnAddCategory.TabIndex = 1;
            this.btnAddCategory.Text = "+ Add New Category";
            this.btnAddCategory.UseVisualStyleBackColor = false;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // tabConditions
            // 
            this.tabConditions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.tabConditions.Controls.Add(this.conditionsPanel);
            this.tabConditions.Location = new System.Drawing.Point(4, 34);
            this.tabConditions.Name = "tabConditions";
            this.tabConditions.Padding = new System.Windows.Forms.Padding(20);
            this.tabConditions.Size = new System.Drawing.Size(1814, 808);
            this.tabConditions.TabIndex = 1;
            this.tabConditions.Text = "Conditions";
            // 
            // conditionsPanel
            // 
            this.conditionsPanel.BackColor = System.Drawing.Color.Transparent;
            this.conditionsPanel.Controls.Add(this.conditionsFlowPanel);
            this.conditionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionsPanel.Location = new System.Drawing.Point(20, 20);
            this.conditionsPanel.Name = "conditionsPanel";
            this.conditionsPanel.Size = new System.Drawing.Size(1774, 768);
            this.conditionsPanel.TabIndex = 0;
            // 
            // conditionsFlowPanel
            // 
            this.conditionsFlowPanel.AutoScroll = true;
            this.conditionsFlowPanel.BackColor = System.Drawing.Color.Transparent;
            this.conditionsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionsFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.conditionsFlowPanel.Name = "conditionsFlowPanel";
            this.conditionsFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.conditionsFlowPanel.Size = new System.Drawing.Size(1774, 768);
            this.conditionsFlowPanel.TabIndex = 0;
            // 
            // btnCloseManagement
            // 
            this.btnCloseManagement.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCloseManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnCloseManagement.FlatAppearance.BorderSize = 0;
            this.btnCloseManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseManagement.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnCloseManagement.ForeColor = System.Drawing.Color.White;
            this.btnCloseManagement.Location = new System.Drawing.Point(900, 892);
            this.btnCloseManagement.Name = "btnCloseManagement";
            this.btnCloseManagement.Size = new System.Drawing.Size(120, 40);
            this.btnCloseManagement.TabIndex = 3;
            this.btnCloseManagement.Text = "Close";
            this.btnCloseManagement.UseVisualStyleBackColor = false;
            this.btnCloseManagement.Click += new System.EventHandler(this.btnCloseManagement_Click);
            // 
            // GameEquipment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
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