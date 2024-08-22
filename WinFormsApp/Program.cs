namespace WinFormsApp
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            AllocConsole();

            // Afficher le formulaire de connexion
            using (var loginForm = new frmLogin())
            {
                var result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Si la connexion est r�ussie, lancer le formulaire principal
                    Application.Run(new frmMain());
                }
                else
                {
                    // Si la connexion �choue ou si l'utilisateur ferme le formulaire de connexion
                    Application.Exit();
                }
            }

            FreeConsole();
        }
    }
}