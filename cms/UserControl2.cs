using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace cms
{
    public partial class UserControl2 : UserControl
    {
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

        // Database connection string
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";

        // Store current data
        private int[] dailySales = new int[7];
        private int[] dailyCustomerCounts = new int[7];
        private string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        private string[] fullDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        // Additional metrics
        private decimal totalRevenue = 0;
        private int totalCustomers = 0;
        private int pendingOrders = 0;
        private int completedOrders = 0;
        private int tablesInUse = 0;
        private decimal averageOrderValue = 0;

        // Game performance data
        private Dictionary<string, decimal> gameRevenue = new Dictionary<string, decimal>();
        private Dictionary<string, int> gameSessions = new Dictionary<string, int>();

        public UserControl2()
        {
            InitializeComponent();
            InitializePrinting();
            LoadDataFromDatabase();

            // Wire up the click events
            if (generateReport != null)
                generateReport.Click += generateReport_Click;

            if (refreshData != null)
                refreshData.Click += refreshData_Click;
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    // Check if tables exist
                    string checkOrdersTable = "SHOW TABLES LIKE 'orders'";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkOrdersTable, conn))
                    {
                        object result = checkCmd.ExecuteScalar();
                        if (result == null)
                        {
                            // No orders table, use demo data
                            LoadDemoData();
                            SetupCharts();
                            return;
                        }
                    }

                    // Load all data from database
                    LoadWeeklySales(conn);
                    LoadWeeklyCustomers(conn);
                    LoadSummaryMetrics(conn);
                    LoadGamePerformance(conn);
                    LoadTablesInUse(conn);

                    // Update UI with loaded data
                    UpdateMetricDisplays();

                    // Setup charts after data is loaded
                    SetupCharts();

                    // Log successful data load
                    try
                    {
                        GlobalLogger.LogInfo("Dashboard", $"Dashboard data loaded: {totalRevenue:C}, {totalCustomers} customers");
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading dashboard data: {ex.Message}");

                // Fallback to demo data if database connection fails
                LoadDemoData();
                SetupCharts();

                try
                {
                    GlobalLogger.LogError("Dashboard", $"Error loading data: {ex.Message}");
                }
                catch { }

                MessageBox.Show($"Unable to connect to database. Using demo data.\n\nError: {ex.Message}",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void LoadWeeklySales(MySqlConnection conn)
        {
            // Initialize with zeros
            for (int i = 0; i < 7; i++)
            {
                dailySales[i] = 0;
            }

            // Get current week's date range (Monday to Sunday)
            DateTime today = DateTime.Now;
            int daysUntilMonday = ((int)today.DayOfWeek == 0 ? 6 : (int)today.DayOfWeek - 1);
            DateTime startOfWeek = today.AddDays(-daysUntilMonday).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7).Date;

            // Query sales from orders table where status is 'Completed'
            string query = @"
                SELECT 
                    DATE(order_date) as OrderDate,
                    COALESCE(SUM(total_amount), 0) as DailyTotal
                FROM orders
                WHERE order_date >= @startDate 
                    AND order_date < @endDate
                    AND status = 'Completed'
                GROUP BY DATE(order_date)";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@startDate", startOfWeek);
                cmd.Parameters.AddWithValue("@endDate", endOfWeek);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime orderDate = reader.GetDateTime("OrderDate");
                        decimal dailyTotal = reader.GetDecimal("DailyTotal");

                        // Determine day of week index (0 = Monday, 6 = Sunday)
                        int dayIndex = ((int)orderDate.DayOfWeek == 0 ? 6 : (int)orderDate.DayOfWeek - 1);
                        if (dayIndex >= 0 && dayIndex < 7)
                        {
                            dailySales[dayIndex] = (int)dailyTotal;
                        }
                    }
                }
            }
        }

        private void LoadWeeklyCustomers(MySqlConnection conn)
        {
            // Initialize with zeros
            for (int i = 0; i < 7; i++)
            {
                dailyCustomerCounts[i] = 0;
            }

            // Get current week's date range
            DateTime today = DateTime.Now;
            int daysUntilMonday = ((int)today.DayOfWeek == 0 ? 6 : (int)today.DayOfWeek - 1);
            DateTime startOfWeek = today.AddDays(-daysUntilMonday).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7).Date;

            // Query customer counts from orders table (count unique order numbers)
            string query = @"
                SELECT 
                    DATE(order_date) as OrderDate,
                    COUNT(DISTINCT order_number) as CustomerCount
                FROM orders
                WHERE order_date >= @startDate 
                    AND order_date < @endDate
                    AND status = 'Completed'
                GROUP BY DATE(order_date)";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@startDate", startOfWeek);
                cmd.Parameters.AddWithValue("@endDate", endOfWeek);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime orderDate = reader.GetDateTime("OrderDate");
                        int customerCount = reader.GetInt32("CustomerCount");

                        int dayIndex = ((int)orderDate.DayOfWeek == 0 ? 6 : (int)orderDate.DayOfWeek - 1);
                        if (dayIndex >= 0 && dayIndex < 7)
                        {
                            dailyCustomerCounts[dayIndex] = customerCount;
                        }
                    }
                }
            }
        }

        private void LoadSummaryMetrics(MySqlConnection conn)
        {
            // Get current week's date range
            DateTime today = DateTime.Now;
            int daysUntilMonday = ((int)today.DayOfWeek == 0 ? 6 : (int)today.DayOfWeek - 1);
            DateTime startOfWeek = today.AddDays(-daysUntilMonday).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7).Date;

            // Query summary metrics for current week
            string query = @"
                SELECT 
                    COALESCE(SUM(CASE WHEN status = 'Completed' THEN total_amount ELSE 0 END), 0) as TotalRevenue,
                    COUNT(DISTINCT CASE WHEN status = 'Completed' THEN order_number END) as TotalCustomers,
                    COUNT(CASE WHEN status = 'Pending' THEN 1 END) as PendingOrders,
                    COUNT(CASE WHEN status = 'Completed' THEN 1 END) as CompletedOrders,
                    COALESCE(AVG(CASE WHEN status = 'Completed' THEN total_amount END), 0) as AvgOrderValue
                FROM orders
                WHERE order_date >= @startDate 
                    AND order_date < @endDate";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@startDate", startOfWeek);
                cmd.Parameters.AddWithValue("@endDate", endOfWeek);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totalRevenue = reader.GetDecimal("TotalRevenue");
                        totalCustomers = reader.GetInt32("TotalCustomers");
                        pendingOrders = reader.GetInt32("PendingOrders");
                        completedOrders = reader.GetInt32("CompletedOrders");
                        averageOrderValue = reader.GetDecimal("AvgOrderValue");
                    }
                }
            }
        }

        private void LoadGamePerformance(MySqlConnection conn)
        {
            gameRevenue.Clear();
            gameSessions.Clear();

            // Get current week's date range
            DateTime today = DateTime.Now;
            int daysUntilMonday = ((int)today.DayOfWeek == 0 ? 6 : (int)today.DayOfWeek - 1);
            DateTime startOfWeek = today.AddDays(-daysUntilMonday).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7).Date;

            string query = @"
                SELECT 
                    oi.game_name,
                    COUNT(*) as total_sessions,
                    COALESCE(SUM(oi.price), 0) as total_revenue,
                    COALESCE(SUM(oi.duration_minutes), 0) as total_minutes
                FROM order_items oi
                JOIN orders o ON oi.order_number = o.order_number
                WHERE o.status = 'Completed'
                    AND o.order_date >= @startDate 
                    AND o.order_date < @endDate
                GROUP BY oi.game_name
                ORDER BY total_revenue DESC";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@startDate", startOfWeek);
                cmd.Parameters.AddWithValue("@endDate", endOfWeek);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string gameName = reader["game_name"].ToString();
                        int sessions = reader.GetInt32("total_sessions");
                        decimal revenue = reader.GetDecimal("total_revenue");

                        gameRevenue[gameName] = revenue;
                        gameSessions[gameName] = sessions;
                    }
                }
            }

            // If no data, add demo games
            if (gameRevenue.Count == 0)
            {
                gameRevenue["Billiards"] = 0;
                gameRevenue["Badminton"] = 0;
                gameRevenue["Scooter"] = 0;
                gameRevenue["Table Tennis"] = 0;
                gameSessions["Billiards"] = 0;
                gameSessions["Badminton"] = 0;
                gameSessions["Scooter"] = 0;
                gameSessions["Table Tennis"] = 0;
            }
        }

        private void LoadTablesInUse(MySqlConnection conn)
        {
            // Count active tables (tables with pending orders)
            string query = @"
                SELECT COUNT(DISTINCT table_number) as TablesInUse
                FROM orders
                WHERE status = 'Pending'
                    AND table_number IS NOT NULL
                    AND table_number != ''";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    tablesInUse = result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                tablesInUse = pendingOrders; // Fallback
            }
        }

        private void UpdateMetricDisplays()
        {
            // Update labels - check if controls exist
            if (totalSalesTxt != null)
                totalSalesTxt.Text = $"₱{totalRevenue:#,##0}";

            if (totalSessionTxt != null)
                totalSessionTxt.Text = completedOrders.ToString();

            if (activeSessionTxt != null)
                activeSessionTxt.Text = pendingOrders.ToString();

            if (tablesInUseTxt != null)
                tablesInUseTxt.Text = tablesInUse.ToString();
        }

        private void LoadDemoData()
        {
            // Fallback demo data for testing
            dailySales = new int[] { 12500, 18700, 15400, 21000, 28500, 32000, 27500 };
            dailyCustomerCounts = new int[] { 45, 67, 54, 78, 95, 120, 105 };
            totalRevenue = 155600;
            totalCustomers = 564;
            pendingOrders = 12;
            completedOrders = 89;
            tablesInUse = 6;
            averageOrderValue = 1750;

            // Demo game performance
            gameRevenue["Billiards"] = 45000;
            gameRevenue["Badminton"] = 32000;
            gameRevenue["Scooter"] = 28000;
            gameRevenue["Table Tennis"] = 18000;

            gameSessions["Billiards"] = 45;
            gameSessions["Badminton"] = 38;
            gameSessions["Scooter"] = 42;
            gameSessions["Table Tennis"] = 25;

            UpdateMetricDisplays();
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

        private void SetupCharts()
        {
            SetupWeeklySalesChart();
            SetupWeeklyCustomerChart();
        }

        private void SetupWeeklySalesChart()
        {
            if (weeklySale == null) return;

            weeklySale.Controls.Clear();

            Chart weeklyChart = new Chart();
            weeklyChart.Dock = DockStyle.Fill;
            weeklyChart.BackColor = Color.White;

            ChartArea chartArea = new ChartArea();
            chartArea.Name = "WeeklyChartArea";
            chartArea.BackColor = Color.FromArgb(245, 245, 245);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
            chartArea.AxisX.Title = "Days";
            chartArea.AxisY.Title = "Sales (₱)";
            chartArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
            chartArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
            weeklyChart.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.Name = "WeeklySales";
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.FromArgb(40, 41, 34);
            series.BorderWidth = 2;
            series.IsValueShownAsLabel = true;
            series.LabelForeColor = Color.FromArgb(40, 41, 34);
            series.Font = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            series.LabelFormat = "₱{0:#,0}";

            for (int i = 0; i < days.Length; i++)
            {
                DataPoint point = new DataPoint();
                point.SetValueXY(days[i], dailySales[i]);
                point.Color = i >= 5 ? Color.FromArgb(228, 186, 94) : Color.FromArgb(40, 41, 34);
                series.Points.Add(point);
            }

            weeklyChart.Series.Add(series);

            Label weeklyTitle = new Label();
            weeklyTitle.Text = $"Weekly Sales Report ({DateTime.Now:MMMM dd, yyyy})";
            weeklyTitle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            weeklyTitle.TextAlign = ContentAlignment.MiddleCenter;
            weeklyTitle.Dock = DockStyle.Top;
            weeklyTitle.Height = 30;
            weeklyTitle.BackColor = Color.FromArgb(40, 41, 34);
            weeklyTitle.ForeColor = Color.FromArgb(228, 186, 94);

            weeklySale.Controls.Add(weeklyTitle);
            weeklySale.Controls.Add(weeklyChart);
        }

        private void SetupWeeklyCustomerChart()
        {
            if (customerCount == null) return;

            customerCount.Controls.Clear();

            Chart weeklyCustomerChart = new Chart();
            weeklyCustomerChart.Dock = DockStyle.Fill;
            weeklyCustomerChart.BackColor = Color.White;

            ChartArea chartArea = new ChartArea();
            chartArea.Name = "WeeklyCustomerChartArea";
            chartArea.BackColor = Color.FromArgb(250, 250, 250);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
            chartArea.AxisX.Title = "Days";
            chartArea.AxisY.Title = "Customer Count";
            chartArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
            chartArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
            weeklyCustomerChart.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.Name = "WeeklyCustomerCount";
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.FromArgb(40, 41, 34);
            series.BorderWidth = 2;
            series.IsValueShownAsLabel = true;
            series.LabelForeColor = Color.FromArgb(40, 41, 34);
            series.Font = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            series.LabelFormat = "{0:#,0}";

            for (int i = 0; i < days.Length; i++)
            {
                DataPoint point = new DataPoint();
                point.SetValueXY(days[i], dailyCustomerCounts[i]);
                point.Color = i >= 5 ? Color.FromArgb(228, 186, 94) : Color.FromArgb(40, 41, 34);
                series.Points.Add(point);
            }

            weeklyCustomerChart.Series.Add(series);

            Label customerTitle = new Label();
            customerTitle.Text = "Weekly Customer Counts";
            customerTitle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            customerTitle.TextAlign = ContentAlignment.MiddleCenter;
            customerTitle.Dock = DockStyle.Top;
            customerTitle.Height = 30;
            customerTitle.BackColor = Color.FromArgb(228, 186, 94);
            customerTitle.ForeColor = Color.FromArgb(40, 41, 34);

            customerCount.Controls.Add(customerTitle);
            customerCount.Controls.Add(weeklyCustomerChart);
        }

        private void generateReport_Click(object sender, EventArgs e)
        {
            try
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

                reportMenu.Show(generateReport, new Point(0, generateReport.Height));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing report menu: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshData_Click(object sender, EventArgs e)
        {
            try
            {
                // Reload data from database
                LoadDataFromDatabase();

                // Refresh charts
                SetupCharts();

                MessageBox.Show("Data refreshed successfully from database!", "Dashboard",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    GlobalLogger.LogInfo("Dashboard", "Dashboard data refreshed by user");
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateReportData()
        {
            reportLines = new List<string>();

            try
            {
                // Report header
                reportLines.Add("MATCHPOINT GAME HUB");
                reportLines.Add("WEEKLY PERFORMANCE REPORT");
                reportLines.Add("===============================");
                reportLines.Add($"Generated on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
                reportLines.Add($"Reporting Period: {DateTime.Now.AddDays(-6):MMMM dd, yyyy} - {DateTime.Now:MMMM dd, yyyy}");
                reportLines.Add("");
                reportLines.Add("");

                // Summary Metrics
                reportLines.Add("SUMMARY METRICS");
                reportLines.Add("---------------");
                reportLines.Add($"  Total Revenue: ₱{totalRevenue:#,##0}");
                reportLines.Add($"  Total Customers: {totalCustomers:#,##0}");
                reportLines.Add($"  Completed Orders: {completedOrders:#,##0}");
                reportLines.Add($"  Pending Orders: {pendingOrders:#,##0}");
                reportLines.Add($"  Tables in Use: {tablesInUse}");
                reportLines.Add($"  Average Order Value: ₱{averageOrderValue:#,##0}");
                reportLines.Add("");
                reportLines.Add("");

                // Weekly Sales Section
                reportLines.Add("WEEKLY SALES BREAKDOWN");
                reportLines.Add("---------------------");

                for (int i = 0; i < fullDays.Length; i++)
                {
                    reportLines.Add($"  {fullDays[i].PadRight(12)} ₱{dailySales[i]:#,##0}");
                }

                int weeklyTotal = dailySales.Sum();
                reportLines.Add($"");
                reportLines.Add($"  Total Weekly Sales: ₱{weeklyTotal:#,##0}");
                reportLines.Add($"  Average Daily Sales: ₱{(weeklyTotal / 7):#,##0}");
                reportLines.Add("");
                reportLines.Add("");

                // Weekly Customer Counts Section
                reportLines.Add("WEEKLY CUSTOMER BREAKDOWN");
                reportLines.Add("------------------------");

                for (int i = 0; i < fullDays.Length; i++)
                {
                    reportLines.Add($"  {fullDays[i].PadRight(12)} {dailyCustomerCounts[i]:#,##0} customers");
                }

                int weeklyCustomerTotal = dailyCustomerCounts.Sum();
                reportLines.Add($"");
                reportLines.Add($"  Total Weekly Customers: {weeklyCustomerTotal:#,##0}");
                reportLines.Add($"  Average Daily Customers: {(weeklyCustomerTotal / 7):#,##0}");
                if (dailyCustomerCounts.Length > 0)
                {
                    int maxIndex = Array.IndexOf(dailyCustomerCounts, dailyCustomerCounts.Max());
                    reportLines.Add($"  Peak Day: {fullDays[maxIndex]} with {dailyCustomerCounts.Max():#,##0} customers");
                }
                reportLines.Add("");
                reportLines.Add("");

                // Game Performance Section
                reportLines.Add("GAME PERFORMANCE");
                reportLines.Add("----------------");

                var sortedGames = gameRevenue.OrderByDescending(g => g.Value).ToList();
                for (int i = 0; i < sortedGames.Count && i < 5; i++)
                {
                    var game = sortedGames[i];
                    int sessions = gameSessions.ContainsKey(game.Key) ? gameSessions[game.Key] : 0;
                    reportLines.Add($"  {i + 1}. {game.Key.PadRight(15)} ₱{game.Value:#,##0} ({sessions} sessions)");
                }
                reportLines.Add("");
                reportLines.Add("");

                // Peak Hours Information
                reportLines.Add("PEAK HOURS & DAYS");
                reportLines.Add("-----------------");
                reportLines.Add("  Peak Hours: 6:00 PM - 9:00 PM");
                reportLines.Add("  Peak Days: Friday, Saturday, Sunday");
                reportLines.Add("  Most Popular Games: " + string.Join(", ", sortedGames.Take(3).Select(g => g.Key)));
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
                    // Draw report title
                    string reportTitle = reportLines[0] + " " + reportLines[1];
                    g.DrawString(reportTitle, titleFont, Brushes.Black,
                                (pageWidth - g.MeasureString(reportTitle, titleFont).Width) / 2, yPos);
                    yPos += 50;
                    currentLineIndex = 2;

                    // Draw remaining lines
                    for (; currentLineIndex < reportLines.Count; currentLineIndex++)
                    {
                        if (yPos > pageHeight - 50)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        string line = reportLines[currentLineIndex];
                        System.Drawing.Font currentFont = reportFont;

                        // Style headers
                        if (line.Contains("SUMMARY METRICS") || line.Contains("WEEKLY SALES BREAKDOWN") ||
                            line.Contains("WEEKLY CUSTOMER BREAKDOWN") || line.Contains("GAME PERFORMANCE") ||
                            line.Contains("PEAK HOURS & DAYS") || line.Contains("END OF REPORT"))
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
            // Cleanup if needed
        }

        private void SaveAsTextFile()
        {
            try
            {
                GenerateReportData();

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "txt";
                saveDialog.FileName = $"MatchPoint_Report_{DateTime.Now:yyyyMMdd_HHmm}";

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
                saveDialog.FileName = $"MatchPoint_Data_{DateTime.Now:yyyyMMdd_HHmm}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveDialog.FileName))
                    {
                        // Write headers
                        sw.WriteLine("Date,Day,Sales (₱),Customers");

                        // Write daily data
                        for (int i = 0; i < fullDays.Length; i++)
                        {
                            DateTime currentDate = DateTime.Now.AddDays(-(6 - i));
                            sw.WriteLine($"{currentDate:MM/dd/yyyy},{fullDays[i]},{dailySales[i]},{dailyCustomerCounts[i]}");
                        }

                        // Write summary
                        sw.WriteLine();
                        sw.WriteLine($"Total Revenue,₱{totalRevenue:#,##0}");
                        sw.WriteLine($"Total Customers,{totalCustomers}");
                        sw.WriteLine($"Completed Orders,{completedOrders}");
                        sw.WriteLine($"Pending Orders,{pendingOrders}");
                        sw.WriteLine($"Tables in Use,{tablesInUse}");
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

        // Existing event handlers
        private void weeklySale_Click(object sender, EventArgs e)
        {
            SetupCharts();
        }

        private void panel12_Click(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
            SetupCharts();
        }

        private void label7_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void panel8_Paint(object sender, PaintEventArgs e) { }
        private void monthlySale_Paint(object sender, PaintEventArgs e) { }
        private void weeklySale_Paint(object sender, PaintEventArgs e) { }
        private void activeSessionTxt_Click(object sender, EventArgs e) { }
        private void totalSessionTxt_Click(object sender, EventArgs e) { }
        private void totalSalesTxt_Click(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}