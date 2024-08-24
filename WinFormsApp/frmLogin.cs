using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WinFormsApp.Services;

namespace WinFormsApp
{
    public partial class frmLogin : Form
    {
        private readonly AuthenticationService _authService;

        public frmLogin()
        {
            InitializeComponent();
            var interceptor = new Interceptor(new HttpClientHandler());
            _authService = new AuthenticationService(interceptor);

            // Charger l'image de fond à partir des ressources intégrées
            try
            {
                pictureBox1.BackgroundImage = WinFormsApp.Properties.Resources.takalo; 
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de l'image: {ex.Message}");
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var username = textBox1.Text;
            var password = textBox2.Text;

            // Vérifier si les champs utilisateur et mot de passe sont vides
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Le champ du nom d'utilisateur ne peut pas être vide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Le champ du mot de passe ne peut pas être vide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Afficher le ProgressBar lors du début de la connexion
            progressBarLoading.Visible = true;

            try
            {
                var authenticated = await _authService.Login(username, password);

                if (authenticated)
                {
                    MessageBox.Show("Connexion réussie !");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Masquer le ProgressBar après la fin de la tentative de connexion
                progressBarLoading.Visible = false;
            }
        }
    }
}