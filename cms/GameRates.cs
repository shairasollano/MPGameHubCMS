using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using Font = System.Drawing.Font;

namespace cms
{
    public partial class GameRates : UserControl
    {
        // Modern color scheme
        private readonly Color primaryColor = Color.FromArgb(79, 70, 229); // Indigo
        private readonly Color successColor = Color.FromArgb(16, 185, 129); // Emerald
        private readonly Color dangerColor = Color.FromArgb(239, 68, 68); // Red
        private readonly Color warningColor = Color.FromArgb(245, 158, 11); // Amber
        private readonly Color infoColor = Color.FromArgb(59, 130, 246); // Blue
        private readonly Color cardBgColor = Color.White;
        private readonly Color hoverColor = Color.FromArgb(249, 250, 251);

        private List<GameRate> gameRates;
        private List<CourtType> courtTypes;
        private List<GameType> gameTypesList;
        private Dictionary<int, byte[]> _imageCache;

        // Current user (you should set this from your login system)
        private string currentUser = "admin";

        // Database connection string for XAMPP MySQL
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;Allow User Variables=True;";
        private MySqlConnection connection;

        // Model classes matching your database structure
        private class GameRate
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string CourtType { get; set; }
            public string GameType { get; set; }
            public decimal Rate { get; set; }
            public string Description { get; set; }
            public string Status { get; set; } // 'Enabled' or 'Disabled'
            public byte[] ImageData { get; set; }
            public Image Image { get; set; }

            public bool IsEnabled => Status == "Enabled";
        }

        private class CourtType
        {
            public int Id { get; set; }
            public string CourtName { get; set; }
            public string Description { get; set; }

            // For compatibility with UI
            public string Name => CourtName;
        }

        private class GameType
        {
            public int Id { get; set; }
            public string GameName { get; set; }
            public string Description { get; set; }

            // For compatibility with UI
            public string Name => GameName;
        }

