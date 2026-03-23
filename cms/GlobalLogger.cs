using System;

namespace cms
{
    /// <summary>
    /// Global logger for easy access from any form or control
    /// </summary>
    public static class GlobalLogger
    {
        // Store the current username globally
        public static string CurrentUsername { get; set; } = "System";
        public static string CurrentUserRole { get; set; } = "System";
        public static string CurrentUserId { get; set; } = "0";

        // Safe logging method that doesn't throw exceptions
        public static void Log(string activityType, string description,
                               string severity = "Info", string module = "System")
        {
            try
            {
                if (Activitylogs.Instance != null && !Activitylogs.Instance.IsDisposed)
                {
                    Activitylogs.Instance.AddLogEntry(
                        CurrentUsername,
                        activityType,
                        description,
                        severity,
                        module
                    );
                }
            }
            catch (Exception ex)
            {
                // Silent fail - don't crash the app if logging fails
                System.Diagnostics.Debug.WriteLine($"Logging failed: {ex.Message}");
            }
        }

        // Convenience methods
        public static void LogInfo(string module, string description)
        {
            Log("Information", description, "Info", module);
        }

        public static void LogWarning(string module, string warning)
        {
            Log("Warning", warning, "Warning", module);
        }

        // FIXED: LogError with 3 parameters (error, details combined)
        public static void LogError(string module, string error)
        {
            Log("System Error", error, "Error", module);
        }

        // Overload with details
        public static void LogError(string module, string error, string details)
        {
            string fullError = string.IsNullOrEmpty(details) ? error : $"{error} - {details}";
            Log("System Error", fullError, "Error", module);
        }

        // Game Rate specific
        public static void LogGameRate(string action, string rateName, string details = "")
        {
            try
            {
                Activitylogs.Instance?.LogGameRateActivity(CurrentUsername, action, rateName, details);
            }
            catch { }
        }

        // Equipment specific
        public static void LogEquipment(string action, string equipmentName, string details = "")
        {
            try
            {
                Activitylogs.Instance?.LogEquipmentActivity(CurrentUsername, action, equipmentName, details);
            }
            catch { }
        }

        public static void LogEquipmentCheckout(string equipmentName, int quantity, string checkedOutBy)
        {
            try
            {
                Activitylogs.Instance?.LogEquipmentCheckout(CurrentUsername, equipmentName, quantity, checkedOutBy);
            }
            catch { }
        }

        public static void LogEquipmentCheckin(string equipmentName, int quantity, string returnedBy)
        {
            try
            {
                Activitylogs.Instance?.LogEquipmentCheckin(CurrentUsername, equipmentName, quantity, returnedBy);
            }
            catch { }
        }

        public static void LogEquipmentMaintenance(string equipmentName, string condition)
        {
            try
            {
                Activitylogs.Instance?.LogEquipmentMaintenance(CurrentUsername, equipmentName, condition);
            }
            catch { }
        }
    }
}