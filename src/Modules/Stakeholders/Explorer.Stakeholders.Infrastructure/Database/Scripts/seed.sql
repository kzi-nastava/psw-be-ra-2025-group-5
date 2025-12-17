DELETE FROM stakeholders."TourProblems";
DELETE FROM stakeholders."People";
DELETE FROM stakeholders."Users";
DELETE FROM stakeholders."AppRatings";
DELETE FROM stakeholders."Clubs";
DELETE FROM stakeholders."Notifications";

INSERT INTO stakeholders."Users" ("Id", "Username", "Password", "Email", "Role", "IsActive") VALUES
(0,  'admin',   'admin',   'admin@gmail.com', 0, true),
(1, 'autor1',  'autor1',  'autor1@gmail.com', 1, true),
(2, 'autor2',  'autor2',  'autor2@gmail.com', 1, true),
(3, 'autor3',  'autor3',  'autor3@gmail.com', 1, true),
(4, 'turista1','turista1','turista1@gmail.com', 2, true),
(5, 'turista2','turista2','turista2@gmail.com', 2, true),
(6, 'turista3','turista3','turista3@gmail.com', 2, true);

INSERT INTO stakeholders."Users" VALUES (7, 'zika', 'zika', 'zika@gmail.com', 1, true);
INSERT INTO stakeholders."Users" VALUES (8, 'mika', 'mika', 'mika@gmail.com', 1, true);


INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "Biography", "Motto", "ProfileImage")
VALUES 
(1, 1, 'Ana', 'Anić', 'autor1@gmail.com', NULL, NULL, NULL),
(2, 2, 'Lena', 'Lenić', 'autor2@gmail.com', NULL, NULL, NULL),
(3, 3, 'Sara', 'Sarić', 'autor3@gmail.com', NULL, NULL, NULL),
(4, 4, 'Pera', 'Perić', 'turista1@gmail.com', 'Biografija Pere Perića', 'Moto Pere', NULL),
(5, 5, 'Mika', 'Mikić', 'turista2@gmail.com', NULL, NULL, NULL),
(6, 6, 'Steva', 'Stević', 'turista3@gmail.com', NULL, 'Carpe Diem', NULL),
(7, 7, 'Zika', 'Zikić', 'zika@gmail.com', NULL, 'Carpe Diem', NULL),
(8, 8, 'Mika', 'Mikić', 'mika@gmail.com', NULL, 'Carpe Diem', NULL);



INSERT INTO stakeholders."AppRatings" 
("Id", "UserId", "Rating", "Comment", "CreatedAt", "UpdatedAt") VALUES
(1, 4, 5, 'Odlična aplikacija', NOW(), NOW()),
(2, 5, 4, 'Solidno', NOW(), NOW());

INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (1, 1, 4, 0, 2, 'Problem sa bezbednošću na turi', '2023-10-25T10:00:00Z', '2023-10-25T10:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (2, 2, 5, 2, 1, 'Problem sa planom puta', '2023-10-26T11:00:00Z', '2023-10-26T11:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (3, 3, 6, 1, 0, 'Problem sa vodičem', '2023-10-27T12:00:00Z', '2023-10-27T12:05:00Z', ARRAY[]::bigint[], true, null);


SELECT setval(pg_get_serial_sequence('stakeholders."TourProblems"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."TourProblems"));
SELECT setval(pg_get_serial_sequence('stakeholders."People"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."People"));
SELECT setval(pg_get_serial_sequence('stakeholders."Users"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Users"));
SELECT setval(pg_get_serial_sequence('stakeholders."AppRatings"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."AppRatings"));