using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Services;

namespace WinFormsApp.UI.components
{
    public partial class CategoryModal : Form
    {
        private int? _categoryId;

        public CategoryModal(int? categoryId = null, string categoryName = "")
        {
            InitializeComponent();
            _categoryId = categoryId;

            if (_categoryId.HasValue)
            {
                this.Text = "Modifier Catégorie";
                tbCategoryName.Text = categoryName;
                btnSave.Text = "Mettre à jour";
            }
            else
            {
                this.Text = "Ajouter Catégorie";
                btnSave.Text = "Ajouter";
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var categoryService = new CategoryService();
            var categoryName = tbCategoryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Le nom est requis.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (_categoryId.HasValue)
                {
                    var category = new Category { Id = _categoryId.Value, Name = categoryName };
                    await categoryService.UpdateCategoryAsync(category);
                    MessageBox.Show("Catégorie mise à jour avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var category = new Category { Name = categoryName };
                    await categoryService.SaveCategoryAsync(category);
                    MessageBox.Show("Catégorie créée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Close(); // Close the modal after save
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.lblCategoryName = new Label();
            this.tbCategoryName = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            // 
            // lblCategoryName
            // 
            this.lblCategoryName.AutoSize = true;
            this.lblCategoryName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCategoryName.Location = new Point(25, 30);
            this.lblCategoryName.Name = "lblCategoryName";
            this.lblCategoryName.Size = new Size(132, 21);
            this.lblCategoryName.TabIndex = 0;
            this.lblCategoryName.Text = "Nom de Catégorie";

            // 
            // tbCategoryName
            // 
            this.tbCategoryName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.tbCategoryName.Location = new Point(25, 60);
            this.tbCategoryName.Name = "tbCategoryName";
            this.tbCategoryName.Size = new Size(350, 29);
            this.tbCategoryName.TabIndex = 1;

            // 
            // btnSave
            // 
            this.btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnSave.Location = new Point(25, 110);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(150, 40);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.BackColor = ColorTranslator.FromHtml("#bc8246"); // Submit button background color
            this.btnSave.ForeColor = Color.White; // White font color
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // 
            // btnCancel
            // 
            this.btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnCancel.Location = new Point(225, 110);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(150, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Annuler";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.BackColor = ColorTranslator.FromHtml("#383838"); // Cancel button background color
            this.btnCancel.ForeColor = Color.White; // White font color
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Click += (s, e) => this.Close();

            // 
            // CategoryModal
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 180);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbCategoryName);
            this.Controls.Add(this.lblCategoryName);
            this.Name = "CategoryModal";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "CategoryModal";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblCategoryName;
        private TextBox tbCategoryName;
        private Button btnSave;
        private Button btnCancel;
    }
}
