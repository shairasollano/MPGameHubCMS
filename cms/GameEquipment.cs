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
            public DateTime? ExpectedReturnDate { get; set; }
            public DateTime? ActualReturnDate { get; set; }
            public string Notes { get; set; }
            public string Status { get; set; } // "Checked Out", "Returned", "Overdue"
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

            // Hide management panel initially
            managementPanel.Visible = false;
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
                        expected_return_date DATE,
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

                    // Load checkout logs
                    checkoutLogs.Clear();
                    string logQuery = @"
                        SELECT id, equipment_id, equipment_name, quantity, checked_out_by,
                               check_out_date, expected_return_date, actual_return_date, notes, status
                        FROM equipment_checkout_logs ORDER BY check_out_date DESC";

                    MySqlCommand logCmd = new MySqlCommand(logQuery, connection);
                    using (MySqlDataReader reader = logCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            checkoutLogs.Add(new CheckoutLog
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                EquipmentId = Convert.ToInt32(reader["equipment_id"]),
                                EquipmentName = reader["equipment_name"].ToString(),
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                CheckedOutBy = reader["checked_out_by"].ToString(),
                                CheckOutDate = Convert.ToDateTime(reader["check_out_date"]),
                                ExpectedReturnDate = reader["expected_return_date"] != DBNull.Value ?
                                    Convert.ToDateTime(reader["expected_return_date"]) : (DateTime?)null,
                                ActualReturnDate = reader["actual_return_date"] != DBNull.Value ?
                                    Convert.ToDateTime(reader["actual_return_date"]) : (DateTime?)null,
                                Notes = reader["notes"]?.ToString() ?? "",
                                Status = reader["status"].ToString()
                            });
                        }
                    }

                    // Update category item counts
                    foreach (var category in categoryList)
                    {
                        category.ItemCount = equipmentList.Count(e => e.Category == category.Name);
                    }
                }
            }
            catch (Exception ex)
            {
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
        }

        private void StyleButtons()
        {
            StyleButton(btnAddEquipment, successColor);
            StyleButton(btnManageCategories, primaryColor);
            StyleButton(btnAddCategory, successColor);
            StyleButton(btnAddCondition, successColor);
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
                dialog.Size = new Size(400, 300);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.BackColor = Color.White;

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 5,
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
                TextBox txtCheckedBy = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill };
                tlp.Controls.Add(txtCheckedBy, 1, 2);

                // Expected return date
                tlp.Controls.Add(new Label { Text = "Expected return:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 3);
                DateTimePicker dtpReturn = new DateTimePicker
                {
                    Value = DateTime.Now.AddDays(7),
                    MinDate = DateTime.Now,
                    Font = new Font("Segoe UI", 10F),
                    Width = 200
                };
                tlp.Controls.Add(dtpReturn, 1, 3);

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

                Button btnConfirm = new Button
                {
                    Text = "Check Out",
                    Size = new Size(100, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
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
                                (equipment_id, equipment_name, quantity, checked_out_by, check_out_date, expected_return_date, status) 
                                VALUES (@eqId, @eqName, @qty, @by, @outDate, @returnDate, 'Checked Out')";

                            MySqlCommand logCmd = new MySqlCommand(insertLog, conn);
                            logCmd.Parameters.AddWithValue("@eqId", item.Id);
                            logCmd.Parameters.AddWithValue("@eqName", item.Name);
                            logCmd.Parameters.AddWithValue("@qty", quantity);
                            logCmd.Parameters.AddWithValue("@by", txtCheckedBy.Text);
                            logCmd.Parameters.AddWithValue("@outDate", DateTime.Now);
                            logCmd.Parameters.AddWithValue("@returnDate", dtpReturn.Value);
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
                            ExpectedReturnDate = dtpReturn.Value,
                            Status = "Checked Out"
                        });

                        MessageBox.Show($"Successfully checked out {quantity} {item.Name}(s) to {txtCheckedBy.Text}.\n" +
                                      $"Expected return: {dtpReturn.Value:MMM dd, yyyy}\n" +
                                      $"Remaining stock: {item.AvailableQuantity}",
                            "Check Out Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnConfirm);
                tlp.Controls.Add(btnPanel, 1, 4);

                dialog.Controls.Add(tlp);
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
                    RowCount = 4,
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

                // Condition after use
                tlp.Controls.Add(new Label { Text = "Condition:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 2);
                ComboBox cboCondition = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Width = 150
                };
                cboCondition.Items.AddRange(conditionList.Select(c => c.Name).ToArray());
                cboCondition.SelectedItem = item.Condition;
                tlp.Controls.Add(cboCondition, 1, 2);

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

                Button btnConfirm = new Button
                {
                    Text = "Check In",
                    Size = new Size(100, 35),
                    BackColor = warningColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnConfirm.FlatAppearance.BorderSize = 0;
                btnConfirm.Click += (s, args) =>
                {
                    int quantity = (int)nudQuantity.Value;
                    string newCondition = cboCondition.SelectedItem.ToString();

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            // Update equipment stock and condition
                            string updateEq = "UPDATE equipment_items SET available_quantity = available_quantity + @qty, condition_name = @cond WHERE id = @id";
                            MySqlCommand updateCmd = new MySqlCommand(updateEq, conn);
                            updateCmd.Parameters.AddWithValue("@qty", quantity);
                            updateCmd.Parameters.AddWithValue("@cond", newCondition);
                            updateCmd.Parameters.AddWithValue("@id", item.Id);
                            updateCmd.ExecuteNonQuery();

                            // Create a return log
                            string insertLog = @"
                                INSERT INTO equipment_checkout_logs 
                                (equipment_id, equipment_name, quantity, checked_out_by, check_out_date, actual_return_date, status, notes) 
                                VALUES (@eqId, @eqName, @qty, 'Manual Return', @outDate, @returnDate, 'Returned', 'Manual check-in')";

                            MySqlCommand logCmd = new MySqlCommand(insertLog, conn);
                            logCmd.Parameters.AddWithValue("@eqId", item.Id);
                            logCmd.Parameters.AddWithValue("@eqName", item.Name);
                            logCmd.Parameters.AddWithValue("@qty", quantity);
                            logCmd.Parameters.AddWithValue("@outDate", DateTime.Now.AddDays(-1));
                            logCmd.Parameters.AddWithValue("@returnDate", DateTime.Now);
                            logCmd.ExecuteNonQuery();
                        }

                        // Update local object
                        item.AvailableQuantity += quantity;
                        item.Condition = newCondition;

                        MessageBox.Show($"Successfully checked in {quantity} {item.Name}(s).\n" +
                                      $"Current stock: {item.AvailableQuantity}/{item.TotalQuantity}",
                            "Check In Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnConfirm);
                tlp.Controls.Add(btnPanel, 1, 3);

                dialog.Controls.Add(tlp);
                dialog.ShowDialog(this);
            }
        }

        private void SelectCheckoutToReturn(EquipmentItem item, List<CheckoutLog> activeCheckouts)
        {
            using (var dialog = new Form())
            {
                dialog.Text = $"Select Checkout to Return - {item.Name}";
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
                listView.Columns.Add("Expected Return", 150);
                listView.Columns.Add("Days Out", 80);

                foreach (var checkout in activeCheckouts)
                {
                    var daysOut = (DateTime.Now - checkout.CheckOutDate).Days;
                    var item2 = new ListViewItem(checkout.CheckedOutBy);
                    item2.SubItems.Add(checkout.Quantity.ToString());
                    item2.SubItems.Add(checkout.CheckOutDate.ToString("MMM dd, yyyy"));
                    item2.SubItems.Add(checkout.ExpectedReturnDate?.ToString("MMM dd, yyyy") ?? "N/A");
                    item2.SubItems.Add(daysOut.ToString());
                    item2.Tag = checkout;
                    listView.Items.Add(item2);
                }

                Button btnReturn = new Button
                {
                    Text = "Return Selected",
                    Size = new Size(150, 40),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Location = new Point(220, 310),
                    Cursor = Cursors.Hand
                };
                btnReturn.FlatAppearance.BorderSize = 0;

                btnReturn.Click += (s, args) =>
                {
                    if (listView.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select a checkout to return.",
                            "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var selectedCheckout = (CheckoutLog)listView.SelectedItems[0].Tag;

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            // Update equipment stock
                            string updateEq = "UPDATE equipment_items SET available_quantity = available_quantity + @qty WHERE id = @id";
                            MySqlCommand updateCmd = new MySqlCommand(updateEq, conn);
                            updateCmd.Parameters.AddWithValue("@qty", selectedCheckout.Quantity);
                            updateCmd.Parameters.AddWithValue("@id", item.Id);
                            updateCmd.ExecuteNonQuery();

                            // Update checkout log
                            string updateLog = "UPDATE equipment_checkout_logs SET actual_return_date = @returnDate, status = 'Returned' WHERE id = @id";
                            MySqlCommand logCmd = new MySqlCommand(updateLog, conn);
                            logCmd.Parameters.AddWithValue("@returnDate", DateTime.Now);
                            logCmd.Parameters.AddWithValue("@id", selectedCheckout.Id);
                            logCmd.ExecuteNonQuery();
                        }

                        // Update local objects
                        item.AvailableQuantity += selectedCheckout.Quantity;
                        selectedCheckout.ActualReturnDate = DateTime.Now;
                        selectedCheckout.Status = "Returned";

                        MessageBox.Show($"Successfully returned {selectedCheckout.Quantity} {item.Name}(s) from {selectedCheckout.CheckedOutBy}.",
                            "Return Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                dialog.Controls.Add(listView);
                dialog.Controls.Add(btnReturn);
                dialog.ShowDialog(this);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            EquipmentItem item = (EquipmentItem)btn.Tag;
            ShowEditDialog(item);
        }

        private void ShowEditDialog(EquipmentItem item)
        {
            using (var dialog = new Form())
            {
                dialog.Text = $"Edit Equipment - {item.Name}";
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
                cboCategory.SelectedItem = item.Category;
                tlp.Controls.Add(cboCategory, 1, 1);

                // Total Quantity
                tlp.Controls.Add(new Label { Text = "Total Quantity:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 2);
                NumericUpDown nudTotalQty = new NumericUpDown
                {
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 1,
                    Maximum = 1000,
                    Value = item.TotalQuantity,
                    Width = 100
                };
                tlp.Controls.Add(nudTotalQty, 1, 2);

                // Available Quantity
                tlp.Controls.Add(new Label { Text = "Available:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 3);
                NumericUpDown nudAvailableQty = new NumericUpDown
                {
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 0,
                    Maximum = item.TotalQuantity,
                    Value = item.AvailableQuantity,
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
                cboCondition.SelectedItem = item.Condition;
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
                    Value = item.LastMaintenance,
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
                    Text = "Save Changes",
                    Size = new Size(120, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnSave.FlatAppearance.BorderSize = 0;
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
                        }

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

                        MessageBox.Show("Equipment updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnSave);
                tlp.Controls.Add(btnPanel, 1, 9);

                dialog.Controls.Add(tlp);
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

                        item.LastMaintenance = dtpDate.Value;
                        item.Condition = cboCondition.SelectedItem.ToString();
                        item.NeedsMaintenance = false;

                        MessageBox.Show($"Maintenance completed for {item.Name}.\n" +
                                      $"New condition: {item.Condition}",
                            "Maintenance Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnComplete);
                tlp.Controls.Add(btnPanel, 1, 2);

                dialog.Controls.Add(tlp);
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
                Id = equipmentList.Count > 0 ? equipmentList.Max(i => i.Id) + 1 : 1,
                Name = "New Equipment",
                Category = categoryList.First().Name,
                TotalQuantity = 1,
                AvailableQuantity = 1,
                Condition = "Good",
                Location = "TBD",
                Notes = "",
                NeedsMaintenance = false,
                LastMaintenance = DateTime.Now
            };

            ShowEditDialog(newItem);

            // Add to list if saved
            if (!equipmentList.Contains(newItem))
            {
                equipmentList.Add(newItem);
            }

            DisplayEquipment();
        }

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            managementPanel.Visible = !managementPanel.Visible;
            btnManageCategories.Text = managementPanel.Visible ? "Hide Categories" : "Manage Categories";

            if (managementPanel.Visible)
            {
                LoadCategories();
                LoadConditions();
            }
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
            MessageBox.Show("Add New Category\n\nThis would open a form to add new equipment categories.",
                "Add Category", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add New Condition\n\nThis would open a form to add new equipment conditions.",
                "Add Condition", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}