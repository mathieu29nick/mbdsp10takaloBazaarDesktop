using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Services;
using WinFormsApp.UI.components;

namespace WinFormsApp.UI
{
    public partial class ReportListScreen : UserControl
    {
        private readonly ReportService _reportService;
        private int _currentPage = 1;
        private int _totalPages = 1;
        private const int PageSize = 10;
        private bool _isLoading = false;

        public ReportListScreen()
        {
            InitializeComponent();
            _reportService = new ReportService();
        }

        private async void ReportListScreen_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            await LoadReportsAsync();
        }

        private async Task LoadReportsAsync(int page = 1, int limit = 10)
        {
            if (_isLoading) return;

            _isLoading = true;
            ShowLoading(true);

            try
            {
                var reportResponse = await _reportService.GetReportsAsync(page, limit);

                _totalPages = reportResponse.TotalPages;
                _currentPage = reportResponse.CurrentPage;

                UpdatePaginationControls();
                PopulateDataGrid(reportResponse.Reports);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des rapports: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
                _isLoading = false;
            }
        }

        private void PopulateDataGrid(List<ReportItem> reports)
        {
            dgvReports.Rows.Clear();

            foreach (var report in reports)
            {
                int rowIndex = dgvReports.Rows.Add(report.ObjectId, report.ObjectName, report.reportCount, "Détail");
                dgvReports.Rows[rowIndex].Cells["Detail"].Tag = report.ObjectId;
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
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Enabled = pageNumber != _currentPage
                };

                btnPageNumber.ForeColor = pageNumber == _currentPage ? Color.White : ColorTranslator.FromHtml("#383838");
                btnPageNumber.BackColor = pageNumber == _currentPage ? ColorTranslator.FromHtml("#bc8246") : Color.White;

                btnPageNumber.Paint += (s, e) =>
                {
                    var btn = (Button)s;
                    e.Graphics.Clear(btn.BackColor);
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, e.ClipRectangle, btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };

                if (pageNumber != _currentPage)
                {
                    btnPageNumber.Click += async (s, e) =>
                    {
                        _currentPage = pageNumber;
                        await LoadReportsAsync(pageNumber, PageSize);
                    };
                }
                pnlPagination.Controls.Add(btnPageNumber);
            }
        }


        private void dgvReports_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvReports.Columns[e.ColumnIndex].Name == "Detail")
            {
                int objectId = (int)dgvReports.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                OpenReportDetailModal(objectId);
            }
        }

        private async void OpenReportDetailModal(int objectId)
        {
            try
            {
                var modal = new ReportDetailModal(objectId);
                modal.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des détails du rapport: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowLoading(bool show)
        {
            loadingLabel.Visible = show;
            dgvReports.Visible = !show;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.dgvReports = new DataGridView();
            this.pnlPagination = new FlowLayoutPanel();
            this.loadingLabel = new Label();

            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "Liste des objets signalés";
            this.lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitle.Location = new Point(0, 60);
            this.lblTitle.Size = new Size(1024, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // dgvReports
            this.dgvReports.Location = new Point(50, 120);
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.Size = new Size(924, 400);
            this.dgvReports.TabIndex = 0;
            this.dgvReports.AllowUserToAddRows = false;
            this.dgvReports.AllowUserToDeleteRows = false;
            this.dgvReports.AllowUserToResizeRows = false;
            this.dgvReports.RowHeadersVisible = false;
            this.dgvReports.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReports.ReadOnly = true;
            this.dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReports.BackgroundColor = Color.White;

            // Adding Columns
            var colId = new DataGridViewTextBoxColumn { HeaderText = "Id", Name = "Id", Width = 50 };
            var colObjectName = new DataGridViewTextBoxColumn { HeaderText = "Nom", Name = "ObjectName", Width = 300 };
            var colReportCount = new DataGridViewTextBoxColumn { HeaderText = "Nombre", Name = "ReportCount", Width = 150 };
            var colDetail = new DataGridViewButtonColumn { HeaderText = "", Name = "Detail", Width = 100 };

            this.dgvReports.Columns.AddRange(new DataGridViewColumn[] { colId, colObjectName, colReportCount, colDetail });
            this.dgvReports.CellClick += new DataGridViewCellEventHandler(this.dgvReports_CellClick);

            // pnlPagination
            this.pnlPagination.Location = new Point(50, 530);
            this.pnlPagination.Name = "pnlPagination";
            this.pnlPagination.Size = new Size(500, 30);
            this.pnlPagination.TabIndex = 6;
            this.pnlPagination.AutoSize = true;

            // loadingLabel
            this.loadingLabel.Location = new Point(12, 250);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new Size(760, 50);
            this.loadingLabel.TabIndex = 7;
            this.loadingLabel.Text = "Chargement...";
            this.loadingLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.loadingLabel.Visible = false;

            // ReportListScreen
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvReports);
            this.Controls.Add(this.pnlPagination);
            this.Controls.Add(this.loadingLabel);
            this.Name = "ReportListScreen";
            this.Size = new Size(1024, 600);
            this.Load += new EventHandler(this.ReportListScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private DataGridView dgvReports;
        private FlowLayoutPanel pnlPagination;
        private Label loadingLabel;
    }
}
