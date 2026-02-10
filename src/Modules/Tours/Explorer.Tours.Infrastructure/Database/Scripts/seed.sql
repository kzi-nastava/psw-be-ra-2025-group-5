DELETE FROM tours."TouristPreferences";
DELETE FROM tours."TourSponsorships";
DELETE FROM tours."Tours";
DELETE FROM tours."Facilities";
DELETE FROM tours."Monument";
DELETE FROM tours."Equipment";
DELETE FROM tours."TouristEquipment";
DELETE FROM tours."TourReviews";
DELETE FROM tours."TourRequests";
DELETE FROM tours."TourRequiredEquipment";
DELETE FROM tours."KeyPoints";
DELETE FROM tours."TourDuration";

-- TouristPreferences (Walter White and Jesse Pinkman)
INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (1, 4, 1, '{"Walking":3,"Bicycle":2,"Car":1,"Boat":0}', '["Education","Science","History"]');

INSERT INTO tours."TouristPreferences" ("Id", "UserId", "PreferredDifficulty", "TransportationRatings", "PreferredTags")
VALUES (2, 5, 0, '{"Walking":2,"Bicycle":3,"Car":2,"Boat":1}', '["Adventure","Nature","Urban"]');

-- Tours with random IDs
-- James Bond tours (Author ID 6) - 5 tours
INSERT INTO tours."Tours" VALUES (3, 6, 'London Espionage Trail', 'Discover the secret world of British intelligence as we explore MI6 headquarters, historic spy meeting points, and iconic locations from espionage history.', 0, '{History,Culture,Urban,Espionage}', 45.00, 1, '2024-08-15 10:00:00', '2024-08-20 12:00:00', NULL, 4.8, 12.5, '/images/tours/london-spy.jpg');

INSERT INTO tours."Tours" VALUES (7, 6, 'Monte Carlo Casino Luxury Tour', 'Experience the glamour of Monte Carlo with visits to the famous casino, yacht harbor, and exclusive venues frequented by high society.', 0, '{Luxury,Culture,Entertainment}', 120.00, 1, '2024-07-20 09:00:00', '2024-07-25 11:00:00', NULL, 4.9, 8.0, '/images/tours/monte-carlo.jpg');

INSERT INTO tours."Tours" VALUES (12, 6, 'Venice Canal Mystery Tour', 'Navigate the mysterious canals of Venice, exploring hidden passages and secret meeting points used throughout history.', 1, '{Mystery,Culture,Water,Adventure}', 65.00, 1, '2024-06-10 08:00:00', '2024-06-15 10:00:00', NULL, 4.7, 10.5, '/images/tours/venice.jpeg');

INSERT INTO tours."Tours" VALUES (18, 6, 'Swiss Alps Skiing Adventure', 'Hit the slopes of the Swiss Alps with expert guides, combining thrilling skiing with breathtaking alpine scenery.', 2, '{Adventure,Sports,Nature,Winter}', 180.00, 1, '2024-02-01 07:00:00', '2024-02-05 09:00:00', NULL, 4.6, 25.0, '/images/tours/swiss-alps.jpg');

INSERT INTO tours."Tours" VALUES (24, 6, 'Istanbul Grand Bazaar Experience', 'Immerse yourself in the vibrant culture of Istanbul, exploring the Grand Bazaar, historic mosques, and traditional Turkish cuisine.', 0, '{Culture,Shopping,Food,History}', 55.00, 1, '2024-09-12 11:00:00', '2024-09-17 13:00:00', NULL, 4.5, 9.0, '/images/tours/istanbul.jpg');

-- Bruce Wayne tours (Author ID 7) - 5 tours
INSERT INTO tours."Tours" VALUES (1, 7, 'Chicago Night Architecture Tour', 'Experience Chicago after dark, touring famous Art Deco skyscrapers, rooftop viewpoints, and illuminated architectural masterpieces.', 1, '{Urban,Night,Architecture,Culture}', 75.00, 1, '2024-10-05 19:00:00', '2024-10-10 20:00:00', NULL, 4.9, 15.0, '/images/tours/chicago-night.jpg');

INSERT INTO tours."Tours" VALUES (5, 7, 'Newport Mansions Heritage Tour', 'Explore the opulent Gilded Age mansions of Newport, Rhode Island, learning about architectural history and America''s wealthiest families.', 0, '{History,Architecture,Culture}', 85.00, 1, '2024-08-01 10:00:00', '2024-08-06 11:00:00', NULL, 4.8, 11.0, '/images/tours/newport.jpg');

INSERT INTO tours."Tours" VALUES (11, 7, 'New York Skyline Photography', 'Capture stunning cityscape photographs from exclusive Manhattan rooftop locations, perfect for photography enthusiasts.', 1, '{Photography,Urban,Art}', 95.00, 1, '2024-07-15 06:00:00', '2024-07-20 08:00:00', NULL, 4.7, 8.5, '/images/tours/nys.jpg');

INSERT INTO tours."Tours" VALUES (16, 7, 'Boston Crime History Tour', 'Follow the trail of famous Boston crimes through historic neighborhoods, learning about detective cases and criminal investigations.', 0, '{Mystery,Urban,Education,Entertainment}', 60.00, 1, '2024-09-08 14:00:00', '2024-09-13 15:00:00', NULL, 4.6, 10.0, '/images/tours/boston.jpg');

INSERT INTO tours."Tours" VALUES (22, 7, 'San Francisco Social Impact Tour', 'Visit innovative non-profits and social enterprises in Silicon Valley, learning about modern philanthropy and community service.', 0, '{Culture,Education,Social}', 40.00, 1, '2024-10-20 09:00:00', '2024-10-25 10:00:00', NULL, 4.5, 7.5, '/images/tours/san.jpg');

-- Peter Parker tours (Author ID 8) - 2 tours
INSERT INTO tours."Tours" VALUES (2, 8, 'New York Tour', 'Swing through New York''s most iconic skyscrapers and landmarks, exploring the city from unique vantage points.', 1, '{Urban,Adventure,Photography,Architecture}', 70.00, 1, '2024-09-01 09:00:00', '2024-09-06 10:00:00', NULL, 4.7, 14.0, '/images/tours/ny.jpg');

INSERT INTO tours."Tours" VALUES (14, 8, 'Queens Neighborhood Heritage', 'Discover the diverse neighborhoods of Queens, exploring local culture, food, and community history.', 0, '{Culture,Food,Urban,History}', 45.00, 1, '2024-08-22 11:00:00', '2024-08-27 12:00:00', NULL, 4.4, 9.0, '/images/tours/queens.jpg');

