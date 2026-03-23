using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class SALES : UserControl
    {
        // Database connection string
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";

        // Print-related variables
        private PrintDocument printDoc;
        private List<string> reportLines;
        private System.Drawing.Font reportFont;
        private System.Drawing.Font headerFont;
        private System.Drawing.Font titleFont;
        private int currentLineIndex = 0;
        private float yPos = 0;
        private float leftMargin = 40;
        private float topMargin = 40;
        private float lineHeight = 20;

        // Store current report data
        private DataTable currentSalesData;
        private string currentReportTitle = "";
        private string currentDateRange = "";

        // Store selected row for highlighting
        private DataGridViewRow selectedRow = null;
        private Color highlightColor = Color.FromArgb(228, 186, 94);
        private Color defaultColor = Color.White;

        // Current user info
        private string currentUser = "";
        private string currentUserRole = "";

        public SALES()
        {
            InitializeComponent();

            // Get current user from GlobalLogger
            if (!string.IsNullOrEmpty(GlobalLogger.CurrentUsername))
            {
                currentUser = GlobalLogger.CurrentUsername;
                currentUserRole = GlobalLogger.CurrentUserRole;
            }
            else
            {
                currentUser = "System";
                currentUserRole = "ADMIN";
            }

            // Style the control
            ApplyStyling();

            LoadWeeks();
            LoadMonths();
            if (cmbFilterType != null) cmbFilterType.SelectedIndex = 0;
            this.Size = new Size(1684, 939);

            // Initialize printing
            InitializePrinting();

            // Add cell click event for date highlighting
            if (dgvSales != null)
            {
                dgvSales.CellClick += DgvSales_CellClick;
                dgvSales.CellDoubleClick += DgvSales_CellDoubleClick;
                StyleDataGridView();
            }

            // Load initial data (current week)
            LoadCurrentWeekData();

            // Log that SALES module was opened
            try
            {
                GlobalLogger.LogInfo("Sales", $"User {currentUser} ({currentUserRole}) opened Sales module");
            }
            catch { }
        }

        private void ApplyStyling()
        {
            // Style the header panel
            if (filterPanel != null)
            {
                filterPanel.BackColor = Color.White;
                filterPanel.BorderStyle = BorderStyle.None;
                filterPanel.Padding = new Padding(20, 15, 20, 15);
            }

            // Style labels
            Color labelColor = Color.FromArgb(70, 70, 70);
            if (lblFilterType != null) lblFilterType.ForeColor = labelColor;
            if (lblWeek != null) lblWeek.ForeColor = labelColor;
            if (lblMonth != null) lblMonth.ForeColor = labelColor;
            if (lblYear != null) lblYear.ForeColor = labelColor;
            if (lblFilterInfo != null) lblFilterInfo.ForeColor = Color.FromArgb(120, 120, 120);

            // Style combo boxes
            foreach (var combo in new[] { cmbFilterType, cmbWeek, cmbMonth })
            {
                if (combo != null)
                {
                    combo.BackColor = Color.FromArgb(245, 245, 245);
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.Font = new Font("Segoe UI", 10F);
                    combo.ForeColor = Color.FromArgb(70, 70, 70);
                }
            }

            // Style date picker
            if (dtpYear != null)
            {
                dtpYear.BackColor = Color.FromArgb(245, 245, 245);
                dtpYear.Font = new Font("Segoe UI", 10F);
            }

            // Style buttons
            StyleButton(btnLoadSales, Color.FromArgb(40, 41, 34), Color.FromArgb(228, 186, 94));

            // Style generate report button (will be created programmatically)
        }

        private void StyleButton(Button btn, Color backColor, Color foreColor)
        {
            if (btn != null)
            {
                btn.BackColor = backColor;
                btn.ForeColor = foreColor;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                btn.Size = new Size(130, 38);

                btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(backColor, 0.2f);
                btn.MouseLeave += (s, e) => btn.BackColor = backColor;
            }
        }

        private void StyleDataGridView()
        {
            if (dgvSales != null)
            {
                dgvSales.BackgroundColor = Color.White;
                dgvSales.BorderStyle = BorderStyle.None;
                dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvSales.GridColor = Color.FromArgb(230, 230, 230);
                dgvSales.RowHeadersVisible = false;
                dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvSales.MultiSelect = false;
                dgvSales.DefaultCellStyle.SelectionBackColor = Color.FromArgb(228, 186, 94);
                dgvSales.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 41, 34);
                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Column headers styling
                dgvSales.EnableHeadersVisualStyles = false;
                dgvSales.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 41, 34);
                dgvSales.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(228, 186, 94);
                dgvSales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                dgvSales.ColumnHeadersHeight = 45;
                dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            }
        }

        private void LoadCurrentWeekData()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                int currentWeek = GetWeekNumberOfYear(DateTime.Now);
                LoadSalesByWeek(currentYear, currentWeek);

                try
                {
                    GlobalLogger.LogInfo("Sales", $"Loaded current week data (Week {currentWeek}, {currentYear})");
                }
                catch { }
            }
            catch (Exception ex)
            {
                try
                {
                    GlobalLogger.LogError("Sales", ex.Message, "Error loading current week data");
                }
                catch { }
                System.Diagnostics.Debug.WriteLine($"Error loading current week data: {ex.Message}");
            }
        }

        private int GetWeekNumberOfYear(DateTime date)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            System.Globalization.Calendar cal = ci.Calendar;
            CalendarWeekRule weekRule = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;

            return cal.GetWeekOfYear(date, weekRule, firstDayOfWeek);
        }

        // Method to set current user (called from Form1)
        public void SetCurrentUser(string username, string role)
        {
            currentUser = username;
            currentUserRole = role;

            try
            {
                GlobalLogger.LogInfo("Sales", $"User {username} ({role}) accessed Sales module");
            }
            catch { }
        }

        private void LoadWeeks()
        {
            if (cmbWeek != null)
            {
                cmbWeek.Items.Clear();
                for (int i = 1; i <= 52; i++)
                {
                    cmbWeek.Items.Add($"Week {i}");
                }
                int currentWeek = GetWeekNumberOfYear(DateTime.Now);
                if (cmbWeek.Items.Count >= currentWeek)
                    cmbWeek.SelectedIndex = currentWeek - 1;
            }
        }

        private void LoadMonths()
        {
            if (cmbMonth != null)
            {
                cmbMonth.Items.Clear();
                string[] months = { "January", "February", "March", "April", "May", "June",
                                    "July", "August", "September", "October", "November", "December" };
                cmbMonth.Items.AddRange(months);
                cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
            }
        }

        private void CmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterType != null && cmbWeek != null && cmbMonth != null && lblWeek != null && lblMonth != null)
            {
                string selected = cmbFilterType.SelectedItem?.ToString();
                if (selected == "Select Week")
                {
                    cmbWeek.Visible = true;
                    cmbMonth.Visible = false;
                    lblWeek.Visible = true;
                    lblMonth.Visible = false;
                    if (dtpYear != null) dtpYear.Visible = true;
                    if (lblYear != null) lblYear.Visible = true;

                    try
                    {
                        GlobalLogger.LogInfo("Sales", $"Filter changed to Week view by {currentUser}");
                    }
                    catch { }
                }
                else if (selected == "Select Month")
                {
                    cmbWeek.Visible = false;
                    cmbMonth.Visible = true;
                    lblWeek.Visible = false;
                    lblMonth.Visible = true;
                    if (dtpYear != null) dtpYear.Visible = true;
                    if (lblYear != null) lblYear.Visible = true;

                    try
                    {
                        GlobalLogger.LogInfo("Sales", $"Filter changed to Month view by {currentUser}");
                    }
                    catch { }
                }
            }
        }

        private void BtnLoadSales_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFilterType != null && dtpYear != null)
                {
                    int year = dtpYear.Value.Year;
                    string selectedFilter = cmbFilterType.SelectedItem?.ToString();

                    if (selectedFilter == "Select Week" && cmbWeek != null && cmbWeek.SelectedItem != null)
                    {
                        int weekNumber = cmbWeek.SelectedIndex + 1;
                        LoadSalesByWeek(year, weekNumber);

                        try
                        {
                            GlobalLogger.LogInfo("Sales", $"Loaded sales data for Week {weekNumber}, {year} by {currentUser}");
                        }
                        catch { }
                    }
                    else if (selectedFilter == "Select Month" && cmbMonth != null && cmbMonth.SelectedItem != null)
                    {
                        int month = cmbMonth.SelectedIndex + 1;
                        LoadSalesByMonth(year, month);

                        try
                        {
                            GlobalLogger.LogInfo("Sales", $"Loaded sales data for {new DateTime(year, month, 1):MMMM} {year} by {currentUser}");
                        }
                        catch { }
                    }

                    if (lblFilterInfo != null)
                    {
                        lblFilterInfo.Text = $"Showing sales data for {selectedFilter}";
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    GlobalLogger.LogError("Sales", ex.Message, "Error loading sales data");
                }
                catch { }
                MessageBox.Show($"Error loading sales: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSalesByWeek(int year, int weekNumber)
        {
            try
            {
                DateTime startDate = GetStartDateOfWeek(year, weekNumber);
                DateTime endDate = startDate.AddDays(6);

                DataTable salesData = GetDailySalesSummaryFromDatabase(startDate, endDate);
                dgvSales.DataSource = salesData;
                FormatDataGridView();
                CalculateAndDisplayTotals(salesData);

                if (lblFilterInfo != null)
                {
                    lblFilterInfo.Text = $"Week {weekNumber}, {year} ({startDate:MMM dd} - {endDate:MMM dd, yyyy})";
                }
            }
            catch (Exception ex)
            {
                try
                {
                    GlobalLogger.LogError("Sales", ex.Message, $"Error loading sales for Week {weekNumber}, {year}");
                }
                catch { }
                MessageBox.Show($"Error loading sales by week: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSalesByMonth(int year, int month)
        {
            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                DataTable salesData = GetDailySalesSummaryFromDatabase(startDate, endDate);
                dgvSales.DataSource = salesData;
                FormatDataGridView();
                CalculateAndDisplayTotals(salesData);

                if (lblFilterInfo != null)
                {
                    lblFilterInfo.Text = $"{new DateTime(year, month, 1):MMMM yyyy}";
                }
            }
            catch (Exception ex)
            {
                try
                {
                    GlobalLogger.LogError("Sales", ex.Message, $"Error loading sales for {new DateTime(year, month, 1):MMMM} {year}");
                }
                catch { }
                MessageBox.Show($"Error loading sales by month: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DateTime GetStartDateOfWeek(int year, int weekNumber)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            DateTime firstMonday = jan1.AddDays((8 - (int)jan1.DayOfWeek) % 7);
            DateTime startDate = firstMonday.AddDays((weekNumber - 1) * 7);
            return startDate;
        }

        private void FormatDataGridView()
        {
            if (dgvSales != null && dgvSales.Columns.Count > 0)
            {
                // Format currency columns
                if (dgvSales.Columns.Contains("Total Revenue"))
                {
                    dgvSales.Columns["Total Revenue"].DefaultCellStyle.Format = "₱#,##0.00";
                    dgvSales.Columns["Total Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvSales.Columns.Contains("Average Transaction"))
                {
                    dgvSales.Columns["Average Transaction"].DefaultCellStyle.Format = "₱#,##0.00";
                    dgvSales.Columns["Average Transaction"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvSales.Columns.Contains("Number of Transactions"))
                {
                    dgvSales.Columns["Number of Transactions"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Format date column
                if (dgvSales.Columns.Contains("Date"))
                {
                    dgvSales.Columns["Date"].DefaultCellStyle.Format = "MMM dd, yyyy";
                }
            }
        }

        private void CalculateAndDisplayTotals(DataTable salesData)
        {
            if (salesData != null && salesData.Rows.Count > 0)
            {
                int totalTransactions = 0;
                decimal totalRevenue = 0;

                foreach (DataRow row in salesData.Rows)
                {
                    if (salesData.Columns.Contains("Number of Transactions"))
                        totalTransactions += Convert.ToInt32(row["Number of Transactions"]);
                    if (salesData.Columns.Contains("Total Revenue"))
                        totalRevenue += Convert.ToDecimal(row["Total Revenue"]);
                }

                // You can display totals in a status bar or label if you have one
                System.Diagnostics.Debug.WriteLine($"Total Transactions: {totalTransactions}");
                System.Diagnostics.Debug.WriteLine($"Total Revenue: ₱{totalRevenue:N2}");
            }
        }

        private void DgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSales.Rows.Count)
            {
                // Clear previous highlight
                if (selectedRow != null)
                {
                    foreach (DataGridViewCell cell in selectedRow.Cells)
                    {
                        cell.Style.BackColor = defaultColor;
                        cell.Style.ForeColor = Color.Black;
                    }
                }

                // Highlight the selected row
                selectedRow = dgvSales.Rows[e.RowIndex];
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    cell.Style.BackColor = highlightColor;
                    cell.Style.ForeColor = Color.FromArgb(40, 41, 34);
                }

                if (dgvSales.Columns.Contains("Date") && selectedRow.Cells["Date"].Value != null)
                {
                    DateTime selectedDate = Convert.ToDateTime(selectedRow.Cells["Date"].Value);
                    System.Diagnostics.Debug.WriteLine($"Selected date: {selectedDate:MMM dd, yyyy}");

                    try
                    {
                        GlobalLogger.LogInfo("Sales", $"User {currentUser} selected date: {selectedDate:MMM dd, yyyy}");
                    }
                    catch { }
                }
            }
        }

        private void DgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSales.Rows.Count && dgvSales.Columns.Contains("Date"))
            {
                DateTime selectedDate = Convert.ToDateTime(dgvSales.Rows[e.RowIndex].Cells["Date"].Value);
                GenerateReportForDate(selectedDate);
            }
        }

        private void GenerateReportForDate(DateTime date)
        {
            try
            {
                DataTable dailyData = GetDailySalesSummaryFromDatabase(date, date);

                if (dailyData.Rows.Count > 0)
                {
                    currentSalesData = dailyData;
                    currentReportTitle = $"Sales Report for {date:MMMM dd, yyyy}";
                    currentDateRange = date.ToString("MMM dd, yyyy");
                    ShowReportMenu();
                }
                else
                {
                    MessageBox.Show($"No sales data found for {date:MMMM dd, yyyy}.",
                        "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    GlobalLogger.LogError("Sales", ex.Message, $"Error generating report for {date:MM/dd/yyyy}");
                }
                catch { }
                MessageBox.Show($"Error generating report: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowReportMenu()
        {
            ContextMenuStrip reportMenu = new ContextMenuStrip();
            reportMenu.BackColor = Color.FromArgb(40, 41, 34);
            reportMenu.ForeColor = Color.FromArgb(228, 186, 94);

            ToolStripMenuItem previewItem = new ToolStripMenuItem("Preview Report");
            previewItem.ForeColor = Color.FromArgb(228, 186, 94);
            previewItem.BackColor = Color.FromArgb(40, 41, 34);
            previewItem.Click += (s, args) => PreviewReport();
            reportMenu.Items.Add(previewItem);

            ToolStripMenuItem printItem = new ToolStripMenuItem("Print Report");
            printItem.ForeColor = Color.FromArgb(228, 186, 94);
            printItem.BackColor = Color.FromArgb(40, 41, 34);
            printItem.Click += (s, args) => PrintReport();
            reportMenu.Items.Add(printItem);

            ToolStripMenuItem saveTextItem = new ToolStripMenuItem("Save as Text File");
            saveTextItem.ForeColor = Color.FromArgb(228, 186, 94);
            saveTextItem.BackColor = Color.FromArgb(40, 41, 34);
            saveTextItem.Click += (s, args) => SaveAsTextFile();
            reportMenu.Items.Add(saveTextItem);

            ToolStripMenuItem saveCsvItem = new ToolStripMenuItem("Save as CSV File");
            saveCsvItem.ForeColor = Color.FromArgb(228, 186, 94);
            saveCsvItem.BackColor = Color.FromArgb(40, 41, 34);
            saveCsvItem.Click += (s, args) => SaveAsCsvFile();
            reportMenu.Items.Add(saveCsvItem);

            reportMenu.Show(btnLoadSales, new Point(btnLoadSales.Width - 150, btnLoadSales.Height));
        }

        private void InitializePrinting()
        {
            printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDocument_PrintPage;
            printDoc.BeginPrint += PrintDocument_BeginPrint;
            printDoc.EndPrint += PrintDocument_EndPrint;

            reportFont = new System.Drawing.Font("Arial", 10);
            headerFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            titleFont = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
        }

        private void GenerateReportData()
        {
            reportLines = new List<string>();

            try
            {
                // Report header
                reportLines.Add("MATCHPOINT GAME HUB");
                reportLines.Add("SALES REPORT");
                reportLines.Add("===============================");
                reportLines.Add($"Generated on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                reportLines.Add($"Generated by: {currentUser} ({currentUserRole})");
                reportLines.Add($"Report Period: {currentReportTitle}");
                reportLines.Add("");
                reportLines.Add("");

                if (currentSalesData != null && currentSalesData.Rows.Count > 0)
                {
                    // Sales Summary Section
                    reportLines.Add("SALES SUMMARY");
                    reportLines.Add("-------------");

                    int totalTransactions = 0;
                    decimal totalRevenue = 0;
                    decimal totalAvgTransaction = 0;

                    foreach (DataRow row in currentSalesData.Rows)
                    {
                        if (currentSalesData.Columns.Contains("Number of Transactions"))
                            totalTransactions += Convert.ToInt32(row["Number of Transactions"]);
                        if (currentSalesData.Columns.Contains("Total Revenue"))
                            totalRevenue += Convert.ToDecimal(row["Total Revenue"]);
                        if (currentSalesData.Columns.Contains("Average Transaction"))
                            totalAvgTransaction += Convert.ToDecimal(row["Average Transaction"]);
                    }

                    decimal avgOverallTransaction = currentSalesData.Rows.Count > 0 ?
                        totalAvgTransaction / currentSalesData.Rows.Count : 0;

                    reportLines.Add($"Total Transactions: {totalTransactions:N0}");
                    reportLines.Add($"Total Revenue: ₱{totalRevenue:N2}");
                    reportLines.Add($"Average Transaction Value: ₱{avgOverallTransaction:N2}");
                    reportLines.Add($"Number of Days: {currentSalesData.Rows.Count}");
                    if (currentSalesData.Rows.Count > 0)
                        reportLines.Add($"Average Daily Revenue: ₱{(totalRevenue / currentSalesData.Rows.Count):N2}");
                    reportLines.Add("");
                    reportLines.Add("");

                    // Daily Sales Details Section
                    reportLines.Add("DAILY SALES DETAILS");
                    reportLines.Add("-------------------");
                    reportLines.Add("");

                    reportLines.Add($"{"Date",-12} {"Day",-10} {"Transactions",-15} {"Revenue",-15} {"Avg Transaction",-18} {"Top Game",-15} {"Payment Methods"}");
                    reportLines.Add(new string('-', 110));

                    foreach (DataRow row in currentSalesData.Rows)
                    {
                        DateTime date = Convert.ToDateTime(row["Date"]);
                        string day = row["Day"].ToString();
                        int transactions = currentSalesData.Columns.Contains("Number of Transactions") ? Convert.ToInt32(row["Number of Transactions"]) : 0;
                        decimal revenue = currentSalesData.Columns.Contains("Total Revenue") ? Convert.ToDecimal(row["Total Revenue"]) : 0;
                        decimal avgTransaction = currentSalesData.Columns.Contains("Average Transaction") ? Convert.ToDecimal(row["Average Transaction"]) : 0;
                        string topGame = row["Top Game"].ToString();
                        string paymentMethods = row["Payment Methods"].ToString();

                        reportLines.Add($"{date:MM/dd/yyyy,-12} {day,-10} {transactions,-15:N0} ₱{revenue,-14:N2} ₱{avgTransaction,-17:N2} {topGame,-15} {paymentMethods}");
                    }

                    reportLines.Add("");
                    reportLines.Add("");

                    // Best Performing Day Section
                    if (currentSalesData.Rows.Count > 0 && currentSalesData.Columns.Contains("Total Revenue"))
                    {
                        reportLines.Add("BEST PERFORMING DAY");
                        reportLines.Add("------------------");

                        DataRow bestDay = currentSalesData.AsEnumerable()
                            .OrderByDescending(r => r.Field<decimal>("Total Revenue"))
                            .First();

                        DateTime bestDate = Convert.ToDateTime(bestDay["Date"]);
                        decimal bestRevenue = Convert.ToDecimal(bestDay["Total Revenue"]);
                        int bestTransactions = currentSalesData.Columns.Contains("Number of Transactions") ? Convert.ToInt32(bestDay["Number of Transactions"]) : 0;
                        string bestGame = bestDay["Top Game"].ToString();

                        reportLines.Add($"Date: {bestDate:MMMM dd, yyyy} ({bestDate:dddd})");
                        reportLines.Add($"Revenue: ₱{bestRevenue:N2}");
                        reportLines.Add($"Transactions: {bestTransactions:N0}");
                        reportLines.Add($"Top Game: {bestGame}");
                        reportLines.Add("");
                    }
                }

                // Peak Hours Information
                reportLines.Add("PEAK HOURS");
                reportLines.Add("----------");
                reportLines.Add("Peak Hours: 6:00 PM - 9:00 PM");
                reportLines.Add("Peak Days: Friday, Saturday, Sunday");
                reportLines.Add("");
                reportLines.Add("");

                // Footer
                reportLines.Add("==========================================");
                reportLines.Add("END OF REPORT");
                reportLines.Add("==========================================");
            }
            catch (Exception ex)
            {
                reportLines.Add($"ERROR GENERATING REPORT: {ex.Message}");
                try
                {
                    GlobalLogger.LogError("Sales", ex.Message, "Error generating report data");
                }
                catch { }
            }
        }

        private void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            GenerateReportData();
            currentLineIndex = 0;
            yPos = topMargin;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                float pageHeight = e.MarginBounds.Height;
                float pageWidth = e.MarginBounds.Width;

                if (currentLineIndex < reportLines.Count)
                {
                    string reportTitle = reportLines[0] + " " + reportLines[1];
                    g.DrawString(reportTitle, titleFont, Brushes.Black,
                                (pageWidth - g.MeasureString(reportTitle, titleFont).Width) / 2, yPos);
                    yPos += 50;
                    currentLineIndex = 2;

                    for (; currentLineIndex < reportLines.Count; currentLineIndex++)
                    {
                        if (yPos > pageHeight - 50)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        string line = reportLines[currentLineIndex];
                        System.Drawing.Font currentFont = reportFont;

                        if (line.Contains("SALES SUMMARY") || line.Contains("DAILY SALES DETAILS") ||
                            line.Contains("BEST PERFORMING DAY") || line.Contains("PEAK HOURS") ||
                            line.Contains("END OF REPORT"))
                        {
                            currentFont = headerFont;
                        }

                        g.DrawString(line, currentFont, Brushes.Black, leftMargin, yPos);
                        yPos += lineHeight;
                    }
                }

                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                e.Graphics.DrawString($"Error printing: {ex.Message}", reportFont, Brushes.Red, leftMargin, yPos);
                e.HasMorePages = false;
            }
        }

        private void PrintDocument_EndPrint(object sender, PrintEventArgs e)
        {
            try
            {
                GlobalLogger.LogInfo("Sales", $"Report printed by {currentUser}");
            }
            catch { }
        }

        private void SaveAsTextFile()
        {
            try
            {
                GenerateReportData();

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "txt";
                saveDialog.FileName = $"MatchPoint_Sales_Report_{DateTime.Now:yyyyMMdd_HHmm}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(saveDialog.FileName, reportLines);
                    MessageBox.Show($"Report saved successfully as:\n{saveDialog.FileName}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAsCsvFile()
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "csv";
                saveDialog.FileName = $"MatchPoint_Sales_Data_{DateTime.Now:yyyyMMdd_HHmm}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveDialog.FileName))
                    {
                        if (dgvSales != null && dgvSales.Columns.Count > 0)
                        {
                            string[] headers = dgvSales.Columns.Cast<DataGridViewColumn>()
                                .Select(column => column.HeaderText)
                                .ToArray();
                            sw.WriteLine(string.Join(",", headers));

                            foreach (DataGridViewRow row in dgvSales.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    string[] cells = row.Cells.Cast<DataGridViewCell>()
                                        .Select(cell => $"\"{cell.Value}\"")
                                        .ToArray();
                                    sw.WriteLine(string.Join(",", cells));
                                }
                            }
                        }
                    }

                    MessageBox.Show($"Data saved successfully as CSV:\n{saveDialog.FileName}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving CSV file: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReport()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDoc;
                printDialog.UseEXDialog = true;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                    MessageBox.Show("Report sent to printer successfully!",
                        "Print Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing: {ex.Message}",
                    "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreviewReport()
        {
            try
            {
                PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                previewDialog.Document = printDoc;
                previewDialog.WindowState = FormWindowState.Maximized;
                previewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing report: {ex.Message}",
                    "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetDailySalesSummaryFromDatabase(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Day", typeof(string));
            dt.Columns.Add("Number of Transactions", typeof(int));
            dt.Columns.Add("Total Revenue", typeof(decimal));
            dt.Columns.Add("Average Transaction", typeof(decimal));
            dt.Columns.Add("Top Game", typeof(string));
            dt.Columns.Add("Payment Methods", typeof(string));

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string checkTablesQuery = "SHOW TABLES LIKE 'orders'";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkTablesQuery, conn))
                    {
                        object result = checkCmd.ExecuteScalar();
                        if (result == null)
                        {
                            dt.Rows.Add(DateTime.Now, DateTime.Now.ToString("dddd"), 5, 2500.00m, 500.00m, "Billiards", "Cash, GCash");
                            dt.Rows.Add(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1).ToString("dddd"), 3, 1800.00m, 600.00m, "Badminton", "Cash");
                            return dt;
                        }
                    }

                    string query = @"
                        SELECT 
                            DATE(o.order_date) as SaleDate,
                            COUNT(DISTINCT o.order_number) as TransactionCount,
                            COALESCE(SUM(oi.price), 0) as DailyRevenue,
                            COALESCE(AVG(oi.price), 0) as AvgTransactionValue,
                            GROUP_CONCAT(DISTINCT 
                                CASE 
                                    WHEN p.payment_method IS NOT NULL THEN p.payment_method 
                                    ELSE 'Pending'
                                END
                            ) as PaymentMethods,
                            (
                                SELECT oi2.game_name 
                                FROM order_items oi2 
                                WHERE DATE(oi2.created_at) = DATE(o.order_date)
                                GROUP BY oi2.game_name 
                                ORDER BY COUNT(*) DESC 
                                LIMIT 1
                            ) as TopGame
                        FROM orders o
                        LEFT JOIN order_items oi ON o.order_number = oi.order_number
                        LEFT JOIN payments p ON o.order_number = p.receipt_no
                        WHERE DATE(o.order_date) BETWEEN @startDate AND @endDate
                        AND o.status = 'Completed'
                        GROUP BY DATE(o.order_date)
                        ORDER BY SaleDate";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@startDate", startDate);
                        cmd.Parameters.AddWithValue("@endDate", endDate);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime saleDate = reader.GetDateTime("SaleDate");
                                int transactionCount = reader.GetInt32("TransactionCount");
                                decimal dailyRevenue = reader.GetDecimal("DailyRevenue");
                                decimal avgTransaction = reader.GetDecimal("AvgTransactionValue");
                                string topGame = reader.IsDBNull(reader.GetOrdinal("TopGame")) ? "N/A" : reader.GetString("TopGame");
                                string paymentMethods = reader.IsDBNull(reader.GetOrdinal("PaymentMethods")) ? "N/A" : reader.GetString("PaymentMethods");

                                dt.Rows.Add(
                                    saleDate,
                                    saleDate.ToString("dddd"),
                                    transactionCount,
                                    dailyRevenue,
                                    avgTransaction,
                                    topGame,
                                    paymentMethods
                                );
                            }
                        }
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add(DateTime.Now, DateTime.Now.ToString("dddd"), 5, 2500.00m, 500.00m, "Billiards", "Cash, GCash");
                    dt.Rows.Add(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1).ToString("dddd"), 3, 1800.00m, 600.00m, "Badminton", "Cash");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving sales data: {ex.Message}\n\nUsing sample data for demonstration.",
                    "Database Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dt.Rows.Add(DateTime.Now, DateTime.Now.ToString("dddd"), 5, 2500.00m, 500.00m, "Billiards", "Cash, GCash");
                dt.Rows.Add(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1).ToString("dddd"), 3, 1800.00m, 600.00m, "Badminton", "Cash");
            }

            return dt;
        }

        public void ClearHighlight()
        {
            if (selectedRow != null)
            {
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    cell.Style.BackColor = defaultColor;
                    cell.Style.ForeColor = Color.Black;
                }
                selectedRow = null;
            }
        }

        public void HighlightDateRange(DateTime startDate, DateTime endDate)
        {
            if (dgvSales == null) return;
            ClearHighlight();

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.Cells["Date"] != null && row.Cells["Date"].Value != null)
                {
                    DateTime rowDate = Convert.ToDateTime(row.Cells["Date"].Value);
                    if (rowDate >= startDate && rowDate <= endDate)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = highlightColor;
                            cell.Style.ForeColor = Color.FromArgb(40, 41, 34);
                        }
                    }
                }
            }
        }

        private void dtpYear_ValueChanged(object sender, EventArgs e)
        {
            if (cmbWeek != null && cmbFilterType?.SelectedItem?.ToString() == "Select Week")
            {
                int year = dtpYear.Value.Year;
                int weeksInYear = GetWeeksInYear(year);

                cmbWeek.Items.Clear();
                for (int i = 1; i <= weeksInYear; i++)
                {
                    cmbWeek.Items.Add($"Week {i}");
                }
                cmbWeek.SelectedIndex = 0;
            }
        }

        private int GetWeeksInYear(int year)
        {
            DateTime dec31 = new DateTime(year, 12, 31);
            return GetWeekNumberOfYear(dec31);
        }

        private void SALES_Load(object sender, EventArgs e)
        {
            if (dtpYear != null)
                dtpYear.Value = new DateTime(DateTime.Now.Year, 1, 1);
            LoadCurrentWeekData();

            try
            {
                GlobalLogger.LogInfo("Sales", $"Sales module loaded by {currentUser}");
            }
            catch { }
        }
    }
}