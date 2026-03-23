using System;
using MySql.Data.MySqlClient;

namespace KGHCashierPOS
{
    public static class GlobalLogger
    {
        private static string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";

        public static string CurrentUsername { get; set; } = "System";
        public static string CurrentUserRole { get; set; } = "System";
        public static string CurrentUserId { get; set; } = "0";

        public static void LogInfo(string module, string description)
        {
            LogToDatabase("Information", description, "Info", module);
        }

        public static void LogError(string module, string error)
        {
            LogToDatabase("System Error", error, "Error", module);
        }

        public static void LogEquipmentCheckout(string equipmentName, int quantity, string checkedOutBy)
        {
            string description = $"{quantity}x '{equipmentName}' checked out to {checkedOutBy} by '{CurrentUsername}'";
            LogToDatabase("Equipment Checked Out", description, "Info", "GameEquipment");
        }

        public static void LogEquipmentCheckin(string equipmentName, int quantity, string returnedBy)
        {
            string description = $"{quantity}x '{equipmentName}' checked in from {returnedBy} by '{CurrentUsername}'";
            LogToDatabase("Equipment Checked In", description, "Info", "GameEquipment");
        }

        public static void LogEquipmentMaintenance(string equipmentName, string condition)
        {
            string description = $"Maintenance completed for '{equipmentName}', new condition: {condition}";
            LogToDatabase("Equipment Maintenance", description, "Info", "GameEquipment");
        }

        private static void LogToDatabase(string activityType, string description, string severity, string module)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    conn.ChangeDatabase("matchpoint_db");

                    string query = @"
                        INSERT INTO activity_logs 
                        (timestamp, user_id, username, activity_type, description, severity, module, ip_address, details) 
                        VALUES (@timestamp, @userId, @username, @activity, @description, @severity, @module, @ip, @details)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                        cmd.Parameters.AddWithValue("@userId", CurrentUserId);
                        cmd.Parameters.AddWithValue("@username", CurrentUsername);
                        cmd.Parameters.AddWithValue("@activity", activityType);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@severity", severity);
                        cmd.Parameters.AddWithValue("@module", module);
                        cmd.Parameters.AddWithValue("@ip", GetLocalIPAddress());
                        cmd.Parameters.AddWithValue("@details", $"Module: {module}");
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logging failed: {ex.Message}");
            }
        }

        private static string GetLocalIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return ip.ToString();
                }
            }
            catch { }
            return "127.0.0.1";
        }
    }
}