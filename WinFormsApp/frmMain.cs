using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using WinFormsApp.Helpers;
using WinFormsApp.Services;

namespace WinFormsApp
{
    public partial class frmMain : Form
    {
        private readonly AuthenticationService _authService;

        public frmMain()
        {
            InitializeComponent();
            var interceptor = new Interceptor(new HttpClientHandler());
            _authService = new AuthenticationService(interceptor);
        }

        // Méthode pour gérer le clic sur le bouton de déconnexion
        private async void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // Appeler le service pour effectuer la déconnexion
                await _authService.Logout();
                MessageBox.Show("Vous avez été déconnecté.");

                // Après la déconnexion, revenir à l'écran de connexion
                var loginForm = new frmLogin();
                loginForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la déconnexion : {ex.Message}");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            PlaceHolder.SetPlaceHolder(tbSearch.Handle, "Search");
        }

        private void btnMenuBar_Click(object sender, EventArgs e)
        {
            toggleLeftPane();
        }


        private void toggleLeftPane()
        {
            bool shrink = (pnlLeft.Width == 350) ? true : false;

            foreach (Control c in pnlLeft.Controls)
            {
                if (c is Button btn)
                {
                    btn.Text = shrink ? string.Empty : btn.Tag!.ToString();
                }
            }

            btnLogoSmall.ImageAlign = shrink ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft;
            btnLogoSmall.Text = shrink ? string.Empty : btnLogoSmall.Tag!.ToString();
            btnLogoSmall.Padding = shrink ? new Padding(0) : new Padding(20, 0, 0, 0);
            pnlLeft.Width = shrink ? 100 : 350;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            findButton(tbSearch.Text);
        }

        private void findButton(string btnlabel)
        {
            bool emptyText = string.IsNullOrWhiteSpace(btnlabel);
            foreach (Control c in pnlLeft.Controls)
            {
                if (c is Button btn)
                {
                    if (emptyText)
                        btn.Visible = true;
                    else
                    {
                        btn.Visible = (btn.Text.Contains(btnlabel, StringComparison.OrdinalIgnoreCase));
                    }
                }

            }
        }


    }
}
