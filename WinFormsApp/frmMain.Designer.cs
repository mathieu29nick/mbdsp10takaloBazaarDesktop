using WinFormsApp.UI;

namespace WinFormsApp
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;
        private WinFormsApp.UI.ObjectListManager _objectListManager;
        private WinFormsApp.UI.TypeReportListScreen _typeReportListManager;
        private WinFormsApp.UI.ExchangeListScreen _exchangeListManager;
        private DashboardScreen _dashboardControl;

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
            btnDashboard = new Button();
            btnExchange = new Button();
            pnlExchange =new Panel();
            btnObject = new Button();
            btnLogout = new Button();

            pnlObjects = new Panel();
            pnlTypeReport = new Panel();
            pnlDashboard = new Panel();
            tbSearch = new Custom_Controls.RoundTB();
            panel3 = new Panel();
            btnLogoSmall = new Button();
            panel2 = new Panel();
            btnMenuBar = new Button();
            pnlCategories = new Panel();
            btnUsers = new Button();
            btnReports = new Button();
            pnlReports = new Panel();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            pnlLeft.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();

            // 
            // pnlLeft
            // 
            pnlLeft.AutoScroll = true;
            pnlLeft.BackColor = ColorTranslator.FromHtml("#8a8f6a");
            pnlLeft.Controls.Add(btnLogout);
            pnlLeft.Controls.Add(btnTypeReport);
            pnlLeft.Controls.Add(btnCategories);
            pnlLeft.Controls.Add(btnUsers);
            pnlLeft.Controls.Add(btnObject);
            pnlLeft.Controls.Add(btnExchange);
            pnlLeft.Controls.Add(btnReports);
            pnlLeft.Controls.Add(btnDashboard);
            pnlLeft.Controls.Add(panel3);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(350, 1143);
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
            btnUsers.Size = new Size(350, 70);
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
            btnTypeReport.Padding = new Padding(20, 0, 0, 0);
            btnTypeReport.Size = new Size(350, 52);
            btnTypeReport.TabIndex = 1;
            btnTypeReport.Tag = "          Type de signalements";
            btnTypeReport.Text = "          Type de signalements";
            btnTypeReport.TextAlign = ContentAlignment.MiddleLeft;
            btnTypeReport.UseVisualStyleBackColor = true;
            btnTypeReport.Click += btnTypeReport_Click;


            // 
            // btnLogout
            // 
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.BackColor = ColorTranslator.FromHtml("#bc8246");
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(0, 759);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(0); 
            btnLogout.Size = new Size(310, 70);
            btnLogout.TabIndex = 3;
            btnLogout.Tag = "          Logout";
            btnLogout.Text = "Déconnexion";
            btnLogout.TextAlign = ContentAlignment.MiddleCenter;
            btnLogout.Font = new Font(btnLogout.Font.FontFamily, 11, FontStyle.Bold);
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += new EventHandler(this.btnLogout_Click); 



            // 
            // btnHelp
            // pnlTypeReport
            // 
            pnlTypeReport.Dock = DockStyle.Fill;
            pnlTypeReport.BackColor = Color.FromArgb(244, 246, 249);
            pnlTypeReport.Location = new Point(350, 70);
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
            btnCategories.Size = new Size(350, 70);
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
            pnlCategories.Location = new Point(350, 70);
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
            btnExchange.Size = new Size(350, 70);
            btnExchange.TabIndex = 2;
            btnExchange.Tag = "          Echanges";
            btnExchange.Text = "          Echanges";
            btnExchange.TextAlign = ContentAlignment.MiddleLeft;
            btnExchange.UseVisualStyleBackColor = true;
            btnExchange.Click += new EventHandler(this.btnExchange_Click);

            // 
            // pnlExchange
            // 
            pnlExchange.Dock = DockStyle.Fill;
            pnlExchange.BackColor = Color.FromArgb(244, 246, 249);
            pnlExchange.Location = new Point(350, 70);
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
            pnlUsers.Location = new Point(350, 70);
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
            btnObject.Size = new Size(350, 70);
            btnObject.TabIndex = 2;
            btnObject.Tag = "          Objets";
            btnObject.Text = "          Objets";
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
            _dashboardControl = new DashboardScreen(pnlDashboard);

            // 
            // btnDashboard
            // 
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Image = (Image)resources.GetObject("btn4.Image");
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(250, 339);
            btnDashboard.Name = "Dashboard";
            btnDashboard.Padding = new Padding(20, 0, 0, 0);
            btnDashboard.Size = new Size(350, 70);
            btnDashboard.TabIndex = 1;
            btnDashboard.Tag = "         Dashboard";
            btnDashboard.Text = "          Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = true;
            btnDashboard.Click += btnDashboard_Click;


            // 
            // pnlDashboard
            // 
            pnlDashboard.Dock = DockStyle.Fill;
            pnlDashboard.BackColor = Color.FromArgb(244, 246, 249);
            pnlDashboard.Location = new Point(350, 70);
            pnlDashboard.Name = "pnlDashboard";
            pnlDashboard.Size = new Size(1624, 1073);
            pnlDashboard.TabIndex = 2;
            pnlDashboard.Visible = true;

            // btnReports
            btnReports.Dock = DockStyle.Top;
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.ForeColor = Color.White;
            btnReports.Image = (Image)resources.GetObject("btnHelp.Image");
            btnReports.ImageAlign = ContentAlignment.MiddleLeft;
            btnReports.Location = new Point(0, 889);
            btnReports.Name = "btnReports";
            btnReports.Padding = new Padding(20, 0, 0, 0);
            btnReports.Size = new Size(350, 70);
            btnReports.TabIndex = 2;
            btnReports.Tag = "          Objets signalés";
            btnReports.Text = "          Objets signalés";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = true;
            btnReports.Click += new EventHandler(this.btnReports_Click);


            // pnlReports
            pnlReports = new Panel();
            pnlReports.Dock = DockStyle.Fill;
            pnlReports.BackColor = Color.FromArgb(244, 246, 249);
            pnlReports.Location = new Point(350, 70);
            pnlReports.Name = "pnlReports";
            pnlReports.Size = new Size(1624, 1073);
            pnlReports.TabIndex = 4;
            pnlReports.Visible = false;
            Controls.Add(pnlReports);

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
            panel3.Size = new Size(350, 70);
            panel3.TabIndex = 0;

            // 
            // btnLogoSmall
            // 
            btnLogoSmall.Dock = DockStyle.Top;
            btnLogoSmall.FlatAppearance.BorderSize = 0;
            btnLogoSmall.FlatStyle = FlatStyle.Flat;
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            btnLogoSmall.Image = WinFormsApp.Properties.Resources.icons8_ai_50;
            btnLogoSmall.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogoSmall.Location = new Point(0, 0);
            btnLogoSmall.Padding = new Padding(20, 0, 0, 0);
            btnLogoSmall.Name = "btnLogoSmall";
            btnLogoSmall.Size = new Size(350, 70);
            btnLogoSmall.TabIndex = 1;
            btnLogoSmall.Tag = "TAKALOBAZAAR'Ô";
            btnLogoSmall.Text = "TAKALOBAZAAR'Ô";
            btnLogoSmall.UseVisualStyleBackColor = true;

            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnMenuBar);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(350, 0);
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
            Controls.Add(pnlDashboard);
            Controls.Add(pnlCategories);
            Controls.Add(pnlExchange);
            Controls.Add(pnlLeft);
            Controls.Add(statusStrip1);
            DoubleBuffered = true;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TakaloBazaar'ô";
            Load += frmMain_Load;
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
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
        private Button btnTypeReport;
        private Button btnDashboard;
        private Custom_Controls.RoundTB tbSearch;
        private Button btnCategories;
        private Panel pnlCategories;
        private Button btnUsers;
        private Panel pnlUsers;
        private Button btnObject;
        private Panel pnlObjects;
        private Button btnLogout;
        private Panel pnlTypeReport;
        private Button btnExchange;
        private Panel pnlExchange;
        private Button btnReports;
        private Panel pnlReports;
        private Panel pnlDashboard;

        private void HideAllPanels()
        {
            pnlCategories.Visible = false;
            pnlObjects.Visible = false;
            pnlUsers.Visible = false;
            pnlExchange.Visible = false;
            pnlTypeReport.Visible = false;
            pnlDashboard.Visible = false;
            pnlReports.Visible = false;
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

        private async void btnReports_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlReports.Visible = true;

            pnlReports.Controls.Clear();
            var reportListScreen = new WinFormsApp.UI.ReportListScreen();
            reportListScreen.Dock = DockStyle.Fill;
            pnlReports.Controls.Add(reportListScreen);
        }


        private async void btnExchange_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlExchange.Visible = true;
            await _exchangeListManager.LoadExchangesAsync();
        }

        private async void btnDashboard_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlDashboard.Visible = true;
            _dashboardControl.LoadDashboard();
        }
    }
}
