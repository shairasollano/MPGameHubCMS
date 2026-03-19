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
            this.categoriesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.tabConditions = new System.Windows.Forms.TabPage();
            this.conditionsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCondition = new System.Windows.Forms.Button();
            this.btnCloseManagement = new System.Windows.Forms.Button();
            this.mainContainer.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.headerControlsPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.managementOverlay.SuspendLayout();
            this.managementTabs.SuspendLayout();
            this.tabCategories.SuspendLayout();
            this.tabConditions.SuspendLayout();
            this.SuspendLayout();

            // Color scheme (matching GameRates)
            System.Drawing.Color bgColor = System.Drawing.Color.FromArgb(245, 247, 250);
            System.Drawing.Color primaryColor = System.Drawing.Color.FromArgb(67, 97, 238);
            System.Drawing.Color successColor = System.Drawing.Color.FromArgb(76, 175, 80);
            System.Drawing.Color dangerColor = System.Drawing.Color.FromArgb(244, 67, 54);
            System.Drawing.Color warningColor = System.Drawing.Color.FromArgb(255, 152, 0);
            System.Drawing.Color infoColor = System.Drawing.Color.FromArgb(0, 150, 255);
            System.Drawing.Color textColor = System.Drawing.Color.FromArgb(33, 37, 41);

            // mainContainer
            this.mainContainer.BackColor = bgColor;
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.headerPanel, 0, 0);
            this.mainContainer.Controls.Add(this.contentPanel, 0, 1);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 0);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(20);
            this.mainContainer.RowCount = 2;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Size = new System.Drawing.Size(1665, 891);
            this.mainContainer.TabIndex = 0;

            // headerPanel
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.headerControlsPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(15);
            this.headerPanel.Size = new System.Drawing.Size(1625, 65);
            this.headerPanel.TabIndex = 0;

            // headerControlsPanel
            this.headerControlsPanel.BackColor = System.Drawing.Color.White;
            this.headerControlsPanel.Controls.Add(this.lblTitle);
            this.headerControlsPanel.Controls.Add(this.filterCombo);
            this.headerControlsPanel.Controls.Add(this.btnAddEquipment);
            this.headerControlsPanel.Controls.Add(this.btnManageCategories);
            this.headerControlsPanel.Controls.Add(this.btnViewLogs);
            this.headerControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerControlsPanel.Location = new System.Drawing.Point(15, 15);
            this.headerControlsPanel.Name = "headerControlsPanel";
            this.headerControlsPanel.Size = new System.Drawing.Size(1595, 35);
            this.headerControlsPanel.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = textColor;
            this.lblTitle.Location = new System.Drawing.Point(0, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Game Equipment";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // filterCombo
            this.filterCombo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.filterCombo.BackColor = System.Drawing.Color.White;
            this.filterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterCombo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.filterCombo.ForeColor = textColor;
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Items.AddRange(new object[] {
            "All Equipment",
            "Available Only",
            "Unavailable Only",
            "Needs Maintenance"});
            this.filterCombo.Location = new System.Drawing.Point(850, 3);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(180, 28);
            this.filterCombo.TabIndex = 1;

            // btnAddEquipment
            this.btnAddEquipment.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddEquipment.BackColor = successColor;
            this.btnAddEquipment.FlatAppearance.BorderSize = 0;
            this.btnAddEquipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddEquipment.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddEquipment.ForeColor = System.Drawing.Color.White;
            this.btnAddEquipment.Location = new System.Drawing.Point(1040, 0);
            this.btnAddEquipment.Name = "btnAddEquipment";
            this.btnAddEquipment.Size = new System.Drawing.Size(160, 35);
            this.btnAddEquipment.TabIndex = 2;
            this.btnAddEquipment.Text = "+ Add Equipment";
            this.btnAddEquipment.UseVisualStyleBackColor = false;
            this.btnAddEquipment.Click += new System.EventHandler(this.btnAddEquipment_Click);

            // btnManageCategories
            this.btnManageCategories.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnManageCategories.BackColor = primaryColor;
            this.btnManageCategories.FlatAppearance.BorderSize = 0;
            this.btnManageCategories.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageCategories.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnManageCategories.ForeColor = System.Drawing.Color.White;
            this.btnManageCategories.Location = new System.Drawing.Point(1210, 0);
            this.btnManageCategories.Name = "btnManageCategories";
            this.btnManageCategories.Size = new System.Drawing.Size(180, 35);
            this.btnManageCategories.TabIndex = 3;
            this.btnManageCategories.Text = "Manage Categories";
            this.btnManageCategories.UseVisualStyleBackColor = false;
            this.btnManageCategories.Click += new System.EventHandler(this.btnManageCategories_Click);

            // btnViewLogs
            this.btnViewLogs.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnViewLogs.BackColor = infoColor;
            this.btnViewLogs.FlatAppearance.BorderSize = 0;
            this.btnViewLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewLogs.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnViewLogs.ForeColor = System.Drawing.Color.White;
            this.btnViewLogs.Location = new System.Drawing.Point(1400, 0);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.Size = new System.Drawing.Size(150, 35);
            this.btnViewLogs.TabIndex = 4;
            this.btnViewLogs.Text = "View Logs";
            this.btnViewLogs.UseVisualStyleBackColor = false;
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);

            // contentPanel
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Controls.Add(this.equipmentFlowPanel);
            this.contentPanel.Controls.Add(this.managementOverlay);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(20, 115);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20);
            this.contentPanel.Size = new System.Drawing.Size(1625, 756);
            this.contentPanel.TabIndex = 1;

            // equipmentFlowPanel
            this.equipmentFlowPanel.AutoScroll = true;
            this.equipmentFlowPanel.BackColor = System.Drawing.Color.White;
            this.equipmentFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.equipmentFlowPanel.Location = new System.Drawing.Point(20, 20);
            this.equipmentFlowPanel.Name = "equipmentFlowPanel";
            this.equipmentFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.equipmentFlowPanel.Size = new System.Drawing.Size(1585, 716);
            this.equipmentFlowPanel.TabIndex = 0;

            // managementOverlay
            this.managementOverlay.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.managementOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.managementOverlay.Controls.Add(this.managementTabs);
            this.managementOverlay.Controls.Add(this.btnCloseManagement);
            this.managementOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementOverlay.Location = new System.Drawing.Point(20, 20);
            this.managementOverlay.Name = "managementOverlay";
            this.managementOverlay.Padding = new System.Windows.Forms.Padding(20, 20, 20, 60);
            this.managementOverlay.Size = new System.Drawing.Size(1585, 716);
            this.managementOverlay.TabIndex = 1;
            this.managementOverlay.Visible = false;

            // managementTabs
            this.managementTabs.Controls.Add(this.tabCategories);
            this.managementTabs.Controls.Add(this.tabConditions);
            this.managementTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementTabs.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.managementTabs.Location = new System.Drawing.Point(20, 20);
            this.managementTabs.Name = "managementTabs";
            this.managementTabs.SelectedIndex = 0;
            this.managementTabs.Size = new System.Drawing.Size(1543, 634);
            this.managementTabs.TabIndex = 0;

            // tabCategories
            this.tabCategories.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.tabCategories.Controls.Add(this.categoriesFlowPanel);
            this.tabCategories.Controls.Add(this.btnAddCategory);
            this.tabCategories.Location = new System.Drawing.Point(4, 29);
            this.tabCategories.Name = "tabCategories";
            this.tabCategories.Padding = new System.Windows.Forms.Padding(15);
            this.tabCategories.Size = new System.Drawing.Size(1535, 601);
            this.tabCategories.TabIndex = 0;
            this.tabCategories.Text = "Categories";

            // categoriesFlowPanel
            this.categoriesFlowPanel.AutoScroll = true;
            this.categoriesFlowPanel.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.categoriesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoriesFlowPanel.Location = new System.Drawing.Point(15, 15);
            this.categoriesFlowPanel.Name = "categoriesFlowPanel";
            this.categoriesFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.categoriesFlowPanel.Size = new System.Drawing.Size(1505, 500);
            this.categoriesFlowPanel.TabIndex = 0;

            // btnAddCategory
            this.btnAddCategory.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddCategory.BackColor = successColor;
            this.btnAddCategory.FlatAppearance.BorderSize = 0;
            this.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddCategory.ForeColor = System.Drawing.Color.White;
            this.btnAddCategory.Location = new System.Drawing.Point(690, 530);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(160, 40);
            this.btnAddCategory.TabIndex = 1;
            this.btnAddCategory.Text = "+ Add Category";
            this.btnAddCategory.UseVisualStyleBackColor = false;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);

            // tabConditions
            this.tabConditions.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.tabConditions.Controls.Add(this.conditionsFlowPanel);
            this.tabConditions.Controls.Add(this.btnAddCondition);
            this.tabConditions.Location = new System.Drawing.Point(4, 29);
            this.tabConditions.Name = "tabConditions";
            this.tabConditions.Padding = new System.Windows.Forms.Padding(15);
            this.tabConditions.Size = new System.Drawing.Size(1535, 601);
            this.tabConditions.TabIndex = 1;
            this.tabConditions.Text = "Conditions";

            // conditionsFlowPanel
            this.conditionsFlowPanel.AutoScroll = true;
            this.conditionsFlowPanel.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.conditionsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionsFlowPanel.Location = new System.Drawing.Point(15, 15);
            this.conditionsFlowPanel.Name = "conditionsFlowPanel";
            this.conditionsFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.conditionsFlowPanel.Size = new System.Drawing.Size(1505, 500);
            this.conditionsFlowPanel.TabIndex = 1;

            // btnAddCondition
            this.btnAddCondition.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddCondition.BackColor = successColor;
            this.btnAddCondition.FlatAppearance.BorderSize = 0;
            this.btnAddCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCondition.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddCondition.ForeColor = System.Drawing.Color.White;
            this.btnAddCondition.Location = new System.Drawing.Point(690, 530);
            this.btnAddCondition.Name = "btnAddCondition";
            this.btnAddCondition.Size = new System.Drawing.Size(160, 40);
            this.btnAddCondition.TabIndex = 2;
            this.btnAddCondition.Text = "+ Add Condition";
            this.btnAddCondition.UseVisualStyleBackColor = false;
            this.btnAddCondition.Click += new System.EventHandler(this.btnAddCondition_Click);

            // btnCloseManagement
            this.btnCloseManagement.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCloseManagement.BackColor = dangerColor;
            this.btnCloseManagement.FlatAppearance.BorderSize = 0;
            this.btnCloseManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseManagement.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnCloseManagement.ForeColor = System.Drawing.Color.White;
            this.btnCloseManagement.Location = new System.Drawing.Point(715, 670);
            this.btnCloseManagement.Name = "btnCloseManagement";
            this.btnCloseManagement.Size = new System.Drawing.Size(160, 40);
            this.btnCloseManagement.TabIndex = 3;
            this.btnCloseManagement.Text = "Close";
            this.btnCloseManagement.UseVisualStyleBackColor = false;
            this.btnCloseManagement.Click += new System.EventHandler(this.btnCloseManagement_Click);

            // GameEquipment
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.mainContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "GameEquipment";
            this.Size = new System.Drawing.Size(1665, 891);
            this.mainContainer.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerControlsPanel.ResumeLayout(false);
            this.headerControlsPanel.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            this.managementOverlay.ResumeLayout(false);
            this.managementTabs.ResumeLayout(false);
            this.tabCategories.ResumeLayout(false);
            this.tabConditions.ResumeLayout(false);
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
        private System.Windows.Forms.FlowLayoutPanel categoriesFlowPanel;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.TabPage tabConditions;
        private System.Windows.Forms.FlowLayoutPanel conditionsFlowPanel;
        private System.Windows.Forms.Button btnAddCondition;
        private System.Windows.Forms.Button btnCloseManagement;
    }
}