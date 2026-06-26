using SMIUCarpool.Models;

namespace SMIUCarpool.Data;

public static class DatabaseSeeder
{
    public static void SeedData()
    {
        RemoveOldDemoData();

        AddUser("Muhammad Hasnain", "hasnain@smiu.edu", "03001234567", "Rider", "Car");
        AddUser("Jawad Raza", "jawad@smiu.edu", "03111234567", "Rider", "Bike");
        AddUser("Wasiq Shah", "wasiq@smiu.edu", "03099887766", "Passenger", null);
        AddUser("Hassan Mehmood", "hassan@smiu.edu", "03122334455", "Passenger", null);
        AddUser("Zaid Ghanchi", "zaid@smiu.edu", "03211234567", "Rider", "Car");
        AddUser("Anum Zakir", "anum@smiu.edu", "03331234567", "Rider", "Car");
        AddUser("Usman Malik", "usman@smiu.edu", "03451234567", "Rider", "Bike");
        AddUser("Isha Noor", "isha@smiu.edu", "03077665544", "Rider", "Car");
        AddUser("Saim Abbas", "saim@smiu.edu", "03155667788", "Rider", "Bike");
        AddUser("Zara Amjad", "zara@smiu.edu", "03233445566", "Passenger", null);
        AddUser("Omer Farooq", "omer@smiu.edu", "03344556677", "Passenger", null);
        AddUser("Maryam Tariq", "maryam@smiu.edu", "03455667788", "Passenger", null);
        AddUser("Ahmed Ali", "ahmed@smiu.edu", "03001234567", "Rider", "Car");
        AddUser("Sara Khan", "sara@smiu.edu", "03111234567", "Rider", "Bike");

        AddRide("hasnain@smiu.edu", "Car", "North Nazimabad", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(7.5), 3, 250);
        AddRide("jawad@smiu.edu", "Bike", "Gulshan-e-Iqbal", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(8), 1, 180);
        AddRide("zaid@smiu.edu", "Car", "DHA Phase 5", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(8.25), 2, 320);
        AddRide("anum@smiu.edu", "Car", "Federal B Area", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(8.5), 3, 230);
        AddRide("usman@smiu.edu", "Bike", "Korangi", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(8.75), 1, 200);
        AddRide("hasnain@smiu.edu", "Car", "Nazimabad", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(9), 2, 210);
        AddRide("isha@smiu.edu", "Car", "Clifton", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(9.25), 3, 300);
        AddRide("saim@smiu.edu", "Bike", "PECHS", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(9.5), 1, 170);
        AddRide("zaid@smiu.edu", "Car", "Gulistan-e-Johar", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(10), 4, 260);
        AddRide("anum@smiu.edu", "Car", "Saddar", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(10.5), 2, 120);
        AddRide("isha@smiu.edu", "Car", "Malir", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(11), 3, 280);
        AddRide("usman@smiu.edu", "Bike", "Landhi", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(11.5), 1, 220);
        AddRide("ahmed@smiu.edu", "Car", "Saddar", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(12), 3, 190);
        AddRide("sara@smiu.edu", "Bike", "Gulshan-e-Iqbal", "SMIU Main Campus", DateTime.Today.AddDays(1).AddHours(12.5), 1, 150);
    }

    private static void RemoveOldDemoData()
    {
        string[] oldEmails = new[]
        {
            "bilal@smiu.edu",
            "fatima@smiu.edu",
            "hina@smiu.edu",
            "danish@smiu.edu",
            "ayesha@smiu.edu"
        };

        foreach (string email in oldEmails)
        {
            User? user = UserRepository.GetUserByEmail(email);
            if (user is null)
            {
                continue;
            }

            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM Bookings WHERE PassengerID=@id OR RideID IN (SELECT RideID FROM Rides WHERE RiderID=@id)",
                new Dictionary<string, object?> { ["@id"] = user.UserID });
            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM Rides WHERE RiderID=@id",
                new Dictionary<string, object?> { ["@id"] = user.UserID });
            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM Users WHERE UserID=@id",
                new Dictionary<string, object?> { ["@id"] = user.UserID });
        }
    }

    private static void AddUser(string name, string email, string phone, string role, string? vehicleType)
    {
        if (UserRepository.EmailExists(email))
        {
            return;
        }

        UserRepository.CreateUser(name, email, phone, role, vehicleType, "password123");
    }

    private static void AddRide(string email, string vehicle, string start, string end, DateTime time, int seats, double price)
    {
        User? rider = UserRepository.GetUserByEmail(email);
        if (rider is null)
        {
            return;
        }

        if (RideRepository.RideExists(rider.UserID, start, end, time))
        {
            return;
        }

        RideRepository.CreateRide(new Ride
        {
            RiderID = rider.UserID,
            VehicleType = vehicle,
            StartPoint = start,
            EndPoint = end,
            DepartureTime = time,
            AvailableSeats = seats,
            PricePerSeat = price,
            Status = "Active"
        });
    }
}
