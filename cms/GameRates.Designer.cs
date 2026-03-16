using System.Drawing;

namespace cms
{
    partial class GameRates
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
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnManage = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.ratesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.managementPanel = new System.Windows.Forms.Panel();
            this.managementTabs = new System.Windows.Forms.TabControl();
            this.tabCourts = new System.Windows.Forms.TabPage();
            this.courtsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCourt = new System.Windows.Forms.Button();
            this.tabGameTypes = new System.Windows.Forms.TabPage();
            this.gameTypesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddGameType = new System.Windows.Forms.Button();
            this.mainContainer.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.headerControlsPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.managementPanel.SuspendLayout();
            this.managementTabs.SuspendLayout();
            this.tabCourts.SuspendLayout();
            this.tabGameTypes.SuspendLayout();
            this.SuspendLayout();

            // Color scheme
            Color bgColor = Color.FromArgb(245, 247, 250);
            Color primaryColor = Color.FromArgb(67, 97, 238);
            Color successColor = Color.FromArgb(76, 175, 80);
            Color dangerColor = Color.FromArgb(244, 67, 54);
            Color textColor = Color.FromArgb(33, 37, 41);

            // mainContainer
            this.mainContainer.BackColor = bgColor;
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.headerPanel, 0, 0);
            this.mainContainer.Controls.Add(this.contentPanel, 0, 1);
            this.mainContainer.Controls.Add(this.managementPanel, 0, 2);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 0);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(20);
            this.mainContainer.RowCount = 3;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainContainer.Size = new System.Drawing.Size(1920, 1080);
            this.mainContainer.TabIndex = 0;

            // headerPanel
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Controls.Add(this.headerControlsPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(15);
            this.headerPanel.Size = new System.Drawing.Size(1880, 65);
            this.headerPanel.TabIndex = 0;

            // headerControlsPanel
            this.headerControlsPanel.BackColor = Color.White;
            this.headerControlsPanel.Controls.Add(this.lblTitle);
            this.headerControlsPanel.Controls.Add(this.filterCombo);
            this.headerControlsPanel.Controls.Add(this.btnAddNew);
            this.headerControlsPanel.Controls.Add(this.btnManage);
            this.headerControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerControlsPanel.Location = new System.Drawing.Point(15, 15);
            this.headerControlsPanel.Name = "headerControlsPanel";
            this.headerControlsPanel.Size = new System.Drawing.Size(1850, 35);
            this.headerControlsPanel.TabIndex = 0;

            // lblTitle - FIXED: Properly positioned and sized
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = textColor;
            this.lblTitle.Location = new System.Drawing.Point(0, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(263, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Game Rates Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // filterCombo - FIXED: Properly aligned to the right
            this.filterCombo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.filterCombo.BackColor = Color.White;
            this.filterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterCombo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.filterCombo.ForeColor = textColor;
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Items.AddRange(new object[] {
            "All Rates",
            "Active Only",
            "Inactive Only"});
            this.filterCombo.Location = new System.Drawing.Point(1250, 3);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(180, 28);
            this.filterCombo.TabIndex = 1;
            this.filterCombo.SelectedIndexChanged += new System.EventHandler(this.filterCombo_SelectedIndexChanged);

            // btnAddNew - FIXED: Properly positioned
            this.btnAddNew.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddNew.BackColor = successColor;
            this.btnAddNew.FlatAppearance.BorderSize = 0;
            this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNew.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddNew.ForeColor = Color.White;
            this.btnAddNew.Location = new System.Drawing.Point(1440, 0);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(140, 35);
            this.btnAddNew.TabIndex = 2;
            this.btnAddNew.Text = "+ New Rate";
            this.btnAddNew.UseVisualStyleBackColor = false;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);

            // btnManage - FIXED: Properly positioned
            this.btnManage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnManage.BackColor = primaryColor;
            this.btnManage.FlatAppearance.BorderSize = 0;
            this.btnManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManage.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnManage.ForeColor = Color.White;
            this.btnManage.Location = new System.Drawing.Point(1590, 0);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(160, 35);
            this.btnManage.TabIndex = 3;
            this.btnManage.Text = "Manage Courts";
            this.btnManage.UseVisualStyleBackColor = false;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);

            // contentPanel
            this.contentPanel.AutoScroll = true;
            this.contentPanel.BackColor = Color.White;
            this.contentPanel.Controls.Add(this.ratesFlowPanel);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(20, 100);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20);
            this.contentPanel.Size = new System.Drawing.Size(1880, 565);
            this.contentPanel.TabIndex = 1;

            // ratesFlowPanel
            this.ratesFlowPanel.AutoScroll = true;
            this.ratesFlowPanel.BackColor = Color.White;
            this.ratesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratesFlowPanel.Location = new System.Drawing.Point(20, 20);
            this.ratesFlowPanel.Name = "ratesFlowPanel";
            this.ratesFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.ratesFlowPanel.Size = new System.Drawing.Size(1840, 525);
            this.ratesFlowPanel.TabIndex = 0;

            // managementPanel
            this.managementPanel.BackColor = Color.White;
            this.managementPanel.Controls.Add(this.managementTabs);
            this.managementPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementPanel.Location = new System.Drawing.Point(20, 680);
            this.managementPanel.Margin = new System.Windows.Forms.Padding(0);
            this.managementPanel.Name = "managementPanel";
            this.managementPanel.Padding = new System.Windows.Forms.Padding(20);
            this.managementPanel.Size = new System.Drawing.Size(1880, 380);
            this.managementPanel.TabIndex = 2;
            this.managementPanel.Visible = false;

            // managementTabs
            this.managementTabs.Controls.Add(this.tabCourts);
            this.managementTabs.Controls.Add(this.tabGameTypes);
            this.managementTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementTabs.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.managementTabs.Location = new System.Drawing.Point(20, 20);
            this.managementTabs.Name = "managementTabs";
            this.managementTabs.SelectedIndex = 0;
            this.managementTabs.Size = new System.Drawing.Size(1840, 340);
            this.managementTabs.TabIndex = 0;

            // tabCourts
            this.tabCourts.BackColor = Color.White;
            this.tabCourts.Controls.Add(this.courtsFlowPanel);
            this.tabCourts.Controls.Add(this.btnAddCourt);
            this.tabCourts.Location = new System.Drawing.Point(4, 29);
            this.tabCourts.Name = "tabCourts";
            this.tabCourts.Padding = new System.Windows.Forms.Padding(15);
            this.tabCourts.Size = new System.Drawing.Size(1832, 307);
            this.tabCourts.TabIndex = 0;
            this.tabCourts.Text = "Courts";

            // courtsFlowPanel
            this.courtsFlowPanel.AutoScroll = true;
            this.courtsFlowPanel.BackColor = Color.White;
            this.courtsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.courtsFlowPanel.Location = new System.Drawing.Point(15, 15);
            this.courtsFlowPanel.Name = "courtsFlowPanel";
            this.courtsFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.courtsFlowPanel.Size = new System.Drawing.Size(1802, 227);
            this.courtsFlowPanel.TabIndex = 0;

            // btnAddCourt
            this.btnAddCourt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddCourt.BackColor = successColor;
            this.btnAddCourt.FlatAppearance.BorderSize = 0;
            this.btnAddCourt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCourt.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddCourt.ForeColor = Color.White;
            this.btnAddCourt.Location = new System.Drawing.Point(836, 252);
            this.btnAddCourt.Name = "btnAddCourt";
            this.btnAddCourt.Size = new System.Drawing.Size(160, 40);
            this.btnAddCourt.TabIndex = 1;
            this.btnAddCourt.Text = "+ Add Court";
            this.btnAddCourt.UseVisualStyleBackColor = false;
            this.btnAddCourt.Click += new System.EventHandler(this.btnAddCourt_Click);

            // tabGameTypes
            this.tabGameTypes.BackColor = Color.White;
            this.tabGameTypes.Controls.Add(this.gameTypesFlowPanel);
            this.tabGameTypes.Controls.Add(this.btnAddGameType);
            this.tabGameTypes.Location = new System.Drawing.Point(4, 29);
            this.tabGameTypes.Name = "tabGameTypes";
            this.tabGameTypes.Padding = new System.Windows.Forms.Padding(15);
            this.tabGameTypes.Size = new System.Drawing.Size(1832, 307);
            this.tabGameTypes.TabIndex = 1;
            this.tabGameTypes.Text = "Game Types";

            // gameTypesFlowPanel
            this.gameTypesFlowPanel.AutoScroll = true;
            this.gameTypesFlowPanel.BackColor = Color.White;
            this.gameTypesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameTypesFlowPanel.Location = new System.Drawing.Point(15, 15);
            this.gameTypesFlowPanel.Name = "gameTypesFlowPanel";
            this.gameTypesFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.gameTypesFlowPanel.Size = new System.Drawing.Size(1802, 227);
            this.gameTypesFlowPanel.TabIndex = 1;

            // btnAddGameType
            this.btnAddGameType.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddGameType.BackColor = successColor;
            this.btnAddGameType.FlatAppearance.BorderSize = 0;
            this.btnAddGameType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGameType.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddGameType.ForeColor = Color.White;
            this.btnAddGameType.Location = new System.Drawing.Point(826, 252);
            this.btnAddGameType.Name = "btnAddGameType";
            this.btnAddGameType.Size = new System.Drawing.Size(180, 40);
            this.btnAddGameType.TabIndex = 2;
            this.btnAddGameType.Text = "+ Add Game Type";
            this.btnAddGameType.UseVisualStyleBackColor = false;
            this.btnAddGameType.Click += new System.EventHandler(this.btnAddGameType_Click);

            // GameRates
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.mainContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "GameRates";
            this.Size = new System.Drawing.Size(1920, 1080);
            this.mainContainer.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerControlsPanel.ResumeLayout(false);
            this.headerControlsPanel.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            this.managementPanel.ResumeLayout(false);
            this.managementTabs.ResumeLayout(false);
            this.tabCourts.ResumeLayout(false);
            this.tabGameTypes.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel headerControlsPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox filterCombo;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.FlowLayoutPanel ratesFlowPanel;
        private System.Windows.Forms.Panel managementPanel;
        private System.Windows.Forms.TabControl managementTabs;
        private System.Windows.Forms.TabPage tabCourts;
        private System.Windows.Forms.FlowLayoutPanel courtsFlowPanel;
        private System.Windows.Forms.Button btnAddCourt;
        private System.Windows.Forms.TabPage tabGameTypes;
        private System.Windows.Forms.FlowLayoutPanel gameTypesFlowPanel;
        private System.Windows.Forms.Button btnAddGameType;
    }
}