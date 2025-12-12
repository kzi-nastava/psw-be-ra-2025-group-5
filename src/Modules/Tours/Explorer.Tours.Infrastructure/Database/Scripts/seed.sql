DELETE FROM tours."TouristPreferences";
DELETE FROM tours."Tours";
DELETE FROM tours."Facilities";
DELETE FROM tours."Monument";
DELETE FROM tours."Equipment";
DELETE FROM tours."TouristEquipment";
DELETE FROM tours."ShoppingCarts";

INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (1, 4, 1, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["Adventure","Nature"]');
INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (2, 6, 2, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["Adventure","Nature"]');

INSERT INTO tours."Tours" VALUES (1, 7, 'Uvac Canyon Lookout Tour', 'A guided visit to the iconic Uvac meanders, including hiking to the best panoramic viewpoints and observing the griffon vultures.', 1, '{Nature,Scenic,Wildlife}', 0, 0, NULL, NULL);
INSERT INTO tours."Tours" VALUES (2, 7, 'Niš WWII History Trail', 'Explore key WWII historical locations in Niš, including the Red Cross Concentration Camp and Bubanj Memorial Park.', 0, '{History,Culture,Education}', 5.05, 1, NULL, NULL);
INSERT INTO tours."Tours" VALUES (3, 8, 'Kopaonik Ski & Snow Walk', 'A winter sports experience on Kopaonik with beginner–friendly skiing and guided snow trail walks.', 2, '{}', 0, 0, NULL, NULL);
INSERT INTO tours."Tours" VALUES (5, 2, 'Belgrade Historical Tour', 'Explore key historical locations in Belgrade, including Kalemegdan Fortress and the old town.', 0, '{History,Culture,Education}', 5.05, 1, NULL, NULL);

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
VALUES (1, 'Voda', 'Količina vode varira od temperature i trajanja ture. Preporuka je da se pije pola litre vode na jedan sat umerena fizičke aktivnosti (npr. hajk u prirodi bez značajnog uspona) po umerenoj vrućini');
INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (2, 'Štapovi za šetanje', 'Štapovi umanjuju umor nogu, pospešuju aktivnost gornjeg dela tela i pružaju stabilnost na neravnom terenu.');
INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (3, 'Obična baterijska lampa', 'Baterijska lampa od 200 do 400 lumena.');

INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (1, 4, 1);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (2, 4, 2);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (3, 5, 3);

INSERT INTO tours."ShoppingCart" ("Id", "TouristId", "Items") VALUES (1, 4, '[{"TourId": 2, "TourName": "Niš WWII History Trail", "ItemPrice": 5.05}]'), (2, 5, '[]');

SELECT setval(pg_get_serial_sequence('tours."TouristPreferences"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TouristPreferences"));
SELECT setval(pg_get_serial_sequence('tours."Tours"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Tours"));
SELECT setval(pg_get_serial_sequence('tours."Facilities"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Facilities"));
SELECT setval(pg_get_serial_sequence('tours."Monument"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Monument"));
SELECT setval(pg_get_serial_sequence('tours."Equipment"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Equipment"));
SELECT setval(pg_get_serial_sequence('tours."TouristEquipment"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TouristEquipment"));
SELECT setval(pg_get_serial_sequence('tours."ShoppingCart"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."ShoppingCart"));

