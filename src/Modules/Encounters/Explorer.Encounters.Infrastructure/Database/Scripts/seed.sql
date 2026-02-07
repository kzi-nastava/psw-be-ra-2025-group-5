DELETE FROM encounters."Challenges";

-- ChallengeStatus enum values:
-- 0 = Draft, 1 = Active, 2 = Archived, 3 = Pending

-- ChallengeType enum values:
-- 0 = Social (requires RequiredParticipants >= 2 and RadiusInMeters > 0)
-- 1 = Location
-- 2 = Misc
-- 3 = TimeBased (requires EndChallenge != null and in future)
-- 4 = Community (requires DailyParticipantLimit >= 1)

-- Belgrade Challenges (Serbia)
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (1, 'Belgrade Fortress Explorer', 'Visit Belgrade Fortress and capture a photo with the river view from Kalemegdan.', 44.823398, 20.450554, 100, 1, 1, null, null, null, '/images/challenge/belgrade-fortress.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (2, 'Meet the Locals', 'Meet three local residents and learn their stories about Belgrade history.', 44.815556, 20.460833, 200, 1, 0, null, 3, 50, '/images/challenge/slika1.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (3, 'Hidden Location Mystery', 'Find the hidden location in the old town and discover its secret.', 44.818611, 20.457222, 150, 0, 2, null, null, null, '/images/challenge/slika1.jpg', null, null);

-- Chicago Challenges (USA)
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (4, 'Willis Tower Skydeck Challenge', 'Step onto the glass ledge at Willis Tower Skydeck and take a photo 103 floors above Chicago!', 41.8789, -87.6359, 250, 1, 1, null, null, null, '/images/challenge/willis-tower.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (5, 'Riverwalk Group Stroll', 'Walk the Chicago Riverwalk with friends and collect stories from at least 3 different bridges.', 41.8881, -87.6238, 180, 1, 0, null, 2, 100, '/images/challenge/slika1.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (6, 'Architecture Photo Hunt', 'Capture photos of 5 different Art Deco buildings in downtown Chicago at night.', 41.8826, -87.6226, 200, 1, 2, null, null, null, '/images/challenge/slika1.jpg', null, null);

-- Iceland Challenges
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (7, 'Ice Cave Explorer', 'Enter the blue ice caves in Vatnajökull and capture the crystal formations.', 64.0420, -16.1788, 300, 1, 1, null, null, null, '/images/challenge/ice-cave.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (8, 'Diamond Beach Treasure Hunt', 'Find and photograph 3 different ice chunks on Diamond Beach that look like diamonds.', 64.0488, -16.1796, 180, 1, 2, null, null, null, '/images/challenge/slika1.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (9, 'Glacier Group Adventure', 'Complete a guided glacier walk with your group on Vatnajökull.', 64.4167, -16.8167, 350, 1, 0, null, 4, 200, '/images/challenge/slika1.jpg', null, null);

-- New York Challenges (USA)
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (10, 'Empire State Building Sunrise', 'Reach the Empire State Building observation deck before sunrise and capture the city waking up.', 40.7484, -73.9857, 280, 1, 3, null, null, null, '/images/challenge/slika1.jpg', '2027-06-30 23:59:59+00', null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (11, 'Brooklyn Bridge Walk Challenge', 'Walk across the entire Brooklyn Bridge and meet someone from another country.', 40.7061, -73.9969, 150, 1, 0, null, 2, 150, '/images/challenge/slika1.jpg', null, null);

-- Ancient Greece Challenges
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (12, 'Acropolis Mythology Quest', 'Visit the Acropolis and learn about at least 3 Greek gods associated with the site.', 37.9715, 23.7266, 200, 1, 1, null, null, null, '/images/challenge/acropolis.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (13, 'Oracle of Delphi Secret', 'Discover the hidden chamber beneath the Oracle of Delphi temple.', 38.4824, 22.5010, 250, 1, 2, null, null, null, '/images/challenge/slika1.jpg', null, null);

-- Venice Challenges (Italy)
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (14, 'Secret Canal Discovery', 'Navigate through Casanova escape route in Venice hidden canals with a guide.', 45.4371, 12.3326, 220, 1, 1, null, null, null, '/images/challenge/canal.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (15, 'Gondola Group Experience', 'Share a traditional gondola ride with new friends through Venice canals.', 45.4380, 12.3358, 180, 1, 0, null, 4, 100, '/images/challenge/slika1.jpg', null, null);

-- Egypt Challenges
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (16, 'Great Pyramid Challenge', 'Find the hidden chamber marker at the Great Pyramid of Giza.', 29.9792, 31.1342, 300, 1, 1, null, null, null, '/images/challenge/great-pyramid.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (17, 'Valley of Kings Explorer', 'Locate the undiscovered tomb entrance marker in the Valley of the Kings.', 25.7402, 32.6014, 320, 1, 2, null, null, null, '/images/challenge/slika1.jpg', null, null);

-- Time-Limited Challenges
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (18, 'Summer Solstice at Machu Picchu', 'Witness the sunrise at Machu Picchu Sun Gate during the summer solstice period.', -13.1635, -72.5450, 400, 1, 3, null, null, null, '/images/challenge/slika1.jpg', '2027-06-21 23:59:59+00', null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (19, 'New Year at Times Square', 'Experience New Year Eve celebration at Times Square before the deadline.', 40.7580, -73.9855, 350, 1, 3, null, null, null, '/images/challenge/slika1.jpg', '2026-12-31 23:59:59+00', null);

-- Community Challenges (Daily Participant Limit)
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (20, 'Exclusive Skaftafell Tour', 'Join the exclusive daily limited tour of Skaftafell Nature Reserve hidden trails.', 64.0157, -16.9758, 280, 1, 4, null, null, null, '/images/challenge/skaftafell-exclusive.jpg', null, 5);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (21, 'VIP Louvre Access', 'Gain access to the exclusive daily limited VIP tour of hidden Louvre chambers.', 48.8606, 2.3376, 300, 1, 4, null, null, null, '/images/challenge/louvre-vip.jpg', null, 3);

-- Draft and Archived Challenges
INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (22, 'Boston Tea Party Reenactment', 'Participate in historical reenactment at Boston Harbor (Draft for approval).', 42.3551, -71.0567, 180, 0, 2, null, null, null, '/images/challenge/boston-tea.jpg', null, null);

INSERT INTO encounters."Challenges"(
    "Id", "Name", "Description", "Latitude", "Longitude", "ExperiencePoints", "Status", "Type", "CreatedByTouristId", "RequiredParticipants", "RadiusInMeters", "ImageUrl", "EndChallenge", "DailyParticipantLimit")
VALUES (23, 'Colosseum Night Tour', 'Special night tour of Colosseum (Archived - completed season).', 41.8902, 12.4922, 250, 2, 1, null, null, null, '/images/challenge/colosseum-night.jpg', null, null);

SELECT setval(pg_get_serial_sequence('encounters."Challenges"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM encounters."Challenges"));