-- Diana Prince tours (Author ID 9) - 3 tours
INSERT INTO tours."Tours" VALUES (4, 9, 'Ancient Greece Mythology Tour', 'Walk in the footsteps of gods and heroes, exploring ancient Greek ruins, temples, and archaeological sites.', 1, '{History,Culture,Mythology,Ancient}', 90.00, 1, '2024-07-10 08:00:00', '2024-07-15 09:00:00', NULL, 4.9, 16.5, '/images/tours/greece.jpg');

INSERT INTO tours."Tours" VALUES (9, 9, 'Amazon Rainforest Expedition', 'Journey into the heart of the Amazon, discovering indigenous cultures and incredible biodiversity.', 2, '{Nature,Adventure,Wildlife,Culture}', 250.00, 1, '2024-06-01 06:00:00', '2024-06-06 07:00:00', NULL, 4.8, 35.0, '/images/tours/amazon.jpg');

INSERT INTO tours."Tours" VALUES (20, 9, 'Mediterranean Island Hopping', 'Sail through the Mediterranean, visiting beautiful islands, ancient ruins, and pristine beaches.', 1, '{Water,Culture,Relaxation,History}', 180.00, 1, '2024-08-05 10:00:00', '2024-08-10 11:00:00', NULL, 4.6, 45.0, '/images/tours/mediterranean.jpg');

-- Natasha Romanoff tours (Author ID 10) - 2 tours
INSERT INTO tours."Tours" VALUES (6, 10, 'Moscow Underground Secrets', 'Explore hidden Soviet-era bunkers, secret metro stations, and Cold War espionage sites in Moscow.', 1, '{History,Underground,Espionage,Urban}', 85.00, 1, '2024-09-15 12:00:00', '2024-09-20 13:00:00', NULL, 4.7, 11.0, '/images/tours/moscow.jpg');

INSERT INTO tours."Tours" VALUES (17, 10, 'Budapest Thermal Spa Experience', 'Relax in Budapest''s historic thermal baths while learning about their Roman origins and healing properties.', 0, '{Relaxation,History,Wellness,Culture}', 55.00, 1, '2024-10-01 14:00:00', '2024-10-06 15:00:00', NULL, 4.5, 6.0, '/images/tours/budapest.jpg');

-- Steve Rogers tours (Author ID 11) - 2 tours
INSERT INTO tours."Tours" VALUES (8, 11, 'WWII Memorial Tour', 'Visit significant WWII memorials and museums, honoring the sacrifices and learning about wartime history.', 0, '{History,Education,Memorial,Culture}', 50.00, 1, '2024-07-04 10:00:00', '2024-07-09 11:00:00', NULL, 4.8, 12.0, '/images/tours/wwii-memorial.jpg');

INSERT INTO tours."Tours" VALUES (19, 11, 'Brooklyn Historical Walking Tour', 'Discover Brooklyn''s rich history, from colonial times to modern day, through its neighborhoods and landmarks.', 0, '{History,Urban,Culture,Walking}', 40.00, 1, '2024-08-18 09:00:00', '2024-08-23 10:00:00', NULL, 4.6, 8.5, '/images/tours/brooklyn.jpg');

-- Indiana Jones tours (Author ID 12) - 3 tours
INSERT INTO tours."Tours" VALUES (10, 12, 'Egyptian Pyramids Expedition', 'Uncover the secrets of ancient Egypt, exploring pyramids, tombs, and archaeological treasures.', 1, '{History,Ancient,Adventure,Archaeology}', 200.00, 1, '2024-05-15 07:00:00', '2024-05-20 08:00:00', NULL, 4.9, 18.0, '/images/tours/egypt.jpg');

INSERT INTO tours."Tours" VALUES (15, 12, 'Petra Lost City Discovery', 'Trek through the desert to discover Petra, the ancient Nabatean city carved into rose-red cliffs.', 2, '{History,Adventure,Ancient,Desert}', 175.00, 1, '2024-06-20 06:00:00', '2024-06-25 07:00:00', NULL, 4.8, 22.0, '/images/tours/petra.jpg');

INSERT INTO tours."Tours" VALUES (23, 12, 'Machu Picchu Trail Adventure', 'Follow the Inca Trail to the legendary mountain citadel of Machu Picchu.', 2, '{History,Ancient,Adventure,Hiking}', 220.00, 1, '2024-04-10 05:00:00', '2024-04-15 06:00:00', NULL, 4.9, 26.0, '/images/tours/machu-picchu.jpg');

-- Ellen Ripley tours (Author ID 13) - 2 tours
INSERT INTO tours."Tours" VALUES (13, 13, 'Space Center Houston Tour', 'Explore NASA''s Johnson Space Center, learning about space exploration and astronaut training.', 0, '{Science,Space,Education,Technology}', 65.00, 1, '2024-07-25 09:00:00', '2024-07-30 10:00:00', NULL, 4.7, 7.5, '/images/tours/space-center.jpg');

INSERT INTO tours."Tours" VALUES (21, 13, 'Kennedy Space Center Experience', 'Witness rocket launches and explore the history of space exploration at Kennedy Space Center.', 0, '{Science,Space,Education,Technology}', 75.00, 1, '2024-08-12 08:00:00', '2024-08-17 09:00:00', NULL, 4.6, 8.0, '/images/tours/kennedy-space.jpg');

-- Lara Croft tour (Author ID 16) - 2 tours
INSERT INTO tours."Tours" VALUES (25, 16, 'Angkor Wat Temple Explorer', 'Navigate ancient temples in the Cambodian jungle, uncovering hidden chambers and ancient secrets.', 2, '{Adventure,History,Ancient,Jungle}', 160.00, 1, '2024-05-05 06:00:00', '2024-05-10 07:00:00', NULL, 4.8, 20.0, '/images/tours/angkor-wat.jpg');

INSERT INTO tours."Tours" VALUES (27, 16, 'Icelandic Glacier Trek', 'Trek across stunning glaciers and explore ice caves in Iceland''s dramatic landscape.', 2, '{Adventure,Nature,Winter,Extreme}', 195.00, 1, '2024-03-15 08:00:00', '2024-03-20 09:00:00', NULL, 4.7, 15.0, '/images/tours/iceland-glacier.jpg');

-- Max Rockatansky tour (Author ID 18) - 1 tour
INSERT INTO tours."Tours" VALUES (26, 18, 'Australian Outback Survival', 'Learn survival skills while exploring the harsh beauty of the Australian Outback.', 2, '{Adventure,Nature,Survival,Desert}', 140.00, 1, '2024-04-20 07:00:00', '2024-04-25 08:00:00', NULL, 4.5, 30.0, '/images/tours/outback.jpg');

