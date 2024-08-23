using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp.Services;
using WinFormsApp.Models;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp.UI
{
    public class DashboardScreen
    {
        private readonly Panel _panel;
        private readonly DashboardService _dashboardService;
        private DataGridView _dataGridView;
        private Label lblOngoingExchanges;
        private Label lblAcceptedExchanges;
        private Label lblRefusedExchanges;
        private Label lblCancelledExchanges;
        private Chart pieChart1;
        private Chart columnChart;
        private Label loadingLabel;
        private Panel dash;

        public DashboardScreen(Panel panel)
        {
            _panel = panel;
            _dashboardService = new DashboardService();

            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Color.White
            };
            dash = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Color.White,
                AutoScroll = true
            };
            TableLayoutPanel mainPanel1 = new TableLayoutPanel
            {
                Dock = DockStyle.Top, 
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(30),
                BackColor = Color.White,
                AutoSize = true
            };

            mainPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            loadingLabel = new Label();
            this.loadingLabel.Location = new Point(12, 250);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new Size(760, 50);
            this.loadingLabel.TabIndex = 7;
            this.loadingLabel.Text = "Chargement...";
            this.loadingLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.loadingLabel.Visible = false;

            ShowLoading(true);

            // Nombre d'échange par statut
            FlowLayoutPanel cardPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                FlowDirection = FlowDirection.LeftToRight,
                Width = 700,
                AutoSize = true,
                Padding = new Padding(10),
                WrapContents = true
            };

            FlowLayoutPanel cardPanel1 = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                FlowDirection = FlowDirection.LeftToRight,
                Width = 700,
                AutoSize = true,
                Padding = new Padding(10),
                WrapContents = false
            };

            FlowLayoutPanel cardPanel2 = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                FlowDirection = FlowDirection.LeftToRight,
                Width = 700,
                AutoSize = true,
                Padding = new Padding(10),
                WrapContents = false
            };

            lblOngoingExchanges = new Label();
            Panel ongoingExchangesPanel = CreateCard("Échanges en cours", ColorTranslator.FromHtml("#bc8246"), lblOngoingExchanges);
            lblAcceptedExchanges = new Label();
            Panel acceptedExchangesPanel = CreateCard("Échanges validés", ColorTranslator.FromHtml("#8a8f6a"), lblAcceptedExchanges);
            lblRefusedExchanges = new Label();
            Panel refusedExchangesPanel = CreateCard("Échanges refusés", Color.Red, lblRefusedExchanges);
            lblCancelledExchanges = new Label();
            Panel cancelledExchangesPanel = CreateCard("Échanges annulés", Color.Black, lblCancelledExchanges);
            cardPanel1.Controls.Add(ongoingExchangesPanel);
            cardPanel1.Controls.Add(acceptedExchangesPanel);
            cardPanel2.Controls.Add(refusedExchangesPanel);
            cardPanel2.Controls.Add(cancelledExchangesPanel);
            cardPanel.Controls.Add(cardPanel1);
            cardPanel.Controls.Add(cardPanel2);

            // Pie chart
            Panel cardCategory = new Panel
            {
                Dock = DockStyle.Fill,
                Width = 700,
                Height = 700, 
                Padding = new Padding(10),
                BackColor = Color.White
            };

            Label labelCategory = new Label();
            labelCategory.Name = "labelCategory";
            labelCategory.Text = "Nombre d'objet par catégorie";
            labelCategory.Font = new Font("Arial", 14, FontStyle.Bold);
            labelCategory.TextAlign = ContentAlignment.MiddleCenter;
            labelCategory.ForeColor = ColorTranslator.FromHtml("#bc8246");
            labelCategory.Size = new Size(cardCategory.Width, 50); 
            labelCategory.Location = new Point(0, 0);

            pieChart1 = new Chart
            {
                Size = new Size(cardCategory.Width, cardCategory.Height - labelCategory.Height - 25), 
                Location = new Point(10, labelCategory.Height  + 25), 
                Margin = new Padding(10),
            };

            pieChart1.Series.Clear();
            pieChart1.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea("MainArea");
            pieChart1.ChartAreas.Add(chartArea);
            chartArea.Position = new ElementPosition(0, 0, 100, 100); 
            cardCategory.Controls.Add(labelCategory);
            cardCategory.Controls.Add(pieChart1);

            // Column chart
            Panel cardExchange = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 700,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            Label labelExchange = new Label();
            labelExchange.Name = "labelExchange";
            labelExchange.Text = "Nombre d'échanges";
            labelExchange.Font = new Font("Arial", 14, FontStyle.Bold);
            labelExchange.ForeColor = ColorTranslator.FromHtml("#bc8246");
            labelExchange.Size = new Size(cardCategory.Width, 50);
            labelExchange.Location = new Point(0, 25);



            FlowLayoutPanel searchPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(20, 20, 0, 20),
                Padding = new Padding(30),
                WrapContents = false
            };


            DateTimePicker date1 = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 200,
                Margin = new Padding(0, 0, 10, 0)
            };

            DateTimePicker date2 = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 200,
                Margin = new Padding(0, 0, 10, 0)
            };

            ComboBox comboBox = new ComboBox
            {
                Width = 200,
                Margin = new Padding(0, 0, 10, 0)
            };

            comboBox.Items.Add("Statut (tous)");
            comboBox.Items.Add("Accepté");
            comboBox.Items.Add("Annulé");
            comboBox.Items.Add("En cours");
            comboBox.Items.Add("Refusé");

            comboBox.SelectedIndex = 0;

            Button searchButton = new Button
            {
                Text = "Rechercher",
                BackColor = ColorTranslator.FromHtml("#8a8f6a"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 250,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 10, 0)
            };

            searchPanel.Controls.Add(date1);
            searchPanel.Controls.Add(date2);
            searchPanel.Controls.Add(comboBox);
            searchPanel.Controls.Add(searchButton);

            searchButton.Click += async (sender, e) =>
            {
                string selectedItem = comboBox.SelectedItem.ToString();
                string statusValue = null;

                switch (selectedItem)
                {
                    case "Statut (tous)":
                        statusValue = null;
                        break;
                    case "Accepté":
                        statusValue = "Accepted";
                        break;
                    case "Annulé":
                        statusValue = "Cancelled";
                        break;
                    case "En cours":
                        statusValue = "Proposed";
                        break;
                    case "Refusé":
                        statusValue = "Refused";
                        break;
                    default:
                        statusValue = null;
                        break;
                }
                LoadDashboard(date1.Value != null ? date1.Value.ToString("yyyy-MM-dd") : null, date2.Value != null ? date2.Value.ToString("yyyy-MM-dd") : null, statusValue);
            };

            columnChart = new Chart
            {
                Dock = DockStyle.Bottom, 
                Size = new Size(cardCategory.Width*3, cardExchange.Height - searchPanel.Height - labelExchange.Height - 25),
                Location = new Point(10, labelExchange.Height + searchPanel.Height + 25),
                Margin = new Padding(10),
            };

            columnChart.Series.Clear();
            columnChart.ChartAreas.Clear();

            ChartArea chartArea1 = new ChartArea("chart2");
            columnChart.ChartAreas.Add(chartArea1);
            chartArea1.AxisX.LabelStyle.Angle = -45;
            chartArea1.Position = new ElementPosition(0, 5, 100, 90);
            cardExchange.Controls.Add(labelExchange);
            cardExchange.Controls.Add(searchPanel);
            cardExchange.Controls.Add(columnChart);


            // TOP user

            Panel cardTopUser = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 700,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            Label labelTopUser = new Label();
            labelTopUser.Name = "labelTopUser";
            labelTopUser.Text = "Les 10 Utilisateurs Ayant Réalisé le Plus d'Échanges";
            labelTopUser.Font = new Font("Arial", 14, FontStyle.Bold);
            labelTopUser.ForeColor = ColorTranslator.FromHtml("#bc8246");
            labelTopUser.Size = new Size(cardCategory.Width * 3, 50);
            labelTopUser.Location = new Point(0, 25);


            _dataGridView = new DataGridView
            {
                Dock = DockStyle.None,
                Location = new Point(0, 100),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoGenerateColumns = false,
                Margin = new Padding(20, 0, 0, 20),
                Width = cardCategory.Width * 2,
                Height = 520,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };


            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Numero", HeaderText = "#", DataPropertyName = "Numero" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Pseudo", HeaderText = "Pseudo", DataPropertyName = "Username" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Exchange", HeaderText = "Échanges réalisés", DataPropertyName = "ExchangeCount" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Pourcentage", HeaderText = "Pourcentage d'Échanges Réalisés", DataPropertyName = "Percentage" });

            cardTopUser.Controls.Add(labelTopUser);
            cardTopUser.Controls.Add(_dataGridView);

            mainPanel1.Controls.Add(cardPanel, 0, 0); 
            mainPanel1.Controls.Add(cardCategory, 1, 0);
            dash.Controls.Add(mainPanel1);
            dash.Controls.Add(cardExchange);
            dash.Controls.Add(cardTopUser);
            mainPanel.Controls.Add(dash);
            mainPanel.Controls.Add(loadingLabel);

            _panel.Controls.Add(mainPanel);
            LoadDashboard();
        }


        public async void LoadDashboard(string date1 = null, string date2 = null, string status =null)
        {
            try
            {
                ShowLoading(true);

                var dashboardData = await _dashboardService.GetDashboardAsync(date1, date2, status);

                if (dashboardData != null)
                {
                    lblOngoingExchanges.Text = dashboardData.OngoingExchanges.ToString();
                    lblAcceptedExchanges.Text = dashboardData.AcceptedExchanges.ToString();
                    lblRefusedExchanges.Text = dashboardData.RefusedExchanges.ToString();
                    lblCancelledExchanges.Text = dashboardData.CancelledExchanges.ToString();
                    // Nombre d'objet par cat
                    pieChart1.Series.Clear();
                    pieChart1.ChartAreas.Clear();

                    ChartArea chartArea = new ChartArea("MainArea");
                    pieChart1.ChartAreas.Add(chartArea);

                    Series series = new Series("Nombre des objets par catégorie")
                    {
                        ChartType = SeriesChartType.Pie 
                    };

                    foreach (var category in dashboardData.ObjectsByCategory)
                    {
                        if (int.Parse(category.ObjectCount) > 0)
                        {
                            series.Points.AddXY(category.Name + ": " + category.ObjectCount, category.ObjectCount);
                        }
                    }
                    pieChart1.Series.Add(series);

                    // Echange entre 2 dates
                    columnChart.Series.Clear();
                    columnChart.ChartAreas.Clear();

                    ChartArea chartArea1 = new ChartArea("MainArea");
                    columnChart.ChartAreas.Add(chartArea1);

                    Series series1 = new Series("dahsboard")
                    {
                        ChartType = SeriesChartType.Column
                    };

                    foreach (var category in dashboardData.ExchangesBetweenDates)
                    {
                        series1.Points.AddXY(category.Period, category.ExchangeCount);
                    }
                    series1.Color = ColorTranslator.FromHtml("#bc8246");
                    columnChart.Series.Add(series1);

                    //Top User
                    foreach (var (topUser, index) in dashboardData.ExchangesByUser.Select((user, i) => (user, i)))
                    {
                        _dataGridView.Rows.Add(index+1, topUser.Username, topUser.ExchangeCount, topUser.Percentage);
                    }
                    _dataGridView.CellFormatting += _dataGridView_CellFormatting;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du tableau de bord : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void _dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_dataGridView.Columns[e.ColumnIndex].Name == "Pourcentage")
            {
                if (double.TryParse(e.Value?.ToString(), out double percentage))
                {

                    e.Value = Math.Round(percentage, 2)  +" %";
                    if (percentage < 10)
                    {
                        e.CellStyle.ForeColor = Color.Red; 
                    }
                    else if (percentage <= 20)
                    {
                        e.CellStyle.ForeColor = ColorTranslator.FromHtml("#bc8246"); 
                    }
                    else if (percentage < 40)
                    {
                        e.CellStyle.ForeColor = Color.Blue;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = ColorTranslator.FromHtml("#8a8f6a");
                    }
                }
            }
        }


        private void ShowLoading(bool show)
        {
            loadingLabel.Visible = show;
            dash.Visible = !show;
        }

        private Panel CreateCard(string title, Color color, Label exchangeLabel)
        {
            Panel cardPanel = new Panel
            {
                Size = new Size(325, 250),
                BackColor = color,
                Margin = new Padding(10),
            };

            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
            };

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F)); 
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60F)); 

            Label titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            exchangeLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            exchangeLabel.ForeColor = Color.White;
            exchangeLabel.TextAlign = ContentAlignment.MiddleCenter;
            exchangeLabel.Dock = DockStyle.Fill;

            layoutPanel.Controls.Add(titleLabel, 0, 0);
            layoutPanel.Controls.Add(exchangeLabel, 0, 1);

            cardPanel.Controls.Add(layoutPanel);
            return cardPanel;
        }
    }
}