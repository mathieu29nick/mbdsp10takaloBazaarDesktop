namespace WinFormsApp
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private ProgressBar progressBarLoading;

        private void InitializeComponent()
        {
            pictureBoxLogo = new PictureBox();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            btnLogin = new Button();
            progressBarLoading = new ProgressBar();

            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Location = new Point(780, 70); // Positionnez le logo en haut au centre
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(200, 50); //taille du logo
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.Image = WinFormsApp.Properties.Resources.logo_no_background; 

            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(600, 719);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(650, 200); 
            label1.Name = "label1";
            label1.Size = new Size(432, 32);
            label1.TabIndex = 1;
            label1.Text = "Nom d'utilisateur";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.ForeColor = Color.Black; 
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(650, 350); 
            label2.Name = "label2";
            label2.Size = new Size(432, 32);
            label2.TabIndex = 1;
            label2.Text = "Mot de passe";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.ForeColor = Color.Black; 
            // 
            // textBox1
            // 
            textBox1.Location = new Point(650, 250);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(432, 39);
            textBox1.TabIndex = 2;
            // 
            // textBox2 (Mot de passe)
            // 
            textBox2.Location = new Point(650, 400);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(432, 39);
            textBox2.TabIndex = 2;
            textBox2.PasswordChar = '*';
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(650, 500);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(432, 50);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Connexion";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.BackColor = ColorTranslator.FromHtml("#8a8f6a");
            btnLogin.ForeColor = Color.White;
            btnLogin.Click += new EventHandler(this.btnLogin_Click);
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(650, 570);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(432, 30);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.MarqueeAnimationSpeed = 30;
            progressBarLoading.Visible = false;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(1200, 719);
            Controls.Add(pictureBoxLogo);
            Controls.Add(progressBarLoading);
            Controls.Add(btnLogin);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxLogo;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button btnLogin;
        private PictureBox pictureBoxSpinner;
    }
}
