using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var httpClient = new HttpClient(new TokenHandler(new SessionService()));
            _authService = new AuthenticationService(httpClient, new SessionService());
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var username = textBox1.Text;
            var password = textBox2.Text;

            try
            {
                var authenticated = await _authService.Login(username, password);

                if (authenticated)
                {
                    MessageBox.Show("Login successful!");
                    // You can either close the form or redirect to another form
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Unexpected error. Please contact support if this persists.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}");
            }
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            await _authService.Logout();
            MessageBox.Show("You have been logged out.");
            textBox1.Clear();
            textBox2.Clear();
            this.Show();
        }
    }
}
