using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp.Models;

namespace WinFormsApp.UI
{
    public class ObjectDetailForm : Form
    {
        private Models.Object _object;

        public ObjectDetailForm(Models.Object obj)
        {
            _object = obj;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Détails de l'Objet";
            this.Size = new Size(600, 850);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label titleLabel = new Label
            {
                Text = "Détail d'un objet",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                Location = new Point(150, 60),
                Margin = new Padding(0, 0, 20, 0),
                ForeColor = ColorTranslator.FromHtml("#bc8246"),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            Label nameLabel = new Label { Text = "Nom :", Location = new Point(20, 60), AutoSize = true };
            Label nameValueLabel = new Label { Text = _object.Name, Location = new Point(150, 60), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };

            Label descriptionLabel = new Label { Text = "Description :", Location = new Point(20, 100), AutoSize = true };
            Label descriptionValueLabel = new Label { Text = _object.Description, Location = new Point(150, 100), AutoSize = true };

            Label categoryLabel = new Label { Text = "Catégorie :", Location = new Point(20, 140), AutoSize = true };
            Label categoryValueLabel = new Label { Text = _object.Category?.Name ?? "Non spécifié", Location = new Point(150, 140), AutoSize = true };

            Label statusLabel = new Label { Text = "Statut :", Location = new Point(20, 180), AutoSize = true };
            string statusText;
            switch (_object.Status)
            {
                case "Available":
                    statusText = "Disponible";
                    break;
                case "Removed":
                    statusText = "Retiré";
                    break;
                case "Deleted":
                    statusText = "Supprimé";
                    break;
                default:
                    statusText = "Inconnu";
                    break;
            }
            Label statusValueLabel = new Label { Text = statusText, Location = new Point(150, 180), AutoSize = true };


            Label userLabel = new Label { Text = "Crée par :", Location = new Point(20, 220), AutoSize = true };
            Label userValueLabel = new Label { Text = _object.User.Email, Location = new Point(150, 220), AutoSize = true };

            Label dateLabel = new Label { Text = "Crée le :", Location = new Point(20, 260), AutoSize = true };
            Label dateValueLabel = new Label { Text = _object.Created_At.ToString("dd MMMM yyyy"), Location = new Point(150, 260), AutoSize = true };

            PictureBox imageBox = new PictureBox
            {
                Location = new Point(130, 300),
                Size = new Size(400, 400),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            if (!string.IsNullOrEmpty(_object.Image))
            {
                imageBox.Load(_object.Image);
            }

            Button submitButton = new Button
            {
                Text = "Fermer",
                Location = new Point(150, 750),
                Width = 150,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#8a8f6a"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            submitButton.Click += async (sender, e) => closePage();


            this.Controls.Add(titleLabel);
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameValueLabel);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(descriptionValueLabel);
            this.Controls.Add(categoryLabel);
            this.Controls.Add(categoryValueLabel);
            this.Controls.Add(statusLabel);
            this.Controls.Add(statusValueLabel);
            this.Controls.Add(userLabel);
            this.Controls.Add(userValueLabel);
            this.Controls.Add(dateLabel);
            this.Controls.Add(dateValueLabel);
            this.Controls.Add(imageBox);
            this.Controls.Add(submitButton);
        }

        private void closePage()
        {
            this.Close();
        }

    }
}
