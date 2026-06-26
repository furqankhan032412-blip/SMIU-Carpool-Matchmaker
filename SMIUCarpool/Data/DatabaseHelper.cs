using System.Data;
using System.Data.SQLite;

namespace SMIUCarpool.Data;

public static class DatabaseHelper
{
    private static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "carpool-matchmaker.db");
    private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

    public static SQLiteConnection GetConnection()
    {
        SQLiteConnection connection = new(ConnectionString);
        connection.Open();
        return connection;
    }

    public static int ExecuteNonQuery(string sql, Dictionary<string, object?>? parameters = null)
    {
        using SQLiteConnection connection = GetConnection();
        using SQLiteCommand command = new(sql, connection);
        AddParameters(command, parameters);
        return command.ExecuteNonQuery();
    }

    public static object? ExecuteScalar(string sql, Dictionary<string, object?>? parameters = null)
    {
        using SQLiteConnection connection = GetConnection();
        using SQLiteCommand command = new(sql, connection);
        AddParameters(command, parameters);
        return command.ExecuteScalar();
    }

    public static DataTable ExecuteQuery(string sql, Dictionary<string, object?>? parameters = null)
    {
        using SQLiteConnection connection = GetConnection();
        using SQLiteCommand command = new(sql, connection);
        AddParameters(command, parameters);
        DataTable table = new();
        using SQLiteDataAdapter adapter = new(command);
        adapter.Fill(table);
        return table;
    }

    private static void AddParameters(SQLiteCommand command, Dictionary<string, object?>? parameters)
    {
        if (parameters is null)
        {
            return;
        }

        foreach (KeyValuePair<string, object?> item in parameters)
        {
            command.Parameters.AddWithValue(item.Key, item.Value ?? DBNull.Value);
        }
    }

    public static void InitializeDatabase()
    {
        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS Users (
            UserID INTEGER PRIMARY KEY AUTOINCREMENT,
            FullName TEXT NOT NULL,
            Email TEXT NOT NULL UNIQUE,
            Password TEXT NOT NULL,
            PhoneNumber TEXT NOT NULL,
            Role TEXT NOT NULL,
            VehicleType TEXT,
            CreatedAt TEXT NOT NULL
        );");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS Rides (
            RideID INTEGER PRIMARY KEY AUTOINCREMENT,
            RiderID INTEGER NOT NULL,
            VehicleType TEXT NOT NULL,
            StartPoint TEXT NOT NULL,
            EndPoint TEXT NOT NULL,
            DepartureTime TEXT NOT NULL,
            AvailableSeats INTEGER NOT NULL,
            PricePerSeat REAL NOT NULL,
            Status TEXT NOT NULL,
            CreatedAt TEXT NOT NULL
        );");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS Bookings (
            BookingID INTEGER PRIMARY KEY AUTOINCREMENT,
            RideID INTEGER NOT NULL,
            PassengerID INTEGER NOT NULL,
            SeatsBooked INTEGER NOT NULL,
            Status TEXT NOT NULL,
            BookedAt TEXT NOT NULL
        );");
    }
}
