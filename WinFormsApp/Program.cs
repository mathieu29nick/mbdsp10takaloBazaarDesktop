namespace WinFormsApp
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        private const string TokenFilePath = "token.txt";
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

            if (File.Exists(TokenFilePath))
            {
                Configuration.Configuration.TOKKEN = File.ReadAllText(TokenFilePath);
                // Token exists, proceed to the main form
                Application.Run(new frmMain());
            }
            else
            {
                // Afficher le formulaire de connexion
                using (var loginForm = new frmLogin())
                {
                    var result = loginForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        // Si la connexion est réussie, lancer le formulaire principal
                        Application.Run(new frmMain());
                    }
                    else
                    {
                        // Si la connexion échoue ou si l'utilisateur ferme le formulaire de connexion
                        Application.Exit();
                    }
                }
            }

            FreeConsole();
        }
    }
}