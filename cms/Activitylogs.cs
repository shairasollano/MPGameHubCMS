using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cms
{
    public partial class Activitylogs : UserControl
    {
        // Log entry class
        public class LogEntry
        {
            public int LogId { get; set; }
            public DateTime Timestamp { get; set; }
            public string UserId { get; set; }
            public string Username { get; set; }
            public string ActivityType { get; set; }
            public string Description { get; set; }
            public string Severity { get; set; }
            public string IPAddress { get; set; }
            public string Details { get; set; }

            public Color SeverityColor
            {
                get
                {
                    switch (Severity)
                    {
                        case "Info": return Color.FromArgb(0, 123, 255);
                        case "Warning": return Color.FromArgb(255, 193, 7);
                        case "Error": return Color.FromArgb(220, 53, 69);
                        case "Critical": return Color.FromArgb(220, 53, 69);
                        default: return Color.FromArgb(108, 117, 125);
                    }
                }
            }

            public string SeverityIcon
            {
                get
                {
                    switch (Severity)
                    {
                        case "Info": return "ℹ️";
                        case "Warning": return "⚠️";
                        case "Error": return "❌";
                        case "Critical": return "🔥";
                        default: return "📝";
                    }
                }
            }
        }

        // Sample data (in real app, this would come from database)
        private List<LogEntry> logEntries = new List<LogEntry>();
        private List<LogEntry> filteredLogs = new List<LogEntry>();

        // Statistics
        private int totalLogs = 0;
        private int todayLogs = 0;
        private int distinctUsers = 0;
        private int activeToday = 0;
        private int infoCount = 0;
        private int warningCount = 0;
        private int errorCount = 0;
        private int criticalCount = 0;

        public Activitylogs()
        {
            InitializeComponent();

            // Set up the control
            SetupControl();

            // Load sample data
            LoadSampleData();

            // Set up event handlers
            SetupEventHandlers();

            // Initial load
            RefreshLogs();
        }

        private void SetupControl()
        {
            // Configure DataGridView
            ConfigureDataGridView();

            // Set default dates
            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpEndDate.Value = DateTime.Today;

            // Set default filter values
            cmbActivityType.SelectedIndex = 0;
            cmbSeverity.SelectedIndex = 0;

            // Apply styling
            ApplyStyling();

            // Set placeholder text manually (since PlaceholderText property doesn't exist in older .NET)
            SetupPlaceholderText();
        }

        private void SetupPlaceholderText()
        {
            // Set initial placeholder text for search user
            txtSearchUser.Text = "Enter username...";
            txtSearchUser.ForeColor = Color.Gray;

            // Handle focus events
            txtSearchUser.Enter += TxtSearchUser_Enter;
            txtSearchUser.Leave += TxtSearchUser_Leave;

            // Make label3 clickable to focus on search box
            label3.Click += Label3_Click;
            label3.Cursor = Cursors.Hand;
            label3.MouseEnter += Label3_MouseEnter;
            label3.MouseLeave += Label3_MouseLeave;
        }

        private void TxtSearchUser_Enter(object sender, EventArgs e)
        {
            if (txtSearchUser.Text == "Enter username...")
            {
                txtSearchUser.Text = "";
                txtSearchUser.ForeColor = Color.FromArgb(70, 70, 70);
            }
        }

        private void TxtSearchUser_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchUser.Text))
            {
                txtSearchUser.Text = "Enter username...";
                txtSearchUser.ForeColor = Color.Gray;
            }
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            // When label3 is clicked, focus on the search textbox
            txtSearchUser.Focus();

            // If it has placeholder text, clear it and set proper color
            if (txtSearchUser.Text == "Enter username...")
            {
                txtSearchUser.Text = "";
                txtSearchUser.ForeColor = Color.FromArgb(70, 70, 70);
            }

            // Optional: Highlight the text for easy editing
            txtSearchUser.SelectAll();
        }

        private void Label3_MouseEnter(object sender, EventArgs e)
        {
            // Change color on hover to indicate it's clickable
            label3.ForeColor = Color.FromArgb(0, 123, 255); // Blue color
            label3.Font = new System.Drawing.Font(label3.Font, FontStyle.Underline);
        }

        private void Label3_MouseLeave(object sender, EventArgs e)
        {
            // Restore original color
            label3.ForeColor = Color.FromArgb(70, 70, 70);
            label3.Font = new System.Drawing.Font(label3.Font, FontStyle.Regular);
        }

        private void ConfigureDataGridView()
        {
            // Clear existing columns
            dataGridViewLogs.Columns.Clear();

            // Create columns
            DataGridViewTextBoxColumn colIcon = new DataGridViewTextBoxColumn
            {
                HeaderText = "",
                Name = "Icon",
                Width = 40,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            DataGridViewTextBoxColumn colTimestamp = new DataGridViewTextBoxColumn
            {
                HeaderText = "Timestamp",
                Name = "Timestamp",
                Width = 150,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "MM/dd/yyyy HH:mm",
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            };

            DataGridViewTextBoxColumn colUser = new DataGridViewTextBoxColumn
            {
                HeaderText = "User",
                Name = "User",
                Width = 120,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn colActivity = new DataGridViewTextBoxColumn
            {
                HeaderText = "Activity",
                Name = "Activity",
                Width = 150,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn colDescription = new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                Name = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn colSeverity = new DataGridViewTextBoxColumn
            {
                HeaderText = "Severity",
                Name = "Severity",
                Width = 100,
                ReadOnly = true
            };

            // Add columns
            dataGridViewLogs.Columns.AddRange(colIcon, colTimestamp, colUser, colActivity, colDescription, colSeverity);
        }

        private void ApplyStyling()
        {
            // Set colors to match your theme
            this.BackColor = Color.FromArgb(250, 250, 250);

            // Header styling
            panelHeader.BackColor = Color.FromArgb(40, 41, 34);
            labelTitle.ForeColor = Color.FromArgb(228, 186, 94);

            // Button colors
            btnRefresh.BackColor = Color.FromArgb(70, 130, 180);
            btnDeleteSelected.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteAll.BackColor = Color.FromArgb(220, 53, 69);
            btnGenReport.BackColor = Color.FromArgb(46, 125, 50);
            
            btnSearch.BackColor = Color.FromArgb(70, 130, 180);
            btnClearFilters.BackColor = Color.FromArgb(220, 220, 220);

            // Set all button text to white
            foreach (Control control in panelActions.Controls)
            {
                if (control is Button button)
                {
                    button.ForeColor = Color.White;
                }
            }

            btnClearFilters.ForeColor = Color.FromArgb(70, 70, 70);
        }

        private void SetupEventHandlers()
        {
            // Button click events
            btnRefresh.Click += BtnRefresh_Click;
            btnSearch.Click += BtnSearch_Click;
            btnClearFilters.Click += BtnClearFilters_Click;
            btnDeleteSelected.Click += BtnDeleteSelected_Click;
            btnDeleteAll.Click += BtnDeleteAll_Click;
            btnGenReport.Click += BtnExportExcel_Click;
            

            // Date picker events
            dtpStartDate.ValueChanged += FilterChanged;
            dtpEndDate.ValueChanged += FilterChanged;

            // Combo box events
            cmbActivityType.SelectedIndexChanged += FilterChanged;
            cmbSeverity.SelectedIndexChanged += FilterChanged;

            // Text box events (already set in SetupControl)

            // DataGridView events
            dataGridViewLogs.CellFormatting += DataGridViewLogs_CellFormatting;
            dataGridViewLogs.SelectionChanged += DataGridViewLogs_SelectionChanged;
        }

        private void LoadSampleData()
        {
            // Clear existing data
            logEntries.Clear();

            // Generate sample log entries
            Random random = new Random();
            string[] users = { "admin", "john_doe", "jane_smith", "mike_jones", "sarah_williams", "david_brown" };
            string[] activities = { "Login", "Logout", "User Created", "User Updated", "User Deleted",
                                   "Game Rate Changed", "Settings Updated", "Backup Created",
                                   "Database Restored", "Payment Processed", "System Error" };
            string[] severities = { "Info", "Warning", "Error", "Critical" };
            string[] ipAddresses = { "192.168.1.100", "192.168.1.101", "192.168.1.102", "192.168.1.103",
                                    "192.168.1.104", "192.168.1.105" };

            // Generate 50 sample log entries
            for (int i = 1; i <= 50; i++)
            {
                DateTime timestamp = DateTime.Today.AddDays(-random.Next(0, 30))
                    .AddHours(random.Next(0, 24))
                    .AddMinutes(random.Next(0, 60));

                string user = users[random.Next(users.Length)];
                string activity = activities[random.Next(activities.Length)];
                string severity = severities[random.Next(severities.Length)];
                string ip = ipAddresses[random.Next(ipAddresses.Length)];

                logEntries.Add(new LogEntry
                {
                    LogId = i,
                    Timestamp = timestamp,
                    UserId = "USER" + (1000 + i),
                    Username = user,
                    ActivityType = activity,
                    Description = GetDescriptionForActivity(activity, user),
                    Severity = severity,
                    IPAddress = ip,
                    Details = "IP: " + ip + ", Session: " + Guid.NewGuid().ToString().Substring(0, 8)
                });
            }

            // Sort by timestamp (newest first)
            logEntries = logEntries.OrderByDescending(l => l.Timestamp).ToList();
        }

        private string GetDescriptionForActivity(string activity, string user)
        {
            switch (activity)
            {
                case "Login": return "User '" + user + "' logged into the system";
                case "Logout": return "User '" + user + "' logged out of the system";
                case "User Created": return "New user '" + user + "' was created";
                case "User Updated": return "User '" + user + "' profile was updated";
                case "User Deleted": return "User '" + user + "' was deleted from the system";
                case "Game Rate Changed": return "Game rates were updated by '" + user + "'";
                case "Settings Updated": return "System settings were updated by '" + user + "'";
                case "Backup Created": return "Database backup was created by '" + user + "'";
                case "Database Restored": return "Database was restored by '" + user + "'";
                case "Payment Processed": return "Payment transaction processed by '" + user + "'";
                case "System Error": return "System error occurred, handled by '" + user + "'";
                default: return "Activity '" + activity + "' performed by '" + user + "'";
            }
        }

        private void RefreshLogs()
        {
            ApplyFilters();
            UpdateStatistics();
            UpdateDataGridView();
        }

        private void ApplyFilters()
        {
            filteredLogs = logEntries.ToList();

            // Apply date filter
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);

            filteredLogs = filteredLogs.Where(log =>
                log.Timestamp >= startDate && log.Timestamp <= endDate).ToList();

            // Apply user filter
            string searchText = txtSearchUser.Text;
            if (searchText != "Enter username..." && !string.IsNullOrWhiteSpace(searchText))
            {
                string searchTerm = searchText.ToLower();
                filteredLogs = filteredLogs.Where(log =>
                    log.Username.ToLower().Contains(searchTerm) ||
                    log.UserId.ToLower().Contains(searchTerm)).ToList();
            }

            // Apply activity type filter
            if (cmbActivityType.SelectedIndex > 0)
            {
                string selectedActivity = cmbActivityType.Text;
                filteredLogs = filteredLogs.Where(log => log.ActivityType == selectedActivity).ToList();
            }

            // Apply severity filter
            if (cmbSeverity.SelectedIndex > 0)
            {
                string selectedSeverity = cmbSeverity.Text;
                filteredLogs = filteredLogs.Where(log => log.Severity == selectedSeverity).ToList();
            }

            // Sort by timestamp (newest first)
            filteredLogs = filteredLogs.OrderByDescending(l => l.Timestamp).ToList();
        }

        private void UpdateStatistics()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            // Calculate statistics
            totalLogs = filteredLogs.Count;
            todayLogs = filteredLogs.Count(l => l.Timestamp.Date == today);

            var distinctUserList = filteredLogs.Select(l => l.Username).Distinct().ToList();
            distinctUsers = distinctUserList.Count;

            activeToday = filteredLogs
                .Where(l => l.Timestamp.Date == today && l.ActivityType == "Login")
                .Select(l => l.Username)
                .Distinct()
                .Count();

            infoCount = filteredLogs.Count(l => l.Severity == "Info");
            warningCount = filteredLogs.Count(l => l.Severity == "Warning");
            errorCount = filteredLogs.Count(l => l.Severity == "Error");
            criticalCount = filteredLogs.Count(l => l.Severity == "Critical");

            // Update labels
            lblTotalLogs.Text = totalLogs.ToString();
            lblTodayLogs.Text = todayLogs.ToString();
            lblUserCount.Text = distinctUsers.ToString();
            lblActiveToday.Text = activeToday.ToString();
            lblInfoCount.Text = infoCount.ToString();
            lblWarningCount.Text = warningCount.ToString();
            lblErrorCount.Text = errorCount.ToString();
            lblCriticalCount.Text = criticalCount.ToString();
        }

        private void UpdateDataGridView()
        {
            // Clear existing rows
            dataGridViewLogs.Rows.Clear();

            // Add filtered logs to DataGridView
            foreach (var log in filteredLogs)
            {
                int rowIndex = dataGridViewLogs.Rows.Add(
                    log.SeverityIcon,
                    log.Timestamp,
                    log.Username,
                    log.ActivityType,
                    log.Description,
                    log.Severity
                );

                // Store the LogId in the row's Tag property
                dataGridViewLogs.Rows[rowIndex].Tag = log.LogId;
            }

            // Update status
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            // You can add a status bar label if needed
            // For now, we'll just update the title with count
            labelTitle.Text = "Activity Logs (" + filteredLogs.Count + " records)";
        }

        // ==============================================
        // EVENT HANDLERS
        // ==============================================

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshLogs();
            MessageBox.Show("Logs refreshed successfully!", "Refresh",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            // Reset filters to defaults
            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpEndDate.Value = DateTime.Today;
            txtSearchUser.Text = "Enter username...";
            txtSearchUser.ForeColor = Color.Gray;
            cmbActivityType.SelectedIndex = 0;
            cmbSeverity.SelectedIndex = 0;

            RefreshLogs();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            // Auto-refresh when filters change
            RefreshLogs();
        }

        private void BtnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (dataGridViewLogs.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select log entries to delete.", "No Selection",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete " + dataGridViewLogs.SelectedRows.Count + " selected log entries?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Get selected log IDs
                List<int> logIdsToDelete = new List<int>();
                foreach (DataGridViewRow row in dataGridViewLogs.SelectedRows)
                {
                    if (row.Tag != null && row.Tag is int logId)
                    {
                        logIdsToDelete.Add(logId);
                    }
                }

                // Remove from data source
                logEntries.RemoveAll(log => logIdsToDelete.Contains(log.LogId));

                // Refresh display
                RefreshLogs();

                MessageBox.Show(logIdsToDelete.Count + " log entries deleted successfully!", "Delete Complete",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            if (filteredLogs.Count == 0)
            {
                MessageBox.Show("No log entries to delete.", "Empty",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "WARNING: This will delete ALL " + filteredLogs.Count + " log entries in the current view.\n\nThis action cannot be undone!",
                "Confirm Delete All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Get all log IDs in current view
                List<int> logIdsToDelete = filteredLogs.Select(log => log.LogId).ToList();

                // Remove from data source
                logEntries.RemoveAll(log => logIdsToDelete.Contains(log.LogId));

                // Refresh display
                RefreshLogs();

                MessageBox.Show("All " + logIdsToDelete.Count + " log entries deleted successfully!", "Delete Complete",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (filteredLogs.Count == 0)
            {
                MessageBox.Show("No log entries to export.", "Empty",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                FileName = "ActivityLogs_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx",
                Title = "Export to Excel"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // In a real application, you would use a library like EPPlus or ClosedXML
                    // For this example, we'll create a simple CSV file
                    StringBuilder csv = new StringBuilder();

                    // Add headers
                    csv.AppendLine("Timestamp,User ID,Username,Activity Type,Description,Severity,IP Address,Details");

                    // Add data
                    foreach (var log in filteredLogs)
                    {
                        csv.AppendLine("\"" + log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                                      "\"" + log.UserId + "\"," +
                                      "\"" + log.Username + "\"," +
                                      "\"" + log.ActivityType + "\"," +
                                      "\"" + log.Description + "\"," +
                                      "\"" + log.Severity + "\"," +
                                      "\"" + log.IPAddress + "\"," +
                                      "\"" + log.Details + "\"");
                    }

                    string fileName = saveFileDialog.FileName;
                    if (fileName.EndsWith(".xlsx"))
                    {
                        fileName = fileName.Replace(".xlsx", ".csv");
                    }

                    File.WriteAllText(fileName, csv.ToString());

                    MessageBox.Show("Logs exported successfully to:\n" + fileName,
                                  "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting logs: " + ex.Message, "Export Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExportPDF_Click(object sender, EventArgs e)
        {
            if (filteredLogs.Count == 0)
            {
                MessageBox.Show("No log entries to export.", "Empty",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                FileName = "ActivityLogs_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf",
                Title = "Export to PDF"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // In a real application, you would use a PDF library like iTextSharp or PDFSharp
                    // For this example, we'll create a simple text file
                    StringBuilder pdfContent = new StringBuilder();

                    // Add header
                    pdfContent.AppendLine("ACTIVITY LOGS REPORT");
                    pdfContent.AppendLine("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    pdfContent.AppendLine("Date Range: " + dtpStartDate.Value.ToString("yyyy-MM-dd") + " to " + dtpEndDate.Value.ToString("yyyy-MM-dd"));
                    pdfContent.AppendLine("Total Records: " + filteredLogs.Count);
                    pdfContent.AppendLine("".PadRight(80, '='));
                    pdfContent.AppendLine();

                    // Add data
                    foreach (var log in filteredLogs)
                    {
                        pdfContent.AppendLine("[" + log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "] " + log.Severity.ToUpper());
                        pdfContent.AppendLine("User: " + log.Username + " (" + log.UserId + ")");
                        pdfContent.AppendLine("Activity: " + log.ActivityType);
                        pdfContent.AppendLine("Description: " + log.Description);
                        pdfContent.AppendLine("IP Address: " + log.IPAddress);
                        pdfContent.AppendLine("".PadRight(80, '-'));
                        pdfContent.AppendLine();
                    }

                    string fileName = saveFileDialog.FileName;
                    if (fileName.EndsWith(".pdf"))
                    {
                        fileName = fileName.Replace(".pdf", ".txt");
                    }

                    File.WriteAllText(fileName, pdfContent.ToString());

                    MessageBox.Show("Logs exported successfully to:\n" + fileName,
                                  "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting logs: " + ex.Message, "Export Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DataGridViewLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridViewLogs.Rows[e.RowIndex];

                // Format severity column with colors
                if (dataGridViewLogs.Columns[e.ColumnIndex].Name == "Severity")
                {
                    string severity = row.Cells["Severity"].Value != null ? row.Cells["Severity"].Value.ToString() : "";

                    switch (severity)
                    {
                        case "Info":
                            e.CellStyle.ForeColor = Color.FromArgb(0, 123, 255);
                            e.CellStyle.Font = new System.Drawing.Font(dataGridViewLogs.Font, FontStyle.Bold);
                            break;
                        case "Warning":
                            e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                            e.CellStyle.Font = new System.Drawing.Font(dataGridViewLogs.Font, FontStyle.Bold);
                            break;
                        case "Error":
                            e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                            e.CellStyle.Font = new System.Drawing.Font(dataGridViewLogs.Font, FontStyle.Bold);
                            break;
                        case "Critical":
                            e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                            e.CellStyle.Font = new System.Drawing.Font(dataGridViewLogs.Font, FontStyle.Bold);
                            e.CellStyle.BackColor = Color.FromArgb(255, 240, 240);
                            break;
                    }
                }

                // Format icon column with emojis
                if (dataGridViewLogs.Columns[e.ColumnIndex].Name == "Icon")
                {
                    string severity = row.Cells["Severity"].Value != null ? row.Cells["Severity"].Value.ToString() : "";
                    string icon = "";

                    switch (severity)
                    {
                        case "Info": icon = "ℹ️"; break;
                        case "Warning": icon = "⚠️"; break;
                        case "Error": icon = "❌"; break;
                        case "Critical": icon = "🔥"; break;
                        default: icon = "📝"; break;
                    }

                    e.Value = icon;
                    e.CellStyle.Font = new System.Drawing.Font("Segoe UI Emoji", 12);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void DataGridViewLogs_SelectionChanged(object sender, EventArgs e)
        {
            // Update button states based on selection
            btnDeleteSelected.Enabled = dataGridViewLogs.SelectedRows.Count > 0;
        }

        // ==============================================
        // PUBLIC METHODS (for other parts of application to use)
        // ==============================================

        public void AddLogEntry(string username, string activityType, string description, string severity = "Info")
        {
            int newLogId = 1;
            if (logEntries.Count > 0)
            {
                newLogId = logEntries.Max(l => l.LogId) + 1;
            }

            LogEntry newLog = new LogEntry
            {
                LogId = newLogId,
                Timestamp = DateTime.Now,
                UserId = GetUserIdFromUsername(username),
                Username = username,
                ActivityType = activityType,
                Description = description,
                Severity = severity,
                IPAddress = "192.168.1.100", // In real app, get actual IP
                Details = "Automated log entry"
            };

            logEntries.Insert(0, newLog); // Add at beginning (newest first)

            // If this control is currently visible, refresh the display
            if (this.Visible)
            {
                RefreshLogs();
            }
        }

        private string GetUserIdFromUsername(string username)
        {
            // In a real app, this would look up the user ID from database
            // For now, return a placeholder
            return "USER" + (1000 + Math.Abs(username.GetHashCode()) % 1000);
        }

        public List<LogEntry> GetLogsForUser(string username, int days = 7)
        {
            DateTime cutoffDate = DateTime.Today.AddDays(-days);
            return logEntries
                .Where(l => l.Username == username && l.Timestamp >= cutoffDate)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
        }

        public List<LogEntry> GetErrorLogs(int limit = 50)
        {
            return logEntries
                .Where(l => l.Severity == "Error" || l.Severity == "Critical")
                .OrderByDescending(l => l.Timestamp)
                .Take(limit)
                .ToList();
        }

        public void ClearOldLogs(int daysToKeep = 30)
        {
            DateTime cutoffDate = DateTime.Today.AddDays(-daysToKeep);
            int oldCount = logEntries.RemoveAll(l => l.Timestamp < cutoffDate);

            if (this.Visible)
            {
                RefreshLogs();
                MessageBox.Show("Cleared " + oldCount + " log entries older than " + daysToKeep + " days.",
                              "Cleanup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Refresh method for Form1 to call
        public void Refresh()
        {
            RefreshLogs();
        }

        private void groupBoxStatistics_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridViewLogs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnGenReport_Click(object sender, EventArgs e)
        {

        }
    }
}