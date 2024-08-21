namespace WinFormsApp
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;
        private WinFormsApp.UI.ObjectListManager _objectListManager;
        private WinFormsApp.UI.TypeReportListScreen _typeReportListManager;
        private WinFormsApp.UI.ExchangeListScreen _exchangeListManager;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            pnlLeft = new Panel();
            btnTypeReport = new Button();
            btnCategories = new Button();
            btnSettings = new Button();
            btn5 = new Button();
            btn4 = new Button();
            btn3 = new Button();
            btn2 = new Button();
            btn1 = new Button();
            btnExchange = new Button();
            pnlExchange =new Panel();
            btnObject = new Button();
            btnHome = new Button();
            pnlSearch = new Panel();
            pnlObjects = new Panel();
            pnlTypeReport = new Panel();
            tbSearch = new Custom_Controls.RoundTB();
            panel3 = new Panel();
            btnLogoSmall = new Button();
            panel2 = new Panel();
            btnMenuBar = new Button();
            pnlCategories = new Panel();
            btnUsers = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            pnlLeft.SuspendLayout();
            pnlSearch.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();

            // 
            // pnlLeft
            // 
            pnlLeft.AutoScroll = true;
            pnlLeft.BackColor = Color.FromArgb(73, 78, 83);
            pnlLeft.Controls.Add(btnTypeReport);
            pnlLeft.Controls.Add(btnCategories);
            pnlLeft.Controls.Add(btnUsers);
            pnlLeft.Controls.Add(btnObject);
            pnlLeft.Controls.Add(btnExchange);
            pnlLeft.Controls.Add(btnSettings);
            pnlLeft.Controls.Add(btn5);
            pnlLeft.Controls.Add(btn4);
            pnlLeft.Controls.Add(btn3);
            pnlLeft.Controls.Add(btn2);
            pnlLeft.Controls.Add(btn1);
            pnlLeft.Controls.Add(btnHome);
            pnlLeft.Controls.Add(pnlSearch);
            pnlLeft.Controls.Add(panel3);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(310, 1143);
            pnlLeft.TabIndex = 0;

            // User button
            btnUsers.Dock = DockStyle.Top;
            btnUsers.FlatAppearance.BorderSize = 0;
            btnUsers.FlatStyle = FlatStyle.Flat;
            btnUsers.ForeColor = Color.White;
            btnUsers.Image = (Image)resources.GetObject("btnHelp.Image"); // You can change this image to whatever fits.
            btnUsers.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsers.Location = new Point(0, 759); // Adjust the location as needed.
            btnUsers.Name = "btnUsers";
            btnUsers.Padding = new Padding(20, 0, 0, 0);
            btnUsers.Size = new Size(310, 70);
            btnUsers.TabIndex = 2;
            btnUsers.Tag = "          Utilisateurs";
            btnUsers.Text = "          Utilisateurs";
            btnUsers.TextAlign = ContentAlignment.MiddleLeft;
            btnUsers.UseVisualStyleBackColor = true;
            btnUsers.Click += new EventHandler(this.btnUsers_Click);
          
            // btnTypeReport
            btnTypeReport.Dock = DockStyle.Top;
            btnTypeReport.FlatAppearance.BorderSize = 0;
            btnTypeReport.FlatStyle = FlatStyle.Flat;
            btnTypeReport.ForeColor = Color.White;
            btnTypeReport.Image = (Image)resources.GetObject("btnHelp.Image");
            btnTypeReport.ImageAlign = ContentAlignment.MiddleLeft;
            btnTypeReport.Location = new Point(0, 567);
            btnTypeReport.Margin = new Padding(2, 2, 2, 2);
            btnTypeReport.Name = "btnTypeReport";
            btnTypeReport.Padding = new Padding(15, 0, 0, 0);
            btnTypeReport.Size = new Size(232, 52);
            btnTypeReport.TabIndex = 1;
            btnTypeReport.Tag = "          Type de signalements";
            btnTypeReport.Text = "          Type de signalements";
            btnTypeReport.TextAlign = ContentAlignment.MiddleLeft;
            btnTypeReport.UseVisualStyleBackColor = true;
            btnTypeReport.Click += btnExchange_Click;


            // 
            // pnlTypeReport
            // 
            pnlTypeReport.Dock = DockStyle.Fill;
            pnlTypeReport.BackColor = Color.FromArgb(244, 246, 249);
            pnlTypeReport.Location = new Point(310, 70);
            pnlTypeReport.Name = "pnlTypeReport";
            pnlTypeReport.Size = new Size(1624, 1073);
            pnlTypeReport.TabIndex = 2;
            pnlTypeReport.Visible = false;

            // 
            // btnCategories
            // 
            btnCategories.Dock = DockStyle.Top;
            btnCategories.FlatAppearance.BorderSize = 0;
            btnCategories.FlatStyle = FlatStyle.Flat;
            btnCategories.ForeColor = Color.White;
            btnCategories.Image = (Image)resources.GetObject("btnHelp.Image");
            btnCategories.ImageAlign = ContentAlignment.MiddleLeft;
            btnCategories.Location = new Point(0, 619);
            btnCategories.Name = "btnCategories";
            btnCategories.Padding = new Padding(20, 0, 0, 0);
            btnCategories.Size = new Size(310, 70);
            btnCategories.TabIndex = 2;
            btnCategories.Tag = "          Catégories";
            btnCategories.Text = "          Catégories";
            btnCategories.TextAlign = ContentAlignment.MiddleLeft;
            btnCategories.UseVisualStyleBackColor = true;
            btnCategories.Click += new EventHandler(this.btnCategories_Click);

            // 
            // pnlCategories
            // 
            pnlCategories.Dock = DockStyle.Fill;
            pnlCategories.BackColor = Color.FromArgb(244, 246, 249);
            pnlCategories.Location = new Point(310, 70);
            pnlCategories.Name = "pnlCategories";
            pnlCategories.Size = new Size(1624, 1073);
            pnlCategories.TabIndex = 2;
            pnlCategories.Visible = false;

            // 
            // btnExchange
            // 
            btnExchange.Dock = DockStyle.Top;
            btnExchange.FlatAppearance.BorderSize = 0;
            btnExchange.FlatStyle = FlatStyle.Flat;
            btnExchange.ForeColor = Color.White;
            btnExchange.Image = (Image)resources.GetObject("btnHelp.Image");
            btnExchange.ImageAlign = ContentAlignment.MiddleLeft;
            btnExchange.Location = new Point(0, 700);
            btnExchange.Name = "btnCategories";
            btnExchange.Padding = new Padding(20, 0, 0, 0);
            btnExchange.Size = new Size(310, 70);
            btnExchange.TabIndex = 2;
            btnExchange.Tag = "          Liste Exchanges";
            btnExchange.Text = "          Liste Echanges";
            btnExchange.TextAlign = ContentAlignment.MiddleLeft;
            btnExchange.UseVisualStyleBackColor = true;
            btnExchange.Click += new EventHandler(this.btnExchange_Click);

            // 
            // pnlExchange
            // 
            pnlExchange.Dock = DockStyle.Fill;
            pnlExchange.BackColor = Color.FromArgb(244, 246, 249);
            pnlExchange.Location = new Point(310, 70);
            pnlExchange.Name = "pnlExchange";
            pnlExchange.Size = new Size(1624, 1073);
            pnlExchange.TabIndex = 2;
            pnlExchange.Visible = false;

            ListBox listBoxCategories = new ListBox();
            listBoxCategories.Dock = DockStyle.Fill;
            listBoxCategories.BackColor = Color.White;
            listBoxCategories.ForeColor = Color.Black;
            pnlCategories.Controls.Add(listBoxCategories);

            pnlUsers = new Panel();
            pnlUsers.Dock = DockStyle.Fill;
            pnlUsers.BackColor = Color.FromArgb(244, 246, 249);
            pnlUsers.Location = new Point(310, 70);
            pnlUsers.Name = "pnlUsers";
            pnlUsers.Size = new Size(1624, 1073);
            pnlUsers.TabIndex = 4;
            pnlUsers.Visible = false;
            Controls.Add(pnlUsers);


            // 
            // btnObject
            // 
            btnObject.Dock = DockStyle.Top;
            btnObject.FlatAppearance.BorderSize = 0;
            btnObject.FlatStyle = FlatStyle.Flat;
            btnObject.ForeColor = Color.White;
            btnObject.Image = (Image)resources.GetObject("btnHelp.Image");
            btnObject.ImageAlign = ContentAlignment.MiddleLeft;
            btnObject.Location = new Point(0, 689);
            btnObject.Name = "btnObject";
            btnObject.Padding = new Padding(20, 0, 0, 0);
            btnObject.Size = new Size(310, 70);
            btnObject.TabIndex = 2;
            btnObject.Tag = "          Liste Objet";
            btnObject.Text = "          Liste Objet";
            btnObject.TextAlign = ContentAlignment.MiddleLeft;
            btnObject.UseVisualStyleBackColor = true;
            btnObject.Click += new EventHandler(this.btnObject_Click);

            // 
            // pnlObjects
            // 
            pnlObjects.Dock = DockStyle.Fill;
            pnlObjects.BackColor = Color.FromArgb(244, 246, 249);
            pnlObjects.Location = new Point(950, 950);
            pnlObjects.Name = "pnlObjects";
            pnlObjects.Size = new Size(1624,73);
            pnlObjects.TabIndex = 3;
            pnlObjects.Visible = false;

            _objectListManager = new WinFormsApp.UI.ObjectListManager(pnlObjects);
            _typeReportListManager = new WinFormsApp.UI.TypeReportListScreen(pnlTypeReport);
            _exchangeListManager = new WinFormsApp.UI.ExchangeListScreen(pnlExchange);


            // 
            // btnSettings
            // 
            btnSettings.Dock = DockStyle.Top;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.ForeColor = Color.White;
            btnSettings.Image = (Image)resources.GetObject("btnSettings.Image");
            btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
            btnSettings.Location = new Point(0, 479);
            btnSettings.Name = "btnSettings";
            btnSettings.Padding = new Padding(20, 0, 0, 0);
            btnSettings.Size = new Size(310, 70);
            btnSettings.TabIndex = 1;
            btnSettings.Tag = "          Settings";
            btnSettings.Text = "          Settings";
            btnSettings.TextAlign = ContentAlignment.MiddleLeft;
            btnSettings.UseVisualStyleBackColor = true;

            // 
            // btn5
            // 
            btn5.Dock = DockStyle.Top;
            btn5.FlatAppearance.BorderSize = 0;
            btn5.FlatStyle = FlatStyle.Flat;
            btn5.ForeColor = Color.White;
            btn5.Image = (Image)resources.GetObject("btn5.Image");
            btn5.ImageAlign = ContentAlignment.MiddleLeft;
            btn5.Location = new Point(0, 409);
            btn5.Name = "btn5";
            btn5.Padding = new Padding(20, 0, 0, 0);
            btn5.Size = new Size(310, 70);
            btn5.TabIndex = 1;
            btn5.Tag = "          Button 5";
            btn5.Text = "          Button 5";
            btn5.TextAlign = ContentAlignment.MiddleLeft;
            btn5.UseVisualStyleBackColor = true;

            // 
            // btn4
            // 
            btn4.Dock = DockStyle.Top;
            btn4.FlatAppearance.BorderSize = 0;
            btn4.FlatStyle = FlatStyle.Flat;
            btn4.ForeColor = Color.White;
            btn4.Image = (Image)resources.GetObject("btn4.Image");
            btn4.ImageAlign = ContentAlignment.MiddleLeft;
            btn4.Location = new Point(250, 339);
            btn4.Name = "btn4";
            btn4.Padding = new Padding(20, 0, 0, 0);
            btn4.Size = new Size(310, 70);
            btn4.TabIndex = 1;
            btn4.Tag = "          Button 4";
            btn4.Text = "          Button 4";
            btn4.TextAlign = ContentAlignment.MiddleLeft;
            btn4.UseVisualStyleBackColor = true;

            // 
            // btn3
            // 
            btn3.Dock = DockStyle.Top;
            btn3.FlatAppearance.BorderSize = 0;
            btn3.FlatStyle = FlatStyle.Flat;
            btn3.ForeColor = Color.White;
            btn3.Image = (Image)resources.GetObject("btn3.Image");
            btn3.ImageAlign = ContentAlignment.MiddleLeft;
            btn3.Location = new Point(0, 269);
            btn3.Name = "btn3";
            btn3.Padding = new Padding(20, 0, 0, 0);
            btn3.Size = new Size(310, 70);
            btn3.TabIndex = 1;
            btn3.Tag = "          Button 3";
            btn3.Text = "          Button 3";
            btn3.TextAlign = ContentAlignment.MiddleLeft;
            btn3.UseVisualStyleBackColor = true;

            // 
            // btn2
            // 
            btn2.Dock = DockStyle.Top;
            btn2.FlatAppearance.BorderSize = 0;
            btn2.FlatStyle = FlatStyle.Flat;
            btn2.ForeColor = Color.White;
            btn2.Image = (Image)resources.GetObject("btn2.Image");
            btn2.ImageAlign = ContentAlignment.MiddleLeft;
            btn2.Location = new Point(0, 199);
            btn2.Name = "btn2";
            btn2.Padding = new Padding(20, 0, 0, 0);
            btn2.Size = new Size(310, 70);
            btn2.TabIndex = 1;
            btn2.Tag = "          Button 2";
            btn2.Text = "          Button 2";
            btn2.TextAlign = ContentAlignment.MiddleLeft;
            btn2.UseVisualStyleBackColor = true;

            // 
            // btn1
            // 
            btn1.Dock = DockStyle.Top;
            btn1.FlatAppearance.BorderSize = 0;
            btn1.FlatStyle = FlatStyle.Flat;
            btn1.ForeColor = Color.White;
            btn1.Image = (Image)resources.GetObject("btn1.Image");
            btn1.ImageAlign = ContentAlignment.MiddleLeft;
            btn1.Location = new Point(0, 129);
            btn1.Name = "btn1";
            btn1.Padding = new Padding(20, 0, 0, 0);
            btn1.Size = new Size(310, 70);
            btn1.TabIndex = 1;
            btn1.Tag = "          Button 1";
            btn1.Text = "          Button 1";
            btn1.TextAlign = ContentAlignment.MiddleLeft;
            btn1.UseVisualStyleBackColor = true;

            // 
            // btnHome
            // 
            btnHome.Dock = DockStyle.Top;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.ForeColor = Color.White;
            btnHome.Image = (Image)resources.GetObject("btnHome.Image");
            btnHome.ImageAlign = ContentAlignment.MiddleLeft;
            btnHome.Location = new Point(0, 59);
            btnHome.Name = "btnHome";
            btnHome.Padding = new Padding(20, 0, 0, 0);
            btnHome.Size = new Size(310, 70);
            btnHome.TabIndex = 1;
            btnHome.Tag = "          Home";
            btnHome.Text = "          Home";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.UseVisualStyleBackColor = true;

            // 
            // pnlSearch
            // 
            pnlSearch.AutoSize = true;
            pnlSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlSearch.Controls.Add(tbSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(0, 70);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Padding = new Padding(10);
            pnlSearch.Size = new Size(310, 59);
            pnlSearch.TabIndex = 3;

            // 
            // tbSearch
            // 
            tbSearch.Dock = DockStyle.Top;
            tbSearch.Location = new Point(10, 10);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(290, 39);
            tbSearch.TabIndex = 4;
            tbSearch.TextChanged += tbSearch_TextChanged;

            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(btnLogoSmall);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(310, 70);
            panel3.TabIndex = 0;

            // 
            // btnLogoSmall
            // 
            btnLogoSmall.Dock = DockStyle.Top;
            btnLogoSmall.FlatAppearance.BorderSize = 0;
            btnLogoSmall.FlatStyle = FlatStyle.Flat;
            btnLogoSmall.Image = (Image)resources.GetObject("btnLogoSmall.Image");
            btnLogoSmall.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogoSmall.Location = new Point(0, 0);
            btnLogoSmall.Name = "btnLogoSmall";
            btnLogoSmall.Padding = new Padding(20, 0, 0, 0);
            btnLogoSmall.Size = new Size(310, 70);
            btnLogoSmall.TabIndex = 1;
            btnLogoSmall.Tag = "ADMIN";
            btnLogoSmall.Text = "ADMIN";
            btnLogoSmall.UseVisualStyleBackColor = true;

            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnMenuBar);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(310, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1624, 70);
            panel2.TabIndex = 1;

            // 
            // btnMenuBar
            // 
            btnMenuBar.Dock = DockStyle.Left;
            btnMenuBar.FlatAppearance.BorderSize = 0;
            btnMenuBar.FlatStyle = FlatStyle.Flat;
            btnMenuBar.Image = (Image)resources.GetObject("btnMenuBar.Image");
            btnMenuBar.Location = new Point(0, 0);
            btnMenuBar.Name = "btnMenuBar";
            btnMenuBar.Size = new Size(70, 70);
            btnMenuBar.TabIndex = 0;
            btnMenuBar.UseVisualStyleBackColor = true;
            btnMenuBar.Click += btnMenuBar_Click;

            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(32, 32);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 1143);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1934, 42);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";

            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(78, 32);
            toolStripStatusLabel1.Text = "Ready";

            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(1934, 1185);
            Controls.Add(panel2);
            Controls.Add(pnlObjects);
            Controls.Add(pnlTypeReport);
            Controls.Add(pnlCategories);
            Controls.Add(pnlExchange);
            Controls.Add(pnlLeft);
            Controls.Add(statusStrip1);
            DoubleBuffered = true;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += frmMain_Load;
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlLeft;
        private Panel panel2;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Panel panel3;
        private Button btnMenuBar;
        private Button btnLogoSmall;
        private Button btnHome;
        private Button btnTypeReport;
        private Button btnSettings;
        private Button btn5;
        private Button btn4;
        private Button btn3;
        private Button btn2;
        private Button btn1;
        private Custom_Controls.RoundTB tbSearch;
        private Panel pnlSearch;
        private Button btnCategories;
        private Panel pnlCategories;
        private Button btnUsers;
        private Panel pnlUsers;
        private Button btnObject;
        private Panel pnlObjects;
        private Panel pnlTypeReport;
        private Button btnExchange;
        private Panel pnlExchange;

        private void HideAllPanels()
        {
            pnlCategories.Visible = false;
            pnlObjects.Visible = false;
            pnlUsers.Visible = false;
            pnlExchange.Visible = false;
            pnlTypeReport.Visible = false;
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlCategories.Visible = true;
            pnlCategories.Controls.Clear();
            var categoryListControl = new WinFormsApp.UI.CategoryListControl();
            categoryListControl.Dock = DockStyle.Fill;
            pnlCategories.Controls.Add(categoryListControl);
        }

        // Liste des Objets
        private async void btnObject_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlObjects.Visible = true;
            await _objectListManager.LoadObjectsAsync();
        }

      private void btnUsers_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlUsers.Visible = true;

            pnlUsers.Controls.Clear();

            var userListControl = new WinFormsApp.UI.UserListControl();
            userListControl.Dock = DockStyle.Fill;
            pnlUsers.Controls.Add(userListControl);
        }
      
        private async void btnTypeReport_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlTypeReport.Visible = true;
            await _typeReportListManager.LoadTypeReportsAsync();
        }

        private async void btnExchange_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlExchange.Visible = true;
            await _exchangeListManager.LoadExchangesAsync();
        }
    }
}
