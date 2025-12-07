using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.Core.Domain.Shared;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.Tours.Core.Domain;

public class TourReview: Entity
{
    public int Grade { get; private set; }
    public string? Comment { get; private set; }
    public List<ReviewImage> Images { get; private set; }
    public DateTime? ReviewTime { get; private set; }
    public TourProgress Progress { get; private set; }
    public long TouristID { get; set; }
    public long TourID { get; set; }

    public TourReview()
    {
        Images = new List<ReviewImage>();
        ReviewTime = DateTime.MinValue;
        Progress = new TourProgress(0);
    }
    
    public TourReview(int grade, string comment, DateTime? reviewTime, double progress, long touristID, long tourID, List<ReviewImage> images)
    {
        if (grade < 1 || grade > 5)
            throw new ArgumentException("Grade must be between 1 and 5.", nameof(grade));

        Guard.AgainstNull(touristID, nameof(touristID));
        Guard.AgainstNull(tourID, nameof(tourID));

        Grade = grade;
        Comment = comment;
        TouristID = touristID;
        TourID = tourID;
        Progress = new TourProgress(progress);
        ReviewTime = reviewTime ?? DateTime.UtcNow;
        Images = images ?? new List<ReviewImage>();
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

    public void UpdatePercentage(double newPercentage)
    {
        Progress = new TourProgress(newPercentage);
    }

    public void AddImage(string imagePath)
    {
        Images.Add(new ReviewImage(imagePath));
    }

    public void RemoveImage(int imageId)
    {
        Images.RemoveAll(img => img.Id == imageId);
    }

    public void Update(int grade, string comment, double progress, List<ReviewImage>? images = null)
    {
        if (grade < 1 || grade > 5)
            throw new ArgumentException("Grade must be between 1 and 5.", nameof(grade));

        Grade = grade;
        Comment = comment;
        Progress = new TourProgress(progress);

        if (images != null)
        {
            foreach (var img in images)
            {
                if (!Images.Any(i => i.ImagePath == img.ImagePath))
                    Images.Add(new ReviewImage { ImagePath = img.ImagePath });
            }
            Images.RemoveAll(i => !images.Any(d => d.ImagePath == i.ImagePath));
        }
    }
}
