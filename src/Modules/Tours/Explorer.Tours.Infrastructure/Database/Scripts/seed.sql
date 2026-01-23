DELETE FROM tours."TouristPreferences";
DELETE FROM tours."Tours";
DELETE FROM tours."Facilities";
DELETE FROM tours."Monument";
DELETE FROM tours."Equipment";
DELETE FROM tours."TouristEquipment";
DELETE FROM tours."TourReviews";
DELETE FROM tours."TourRequests";

INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (1, 4, 1, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["Adventure","Nature"]');
INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (2, 6, 2, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["Adventure","Nature"]');

INSERT INTO tours."Tours" VALUES (1, 7, 'Uvac Canyon Lookout Tour', 'A guided visit to the iconic Uvac meanders, including hiking to the best panoramic viewpoints and observing the griffon vultures.', 1, '{Nature,Scenic,Wildlife}', 0, 0, NULL, NULL, 0.0, 0.0);
INSERT INTO tours."Tours" VALUES (2, 7, 'Niš WWII History Trail', 'Explore key WWII historical locations in Niš, including the Red Cross Concentration Camp and Bubanj Memorial Park.', 0, '{History,Culture,Education}', 5.05, 1, NULL, NULL, 0.0, 0.0);
INSERT INTO tours."Tours" VALUES (3, 8, 'Kopaonik Ski & Snow Walk', 'A winter sports experience on Kopaonik with beginner–friendly skiing and guided snow trail walks.', 2, '{}', 0, 0, NULL, NULL, 0.0, 0.0);
INSERT INTO tours."Tours" VALUES (4, 2, 'Belgrade Historical Tour', 'Explore key historical locations in Belgrade, including Kalemegdan Fortress and the old town.', 0, '{History,Culture,Education}', 5.05, 1, NULL, NULL, 0.0, 0.0);
INSERT INTO tours."Tours" VALUES (5, 7, 'Zlatibor Gold Gondola', 'A relaxing ride on the longest panoramic gondola, enjoying the view of Zlatibor mountain ranges.', 0, '{Nature}', 15.0, 1, NULL, NULL, 0.0, 0.0);
INSERT INTO tours."Tours" VALUES (6, 7, 'Belgrade Night Walking Tour', 'A guided evening walk through Belgrade, exploring historical streets, local cafes, and nightlife.', 0, '{History,Culture,Nightlife}', 8.0, 1, NULL, NULL, 0.0, 0.0);
INSERT INTO tours."Tours" VALUES (7, 7, 'Ada Ciganlija Outdoor Adventure', 'Experience biking, kayaking, and outdoor fun at Ada Ciganlija, suitable for all ages.', 1, '{Adventure,Sports,Nature}', 12.0, 1, NULL, NULL, 0.0, 0.0);

INSERT INTO tours."Facilities"(
	"Id", "Name", "Latitude", "Longitude", "Type")
VALUES (1, 'Planinarski dom Kopaonik', 43.2711, 20.8444, 1);
INSERT INTO tours."Facilities"(
	"Id", "Name", "Latitude", "Longitude", "Type")
VALUES (2, 'Vikendica Zlatibor', 43.7180, 19.6990, 2);
INSERT INTO tours."Facilities"(
	"Id", "Name", "Latitude", "Longitude", "Type")
VALUES (3, 'Hotel Tara', 43.3225, 19.5400, 3);
INSERT INTO tours."Facilities"(
	"Id", "Name", "Latitude", "Longitude", "Type")
VALUES (4, 'Hostel Drina', 44.1375, 19.4419, 0);

INSERT INTO tours."KeyPoints"
("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES 
(1, 'Uvac Viewpoint', 'Best viewpoint over Uvac meanders.', '{"Latitude":43.123,"Longitude":19.456}', '/images/keypoints/uvac.jpg', false, 1, 1),
(2, 'Belgrade Fortress', 'Historic Kalemegdan Fortress.', '{"Latitude":44.820,"Longitude":20.456}', '/images/keypoints/kalemegdan.jpg', false, 2, 1);

INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES
(1, 'Test Monument One', 'Description 1', 1945, 0, 44.815, 20.460);
INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES
(2, 'Test Monument Two', 'Description 2', 1920, 0, 45.000, 19.900);
INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES
(3, 'Test Monument Three', 'Description 3', 1920, 0, 45.022, 19.900);

INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (1, 'Water', 'The amount of water depends on the temperature and the duration of the tour. It is recommended to drink half a liter of water per hour of moderate physical activity (e.g. hiking in nature without significant elevation gain) in moderate heat.');

INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (2, 'Walking poles', 'Walking poles reduce leg fatigue, encourage upper body activity, and provide stability on uneven terrain.');

INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (3, 'Standard flashlight', 'A flashlight with a brightness of 200 to 400 lumens.');


INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (1, 4, 1);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (2, 4, 2);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (3, 5, 3);

-- TourReviews
INSERT INTO tours."TourReviews" 
("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES
(5, 'Amazing tour!', NOW(), 100, 4, 1, 'turista1'),
(4, 'Very informative', NOW(), 100, 5, 2, 'turista2'),
(3, 'It was ok', NOW(), 75, 6, 3, 'turista3');

INSERT INTO tours."TourRequests" 
("Id", "TouristId", "AuthorId", "Difficulty", "Status", "Description", "MaxPrice", "Tags", "Location_Latitude", "Location_Longitude", "AcceptedTourId")
VALUES 
(1, 4, 7, 0, 0, 'I would like a relaxing walk by the river focusing on nature.', 50.0, '["Nature", "Walking"]', 45.2671, 19.8335, NULL);

SELECT setval(pg_get_serial_sequence('tours."TouristPreferences"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TouristPreferences"));
SELECT setval(pg_get_serial_sequence('tours."Tours"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Tours"));
SELECT setval(pg_get_serial_sequence('tours."Facilities"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Facilities"));
SELECT setval(pg_get_serial_sequence('tours."Monument"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Monument"));
SELECT setval(pg_get_serial_sequence('tours."Equipment"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Equipment"));
SELECT setval(pg_get_serial_sequence('tours."TouristEquipment"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TouristEquipment"));
SELECT setval(pg_get_serial_sequence('tours."TourReviews"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TourReviews"));
SELECT setval(pg_get_serial_sequence('tours."KeyPoints"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."KeyPoints"));
SELECT setval(pg_get_serial_sequence('tours."TourRequests"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TourRequests"));