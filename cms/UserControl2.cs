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

        public UserControl2()
        {
            InitializeComponent();
            SetupCharts();
            InitializePrinting();

            // Wire up the click events for the new controls
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

            // Set up fonts for printing - use System.Drawing.Font explicitly
            reportFont = new System.Drawing.Font("Arial", 10);
            headerFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            titleFont = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
        }

        private void SetupCharts()
        {
            SetupWeeklyChart();
            SetupMonthlyChart();
        }

        private void SetupWeeklyChart()
        {
            if (weeklySale != null)
            {
                weeklySale.Controls.Clear();

                // Create a Chart control
                Chart weeklyChart = new Chart();
                weeklyChart.Dock = DockStyle.Fill;
                weeklyChart.BackColor = Color.White;

                // Create chart area
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

                // Create series
                Series series = new Series();
                series.Name = "WeeklySales";
                series.ChartType = SeriesChartType.Column;
                series.Color = Color.FromArgb(40, 41, 34);
                series.BorderWidth = 2;
                series.IsValueShownAsLabel = true;
                series.LabelForeColor = Color.FromArgb(40, 41, 34);
                series.Font = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
                series.LabelFormat = "₱{0:#,0}";

                // Add data
                string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                int[] sales = { 12500, 18700, 15400, 21000, 28500, 32000, 27500 };

                for (int i = 0; i < days.Length; i++)
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(days[i], sales[i]);
                    point.Color = i >= 5 ? Color.FromArgb(228, 186, 94) : Color.FromArgb(40, 41, 34); // Highlight weekends
                    series.Points.Add(point);
                }

                weeklyChart.Series.Add(series);

                // Add title
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

        private void SetupMonthlyChart()
        {
            if (monthlySale != null)
            {
                monthlySale.Controls.Clear();

                // Create a Chart control
                Chart monthlyChart = new Chart();
                monthlyChart.Dock = DockStyle.Fill;
                monthlyChart.BackColor = Color.White;

                // Create chart area
                ChartArea chartArea = new ChartArea();
                chartArea.Name = "MonthlyChartArea";
                chartArea.BackColor = Color.FromArgb(250, 250, 250);
                chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
                chartArea.AxisX.Title = "Months";
                chartArea.AxisY.Title = "Sales (₱)";
                chartArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
                chartArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 9, FontStyle.Bold);

                // Rotate month labels for better fit
                chartArea.AxisX.LabelStyle.Angle = -45;
                chartArea.AxisX.Interval = 1;
                monthlyChart.ChartAreas.Add(chartArea);

                // Create series - use line chart for monthly trend
                Series series = new Series();
                series.Name = "MonthlySales";
                series.ChartType = SeriesChartType.Line;
                series.Color = Color.FromArgb(40, 41, 34);
                series.BorderWidth = 3;
                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 8;
                series.MarkerColor = Color.FromArgb(228, 186, 94);
                series.IsValueShownAsLabel = true;
                series.LabelForeColor = Color.FromArgb(40, 41, 34);
                series.Font = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
                series.LabelFormat = "₱{0:#,0}";

                // Add data
                string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                                   "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                int[] monthlySales = { 125000, 187000, 154000, 210000, 285000, 320000,
                                      295000, 350000, 310000, 380000, 420000, 500000 };

                for (int i = 0; i < months.Length; i++)
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(months[i], monthlySales[i]);
                    series.Points.Add(point);
                }

                monthlyChart.Series.Add(series);

                // Add title
                Label monthlyTitle = new Label();
                monthlyTitle.Text = "Monthly Sales Trend";
                monthlyTitle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                monthlyTitle.TextAlign = ContentAlignment.MiddleCenter;
                monthlyTitle.Dock = DockStyle.Top;
                monthlyTitle.Height = 30;
                monthlyTitle.BackColor = Color.FromArgb(228, 186, 94);
                monthlyTitle.ForeColor = Color.FromArgb(40, 41, 34);

                monthlySale.Controls.Add(monthlyTitle);
                monthlySale.Controls.Add(monthlyChart);
            }
        }

        // Alternative: Create bar chart for monthly sales
        private void SetupMonthlyBarChart()
        {
            if (monthlySale != null)
            {
                monthlySale.Controls.Clear();

                Chart monthlyChart = new Chart();
                monthlyChart.Dock = DockStyle.Fill;
                monthlyChart.BackColor = Color.White;

                ChartArea chartArea = new ChartArea();
                chartArea.Name = "MonthlyChartArea";
                chartArea.BackColor = Color.FromArgb(250, 250, 250);
                chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
                chartArea.AxisX.Title = "Months";
                chartArea.AxisY.Title = "Sales (₱)";
                monthlyChart.ChartAreas.Add(chartArea);

                // Create bar series
                Series series = new Series();
                series.Name = "MonthlySales";
                series.ChartType = SeriesChartType.Column;
                series.Color = Color.FromArgb(40, 41, 34);
                series.IsValueShownAsLabel = true;
                series.LabelFormat = "₱{0:#,0}";

                // Add data with gradient colors
                string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                                   "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                int[] monthlySales = { 125000, 187000, 154000, 210000, 285000, 320000,
                                      295000, 350000, 310000, 380000, 420000, 500000 };

                for (int i = 0; i < months.Length; i++)
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(months[i], monthlySales[i]);

                    // Color gradient based on sales value
                    if (monthlySales[i] >= 400000)
                        point.Color = Color.FromArgb(0, 150, 0); // Dark green for high sales
                    else if (monthlySales[i] >= 300000)
                        point.Color = Color.FromArgb(228, 186, 94); // Gold for medium sales
                    else
                        point.Color = Color.FromArgb(40, 41, 34); // Dark for lower sales

                    series.Points.Add(point);
                }

                monthlyChart.Series.Add(series);

                Label monthlyTitle = new Label();
                monthlyTitle.Text = "Monthly Sales Comparison";
                monthlyTitle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                monthlyTitle.TextAlign = ContentAlignment.MiddleCenter;
                monthlyTitle.Dock = DockStyle.Top;
                monthlyTitle.Height = 30;
                monthlyTitle.BackColor = Color.FromArgb(228, 186, 94);
                monthlyTitle.ForeColor = Color.FromArgb(40, 41, 34);

                monthlySale.Controls.Add(monthlyTitle);
                monthlySale.Controls.Add(monthlyChart);
            }
        }

        // New click handler for generateReport (label4)
        private void generateReport_Click(object sender, EventArgs e)
        {
            // Show options menu
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

            // Show menu at cursor position
            reportMenu.Show(System.Windows.Forms.Cursor.Position);
        }

        // New click handler for refreshData (label10)
        private void refreshData_Click(object sender, EventArgs e)
        {
            SetupCharts();
            MessageBox.Show("Data refreshed successfully!", "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GenerateReportData()
        {
            reportLines = new List<string>();

            // Report header - Updated to MATCHPOINT GAME HUB REPORT
            reportLines.Add("MATCHPOINT GAME HUB REPORT");
            reportLines.Add("===============================");
            reportLines.Add($"Generated on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
            reportLines.Add("");
            reportLines.Add("");

            // Weekly Sales Section
            reportLines.Add("WEEKLY SALES REPORT");
            reportLines.Add("-------------------");
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int[] weeklySales = { 12500, 18700, 15400, 21000, 28500, 32000, 27500 };

            for (int i = 0; i < days.Length; i++)
            {
                reportLines.Add($"  {days[i].PadRight(12)} ₱{weeklySales[i]:#,##0}");
            }

            int weeklyTotal = weeklySales.Sum();
            reportLines.Add($"");
            reportLines.Add($"  Total Weekly Sales: ₱{weeklyTotal:#,##0}");
            reportLines.Add("");
            reportLines.Add("");

            // Monthly Sales Section
            reportLines.Add("MONTHLY SALES REPORT");
            reportLines.Add("--------------------");
            string[] months = { "January", "February", "March", "April", "May", "June",
                              "July", "August", "September", "October", "November", "December" };
            int[] monthlySales = { 125000, 187000, 154000, 210000, 285000, 320000,
                                  295000, 350000, 310000, 380000, 420000, 500000 };

            for (int i = 0; i < months.Length; i++)
            {
                reportLines.Add($"  {months[i].PadRight(12)} ₱{monthlySales[i]:#,##0}");
            }

            int monthlyTotal = monthlySales.Sum();
            reportLines.Add($"");
            reportLines.Add($"  Total Monthly Sales: ₱{monthlyTotal:#,##0}");
            reportLines.Add("");
            reportLines.Add("");

            // Summary Section
            reportLines.Add("SUMMARY");
            reportLines.Add("-------");
            reportLines.Add($"  Average Daily Sales: ₱{(weeklyTotal / 7):#,##0}");
            reportLines.Add($"  Average Monthly Sales: ₱{(monthlyTotal / 12):#,##0}");
            reportLines.Add($"  Best Performing Day: {days[Array.IndexOf(weeklySales, weeklySales.Max())]}");
            reportLines.Add($"  Best Performing Month: {months[Array.IndexOf(monthlySales, monthlySales.Max())]}");
            reportLines.Add($"  Peak Hour: 6:00 PM - 9:00 PM");
            reportLines.Add("");
            reportLines.Add("");

            // Footer
            reportLines.Add("==========================================");
            reportLines.Add("END OF REPORT");
            reportLines.Add("==========================================");
        }

        private void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            currentLineIndex = 0;
            yPos = topMargin;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            float pageHeight = e.MarginBounds.Height;
            float pageWidth = e.MarginBounds.Width;

            // Draw report title - Updated to MATCHPOINT GAME HUB REPORT
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

            // Define data
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int[] weeklySales = { 12500, 18700, 15400, 21000, 28500, 22000, 27500 };
            string[] months = { "January", "February", "March", "April", "May", "June",
                              "July", "August", "September", "October", "November", "December" };
            int[] monthlySales = { 125000, 187000, 154000, 210000, 285000, 320000,
                                  295000, 350000, 310000, 380000, 420000, 500000 };

            // Draw Weekly Sales Section
            g.DrawString("WEEKLY SALES", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            for (int i = 0; i < days.Length; i++)
            {
                if (yPos > pageHeight - 50)
                {
                    e.HasMorePages = true;
                    return;
                }
                g.DrawString($"{days[i].PadRight(15)} ₱{weeklySales[i]:#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
            }

            int weeklyTotal = weeklySales.Sum();
            g.DrawString($"Total Weekly Sales: ₱{weeklyTotal:#,##0}", headerFont, Brushes.Black, leftMargin + 20, yPos);
            yPos += 40;

            // Draw Monthly Sales Section
            g.DrawString("MONTHLY SALES", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            for (int i = 0; i < months.Length; i++)
            {
                if (yPos > pageHeight - 50)
                {
                    e.HasMorePages = true;
                    return;
                }
                g.DrawString($"{months[i].PadRight(15)} ₱{monthlySales[i]:#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
                yPos += lineHeight;
            }

            int monthlyTotal = monthlySales.Sum();
            g.DrawString($"Total Monthly Sales: ₱{monthlyTotal:#,##0}", headerFont, Brushes.Black, leftMargin + 20, yPos);
            yPos += 40;

            // Draw Summary Section
            g.DrawString("SUMMARY", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            g.DrawString($"Average Daily Sales: ₱{(weeklyTotal / 7):#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
            yPos += lineHeight;
            g.DrawString($"Average Monthly Sales: ₱{(monthlyTotal / 12):#,##0}", reportFont, Brushes.Black, leftMargin + 20, yPos);
            yPos += lineHeight;
            g.DrawString($"Best Performing Day: {days[Array.IndexOf(weeklySales, weeklySales.Max())]}", reportFont, Brushes.Black, leftMargin + 20, yPos);
            yPos += lineHeight;
            g.DrawString($"Best Performing Month: {months[Array.IndexOf(monthlySales, monthlySales.Max())]}", reportFont, Brushes.Black, leftMargin + 20, yPos);
            yPos += lineHeight;

            // Footer
            g.DrawLine(Pens.Black, leftMargin, yPos, pageWidth - leftMargin, yPos);
            yPos += 20;
            g.DrawString("END OF REPORT", reportFont, Brushes.Black,
                        (pageWidth - g.MeasureString("END OF REPORT", reportFont).Width) / 2, yPos);

            e.HasMorePages = false;
        }

        private void PrintDocument_EndPrint(object sender, PrintEventArgs e)
        {
            // Cleanup if needed
        }

        private void SaveAsTextFile()
        {
            GenerateReportData();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text Files (*.txt)|*.txt";
            saveDialog.DefaultExt = "txt";
            saveDialog.FileName = $"MatchPoint_Report_{DateTime.Now:yyyyMMdd_HHmm}.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllLines(saveDialog.FileName, reportLines);
                    MessageBox.Show($"Report saved successfully as:\n{saveDialog.FileName}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintReport()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDoc;

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

        // Optional: Add PDF export with iTextSharp (requires NuGet package)
        private void SaveAsPDF()
        {
            // This requires iTextSharp NuGet package
            // Uncomment if you install iTextSharp
            /*
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveDialog.DefaultExt = "pdf";
            saveDialog.FileName = $"MatchPoint_Report_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        
                        document.Open();
                        
                        // Add content to PDF
                        iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font headerFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font normalFont = FontFactory.GetFont("Arial", 10);
                        
                        document.Add(new Paragraph("MATCHPOINT GAME HUB REPORT", titleFont));
                        document.Add(new Paragraph($"Generated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}", normalFont));
                        document.Add(new Paragraph(" "));
                        
                        // Add your data here...
                        
                        document.Close();
                    }
                    
                    MessageBox.Show($"PDF saved successfully as:\n{saveDialog.FileName}", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving PDF: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            */
            MessageBox.Show("PDF export requires iTextSharp NuGet package. Please install it or use text file export.",
                "PDF Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Existing methods...
        private void weeklySale_Click(object sender, EventArgs e)
        {
            SetupCharts();
            MessageBox.Show("Charts refreshed!", "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panel12_Click(object sender, EventArgs e)
        {
            // This might be an old method - we'll keep it for backward compatibility
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