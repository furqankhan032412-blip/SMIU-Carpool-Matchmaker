using SMIUCarpool.Helpers;

namespace SMIUCarpool.Forms;

internal static class UiHelpers
{
    public static Label Title(string text)
    {
        return new Label
        {
            Text = text,
            Font = AppTheme.TitleFont,
            ForeColor = AppTheme.PrimaryDark,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6)
        };
    }

    public static Label Muted(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 14)
        };
    }

    public static Panel Header(string title, string subtitle = "")
    {
        Panel header = new()
        {
            Dock = DockStyle.Top,
            Height = string.IsNullOrWhiteSpace(subtitle) ? 72 : 90,
            Padding = new Padding(20, 10, 20, 10),
            BackColor = AppTheme.Panel
        };

        FlowLayoutPanel row = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoSize = false
        };

        PictureBox logo = new()
        {
            Width = 58,
            Height = 58,
            SizeMode = PictureBoxSizeMode.Zoom,
            Margin = new Padding(0, 0, 12, 0)
        };

        string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "smiu-carpool-logo.png");
        if (File.Exists(logoPath))
        {
            logo.Image = Image.FromFile(logoPath);
        }

        FlowLayoutPanel stack = new()
        {
            Width = 720,
            Height = 66,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Margin = new Padding(0, 4, 0, 0)
        };

        stack.Controls.Add(Title(title));
        if (!string.IsNullOrWhiteSpace(subtitle))
        {
            stack.Controls.Add(Muted(subtitle));
        }

        row.Controls.Add(logo);
        row.Controls.Add(stack);
        header.Controls.Add(row);
        return header;
    }
}
