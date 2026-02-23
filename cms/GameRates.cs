using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cms
{
    public partial class GameRates : UserControl
    {
        private bool _isAddingNew;
        private int _currentRowIndex;
        private List<string> _courtTypes;
        private List<string> _gameTypes;

        // Database connection string for XAMPP MySQL
        private string connectionString = "Server=localhost;Database=matchpoint_db;Uid=root;Pwd=;";
        private MySqlConnection connection;

        public GameRates()
        {
            InitializeComponent();
            _courtTypes = new List<string>();
            _gameTypes = new List<string>();
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

                // Create game_rates table
                string createGameRatesTable = @"
                    CREATE TABLE IF NOT EXISTS game_rates (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(200) NOT NULL,
                        court_type VARCHAR(100) NOT NULL,
                        game_type VARCHAR(100) NOT NULL,
                        rate DECIMAL(10,2) NOT NULL,
                        description TEXT,
                        status ENUM('Enabled', 'Disabled') DEFAULT 'Enabled',
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        FOREIGN KEY (court_type) REFERENCES court_types(court_name) ON DELETE RESTRICT,
                        FOREIGN KEY (game_type) REFERENCES game_types(game_name) ON DELETE RESTRICT
                    )";
                MySqlCommand cmd3 = new MySqlCommand(createGameRatesTable, connection);
                cmd3.ExecuteNonQuery();

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

                string query = "SELECT status, name, court_type, game_type, rate, description FROM game_rates ORDER BY name";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvGameRates.Rows.Add(
                            reader["status"].ToString(),
                            reader["name"].ToString(),
                            reader["court_type"].ToString(),
                            reader["game_type"].ToString(),
                            reader["rate"].ToString(),
                            reader["description"].ToString()
                        );
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

        private void InitializeControls()
        {
            // Setup event handlers
            btnAddNew.Click += BtnAddNew_Click;
            btnManage.Click += BtnManage_Click;
            dgvGameRates.CellContentClick += DgvGameRates_CellContentClick;

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

            // Add sample data with status (Enabled/Disabled)
            dgvGameRates.Rows.Add("Enabled", "Badminton Court 1", "Indoor", "Badminton", "500", "Professional indoor court, shuttlecock provided");
            dgvGameRates.Rows.Add("Enabled", "Tennis Court A", "Outdoor", "Tennis", "800", "Floodlit court, racket rental available");
            dgvGameRates.Rows.Add("Disabled", "Basketball Court", "Indoor", "Basketball", "700", "Full court with spectators area, glass backboards");
            dgvGameRates.Rows.Add("Enabled", "Volleyball Court", "Outdoor", "Volleyball", "600", "Beach sand court, net included, lights available");

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

        private void ShowAddRateDialog()
        {
            using (var addRateDialog = new Form())
            {
                addRateDialog.Text = "Add New Game Rate";
                addRateDialog.Size = new Size(550, 550);
                addRateDialog.StartPosition = FormStartPosition.CenterParent;
                addRateDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                addRateDialog.MaximizeBox = false;
                addRateDialog.MinimizeBox = false;
                addRateDialog.BackColor = Color.White;

                // Create controls with proper font initialization
                System.Drawing.Font regularFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                System.Drawing.Font semiboldFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);

                Label lblName = new Label { Text = "Name:", Left = 30, Top = 30, Width = 100, Font = regularFont };
                TextBox txtName = new TextBox { Left = 150, Top = 27, Width = 320, Font = regularFont };

                // Court Type 
                Label lblCourtType = new Label { Text = "Court Type:", Left = 30, Top = 80, Width = 100, Font = regularFont };
                ComboBox cboCourtType = new ComboBox
                {
                    Left = 150,
                    Top = 77,
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = regularFont
                };

                // Game Type 
                Label lblGameType = new Label { Text = "Game Type:", Left = 30, Top = 130, Width = 100, Font = regularFont };
                ComboBox cboGameType = new ComboBox
                {
                    Left = 150,
                    Top = 127,
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = regularFont
                };

                // Load initial data
                LoadCourtTypes(cboCourtType);
                LoadGameTypes(cboGameType);

                Label lblRate = new Label { Text = "Rate per hour:", Left = 30, Top = 180, Width = 100, Font = regularFont };
                TextBox txtRate = new TextBox { Left = 150, Top = 177, Width = 320, Font = regularFont };

                Label lblDescription = new Label { Text = "Description:", Left = 30, Top = 230, Width = 100, Font = regularFont };
                TextBox txtDescription = new TextBox
                {
                    Left = 150,
                    Top = 227,
                    Width = 320,
                    Height = 80,
                    Multiline = true,
                    Font = regularFont,
                    Text = "e.g., Fast-paced 5v5, indoor court, includes referee",
                    ForeColor = Color.Gray
                };

                // Status selection
                Label lblStatus = new Label { Text = "Initial Status:", Left = 30, Top = 320, Width = 100, Font = regularFont };
                ComboBox cboStatus = new ComboBox
                {
                    Left = 150,
                    Top = 317,
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = regularFont
                };
                cboStatus.Items.AddRange(new object[] { "Enabled", "Disabled" });
                cboStatus.SelectedIndex = 0;

                // Placeholder events
                txtDescription.GotFocus += (s, args) =>
                {
                    if (txtDescription.Text == "e.g., Fast-paced 5v5, indoor court, includes referee")
                    {
                        txtDescription.Text = "";
                        txtDescription.ForeColor = Color.Black;
                    }
                };

                txtDescription.LostFocus += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtDescription.Text))
                    {
                        txtDescription.Text = "e.g., Fast-paced 5v5, indoor court, includes referee";
                        txtDescription.ForeColor = Color.Gray;
                    }
                };

                Button btnAdd = new Button
                {
                    Text = "Add Rate",
                    Left = 150,
                    Top = 380,
                    Width = 120,
                    Height = 40,
                    BackColor = Color.FromArgb(40, 167, 69),
                    FlatStyle = FlatStyle.Flat,
                    Font = semiboldFont,
                    ForeColor = Color.White
                };
                btnAdd.FlatAppearance.BorderSize = 0;

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Left = 280,
                    Top = 380,
                    Width = 120,
                    Height = 40,
                    BackColor = Color.FromArgb(108, 117, 125),
                    FlatStyle = FlatStyle.Flat,
                    Font = regularFont,
                    ForeColor = Color.White
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                // Add click event
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
                            if (description == "e.g., Fast-paced 5v5, indoor court, includes referee")
                                description = "";

                            string insertQuery = @"
                                INSERT INTO game_rates (name, court_type, game_type, rate, description, status) 
                                VALUES (@name, @courtType, @gameType, @rate, @description, @status)";

                            MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@courtType", cboCourtType.Text);
                            cmd.Parameters.AddWithValue("@gameType", cboGameType.Text);
                            cmd.Parameters.AddWithValue("@rate", rate);
                            cmd.Parameters.AddWithValue("@description", description);
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

                // Add controls to form
                addRateDialog.Controls.AddRange(new Control[] {
                    lblName, txtName,
                    lblCourtType, cboCourtType,
                    lblGameType, cboGameType,
                    lblRate, txtRate,
                    lblDescription, txtDescription,
                    lblStatus, cboStatus,
                    btnAdd, btnCancel
                });

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

            string name = dgvGameRates.Rows[rowIndex].Cells["colName"].Value?.ToString() ?? "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE game_rates SET status = @status WHERE name = @name";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@name", name);
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

            string status = row.Cells["colStatus"].Value?.ToString() ?? "Enabled";
            string name = row.Cells["colName"].Value?.ToString() ?? "";
            string courtType = row.Cells["colCourtType"].Value?.ToString() ?? "";
            string gameType = row.Cells["colGameType"].Value?.ToString() ?? "";
            string rate = row.Cells["colRate"].Value?.ToString() ?? "";
            string description = row.Cells["colDescription"].Value?.ToString() ?? "";
            string originalName = name; // Store original name for database update

            // Create edit dialog
            using (var editDialog = new Form())
            {
                editDialog.Text = "Edit Game Rate";
                editDialog.Size = new Size(550, 550);
                editDialog.StartPosition = FormStartPosition.CenterParent;
                editDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                editDialog.MaximizeBox = false;
                editDialog.MinimizeBox = false;
                editDialog.BackColor = Color.White;

                // Create controls with proper font initialization
                System.Drawing.Font regularFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                System.Drawing.Font semiboldFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);

                Label lblName = new Label { Text = "Name:", Left = 30, Top = 30, Width = 100, Font = regularFont };
                TextBox txtNameEdit = new TextBox { Left = 150, Top = 27, Width = 320, Font = regularFont, Text = name };

                // Court Type
                Label lblCourtType = new Label { Text = "Court Type:", Left = 30, Top = 80, Width = 100, Font = regularFont };
                ComboBox cboCourtTypeEdit = new ComboBox
                {
                    Left = 150,
                    Top = 77,
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = regularFont
                };

                // Load court types
                LoadCourtTypes(cboCourtTypeEdit);
                if (!string.IsNullOrEmpty(courtType))
                    cboCourtTypeEdit.SelectedItem = courtType;

                // Game Type
                Label lblGameType = new Label { Text = "Game Type:", Left = 30, Top = 130, Width = 100, Font = regularFont };
                ComboBox cboGameTypeEdit = new ComboBox
                {
                    Left = 150,
                    Top = 127,
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = regularFont
                };

                // Load game types
                LoadGameTypes(cboGameTypeEdit);
                if (!string.IsNullOrEmpty(gameType))
                    cboGameTypeEdit.SelectedItem = gameType;

                Label lblRate = new Label { Text = "Rate per hour:", Left = 30, Top = 180, Width = 100, Font = regularFont };
                TextBox txtRateEdit = new TextBox { Left = 150, Top = 177, Width = 320, Font = regularFont, Text = rate };

                Label lblDescription = new Label { Text = "Description:", Left = 30, Top = 230, Width = 100, Font = regularFont };
                TextBox txtDescriptionEdit = new TextBox
                {
                    Left = 150,
                    Top = 227,
                    Width = 320,
                    Height = 80,
                    Multiline = true,
                    Font = regularFont,
                    Text = string.IsNullOrEmpty(description) ? "e.g., Fast-paced 5v5, indoor court, includes referee" : description,
                    ForeColor = string.IsNullOrEmpty(description) ? Color.Gray : Color.Black
                };

                // Status
                Label lblStatus = new Label { Text = "Status:", Left = 30, Top = 320, Width = 100, Font = regularFont };
                ComboBox cboStatusEdit = new ComboBox
                {
                    Left = 150,
                    Top = 317,
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = regularFont
                };
                cboStatusEdit.Items.AddRange(new object[] { "Enabled", "Disabled" });
                cboStatusEdit.SelectedItem = status;

                // Placeholder events
                txtDescriptionEdit.GotFocus += (s, args) =>
                {
                    if (txtDescriptionEdit.Text == "e.g., Fast-paced 5v5, indoor court, includes referee")
                    {
                        txtDescriptionEdit.Text = "";
                        txtDescriptionEdit.ForeColor = Color.Black;
                    }
                };

                txtDescriptionEdit.LostFocus += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtDescriptionEdit.Text))
                    {
                        txtDescriptionEdit.Text = "e.g., Fast-paced 5v5, indoor court, includes referee";
                        txtDescriptionEdit.ForeColor = Color.Gray;
                    }
                };

                Button btnSave = new Button
                {
                    Text = "Save Changes",
                    Left = 150,
                    Top = 380,
                    Width = 120,
                    Height = 40,
                    BackColor = Color.FromArgb(40, 167, 69),
                    FlatStyle = FlatStyle.Flat,
                    Font = semiboldFont,
                    ForeColor = Color.White
                };
                btnSave.FlatAppearance.BorderSize = 0;

                Button btnDelete = new Button
                {
                    Text = "Delete",
                    Left = 30,
                    Top = 380,
                    Width = 100,
                    Height = 40,
                    BackColor = Color.FromArgb(220, 53, 69),
                    FlatStyle = FlatStyle.Flat,
                    Font = regularFont,
                    ForeColor = Color.White
                };
                btnDelete.FlatAppearance.BorderSize = 0;

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Left = 280,
                    Top = 380,
                    Width = 120,
                    Height = 40,
                    BackColor = Color.FromArgb(108, 117, 125),
                    FlatStyle = FlatStyle.Flat,
                    Font = regularFont,
                    ForeColor = Color.White
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                // Save click
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
                            if (newDescription == "e.g., Fast-paced 5v5, indoor court, includes referee")
                                newDescription = "";

                            string updateQuery = @"
                                UPDATE game_rates 
                                SET name = @newName, court_type = @courtType, game_type = @gameType, 
                                    rate = @rate, description = @description, status = @status
                                WHERE name = @originalName";

                            MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                            cmd.Parameters.AddWithValue("@newName", txtNameEdit.Text.Trim());
                            cmd.Parameters.AddWithValue("@courtType", cboCourtTypeEdit.Text);
                            cmd.Parameters.AddWithValue("@gameType", cboGameTypeEdit.Text);
                            cmd.Parameters.AddWithValue("@rate", rateVal);
                            cmd.Parameters.AddWithValue("@description", newDescription);
                            cmd.Parameters.AddWithValue("@status", cboStatusEdit.Text);
                            cmd.Parameters.AddWithValue("@originalName", originalName);

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

                // Delete click
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
                                string deleteQuery = "DELETE FROM game_rates WHERE name = @name";
                                MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                                cmd.Parameters.AddWithValue("@name", originalName);
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

                // Add controls
                editDialog.Controls.AddRange(new Control[] {
                    lblName, txtNameEdit,
                    lblCourtType, cboCourtTypeEdit,
                    lblGameType, cboGameTypeEdit,
                    lblRate, txtRateEdit,
                    lblDescription, txtDescriptionEdit,
                    lblStatus, cboStatusEdit,
                    btnSave, btnDelete, btnCancel
                });

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
    }
}