        // Create a placeholder image
        private Image CreatePlaceholderImage()
        {
            Bitmap placeholder = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.FromArgb(249, 250, 251));
                using (Pen pen = new Pen(Color.FromArgb(209, 213, 219), 2))
                {
                    g.DrawRectangle(pen, 1, 1, 98, 98);
                }
                using (Font font = new Font("Segoe UI", 10, FontStyle.Regular))
                using (Brush brush = new SolidBrush(Color.FromArgb(156, 163, 175)))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString("No Image", font, brush, new Rectangle(0, 0, 100, 100), sf);
                }
            }
            return placeholder;
        }

        public GameRates()
        {
            InitializeComponent();
            gameRates = new List<GameRate>();
            courtTypes = new List<CourtType>();
            gameTypesList = new List<GameType>();
            _imageCache = new Dictionary<int, byte[]>();

            // Style buttons
            StyleButton(btnAddNew, successColor);
            StyleButton(btnManage, primaryColor);
            StyleButton(btnAddCourt, successColor);
            StyleButton(btnAddGameType, successColor);
            StyleButton(btnCloseManagement, dangerColor);

            // Initialize database and load data
            InitializeDatabase();
            InitializeControls();

            // Log that GameRates module was opened
            Activitylogs.Instance.AddLogEntry(currentUser, "Module Opened", "Game Rates management module was opened", "Info", "GameRates");
        }

        private void StyleButton(Button btn, Color backColor)
        {
            btn.BackColor = backColor;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Height = 32;

            // Hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(backColor, 0.2f);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;
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
                Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, "Database initialization error");
                MessageBox.Show($"Database initialization error: {ex.Message}\n\nUsing default sample data.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InitializeDefaultOptions();
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
                    string createDbQuery = "CREATE DATABASE matchpoint_db";
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

                // Create court_types table with your structure
                string createCourtTypesTable = @"
                    CREATE TABLE IF NOT EXISTS court_types (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        court_name VARCHAR(100) NOT NULL UNIQUE,
                        description TEXT,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";
                MySqlCommand cmd1 = new MySqlCommand(createCourtTypesTable, connection);
                cmd1.ExecuteNonQuery();

                // Create game_types table with your structure
                string createGameTypesTable = @"
                    CREATE TABLE IF NOT EXISTS game_types (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        game_name VARCHAR(100) NOT NULL UNIQUE,
                        description TEXT,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";
                MySqlCommand cmd2 = new MySqlCommand(createGameTypesTable, connection);
                cmd2.ExecuteNonQuery();

                // Check if game_rates table exists
                string checkTableQuery = @"
                    SELECT COUNT(*) 
                    FROM information_schema.tables 
                    WHERE table_schema = 'matchpoint_db' 
                    AND table_name = 'game_rates'";

                MySqlCommand checkTableCmd = new MySqlCommand(checkTableQuery, connection);
                int tableExists = Convert.ToInt32(checkTableCmd.ExecuteScalar());

                if (tableExists == 0)
                {
                    // Create game_rates table with your structure
                    string createGameRatesTable = @"
                        CREATE TABLE game_rates (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            name VARCHAR(200) NOT NULL,
                            court_type VARCHAR(100) NOT NULL,
                            game_type VARCHAR(100) NOT NULL,
                            rate DECIMAL(10,2) NOT NULL,
                            description TEXT,
                            image LONGBLOB,
                            status ENUM('Enabled', 'Disabled') DEFAULT 'Enabled',
                            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                            FOREIGN KEY (court_type) REFERENCES court_types(court_name) ON DELETE RESTRICT,
                            FOREIGN KEY (game_type) REFERENCES game_types(game_name) ON DELETE RESTRICT
                        )";
                    MySqlCommand cmd3 = new MySqlCommand(createGameRatesTable, connection);
                    cmd3.ExecuteNonQuery();
                }
                else
                {
                    // Check if image column exists in game_rates table
                    string checkColumnQuery = @"
                        SELECT COUNT(*) 
                        FROM information_schema.columns 
                        WHERE table_schema = 'matchpoint_db' 
                        AND table_name = 'game_rates' 
                        AND column_name = 'image'";

                    MySqlCommand checkColumnCmd = new MySqlCommand(checkColumnQuery, connection);
                    int columnExists = Convert.ToInt32(checkColumnCmd.ExecuteScalar());

                    if (columnExists == 0)
                    {
                        // Add image column to existing table
                        string addColumnQuery = "ALTER TABLE game_rates ADD COLUMN image LONGBLOB AFTER description";
                        MySqlCommand addColumnCmd = new MySqlCommand(addColumnQuery, connection);
                        addColumnCmd.ExecuteNonQuery();
                    }
                }

                // Check if tables are empty and insert default data if needed
                CheckAndInsertDefaultData();
            }
        }

        private void CheckAndInsertDefaultData()
        {
            // Check court_types
            string checkCourtTypes = "SELECT COUNT(*) FROM court_types";
            MySqlCommand checkCourtCmd = new MySqlCommand(checkCourtTypes, connection);
            int courtCount = Convert.ToInt32(checkCourtCmd.ExecuteScalar());

            if (courtCount == 0)
            {
                string[] defaultCourts = { "Indoor", "Outdoor", "Hard Court", "Clay Court", "Grass Court", "Synthetic" };
                foreach (string court in defaultCourts)
                {
                    string insertCourt = "INSERT INTO court_types (court_name, description) VALUES (@name, @desc)";
                    MySqlCommand insertCmd = new MySqlCommand(insertCourt, connection);
                    insertCmd.Parameters.AddWithValue("@name", court);
                    insertCmd.Parameters.AddWithValue("@desc", $"Court type for {court} games");
                    insertCmd.ExecuteNonQuery();
                }

                Activitylogs.Instance.AddLogEntry(currentUser, "Default Data", "Default court types were created", "Info", "GameRates");
            }

            // Check game_types
            string checkGameTypes = "SELECT COUNT(*) FROM game_types";
            MySqlCommand checkGameCmd = new MySqlCommand(checkGameTypes, connection);
            int gameCount = Convert.ToInt32(checkGameCmd.ExecuteScalar());

            if (gameCount == 0)
            {
                string[] defaultGames = { "Badminton", "Tennis", "Basketball", "Volleyball", "Squash", "Table Tennis", "Futsal" };
                foreach (string game in defaultGames)
                {
                    string insertGame = "INSERT INTO game_types (game_name, description) VALUES (@name, @desc)";
                    MySqlCommand insertCmd = new MySqlCommand(insertGame, connection);
                    insertCmd.Parameters.AddWithValue("@name", game);
                    insertCmd.Parameters.AddWithValue("@desc", $"{game} game type");
                    insertCmd.ExecuteNonQuery();
                }

                Activitylogs.Instance.AddLogEntry(currentUser, "Default Data", "Default game types were created", "Info", "GameRates");
            }

            // Check game_rates
            string checkGameRates = "SELECT COUNT(*) FROM game_rates";
            MySqlCommand checkRatesCmd = new MySqlCommand(checkGameRates, connection);
            int ratesCount = Convert.ToInt32(checkRatesCmd.ExecuteScalar());

            if (ratesCount == 0)
            {
                // Insert sample game rates
                string insertRate = @"
                    INSERT INTO game_rates (name, court_type, game_type, rate, description, status) VALUES
                    ('Badminton Court 1', 'Indoor', 'Badminton', 500, 'Professional indoor court, shuttlecock provided', 'Enabled'),
                    ('Tennis Court A', 'Outdoor', 'Tennis', 800, 'Floodlit court, racket rental available', 'Enabled'),
                    ('Basketball Court', 'Indoor', 'Basketball', 700, 'Full court with spectators area, glass backboards', 'Disabled'),
                    ('Volleyball Court', 'Outdoor', 'Volleyball', 600, 'Beach sand court, net included, lights available', 'Enabled')";
                MySqlCommand insertRatesCmd = new MySqlCommand(insertRate, connection);
                insertRatesCmd.ExecuteNonQuery();

                Activitylogs.Instance.AddLogEntry(currentUser, "Default Data", "Default game rates were created", "Info", "GameRates");
            }
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Load court types
                    courtTypes.Clear();
                    string courtQuery = "SELECT id, court_name, description FROM court_types ORDER BY court_name";
                    MySqlCommand courtCmd = new MySqlCommand(courtQuery, connection);
                    using (MySqlDataReader reader = courtCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courtTypes.Add(new CourtType
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                CourtName = reader["court_name"].ToString(),
                                Description = reader["description"]?.ToString() ?? ""
                            });
                        }
                    }

                    // Load game types
                    gameTypesList.Clear();
                    string gameQuery = "SELECT id, game_name, description FROM game_types ORDER BY game_name";
                    MySqlCommand gameCmd = new MySqlCommand(gameQuery, connection);
                    using (MySqlDataReader reader = gameCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gameTypesList.Add(new GameType
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                GameName = reader["game_name"].ToString(),
                                Description = reader["description"]?.ToString() ?? ""
                            });
                        }
                    }

                    // Load game rates
                    LoadGameRatesFromDatabase();
                }

                Activitylogs.Instance.AddLogEntry(currentUser, "Data Loaded", $"Loaded {gameRates.Count} game rates from database", "Info", "GameRates");
            }
            catch (Exception ex)
            {
                Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, "Error loading data from database");
                MessageBox.Show($"Error loading data from database: {ex.Message}\nUsing sample data instead.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InitializeDefaultOptions();
            }
        }

        private void LoadGameRatesFromDatabase()
        {
            try
            {
                gameRates.Clear();
                _imageCache.Clear();

                if (ratesFlowPanel.InvokeRequired)
                {
                    ratesFlowPanel.Invoke(new MethodInvoker(() => ratesFlowPanel.Controls.Clear()));
                }
                else
                {
                    ratesFlowPanel.Controls.Clear();
                }

                string query = "SELECT id, status, name, court_type, game_type, rate, description, image FROM game_rates ORDER BY name";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        byte[] imageData = reader["image"] as byte[];
                        Image image = null;

                        if (imageData != null && imageData.Length > 0)
                        {
                            try
                            {
                                image = GetImageFromBytes(imageData);
                                if (image != null)
                                {
                                    _imageCache[id] = imageData;
                                }
                            }
                            catch
                            {
                                image = null;
                            }
                        }

                        gameRates.Add(new GameRate
                        {
                            Id = id,
                            Name = reader["name"].ToString(),
                            CourtType = reader["court_type"].ToString(),
                            GameType = reader["game_type"].ToString(),
                            Rate = Convert.ToDecimal(reader["rate"]),
                            Description = reader["description"]?.ToString() ?? "",
                            Status = reader["status"].ToString(),
                            Image = image,
                            ImageData = imageData
                        });
                    }
                }

                DisplayGameRates();
                UpdateStatistics();
                LoadCourtCards();
                LoadGameTypeCards();
            }
            catch (Exception ex)
            {
                Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, "Error loading game rates");
                MessageBox.Show($"Error loading game rates: {ex.Message}\nUsing sample data.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AddSampleData();
            }
        }

        private Image GetImageFromBytes(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        private byte[] GetBytesFromImage(Image image)
        {
            if (image == null)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        private void InitializeControls()
        {
            if (filterCombo.InvokeRequired)
            {
                filterCombo.Invoke(new MethodInvoker(() => {
                    filterCombo.SelectedIndex = 0;
                    managementOverlay.Visible = false;
                }));
            }
            else
            {
                filterCombo.SelectedIndex = 0;
                managementOverlay.Visible = false;
            }
            UpdateStatistics();
        }

        private void InitializeDefaultOptions()
        {
            // Initialize default court types
            courtTypes.Clear();
            string[] defaultCourts = { "Indoor", "Outdoor", "Hard Court", "Clay Court", "Grass Court", "Synthetic" };
            for (int i = 0; i < defaultCourts.Length; i++)
            {
                courtTypes.Add(new CourtType
                {
                    Id = i + 1,
                    CourtName = defaultCourts[i],
                    Description = $"Court type for {defaultCourts[i]} games"
                });
            }

            // Initialize default game types
            gameTypesList.Clear();
            string[] defaultGames = { "Badminton", "Tennis", "Basketball", "Volleyball", "Squash", "Table Tennis", "Futsal" };
            for (int i = 0; i < defaultGames.Length; i++)
            {
                gameTypesList.Add(new GameType
                {
                    Id = i + 1,
                    GameName = defaultGames[i],
                    Description = $"{defaultGames[i]} game type"
                });
            }

            // Add sample data
            AddSampleData();

            // Load management grids
            LoadCourtCards();
            LoadGameTypeCards();
        }

        private void AddSampleData()
        {
            gameRates.Clear();

            gameRates.Add(new GameRate
            {
                Id = 1,
                Name = "Badminton Court 1",
                CourtType = "Indoor",
                GameType = "Badminton",
                Rate = 500,
                Description = "Professional indoor court, shuttlecock provided",
                Status = "Enabled"
            });

            gameRates.Add(new GameRate
            {
                Id = 2,
                Name = "Tennis Court A",
                CourtType = "Outdoor",
                GameType = "Tennis",
                Rate = 800,
                Description = "Floodlit court, racket rental available",
                Status = "Enabled"
            });

            gameRates.Add(new GameRate
            {
                Id = 3,
                Name = "Basketball Court",
                CourtType = "Indoor",
                GameType = "Basketball",
                Rate = 700,
                Description = "Full court with spectators area, glass backboards",
                Status = "Disabled"
            });

            gameRates.Add(new GameRate
            {
                Id = 4,
                Name = "Volleyball Court",
                CourtType = "Outdoor",
                GameType = "Volleyball",
                Rate = 600,
                Description = "Beach sand court, net included, lights available",
                Status = "Enabled"
            });

            DisplayGameRates();
            UpdateStatistics();
        }

        private void LoadCourtCards()
        {
            if (courtsFlowPanel.InvokeRequired)
            {
                courtsFlowPanel.Invoke(new MethodInvoker(() => LoadCourtCards()));
                return;
            }

            courtsFlowPanel.Controls.Clear();
            foreach (var court in courtTypes)
            {
                var card = CreateCourtCard(court);
                courtsFlowPanel.Controls.Add(card);
            }
        }

        private Panel CreateCourtCard(CourtType court)
        {
            Panel card = new Panel
            {
                Width = 280,
                Height = 100,
                BackColor = cardBgColor,
                Margin = new Padding(10),
                Tag = court
            };

            // Modern card styling
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }

                using (Pen shadowPen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                {
                    e.Graphics.DrawLine(shadowPen, 2, card.Height - 1, card.Width - 3, card.Height - 1);
                }
            };

            Label lblName = new Label
            {
                Text = court.CourtName,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 15),
                Size = new Size(250, 25),
                ForeColor = Color.FromArgb(17, 24, 39)
            };

            Label lblDesc = new Label
            {
                Text = court.Description,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(15, 40),
                Size = new Size(250, 20),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            Button btnDelete = new Button
            {
                Text = "Delete",
                FlatStyle = FlatStyle.Flat,
                BackColor = dangerColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(70, 28),
                Location = new Point(190, 62),
                Tag = court,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };

            // Hover effect
            btnDelete.MouseEnter += (s, e) => btnDelete.BackColor = ControlPaint.Light(dangerColor, 0.2f);
            btnDelete.MouseLeave += (s, e) => btnDelete.BackColor = dangerColor;
            btnDelete.Click += BtnDeleteCourt_Click;

            card.Controls.AddRange(new Control[] { lblName, lblDesc, btnDelete });
            return card;
        }

        private void LoadGameTypeCards()
        {
            if (gameTypesFlowPanel.InvokeRequired)
            {
                gameTypesFlowPanel.Invoke(new MethodInvoker(() => LoadGameTypeCards()));
                return;
            }

            gameTypesFlowPanel.Controls.Clear();
            foreach (var gameType in gameTypesList)
            {
                var card = CreateGameTypeCard(gameType);
                gameTypesFlowPanel.Controls.Add(card);
            }
        }

        private Panel CreateGameTypeCard(GameType gameType)
        {
            Panel card = new Panel
            {
                Width = 280,
                Height = 100,
                BackColor = cardBgColor,
                Margin = new Padding(10),
                Tag = gameType
            };

            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
                using (Pen shadowPen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                {
                    e.Graphics.DrawLine(shadowPen, 2, card.Height - 1, card.Width - 3, card.Height - 1);
                }
            };

            Label lblName = new Label
            {
                Text = gameType.GameName,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 15),
                Size = new Size(250, 25),
                ForeColor = Color.FromArgb(17, 24, 39)
            };

            Label lblDesc = new Label
            {
                Text = gameType.Description,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(15, 40),
                Size = new Size(250, 20),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            Button btnDelete = new Button
            {
                Text = "Delete",
                FlatStyle = FlatStyle.Flat,
                BackColor = dangerColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(70, 28),
                Location = new Point(190, 62),
                Tag = gameType,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };

            btnDelete.MouseEnter += (s, e) => btnDelete.BackColor = ControlPaint.Light(dangerColor, 0.2f);
            btnDelete.MouseLeave += (s, e) => btnDelete.BackColor = dangerColor;
            btnDelete.Click += BtnDeleteGameType_Click;

            card.Controls.AddRange(new Control[] { lblName, lblDesc, btnDelete });
            return card;
        }

        private void DisplayGameRates()
        {
            if (ratesFlowPanel.InvokeRequired)
            {
                ratesFlowPanel.Invoke(new MethodInvoker(() => DisplayGameRates()));
                return;
            }

            ratesFlowPanel.Controls.Clear();

            string filter = filterCombo.SelectedItem?.ToString() ?? "All Rates";
            var filteredRates = gameRates.Where(r =>
                filter == "All Rates" ||
                (filter == "Active Only" && r.Status == "Enabled") ||
                (filter == "Inactive Only" && r.Status == "Disabled")
            ).ToList();

            foreach (var rate in filteredRates)
            {
                var card = CreateGameRateCard(rate);
                ratesFlowPanel.Controls.Add(card);
            }
        }

        private Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        private Panel CreateGameRateCard(GameRate rate)
        {
            Panel card = new Panel
            {
                Width = 350,
                Height = 280,
                BackColor = cardBgColor,
                Margin = new Padding(12),
                Tag = rate
            };

            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }

                using (Pen shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), 2))
                {
                    e.Graphics.DrawLine(shadowPen, 2, card.Height - 1, card.Width - 3, card.Height - 1);
                }
            };

            Panel statusBar = new Panel
            {
                Height = 6,
                Dock = DockStyle.Top,
                BackColor = rate.Status == "Enabled" ? successColor : dangerColor
            };

            Panel imagePanel = new Panel
            {
                Size = new Size(100, 100),
                Location = new Point(16, 22),
                BackColor = Color.FromArgb(249, 250, 251),
                BorderStyle = BorderStyle.None
            };

            imagePanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(209, 213, 219), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, imagePanel.Width - 1, imagePanel.Height - 1);
                }
            };

            PictureBox pb = new PictureBox
            {
                Size = new Size(94, 94),
                Location = new Point(3, 3),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = rate.Image ?? CreatePlaceholderImage(),
                BackColor = Color.Transparent
            };
            imagePanel.Controls.Add(pb);

            Label lblName = new Label
            {
                Text = rate.Name,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(130, 22),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(17, 24, 39)
            };

            Label lblType = new Label
            {
                Text = $"🏟️ {rate.CourtType}  •  🎮 {rate.GameType}",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(130, 47),
                Size = new Size(200, 20),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            Label lblRate = new Label
            {
                Text = $"₱{rate.Rate:N0}/hr",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                Location = new Point(130, 70),
                Size = new Size(200, 30),
                ForeColor = primaryColor
            };

            Label lblDesc = new Label
            {
                Text = rate.Description,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(16, 135),
                Size = new Size(320, 40),
                ForeColor = Color.FromArgb(75, 85, 99)
            };

            Button btnToggle = new Button
            {
                Text = rate.Status == "Enabled" ? "Disable" : "Enable",
                FlatStyle = FlatStyle.Flat,
                BackColor = rate.Status == "Enabled" ? warningColor : successColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(75, 32),
                Location = new Point(180, 185),
                Tag = rate,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };

            btnToggle.MouseEnter += (s, e) => btnToggle.BackColor = ControlPaint.Light(rate.Status == "Enabled" ? warningColor : successColor, 0.2f);
            btnToggle.MouseLeave += (s, e) => btnToggle.BackColor = rate.Status == "Enabled" ? warningColor : successColor;
            btnToggle.Click += BtnToggle_Click;

            Button btnEdit = new Button
            {
                Text = "Edit",
                FlatStyle = FlatStyle.Flat,
                BackColor = primaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(75, 32),
                Location = new Point(260, 185),
                Tag = rate,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };

            btnEdit.MouseEnter += (s, e) => btnEdit.BackColor = ControlPaint.Light(primaryColor, 0.2f);
            btnEdit.MouseLeave += (s, e) => btnEdit.BackColor = primaryColor;
            btnEdit.Click += BtnEdit_Click;

            card.Controls.AddRange(new Control[] {
                statusBar, imagePanel, lblName, lblType,
                lblRate, lblDesc, btnToggle, btnEdit
            });

            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = cardBgColor;

            return card;
        }

        private void UpdateStatistics()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => UpdateStatistics()));
                    return;
                }

                int totalRates = gameRates.Count;
                int activeCount = gameRates.Count(r => r.Status == "Enabled");
                int inactiveCount = gameRates.Count(r => r.Status == "Disabled");

                decimal avgRate = 0;
                if (gameRates.Count > 0)
                {
                    avgRate = gameRates.Average(r => r.Rate);
                }

                lblStatsValue1.Text = totalRates.ToString();
                lblStatsValue2.Text = activeCount.ToString();
                lblStatsValue3.Text = inactiveCount.ToString();
                lblStatsValue4.Text = $"₱{avgRate:F0}";

                lblStatsSub1.Text = totalRates == 1 ? "total rate" : "total rates";
                lblStatsSub2.Text = activeCount == 1 ? "active" : "active";
                lblStatsSub3.Text = inactiveCount == 1 ? "inactive" : "inactive";
            }
            catch (Exception ex)
            {
                Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, "Error updating statistics");
            }
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GameRate rate = (GameRate)btn.Tag;

            string oldStatus = rate.Status;
            string newStatus = rate.Status == "Enabled" ? "Disabled" : "Enabled";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE game_rates SET status = @status WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", rate.Id);
                    cmd.ExecuteNonQuery();
                }

                rate.Status = newStatus;
                DisplayGameRates();
                UpdateStatistics();

                Activitylogs.Instance.LogGameRateActivity(currentUser, "Status Changed", rate.Name, $"Changed from {oldStatus} to {newStatus}");

                MessageBox.Show($"Game rate {newStatus.ToLower()}!", "Status Changed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, $"Error changing status for {rate.Name}");
                MessageBox.Show($"Error updating status: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GameRate rate = (GameRate)btn.Tag;
            ShowEditDialog(rate);
        }

        private void ShowEditDialog(GameRate rate)
        {
            using (var editDialog = new Form())
            {
                editDialog.Text = "Edit Game Rate";
                editDialog.Size = new Size(700, 700);
                editDialog.StartPosition = FormStartPosition.CenterParent;
                editDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                editDialog.MaximizeBox = false;
                editDialog.MinimizeBox = false;
                editDialog.BackColor = Color.White;

                Font regularFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                Font semiboldFont = new Font("Segoe UI", 10F, FontStyle.Bold);

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 8,
                    Padding = new Padding(20),
                    AutoSize = true
                };
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                // Name
                tlp.Controls.Add(new Label { Text = "Name:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 0);
                TextBox txtNameEdit = new TextBox { Font = regularFont, Dock = DockStyle.Fill, Text = rate.Name };
                tlp.Controls.Add(txtNameEdit, 1, 0);

                // Court Type
                tlp.Controls.Add(new Label { Text = "Court Type:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboCourtTypeEdit = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboCourtTypeEdit.Items.AddRange(courtTypes.Select(c => c.CourtName).ToArray());
                if (!string.IsNullOrEmpty(rate.CourtType))
                    cboCourtTypeEdit.SelectedItem = rate.CourtType;
                tlp.Controls.Add(cboCourtTypeEdit, 1, 1);

                // Game Type
                tlp.Controls.Add(new Label { Text = "Game Type:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 2);
                ComboBox cboGameTypeEdit = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboGameTypeEdit.Items.AddRange(gameTypesList.Select(g => g.GameName).ToArray());
                if (!string.IsNullOrEmpty(rate.GameType))
                    cboGameTypeEdit.SelectedItem = rate.GameType;
                tlp.Controls.Add(cboGameTypeEdit, 1, 2);

                // Rate
                tlp.Controls.Add(new Label { Text = "Rate per hour:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 3);
                TextBox txtRateEdit = new TextBox { Font = regularFont, Dock = DockStyle.Fill, Text = rate.Rate.ToString() };
                tlp.Controls.Add(txtRateEdit, 1, 3);

                // Image
                tlp.Controls.Add(new Label { Text = "Image:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 4);

                Panel imagePanel = new Panel { Height = 120, Dock = DockStyle.Fill };
                PictureBox pbPreview = new PictureBox
                {
                    Size = new Size(100, 100),
                    Location = new Point(0, 0),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightGray,
                    Image = rate.Image ?? CreatePlaceholderImage()
                };

                Button btnBrowse = new Button
                {
                    Text = "Browse...",
                    Location = new Point(110, 20),
                    Size = new Size(100, 30),
                    Font = regularFont,
                    BackColor = primaryColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnBrowse.FlatAppearance.BorderSize = 0;

                Button btnRemoveImage = new Button
                {
                    Text = "Remove",
                    Location = new Point(110, 60),
                    Size = new Size(100, 30),
                    Font = regularFont,
                    BackColor = dangerColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Visible = rate.Image != null,
                    Cursor = Cursors.Hand
                };
                btnRemoveImage.FlatAppearance.BorderSize = 0;

                Label lblImageName = new Label
                {
                    Location = new Point(220, 40),
                    Size = new Size(200, 60),
                    Font = regularFont,
                    ForeColor = rate.Image != null ? Color.Black : Color.Gray,
                    Text = rate.Image != null ? "Image loaded" : "No image selected"
                };

                byte[] selectedImageBytes = rate.ImageData;
                bool imageChanged = false;

                btnBrowse.Click += (s, args) =>
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                        ofd.Title = "Select an image";

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                using (Image img = Image.FromFile(ofd.FileName))
                                {
                                    Image resizedImg = ResizeImage(img, 300, 300);
                                    pbPreview.Image = resizedImg;
                                    selectedImageBytes = GetBytesFromImage(resizedImg);
                                    lblImageName.Text = Path.GetFileName(ofd.FileName);
                                    lblImageName.ForeColor = Color.Black;
                                    btnRemoveImage.Visible = true;
                                    imageChanged = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                };

                btnRemoveImage.Click += (s, args) =>
                {
                    pbPreview.Image = CreatePlaceholderImage();
                    selectedImageBytes = null;
                    lblImageName.Text = "No image selected";
                    lblImageName.ForeColor = Color.Gray;
                    btnRemoveImage.Visible = false;
                    imageChanged = true;
                };

                imagePanel.Controls.Add(pbPreview);
                imagePanel.Controls.Add(btnBrowse);
                imagePanel.Controls.Add(btnRemoveImage);
                imagePanel.Controls.Add(lblImageName);
                tlp.Controls.Add(imagePanel, 1, 4);

                // Description
                tlp.Controls.Add(new Label { Text = "Description:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 5);
                TextBox txtDescriptionEdit = new TextBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    Height = 80,
                    Text = rate.Description
                };
                tlp.Controls.Add(txtDescriptionEdit, 1, 5);

                // Status
                tlp.Controls.Add(new Label { Text = "Status:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 6);
                ComboBox cboStatusEdit = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboStatusEdit.Items.AddRange(new object[] { "Enabled", "Disabled" });
                cboStatusEdit.SelectedItem = rate.Status;
                tlp.Controls.Add(cboStatusEdit, 1, 6);

                // Buttons panel
                FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Dock = DockStyle.Fill,
                    Height = 50,
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                    WrapContents = false
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 40),
                    Font = regularFont,
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(10, 5, 0, 5),
                    Cursor = Cursors.Hand
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                Button btnSave = new Button
                {
                    Text = "Save Changes",
                    Size = new Size(120, 40),
                    Font = semiboldFont,
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(10, 5, 0, 5),
                    Cursor = Cursors.Hand
                };
                btnSave.FlatAppearance.BorderSize = 0;

                Button btnDelete = new Button
                {
                    Text = "Delete",
                    Size = new Size(100, 40),
                    Font = regularFont,
                    BackColor = dangerColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(10, 5, 0, 5),
                    Cursor = Cursors.Hand
                };
                btnDelete.FlatAppearance.BorderSize = 0;

                buttonPanel.Controls.Add(btnCancel);
                buttonPanel.Controls.Add(btnDelete);
                buttonPanel.Controls.Add(btnSave);
                tlp.Controls.Add(buttonPanel, 1, 7);

                editDialog.Controls.Add(tlp);

                btnSave.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNameEdit.Text))
                    {
                        MessageBox.Show("Please enter a name.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboCourtTypeEdit.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a court type.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboGameTypeEdit.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a game type.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtRateEdit.Text) || !decimal.TryParse(txtRateEdit.Text, out decimal rateVal) || rateVal <= 0)
                    {
                        MessageBox.Show("Please enter a valid rate.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            string updateQuery = @"
                                UPDATE game_rates 
                                SET name = @newName, court_type = @courtType, game_type = @gameType, 
                                    rate = @rate, description = @description, image = @image, status = @status
                                WHERE id = @id";

                            MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                            cmd.Parameters.AddWithValue("@newName", txtNameEdit.Text.Trim());
                            cmd.Parameters.AddWithValue("@courtType", cboCourtTypeEdit.Text);
                            cmd.Parameters.AddWithValue("@gameType", cboGameTypeEdit.Text);
                            cmd.Parameters.AddWithValue("@rate", rateVal);
                            cmd.Parameters.AddWithValue("@description", txtDescriptionEdit.Text);
                            cmd.Parameters.AddWithValue("@image", selectedImageBytes ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@status", cboStatusEdit.Text);
                            cmd.Parameters.AddWithValue("@id", rate.Id);

                            cmd.ExecuteNonQuery();
                        }

                        string oldName = rate.Name;
                        string oldCourtType = rate.CourtType;
                        string oldGameType = rate.GameType;
                        decimal oldRate = rate.Rate;

                        rate.Name = txtNameEdit.Text.Trim();
                        rate.CourtType = cboCourtTypeEdit.Text;
                        rate.GameType = cboGameTypeEdit.Text;
                        rate.Rate = rateVal;
                        rate.Description = txtDescriptionEdit.Text;
                        rate.Status = cboStatusEdit.Text;
                        rate.ImageData = selectedImageBytes;
                        if (selectedImageBytes != null)
                        {
                            rate.Image = GetImageFromBytes(selectedImageBytes);
                        }
                        else
                        {
                            rate.Image = null;
                        }

                        DisplayGameRates();
                        UpdateStatistics();

                        string changes = "";
                        if (oldName != rate.Name) changes += $"Name: '{oldName}' → '{rate.Name}' ";
                        if (oldCourtType != rate.CourtType) changes += $"Court: '{oldCourtType}' → '{rate.CourtType}' ";
                        if (oldGameType != rate.GameType) changes += $"Game: '{oldGameType}' → '{rate.GameType}' ";
                        if (oldRate != rate.Rate) changes += $"Rate: ₱{oldRate} → ₱{rate.Rate} ";
                        if (imageChanged) changes += "Image updated ";

                        Activitylogs.Instance.LogGameRateActivity(currentUser, "Updated", rate.Name, changes);

                        MessageBox.Show("Game rate updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        editDialog.Close();
                    }
                    catch (Exception ex)
                    {
                        Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, $"Error updating {rate.Name}");
                        MessageBox.Show($"Error updating database: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnDelete.Click += (s, args) =>
                {
                    DialogResult result = MessageBox.Show($"Are you sure you want to delete '{rate.Name}'?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                conn.Open();
                                string deleteQuery = "DELETE FROM game_rates WHERE id = @id";
                                MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                                cmd.Parameters.AddWithValue("@id", rate.Id);
                                cmd.ExecuteNonQuery();
                            }

                            string deletedName = rate.Name;
                            gameRates.Remove(rate);
                            DisplayGameRates();
                            UpdateStatistics();

                            Activitylogs.Instance.LogGameRateActivity(currentUser, "Deleted", deletedName);

                            MessageBox.Show("Game rate deleted successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            editDialog.Close();
                        }
                        catch (Exception ex)
                        {
                            Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, $"Error deleting {rate.Name}");
                            MessageBox.Show($"Error deleting: {ex.Message}",
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                editDialog.ShowDialog(this);
            }
        }

        private void ShowAddRateDialog()
        {
            using (var addRateDialog = new Form())
            {
                addRateDialog.Text = "Add New Game Rate";
                addRateDialog.Size = new Size(700, 700);
                addRateDialog.StartPosition = FormStartPosition.CenterParent;
                addRateDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                addRateDialog.MaximizeBox = false;
                addRateDialog.MinimizeBox = false;
                addRateDialog.BackColor = Color.White;

                Font regularFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                Font semiboldFont = new Font("Segoe UI", 10F, FontStyle.Bold);

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 8,
                    Padding = new Padding(20),
                    AutoSize = true
                };
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                // Name
                tlp.Controls.Add(new Label { Text = "Name:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 0);
                TextBox txtName = new TextBox { Font = regularFont, Dock = DockStyle.Fill };
                tlp.Controls.Add(txtName, 1, 0);

                // Court Type
                tlp.Controls.Add(new Label { Text = "Court Type:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboCourtType = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboCourtType.Items.AddRange(courtTypes.Select(c => c.CourtName).ToArray());
                if (cboCourtType.Items.Count > 0) cboCourtType.SelectedIndex = 0;
                tlp.Controls.Add(cboCourtType, 1, 1);

                // Game Type
                tlp.Controls.Add(new Label { Text = "Game Type:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 2);
                ComboBox cboGameType = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboGameType.Items.AddRange(gameTypesList.Select(g => g.GameName).ToArray());
                if (cboGameType.Items.Count > 0) cboGameType.SelectedIndex = 0;
                tlp.Controls.Add(cboGameType, 1, 2);

                // Rate
                tlp.Controls.Add(new Label { Text = "Rate per hour:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 3);
                TextBox txtRate = new TextBox { Font = regularFont, Dock = DockStyle.Fill };
                tlp.Controls.Add(txtRate, 1, 3);

                // Image
                tlp.Controls.Add(new Label { Text = "Image:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 4);

                Panel imagePanel = new Panel { Height = 120, Dock = DockStyle.Fill };
                PictureBox pbPreview = new PictureBox
                {
                    Size = new Size(100, 100),
                    Location = new Point(0, 0),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightGray,
                    Image = CreatePlaceholderImage()
                };
                Button btnBrowse = new Button
                {
                    Text = "Browse...",
                    Location = new Point(110, 20),
                    Size = new Size(100, 30),
                    Font = regularFont,
                    BackColor = primaryColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnBrowse.FlatAppearance.BorderSize = 0;

                Button btnRemoveImage = new Button
                {
                    Text = "Remove",
                    Location = new Point(110, 60),
                    Size = new Size(100, 30),
                    Font = regularFont,
                    BackColor = dangerColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Visible = false,
                    Cursor = Cursors.Hand
                };
                btnRemoveImage.FlatAppearance.BorderSize = 0;

                Label lblImageName = new Label
                {
                    Location = new Point(220, 40),
                    Size = new Size(200, 60),
                    Font = regularFont,
                    ForeColor = Color.Gray,
                    Text = "No image selected"
                };

                byte[] selectedImageBytes = null;

                btnBrowse.Click += (s, args) =>
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                        ofd.Title = "Select an image";

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                using (Image img = Image.FromFile(ofd.FileName))
                                {
                                    Image resizedImg = ResizeImage(img, 300, 300);
                                    pbPreview.Image = resizedImg;
                                    selectedImageBytes = GetBytesFromImage(resizedImg);
                                    lblImageName.Text = Path.GetFileName(ofd.FileName);
                                    lblImageName.ForeColor = Color.Black;
                                    btnRemoveImage.Visible = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                };

                btnRemoveImage.Click += (s, args) =>
                {
                    pbPreview.Image = CreatePlaceholderImage();
                    selectedImageBytes = null;
                    lblImageName.Text = "No image selected";
                    lblImageName.ForeColor = Color.Gray;
                    btnRemoveImage.Visible = false;
                };

                imagePanel.Controls.Add(pbPreview);
                imagePanel.Controls.Add(btnBrowse);
                imagePanel.Controls.Add(btnRemoveImage);
                imagePanel.Controls.Add(lblImageName);
                tlp.Controls.Add(imagePanel, 1, 4);

                // Description
                tlp.Controls.Add(new Label { Text = "Description:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 5);
                TextBox txtDescription = new TextBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    Height = 80
                };
                tlp.Controls.Add(txtDescription, 1, 5);

                // Status
                tlp.Controls.Add(new Label { Text = "Status:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 6);
                ComboBox cboStatus = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboStatus.Items.AddRange(new object[] { "Enabled", "Disabled" });
                cboStatus.SelectedIndex = 0;
                tlp.Controls.Add(cboStatus, 1, 6);

                // Buttons panel
                FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Dock = DockStyle.Fill,
                    Height = 50,
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                    WrapContents = false
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 40),
                    Font = regularFont,
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(10, 5, 0, 5),
                    Cursor = Cursors.Hand
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                Button btnAdd = new Button
                {
                    Text = "Add Rate",
                    Size = new Size(120, 40),
                    Font = semiboldFont,
                    BackColor = successColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(10, 5, 0, 5),
                    Cursor = Cursors.Hand
                };
                btnAdd.FlatAppearance.BorderSize = 0;

                buttonPanel.Controls.Add(btnCancel);
                buttonPanel.Controls.Add(btnAdd);
                tlp.Controls.Add(buttonPanel, 1, 7);

                addRateDialog.Controls.Add(tlp);

                btnAdd.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Please enter a name.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboCourtType.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a court type.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cboGameType.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a game type.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtRate.Text) || !decimal.TryParse(txtRate.Text, out decimal rate) || rate <= 0)
                    {
                        MessageBox.Show("Please enter a valid rate.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            string insertQuery = @"
                                INSERT INTO game_rates (name, court_type, game_type, rate, description, image, status) 
                                VALUES (@name, @courtType, @gameType, @rate, @description, @image, @status);
                                SELECT LAST_INSERT_ID();";

                            MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@courtType", cboCourtType.Text);
                            cmd.Parameters.AddWithValue("@gameType", cboGameType.Text);
                            cmd.Parameters.AddWithValue("@rate", rate);
                            cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                            cmd.Parameters.AddWithValue("@image", selectedImageBytes ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@status", cboStatus.Text);

                            int newId = Convert.ToInt32(cmd.ExecuteScalar());

                            var newRate = new GameRate
                            {
                                Id = newId,
                                Name = txtName.Text.Trim(),
                                CourtType = cboCourtType.Text,
                                GameType = cboGameType.Text,
                                Rate = rate,
                                Description = txtDescription.Text,
                                Status = cboStatus.Text,
                                ImageData = selectedImageBytes
                            };

                            if (selectedImageBytes != null)
                            {
                                newRate.Image = GetImageFromBytes(selectedImageBytes);
                            }

                            gameRates.Add(newRate);
                            DisplayGameRates();
                            UpdateStatistics();
                        }

                        Activitylogs.Instance.LogGameRateActivity(currentUser, "Added", txtName.Text.Trim(),
                            $"Court: {cboCourtType.Text}, Game: {cboGameType.Text}, Rate: ₱{rate}");

                        MessageBox.Show("Game rate added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        addRateDialog.Close();
                    }
                    catch (Exception ex)
                    {
                        Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, "Error adding new game rate");
                        MessageBox.Show($"Error saving: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                addRateDialog.ShowDialog(this);
            }
        }

        private bool IsCourtTypeUsed(string courtName)
        {
            return gameRates.Any(r => r.CourtType == courtName);
        }

        private bool IsGameTypeUsed(string gameName)
        {
            return gameRates.Any(r => r.GameType == gameName);
        }

        private void BtnDeleteCourt_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CourtType court = (CourtType)btn.Tag;

            if (IsCourtTypeUsed(court.CourtName))
            {
                MessageBox.Show($"Cannot delete '{court.CourtName}' because it's used in existing rates.",
                    "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Delete court type '{court.CourtName}'?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM court_types WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@id", court.Id);
                        cmd.ExecuteNonQuery();
                    }

                    string deletedCourt = court.CourtName;
                    courtTypes.Remove(court);
                    LoadCourtCards();

                    Activitylogs.Instance.AddLogEntry(currentUser, "Court Type Deleted", $"Court type '{deletedCourt}' was deleted", "Info", "GameRates");

                    MessageBox.Show($"Court type '{deletedCourt}' deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, $"Error deleting court type {court.CourtName}");
                    MessageBox.Show($"Error deleting: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteGameType_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GameType gameType = (GameType)btn.Tag;

            if (IsGameTypeUsed(gameType.GameName))
            {
                MessageBox.Show($"Cannot delete '{gameType.GameName}' because it's used in existing rates.",
                    "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Delete game type '{gameType.GameName}'?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM game_types WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@id", gameType.Id);
                        cmd.ExecuteNonQuery();
                    }

                    string deletedGame = gameType.GameName;
                    gameTypesList.Remove(gameType);
                    LoadGameTypeCards();

                    Activitylogs.Instance.AddLogEntry(currentUser, "Game Type Deleted", $"Game type '{deletedGame}' was deleted", "Info", "GameRates");

                    MessageBox.Show($"Game type '{deletedGame}' deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, $"Error deleting game type {gameType.GameName}");
                    MessageBox.Show($"Error deleting: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ADD COURT BUTTON CLICK HANDLER
        private void btnAddCourt_Click(object sender, EventArgs e)
        {
            string newCourtName = ShowInputDialog("Add Court Type", "Enter new court type:");
            if (!string.IsNullOrWhiteSpace(newCourtName))
            {
                if (courtTypes.Any(c => c.CourtName.Equals(newCourtName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Court type already exists!", "Duplicate",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "INSERT INTO court_types (court_name, description) VALUES (@name, @desc); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@name", newCourtName);
                        cmd.Parameters.AddWithValue("@desc", $"Court type for {newCourtName} games");

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        courtTypes.Add(new CourtType
                        {
                            Id = newId,
                            CourtName = newCourtName,
                            Description = $"Court type for {newCourtName} games"
                        });

                        LoadCourtCards();
                    }

                    Activitylogs.Instance.AddLogEntry(currentUser, "Court Type Added", $"New court type '{newCourtName}' was added", "Info", "GameRates");

                    MessageBox.Show($"Court type '{newCourtName}' added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, $"Error adding court type {newCourtName}");
                    MessageBox.Show($"Error adding: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ADD GAME TYPE BUTTON CLICK HANDLER
        private void btnAddGameType_Click(object sender, EventArgs e)
        {
            string newGameName = ShowInputDialog("Add Game Type", "Enter new game type:");
            if (!string.IsNullOrWhiteSpace(newGameName))
            {
                if (gameTypesList.Any(g => g.GameName.Equals(newGameName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Game type already exists!", "Duplicate",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "INSERT INTO game_types (game_name, description) VALUES (@name, @desc); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@name", newGameName);
                        cmd.Parameters.AddWithValue("@desc", $"{newGameName} game type");

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        gameTypesList.Add(new GameType
                        {
                            Id = newId,
                            GameName = newGameName,
                            Description = $"{newGameName} game type"
                        });

                        LoadGameTypeCards();
                    }

                    Activitylogs.Instance.AddLogEntry(currentUser, "Game Type Added", $"New game type '{newGameName}' was added", "Info", "GameRates");

                    MessageBox.Show($"Game type '{newGameName}' added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Activitylogs.Instance.LogError(currentUser, "GameRates", ex.Message, $"Error adding game type {newGameName}");
                    MessageBox.Show($"Error adding: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            // Show the management overlay
            managementOverlay.Visible = true;
            managementOverlay.BringToFront();
            btnManage.Text = "Hide Management";

            LoadCourtCards();
            LoadGameTypeCards();

            Activitylogs.Instance.AddLogEntry(currentUser, "Management View", "Opened court and game type management", "Info", "GameRates");
        }

        private void btnCloseManagement_Click(object sender, EventArgs e)
        {
            // Hide the management overlay
            managementOverlay.Visible = false;
            btnManage.Text = "Manage Courts/Types";
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ShowAddRateDialog();
        }

        private void filterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayGameRates();
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