using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Services;

namespace WinFormsApp.UI.components
{
    public partial class ReportDetailModal : Form
    {
        private readonly int _objectId;
        private readonly ReportService _reportService;
        private readonly ObjectService _objectService; // Define ObjectService
        private int _currentPage = 1;
        private int _totalPages = 1;
        private const int PageSize = 10;
        private bool _isLoading = false;
        private TableLayoutPanel detailsLayout; // Store reference to detailsLayout

        // Controls for Top Section
        private Label lblObjectDetailsTitle;
        private PictureBox pbObjectImage;
        private Label lblObjectName;
        private Label lblObjectDescription;
        private Label lblCategory;
        private Label lblUserId;
        private Label lblStatus;
        private Button btnDeleteObject;
        private Label lblObjectDeleted;

        // Controls for Bottom Section
        private Label lblAssociatedReportsTitle;
        private DateTimePicker dtpCreatedAtStart;
        private DateTimePicker dtpCreatedAtEnd;
        private TextBox txtMotif;
        private Button btnFilter;
        private DataGridView dgvAssociatedReports;
        private FlowLayoutPanel pnlPagination;
        private Label loadingLabel;

        // Containers
        private TableLayoutPanel mainLayout;
        private TableLayoutPanel topLayout;
        private TableLayoutPanel objectDetailsLayout;
        private TableLayoutPanel bottomLayout;
        private FlowLayoutPanel filterPanel;

        public ReportDetailModal(int objectId)
        {
            _objectId = objectId;
            _reportService = new ReportService();
            _objectService = new ObjectService(); // Initialize ObjectService
            InitializeComponent();
            LoadReportDetails(); // Load initial data
        }

