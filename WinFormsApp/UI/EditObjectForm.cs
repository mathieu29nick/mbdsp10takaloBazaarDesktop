using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Services;

namespace WinFormsApp.UI
{
    public class EditObjectForm : Form
    {
        private readonly ObjectService _objectService;
        private readonly CategoryService _categoryService;
        private int _objectId;
        private Models.Object _currentObject;

        private TextBox nameTextBox;
        private TextBox descriptionTextBox;
        private ComboBox categoryComboBox;
        private Button uploadImageButton;
        private string uploadedImageUrl = null;
        private PictureBox imageBox;

        public EditObjectForm(ObjectService objectService, CategoryService categoryService, int objectId)
        {
            _objectService = objectService;
            _categoryService = categoryService;
            _objectId = objectId;

            InitializeComponents();
            LoadObjectDataAsync().ConfigureAwait(false);
        }

        private void InitializeComponents()
        {
            this.Text = "Modifier un Objet";
            this.Size = new Size(600, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label titleLabel = new Label
            {
                Text = "Modification d'un objet",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                Location = new Point(150, 20),
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

            Label imageLabel = new Label { Text = "Image:", Location = new Point(20, 180), AutoSize = true };
            uploadImageButton = new Button { Text = "Télécharger l'image", Location = new Point(150, 180), Width = 400, Height = 50 };
            uploadImageButton.Click += UploadImageButton_Click;

            imageBox = new PictureBox
            {
                Location = new Point(150, 250),
                Size = new Size(200, 200),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            Button submitButton = new Button
            {
                Text = "Modifier",
                Location = new Point(150, 480),
                Width = 150,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#8a8f6a"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            submitButton.Click += async (sender, e) => await UpdateObjectAsync();

            this.Controls.Add(titleLabel);
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(descriptionTextBox);
            this.Controls.Add(categoryLabel);
            this.Controls.Add(categoryComboBox);
            this.Controls.Add(imageLabel);
            this.Controls.Add(uploadImageButton);
            this.Controls.Add(imageBox);
            this.Controls.Add(submitButton);
        }

        private async Task PopulateCategoryComboBoxAsync()
        {
            try
            {
                var categoriesResponse = await _categoryService.GetCategoriesAsync(1, 1000, "");
                var categories = categoriesResponse.Data.Categories;

                if (categories != null && categories.Count > 0)
                {
                    categoryComboBox.DataSource = categories;
                    categoryComboBox.DisplayMember = "Name";
                    categoryComboBox.ValueMember = "Id";
                }
                else
                {
                    MessageBox.Show("Aucune catégorie trouvée.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des catégories : {ex.Message}");
            }
        }

        private async Task LoadObjectDataAsync()
        {
            try
            {
                _currentObject = await _objectService.GetObjectByIdAsync(_objectId);

                if (_currentObject != null)
                {
                    await PopulateCategoryComboBoxAsync();
                    nameTextBox.Text = _currentObject.Name;
                    descriptionTextBox.Text = _currentObject.Description;
                    categoryComboBox.SelectedValue = _currentObject.Category.Id;

                    uploadedImageUrl = await ConvertImageToBase64Async(_currentObject.Image);
                    uploadImageButton.Text = "Image téléchargée";

                    if (!string.IsNullOrEmpty(uploadedImageUrl))
                    {
                        if (Uri.IsWellFormedUriString(_currentObject.Image, UriKind.Absolute))
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                byte[] imageBytes = await client.GetByteArrayAsync(_currentObject.Image);
                                using (var ms = new MemoryStream(imageBytes))
                                {
                                    imageBox.Image = Image.FromStream(ms);
                                }
                            }
                        }
                        else if (File.Exists(_currentObject.Image))
                        {
                            imageBox.Image = Image.FromFile(_currentObject.Image);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement de l'objet.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de l'objet : {ex.Message}");
                this.Close();
            }
        }

        private async Task<string> ConvertImageToBase64Async(string imageUrlOrPath)
        {
            try
            {
                if (File.Exists(imageUrlOrPath))
                {
                    return ConvertImageToBase64(imageUrlOrPath);
                }
                else
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] imageBytes = await client.GetByteArrayAsync(imageUrlOrPath);
                        string base64String = Convert.ToBase64String(imageBytes);
                        string fileExtension = Path.GetExtension(imageUrlOrPath).ToLower();

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la conversion de l'image : {ex.Message}");
                return null;
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

        private void UploadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    uploadedImageUrl = ConvertImageToBase64(openFileDialog.FileName);
                    uploadImageButton.Text = "Image téléchargée";
                    imageBox.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private async Task UpdateObjectAsync()
        {
            try
            {
                var updatedObject = new Models.Object
                {
                    Name = nameTextBox.Text,
                    Description = descriptionTextBox.Text,
                    CategoryId = (int)categoryComboBox.SelectedValue,
                    Image = uploadedImageUrl
                };

                bool success = await _objectService.UpdateObjectAsync(_objectId, updatedObject);
                if (success)
                {
                    MessageBox.Show("Objet modifié avec succès.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de la modification de l'objet.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification de l'objet : {ex.Message}");
            }
        }
    }
}