-- KeyPoints for tours
-- Tour 1: Chicago Night Architecture Tour (Bruce Wayne) - 4 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (1, 'Willis Tower Skydeck', 'Start at the iconic Willis Tower with breathtaking night views from 103rd floor.', '{"Latitude":41.8789,"Longitude":-87.6359}', '/images/keypoints/willis-tower-skydeck.jpg', 'Glass ledge viewing box', 0, 1);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (2, 'Tribune Tower', 'Marvel at the neo-Gothic architecture with fragments from world landmarks.', '{"Latitude":41.8902,"Longitude":-87.6234}', '/images/keypoints/grand-central.jpg', 'Pieces of famous buildings embedded in walls', 1, 1);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (3, 'Chicago Riverwalk', 'Walk along the illuminated riverwalk with stunning bridge views.', '{"Latitude":41.8881,"Longitude":-87.6238}', '/images/keypoints/riverwalk.jpg', NULL, 2, 1);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (4, '360 Chicago Observatory', 'End at John Hancock Center with panoramic city views and TILT experience.', '{"Latitude":41.8989,"Longitude":-87.6230}', '/images/keypoints/360.jpg', 'Tilting glass window experience', 3, 1);

-- Tour 2: New York Tour (Peter Parker) - 5 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (5, 'Empire State Building', 'Start at the iconic Empire State Building observation deck.', '{"Latitude":40.7484,"Longitude":-73.9857}', '/images/keypoints/empire-state.jpg', NULL, 0, 2);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (6, 'Times Square', 'Experience the energy of Times Square from above.', '{"Latitude":40.7580,"Longitude":-73.9855}', '/images/keypoints/times-square.jpg', NULL, 1, 2);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (7, 'One World Trade Center', 'Visit the observation deck of One World Trade.', '{"Latitude":40.7127,"Longitude":-74.0134}', '/images/keypoints/one-world.jpg', NULL, 2, 2);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (8, 'Chrysler Building', 'Admire the Art Deco architecture of the Chrysler Building.', '{"Latitude":40.7516,"Longitude":-73.9756}', '/images/keypoints/chrysler.jpg', NULL, 3, 2);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (9, 'Central Park Overlook', 'Finish with views of Central Park from above.', '{"Latitude":40.7829,"Longitude":-73.9654}', '/images/keypoints/central-park-view.jpg', NULL, 4, 2);

-- Tour 3: London Espionage Trail (James Bond) - 5 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (10, 'MI6 Headquarters', 'View the distinctive MI6 building from the Thames.', '{"Latitude":51.4874,"Longitude":-0.1245}', '/images/keypoints/mi6.jpg', 'Q-Branch entrance location', 0, 3);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (11, 'Churchill War Rooms', 'Explore the secret underground headquarters from WWII.', '{"Latitude":51.5022,"Longitude":-0.1295}', '/images/keypoints/war-rooms.jpg', NULL, 1, 3);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (12, 'Bletchley Park Memorial', 'Learn about the codebreakers who changed history.', '{"Latitude":51.9976,"Longitude":-0.7406}', '/images/keypoints/bletchley.jpg', NULL, 2, 3);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (13, 'The Ritz Hotel', 'Visit the luxurious hotel frequented by spies and diplomats.', '{"Latitude":51.5074,"Longitude":-0.1419}', '/images/keypoints/ritz.jpg', 'Secret meeting room', 3, 3);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (14, 'London Bridge', 'Historical espionage meeting point with river views.', '{"Latitude":51.5081,"Longitude":-0.0877}', '/images/keypoints/london-bridge.jpg', NULL, 4, 3);

-- Tour 4: Ancient Greece Mythology Tour (Diana Prince) - 4 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (15, 'Acropolis of Athens', 'Begin at the iconic Parthenon and Acropolis complex.', '{"Latitude":37.9715,"Longitude":23.7266}', '/images/keypoints/acropolis.jpg', NULL, 0, 4);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (16, 'Temple of Poseidon', 'Visit the clifftop temple overlooking the Aegean Sea.', '{"Latitude":37.6531,"Longitude":24.0250}', '/images/keypoints/temple.jpg', 'Oracle inscription', 1, 4);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (17, 'Oracle of Delphi', 'Explore the ancient sanctuary where prophecies were made.', '{"Latitude":38.4824,"Longitude":22.5010}', '/images/keypoints/delphi.jpg', 'Hidden chamber beneath temple', 2, 4);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (18, 'Theater of Epidaurus', 'Experience the incredible acoustics of this ancient theater.', '{"Latitude":37.5960,"Longitude":23.0788}', '/images/keypoints/epidaurus.jpg', NULL, 3, 4);

-- Tour 5: Newport Mansions Heritage Tour (Bruce Wayne) - 3 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (19, 'The Breakers', 'Tour the grandest of Newport''s Gilded Age mansions, built by Cornelius Vanderbilt II.', '{"Latitude":41.4707,"Longitude":-71.3006}', '/images/keypoints/breakers.jpg', NULL, 0, 5);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (20, 'Marble House', 'Explore the opulent mansion inspired by Versailles, built for $11 million in 1892.', '{"Latitude":41.4695,"Longitude":-71.3028}', '/images/keypoints/biltmore.jpg', 'Secret Chinese Tea House', 1, 5);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (21, 'Rosecliff Mansion', 'Visit the elegant ballroom mansion modeled after Grand Trianon at Versailles.', '{"Latitude":41.4752,"Longitude":-71.3045}', '/images/keypoints/hearst.jpg', NULL, 2, 5);

-- Tour 6: Moscow Underground Secrets (Natasha Romanoff) - 4 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (22, 'Bunker 42', 'Descend into a Cold War-era nuclear bunker 60 meters underground.', '{"Latitude":55.7419,"Longitude":37.6533}', '/images/keypoints/bunker42.jpg', 'KGB communication room', 0, 6);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (23, 'Stalin''s Secret Metro', 'Explore rumored secret metro lines and stations.', '{"Latitude":55.7558,"Longitude":37.6173}', '/images/keypoints/secret-metro.jpg', 'Metro-2 entrance', 1, 6);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (24, 'KGB Museum', 'Visit the historic headquarters and learn about espionage tactics.', '{"Latitude":55.7601,"Longitude":37.6308}', '/images/keypoints/kgb.jpg', NULL, 2, 6);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (25, 'Kremlin Tunnels', 'Discover the network of underground passages beneath the Kremlin.', '{"Latitude":55.7520,"Longitude":37.6175}', '/images/keypoints/kremlin-tunnels.jpg', 'Emergency escape route', 3, 6);

-- Tour 7: Monte Carlo Casino Luxury Tour (James Bond) - 3 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (26, 'Casino de Monte-Carlo', 'Experience the world-famous casino in all its glamour.', '{"Latitude":43.7396,"Longitude":7.4284}', '/images/keypoints/monte-carlo-casino.jpg', 'High roller''s private room', 0, 7);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (27, 'Port Hercules', 'Admire luxury yachts at Monaco''s prestigious harbor.', '{"Latitude":43.7342,"Longitude":7.4256}', '/images/keypoints/port-hercules.jpg', NULL, 1, 7);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (28, 'Prince''s Palace', 'Visit the official residence of the Prince of Monaco.', '{"Latitude":43.7312,"Longitude":7.4197}', '/images/keypoints/palace-monaco.jpg', NULL, 2, 7);

