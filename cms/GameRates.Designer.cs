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
            this.managementOverlay = new System.Windows.Forms.Panel();
            this.managementTabs = new System.Windows.Forms.TabControl();
            this.tabCourts = new System.Windows.Forms.TabPage();
            this.courtsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCourt = new System.Windows.Forms.Button();
            this.tabGameTypes = new System.Windows.Forms.TabPage();
            this.gameTypesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddGameType = new System.Windows.Forms.Button();
            this.btnCloseManagement = new System.Windows.Forms.Button();
            this.mainContainer.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.headerControlsPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.managementOverlay.SuspendLayout();
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

            // mainContainer - CHANGED: Removed management panel from rows
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

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = textColor;
            this.lblTitle.Location = new System.Drawing.Point(0, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(263, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Game Rates Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // filterCombo
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

            // btnAddNew
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

            // btnManage - CHANGED: Text updated
            this.btnManage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnManage.BackColor = primaryColor;
            this.btnManage.FlatAppearance.BorderSize = 0;
            this.btnManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManage.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnManage.ForeColor = Color.White;
            this.btnManage.Location = new System.Drawing.Point(1590, 0);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(180, 35);
            this.btnManage.TabIndex = 3;
            this.btnManage.Text = "Manage Courts/Types";
            this.btnManage.UseVisualStyleBackColor = false;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);

            // contentPanel - CHANGED: Now fills remaining space
            this.contentPanel.BackColor = Color.White;
            this.contentPanel.Controls.Add(this.ratesFlowPanel);
            this.contentPanel.Controls.Add(this.managementOverlay);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(20, 115);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20);
            this.contentPanel.Size = new System.Drawing.Size(1880, 945);
            this.contentPanel.TabIndex = 1;

            // ratesFlowPanel - CHANGED: Fills the panel
            this.ratesFlowPanel.AutoScroll = true;
            this.ratesFlowPanel.BackColor = Color.White;
            this.ratesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratesFlowPanel.Location = new System.Drawing.Point(20, 20);
            this.ratesFlowPanel.Name = "ratesFlowPanel";
            this.ratesFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.ratesFlowPanel.Size = new System.Drawing.Size(1840, 905);
            this.ratesFlowPanel.TabIndex = 0;

            // managementOverlay - NEW: Overlay panel
            this.managementOverlay.BackColor = Color.FromArgb(250, 250, 252);
            this.managementOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.managementOverlay.Controls.Add(this.managementTabs);
            this.managementOverlay.Controls.Add(this.btnCloseManagement);
            this.managementOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementOverlay.Location = new System.Drawing.Point(20, 20);
            this.managementOverlay.Name = "managementOverlay";
            this.managementOverlay.Padding = new System.Windows.Forms.Padding(20, 20, 20, 60);
            this.managementOverlay.Size = new System.Drawing.Size(1840, 905);
            this.managementOverlay.TabIndex = 1;
            this.managementOverlay.Visible = false;

            // managementTabs
            this.managementTabs.Controls.Add(this.tabCourts);
            this.managementTabs.Controls.Add(this.tabGameTypes);
            this.managementTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementTabs.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.managementTabs.Location = new System.Drawing.Point(20, 20);
            this.managementTabs.Name = "managementTabs";
            this.managementTabs.SelectedIndex = 0;
            this.managementTabs.Size = new System.Drawing.Size(1798, 823);
            this.managementTabs.TabIndex = 0;

            // tabCourts
            this.tabCourts.BackColor = Color.FromArgb(250, 250, 252);
            this.tabCourts.Controls.Add(this.courtsFlowPanel);
            this.tabCourts.Controls.Add(this.btnAddCourt);
            this.tabCourts.Location = new System.Drawing.Point(4, 29);
            this.tabCourts.Name = "tabCourts";
            this.tabCourts.Padding = new System.Windows.Forms.Padding(15);
            this.tabCourts.Size = new System.Drawing.Size(1790, 790);
            this.tabCourts.TabIndex = 0;
            this.tabCourts.Text = "Courts";

            // courtsFlowPanel - CHANGED: Adjusted for new layout
            this.courtsFlowPanel.AutoScroll = true;
            this.courtsFlowPanel.BackColor = Color.FromArgb(250, 250, 252);
            this.courtsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.courtsFlowPanel.Location = new System.Drawing.Point(15, 15);
            this.courtsFlowPanel.Name = "courtsFlowPanel";
            this.courtsFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.courtsFlowPanel.Size = new System.Drawing.Size(1760, 689);
            this.courtsFlowPanel.TabIndex = 0;

            // btnAddCourt - CHANGED: Position updated
            this.btnAddCourt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddCourt.BackColor = successColor;
            this.btnAddCourt.FlatAppearance.BorderSize = 0;
            this.btnAddCourt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCourt.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddCourt.ForeColor = Color.White;
            this.btnAddCourt.Location = new System.Drawing.Point(815, 730);
            this.btnAddCourt.Name = "btnAddCourt";
            this.btnAddCourt.Size = new System.Drawing.Size(160, 40);
            this.btnAddCourt.TabIndex = 1;
            this.btnAddCourt.Text = "+ Add Court";
            this.btnAddCourt.UseVisualStyleBackColor = false;
            this.btnAddCourt.Click += new System.EventHandler(this.btnAddCourt_Click);

            // tabGameTypes
            this.tabGameTypes.BackColor = Color.FromArgb(250, 250, 252);
            this.tabGameTypes.Controls.Add(this.gameTypesFlowPanel);
            this.tabGameTypes.Controls.Add(this.btnAddGameType);
            this.tabGameTypes.Location = new System.Drawing.Point(4, 29);
            this.tabGameTypes.Name = "tabGameTypes";
            this.tabGameTypes.Padding = new System.Windows.Forms.Padding(15);
            this.tabGameTypes.Size = new System.Drawing.Size(1790, 790);
            this.tabGameTypes.TabIndex = 1;
            this.tabGameTypes.Text = "Game Types";

            // gameTypesFlowPanel - CHANGED: Adjusted for new layout
            this.gameTypesFlowPanel.AutoScroll = true;
            this.gameTypesFlowPanel.BackColor = Color.FromArgb(250, 250, 252);
            this.gameTypesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameTypesFlowPanel.Location = new System.Drawing.Point(15, 15);
            this.gameTypesFlowPanel.Name = "gameTypesFlowPanel";
            this.gameTypesFlowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.gameTypesFlowPanel.Size = new System.Drawing.Size(1760, 689);
            this.gameTypesFlowPanel.TabIndex = 1;

            // btnAddGameType - CHANGED: Position updated
            this.btnAddGameType.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddGameType.BackColor = successColor;
            this.btnAddGameType.FlatAppearance.BorderSize = 0;
            this.btnAddGameType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGameType.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddGameType.ForeColor = Color.White;
            this.btnAddGameType.Location = new System.Drawing.Point(805, 730);
            this.btnAddGameType.Name = "btnAddGameType";
            this.btnAddGameType.Size = new System.Drawing.Size(180, 40);
            this.btnAddGameType.TabIndex = 2;
            this.btnAddGameType.Text = "+ Add Game Type";
            this.btnAddGameType.UseVisualStyleBackColor = false;
            this.btnAddGameType.Click += new System.EventHandler(this.btnAddGameType_Click);

            // btnCloseManagement - NEW: Close button for overlay
            this.btnCloseManagement.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCloseManagement.BackColor = dangerColor;
            this.btnCloseManagement.FlatAppearance.BorderSize = 0;
            this.btnCloseManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseManagement.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnCloseManagement.ForeColor = Color.White;
            this.btnCloseManagement.Location = new System.Drawing.Point(880, 860);
            this.btnCloseManagement.Name = "btnCloseManagement";
            this.btnCloseManagement.Size = new System.Drawing.Size(160, 40);
            this.btnCloseManagement.TabIndex = 4;
            this.btnCloseManagement.Text = "Close";
            this.btnCloseManagement.UseVisualStyleBackColor = false;
            this.btnCloseManagement.Click += new System.EventHandler(this.btnCloseManagement_Click);

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
            this.managementOverlay.ResumeLayout(false);
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
        private System.Windows.Forms.Panel managementOverlay;
        private System.Windows.Forms.TabControl managementTabs;
        private System.Windows.Forms.TabPage tabCourts;
        private System.Windows.Forms.FlowLayoutPanel courtsFlowPanel;
        private System.Windows.Forms.Button btnAddCourt;
        private System.Windows.Forms.TabPage tabGameTypes;
        private System.Windows.Forms.FlowLayoutPanel gameTypesFlowPanel;
        private System.Windows.Forms.Button btnAddGameType;
        private System.Windows.Forms.Button btnCloseManagement;
    }
}