using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class paymentControl1 : UserControl
    {
        // ============ MANAGERS ============
        private Dictionary<string, GameSession> _sessions;
        private DiscountManager discountManager;
        private PaymentCalculator calculator;
        private bool isPaymentMethodValid = false;
        private string currentOrderNumber = "";

        private string selectedPaymentMethod = "Cash";


        // ============ EVENT ============
        public event Action PaymentSuccessful;

        // ============ CONSTRUCTOR ============
        public paymentControl1()
        {
            InitializeComponent();
            discountManager = new DiscountManager();
            calculator = new PaymentCalculator();
            InitializeControls();
            SetupPaymentButtonStyles();
        }

        private void PaymentControl1_Load(object sender, EventArgs e)
        {
            calculator.Calculate(_sessions, discountManager.DiscountAmount);
            UpdateDisplays();
        }

        // ============ INITIALIZATION ============
        private void InitializeControls()
        {
            InitializeRichTextBox();
            InitializeDiscountComboBox();
            InitializePaymentMethod();
        }

        private void InitializeRichTextBox()
        {
            rtbSummary.ReadOnly = true;
            rtbSummary.Font = new Font("Courier New", 9);
            rtbSummary.BackColor = Color.White;
            rtbSummary.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitializeDiscountComboBox()
        {
            cboDiscountType.Items.Clear();
            cboDiscountType.Items.Add("None");
            cboDiscountType.Items.Add("Senior Citizen (20%)");
            cboDiscountType.Items.Add("PWD (20%)");
            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Enabled = false;
        }

        private void InitializePaymentMethod()
        {
            // Set default to Cash
            selectedPaymentMethod = "Cash";

            // Set button styles
            btnCash.BackColor = Color.FromArgb(76, 175, 80); // Green (selected)
            btnCash.ForeColor = Color.White;
            btnGcash.BackColor = Color.FromArgb(189, 189, 189); // Gray (unselected)
            btnGcash.ForeColor = Color.White;

            // Show cash controls by default
            txtGcashRef.Visible = false;
            txtGcashRef.Enabled = false;

            txtCashReceived.Visible = true;
            txtCashReceived.Enabled = true;
            lblChange.Visible = true;

            btnConfirmPayment.Enabled = false;
        }

        // ============ LOAD PAYMENT DATA ============
        public void LoadPaymentData(Dictionary<string, GameSession> sessions, decimal total, string orderNumber = "")
        {
            _sessions = sessions;
            currentOrderNumber = orderNumber;
            discountManager.ClearDiscount();

            // ⭐ Calculate subtotal from sessions (includes equipment)
            decimal calculatedTotal = 0;
            if (sessions != null && sessions.Count > 0)
            {
                foreach (var session in sessions.Values)
                {
                    calculatedTotal += session.TotalPrice + session.EquipmentCost;
                }
            }
            else
            {
                calculatedTotal = total;
            }

            discountManager.SetSubtotal(calculatedTotal);

            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Clear();
            txtDiscountAmount.Enabled = false;

            BuildTransactionSummary();
            calculator.Calculate(_sessions, 0);
            UpdateDisplays();
            InitializePaymentMethod();

            System.Diagnostics.Debug.WriteLine($"=== Payment Data Loaded ===");
            System.Diagnostics.Debug.WriteLine($"Sessions: {sessions?.Count ?? 0}");
            System.Diagnostics.Debug.WriteLine($"Calculated Total: {calculatedTotal:C}");
            System.Diagnostics.Debug.WriteLine($"Passed Total: {total:C}");
        }

        private void BuildTransactionSummary()
        {
            rtbSummary.Clear();

            // ⭐ Set formatting
            rtbSummary.SelectionFont = new Font("Courier New", 9);
            rtbSummary.SelectionColor = Color.Black;
            rtbSummary.SelectionAlignment = HorizontalAlignment.Left;

            StringBuilder summary = new StringBuilder();

            summary.AppendLine("        ════════════════════════════════════════════════");
            summary.AppendLine("            TRANSACTION DETAILS");
            summary.AppendLine("        ════════════════════════════════════════════════");
            summary.AppendLine();

            int itemNumber = 1;

            foreach (var session in _sessions.Values)
            {
                string duration = DurationFormatter.Format(session.TotalMinutes);

                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                decimal hourlyRate = session.TotalMinutes > 0
                    ? session.TotalPrice / (session.TotalMinutes / 60.0m)
                    : session.TotalPrice;

                // ⭐ Item header
                summary.AppendLine($"       ┌─ ITEM #{itemNumber} ──────────────────────────────────");
                summary.AppendLine($"       │");
                summary.AppendLine($"       │  Game Type:        {session.GameName}");
                summary.AppendLine($"       │  Start Time:       {session.StartTime:hh:mm tt}");
                summary.AppendLine($"       │  End Time:         {session.EndTime:hh:mm tt}");
                summary.AppendLine($"       │  Duration:         {duration}");
                summary.AppendLine($"       │  Rate:             {PriceFormatter.Format(hourlyRate)}/hour");
                summary.AppendLine($"       │");
                summary.AppendLine($"       │  Game Price:       {PriceFormatter.Format(session.TotalPrice)}");

                // ⭐ Equipment details
                if (session.Equipment != null && session.Equipment.Count > 0)
                {
                    bool hasRentalEquipment = session.Equipment.Any(e => e.RentalQuantity > 0 || e.DefaultQuantity > 0);

                    if (hasRentalEquipment)
                    {
                        summary.AppendLine($"       │");
                        summary.AppendLine($"       │  Equipment:");

                        foreach (var eq in session.Equipment)
                        {
                            if (eq.DefaultQuantity > 0)
                            {
                                summary.AppendLine($"       │    ✓ {eq.Name} x{eq.DefaultQuantity} (Included)");
                            }
                            if (eq.RentalQuantity > 0)
                            {
                                summary.AppendLine($"       │    • {eq.Name} x{eq.RentalQuantity} ({eq.Type}) - {PriceFormatter.Format(eq.TotalCost)}");
                            }
                        }

                        if (session.EquipmentCost > 0)
                        {
                            summary.AppendLine($"       │");
                            summary.AppendLine($"       │  Equipment Cost:   {PriceFormatter.Format(session.EquipmentCost)}");
                        }
                    }
                }

                summary.AppendLine($"       │");
                summary.AppendLine($"       │  ───────────────────────────────────────────────");
                summary.AppendLine($"       │  Subtotal:         {PriceFormatter.Format(session.TotalPrice + session.EquipmentCost)}");
                summary.AppendLine($"       └─────────────────────────────────────────────────");
                summary.AppendLine();

                itemNumber++;
            }

            rtbSummary.Text = summary.ToString();
        }

        // ============ DISCOUNT HANDLING ============
        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            if (cboDiscountType.SelectedItem == null)
            {
                MessageBox.Show("Please select a discount type.", "Apply Discount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selected = cboDiscountType.SelectedItem.ToString();

            switch (selected)
            {
                case "None":
                    MessageBox.Show("No discount selected.", "Apply Discount",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Senior Citizen (20%)":
                case "PWD (20%)":
                    if (ValidateDiscountEligibility(selected))
                    {
                        discountManager.ApplyPercentageDiscount(0.20m, selected);
                        MessageBox.Show($"20% discount applied!\nDiscount: {PriceFormatter.Format(discountManager.DiscountAmount)}",
                            "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }

            calculator.Calculate(_sessions, discountManager.DiscountAmount);
            UpdateDisplays();
            System.Diagnostics.Debug.WriteLine($"Discount applied: {discountManager.DiscountAmount}, New Total: {calculator.GetFinalAmount()}");
        }

        private bool ValidateDiscountEligibility(string discountType)
        {
            if (discountType.Contains("Senior") || discountType.Contains("PWD"))
            {
                DialogResult result = MessageBox.Show(
                    "Has the customer presented a valid ID?",
                    "Discount Verification",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    cboDiscountType.SelectedIndex = 0;
                    return false;
                }
            }

            return true;
        }

        // ============ PAYMENT METHOD BUTTONS ============
        private void btnCash_Click(object sender, EventArgs e)
        {
            // Set payment method
            selectedPaymentMethod = "Cash";

            // Update button styles - highlight selected
            btnCash.BackColor = Color.FromArgb(76, 175, 80); // Green (selected)
            btnCash.ForeColor = Color.White;
            btnGcash.BackColor = Color.FromArgb(189, 189, 189); // Gray (unselected)
            btnGcash.ForeColor = Color.White;

            // Show cash controls
            txtCashReceived.Visible = true;
            txtCashReceived.Enabled = true;
            txtCashReceived.Clear();
            txtCashReceived.Focus();
            lblChange.Visible = true;
            lblChange.Text = "₱0.00";

            // Hide GCash controls
            txtGcashRef.Visible = false;
            txtGcashRef.Enabled = false;
            txtGcashRef.Clear();
            txtGcashRef.BackColor = Color.White;

            // Reset validation
            isPaymentMethodValid = false;
            btnConfirmPayment.Enabled = false;

            System.Diagnostics.Debug.WriteLine("Payment method: CASH selected");
        }

        private void btnGcash_Click(object sender, EventArgs e)
        {
            // Set payment method
            selectedPaymentMethod = "GCash";

            // Update button styles - highlight selected
            btnGcash.BackColor = Color.FromArgb(33, 150, 243); // Blue (selected)
            btnGcash.ForeColor = Color.White;
            btnCash.BackColor = Color.FromArgb(189, 189, 189); // Gray (unselected)
            btnCash.ForeColor = Color.White;

            // Show GCash controls
            txtGcashRef.Visible = true;
            txtGcashRef.Enabled = true;
            txtGcashRef.Clear();
            txtGcashRef.Focus();

            // Hide cash controls
            txtCashReceived.Visible = false;
            txtCashReceived.Enabled = false;
            txtCashReceived.Clear();
            lblChange.Visible = false;
            lblChange.Text = "₱0.00";

            // Reset validation
            isPaymentMethodValid = false;
            btnConfirmPayment.Enabled = false;

            System.Diagnostics.Debug.WriteLine("Payment method: GCASH selected");
        }

        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            ValidateCashPayment();
        }

        private void txtGcashRef_TextChanged(object sender, EventArgs e)
        {
            ValidateGCashReference();
        }

        private void ValidateCashPayment()
        {
            if (selectedPaymentMethod != "Cash") return;  // ⭐ Changed from rbCash.Checked

            var result = PaymentValidator.ValidateCashPayment(
                txtCashReceived.Text,
                calculator.GetFinalAmount()
            );

            lblChange.Text = result.DisplayText;

            if (result.IsValid)
            {
                lblChange.ForeColor = Color.Green;
                btnConfirmPayment.Enabled = true;
                isPaymentMethodValid = true;
            }
            else
            {
                lblChange.ForeColor = Color.Red;
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;
            }
        }

        private void ValidateGCashReference()
        {
            if (selectedPaymentMethod != "GCash") return;  // ⭐ Changed from rbGCash.Checked

            var result = PaymentValidator.ValidateGCashReference(txtGcashRef.Text);

            if (result.IsValid)
            {
                txtGcashRef.BackColor = Color.LightGreen;
                btnConfirmPayment.Enabled = true;
                isPaymentMethodValid = true;
            }
            else
            {
                txtGcashRef.BackColor = result.Message == "Please enter GCash reference"
                    ? Color.White
                    : Color.LightCoral;
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;
            }
        }

        // ============ PAYMENT PROCESSING ============
        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            if (!isPaymentMethodValid)
            {
                MessageBox.Show("Please enter valid payment information!", "Invalid Payment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paymentMethod = selectedPaymentMethod;  // ⭐ Use the variable
            decimal cashReceived = 0;
            decimal change = 0;
            string reference = "";

            // Validate and get payment details
            if (paymentMethod == "Cash")
            {
                var validation = PaymentValidator.ValidateCashPayment(
                    txtCashReceived.Text,
                    calculator.GetFinalAmount()
                );

                if (!validation.IsValid)
                {
                    MessageBox.Show("Invalid cash payment!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cashReceived = validation.CashReceived;
                change = validation.Change;
                reference = cashReceived.ToString("0.00");
            }
            else if (paymentMethod == "GCash")
            {
                var validation = PaymentValidator.ValidateGCashReference(txtGcashRef.Text);

                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.Message, "Invalid GCash Reference",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                reference = validation.GCashReference;

                // Check duplicate
                if (PaymentRepository.IsDuplicateGCashReference(reference))
                {
                    if (MessageBox.Show("This GCash reference was used before. Continue?",
                        "Duplicate Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            // Rest of existing confirmation and processing code...
            ProcessPayment(paymentMethod, cashReceived, change, reference);
        }

        private void ProcessPayment(string method, decimal cashReceived, decimal change, string reference)
        {
            try
            {
                string receiptNo = "MPGH-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                System.Diagnostics.Debug.WriteLine("PROCESSING PAYMENT");
                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");

                // Save sessions and payments to database
                foreach (var session in _sessions.Values)
                {
                    System.Diagnostics.Debug.WriteLine($"\nProcessing session: {session.GameName}");

                    // Save session (returns session_id)
                    int sessionId = PaymentRepository.SaveSession(session);

                    if (sessionId <= 0)
                    {
                        throw new Exception("Failed to save session - invalid session ID");
                    }

                    // Save payment for this session
                    PaymentRepository.SavePayment(new PaymentData
                    {
                        SessionId = sessionId,
                        PaymentMethod = method,
                        AmountPaid = calculator.Subtotal,
                        DiscountType = discountManager.DiscountType,
                        DiscountAmount = discountManager.DiscountAmount,
                        FinalAmount = calculator.GetFinalAmount(),
                        ReceiptNo = receiptNo,
                        Reference = reference,
                        PaymentDate = DateTime.Now
                    });
                }

                // ⭐ Update order status to 'Completed' if this payment is from an order
                if (!string.IsNullOrEmpty(currentOrderNumber))
                {
                    OrderRepository.UpdateOrderStatus(currentOrderNumber, "Completed");

                    System.Diagnostics.Debug.WriteLine("");
                    System.Diagnostics.Debug.WriteLine($"✓ Order {currentOrderNumber} marked as COMPLETED");
                    System.Diagnostics.Debug.WriteLine("");
                }

                // Generate receipt
                string receiptPath = ReceiptGenerator.GenerateReceipt(new ReceiptData
                {
                    Sessions = _sessions,
                    Subtotal = calculator.Subtotal,
                    DiscountAmount = discountManager.DiscountAmount,
                    DiscountType = discountManager.DiscountType,
                    FinalAmount = calculator.GetFinalAmount(),
                    PaymentMethod = method,
                    CashReceived = cashReceived,
                    Change = change,
                    GCashReference = reference
                });

                System.Diagnostics.Debug.WriteLine($"✓ Receipt generated: {receiptPath}");
                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");

                MessageBox.Show(
                    "Payment successful!\n" +
                    "Receipt has been generated.\n\n" +
                    (!string.IsNullOrEmpty(currentOrderNumber) ? $"Order #{currentOrderNumber} completed!" : ""),
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                System.Diagnostics.Process.Start(receiptPath);

                ClearPaymentData();
                this.Visible = false;
                PaymentSuccessful?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error processing payment:\n{ex.Message}\n\nPlease try again.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                System.Diagnostics.Debug.WriteLine($"❌ Payment Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
            }
        }

        // ============ UPDATE DISPLAYS ============
        private void UpdateDisplays()
        {
            if (lblSubtotal != null)
                lblSubtotal.Text = PriceFormatter.Format(calculator.Subtotal);

            if (lblDiscountAmount != null)
            {
                lblDiscountAmount.Text = "-" + PriceFormatter.Format(discountManager.DiscountAmount);
                lblDiscountAmount.ForeColor = Color.Red;
            }

            if (lblTotalAmount != null)
                lblTotalAmount.Text = PriceFormatter.Format(calculator.GetFinalAmount());

            // ⭐ Changed from rbCash != null && rbCash.Checked
            if (selectedPaymentMethod == "Cash")
            {
                ValidateCashPayment();
            }
        }

        private void SetupPaymentButtonStyles()
        {
            // Cash button hover effects
            btnCash.MouseEnter += (s, e) =>
            {
                if (selectedPaymentMethod == "Cash")
                    btnCash.BackColor = Color.FromArgb(56, 142, 60); // Darker green
                else
                    btnCash.BackColor = Color.FromArgb(158, 158, 158); // Lighter gray
            };

            btnCash.MouseLeave += (s, e) =>
            {
                if (selectedPaymentMethod == "Cash")
                    btnCash.BackColor = Color.FromArgb(76, 175, 80); // Green
                else
                    btnCash.BackColor = Color.FromArgb(189, 189, 189); // Gray
            };

            // GCash button hover effects
            btnGcash.MouseEnter += (s, e) =>
            {
                if (selectedPaymentMethod == "GCash")
                    btnGcash.BackColor = Color.FromArgb(25, 118, 210); // Darker blue
                else
                    btnGcash.BackColor = Color.FromArgb(158, 158, 158); // Lighter gray
            };

            btnGcash.MouseLeave += (s, e) =>
            {
                if (selectedPaymentMethod == "GCash")
                    btnGcash.BackColor = Color.FromArgb(33, 150, 243); // Blue
                else
                    btnGcash.BackColor = Color.FromArgb(189, 189, 189); // Gray
            };
        }

        // ============ CLEAR DATA ============
        private void ClearPaymentData()
        {
            _sessions?.Clear();
            discountManager.ClearDiscount();
            calculator.Clear();

            rtbSummary.Clear();
            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Clear();
            txtCashReceived.Clear();
            txtGcashRef.Clear();

            lblSubtotal.Text = "₱0.00";
            lblDiscountAmount.Text = "-₱0.00";
            lblTotalAmount.Text = "₱0.00";
            lblChange.Text = "₱0.00";

            // ⭐ Reset to Cash (default)
            selectedPaymentMethod = "Cash";
            btnCash.BackColor = Color.FromArgb(76, 175, 80); // Green
            btnGcash.BackColor = Color.FromArgb(189, 189, 189); // Gray

            txtCashReceived.Visible = true;
            txtGcashRef.Visible = false;
            lblChange.Visible = true;

            isPaymentMethodValid = false;
            btnConfirmPayment.Enabled = false;
            txtGcashRef.BackColor = Color.White;
            currentOrderNumber = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void btnPreviewReceipt_Click(object sender, EventArgs e)
        {
            if (_sessions == null || _sessions.Count == 0)
            {
                MessageBox.Show("No transactions to preview!", "Preview Receipt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create preview form
            Form previewForm = new Form();
            previewForm.Text = "Receipt Preview";
            previewForm.Size = new Size(550, 750);
            previewForm.StartPosition = FormStartPosition.CenterParent;
            previewForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            previewForm.MaximizeBox = false;
            previewForm.MinimizeBox = false;
            previewForm.BackColor = Color.White;

            // Create RichTextBox for preview
            RichTextBox rtbPreview = new RichTextBox();
            rtbPreview.Dock = DockStyle.Fill;
            rtbPreview.Font = new Font("Courier New", 9);
            rtbPreview.ReadOnly = true;
            rtbPreview.BackColor = Color.White;
            rtbPreview.BorderStyle = BorderStyle.None;
            rtbPreview.Padding = new Padding(10);

            // Build receipt content
            StringBuilder receipt = new StringBuilder();

            receipt.AppendLine("          ═══════════════════════════════════════");
            receipt.AppendLine("                 MATCH POINT GAMING HUB");
            receipt.AppendLine("               123 Gaming Street, Quezon City");
            receipt.AppendLine("                Metro Manila, Philippines 1100");
            receipt.AppendLine("                   Tel: (02) 8123-4567");
            receipt.AppendLine("                   TIN: 123-456-789-000");
            receipt.AppendLine("          ═══════════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine("                    RECEIPT PREVIEW");
            receipt.AppendLine("                   (VAT Registered)");
            receipt.AppendLine();
            receipt.AppendLine($"        Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}");
            receipt.AppendLine($"        Cashier: ");  // {UserSession.Username ?? Environment.UserName}
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();
            receipt.AppendLine("        ITEMS:");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();

            // ⭐ Add items with equipment details
            int itemNumber = 1;
            foreach (var session in _sessions.Values)
            {
                string duration = DurationFormatter.Format(session.TotalMinutes);

                receipt.AppendLine($"        {itemNumber}. {session.GameName}");
                receipt.AppendLine($"           Duration: {duration}");
                receipt.AppendLine($"           Game Price: {PriceFormatter.Format(session.TotalPrice)}");

                // ⭐ Add equipment details
                if (session.Equipment != null && session.Equipment.Count > 0)
                {
                    foreach (var eq in session.Equipment)
                    {
                        if (eq.DefaultQuantity > 0)
                        {
                            receipt.AppendLine($"             ✓ {eq.Name} x{eq.DefaultQuantity} (Included)");
                        }
                        if (eq.RentalQuantity > 0)
                        {
                            receipt.AppendLine($"             • {eq.Name} x{eq.RentalQuantity}");
                            receipt.AppendLine($"               ({eq.Type}) - {PriceFormatter.Format(eq.TotalCost)}");
                        }
                    }

                    if (session.EquipmentCost > 0)
                    {
                        receipt.AppendLine($"           Equipment Cost: {PriceFormatter.Format(session.EquipmentCost)}");
                    }
                }

                receipt.AppendLine($"           ───────────────────────────────────");
                receipt.AppendLine($"           Item Total: {PriceFormatter.Format(session.TotalPrice + session.EquipmentCost)}");
                receipt.AppendLine();

                itemNumber++;
            }

            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();

            // ⭐ Totals with VAT breakdown
            decimal subtotalBeforeTax = calculator.Subtotal / 1.12m;
            decimal vatAmount = calculator.Subtotal - subtotalBeforeTax;

            receipt.AppendLine($"        Subtotal:              {PriceFormatter.Format(calculator.Subtotal)}");

            if (discountManager.DiscountAmount > 0)
            {
                receipt.AppendLine($"        Discount ({discountManager.DiscountType}):");
                receipt.AppendLine($"                              -{PriceFormatter.Format(discountManager.DiscountAmount)}");
                receipt.AppendLine($"        Subtotal after disc:   {PriceFormatter.Format(calculator.Subtotal - discountManager.DiscountAmount)}");
            }

            receipt.AppendLine();
            receipt.AppendLine("        VAT Breakdown:");
            receipt.AppendLine($"          VATable Sale:        {PriceFormatter.Format(calculator.TaxableAmount)}");
            receipt.AppendLine($"          VAT (12%):           {PriceFormatter.Format(calculator.TaxAmount)}");
            receipt.AppendLine();

            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine($"        TOTAL AMOUNT DUE:      {PriceFormatter.Format(calculator.GetFinalAmount())}");
            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine();

            // ⭐ Payment method (updated to use selectedPaymentMethod)
            string paymentMethod = selectedPaymentMethod;  // ⭐ FIXED - use variable instead of rbCash
            receipt.AppendLine("        PAYMENT METHOD");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine($"        Payment Type: {paymentMethod}");

            if (paymentMethod == "Cash" && !string.IsNullOrWhiteSpace(txtCashReceived.Text))
            {
                if (decimal.TryParse(txtCashReceived.Text, out decimal cash))
                {
                    decimal change = cash - calculator.GetFinalAmount();
                    receipt.AppendLine($"        Amount Tendered:       {PriceFormatter.Format(cash)}");
                    receipt.AppendLine($"        Change:                {PriceFormatter.Format(change)}");
                }
            }
            else if (paymentMethod == "GCash" && !string.IsNullOrWhiteSpace(txtGcashRef.Text))
            {
                receipt.AppendLine($"        Reference No: {txtGcashRef.Text}");
            }
            else
            {
                receipt.AppendLine("        (Payment details not yet entered)");
            }

            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();

            // ⭐ Session times
            if (_sessions.Count > 0)
            {
                receipt.AppendLine("        SESSION TIMES:");
                receipt.AppendLine("        ───────────────────────────────────────");

                foreach (var session in _sessions.Values)
                {
                    receipt.AppendLine($"        {session.GameName}:");
                    receipt.AppendLine($"          Start: {session.StartTime:hh:mm tt}");
                    receipt.AppendLine($"          End:   {session.EndTime:hh:mm tt}");
                }

                receipt.AppendLine("        ───────────────────────────────────────");
                receipt.AppendLine();
            }

            // Footer
            receipt.AppendLine("              Thank you for choosing");
            receipt.AppendLine("             MATCH POINT GAMING HUB!");
            receipt.AppendLine("               Please visit us again!");
            receipt.AppendLine();
            receipt.AppendLine("        This serves as your official receipt.");
            receipt.AppendLine("          VAT included in total amount.");
            receipt.AppendLine();
            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine("        This is a PREVIEW only. No payment");
            receipt.AppendLine("        has been processed yet.");
            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine($"        Preview generated: {DateTime.Now:MMM dd, yyyy hh:mm tt}");

            rtbPreview.Text = receipt.ToString();

            // Close button
            Button btnClose = new Button();
            btnClose.Text = "Close Preview";
            btnClose.Dock = DockStyle.Bottom;
            btnClose.Height = 50;
            btnClose.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnClose.BackColor = Color.FromArgb(158, 158, 158);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;

            // Hover effects
            btnClose.MouseEnter += (s, ev) => btnClose.BackColor = Color.FromArgb(97, 97, 97);
            btnClose.MouseLeave += (s, ev) => btnClose.BackColor = Color.FromArgb(158, 158, 158);

            btnClose.Click += (s, ev) => previewForm.Close();

            // Add controls to form
            previewForm.Controls.Add(rtbPreview);
            previewForm.Controls.Add(btnClose);

            // Show preview
            previewForm.ShowDialog();
        }
    }
}