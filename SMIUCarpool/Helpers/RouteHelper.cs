namespace SMIUCarpool.Helpers;

public static class RouteHelper
{
    private static readonly List<string> Areas = new()
    {
        "North Nazimabad",
        "Nazimabad",
        "Gulshan-e-Iqbal",
        "Gulistan-e-Johar",
        "DHA Phase 5",
        "Clifton",
        "Federal B Area",
        "Korangi",
        "PECHS",
        "Saddar",
        "Malir",
        "Landhi",
        "SMIU Main Campus"
    };

    public static List<string> GetAreaList()
    {
        return Areas;
    }
}
