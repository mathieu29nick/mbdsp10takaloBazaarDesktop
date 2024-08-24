using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Services;
using WinFormsApp.UI.components;

namespace WinFormsApp.UI
{
    public class TypeReportListScreen
    {
        private readonly Panel _panel;
        private readonly DataGridView _dataGridView;
        private readonly TypeReportService _typeReportService;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int totalPage = 1;
        private Button previousButton;
        private Button nextButton;
        private bool _isLoading = false;
        private Label loadingLabel;
        private Panel listPanel;

        public TypeReportListScreen(Panel panel)
        {
            _panel = panel;
            _typeReportService = new TypeReportService();
            loadingLabel = new Label();
            this.loadingLabel.Location = new Point(12, 250);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new Size(760, 50);
            this.loadingLabel.TabIndex = 7;
            this.loadingLabel.Text = "Chargement...";
            this.loadingLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.loadingLabel.Visible = false;

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(60),
                BackColor = Color.White
            };

            listPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(60),
                BackColor = Color.White
            };

            Label titleLabel = new Label
            {
                Text = "Liste des types de signalement",
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

            TextBox searchTextBox = new TextBox
            {
                Width = 600,
                PlaceholderText = "Nom",
                Height = 45,
                Margin = new Padding(0, 0, 10, 0)
            };

            Button searchButton = new Button
            {
                Text = "Rechercher",
                BackColor = ColorTranslator.FromHtml("#bc8246"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 250,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 10, 0)
            };

            Button addButton = new Button
            {
                Text = "Ajouter un nouveau type de signalement",
                BackColor = ColorTranslator.FromHtml("#8a8f6a"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 600,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 10, 0)
            };

            previousButton = new Button
            {
                Text = "Précédent",
                Width = 150,
                Height = 50,
                BackColor = ColorTranslator.FromHtml("#8a8f6a"),
                ForeColor = Color.White
            };

            nextButton = new Button
            {
                Text = "Suivant",
                Width = 150,
                Height = 50,
                BackColor = ColorTranslator.FromHtml("#8a8f6a"),
                ForeColor = Color.White
            };

            searchButton.Click += async (sender, e) =>
            {
                _currentPage = 1;
                await LoadTypeReportsAsync(_currentPage, _pageSize, searchTextBox.Text);
            };

            addButton.Click += async (sender, e) =>
            {
                OpenInsertModal();
            };

            searchPanel.Controls.Add(searchTextBox);
            searchPanel.Controls.Add(searchButton);
            searchPanel.Controls.Add(addButton);

            FlowLayoutPanel paginationPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.None,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 20),
                Padding = new Padding(20),
                Location = new Point(0, 700),
                WrapContents = false
            };


            previousButton.Click += async (sender, e) =>
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    await LoadTypeReportsAsync(_currentPage, _pageSize, searchTextBox.Text);
                }
            };

            nextButton.Click += async (sender, e) =>
            {
                _currentPage++;
                await LoadTypeReportsAsync(_currentPage, _pageSize, searchTextBox.Text);
            };

            paginationPanel.Controls.Add(previousButton);
            paginationPanel.Controls.Add(nextButton);

            _dataGridView = new DataGridView
            {
                Dock = DockStyle.None, 
                Location = new Point(0, 200),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoGenerateColumns = false,
                Margin = new Padding(20, 20, 0, 20),
                Width = 1500,
                Height = 490,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            _dataGridView.CellClick += _dataGridView_CellClick;


            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "Id" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Nom", DataPropertyName = "Name" });

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
            listPanel.Controls.Add(paginationPanel);
            listPanel.Controls.Add(_dataGridView);
            mainPanel.Controls.Add(searchPanel);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(listPanel);
            mainPanel.Controls.Add(loadingLabel);

            _panel.Controls.Add(mainPanel);
        }

        private async void _dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == _dataGridView.Columns["DeleteButton"].Index)
            {
                int id = (int)_dataGridView.Rows[e.RowIndex].Cells["Id"].Value;
                var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce type de signalement ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    await DeleteTypeReportAsync(id);
                }
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == _dataGridView.Columns["EditButton"].Index)
            {
                int id = (int)_dataGridView.Rows[e.RowIndex].Cells["Id"].Value;
                string name = (string)_dataGridView.Rows[e.RowIndex].Cells["Name"].Value;
                OpenEditModal(id, name);

            }
        }

        private void OpenInsertModal()
        {
            var modal = new TypeReportModal();
            modal.FormClosed += async (s, e) => await LoadTypeReportsAsync(_currentPage,10); 
            modal.ShowDialog();
        }

        private void OpenEditModal(int? id, string name)
        {
            var modal = new TypeReportModal(id, name);
            modal.FormClosed += async (s, e) => await LoadTypeReportsAsync(totalPage, 10); 
            modal.ShowDialog();
        }



        private async Task DeleteTypeReportAsync(int? id)
        {
            _currentPage = 1;
            try
            {
                if (id != null)
                {
                    ShowLoading(true);
                    bool isDeleted = await _typeReportService.DeleteTypeReportAsync(id ?? 0);
                    if (isDeleted)
                    {
                        MessageBox.Show("Type de signalement supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadTypeReportsAsync(_currentPage,10);
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

        private void ShowLoading(bool show)
        {
            loadingLabel.Visible = show;
            listPanel.Visible = !show;
        }

        public async Task LoadTypeReportsAsync(int page = 1, int limit = 10, string name = null)
        {
            try
            {
                if (_isLoading) return;

                _isLoading = true;
                ShowLoading(true);
                TypeReportResponse typeReportResponse = await _typeReportService.GetTypeReportsAsync(page, limit, name);
                List<TypeReport> typeReports = typeReportResponse.Data.TypeReports;
                if (typeReports == null || typeReports.Count == 0)
                {
                    MessageBox.Show("Aucun type de signalement trouvé.");
                    _dataGridView.DataSource = null;
                }
                else
                {
                    _dataGridView.DataSource = typeReports;
                    totalPage = typeReportResponse.Data.TotalPages ?? totalPage;
                }

                if(previousButton != null)
                {
                    previousButton.Visible = page > 1;
                }
                if(nextButton != null)
                {
                    nextButton.Visible = _currentPage != typeReportResponse.Data.TotalPages;
                }
                ShowLoading(false);
                _isLoading = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du chargement des types de signalement : {ex.Message}");
            }
        }
    }
}
