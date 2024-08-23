using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Services;

namespace WinFormsApp.UI
{
    public partial class UserModal : Form
    {
        private readonly int? _userId;
        private readonly UserService _userService;
        private string _base64ProfilePicture;
        private bool _isNewPictureSelected = false;

        public UserModal(int? userId = null)
        {
            _userId = userId;
            _userService = new UserService();
            InitializeComponent();

            if (_userId.HasValue)
            {
                LoadUserData();
            }
        }

        private async void LoadUserData()
        {
            var user = await _userService.GetUserByIdAsync(_userId.Value);

            if (user != null)
            {
                tbUsername.Text = user.Username;
                tbEmail.Text = user.Email;
                tbFirstName.Text = user.FirstName;
                tbLastName.Text = user.LastName;
                cbGender.SelectedItem = user.Gender == "Male" ? "Masculin" : "Féminin";

                // If the profile picture is a URL, display it but don't convert to base64
                if (Uri.IsWellFormedUriString(user.ProfilePicture, UriKind.Absolute))
                {
                    pbProfilePicture.Load(user.ProfilePicture);
                    _base64ProfilePicture = user.ProfilePicture;  // Use the URL if no new picture is selected
                }
                else if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    pbProfilePicture.Image = ConvertBase64ToImage(user.ProfilePicture);
                    _base64ProfilePicture = user.ProfilePicture;
                }
            }
            else
            {
                MessageBox.Show("Impossible de charger les données utilisateur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            var user = new User
            {
                Id = _userId ?? 0,
                Username = tbUsername.Text.Trim(),
                Email = tbEmail.Text.Trim(),
                FirstName = tbFirstName.Text.Trim(),
                LastName = tbLastName.Text.Trim(),
                Gender = cbGender.SelectedItem?.ToString() == "Masculin" ? "Male" : "Female"
            };

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email))
            {
                MessageBox.Show("Le nom d'utilisateur et l'email sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_userId.HasValue)
                {
                    Boolean success = await _userService.UpdateUserAsync(user, _isNewPictureSelected ? _base64ProfilePicture : null);
                    if (success)
                    {
                        MessageBox.Show("Utilisateur mis à jour avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement de l'utilisateur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChoosePicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbProfilePicture.Image = Image.FromFile(ofd.FileName);
                    _base64ProfilePicture = ConvertImageToBase64(pbProfilePicture.Image);
                    _isNewPictureSelected = true;
                }
            }
        }

        private string ConvertImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                string mimeType;
                if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    mimeType = "image/jpeg";
                }
                else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    mimeType = "image/png";
                }
                else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    mimeType = "image/gif";
                }
                else
                {
                    throw new NotSupportedException("Unsupported image format");
                }

