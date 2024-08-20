using System;
using System.Collections.Generic;
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
            Console.WriteLine(response.Data.CurrentPage);

            _totalPages = response.Data.TotalPages ?? 1;
            _currentPage = response.Data.CurrentPage ?? 1;

            UpdatePaginationControls();

            listViewCategories.Items.Clear();

            foreach (var category in response.Data.Categories)
            {
                var listItem = new ListViewItem(category.Id.ToString());
                listItem.SubItems.Add(category.Name);

                // Add an empty subitem for the Edit column
                var editSubItem = new ListViewItem.ListViewSubItem();
                editSubItem.Text = "Edit";
                editSubItem.Tag = category.Id;
                editSubItem.Font = new Font("Segoe UI", 10, FontStyle.Underline);
                editSubItem.ForeColor = Color.Blue;
                listItem.SubItems.Add(editSubItem);

                // Add an empty subitem for the Delete column
                var deleteSubItem = new ListViewItem.ListViewSubItem();
                deleteSubItem.Text = "Delete";
                deleteSubItem.Tag = category.Id;
                deleteSubItem.Font = new Font("Segoe UI", 10, FontStyle.Underline);
                deleteSubItem.ForeColor = Color.Red;
                listItem.SubItems.Add(deleteSubItem);

                listViewCategories.Items.Add(listItem);
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
                    BackColor = pageNumber == _currentPage ? Color.FromArgb(56, 56, 56) : Color.White,
                    ForeColor = pageNumber == _currentPage ? Color.White : Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Enabled = pageNumber != _currentPage
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
            listViewCategories.Visible = !show;
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
            this.listViewCategories = new ListView();
            this.colId = new ColumnHeader();
            this.colName = new ColumnHeader();
            this.colEdit = new ColumnHeader();
            this.colDelete = new ColumnHeader();
            this.tbSearch = new TextBox();
            this.btnSearch = new Button();
            this.btnInsert = new Button();
            this.pnlPaginationNumbers = new FlowLayoutPanel();
            this.loadingLabel = new Label();

            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Liste catégories";
            this.lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitle.Location = new Point(0, 60); // Adjusted to appear below the nav
            this.lblTitle.Size = new Size(1024, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // listViewCategories
            // 
            this.listViewCategories.Columns.AddRange(new ColumnHeader[] {
            this.colId,
            this.colName,
            this.colEdit,
            this.colDelete});
            this.listViewCategories.FullRowSelect = true;
            this.listViewCategories.GridLines = true;
            this.listViewCategories.Location = new Point(50, 170);
            this.listViewCategories.Name = "listViewCategories";
            this.listViewCategories.Size = new Size(1000, 350);
            this.listViewCategories.TabIndex = 0;
            this.listViewCategories.UseCompatibleStateImageBehavior = false;
            this.listViewCategories.View = View.Details;
            this.listViewCategories.BackColor = Color.WhiteSmoke;
            this.listViewCategories.BorderStyle = BorderStyle.None;
            this.listViewCategories.Scrollable = false;

            this.listViewCategories.MouseClick += ListViewCategories_MouseClick;

            // 
            // colId
            // 
            this.colId.Text = "ID";
            this.colId.Width = 100;

            // 
            // colName
            // 
            this.colName.Text = "Nom";
            this.colName.Width = 600;

            // 
            // colEdit
            // 
            this.colEdit.Text = "";
            this.colEdit.Width = 100;

            // 
            // colDelete
            // 
            this.colDelete.Text = "";
            this.colDelete.Width = 100;

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
            this.btnSearch.Size = new Size(140, 40);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Rechercher";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.BackColor = Color.FromArgb(56, 56, 56);
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // 
            // btnInsert
            // 
            this.btnInsert.Location = new Point(820, 110);
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
            this.Controls.Add(this.listViewCategories);
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

        private async void ListViewCategories_MouseClick(object sender, MouseEventArgs e)
        {
            var hitTest = listViewCategories.HitTest(e.Location);
            if (hitTest.Item != null)
            {
                int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                var id = (int)hitTest.Item.SubItems[2].Tag;

                if (columnIndex == 2) // Edit column
                {
                    OpenEditModal(id, hitTest.Item.SubItems[1].Text);
                }
                else if (columnIndex == 3) // Delete column
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
        private ListView listViewCategories;
        private ColumnHeader colId;
        private ColumnHeader colName;
        private ColumnHeader colEdit;
        private ColumnHeader colDelete;
        private TextBox tbSearch;
        private Button btnSearch;
        private Button btnInsert;
        private FlowLayoutPanel pnlPaginationNumbers;
        private Label loadingLabel;
    }
}
