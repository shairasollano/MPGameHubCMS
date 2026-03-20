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

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern System.IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private void InitializeComponent()
        {
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerControlsPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.filterCombo = new System.Windows.Forms.ComboBox();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnManage = new System.Windows.Forms.Button();
            this.statsPanel = new System.Windows.Forms.Panel();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.ratesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.managementOverlay = new System.Windows.Forms.Panel();
            this.managementTabs = new System.Windows.Forms.TabControl();
            this.tabCourts = new System.Windows.Forms.TabPage();
            this.courtsPanel = new System.Windows.Forms.Panel();
            this.courtsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCourt = new System.Windows.Forms.Button();
            this.tabGameTypes = new System.Windows.Forms.TabPage();
            this.gameTypesPanel = new System.Windows.Forms.Panel();
            this.gameTypesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddGameType = new System.Windows.Forms.Button();
            this.btnCloseManagement = new System.Windows.Forms.Button();

            // Stats panel controls
            this.statsCard1 = new System.Windows.Forms.Panel();
            this.statsIcon1 = new System.Windows.Forms.Label();
            this.lblStatsTitle1 = new System.Windows.Forms.Label();
            this.lblStatsValue1 = new System.Windows.Forms.Label();
            this.lblStatsSub1 = new System.Windows.Forms.Label();

            this.statsCard2 = new System.Windows.Forms.Panel();
            this.statsIcon2 = new System.Windows.Forms.Label();
            this.lblStatsTitle2 = new System.Windows.Forms.Label();
            this.lblStatsValue2 = new System.Windows.Forms.Label();
            this.lblStatsSub2 = new System.Windows.Forms.Label();

            this.statsCard3 = new System.Windows.Forms.Panel();
            this.statsIcon3 = new System.Windows.Forms.Label();
            this.lblStatsTitle3 = new System.Windows.Forms.Label();
            this.lblStatsValue3 = new System.Windows.Forms.Label();
            this.lblStatsSub3 = new System.Windows.Forms.Label();

            this.statsCard4 = new System.Windows.Forms.Panel();
            this.statsIcon4 = new System.Windows.Forms.Label();
            this.lblStatsTitle4 = new System.Windows.Forms.Label();
            this.lblStatsValue4 = new System.Windows.Forms.Label();
            this.lblStatsSub4 = new System.Windows.Forms.Label();

            this.mainContainer.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.headerControlsPanel.SuspendLayout();
            this.statsPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.managementOverlay.SuspendLayout();
            this.managementTabs.SuspendLayout();
            this.tabCourts.SuspendLayout();
            this.courtsPanel.SuspendLayout();
            this.tabGameTypes.SuspendLayout();
            this.gameTypesPanel.SuspendLayout();
            this.SuspendLayout();

            // Modern color scheme
            Color bgColor = Color.FromArgb(248, 250, 252);
            Color primaryColor = Color.FromArgb(79, 70, 229); // Indigo
            Color successColor = Color.FromArgb(16, 185, 129); // Emerald
            Color dangerColor = Color.FromArgb(239, 68, 68); // Red
            Color warningColor = Color.FromArgb(245, 158, 11); // Amber
            Color infoColor = Color.FromArgb(59, 130, 246); // Blue
            Color textColor = Color.FromArgb(17, 24, 39);
            Color cardBgColor = Color.White;
            Color borderColor = Color.FromArgb(229, 231, 235);

            // mainContainer
            this.mainContainer.BackColor = bgColor;
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.headerPanel, 0, 0);
            this.mainContainer.Controls.Add(this.statsPanel, 0, 1);
            this.mainContainer.Controls.Add(this.contentPanel, 0, 2);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 0);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(24);
            this.mainContainer.RowCount = 3;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Size = new System.Drawing.Size(1920, 1080);
            this.mainContainer.TabIndex = 0;

            // headerPanel
            this.headerPanel.BackColor = Color.White;
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
            this.headerControlsPanel.BackColor = Color.White;
            this.headerControlsPanel.Controls.Add(this.lblTitle);
            this.headerControlsPanel.Controls.Add(this.filterCombo);
            this.headerControlsPanel.Controls.Add(this.btnAddNew);
            this.headerControlsPanel.Controls.Add(this.btnManage);
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
            this.lblTitle.Size = new System.Drawing.Size(312, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Game Rates Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // filterCombo
            this.filterCombo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.filterCombo.BackColor = Color.White;
            this.filterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterCombo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.filterCombo.ForeColor = textColor;
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Items.AddRange(new object[] {
            "All Rates",
            "Active Only",
            "Inactive Only"});
            this.filterCombo.Location = new System.Drawing.Point(1150, 4);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(180, 25);
            this.filterCombo.TabIndex = 1;
            this.filterCombo.SelectedIndexChanged += new System.EventHandler(this.filterCombo_SelectedIndexChanged);

            // btnAddNew
            this.btnAddNew.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddNew.BackColor = successColor;
            this.btnAddNew.FlatAppearance.BorderSize = 0;
            this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNew.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddNew.ForeColor = Color.White;
            this.btnAddNew.Location = new System.Drawing.Point(1340, 0);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(130, 32);
            this.btnAddNew.TabIndex = 2;
            this.btnAddNew.Text = "+ New Rate";
            this.btnAddNew.UseVisualStyleBackColor = false;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            this.btnAddNew.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.btnAddNew.Width, this.btnAddNew.Height, 8, 8));

            // btnManage
            this.btnManage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnManage.BackColor = primaryColor;
            this.btnManage.FlatAppearance.BorderSize = 0;
            this.btnManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManage.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnManage.ForeColor = Color.White;
            this.btnManage.Location = new System.Drawing.Point(1480, 0);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(170, 32);
            this.btnManage.TabIndex = 3;
            this.btnManage.Text = "Manage Courts/Types";
            this.btnManage.UseVisualStyleBackColor = false;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            this.btnManage.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.btnManage.Width, this.btnManage.Height, 8, 8));

            // statsPanel
            this.statsPanel.BackColor = Color.Transparent;
            this.statsPanel.Controls.Add(this.statsCard1);
            this.statsPanel.Controls.Add(this.statsCard2);
            this.statsPanel.Controls.Add(this.statsCard3);
            this.statsPanel.Controls.Add(this.statsCard4);
            this.statsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsPanel.Location = new System.Drawing.Point(24, 104);
            this.statsPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Size = new System.Drawing.Size(1872, 104);
            this.statsPanel.TabIndex = 2;

            // Stats Card 1 - Total Rates
            this.statsCard1.BackColor = Color.White;
            this.statsCard1.Location = new System.Drawing.Point(0, 0);
            this.statsCard1.Name = "statsCard1";
            this.statsCard1.Size = new System.Drawing.Size(280, 104);
            this.statsCard1.TabIndex = 0;

            this.statsIcon1.AutoSize = false;
            this.statsIcon1.BackColor = Color.FromArgb(239, 246, 255);
            this.statsIcon1.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.statsIcon1.ForeColor = primaryColor;
            this.statsIcon1.Location = new System.Drawing.Point(16, 24);
            this.statsIcon1.Name = "statsIcon1";
            this.statsIcon1.Size = new System.Drawing.Size(56, 56);
            this.statsIcon1.TabIndex = 0;
            this.statsIcon1.Text = "📊";
            this.statsIcon1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblStatsTitle1.AutoSize = true;
            this.lblStatsTitle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblStatsTitle1.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblStatsTitle1.Location = new System.Drawing.Point(88, 24);
            this.lblStatsTitle1.Name = "lblStatsTitle1";
            this.lblStatsTitle1.Size = new System.Drawing.Size(72, 19);
            this.lblStatsTitle1.TabIndex = 1;
            this.lblStatsTitle1.Text = "Total Rates";

            this.lblStatsValue1.AutoSize = true;
            this.lblStatsValue1.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatsValue1.ForeColor = textColor;
            this.lblStatsValue1.Location = new System.Drawing.Point(88, 44);
            this.lblStatsValue1.Name = "lblStatsValue1";
            this.lblStatsValue1.Size = new System.Drawing.Size(42, 50);
            this.lblStatsValue1.TabIndex = 2;
            this.lblStatsValue1.Text = "0";

            this.lblStatsSub1.AutoSize = true;
            this.lblStatsSub1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblStatsSub1.ForeColor = Color.FromArgb(156, 163, 175);
            this.lblStatsSub1.Location = new System.Drawing.Point(160, 70);
            this.lblStatsSub1.Name = "lblStatsSub1";
            this.lblStatsSub1.Size = new System.Drawing.Size(63, 15);
            this.lblStatsSub1.TabIndex = 3;
            this.lblStatsSub1.Text = "total rates";

            this.statsCard1.Controls.Add(this.statsIcon1);
            this.statsCard1.Controls.Add(this.lblStatsTitle1);
            this.statsCard1.Controls.Add(this.lblStatsValue1);
            this.statsCard1.Controls.Add(this.lblStatsSub1);

            // Stats Card 2 - Active
            this.statsCard2.BackColor = Color.White;
            this.statsCard2.Location = new System.Drawing.Point(300, 0);
            this.statsCard2.Name = "statsCard2";
            this.statsCard2.Size = new System.Drawing.Size(280, 104);
            this.statsCard2.TabIndex = 1;

            this.statsIcon2.AutoSize = false;
            this.statsIcon2.BackColor = Color.FromArgb(236, 253, 245);
            this.statsIcon2.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.statsIcon2.ForeColor = successColor;
            this.statsIcon2.Location = new System.Drawing.Point(16, 24);
            this.statsIcon2.Name = "statsIcon2";
            this.statsIcon2.Size = new System.Drawing.Size(56, 56);
            this.statsIcon2.TabIndex = 0;
            this.statsIcon2.Text = "✅";
            this.statsIcon2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblStatsTitle2.AutoSize = true;
            this.lblStatsTitle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblStatsTitle2.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblStatsTitle2.Location = new System.Drawing.Point(88, 24);
            this.lblStatsTitle2.Name = "lblStatsTitle2";
            this.lblStatsTitle2.Size = new System.Drawing.Size(46, 19);
            this.lblStatsTitle2.TabIndex = 1;
            this.lblStatsTitle2.Text = "Active";

            this.lblStatsValue2.AutoSize = true;
            this.lblStatsValue2.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatsValue2.ForeColor = textColor;
            this.lblStatsValue2.Location = new System.Drawing.Point(88, 44);
            this.lblStatsValue2.Name = "lblStatsValue2";
            this.lblStatsValue2.Size = new System.Drawing.Size(42, 50);
            this.lblStatsValue2.TabIndex = 2;
            this.lblStatsValue2.Text = "0";

            this.lblStatsSub2.AutoSize = true;
            this.lblStatsSub2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblStatsSub2.ForeColor = Color.FromArgb(156, 163, 175);
            this.lblStatsSub2.Location = new System.Drawing.Point(160, 70);
            this.lblStatsSub2.Name = "lblStatsSub2";
            this.lblStatsSub2.Size = new System.Drawing.Size(40, 15);
            this.lblStatsSub2.TabIndex = 3;
            this.lblStatsSub2.Text = "active";

            this.statsCard2.Controls.Add(this.statsIcon2);
            this.statsCard2.Controls.Add(this.lblStatsTitle2);
            this.statsCard2.Controls.Add(this.lblStatsValue2);
            this.statsCard2.Controls.Add(this.lblStatsSub2);

            // Stats Card 3 - Inactive
            this.statsCard3.BackColor = Color.White;
            this.statsCard3.Location = new System.Drawing.Point(600, 0);
            this.statsCard3.Name = "statsCard3";
            this.statsCard3.Size = new System.Drawing.Size(280, 104);
            this.statsCard3.TabIndex = 2;

            this.statsIcon3.AutoSize = false;
            this.statsIcon3.BackColor = Color.FromArgb(254, 242, 242);
            this.statsIcon3.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.statsIcon3.ForeColor = dangerColor;
            this.statsIcon3.Location = new System.Drawing.Point(16, 24);
            this.statsIcon3.Name = "statsIcon3";
            this.statsIcon3.Size = new System.Drawing.Size(56, 56);
            this.statsIcon3.TabIndex = 0;
            this.statsIcon3.Text = "❌";
            this.statsIcon3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblStatsTitle3.AutoSize = true;
            this.lblStatsTitle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblStatsTitle3.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblStatsTitle3.Location = new System.Drawing.Point(88, 24);
            this.lblStatsTitle3.Name = "lblStatsTitle3";
            this.lblStatsTitle3.Size = new System.Drawing.Size(57, 19);
            this.lblStatsTitle3.TabIndex = 1;
            this.lblStatsTitle3.Text = "Inactive";

            this.lblStatsValue3.AutoSize = true;
            this.lblStatsValue3.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatsValue3.ForeColor = textColor;
            this.lblStatsValue3.Location = new System.Drawing.Point(88, 44);
            this.lblStatsValue3.Name = "lblStatsValue3";
            this.lblStatsValue3.Size = new System.Drawing.Size(42, 50);
            this.lblStatsValue3.TabIndex = 2;
            this.lblStatsValue3.Text = "0";

            this.lblStatsSub3.AutoSize = true;
            this.lblStatsSub3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblStatsSub3.ForeColor = Color.FromArgb(156, 163, 175);
            this.lblStatsSub3.Location = new System.Drawing.Point(160, 70);
            this.lblStatsSub3.Name = "lblStatsSub3";
            this.lblStatsSub3.Size = new System.Drawing.Size(47, 15);
            this.lblStatsSub3.TabIndex = 3;
            this.lblStatsSub3.Text = "inactive";

            this.statsCard3.Controls.Add(this.statsIcon3);
            this.statsCard3.Controls.Add(this.lblStatsTitle3);
            this.statsCard3.Controls.Add(this.lblStatsValue3);
            this.statsCard3.Controls.Add(this.lblStatsSub3);

            // Stats Card 4 - Average Rate
            this.statsCard4.BackColor = Color.White;
            this.statsCard4.Location = new System.Drawing.Point(900, 0);
            this.statsCard4.Name = "statsCard4";
            this.statsCard4.Size = new System.Drawing.Size(280, 104);
            this.statsCard4.TabIndex = 3;

            this.statsIcon4.AutoSize = false;
            this.statsIcon4.BackColor = Color.FromArgb(239, 246, 255);
            this.statsIcon4.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.statsIcon4.ForeColor = primaryColor;
            this.statsIcon4.Location = new System.Drawing.Point(16, 24);
            this.statsIcon4.Name = "statsIcon4";
            this.statsIcon4.Size = new System.Drawing.Size(56, 56);
            this.statsIcon4.TabIndex = 0;
            this.statsIcon4.Text = "💰";
            this.statsIcon4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblStatsTitle4.AutoSize = true;
            this.lblStatsTitle4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblStatsTitle4.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblStatsTitle4.Location = new System.Drawing.Point(88, 24);
            this.lblStatsTitle4.Name = "lblStatsTitle4";
            this.lblStatsTitle4.Size = new System.Drawing.Size(88, 19);
            this.lblStatsTitle4.TabIndex = 1;
            this.lblStatsTitle4.Text = "Average Rate";

            this.lblStatsValue4.AutoSize = true;
            this.lblStatsValue4.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatsValue4.ForeColor = textColor;
            this.lblStatsValue4.Location = new System.Drawing.Point(88, 44);
            this.lblStatsValue4.Name = "lblStatsValue4";
            this.lblStatsValue4.Size = new System.Drawing.Size(98, 50);
            this.lblStatsValue4.TabIndex = 2;
            this.lblStatsValue4.Text = "₱650";

            this.lblStatsSub4.AutoSize = true;
            this.lblStatsSub4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblStatsSub4.ForeColor = Color.FromArgb(156, 163, 175);
            this.lblStatsSub4.Location = new System.Drawing.Point(190, 70);
            this.lblStatsSub4.Name = "lblStatsSub4";
            this.lblStatsSub4.Size = new System.Drawing.Size(50, 15);
            this.lblStatsSub4.TabIndex = 3;
            this.lblStatsSub4.Text = "avg / hr";

            this.statsCard4.Controls.Add(this.statsIcon4);
            this.statsCard4.Controls.Add(this.lblStatsTitle4);
            this.statsCard4.Controls.Add(this.lblStatsValue4);
            this.statsCard4.Controls.Add(this.lblStatsSub4);

            // contentPanel
            this.contentPanel.BackColor = Color.Transparent;
            this.contentPanel.Controls.Add(this.ratesFlowPanel);
            this.contentPanel.Controls.Add(this.managementOverlay);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(24, 224);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0);
            this.contentPanel.Size = new System.Drawing.Size(1872, 832);
            this.contentPanel.TabIndex = 1;

            // ratesFlowPanel
            this.ratesFlowPanel.AutoScroll = true;
            this.ratesFlowPanel.BackColor = Color.Transparent;
            this.ratesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratesFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.ratesFlowPanel.Name = "ratesFlowPanel";
            this.ratesFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.ratesFlowPanel.Size = new System.Drawing.Size(1872, 832);
            this.ratesFlowPanel.TabIndex = 0;

            // managementOverlay
            this.managementOverlay.BackColor = Color.FromArgb(255, 255, 255);
            this.managementOverlay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.managementOverlay.Controls.Add(this.managementTabs);
            this.managementOverlay.Controls.Add(this.btnCloseManagement);
            this.managementOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementOverlay.Location = new System.Drawing.Point(0, 0);
            this.managementOverlay.Name = "managementOverlay";
            this.managementOverlay.Padding = new System.Windows.Forms.Padding(24, 24, 24, 64);
            this.managementOverlay.Size = new System.Drawing.Size(1872, 832);
            this.managementOverlay.TabIndex = 1;
            this.managementOverlay.Visible = false;

            // managementTabs - REMOVED DrawItem code
            this.managementTabs.Controls.Add(this.tabCourts);
            this.managementTabs.Controls.Add(this.tabGameTypes);
            this.managementTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managementTabs.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.managementTabs.Location = new System.Drawing.Point(24, 24);
            this.managementTabs.Name = "managementTabs";
            this.managementTabs.SelectedIndex = 0;
            this.managementTabs.Size = new System.Drawing.Size(1824, 744);
            this.managementTabs.TabIndex = 0;

            // tabCourts
            this.tabCourts.BackColor = Color.White;
            this.tabCourts.Controls.Add(this.courtsPanel);
            this.tabCourts.Location = new System.Drawing.Point(4, 29);
            this.tabCourts.Name = "tabCourts";
            this.tabCourts.Padding = new System.Windows.Forms.Padding(20);
            this.tabCourts.Size = new System.Drawing.Size(1816, 711);
            this.tabCourts.TabIndex = 0;
            this.tabCourts.Text = "Courts";

            // courtsPanel
            this.courtsPanel.BackColor = Color.White;
            this.courtsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.courtsPanel.Location = new System.Drawing.Point(20, 20);
            this.courtsPanel.Name = "courtsPanel";
            this.courtsPanel.Size = new System.Drawing.Size(1776, 671);
            this.courtsPanel.TabIndex = 0;

            // courtsFlowPanel
            this.courtsFlowPanel.AutoScroll = true;
            this.courtsFlowPanel.BackColor = Color.White;
            this.courtsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.courtsFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.courtsFlowPanel.Name = "courtsFlowPanel";
            this.courtsFlowPanel.Padding = new System.Windows.Forms.Padding(12);
            this.courtsFlowPanel.Size = new System.Drawing.Size(1776, 611);
            this.courtsFlowPanel.TabIndex = 0;

            // btnAddCourt
            this.btnAddCourt.BackColor = successColor;
            this.btnAddCourt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddCourt.FlatAppearance.BorderSize = 0;
            this.btnAddCourt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCourt.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddCourt.ForeColor = Color.White;
            this.btnAddCourt.Location = new System.Drawing.Point(0, 611);
            this.btnAddCourt.Name = "btnAddCourt";
            this.btnAddCourt.Size = new System.Drawing.Size(1776, 60);
            this.btnAddCourt.TabIndex = 1;
            this.btnAddCourt.Text = "+ Add New Court";
            this.btnAddCourt.UseVisualStyleBackColor = false;
            this.btnAddCourt.Click += new System.EventHandler(this.btnAddCourt_Click);
            this.btnAddCourt.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.btnAddCourt.Width, this.btnAddCourt.Height, 8, 8));

            // Add controls to courtsPanel
            this.courtsPanel.Controls.Add(this.courtsFlowPanel);
            this.courtsPanel.Controls.Add(this.btnAddCourt);

            // tabGameTypes
            this.tabGameTypes.BackColor = Color.White;
            this.tabGameTypes.Controls.Add(this.gameTypesPanel);
            this.tabGameTypes.Location = new System.Drawing.Point(4, 29);
            this.tabGameTypes.Name = "tabGameTypes";
            this.tabGameTypes.Padding = new System.Windows.Forms.Padding(20);
            this.tabGameTypes.Size = new System.Drawing.Size(1816, 711);
            this.tabGameTypes.TabIndex = 1;
            this.tabGameTypes.Text = "Game Types";

            // gameTypesPanel
            this.gameTypesPanel.BackColor = Color.White;
            this.gameTypesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameTypesPanel.Location = new System.Drawing.Point(20, 20);
            this.gameTypesPanel.Name = "gameTypesPanel";
            this.gameTypesPanel.Size = new System.Drawing.Size(1776, 671);
            this.gameTypesPanel.TabIndex = 0;

            // gameTypesFlowPanel
            this.gameTypesFlowPanel.AutoScroll = true;
            this.gameTypesFlowPanel.BackColor = Color.White;
            this.gameTypesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameTypesFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.gameTypesFlowPanel.Name = "gameTypesFlowPanel";
            this.gameTypesFlowPanel.Padding = new System.Windows.Forms.Padding(12);
            this.gameTypesFlowPanel.Size = new System.Drawing.Size(1776, 611);
            this.gameTypesFlowPanel.TabIndex = 0;

            // btnAddGameType
            this.btnAddGameType.BackColor = successColor;
            this.btnAddGameType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddGameType.FlatAppearance.BorderSize = 0;
            this.btnAddGameType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGameType.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddGameType.ForeColor = Color.White;
            this.btnAddGameType.Location = new System.Drawing.Point(0, 611);
            this.btnAddGameType.Name = "btnAddGameType";
            this.btnAddGameType.Size = new System.Drawing.Size(1776, 60);
            this.btnAddGameType.TabIndex = 1;
            this.btnAddGameType.Text = "+ Add New Game Type";
            this.btnAddGameType.UseVisualStyleBackColor = false;
            this.btnAddGameType.Click += new System.EventHandler(this.btnAddGameType_Click);
            this.btnAddGameType.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.btnAddGameType.Width, this.btnAddGameType.Height, 8, 8));

            // Add controls to gameTypesPanel
            this.gameTypesPanel.Controls.Add(this.gameTypesFlowPanel);
            this.gameTypesPanel.Controls.Add(this.btnAddGameType);

            // btnCloseManagement
            this.btnCloseManagement.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCloseManagement.BackColor = dangerColor;
            this.btnCloseManagement.FlatAppearance.BorderSize = 0;
            this.btnCloseManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseManagement.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnCloseManagement.ForeColor = Color.White;
            this.btnCloseManagement.Location = new System.Drawing.Point(876, 776);
            this.btnCloseManagement.Name = "btnCloseManagement";
            this.btnCloseManagement.Size = new System.Drawing.Size(120, 40);
            this.btnCloseManagement.TabIndex = 4;
            this.btnCloseManagement.Text = "Close";
            this.btnCloseManagement.UseVisualStyleBackColor = false;
            this.btnCloseManagement.Click += new System.EventHandler(this.btnCloseManagement_Click);
            this.btnCloseManagement.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.btnCloseManagement.Width, this.btnCloseManagement.Height, 8, 8));

            // GameRates
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.mainContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "GameRates";
            this.Size = new System.Drawing.Size(1920, 1080);
            this.mainContainer.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerControlsPanel.ResumeLayout(false);
            this.headerControlsPanel.PerformLayout();
            this.statsPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            this.managementOverlay.ResumeLayout(false);
            this.managementTabs.ResumeLayout(false);
            this.tabCourts.ResumeLayout(false);
            this.courtsPanel.ResumeLayout(false);
            this.tabGameTypes.ResumeLayout(false);
            this.gameTypesPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel headerControlsPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox filterCombo;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Panel statsPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.FlowLayoutPanel ratesFlowPanel;
        private System.Windows.Forms.Panel managementOverlay;
        private System.Windows.Forms.TabControl managementTabs;
        private System.Windows.Forms.TabPage tabCourts;
        private System.Windows.Forms.Panel courtsPanel;
        private System.Windows.Forms.FlowLayoutPanel courtsFlowPanel;
        private System.Windows.Forms.Button btnAddCourt;
        private System.Windows.Forms.TabPage tabGameTypes;
        private System.Windows.Forms.Panel gameTypesPanel;
        private System.Windows.Forms.FlowLayoutPanel gameTypesFlowPanel;
        private System.Windows.Forms.Button btnAddGameType;
        private System.Windows.Forms.Button btnCloseManagement;

        // Stats card controls
        private System.Windows.Forms.Panel statsCard1;
        private System.Windows.Forms.Label statsIcon1;
        private System.Windows.Forms.Label lblStatsTitle1;
        private System.Windows.Forms.Label lblStatsValue1;
        private System.Windows.Forms.Label lblStatsSub1;

        private System.Windows.Forms.Panel statsCard2;
        private System.Windows.Forms.Label statsIcon2;
        private System.Windows.Forms.Label lblStatsTitle2;
        private System.Windows.Forms.Label lblStatsValue2;
        private System.Windows.Forms.Label lblStatsSub2;

        private System.Windows.Forms.Panel statsCard3;
        private System.Windows.Forms.Label statsIcon3;
        private System.Windows.Forms.Label lblStatsTitle3;
        private System.Windows.Forms.Label lblStatsValue3;
        private System.Windows.Forms.Label lblStatsSub3;

        private System.Windows.Forms.Panel statsCard4;
        private System.Windows.Forms.Label statsIcon4;
        private System.Windows.Forms.Label lblStatsTitle4;
        private System.Windows.Forms.Label lblStatsValue4;
        private System.Windows.Forms.Label lblStatsSub4;
    }
}