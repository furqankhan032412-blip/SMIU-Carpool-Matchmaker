using System.Data;
using SMIUCarpool.Models;

namespace SMIUCarpool.Data;

public static class BookingRepository
{
    public static string CreateBooking(int rideId, int passengerId)
    {
        Ride? ride = RideRepository.GetRideById(rideId);
        if (ride is null)
        {
            return "Ride not found.";
        }

        if (ride.RiderID == passengerId)
        {
            return "You cannot book your own ride.";
        }

        if (ride.AvailableSeats <= 0)
        {
            return "This ride is full.";
        }

        if (HasBooking(rideId, passengerId))
        {
            return "You already booked this ride.";
        }

        DatabaseHelper.ExecuteNonQuery(
            @"INSERT INTO Bookings (RideID, PassengerID, SeatsBooked, Status, BookedAt)
              VALUES (@ride, @passenger, 1, 'Confirmed', @time)",
            new Dictionary<string, object?>
            {
                ["@ride"] = rideId,
                ["@passenger"] = passengerId,
                ["@time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });

        RideRepository.UpdateSeats(rideId, ride.AvailableSeats - 1);
        return "Ride booked successfully.";
    }

    public static List<Booking> GetBookingsByPassenger(int passengerId)
    {
        string sql = @"SELECT b.*, r.StartPoint, r.EndPoint, r.DepartureTime, r.PricePerSeat, u.FullName AS RiderName
                       FROM Bookings b
                       JOIN Rides r ON r.RideID = b.RideID
                       JOIN Users u ON u.UserID = r.RiderID
                       WHERE b.PassengerID=@id
                       ORDER BY b.BookedAt DESC";
        DataTable table = DatabaseHelper.ExecuteQuery(sql, new Dictionary<string, object?> { ["@id"] = passengerId });
        return table.Rows.Cast<DataRow>().Select(MapBooking).ToList();
    }

    public static List<Booking> GetBookingsByRide(int rideId)
    {
        string sql = @"SELECT b.*, r.StartPoint, r.EndPoint, r.DepartureTime, r.PricePerSeat, u.FullName AS RiderName
                       FROM Bookings b
                       JOIN Rides r ON r.RideID = b.RideID
                       JOIN Users u ON u.UserID = b.PassengerID
                       WHERE b.RideID=@id
                       ORDER BY b.BookedAt DESC";
        DataTable table = DatabaseHelper.ExecuteQuery(sql, new Dictionary<string, object?> { ["@id"] = rideId });
        return table.Rows.Cast<DataRow>().Select(MapBooking).ToList();
    }

    private static bool HasBooking(int rideId, int passengerId)
    {
        object? value = DatabaseHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM Bookings WHERE RideID=@ride AND PassengerID=@passenger",
            new Dictionary<string, object?> { ["@ride"] = rideId, ["@passenger"] = passengerId });
        return Convert.ToInt32(value) > 0;
    }

    private static Booking MapBooking(DataRow row)
    {
        return new Booking
        {
            BookingID = Convert.ToInt32(row["BookingID"]),
            RideID = Convert.ToInt32(row["RideID"]),
            PassengerID = Convert.ToInt32(row["PassengerID"]),
            PersonName = row["RiderName"].ToString() ?? string.Empty,
            StartPoint = row["StartPoint"].ToString() ?? string.Empty,
            EndPoint = row["EndPoint"].ToString() ?? string.Empty,
            DepartureTime = DateTime.Parse(row["DepartureTime"].ToString() ?? DateTime.Now.ToString("s")),
            SeatsBooked = Convert.ToInt32(row["SeatsBooked"]),
            Status = row["Status"].ToString() ?? "Confirmed",
            BookedAt = DateTime.Parse(row["BookedAt"].ToString() ?? DateTime.Now.ToString("s")),
            PricePerSeat = Convert.ToDouble(row["PricePerSeat"])
        };
    }
}
