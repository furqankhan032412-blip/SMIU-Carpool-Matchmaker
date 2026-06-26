using SMIUCarpool.Data;
using SMIUCarpool.Helpers;
using SMIUCarpool.Models;

namespace SMIUCarpool.Forms;

public class LoginForm : Form
{
    private readonly TextBox _txtEmail = new();
    private readonly TextBox _txtPassword = new();
    private readonly Label _lblError = new();

    public LoginForm()
    {
        AppTheme.PrepareForm(this, "Login");
        MinimumSize = new Size(540, 440);
        BuildLayout();
    }

    private void BuildLayout()
    {
        TableLayoutPanel outer = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            BackColor = AppTheme.Background
        };
        outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        outer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420));
        outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        outer.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        outer.RowStyles.Add(new RowStyle(SizeType.Absolute, 318));
        outer.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        Panel card = new()
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Panel,
            Padding = new Padding(28)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 8
        };

        AppTheme.StyleTextBox(_txtEmail);
        AppTheme.StyleTextBox(_txtPassword);
        _txtPassword.UseSystemPasswordChar = true;
        _lblError.ForeColor = AppTheme.Danger;
        _lblError.AutoSize = true;

        Button btnLogin = new() { Text = "Login", Width = 118 };
        Button btnRegister = new() { Text = "Create Account", Width = 150 };
        AppTheme.StylePrimaryButton(btnLogin);
        AppTheme.StyleSecondaryButton(btnRegister);

        btnLogin.Click += BtnLogin_Click;
        btnRegister.Click += (_, _) =>
        {
            using RegisterForm form = new();
            form.ShowDialog();
        };

        FlowLayoutPanel buttons = new() { AutoSize = true, Margin = new Padding(0, 8, 0, 0) };
        buttons.Controls.Add(btnLogin);
        buttons.Controls.Add(btnRegister);

        layout.Controls.Add(UiHelpers.Title(AppTheme.AppName));
        layout.Controls.Add(UiHelpers.Muted("Login with one of the demo accounts or create a new account."));
        layout.Controls.Add(new Label { Text = "Email", AutoSize = true });
        layout.Controls.Add(_txtEmail);
        layout.Controls.Add(new Label { Text = "Password", AutoSize = true, Margin = new Padding(0, 8, 0, 0) });
        layout.Controls.Add(_txtPassword);
        layout.Controls.Add(buttons);
        layout.Controls.Add(_lblError);

        card.Controls.Add(layout);
        outer.Controls.Add(card, 1, 1);
        Controls.Add(outer);
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // LoginForm
        // 
        ClientSize = new Size(1533, 598);
        Name = "LoginForm";
        Load += LoginForm_Load;
        ResumeLayout(false);

    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        _lblError.Text = string.Empty;

        User? user = UserRepository.Login(_txtEmail.Text.Trim(), _txtPassword.Text);
        if (user is null)
        {
            _lblError.Text = "Invalid email or password.";
            return;
        }

        SessionManager.SetCurrentUser(user);
        DashboardForm dashboard = new DashboardForm();
        dashboard.Show();
        this.Hide();
    }

    private void LoginForm_Load(object sender, EventArgs e)
    {

    }
}
