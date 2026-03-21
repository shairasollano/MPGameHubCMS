using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class SALES : UserControl
    {
        public SALES()
        {
            InitializeComponent();
            LoadWeeks();
            LoadMonths();
            cmbFilterType.SelectedIndex = 0;
            this.Size = new Size(1684, 939);
        }

        private void LoadWeeks()
        {
            cmbWeek.Items.Clear();

            // Get current year
            int year = dtpYear.Value.Year;

            // Track current month
            string currentMonth = "";
            int weekCounter = 1;

            // Generate weeks 1-52 with month names
            for (int weekNum = 1; weekNum <= 52; weekNum++)
            {
                DateTime weekStart = GetStartDateOfWeek(year, weekNum);
                string monthName = weekStart.ToString("MMMM");

                // Reset week counter when month changes
                if (monthName != currentMonth)
                {
                    currentMonth = monthName;
                    weekCounter = 1;
                }

                // Format: "January, Week 1"
                string weekDisplay = $"{monthName}, Week {weekCounter}";
                cmbWeek.Items.Add(weekDisplay);

                weekCounter++;
            }

            // Select current week
            int currentWeek = GetCurrentWeek();
            if (currentWeek <= cmbWeek.Items.Count)
            {
                cmbWeek.SelectedIndex = currentWeek - 1;
            }
        }

        private void LoadMonths()
        {
            cmbMonth.Items.Clear();
            string[] months = {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };
            cmbMonth.Items.AddRange(months);
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
        }

        private int GetCurrentWeek()
        {
            System.Globalization.CultureInfo ciCurr = System.Globalization.CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        private DateTime GetStartDateOfWeek(int year, int weekNumber)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)jan1.DayOfWeek - (int)DayOfWeek.Monday;

            if (daysOffset < 0)
                daysOffset += 7;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            if (weekNumber == 1)
                return firstMonday;

            return firstMonday.AddDays((weekNumber - 1) * 7);
        }

        private void CmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isWeekSelected = cmbFilterType.SelectedItem.ToString() == "Select Week";
            cmbWeek.Visible = isWeekSelected;
            lblWeek.Visible = isWeekSelected;
            cmbMonth.Visible = !isWeekSelected;
            lblMonth.Visible = !isWeekSelected;
        }

        private void BtnLoadSales_Click(object sender, EventArgs e)
        {
            try
            {
                int year = dtpYear.Value.Year;

                if (cmbFilterType.SelectedItem.ToString() == "Select Week")
                {
                    if (cmbWeek.SelectedIndex >= 0)
                    {
                        int weekNumber = cmbWeek.SelectedIndex + 1;
                        LoadSalesByWeek(year, weekNumber);
                    }
                    else
                    {
                        MessageBox.Show("Please select a week.", "No Selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    int month = cmbMonth.SelectedIndex + 1;
                    LoadSalesByMonth(year, month);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sales: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSalesByWeek(int year, int weekNumber)
        {
            DateTime startOfWeek = GetStartDateOfWeek(year, weekNumber);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            DataTable salesData = GetDailySalesSummary(startOfWeek, endOfWeek);
            dgvSales.DataSource = salesData;

            // Get the selected week display text
            string weekDisplay = cmbWeek.SelectedItem.ToString();
            lblFilterInfo.Text = $"Showing daily sales summary for {weekDisplay}, {year}";
            FormatDataGridView();
            CalculateAndDisplayTotals(salesData);
        }

        private void LoadSalesByMonth(int year, int month)
        {
            DateTime startOfMonth = new DateTime(year, month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            DataTable salesData = GetDailySalesSummary(startOfMonth, endOfMonth);
            dgvSales.DataSource = salesData;

            lblFilterInfo.Text = $"Showing daily sales summary for {startOfMonth:MMMM yyyy}";
            FormatDataGridView();
            CalculateAndDisplayTotals(salesData);
        }

        private DataTable GetDailySalesSummary(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();

            // Define columns for summary view
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Day", typeof(string));
            dt.Columns.Add("Number of Transactions", typeof(int));
            dt.Columns.Add("Total Revenue", typeof(decimal));
            dt.Columns.Add("Average Transaction", typeof(decimal));
            dt.Columns.Add("Top Game", typeof(string));
            dt.Columns.Add("Payment Methods", typeof(string));

            // Games available
            string[] games = { "Billiards", "Table Tennis", "Badminton", "Scooter" };
            string[] paymentMethods = { "Cash", "GCash" };

            Random rand = new Random();
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                // Generate number of transactions for the day
                int numberOfTransactions = rand.Next(50, 200);

                // Generate total revenue based on number of transactions and average spend
                decimal averageSpend = rand.Next(50, 300);
                decimal totalRevenue = numberOfTransactions * averageSpend;

                // Determine top game for the day (most played game)
                string topGame = games[rand.Next(games.Length)];

                // Payment method distribution (Cash vs GCash)
                int cashPercentage = rand.Next(40, 80);
                int gcashPercentage = 100 - cashPercentage;
                string paymentDistribution = $"Cash: {cashPercentage}%, GCash: {gcashPercentage}%";

                dt.Rows.Add(
                    currentDate,
                    currentDate.ToString("dddd"),
                    numberOfTransactions,
                    totalRevenue,
                    averageSpend,
                    topGame,
                    paymentDistribution
                );

                currentDate = currentDate.AddDays(1);
            }

            return dt;
        }

        private void CalculateAndDisplayTotals(DataTable salesData)
        {
            if (salesData.Rows.Count == 0) return;

            int totalTransactions = 0;
            decimal totalRevenue = 0;

            foreach (DataRow row in salesData.Rows)
            {
                totalTransactions += Convert.ToInt32(row["Number of Transactions"]);
                totalRevenue += Convert.ToDecimal(row["Total Revenue"]);
            }

            decimal averageDailyRevenue = totalRevenue / salesData.Rows.Count;
            int averageDailyTransactions = totalTransactions / salesData.Rows.Count;

            string summaryText = $"SUMMARY: Total Transactions: {totalTransactions:N0} | Total Revenue: ₱{totalRevenue:N2} | " +
                                $"Avg Daily Transactions: {averageDailyTransactions:N0} | Avg Daily Revenue: ₱{averageDailyRevenue:N2}";

            lblFilterInfo.Text = summaryText;
        }

        private void FormatDataGridView()
        {
            if (dgvSales.Columns.Count == 0) return;

            // Set column widths
            dgvSales.Columns["Date"].Width = 120;
            dgvSales.Columns["Date"].DefaultCellStyle.Format = "MMM dd, yyyy";
            dgvSales.Columns["Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvSales.Columns["Day"].Width = 100;
            dgvSales.Columns["Day"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvSales.Columns["Number of Transactions"].Width = 150;
            dgvSales.Columns["Number of Transactions"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSales.Columns["Number of Transactions"].DefaultCellStyle.Format = "N0";

            dgvSales.Columns["Total Revenue"].Width = 150;
            dgvSales.Columns["Total Revenue"].DefaultCellStyle.Format = "₱#,##0.00";
            dgvSales.Columns["Total Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvSales.Columns["Average Transaction"].Width = 150;
            dgvSales.Columns["Average Transaction"].DefaultCellStyle.Format = "₱#,##0.00";
            dgvSales.Columns["Average Transaction"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvSales.Columns["Top Game"].Width = 120;
            dgvSales.Columns["Top Game"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvSales.Columns["Payment Methods"].Width = 150;
            dgvSales.Columns["Payment Methods"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set alternating row colors for better readability
            dgvSales.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            // Set header style
            dgvSales.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvSales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvSales.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Enable auto-resize for remaining space
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Make sure the last column fills the remaining space
            dgvSales.Columns[dgvSales.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void dtpYear_ValueChanged(object sender, EventArgs e)
        {
            // Reload weeks when year changes
            LoadWeeks();
        }

        private void SALES_Load(object sender, EventArgs e)
        {
            // Auto-load current week's data when form loads
            int currentWeek = GetCurrentWeek();
            int currentYear = DateTime.Now.Year;
            LoadSalesByWeek(currentYear, currentWeek);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click if needed
        }
    }
}