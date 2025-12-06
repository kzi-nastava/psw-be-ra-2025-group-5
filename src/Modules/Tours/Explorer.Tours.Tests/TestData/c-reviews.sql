INSERT INTO tours."TourReviews"(
    "Id", "Grade", "Comment", "ReviewTime", "ProgressPercentage", "TouristID", "TourID")
VALUES (-1, 5, 'Excellent tour!', NOW(), 100, -11, -1);

INSERT INTO tours."TourReviews"(
    "Id", "Grade", "Comment", "ReviewTime", "ProgressPercentage", "TouristID", "TourID")
VALUES (-2, 3, 'It was ok.', NOW(), 60, -12, -2);


INSERT INTO tours."ReviewImages"(
    "Id", "ImagePath", "TourReviewId")
VALUES (-1, '/images/reviews/1/img1.jpg', -1);

INSERT INTO tours."ReviewImages"(
    "Id", "ImagePath", "TourReviewId")
VALUES (-2, '/images/reviews/1/img2.jpg', -1);

INSERT INTO tours."ReviewImages"(
    "Id", "ImagePath", "TourReviewId")
VALUES (-3, '/images/reviews/2/img1.jpg', -2);