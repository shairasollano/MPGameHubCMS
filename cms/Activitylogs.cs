using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class Activitylogs : UserControl
    {
        // ==============================================
        // DATABASE CONFIGURATION
        // ==============================================
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";
        private MySqlConnection connection;

        // ==============================================
        // SINGLETON INSTANCE
        // ==============================================
        private static Activitylogs _instance;
        private static readonly object _lock = new object();

        public static Activitylogs Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    lock (_lock)
                    {
                        if (_instance == null || _instance.IsDisposed)
                        {
                            _instance = new Activitylogs();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Ensures the Activity Logs instance is created (for initialization)
        /// </summary>
        public static void EnsureInitialized()
        {
            var instance = Instance; // This will create it if needed
        }

        // ==============================================
        // LOG ENTRY MODEL
        // ==============================================
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
            public string Module { get; set; }
        }

        // Data storage
        private List<LogEntry> logEntries = new List<LogEntry>();
        private List<LogEntry> filteredLogs = new List<LogEntry>();

        // ==============================================
        // CONSTRUCTOR
        // ==============================================
        public Activitylogs()
        {
            InitializeComponent();
            _instance = this;

            InitializeDatabase();
            SetupControl();
            SetupEventHandlers();
            RefreshLogs();
        }

        // ==============================================
        // DATABASE INITIALIZATION
        // ==============================================
        private void InitializeDatabase()
        {
            try
            {
                CreateDatabaseIfNotExists();
                connection = new MySqlConnection(connectionString);
                CreateTablesIfNotExist();
                LoadDataFromDatabase();
            }
            catch (Exception ex)
            {
                LogError("System", "ActivityLogs", $"Database initialization error: {ex.Message}");
                MessageBox.Show($"Database initialization error: {ex.Message}\n\nUsing default sample data.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadSampleData();
            }
        }

        private void CreateDatabaseIfNotExists()
        {
            string createDbConnectionString = "Server=localhost;Uid=root;Pwd=;";
            using (MySqlConnection tempConn = new MySqlConnection(createDbConnectionString))
            {
                tempConn.Open();
                string checkDbQuery = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'matchpoint_db'";
                MySqlCommand checkCmd = new MySqlCommand(checkDbQuery, tempConn);
                object result = checkCmd.ExecuteScalar();

                if (result == null)
                {
                    string createDbQuery = "CREATE DATABASE matchpoint_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci";
                    MySqlCommand createCmd = new MySqlCommand(createDbQuery, tempConn);
                    createCmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateTablesIfNotExist()
        {
            using (connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string createLogsTable = @"
                    CREATE TABLE IF NOT EXISTS activity_logs (
                        log_id INT AUTO_INCREMENT PRIMARY KEY,
                        timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        user_id VARCHAR(50),
                        username VARCHAR(100) NOT NULL,
                        activity_type VARCHAR(100) NOT NULL,
                        description TEXT,
                        severity VARCHAR(20) DEFAULT 'Info',
                        ip_address VARCHAR(45),
                        details TEXT,
                        module VARCHAR(50) DEFAULT 'System',
                        created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
                        INDEX idx_timestamp (timestamp),
                        INDEX idx_username (username),
                        INDEX idx_activity (activity_type),
                        INDEX idx_severity (severity),
                        INDEX idx_module (module)
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";

                new MySqlCommand(createLogsTable, connection).ExecuteNonQuery();

                // Add module column if it doesn't exist (backward compatibility)
                try
                {
                    string checkColumnQuery = "SELECT module FROM activity_logs LIMIT 1";
                    new MySqlCommand(checkColumnQuery, connection).ExecuteScalar();
                }
                catch
                {
                    string addColumnQuery = "ALTER TABLE activity_logs ADD COLUMN module VARCHAR(50) DEFAULT 'System' AFTER details";
                    new MySqlCommand(addColumnQuery, connection).ExecuteNonQuery();
                }

                // Insert sample data if table is empty
                string checkCount = "SELECT COUNT(*) FROM activity_logs";
                int count = Convert.ToInt32(new MySqlCommand(checkCount, connection).ExecuteScalar());

                if (count == 0)
                {
                    InsertSampleData();
                }
            }
        }

        private void InsertSampleData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string[] users = { "admin", "john_doe", "jane_smith", "mike_jones", "sarah_williams", "david_brown" };
                    string[] activities = {
                        "Login", "Logout", "User Created", "User Updated", "User Deleted",
                        "Game Rate Added", "Game Rate Updated", "Game Rate Deleted", "Game Rate Status Changed",
                        "Equipment Added", "Equipment Updated", "Equipment Deleted", "Equipment Checked Out",
                        "Equipment Checked In", "Equipment Maintenance",
                        "Settings Updated", "Backup Created", "Database Restored", "Payment Processed", "System Error"
                    };
                    string[] severities = { "Info", "Warning", "Error", "Critical" };
                    string[] modules = { "System", "GameRates", "GameEquipment", "Users", "Payments" };
                    string[] ipAddresses = { "192.168.1.100", "192.168.1.101", "192.168.1.102", "192.168.1.103",
                                            "192.168.1.104", "192.168.1.105" };

                    Random random = new Random();

                    for (int i = 1; i <= 50; i++)
                    {
                        DateTime timestamp = DateTime.Today.AddDays(-random.Next(0, 30))
                            .AddHours(random.Next(0, 24))
                            .AddMinutes(random.Next(0, 60));

                        string user = users[random.Next(users.Length)];
                        string activity = activities[random.Next(activities.Length)];
                        string severity = severities[random.Next(severities.Length)];
                        string module = modules[random.Next(modules.Length)];
                        string ip = ipAddresses[random.Next(ipAddresses.Length)];

                        string insertQuery = @"
                            INSERT INTO activity_logs 
                            (timestamp, user_id, username, activity_type, description, severity, ip_address, details, module) 
                            VALUES (@timestamp, @userId, @username, @activity, @description, @severity, @ip, @details, @module)";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@timestamp", timestamp);
                        cmd.Parameters.AddWithValue("@userId", "USER" + (1000 + i));
                        cmd.Parameters.AddWithValue("@username", user);
                        cmd.Parameters.AddWithValue("@activity", activity);
                        cmd.Parameters.AddWithValue("@description", GetDescriptionForActivity(activity, user, module));
                        cmd.Parameters.AddWithValue("@severity", severity);
                        cmd.Parameters.AddWithValue("@ip", ip);
                        cmd.Parameters.AddWithValue("@details", $"Session: {Guid.NewGuid().ToString().Substring(0, 8)}");
                        cmd.Parameters.AddWithValue("@module", module);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting sample data: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                logEntries.Clear();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT log_id, timestamp, user_id, username, activity_type, 
                               description, severity, ip_address, details, module 
                        FROM activity_logs 
                        ORDER BY timestamp DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logEntries.Add(new LogEntry
                            {
                                LogId = Convert.ToInt32(reader["log_id"]),
                                Timestamp = Convert.ToDateTime(reader["timestamp"]),
                                UserId = reader["user_id"]?.ToString() ?? "",
                                Username = reader["username"].ToString(),
                                ActivityType = reader["activity_type"].ToString(),
                                Description = reader["description"]?.ToString() ?? "",
                                Severity = reader["severity"].ToString(),
                                IPAddress = reader["ip_address"]?.ToString() ?? "",
                                Details = reader["details"]?.ToString() ?? "",
                                Module = reader["module"]?.ToString() ?? "System"
                            });
                        }
                    }
                }

                logEntries = logEntries.OrderByDescending(l => l.Timestamp).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from database: {ex.Message}\nUsing sample data.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadSampleData();
            }
        }

        private void LoadSampleData()
        {
            logEntries.Clear();

            Random random = new Random();
            string[] users = { "admin", "john_doe", "jane_smith", "mike_jones", "sarah_williams", "david_brown" };
            string[] activities = {
                "Login", "Logout", "User Created", "User Updated", "User Deleted",
                "Game Rate Added", "Game Rate Updated", "Game Rate Deleted", "Game Rate Status Changed",
                "Equipment Added", "Equipment Updated", "Equipment Deleted", "Equipment Checked Out",
                "Equipment Checked In", "Equipment Maintenance",
                "Settings Updated", "Backup Created", "Database Restored", "Payment Processed", "System Error"
            };
            string[] severities = { "Info", "Warning", "Error", "Critical" };
            string[] modules = { "System", "GameRates", "GameEquipment", "Users", "Payments" };
            string[] ipAddresses = { "192.168.1.100", "192.168.1.101", "192.168.1.102", "192.168.1.103",
                                    "192.168.1.104", "192.168.1.105" };

            for (int i = 1; i <= 50; i++)
            {
                DateTime timestamp = DateTime.Today.AddDays(-random.Next(0, 30))
                    .AddHours(random.Next(0, 24))
                    .AddMinutes(random.Next(0, 60));

                string user = users[random.Next(users.Length)];
                string activity = activities[random.Next(activities.Length)];
                string severity = severities[random.Next(severities.Length)];
                string module = modules[random.Next(modules.Length)];
                string ip = ipAddresses[random.Next(ipAddresses.Length)];

                logEntries.Add(new LogEntry
                {
                    LogId = i,
                    Timestamp = timestamp,
                    UserId = "USER" + (1000 + i),
                    Username = user,
                    ActivityType = activity,
                    Description = GetDescriptionForActivity(activity, user, module),
                    Severity = severity,
                    IPAddress = ip,
                    Details = "IP: " + ip + ", Session: " + Guid.NewGuid().ToString().Substring(0, 8),
                    Module = module
                });
            }

            logEntries = logEntries.OrderByDescending(l => l.Timestamp).ToList();
        }

        private string GetDescriptionForActivity(string activity, string user, string module = "System")
        {
            switch (activity)
            {
                case "Login": return $"User '{user}' logged into the system";
                case "Logout": return $"User '{user}' logged out of the system";
                case "User Created": return $"New user account '{user}' was created";
                case "User Updated": return $"User '{user}' profile was updated";
                case "User Deleted": return $"User '{user}' was deleted from the system";
                case "Game Rate Added": return $"New game rate was added by '{user}'";
                case "Game Rate Updated": return $"Game rate was updated by '{user}'";
                case "Game Rate Deleted": return $"Game rate was deleted by '{user}'";
                case "Game Rate Status Changed": return $"Game rate status was changed by '{user}'";
                case "Equipment Added": return $"New equipment was added by '{user}'";
                case "Equipment Updated": return $"Equipment was updated by '{user}'";
                case "Equipment Deleted": return $"Equipment was deleted by '{user}'";
                case "Equipment Checked Out": return $"Equipment was checked out by '{user}'";
                case "Equipment Checked In": return $"Equipment was checked in by '{user}'";
                case "Equipment Maintenance": return $"Equipment maintenance was performed by '{user}'";
                case "Settings Updated": return $"System settings were updated by '{user}'";
                case "Backup Created": return $"Database backup was created by '{user}'";
                case "Database Restored": return $"Database was restored by '{user}'";
                case "Payment Processed": return $"Payment transaction processed by '{user}'";
                case "System Error": return $"System error occurred, handled by '{user}'";
                default: return $"Activity '{activity}' performed by '{user}' in {module}";
            }
        }

        // ==============================================
        // UI SETUP
        // ==============================================
        private void SetupControl()
        {
            // Set default dates
            startDatePicker.Value = DateTime.Today.AddDays(-7);
            endDatePicker.Value = DateTime.Today;

            // Setup activity filter combo
            activityCombo.Items.Clear();
            activityCombo.Items.Add("All");
            activityCombo.Items.Add("Login");
            activityCombo.Items.Add("Logout");
            activityCombo.Items.Add("User Created");
            activityCombo.Items.Add("User Updated");
            activityCombo.Items.Add("User Deleted");
            activityCombo.Items.Add("Game Rate Added");
            activityCombo.Items.Add("Game Rate Updated");
            activityCombo.Items.Add("Game Rate Deleted");
            activityCombo.Items.Add("Game Rate Status Changed");
            activityCombo.Items.Add("Equipment Added");
            activityCombo.Items.Add("Equipment Updated");
            activityCombo.Items.Add("Equipment Deleted");
            activityCombo.Items.Add("Equipment Checked Out");
            activityCombo.Items.Add("Equipment Checked In");
            activityCombo.Items.Add("Equipment Maintenance");
            activityCombo.Items.Add("Settings Updated");
            activityCombo.Items.Add("Backup Created");
            activityCombo.Items.Add("Database Restored");
            activityCombo.Items.Add("Payment Processed");
            activityCombo.Items.Add("System Error");
            activityCombo.SelectedIndex = 0;

            // Setup severity filter combo
            severityCombo.Items.Clear();
            severityCombo.Items.Add("All");
            severityCombo.Items.Add("Info");
            severityCombo.Items.Add("Warning");
            severityCombo.Items.Add("Error");
            severityCombo.Items.Add("Critical");
            severityCombo.SelectedIndex = 0;

            // Setup module filter combo
            moduleCombo.Items.Clear();
            moduleCombo.Items.Add("All");
            moduleCombo.Items.Add("System");
            moduleCombo.Items.Add("GameRates");
            moduleCombo.Items.Add("GameEquipment");
            moduleCombo.Items.Add("Users");
            moduleCombo.Items.Add("Payments");
            moduleCombo.SelectedIndex = 0;

            SetupPlaceholderText();
            deleteSelectedButton.Enabled = false;
        }

        private void SetupPlaceholderText()
        {
            // Set initial placeholder text
            userTextBox.Text = "Enter username...";
            userTextBox.ForeColor = Color.Gray;

            // Handle focus events
            userTextBox.Enter += UserTextBox_Enter;
            userTextBox.Leave += UserTextBox_Leave;

            // Make user label clickable
            userLabel.Click += UserLabel_Click;
            userLabel.Cursor = Cursors.Hand;
        }

        // Event handlers for user textbox
        private void UserTextBox_Enter(object sender, EventArgs e)
        {
            if (userTextBox.Text == "Enter username...")
            {
                userTextBox.Text = "";
                userTextBox.ForeColor = Color.FromArgb(70, 70, 70);
            }
        }

        private void UserTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userTextBox.Text))
            {
                userTextBox.Text = "Enter username...";
                userTextBox.ForeColor = Color.Gray;
            }
        }

        private void UserLabel_Click(object sender, EventArgs e)
        {
            userTextBox.Focus();
            if (userTextBox.Text == "Enter username...")
            {
                userTextBox.Text = "";
                userTextBox.ForeColor = Color.FromArgb(70, 70, 70);
            }
            userTextBox.SelectAll();
        }

        private void SetupEventHandlers()
        {
            refreshButton.Click += refreshButton_Click;
            searchButton.Click += searchButton_Click;
            clearButton.Click += clearButton_Click;
            deleteSelectedButton.Click += deleteSelectedButton_Click;
            deleteAllButton.Click += deleteAllButton_Click;
            exportButton.Click += exportButton_Click;

            startDatePicker.ValueChanged += FilterChanged;
            endDatePicker.ValueChanged += FilterChanged;
            activityCombo.SelectedIndexChanged += FilterChanged;
            severityCombo.SelectedIndexChanged += FilterChanged;
            moduleCombo.SelectedIndexChanged += FilterChanged;

            logsDataGridView.CellFormatting += logsDataGridView_CellFormatting;
            logsDataGridView.SelectionChanged += logsDataGridView_SelectionChanged;
            logsDataGridView.CellDoubleClick += logsDataGridView_CellDoubleClick;
        }

        // ==============================================
        // FILTERING AND DISPLAY
        // ==============================================
        private void RefreshLogs()
        {
            ApplyFilters();
            UpdateStatistics();
            UpdateDataGridView();
        }

        private void ApplyFilters()
        {
            filteredLogs = logEntries.ToList();

            DateTime startDate = startDatePicker.Value.Date;
            DateTime endDate = endDatePicker.Value.Date.AddDays(1).AddSeconds(-1);
            filteredLogs = filteredLogs.Where(log => log.Timestamp >= startDate && log.Timestamp <= endDate).ToList();

            string searchText = userTextBox.Text;
            if (searchText != "Enter username..." && !string.IsNullOrWhiteSpace(searchText))
            {
                string searchTerm = searchText.ToLower();
                filteredLogs = filteredLogs.Where(log =>
                    log.Username.ToLower().Contains(searchTerm) ||
                    log.UserId.ToLower().Contains(searchTerm)).ToList();
            }

            if (activityCombo.SelectedIndex > 0)
            {
                string selectedActivity = activityCombo.SelectedItem.ToString();
                filteredLogs = filteredLogs.Where(log => log.ActivityType == selectedActivity).ToList();
            }

            if (severityCombo.SelectedIndex > 0)
            {
                string selectedSeverity = severityCombo.SelectedItem.ToString();
                filteredLogs = filteredLogs.Where(log => log.Severity == selectedSeverity).ToList();
            }

            if (moduleCombo.SelectedIndex > 0)
            {
                string selectedModule = moduleCombo.SelectedItem.ToString();
                filteredLogs = filteredLogs.Where(log => log.Module == selectedModule).ToList();
            }

            filteredLogs = filteredLogs.OrderByDescending(l => l.Timestamp).ToList();
        }

        private void UpdateStatistics()
        {
            DateTime today = DateTime.Today;

            totalLogsValue.Text = filteredLogs.Count.ToString();
            todayLogsValue.Text = filteredLogs.Count(l => l.Timestamp.Date == today).ToString();
            distinctUsersValue.Text = filteredLogs.Select(l => l.Username).Distinct().Count().ToString();
            activeTodayValue.Text = filteredLogs
                .Where(l => l.Timestamp.Date == today && l.ActivityType == "Login")
                .Select(l => l.Username)
                .Distinct()
                .Count().ToString();

            infoValue.Text = filteredLogs.Count(l => l.Severity == "Info").ToString();
            warningValue.Text = filteredLogs.Count(l => l.Severity == "Warning").ToString();
            errorValue.Text = filteredLogs.Count(l => l.Severity == "Error").ToString();
            criticalValue.Text = filteredLogs.Count(l => l.Severity == "Critical").ToString();

            gameRateChangesValue.Text = filteredLogs.Count(l =>
                l.Module == "GameRates" &&
                (l.ActivityType.Contains("Added") || l.ActivityType.Contains("Updated") ||
                 l.ActivityType.Contains("Deleted") || l.ActivityType.Contains("Status"))).ToString();

            equipmentChangesValue.Text = filteredLogs.Count(l =>
                l.Module == "GameEquipment" &&
                (l.ActivityType.Contains("Added") || l.ActivityType.Contains("Updated") ||
                 l.ActivityType.Contains("Deleted") || l.ActivityType.Contains("Checked"))).ToString();
        }

        private void UpdateDataGridView()
        {
            logsDataGridView.Rows.Clear();

            foreach (var log in filteredLogs)
            {
                string icon = GetSeverityIcon(log.Severity);
                string formattedTimestamp = log.Timestamp.ToString("MM/dd/yyyy HH:mm:ss");
                string description = log.Description.Length > 50 ? log.Description.Substring(0, 47) + "..." : log.Description;

                int rowIndex = logsDataGridView.Rows.Add(
                    icon, formattedTimestamp, log.Username, log.ActivityType,
                    description, log.Severity, log.Module
                );
                logsDataGridView.Rows[rowIndex].Tag = log.LogId;
            }

            titleLabel.Text = $"Activity Logs ({filteredLogs.Count} records)";
            logsDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            if (logsDataGridView.Columns["descriptionColumn"] != null)
                logsDataGridView.Columns["descriptionColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private string GetSeverityIcon(string severity)
        {
            switch (severity)
            {
                case "Info": return "ℹ️";
                case "Warning": return "⚠️";
                case "Error": return "❌";
                case "Critical": return "🔥";
                default: return "📝";
            }
        }

        private Color GetSeverityColor(string severity)
        {
            switch (severity)
            {
                case "Info": return Color.FromArgb(0, 123, 255);
                case "Warning": return Color.FromArgb(255, 193, 7);
                case "Error": return Color.FromArgb(220, 53, 69);
                case "Critical": return Color.FromArgb(156, 39, 176);
                default: return Color.FromArgb(108, 117, 125);
            }
        }

        // ==============================================
        // LOGGING METHODS (PUBLIC API)
        // ==============================================

        public void LogGameRateActivity(string username, string action, string rateName, string details = "")
        {
            string activityType = $"Game Rate {action}";
            string description = $"Game rate '{rateName}' was {action.ToLower()} by '{username}'";
            if (!string.IsNullOrEmpty(details))
                description += $" - {details}";

            AddLogEntry(username, activityType, description, "Info", "GameRates");
        }

        public void LogEquipmentActivity(string username, string action, string equipmentName, string details = "")
        {
            string activityType = $"Equipment {action}";
            string description = $"Equipment '{equipmentName}' was {action.ToLower()} by '{username}'";
            if (!string.IsNullOrEmpty(details))
                description += $" - {details}";

            AddLogEntry(username, activityType, description, "Info", "GameEquipment");
        }

        public void LogEquipmentCheckout(string username, string equipmentName, int quantity, string checkedOutBy)
        {
            string description = $"{quantity}x '{equipmentName}' checked out to {checkedOutBy} by '{username}'";
            AddLogEntry(username, "Equipment Checked Out", description, "Info", "GameEquipment");
        }

        public void LogEquipmentCheckin(string username, string equipmentName, int quantity, string returnedBy)
        {
            string description = $"{quantity}x '{equipmentName}' checked in from {returnedBy} by '{username}'";
            AddLogEntry(username, "Equipment Checked In", description, "Info", "GameEquipment");
        }

        public void LogEquipmentMaintenance(string username, string equipmentName, string condition)
        {
            string description = $"Maintenance completed for '{equipmentName}', new condition: {condition}";
            AddLogEntry(username, "Equipment Maintenance", description, "Info", "GameEquipment");
        }

        public void LogWarning(string username, string module, string warning)
        {
            AddLogEntry(username, "Warning", warning, "Warning", module);
        }

        public void LogError(string username, string module, string error, string details = "")
        {
            string description = error;
            if (!string.IsNullOrEmpty(details))
                description += $" - {details}";

            AddLogEntry(username, "System Error", description, "Error", module);
        }

        public void AddLogEntry(string username, string activityType, string description, string severity = "Info", string module = "System")
        {
            int newLogId = logEntries.Count > 0 ? logEntries.Max(l => l.LogId) + 1 : 1;

            LogEntry newLog = new LogEntry
            {
                LogId = newLogId,
                Timestamp = DateTime.Now,
                UserId = GetUserIdFromUsername(username),
                Username = username,
                ActivityType = activityType,
                Description = description,
                Severity = severity,
                IPAddress = GetLocalIPAddress(),
                Details = $"Module: {module}",
                Module = module
            };

            AddLogToDatabase(newLog);
            logEntries.Insert(0, newLog);

            if (this.Visible)
            {
                RefreshLogs();
            }
        }

        private void AddLogToDatabase(LogEntry log)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string insertQuery = @"
                        INSERT INTO activity_logs 
                        (timestamp, user_id, username, activity_type, description, severity, ip_address, details, module) 
                        VALUES (@timestamp, @userId, @username, @activity, @description, @severity, @ip, @details, @module);
                        SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@timestamp", log.Timestamp);
                    cmd.Parameters.AddWithValue("@userId", log.UserId);
                    cmd.Parameters.AddWithValue("@username", log.Username);
                    cmd.Parameters.AddWithValue("@activity", log.ActivityType);
                    cmd.Parameters.AddWithValue("@description", log.Description);
                    cmd.Parameters.AddWithValue("@severity", log.Severity);
                    cmd.Parameters.AddWithValue("@ip", log.IPAddress);
                    cmd.Parameters.AddWithValue("@details", log.Details);
                    cmd.Parameters.AddWithValue("@module", log.Module ?? "System");

                    log.LogId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding log to database: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetUserIdFromUsername(string username)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id FROM users WHERE username = @username LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        return result.ToString();
                }
            }
            catch { }

            return "USER" + (1000 + Math.Abs(username.GetHashCode()) % 1000);
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        return ip.ToString();
                }
            }
            catch { }
            return "127.0.0.1";
        }

        // ==============================================
        // QUERY METHODS
        // ==============================================

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

        public List<LogEntry> GetModuleLogs(string module, int days = 7)
        {
            DateTime cutoffDate = DateTime.Today.AddDays(-days);
            return logEntries
                .Where(l => l.Module == module && l.Timestamp >= cutoffDate)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
        }

        public void ClearOldLogs(int daysToKeep = 30)
        {
            DateTime cutoffDate = DateTime.Today.AddDays(-daysToKeep);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM activity_logs WHERE timestamp < @cutoffDate";
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                    cmd.Parameters.AddWithValue("@cutoffDate", cutoffDate);
                    int deletedCount = cmd.ExecuteNonQuery();

                    LoadDataFromDatabase();

                    if (this.Visible)
                    {
                        RefreshLogs();
                        MessageBox.Show($"Cleared {deletedCount} log entries older than {daysToKeep} days.",
                                      "Cleanup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing old logs: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData()
        {
            LoadDataFromDatabase();
            RefreshLogs();
        }

        // ==============================================
        // EVENT HANDLERS
        // ==============================================

        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshData();
            MessageBox.Show("Logs refreshed successfully!", "Refresh",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            startDatePicker.Value = DateTime.Today.AddDays(-7);
            endDatePicker.Value = DateTime.Today;
            userTextBox.Text = "Enter username...";
            userTextBox.ForeColor = Color.Gray;
            activityCombo.SelectedIndex = 0;
            severityCombo.SelectedIndex = 0;
            moduleCombo.SelectedIndex = 0;
            RefreshLogs();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void deleteSelectedButton_Click(object sender, EventArgs e)
        {
            if (logsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select log entries to delete.", "No Selection",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete {logsDataGridView.SelectedRows.Count} selected log entries?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                List<int> logIdsToDelete = new List<int>();
                foreach (DataGridViewRow row in logsDataGridView.SelectedRows)
                {
                    if (row.Tag != null && row.Tag is int logId)
                        logIdsToDelete.Add(logId);
                }

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        foreach (int logId in logIdsToDelete)
                        {
                            string deleteQuery = "DELETE FROM activity_logs WHERE log_id = @logId";
                            MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                            cmd.Parameters.AddWithValue("@logId", logId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadDataFromDatabase();
                    RefreshLogs();

                    MessageBox.Show($"{logIdsToDelete.Count} log entries deleted successfully!", "Delete Complete",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting logs: {ex.Message}", "Database Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deleteAllButton_Click(object sender, EventArgs e)
        {
            if (filteredLogs.Count == 0)
            {
                MessageBox.Show("No log entries to delete.", "Empty",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"WARNING: This will delete ALL {filteredLogs.Count} log entries in the current view.\n\nThis action cannot be undone!",
                "Confirm Delete All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        StringBuilder whereClause = new StringBuilder("WHERE 1=1");
                        DateTime startDate = startDatePicker.Value.Date;
                        DateTime endDate = endDatePicker.Value.Date.AddDays(1).AddSeconds(-1);
                        whereClause.Append($" AND timestamp >= '{startDate:yyyy-MM-dd HH:mm:ss}' AND timestamp <= '{endDate:yyyy-MM-dd HH:mm:ss}'");

                        string searchText = userTextBox.Text;
                        if (searchText != "Enter username..." && !string.IsNullOrWhiteSpace(searchText))
                        {
                            string escapedSearch = MySqlHelper.EscapeString(searchText);
                            whereClause.Append($" AND (username LIKE '%{escapedSearch}%' OR user_id LIKE '%{escapedSearch}%')");
                        }

                        if (activityCombo.SelectedIndex > 0)
                            whereClause.Append($" AND activity_type = '{activityCombo.SelectedItem}'");

                        if (severityCombo.SelectedIndex > 0)
                            whereClause.Append($" AND severity = '{severityCombo.SelectedItem}'");

                        if (moduleCombo.SelectedIndex > 0)
                            whereClause.Append($" AND module = '{moduleCombo.SelectedItem}'");

                        string deleteQuery = $"DELETE FROM activity_logs {whereClause}";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        int deletedCount = cmd.ExecuteNonQuery();

                        LoadDataFromDatabase();
                        RefreshLogs();

                        MessageBox.Show($"{deletedCount} log entries deleted successfully!", "Delete Complete",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting logs: {ex.Message}", "Database Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (filteredLogs.Count == 0)
            {
                MessageBox.Show("No log entries to export.", "Empty",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = "ActivityLogs_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                Title = "Export Logs"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder csv = new StringBuilder();
                    csv.AppendLine("Timestamp,User ID,Username,Activity Type,Description,Severity,IP Address,Details,Module");

                    foreach (var log in filteredLogs)
                    {
                        csv.AppendLine($"\"{log.Timestamp:yyyy-MM-dd HH:mm:ss}\"," +
                                      $"\"{log.UserId}\"," +
                                      $"\"{log.Username}\"," +
                                      $"\"{log.ActivityType}\"," +
                                      $"\"{log.Description.Replace("\"", "\"\"")}\"," +
                                      $"\"{log.Severity}\"," +
                                      $"\"{log.IPAddress}\"," +
                                      $"\"{log.Details}\"," +
                                      $"\"{log.Module}\"");
                    }

                    File.WriteAllText(saveFileDialog.FileName, csv.ToString());

                    MessageBox.Show($"Logs exported successfully to:\n{saveFileDialog.FileName}",
                                  "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting logs: {ex.Message}", "Export Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void logsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (logsDataGridView.Columns[e.ColumnIndex].Name == "severityColumn" &&
                    logsDataGridView.Rows[e.RowIndex].Cells["severityColumn"].Value != null)
                {
                    string severity = logsDataGridView.Rows[e.RowIndex].Cells["severityColumn"].Value.ToString();
                    e.CellStyle.ForeColor = GetSeverityColor(severity);
                    e.CellStyle.Font = new Font(logsDataGridView.Font, FontStyle.Bold);
                }

                if (logsDataGridView.Columns[e.ColumnIndex].Name == "iconColumn")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.Font = new Font("Segoe UI Emoji", 12);
                }
            }
        }

        private void logsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            deleteSelectedButton.Enabled = logsDataGridView.SelectedRows.Count > 0;
        }

        private void logsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = logsDataGridView.Rows[e.RowIndex];
                if (row.Tag != null && row.Tag is int logId)
                {
                    var log = logEntries.FirstOrDefault(l => l.LogId == logId);
                    if (log != null)
                    {
                        ShowLogDetailsDialog(log);
                    }
                }
            }
        }

        private void ShowLogDetailsDialog(LogEntry log)
        {
            Form detailsForm = new Form();
            detailsForm.Text = "Log Details";
            detailsForm.Size = new Size(600, 500);
            detailsForm.StartPosition = FormStartPosition.CenterParent;
            detailsForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            detailsForm.MaximizeBox = false;
            detailsForm.MinimizeBox = false;
            detailsForm.BackColor = Color.White;

            RichTextBox rtbDetails = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                ReadOnly = true,
                BackColor = Color.White
            };

            StringBuilder details = new StringBuilder();
            details.AppendLine("═══════════════════════════════════════════════════════════");
            details.AppendLine($"  LOG DETAILS - ID: {log.LogId}");
            details.AppendLine("═══════════════════════════════════════════════════════════");
            details.AppendLine();
            details.AppendLine($"  Timestamp:     {log.Timestamp:yyyy-MM-dd HH:mm:ss}");
            details.AppendLine($"  User ID:       {log.UserId}");
            details.AppendLine($"  Username:      {log.Username}");
            details.AppendLine($"  Activity Type: {log.ActivityType}");
            details.AppendLine($"  Module:        {log.Module}");
            details.AppendLine($"  Severity:      {log.Severity}");
            details.AppendLine($"  IP Address:    {log.IPAddress}");
            details.AppendLine();
            details.AppendLine($"  Description:");
            details.AppendLine($"  {log.Description}");
            details.AppendLine();
            details.AppendLine($"  Details:");
            details.AppendLine($"  {log.Details}");
            details.AppendLine();
            details.AppendLine("═══════════════════════════════════════════════════════════");

            rtbDetails.Text = details.ToString();

            Button btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(detailsForm.ClientSize.Width - 120, detailsForm.ClientSize.Height - 50),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, ev) => detailsForm.Close();

            detailsForm.Controls.Add(rtbDetails);
            detailsForm.Controls.Add(btnClose);

            detailsForm.ShowDialog(this);
        }
    }
}