namespace SMIUCarpool.Models;

public class Passenger : User
{
    public Passenger(int userId, string fullName, string email, string phone)
        : base(userId, fullName, email, phone, "Passenger")
    {
    }

    public override string GetDashboardText()
    {
        return "Passenger | Can book available rides";
    }
}
