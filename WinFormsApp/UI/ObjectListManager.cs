using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Services;

namespace WinFormsApp.UI
{
    public class ObjectListManager
    {
        private readonly Panel _panel;
        private readonly DataGridView _dataGridView;
        private readonly ObjectService _objectService;

        public ObjectListManager(Panel panel)
        {
            _panel = panel;

            _panel.Padding = new Padding(80);

            var titleLabel = new Label
            {
                Text = "Liste des Objets",
                Font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Height = 50,
                ForeColor = System.Drawing.Color.Black,
                Padding = new Padding(0, 10, 0, 10)
            };
           
            Panel innerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 80, 20, 20)
            };

            _dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoGenerateColumns = false
            };

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "Id" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nom", DataPropertyName = "Name" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", DataPropertyName = "Description" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Image", DataPropertyName = "Image" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Créé le", DataPropertyName = "CreatedAt" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mis à jour le", DataPropertyName = "UpdatedAt" });
            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Statut", DataPropertyName = "Status" });

            innerPanel.Controls.Add(_dataGridView);
            _panel.Controls.Add(innerPanel);

            _objectService = new ObjectService();
        }

        public async Task LoadObjectsAsync(int page = 1, int limit = 10)
        {
            try
            {
                List<Models.Object> objects = await _objectService.GetObjectsAsync(page, limit);

                if (objects == null || objects.Count == 0)
                {
                    MessageBox.Show("Aucun objet trouvé.");
                }
                else
                {
                    _dataGridView.DataSource = objects;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du chargement des objets : {ex.Message}");
            }
        }
    }

}
