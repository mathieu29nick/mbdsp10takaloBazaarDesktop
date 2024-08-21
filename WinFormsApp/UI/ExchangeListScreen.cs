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
    public class ExchangeListScreen
    {
        private readonly Panel _panel;
        private readonly DataGridView _dataGridView;
        private readonly ExchangeService _exchangeService;
        private readonly UserService _userService;
        private ComboBox userComboBox1;
        private ComboBox userComboBox2;

        private int _currentPage = 1;
        private int _pageSize = 10;

        public ExchangeListScreen(Panel panel)
        {
            _panel = panel;
            _exchangeService = new ExchangeService();
            _userService = new UserService();

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(60),
                BackColor = Color.White
            };

            Label titleLabel = new Label
            {
                Text = "LISTE DES ÉCHANGES",
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

            ComboBox statusComboBox = new ComboBox
            {
                Width = 400,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 10, 0)
            };

            var statusItems = new Dictionary<string, string>
            {
                { "Tous les statuts", "" },
                { "Proposé", "Proposed" },
                { "Accepté", "Accepted" },
                { "Refusé", "Refused" },
                { "Annulé", "Cancelled" }
            };

            foreach (var item in statusItems)
            {
                statusComboBox.Items.Add(item.Key);
            }

            statusComboBox.SelectedIndex = 0;

            userComboBox1 = new ComboBox
            {
                Width = 400,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 10, 0)
            };

            userComboBox2 = new ComboBox
            {
                Width = 400,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 10, 0)
            };

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
                string selectedStatus = statusItems.ContainsKey(statusComboBox.SelectedItem?.ToString()) ? statusItems[statusComboBox.SelectedItem.ToString()] : "";
                await LoadExchangesAsync(_currentPage, _pageSize, selectedStatus, previousButton, nextButton);
            };

            searchPanel.Controls.Add(statusComboBox);
            searchPanel.Controls.Add(userComboBox1);
            searchPanel.Controls.Add(userComboBox2);
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
                    string selectedStatus = statusItems.ContainsKey(statusComboBox.SelectedItem?.ToString()) ? statusItems[statusComboBox.SelectedItem.ToString()] : "";
                    await LoadExchangesAsync(_currentPage, _pageSize, selectedStatus, previousButton, nextButton);
                }
            };

            nextButton.Click += async (sender, e) =>
            {
                _currentPage++;
                string selectedStatus = statusItems.ContainsKey(statusComboBox.SelectedItem?.ToString()) ? statusItems[statusComboBox.SelectedItem.ToString()] : "";
                await LoadExchangesAsync(_currentPage, _pageSize, selectedStatus, previousButton, nextButton);
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
                Height = 200,
                Margin = new Padding(0, 0, 0, 20),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "Id", Name = "Id" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Proposeur", DataPropertyName = "ProposerUsername" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Receveur", DataPropertyName = "ReceiverUsername" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Statut", DataPropertyName = "Status" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date de création", DataPropertyName = "Created_At" });

            var detailButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "-",
                Text = "Détail",
                UseColumnTextForButtonValue = true,
                Name = "DetailButton",
                DataPropertyName = "DetailButton",
                FlatStyle = FlatStyle.Flat,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            _dataGridView.Columns.Add(detailButtonColumn);

            _dataGridView.CellClick += DataGridView_CellClick;

            mainPanel.Controls.Add(_dataGridView);
            mainPanel.Controls.Add(searchPanel);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(paginationPanel);

            _panel.Controls.Add(mainPanel);
            PopulateUserComboBoxAsync(userComboBox1, "Proposeur").ConfigureAwait(false);
            PopulateUserComboBoxAsync(userComboBox2, "Receveur").ConfigureAwait(false);
            //await LoadExchangesAsync(_currentPage, _pageSize, "", previousButton, nextButton);
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (_dataGridView.Columns[e.ColumnIndex].Name == "DetailButton")
                {
                    int exchangeId = (int)_dataGridView.Rows[e.RowIndex].Cells["Id"].Value;
                   // ShowExchangeDetailsAsync(exchangeId).ConfigureAwait(false);
                }
            }
        }
        //public async Task LoadObjectsAsync(int page = 1, int limit = 30, string name = null, string description = null, int? categoryId = null, string status = null, Button previousButton = null, Button nextButton = null)
        public async Task LoadExchangesAsync(int page=1, int limit=10, string status=null, Button previousButton=null, Button nextButton=null)
        {
            try
            {
                var filters = new Dictionary<string, string>
                {
                    { "status", status },
                    {"proposer_user_id", userComboBox1.SelectedValue != null && (int)userComboBox1.SelectedValue != -1 ? userComboBox1.SelectedValue.ToString() : ""},
                    {"receiver_user_id",userComboBox2.SelectedValue != null && (int)userComboBox2.SelectedValue != -1 ? userComboBox2.SelectedValue.ToString() : "" }
                };

                List<Exchange> exchanges = await _exchangeService.GetExchangesAsync(page, limit, null, null, filters);

                if (exchanges == null || exchanges.Count == 0)
                {
                    MessageBox.Show("Aucun échange trouvé.");
                    _dataGridView.DataSource = null;
                }
                else
                {
                    _dataGridView.DataSource = exchanges;
                }

                if (previousButton != null)
                {
                    previousButton.Visible = page > 1;
                }

                if (nextButton != null)
                {
                    nextButton.Visible = exchanges.Count == limit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du chargement des échanges : {ex.Message}");
            }
        }

        /*private async Task ShowExchangeDetailsAsync(int exchangeId)
        {
            try
            {
                Exchange selectedExchange = await _exchangeService.GetExchangeByIdAsync(exchangeId);

                if (selectedExchange != null)
                {
                    ExchangeDetailForm detailForm = new ExchangeDetailForm(selectedExchange);
                    detailForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la récupération des détails de l'échange.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }*/

        private async Task PopulateUserComboBoxAsync(ComboBox comboBox,string role)
        {
            try
            {
                UserResponse userResponse = await _userService.GetUsersAsync(1, 100);
                List<User> user = userResponse.Users;

                user.Insert(0, new User { Id = -1, Username = role });

                comboBox.DataSource = user;
                comboBox.DisplayMember = "Username";
                comboBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des utilisateurs: {ex.Message}");
            }
        }
    }
}
