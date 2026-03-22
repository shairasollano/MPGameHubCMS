namespace cms
{
    partial class SALES
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
            this.filterPanel = new System.Windows.Forms.Panel();
            this.lblFilterInfo = new System.Windows.Forms.Label();
            this.btnLoadSales = new System.Windows.Forms.Button();
            this.dtpYear = new System.Windows.Forms.DateTimePicker();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.cmbWeek = new System.Windows.Forms.ComboBox();
            this.cmbFilterType = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblWeek = new System.Windows.Forms.Label();
            this.lblFilterType = new System.Windows.Forms.Label();
            this.dgvSales = new System.Windows.Forms.DataGridView();
            this.filterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            this.SuspendLayout();
            // 
            // filterPanel
            // 
            this.filterPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.filterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filterPanel.Controls.Add(this.lblFilterInfo);
            this.filterPanel.Controls.Add(this.btnLoadSales);
            this.filterPanel.Controls.Add(this.dtpYear);
            this.filterPanel.Controls.Add(this.cmbMonth);
            this.filterPanel.Controls.Add(this.cmbWeek);
            this.filterPanel.Controls.Add(this.cmbFilterType);
            this.filterPanel.Controls.Add(this.lblYear);
            this.filterPanel.Controls.Add(this.lblMonth);
            this.filterPanel.Controls.Add(this.lblWeek);
            this.filterPanel.Controls.Add(this.lblFilterType);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(0, 0);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Padding = new System.Windows.Forms.Padding(10);
            this.filterPanel.Size = new System.Drawing.Size(1684, 100);
            this.filterPanel.TabIndex = 0;
            // 
            // lblFilterInfo
            // 
            this.lblFilterInfo.AutoSize = true;
            this.lblFilterInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblFilterInfo.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblFilterInfo.Location = new System.Drawing.Point(15, 55);
            this.lblFilterInfo.Name = "lblFilterInfo";
            this.lblFilterInfo.Size = new System.Drawing.Size(213, 15);
            this.lblFilterInfo.TabIndex = 9;
            this.lblFilterInfo.Text = "Select filter criteria and click Load Sales";
            // 
            // btnLoadSales
            // 
            this.btnLoadSales.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnLoadSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadSales.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoadSales.ForeColor = System.Drawing.Color.White;
            this.btnLoadSales.Location = new System.Drawing.Point(605, 8);
            this.btnLoadSales.Name = "btnLoadSales";
            this.btnLoadSales.Size = new System.Drawing.Size(110, 38);
            this.btnLoadSales.TabIndex = 8;
            this.btnLoadSales.Text = "Load Sales";
            this.btnLoadSales.UseVisualStyleBackColor = false;
            this.btnLoadSales.Click += new System.EventHandler(this.BtnLoadSales_Click);
            // 
            // dtpYear
            // 
            this.dtpYear.CustomFormat = "yyyy";
            this.dtpYear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpYear.Location = new System.Drawing.Point(485, 12);
            this.dtpYear.Name = "dtpYear";
            this.dtpYear.ShowUpDown = true;
            this.dtpYear.Size = new System.Drawing.Size(100, 25);
            this.dtpYear.TabIndex = 7;
            this.dtpYear.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbMonth.Location = new System.Drawing.Point(295, 12);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(120, 25);
            this.cmbMonth.TabIndex = 6;
            this.cmbMonth.Visible = false;
            // 
            // cmbWeek
            // 
            this.cmbWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeek.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbWeek.Location = new System.Drawing.Point(295, 12);
            this.cmbWeek.Name = "cmbWeek";
            this.cmbWeek.Size = new System.Drawing.Size(120, 25);
            this.cmbWeek.TabIndex = 5;
            // 
            // cmbFilterType
            // 
            this.cmbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilterType.Items.AddRange(new object[] {
            "Select Week",
            "Select Month"});
            this.cmbFilterType.Location = new System.Drawing.Point(90, 12);
            this.cmbFilterType.Name = "cmbFilterType";
            this.cmbFilterType.Size = new System.Drawing.Size(130, 25);
            this.cmbFilterType.TabIndex = 4;
            this.cmbFilterType.SelectedIndexChanged += new System.EventHandler(this.CmbFilterType_SelectedIndexChanged);
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblYear.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblYear.Location = new System.Drawing.Point(435, 15);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(42, 19);
            this.lblYear.TabIndex = 3;
            this.lblYear.Text = "Year:";
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMonth.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblMonth.Location = new System.Drawing.Point(240, 15);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(52, 19);
            this.lblMonth.TabIndex = 2;
            this.lblMonth.Text = "Month:";
            this.lblMonth.Visible = false;
            // 
            // lblWeek
            // 
            this.lblWeek.AutoSize = true;
            this.lblWeek.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWeek.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblWeek.Location = new System.Drawing.Point(240, 15);
            this.lblWeek.Name = "lblWeek";
            this.lblWeek.Size = new System.Drawing.Size(48, 19);
            this.lblWeek.TabIndex = 1;
            this.lblWeek.Text = "Week:";
            // 
            // lblFilterType
            // 
            this.lblFilterType.AutoSize = true;
            this.lblFilterType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFilterType.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblFilterType.Location = new System.Drawing.Point(15, 15);
            this.lblFilterType.Name = "lblFilterType";
            this.lblFilterType.Size = new System.Drawing.Size(72, 19);
            this.lblFilterType.TabIndex = 0;
            this.lblFilterType.Text = "Filter By:";
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.BackgroundColor = System.Drawing.Color.White;
            this.dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.Location = new System.Drawing.Point(0, 100);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.ReadOnly = true;
            this.dgvSales.RowHeadersVisible = false;
            this.dgvSales.Size = new System.Drawing.Size(1684, 839);
            this.dgvSales.TabIndex = 1;
            // 
            // SALES
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvSales);
            this.Controls.Add(this.filterPanel);
            this.Name = "SALES";
            this.Size = new System.Drawing.Size(1684, 939);
            this.Load += new System.EventHandler(this.SALES_Load);
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.Label lblFilterInfo;
        private System.Windows.Forms.Button btnLoadSales;
        private System.Windows.Forms.DateTimePicker dtpYear;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.ComboBox cmbWeek;
        private System.Windows.Forms.ComboBox cmbFilterType;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblWeek;
        private System.Windows.Forms.Label lblFilterType;
    }
}