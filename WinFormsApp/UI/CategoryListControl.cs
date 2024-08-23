using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Services;
using WinFormsApp.UI.components;

namespace WinFormsApp.UI
{
    public partial class CategoryListControl : UserControl
    {
        private int _currentPage = 1;
        private int _totalPages = 1;
        private const int PageSize = 10;
        private bool _isLoading = false;

        public CategoryListControl()
        {
            InitializeComponent();
        }

        private async void CategoryListControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            await LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync(string searchQuery = "")
        {
            if (_isLoading) return;

            _isLoading = true;
            ShowLoading(true);

            var categoryService = new CategoryService();
            var response = await categoryService.GetCategoriesAsync(_currentPage, PageSize, searchQuery);

            _totalPages = response.Data.TotalPages ?? 1;
            _currentPage = response.Data.CurrentPage ?? 1;

            UpdatePaginationControls();

            dgvCategories.Rows.Clear();

            foreach (var category in response.Data.Categories)
            {
                dgvCategories.Rows.Add(category.Id, category.Name);
            }

            ShowLoading(false);
            _isLoading = false;
        }

        private void UpdatePaginationControls()
        {
            pnlPaginationNumbers.Controls.Clear();
            for (int i = 1; i <= _totalPages; i++)
            {
                int pageNumber = i; // Create a local copy of the loop variable
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
                        await LoadCategoriesAsync(tbSearch.Text);
                    };
                }
                pnlPaginationNumbers.Controls.Add(btnPageNumber);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            await LoadCategoriesAsync(tbSearch.Text);
        }

        private void ShowLoading(bool show)
        {
            loadingLabel.Visible = show;
            dgvCategories.Visible = !show;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            OpenInsertModal();
        }

        private void OpenInsertModal()
        {
            var modal = new CategoryModal();
            modal.FormClosed += async (s, e) => await LoadCategoriesAsync(); // Refresh after insert
            modal.ShowDialog();
        }

        private void OpenEditModal(int? id, string name)
        {
            var modal = new CategoryModal(id, name);
            modal.FormClosed += async (s, e) => await LoadCategoriesAsync(); // Refresh after edit
            modal.ShowDialog();
        }

        private async Task DeleteCategoryAsync(int? id)
        {
            var categoryService = new CategoryService();
            try
            {
                if (id != null)
                {
                    ShowLoading(true);
                    bool isDeleted = await categoryService.DeleteCategoryAsync(id ?? 0);
                    if (isDeleted)
                    {
                        MessageBox.Show("Catégorie supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadCategoriesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.dgvCategories = new DataGridView();
            this.tbSearch = new TextBox();
            this.btnSearch = new Button();
            this.btnInsert = new Button();
            this.pnlPaginationNumbers = new FlowLayoutPanel();
            this.loadingLabel = new Label();

            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Liste des catégories";
            this.lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitle.Location = new Point(0, 60); // Adjusted to appear below the nav
            this.lblTitle.Size = new Size(1024, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // dgvCategories
            // 
            this.dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Columns.AddRange(new DataGridViewColumn[] {
        new DataGridViewTextBoxColumn { HeaderText = "ID", Name = "colId", Width = 100 },
        new DataGridViewTextBoxColumn { HeaderText = "Nom", Name = "colName", Width = 600 },
        new DataGridViewButtonColumn { HeaderText = "-", Text = "Modifier", Name = "colEdit", Width = 100, UseColumnTextForButtonValue = true },
        new DataGridViewButtonColumn { HeaderText = "-", Text = "Supprimer", Name = "colDelete", Width = 100, UseColumnTextForButtonValue = true }
    });
            this.dgvCategories.Location = new Point(50, 170);
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.Size = new Size(1000, 350);
            this.dgvCategories.TabIndex = 0;
            this.dgvCategories.CellClick += dgvCategories_CellClick;
            this.dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCategories.ReadOnly = true;
            this.dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategories.BackgroundColor = Color.White;
            this.dgvCategories.AllowUserToAddRows = false;

            // 
            // tbSearch
            // 
            this.tbSearch.Location = new Point(50, 110);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new Size(600, 40);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.Font = new Font("Segoe UI", 12);

            // 
            // btnSearch
            // 
            this.btnSearch.Location = new Point(670, 110);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(150, 40);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Rechercher";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.BackColor = ColorTranslator.FromHtml("#bc8246");
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // 
            // btnInsert
            // 
            this.btnInsert.Location = new Point(830, 110);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new Size(140, 40);
            this.btnInsert.TabIndex = 3;
            this.btnInsert.Text = "Ajouter";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.BackColor = Color.FromArgb(56, 56, 56);
            this.btnInsert.ForeColor = Color.White;
            this.btnInsert.FlatStyle = FlatStyle.Flat;
            this.btnInsert.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnInsert.Click += new EventHandler(this.btnInsert_Click);

            // 
            // pnlPaginationNumbers
            // 
            this.pnlPaginationNumbers.Location = new Point(50, 530);
            this.pnlPaginationNumbers.Name = "pnlPaginationNumbers";
            this.pnlPaginationNumbers.Size = new Size(500, 30);
            this.pnlPaginationNumbers.TabIndex = 6;
            this.pnlPaginationNumbers.AutoSize = true;

            // 
            // loadingLabel
            // 
            this.loadingLabel.Location = new Point(12, 250);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new Size(760, 50);
            this.loadingLabel.TabIndex = 7;
            this.loadingLabel.Text = "Chargement...";
            this.loadingLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.loadingLabel.Visible = false;

            // 
            // CategoryListControl
            // 
            this.AutoScaleDimensions = new SizeF(9F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvCategories);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.pnlPaginationNumbers);
            this.Controls.Add(this.loadingLabel);
            this.Name = "CategoryListControl";
            this.Size = new Size(1024, 600); // Adjust this if needed for your layout
            this.Load += new EventHandler(this.CategoryListControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private async void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = Convert.ToInt32(dgvCategories.Rows[e.RowIndex].Cells[0].Value);

                if (e.ColumnIndex == dgvCategories.Columns["colEdit"].Index)
                {
                    OpenEditModal(id, dgvCategories.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
                else if (e.ColumnIndex == dgvCategories.Columns["colDelete"].Index)
                {
                    var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette catégorie ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        await DeleteCategoryAsync(id);
                    }
                }
            }
        }

        private Label lblTitle;
        private DataGridView dgvCategories;
        private TextBox tbSearch;
        private Button btnSearch;
        private Button btnInsert;
        private FlowLayoutPanel pnlPaginationNumbers;
        private Label loadingLabel;
    }
}
