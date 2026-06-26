namespace SMIUCarpool.Models;

public class Rider : User
{
    public Rider(int userId, string fullName, string email, string phone, string vehicleType)
        : base(userId, fullName, email, phone, "Rider")
    {
        VehicleType = vehicleType;
    }

    public string VehicleType { get; set; }

    public override string GetDashboardText()
    {
        return $"Rider | Vehicle: {VehicleType}";
    }
}
