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

            dgvUsers.Rows.Clear();

            foreach (var user in response.Users)
            {
                dgvUsers.Rows.Add(user.Id, user.Username, user.Email, user.Status);
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
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Enabled = pageNumber != _currentPage
                };
                btnPageNumber.ForeColor = pageNumber == _currentPage ? Color.White : ColorTranslator.FromHtml("#383838");
                btnPageNumber.BackColor = pageNumber == _currentPage ? ColorTranslator.FromHtml("#bc8246") : Color.White;

                btnPageNumber.Paint += (s, e) =>
                {
                    var btn = (Button)s;
                    e.Graphics.Clear(btn.BackColor);
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, e.ClipRectangle, btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
            dgvUsers.Visible = !show;
        }

        private async void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var id = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells[0].Value);

                if (e.ColumnIndex == dgvUsers.Columns["colView"].Index)
                {
                    OpenViewModal(id);
                }
                else if (e.ColumnIndex == dgvUsers.Columns["colEdit"].Index)
                {
                    OpenEditModal(id);
                }
                else if (e.ColumnIndex == dgvUsers.Columns["colDelete"].Index)
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
            this.dgvUsers = new DataGridView();
            this.tbSearch = new TextBox();
            this.cbGender = new ComboBox();
            this.cbType = new ComboBox();
            this.btnSearch = new Button();
            this.pnlPaginationNumbers = new FlowLayoutPanel();
            this.loadingLabel = new Label();
            this.tableLayoutPanel = new TableLayoutPanel();

            this.SuspendLayout();

            // TableLayoutPanel setup
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Title row
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Search row
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // DataGridView row
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Pagination row

            // lblTitle
            this.lblTitle.Text = "Liste des utilisateurs";
            this.lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitle.Dock = DockStyle.Fill;
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // dgvUsers
            this.dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new DataGridViewColumn[] {
        new DataGridViewTextBoxColumn { HeaderText = "ID", Name = "colId", Width = 100 },
        new DataGridViewTextBoxColumn { HeaderText = "Nom d'utilisateur", Name = "colUsername", Width = 300 },
        new DataGridViewTextBoxColumn { HeaderText = "Email", Name = "colEmail", Width = 300 },
        new DataGridViewTextBoxColumn { HeaderText = "Statut", Name = "colStatus", Width = 200 },
        new DataGridViewButtonColumn { HeaderText = "-", Text = "Voir", Name = "colView", Width = 100, UseColumnTextForButtonValue = true },
        new DataGridViewButtonColumn { HeaderText = "-", Text = "Modifier", Name = "colEdit", Width = 100, UseColumnTextForButtonValue = true },
        new DataGridViewButtonColumn { HeaderText = "-", Text = "Supprimer", Name = "colDelete", Width = 100, UseColumnTextForButtonValue = true }
    });
            this.dgvUsers.Dock = DockStyle.Fill;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = Color.White;
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.CellClick += new DataGridViewCellEventHandler(this.dgvUsers_CellClick);

            // pnlPaginationNumbers
            this.pnlPaginationNumbers.Dock = DockStyle.Fill;
            this.pnlPaginationNumbers.AutoSize = true;

            // loadingLabel
            this.loadingLabel.Dock = DockStyle.Fill;
            this.loadingLabel.Text = "Chargement...";
            this.loadingLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.loadingLabel.Visible = false;

            // Adding controls to the TableLayoutPanel
            this.tableLayoutPanel.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel.Controls.Add(CreateSearchPanel(), 0, 1); // Adds the search panel
            this.tableLayoutPanel.Controls.Add(this.dgvUsers, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.pnlPaginationNumbers, 0, 3);

            // Add TableLayoutPanel to the UserControl
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.loadingLabel);

            this.Name = "UserListControl";
            this.Size = new Size(1024, 600);
            this.Load += new EventHandler(this.UserListControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private Panel CreateSearchPanel()
        {
            // Create a TableLayoutPanel with two columns
            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };

            // Set the first column to fill the remaining space and the second column to auto size
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Create a FlowLayoutPanel for the search controls and align them to the right
            var flowLayoutPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                AutoSize = true,
                WrapContents = false
            };

            // Restore the TextBox for search
            this.tbSearch = new TextBox
            {
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 12)
            };

            // Restore the ComboBox for gender
            this.cbGender = new ComboBox
            {
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 12),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "Text",
                ValueMember = "Value"
            };
            this.cbGender.Items.AddRange(new object[]
            {
        new { Text = "Genre (tous)", Value = "" },
        new { Text = "Masculin", Value = "Male" },
        new { Text = "Féminin", Value = "Female" }
            });
            this.cbGender.SelectedIndex = 0;

            // Restore the ComboBox for type
            this.cbType = new ComboBox
            {
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 12),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "Text",
                ValueMember = "Value"
            };
            this.cbType.Items.AddRange(new object[]
            {
        new { Text = "Type (tous)", Value = "" },
        new { Text = "Admin", Value = "ADMIN" },
        new { Text = "Utilisateur", Value = "USER" }
            });
            this.cbType.SelectedIndex = 0;

            // Restore the Search button
            this.btnSearch = new Button
            {
                Text = "Rechercher",
                Size = new Size(140, 40),
                BackColor = ColorTranslator.FromHtml("#bc8246"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // Add the controls to the FlowLayoutPanel
            flowLayoutPanel.Controls.Add(this.tbSearch);
            flowLayoutPanel.Controls.Add(this.cbGender);
            flowLayoutPanel.Controls.Add(this.cbType);
            flowLayoutPanel.Controls.Add(this.btnSearch);

            // Add an empty panel to the first column of the TableLayoutPanel
            tableLayoutPanel.Controls.Add(new Panel(), 0, 0);

            // Add the FlowLayoutPanel with controls to the second column of the TableLayoutPanel
            tableLayoutPanel.Controls.Add(flowLayoutPanel, 1, 0);

            return tableLayoutPanel;
        }







        private Label lblTitle;
        private DataGridView dgvUsers;
        private TextBox tbSearch;
        private ComboBox cbGender;
        private ComboBox cbType;
        private Button btnSearch;
        private FlowLayoutPanel pnlPaginationNumbers;
        private Label loadingLabel;
        private TableLayoutPanel tableLayoutPanel;
    }
}
