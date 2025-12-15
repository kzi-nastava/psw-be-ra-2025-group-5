INSERT INTO tours."Tours"(
	"Id", "AuthorId", "Name", "Description", "Difficulty", "Tags", "Price", "Status", "TourLength")
VALUES (-1,-11, 'Belgrade Fortress Walk', 'A guided walking tour through Kalemegdan and the old fortress', 0, ARRAY['History','Sightseeing'], 5.0, 1, 0);

INSERT INTO tours."Tours"(
	"Id", "AuthorId", "Name", "Description", "Difficulty", "Tags", "Price", "Status", "TourLength")
	VALUES (-2, -11, 'Tara National Park Hike', 'A full-day hiking adventure across the Tara mountain ridges.', 2, ARRAY['Nature','Hiking','Scenic'], 0, 0, 0);
INSERT INTO tours."Tours"(
	"Id", "AuthorId", "Name", "Description", "Difficulty", "Tags", "Price", "Status", "TourLength")
	VALUES (-3, -12, 'Novi Sad Food & Culture Tour', 'Discover the best local food spots and cultural landmarks of Novi Sad.', 1, ARRAY['Food','Culture','History'], 0, 0, 0);