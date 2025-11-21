DELETE FROM tours."TouristPreferences";
﻿DELETE FROM stakeholders."People";
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
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-11, -11, 'Ana', 'Anić', 'autor1@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-12, -12, 'Lena', 'Lenić', 'autor2@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-13, -13, 'Sara', 'Sarić', 'autor3@gmail.com');

INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-21, -21, 'Pera', 'Perić', 'turista1@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-22, -22, 'Mika', 'Mikić', 'turista2@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-23, -23, 'Steva', 'Stević', 'turista3@gmail.com');
INSERT INTO stakeholders."Users" VALUES (2, 'zika', 'zika', 'zika@gmail.com', 1, true);
INSERT INTO stakeholders."Users" VALUES (3, 'mika', 'mika', 'mika@gmail.com', 1, true);

INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (-11, -21, 1, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["adventure","nature"]');
INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (-12, -23, 1, '{"Walking":2,"Bicycle":3,"Car":1,"Boat":0}', '["adventure","nature"]');

INSERT INTO tours."Tours" VALUES (1, 'Uvac Canyon Lookout Tour', 'A guided visit to the iconic Uvac meanders, including hiking to the best panoramic viewpoints and observing the griffon vultures.', 1, '{Nature,Scenic,Wildlife}', 0, 0, 2);
INSERT INTO tours."Tours" VALUES (2, 'Niš WWII History Trail', 'Explore key WWII historical locations in Niš, including the Red Cross Concentration Camp and Bubanj Memorial Park.', 0, '{History,Culture,Education}', 5.05, 1, 2);
INSERT INTO tours."Tours" VALUES (3, 'Kopaonik Ski & Snow Walk', 'A winter sports experience on Kopaonik with beginner–friendly skiing and guided snow trail walks.', 2, '{}', 0, 0, 3);