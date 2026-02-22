namespace cms
{
    partial class UserControl2
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
            this.monthlySale = new System.Windows.Forms.Panel();
            this.weeklySale = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.generateReport = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.refreshData = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.totalSalesTxt = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tablesInUseTxt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.totalSessionTxt = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.activeSessionTxt = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // monthlySale
            // 
            this.monthlySale.BackColor = System.Drawing.SystemColors.Control;
            this.monthlySale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.monthlySale.Location = new System.Drawing.Point(867, 344);
            this.monthlySale.Name = "monthlySale";
            this.monthlySale.Size = new System.Drawing.Size(692, 379);
            this.monthlySale.TabIndex = 14;
            this.monthlySale.Paint += new System.Windows.Forms.PaintEventHandler(this.monthlySale_Paint);
            // 
            // weeklySale
            // 
            this.weeklySale.BackColor = System.Drawing.SystemColors.Control;
            this.weeklySale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.weeklySale.Location = new System.Drawing.Point(93, 344);
            this.weeklySale.Name = "weeklySale";
            this.weeklySale.Size = new System.Drawing.Size(692, 379);
            this.weeklySale.TabIndex = 13;
            this.weeklySale.Click += new System.EventHandler(this.weeklySale_Click);
            this.weeklySale.Paint += new System.Windows.Forms.PaintEventHandler(this.weeklySale_Paint);
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.panel11.Controls.Add(this.generateReport);
            this.panel11.Location = new System.Drawing.Point(1295, 796);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(292, 47);
            this.panel11.TabIndex = 12;
            this.generateReport.Click += new System.EventHandler(this.generateReport_Click);
            // 
            // generateReport
            // 
            this.generateReport.AutoSize = true;
            this.generateReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.generateReport.Location = new System.Drawing.Point(28, 12);
            this.generateReport.Name = "generateReport";
            this.generateReport.Size = new System.Drawing.Size(221, 25);
            this.generateReport.TabIndex = 4;
            this.generateReport.Text = "GENERATE REPORT";
            this.generateReport.Click += new System.EventHandler(this.generateReport_Click);
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.panel12.Controls.Add(this.refreshData);
            this.panel12.Location = new System.Drawing.Point(1001, 795);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(283, 47);
            this.panel12.TabIndex = 11;
            this.panel12.Click += new System.EventHandler(this.panel12_Click);
            // 
            // refreshData
            // 
            this.refreshData.AutoSize = true;
            this.refreshData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.refreshData.Location = new System.Drawing.Point(46, 12);
            this.refreshData.Name = "refreshData";
            this.refreshData.Size = new System.Drawing.Size(176, 25);
            this.refreshData.TabIndex = 5;
            this.refreshData.Text = "REFRESH DATA";
            this.refreshData.Click += new System.EventHandler(this.panel12_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.totalSalesTxt);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Location = new System.Drawing.Point(1250, 39);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(344, 267);
            this.panel6.TabIndex = 8;
            // 
            // totalSalesTxt
            // 
            this.totalSalesTxt.AutoSize = true;
            this.totalSalesTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalSalesTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.totalSalesTxt.Location = new System.Drawing.Point(27, 103);
            this.totalSalesTxt.Name = "totalSalesTxt";
            this.totalSalesTxt.Size = new System.Drawing.Size(228, 91);
            this.totalSalesTxt.TabIndex = 12;
            this.totalSalesTxt.Text = "P560";
            this.totalSalesTxt.Click += new System.EventHandler(this.totalSalesTxt_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.label9.Location = new System.Drawing.Point(38, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 25);
            this.label9.TabIndex = 11;
            this.label9.Text = "TOTAL SALES";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(344, 66);
            this.panel7.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.tablesInUseTxt);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Location = new System.Drawing.Point(865, 39);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(344, 267);
            this.panel5.TabIndex = 9;
            // 
            // tablesInUseTxt
            // 
            this.tablesInUseTxt.AutoSize = true;
            this.tablesInUseTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablesInUseTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.tablesInUseTxt.Location = new System.Drawing.Point(27, 103);
            this.tablesInUseTxt.Name = "tablesInUseTxt";
            this.tablesInUseTxt.Size = new System.Drawing.Size(84, 91);
            this.tablesInUseTxt.TabIndex = 10;
            this.tablesInUseTxt.Text = "6";
            this.tablesInUseTxt.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(91)))), ((int)(((byte)(86)))));
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.label7.Location = new System.Drawing.Point(35, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(173, 25);
            this.label7.TabIndex = 9;
            this.label7.Text = "TABLES IN USE";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(91)))), ((int)(((byte)(86)))));
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(344, 66);
            this.panel8.TabIndex = 2;
            this.panel8.Paint += new System.Windows.Forms.PaintEventHandler(this.panel8_Paint);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.totalSessionTxt);
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Location = new System.Drawing.Point(478, 39);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(344, 267);
            this.panel4.TabIndex = 10;
            // 
            // totalSessionTxt
            // 
            this.totalSessionTxt.AutoSize = true;
            this.totalSessionTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalSessionTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.totalSessionTxt.Location = new System.Drawing.Point(20, 103);
            this.totalSessionTxt.Name = "totalSessionTxt";
            this.totalSessionTxt.Size = new System.Drawing.Size(129, 91);
            this.totalSessionTxt.TabIndex = 8;
            this.totalSessionTxt.Text = "17";
            this.totalSessionTxt.Click += new System.EventHandler(this.totalSessionTxt_Click);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(186)))), ((int)(((byte)(94)))));
            this.panel9.Controls.Add(this.label2);
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(344, 66);
            this.panel9.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.activeSessionTxt);
            this.panel3.Controls.Add(this.panel10);
            this.panel3.Location = new System.Drawing.Point(93, 39);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(344, 267);
            this.panel3.TabIndex = 7;
            // 
            // activeSessionTxt
            // 
            this.activeSessionTxt.AutoSize = true;
            this.activeSessionTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeSessionTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.activeSessionTxt.Location = new System.Drawing.Point(16, 103);
            this.activeSessionTxt.Name = "activeSessionTxt";
            this.activeSessionTxt.Size = new System.Drawing.Size(84, 91);
            this.activeSessionTxt.TabIndex = 6;
            this.activeSessionTxt.Text = "6";
            this.activeSessionTxt.Click += new System.EventHandler(this.activeSessionTxt_Click);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(231)))), ((int)(((byte)(182)))));
            this.panel10.Controls.Add(this.label1);
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(344, 66);
            this.panel10.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.label1.Location = new System.Drawing.Point(27, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "ACTIVE SESSIONS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(41)))), ((int)(((byte)(34)))));
            this.label2.Location = new System.Drawing.Point(31, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "TOTAL SESSIONS";
            // 
            // UserControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.monthlySale);
            this.Controls.Add(this.weeklySale);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Name = "UserControl2";
            this.Size = new System.Drawing.Size(1684, 939);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel monthlySale;
        private System.Windows.Forms.Panel weeklySale;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label generateReport;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label refreshData;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label tablesInUseTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label totalSessionTxt;
        private System.Windows.Forms.Label activeSessionTxt;
        private System.Windows.Forms.Label totalSalesTxt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
    }
}