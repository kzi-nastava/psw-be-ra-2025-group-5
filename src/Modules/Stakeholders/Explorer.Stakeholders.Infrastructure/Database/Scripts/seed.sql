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
DELETE FROM stakeholders."Badges";
DELETE FROM stakeholders."UserStatistics";
DELETE FROM stakeholders."UserBadges";
DELETE FROM stakeholders."UserPremiums";
DELETE FROM stakeholders."Streaks";
DELETE FROM stakeholders."Diaries";

INSERT INTO stakeholders."Users" ("Id", "Username", "Password", "Email", "Role", "IsActive") VALUES
-- 1 Admin
(0, 'tonystark', 'password', 'tony.stark@starkindustries.com', 0, true),
-- 5 Tourists
(1, 'frodobaggins', 'password', 'frodo.baggins@shire.com', 2, true),
(2, 'hermionegranger', 'password', 'hermione.granger@hogwarts.edu', 2, true),
(3, 'lukeskywalker', 'password', 'luke.skywalker@rebelalliance.org', 2, true),
(4, 'walterwhite', 'password', 'walter.white@graymatter.com', 2, true),
(5, 'jessepinkman', 'password', 'jesse.pinkman@capncook.com', 2, true),
-- 14 Authors
(6, 'jamesbond', 'password', 'james.bond@mi6.gov.uk', 1, true),
(7, 'brucewayne', 'password', 'bruce.wayne@wayneenterprises.com', 1, true),
(8, 'peterparker', 'password', 'peter.parker@dailybugle.com', 1, true),
(9, 'dianaprince', 'password', 'diana.prince@themyscira.org', 1, true),
(10, 'natasharomanoff', 'password', 'natasha.romanoff@shield.gov', 1, true),
(11, 'steverogers', 'password', 'steve.rogers@avengers.org', 1, true),
(12, 'indianajones', 'password', 'indiana.jones@university.edu', 1, true),
(13, 'ellenripley', 'password', 'ellen.ripley@weyland.com', 1, true),
(14, 'rickdeckard', 'password', 'rick.deckard@lapd.gov', 1, true),
(15, 'johnmcclane', 'password', 'john.mcclane@nypd.gov', 1, true),
(16, 'laracroft', 'password', 'lara.croft@croftmanor.uk', 1, true),
(17, 'ethanhunt', 'password', 'ethan.hunt@imf.gov', 1, true),
(18, 'maxrockatansky', 'password', 'max.rockatansky@roadwarrior.com', 1, true),
(19, 'trinity', 'password', 'trinity@matrix.net', 1, true);


INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "Biography", "Motto", "ProfileImagePath", "Level", "ExperiencePoints", "CreatedAt")
VALUES
-- 5 Tourists
(1, 1, 'Frodo', 'Baggins', 'frodo.baggins@shire.com', 'Adventurer from the Shire seeking the next great journey', 'Not all those who wander are lost', '/images/profiles/frodo.jpg', 0, 0, '2023-03-15T10:00:00Z'),
(2, 2, 'Hermione', 'Granger', 'hermione.granger@hogwarts.edu', 'Always eager to learn about new places and cultures', 'Knowledge is the greatest treasure', '/images/profiles/hermiona.jpeg', 0, 0, '2023-05-20T10:00:00Z'),
(3, 3, 'Luke', 'Skywalker', 'luke.skywalker@rebelalliance.org', 'Exploring the galaxy one planet at a time', 'Adventure awaits in every corner', '/images/profiles/luke.jpg', 0, 0, '2023-08-10T10:00:00Z'),
(4, 4, 'Walter', 'White', 'walter.white@graymatter.com', 'Chemistry teacher turned adventure seeker exploring new destinations', 'I am the one who travels', '/images/profiles/walter.jpg', 0, 0, '2024-01-12T10:00:00Z'),
(5, 5, 'Jesse', 'Pinkman', 'jesse.pinkman@capncook.com', 'Young traveler with enthusiasm for discovering exciting places', 'Science and adventure, yeah!', '/images/profiles/jesse.png', 0, 0, '2024-03-25T10:00:00Z'),
-- 14 Authors
(6, 6, 'James', 'Bond', 'james.bond@mi6.gov.uk', 'International tour guide specializing in exotic destinations', 'Travel with style and sophistication', '/images/profiles/hermiona.jpeg', 0, 0, '2022-01-15T10:00:00Z'),
(7, 7, 'Bruce', 'Wayne', 'bruce.wayne@wayneenterprises.com', 'Philanthropist creating exclusive nighttime city tours', 'Discover the hidden beauty of cities after dark', '/images/profiles/bruce.jpg', 0, 0, '2022-03-20T10:00:00Z'),
(8, 8, 'Peter', 'Parker', 'peter.parker@dailybugle.com', 'Your friendly neighborhood tour guide', 'With great power comes great responsibility', '/images/profiles/peter.jpg', 0, 0, '2022-06-10T10:00:00Z'),
(9, 9, 'Diana', 'Prince', 'diana.prince@themyscira.org', 'Cultural historian sharing ancient wonders and mythology', 'Preserving history for future generations', NULL, 0, 0, '2022-08-05T10:00:00Z'),
(10, 10, 'Natasha', 'Romanoff', 'natasha.romanoff@shield.gov', 'Former intelligence agent creating adventure tours', 'Every journey tells a story', '/images/profiles/natasha.jpeg', 0, 0, '2023-01-18T10:00:00Z'),
(11, 11, 'Steve', 'Rogers', 'steve.rogers@avengers.org', 'Traditional values meet modern adventures', 'Never give up on your dreams', '/images/profiles/steve.jpg', 0, 0, '2023-04-22T10:00:00Z'),
(12, 12, 'Indiana', 'Jones', 'indiana.jones@university.edu', 'Archaeologist offering historical adventure tours', 'History comes alive through exploration', '/images/profiles/jones.jpg', 0, 0, '2023-07-14T10:00:00Z'),
(13, 13, 'Ellen', 'Ripley', 'ellen.ripley@weyland.com', 'Space explorer bringing cosmic perspective to Earth tours', 'Explore beyond the boundaries', '/images/profiles/ellen.jpg', 0, 0, '2023-09-30T10:00:00Z'),
(14, 14, 'Rick', 'Deckard', 'rick.deckard@lapd.gov', 'Detective creating mystery-solving city tours', 'Every city has secrets to discover', NULL, 0, 0, '2024-02-11T10:00:00Z'),
(15, 15, 'John', 'McClane', 'john.mcclane@nypd.gov', 'Action-packed urban exploration specialist', 'Adventure is around every corner', '/images/profiles/john.jpg', 0, 0, '2024-04-17T10:00:00Z'),
(16, 16, 'Lara', 'Croft', 'lara.croft@croftmanor.uk', 'Archaeologist offering extreme adventure tours', 'Fortune favors the bold', '/images/profiles/lara.jpg', 0, 0, '2024-06-23T10:00:00Z'),
(17, 17, 'Ethan', 'Hunt', 'ethan.hunt@imf.gov', 'Adventure specialist creating unforgettable experiences', 'Every journey is a new mission', NULL, 0, 0, '2024-08-08T10:00:00Z'),
(18, 18, 'Max', 'Rockatansky', 'max.rockatansky@roadwarrior.com', 'Desert survival expert offering wilderness tours', 'Survive and thrive in any environment', NULL, 0, 0, '2024-10-19T10:00:00Z'),
(19, 19, 'Trinity', 'Trinity', 'trinity@matrix.net', 'Technology expert creating innovative digital city tours', 'Embrace the future of travel', NULL, 0, 0, '2024-12-05T10:00:00Z');

INSERT INTO stakeholders."ProfileFollows"("FollowerId", "FollowingId") VALUES
(1, 6), (1, 12), (2, 9), (2, 16), (3, 11), (3, 6), (4, 12), (4, 13), (4, 5), (5, 7), (5, 8), (5, 4);

