DELETE FROM tours."TouristPreferences";
DELETE FROM stakeholders."People";
DELETE FROM stakeholders."Users";


INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-1, 'admin@gmail.com', 'admin', 'admin@gmail.com', 0, true);

INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-11, 'autor1@gmail.com', 'autor1', 'autor1@gmail.com', 1, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-12, 'autor2@gmail.com', 'autor2', 'autor2@gmail.com',1, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-13, 'autor3@gmail.com', 'autor3', 'autor3@gmail.com', 1, true);

INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-21, 'turista1@gmail.com', 'turista1', 'turista1@gmail.com', 2, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-22, 'turista2@gmail.com', 'turista2', 'turista2@gmail.com', 2, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-23, 'turista3@gmail.com', 'turista3', 'turista3@gmail.com', 2, true);

INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "Biography", "Motto", "ProfileImage")
VALUES 
(-11, -11, 'Ana', 'Anić', 'autor1@gmail.com', NULL, NULL, NULL),
(-12, -12, 'Lena', 'Lenić', 'autor2@gmail.com', NULL, NULL, NULL),
(-13, -13, 'Sara', 'Sarić', 'autor3@gmail.com', NULL, NULL, NULL),
(-21, -21, 'Pera', 'Perić', 'turista1@gmail.com', 'Biografija Pere Perića', 'Moto Pere', NULL),
(-22, -22, 'Mika', 'Mikić', 'turista2@gmail.com', NULL, NULL, NULL),
(-23, -23, 'Steva', 'Stević', 'turista3@gmail.com', NULL, 'Carpe Diem', NULL);

INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (-11, -21, 1, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["adventure","nature"]');
INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (-12, -23, 1, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["adventure","nature"]');