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
DELETE FROM stakeholders."Badges";
DELETE FROM stakeholders."UserStatistics";
DELETE FROM stakeholders."UserBadges";
DELETE FROM stakeholders."UserPremiums";

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
    "Id", "UserId", "Name", "Surname", "Email", "Biography", "Motto", "ProfileImagePath", "Level", "ExperiencePoints", "CreatedAt")
VALUES 
(1, 1, 'Ana', 'Anić', 'autor1@gmail.com', NULL, NULL, NULL, 0, 0, '2023-10-25T10:00:00Z'),
(2, 2, 'Lena', 'Lenić', 'autor2@gmail.com', NULL, NULL, NULL, 0, 0, '2022-10-25T10:00:00Z'),
(3, 3, 'Sara', 'Sarić', 'autor3@gmail.com', NULL, NULL, NULL, 0, 0, '2025-10-25T10:00:00Z'),
(4, 4, 'Pera', 'Perić', 'turista1@gmail.com', 'Biografija Pere Perića', 'Moto Pere', '/images/profiles/profilna1.jpg', 0, 0, '2023-10-25T10:00:00Z'),
(5, 5, 'Mika', 'Mikić', 'turista2@gmail.com', NULL, NULL, NULL, 0, 0, '2023-10-25T10:00:00Z'),
(6, 6, 'Steva', 'Stević', 'turista3@gmail.com', NULL, 'Carpe Diem', NULL, 0, 0, '2024-07-25T10:00:00Z'),
(7, 7, 'Zika', 'Zikić', 'zika@gmail.com', NULL, 'Carpe Diem', NULL, 0, 0, '2022-10-25T10:00:00Z'),
(8, 8, 'Mika', 'Mikić', 'mika@gmail.com', NULL, 'Carpe Diem', NULL, 0, 0, '2023-10-25T10:00:00Z');

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
VALUES (1, 1, 4, 0, 2, 'Problem sa bezbednošću na turi', '2024-10-25T10:00:00Z', '2024-10-25T10:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (2, 2, 5, 2, 1, 'Problem sa planom puta', '2024-10-26T11:00:00Z', '2024-10-26T11:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (3, 3, 6, 1, 0, 'Problem sa vodičem', '2023-10-27T12:00:00Z', '2023-10-27T12:05:00Z', ARRAY[]::bigint[], true, null);

INSERT INTO stakeholders."Streaks"(
    "Id", "UserId", "StartDate", "LastActivity", "LongestStreak")
VALUES (1, 21, '2026-01-18', '2026-01-19', 2);
INSERT INTO stakeholders."UserPremiums" 
	("Id", "UserId", "ValidUntil")
VALUES (1, 4, '2026-02-20T00:00:00Z');





SELECT setval(pg_get_serial_sequence('stakeholders."TourProblems"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."TourProblems"));
SELECT setval(pg_get_serial_sequence('stakeholders."People"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."People"));
SELECT setval(pg_get_serial_sequence('stakeholders."Users"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Users"));
SELECT setval(pg_get_serial_sequence('stakeholders."AppRatings"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."AppRatings"));
SELECT setval(pg_get_serial_sequence('stakeholders."Clubs"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Clubs"));

SELECT setval(pg_get_serial_sequence('stakeholders."ProfileMessages"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."ProfileMessages"));
SELECT setval(pg_get_serial_sequence('stakeholders."ProfileMessages"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."ProfileMessages"));


-- Pathfinder Badges (Level)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (1, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_bronze.png', 0, 0, 1),
    (2, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_silver.png', 1, 0, 10),
    (3, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_gold.png', 2, 0, 25),
    (4, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_epic.png', 3, 0, 50);

-- Veteran Badges (Account Age in days)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (5, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_bronze.png', 0, 1, 1),
    (6, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_silver.png', 1, 1, 365),
    (7, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_gold.png', 2, 1, 1095),
    (8, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_epic.png', 3, 1, 1825);

-- Explorer Badges (Completed Tours)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (9, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_bronze.png', 0, 2, 1),
    (10, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_silver.png', 1, 2, 10),
    (11, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_gold.png', 2, 2, 25),
    (12, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_epic.png', 3, 2, 50);

-- Challenger Badges (Completed Challenges)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (13, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_bronze.png', 0, 3, 1),
    (14, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_silver.png', 1, 3, 25),
    (15, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_gold.png', 2, 3, 50),
    (16, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_epic.png', 3, 3, 100);

-- Creator Badges (Published Tours)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (17, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_bronze.png', 0, 4, 1),
    (18, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_silver.png', 1, 4, 5),
    (19, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_gold.png', 2, 4, 10),
    (20, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_epic.png', 3, 4, 25);

-- Entrepreneur Badges (Sold Tours)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (21, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_bronze.png', 0, 5, 1),
    (22, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_silver.png', 1, 5, 10),
    (23, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_gold.png', 2, 5, 50),
    (24, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_epic.png', 3, 5, 100);

-- Completionist Badge (All Challenge Types Completed)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (25, 'Completionist', 'Unlocked by completing every challenge type at least once.', '/images/badges/completionist.png', 3, 6, 1);

-- Storyteller Badges (Blog Posts)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (26, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_bronze.png', 0, 7, 1),
    (27, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_silver.png', 1, 7, 5),
    (28, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_gold.png', 2, 7, 10),
    (29, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_epic.png', 3, 7, 25);

-- Community Member Badge (Club Member)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue")
VALUES 
    (30, 'Community Member', 'Earned by joining or founding a club and becoming part of the community.', '/images/badges/community_member.png', 1, 8, 1);


INSERT INTO stakeholders."UserStatistics"(
	"Id", "UserId", "Level", "AccountAgeDays", "CompletedToursCount", "CompletedChallengesCount", "PublishedToursCount", "SoldToursCount", "BlogPostsCount", "ChallengeTypesCompletedMask", "JoinedClub", "CreatedAt", "UpdatedAt")
	VALUES 
        (1, 1, 0, 817, 0, 0, 0, 0, 2, 0, false, NOW(), NOW()),
        (2, 2, 0, 1182, 0, 0, 1, 1, 1, 0, false, NOW(), NOW()),
        (3, 3, 0, 86, 0, 0, 0, 0, 2, 0, false, NOW(), NOW()),
        (4, 4, 0, 817, 0, 0, 0, 0, 0, 0, true, NOW(), NOW()),
        (5, 5, 0, 817, 0, 0, 0, 0, 0, 0, true, NOW(), NOW()),
        (6, 6, 0, 543, 0, 0, 0, 0, 0, 0, false, NOW(), NOW()),
        (7, 7, 0, 1182, 0, 0, 4, 0, 0, 0, false, NOW(), NOW()),
        (8, 8, 0, 817, 0, 0, 0, 0, 0, 0, false, NOW(), NOW());

INSERT INTO stakeholders."UserBadges"(
	"Id", "UserId", "BadgeId", "EarnedAt")
	VALUES 
        (1, 4, 30, NOW()),
        (2, 5, 30, NOW()),
        (3, 1, 26, NOW()),
        (4, 2, 26, NOW()),
        (5, 3, 26, NOW()),
        (6, 2, 21, NOW()),
        (7, 2, 17, NOW()),
        (8, 7, 17, NOW()),
        (9, 3, 5, NOW()),
        (10, 6, 6, NOW()),
        (11, 1, 6, NOW()),
        (12, 4, 6, NOW()),
        (13, 5, 6, NOW()),
        (14, 8, 6, NOW()),
        (15, 2, 7, NOW()),
        (16, 7, 7, NOW());

SELECT setval(pg_get_serial_sequence('stakeholders."Badges"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Badges"));
SELECT setval(pg_get_serial_sequence('stakeholders."UserStatistics"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."UserStatistics"));
SELECT setval(pg_get_serial_sequence('stakeholders."UserBadges"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."UserBadges"));

SELECT setval(pg_get_serial_sequence('stakeholders."UserPremiums"', 'Id'), (SELECT COALESCE(MAX("Id"), 0) FROM stakeholders."UserPremiums"));
