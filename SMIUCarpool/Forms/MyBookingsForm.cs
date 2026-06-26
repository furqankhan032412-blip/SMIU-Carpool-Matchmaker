using SMIUCarpool.Data;
using SMIUCarpool.Helpers;

namespace SMIUCarpool.Forms;

public class MyBookingsForm : Form
{
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        AutoGenerateColumns = true,
        ReadOnly = true,
        AllowUserToAddRows = false
    };

    public MyBookingsForm()
    {
        AppTheme.PrepareForm(this, "My Bookings");
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(860, 480);
        BuildLayout();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (SessionManager.CurrentUser is not null)
        {
            _grid.DataSource = BookingRepository.GetBookingsByPassenger(SessionManager.CurrentUser.UserID);
            HideUnusedColumns();
        }
    }

    private void BuildLayout()
    {
        AppTheme.StyleGrid(_grid);
        Panel content = new() { Dock = DockStyle.Fill, Padding = new Padding(18) };
        content.Controls.Add(_grid);
        Controls.Add(content);
        Controls.Add(UiHelpers.Header("My Bookings", "Passenger bookings are shown here."));
    }

    private void HideUnusedColumns()
    {
        foreach (string name in new[] { "BookingID", "RideID", "PassengerID" })
        {
            if (_grid.Columns.Contains(name))
            {
                _grid.Columns[name].Visible = false;
            }
        }

        if (_grid.Columns.Contains("PersonName"))
        {
            _grid.Columns["PersonName"].HeaderText = "Rider";
        }
        if (_grid.Columns.Contains("PricePerSeat"))
        {
            _grid.Columns["PricePerSeat"].HeaderText = "Price";
        }
    }
}