-- Tour 8: WWII Memorial Tour (Steve Rogers) - 4 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (29, 'National WWII Memorial', 'Pay respects at the memorial honoring WWII veterans.', '{"Latitude":38.8894,"Longitude":-77.0404}', '/images/keypoints/wwii-memorial.jpg', NULL, 0, 8);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (30, 'Arlington National Cemetery', 'Visit the Tomb of the Unknown Soldier and memorial sections.', '{"Latitude":38.8783,"Longitude":-77.0687}', '/images/keypoints/arlington.jpg', NULL, 1, 8);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (31, 'USS Intrepid Museum', 'Explore the historic aircraft carrier and air museum.', '{"Latitude":40.7645,"Longitude":-73.9997}', '/images/keypoints/intrepid.jpg', NULL, 2, 8);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (32, 'D-Day Memorial', 'Learn about the Normandy invasion and Allied victory.', '{"Latitude":37.3307,"Longitude":-79.5439}', '/images/keypoints/dday.jpg', NULL, 3, 8);

-- Tour 9: Amazon Rainforest Expedition (Diana Prince) - 5 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (33, 'Manaus Starting Point', 'Begin in the gateway city to the Amazon.', '{"Latitude":-3.1190,"Longitude":-60.0217}', '/images/keypoints/manaus.jpg', NULL, 0, 9);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (34, 'Indigenous Village', 'Meet local tribes and learn about their traditions.', '{"Latitude":-3.4653,"Longitude":-62.2159}', '/images/keypoints/village.jpg', 'Sacred ceremony site', 1, 9);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (35, 'Canopy Walkway', 'Walk among the treetops on suspended bridges.', '{"Latitude":-2.9300,"Longitude":-59.9719}', '/images/keypoints/canopy.jpg', NULL, 2, 9);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (36, 'Piranha Fishing', 'Try traditional fishing methods in the Amazon River.', '{"Latitude":-3.3792,"Longitude":-58.7525}', '/images/keypoints/fishing.jpg', NULL, 3, 9);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (37, 'Night Safari', 'Experience the rainforest at night with guide.', '{"Latitude":-3.1028,"Longitude":-60.0250}', '/images/keypoints/night-safari.jpg', 'Rare nocturnal species location', 4, 9);

-- Tour 10: Egyptian Pyramids Expedition (Indiana Jones) - 5 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (38, 'Great Pyramid of Giza', 'Marvel at the last standing wonder of the ancient world.', '{"Latitude":29.9792,"Longitude":31.1342}', '/images/keypoints/great-pyramid.jpg', 'Hidden chamber discovered', 0, 10);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (39, 'Sphinx', 'See the enigmatic guardian of the pyramids.', '{"Latitude":29.9753,"Longitude":31.1376}', '/images/keypoints/sphinx.jpg', NULL, 1, 10);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (40, 'Valley of the Kings', 'Explore tombs of pharaohs in this ancient necropolis.', '{"Latitude":25.7402,"Longitude":32.6014}', '/images/keypoints/valley-kings.jpg', 'Undiscovered tomb entrance', 2, 10);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (41, 'Karnak Temple', 'Walk through the vast temple complex dedicated to Amun.', '{"Latitude":25.7188,"Longitude":32.6573}', '/images/keypoints/karnak.jpg', NULL, 3, 10);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (42, 'Egyptian Museum', 'View Tutankhamun''s treasures and ancient artifacts.', '{"Latitude":30.0478,"Longitude":31.2336}', '/images/keypoints/museum-cairo.jpg', NULL, 4, 10);

-- Tour 11: New York Skyline Photography (Bruce Wayne) - 3 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (43, 'Top of the Rock', 'Capture golden hour from Rockefeller Center''s observation deck with Empire State views.', '{"Latitude":40.7587,"Longitude":-73.9787}', '/images/keypoints/sunrise-roof.jpg', 'Best photo angle for Empire State Building', 0, 11);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (44, 'Brooklyn Bridge Park', 'Perfect Manhattan skyline composition from Brooklyn waterfront.', '{"Latitude":40.7024,"Longitude":-73.9875}', '/images/keypoints/skyline-point.jpg', NULL, 1, 11);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (45, 'SUMMIT One Vanderbilt', 'End with breathtaking sunset from NYC''s newest observation deck.', '{"Latitude":40.7529,"Longitude":-73.9785}', '/images/keypoints/sunset-platform.jpg', NULL, 2, 11);

-- Tour 12: Venice Canal Mystery Tour (James Bond) - 4 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (46, 'Rialto Bridge', 'Start at Venice''s most famous bridge over Grand Canal.', '{"Latitude":45.4380,"Longitude":12.3358}', '/images/keypoints/rialto.jpg', NULL, 0, 12);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (47, 'Secret Passages', 'Navigate hidden canals known only to locals.', '{"Latitude":45.4371,"Longitude":12.3326}', '/images/keypoints/secret-canal.jpg', 'Casanova''s escape route', 1, 12);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (48, 'Bridge of Sighs', 'Learn the history of this iconic enclosed bridge.', '{"Latitude":45.4341,"Longitude":12.3407}', '/images/keypoints/bridge-sighs.jpg', NULL, 2, 12);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (49, 'Gondola Workshop', 'Visit traditional gondola construction site.', '{"Latitude":45.4235,"Longitude":12.3419}', '/images/keypoints/gondola-workshop.jpg', 'Master craftsman technique', 3, 12);

-- Continue with remaining tours (13-27) with 3-4 key points each...
-- Tour 13: Space Center Houston Tour (Ellen Ripley) - 3 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (50, 'Mission Control', 'Visit the historic Apollo mission control center.', '{"Latitude":29.5631,"Longitude":-95.0903}', '/images/keypoints/mission-control.jpg', NULL, 0, 13);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (51, 'Astronaut Training', 'See where astronauts prepare for space missions.', '{"Latitude":29.5607,"Longitude":-95.0899}', '/images/keypoints/training.jpg', NULL, 1, 13);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (52, 'Rocket Park', 'Stand beneath massive Saturn V rocket.', '{"Latitude":29.5519,"Longitude":-95.0978}', '/images/keypoints/rocket-park.jpg', NULL, 2, 13);

