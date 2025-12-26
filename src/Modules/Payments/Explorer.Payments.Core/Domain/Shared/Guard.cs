namespace Explorer.Payments.Core.Domain.Shared;

public static class Guard
{
    public static void AgainstNull(object? value, string paramName)
    {
        if (value is null)
            throw new ArgumentException($"{paramName} cannot be null.", paramName);
    }

    public static void AgainstZero(object? value, string paramName)
    {
        AgainstNull(value, paramName);

        if (Convert.ToDouble(value) == 0)
            throw new ArgumentException($"{paramName} cannot be 0.", paramName);
    }

    public static void AgainstNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be empty.", paramName);
    }

    public static void AgainstNegative(double value, string paramName)
    {
        if (value < 0)
            throw new ArgumentException($"{paramName} cannot be negative.", paramName);
    }
}
