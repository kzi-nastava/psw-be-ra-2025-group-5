using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.Core.Domain.Shared;

namespace Explorer.Tours.Core.Domain;

public enum TourStatus { Draft, Published, Archived, Closed };
public enum TourDifficulty { Easy, Medium, Hard };

public class Tour : AggregateRoot
{
    public int AuthorId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public TourDifficulty Difficulty { get; private set; }
    public List<string> Tags { get; private set; }
    public double Price { get; private set; }
    public TourStatus Status { get; private set; }
    public DateTime? PublishedDate { get; private set; }
    public DateTime? ArchivedDate { get; private set; }
    public List<KeyPoint> KeyPoints { get; private set; }
    //public List<TourReview> Reviews { get; set; }
    public double? AverageRating { get; private set; }
    public List<TourReview> Reviews { get; private set; }
    public List<TourDuration> Durations { get; private set; }
    public List<RequiredEquipment> RequiredEquipment { get; private set; }
    public double TourLength { get; set; }

    private Tour() 
    {
        Tags = new List<string>();
        KeyPoints = new List<KeyPoint>();
        Reviews = new List<TourReview>();
        AverageRating = 0;
        Durations = new List<TourDuration>();
        RequiredEquipment = new List<RequiredEquipment>();
        TourLength = 0;
    }

    public Tour(int authorId, string name, string? description, TourDifficulty difficulty, List<string> tags, double price = 0.0)
    {
        Guard.AgainstZero(authorId, nameof(authorId));
        Guard.AgainstNegative(price, nameof(price));
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstInvalidEnum(difficulty, nameof(difficulty));
        Guard.AgainstDuplicateStrings(tags, nameof(tags));

        AuthorId = authorId;
        Name = name;
        Description = description;
        Difficulty = difficulty;
        Tags = tags ?? new List<string>();
        Price = price;
        Status = TourStatus.Draft;
        KeyPoints = new List<KeyPoint>();
        Durations = new List<TourDuration>();
        Reviews = new List<TourReview>();
        RequiredEquipment = new List<RequiredEquipment>();
        AverageRating = 0;
        TourLength = 0;
    }

    public void AddKeyPoint(string name, string description, Location location, byte[]? image, string? secret)
    {
        if (Status != TourStatus.Draft)
            throw new InvalidOperationException("Key points can only be added to tours in draft status.");

        int nextPosition = KeyPoints.Any() ? KeyPoints.Max(kp => kp.Position) + 1 : 0;
        var keyPoint = new KeyPoint(name, description, location, image, secret, nextPosition);
        KeyPoints.Add(keyPoint);
    }

    public void RemoveKeyPoint(long keyPointId)
    {
        if (Status != TourStatus.Draft)
            throw new InvalidOperationException("Key points can only be removed from tours in draft status.");

        var keyPoint = KeyPoints.SingleOrDefault(kp => kp.Id == keyPointId);
        if (keyPoint == null)
            throw new InvalidOperationException("Key point not found.");

        KeyPoints.Remove(keyPoint);
        
        ReorderKeyPoints();
    }

    public void UpdateKeyPoint(long keyPointId, string name, string description, byte[]? image, string? secret, Location location)
    {
        if (Status != TourStatus.Draft)
            throw new InvalidOperationException("Key points can only be updated in tours in draft status.");

        var keyPoint = KeyPoints.SingleOrDefault(kp => kp.Id == keyPointId);
        if (keyPoint == null)
            throw new InvalidOperationException("Key point not found.");

        keyPoint.Update(name, description, image, secret, location);
    }

    public void ReorderKeyPoints(List<long> orderedKeyPointIds)
    {
        if (Status != TourStatus.Draft)
            throw new InvalidOperationException("Key points can only be reordered in tours in draft status.");

        if (orderedKeyPointIds.Count != KeyPoints.Count)
            throw new InvalidOperationException("All key points must be included in reorder operation.");

        var existingIds = KeyPoints.Select(kp => kp.Id).ToHashSet();
        if (orderedKeyPointIds.Any(id => !existingIds.Contains(id)))
            throw new InvalidOperationException("Invalid key point ID in reorder list.");

        for (int i = 0; i < orderedKeyPointIds.Count; i++)
        {
            var keyPoint = KeyPoints.Single(kp => kp.Id == orderedKeyPointIds[i]);
            keyPoint.UpdatePosition(i);
        }
    }

