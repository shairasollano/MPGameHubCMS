using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class CashierForm : Form
    {
        // ============ MANAGERS ============
        private CashierSessionManager sessionManager;
        private paymentControl1 paymentControl;

        // ============ USER INFO ============
        private string currentUsername = "";
        private string currentUserRole = "";

        // Timer for date/time
        private Timer dateTimeTimer;

        // ============ CONSTRUCTOR ============
        public CashierForm()
        {
            InitializeComponent();

            sessionManager = new CashierSessionManager();

            paymentControl = new paymentControl1();
            paymentControl.Visible = false;
            paymentControl.Dock = DockStyle.Fill;
            paymentControl.BringToFront();
            paymentControl.PaymentSuccessful += OnPaymentSuccessful;
            this.Controls.Add(paymentControl);

            InitializeEquipmentControl();

            InitializeButtonStyles();
            InitializeRichTextBox();

            // Set form properties
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            // Wire up form closing events
            this.FormClosing += CashierForm_FormClosing;
            this.FormClosed += CashierForm_FormClosed;
        }

        // ⭐ NEW METHOD - Equipment Control
        private void InitializeEquipmentControl()
        {
            equipmentRentalControl1 = new EquipmentRentalControl();
            equipmentRentalControl1.Visible = false;
            equipmentRentalControl1.Location = new Point(
                (this.ClientSize.Width - equipmentRentalControl1.Width) / 2,
                (this.ClientSize.Height - equipmentRentalControl1.Height) / 2
            );
            equipmentRentalControl1.BringToFront();
            this.Controls.Add(equipmentRentalControl1);
        }

        // ============ SET CURRENT USER METHOD ============
        public void SetCurrentUser(string username, string role)
        {
            currentUsername = username;
            currentUserRole = role;

            // Update welcome message
            UpdateCashierDisplay();
        }

        private void UpdateCashierDisplay()
        {
            // Update user label
            if (lblUser != null)
            {
                lblUser.Text = currentUsername;
            }

            // Update form title
            this.Text = $"Cashier POS - {currentUsername}";
        }

        private void InitializeButtonStyles()
        {
            // Game buttons
            ButtonStyleHelper.ApplyGameButtonStyle(btnBilliards);
            ButtonStyleHelper.ApplyGameButtonStyle(btnScooter);
            ButtonStyleHelper.ApplyGameButtonStyle(btnBadminton);
            ButtonStyleHelper.ApplyGameButtonStyle(btnTableTennis);

            // Duration buttons
            ButtonStyleHelper.ApplyDurationButtonStyle(btn30min);
            ButtonStyleHelper.ApplyDurationButtonStyle(btn1hour);

            // Action buttons
            ButtonStyleHelper.ApplyActionButtonStyle(btnProceedPayment, Color.FromArgb(76, 175, 80));
            ButtonStyleHelper.ApplyActionButtonStyle(btnRemoveGame, Color.FromArgb(244, 67, 54));
            ButtonStyleHelper.ApplyActionButtonStyle(btnClearCashierForm, Color.FromArgb(255, 152, 0));
        }

        private void InitializeRichTextBox()
        {
            if (rtbSelectedGames != null)
            {
                rtbSelectedGames.ReadOnly = true;
                rtbSelectedGames.Font = new Font("Courier New", 9);
                rtbSelectedGames.BackColor = Color.White;
                rtbSelectedGames.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void CashierForm_Load(object sender, EventArgs e)
        {
            UpdateDateTime();

            // Create and start date/time timer
            dateTimeTimer = new Timer();
            dateTimeTimer.Interval = 1000;
            dateTimeTimer.Tick += DateTimeTimer_Tick;
            dateTimeTimer.Start();

            RefreshDisplay();
        }

        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        // ============ GAME SELECTION ============
        private void btnBilliards_Click(object sender, EventArgs e)
        {
            SelectGame("Billiards", btnBilliards);
        }

        private void btnScooter_Click(object sender, EventArgs e)
        {
            SelectGame("Scooter", btnScooter);
        }

        private void btnBadminton_Click(object sender, EventArgs e)
        {
            SelectGame("Badminton", btnBadminton);
        }

        private void btnTableTennis_Click(object sender, EventArgs e)
        {
            SelectGame("Table Tennis", btnTableTennis);
        }

        private void SelectGame(string gameName, Button clickedButton)
        {
            if (sessionManager == null)
                sessionManager = new CashierSessionManager();

            sessionManager.SelectedGame = gameName;
            ResetGameButtonColors();
            clickedButton.BackColor = ButtonStyleHelper.SelectedColor;
        }

        private void ResetGameButtonColors()
        {
            ButtonStyleHelper.ResetGameButtons(btnBilliards, btnScooter, btnBadminton, btnTableTennis, btn30min, btn1hour);
        }

        // ============ DURATION SELECTION ============
        private void btn30Min_Click(object sender, EventArgs e)
        {
            AddDurationToGame(30);
            ResetGameButtonColors();
        }

        private void btn1Hour_Click(object sender, EventArgs e)
        {
            AddDurationToGame(60);
            ResetGameButtonColors();
        }

        private void AddDurationToGame(int minutes)
        {
            if (sessionManager == null)
                sessionManager = new CashierSessionManager();

            if (string.IsNullOrEmpty(sessionManager.SelectedGame))
            {
                MessageBox.Show("Please select a game first!", "No Game Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for equipment
            if (sessionManager.HasEquipment(sessionManager.SelectedGame))
            {
                ShowEquipmentSelection(minutes);
            }
            else
            {
                AddSessionWithoutEquipment(minutes);
            }
        }

        // ============ EQUIPMENT SELECTION ============
        // ⭐ NEW METHOD - Show Equipment Modal
        private void ShowEquipmentSelection(int minutes)
        {
            var equipment = sessionManager.GetEquipmentForGame(sessionManager.SelectedGame);

            // ⭐ Disable other controls while modal is open
            btnBilliards.Enabled = false;
            btnScooter.Enabled = false;
            btnBadminton.Enabled = false;
            btnTableTennis.Enabled = false;
            btn30min.Enabled = false;
            btn1hour.Enabled = false;

            

            // Load and show equipment control
            equipmentRentalControl1.LoadEquipment(sessionManager.SelectedGame, equipment);
            equipmentRentalControl1.Visible = true;
            equipmentRentalControl1.BringToFront();

            // Wait for user action
            Timer checkTimer = new Timer();
            checkTimer.Interval = 100;
            int capturedMinutes = minutes;

            checkTimer.Tick += (s, e) =>
            {
                if (!equipmentRentalControl1.Visible)
                {
                    checkTimer.Stop();

                    // ⭐ Re-enable controls
                    btnBilliards.Enabled = true;
                    btnScooter.Enabled = true;
                    btnBadminton.Enabled = true;
                    btnTableTennis.Enabled = true;
                    btn30min.Enabled = true;
                    btn1hour.Enabled = true;

                   

                    // Process result
                    if (equipmentRentalControl1.IsConfirmed)
                    {
                        AddSessionWithEquipment(
                            capturedMinutes,
                            equipmentRentalControl1.SelectedEquipment,
                            equipmentRentalControl1.TotalEquipmentCost
                        );
                    }

                    checkTimer.Dispose();
                }
            };

            checkTimer.Start();
        }

        // ⭐ NEW METHOD - Add Session With Equipment
        private void AddSessionWithEquipment(int minutes, List<Equipment> equipment, decimal equipmentCost)
        {
            sessionManager.AddOrExtendSession(
                sessionManager.SelectedGame,
                minutes,
                equipment,
                equipmentCost
            );

            RefreshDisplay();
            ResetGameSelection();

            // Show confirmation
            string equipSummary = equipmentCost > 0
                ? $"\nEquipment: {PriceFormatter.Format(equipmentCost)}"
                : "";

            MessageBox.Show(
                $"{sessionManager.SelectedGame} added!\n" +
                $"Duration: {DurationFormatter.Format(minutes)}\n" +
                $"Game: {PriceFormatter.Format(PriceManager.GetPrice(sessionManager.SelectedGame, minutes))}" +
                equipSummary,
                "Session Added",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // ⭐ EXISTING METHOD - Add Without Equipment
        private void AddSessionWithoutEquipment(int minutes)
        {
            sessionManager.AddOrExtendSession(
                sessionManager.SelectedGame,
                minutes,
                new List<Equipment>(),
                0
            );

            RefreshDisplay();
            ResetGameSelection();

            MessageBox.Show(
                $"{sessionManager.SelectedGame} added!\n" +
                $"Duration: {DurationFormatter.Format(minutes)}\n" +
                $"Price: {PriceFormatter.Format(PriceManager.GetPrice(sessionManager.SelectedGame, minutes))}",
                "Session Added",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void ResetGameSelection()
        {
            ResetGameButtonColors();
            sessionManager.SelectedGame = "";
        }

        private void RefreshDisplay()
        {
            if (rtbSelectedGames == null) return;

            rtbSelectedGames.Clear();

            if (sessionManager == null || sessionManager.ActiveSessions.Count == 0)
            {
                rtbSelectedGames.Text = "\n\n          No games selected yet.\n\n          Select a game and duration to begin.";
                if (lblTotal != null) lblTotal.Text = "₱0.00";
                return;
            }

            StringBuilder summary = new StringBuilder();

            summary.AppendLine("════════════════════════════════════════════════════════");
            summary.AppendLine("                    CURRENT ORDER");
            summary.AppendLine("════════════════════════════════════════════════════════");
            summary.AppendLine();

            decimal totalAmount = 0;

            foreach (var session in sessionManager.ActiveSessions.Values)
            {
                string durationText = DurationFormatter.Format(session.TotalMinutes);

                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                decimal displayPrice = session.TotalPrice + session.EquipmentCost;
                totalAmount += displayPrice;

                summary.AppendLine($"  Game:             {session.GameName}");
                summary.AppendLine($"  Duration:         {durationText}");
                summary.AppendLine($"  Start Time:       {session.StartTime:hh:mm tt}");
                summary.AppendLine($"  End Time:         {session.EndTime:hh:mm tt}");
                summary.AppendLine($"  Game Price:       {PriceFormatter.Format(session.TotalPrice)}");

                if (session.Equipment != null && session.Equipment.Count > 0)
                {
                    summary.AppendLine("  Equipment:");

                    foreach (var eq in session.Equipment)
                    {
                        if (eq.DefaultQuantity > 0)
                        {
                            summary.AppendLine($"    • {eq.Name} x{eq.DefaultQuantity} (Included)");
                        }
                        if (eq.RentalQuantity > 0)
                        {
                            summary.AppendLine($"    • {eq.Name} x{eq.RentalQuantity} ({eq.Type}) - {PriceFormatter.Format(eq.TotalCost)}");
                        }
                    }

                    if (session.EquipmentCost > 0)
                    {
                        summary.AppendLine($"  Equipment Cost:   {PriceFormatter.Format(session.EquipmentCost)}");
                    }
                }

                summary.AppendLine("  ────────────────────────────────────────────────────");
                summary.AppendLine($"  Subtotal:         {PriceFormatter.Format(displayPrice)}");
                summary.AppendLine();
            }

            summary.AppendLine("════════════════════════════════════════════════════════");
            summary.AppendLine($"  TOTAL AMOUNT:     {PriceFormatter.Format(totalAmount)}");
            summary.AppendLine("════════════════════════════════════════════════════════");

            rtbSelectedGames.Text = summary.ToString();
            if (lblTotal != null) lblTotal.Text = PriceFormatter.Format(totalAmount);
        }

        // ============ REMOVE GAME ============
        private void btnRemoveGame_Click(object sender, EventArgs e)
        {
            if (sessionManager == null || sessionManager.ActiveSessions.Count == 0)
            {
                MessageBox.Show("No games to remove!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var gameNames = sessionManager.ActiveSessions.Keys.ToList();

            Form selectionForm = new Form();
            selectionForm.Text = "Select Game to Remove";
            selectionForm.Size = new Size(350, 250);
            selectionForm.StartPosition = FormStartPosition.CenterParent;
            selectionForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            selectionForm.MaximizeBox = false;
            selectionForm.MinimizeBox = false;

            ListBox listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.Font = new Font("Segoe UI", 10);

            foreach (var key in gameNames)
            {
                var session = sessionManager.ActiveSessions[key];
                listBox.Items.Add($"{session.GameName} - {DurationFormatter.Format(session.TotalMinutes)}");
            }

            Button btnRemove = new Button();
            btnRemove.Text = "Remove Selected";
            btnRemove.Dock = DockStyle.Bottom;
            btnRemove.Height = 40;
            btnRemove.DialogResult = DialogResult.OK;

            selectionForm.Controls.Add(listBox);
            selectionForm.Controls.Add(btnRemove);

            if (selectionForm.ShowDialog() == DialogResult.OK && listBox.SelectedIndex >= 0)
            {
                string selectedKey = gameNames[listBox.SelectedIndex];
                var session = sessionManager.ActiveSessions[selectedKey];

                if (MessageBox.Show($"Remove {session.GameName}?", "Confirm Remove",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sessionManager.RemoveSession(selectedKey);
                    RefreshDisplay();
                }
            }
        }

        // ============ ORDER NUMBER KEYPAD ============
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && txtOrderNumber != null)
            {
                txtOrderNumber.Text += btn.Text;
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtOrderNumber != null && txtOrderNumber.Text.Length > 0)
            {
                txtOrderNumber.Text = txtOrderNumber.Text.Substring(0, txtOrderNumber.Text.Length - 1);
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtOrderNumber == null) return;

            string orderNumber = txtOrderNumber.Text.Trim();

            if (string.IsNullOrEmpty(orderNumber))
            {
                MessageBox.Show("Please enter an order number!", "No Order Number",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrderNumber.Focus();
                return;
            }

            orderNumber = new string(orderNumber.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(orderNumber))
            {
                MessageBox.Show("Invalid order number!", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrderNumber.Clear();
                return;
            }

            if (orderNumber.Length < 6)
            {
                orderNumber = orderNumber.PadLeft(6, '0');
            }

            txtOrderNumber.Text = orderNumber;
            LoadOrderFromDatabase(orderNumber);
        }

        private void LoadOrderFromDatabase(string orderNumber)
        {
            try
            {
                var items = OrderRepository.LoadOrder(orderNumber);

                if (items == null || items.Count == 0)
                {
                    MessageBox.Show(
                        $"Order #{orderNumber} not found or already processed!",
                        "Invalid Order",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    if (txtOrderNumber != null)
                    {
                        txtOrderNumber.Clear();
                        txtOrderNumber.Focus();
                    }
                    return;
                }

                if (sessionManager != null)
                {
                    sessionManager.ClearAll();
                }
                if (rtbSelectedGames != null)
                {
                    rtbSelectedGames.Clear();
                }

                StringBuilder summary = new StringBuilder();
                summary.AppendLine("════════════════════════════════════════════════════════");
                summary.AppendLine($"              ORDER #{orderNumber}");
                summary.AppendLine("════════════════════════════════════════════════════════");
                summary.AppendLine();

                decimal orderTotal = 0;

                foreach (var item in items)
                {
                    GameSession session = new GameSession
                    {
                        GameName = item.GameName,
                        TotalMinutes = item.Duration,
                        TotalPrice = item.Price,
                        EquipmentCost = item.EquipmentCost,
                        Equipment = new List<Equipment>()
                    };

                    if (sessionManager != null)
                    {
                        sessionManager.ActiveSessions[item.GameName + Guid.NewGuid()] = session;
                    }

                    string durationText = DurationFormatter.Format(item.Duration);
                    decimal itemTotal = item.TotalPrice;
                    orderTotal += itemTotal;

                    summary.AppendLine($"  Game:             {item.GameName}");
                    summary.AppendLine($"  Duration:         {durationText}");
                    summary.AppendLine($"  Game Price:       {PriceFormatter.Format(item.Price)}");

                    if (item.EquipmentCost > 0)
                    {
                        summary.AppendLine($"  Equipment Cost:   {PriceFormatter.Format(item.EquipmentCost)}");
                    }

                    summary.AppendLine("  ────────────────────────────────────────────────────");
                    summary.AppendLine($"  Subtotal:         {PriceFormatter.Format(itemTotal)}");
                    summary.AppendLine();
                }

                summary.AppendLine("════════════════════════════════════════════════════════");
                summary.AppendLine($"  TOTAL AMOUNT:     {PriceFormatter.Format(orderTotal)}");
                summary.AppendLine("════════════════════════════════════════════════════════");

                if (rtbSelectedGames != null) rtbSelectedGames.Text = summary.ToString();
                if (lblTotal != null) lblTotal.Text = PriceFormatter.Format(orderTotal);

                MessageBox.Show(
                    $"Order #{orderNumber} loaded successfully!\n\n" +
                    $"Items: {items.Count}\n" +
                    $"Total: {PriceFormatter.Format(orderTotal)}",
                    "Order Loaded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                if (txtOrderNumber != null) txtOrderNumber.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading order:\n{ex.Message}\n\nPlease check database connection.",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ============ PROCEED TO PAYMENT ============
        private void btnProceedPayment_Click(object sender, EventArgs e)
        {
            string orderNumberToPass = txtOrderNumber != null ? txtOrderNumber.Text.Trim() : "";

            if (sessionManager == null || sessionManager.ActiveSessions.Count == 0)
            {
                if (rtbSelectedGames == null || string.IsNullOrWhiteSpace(rtbSelectedGames.Text) ||
                    rtbSelectedGames.Text.Contains("No games selected"))
                {
                    MessageBox.Show("Please add games to order!", "No Items",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            Dictionary<string, GameSession> sessions = new Dictionary<string, GameSession>();
            decimal total = 0;

            if (sessionManager != null && sessionManager.ActiveSessions.Count > 0)
            {
                sessions = sessionManager.ActiveSessions;

                foreach (var session in sessions.Values)
                {
                    total += session.TotalPrice + session.EquipmentCost;
                }
            }
            else
            {
                if (lblTotal != null && decimal.TryParse(lblTotal.Text.Replace("₱", "").Replace(",", "").Trim(), out decimal parsedTotal))
                {
                    total = parsedTotal;
                }
            }

            if (total <= 0)
            {
                MessageBox.Show("Invalid order total!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (paymentControl != null)
            {
                paymentControl.Visible = true;
                paymentControl.BringToFront();
                paymentControl.LoadPaymentData(sessions, total, orderNumberToPass);
            }
        }

        // ============ CLEAR & RESET ============
        private void btnClearCashierForm_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all items?", "Confirm Clear",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetTransaction();
            }
        }

        public void ResetTransaction()
        {
            if (txtOrderNumber != null) txtOrderNumber.Clear();
            if (rtbSelectedGames != null) rtbSelectedGames.Clear();
            if (lblTotal != null) lblTotal.Text = "₱0.00";

            ResetGameButtonColors();
            RefreshDisplay();
            if (txtOrderNumber != null) txtOrderNumber.Focus();
        }

        private void OnPaymentSuccessful()
        {
            ResetTransaction();
            if (paymentControl != null) paymentControl.Visible = false;

            MessageBox.Show("Payment completed!\nForm reset for next customer.",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============ DATE/TIME ============
        private void UpdateDateTime()
        {
            if (lblDate != null) lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            if (lblTime != null) lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        // ============ SIGN OUT ============
        private void btnSignOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to sign out?",
                "Sign Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        // ============ FORM CLOSING ============
        private void CashierForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop and dispose timer
            if (dateTimeTimer != null)
            {
                dateTimeTimer.Stop();
                dateTimeTimer.Dispose();
                dateTimeTimer = null;
            }

            if (sessionManager != null && sessionManager.ActiveSessions.Count > 0)
            {
                DialogResult result = MessageBox.Show("You have pending orders. Are you sure you want to exit?",
                    "Confirm Exit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void CashierForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Final cleanup
            if (dateTimeTimer != null)
            {
                dateTimeTimer.Dispose();
                dateTimeTimer = null;
            }
        }
    }
}