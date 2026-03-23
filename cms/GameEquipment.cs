using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class GameEquipment : UserControl
    {
        // Modern color scheme
        private static readonly Color primaryColor = Color.FromArgb(67, 97, 238);
        private static readonly Color successColor = Color.FromArgb(76, 175, 80);
        private static readonly Color dangerColor = Color.FromArgb(244, 67, 54);
        private static readonly Color warningColor = Color.FromArgb(255, 152, 0);
        private static readonly Color infoColor = Color.FromArgb(0, 150, 255);
        private static readonly Color cardBgColor = Color.White;
        private static readonly Color hoverColor = Color.FromArgb(245, 247, 250);

        // Current user - will be set from login
        private string currentUser = "";
        private string currentUserRole = "";

        // Database connection
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";
        private MySqlConnection connection;

        // Data lists
        private List<EquipmentItem> equipmentList;
        private List<Category> categoryList;
        private List<Condition> conditionList;

        // Model classes
        private class EquipmentItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public int TotalQuantity { get; set; }
            public int AvailableQuantity { get; set; }
            public string Condition { get; set; }
            public string Location { get; set; }
            public string Notes { get; set; }
            public bool NeedsMaintenance { get; set; }
            public DateTime LastMaintenance { get; set; }
            public string ImagePath { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

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

        public GameEquipment()
        {
            InitializeComponent();

            // Get current user from GlobalLogger if available
            if (!string.IsNullOrEmpty(GlobalLogger.CurrentUsername))
            {
                currentUser = GlobalLogger.CurrentUsername;
                currentUserRole = GlobalLogger.CurrentUserRole;
            }
            else
            {
                currentUser = "System";
                currentUserRole = "ADMIN";
            }

            // Add event handlers
            if (filterCombo != null)
                filterCombo.SelectedIndexChanged += FilterCombo_SelectedIndexChanged;
            if (btnAddEquipment != null)
                btnAddEquipment.Click += btnAddEquipment_Click;
            if (btnManageCategories != null)
                btnManageCategories.Click += btnManageCategories_Click;
            if (btnViewLogs != null)
                btnViewLogs.Click += btnViewLogs_Click;
            if (btnAddCategory != null)
                btnAddCategory.Click += btnAddCategory_Click;
            if (btnCloseManagement != null)
                btnCloseManagement.Click += btnCloseManagement_Click;
            if (managementTabs != null)
                managementTabs.SelectedIndexChanged += ManagementTabs_SelectedIndexChanged;

            // Style the tabs
            StyleTabs();

            // Set up the initial UI state
            InitializeUI();

            // Initialize lists
            equipmentList = new List<EquipmentItem>();
            categoryList = new List<Category>();
            conditionList = new List<Condition>();

            // Initialize database
            InitializeDatabase();

            StyleButtons();
            DisplayEquipment();
            LoadCategories();
            LoadConditions();

            // Setup filter initial value
            if (filterCombo != null)
            {
                filterCombo.SelectedIndex = 0;
            }

            // Hide management overlay initially
            if (managementOverlay != null)
                managementOverlay.Visible = false;

            // Log that GameEquipment module was opened
            try
            {
                GlobalLogger.LogInfo("GameEquipment", $"User {currentUser} ({currentUserRole}) opened Game Equipment module");
            }
            catch { }
        }

        // Method to set current user (called from Form1)
        public void SetCurrentUser(string username, string role)
        {
            currentUser = username;
            currentUserRole = role;

            // Log that user accessed GameEquipment
            try
            {
                GlobalLogger.LogInfo("GameEquipment", $"User {username} ({role}) opened Game Equipment module");
            }
            catch { }
        }

        private void StyleTabs()
        {
            if (managementTabs != null)
            {
                managementTabs.DrawMode = TabDrawMode.OwnerDrawFixed;
                managementTabs.DrawItem += (s, e) =>
                {
                    Rectangle tabRect = managementTabs.GetTabRect(e.Index);
                    Color tabPrimaryColor = Color.FromArgb(79, 70, 229);

                    using (Brush brush = new SolidBrush(e.Index == managementTabs.SelectedIndex ? tabPrimaryColor : Color.FromArgb(243, 244, 246)))
                    using (Brush textBrush = new SolidBrush(e.Index == managementTabs.SelectedIndex ? Color.White : Color.FromArgb(75, 85, 99)))
                    {
                        e.Graphics.FillRectangle(brush, tabRect);
                        string tabText = managementTabs.TabPages[e.Index].Text;
                        StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        e.Graphics.DrawString(tabText, e.Font, textBrush, tabRect, sf);
                    }
                };
            }
        }

        private void InitializeUI()
        {
            if (lblTitle != null)
            {
                lblTitle.Text = "Game Equipment";
                lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
                lblTitle.ForeColor = Color.FromArgb(33, 33, 33);
            }

            if (filterCombo != null)
            {
                filterCombo.Items.Clear();
                filterCombo.Items.AddRange(new object[] {
                    "All Equipment",
                    "Available Only",
                    "Unavailable Only",
                    "Needs Maintenance"
                });
                filterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            if (mainContainer != null)
            {
                mainContainer.Visible = true;
                mainContainer.Dock = DockStyle.Fill;
            }

            if (equipmentFlowPanel != null)
            {
                equipmentFlowPanel.AutoScroll = true;
                equipmentFlowPanel.WrapContents = true;
                equipmentFlowPanel.FlowDirection = FlowDirection.LeftToRight;
                equipmentFlowPanel.Padding = new Padding(10);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Timer autoLoadTimer = new Timer();
            autoLoadTimer.Interval = 500;
            autoLoadTimer.Tick += (s, args) =>
            {
                autoLoadTimer.Stop();
                if (equipmentList == null || equipmentList.Count == 0)
                {
                    RefreshDataFromDatabase();
                    DisplayEquipment();
                }
            };
            autoLoadTimer.Start();
        }

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
                try
                {
                    GlobalLogger.LogError("GameEquipment", ex.Message, "Database initialization error");
                }
                catch { }

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

                string createCategoriesTable = @"
                    CREATE TABLE IF NOT EXISTS equipment_categories (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(100) NOT NULL UNIQUE,
                        description TEXT,
                        icon VARCHAR(10) DEFAULT '📦',
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                new MySqlCommand(createCategoriesTable, connection).ExecuteNonQuery();

                string createConditionsTable = @"
                    CREATE TABLE IF NOT EXISTS equipment_conditions (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(50) NOT NULL UNIQUE,
                        color VARCHAR(20) DEFAULT '#808080',
                        description TEXT,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                new MySqlCommand(createConditionsTable, connection).ExecuteNonQuery();

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

                InsertDefaultDataIfEmpty();
            }
        }

        private void InsertDefaultDataIfEmpty()
        {
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

                try
                {
                    GlobalLogger.LogInfo("GameEquipment", "Default equipment categories were created");
                }
                catch { }
            }

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

                try
                {
                    GlobalLogger.LogInfo("GameEquipment", "Default equipment conditions were created");
                }
                catch { }
            }

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

                try
                {
                    GlobalLogger.LogInfo("GameEquipment", "Default equipment items were created");
                }
                catch { }
            }
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

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
                                ItemCount = 0
                            });
                        }
                    }

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

                    foreach (var category in categoryList)
                    {
                        category.ItemCount = equipmentList.Count(e => e.Category == category.Name);
                    }
                }

                try
                {
                    GlobalLogger.LogInfo("GameEquipment", $"Loaded {equipmentList.Count} equipment items from database");
                }
                catch { }
            }
            catch (Exception ex)
            {
                try
                {
                    GlobalLogger.LogError("GameEquipment", ex.Message, "Error loading data from database");
                }
                catch { }

                MessageBox.Show($"Error loading data: {ex.Message}\nUsing sample data.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InitializeSampleData();
            }
        }

        private void InitializeSampleData()
        {
            categoryList.Add(new Category { Id = 1, Name = "Rackets", Description = "Tennis, Badminton, Squash rackets", ItemCount = 15, Icon = "🏸" });
            categoryList.Add(new Category { Id = 2, Name = "Balls", Description = "Various game balls", ItemCount = 50, Icon = "⚽" });
            categoryList.Add(new Category { Id = 3, Name = "Nets", Description = "Game nets and accessories", ItemCount = 8, Icon = "🏐" });
            categoryList.Add(new Category { Id = 4, Name = "Protective Gear", Description = "Helmets, pads, guards", ItemCount = 20, Icon = "🛡️" });
            categoryList.Add(new Category { Id = 5, Name = "Footwear", Description = "Sports shoes", ItemCount = 25, Icon = "👟" });

            conditionList.Add(new Condition { Id = 1, Name = "New", Color = "#4CAF50", Description = "Brand new condition" });
            conditionList.Add(new Condition { Id = 2, Name = "Good", Color = "#2196F3", Description = "Good condition, minor wear" });
            conditionList.Add(new Condition { Id = 3, Name = "Fair", Color = "#FF9800", Description = "Fair condition, visible wear" });
            conditionList.Add(new Condition { Id = 4, Name = "Poor", Color = "#F44336", Description = "Poor condition, needs replacement" });
            conditionList.Add(new Condition { Id = 5, Name = "Maintenance", Color = "#9C27B0", Description = "Needs maintenance" });

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
            if (btnAddEquipment != null) StyleButton(btnAddEquipment, successColor);
            if (btnManageCategories != null) StyleButton(btnManageCategories, primaryColor);
            if (btnAddCategory != null) StyleButton(btnAddCategory, successColor);
            if (btnCloseManagement != null) StyleButton(btnCloseManagement, dangerColor);
            if (btnViewLogs != null) StyleButton(btnViewLogs, infoColor);
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
            if (equipmentFlowPanel == null) return;

            if (equipmentFlowPanel.InvokeRequired)
            {
                equipmentFlowPanel.Invoke(new MethodInvoker(() => DisplayEquipment()));
                return;
            }

            equipmentFlowPanel.Controls.Clear();

            if (equipmentList == null || equipmentList.Count == 0)
            {
                Label lblNoData = new Label
                {
                    Text = "No equipment found. Click '+ Add Equipment' to add new items.",
                    Font = new Font("Segoe UI", 12F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(400, 50),
                    Location = new Point((equipmentFlowPanel.Width - 400) / 2, 50)
                };
                equipmentFlowPanel.Controls.Add(lblNoData);
                return;
            }

            string filter = filterCombo?.SelectedItem?.ToString() ?? "All Equipment";
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
            var cat = categoryList?.FirstOrDefault(c => c.Name == category);
            return cat?.Icon ?? "📦";
        }

        private Color GetConditionColor(string condition)
        {
            var cond = conditionList?.FirstOrDefault(c => c.Name == condition);
            if (cond != null) return ColorTranslator.FromHtml(cond.Color);

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

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
            };

            Panel statusBar = new Panel
            {
                Height = 6,
                Dock = DockStyle.Top,
                BackColor = GetConditionColor(item.Condition)
            };

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

            Label lblName = new Label
            {
                Text = item.Name,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 50),
                Size = new Size(310, 25),
                ForeColor = Color.FromArgb(33, 33, 33)
            };

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

            Label lblMaintenance = new Label
            {
                Text = $"Last maintenance: {item.LastMaintenance:MMM dd, yyyy}",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                Location = new Point(15, 155),
                Size = new Size(310, 15),
                ForeColor = item.NeedsMaintenance ? dangerColor : Color.FromArgb(140, 140, 140)
            };

            Label lblNotes = new Label
            {
                Text = item.Notes,
                Font = new Font("Segoe UI", 8F),
                Location = new Point(15, 175),
                Size = new Size(310, 30),
                ForeColor = Color.FromArgb(140, 140, 140)
            };

            Button btnEdit = new Button
            {
                Text = "Edit",
                FlatStyle = FlatStyle.Flat,
                BackColor = primaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(75, 35),
                Location = new Point(15, 215),
                Tag = item,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            Button btnMaintenance = new Button
            {
                Text = "🔧",
                FlatStyle = FlatStyle.Flat,
                BackColor = item.NeedsMaintenance ? warningColor : Color.LightGray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(45, 35),
                Location = new Point(280, 215),
                Tag = item,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
                Enabled = item.NeedsMaintenance
            };
            btnMaintenance.Click += BtnMaintenance_Click;

            card.Controls.AddRange(new Control[] {
                statusBar, categoryBadge, lblName, stockPanel,
                lblCondition, lblLocation, lblMaintenance, lblNotes,
                btnEdit, btnMaintenance
            });

            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = cardBgColor;

            return card;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            EquipmentItem item = (EquipmentItem)btn.Tag;
            ShowEditDialog(item);
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

                tlp.Controls.Add(new Label { Text = "Maintenance Date:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                DateTimePicker dtpDate = new DateTimePicker
                {
                    Value = DateTime.Now,
                    Font = new Font("Segoe UI", 10F),
                    Width = 200
                };
                tlp.Controls.Add(dtpDate, 1, 0);

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

                        try
                        {
                            GlobalLogger.LogInfo("GameEquipment", $"Maintenance completed for '{item.Name}'. New condition: {item.Condition}");
                        }
                        catch { }

                        MessageBox.Show($"Maintenance completed for {item.Name}.\n" +
                                      $"New condition: {item.Condition}",
                            "Maintenance Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            GlobalLogger.LogError("GameEquipment", ex.Message, $"Error completing maintenance for {item.Name}");
                        }
                        catch { }

                        MessageBox.Show($"Database error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

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
                }
            }
            catch (Exception ex)
            {
                try
                {
                    GlobalLogger.LogError("GameEquipment", ex.Message, "Error refreshing data");
                }
                catch { }
            }
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
                    RowCount = 9,
                    Padding = new Padding(20),
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Absolute, 120),
                        new ColumnStyle(SizeType.Percent, 100)
                    }
                };

                tlp.Controls.Add(new Label { Text = "Name:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                TextBox txtName = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill, Text = item.Name };
                tlp.Controls.Add(txtName, 1, 0);

                tlp.Controls.Add(new Label { Text = "Category:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboCategory = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboCategory.Items.AddRange(categoryList.Select(c => c.Name).ToArray());
                if (!string.IsNullOrEmpty(item.Category)) cboCategory.SelectedItem = item.Category;
                else if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
                tlp.Controls.Add(cboCategory, 1, 1);

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

                tlp.Controls.Add(new Label { Text = "Available:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 3);
                NumericUpDown nudAvailableQty = new NumericUpDown
                {
                    Font = new Font("Segoe UI", 10F),
                    Minimum = 0,
                    Maximum = (int)nudTotalQty.Value,
                    Value = item.AvailableQuantity > 0 ? item.AvailableQuantity : 1,
                    Width = 100
                };
                nudTotalQty.ValueChanged += (s, e) => nudAvailableQty.Maximum = (int)nudTotalQty.Value;
                tlp.Controls.Add(nudAvailableQty, 1, 3);

                tlp.Controls.Add(new Label { Text = "Condition:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 4);
                ComboBox cboCondition = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboCondition.Items.AddRange(conditionList.Select(c => c.Name).ToArray());
                if (!string.IsNullOrEmpty(item.Condition)) cboCondition.SelectedItem = item.Condition;
                else if (cboCondition.Items.Count > 0) cboCondition.SelectedIndex = 0;
                tlp.Controls.Add(cboCondition, 1, 4);

                tlp.Controls.Add(new Label { Text = "Location:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 5);
                TextBox txtLocation = new TextBox { Font = new Font("Segoe UI", 10F), Dock = DockStyle.Fill, Text = item.Location };
                tlp.Controls.Add(txtLocation, 1, 5);

                tlp.Controls.Add(new Label { Text = "Last Maintenance:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 6);
                DateTimePicker dtpMaintenance = new DateTimePicker
                {
                    Font = new Font("Segoe UI", 10F),
                    Value = item.LastMaintenance != DateTime.MinValue ? item.LastMaintenance : DateTime.Now,
                    Width = 200
                };
                tlp.Controls.Add(dtpMaintenance, 1, 6);

                tlp.Controls.Add(new Label { Text = "Needs Maintenance:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 7);
                CheckBox chkMaintenance = new CheckBox
                {
                    Text = "Yes",
                    Font = new Font("Segoe UI", 10F),
                    Checked = item.NeedsMaintenance
                };
                tlp.Controls.Add(chkMaintenance, 1, 7);

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

                FlowLayoutPanel btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 50,
                    Padding = new Padding(0, 0, 20, 10)
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
                        Margin = new Padding(10, 0, 0, 0),
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

                                try
                                {
                                    GlobalLogger.LogInfo("GameEquipment", $"New equipment '{newItem.Name}' added - Category: {newItem.Category}, Quantity: {newItem.TotalQuantity}");
                                }
                                catch { }

                                MessageBox.Show("Equipment added successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                string oldName = item.Name;
                                string oldCategory = item.Category;
                                int oldTotal = item.TotalQuantity;
                                int oldAvailable = item.AvailableQuantity;
                                string oldCondition = item.Condition;
                                string oldLocation = item.Location;
                                bool oldMaintenance = item.NeedsMaintenance;

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

                                item.Name = txtName.Text;
                                item.Category = cboCategory.SelectedItem.ToString();
                                item.TotalQuantity = (int)nudTotalQty.Value;
                                item.AvailableQuantity = (int)nudAvailableQty.Value;
                                item.Condition = cboCondition.SelectedItem.ToString();
                                item.Location = txtLocation.Text;
                                item.Notes = txtNotes.Text;
                                item.NeedsMaintenance = chkMaintenance.Checked;
                                item.LastMaintenance = dtpMaintenance.Value;

                                string changes = "";
                                if (oldName != item.Name) changes += $"Name: '{oldName}' → '{item.Name}' ";
                                if (oldCategory != item.Category) changes += $"Category: '{oldCategory}' → '{item.Category}' ";
                                if (oldTotal != item.TotalQuantity) changes += $"Total Qty: {oldTotal} → {item.TotalQuantity} ";
                                if (oldAvailable != item.AvailableQuantity) changes += $"Available: {oldAvailable} → {item.AvailableQuantity} ";
                                if (oldCondition != item.Condition) changes += $"Condition: '{oldCondition}' → '{item.Condition}' ";
                                if (oldLocation != item.Location) changes += $"Location: '{oldLocation}' → '{item.Location}' ";

                                try
                                {
                                    GlobalLogger.LogInfo("GameEquipment", $"Equipment '{item.Name}' updated: {changes}");
                                }
                                catch { }

                                MessageBox.Show("Equipment updated successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        dialog.Close();
                        DisplayEquipment();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            GlobalLogger.LogError("GameEquipment", ex.Message, isNewItem ? "Error adding new equipment" : $"Error updating {item.Name}");
                        }
                        catch { }

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

                                try
                                {
                                    GlobalLogger.LogInfo("GameEquipment", $"Equipment '{deletedName}' was deleted");
                                }
                                catch { }

                                MessageBox.Show("Equipment deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dialog.Close();
                                DisplayEquipment();
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    GlobalLogger.LogError("GameEquipment", ex.Message, $"Error deleting {item.Name}");
                                }
                                catch { }

                                MessageBox.Show($"Database error: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };
                }

                dialog.ShowDialog(this);
            }
        }

        private void FilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEquipment();
        }

        private void btnAddEquipment_Click(object sender, EventArgs e)
        {
            EquipmentItem newItem = new EquipmentItem
            {
                Id = 0,
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
            if (managementOverlay != null)
            {
                managementOverlay.Visible = true;
                managementOverlay.BringToFront();
            }

            if (btnManageCategories != null)
                btnManageCategories.Text = "Hide Management";

            LoadCategories();
            LoadConditions();

            try
            {
                GlobalLogger.LogInfo("GameEquipment", "Opened categories and conditions management");
            }
            catch { }
        }

        private void btnCloseManagement_Click(object sender, EventArgs e)
        {
            if (managementOverlay != null)
                managementOverlay.Visible = false;

            if (btnManageCategories != null)
                btnManageCategories.Text = "Manage Categories";
        }

        private void ManagementTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (managementTabs != null)
            {
                if (managementTabs.SelectedTab == tabCategories)
                {
                    LoadCategories();
                }
                else if (managementTabs.SelectedTab == tabConditions)
                {
                    LoadConditions();
                }
            }
        }

        private void LoadCategories()
        {
            if (categoriesFlowPanel == null) return;

            if (categoriesFlowPanel.InvokeRequired)
            {
                categoriesFlowPanel.Invoke(new MethodInvoker(() => LoadCategories()));
                return;
            }

            categoriesFlowPanel.Controls.Clear();

            if (categoryList == null || categoryList.Count == 0)
            {
                Label lblNoData = new Label
                {
                    Text = "No categories found. Click '+ Add New Category' to add one.",
                    Font = new Font("Segoe UI", 11F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                categoriesFlowPanel.Controls.Add(lblNoData);
                return;
            }

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
                Width = 320,
                Height = 130,
                BackColor = cardBgColor,
                Margin = new Padding(10),
                Tag = category
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
            };

            Panel iconPanel = new Panel
            {
                Width = 60,
                Height = 60,
                Location = new Point(15, 35),
                BackColor = Color.FromArgb(239, 246, 255)
            };

            Label lblIcon = new Label
            {
                Text = category.Icon ?? "📦",
                Font = new Font("Segoe UI", 28F),
                Location = new Point(10, 10),
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = primaryColor
            };
            iconPanel.Controls.Add(lblIcon);

            Label lblName = new Label
            {
                Text = category.Name,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(90, 25),
                Size = new Size(140, 20),
                ForeColor = Color.FromArgb(33, 33, 33)
            };

            Label lblDesc = new Label
            {
                Text = category.Description,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(90, 45),
                Size = new Size(140, 20),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            Label lblCount = new Label
            {
                Text = $"Items: {category.ItemCount}",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(90, 70),
                Size = new Size(100, 15),
                ForeColor = primaryColor
            };

            Button btnEdit = new Button
            {
                Text = "Edit",
                FlatStyle = FlatStyle.Flat,
                BackColor = primaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(70, 30),
                Location = new Point(235, 25),
                Tag = category,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnEdit.Click += BtnEditCategory_Click;

            Button btnDelete = new Button
            {
                Text = "Delete",
                FlatStyle = FlatStyle.Flat,
                BackColor = dangerColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(70, 30),
                Location = new Point(235, 60),
                Tag = category,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnDelete.Click += BtnDeleteCategory_Click;

            card.Controls.AddRange(new Control[] { iconPanel, lblName, lblDesc, lblCount, btnEdit, btnDelete });

            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = cardBgColor;

            return card;
        }

        private void LoadConditions()
        {
            if (conditionsFlowPanel == null) return;

            if (conditionsFlowPanel.InvokeRequired)
            {
                conditionsFlowPanel.Invoke(new MethodInvoker(() => LoadConditions()));
                return;
            }

            conditionsFlowPanel.Controls.Clear();

            if (conditionList == null || conditionList.Count == 0)
            {
                Label lblNoData = new Label
                {
                    Text = "No conditions available.",
                    Font = new Font("Segoe UI", 11F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                conditionsFlowPanel.Controls.Add(lblNoData);
                return;
            }

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
                Width = 280,
                Height = 100,
                BackColor = cardBgColor,
                Margin = new Padding(10),
                Tag = condition
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
            };

            Panel colorIndicator = new Panel
            {
                Width = 50,
                Height = 50,
                Location = new Point(15, 25),
                BackColor = ColorTranslator.FromHtml(condition.Color),
                BorderStyle = BorderStyle.None
            };

            Label lblName = new Label
            {
                Text = condition.Name,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(80, 25),
                Size = new Size(180, 20),
                ForeColor = Color.FromArgb(33, 33, 33)
            };

            Label lblDesc = new Label
            {
                Text = condition.Description,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(80, 45),
                Size = new Size(180, 30),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            card.Controls.AddRange(new Control[] { colorIndicator, lblName, lblDesc });

            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = cardBgColor;

            return card;
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Category category = (Category)btn.Tag;

            if (equipmentList.Any(eq => eq.Category == category.Name))
            {
                MessageBox.Show($"Cannot delete '{category.Name}' because it's used by existing equipment.",
                    "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Delete category '{category.Name}'?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM equipment_categories WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@id", category.Id);
                        cmd.ExecuteNonQuery();
                    }

                    categoryList.Remove(category);
                    LoadCategories();

                    try
                    {
                        GlobalLogger.LogInfo("GameEquipment", $"Category '{category.Name}' was deleted");
                    }
                    catch { }

                    MessageBox.Show("Category deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    try
                    {
                        GlobalLogger.LogError("GameEquipment", ex.Message, $"Error deleting category {category.Name}");
                    }
                    catch { }

                    MessageBox.Show($"Error deleting: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Category category = (Category)btn.Tag;
            ShowEditCategoryDialog(category);
        }

        private void ShowEditCategoryDialog(Category category)
        {
            using (var dialog = new Form())
            {
                dialog.Text = "Edit Category";
                dialog.Size = new Size(450, 350);
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
                        new ColumnStyle(SizeType.Absolute, 100),
                        new ColumnStyle(SizeType.Percent, 100)
                    }
                };

                tlp.Controls.Add(new Label { Text = "Name:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                TextBox txtName = new TextBox
                {
                    Text = category.Name,
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill
                };
                tlp.Controls.Add(txtName, 1, 0);

                tlp.Controls.Add(new Label { Text = "Icon:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboIcon = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboIcon.Items.AddRange(new object[] { "🏸", "⚽", "🏐", "🛡️", "👟", "🎾", "🏓", "🥊", "🎽", "📦" });
                cboIcon.SelectedItem = category.Icon ?? "📦";
                tlp.Controls.Add(cboIcon, 1, 1);

                tlp.Controls.Add(new Label { Text = "Description:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 2);
                TextBox txtDesc = new TextBox
                {
                    Text = category.Description,
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    Height = 80
                };
                tlp.Controls.Add(txtDesc, 1, 2);

                FlowLayoutPanel btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 50,
                    Padding = new Padding(0, 0, 20, 10)
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

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnSave);
                tlp.Controls.Add(btnPanel, 1, 3);

                dialog.Controls.Add(tlp);

                btnSave.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Please enter a category name.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboIcon.SelectedItem == null)
                    {
                        MessageBox.Show("Please select an icon.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (categoryList.Any(c => c.Name.Equals(txtName.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.Id != category.Id))
                    {
                        MessageBox.Show("A category with this name already exists!", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            string updateQuery = "UPDATE equipment_categories SET name = @name, description = @desc, icon = @icon WHERE id = @id";
                            MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@desc", txtDesc.Text);
                            cmd.Parameters.AddWithValue("@icon", cboIcon.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@id", category.Id);
                            cmd.ExecuteNonQuery();
                        }

                        string oldName = category.Name;
                        category.Name = txtName.Text.Trim();
                        category.Description = txtDesc.Text;
                        category.Icon = cboIcon.SelectedItem.ToString();

                        LoadCategories();

                        try
                        {
                            GlobalLogger.LogInfo("GameEquipment", $"Category '{oldName}' was updated to '{category.Name}'");
                        }
                        catch { }

                        MessageBox.Show("Category updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            GlobalLogger.LogError("GameEquipment", ex.Message, $"Error updating category {category.Name}");
                        }
                        catch { }

                        MessageBox.Show($"Error updating category: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                dialog.ShowDialog(this);
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            ShowAddCategoryDialog();
        }

        private void ShowAddCategoryDialog()
        {
            using (var dialog = new Form())
            {
                dialog.Text = "Add New Category";
                dialog.Size = new Size(450, 350);
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
                        new ColumnStyle(SizeType.Absolute, 100),
                        new ColumnStyle(SizeType.Percent, 100)
                    }
                };

                tlp.Controls.Add(new Label { Text = "Name:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 0);
                TextBox txtName = new TextBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill
                };
                tlp.Controls.Add(txtName, 1, 0);

                tlp.Controls.Add(new Label { Text = "Icon:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboIcon = new ComboBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboIcon.Items.AddRange(new object[] { "🏸", "⚽", "🏐", "🛡️", "👟", "🎾", "🏓", "🥊", "🎽", "📦" });
                cboIcon.SelectedIndex = 9;
                tlp.Controls.Add(cboIcon, 1, 1);

                tlp.Controls.Add(new Label { Text = "Description:", Font = new Font("Segoe UI", 10F), Anchor = AnchorStyles.Left }, 0, 2);
                TextBox txtDesc = new TextBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    Height = 80
                };
                tlp.Controls.Add(txtDesc, 1, 2);

                FlowLayoutPanel btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 50,
                    Padding = new Padding(0, 0, 20, 10)
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

                Button btnAdd = new Button
                {
                    Text = "Add Category",
                    Size = new Size(120, 35),
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnAdd.FlatAppearance.BorderSize = 0;

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnAdd);
                tlp.Controls.Add(btnPanel, 1, 3);

                dialog.Controls.Add(tlp);

                btnAdd.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Please enter a category name.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboIcon.SelectedItem == null)
                    {
                        MessageBox.Show("Please select an icon.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (categoryList.Any(c => c.Name.Equals(txtName.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("A category with this name already exists!", "Duplicate",
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
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@desc", txtDesc.Text);
                            cmd.Parameters.AddWithValue("@icon", cboIcon.SelectedItem.ToString());

                            int newId = Convert.ToInt32(cmd.ExecuteScalar());

                            categoryList.Add(new Category
                            {
                                Id = newId,
                                Name = txtName.Text.Trim(),
                                Description = txtDesc.Text,
                                Icon = cboIcon.SelectedItem.ToString(),
                                ItemCount = 0
                            });

                            LoadCategories();
                        }

                        try
                        {
                            GlobalLogger.LogInfo("GameEquipment", $"New category '{txtName.Text.Trim()}' was added");
                        }
                        catch { }

                        MessageBox.Show("Category added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            GlobalLogger.LogError("GameEquipment", ex.Message, $"Error adding category {txtName.Text}");
                        }
                        catch { }

                        MessageBox.Show($"Error adding category: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                dialog.ShowDialog(this);
            }
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is coming soon!\n\nActivity logs will be available in the next update.",
                "Coming Soon",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            try
            {
                GlobalLogger.LogInfo("GameEquipment", "User attempted to view activity logs (coming soon)");
            }
            catch { }
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