-- Tour 14: Queens Neighborhood Heritage (Peter Parker) - 3 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (53, 'Flushing Chinatown', 'Explore vibrant Asian cuisine and culture.', '{"Latitude":40.7596,"Longitude":-73.8303}', '/images/keypoints/flushing.jpg', NULL, 0, 14);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (54, 'Astoria Greek District', 'Taste authentic Greek food and traditions.', '{"Latitude":40.7720,"Longitude":-73.9300}', '/images/keypoints/astoria.jpg', NULL, 1, 14);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (55, 'Queens Museum', 'See the famous NYC panorama and local art.', '{"Latitude":40.7453,"Longitude":-73.8456}', '/images/keypoints/queens-museum.jpg', NULL, 2, 14);

-- Tour 15: Petra Lost City Discovery (Indiana Jones) - 4 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (56, 'The Siq', 'Enter through the dramatic narrow canyon entrance.', '{"Latitude":30.3216,"Longitude":35.4519}', '/images/keypoints/siq.jpg', NULL, 0, 15);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (57, 'The Treasury', 'Behold the iconic facade carved into rock.', '{"Latitude":30.3222,"Longitude":35.4517}', '/images/keypoints/treasury.jpg', 'Hidden chamber inside', 1, 15);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (58, 'Monastery', 'Climb to the massive hilltop monument.', '{"Latitude":30.3368,"Longitude":35.4446}', '/images/keypoints/monastery.jpg', NULL, 2, 15);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (59, 'Royal Tombs', 'Explore elaborately carved burial chambers.', '{"Latitude":30.3238,"Longitude":35.4510}', '/images/keypoints/royal-tombs.jpg', NULL, 3, 15);

-- Tour 16: Boston Crime History Tour (Bruce Wayne) - 4 key points
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (60, 'Isabella Stewart Gardner Museum', 'Site of the infamous 1990 art heist, still unsolved today.', '{"Latitude":42.3381,"Longitude":-71.0995}', '/images/keypoints/crime1.jpg', 'Empty frames still on display', 0, 16);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (61, 'Boston Police Museum', 'Explore the history of law enforcement and famous cases.', '{"Latitude":42.3551,"Longitude":-71.0567}', '/images/keypoints/agency.jpg', NULL, 1, 16);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (62, 'Charlestown Navy Yard', 'Learn about the infamous Charlestown armored car robberies.', '{"Latitude":42.3737,"Longitude":-71.0561}', '/images/keypoints/speakeasy.jpg', NULL, 2, 16);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (63, 'Old State House', 'Explore colonial crime and punishment at Boston''s oldest building.', '{"Latitude":42.3583,"Longitude":-71.0573}', '/images/keypoints/solution.jpg', NULL, 3, 16);

-- Add more key points for remaining tours (17-27) - keeping it concise with 3 points each
-- Tour 17: Budapest Thermal Spa (Natasha Romanoff)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (64, 'Széchenyi Baths', 'Visit Europe''s largest thermal bath complex.', '{"Latitude":47.5196,"Longitude":19.0815}', '/images/keypoints/szechenyi.jpg', NULL, 0, 17);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (65, 'Gellért Baths', 'Experience Art Nouveau architecture and healing waters.', '{"Latitude":47.4838,"Longitude":19.0522}', '/images/keypoints/gellert.jpg', NULL, 1, 17);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (66, 'Rudas Baths', 'Relax in 16th century Turkish bath with Danube views.', '{"Latitude":47.4912,"Longitude":19.0486}', '/images/keypoints/rudas.jpg', NULL, 2, 17);

-- Tour 18: Swiss Alps Skiing (James Bond)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (67, 'Zermatt Base', 'Start at the car-free alpine resort town.', '{"Latitude":46.0207,"Longitude":7.7491}', '/images/keypoints/zermatt.jpg', NULL, 0, 18);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (68, 'Matterhorn Glacier', 'Ski the iconic Matterhorn slopes.', '{"Latitude":45.9763,"Longitude":7.6586}', '/images/keypoints/matterhorn.jpg', 'Secret off-piste route', 1, 18);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (69, 'Alpine Lodge', 'Warm up at exclusive mountain lodge.', '{"Latitude":46.0093,"Longitude":7.7404}', '/images/keypoints/lodge.jpg', NULL, 2, 18);

-- Tour 19: Brooklyn Historical Walking (Steve Rogers)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (70, 'Brooklyn Heights', 'Start in historic neighborhood with brownstones.', '{"Latitude":40.6958,"Longitude":-73.9936}', '/images/keypoints/brooklyn-heights.jpg', NULL, 0, 19);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (71, 'DUMBO District', 'Explore Down Under Manhattan Bridge Overpass.', '{"Latitude":40.7033,"Longitude":-73.9888}', '/images/keypoints/dumbo.jpg', NULL, 1, 19);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (72, 'Prospect Park', 'End at Frederick Olmsted''s masterpiece park.', '{"Latitude":40.6602,"Longitude":-73.9690}', '/images/keypoints/prospect-park.jpg', NULL, 2, 19);

-- Tour 20: Mediterranean Island Hopping (Diana Prince)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (73, 'Santorini', 'Visit iconic white and blue clifftop villages.', '{"Latitude":36.3932,"Longitude":25.4615}', '/images/keypoints/santorini.jpg', NULL, 0, 20);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (74, 'Mykonos', 'Explore windmills and charming harbor town.', '{"Latitude":37.4467,"Longitude":25.3289}', '/images/keypoints/mykonos.jpg', NULL, 1, 20);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (75, 'Crete Palace', 'Discover Minoan civilization at Knossos.', '{"Latitude":35.2989,"Longitude":25.1631}', '/images/keypoints/knossos.jpg', 'Labyrinth entrance', 2, 20);

-- Tour 21: Kennedy Space Center (Ellen Ripley)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (76, 'Launch Complex', 'View active rocket launch facilities.', '{"Latitude":28.5729,"Longitude":-80.6490}', '/images/keypoints/launch-pad.jpg', NULL, 0, 21);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (77, 'Space Shuttle Atlantis', 'Stand beneath the retired space shuttle.', '{"Latitude":28.5240,"Longitude":-80.6827}', '/images/keypoints/atlantis.jpg', NULL, 1, 21);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (78, 'Astronaut Hall of Fame', 'Meet real astronauts and see artifacts.', '{"Latitude":28.5465,"Longitude":-80.6512}', '/images/keypoints/hall-fame.jpg', NULL, 2, 21);

-- Tour 22: San Francisco Social Impact Tour (Bruce Wayne)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (79, 'Chan Zuckerberg Initiative', 'Explore tech philanthropy and modern social impact approaches.', '{"Latitude":37.4852,"Longitude":-122.1483}', '/images/keypoints/community.jpg', NULL, 0, 22);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (80, 'SF-Marin Food Bank', 'Learn about innovative hunger relief and food distribution programs.', '{"Latitude":37.7699,"Longitude":-122.3899}', '/images/keypoints/food-bank.jpg', NULL, 1, 22);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (81, 'KIPP Bay Area Schools', 'See educational programs closing the achievement gap in action.', '{"Latitude":37.7749,"Longitude":-122.4194}', '/images/keypoints/education.jpg', NULL, 2, 22);

