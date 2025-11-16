namespace Explorer.Tours.Core.Domain.Shared;

public static class Guard
{
    public static void AgainstNull(object? value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
    }

    public static void AgainstNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be empty.", paramName);
    }

    public static void AgainstDuplicateStrings(IEnumerable<string> collection, string paramName)
    {
        AgainstNull(collection, nameof(collection));

        var duplicates = collection.GroupBy(s => s.Trim().ToLowerInvariant()).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

        if (duplicates.Any())
            throw new ArgumentException($"{paramName} contains duplicate values: {string.Join(", ", duplicates)}", paramName);
    }

    public static void AgainstInvalidEnum<TEnum>(TEnum value, string paramName) where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
            throw new ArgumentException($"{paramName} has an invalid value.", paramName);
    }
}
