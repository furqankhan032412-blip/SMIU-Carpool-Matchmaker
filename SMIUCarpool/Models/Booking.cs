namespace SMIUCarpool.Models;

public class Booking
{
    public int BookingID { get; set; }
    public int RideID { get; set; }
    public int PassengerID { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string StartPoint { get; set; } = string.Empty;
    public string EndPoint { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public int SeatsBooked { get; set; }
    public string Status { get; set; } = "Confirmed";
    public DateTime BookedAt { get; set; }
    public double PricePerSeat { get; set; }
}
