using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Services;
using System;
using System.IO;

namespace WinFormsApp.UI
{
    public class AddObjectForm : Form
    {
        private readonly ObjectService _objectService;
        private readonly CategoryService _categoryService;
        private readonly UserService _userService;

        private TextBox nameTextBox;
        private TextBox descriptionTextBox;
        private ComboBox categoryComboBox;
        private ComboBox userComboBox;
        private Button uploadImageButton;
        private string uploadedImageUrl = null;

        public AddObjectForm(ObjectService objectService, CategoryService categoryService, UserService userService)
        {
            _objectService = objectService;
            _categoryService = categoryService;
            _userService = userService;

            InitializeComponents();
            PopulateCategoryComboBoxAsync().ConfigureAwait(false);
            PopulateUserComboBoxAsync().ConfigureAwait(false);
        }

        private void InitializeComponents()
        {
            this.Text = "Ajouter un Objet";
            this.Size = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label titleLabel = new Label
            {
                Text = "Création d'un objet",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                Location = new Point(150, 60),
                Margin = new Padding(0, 0, 20, 0),
                ForeColor = ColorTranslator.FromHtml("#bc8246"),
                TextAlign = ContentAlignment.MiddleCenter,
            };


            Label nameLabel = new Label { Text = "Nom:", Location = new Point(20, 60), AutoSize = true };
            nameTextBox = new TextBox { Location = new Point(150, 60), Width = 400 };

            Label descriptionLabel = new Label { Text = "Description:", Location = new Point(20, 100), AutoSize = true };
            descriptionTextBox = new TextBox { Location = new Point(150, 100), Width = 400 };

            Label categoryLabel = new Label { Text = "Catégorie:", Location = new Point(20, 140), AutoSize = true };
            categoryComboBox = new ComboBox { Location = new Point(150, 140), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };

            Label userLabel = new Label { Text = "Utilisateur:", Location = new Point(20, 180), AutoSize = true };
            userComboBox = new ComboBox { Location = new Point(150, 180), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };

            Label imageLabel = new Label { Text = "Image:", Location = new Point(20, 220), AutoSize = true };
            uploadImageButton = new Button { Text = "Télécharger l'image", Location = new Point(150, 220), Width = 400,Height = 50 };
            uploadImageButton.Click += UploadImageButton_Click;

            Button submitButton = new Button { 
                Text = "Ajouter",
                Location = new Point(150, 280),
                Width = 150,
                Height=30,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#8a8f6a"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            submitButton.Click += async (sender, e) => await AddObjectAsync();

            this.Controls.Add(titleLabel);
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(descriptionTextBox);
            this.Controls.Add(categoryLabel);
            this.Controls.Add(categoryComboBox);
            this.Controls.Add(userLabel);
            this.Controls.Add(userComboBox);
            this.Controls.Add(imageLabel);
            this.Controls.Add(uploadImageButton);
            this.Controls.Add(submitButton);
        }

        private async Task PopulateCategoryComboBoxAsync()
        {
            try
            {
                CategoriesResponse categoriesResponse = await _categoryService.GetCategoriesAsync(1, 1000, "");
                List<Category> categories = categoriesResponse.Data.Categories;

                categoryComboBox.DataSource = categories;
                categoryComboBox.DisplayMember = "Name";
                categoryComboBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des catégories: {ex.Message}");
            }
        }

        private async Task PopulateUserComboBoxAsync()
        {
            try
            {
                UserResponse userResponse = await _userService.GetUsersAsync(1, 100);
                List<User> users = userResponse.Users;

                userComboBox.DataSource = users;
                userComboBox.DisplayMember = "Username";
                userComboBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des utilisateurs: {ex.Message}");
            }
        }


        private void UploadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    uploadedImageUrl = openFileDialog.FileName;
                    uploadImageButton.Text = "Image téléchargée";
                }
            }
        }

        private async Task AddObjectAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(descriptionTextBox.Text) ||
                categoryComboBox.SelectedValue == null ||
                string.IsNullOrWhiteSpace(uploadedImageUrl))
                {
                    MessageBox.Show("Veuillez remplir tous les champs requis.");
                    return;
                }
                var newObject = new Models.Object
                {
                    Name = nameTextBox.Text,
                    Description = descriptionTextBox.Text,
                    CategoryId = (int)categoryComboBox.SelectedValue,
                    Image = ConvertImageToBase64(uploadedImageUrl),
                    UserId = (int)userComboBox.SelectedValue,
                    Status = "Available"
                };

                bool success = await _objectService.CreateObjectAsync(newObject);
                if (success)
                {
                    MessageBox.Show("Objet ajouté avec succès.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de l'ajout de l'objet.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de l'objet: {ex.Message}");
            }
        }

        public static string ConvertImageToBase64(string imagePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);
                string fileExtension = Path.GetExtension(imagePath).ToLower();

                string mimeType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".bmp" => "image/bmp",
                    _ => "application/octet-stream",
                };

                return $"data:{mimeType};base64,{base64String}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la conversion de l'image en Base64 : {ex.Message}");
            }
        }




    }
}