                return $"data:{mimeType};base64,{Convert.ToBase64String(ms.ToArray())}";
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
            this.tbUsername = new TextBox();
            this.lblEmail = new Label();
            this.tbEmail = new TextBox();
            this.lblFirstName = new Label();
            this.tbFirstName = new TextBox();
            this.lblLastName = new Label();
            this.tbLastName = new TextBox();
            this.lblGender = new Label();
            this.cbGender = new ComboBox();
            this.lblProfilePicture = new Label();
            this.pbProfilePicture = new PictureBox();
            this.btnChoosePicture = new Button();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.Text = _userId.HasValue ? "Modifier l'utilisateur" : "Créer un utilisateur";
            this.lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitle.Location = new Point(10, 10);
            this.lblTitle.Size = new Size(300, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblUsername
            // 
            this.lblUsername.Text = "Nom d'utilisateur:";
            this.lblUsername.Location = new Point(20, 60);
            this.lblUsername.Size = new Size(260, 25);
            this.lblUsername.Font = new Font("Segoe UI", 10);

            // 
            // tbUsername
            // 
            this.tbUsername.Location = new Point(20, 90);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new Size(260, 30);
            this.tbUsername.Font = new Font("Segoe UI", 12);

            // 
            // lblEmail
            // 
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new Point(20, 130);
            this.lblEmail.Size = new Size(120, 25);
            this.lblEmail.Font = new Font("Segoe UI", 10);

            // 
            // tbEmail
            // 
            this.tbEmail.Location = new Point(20, 160);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new Size(260, 30);
            this.tbEmail.Font = new Font("Segoe UI", 12);

            // 
            // lblFirstName
            // 
            this.lblFirstName.Text = "Prénom:";
            this.lblFirstName.Location = new Point(20, 200);
            this.lblFirstName.Size = new Size(120, 25);
            this.lblFirstName.Font = new Font("Segoe UI", 10);

            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new Point(20, 230);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new Size(260, 30);
            this.tbFirstName.Font = new Font("Segoe UI", 12);

            // 
            // lblLastName
            // 
            this.lblLastName.Text = "Nom:";
            this.lblLastName.Location = new Point(20, 270);
            this.lblLastName.Size = new Size(120, 25);
            this.lblLastName.Font = new Font("Segoe UI", 10);

            // 
            // tbLastName
            // 
            this.tbLastName.Location = new Point(20, 300);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new Size(260, 30);
            this.tbLastName.Font = new Font("Segoe UI", 12);

            // 
            // lblGender
            // 
            this.lblGender.Text = "Genre:";
            this.lblGender.Location = new Point(20, 340);
            this.lblGender.Size = new Size(120, 25);
            this.lblGender.Font = new Font("Segoe UI", 10);

            // 
            // cbGender
            // 
            this.cbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbGender.Items.AddRange(new object[] { "Masculin", "Féminin" });
            this.cbGender.Location = new Point(20, 370);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new Size(260, 30);
            this.cbGender.Font = new Font("Segoe UI", 12);

            // 
            // lblProfilePicture
            // 
            this.lblProfilePicture.Text = "Photo de profil:";
            this.lblProfilePicture.Location = new Point(20, 410);
            this.lblProfilePicture.Size = new Size(260, 25);
            this.lblProfilePicture.Font = new Font("Segoe UI", 10);

            // 
            // pbProfilePicture
            // 
            this.pbProfilePicture.Location = new Point(20, 440);
            this.pbProfilePicture.Name = "pbProfilePicture";
            this.pbProfilePicture.Size = new Size(100, 100);
            this.pbProfilePicture.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbProfilePicture.BorderStyle = BorderStyle.FixedSingle;

            // 
            // btnChoosePicture
            // 
            this.btnChoosePicture.Location = new Point(130, 480);
            this.btnChoosePicture.Name = "btnChoosePicture";
            this.btnChoosePicture.Size = new Size(150, 30);
            this.btnChoosePicture.Text = "Choisir une image...";
            this.btnChoosePicture.UseVisualStyleBackColor = true;
            this.btnChoosePicture.Click += new EventHandler(this.btnChoosePicture_Click);

            // 
            // btnSave
            this.btnSave.Location = new Point(20, 570);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(130, 40);
            this.btnSave.Text = "Enregistrer";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = Color.FromArgb(188, 130, 70);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new Point(160, 570);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(130, 40);
            this.btnCancel.Text = "Annuler";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = Color.FromArgb(56, 56, 56);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);


            // 
            // UserModal
            // 
            this.ClientSize = new Size(300, 640);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.tbFirstName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.cbGender);
            this.Controls.Add(this.lblProfilePicture);
            this.Controls.Add(this.pbProfilePicture);
            this.Controls.Add(this.btnChoosePicture);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "UserModal";
            this.Text = _userId.HasValue ? "Modifier l'utilisateur" : "Créer un utilisateur";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private Label lblUsername;
        private TextBox tbUsername;
        private Label lblEmail;
        private TextBox tbEmail;
        private Label lblFirstName;
        private TextBox tbFirstName;
        private Label lblLastName;
        private TextBox tbLastName;
        private Label lblGender;
        private ComboBox cbGender;
        private Label lblProfilePicture;
        private PictureBox pbProfilePicture;
        private Button btnChoosePicture;
        private Button btnSave;
        private Button btnCancel;
    }
}
