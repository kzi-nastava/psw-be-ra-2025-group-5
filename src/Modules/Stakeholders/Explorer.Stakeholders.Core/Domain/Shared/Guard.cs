namespace Explorer.Stakeholders.Core.Domain.Shared;

public static class Guard
{
    public static void AgainstNull(object? value, string paramName)
    {
        if (value is null || (value is int && (int)value == 0))
            throw new ArgumentException($"{paramName} cannot be null.", paramName);
    }

    public static void AgainstOutOfRange<T>(T value, string paramName, T? min = null, T? max = null) where T : struct, IComparable<T>
    {
        T actualMin = min ?? (T)typeof(T).GetField("MinValue")!.GetValue(null)!;
        T actualMax = max ?? (T)typeof(T).GetField("MaxValue")!.GetValue(null)!;

        if (value.CompareTo(actualMin) < 0 || value.CompareTo(actualMax) > 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {actualMin} and {actualMax}.");
    }
}
