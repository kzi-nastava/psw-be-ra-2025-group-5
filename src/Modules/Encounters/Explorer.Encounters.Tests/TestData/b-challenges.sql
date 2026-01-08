-- Status: 0 = Draft, 1 = Active, 2 = Archived, 3 = Pending
-- Type: 0 = Social, 1 = Location, 2 = Misc

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters")
VALUES (-1, 'Beogradska tvrdjava', 'Posetite Beogradsku tvrdjavu i napravite fotografiju sa pogledom na reku.', 44.823398, 20.450554, 100, 1, 1, null, null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters")
VALUES (-2, 'Susret sa lokalcima', 'Upoznajte tri lokalna stanovnika i saznajte njihove price.', 44.815556, 20.460833, 150, 1, 0, null, 2, 100);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters")
VALUES (-3, 'Skrivena lokacija', 'Pronadjite skrivenu lokaciju u starom gradu i otkrijte tajnu.', 44.818611, 20.457222, 200, 3, 2, null, null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters")
VALUES (-4, 'Istorijski spomenik', 'Posetite istorijski spomenik i naucite njegovu pricu.', 44.821111, 20.462778, 120, 3, 1, null, null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters")
VALUES (-5, 'Super skrivena lokacija', 'Najskrivenija lokacija', 44.82, 20.45, 50, 3, 1, null, 2, 100);

