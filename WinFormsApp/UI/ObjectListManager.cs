using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Services;

namespace WinFormsApp.UI
{
    public class ObjectListManager
    {
        private readonly Panel _panel;
        private readonly DataGridView _dataGridView;
        private readonly ObjectService _objectService;
        private readonly CategoryService _categoryService;

        private int _currentPage = 1; 
        private int _pageSize = 30;  

        public ObjectListManager(Panel panel)
        {
            _panel = panel;
            _objectService = new ObjectService();
            _categoryService = new CategoryService();

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(60),
                BackColor = Color.White
            };

            Label titleLabel = new Label
            {
                Text = "LISTE DES OBJETS",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 90,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 40, 0)
            };

            FlowLayoutPanel searchPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(20, 20, 0, 20),
                Padding = new Padding(30),
                WrapContents = false
            };

            TextBox searchTextBox1 = new TextBox { Width = 400, PlaceholderText = "Libellé", Margin = new Padding(0, 0, 10, 0) };
            TextBox searchTextBox3 = new TextBox { Width = 400, PlaceholderText = "Description", Margin = new Padding(0, 0, 10, 0) };

            ComboBox categoryComboBox = new ComboBox
            {
                Width = 400,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 10, 0)
            };

            ComboBox statusComboBox = new ComboBox
            {
                Width = 400,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 10, 0)
            };

            var statusItems = new Dictionary<string, string>
            {
                { "Tous les statuts", "" },
                { "Disponible", "Available" },
                { "Retiré", "Removed" }
            };

            foreach (var item in statusItems)
            {
                statusComboBox.Items.Add(item.Key);
            }

            statusComboBox.SelectedIndex = 0;

            Button searchButton = new Button
            {
                Text = "Rechercher",
                BackColor = ColorTranslator.FromHtml("#bc8246"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 250,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0)
            };

            Button previousButton = new Button { Text = "Précédent", Width = 150, Height = 50, BackColor = ColorTranslator.FromHtml("#8a8f6a"), ForeColor = Color.White };
            Button nextButton = new Button { Text = "Suivant", Width = 150, Height = 50, BackColor = ColorTranslator.FromHtml("#8a8f6a"), ForeColor = Color.White };

            searchButton.Click += async (sender, e) =>
            {
                _currentPage = 1;
                string selectedText = statusComboBox.SelectedItem?.ToString();
                string selectedStatus = statusItems.ContainsKey(selectedText) ? statusItems[selectedText] : "";
                await LoadObjectsAsync(_currentPage, _pageSize, searchTextBox1.Text, searchTextBox3.Text, (int?)categoryComboBox.SelectedValue, selectedStatus, previousButton, nextButton);
            };

            searchPanel.Controls.Add(searchTextBox1);
            searchPanel.Controls.Add(searchTextBox3);
            searchPanel.Controls.Add(categoryComboBox);
            searchPanel.Controls.Add(statusComboBox);
            searchPanel.Controls.Add(searchButton);

            FlowLayoutPanel paginationPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 20),
                Padding = new Padding(20),
                WrapContents = false
            };

            previousButton.Click += async (sender, e) =>
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    string selectedText = statusComboBox.SelectedItem?.ToString();
                    string selectedStatus = statusItems.ContainsKey(selectedText) ? statusItems[selectedText] : "";
                    await LoadObjectsAsync(_currentPage, _pageSize, searchTextBox1.Text, searchTextBox3.Text, (int?)categoryComboBox.SelectedValue, selectedStatus, previousButton, nextButton);
                }
            };

            nextButton.Click += async (sender, e) =>
            {
                _currentPage++;
                string selectedText = statusComboBox.SelectedItem?.ToString();
                string selectedStatus = statusItems.ContainsKey(selectedText) ? statusItems[selectedText] : "";
                await LoadObjectsAsync(_currentPage, _pageSize, searchTextBox1.Text, searchTextBox3.Text, (int?)categoryComboBox.SelectedValue, selectedStatus, previousButton, nextButton);
            };


            paginationPanel.Controls.Add(previousButton);
            paginationPanel.Controls.Add(nextButton);

            _dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoGenerateColumns = false,
                Width = 200,
                Margin = new Padding(0, 0, 0, 20),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "Id" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nom", DataPropertyName = "Name" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Catégorie", DataPropertyName = "CategoryName" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Créé le", DataPropertyName = "CreatedAt" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Statut", DataPropertyName = "Status" });

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "Modifier",
                UseColumnTextForButtonValue = true,
                Name = "EditButton",
                DataPropertyName = "EditButton",
                FlatStyle = FlatStyle.Flat,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            _dataGridView.Columns.Add(editButtonColumn);

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "Supprimer",
                UseColumnTextForButtonValue = true,
                Name = "DeleteButton",
                DataPropertyName = "DeleteButton",
                FlatStyle = FlatStyle.Flat,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            _dataGridView.Columns.Add(deleteButtonColumn);

            mainPanel.Controls.Add(_dataGridView);
            mainPanel.Controls.Add(searchPanel);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(paginationPanel);

            _panel.Controls.Add(mainPanel);
            PopulateCategoryComboBoxAsync(categoryComboBox).ConfigureAwait(false);
        }

        public async Task LoadObjectsAsync(int page = 1, int limit = 30, string name = null, string description = null, int? categoryId = null, string status = null, Button previousButton = null, Button nextButton = null)
        {
            try
            {
                List<Models.Object> objects = await _objectService.GetObjectsAsync(page, limit, name, description, null, categoryId, status);

                if (objects == null || objects.Count == 0)
                {
                    MessageBox.Show("Aucun objet trouvé.");
                    _dataGridView.DataSource = null;
                }
                else
                {
                    _dataGridView.DataSource = objects;
                }

                
                if (previousButton != null)
                {
                    previousButton.Visible = page > 1;
                }

                if (nextButton != null)
                {
                    nextButton.Visible = objects.Count == limit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du chargement des objets : {ex.Message}");
            }
        }

        public async Task PopulateCategoryComboBoxAsync(ComboBox comboBox)
        {
            try
            {
                CategoriesResponse categoriesresponse = await _categoryService.GetCategoriesAsync(1,1000,"");
                List<Category> categories = categoriesresponse.Data.Categories;
                categories.Insert(0, new Category { Id = -1, Name = "Toutes les catégories" });
                if (categories != null && categories.Count > 0)
                {
                    comboBox.DataSource = categories;
                    comboBox.DisplayMember = "Name";
                    comboBox.ValueMember = "Id";
                }
                else
                {
                    MessageBox.Show("Aucune catégorie trouvée.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du chargement des catégories : {ex.Message}");
            }
        }
    }
}
