namespace SMIUCarpool.Helpers;

public static class AppTheme
{
    public const string AppName = "SMIU Carpool Matchmaker";

    public static readonly Color Primary = Color.FromArgb(36, 82, 135);
    public static readonly Color PrimaryDark = Color.FromArgb(25, 55, 92);
    public static readonly Color Accent = Color.FromArgb(18, 122, 112);
    public static readonly Color Background = Color.FromArgb(244, 246, 249);
    public static readonly Color Panel = Color.White;
    public static readonly Color Border = Color.FromArgb(214, 221, 230);
    public static readonly Color Text = Color.FromArgb(35, 40, 48);
    public static readonly Color MutedText = Color.FromArgb(95, 103, 115);
    public static readonly Color Danger = Color.FromArgb(165, 45, 45);

    public static Font TitleFont => new("Segoe UI", 16, FontStyle.Bold);
    public static Font HeadingFont => new("Segoe UI", 11, FontStyle.Bold);
    public static Font BodyFont => new("Segoe UI", 10);
    public static Font SmallFont => new("Segoe UI", 9);

    public static void PrepareForm(Form form, string title)
    {
        form.Text = $"{AppName} - {title}";
        form.StartPosition = FormStartPosition.CenterScreen;
        form.BackColor = Background;
        form.Font = BodyFont;
        form.ForeColor = Text;
    }

    public static void StylePrimaryButton(Button button)
    {
        button.Height = 38;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.BackColor = Primary;
        button.ForeColor = Color.White;
        button.Font = BodyFont;
        button.Cursor = Cursors.Hand;
        button.Padding = new Padding(10, 0, 10, 0);
    }

    public static void StyleSecondaryButton(Button button)
    {
        StylePrimaryButton(button);
        button.BackColor = Color.FromArgb(232, 237, 245);
        button.ForeColor = Text;
    }

    public static void StyleDangerButton(Button button)
    {
        StylePrimaryButton(button);
        button.BackColor = Danger;
    }

    public static void StyleMenuButton(Button button, bool primary = false)
    {
        button.Height = 42;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = primary ? Primary : Border;
        button.FlatAppearance.MouseOverBackColor = primary ? PrimaryDark : Color.FromArgb(222, 230, 240);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(198, 214, 232);
        button.BackColor = primary ? Primary : Color.White;
        button.ForeColor = primary ? Color.White : Text;
        button.Font = BodyFont;
        button.TextAlign = ContentAlignment.MiddleLeft;
        button.Cursor = Cursors.Hand;
        button.Padding = new Padding(14, 0, 10, 0);
    }

    public static void StyleMenuDangerButton(Button button)
    {
        StyleMenuButton(button, true);
        button.BackColor = Danger;
        button.FlatAppearance.BorderColor = Danger;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(135, 35, 35);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(115, 30, 30);
    }

    public static void StyleTextBox(TextBox textBox)
    {
        textBox.Font = BodyFont;
        textBox.Width = 260;
        textBox.Height = 28;
        textBox.BorderStyle = BorderStyle.FixedSingle;
    }

    public static void StyleComboBox(ComboBox comboBox)
    {
        comboBox.Font = BodyFont;
        comboBox.Width = 260;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Panel;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.GridColor = Border;
        grid.RowHeadersVisible = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryDark;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = HeadingFont;
        grid.DefaultCellStyle.Font = BodyFont;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(206, 226, 245);
        grid.DefaultCellStyle.SelectionForeColor = Text;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        grid.RowTemplate.Height = 32;
        grid.ColumnHeadersHeight = 36;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
    }
}
