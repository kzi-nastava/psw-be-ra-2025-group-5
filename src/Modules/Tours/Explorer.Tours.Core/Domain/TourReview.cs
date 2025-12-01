using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.Core.Domain.Shared;

namespace Explorer.Tours.Core.Domain;

public class TourReview: AggregateRoot
{
    public int Grade { get; private set; }
    public string? Comment { get; private set; }
    public List<ReviewImage> Images { get; private set; }
    public DateTime? ReviewTime { get; private set; }
    public TourProgress Progress { get; private set; }
    public long TouristID { get; private set; }
    public long TourID { get; private set; }

    public TourReview()
    {
        Images = new List<ReviewImage>();
        ReviewTime = DateTime.MinValue;
        Progress = new TourProgress(0);
    }
    
    public TourReview(int grade, string comment, List<ReviewImage> images, DateTime? reviewTime, TourProgress progeress, int touristID, int tourID)
    {
        Guard.AgainstNull(touristID, nameof(touristID));
        Guard.AgainstNull(tourID, nameof(tourID));
        if (grade < 1 || grade > 5)
            throw new ArgumentException("Grade must be between 1 and 5.");

        Grade = grade;
        Comment = comment;
        Images = images;
        ReviewTime = reviewTime;
        Progress = progeress;
        TouristID = touristID;
        TourID = tourID;
    }

    public void AddImage(ReviewImage image)
    {
        Images.Add(image);
    }
    public void RemoveImage(ReviewImage image) { Images.Remove(image); }

    public void UpdateGrade(int newGrade)
    {
        Grade = newGrade;
    }

    public void UpdateComment(string newComment)
    {
        Comment = newComment;
    }

    public void ReplaceImages(List<ReviewImage> newImages)
    {
        Images = newImages;
    }

    public void UpdatePercentage(Percentage newPercentage)
    {
        Percentage = newPercentage;
    }

}
