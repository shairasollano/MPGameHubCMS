using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace cms
{
    public partial class GameRates : UserControl
    {
        private bool _isAddingNew;
        private int _currentRowIndex;
        private List<string> _courtTypes;
        private List<string> _gameTypes;
        private Dictionary<int, byte[]> _imageCache; // Cache for images

        // Database connection string for XAMPP MySQL
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";
        private MySqlConnection connection;

        public GameRates()
        {
            InitializeComponent();
            _courtTypes = new List<string>();
            _gameTypes = new List<string>();
            _imageCache = new Dictionary<int, byte[]>();
            InitializeDatabase();
            InitializeControls();
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

                // Create court_types table
                string createCourtTypesTable = @"
                    CREATE TABLE IF NOT EXISTS court_types (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        court_name VARCHAR(100) NOT NULL UNIQUE,
                        description TEXT,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";
                MySqlCommand cmd1 = new MySqlCommand(createCourtTypesTable, connection);
                cmd1.ExecuteNonQuery();

                // Create game_types table
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
                    // Create game_rates table with image column
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
            }

            // Check game_rates (don't insert sample data if table already has data)
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
                    _courtTypes.Clear();
                    string courtQuery = "SELECT court_name FROM court_types ORDER BY court_name";
                    MySqlCommand courtCmd = new MySqlCommand(courtQuery, connection);
                    using (MySqlDataReader reader = courtCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _courtTypes.Add(reader["court_name"].ToString());
                        }
                    }

                    // Load game types
                    _gameTypes.Clear();
                    string gameQuery = "SELECT game_name FROM game_types ORDER BY game_name";
                    MySqlCommand gameCmd = new MySqlCommand(gameQuery, connection);
                    using (MySqlDataReader reader = gameCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _gameTypes.Add(reader["game_name"].ToString());
                        }
                    }

                    // Load game rates into DataGridView
                    LoadGameRatesFromDatabase();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from database: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InitializeDefaultOptions();
            }
        }

        private void LoadGameRatesFromDatabase()
        {
            try
            {
                dgvGameRates.Rows.Clear();
                _imageCache.Clear();

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

                        int rowIndex = dgvGameRates.Rows.Add(
                            reader["status"].ToString(),
                            image,
                            reader["name"].ToString(),
                            reader["court_type"].ToString(),
                            reader["game_type"].ToString(),
                            reader["rate"].ToString(),
                            reader["description"].ToString()
                        );

                        // Store the ID in the row tag for later reference
                        dgvGameRates.Rows[rowIndex].Tag = id;
                    }
                }

                StyleStatusCells();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game rates: {ex.Message}",
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
                    // Save as JPEG to reduce size
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
            // Setup event handlers
            btnAddNew.Click += BtnAddNew_Click;
            btnManage.Click += BtnManage_Click;
            dgvGameRates.CellContentClick += DgvGameRates_CellContentClick;
            dgvGameRates.CellFormatting += DgvGameRates_CellFormatting;
            dgvGameRates.DataError += DgvGameRates_DataError;

            // Setup status filter
            cboFilterStatus.SelectedIndexChanged += CboFilterStatus_SelectedIndexChanged;

            // Setup management panel event handlers
            btnAddCourt.Click += BtnAddCourt_Click;
            btnAddGameType.Click += BtnAddGameType_Click;
            btnCloseCourts.Click += BtnCloseManagement_Click;
            btnCloseGameTypes.Click += BtnCloseManagement_Click;

            // Setup DataGridView events for management
            dgvCourts.CellContentClick += DgvCourts_CellContentClick;
            dgvGameTypes.CellContentClick += DgvGameTypes_CellContentClick;

            // Load data into management grids
            LoadCourtsData();
            LoadGameTypesData();

            // Initially hide management panel
            panelManagement.Visible = false;

            // Set default filter to "All"
            cboFilterStatus.SelectedIndex = 0;
        }

        private void DgvGameRates_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Handle the data error to prevent the default error dialog
            if (e.ColumnIndex == dgvGameRates.Columns["colImage"].Index)
            {
                // For image column errors, just set the cell value to null
                dgvGameRates.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
            }

            // Mark the error as handled
            e.ThrowException = false;
            e.Cancel = true;
        }

        private void DgvGameRates_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Handle image column formatting if needed
            if (dgvGameRates.Columns[e.ColumnIndex].Name == "colImage")
            {
                if (e.Value == null)
                {
                    // Leave empty for null images
                    e.Value = null;
                    e.FormattingApplied = true;
                }
            }
        }

        private void InitializeDefaultOptions()
        {
            // Initialize default court types
            _courtTypes.AddRange(new[] { "Indoor", "Outdoor", "Hard Court", "Clay Court", "Grass Court", "Synthetic" });

            // Initialize default game types
            _gameTypes.AddRange(new[] { "Badminton", "Tennis", "Basketball", "Volleyball", "Squash", "Table Tennis", "Futsal" });

            // Load data into management grids
            LoadCourtsData();
            LoadGameTypesData();

            // Add sample data to main grid
            AddSampleData();
        }

        private void LoadCourtsData()
        {
            dgvCourts.Rows.Clear();
            for (int i = 0; i < _courtTypes.Count; i++)
            {
                dgvCourts.Rows.Add(i + 1, _courtTypes[i], $"Court type for {_courtTypes[i]} games");
            }
        }

        private void LoadGameTypesData()
        {
            dgvGameTypes.Rows.Clear();
            for (int i = 0; i < _gameTypes.Count; i++)
            {
                dgvGameTypes.Rows.Add(i + 1, _gameTypes[i], $"{_gameTypes[i]} game type");
            }
        }

        private void AddSampleData()
        {
            // Clear existing data
            dgvGameRates.Rows.Clear();

            // Add sample data with status (Enabled/Disabled) - no images
            dgvGameRates.Rows.Add("Enabled", null, "Badminton Court 1", "Indoor", "Badminton", "500", "Professional indoor court, shuttlecock provided");
            dgvGameRates.Rows.Add("Enabled", null, "Tennis Court A", "Outdoor", "Tennis", "800", "Floodlit court, racket rental available");
            dgvGameRates.Rows.Add("Disabled", null, "Basketball Court", "Indoor", "Basketball", "700", "Full court with spectators area, glass backboards");
            dgvGameRates.Rows.Add("Enabled", null, "Volleyball Court", "Outdoor", "Volleyball", "600", "Beach sand court, net included, lights available");

            // Style the status cells
            StyleStatusCells();
        }

        private void StyleStatusCells()
        {
            foreach (DataGridViewRow row in dgvGameRates.Rows)
            {
                if (row.IsNewRow) continue;

                string status = row.Cells["colStatus"].Value?.ToString() ?? "Disabled";
                if (status == "Enabled")
                {
                    row.Cells["colStatus"].Style.BackColor = Color.FromArgb(40, 167, 69);
                    row.Cells["colStatus"].Style.ForeColor = Color.White;
                    row.Cells["colStatus"].Style.SelectionBackColor = Color.FromArgb(40, 167, 69);
                    row.Cells["colStatus"].Style.SelectionForeColor = Color.White;
                }
                else
                {
                    row.Cells["colStatus"].Style.BackColor = Color.FromArgb(220, 53, 69);
                    row.Cells["colStatus"].Style.ForeColor = Color.White;
                    row.Cells["colStatus"].Style.SelectionBackColor = Color.FromArgb(220, 53, 69);
                    row.Cells["colStatus"].Style.SelectionForeColor = Color.White;
                }
            }
        }

        private void LoadCourtTypes(ComboBox cbo)
        {
            cbo.Items.Clear();
            foreach (var courtType in _courtTypes)
                cbo.Items.Add(courtType);
            if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
        }

        private void LoadGameTypes(ComboBox cbo)
        {
            cbo.Items.Clear();
            foreach (var gameType in _gameTypes)
                cbo.Items.Add(gameType);
            if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
        }

        private bool IsCourtTypeUsed(string courtType)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM game_rates WHERE court_type = @courtType";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@courtType", courtType);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch
            {
                // Fallback to checking DataGridView
                foreach (DataGridViewRow row in dgvGameRates.Rows)
                {
                    if (row.Cells["colCourtType"].Value?.ToString() == courtType)
                        return true;
                }
                return false;
            }
        }

        private bool IsGameTypeUsed(string gameType)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM game_rates WHERE game_type = @gameType";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@gameType", gameType);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch
            {
                // Fallback to checking DataGridView
                foreach (DataGridViewRow row in dgvGameRates.Rows)
                {
                    if (row.Cells["colGameType"].Value?.ToString() == gameType)
                        return true;
                }
                return false;
            }
        }

        private string ShowInputDialog(string title, string promptText)
        {
            Form inputForm = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            inputForm.Text = title;
            label.Text = promptText;
            textBox.Text = "";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            inputForm.ClientSize = new Size(396, 107);
            inputForm.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            inputForm.ClientSize = new Size(Math.Max(300, label.Right + 10), inputForm.ClientSize.Height);
            inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputForm.StartPosition = FormStartPosition.CenterParent;
            inputForm.MinimizeBox = false;
            inputForm.MaximizeBox = false;
            inputForm.AcceptButton = buttonOk;
            inputForm.CancelButton = buttonCancel;

            DialogResult dialogResult = inputForm.ShowDialog(this);
            return dialogResult == DialogResult.OK ? textBox.Text : null;
        }

        private void BtnManage_Click(object sender, EventArgs e)
        {
            // Toggle the management panel visibility
            panelManagement.Visible = !panelManagement.Visible;

            // Refresh the data when showing
            if (panelManagement.Visible)
            {
                RefreshDataFromDatabase();
            }
        }

        private void RefreshDataFromDatabase()
        {
            try
            {
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Reload court types
                    _courtTypes.Clear();
                    string courtQuery = "SELECT court_name FROM court_types ORDER BY court_name";
                    MySqlCommand courtCmd = new MySqlCommand(courtQuery, connection);
                    using (MySqlDataReader reader = courtCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _courtTypes.Add(reader["court_name"].ToString());
                        }
                    }

                    // Reload game types
                    _gameTypes.Clear();
                    string gameQuery = "SELECT game_name FROM game_types ORDER BY game_name";
                    MySqlCommand gameCmd = new MySqlCommand(gameQuery, connection);
                    using (MySqlDataReader reader = gameCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _gameTypes.Add(reader["game_name"].ToString());
                        }
                    }

                    // Reload game rates
                    LoadGameRatesFromDatabase();

                    // Update management grids
                    LoadCourtsData();
                    LoadGameTypesData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAddNew_Click(object sender, EventArgs e)
        {
            ShowAddRateDialog();
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
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
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

                // Create controls with proper font initialization
                System.Drawing.Font regularFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                System.Drawing.Font semiboldFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);

                // Create a TableLayoutPanel for better organization
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
                LoadCourtTypes(cboCourtType);
                tlp.Controls.Add(cboCourtType, 1, 1);

                // Game Type
                tlp.Controls.Add(new Label { Text = "Game Type:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 2);
                ComboBox cboGameType = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                LoadGameTypes(cboGameType);
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
                    BackColor = Color.LightGray
                };
                Button btnBrowse = new Button
                {
                    Text = "Browse...",
                    Location = new Point(110, 40),
                    Size = new Size(100, 30),
                    Font = regularFont
                };
                Label lblImagePath = new Label
                {
                    Text = "No image selected",
                    Location = new Point(220, 45),
                    AutoSize = true,
                    Font = regularFont,
                    ForeColor = Color.Gray
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
                                    // Resize image if too large
                                    Image resizedImg = ResizeImage(img, 300, 300);
                                    pbPreview.Image = resizedImg;
                                    selectedImageBytes = GetBytesFromImage(resizedImg);
                                    lblImagePath.Text = Path.GetFileName(ofd.FileName);
                                    lblImagePath.ForeColor = Color.Black;
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

                imagePanel.Controls.Add(pbPreview);
                imagePanel.Controls.Add(btnBrowse);
                imagePanel.Controls.Add(lblImagePath);
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
                tlp.Controls.Add(new Label { Text = "Initial Status:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 6);
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
                    Height = 50
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 35),
                    Font = regularFont,
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                Button btnAdd = new Button
                {
                    Text = "Add Rate",
                    Size = new Size(120, 35),
                    Font = semiboldFont,
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(10, 0, 0, 0)
                };
                btnAdd.FlatAppearance.BorderSize = 0;

                buttonPanel.Controls.Add(btnCancel);
                buttonPanel.Controls.Add(btnAdd);
                tlp.Controls.Add(buttonPanel, 1, 7);

                addRateDialog.Controls.Add(tlp);

                btnAdd.Click += (s, args) =>
                {
                    // Validate inputs
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
                        MessageBox.Show("Please enter a valid rate (positive number).", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            string description = txtDescription.Text;
                            string insertQuery = @"
                                INSERT INTO game_rates (name, court_type, game_type, rate, description, image, status) 
                                VALUES (@name, @courtType, @gameType, @rate, @description, @image, @status)";

                            MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@courtType", cboCourtType.Text);
                            cmd.Parameters.AddWithValue("@gameType", cboGameType.Text);
                            cmd.Parameters.AddWithValue("@rate", rate);
                            cmd.Parameters.AddWithValue("@description", description);
                            cmd.Parameters.AddWithValue("@image", selectedImageBytes ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@status", cboStatus.Text);

                            cmd.ExecuteNonQuery();
                        }

                        // Refresh the grid
                        RefreshDataFromDatabase();

                        MessageBox.Show("Game rate added successfully to database!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        addRateDialog.DialogResult = DialogResult.OK;
                        addRateDialog.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving to database: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancel.Click += (s, args) =>
                {
                    addRateDialog.DialogResult = DialogResult.Cancel;
                    addRateDialog.Close();
                };

                addRateDialog.AcceptButton = btnAdd;
                addRateDialog.CancelButton = btnCancel;

                addRateDialog.ShowDialog(this);
            }
        }

        private void DgvGameRates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Handle Status toggle
            if (e.ColumnIndex == dgvGameRates.Columns["colStatus"].Index)
            {
                ToggleStatus(e.RowIndex);
            }
            // Handle Edit button
            else if (e.ColumnIndex == dgvGameRates.Columns["colEdit"].Index)
            {
                ShowEditDialog(e.RowIndex);
            }
        }

        private void ToggleStatus(int rowIndex)
        {
            var currentValue = dgvGameRates.Rows[rowIndex].Cells["colStatus"].Value?.ToString() ?? "Disabled";
            string newStatus = currentValue == "Enabled" ? "Disabled" : "Enabled";

            int id = (int)dgvGameRates.Rows[rowIndex].Tag;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE game_rates SET status = @status WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                // Update status in grid
                dgvGameRates.Rows[rowIndex].Cells["colStatus"].Value = newStatus;

                // Update styling
                if (newStatus == "Enabled")
                {
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.BackColor = Color.FromArgb(40, 167, 69);
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.ForeColor = Color.White;
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.SelectionBackColor = Color.FromArgb(40, 167, 69);
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.SelectionForeColor = Color.White;
                }
                else
                {
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.BackColor = Color.FromArgb(220, 53, 69);
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.ForeColor = Color.White;
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.SelectionBackColor = Color.FromArgb(220, 53, 69);
                    dgvGameRates.Rows[rowIndex].Cells["colStatus"].Style.SelectionForeColor = Color.White;
                }

                // Re-apply filter based on current selection
                if (cboFilterStatus.SelectedItem != null)
                {
                    ApplyStatusFilter(cboFilterStatus.SelectedItem.ToString());
                }

                MessageBox.Show($"Game rate {newStatus.ToLower()} in database!", "Status Changed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating status: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = cboFilterStatus.SelectedItem?.ToString() ?? "All";
            ApplyStatusFilter(filter);
        }

        private void ApplyStatusFilter(string filter)
        {
            foreach (DataGridViewRow row in dgvGameRates.Rows)
            {
                if (row.IsNewRow) continue;

                string status = row.Cells["colStatus"].Value?.ToString() ?? "Disabled";
                bool showRow = filter == "All" ||
                              (filter == "Enabled Only" && status == "Enabled") ||
                              (filter == "Disabled Only" && status == "Disabled");

                row.Visible = showRow;
            }
        }

        private void ShowEditDialog(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvGameRates.Rows.Count)
                return;

            DataGridViewRow row = dgvGameRates.Rows[rowIndex];
            int id = (int)row.Tag;
            string status = row.Cells["colStatus"].Value?.ToString() ?? "Enabled";
            string name = row.Cells["colName"].Value?.ToString() ?? "";
            string courtType = row.Cells["colCourtType"].Value?.ToString() ?? "";
            string gameType = row.Cells["colGameType"].Value?.ToString() ?? "";
            string rate = row.Cells["colRate"].Value?.ToString() ?? "";
            string description = row.Cells["colDescription"].Value?.ToString() ?? "";
            byte[] existingImage = _imageCache.ContainsKey(id) ? _imageCache[id] : null;

            // Create edit dialog
            using (var editDialog = new Form())
            {
                editDialog.Text = "Edit Game Rate";
                editDialog.Size = new Size(700, 700);
                editDialog.StartPosition = FormStartPosition.CenterParent;
                editDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                editDialog.MaximizeBox = false;
                editDialog.MinimizeBox = false;
                editDialog.BackColor = Color.White;

                System.Drawing.Font regularFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                System.Drawing.Font semiboldFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);

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
                TextBox txtNameEdit = new TextBox { Font = regularFont, Dock = DockStyle.Fill, Text = name };
                tlp.Controls.Add(txtNameEdit, 1, 0);

                // Court Type
                tlp.Controls.Add(new Label { Text = "Court Type:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 1);
                ComboBox cboCourtTypeEdit = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                LoadCourtTypes(cboCourtTypeEdit);
                if (!string.IsNullOrEmpty(courtType))
                    cboCourtTypeEdit.SelectedItem = courtType;
                tlp.Controls.Add(cboCourtTypeEdit, 1, 1);

                // Game Type
                tlp.Controls.Add(new Label { Text = "Game Type:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 2);
                ComboBox cboGameTypeEdit = new ComboBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                LoadGameTypes(cboGameTypeEdit);
                if (!string.IsNullOrEmpty(gameType))
                    cboGameTypeEdit.SelectedItem = gameType;
                tlp.Controls.Add(cboGameTypeEdit, 1, 2);

                // Rate
                tlp.Controls.Add(new Label { Text = "Rate per hour:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 3);
                TextBox txtRateEdit = new TextBox { Font = regularFont, Dock = DockStyle.Fill, Text = rate };
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
                    BackColor = Color.LightGray
                };

                if (existingImage != null)
                {
                    try
                    {
                        pbPreview.Image = GetImageFromBytes(existingImage);
                    }
                    catch
                    {
                        pbPreview.Image = null;
                    }
                }

                Button btnBrowse = new Button
                {
                    Text = "Browse...",
                    Location = new Point(110, 40),
                    Size = new Size(100, 30),
                    Font = regularFont
                };

                Label lblImagePath = new Label
                {
                    Text = existingImage != null ? "Image loaded" : "No image selected",
                    Location = new Point(220, 45),
                    AutoSize = true,
                    Font = regularFont,
                    ForeColor = existingImage != null ? Color.Black : Color.Gray
                };

                Button btnRemoveImage = new Button
                {
                    Text = "Remove",
                    Location = new Point(220, 70),
                    Size = new Size(80, 25),
                    Font = regularFont,
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Visible = existingImage != null
                };
                btnRemoveImage.FlatAppearance.BorderSize = 0;

                byte[] selectedImageBytes = existingImage;

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
                                    lblImagePath.Text = Path.GetFileName(ofd.FileName);
                                    lblImagePath.ForeColor = Color.Black;
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
                    pbPreview.Image = null;
                    selectedImageBytes = null;
                    lblImagePath.Text = "No image selected";
                    lblImagePath.ForeColor = Color.Gray;
                    btnRemoveImage.Visible = false;
                };

                imagePanel.Controls.Add(pbPreview);
                imagePanel.Controls.Add(btnBrowse);
                imagePanel.Controls.Add(lblImagePath);
                imagePanel.Controls.Add(btnRemoveImage);
                tlp.Controls.Add(imagePanel, 1, 4);

                // Description
                tlp.Controls.Add(new Label { Text = "Description:", Font = regularFont, Anchor = AnchorStyles.Left }, 0, 5);
                TextBox txtDescriptionEdit = new TextBox
                {
                    Font = regularFont,
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    Height = 80,
                    Text = description
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
                cboStatusEdit.SelectedItem = status;
                tlp.Controls.Add(cboStatusEdit, 1, 6);

                // Buttons panel
                FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Dock = DockStyle.Fill,
                    Height = 50
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Size = new Size(100, 35),
                    Font = regularFont,
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                Button btnDelete = new Button
                {
                    Text = "Delete",
                    Size = new Size(100, 35),
                    Font = regularFont,
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnDelete.FlatAppearance.BorderSize = 0;

                Button btnSave = new Button
                {
                    Text = "Save Changes",
                    Size = new Size(120, 35),
                    Font = semiboldFont,
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(10, 0, 0, 0)
                };
                btnSave.FlatAppearance.BorderSize = 0;

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

                            string newDescription = txtDescriptionEdit.Text;
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
                            cmd.Parameters.AddWithValue("@description", newDescription);
                            cmd.Parameters.AddWithValue("@image", selectedImageBytes ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@status", cboStatusEdit.Text);
                            cmd.Parameters.AddWithValue("@id", id);

                            cmd.ExecuteNonQuery();
                        }

                        // Refresh the grid
                        RefreshDataFromDatabase();

                        MessageBox.Show("Game rate updated successfully in database!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        editDialog.DialogResult = DialogResult.OK;
                        editDialog.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating database: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnDelete.Click += (s, args) =>
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this game rate from database?",
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
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }

                            // Refresh the grid
                            RefreshDataFromDatabase();

                            MessageBox.Show("Game rate deleted successfully from database!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            editDialog.DialogResult = DialogResult.OK;
                            editDialog.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting from database: {ex.Message}",
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                btnCancel.Click += (s, args) =>
                {
                    editDialog.DialogResult = DialogResult.Cancel;
                    editDialog.Close();
                };

                editDialog.AcceptButton = btnSave;
                editDialog.CancelButton = btnCancel;

                editDialog.ShowDialog(this);
            }
        }

        #region Management Panel Event Handlers

        private void BtnAddCourt_Click(object sender, EventArgs e)
        {
            string newCourtType = ShowInputDialog("Add Court Type", "Enter new court type:");
            if (!string.IsNullOrWhiteSpace(newCourtType))
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // Check if exists
                        string checkQuery = "SELECT COUNT(*) FROM court_types WHERE court_name = @name";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@name", newCourtType);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (exists > 0)
                        {
                            MessageBox.Show("Court type already exists in database!", "Duplicate",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Insert new court type
                        string insertQuery = "INSERT INTO court_types (court_name, description) VALUES (@name, @desc)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@name", newCourtType);
                        insertCmd.Parameters.AddWithValue("@desc", $"Court type for {newCourtType} games");
                        insertCmd.ExecuteNonQuery();
                    }

                    // Refresh data
                    RefreshDataFromDatabase();

                    MessageBox.Show($"Court type '{newCourtType}' added successfully to database!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding to database: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAddGameType_Click(object sender, EventArgs e)
        {
            string newGameType = ShowInputDialog("Add Game Type", "Enter new game type:");
            if (!string.IsNullOrWhiteSpace(newGameType))
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // Check if exists
                        string checkQuery = "SELECT COUNT(*) FROM game_types WHERE game_name = @name";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@name", newGameType);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (exists > 0)
                        {
                            MessageBox.Show("Game type already exists in database!", "Duplicate",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Insert new game type
                        string insertQuery = "INSERT INTO game_types (game_name, description) VALUES (@name, @desc)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@name", newGameType);
                        insertCmd.Parameters.AddWithValue("@desc", $"{newGameType} game type");
                        insertCmd.ExecuteNonQuery();
                    }

                    // Refresh data
                    RefreshDataFromDatabase();

                    MessageBox.Show($"Game type '{newGameType}' added successfully to database!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding to database: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCloseManagement_Click(object sender, EventArgs e)
        {
            // Hide the management panel when closing
            panelManagement.Visible = false;
        }

        private void DgvCourts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle court management actions (edit/delete)
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvCourts.Columns["colCourtActions"].Index)
            {
                string courtName = dgvCourts.Rows[e.RowIndex].Cells["colCourtName"].Value?.ToString() ?? "";

                // Check if court type is used before allowing delete
                if (IsCourtTypeUsed(courtName))
                {
                    MessageBox.Show($"Cannot delete '{courtName}' because it's used in existing rates.",
                        "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Do you want to delete '{courtName}' from database?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            string deleteQuery = "DELETE FROM court_types WHERE court_name = @name";
                            MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                            cmd.Parameters.AddWithValue("@name", courtName);
                            cmd.ExecuteNonQuery();
                        }

                        // Refresh data
                        RefreshDataFromDatabase();

                        MessageBox.Show("Court type deleted successfully from database!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting from database: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DgvGameTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle game type management actions (edit/delete)
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvGameTypes.Columns["colGameTypeActions"].Index)
            {
                string gameTypeName = dgvGameTypes.Rows[e.RowIndex].Cells["colGameTypeName"].Value?.ToString() ?? "";

                // Check if game type is used before allowing delete
                if (IsGameTypeUsed(gameTypeName))
                {
                    MessageBox.Show($"Cannot delete '{gameTypeName}' because it's used in existing rates.",
                        "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Do you want to delete '{gameTypeName}' from database?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            string deleteQuery = "DELETE FROM game_types WHERE game_name = @name";
                            MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                            cmd.Parameters.AddWithValue("@name", gameTypeName);
                            cmd.ExecuteNonQuery();
                        }

                        // Refresh data
                        RefreshDataFromDatabase();

                        MessageBox.Show("Game type deleted successfully from database!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting from database: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        private void btnAddNew_Click_1(object sender, EventArgs e)
        {

        }

        private void btnManage_Click_1(object sender, EventArgs e)
        {

        }
    }
}