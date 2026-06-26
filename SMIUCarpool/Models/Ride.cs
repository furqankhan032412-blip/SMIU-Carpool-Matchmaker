namespace SMIUCarpool.Models;

public class Ride
{
    public int RideID { get; set; }
    public int RiderID { get; set; }
    public string RiderName { get; set; } = string.Empty;
    public string RiderPhone { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public string StartPoint { get; set; } = string.Empty;
    public string EndPoint { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public int AvailableSeats { get; set; }
    public double PricePerSeat { get; set; }
    public string Status { get; set; } = "Active";
    public string RouteText => $"{StartPoint} to {EndPoint}";
    public string DepartureText => DepartureTime.ToString("dd MMM hh:mm tt");
    public string PriceText => $"Rs. {PricePerSeat:F0}";

    public override string ToString()
    {
        return $"{StartPoint} to {EndPoint} | {DepartureTime:dd MMM hh:mm tt} | Seats: {AvailableSeats} | Rs. {PricePerSeat:F0}";
    }
}
