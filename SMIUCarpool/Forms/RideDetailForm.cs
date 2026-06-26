using SMIUCarpool.Data;
using SMIUCarpool.Helpers;
using SMIUCarpool.Models;

namespace SMIUCarpool.Forms;

public class RideDetailForm : Form
{
    private readonly int _rideId;
    private readonly TableLayoutPanel _details = new() { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
    private Ride? _ride;

    public RideDetailForm(int rideId)
    {
        _rideId = rideId;
        AppTheme.PrepareForm(this, "Ride Details");
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(620, 460);
        BuildLayout();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        LoadRide();
    }

    private void BuildLayout()
    {
        Panel content = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(28),
            BackColor = AppTheme.Background
        };

        Panel card = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            BackColor = AppTheme.Panel
        };

        _details.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        _details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        Button btnBook = new() { Text = "Book This Ride", Width = 145, Dock = DockStyle.Bottom };
        AppTheme.StylePrimaryButton(btnBook);
        btnBook.Click += BtnBook_Click;

        card.Controls.Add(btnBook);
        card.Controls.Add(_details);
        content.Controls.Add(card);
        Controls.Add(content);
        Controls.Add(UiHelpers.Header("Ride Details", "Review the ride before booking."));
    }

    private void LoadRide()
    {
        _ride = RideRepository.GetRideById(_rideId);
        _details.Controls.Clear();

        if (_ride is null)
        {
            AddDetail("Status", "Ride not found.");
            return;
        }

        AddDetail("Rider", _ride.RiderName);
        AddDetail("Phone", _ride.RiderPhone);
        AddDetail("Vehicle", _ride.VehicleType);
        AddDetail("Route", _ride.RouteText);
        AddDetail("Departure", _ride.DepartureTime.ToString("dd MMM yyyy hh:mm tt"));
        AddDetail("Seats Left", _ride.AvailableSeats.ToString());
        AddDetail("Price", _ride.PriceText);
        AddDetail("Status", _ride.Status);
    }

    private void AddDetail(string label, string value)
    {
        int row = _details.RowCount++;
        _details.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Label labelControl = new()
        {
            Text = label,
            Font = AppTheme.HeadingFont,
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Margin = new Padding(0, 0, 10, 12)
        };

        Label valueControl = new()
        {
            Text = value,
            Font = AppTheme.BodyFont,
            ForeColor = AppTheme.Text,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 12)
        };

        _details.Controls.Add(labelControl, 0, row);
        _details.Controls.Add(valueControl, 1, row);
    }

    private void BtnBook_Click(object? sender, EventArgs e)
    {
        if (_ride is null || SessionManager.CurrentUser is null)
        {
            return;
        }

        if (SessionManager.CurrentUser.Role == "Rider")
        {
            MessageBox.Show("Riders cannot book rides.");
            return;
        }

        string message = BookingRepository.CreateBooking(_ride.RideID, SessionManager.CurrentUser.UserID);
        MessageBox.Show(message);
        LoadRide();
    }
}