        private void InitializeComponent()
        {
            // Initialize Containers
            this.mainLayout = new TableLayoutPanel();
            this.topLayout = new TableLayoutPanel();
            this.objectDetailsLayout = new TableLayoutPanel();
            this.bottomLayout = new TableLayoutPanel();
            this.filterPanel = new FlowLayoutPanel();
            this.pnlPagination = new FlowLayoutPanel();
            this.loadingLabel = new Label();

            // Initialize Controls for Top Section
            this.lblObjectDetailsTitle = new Label();
            this.pbObjectImage = new PictureBox();
            this.lblObjectName = new Label();
            this.lblObjectDescription = new Label();
            this.lblCategory = new Label();
            this.lblUserId = new Label();
            this.lblStatus = new Label();
            this.btnDeleteObject = new Button();
            this.lblObjectDeleted = new Label();

            // Initialize Controls for Bottom Section
            this.lblAssociatedReportsTitle = new Label();
            this.dtpCreatedAtStart = new DateTimePicker();
            this.dtpCreatedAtEnd = new DateTimePicker();
            this.txtMotif = new TextBox();
            this.btnFilter = new Button();
            this.dgvAssociatedReports = new DataGridView();

            // Configure Main Layout
            this.mainLayout.Dock = DockStyle.Fill;
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F)); // Top Section
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F)); // Bottom Section

            // Configure Top Layout
            this.topLayout.Dock = DockStyle.Fill;
            this.topLayout.ColumnCount = 1;
            this.topLayout.RowCount = 2;
            this.topLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Title
            this.topLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Content

            // Configure Object Details Layout
            this.objectDetailsLayout.Dock = DockStyle.Fill;
            this.objectDetailsLayout.ColumnCount = 2;
            this.objectDetailsLayout.RowCount = 1;
            this.objectDetailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Picture
            this.objectDetailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // Details

            // Configure Bottom Layout
            this.bottomLayout.Dock = DockStyle.Fill;
            this.bottomLayout.ColumnCount = 1;
            this.bottomLayout.RowCount = 4;
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Title
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Filter
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // DataGridView
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Pagination

            // Configure Filter Panel
            this.filterPanel.Dock = DockStyle.Top;
            this.filterPanel.Height = 40;
            this.filterPanel.FlowDirection = FlowDirection.LeftToRight;
            this.filterPanel.AutoSize = true;

            // Configure Labels and Controls for Top Section
            // lblObjectDetailsTitle
            this.lblObjectDetailsTitle.Text = "Détails de l'objet";
            this.lblObjectDetailsTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblObjectDetailsTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblObjectDetailsTitle.Dock = DockStyle.Fill;
            this.lblObjectDetailsTitle.ForeColor = ColorTranslator.FromHtml("#bc8246"); // Set to pagination color

            // pbObjectImage
            this.pbObjectImage.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbObjectImage.Dock = DockStyle.Fill;
            this.pbObjectImage.BorderStyle = BorderStyle.FixedSingle;

            // Configure Labels for Object Details
            this.lblObjectName = new Label { Text = "Nom de l'objet:", Font = new Font("Segoe UI", 12, FontStyle.Regular), AutoSize = true };
            this.lblObjectDescription = new Label { Text = "Description:", Font = new Font("Segoe UI", 12, FontStyle.Regular), AutoSize = true };
            this.lblCategory = new Label { Font = new Font("Segoe UI", 12, FontStyle.Regular), AutoSize = true };
            this.lblUserId = new Label { Font = new Font("Segoe UI", 12, FontStyle.Regular), AutoSize = true };
            this.lblStatus = new Label { Font = new Font("Segoe UI", 12, FontStyle.Regular), AutoSize = true };

            // Configure btnDeleteObject
            this.btnDeleteObject.Text = "Effacer objet";
            this.btnDeleteObject.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            this.btnDeleteObject.Size = new Size(140, 40);
            this.btnDeleteObject.BackColor = ColorTranslator.FromHtml("#f8d7da"); // Light red background
            this.btnDeleteObject.ForeColor = ColorTranslator.FromHtml("#721c24"); // Darker red font color
            this.btnDeleteObject.Click += new EventHandler(this.btnDeleteObject_Click);

            // Configure lblObjectDeleted
            this.lblObjectDeleted.Text = "Objet supprimé";
            this.lblObjectDeleted.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            this.lblObjectDeleted.ForeColor = Color.Red;
            this.lblObjectDeleted.AutoSize = true;

            // Arrange Object Details Layout
            var detailsPanel = new Panel();
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Padding = new Padding(10);
            detailsLayout = new TableLayoutPanel(); // Assign to class-level variable
            detailsLayout.Dock = DockStyle.Fill;
            detailsLayout.ColumnCount = 2;
            detailsLayout.RowCount = 7; // Adjusted to accommodate labels and status
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Add labels and values
            detailsLayout.Controls.Add(new Label { Text = "Nom:", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true }, 0, 0);
            detailsLayout.Controls.Add(lblObjectName, 1, 0); // Display the object name

            detailsLayout.Controls.Add(new Label { Text = "Description:", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true }, 0, 1);
            detailsLayout.Controls.Add(lblObjectDescription, 1, 1); // Display the object description

            detailsLayout.Controls.Add(new Label { Text = "Catégorie:", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true }, 0, 2);
            detailsLayout.Controls.Add(lblCategory, 1, 2); // Display the object category

            detailsLayout.Controls.Add(new Label { Text = "Posté par:", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true }, 0, 3);
            detailsLayout.Controls.Add(lblUserId, 1, 3); // Display the user ID

            // Add Status Field
            detailsLayout.Controls.Add(new Label { Text = "Statut:", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true }, 0, 4);
            detailsLayout.Controls.Add(lblStatus, 1, 4); // Display the object status

            detailsLayout.Controls.Add(btnDeleteObject, 1, 5); // Add the delete button
            detailsLayout.Controls.Add(lblObjectDeleted, 1, 6); // Add the deleted label

            detailsLayout.Controls.Add(this.btnDeleteObject, 1, 5);
            detailsLayout.Controls.Add(this.lblObjectDeleted, 1, 6);

            // Add detailsLayout to detailsPanel
            detailsPanel.Controls.Add(detailsLayout);

            // Add controls to objectDetailsLayout
            this.objectDetailsLayout.Controls.Add(this.pbObjectImage, 0, 0);
            this.objectDetailsLayout.Controls.Add(detailsPanel, 1, 0);

            // Arrange Top Layout
            this.topLayout.Controls.Add(this.lblObjectDetailsTitle, 0, 0);
            this.topLayout.Controls.Add(this.objectDetailsLayout, 0, 1);

            // Configure Labels and Controls for Bottom Section
            // lblAssociatedReportsTitle
            this.lblAssociatedReportsTitle.Text = "Rapports associés";
            this.lblAssociatedReportsTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblAssociatedReportsTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblAssociatedReportsTitle.Dock = DockStyle.Fill;
            this.lblAssociatedReportsTitle.ForeColor = ColorTranslator.FromHtml("#bc8246"); // Set to pagination color

            // Configure Filter Panel
            var lblCreatedAtStart = new Label { Text = "Date de début:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 0, 0) };
            this.dtpCreatedAtStart.Format = DateTimePickerFormat.Short;
            this.dtpCreatedAtStart.ShowCheckBox = true; // Allow the user to leave this field empty
            this.dtpCreatedAtStart.Checked = false;

            var lblCreatedAtEnd = new Label { Text = "Date de fin:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 0, 0) };
            this.dtpCreatedAtEnd.Format = DateTimePickerFormat.Short;
            this.dtpCreatedAtEnd.ShowCheckBox = true; // Allow the user to leave this field empty
            this.dtpCreatedAtEnd.Checked = false;

            var lblMotif = new Label { Text = "Motif:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 0, 0) };
            this.txtMotif.Width = 150;
            this.btnFilter.Text = "Filtrer";
            this.btnFilter.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            this.btnFilter.Size = new Size(100, 40);
            this.btnFilter.BackColor = ColorTranslator.FromHtml("#bc8246"); // Match pagination color
            this.btnFilter.ForeColor = Color.White;
            this.btnFilter.Click += new EventHandler(this.btnFilter_Click);

            this.filterPanel.Controls.Add(lblCreatedAtStart);
            this.filterPanel.Controls.Add(this.dtpCreatedAtStart);
            this.filterPanel.Controls.Add(new Label { Text = " ", AutoSize = true }); // Spacer
            this.filterPanel.Controls.Add(lblCreatedAtEnd);
            this.filterPanel.Controls.Add(this.dtpCreatedAtEnd);
            this.filterPanel.Controls.Add(new Label { Text = " ", AutoSize = true }); // Spacer
            this.filterPanel.Controls.Add(lblMotif);
            this.filterPanel.Controls.Add(this.txtMotif);
            this.filterPanel.Controls.Add(new Label { Text = " ", AutoSize = true }); // Spacer
            this.filterPanel.Controls.Add(this.btnFilter);

            // Configure DataGridView for Associated Reports
            this.dgvAssociatedReports.Dock = DockStyle.Fill;
            this.dgvAssociatedReports.AllowUserToAddRows = false;
            this.dgvAssociatedReports.AllowUserToDeleteRows = false;
            this.dgvAssociatedReports.AllowUserToResizeRows = false;
            this.dgvAssociatedReports.RowHeadersVisible = false;
            this.dgvAssociatedReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssociatedReports.ReadOnly = true;
            this.dgvAssociatedReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAssociatedReports.BackgroundColor = Color.White;

            // Add Columns to DataGridView
            var colReportId = new DataGridViewTextBoxColumn { HeaderText = "Id", Name = "Id", FillWeight = 10 };
            var colReason = new DataGridViewTextBoxColumn { HeaderText = "Raison", Name = "Reason", FillWeight = 40 };
            var colCreatedDate = new DataGridViewTextBoxColumn { HeaderText = "Date de création", Name = "CreatedDate", FillWeight = 25 };
            var colUsername = new DataGridViewTextBoxColumn { HeaderText = "Nom d'utilisateur", Name = "Username", FillWeight = 25 };

            this.dgvAssociatedReports.Columns.AddRange(new DataGridViewColumn[] { colReportId, colReason, colCreatedDate, colUsername });

            // Arrange Bottom Layout
            this.bottomLayout.Controls.Add(this.lblAssociatedReportsTitle, 0, 0);
            this.bottomLayout.Controls.Add(this.filterPanel, 0, 1);
            this.bottomLayout.Controls.Add(this.dgvAssociatedReports, 0, 2);
            this.bottomLayout.Controls.Add(this.pnlPagination, 0, 3);

            // Adjust Row Styles for Bottom Layout
            this.bottomLayout.RowStyles.Clear();
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Title
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Filter
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // DataGridView
            this.bottomLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Pagination

            // Add Sections to Main Layout
            this.mainLayout.Controls.Add(this.topLayout, 0, 0);
            this.mainLayout.Controls.Add(this.bottomLayout, 0, 1);

            // Loading Label
            this.loadingLabel.Text = "Chargement...";
            this.loadingLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.loadingLabel.ForeColor = Color.Gray;
            this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.loadingLabel.Dock = DockStyle.Fill;
            this.loadingLabel.Visible = false;

            // Configure ReportDetailModal
            this.ClientSize = new Size(1200, 900);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.loadingLabel);
            this.Text = "Détails de l'objet";
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private async void LoadReportDetails(int page = 1, string reason = null)
        {
            ShowLoading(true);
            try
            {
                // Get the filter values
                string startDate = dtpCreatedAtStart.Checked ? dtpCreatedAtStart.Value.ToString("yyyy-MM-dd") : null;
                string endDate = dtpCreatedAtEnd.Checked ? dtpCreatedAtEnd.Value.ToString("yyyy-MM-dd") : null;

                // Load report details using the objectId, page, and reason
                var response = await _reportService.GetReportDetailsAsync(_objectId, page, startDate, endDate, reason);

                if (response != null)
                {
                    lblObjectName.Text = response.Object.name;
                    lblObjectDescription.Text = response.Object.description;
                    lblCategory.Text = response.Object.category.name;
                    lblUserId.Text = $"{response.Object.User.username} ({response.Object.User.email})";
                    lblStatus.Text = response.Object.status;

                    // Set the status label
                    var statusLabel = detailsLayout.GetControlFromPosition(1, 4) as Label;
                    if (statusLabel != null)
                    {
                        statusLabel.Text = response.Object.status;
                    }

                    // Load the image from the URL if available
                    if (!string.IsNullOrEmpty(response.Object.image))
                    {
                        try
                        {
                            pbObjectImage.Image = await LoadImageFromUrlAsync(response.Object.image);
                        }
                        catch
                        {
                            pbObjectImage.Image = null; // or set a default image
                        }
                    }
                    else
                    {
                        pbObjectImage.Image = null; // or set a default image
                    }

                    if (response.Object.status == "Effacé")
                    {
                        lblObjectDeleted.Visible = true;
                        btnDeleteObject.Visible = false;
                    }
                    else
                    {
                        lblObjectDeleted.Visible = false;
                        btnDeleteObject.Visible = true;
                    }

                    // Update the reports section
                    _totalPages = response.totalPages;
                    _currentPage = page;
                    PopulateAssociatedReports(response.reports);
                    UpdatePaginationControls();
                }
                else
                {
                    MessageBox.Show("Aucun rapport trouvé.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des rapports: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async Task<Image> LoadImageFromUrlAsync(string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        return Image.FromStream(stream);
                    }
                }
                else
                {
                    return null; // or return a default image
                }
            }
        }

        private void PopulateAssociatedReports(List<Report> reports)
        {
            dgvAssociatedReports.Rows.Clear();
            foreach (var report in reports)
            {
                int rowIndex = dgvAssociatedReports.Rows.Add(report.id, report.reason, report.createdAt.ToShortDateString(), report.User.Username);
            }
        }

        private void UpdatePaginationControls()
        {
            pnlPagination.Controls.Clear();
            for (int i = 1; i <= _totalPages; i++)
            {
                int pageNumber = i;
                var btnPageNumber = new Button
                {
                    Text = pageNumber.ToString(),
                    Size = new Size(40, 30),
                    Margin = new Padding(2),
                    BackColor = pageNumber == _currentPage ? ColorTranslator.FromHtml("#bc8246") : Color.White,
                    ForeColor = pageNumber == _currentPage ? Color.White : ColorTranslator.FromHtml("#383838"),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Enabled = pageNumber != _currentPage
                };
                if (pageNumber != _currentPage)
                {
                    btnPageNumber.Click += async (s, e) =>
                    {
                        _currentPage = pageNumber;
                        LoadReportDetails(pageNumber, txtMotif.Text.Trim());
                    };
                }
                pnlPagination.Controls.Add(btnPageNumber);
            }
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            LoadReportDetails(1, txtMotif.Text.Trim());
        }

        private void ShowLoading(bool show)
        {
            loadingLabel.Visible = show;
            mainLayout.Visible = !show;
        }

        private async void btnDeleteObject_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this object?",
                                                 "Confirm Delete!",
                                                 MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    bool response = await _objectService.DeleteObjectAsync(_objectId);
                    if (response)
                    {
                        MessageBox.Show("Object deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadReportDetails();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete object.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression de l'objet: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