-- Tour 23: Machu Picchu Trail (Indiana Jones)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (82, 'Trail Head', 'Begin the Inca Trail trek at Kilometer 82.', '{"Latitude":-13.3050,"Longitude":-72.4283}', '/images/keypoints/trail-start.jpg', NULL, 0, 23);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (83, 'Sun Gate', 'First glimpse of Machu Picchu at dawn.', '{"Latitude":-13.1635,"Longitude":-72.5450}', '/images/keypoints/sun-gate.jpg', NULL, 1, 23);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (84, 'Machu Picchu Citadel', 'Explore the lost city of the Incas.', '{"Latitude":-13.1631,"Longitude":-72.5450}', '/images/keypoints/machu-picchu.jpg', 'Hidden temple location', 2, 23);

-- Tour 24: Istanbul Grand Bazaar (James Bond)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (85, 'Grand Bazaar', 'Navigate the labyrinth of 4000+ shops.', '{"Latitude":41.0108,"Longitude":28.9680}', '/images/keypoints/grand-bazaar.jpg', 'Secret merchant passage', 0, 24);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (86, 'Blue Mosque', 'Marvel at the stunning Islamic architecture.', '{"Latitude":41.0054,"Longitude":28.9768}', '/images/keypoints/blue-mosque.jpg', NULL, 1, 24);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (87, 'Spice Market', 'Experience exotic spices and Turkish delights.', '{"Latitude":41.0166,"Longitude":28.9707}', '/images/keypoints/spice-market.jpg', NULL, 2, 24);

-- Tour 25: Angkor Wat Temple Explorer (Lara Croft)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (88, 'Angkor Wat Main', 'Enter the world''s largest religious monument.', '{"Latitude":13.4125,"Longitude":103.8670}', '/images/keypoints/angkor-main.jpg', NULL, 0, 25);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (89, 'Ta Prohm Temple', 'See trees growing through ancient ruins.', '{"Latitude":13.4350,"Longitude":103.8892}', '/images/keypoints/ta-prohm.jpg', 'Hidden passageway', 1, 25);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (90, 'Bayon Temple', 'View the enigmatic stone faces.', '{"Latitude":13.4411,"Longitude":103.8589}', '/images/keypoints/bayon.jpg', NULL, 2, 25);

-- Tour 26: Australian Outback Survival (Max Rockatansky)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (91, 'Uluru Base', 'Begin at the sacred Ayers Rock.', '{"Latitude":-25.3444,"Longitude":131.0369}', '/images/keypoints/uluru.jpg', NULL, 0, 26);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (92, 'Survival Camp', 'Learn bushcraft and water finding techniques.', '{"Latitude":-25.2744,"Longitude":130.9756}', '/images/keypoints/survival-camp.jpg', 'Aboriginal sacred site', 1, 26);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (93, 'Kings Canyon', 'Trek through dramatic desert landscapes.', '{"Latitude":-24.2693,"Longitude":131.5089}', '/images/keypoints/kings-canyon.jpg', NULL, 2, 26);

-- Tour 27: Icelandic Glacier Trek (Lara Croft)
INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (94, 'Vatnajökull Glacier', 'Begin on Europe''s largest glacier.', '{"Latitude":64.4167,"Longitude":-16.8167}', '/images/keypoints/vatnajokull.jpg', NULL, 0, 27);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (95, 'Ice Cave Exploration', 'Venture into stunning blue ice caves.', '{"Latitude":64.0420,"Longitude":-16.1788}', '/images/keypoints/ice-cave.jpg', 'Crystal chamber', 1, 27);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (96, 'Jökulsárlón Lagoon', 'See icebergs floating in glacial lagoon.', '{"Latitude":64.0784,"Longitude":-16.2306}', '/images/keypoints/lagoon.jpg', NULL, 2, 27);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (97, 'Skaftafell Nature Reserve', 'Explore stunning waterfalls and hiking trails in this protected area.', '{"Latitude":64.0157,"Longitude":-16.9758}', '/images/keypoints/skaftafell.jpg', 'Hidden waterfall trail', 3, 27);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (98, 'Diamond Beach', 'Witness ice chunks sparkling like diamonds on the black sand beach.', '{"Latitude":64.0488,"Longitude":-16.1796}', '/images/keypoints/diamond.jpg', NULL, 4, 27);

INSERT INTO tours."KeyPoints" ("Id", "Name", "Description", "Location", "ImagePath", "Secret", "Position", "TourId")
VALUES (99, 'Svartifoss Waterfall', 'Admire the unique basalt column formations surrounding this waterfall.', '{"Latitude":64.0278,"Longitude":-16.9753}', '/images/keypoints/waterfall.jpg', 'Ancient volcanic patterns', 5, 27);

-- Equipment
INSERT INTO tours."Equipment" ("Id", "Name", "Description")
VALUES (1, 'Water Bottle', 'Essential hydration for any tour. Recommended 1 liter per person per 2 hours of activity.');

INSERT INTO tours."Equipment" ("Id", "Name", "Description")
VALUES (2, 'Hiking Boots', 'Sturdy footwear with ankle support for uneven terrain and long-distance walking.');

INSERT INTO tours."Equipment" ("Id", "Name", "Description")
VALUES (3, 'Camera', 'Capture memories and stunning landscapes. DSLR or high-quality smartphone recommended.');

INSERT INTO tours."Equipment" ("Id", "Name", "Description")
VALUES (4, 'Sunscreen', 'SPF 30+ sun protection, especially important for outdoor and water activities.');

INSERT INTO tours."Equipment" ("Id", "Name", "Description")
VALUES (5, 'Rain Jacket', 'Waterproof and breathable jacket for unpredictable weather conditions.');

-- Facilities near Iceland tour key points
INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (1, 'Skaftafell Visitor Center WC', 64.0180, -16.9720, 0);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (2, 'Skaftafell Cafe & Restaurant', 64.0175, -16.9750, 1);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (3, 'Skaftafell Parking Area', 64.0165, -16.9755, 2);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (4, 'Jökulsárlón Cafe', 64.0480, -16.1800, 1);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (5, 'Jökulsárlón Parking', 64.0485, -16.1790, 2);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (6, 'Jökulsárlón Public Restroom', 64.0482, -16.1795, 0);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (7, 'Diamond Beach Parking', 64.0490, -16.1785, 2);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (8, 'Vatnajökull Information Center', 64.4170, -16.8160, 3);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (9, 'Glacier Guide Station', 64.0425, -16.1780, 3);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (10, 'Svartifoss Trail Parking', 64.0275, -16.9745, 2);

