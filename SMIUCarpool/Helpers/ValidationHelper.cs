namespace SMIUCarpool.Helpers;

public static class ValidationHelper
{
    public static bool IsValidEmail(string value)
    {
        return !string.IsNullOrWhiteSpace(value) && value.Contains('@') && value.Contains('.') && value.Length > 5;
    }

    public static bool IsValidPhone(string value)
    {
        return !string.IsNullOrWhiteSpace(value)
               && value.StartsWith("03")
               && value.Length == 11
               && value.All(char.IsDigit);
    }

    public static bool IsFutureDateTime(DateTime value)
    {
        return value > DateTime.Now;
    }

    public static bool IsPositiveDecimal(string text, out double value)
    {
        bool ok = double.TryParse(text, out value);
        return ok && value > 0;
    }
}
