using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Models;
using WinFormsApp.Services;

namespace WinFormsApp.UI
{
    public partial class UserListControl : UserControl
    {
        private int _currentPage = 1;
        private int _totalPages = 1;
        private const int PageSize = 10;
        private bool _isLoading = false;

        public UserListControl()
        {
            InitializeComponent();
        }

        private async void UserListControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync(string searchQuery = "", string gender = "", string type = "")
        {
            if (_isLoading) return;

            _isLoading = true;
            ShowLoading(true);

            var userService = new UserService();
            var response = await userService.GetUsersAsync(_currentPage, PageSize, searchQuery, gender, type);

            _totalPages = response.TotalPages;
            _currentPage = response.CurrentPage;

            UpdatePaginationControls(searchQuery, gender, type);

            listViewUsers.Items.Clear();

            foreach (var user in response.Users)
            {
                var listItem = new ListViewItem(user.Id.ToString());
                listItem.SubItems.Add(user.Username);
                listItem.SubItems.Add(user.Email);
                listItem.SubItems.Add(user.Status); // Populate Status column
                listItem.SubItems.Add("Voir");
                listItem.SubItems.Add("Edit");
                listItem.SubItems.Add("Delete");

                listViewUsers.Items.Add(listItem);
            }

            ShowLoading(false);
            _isLoading = false;
        }

        private void UpdatePaginationControls(string searchQuery, string gender, string type)
        {
            pnlPaginationNumbers.Controls.Clear();
            for (int i = 1; i <= _totalPages; i++)
            {
                int pageNumber = i;
                var btnPageNumber = new Button
                {
                    Text = pageNumber.ToString(),
                    Size = new Size(40, 30),
                    Margin = new Padding(2),
                    BackColor = pageNumber == _currentPage ? Color.FromArgb(56, 56, 56) : Color.White,
                    ForeColor = pageNumber == _currentPage ? Color.White : Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Enabled = pageNumber != _currentPage
                };
                if (pageNumber != _currentPage)
                {
                    btnPageNumber.Click += async (s, e) =>
                    {
                        _currentPage = pageNumber;
                        await LoadUsersAsync(searchQuery, gender, type);
                    };
                }
                pnlPaginationNumbers.Controls.Add(btnPageNumber);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            var gender = (cbGender.SelectedItem as dynamic)?.Value ?? "";
            var type = (cbType.SelectedItem as dynamic)?.Value ?? "";
            await LoadUsersAsync(tbSearch.Text, gender, type);
        }

        private void ShowLoading(bool show)
        {
            loadingLabel.Visible = show;
            listViewUsers.Visible = !show;
        }

        private async void ListViewUsers_MouseClick(object sender, MouseEventArgs e)
        {
            var hitTest = listViewUsers.HitTest(e.Location);
            if (hitTest.Item != null)
            {
                int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                var id = int.Parse(hitTest.Item.Text);
                if (columnIndex == 4) // Voir column
                {
                    OpenViewModal(id);
                }
                else if (columnIndex == 5) // Edit column
                {
                    OpenEditModal(id);
                }
                else if (columnIndex == 6) // Delete column
                {
                    var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet utilisateur ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        await DeleteUserAsync(id);
                    }
                }
            }
        }

        private async void OpenViewModal(int id)
        {
            var userService = new UserService();
            var user = await userService.GetUserByIdAsync(id);
            var modal = new UserViewModal(user);
            modal.ShowDialog();
        }

        private async void OpenEditModal(int id)
        {
            var userService = new UserService();
            var user = await userService.GetUserByIdAsync(id);
            var modal = new UserModal(user.Id);
            var gender = (cbGender.SelectedItem as dynamic)?.Value ?? "";
            var type = (cbType.SelectedItem as dynamic)?.Value ?? "";
            modal.FormClosed += async (s, e) => await LoadUsersAsync(tbSearch.Text, gender, type);
            modal.ShowDialog();
        }

