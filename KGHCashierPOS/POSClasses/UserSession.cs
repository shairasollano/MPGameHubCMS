using System;

namespace KGHCashierPOS
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string FullName { get; set; }
        public static int RoleId { get; set; }
        public static string RoleName { get; set; }
        public static DateTime LoginTime { get; set; }

        // Role checks for convenience
        public static bool IsSuperAdmin => RoleId == 1;
        public static bool IsAdmin => RoleId == 2;
        public static bool IsCashier => RoleId == 3;
        public static bool IsCustomer => RoleId == 4;

        // Clear session on logout
        public static void Clear()
        {
            UserId = 0;
            Username = null;
            FullName = null;
            RoleId = 0;
            RoleName = null;
            LoginTime = DateTime.MinValue;
        }

        // Check if user has access to a feature
        public static bool HasAccess(params int[] allowedRoles)
        {
            foreach (int roleId in allowedRoles)
            {
                if (RoleId == roleId)
                    return true;
            }
            return false;
        }
    }
}