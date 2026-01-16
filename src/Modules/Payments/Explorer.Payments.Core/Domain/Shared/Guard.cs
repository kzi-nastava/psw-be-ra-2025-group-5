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

    public static void AgainstNullOrEmpty<T>(IEnumerable<T>? collection, string paramName)
    {
        if (collection is null) throw new ArgumentNullException(paramName);

        if (!collection.Any()) throw new ArgumentException("Collection cannot be empty.", paramName);
    }

    public static void AgainstOutOfRange<T>(T value, T min, T max, string paramName) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {min} and {max}.");
    }
}