        private async Task DeleteUserAsync(int id)
        {
            var userService = new UserService();
            try
            {
                ShowLoading(true);
                var success = await userService.DeleteUserAsync(id);
                if (success)
                {
                    MessageBox.Show("Utilisateur supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var gender = (cbGender.SelectedItem as dynamic)?.Value ?? "";
                    var type = (cbType.SelectedItem as dynamic)?.Value ?? "";
                    await LoadUsersAsync(tbSearch.Text, gender, type);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.listViewUsers = new ListView();
            this.colId = new ColumnHeader();
            this.colUsername = new ColumnHeader();
            this.colEmail = new ColumnHeader();
            this.colStatus = new ColumnHeader();
            this.colView = new ColumnHeader();  // Added View Column
            this.colEdit = new ColumnHeader();
            this.colDelete = new ColumnHeader();
            this.tbSearch = new TextBox();
            this.cbGender = new ComboBox();
            this.cbType = new ComboBox();
            this.btnSearch = new Button();
            this.pnlPaginationNumbers = new FlowLayoutPanel();
            this.loadingLabel = new Label();

            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "Liste des utilisateurs";
            this.lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitle.Location = new Point(0, 60);
            this.lblTitle.Size = new Size(1024, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // listViewUsers
            this.listViewUsers.Columns.AddRange(new ColumnHeader[]
            {
                this.colId,
                this.colUsername,
                this.colEmail,
                this.colStatus,  // Adding Status Column
                this.colView,    // Adding View Column
                this.colEdit,
                this.colDelete
            });
            this.listViewUsers.FullRowSelect = true;
            this.listViewUsers.GridLines = true;
            this.listViewUsers.Location = new Point(50, 170);
            this.listViewUsers.Name = "listViewUsers";
            this.listViewUsers.Size = new Size(1024, 350);
            this.listViewUsers.TabIndex = 0;
            this.listViewUsers.UseCompatibleStateImageBehavior = false;
            this.listViewUsers.View = View.Details;
            this.listViewUsers.BackColor = Color.WhiteSmoke;
            this.listViewUsers.BorderStyle = BorderStyle.None;
            this.listViewUsers.Scrollable = false;

            this.listViewUsers.MouseClick += ListViewUsers_MouseClick;

            // colId
            this.colId.Text = "ID";
            this.colId.Width = 50;

            // colUsername
            this.colUsername.Text = "Nom d'utilisateur";
            this.colUsername.Width = 200;

            // colEmail
            this.colEmail.Text = "Email";
            this.colEmail.Width = 200;

            // colStatus
            this.colStatus.Text = "Statut";
            this.colStatus.Width = 200;

            // colView
            this.colView.Text = "";
            this.colView.Width = 80;

            // colEdit
            this.colEdit.Text = "";
            this.colEdit.Width = 80;

            // colDelete
            this.colDelete.Text = "";
            this.colDelete.Width = 80;

            // tbSearch
            this.tbSearch.Location = new Point(50, 110);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new Size(300, 40);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.Font = new Font("Segoe UI", 12);

            // ComboBox for Gender
            this.cbGender = new ComboBox();
            this.cbGender.Location = new Point(360, 110);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new Size(180, 40);
            this.cbGender.TabIndex = 3;
            this.cbGender.Font = new Font("Segoe UI", 12);
            this.cbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbGender.Items.AddRange(new object[]
            {
                new { Text = "Genre (tous)", Value = "" },
                new { Text = "Masculin", Value = "Male" },
                new { Text = "Féminin", Value = "Female" }
            });
            this.cbGender.SelectedIndex = 0;

            // ComboBox for Type
            this.cbType = new ComboBox();
            this.cbType.Location = new Point(550, 110);
            this.cbType.Name = "cbType";
            this.cbType.Size = new Size(180, 40);
            this.cbType.TabIndex = 4;
            this.cbType.Font = new Font("Segoe UI", 12);
            this.cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbType.Items.AddRange(new object[]
            {
                new { Text = "Type (tous)", Value = "" },
                new { Text = "Admin", Value = "ADMIN" },
                new { Text = "Utilisateur", Value = "USER" }
            });
            this.cbType.SelectedIndex = 0;

            // Setting DisplayMember and ValueMember properties
            this.cbGender.DisplayMember = "Text";
            this.cbGender.ValueMember = "Value";

            this.cbType.DisplayMember = "Text";
            this.cbType.ValueMember = "Value";


            // btnSearch
            this.btnSearch.Location = new Point(750, 110);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(140, 40);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Rechercher";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.BackColor = Color.FromArgb(56, 56, 56);
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // pnlPaginationNumbers
            this.pnlPaginationNumbers.Location = new Point(50, 530);
            this.pnlPaginationNumbers.Name = "pnlPaginationNumbers";
            this.pnlPaginationNumbers.Size = new Size(500, 30);
            this.pnlPaginationNumbers.TabIndex = 6;
            this.pnlPaginationNumbers.AutoSize = true;

            // loadingLabel
            this.loadingLabel.Location = new Point(12, 250);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new Size(760, 50);
            this.loadingLabel.TabIndex = 7;
            this.loadingLabel.Text = "Chargement...";
            this.loadingLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.loadingLabel.Visible = false;

            // UserListControl
            this.AutoScaleDimensions = new SizeF(9F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.listViewUsers);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.cbGender);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.pnlPaginationNumbers);
            this.Controls.Add(this.loadingLabel);
            this.Name = "UserListControl";
            this.Size = new Size(1024, 600); // Adjust this if needed for your layout
            this.Load += new EventHandler(this.UserListControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private ListView listViewUsers;
        private ColumnHeader colId;
        private ColumnHeader colUsername;
        private ColumnHeader colEmail;
        private ColumnHeader colStatus;
        private ColumnHeader colView;
        private ColumnHeader colEdit;
        private ColumnHeader colDelete;
        private TextBox tbSearch;
        private ComboBox cbGender;
        private ComboBox cbType;
        private Button btnSearch;
        private FlowLayoutPanel pnlPaginationNumbers;
        private Label loadingLabel;
    }
}