-- Facilities near Chicago tour key points
INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (11, 'Willis Tower Lower Level Restrooms', 41.8785, -87.6355, 0);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (12, 'Skydeck Cafe & Bar', 41.8790, -87.6362, 1);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (13, 'Willis Tower Parking Garage', 41.8782, -87.6365, 2);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (14, 'Riverwalk Cafe', 41.8878, -87.6240, 1);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (15, 'Chicago Riverwalk Public Restrooms', 41.8880, -87.6235, 0);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (16, 'Wacker Drive Parking', 41.8875, -87.6245, 2);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (17, 'Tribune Tower Plaza Cafe', 41.8905, -87.6237, 1);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (18, 'Michigan Avenue Parking', 41.8900, -87.6230, 2);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (19, '360 Chicago Gift Shop & Cafe', 41.8992, -87.6233, 1);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (20, 'John Hancock Center Parking', 41.8987, -87.6227, 2);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (21, 'Navy Pier Public Facilities', 41.8917, -87.6050, 0);

INSERT INTO tours."Facilities" ("Id", "Name", "Latitude", "Longitude", "Type")
VALUES (22, 'Millennium Park Restaurant', 41.8826, -87.6226, 1);

-- Monuments near Iceland tour
INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES (1, 'Vatnajökull National Park Monument', 'Europe largest national park, established to protect unique glacier and volcanic landscapes.', 2008, 0, 64.4167, -16.8167);

INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES (2, 'Skaftafell Nature Reserve Memorial', 'Historic nature reserve merged into Vatnajökull National Park, celebrating Iceland natural heritage.', 1967, 0, 64.0157, -16.9758);

INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES (3, 'Jökulsárlón Glacier Lagoon Marker', 'Commemorating the formation of Iceland most famous glacier lagoon from Breiðamerkurjökull retreat.', 1934, 0, 64.0784, -16.2306);

INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES (4, 'Svartifoss Historical Site', 'Famous waterfall surrounded by dark basalt columns, inspiring Icelandic architectural designs.', 1850, 0, 64.0278, -16.9753);

INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES (5, 'Öræfajökull Volcanic Memorial', 'Memorial to the historic volcanic eruptions that shaped the Vatnajökull region.', 1362, 0, 64.0050, -16.6500);

INSERT INTO tours."Monument" ("Id", "Name", "Description", "Year", "Status", "Location_Latitude", "Location_Longitude")
VALUES (6, 'Breiðamerkurjökull Glacier Tongue', 'Historic glacier outlet that created the iconic Jökulsárlón lagoon through centuries of retreat.', 1900, 0, 64.0600, -16.2000);

-- TouristEquipment (Walter and Jesse)
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (1, 4, 1);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (2, 4, 2);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (3, 4, 3);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (4, 5, 1);
INSERT INTO tours."TouristEquipment" ("Id", "TouristId", "EquipmentId") VALUES (5, 5, 4);

-- TourSponsorships
INSERT INTO tours."TourSponsorships" ("Id", "TourId", "AuthorId", "StartDate", "EndDate", "DurationDays", "Price")
VALUES (1, 27, 16, '2026-03-01 00:00:00', '2026-03-31 00:00:00', 30, 500.00),
(2, 26, 18, '2026-03-01 00:00:00', '2026-03-31 00:00:00', 30, 500.00);

-- TourReviews (from tourists)
INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Amazing tour! The guide was excellent and the views were breathtaking.', NOW(), 100, 1, 3, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Absolutely magical experience! Would recommend to everyone.', NOW(), 100, 2, 4, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Great adventure! Some parts were challenging but worth it.', NOW(), 100, 3, 10, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The science and history combination was perfect. Very educational!', NOW(), 100, 4, 13, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Yeah! This tour was awesome! Great experience.', NOW(), 85, 5, 2, 'jessepinkman');

-- Iceland Glacier Trek (Tour 27) - Many reviews
INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Absolutely stunning! The ice caves were like nothing I have ever seen before. A once in a lifetime experience!', NOW(), 100, 1, 27, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The glacier trek was incredible! Our guide was knowledgeable and made us feel safe the entire time.', NOW(), 100, 2, 27, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Iceland is magical! The diamond beach and ice lagoon were breathtaking. Highly recommend this tour!', NOW(), 100, 3, 27, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Amazing scenery and well-organized tour. Weather was challenging but the experience was worth it!', NOW(), 100, 4, 27, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Yo! This glacier tour was epic! The ice caves blew my mind, Mr. White would love this!', NOW(), 100, 5, 27, 'jessepinkman');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The most beautiful place on Earth! Svartifoss waterfall with basalt columns was stunning.', NOW(), 100, 1, 27, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Perfect winter adventure! The crystal ice formations inside the glacier were otherworldly.', NOW(), 90, 2, 27, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Great tour but physically demanding. Make sure you are in good shape before booking!', NOW(), 85, 3, 27, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Iceland exceeded all expectations. The guided glacier walk was safe and educational.', NOW(), 100, 4, 27, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Jökulsárlón lagoon was incredible! Watching icebergs float by was so peaceful and beautiful.', NOW(), 100, 5, 27, 'jessepinkman');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Best tour I have ever taken! The combination of ice caves, waterfalls, and black sand beaches is unbeatable.', NOW(), 100, 1, 27, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Amazing experience! Only giving 4 stars because of the cold weather, but that is Iceland for you!', NOW(), 100, 2, 27, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The blue ice caves were surreal! Like being inside a giant sapphire. Photography heaven!', NOW(), 100, 3, 27, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Skaftafell Nature Reserve was pristine! The hiking trails and waterfalls were spectacular.', NOW(), 95, 4, 27, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Vatnajökull glacier is massive! Walking on Europe largest glacier was an unforgettable experience.', NOW(), 100, 5, 27, 'jessepinkman');

-- Additional reviews for other popular tours
INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Ancient Greece tour was phenomenal! The history came alive with our excellent guide.', NOW(), 100, 1, 4, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'London espionage trail was fascinating! So much spy history hidden in plain sight.', NOW(), 100, 2, 3, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'New York skyline photography tour gave me the best shots of my life! Top of the Rock at sunset was magical.', NOW(), 100, 3, 11, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Egyptian pyramids expedition was mind-blowing! Standing before the pyramids is humbling.', NOW(), 100, 4, 10, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Petra lost city trek was challenging but absolutely worth every step!', NOW(), 100, 5, 15, 'jessepinkman');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Venice canal mystery tour was romantic and intriguing! Hidden passages were amazing.', NOW(), 100, 1, 12, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Machu Picchu trail adventure was the trek of a lifetime! Inca history is fascinating.', NOW(), 100, 2, 23, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Chicago night architecture tour showcased the city beautiful skyline perfectly!', NOW(), 100, 3, 1, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Boston crime history tour was gripping! The Gardner Museum heist story still unsolved!', NOW(), 100, 4, 16, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Amazon rainforest expedition was wild! The biodiversity is incredible.', NOW(), 100, 5, 9, 'jessepinkman');