INSERT INTO stakeholders."ProfileMessages"("Id", "AuthorId", "ReceiverId", "Content", "CreatedAt", "AttachedResourceType") VALUES
(1, 1, 6, 'Hello James! I am interested in your new tour. Can you provide more details?', NOW(), 0),
(2, 6, 1, 'Hello Frodo! Thanks for your interest. The tour is scheduled for next weekend. It will be quite thrilling!', NOW(), 0),
(3, 2, 9, 'Diana, do you have any available spots for your ancient wonders tour?', NOW(), 0),
(4, 9, 2, 'Yes Hermione, we still have a few spots available. Sign up soon!', NOW(), 0),
(5, 4, 12, 'Mr. Jones, I would like to join your archaeological tour. Chemistry and history go hand in hand!', NOW(), 0),
(6, 5, 8, 'Hey Peter! Your city tour looks awesome. Can I bring a friend?', NOW(), 0),
(7, 4, 5, 'Jesse, we need to talk. Something went wrong with the last batch.', NOW(), 0),
(8, 5, 4, 'Yo Mr. White, I told you it was clean. The problem is the distributor, not the product.', NOW(), 0),
(9, 4, 5, 'This is not a discussion. If we lose their trust, we lose everything.', NOW(), 0),
(10, 5, 4, 'Relax, okay? I can fix it. Just give me a little time.', NOW(), 0),
(11, 4, 5, 'Time is the one thing we do not have. Meet me tonight. No excuses.', NOW(), 0),
(12, 5, 4, 'Alright… I’ll be there. But this better not turn into another lecture.', NOW(), 0);

INSERT INTO stakeholders."AppRatings"
("Id", "UserId", "Rating", "Comment", "CreatedAt", "UpdatedAt") VALUES
(1, 1, 5, 'Excellent app! Found the perfect adventure for my journey to Mount Doom.', NOW(), NOW()),
(2, 2, 5, 'Brilliant! Very well organized and easy to use.', NOW(), NOW()),
(3, 3, 4, 'The Force is strong with this one. Good app!', NOW(), NOW()),
(4, 4, 5, 'This application applies science to travel planning. Excellent work!', NOW(), NOW()),
(5, 5, 4, 'Yeah! This app is awesome, found some great tours to explore!', NOW(), NOW());

INSERT INTO stakeholders."Clubs"
("Id", "Name", "Description", "ImagePaths", "CreatorId", "Status")
VALUES
(1, 'Fellowship of Travelers', 'A club for adventurers who love hiking and exploring nature. We organize weekend trips to mountains and mysterious lands.',
 '/images/club/fellowship.jpg', 1, 0),
(2, 'Avengers Adventure Club', 'Elite group of travelers seeking extraordinary experiences. Join us for superhero-themed tours and city explorations.',
 '/images/club/avengers.jpg', 11, 0),
(3, 'Archaeological Explorers', 'For those who want to discover ancient ruins and historical wonders. Led by professional archaeologists and adventurers.',
 '/images/club/arch.jpg', 12, 0);

INSERT INTO stakeholders."ClubMembers"
	("ClubId", "TouristId", "JoinedAt")
VALUES
	(1, 2, NOW()),
	(1, 3, NOW()),
	(2, 4, NOW()),
	(2, 5, NOW()),
	(3, 1, NOW());


INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (1, 1, 1, 0, 2, 'Safety issue during the tour - path was not clearly marked', '2024-10-25T10:00:00Z', '2024-10-25T10:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (2, 2, 2, 2, 1, 'Issue with the route plan - some locations were inaccessible', '2024-10-26T11:00:00Z', '2024-10-26T11:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt", "Comments", "IsResolved", "Deadline")
VALUES (3, 3, 3, 1, 0, 'Tour guide communication problem - language barrier', '2023-10-27T12:00:00Z', '2023-10-27T12:05:00Z', ARRAY[]::bigint[], true, null);


INSERT INTO stakeholders."Diaries"("Id", "Name", "CreatedAt", "Country", "City", "TouristId", "Content") VALUES
	(1, 'Journey to Mordor', '2023-10-27T12:00:00Z', 'Middle Earth', 'Mordor', 1, 'Must find a tour guide for the treacherous path ahead. Sam insists we pack extra lembas bread.'),
	(2, 'Magical Adventures', '2024-05-15T14:30:00Z', 'United Kingdom', 'London', 2, 'Planning to explore historical sites. Need to research muggle transportation options.'),
	(3, 'New Mexico Adventure', '2025-01-20T08:00:00Z', 'United States', 'Albuquerque', 5, '- Visit local landmarks\n- Try regional cuisine\n- Check out desert landscapes\n- Take lots of photos'),
	(4, 'Travel Chemistry', '2026-01-25T09:00:00Z', 'United States', 'Santa Fe', 4, 'Planning educational tours that combine science and culture. Research best destinations for scientific tourism.');


INSERT INTO stakeholders."Streaks"(
    "Id", "UserId", "StartDate", "LastActivity", "LongestStreak")
VALUES
(1, 4, '2026-01-20', '2026-02-10', 118); -- Walter has 118 days streak

INSERT INTO stakeholders."UserPremiums"
	("Id", "UserId", "ValidUntil")
VALUES
(1, 1, '2026-12-31T00:00:00Z'),
(2, 8, '2026-12-31T00:00:00Z'),
(3, 6, '2027-01-01T00:00:00Z'), -- Autor James Bond has premium
(4, 10, '2026-12-31T00:00:00Z'),
(5, 11, '2026-12-31T00:00:00Z');

SELECT setval(pg_get_serial_sequence('stakeholders."TourProblems"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."TourProblems"));
SELECT setval(pg_get_serial_sequence('stakeholders."People"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."People"));
SELECT setval(pg_get_serial_sequence('stakeholders."Users"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Users"));
SELECT setval(pg_get_serial_sequence('stakeholders."AppRatings"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."AppRatings"));
SELECT setval(pg_get_serial_sequence('stakeholders."Clubs"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Clubs"));
SELECT setval(pg_get_serial_sequence('stakeholders."ProfileMessages"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."ProfileMessages"));
SELECT setval(pg_get_serial_sequence('stakeholders."ProfileMessages"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."ProfileMessages"));


-- Pathfinder Badges (Level)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (1, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_bronze.png', 0, 0, 1, 0),
    (2, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_silver.png', 1, 0, 10, 0),
    (3, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_gold.png', 2, 0, 25, 0),
    (4, 'Pathfinder', 'Earned by leveling up your account. Higher levels unlock higher badge ranks.', '/images/badges/pathfinder_epic.png', 3, 0, 50, 0);

-- Veteran Badges (Account Age in days)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (5, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_bronze.png', 0, 1, 1, 0),
    (6, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_silver.png', 1, 1, 365, 0),
    (7, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_gold.png', 2, 1, 1095, 0),
    (8, 'Veteran', 'Awarded for your long-term presence on the platform. The longer you stay active, the higher the rank.', '/images/badges/veteran_epic.png', 3, 1, 1825, 0);

-- Explorer Badges (Completed Tours)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (9, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_bronze.png', 0, 2, 1, 1),
    (10, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_silver.png', 1, 2, 10, 1),
    (11, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_gold.png', 2, 2, 25, 1),
    (12, 'Explorer', 'Granted for completing tours. Earn higher ranks by experiencing more tours.', '/images/badges/explorer_epic.png', 3, 2, 50, 1);

-- Challenger Badges (Completed Challenges)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (13, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_bronze.png', 0, 3, 1, 1),
    (14, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_silver.png', 1, 3, 25, 1),
    (15, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_gold.png', 2, 3, 50, 1),
    (16, 'Challenger', 'Earned by completing challenges. Progress through ranks by finishing more challenges.', '/images/badges/challenger_epic.png', 3, 3, 100, 1);

-- Creator Badges (Published Tours)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (17, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_bronze.png', 0, 4, 1, 2),
    (18, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_silver.png', 1, 4, 5, 2),
    (19, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_gold.png', 2, 4, 10, 2),
    (20, 'Creator', 'Awarded for publishing tours. Higher ranks reflect the number of tours you have published.', '/images/badges/creator_epic.png', 3, 4, 25, 2);

-- Entrepreneur Badges (Sold Tours)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (21, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_bronze.png', 0, 5, 1, 2),
    (22, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_silver.png', 1, 5, 10, 2),
    (23, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_gold.png', 2, 5, 50, 2),
    (24, 'Entrepreneur', 'Earned by successfully selling tours. Higher ranks represent increased sales volume.', '/images/badges/entrepreneur_epic.png', 3, 5, 100, 2);

-- Completionist Badge (All Challenge Types Completed)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (25, 'Completionist', 'Unlocked by completing every challenge type at least once.', '/images/badges/completionist.png', 3, 6, 1, 1);

-- Storyteller Badges (Blog Posts)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (26, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_bronze.png', 0, 7, 1, 0),
    (27, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_silver.png', 1, 7, 5, 0),
    (28, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_gold.png', 2, 7, 10, 0),
    (29, 'Storyteller', 'Awarded for publishing blog posts. Advance through ranks by sharing more stories.', '/images/badges/storyteller_epic.png', 3, 7, 25, 0);

-- Community Member Badge (Club Member)
INSERT INTO stakeholders."Badges" ("Id", "Name", "Description", "ImagePath", "Rank", "Type", "RequiredValue", "Role")
VALUES 
    (30, 'Community Member', 'Earned by joining or founding a club and becoming part of the community.', '/images/badges/community_member.png', 3, 8, 1, 0);


INSERT INTO stakeholders."UserStatistics"(
	"Id", "UserId", "Level", "AccountAgeDays", "CompletedToursCount", "CompletedChallengesCount", "PublishedToursCount", "SoldToursCount", "BlogPostsCount", "ChallengeTypesCompletedMask", "JoinedClub", "CreatedAt", "UpdatedAt")
	VALUES
        -- Admin (no CreatedAt in People table, keeping original value)
        (1, 0, 10, 1500, 0, 0, 0, 0, 0, 0, false, NOW(), NOW()),
        -- Tourists (calculated from CreatedAt to 2026-02-11)
        (2, 1, 5, 1064, 8, 3, 0, 0, 1, 7, true, NOW(), NOW()),  -- Frodo: 2023-03-15 to 2026-02-11 = 1064 days
        (3, 2, 4, 998, 6, 2, 0, 0, 3, 3, true, NOW(), NOW()),   -- Hermione: 2023-05-20 to 2026-02-11 = 998 days
        (4, 3, 3, 916, 4, 1, 0, 0, 0, 1, true, NOW(), NOW()),   -- Luke: 2023-08-10 to 2026-02-11 = 916 days
        (5, 4, 6, 761, 10, 5, 0, 0, 2, 15, true, NOW(), NOW()), -- Walter: 2024-01-12 to 2026-02-11 = 761 days
        (6, 5, 4, 688, 5, 2, 0, 0, 0, 3, true, NOW(), NOW()),   -- Jesse: 2024-03-25 to 2026-02-11 = 688 days
        -- Authors (calculated from CreatedAt to 2026-02-11)
        (7, 6, 8, 1488, 0, 0, 12, 45, 5, 0, false, NOW(), NOW()),  -- James Bond: 2022-01-15 to 2026-02-11 = 1488 days
        (8, 7, 9, 1424, 0, 0, 15, 67, 8, 0, false, NOW(), NOW()),  -- Bruce Wayne: 2022-03-20 to 2026-02-11 = 1424 days
        (9, 8, 7, 1342, 0, 0, 10, 32, 4, 0, false, NOW(), NOW()),  -- Peter Parker: 2022-06-10 to 2026-02-11 = 1342 days
        (10, 9, 8, 1286, 0, 0, 13, 54, 6, 0, false, NOW(), NOW()), -- Diana Prince: 2022-08-05 to 2026-02-11 = 1286 days
        (11, 10, 6, 1120, 0, 0, 8, 28, 3, 0, false, NOW(), NOW()), -- Natasha: 2023-01-18 to 2026-02-11 = 1120 days
        (12, 11, 7, 1026, 0, 0, 11, 41, 5, 0, true, NOW(), NOW()), -- Steve Rogers: 2023-04-22 to 2026-02-11 = 1026 days
        (13, 12, 10, 943, 0, 0, 18, 89, 12, 0, true, NOW(), NOW()),-- Indiana Jones: 2023-07-14 to 2026-02-11 = 943 days
        (14, 13, 6, 865, 0, 0, 7, 23, 2, 0, false, NOW(), NOW()),  -- Ellen Ripley: 2023-09-30 to 2026-02-11 = 865 days
        (15, 14, 5, 731, 0, 0, 6, 19, 1, 0, false, NOW(), NOW()),  -- Rick Deckard: 2024-02-11 to 2026-02-11 = 731 days
        (16, 15, 4, 665, 0, 0, 5, 15, 0, 0, false, NOW(), NOW()),  -- John McClane: 2024-04-17 to 2026-02-11 = 665 days
        (17, 16, 7, 598, 0, 0, 9, 38, 4, 0, false, NOW(), NOW()),  -- Lara Croft: 2024-06-23 to 2026-02-11 = 598 days
        (18, 17, 6, 552, 0, 0, 8, 31, 3, 0, false, NOW(), NOW()),  -- Ethan Hunt: 2024-08-08 to 2026-02-11 = 552 days
        (19, 18, 3, 480, 0, 0, 3, 8, 1, 0, false, NOW(), NOW()),   -- Max: 2024-10-19 to 2026-02-11 = 480 days
        (20, 19, 5, 433, 0, 0, 7, 26, 2, 0, false, NOW(), NOW());  -- Trinity: 2024-12-05 to 2026-02-11 = 433 days

INSERT INTO stakeholders."UserBadges"(
	"Id", "UserId", "BadgeId", "EarnedAt")
	VALUES
        -- Tourists badges
        (1, 1, 30, NOW()),  -- Frodo - Community Member
        (2, 1, 9, NOW()),   -- Frodo - Explorer Bronze
        (3, 1, 10, NOW()),  -- Frodo - Explorer Silver
        (4, 2, 30, NOW()),  -- Hermione - Community Member
        (5, 2, 9, NOW()),   -- Hermione - Explorer Bronze
        (6, 2, 26, NOW()),  -- Hermione - Storyteller Bronze
        (7, 3, 30, NOW()),  -- Luke - Community Member
        (8, 3, 9, NOW()),   -- Luke - Explorer Bronze
        (9, 4, 30, NOW()),  -- Walter - Community Member
        (10, 4, 9, NOW()),  -- Walter - Explorer Bronze
        (11, 4, 10, NOW()),  -- Walter - Explorer Silver
        (12, 4, 26, NOW()),  -- Walter - Storyteller Bronze
        (13, 5, 30, NOW()),  -- Jesse - Community Member
        (14, 5, 9, NOW()),   -- Jesse - Explorer Bronze
        -- Authors badges
        (15, 6, 17, NOW()),  -- James Bond - Creator Bronze
        (16, 6, 18, NOW()),  -- James Bond - Creator Silver
        (17, 6, 21, NOW()),  -- James Bond - Entrepreneur Bronze
        (18, 6, 22, NOW()),  -- James Bond - Entrepreneur Silver
        (19, 7, 17, NOW()),  -- Bruce Wayne - Creator Bronze
        (20, 7, 18, NOW()),  -- Bruce Wayne - Creator Silver
        (21, 7, 21, NOW()),  -- Bruce Wayne - Entrepreneur Bronze
        (22, 7, 22, NOW()),  -- Bruce Wayne - Entrepreneur Silver
        (23, 8, 17, NOW()),  -- Peter Parker - Creator Bronze
        (24, 8, 18, NOW()),  -- Peter Parker - Creator Silver
        (25, 8, 21, NOW()),  -- Peter Parker - Entrepreneur Bronze
        (26, 9, 17, NOW()),  -- Diana Prince - Creator Bronze
        (27, 9, 18, NOW()),  -- Diana Prince - Creator Silver
        (28, 9, 21, NOW()),  -- Diana Prince - Entrepreneur Bronze
        (29, 9, 22, NOW()),  -- Diana Prince - Entrepreneur Silver
        (30, 10, 17, NOW()), -- Natasha - Creator Bronze
        (31, 10, 21, NOW()), -- Natasha - Entrepreneur Bronze
        (32, 11, 30, NOW()), -- Steve Rogers - Community Member
        (33, 11, 17, NOW()), -- Steve Rogers - Creator Bronze
        (34, 11, 18, NOW()), -- Steve Rogers - Creator Silver
        (35, 11, 21, NOW()), -- Steve Rogers - Entrepreneur Bronze
        (36, 12, 30, NOW()), -- Indiana Jones - Community Member
        (37, 12, 17, NOW()), -- Indiana Jones - Creator Bronze
        (38, 12, 18, NOW()), -- Indiana Jones - Creator Silver
        (39, 12, 19, NOW()), -- Indiana Jones - Creator Gold
        (40, 12, 21, NOW()), -- Indiana Jones - Entrepreneur Bronze
        (41, 12, 22, NOW()), -- Indiana Jones - Entrepreneur Silver
        (42, 12, 23, NOW()), -- Indiana Jones - Entrepreneur Gold
        (43, 13, 17, NOW()), -- Ellen Ripley - Creator Bronze
        (44, 13, 21, NOW()), -- Ellen Ripley - Entrepreneur Bronze
        (45, 16, 17, NOW()), -- Lara Croft - Creator Bronze
        (46, 16, 21, NOW()), -- Lara Croft - Entrepreneur Bronze
        (47, 17, 17, NOW()), -- Ethan Hunt - Creator Bronze
        (48, 17, 21, NOW()), -- Ethan Hunt - Entrepreneur Bronze
        (49, 19, 17, NOW()), -- Trinity - Creator Bronze
        (50, 19, 21, NOW()); -- Trinity - Entrepreneur Bronze

SELECT setval(pg_get_serial_sequence('stakeholders."Badges"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Badges"));
SELECT setval(pg_get_serial_sequence('stakeholders."UserStatistics"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."UserStatistics"));
SELECT setval(pg_get_serial_sequence('stakeholders."UserBadges"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."UserBadges"));
SELECT setval(pg_get_serial_sequence('stakeholders."UserPremiums"', 'Id'), (SELECT COALESCE(MAX("Id"), 0) FROM stakeholders."UserPremiums"));
SELECT setval(pg_get_serial_sequence('stakeholders."Planners"', 'Id'), (SELECT COALESCE(MAX("Id"),1) FROM stakeholders."Planners"));
SELECT setval(pg_get_serial_sequence('stakeholders."PlannerDay"', 'Id'), (SELECT COALESCE(MAX("Id"),1) FROM stakeholders."PlannerDay"));
SELECT setval(pg_get_serial_sequence('stakeholders."PlannerTimeBlock"', 'Id'), (SELECT COALESCE(MAX("Id"),1) FROM stakeholders."PlannerTimeBlock"));
SELECT setval(pg_get_serial_sequence('stakeholders."Diaries"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Diaries"));
SELECT setval(pg_get_serial_sequence('stakeholders."Streaks"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM stakeholders."Streaks"));
