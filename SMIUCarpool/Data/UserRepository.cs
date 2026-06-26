using System.Data;
using SMIUCarpool.Models;

namespace SMIUCarpool.Data;

public static class UserRepository
{
    public static int GetUserCount()
    {
        object? value = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Users");
        return Convert.ToInt32(value);
    }

    public static bool CreateUser(string name, string email, string phone, string role, string? vehicleType, string password)
    {
        string sql = @"INSERT INTO Users (FullName, Email, Password, PhoneNumber, Role, VehicleType, CreatedAt)
                       VALUES (@name, @email, @password, @phone, @role, @vehicle, @date)";

        return DatabaseHelper.ExecuteNonQuery(sql, new Dictionary<string, object?>
        {
            ["@name"] = name,
            ["@email"] = email,
            ["@password"] = password,
            ["@phone"] = phone,
            ["@role"] = role,
            ["@vehicle"] = string.IsNullOrWhiteSpace(vehicleType) ? null : vehicleType,
            ["@date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        }) > 0;
    }

    public static bool EmailExists(string email)
    {
        object? value = DatabaseHelper.ExecuteScalar(
            "SELECT COUNT(*) FROM Users WHERE Email=@email",
            new Dictionary<string, object?> { ["@email"] = email });
        return Convert.ToInt32(value) > 0;
    }

    public static User? Login(string email, string password)
    {
        DataTable table = DatabaseHelper.ExecuteQuery(
            "SELECT * FROM Users WHERE Email=@email AND Password=@password",
            new Dictionary<string, object?> { ["@email"] = email, ["@password"] = password });

        return table.Rows.Count == 0 ? null : MapUser(table.Rows[0]);
    }

    public static User? GetUserByEmail(string email)
    {
        DataTable table = DatabaseHelper.ExecuteQuery(
            "SELECT * FROM Users WHERE Email=@email",
            new Dictionary<string, object?> { ["@email"] = email });

        return table.Rows.Count == 0 ? null : MapUser(table.Rows[0]);
    }

    public static User? GetUserById(int userId)
    {
        DataTable table = DatabaseHelper.ExecuteQuery(
            "SELECT * FROM Users WHERE UserID=@id",
            new Dictionary<string, object?> { ["@id"] = userId });

        return table.Rows.Count == 0 ? null : MapUser(table.Rows[0]);
    }

    private static User MapUser(DataRow row)
    {
        int id = Convert.ToInt32(row["UserID"]);
        string name = row["FullName"].ToString() ?? string.Empty;
        string email = row["Email"].ToString() ?? string.Empty;
        string phone = row["PhoneNumber"].ToString() ?? string.Empty;
        string role = row["Role"].ToString() ?? "Passenger";
        string vehicleType = row["VehicleType"].ToString() ?? string.Empty;

        return role == "Rider"
            ? new Rider(id, name, email, phone, vehicleType)
            : new Passenger(id, name, email, phone);
    }
}
