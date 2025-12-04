-- Insert TourReview
INSERT INTO TourReview (Grade, Comment, ReviewTime, ProgressPercentage, TouristID, TourID)
VALUES (5, 'Excellent tour!', GETDATE(), 100, 11, 101);

INSERT INTO TourReview (Grade, Comment, ReviewTime, ProgressPercentage, TouristID, TourID)
VALUES (3, 'It was ok.', GETDATE(), 60, 12, 102);

-- Insert Review Images
INSERT INTO ReviewImage (ImagePath, TourReviewId)
VALUES ('/images/reviews/1/img1.jpg', 1);

INSERT INTO ReviewImage (ImagePath, TourReviewId)
VALUES ('/images/reviews/1/img2.jpg', 1);

INSERT INTO ReviewImage (ImagePath, TourReviewId)
VALUES ('/images/reviews/2/img1.jpg', 2);