-- Chicago Night Architecture Tour (Tour 1) - Many reviews
INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Willis Tower at night is spectacular! The glass ledge experience was thrilling and the views were unmatched.', NOW(), 100, 1, 1, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Chicago architecture tour exceeded expectations! The Tribune Tower fragments from world buildings were fascinating.', NOW(), 100, 2, 1, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The Riverwalk at night is magical! All the bridges lit up create an amazing atmosphere.', NOW(), 100, 3, 1, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Best architecture tour I have taken! Chicago Art Deco buildings are stunning at night.', NOW(), 100, 4, 1, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Yo! The 360 Chicago TILT experience was insane! Leaning over the city at night - mind blown!', NOW(), 100, 5, 1, 'jessepinkman');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Chicago skyline at night is world-class! The tour guide knowledge about architecture history was impressive.', NOW(), 100, 1, 1, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Great tour! Only complaint is it was a bit cold in October, but the views made up for it.', NOW(), 95, 2, 1, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The contrast between historic and modern architecture in Chicago is incredible at night.', NOW(), 100, 3, 1, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Perfect evening activity! The illuminated buildings showcase Chicago architectural heritage beautifully.', NOW(), 100, 4, 1, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Chicago Riverwalk is so peaceful at night! Great for photography enthusiasts.', NOW(), 100, 5, 1, 'jessepinkman');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The neo-Gothic Tribune Tower is stunning! Learning about the embedded stones from famous buildings worldwide was fascinating.', NOW(), 100, 1, 1, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'John Hancock Center observation deck offers the best Chicago views! The TILT attraction is a must-try.', NOW(), 100, 2, 1, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (4, 'Wonderful tour of Chicago at night! Would have loved to see more buildings, but still excellent.', NOW(), 90, 3, 1, 'lukeskywalker');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Chicago architecture is world-renowned for good reason! This tour highlights the best of it.', NOW(), 100, 4, 1, 'walterwhite');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The combination of rooftop viewpoints and street-level exploration was perfect!', NOW(), 100, 5, 1, 'jessepinkman');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Chicago is the birthplace of the skyscraper! This tour does justice to the city architectural legacy.', NOW(), 100, 1, 1, 'frodobaggins');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'The glass ledge at Willis Tower was terrifying but amazing! 103 floors up with a clear view down.', NOW(), 100, 2, 1, 'hermionegranger');

INSERT INTO tours."TourReviews" ("Grade", "Comment", "ReviewTime", "Progress", "TouristID", "TourID", "TouristUsername")
VALUES (5, 'Loved learning about the Chicago School of Architecture! The buildings are living history lessons.', NOW(), 100, 3, 1, 'lukeskywalker');

-- TourRequiredEquipment
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (1, 2, 9);
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (2, 1, 9);
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (3, 2, 10);
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (4, 4, 10);
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (5, 2, 15);
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (6, 1, 15);
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (7, 5, 18);
INSERT INTO tours."TourRequiredEquipment" ("Id", "EquipmentId", "TourId") VALUES (8, 2, 26);

-- TourDurations (TransportType: 0=Walking, 1=Bicycle, 2=Car; DurationMinutes must be positive)
-- Each tour must have at least 1 duration to be published
INSERT INTO tours."TourDuration" ("Id", "TourId", "TransportType", "DurationMinutes") VALUES
(1, 1, 0, 180),
(2, 1, 1, 120),
(3, 2, 0, 480),
(4, 2, 1, 320),
(5, 2, 2, 240),
(6, 3, 0, 360),
(7, 3, 1, 240),
(8, 3, 2, 180),
(9, 4, 0, 600),
(10, 4, 1, 400),
(11, 5, 0, 300),
(12, 5, 1, 200),
(13, 5, 2, 150),
(14, 6, 0, 300),
(15, 6, 1, 200),
(16, 7, 0, 360),
(17, 7, 1, 240),
(18, 7, 2, 180),
(19, 8, 0, 360),
(20, 8, 1, 240),
(21, 9, 0, 720),
(22, 9, 1, 480),
(23, 9, 2, 360),
(24, 10, 0, 720),
(25, 10, 1, 480),
(26, 11, 0, 240),
(27, 11, 1, 160),
(28, 11, 2, 120),
(29, 12, 0, 360),
(30, 12, 1, 240),
(31, 12, 2, 180),
(32, 13, 0, 360),
(33, 13, 1, 240),
(34, 14, 0, 360),
(35, 14, 1, 240),
(36, 15, 0, 720),
(37, 15, 1, 480),
(38, 15, 2, 360),
(39, 16, 0, 240),
(40, 16, 1, 160),
(41, 16, 2, 120),
(42, 17, 0, 360),
(43, 17, 1, 240),
(44, 18, 0, 480),
(45, 18, 1, 320),
(46, 18, 2, 240),
(47, 19, 0, 300),
(48, 19, 1, 200),
(49, 20, 0, 480),
(50, 20, 1, 320),
(51, 21, 0, 420),
(52, 21, 1, 280),
(53, 22, 0, 240),
(54, 22, 1, 160),
(55, 22, 2, 120),
(56, 23, 0, 720),
(57, 23, 1, 480),
(58, 23, 2, 360),	
(59, 24, 0, 360),
(60, 24, 1, 240),
(61, 24, 2, 180),
(62, 25, 0, 600),
(63, 25, 1, 400),
(64, 26, 0, 720),
(65, 26, 1, 480),
(66, 26, 2, 360),
(67, 27, 0, 360),
(68, 27, 1, 240);

SELECT setval(pg_get_serial_sequence('tours."TouristPreferences"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TouristPreferences"));
SELECT setval(pg_get_serial_sequence('tours."Tours"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Tours"));
SELECT setval(pg_get_serial_sequence('tours."Equipment"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Equipment"));
SELECT setval(pg_get_serial_sequence('tours."TouristEquipment"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TouristEquipment"));
SELECT setval(pg_get_serial_sequence('tours."Facilities"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Facilities"));
SELECT setval(pg_get_serial_sequence('tours."Monument"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."Monument"));
SELECT setval(pg_get_serial_sequence('tours."TourReviews"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TourReviews"));
SELECT setval(pg_get_serial_sequence('tours."KeyPoints"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."KeyPoints"));
SELECT setval(pg_get_serial_sequence('tours."TourRequiredEquipment"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TourRequiredEquipment"));
SELECT setval(pg_get_serial_sequence('tours."TourDuration"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TourDuration"));
SELECT setval(pg_get_serial_sequence('tours."TourSponsorships"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM tours."TourSponsorships"));
