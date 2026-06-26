using System.Data;
using SMIUCarpool.Models;

namespace SMIUCarpool.Data;

public static class RideRepository
{
    public static bool CreateRide(Ride ride)
    {
        string sql = @"INSERT INTO Rides (RiderID, VehicleType, StartPoint, EndPoint, DepartureTime, AvailableSeats, PricePerSeat, Status, CreatedAt)
                       VALUES (@rider, @vehicle, @start, @end, @time, @seats, @price, @status, @created)";

        return DatabaseHelper.ExecuteNonQuery(sql, new Dictionary<string, object?>
        {
            ["@rider"] = ride.RiderID,
            ["@vehicle"] = ride.VehicleType,
            ["@start"] = ride.StartPoint,
            ["@end"] = ride.EndPoint,
            ["@time"] = ride.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss"),
            ["@seats"] = ride.AvailableSeats,
            ["@price"] = ride.PricePerSeat,
            ["@status"] = ride.Status,
            ["@created"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        }) > 0;
    }

    public static List<Ride> GetAllActiveRides()
    {
        string sql = @"SELECT r.*, u.FullName, u.PhoneNumber
                       FROM Rides r
                       JOIN Users u ON u.UserID = r.RiderID
                       WHERE r.Status='Active'
                       ORDER BY r.DepartureTime ASC";
        return ReadRides(sql);
    }

    public static List<Ride> SearchActiveRides(string vehicle, string startPoint, string searchText)
    {
        List<string> conditions = new List<string> { "r.Status='Active'" };
        Dictionary<string, object?> parameters = new();

        if (!string.IsNullOrWhiteSpace(vehicle) && vehicle != "All")
        {
            conditions.Add("r.VehicleType=@vehicle");
            parameters["@vehicle"] = vehicle;
        }

        if (!string.IsNullOrWhiteSpace(startPoint) && startPoint != "All")
        {
            conditions.Add("r.StartPoint=@start");
            parameters["@start"] = startPoint;
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            conditions.Add(@"(r.StartPoint LIKE @search OR r.EndPoint LIKE @search OR u.FullName LIKE @search OR r.VehicleType LIKE @search)");
            parameters["@search"] = $"%{searchText.Trim()}%";
        }

        string sql = $@"SELECT r.*, u.FullName, u.PhoneNumber
                        FROM Rides r
                        JOIN Users u ON u.UserID = r.RiderID
                        WHERE {string.Join(" AND ", conditions)}
                        ORDER BY r.DepartureTime ASC";
        return ReadRides(sql, parameters);
    }

    public static List<Ride> GetRidesByRider(int riderId)
    {
        string sql = @"SELECT r.*, u.FullName, u.PhoneNumber
                       FROM Rides r
                       JOIN Users u ON u.UserID = r.RiderID
                       WHERE r.RiderID=@id
                       ORDER BY r.DepartureTime DESC";
        return ReadRides(sql, new Dictionary<string, object?> { ["@id"] = riderId });
    }

    public static Ride? GetRideById(int rideId)
    {
        string sql = @"SELECT r.*, u.FullName, u.PhoneNumber
                       FROM Rides r
                       JOIN Users u ON u.UserID = r.RiderID
                       WHERE r.RideID=@id";
        return ReadRides(sql, new Dictionary<string, object?> { ["@id"] = rideId }).FirstOrDefault();
    }

    public static bool CancelRide(int rideId)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "UPDATE Rides SET Status='Cancelled' WHERE RideID=@id",
            new Dictionary<string, object?> { ["@id"] = rideId }) > 0;
    }

    public static bool UpdateSeats(int rideId, int seats)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "UPDATE Rides SET AvailableSeats=@seats WHERE RideID=@id",
            new Dictionary<string, object?> { ["@seats"] = seats, ["@id"] = rideId }) > 0;
    }

    public static bool RideExists(int riderId, string start, string end, DateTime departureTime)
    {
        object? value = DatabaseHelper.ExecuteScalar(
            @"SELECT COUNT(*) FROM Rides
              WHERE RiderID=@rider AND StartPoint=@start AND EndPoint=@end AND DepartureTime=@time",
            new Dictionary<string, object?>
            {
                ["@rider"] = riderId,
                ["@start"] = start,
                ["@end"] = end,
                ["@time"] = departureTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
        return Convert.ToInt32(value) > 0;
    }

    private static List<Ride> ReadRides(string sql, Dictionary<string, object?>? parameters = null)
    {
        DataTable table = DatabaseHelper.ExecuteQuery(sql, parameters);
        return table.Rows.Cast<DataRow>().Select(MapRide).ToList();
    }

    private static Ride MapRide(DataRow row)
    {
        return new Ride
        {
            RideID = Convert.ToInt32(row["RideID"]),
            RiderID = Convert.ToInt32(row["RiderID"]),
            VehicleType = row["VehicleType"].ToString() ?? string.Empty,
            StartPoint = row["StartPoint"].ToString() ?? string.Empty,
            EndPoint = row["EndPoint"].ToString() ?? string.Empty,
            DepartureTime = DateTime.Parse(row["DepartureTime"].ToString() ?? DateTime.Now.ToString("s")),
            AvailableSeats = Convert.ToInt32(row["AvailableSeats"]),
            PricePerSeat = Convert.ToDouble(row["PricePerSeat"]),
            Status = row["Status"].ToString() ?? "Active",
            RiderName = row["FullName"].ToString() ?? string.Empty,
            RiderPhone = row["PhoneNumber"].ToString() ?? string.Empty
        };
    }
}
