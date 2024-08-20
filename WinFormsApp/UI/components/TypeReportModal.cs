using System;
using System;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Services;
namespace WinFormsApp.UI.components
{
    public partial class TypeReportModal : Form
    {
        private int? _typeReportId;
        private Label lblCategoryName;
        private TextBox tbCategoryName;
        private Button btnSave;
        private Button btnCancel;
        public TypeReportModal(int? typeReportId = null, string categoryName = "")
        {
            InitializeComponent();
            _typeReportId = typeReportId;

            if (_typeReportId.HasValue)
            {
                this.Text = "Modifier type de signalement";
                tbCategoryName.Text = categoryName;
                btnSave.Text = "Mettre à jour";
            }
            else
            {
                this.Text = "Ajouter type de signalement";
                btnSave.Text = "Enregistrer";
            }
        }

        private void InitializeComponent()
        {
            this.lblCategoryName = new Label();
            this.tbCategoryName = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

          
            this.lblCategoryName.AutoSize = true;
            this.lblCategoryName.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCategoryName.Location = new Point(25, 30);
            this.lblCategoryName.Name = "lblTypeReportName";
            this.lblCategoryName.Size = new Size(132, 21);
            this.lblCategoryName.TabIndex = 0;
            this.lblCategoryName.Text = "Type du signalement";

         
            this.tbCategoryName.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.tbCategoryName.Location = new Point(25, 60);
            this.tbCategoryName.Name = "tbTypeReportyName";
            this.tbCategoryName.Size = new Size(350, 29);
            this.tbCategoryName.TabIndex = 1;

            // 
            // btnSave
            // 
            this.btnSave.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnSave.Location = new Point(225, 110);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(150, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Enregistrer";
            this.btnSave.BackColor = ColorTranslator.FromHtml("#bc8246");
            this.btnSave.ForeColor = Color.White;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

           
            this.btnCancel.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnCancel.Location =  new Point(25, 110);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(150, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Annuler";
            this.btnCancel.BackColor = Color.Black;
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += (s, e) => this.Close();

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 180);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbCategoryName);
            this.Controls.Add(this.lblCategoryName);
            this.Name = "TypeReportModal";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TypeReportModal";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var typeReportService = new TypeReportService();
            var categoryName = tbCategoryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Le nom est requis.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (_typeReportId.HasValue)
                {
                    var typeReport = new TypeReport { Id = _typeReportId.Value, Name = categoryName };
                    await typeReportService.UpdateTypeReportAsync(typeReport);
                    MessageBox.Show("Type de signalement mis à jour avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var typeReport = new TypeReport { Name = categoryName };
                    await typeReportService.SaveTypeReportAsync(typeReport);
                    MessageBox.Show("Type de signalement créé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
