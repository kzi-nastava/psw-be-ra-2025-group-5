using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;

namespace Explorer.Tours.Core.Domain.Tours.Entities;

public class KeyPoint : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Location Location { get; private set; }
    public byte[]? Image { get; private set; }
    public string? Secret { get; private set; }
    public int Position { get; private set; }

    private KeyPoint() { }

    internal KeyPoint(string name, string description, Location location, byte[]? image, string? secret, int position)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Key point name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Key point description is required.", nameof(description));

        if (location == null)
            throw new ArgumentNullException(nameof(location), "Location is required.");

        if (position < 0)
            throw new ArgumentException("Position must be non-negative.", nameof(position));

        Name = name.Trim();
        Description = description.Trim();
        Location = location;
        Image = image;
        Secret = secret?.Trim();
        Position = position;
    }

    public void Update(string name, string description, byte[]? image, string? secret, Location location)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Key point name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Key point description is required.", nameof(description));

        Name = name.Trim();
        Description = description.Trim();
        Image = image;
        Secret = secret?.Trim();
        Location = location;
    }

    public void UpdatePosition(int position)
    {
        if (position < 0)
            throw new ArgumentException("Position must be non-negative.", nameof(position));

        Position = position;
    }
}
