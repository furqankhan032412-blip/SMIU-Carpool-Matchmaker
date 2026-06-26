namespace SMIUCarpool.Models;

public abstract class User
{
    protected User(int userId, string fullName, string email, string phone, string role)
    {
        UserID = userId;
        FullName = fullName;
        Email = email;
        Phone = phone;
        Role = role;
    }

    public int UserID { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }

    public abstract string GetDashboardText();
}
