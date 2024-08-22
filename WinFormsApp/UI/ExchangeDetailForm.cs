using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using WinFormsApp.Models;
using WinFormsApp.Services;
using WinFormsApp.Models.ApiResponse;

namespace WinFormsApp.UI
{
    public class ExchangeDetailForm : Form
    {
        private readonly ExchangeService _exchangeService;
        private readonly int _exchangeId;
        private ExchangeDetailResponse _currentExchange;
        private Label proposerLabel;
        private Label receiverLabel;
        private Label createdAtLabel;
        private Label statusLabel;
        private Label noteLabel;
        private Label appointmentDateLabel;
        private Label meetingPlaceLabel;
        private DataGridView objectsDataGridView;

        public ExchangeDetailForm(int exchangeId)
        {
            _exchangeService = new ExchangeService();
            _exchangeId = exchangeId;

            InitializeComponents();
            LoadExchangeDetailsAsync().ConfigureAwait(false);
        }

        private string TranslateStatus(string status)
        {
            return status switch
            {
                "Proposed" => "Proposé",
                "Accepted" => "Accepté",
                "Refused" => "Refusé",
                "Cancelled" => "Annulé",
                _ => "Inconnu",
            };
        }

        private void InitializeComponents()
        {
            this.Text = "Détail de l'Échange";
            this.Size = new Size(1010, 920);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label titleLabel = new Label
            {
                Text = "Détail de l'échange",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                Location = new Point(110, 10),
                Margin = new Padding(0, 0, 20, 0),
                ForeColor = ColorTranslator.FromHtml("#bc8246"),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            proposerLabel = new Label { Text = "Proposeur :", Location = new Point(220, 60), AutoSize = true };
            receiverLabel = new Label { Text = "Receveur :", Location = new Point(220, 100), AutoSize = true };
            statusLabel = new Label { Text = "Statut :", Location = new Point(220, 140), AutoSize = true };
            noteLabel = new Label { Text = "Note :", Location = new Point(580, 60), AutoSize = true };
            appointmentDateLabel = new Label { Text = "Date du rendez-vous :", Location = new Point(580, 100), AutoSize = true };
            meetingPlaceLabel = new Label { Text = "Lieu de rencontre :", Location = new Point(580, 140), AutoSize = true };

            objectsDataGridView = new DataGridView
            {
                Location = new Point(20, 250),
                Size = new Size(950, 500),
                AutoGenerateColumns = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            objectsDataGridView.RowTemplate.Height = 150;
            objectsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID de l'objet", DataPropertyName = "ObjectId", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            objectsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Libellé", DataPropertyName = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            objectsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", DataPropertyName = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            objectsDataGridView.Columns.Add(new DataGridViewImageColumn { HeaderText = "Image", DataPropertyName = "Image", Width =150, ImageLayout = DataGridViewImageCellLayout.Stretch });
            objectsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Propriétaire", DataPropertyName = "Owner", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            this.Controls.Add(titleLabel);
            this.Controls.Add(proposerLabel);
            this.Controls.Add(receiverLabel);
            this.Controls.Add(statusLabel);
            this.Controls.Add(noteLabel);
            this.Controls.Add(appointmentDateLabel);
            this.Controls.Add(meetingPlaceLabel);
            this.Controls.Add(objectsDataGridView);
        }

        private async Task LoadExchangeDetailsAsync()
        {
            _currentExchange = await _exchangeService.GetExchangeByIdAsync(_exchangeId);
            if (_currentExchange != null)
            {
                proposerLabel.Text += $" {_currentExchange.Proposer?.FirstName} {_currentExchange.Proposer?.LastName}";
                receiverLabel.Text += $" {_currentExchange.Receiver?.FirstName} {_currentExchange.Receiver?.LastName}";
                statusLabel.Text += $" {TranslateStatus(_currentExchange.Status)}";
                noteLabel.Text += $" {(string.IsNullOrEmpty(_currentExchange.Note) ? "N/A" : _currentExchange.Note)}";
                appointmentDateLabel.Text += $" {(_currentExchange.Appointment_Date?.ToString("dd-MM-yyyy HH:mm:ss") ?? "N/A")}";
                meetingPlaceLabel.Text += $" {(string.IsNullOrEmpty(_currentExchange.Meeting_Place) ? "N/A" : _currentExchange.Meeting_Place)}";

                var objectList = _currentExchange.Exchange_Objects.Select(eo => new
                {
                    ObjectId = eo.Object.Id,
                    Name= eo.Object.Name,
                    Description = eo.Object.Description,
                    Image = LoadImageFromUrl(eo.Object.Image),
                    Owner = eo.User_Id == _currentExchange.Proposer.Id ? "Proposeur" : "Receveur"
                }).ToList();
    

                objectsDataGridView.DataSource = objectList;
            }
            else
            {
                MessageBox.Show("Erreur lors du chargement des détails de l'échange.");
                this.Close();
            }
        }

        private Image LoadImageFromUrl(string url)
        {
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(url);
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
