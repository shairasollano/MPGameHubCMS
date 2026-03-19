using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class GameEquipment : UserControl
    {
        // Modern color scheme - make them static readonly
        private static readonly Color primaryColor = Color.FromArgb(67, 97, 238);
        private static readonly Color successColor = Color.FromArgb(76, 175, 80);
        private static readonly Color dangerColor = Color.FromArgb(244, 67, 54);
        private static readonly Color warningColor = Color.FromArgb(255, 152, 0);
        private static readonly Color infoColor = Color.FromArgb(0, 150, 255);
        private static readonly Color cardBgColor = Color.White;
        private static readonly Color hoverColor = Color.FromArgb(245, 247, 250);

        // Current user (you should set this from your login system)
        private string currentUser = "admin";

        // Database connection
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";
        private MySqlConnection connection;

        // Data lists
        private List<EquipmentItem> equipmentList;
        private List<Category> categoryList;
        private List<Condition> conditionList;
        private List<CheckoutLog> checkoutLogs;

        // Model classes
        private class EquipmentItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public int TotalQuantity { get; set; }
            public int AvailableQuantity { get; set; }
            public int CheckedOutQuantity => TotalQuantity - AvailableQuantity;
            public string Condition { get; set; }
            public string Location { get; set; }
            public string Notes { get; set; }
            public bool NeedsMaintenance { get; set; }
            public DateTime LastMaintenance { get; set; }
            public string ImagePath { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

            // Computed properties
            public bool IsAvailable => AvailableQuantity > 0;
            public string AvailabilityStatus => IsAvailable ? "Available" : "Out of Stock";
            public Color StatusColor => IsAvailable ? successColor : dangerColor;
        }

        private class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int ItemCount { get; set; }
            public string Icon { get; set; }
        }

        private class Condition
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
            public string Description { get; set; }
        }

        private class CheckoutLog
        {
            public int Id { get; set; }
            public int EquipmentId { get; set; }
            public string EquipmentName { get; set; }
            public int Quantity { get; set; }
            public string CheckedOutBy { get; set; }
            public DateTime CheckOutDate { get; set; }
            public DateTime? ActualReturnDate { get; set; }
            public string Notes { get; set; }
            public string Status { get; set; } // "Checked Out", "Returned"
            public string Duration
            {
                get
                {
                    if (ActualReturnDate.HasValue)
                    {
                        TimeSpan diff = ActualReturnDate.Value - CheckOutDate;
                        return $"{(int)diff.TotalHours}h {diff.Minutes}m";
                    }
                    else
                    {
                        TimeSpan diff = DateTime.Now - CheckOutDate;
                        return $"{(int)diff.TotalHours}h {diff.Minutes}m (ongoing)";
                    }
                }
            }
        }

        // Helper class for sample logs
        private class SampleLog
        {
            public int eqId { get; set; }
            public string eqName { get; set; }
            public int qty { get; set; }
            public string by { get; set; }
            public DateTime outDate { get; set; }
            public object returnDate { get; set; }
            public string status { get; set; }
        }

        public GameEquipment()
        {
            InitializeComponent();

            // Initialize lists
            equipmentList = new List<EquipmentItem>();
            categoryList = new List<Category>();
            conditionList = new List<Condition>();
            checkoutLogs = new List<CheckoutLog>();

            // Initialize database
            InitializeDatabase();

            StyleButtons();
            DisplayEquipment();
            LoadCategories();
            LoadConditions();

            // Setup filter
            filterCombo.SelectedIndexChanged += FilterCombo_SelectedIndexChanged;
            filterCombo.SelectedIndex = 0;

            // Hide management overlay initially
            managementOverlay.Visible = false;

            // Log that GameEquipment module was opened
            Activitylogs.Instance.AddLogEntry(currentUser, "Module Opened", "Game Equipment management module was opened", "Info", "GameEquipment");
        }

        private void InitializeDatabase()
        {
            try
            {
                // First, ensure database exists
                CreateDatabaseIfNotExists();

                // Initialize connection
                connection = new MySqlConnection(connectionString);

                // Create tables if they don't exist
                CreateTablesIfNotExist();

                // Load data from database
                LoadDataFromDatabase();
            }
            catch (Exception ex)
            {
                Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, "Database initialization error");
                MessageBox.Show($"Database initialization error: {ex.Message}\n\nUsing default sample data.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InitializeSampleData();
            }
        }

        private void CreateDatabaseIfNotExists()
        {
            string createDbConnectionString = "Server=localhost;Uid=root;Pwd=;";
            using (MySqlConnection tempConn = new MySqlConnection(createDbConnectionString))
            {
                tempConn.Open();

                // Check if database exists
                string checkDbQuery = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'matchpoint_db'";
                MySqlCommand checkCmd = new MySqlCommand(checkDbQuery, tempConn);
                object result = checkCmd.ExecuteScalar();

                if (result == null)
                {
                    // Create database
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

                // Create equipment_categories table
                string createCategoriesTable = @"
                    CREATE TABLE IF NOT EXISTS equipment_categories (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(100) NOT NULL UNIQUE,
                        description TEXT,
                        icon VARCHAR(10) DEFAULT '📦',
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                new MySqlCommand(createCategoriesTable, connection).ExecuteNonQuery();

                // Create equipment_conditions table
                string createConditionsTable = @"
                    CREATE TABLE IF NOT EXISTS equipment_conditions (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(50) NOT NULL UNIQUE,
                        color VARCHAR(20) DEFAULT '#808080',
                        description TEXT,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                new MySqlCommand(createConditionsTable, connection).ExecuteNonQuery();

                // Create equipment_items table
                string createEquipmentTable = @"
                    CREATE TABLE IF NOT EXISTS equipment_items (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(200) NOT NULL,
                        category VARCHAR(100) NOT NULL,
                        total_quantity INT NOT NULL DEFAULT 1,
                        available_quantity INT NOT NULL DEFAULT 1,
                        condition_name VARCHAR(50) NOT NULL,
                        location VARCHAR(100),
                        notes TEXT,
                        needs_maintenance BOOLEAN DEFAULT FALSE,
                        last_maintenance DATE,
                        image_path VARCHAR(500),
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        FOREIGN KEY (category) REFERENCES equipment_categories(name) ON DELETE RESTRICT,
                        FOREIGN KEY (condition_name) REFERENCES equipment_conditions(name) ON DELETE RESTRICT,
                        INDEX idx_category (category),
                        INDEX idx_condition (condition_name),
                        INDEX idx_available (available_quantity)
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                new MySqlCommand(createEquipmentTable, connection).ExecuteNonQuery();

                // Create equipment_checkout_logs table
                string createCheckoutTable = @"
                    CREATE TABLE IF NOT EXISTS equipment_checkout_logs (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        equipment_id INT NOT NULL,
                        equipment_name VARCHAR(200) NOT NULL,
                        quantity INT NOT NULL,
                        checked_out_by VARCHAR(100) NOT NULL,
                        check_out_date DATETIME NOT NULL,
                        actual_return_date DATETIME,
                        notes TEXT,
                        status VARCHAR(20) DEFAULT 'Checked Out',
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (equipment_id) REFERENCES equipment_items(id) ON DELETE CASCADE,
                        INDEX idx_equipment (equipment_id),
                        INDEX idx_status (status),
                        INDEX idx_dates (check_out_date)
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                new MySqlCommand(createCheckoutTable, connection).ExecuteNonQuery();

                // Insert default data if tables are empty
                InsertDefaultDataIfEmpty();
            }
        }

        private void InsertDefaultDataIfEmpty()
        {
            // Check and insert default categories
            string checkCategories = "SELECT COUNT(*) FROM equipment_categories";
            int catCount = Convert.ToInt32(new MySqlCommand(checkCategories, connection).ExecuteScalar());

            if (catCount == 0)
            {
                var categories = new[]
                {
                    new { name = "Rackets", desc = "Tennis, Badminton, Squash rackets", icon = "🏸" },
                    new { name = "Balls", desc = "Various game balls", icon = "⚽" },
                    new { name = "Nets", desc = "Game nets and accessories", icon = "🏐" },
                    new { name = "Protective Gear", desc = "Helmets, pads, guards", icon = "🛡️" },
                    new { name = "Footwear", desc = "Sports shoes", icon = "👟" }
                };

                foreach (var cat in categories)
                {
                    string insert = "INSERT INTO equipment_categories (name, description, icon) VALUES (@name, @desc, @icon)";
                    MySqlCommand cmd = new MySqlCommand(insert, connection);
                    cmd.Parameters.AddWithValue("@name", cat.name);
                    cmd.Parameters.AddWithValue("@desc", cat.desc);
                    cmd.Parameters.AddWithValue("@icon", cat.icon);
                    cmd.ExecuteNonQuery();
                }

                Activitylogs.Instance.AddLogEntry(currentUser, "Default Data", "Default equipment categories were created", "Info", "GameEquipment");
            }

            // Check and insert default conditions
            string checkConditions = "SELECT COUNT(*) FROM equipment_conditions";
            int condCount = Convert.ToInt32(new MySqlCommand(checkConditions, connection).ExecuteScalar());

            if (condCount == 0)
            {
                var conditions = new[]
                {
                    new { name = "New", color = "#4CAF50", desc = "Brand new condition" },
                    new { name = "Good", color = "#2196F3", desc = "Good condition, minor wear" },
                    new { name = "Fair", color = "#FF9800", desc = "Fair condition, visible wear" },
                    new { name = "Poor", color = "#F44336", desc = "Poor condition, needs replacement" },
                    new { name = "Maintenance", color = "#9C27B0", desc = "Needs maintenance" }
                };

                foreach (var cond in conditions)
                {
                    string insert = "INSERT INTO equipment_conditions (name, color, description) VALUES (@name, @color, @desc)";
                    MySqlCommand cmd = new MySqlCommand(insert, connection);
                    cmd.Parameters.AddWithValue("@name", cond.name);
                    cmd.Parameters.AddWithValue("@color", cond.color);
                    cmd.Parameters.AddWithValue("@desc", cond.desc);
                    cmd.ExecuteNonQuery();
                }

                Activitylogs.Instance.AddLogEntry(currentUser, "Default Data", "Default equipment conditions were created", "Info", "GameEquipment");
            }

            // Check and insert sample equipment
            string checkEquipment = "SELECT COUNT(*) FROM equipment_items";
            int eqCount = Convert.ToInt32(new MySqlCommand(checkEquipment, connection).ExecuteScalar());

            if (eqCount == 0)
            {
                var equipment = new[]
                {
                    new { name = "Tennis Racket - Pro Staff", cat = "Rackets", total = 10, available = 7, cond = "Good", loc = "Cabinet A1", notes = "Professional grade, includes cover", maint = false, lastMaint = DateTime.Now.AddMonths(-2) },
                    new { name = "Badminton Shuttlecocks (Tube)", cat = "Balls", total = 50, available = 32, cond = "New", loc = "Shelf B2", notes = "Tournament grade, 12 per tube", maint = false, lastMaint = DateTime.Now.AddMonths(-1) },
                    new { name = "Basketball - Official Size", cat = "Balls", total = 15, available = 8, cond = "Good", loc = "Bin C3", notes = "Indoor/outdoor use", maint = false, lastMaint = DateTime.Now.AddMonths(-3) },
                    new { name = "Volleyball Net (Complete Set)", cat = "Nets", total = 5, available = 2, cond = "Fair", loc = "Storage Room", notes = "Standard size, includes poles and ropes", maint = true, lastMaint = DateTime.Now.AddMonths(-6) },
                    new { name = "Knee Pads (Pair)", cat = "Protective Gear", total = 20, available = 15, cond = "Good", loc = "Shelf D4", notes = "Various sizes: S, M, L, XL", maint = false, lastMaint = DateTime.Now.AddMonths(-1) }
                };

                foreach (var eq in equipment)
                {
                    string insert = @"
                        INSERT INTO equipment_items 
                        (name, category, total_quantity, available_quantity, condition_name, location, notes, needs_maintenance, last_maintenance) 
                        VALUES (@name, @cat, @total, @available, @cond, @loc, @notes, @maint, @lastMaint)";

                    MySqlCommand cmd = new MySqlCommand(insert, connection);
                    cmd.Parameters.AddWithValue("@name", eq.name);
                    cmd.Parameters.AddWithValue("@cat", eq.cat);
                    cmd.Parameters.AddWithValue("@total", eq.total);
                    cmd.Parameters.AddWithValue("@available", eq.available);
                    cmd.Parameters.AddWithValue("@cond", eq.cond);
                    cmd.Parameters.AddWithValue("@loc", eq.loc);
                    cmd.Parameters.AddWithValue("@notes", eq.notes);
                    cmd.Parameters.AddWithValue("@maint", eq.maint);
                    cmd.Parameters.AddWithValue("@lastMaint", eq.lastMaint);
                    cmd.ExecuteNonQuery();
                }

                Activitylogs.Instance.AddLogEntry(currentUser, "Default Data", "Default equipment items were created", "Info", "GameEquipment");
            }

            // Insert sample checkout logs for demo
            InsertSampleCheckoutLogs();
        }

        private void InsertSampleCheckoutLogs()
        {
            string checkLogs = "SELECT COUNT(*) FROM equipment_checkout_logs";
            int logCount = Convert.ToInt32(new MySqlCommand(checkLogs, connection).ExecuteScalar());

            if (logCount == 0)
            {
                // Use strongly-typed list instead of array
                List<SampleLog> logs = new List<SampleLog>
                {
                    new SampleLog { eqId = 1, eqName = "Tennis Racket - Pro Staff", qty = 2, by = "John Smith", outDate = DateTime.Now.AddDays(-3), returnDate = DateTime.Now.AddDays(-1), status = "Returned" },
                    new SampleLog { eqId = 1, eqName = "Tennis Racket - Pro Staff", qty = 1, by = "Mike Johnson", outDate = DateTime.Now.AddDays(-2), returnDate = DBNull.Value, status = "Checked Out" },
                    new SampleLog { eqId = 2, eqName = "Badminton Shuttlecocks (Tube)", qty = 3, by = "Sarah Lee", outDate = DateTime.Now.AddDays(-5), returnDate = DateTime.Now.AddDays(-2), status = "Returned" },
                    new SampleLog { eqId = 2, eqName = "Badminton Shuttlecocks (Tube)", qty = 2, by = "Tom Wilson", outDate = DateTime.Now.AddDays(-1), returnDate = DBNull.Value, status = "Checked Out" },
                    new SampleLog { eqId = 3, eqName = "Basketball - Official Size", qty = 1, by = "David Brown", outDate = DateTime.Now.AddDays(-4), returnDate = DateTime.Now.AddDays(-1), status = "Returned" },
                    new SampleLog { eqId = 4, eqName = "Volleyball Net (Complete Set)", qty = 1, by = "Lisa Chen", outDate = DateTime.Now.AddDays(-2), returnDate = DBNull.Value, status = "Checked Out" },
                    new SampleLog { eqId = 5, eqName = "Knee Pads (Pair)", qty = 2, by = "Alex Garcia", outDate = DateTime.Now.AddDays(-3), returnDate = DateTime.Now, status = "Returned" }
                };

                foreach (var log in logs)
                {
                    string insert = @"
                        INSERT INTO equipment_checkout_logs 
                        (equipment_id, equipment_name, quantity, checked_out_by, check_out_date, actual_return_date, status) 
                        VALUES (@eqId, @eqName, @qty, @by, @outDate, @returnDate, @status)";

                    MySqlCommand cmd = new MySqlCommand(insert, connection);
                    cmd.Parameters.AddWithValue("@eqId", log.eqId);
                    cmd.Parameters.AddWithValue("@eqName", log.eqName);
                    cmd.Parameters.AddWithValue("@qty", log.qty);
                    cmd.Parameters.AddWithValue("@by", log.by);
                    cmd.Parameters.AddWithValue("@outDate", log.outDate);
                    cmd.Parameters.AddWithValue("@returnDate", log.returnDate);
                    cmd.Parameters.AddWithValue("@status", log.status);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Load categories
                    categoryList.Clear();
                    string catQuery = "SELECT id, name, description, icon FROM equipment_categories ORDER BY name";
                    MySqlCommand catCmd = new MySqlCommand(catQuery, connection);
                    using (MySqlDataReader reader = catCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoryList.Add(new Category
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Description = reader["description"]?.ToString() ?? "",
                                Icon = reader["icon"]?.ToString() ?? "📦",
                                ItemCount = 0 // Will update later
                            });
                        }
                    }

                    // Load conditions
                    conditionList.Clear();
                    string condQuery = "SELECT id, name, color, description FROM equipment_conditions ORDER BY name";
                    MySqlCommand condCmd = new MySqlCommand(condQuery, connection);
                    using (MySqlDataReader reader = condCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            conditionList.Add(new Condition
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Color = reader["color"]?.ToString() ?? "#808080",
                                Description = reader["description"]?.ToString() ?? ""
                            });
                        }
                    }

                    // Load equipment
                    equipmentList.Clear();
                    string eqQuery = @"
                        SELECT id, name, category, total_quantity, available_quantity, 
                               condition_name, location, notes, needs_maintenance, last_maintenance,
                               created_at, updated_at
                        FROM equipment_items ORDER BY name";

                    MySqlCommand eqCmd = new MySqlCommand(eqQuery, connection);
                    using (MySqlDataReader reader = eqCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipmentList.Add(new EquipmentItem
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Category = reader["category"].ToString(),
                                TotalQuantity = Convert.ToInt32(reader["total_quantity"]),
                                AvailableQuantity = Convert.ToInt32(reader["available_quantity"]),
                                Condition = reader["condition_name"].ToString(),
                                Location = reader["location"]?.ToString() ?? "",
                                Notes = reader["notes"]?.ToString() ?? "",
                                NeedsMaintenance = Convert.ToBoolean(reader["needs_maintenance"]),
                                LastMaintenance = reader["last_maintenance"] != DBNull.Value ?
                                    Convert.ToDateTime(reader["last_maintenance"]) : DateTime.Now,
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            });
                        }
                    }

                    // Load checkout logs - FIXED: Using if-else for nullable DateTime
                    checkoutLogs.Clear();
                    string logQuery = @"
                        SELECT id, equipment_id, equipment_name, quantity, checked_out_by,
                               check_out_date, actual_return_date, notes, status
                        FROM equipment_checkout_logs ORDER BY check_out_date DESC";

                    MySqlCommand logCmd = new MySqlCommand(logQuery, connection);
                    using (MySqlDataReader reader = logCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CheckoutLog log = new CheckoutLog
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                EquipmentId = Convert.ToInt32(reader["equipment_id"]),
                                EquipmentName = reader["equipment_name"].ToString(),
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                CheckedOutBy = reader["checked_out_by"].ToString(),
                                CheckOutDate = Convert.ToDateTime(reader["check_out_date"]),
                                Notes = reader["notes"]?.ToString() ?? "",
                                Status = reader["status"].ToString()
                            };

                            // Handle nullable DateTime for actual_return_date - FIXED
                            if (reader["actual_return_date"] != DBNull.Value)
                            {
                                log.ActualReturnDate = Convert.ToDateTime(reader["actual_return_date"]);
                            }
                            else
                            {
                                log.ActualReturnDate = null;
                            }

                            checkoutLogs.Add(log);
                        }
                    }

                    // Update category item counts
                    foreach (var category in categoryList)
                    {
                        category.ItemCount = equipmentList.Count(e => e.Category == category.Name);
                    }
                }

                Activitylogs.Instance.AddLogEntry(currentUser, "Data Loaded", $"Loaded {equipmentList.Count} equipment items and {checkoutLogs.Count} logs from database", "Info", "GameEquipment");
            }
            catch (Exception ex)
            {
                Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, "Error loading data from database");
                MessageBox.Show($"Error loading data: {ex.Message}\nUsing sample data.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InitializeSampleData();
            }
        }

        private void InitializeSampleData()
        {
            // Add sample categories
            categoryList.Add(new Category { Id = 1, Name = "Rackets", Description = "Tennis, Badminton, Squash rackets", ItemCount = 15, Icon = "🏸" });
            categoryList.Add(new Category { Id = 2, Name = "Balls", Description = "Various game balls", ItemCount = 50, Icon = "⚽" });
            categoryList.Add(new Category { Id = 3, Name = "Nets", Description = "Game nets and accessories", ItemCount = 8, Icon = "🏐" });
            categoryList.Add(new Category { Id = 4, Name = "Protective Gear", Description = "Helmets, pads, guards", ItemCount = 20, Icon = "🛡️" });
            categoryList.Add(new Category { Id = 5, Name = "Footwear", Description = "Sports shoes", ItemCount = 25, Icon = "👟" });

            // Add sample conditions
            conditionList.Add(new Condition { Id = 1, Name = "New", Color = "#4CAF50", Description = "Brand new condition" });
            conditionList.Add(new Condition { Id = 2, Name = "Good", Color = "#2196F3", Description = "Good condition, minor wear" });
            conditionList.Add(new Condition { Id = 3, Name = "Fair", Color = "#FF9800", Description = "Fair condition, visible wear" });
            conditionList.Add(new Condition { Id = 4, Name = "Poor", Color = "#F44336", Description = "Poor condition, needs replacement" });
            conditionList.Add(new Condition { Id = 5, Name = "Maintenance", Color = "#9C27B0", Description = "Needs maintenance" });

            // Add sample equipment
            equipmentList.Add(new EquipmentItem
            {
                Id = 1,
                Name = "Tennis Racket - Pro Staff",
                Category = "Rackets",
                TotalQuantity = 10,
                AvailableQuantity = 7,
                Condition = "Good",
                Location = "Cabinet A1",
                Notes = "Professional grade, includes cover",
                NeedsMaintenance = false,
                LastMaintenance = DateTime.Now.AddMonths(-2)
            });

            equipmentList.Add(new EquipmentItem
            {
                Id = 2,
                Name = "Badminton Shuttlecocks (Tube)",
                Category = "Balls",
                TotalQuantity = 50,
                AvailableQuantity = 32,
                Condition = "New",
                Location = "Shelf B2",
                Notes = "Tournament grade, 12 per tube",
                NeedsMaintenance = false,
                LastMaintenance = DateTime.Now.AddMonths(-1)
            });

            equipmentList.Add(new EquipmentItem
            {
                Id = 3,
                Name = "Basketball - Official Size",
                Category = "Balls",
                TotalQuantity = 15,
                AvailableQuantity = 8,
                Condition = "Good",
                Location = "Bin C3",
                Notes = "Indoor/outdoor use",
                NeedsMaintenance = false,
                LastMaintenance = DateTime.Now.AddMonths(-3)
            });

            equipmentList.Add(new EquipmentItem
            {
                Id = 4,
                Name = "Volleyball Net (Complete Set)",
                Category = "Nets",
                TotalQuantity = 5,
                AvailableQuantity = 2,
                Condition = "Fair",
                Location = "Storage Room",
                Notes = "Standard size, includes poles and ropes",
                NeedsMaintenance = true,
                LastMaintenance = DateTime.Now.AddMonths(-6)
            });

            equipmentList.Add(new EquipmentItem
            {
                Id = 5,
                Name = "Knee Pads (Pair)",
                Category = "Protective Gear",
                TotalQuantity = 20,
                AvailableQuantity = 15,
                Condition = "Good",
                Location = "Shelf D4",
                Notes = "Various sizes: S, M, L, XL",
                NeedsMaintenance = false,
                LastMaintenance = DateTime.Now.AddMonths(-1)
            });

            // Add sample checkout logs
            checkoutLogs.Add(new CheckoutLog
            {
                Id = 1,
                EquipmentId = 1,
                EquipmentName = "Tennis Racket - Pro Staff",
                Quantity = 2,
                CheckedOutBy = "John Smith",
                CheckOutDate = DateTime.Now.AddDays(-3),
                ActualReturnDate = DateTime.Now.AddDays(-1),
                Status = "Returned"
            });

            checkoutLogs.Add(new CheckoutLog
            {
                Id = 2,
                EquipmentId = 1,
                EquipmentName = "Tennis Racket - Pro Staff",
                Quantity = 1,
                CheckedOutBy = "Mike Johnson",
                CheckOutDate = DateTime.Now.AddDays(-2),
                ActualReturnDate = null,
                Status = "Checked Out"
            });

            checkoutLogs.Add(new CheckoutLog
            {
                Id = 3,
                EquipmentId = 2,
                EquipmentName = "Badminton Shuttlecocks (Tube)",
                Quantity = 3,
                CheckedOutBy = "Sarah Lee",
                CheckOutDate = DateTime.Now.AddDays(-5),
                ActualReturnDate = DateTime.Now.AddDays(-2),
                Status = "Returned"
            });

            checkoutLogs.Add(new CheckoutLog
            {
                Id = 4,
                EquipmentId = 2,
                EquipmentName = "Badminton Shuttlecocks (Tube)",
                Quantity = 2,
                CheckedOutBy = "Tom Wilson",
                CheckOutDate = DateTime.Now.AddDays(-1),
                ActualReturnDate = null,
                Status = "Checked Out"
            });

            checkoutLogs.Add(new CheckoutLog
            {
                Id = 5,
                EquipmentId = 3,
                EquipmentName = "Basketball - Official Size",
                Quantity = 1,
                CheckedOutBy = "David Brown",
                CheckOutDate = DateTime.Now.AddDays(-4),
                ActualReturnDate = DateTime.Now.AddDays(-1),
                Status = "Returned"
            });

            checkoutLogs.Add(new CheckoutLog
            {
                Id = 6,
                EquipmentId = 4,
                EquipmentName = "Volleyball Net (Complete Set)",
                Quantity = 1,
                CheckedOutBy = "Lisa Chen",
                CheckOutDate = DateTime.Now.AddDays(-2),
                ActualReturnDate = null,
                Status = "Checked Out"
            });

            checkoutLogs.Add(new CheckoutLog
            {
                Id = 7,
                EquipmentId = 5,
                EquipmentName = "Knee Pads (Pair)",
                Quantity = 2,
                CheckedOutBy = "Alex Garcia",
                CheckOutDate = DateTime.Now.AddDays(-3),
                ActualReturnDate = DateTime.Now,
                Status = "Returned"
            });
        }

        private void StyleButtons()
        {
            StyleButton(btnAddEquipment, successColor);
            StyleButton(btnManageCategories, primaryColor);
            StyleButton(btnAddCategory, successColor);
            StyleButton(btnAddCondition, successColor);
            StyleButton(btnCloseManagement, dangerColor);
            StyleButton(btnViewLogs, infoColor);
        }

        private void StyleButton(Button btn, Color backColor)
        {
            btn.BackColor = backColor;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Height = 35;

            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(backColor, 0.2f);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;
        }

        private void DisplayEquipment()
        {
            if (equipmentFlowPanel.InvokeRequired)
            {
                equipmentFlowPanel.Invoke(new MethodInvoker(() => DisplayEquipment()));
                return;
            }

            equipmentFlowPanel.Controls.Clear();

            string filter = filterCombo.SelectedItem?.ToString() ?? "All Equipment";
            var filteredList = equipmentList.Where(e =>
                filter == "All Equipment" ||
                (filter == "Available Only" && e.IsAvailable) ||
                (filter == "Unavailable Only" && !e.IsAvailable) ||
                (filter == "Needs Maintenance" && e.NeedsMaintenance)
            ).ToList();

            foreach (var item in filteredList)
            {
                var card = CreateEquipmentCard(item);
                equipmentFlowPanel.Controls.Add(card);
            }
        }

        private string GetCategoryIcon(string category)
        {
            var cat = categoryList.FirstOrDefault(c => c.Name == category);
            return cat?.Icon ?? "📦";
        }

        private Color GetConditionColor(string condition)
        {
            var cond = conditionList.FirstOrDefault(c => c.Name == condition);
            if (cond != null)
            {
                return ColorTranslator.FromHtml(cond.Color);
            }

            // Default colors if condition not found
            switch (condition)
            {
                case "New": return Color.FromArgb(76, 175, 80);
                case "Good": return Color.FromArgb(33, 150, 243);
                case "Fair": return Color.FromArgb(255, 152, 0);
                case "Poor": return Color.FromArgb(244, 67, 54);
                case "Maintenance": return Color.FromArgb(156, 39, 176);
                default: return Color.Gray;
            }
        }

        private Panel CreateEquipmentCard(EquipmentItem item)
        {
            Panel card = new Panel
            {
                Width = 340,
                Height = 280,
                BackColor = cardBgColor,
                Margin = new Padding(10),
                Tag = item
            };

            // Add border
            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
            };

            // Status indicator based on condition
            Panel statusBar = new Panel
            {
                Height = 6,
                Dock = DockStyle.Top,
                BackColor = GetConditionColor(item.Condition)
            };

            // Category badge with icon
            Panel categoryBadge = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            string categoryIcon = GetCategoryIcon(item.Category);
            Label lblCategory = new Label
            {
                Text = $"{categoryIcon} {item.Category}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(5, 5),
                Size = new Size(110, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            categoryBadge.Controls.Add(lblCategory);

            // Equipment Name
            Label lblName = new Label
            {
                Text = item.Name,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 50),
                Size = new Size(310, 25),
                ForeColor = Color.FromArgb(33, 33, 33)
            };

            // Stock information with progress bar
            Panel stockPanel = new Panel
            {
                Location = new Point(15, 80),
                Size = new Size(310, 45),
                BackColor = Color.Transparent
            };

            Label lblStock = new Label
            {
                Text = $"Stock: {item.AvailableQuantity}/{item.TotalQuantity} available",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(310, 20),
                ForeColor = item.StatusColor
            };

            // Simple progress bar for stock level
            Panel progressBg = new Panel
            {
                Location = new Point(0, 22),
                Size = new Size(310, 8),
                BackColor = Color.FromArgb(230, 230, 230)
            };

            int fillWidth = (int)((double)item.AvailableQuantity / item.TotalQuantity * 310);
            Panel progressFill = new Panel
            {
                Location = new Point(0, 22),
                Size = new Size(fillWidth, 8),
                BackColor = item.StatusColor
            };

            stockPanel.Controls.AddRange(new Control[] { lblStock, progressBg, progressFill });

            // Condition and Location
            Label lblCondition = new Label
            {
                Text = $"Condition: {item.Condition}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(15, 130),
                Size = new Size(150, 20),
                ForeColor = GetConditionColor(item.Condition)
            };

            Label lblLocation = new Label
            {
                Text = $"📍 {item.Location}",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(170, 130),
                Size = new Size(155, 20),
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            // Last Maintenance
            Label lblMaintenance = new Label
            {
                Text = $"Last maintenance: {item.LastMaintenance:MMM dd, yyyy}",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                Location = new Point(15, 155),
                Size = new Size(310, 15),
                ForeColor = item.NeedsMaintenance ? dangerColor : Color.FromArgb(140, 140, 140)
            };

            // Notes
            Label lblNotes = new Label
            {
                Text = item.Notes,
                Font = new Font("Segoe UI", 8F),
                Location = new Point(15, 175),
                Size = new Size(310, 30),
                ForeColor = Color.FromArgb(140, 140, 140)
            };

            // Action buttons
            Button btnCheckOut = new Button
            {
                Text = "Check Out",
                FlatStyle = FlatStyle.Flat,
                BackColor = item.IsAvailable ? successColor : Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(85, 35),
                Location = new Point(15, 215),
                Tag = item,
                Cursor = Cursors.Hand,
                Enabled = item.IsAvailable
            };
            btnCheckOut.FlatAppearance.BorderSize = 0;
            btnCheckOut.Click += BtnCheckOut_Click;

            Button btnCheckIn = new Button
            {
                Text = "Check In",
                FlatStyle = FlatStyle.Flat,
                BackColor = item.CheckedOutQuantity > 0 ? warningColor : Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(85, 35),
                Location = new Point(110, 215),
                Tag = item,
                Cursor = Cursors.Hand,
                Enabled = item.CheckedOutQuantity > 0
            };
            btnCheckIn.FlatAppearance.BorderSize = 0;
            btnCheckIn.Click += BtnCheckIn_Click;

            Button btnEdit = new Button
            {
                Text = "Edit",
                FlatStyle = FlatStyle.Flat,
                BackColor = primaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(60, 35),
                Location = new Point(205, 215),
                Tag = item,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            Button btnMaintenance = new Button
            {
                Text = "🔧",
                FlatStyle = FlatStyle.Flat,
                BackColor = item.NeedsMaintenance ? dangerColor : Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(45, 35),
                Location = new Point(275, 215),
                Tag = item,
                Cursor = Cursors.Hand,
                Visible = item.NeedsMaintenance
            };
            btnMaintenance.FlatAppearance.BorderSize = 0;
            btnMaintenance.Click += BtnMaintenance_Click;

            card.Controls.AddRange(new Control[] {
                statusBar, categoryBadge, lblName, stockPanel,
                lblCondition, lblLocation, lblMaintenance, lblNotes,
                btnCheckOut, btnCheckIn, btnEdit, btnMaintenance
            });

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = cardBgColor;

            return card;
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            EquipmentItem item = (EquipmentItem)btn.Tag;

            using (var dialog = new Form())
            {
                dialog.Text = $"Check Out - {item.Name}";
                dialog.Size = new Size(400, 200);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.BackColor = Color.White;

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 3,
                    Padding = new Padding(20),
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Absolute, 120),
                        new ColumnStyle(SizeType.Percent, 100)
                    }
                };

                // Available quantity
                tlp.Controls.Add(new Label { Text = "Available:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                Label lblAvailable = new Label
                {
                    Text = item.AvailableQuantity.ToString(),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = successColor,
                    Anchor = AnchorStyles.Left
                };
                tlp.Controls.Add(lblAvailable, 1, 0);

                // Quantity to check out
                tlp.Controls.Add(new Label { Text = "Quantity:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 1);
                NumericUpDown nudQuantity = new NumericUpDown
                {
                    Minimum = 1,
                    Maximum = item.AvailableQuantity,
                    Value = 1,
                    Font = new Font("Segoe UI", 10F),
                    Width = 100
                };
                tlp.Controls.Add(nudQuantity, 1, 1);

                // Checked out by
                tlp.Controls.Add(new Label { Text = "Checked out by:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 2);
                TextBox txtCheckedBy = new TextBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill
                };
                tlp.Controls.Add(txtCheckedBy, 1, 2);

                // Buttons
                FlowLayoutPanel btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 50,
                    Padding = new Padding(0, 0, 20, 0)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 35),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(5)
                };
                btnCancel.FlatAppearance.BorderSize = 0;
                btnCancel.Click += (s, args) => dialog.Close();

                Button btnConfirm = new Button
                {
                    Text = "Check Out",
                    Size = new Size(100, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(5)
                };
                btnConfirm.FlatAppearance.BorderSize = 0;
                btnConfirm.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtCheckedBy.Text))
                    {
                        MessageBox.Show("Please enter who is checking out the equipment.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int quantity = (int)nudQuantity.Value;

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            // Update equipment stock
                            string updateEq = "UPDATE equipment_items SET available_quantity = available_quantity - @qty WHERE id = @id";
                            MySqlCommand updateCmd = new MySqlCommand(updateEq, conn);
                            updateCmd.Parameters.AddWithValue("@qty", quantity);
                            updateCmd.Parameters.AddWithValue("@id", item.Id);
                            updateCmd.ExecuteNonQuery();

                            // Create checkout log
                            string insertLog = @"
                                INSERT INTO equipment_checkout_logs 
                                (equipment_id, equipment_name, quantity, checked_out_by, check_out_date, status) 
                                VALUES (@eqId, @eqName, @qty, @by, @outDate, 'Checked Out')";

                            MySqlCommand logCmd = new MySqlCommand(insertLog, conn);
                            logCmd.Parameters.AddWithValue("@eqId", item.Id);
                            logCmd.Parameters.AddWithValue("@eqName", item.Name);
                            logCmd.Parameters.AddWithValue("@qty", quantity);
                            logCmd.Parameters.AddWithValue("@by", txtCheckedBy.Text);
                            logCmd.Parameters.AddWithValue("@outDate", DateTime.Now);
                            logCmd.ExecuteNonQuery();
                        }

                        // Update local object
                        item.AvailableQuantity -= quantity;

                        // Add to local checkout logs
                        checkoutLogs.Add(new CheckoutLog
                        {
                            Id = checkoutLogs.Count + 1,
                            EquipmentId = item.Id,
                            EquipmentName = item.Name,
                            Quantity = quantity,
                            CheckedOutBy = txtCheckedBy.Text,
                            CheckOutDate = DateTime.Now,
                            Status = "Checked Out"
                        });

                        // Log the checkout
                        Activitylogs.Instance.LogEquipmentCheckout(currentUser, item.Name, quantity, txtCheckedBy.Text);

                        // Check for low stock warning
                        if (item.AvailableQuantity < 5)
                        {
                            Activitylogs.Instance.LogWarning(currentUser, "GameEquipment",
                                $"Low stock warning: {item.Name} only has {item.AvailableQuantity} left");
                        }

                        MessageBox.Show($"Successfully checked out {quantity} {item.Name}(s) to {txtCheckedBy.Text}.\n" +
                                      $"Check out time: {DateTime.Now:MMM dd, yyyy hh:mm tt}\n" +
                                      $"Remaining stock: {item.AvailableQuantity}",
                            "Check Out Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, $"Error checking out {item.Name}");
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnConfirm);

                dialog.Controls.Add(tlp);
                dialog.Controls.Add(btnPanel);
                dialog.ShowDialog(this);
            }
        }

        private void BtnCheckIn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            EquipmentItem item = (EquipmentItem)btn.Tag;

            // Find active checkouts for this item
            var activeCheckouts = checkoutLogs.Where(c => c.EquipmentId == item.Id && c.Status == "Checked Out").ToList();

            if (activeCheckouts.Count == 0)
            {
                // Manual check in if no logs exist
                ManualCheckIn(item);
            }
            else
            {
                // Show list of active checkouts to select which one to return
                SelectCheckoutToReturn(item, activeCheckouts);
            }
        }

        private void ManualCheckIn(EquipmentItem item)
        {
            using (var dialog = new Form())
            {
                dialog.Text = $"Check In - {item.Name}";
                dialog.Size = new Size(400, 250);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.BackColor = Color.White;

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 3,
                    Padding = new Padding(20),
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Absolute, 120),
                        new ColumnStyle(SizeType.Percent, 100)
                    }
                };

                // Currently checked out
                tlp.Controls.Add(new Label { Text = "Checked out:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                Label lblCheckedOut = new Label
                {
                    Text = item.CheckedOutQuantity.ToString(),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = warningColor,
                    Anchor = AnchorStyles.Left
                };
                tlp.Controls.Add(lblCheckedOut, 1, 0);

                // Quantity to check in
                tlp.Controls.Add(new Label { Text = "Quantity to return:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 1);
                NumericUpDown nudQuantity = new NumericUpDown
                {
                    Minimum = 1,
                    Maximum = item.CheckedOutQuantity,
                    Value = 1,
                    Font = new Font("Segoe UI", 10F),
                    Width = 100
                };
                tlp.Controls.Add(nudQuantity, 1, 1);

                // Returned by
                tlp.Controls.Add(new Label { Text = "Returned by:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 2);
                TextBox txtReturnedBy = new TextBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill
                };
                tlp.Controls.Add(txtReturnedBy, 1, 2);

                // Buttons
                FlowLayoutPanel btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 50,
                    Padding = new Padding(0, 0, 20, 0)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 35),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(5)
                };
                btnCancel.FlatAppearance.BorderSize = 0;
                btnCancel.Click += (s, args) => dialog.Close();

                Button btnConfirm = new Button
                {
                    Text = "Check In",
                    Size = new Size(100, 35),
                    BackColor = warningColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(5)
                };
                btnConfirm.FlatAppearance.BorderSize = 0;

                btnConfirm.Click += (s, args) =>
                {
                    int quantity = (int)nudQuantity.Value;
                    string returnedBy = string.IsNullOrWhiteSpace(txtReturnedBy.Text) ? "Unknown" : txtReturnedBy.Text;

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            // Update equipment stock
                            string updateEq = "UPDATE equipment_items SET available_quantity = available_quantity + @qty WHERE id = @id";
                            MySqlCommand updateCmd = new MySqlCommand(updateEq, conn);
                            updateCmd.Parameters.AddWithValue("@qty", quantity);
                            updateCmd.Parameters.AddWithValue("@id", item.Id);
                            updateCmd.ExecuteNonQuery();

                            // Create a return log
                            string insertLog = @"
                                INSERT INTO equipment_checkout_logs 
                                (equipment_id, equipment_name, quantity, checked_out_by, check_out_date, actual_return_date, status, notes) 
                                VALUES (@eqId, @eqName, @qty, @by, @outDate, @returnDate, 'Returned', @notes)";

                            MySqlCommand logCmd = new MySqlCommand(insertLog, conn);
                            logCmd.Parameters.AddWithValue("@eqId", item.Id);
                            logCmd.Parameters.AddWithValue("@eqName", item.Name);
                            logCmd.Parameters.AddWithValue("@qty", quantity);
                            logCmd.Parameters.AddWithValue("@by", returnedBy);
                            logCmd.Parameters.AddWithValue("@outDate", DateTime.Now.AddHours(-1));
                            logCmd.Parameters.AddWithValue("@returnDate", DateTime.Now);
                            logCmd.Parameters.AddWithValue("@notes", $"Returned by {returnedBy}");
                            logCmd.ExecuteNonQuery();
                        }

                        // Update local object
                        item.AvailableQuantity += quantity;

                        // Log the check-in
                        Activitylogs.Instance.LogEquipmentCheckin(currentUser, item.Name, quantity, returnedBy);

                        MessageBox.Show($"Successfully checked in {quantity} {item.Name}(s).\n" +
                                      $"Check in time: {DateTime.Now:MMM dd, yyyy hh:mm tt}\n" +
                                      $"Current stock: {item.AvailableQuantity}/{item.TotalQuantity}",
                            "Check In Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, $"Error checking in {item.Name}");
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnConfirm);

                dialog.Controls.Add(tlp);
                dialog.Controls.Add(btnPanel);
                dialog.ShowDialog(this);
            }
        }

        private void SelectCheckoutToReturn(EquipmentItem item, List<CheckoutLog> activeCheckouts)
        {
            using (var dialog = new Form())
            {
                dialog.Text = $"Return Equipment - {item.Name}";
                dialog.Size = new Size(600, 400);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.BackColor = Color.White;

                ListView listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true,
                    Font = new Font("Segoe UI", 10F)
                };
                listView.Columns.Add("Checked Out By", 150);
                listView.Columns.Add("Quantity", 80);
                listView.Columns.Add("Check Out Date", 150);
                listView.Columns.Add("Time Out", 100);
                listView.Columns.Add("Duration", 100);

                foreach (var checkout in activeCheckouts)
                {
                    var item2 = new ListViewItem(checkout.CheckedOutBy);
                    item2.SubItems.Add(checkout.Quantity.ToString());
                    item2.SubItems.Add(checkout.CheckOutDate.ToString("MMM dd, yyyy"));
                    item2.SubItems.Add(checkout.CheckOutDate.ToString("hh:mm tt"));

                    TimeSpan diff = DateTime.Now - checkout.CheckOutDate;
                    item2.SubItems.Add($"{(int)diff.TotalHours}h {diff.Minutes}m");

                    item2.Tag = checkout;
                    listView.Items.Add(item2);
                }

                // Quantity to return panel
                Panel returnPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 80,
                    BackColor = Color.FromArgb(240, 240, 240)
                };

                Label lblQuantity = new Label
                {
                    Text = "Quantity to return:",
                    Location = new Point(150, 15),
                    Size = new Size(120, 25),
                    Font = new Font("Segoe UI", 10F)
                };

                NumericUpDown nudQuantity = new NumericUpDown
                {
                    Location = new Point(280, 12),
                    Minimum = 1,
                    Maximum = 1,
                    Value = 1,
                    Width = 60,
                    Font = new Font("Segoe UI", 10F)
                };

                Button btnReturn = new Button
                {
                    Text = "Return Selected",
                    Size = new Size(150, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Location = new Point(220, 45),
                    Cursor = Cursors.Hand
                };
                btnReturn.FlatAppearance.BorderSize = 0;

                // Update max quantity when item selected
                listView.SelectedIndexChanged += (s, args) =>
                {
                    if (listView.SelectedItems.Count > 0)
                    {
                        var selectedCheckout = (CheckoutLog)listView.SelectedItems[0].Tag;
                        nudQuantity.Maximum = selectedCheckout.Quantity;
                        nudQuantity.Value = selectedCheckout.Quantity;
                    }
                };

                returnPanel.Controls.Add(lblQuantity);
                returnPanel.Controls.Add(nudQuantity);
                returnPanel.Controls.Add(btnReturn);

                btnReturn.Click += (s, args) =>
                {
                    if (listView.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select a checkout to return.",
                            "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var selectedCheckout = (CheckoutLog)listView.SelectedItems[0].Tag;
                    int quantityToReturn = (int)nudQuantity.Value;

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            // Update equipment stock
                            string updateEq = "UPDATE equipment_items SET available_quantity = available_quantity + @qty WHERE id = @id";
                            MySqlCommand updateCmd = new MySqlCommand(updateEq, conn);
                            updateCmd.Parameters.AddWithValue("@qty", quantityToReturn);
                            updateCmd.Parameters.AddWithValue("@id", item.Id);
                            updateCmd.ExecuteNonQuery();

                            if (quantityToReturn == selectedCheckout.Quantity)
                            {
                                // Full return
                                string updateLog = @"
                                    UPDATE equipment_checkout_logs 
                                    SET actual_return_date = @returnDate, status = 'Returned' 
                                    WHERE id = @id";

                                MySqlCommand logCmd = new MySqlCommand(updateLog, conn);
                                logCmd.Parameters.AddWithValue("@returnDate", DateTime.Now);
                                logCmd.Parameters.AddWithValue("@id", selectedCheckout.Id);
                                logCmd.ExecuteNonQuery();

                                selectedCheckout.Status = "Returned";
                                selectedCheckout.ActualReturnDate = DateTime.Now;
                            }
                            else
                            {
                                // Partial return - split the log
                                string updateLog = @"
                                    UPDATE equipment_checkout_logs 
                                    SET quantity = @newQty 
                                    WHERE id = @id";

                                MySqlCommand updateCmd2 = new MySqlCommand(updateLog, conn);
                                updateCmd2.Parameters.AddWithValue("@newQty", selectedCheckout.Quantity - quantityToReturn);
                                updateCmd2.Parameters.AddWithValue("@id", selectedCheckout.Id);
                                updateCmd2.ExecuteNonQuery();

                                // Create new log for returned items
                                string insertLog = @"
                                    INSERT INTO equipment_checkout_logs 
                                    (equipment_id, equipment_name, quantity, checked_out_by, check_out_date, actual_return_date, status, notes) 
                                    VALUES (@eqId, @eqName, @qty, @by, @outDate, @returnDate, 'Returned', @notes)";

                                MySqlCommand insertCmd = new MySqlCommand(insertLog, conn);
                                insertCmd.Parameters.AddWithValue("@eqId", item.Id);
                                insertCmd.Parameters.AddWithValue("@eqName", item.Name);
                                insertCmd.Parameters.AddWithValue("@qty", quantityToReturn);
                                insertCmd.Parameters.AddWithValue("@by", selectedCheckout.CheckedOutBy);
                                insertCmd.Parameters.AddWithValue("@outDate", selectedCheckout.CheckOutDate);
                                insertCmd.Parameters.AddWithValue("@returnDate", DateTime.Now);
                                insertCmd.Parameters.AddWithValue("@notes", $"Partial return");
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        // Update local object
                        item.AvailableQuantity += quantityToReturn;

                        // Log the check-in
                        Activitylogs.Instance.LogEquipmentCheckin(currentUser, item.Name, quantityToReturn, selectedCheckout.CheckedOutBy);

                        MessageBox.Show($"Successfully returned {quantityToReturn} {item.Name}(s) from {selectedCheckout.CheckedOutBy}.\n" +
                                      $"Return time: {DateTime.Now:MMM dd, yyyy hh:mm tt}",
                            "Return Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh data to update logs
                        RefreshDataFromDatabase();

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, $"Error checking in {item.Name}");
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                dialog.Controls.Add(listView);
                dialog.Controls.Add(returnPanel);
                dialog.ShowDialog(this);
            }
        }

        private void RefreshDataFromDatabase()
        {
            try
            {
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Reload equipment
                    equipmentList.Clear();
                    string eqQuery = @"
                        SELECT id, name, category, total_quantity, available_quantity, 
                               condition_name, location, notes, needs_maintenance, last_maintenance,
                               created_at, updated_at
                        FROM equipment_items ORDER BY name";

                    MySqlCommand eqCmd = new MySqlCommand(eqQuery, connection);
                    using (MySqlDataReader reader = eqCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipmentList.Add(new EquipmentItem
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Category = reader["category"].ToString(),
                                TotalQuantity = Convert.ToInt32(reader["total_quantity"]),
                                AvailableQuantity = Convert.ToInt32(reader["available_quantity"]),
                                Condition = reader["condition_name"].ToString(),
                                Location = reader["location"]?.ToString() ?? "",
                                Notes = reader["notes"]?.ToString() ?? "",
                                NeedsMaintenance = Convert.ToBoolean(reader["needs_maintenance"]),
                                LastMaintenance = reader["last_maintenance"] != DBNull.Value ?
                                    Convert.ToDateTime(reader["last_maintenance"]) : DateTime.Now,
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
                            });
                        }
                    }

                    // Reload checkout logs
                    checkoutLogs.Clear();
                    string logQuery = @"
                        SELECT id, equipment_id, equipment_name, quantity, checked_out_by,
                               check_out_date, actual_return_date, notes, status
                        FROM equipment_checkout_logs ORDER BY check_out_date DESC";

                    MySqlCommand logCmd = new MySqlCommand(logQuery, connection);
                    using (MySqlDataReader reader = logCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CheckoutLog log = new CheckoutLog
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                EquipmentId = Convert.ToInt32(reader["equipment_id"]),
                                EquipmentName = reader["equipment_name"].ToString(),
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                CheckedOutBy = reader["checked_out_by"].ToString(),
                                CheckOutDate = Convert.ToDateTime(reader["check_out_date"]),
                                Notes = reader["notes"]?.ToString() ?? "",
                                Status = reader["status"].ToString()
                            };

                            // Handle nullable DateTime for actual_return_date - FIXED
                            if (reader["actual_return_date"] != DBNull.Value)
                            {
                                log.ActualReturnDate = Convert.ToDateTime(reader["actual_return_date"]);
                            }
                            else
                            {
                                log.ActualReturnDate = null;
                            }

                            checkoutLogs.Add(log);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, "Error refreshing data");
            }
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            ShowLogsDialog();
        }

        private void ShowLogsDialog()
        {
            using (var dialog = new Form())
            {
                dialog.Text = "Equipment Checkout Logs";
                dialog.Size = new Size(1000, 600);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.BackColor = Color.White;

                // Create TabControl for different log views
                TabControl tabControl = new TabControl
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10F)
                };

                // All Logs Tab
                TabPage allLogsTab = new TabPage("All Transactions");
                CreateLogsListView(allLogsTab, checkoutLogs);
                tabControl.TabPages.Add(allLogsTab);

                // Active Checkouts Tab
                TabPage activeTab = new TabPage("Active Checkouts");
                var activeLogs = checkoutLogs.Where(l => l.Status == "Checked Out").ToList();
                CreateLogsListView(activeTab, activeLogs);
                tabControl.TabPages.Add(activeTab);

                // Returned Items Tab
                TabPage returnedTab = new TabPage("Returned Items");
                var returnedLogs = checkoutLogs.Where(l => l.Status == "Returned").ToList();
                CreateLogsListView(returnedTab, returnedLogs);
                tabControl.TabPages.Add(returnedTab);

                // Summary Tab
                TabPage summaryTab = new TabPage("Summary");
                CreateSummaryView(summaryTab);
                tabControl.TabPages.Add(summaryTab);

                // Export Button
                Button btnExport = new Button
                {
                    Text = "Export Logs",
                    Size = new Size(120, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Location = new Point(850, 550),
                    Cursor = Cursors.Hand
                };
                btnExport.FlatAppearance.BorderSize = 0;
                btnExport.Click += (s, args) => ExportLogs();

                Button btnClose = new Button
                {
                    Text = "Close",
                    Size = new Size(100, 35),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(980, 550),
                    Cursor = Cursors.Hand
                };
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.Click += (s, args) => dialog.Close();

                dialog.Controls.Add(tabControl);
                dialog.Controls.Add(btnExport);
                dialog.Controls.Add(btnClose);

                dialog.ShowDialog(this);
            }
        }

        private void CreateLogsListView(TabPage tabPage, List<CheckoutLog> logs)
        {
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 10F)
            };

            listView.Columns.Add("Equipment", 200);
            listView.Columns.Add("Quantity", 80);
            listView.Columns.Add("Checked Out By", 150);
            listView.Columns.Add("Check Out Date", 120);
            listView.Columns.Add("Check Out Time", 100);
            listView.Columns.Add("Return Date", 120);
            listView.Columns.Add("Return Time", 100);
            listView.Columns.Add("Duration", 100);
            listView.Columns.Add("Status", 100);

            foreach (var log in logs)
            {
                var item = new ListViewItem(log.EquipmentName);
                item.SubItems.Add(log.Quantity.ToString());
                item.SubItems.Add(log.CheckedOutBy);
                item.SubItems.Add(log.CheckOutDate.ToString("MMM dd, yyyy"));
                item.SubItems.Add(log.CheckOutDate.ToString("hh:mm tt"));

                if (log.ActualReturnDate.HasValue)
                {
                    item.SubItems.Add(log.ActualReturnDate.Value.ToString("MMM dd, yyyy"));
                    item.SubItems.Add(log.ActualReturnDate.Value.ToString("hh:mm tt"));
                }
                else
                {
                    item.SubItems.Add("-");
                    item.SubItems.Add("-");
                }

                item.SubItems.Add(log.Duration);

                if (log.Status == "Checked Out")
                {
                    item.SubItems.Add("⏳ Active");
                    item.BackColor = Color.FromArgb(255, 255, 225); // Light yellow
                }
                else
                {
                    item.SubItems.Add("✅ Returned");
                    item.BackColor = Color.FromArgb(225, 255, 225); // Light green
                }

                listView.Items.Add(item);
            }

            tabPage.Controls.Add(listView);
        }

        private void CreateSummaryView(TabPage tabPage)
        {
            Panel summaryPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            int activeCount = checkoutLogs.Count(l => l.Status == "Checked Out");
            int returnedCount = checkoutLogs.Count(l => l.Status == "Returned");
            int uniqueItems = checkoutLogs.Select(l => l.EquipmentId).Distinct().Count();
            int totalItemsOut = checkoutLogs.Where(l => l.Status == "Checked Out").Sum(l => l.Quantity);
            int totalItemsReturned = checkoutLogs.Where(l => l.Status == "Returned").Sum(l => l.Quantity);

            // Summary cards
            int yPos = 20;
            CreateSummaryCard(summaryPanel, "📊 Summary Statistics", 20, yPos, 600, 30, true);
            yPos += 40;

            CreateSummaryCard(summaryPanel, $"Total Transactions: {checkoutLogs.Count}", 40, yPos, 300, 60, false);
            CreateSummaryCard(summaryPanel, $"Active Checkouts: {activeCount}", 360, yPos, 300, 60, false);
            yPos += 70;

            CreateSummaryCard(summaryPanel, $"Returned Items: {returnedCount}", 40, yPos, 300, 60, false);
            CreateSummaryCard(summaryPanel, $"Unique Equipment Used: {uniqueItems}", 360, yPos, 300, 60, false);
            yPos += 70;

            CreateSummaryCard(summaryPanel, $"Total Items Currently Out: {totalItemsOut}", 40, yPos, 300, 60, false);
            CreateSummaryCard(summaryPanel, $"Total Items Returned: {totalItemsReturned}", 360, yPos, 300, 60, false);
            yPos += 80;

            // Most active users
            Label lblActiveUsers = new Label
            {
                Text = "👥 Most Active Users",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(40, yPos),
                Size = new Size(300, 25),
                ForeColor = Color.FromArgb(33, 33, 33)
            };
            summaryPanel.Controls.Add(lblActiveUsers);
            yPos += 30;

            var topUsers = checkoutLogs
                .GroupBy(l => l.CheckedOutBy)
                .Select(g => new { User = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            foreach (var user in topUsers)
            {
                Label lblUser = new Label
                {
                    Text = $"• {user.User}: {user.Count} transactions",
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(60, yPos),
                    Size = new Size(300, 25),
                    ForeColor = Color.FromArgb(80, 80, 80)
                };
                summaryPanel.Controls.Add(lblUser);
                yPos += 25;
            }

            tabPage.Controls.Add(summaryPanel);
        }

        private void CreateSummaryCard(Panel parent, string text, int x, int y, int width, int height, bool isHeader)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = isHeader ? Color.FromArgb(240, 240, 240) : Color.White
            };

            if (!isHeader)
            {
                card.Paint += (s, e) =>
                {
                    ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                        Color.FromArgb(200, 200, 200), ButtonBorderStyle.Solid);
                };
            }

            Label label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", isHeader ? 12F : 10F, isHeader ? FontStyle.Bold : FontStyle.Regular),
                Location = new Point(10, 5),
                Size = new Size(width - 20, height - 10),
                ForeColor = isHeader ? primaryColor : Color.FromArgb(60, 60, 60),
                TextAlign = isHeader ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleLeft
            };

            card.Controls.Add(label);
            parent.Controls.Add(card);
        }

        private void ExportLogs()
        {
            // Create a simple text export
            string export = "EQUIPMENT CHECKOUT LOGS\n";
            export += "=======================\n\n";
            export += $"Generated: {DateTime.Now:MMM dd, yyyy hh:mm tt}\n";
            export += $"Total Transactions: {checkoutLogs.Count}\n\n";
            export += "DETAILED LOGS:\n";
            export += "--------------\n\n";

            foreach (var log in checkoutLogs.OrderByDescending(l => l.CheckOutDate))
            {
                export += $"Equipment: {log.EquipmentName}\n";
                export += $"Quantity: {log.Quantity}\n";
                export += $"Checked Out By: {log.CheckedOutBy}\n";
                export += $"Check Out: {log.CheckOutDate:MMM dd, yyyy hh:mm tt}\n";

                if (log.ActualReturnDate.HasValue)
                {
                    export += $"Returned: {log.ActualReturnDate.Value:MMM dd, yyyy hh:mm tt}\n";
                    export += $"Duration: {log.Duration}\n";
                }
                else
                {
                    export += $"Status: ACTIVE\n";
                }

                export += "-------------------\n";
            }

            string tempFile = System.IO.Path.GetTempFileName() + ".txt";
            System.IO.File.WriteAllText(tempFile, export);

            System.Diagnostics.Process.Start("notepad.exe", tempFile);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            EquipmentItem item = (EquipmentItem)btn.Tag;
            ShowEditDialog(item);
        }

        private void ShowEditDialog(EquipmentItem item)
        {
            bool isNewItem = item.Id == 0;
            string dialogTitle = isNewItem ? "Add New Equipment" : $"Edit Equipment - {item.Name}";

            using (var dialog = new Form())
            {
                dialog.Text = dialogTitle;
                dialog.Size = new Size(600, 550);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.BackColor = Color.White;

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 10,
                    Padding = new Padding(20),
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Absolute, 120),
                        new ColumnStyle(SizeType.Percent, 100)
                    }
                };

                // Name
                tlp.Controls.Add(new Label { Text = "Name:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                TextBox txtName = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill, Text = item.Name };
                tlp.Controls.Add(txtName, 1, 0);

                // Category
                tlp.Controls.Add(new Label { Text = "Category:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboCategory = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboCategory.Items.AddRange(categoryList.Select(c => c.Name).ToArray());
                if (!string.IsNullOrEmpty(item.Category))
                    cboCategory.SelectedItem = item.Category;
                else if (cboCategory.Items.Count > 0)
                    cboCategory.SelectedIndex = 0;
                tlp.Controls.Add(cboCategory, 1, 1);

                // Total Quantity
                tlp.Controls.Add(new Label { Text = "Total Quantity:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 2);
                NumericUpDown nudTotalQty = new NumericUpDown
                {
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 1,
                    Maximum = 1000,
                    Value = item.TotalQuantity > 0 ? item.TotalQuantity : 1,
                    Width = 100
                };
                tlp.Controls.Add(nudTotalQty, 1, 2);

                // Available Quantity
                tlp.Controls.Add(new Label { Text = "Available:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 3);
                NumericUpDown nudAvailableQty = new NumericUpDown
                {
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 0,
                    Maximum = item.TotalQuantity > 0 ? item.TotalQuantity : 1,
                    Value = item.AvailableQuantity > 0 ? item.AvailableQuantity : 1,
                    Width = 100
                };
                tlp.Controls.Add(nudAvailableQty, 1, 3);

                // Condition
                tlp.Controls.Add(new Label { Text = "Condition:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 4);
                ComboBox cboCondition = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboCondition.Items.AddRange(conditionList.Select(c => c.Name).ToArray());
                if (!string.IsNullOrEmpty(item.Condition))
                    cboCondition.SelectedItem = item.Condition;
                else if (cboCondition.Items.Count > 0)
                    cboCondition.SelectedIndex = 0;
                tlp.Controls.Add(cboCondition, 1, 4);

                // Location
                tlp.Controls.Add(new Label { Text = "Location:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 5);
                TextBox txtLocation = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill, Text = item.Location };
                tlp.Controls.Add(txtLocation, 1, 5);

                // Last Maintenance
                tlp.Controls.Add(new Label { Text = "Last Maintenance:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 6);
                DateTimePicker dtpMaintenance = new DateTimePicker
                {
                    Font = new Font("Segoe UI", 10F),
                    Value = item.LastMaintenance != DateTime.MinValue ? item.LastMaintenance : DateTime.Now,
                    Width = 200
                };
                tlp.Controls.Add(dtpMaintenance, 1, 6);

                // Needs Maintenance
                tlp.Controls.Add(new Label { Text = "Needs Maintenance:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 7);
                CheckBox chkMaintenance = new CheckBox
                {
                    Text = "Yes",
                    Font = new Font("Segoe UI", 10F),
                    Checked = item.NeedsMaintenance
                };
                tlp.Controls.Add(chkMaintenance, 1, 7);

                // Notes
                tlp.Controls.Add(new Label { Text = "Notes:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 8);
                TextBox txtNotes = new TextBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    Height = 60,
                    Text = item.Notes
                };
                tlp.Controls.Add(txtNotes, 1, 8);

                // Buttons
                FlowLayoutPanel btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 35),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F),
                    Cursor = Cursors.Hand
                };
                btnCancel.FlatAppearance.BorderSize = 0;
                btnCancel.Click += (s, args) => dialog.Close();

                Button btnSave = new Button
                {
                    Text = isNewItem ? "Add Equipment" : "Save Changes",
                    Size = new Size(120, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnSave.FlatAppearance.BorderSize = 0;

                Button btnDelete = null;
                if (!isNewItem)
                {
                    btnDelete = new Button
                    {
                        Text = "Delete",
                        Size = new Size(100, 35),
                        BackColor = dangerColor,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 10F),
                        Margin = new Padding(10, 5, 0, 5),
                        Cursor = Cursors.Hand
                    };
                    btnDelete.FlatAppearance.BorderSize = 0;
                    btnPanel.Controls.Add(btnDelete);
                }

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnSave);
                tlp.Controls.Add(btnPanel, 1, 9);

                dialog.Controls.Add(tlp);

                btnSave.Click += (s, args) =>
                {
                    // Validate inputs
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Please enter equipment name.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtLocation.Text))
                    {
                        MessageBox.Show("Please enter storage location.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            if (isNewItem)
                            {
                                // Insert new equipment
                                string insertQuery = @"
                                    INSERT INTO equipment_items 
                                    (name, category, total_quantity, available_quantity, condition_name, location, notes, needs_maintenance, last_maintenance) 
                                    VALUES (@name, @cat, @total, @available, @cond, @loc, @notes, @maint, @lastMaint);
                                    SELECT LAST_INSERT_ID();";

                                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                                cmd.Parameters.AddWithValue("@name", txtName.Text);
                                cmd.Parameters.AddWithValue("@cat", cboCategory.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@total", (int)nudTotalQty.Value);
                                cmd.Parameters.AddWithValue("@available", (int)nudAvailableQty.Value);
                                cmd.Parameters.AddWithValue("@cond", cboCondition.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@loc", txtLocation.Text);
                                cmd.Parameters.AddWithValue("@notes", txtNotes.Text);
                                cmd.Parameters.AddWithValue("@maint", chkMaintenance.Checked);
                                cmd.Parameters.AddWithValue("@lastMaint", dtpMaintenance.Value);

                                int newId = Convert.ToInt32(cmd.ExecuteScalar());

                                // Create new item
                                EquipmentItem newItem = new EquipmentItem
                                {
                                    Id = newId,
                                    Name = txtName.Text,
                                    Category = cboCategory.SelectedItem.ToString(),
                                    TotalQuantity = (int)nudTotalQty.Value,
                                    AvailableQuantity = (int)nudAvailableQty.Value,
                                    Condition = cboCondition.SelectedItem.ToString(),
                                    Location = txtLocation.Text,
                                    Notes = txtNotes.Text,
                                    NeedsMaintenance = chkMaintenance.Checked,
                                    LastMaintenance = dtpMaintenance.Value
                                };

                                equipmentList.Add(newItem);

                                // Log the addition
                                Activitylogs.Instance.LogEquipmentActivity(currentUser, "Added", newItem.Name,
                                    $"Category: {newItem.Category}, Quantity: {newItem.TotalQuantity}, Location: {newItem.Location}");

                                MessageBox.Show("Equipment added successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                // Store old values for logging
                                string oldName = item.Name;
                                string oldCategory = item.Category;
                                int oldTotal = item.TotalQuantity;
                                int oldAvailable = item.AvailableQuantity;
                                string oldCondition = item.Condition;
                                string oldLocation = item.Location;
                                bool oldMaintenance = item.NeedsMaintenance;

                                // Update existing equipment
                                string updateQuery = @"
                                    UPDATE equipment_items 
                                    SET name = @name, category = @cat, total_quantity = @total, 
                                        available_quantity = @available, condition_name = @cond,
                                        location = @loc, notes = @notes, needs_maintenance = @maint,
                                        last_maintenance = @lastMaint
                                    WHERE id = @id";

                                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                                cmd.Parameters.AddWithValue("@name", txtName.Text);
                                cmd.Parameters.AddWithValue("@cat", cboCategory.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@total", (int)nudTotalQty.Value);
                                cmd.Parameters.AddWithValue("@available", (int)nudAvailableQty.Value);
                                cmd.Parameters.AddWithValue("@cond", cboCondition.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@loc", txtLocation.Text);
                                cmd.Parameters.AddWithValue("@notes", txtNotes.Text);
                                cmd.Parameters.AddWithValue("@maint", chkMaintenance.Checked);
                                cmd.Parameters.AddWithValue("@lastMaint", dtpMaintenance.Value);
                                cmd.Parameters.AddWithValue("@id", item.Id);
                                cmd.ExecuteNonQuery();

                                // Update local object
                                item.Name = txtName.Text;
                                item.Category = cboCategory.SelectedItem.ToString();
                                item.TotalQuantity = (int)nudTotalQty.Value;
                                item.AvailableQuantity = (int)nudAvailableQty.Value;
                                item.Condition = cboCondition.SelectedItem.ToString();
                                item.Location = txtLocation.Text;
                                item.Notes = txtNotes.Text;
                                item.NeedsMaintenance = chkMaintenance.Checked;
                                item.LastMaintenance = dtpMaintenance.Value;

                                // Log the update
                                string changes = "";
                                if (oldName != item.Name) changes += $"Name: '{oldName}' → '{item.Name}' ";
                                if (oldCategory != item.Category) changes += $"Category: '{oldCategory}' → '{item.Category}' ";
                                if (oldTotal != item.TotalQuantity) changes += $"Total Qty: {oldTotal} → {item.TotalQuantity} ";
                                if (oldAvailable != item.AvailableQuantity) changes += $"Available: {oldAvailable} → {item.AvailableQuantity} ";
                                if (oldCondition != item.Condition) changes += $"Condition: '{oldCondition}' → '{item.Condition}' ";
                                if (oldLocation != item.Location) changes += $"Location: '{oldLocation}' → '{item.Location}' ";
                                if (oldMaintenance != item.NeedsMaintenance)
                                    changes += $"Maintenance: {(oldMaintenance ? "Needed" : "Not needed")} → {(item.NeedsMaintenance ? "Needed" : "Not needed")} ";

                                Activitylogs.Instance.LogEquipmentActivity(currentUser, "Updated", item.Name, changes);

                                MessageBox.Show("Equipment updated successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message,
                            isNewItem ? "Error adding new equipment" : $"Error updating {item.Name}");
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                if (btnDelete != null)
                {
                    btnDelete.Click += (s, args) =>
                    {
                        DialogResult result = MessageBox.Show($"Are you sure you want to delete '{item.Name}'?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                using (MySqlConnection conn = new MySqlConnection(connectionString))
                                {
                                    conn.Open();
                                    string deleteQuery = "DELETE FROM equipment_items WHERE id = @id";
                                    MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                                    cmd.Parameters.AddWithValue("@id", item.Id);
                                    cmd.ExecuteNonQuery();
                                }

                                string deletedName = item.Name;
                                equipmentList.Remove(item);

                                // Log the deletion
                                Activitylogs.Instance.LogEquipmentActivity(currentUser, "Deleted", deletedName);

                                MessageBox.Show("Equipment deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dialog.Close();
                                DisplayEquipment();
                            }
                            catch (Exception ex)
                            {
                                Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, $"Error deleting {item.Name}");
                                MessageBox.Show($"Database error: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };
                }

                dialog.ShowDialog(this);
            }
        }

        private void BtnMaintenance_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            EquipmentItem item = (EquipmentItem)btn.Tag;

            using (var dialog = new Form())
            {
                dialog.Text = $"Complete Maintenance - {item.Name}";
                dialog.Size = new Size(400, 250);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.BackColor = Color.White;

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 3,
                    Padding = new Padding(20),
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Absolute, 120),
                        new ColumnStyle(SizeType.Percent, 100)
                    }
                };

                // Maintenance date
                tlp.Controls.Add(new Label { Text = "Maintenance Date:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                DateTimePicker dtpDate = new DateTimePicker
                {
                    Value = DateTime.Now,
                    Font = new Font("Segoe UI", 10F),
                    Width = 200
                };
                tlp.Controls.Add(dtpDate, 1, 0);

                // New condition
                tlp.Controls.Add(new Label { Text = "New Condition:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboCondition = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Width = 150
                };
                cboCondition.Items.AddRange(conditionList.Select(c => c.Name).ToArray());
                cboCondition.SelectedItem = "Good";
                tlp.Controls.Add(cboCondition, 1, 1);

                // Buttons
                FlowLayoutPanel btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 35),
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F),
                    Cursor = Cursors.Hand
                };
                btnCancel.FlatAppearance.BorderSize = 0;
                btnCancel.Click += (s, args) => dialog.Close();

                Button btnComplete = new Button
                {
                    Text = "Complete",
                    Size = new Size(100, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnComplete.FlatAppearance.BorderSize = 0;

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnComplete);
                tlp.Controls.Add(btnPanel, 1, 2);

                dialog.Controls.Add(tlp);

                btnComplete.Click += (s, args) =>
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            string updateQuery = @"
                                UPDATE equipment_items 
                                SET last_maintenance = @date, condition_name = @cond, needs_maintenance = FALSE 
                                WHERE id = @id";

                            MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                            cmd.Parameters.AddWithValue("@date", dtpDate.Value);
                            cmd.Parameters.AddWithValue("@cond", cboCondition.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@id", item.Id);
                            cmd.ExecuteNonQuery();
                        }

                        string oldCondition = item.Condition;
                        item.LastMaintenance = dtpDate.Value;
                        item.Condition = cboCondition.SelectedItem.ToString();
                        item.NeedsMaintenance = false;

                        // Log the maintenance
                        Activitylogs.Instance.LogEquipmentMaintenance(currentUser, item.Name, item.Condition);

                        if (oldCondition != item.Condition)
                        {
                            Activitylogs.Instance.AddLogEntry(currentUser, "Equipment Condition Changed",
                                $"Equipment '{item.Name}' condition changed from {oldCondition} to {item.Condition} after maintenance", "Info", "GameEquipment");
                        }

                        MessageBox.Show($"Maintenance completed for {item.Name}.\n" +
                                      $"New condition: {item.Condition}",
                            "Maintenance Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, $"Error completing maintenance for {item.Name}");
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                dialog.ShowDialog(this);
            }
        }

        private void FilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEquipment();
        }

        private void btnAddEquipment_Click(object sender, EventArgs e)
        {
            // Create new equipment
            EquipmentItem newItem = new EquipmentItem
            {
                Id = 0, // 0 indicates new item
                Name = "",
                Category = categoryList.FirstOrDefault()?.Name ?? "",
                TotalQuantity = 1,
                AvailableQuantity = 1,
                Condition = "Good",
                Location = "",
                Notes = "",
                NeedsMaintenance = false,
                LastMaintenance = DateTime.Now
            };

            ShowEditDialog(newItem);
        }

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            // Show the management overlay
            managementOverlay.Visible = true;
            managementOverlay.BringToFront();
            btnManageCategories.Text = "Hide Management";

            // Load data
            LoadCategories();
            LoadConditions();

            Activitylogs.Instance.AddLogEntry(currentUser, "Management View", "Opened categories and conditions management", "Info", "GameEquipment");
        }

        private void btnCloseManagement_Click(object sender, EventArgs e)
        {
            // Hide the management overlay
            managementOverlay.Visible = false;
            btnManageCategories.Text = "Manage Categories";
        }

        private void LoadCategories()
        {
            if (categoriesFlowPanel.InvokeRequired)
            {
                categoriesFlowPanel.Invoke(new MethodInvoker(() => LoadCategories()));
                return;
            }

            categoriesFlowPanel.Controls.Clear();
            foreach (var category in categoryList)
            {
                var card = CreateCategoryCard(category);
                categoriesFlowPanel.Controls.Add(card);
            }
        }

        private Panel CreateCategoryCard(Category category)
        {
            Panel card = new Panel
            {
                Width = 250,
                Height = 100,
                BackColor = cardBgColor,
                Margin = new Padding(10),
                Tag = category
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
            };

            Label lblIcon = new Label
            {
                Text = category.Icon,
                Font = new Font("Segoe UI", 20F),
                Location = new Point(10, 10),
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblName = new Label
            {
                Text = category.Name,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(60, 15),
                Size = new Size(180, 20),
                ForeColor = Color.FromArgb(33, 33, 33)
            };

            Label lblDesc = new Label
            {
                Text = category.Description,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(60, 35),
                Size = new Size(180, 20),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            Label lblCount = new Label
            {
                Text = $"Items: {category.ItemCount}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(60, 55),
                Size = new Size(100, 20),
                ForeColor = primaryColor
            };

            card.Controls.AddRange(new Control[] { lblIcon, lblName, lblDesc, lblCount });
            return card;
        }

        private void LoadConditions()
        {
            if (conditionsFlowPanel.InvokeRequired)
            {
                conditionsFlowPanel.Invoke(new MethodInvoker(() => LoadConditions()));
                return;
            }

            conditionsFlowPanel.Controls.Clear();
            foreach (var condition in conditionList)
            {
                var card = CreateConditionCard(condition);
                conditionsFlowPanel.Controls.Add(card);
            }
        }

        private Panel CreateConditionCard(Condition condition)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 80,
                BackColor = cardBgColor,
                Margin = new Padding(10),
                Tag = condition
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
            };

            // Color indicator
            Panel colorIndicator = new Panel
            {
                Width = 20,
                Height = 20,
                Location = new Point(10, 15),
                BackColor = ColorTranslator.FromHtml(condition.Color)
            };

            Label lblName = new Label
            {
                Text = condition.Name,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(40, 10),
                Size = new Size(150, 20),
                ForeColor = Color.FromArgb(33, 33, 33)
            };

            Label lblDesc = new Label
            {
                Text = condition.Description,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(40, 30),
                Size = new Size(150, 35),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            card.Controls.AddRange(new Control[] { colorIndicator, lblName, lblDesc });
            return card;
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string newCategory = ShowInputDialog("Add Category", "Enter new category name:");
            if (!string.IsNullOrWhiteSpace(newCategory))
            {
                if (categoryList.Any(c => c.Name.Equals(newCategory, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Category already exists!", "Duplicate",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "INSERT INTO equipment_categories (name, description, icon) VALUES (@name, @desc, @icon); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@name", newCategory);
                        cmd.Parameters.AddWithValue("@desc", $"{newCategory} category");
                        cmd.Parameters.AddWithValue("@icon", "📦");

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        categoryList.Add(new Category
                        {
                            Id = newId,
                            Name = newCategory,
                            Description = $"{newCategory} category",
                            Icon = "📦",
                            ItemCount = 0
                        });

                        LoadCategories();
                    }

                    // Log the addition
                    Activitylogs.Instance.AddLogEntry(currentUser, "Category Added", $"New category '{newCategory}' was added", "Info", "GameEquipment");

                    MessageBox.Show($"Category '{newCategory}' added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, $"Error adding category {newCategory}");
                    MessageBox.Show($"Error adding: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            string newCondition = ShowInputDialog("Add Condition", "Enter new condition name:");
            if (!string.IsNullOrWhiteSpace(newCondition))
            {
                if (conditionList.Any(c => c.Name.Equals(newCondition, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Condition already exists!", "Duplicate",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "INSERT INTO equipment_conditions (name, color, description) VALUES (@name, @color, @desc); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@name", newCondition);
                        cmd.Parameters.AddWithValue("@color", "#808080");
                        cmd.Parameters.AddWithValue("@desc", $"{newCondition} condition");

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        conditionList.Add(new Condition
                        {
                            Id = newId,
                            Name = newCondition,
                            Color = "#808080",
                            Description = $"{newCondition} condition"
                        });

                        LoadConditions();
                    }

                    // Log the addition
                    Activitylogs.Instance.AddLogEntry(currentUser, "Condition Added", $"New condition '{newCondition}' was added", "Info", "GameEquipment");

                    MessageBox.Show($"Condition '{newCondition}' added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Activitylogs.Instance.LogError(currentUser, "GameEquipment", ex.Message, $"Error adding condition {newCondition}");
                    MessageBox.Show($"Error adding: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ShowInputDialog(string title, string promptText)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = title;
                inputForm.Size = new Size(400, 180);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.BackColor = Color.White;

                Label label = new Label()
                {
                    Text = promptText,
                    Left = 20,
                    Top = 20,
                    Width = 340,
                    Font = new Font("Segoe UI", 10F)
                };

                TextBox textBox = new TextBox()
                {
                    Left = 20,
                    Top = 50,
                    Width = 340,
                    Font = new Font("Segoe UI", 10F)
                };

                Button buttonOk = new Button()
                {
                    Text = "OK",
                    Left = 200,
                    Top = 90,
                    Width = 80,
                    Height = 30,
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.OK,
                    Font = new Font("Segoe UI", 10F),
                    Cursor = Cursors.Hand
                };
                buttonOk.FlatAppearance.BorderSize = 0;

                Button buttonCancel = new Button()
                {
                    Text = "Cancel",
                    Left = 290,
                    Top = 90,
                    Width = 80,
                    Height = 30,
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.Cancel,
                    Font = new Font("Segoe UI", 10F),
                    Cursor = Cursors.Hand
                };
                buttonCancel.FlatAppearance.BorderSize = 0;

                inputForm.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
                inputForm.AcceptButton = buttonOk;
                inputForm.CancelButton = buttonCancel;

                return inputForm.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }
    }
}