    private void ReorderKeyPoints()
    {
        var sortedKeyPoints = KeyPoints.OrderBy(kp => kp.Position).ToList();
        for (int i = 0; i < sortedKeyPoints.Count; i++)
        {
            sortedKeyPoints[i].UpdatePosition(i);
        }
    }

    public void Publish()
    {
        if (Status != TourStatus.Draft)
            throw new InvalidOperationException("Only draft tours can be published.");

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || Difficulty == null || Tags.Count == 0)
            throw new InvalidOperationException("Tour must have name, description, difficulty, and at least one tag to be published.");

        if (KeyPoints.Count < 2)
            throw new InvalidOperationException("Tour must have at least 2 key points to be published.");

        if (Durations.Count < 1)
            throw new InvalidOperationException("Tour must have at least one duration to be published.");

        Status = TourStatus.Published;
        PublishedDate = DateTime.UtcNow;
    }

    public void Archive()
    {
        if (Status != TourStatus.Published)
            throw new InvalidOperationException("Only published tours can be archived.");

        Status = TourStatus.Archived;
        ArchivedDate = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        if (Status != TourStatus.Archived)
            throw new InvalidOperationException("Only archived tours can be reactivated.");

        Status = TourStatus.Published;
        ArchivedDate = null;
    }

    public void Update(string name, string? description, TourDifficulty difficulty, List<string> tags, double price)
    {
        //if (Status != TourStatus.Draft)
        //    throw new InvalidOperationException("Only draft tours can be updated.");

        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstInvalidEnum(difficulty, nameof(difficulty));
        Guard.AgainstDuplicateStrings(tags, nameof(tags));
        Guard.AgainstNegative(price, nameof(price));

        Name = name;
        Description = description;
        Difficulty = difficulty;
        Tags = tags ?? new List<string>();
        Price = price;
    }

    public TourReview AddReview(int grade, string? comment, DateTime? reviewTime, double progress, long touristId, string username, List<ReviewImage>? images = null)
    {
        var review = new TourReview(grade, comment, reviewTime, progress, touristId, this.Id, images, username);
        Reviews.Add(review);
        GetAverageGrade();
        return review;
    }

    public TourReview AddReview(TourReview review)
    {
        Reviews.Add(review);
        GetAverageGrade();
        return review;
    }

    public void UpdateReview(long reviewId, int grade, string? comment, double progress, List<ReviewImage>? images = null)
    {
        var review = Reviews.FirstOrDefault(r => r.Id == reviewId);
        if (review == null) throw new KeyNotFoundException("Review not found");
        review.Update(grade, comment, progress, images);
        GetAverageGrade();
    }

    public void RemoveReview(long reviewId)
    {
        var review = Reviews.FirstOrDefault(r => r.Id == reviewId);
        if (review == null)
            throw new NotFoundException($"Review with id {reviewId} not found.");

        Reviews.Remove(review);
        GetAverageGrade();
    }

    public void GetAverageGrade()
    {
        if (Reviews.Any())
            AverageRating = Reviews.Average(r => r.Grade);
        else
            AverageRating = 0;
    }
    public void AddDuration(TourDuration duration)
    {
        if (Durations.Contains(duration))
            throw new InvalidOperationException("Duration already exists for this tour.");

        Durations.Add(duration);
    }

    public void RemoveDuration(TourDuration duration)
    {
        if (!Durations.Remove(duration))
            throw new NotFoundException($"Duration not found.");
    }

    public void UpdateTourLength(double length)
    {
        TourLength = length;
    }

    public void AddRequiredEquipment(long equipmentId)
    {
        if (Status == TourStatus.Archived)
            throw new InvalidOperationException("Equipment can only be modified for tours that are not archived.");

        if (RequiredEquipment.Any(re => re.EquipmentId == equipmentId))
            return;

        RequiredEquipment.Add(new RequiredEquipment(equipmentId));
    }

    public void RemoveRequiredEquipment(long equipmentId)
    {
        if (Status == TourStatus.Archived)
            throw new InvalidOperationException("Equipment can only be modified for tours that are not archived.");

        var existing = RequiredEquipment.FirstOrDefault(re => re.EquipmentId == equipmentId);
        if (existing == null) return;

        RequiredEquipment.Remove(existing);
    }
}
