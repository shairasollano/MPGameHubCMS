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
            if (weeklySale != null && weeklySale.Controls.Count > 0)
            {
                weeklySale.Controls.Clear();
            }

            TableLayoutPanel weeklyPanel = new TableLayoutPanel();
            weeklyPanel.Dock = DockStyle.Fill;
            weeklyPanel.BackColor = Color.White;
            weeklyPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int[] sales = { 12500, 18700, 15400, 21000, 28500, 32000, 27500 };

            weeklyPanel.RowCount = days.Length + 1;
            weeklyPanel.ColumnCount = 2;

            Label header1 = new Label();
            header1.Text = "Day";
            header1.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            header1.TextAlign = ContentAlignment.MiddleCenter;
            header1.BackColor = Color.FromArgb(40, 41, 34);
            header1.ForeColor = Color.White;
            weeklyPanel.Controls.Add(header1, 0, 0);

            Label header2 = new Label();
            header2.Text = "Sales (₱)";
            header2.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            header2.TextAlign = ContentAlignment.MiddleCenter;
            header2.BackColor = Color.FromArgb(40, 41, 34);
            header2.ForeColor = Color.White;
            weeklyPanel.Controls.Add(header2, 1, 0);

            for (int i = 0; i < days.Length; i++)
            {
                Label dayLabel = new Label();
                dayLabel.Text = days[i];
                dayLabel.Font = new System.Drawing.Font("Arial", 9);
                dayLabel.TextAlign = ContentAlignment.MiddleLeft;
                dayLabel.Padding = new Padding(5, 0, 0, 0);
                weeklyPanel.Controls.Add(dayLabel, 0, i + 1);

                Label salesLabel = new Label();
                salesLabel.Text = $"₱{sales[i]:#,##0}";
                salesLabel.Font = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
                salesLabel.TextAlign = ContentAlignment.MiddleRight;
                salesLabel.Padding = new Padding(0, 0, 5, 0);
                weeklyPanel.Controls.Add(salesLabel, 1, i + 1);
            }

            Label weeklyTitle = new Label();
            weeklyTitle.Text = "Weekly Sales Report";
            weeklyTitle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            weeklyTitle.TextAlign = ContentAlignment.MiddleCenter;
            weeklyTitle.Dock = DockStyle.Top;
            weeklyTitle.Height = 30;
            weeklyTitle.BackColor = Color.FromArgb(40, 41, 34);
            weeklyTitle.ForeColor = Color.FromArgb(228, 186, 94);

            weeklySale.Controls.Add(weeklyTitle);
            weeklySale.Controls.Add(weeklyPanel);
            weeklyPanel.Dock = DockStyle.Fill;
        }

        private void SetupMonthlyChart()
        {
            if (monthlySale != null && monthlySale.Controls.Count > 0)
            {
                monthlySale.Controls.Clear();
            }

            TableLayoutPanel monthlyPanel = new TableLayoutPanel();
            monthlyPanel.Dock = DockStyle.Fill;
            monthlyPanel.BackColor = Color.White;
            monthlyPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            int[] monthlySales = { 125000, 187000, 154000, 210000, 285000, 320000, 295000, 350000, 310000, 380000, 420000, 500000 };

            monthlyPanel.RowCount = months.Length + 1;
            monthlyPanel.ColumnCount = 2;

            Label header1 = new Label();
            header1.Text = "Month";
            header1.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            header1.TextAlign = ContentAlignment.MiddleCenter;
            header1.BackColor = Color.FromArgb(228, 186, 94);
            header1.ForeColor = Color.FromArgb(40, 41, 34);
            monthlyPanel.Controls.Add(header1, 0, 0);

            Label header2 = new Label();
            header2.Text = "Sales (₱)";
            header2.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            header2.TextAlign = ContentAlignment.MiddleCenter;
            header2.BackColor = Color.FromArgb(228, 186, 94);
            header2.ForeColor = Color.FromArgb(40, 41, 34);
            monthlyPanel.Controls.Add(header2, 1, 0);

            for (int i = 0; i < months.Length; i++)
            {
                Label monthLabel = new Label();
                monthLabel.Text = months[i];
                monthLabel.Font = new System.Drawing.Font("Arial", 9);
                monthLabel.TextAlign = ContentAlignment.MiddleLeft;
                monthLabel.Padding = new Padding(5, 0, 0, 0);
                monthlyPanel.Controls.Add(monthLabel, 0, i + 1);

                Label salesLabel = new Label();
                salesLabel.Text = $"₱{monthlySales[i]:#,##0}";
                salesLabel.Font = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
                salesLabel.TextAlign = ContentAlignment.MiddleRight;
                salesLabel.Padding = new Padding(0, 0, 5, 0);
                monthlyPanel.Controls.Add(salesLabel, 1, i + 1);
            }

            Label monthlyTitle = new Label();
            monthlyTitle.Text = "Monthly Sales Report";
            monthlyTitle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            monthlyTitle.TextAlign = ContentAlignment.MiddleCenter;
            monthlyTitle.Dock = DockStyle.Top;
            monthlyTitle.Height = 30;
            monthlyTitle.BackColor = Color.FromArgb(228, 186, 94);
            monthlyTitle.ForeColor = Color.FromArgb(40, 41, 34);

            monthlySale.Controls.Add(monthlyTitle);
            monthlySale.Controls.Add(monthlyPanel);
            monthlyPanel.Dock = DockStyle.Fill;
        }

        private void GenerateReportData()
        {
            reportLines = new List<string>();

            // Report header
            reportLines.Add("SPORTS FACILITY SALES REPORT");
            reportLines.Add("=============================");
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

            // Draw report title
            g.DrawString("SPORTS FACILITY SALES REPORT", titleFont, Brushes.Black,
                        (pageWidth - g.MeasureString("SPORTS FACILITY SALES REPORT", titleFont).Width) / 2, yPos);
            yPos += 40;

            // Draw generation date
            g.DrawString($"Generated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}", reportFont, Brushes.Black, leftMargin, yPos);
            yPos += 30;

            // Draw separator line
            g.DrawLine(Pens.Black, leftMargin, yPos, pageWidth - leftMargin, yPos);
            yPos += 20;

            // Define data
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int[] weeklySales = { 12500, 18700, 15400, 21000, 28500, 32000, 27500 };
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
            saveDialog.FileName = $"Sales_Report_{DateTime.Now:yyyyMMdd_HHmm}.txt";

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

        // Updated Generate Report button click handler
        private void panel11_Click(object sender, EventArgs e)
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

            // Show menu below the button
            if (sender is Panel panel)
            {
                reportMenu.Show(panel, new Point(0, panel.Height));
            }
            else
            {
                reportMenu.Show(Cursor.Position);
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
            saveDialog.FileName = $"Sales_Report_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

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
                        
                        document.Add(new Paragraph("SPORTS FACILITY SALES REPORT", titleFont));
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