using SMIUCarpool.Data;
using SMIUCarpool.Helpers;
using SMIUCarpool.Models;

namespace SMIUCarpool.Forms;

public class MyRidesForm : Form
{
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        AutoGenerateColumns = true,
        ReadOnly = true,
        AllowUserToAddRows = false
    };

    public MyRidesForm()
    {
        AppTheme.PrepareForm(this, "My Rides");
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(860, 480);
        BuildLayout();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        LoadRides();
    }

    private void BuildLayout()
    {
        AppTheme.StyleGrid(_grid);
        Panel content = new() { Dock = DockStyle.Fill, Padding = new Padding(18) };
        Button btnCancel = new() { Text = "Cancel Selected Ride", Dock = DockStyle.Bottom, Height = 42 };
        AppTheme.StyleDangerButton(btnCancel);
        btnCancel.Click += BtnCancel_Click;
        content.Controls.Add(_grid);
        content.Controls.Add(btnCancel);
        Controls.Add(content);
        Controls.Add(UiHelpers.Header("My Rides", "Rider-posted rides are shown here."));
    }

    private void LoadRides()
    {
        if (SessionManager.CurrentUser is not null)
        {
            _grid.DataSource = RideRepository.GetRidesByRider(SessionManager.CurrentUser.UserID);
            HideUnusedColumns();
        }
    }

    private void HideUnusedColumns()
    {
        foreach (string name in new[] { "RideID", "RiderID", "RiderPhone" })
        {
            if (_grid.Columns.Contains(name))
            {
                _grid.Columns[name].Visible = false;
            }
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        if (_grid.CurrentRow?.DataBoundItem is not Ride ride)
        {
            MessageBox.Show("Please select a ride first.");
            return;
        }

        RideRepository.CancelRide(ride.RideID);
        LoadRides();
    }
}
