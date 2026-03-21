using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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

        // Store current data
        private int[] dailySales = { 12500, 18700, 15400, 21000, 28500, 32000, 27500 };
        private int[] dailyCustomerCounts = { 45, 67, 54, 78, 95, 120, 105 };
        private string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        private string[] fullDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public UserControl2()
        {
            InitializeComponent();
            SetupCharts();
            InitializePrinting();

            // Wire up the click events
            if (generateReport != null)
                generateReport.Click += generateReport_Click;

            if (refreshData != null)
                refreshData.Click += refreshData_Click;
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
            SetupWeeklyChart(); // Keep this as is (sales chart)
            SetupWeeklyCustomerChart(); // Changed from monthly to weekly customer counts
        }

        private void SetupWeeklyChart()
        {
            if (weeklySale != null)
            {
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
                weeklyTitle.Text = "Weekly Sales Report";
                weeklyTitle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                weeklyTitle.TextAlign = ContentAlignment.MiddleCenter;
                weeklyTitle.Dock = DockStyle.Top;
                weeklyTitle.Height = 30;
                weeklyTitle.BackColor = Color.FromArgb(40, 41, 34);
                weeklyTitle.ForeColor = Color.FromArgb(228, 186, 94);

                weeklySale.Controls.Add(weeklyTitle);
                weeklySale.Controls.Add(weeklyChart);
            }
        }

        private void SetupWeeklyCustomerChart()
        {
            if (customerCount != null)
            {
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
        }

        private void generateReport_Click(object sender, EventArgs e)
        {
            try
            {
                ContextMenuStrip reportMenu = new ContextMenuStrip();

                ToolStripMenuItem previewItem = new ToolStripMenuItem("Preview Report");
                previewItem.Click += (s, args) => PreviewReport();
                reportMenu.Items.Add(previewItem);

                ToolStripMenuItem printItem = new ToolStripMenuItem("Print Report");
                printItem.Click += (s, args) => PrintReport();
                reportMenu.Items.Add(printItem);

                ToolStripMenuItem saveTextItem = new ToolStripMenuItem("Save as Text File");
                saveTextItem.Click += (s, args) => SaveAsTextFile();
                reportMenu.Items.Add(saveTextItem);

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
                SetupCharts();
                MessageBox.Show("Data refreshed successfully!", "Dashboard",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                reportLines.Add("MATCHPOINT GAME HUB REPORT");
                reportLines.Add("===============================");
                reportLines.Add($"Generated on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
                reportLines.Add("");
                reportLines.Add("");

                // Weekly Sales Section
                reportLines.Add("WEEKLY SALES REPORT");
                reportLines.Add("-------------------");

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

                // Weekly Customer Counts Section (NEW - Replaces Monthly Sales)
                reportLines.Add("WEEKLY CUSTOMER COUNTS");
                reportLines.Add("---------------------");

                for (int i = 0; i < fullDays.Length; i++)
                {
                    reportLines.Add($"  {fullDays[i].PadRight(12)} {dailyCustomerCounts[i]:#,##0} customers");
                }

                int weeklyCustomerTotal = dailyCustomerCounts.Sum();
                reportLines.Add($"");
                reportLines.Add($"  Total Weekly Customers: {weeklyCustomerTotal:#,##0}");
                reportLines.Add($"  Average Daily Customers: {(weeklyCustomerTotal / 7):#,##0}");
                reportLines.Add($"  Peak Day: {fullDays[Array.IndexOf(dailyCustomerCounts, dailyCustomerCounts.Max())]} with {dailyCustomerCounts.Max():#,##0} customers");
                reportLines.Add("");
                reportLines.Add("");

                // Summary Section
                reportLines.Add("SUMMARY");
                reportLines.Add("-------");
                reportLines.Add($"  Total Revenue: ₱{weeklyTotal:#,##0}");
                reportLines.Add($"  Total Customers: {weeklyCustomerTotal:#,##0}");
                reportLines.Add($"  Average Transaction Value: ₱{(weeklyTotal / weeklyCustomerTotal):#,##0}");
                reportLines.Add($"  Best Performing Day: {fullDays[Array.IndexOf(dailySales, dailySales.Max())]} (₱{dailySales.Max():#,##0})");
                reportLines.Add($"  Most Customers Day: {fullDays[Array.IndexOf(dailyCustomerCounts, dailyCustomerCounts.Max())]} ({dailyCustomerCounts.Max():#,##0} customers)");
                reportLines.Add($"  Peak Hour: 6:00 PM - 9:00 PM");
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

                // Draw report title
                string reportTitle = "MATCHPOINT GAME HUB REPORT";
                g.DrawString(reportTitle, titleFont, Brushes.Black,
                            (pageWidth - g.MeasureString(reportTitle, titleFont).Width) / 2, yPos);
                yPos += 40;

                // Draw generation date
                g.DrawString($"Generated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}", reportFont, Brushes.Black, leftMargin, yPos);
                yPos += 30;

                // Draw separator line
                g.DrawLine(Pens.Black, leftMargin, yPos, pageWidth - leftMargin, yPos);
                yPos += 20;

                // Draw Weekly Sales Section
                g.DrawString("WEEKLY SALES", headerFont, Brushes.Black, leftMargin, yPos);
                yPos += 25;

                for (int i = 0; i < fullDays.Length; i++)
                {
                    if (yPos > pageHeight - 50)
                    {
                        e.HasMorePages = true;
                        return;
                    }
                    g.DrawString($"{fullDays[i].PadRight(15)} ₱{dailySales[i]:#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                    yPos += lineHeight;
                }

                int weeklyTotal = dailySales.Sum();
                g.DrawString($"Total Weekly Sales: ₱{weeklyTotal:#,##0}", headerFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Average Daily Sales: ₱{(weeklyTotal / 7):#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += 40;

                // Check for page break
                if (yPos > pageHeight - 150)
                {
                    e.HasMorePages = true;
                    return;
                }

                // Draw Weekly Customer Counts Section
                g.DrawString("WEEKLY CUSTOMER COUNTS", headerFont, Brushes.Black, leftMargin, yPos);
                yPos += 25;

                for (int i = 0; i < fullDays.Length; i++)
                {
                    if (yPos > pageHeight - 50)
                    {
                        e.HasMorePages = true;
                        return;
                    }
                    g.DrawString($"{fullDays[i].PadRight(15)} {dailyCustomerCounts[i]:#,##0} customers", reportFont, Brushes.Black, leftMargin + 20, yPos);
                    yPos += lineHeight;
                }

                int weeklyCustomerTotal = dailyCustomerCounts.Sum();
                g.DrawString($"Total Weekly Customers: {weeklyCustomerTotal:#,##0}", headerFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Average Daily Customers: {(weeklyCustomerTotal / 7):#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Peak Day: {fullDays[Array.IndexOf(dailyCustomerCounts, dailyCustomerCounts.Max())]} with {dailyCustomerCounts.Max():#,##0} customers", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += 40;

                // Check for page break
                if (yPos > pageHeight - 150)
                {
                    e.HasMorePages = true;
                    return;
                }

                // Draw Summary Section
                g.DrawString("SUMMARY", headerFont, Brushes.Black, leftMargin, yPos);
                yPos += 25;

                g.DrawString($"Total Revenue: ₱{weeklyTotal:#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Total Customers: {weeklyCustomerTotal:#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Average Transaction Value: ₱{(weeklyTotal / weeklyCustomerTotal):#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Best Performing Day: {fullDays[Array.IndexOf(dailySales, dailySales.Max())]} (₱{dailySales.Max():#,##0})", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Most Customers Day: {fullDays[Array.IndexOf(dailyCustomerCounts, dailyCustomerCounts.Max())]} ({dailyCustomerCounts.Max():#,##0} customers)", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
                g.DrawString($"Peak Hour: 6:00 PM - 9:00 PM", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;

                // Footer
                g.DrawLine(Pens.Black, leftMargin, yPos, pageWidth - leftMargin, yPos);
                yPos += 20;
                g.DrawString("END OF REPORT", reportFont, Brushes.Black,
                            (pageWidth - g.MeasureString("END OF REPORT", reportFont).Width) / 2, yPos);

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
                saveDialog.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
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
            MessageBox.Show("Charts refreshed!", "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panel12_Click(object sender, EventArgs e)
        {
            SetupCharts();
            MessageBox.Show("Data refreshed successfully!", "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label7_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void panel8_Paint(object sender, PaintEventArgs e) { }
        private void monthlySale_Paint(object sender, PaintEventArgs e) { }
        private void weeklySale_Paint(object sender, PaintEventArgs e) { }
        private void activeSessionTxt_Click(object sender, EventArgs e) { }
        private void totalSessionTxt_Click(object sender, EventArgs e) { }
        private void totalSalesTxt_Click(object sender, EventArgs e) { }
    }
}