using MySql.Data.MySqlClient;
using System;

namespace KGHCashierPOS
{
    public static class AuthenticationRepository
    {
        // ============ LOGIN USER ============
        public static LoginResult AuthenticateUser(string username, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    u.user_id,
                    u.username,
                    u.password,
                    u.full_name,
                    u.role_id,
                    r.role_name,
                    u.is_active
                FROM users u
                INNER JOIN roles r ON u.role_id = r.role_id
                WHERE u.username = @username";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Check if active
                                bool isActive = reader.GetBoolean("is_active");
                                if (!isActive)
                                {
                                    // ⭐ Log failed login BEFORE returning
                                    reader.Close();
                                    LogFailedLogin(username, "Account disabled");

                                    return new LoginResult
                                    {
                                        Success = false,
                                        Message = "Account is disabled. Please contact administrator."
                                    };
                                }

                                // Verify password
                                string storedPassword = reader.GetString("password");

                                if (password != storedPassword)
                                {
                                    // ⭐ Close reader before logging
                                    reader.Close();
                                    LogFailedLogin(username, "Invalid password");

                                    return new LoginResult
                                    {
                                        Success = false,
                                        Message = "Invalid username or password"
                                    };
                                }

                                // ⭐ SUCCESSFUL LOGIN - Store user info
                                int userId = reader.GetInt32("user_id");
                                string userName = reader.GetString("username");
                                string fullName = reader.IsDBNull(reader.GetOrdinal("full_name"))
                                    ? userName
                                    : reader.GetString("full_name");
                                int roleId = reader.GetInt32("role_id");
                                string roleName = reader.GetString("role_name");

                                // ⭐ Close reader BEFORE any database operations
                                reader.Close();

                                // ⭐ Now populate session
                                UserSession.UserId = userId;
                                UserSession.Username = userName;
                                UserSession.FullName = fullName;
                                UserSession.RoleId = roleId;
                                UserSession.RoleName = roleName;
                                UserSession.LoginTime = DateTime.Now;

                                // ⭐ Update last login (single transaction)
                                UpdateLastLogin(userId);

                                // ⭐ Log successful login ONCE (single transaction)
                                LogSuccessfulLogin(userId, userName);

                                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                                System.Diagnostics.Debug.WriteLine($"✓ Login logged for {userName}");
                                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");

                                return new LoginResult
                                {
                                    Success = true,
                                    Message = "Login successful",
                                    UserId = userId,
                                    Username = userName,
                                    RoleId = roleId,
                                    RoleName = roleName
                                };
                            }
                            else
                            {
                                // ⭐ User not found - close reader first
                                reader.Close();
                                LogFailedLogin(username, "User not found");

                                return new LoginResult
                                {
                                    Success = false,
                                    Message = "Invalid username or password"
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Authentication Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                return new LoginResult
                {
                    Success = false,
                    Message = "Database connection error. Please try again."
                };
            }
        }

        // ============ UPDATE LAST LOGIN ============
        private static void UpdateLastLogin(int userId)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = "UPDATE users SET last_login = NOW() WHERE user_id = @userId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        System.Diagnostics.Debug.WriteLine($"  ✓ Last login updated: {rowsAffected} row(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"  ❌ Update last login error: {ex.Message}");
            }
        }

        // ============ LOG SUCCESSFUL LOGIN ============
        private static void LogSuccessfulLogin(int userId, string username)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                INSERT INTO login_logs 
                (user_id, username, login_time, login_status)
                VALUES 
                (@userId, @username, NOW(), 'Success')";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@username", username);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        System.Diagnostics.Debug.WriteLine($"  ✓ Login logged: {rowsAffected} row(s) inserted");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"  ❌ Log success error: {ex.Message}");
            }
        }


        // ============ LOG FAILED LOGIN ============
        private static void LogFailedLogin(string username, string reason)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                INSERT INTO login_logs 
                (username, login_time, login_status, failure_reason)
                VALUES 
                (@username, NOW(), 'Failed', @reason)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@reason", reason);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        System.Diagnostics.Debug.WriteLine($"  ✓ Failed login logged: {rowsAffected} row(s) inserted");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"  ❌ Log failure error: {ex.Message}");
            }
        }

        // ============ LOGOUT USER ============
        public static void LogoutUser()
        {
            System.Diagnostics.Debug.WriteLine($"User {UserSession.Username} logged out");
            UserSession.Clear();
        }
    }

    // ============ LOGIN RESULT CLASS ============
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}