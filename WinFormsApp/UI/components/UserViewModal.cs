using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp.Models;

namespace WinFormsApp.UI
{
    public partial class UserViewModal : Form
    {
        private readonly User _user;

        public UserViewModal(User user)
        {
            _user = user;
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            lblUsernameValue.Text = _user.Username;
            lblEmailValue.Text = _user.Email;
            lblFirstNameValue.Text = _user.FirstName;
            lblLastNameValue.Text = _user.LastName;
            lblGenderValue.Text = _user.Gender == "Male" ? "Masculin" : "Féminin";
            lblStatusValue.Text = _user.Status;
            lblCreatedAtValue.Text = _user.CreatedAt.ToString("dd/MM/yyyy");
            lblUpdatedAtValue.Text = _user.UpdatedAt.ToString("dd/MM/yyyy");

            // Display profile picture if available
            if (Uri.IsWellFormedUriString(_user.ProfilePicture, UriKind.Absolute))
            {
                pbProfilePicture.Load(_user.ProfilePicture);
            }
            else if (!string.IsNullOrEmpty(_user.ProfilePicture))
            {
                pbProfilePicture.Image = ConvertBase64ToImage(_user.ProfilePicture);
            }
        }

        private Image ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblUsername = new Label();
            this.lblUsernameValue = new Label();
            this.lblEmail = new Label();
            this.lblEmailValue = new Label();
            this.lblFirstName = new Label();
            this.lblFirstNameValue = new Label();
            this.lblLastName = new Label();
            this.lblLastNameValue = new Label();
            this.lblGender = new Label();
            this.lblGenderValue = new Label();
            this.lblStatus = new Label();
            this.lblStatusValue = new Label();
            this.lblCreatedAt = new Label();
            this.lblCreatedAtValue = new Label();
            this.lblUpdatedAt = new Label();
            this.lblUpdatedAtValue = new Label();
            this.lblProfilePicture = new Label();
            this.pbProfilePicture = new PictureBox();
            this.btnClose = new Button();

            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Détails de l'utilisateur";
            this.lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            this.lblTitle.Location = new Point(10, 10);
            this.lblTitle.Size = new Size(500, 50);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Labels and Values
            SetupLabel(lblUsername, "Nom d'utilisateur:", new Point(20, 70));
            SetupValueLabel(lblUsernameValue, new Point(260, 70));

            SetupLabel(lblEmail, "Email:", new Point(20, 110));
            SetupValueLabel(lblEmailValue, new Point(260, 110));

            SetupLabel(lblFirstName, "Prénom:", new Point(20, 150));
            SetupValueLabel(lblFirstNameValue, new Point(260, 150));

            SetupLabel(lblLastName, "Nom:", new Point(20, 190));
            SetupValueLabel(lblLastNameValue, new Point(260, 190));

            SetupLabel(lblGender, "Genre:", new Point(20, 230));
            SetupValueLabel(lblGenderValue, new Point(260, 230));

            SetupLabel(lblStatus, "Statut:", new Point(20, 270));
            SetupValueLabel(lblStatusValue, new Point(260, 270));

            SetupLabel(lblCreatedAt, "Créé le:", new Point(20, 310));
            SetupValueLabel(lblCreatedAtValue, new Point(260, 310));

            SetupLabel(lblUpdatedAt, "Mis à jour le:", new Point(20, 350));
            SetupValueLabel(lblUpdatedAtValue, new Point(260, 350));

            // Profile picture
            this.lblProfilePicture.Text = "Photo de profil:";
            this.lblProfilePicture.Location = new Point(20, 390);
            this.lblProfilePicture.Size = new Size(240, 30);
            this.lblProfilePicture.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            this.pbProfilePicture.Location = new Point(260, 390);
            this.pbProfilePicture.Name = "pbProfilePicture";
            this.pbProfilePicture.Size = new Size(150, 150);
            this.pbProfilePicture.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbProfilePicture.BorderStyle = BorderStyle.FixedSingle;

            // Close button
            this.btnClose.Location = new Point(200, 560);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(150, 40);
            this.btnClose.Text = "Fermer";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // 
            // UserViewModal
            // 
            this.ClientSize = new Size(550, 640);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblUsernameValue);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblEmailValue);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lblFirstNameValue);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblLastNameValue);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.lblGenderValue);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblStatusValue);
            this.Controls.Add(this.lblCreatedAt);
            this.Controls.Add(this.lblCreatedAtValue);
            this.Controls.Add(this.lblUpdatedAt);
            this.Controls.Add(this.lblUpdatedAtValue);
            this.Controls.Add(this.lblProfilePicture);
            this.Controls.Add(this.pbProfilePicture);
            this.Controls.Add(this.btnClose);
            this.Name = "UserViewModal";
            this.Text = "Détails de l'utilisateur";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void SetupLabel(Label label, string text, Point location)
        {
            label.Text = text;
            label.Location = location;
            label.Size = new Size(240, 30);
            label.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.Controls.Add(label);
        }

        private void SetupValueLabel(Label label, Point location)
        {
            label.Location = location;
            label.Size = new Size(240, 30);
            label.Font = new Font("Segoe UI", 12);
            label.ForeColor = Color.Black;
            this.Controls.Add(label);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Label lblTitle;
        private Label lblUsername;
        private Label lblUsernameValue;
        private Label lblEmail;
        private Label lblEmailValue;
        private Label lblFirstName;
        private Label lblFirstNameValue;
        private Label lblLastName;
        private Label lblLastNameValue;
        private Label lblGender;
        private Label lblGenderValue;
        private Label lblStatus;
        private Label lblStatusValue;
        private Label lblCreatedAt;
        private Label lblCreatedAtValue;
        private Label lblUpdatedAt;
        private Label lblUpdatedAtValue;
        private Label lblProfilePicture;
        private PictureBox pbProfilePicture;
        private Button btnClose;
    }
}
