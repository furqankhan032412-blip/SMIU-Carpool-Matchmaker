using SMIUCarpool.Data;
using SMIUCarpool.Helpers;
using SMIUCarpool.Models;

namespace SMIUCarpool.Forms;

public class PostRideForm : Form
{
    private readonly ComboBox _cmbStart = new();
    private readonly ComboBox _cmbEnd = new();
    private readonly DateTimePicker _dtDeparture = new();
    private readonly NumericUpDown _numSeats = new() { Minimum = 1, Maximum = 4, Value = 1, Width = 260 };
    private readonly TextBox _txtPrice = new();

    public PostRideForm()
    {
        AppTheme.PrepareForm(this, "Post Ride");
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(580, 450);
        BuildLayout();
    }

    private void BuildLayout()
    {
        TableLayoutPanel layout = new() { Dock = DockStyle.Fill, Padding = new Padding(28), ColumnCount = 2 };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64));

        List<string> areas = RouteHelper.GetAreaList();
        _cmbStart.Items.AddRange(areas.Cast<object>().ToArray());
        _cmbEnd.Items.AddRange(areas.Cast<object>().ToArray());
        _cmbStart.SelectedIndex = 0;
        _cmbEnd.SelectedIndex = 1;
        _dtDeparture.Width = 260;
        _dtDeparture.Format = DateTimePickerFormat.Custom;
        _dtDeparture.CustomFormat = "dd MMM yyyy hh:mm tt";
        _dtDeparture.Value = DateTime.Now.AddHours(2);

        AppTheme.StyleComboBox(_cmbStart);
        AppTheme.StyleComboBox(_cmbEnd);
        AppTheme.StyleTextBox(_txtPrice);

        Button btnSave = new() { Text = "Post Ride", Width = 130 };
        AppTheme.StylePrimaryButton(btnSave);
        btnSave.Click += BtnSave_Click;

        layout.Controls.Add(UiHelpers.Title("Post a Ride"), 0, 0);
        layout.SetColumnSpan(layout.Controls[0], 2);
        layout.Controls.Add(UiHelpers.Muted("Enter the basic ride information for passengers."), 0, 1);
        layout.SetColumnSpan(layout.Controls[1], 2);
        AddRow(layout, 2, "Start Point", _cmbStart);
        AddRow(layout, 3, "End Point", _cmbEnd);
        AddRow(layout, 4, "Departure Time", _dtDeparture);
        AddRow(layout, 5, "Seats", _numSeats);
        AddRow(layout, 6, "Price Per Seat", _txtPrice);
        layout.Controls.Add(btnSave, 1, 7);
        Controls.Add(layout);
    }

    private static void AddRow(TableLayoutPanel layout, int row, string label, Control control)
    {
        Label labelControl = new() { Text = label, AutoSize = true, Margin = new Padding(0, 8, 0, 0) };
        control.Margin = new Padding(0, 5, 0, 4);
        layout.Controls.Add(labelControl, 0, row);
        layout.Controls.Add(control, 1, row);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_cmbStart.Text == _cmbEnd.Text)
        {
            MessageBox.Show("Start and end points must be different.");
            return;
        }

        if (!ValidationHelper.IsFutureDateTime(_dtDeparture.Value))
        {
            MessageBox.Show("Departure time must be in the future.");
            return;
        }

        if (!ValidationHelper.IsPositiveDecimal(_txtPrice.Text, out double price))
        {
            MessageBox.Show("Enter a valid price.");
            return;
        }

        if (SessionManager.CurrentUser is not Rider rider)
        {
            MessageBox.Show("Only riders can post rides.");
            return;
        }

        bool saved = RideRepository.CreateRide(new Ride
        {
            RiderID = rider.UserID,
            VehicleType = rider.VehicleType,
            StartPoint = _cmbStart.Text,
            EndPoint = _cmbEnd.Text,
            DepartureTime = _dtDeparture.Value,
            AvailableSeats = (int)_numSeats.Value,
            PricePerSeat = price,
            Status = "Active"
        });

        if (!saved)
        {
            MessageBox.Show("Ride could not be saved.");
            return;
        }

        MessageBox.Show("Ride posted successfully.");
        DialogResult = DialogResult.OK;
        Close();
    }
}
