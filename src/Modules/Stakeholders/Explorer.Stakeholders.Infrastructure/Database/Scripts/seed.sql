DELETE FROM stakeholders."TourProblems";
DELETE FROM stakeholders."People";
DELETE FROM stakeholders."Users";
DELETE FROM stakeholders."AppRatings";
DELETE FROM stakeholders."Clubs";
DELETE FROM stakeholders."Notifications";
DELETE FROM stakeholders."ClubInvites";
DELETE FROM stakeholders."ClubMembers";
DELETE FROM stakeholders."ProfileFollows";
DELETE FROM stakeholders."ProfileMessages";
DELETE FROM stakeholders."Planners";

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
    "Id", "UserId", "Name", "Surname", "Email", "Biography", "Motto", "ProfileImagePath", "Level", "ExperiencePoints")
VALUES 
(1, 1, 'Ana', 'Anić', 'autor1@gmail.com', NULL, NULL, NULL, 0, 0),
(2, 2, 'Lena', 'Lenić', 'autor2@gmail.com', NULL, NULL, NULL, 0, 0),
(3, 3, 'Sara', 'Sarić', 'autor3@gmail.com', NULL, NULL, NULL, 0, 0),
(4, 4, 'Pera', 'Perić', 'turista1@gmail.com', 'Biografija Pere Perića', 'Moto Pere', '/images/profiles/profilna1.jpg', 10, 1000),
(5, 5, 'Mika', 'Mikić', 'turista2@gmail.com', NULL, NULL, NULL, 0, 0),
(6, 6, 'Steva', 'Stević', 'turista3@gmail.com', NULL, 'Carpe Diem', NULL, 0, 0),
(7, 7, 'Zika', 'Zikić', 'zika@gmail.com', NULL, 'Carpe Diem', NULL, 0, 0),
(8, 8, 'Mika', 'Mikić', 'mika@gmail.com', NULL, 'Carpe Diem', NULL, 0, 0);

INSERT INTO stakeholders."ProfileFollows"("FollowerId", "FollowingId") VALUES (4, 7), (4, 6), (2, 1), (5, 1);

INSERT INTO stakeholders."ProfileMessages"("Id", "AuthorId", "ReceiverId", "Content", "CreatedAt", "AttachedResourceType") VALUES
(1, 4, 1, 'Zdravo, zanima me tvoja nova tura.', NOW(), 0),
(2, 1, 4, 'Zdravo! Hvala na interesovanju. Tura je zakazana za sledeći vikend.', NOW(), 0),
(3, 5, 2, 'Da li imaš slobodnih mesta za turu?', NOW(), 0),
(4, 2, 5, 'Da, imamo još nekoliko mesta. Prijavi se što pre!', NOW(), 0);

INSERT INTO stakeholders."AppRatings" 
("Id", "UserId", "Rating", "Comment", "CreatedAt", "UpdatedAt") VALUES
(1, 4, 5, 'Odlična aplikacija', NOW(), NOW()),
(2, 5, 4, 'Solidno', NOW(), NOW());

INSERT INTO stakeholders."Clubs" 
("Id", "Name", "Description", "ImagePaths", "CreatorId", "Status")
VALUES
(1, 'Planinski klub Vršac', 'Klub za ljubitelje planinarenja i prirode. Organizujemo vikend izlete na planine širom Srbije.', 
 '/images/club/planinari1.jpeg', 4, 0),
(2, 'Foto safari klub', 'Fotografisanje divljih životinja i prirode. Naše ture obuhvataju nacionalne parkove i rezervate.', 
 '/images/club/safari1.jpg', 5, 0);

INSERT INTO stakeholders."ClubMembers" 
	("ClubId", "TouristId", "JoinedAt") 
VALUES 
	(1, 5, NOW());


INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (1, 1, 4, 0, 2, 'Problem sa bezbednošću na turi', '2023-10-25T10:00:00Z', '2023-10-25T10:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (2, 2, 5, 2, 1, 'Problem sa planom puta', '2023-10-26T11:00:00Z', '2023-10-26T11:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (3, 3, 6, 1, 0, 'Problem sa vodičem', '2023-10-27T12:00:00Z', '2023-10-27T12:05:00Z', ARRAY[]::bigint[], true, null);

INSERT INTO stakeholders."Planners"("Id", "TouristId") VALUES (1, 4);
INSERT INTO stakeholders."PlannerDay"("Id", "Date", "PlannerId") VALUES (1, NOW(), 1);
INSERT INTO stakeholders."PlannerTimeBlock"("Id", "TourId", "TimeRange", "PlannerDayId") VALUES (1, 5, jsonb_build_object('Start', '09:30', 'End', '11:00'), 1);

INSERT INTO stakeholders."Diaries"("Id", "Name", "CreatedAt", "Country", "City", "TouristId", "Content") VALUES
	(1, 'Trip ideas', '2023-10-27T12:00:00Z', 'Italy', 'Rome', 4, 'Check out if there is any new tours around, or request them if not'),
	(2, '', '2026-01-25T09:00:00Z', '', '', 4, '- Catch plane
	- Take taxi to hotel
	- Drop off stuff
	- Do Belgrade tour');

SELECT setval(pg_get_serial_sequence('stakeholders."TourProblems"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."TourProblems"));
SELECT setval(pg_get_serial_sequence('stakeholders."People"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."People"));
SELECT setval(pg_get_serial_sequence('stakeholders."Users"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Users"));
SELECT setval(pg_get_serial_sequence('stakeholders."AppRatings"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."AppRatings"));
SELECT setval(pg_get_serial_sequence('stakeholders."Clubs"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Clubs"));
SELECT setval(pg_get_serial_sequence('stakeholders."ProfileMessages"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."ProfileMessages"));
SELECT setval(pg_get_serial_sequence('stakeholders."ProfileFollows"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."ProfileFollows"));
SELECT setval(pg_get_serial_sequence('stakeholders."Planners"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Planners"));
SELECT setval(pg_get_serial_sequence('stakeholders."PlannerDay"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."PlannerDay"));
SELECT setval(pg_get_serial_sequence('stakeholders."PlannerTimeBlock"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."PlannerTimeBlock"));
SELECT setval(pg_get_serial_sequence('stakeholders."Diaries"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Diaries"));
