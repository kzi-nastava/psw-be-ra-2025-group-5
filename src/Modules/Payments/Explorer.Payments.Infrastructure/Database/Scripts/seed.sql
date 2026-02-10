DELETE FROM payments."ShoppingCarts";
DELETE FROM payments."TourPurchaseTokens";
DELETE FROM payments."Wallets";
DELETE FROM payments."Coupons";
DELETE FROM payments."TourSales";
DELETE FROM payments."Bundles";
DELETE FROM payments."BundleItems";

-- BundleStatus enum values:
-- 0 = Draft, 1 = Published, 2 = Archived

-- Wallets for all users (Authors: 1-3, 6-13, 16, 18; Tourists: 1-5)
INSERT INTO payments."Wallets" ("Id", "UserId", "Balance") VALUES
(1, 1, 1500.00),  -- Tourist: frodobaggins
(2, 2, 2800.00),  -- Tourist: hermionegranger
(3, 3, 1200.00),  -- Tourist: lukeskywalker
(4, 4, 5000.00),  -- Tourist: walterwhite
(5, 5, 4500.00),  -- Tourist: jessepinkman
(6, 6, 3500.00),  -- Author: James Bond
(7, 7, 8000.00),  -- Author: Bruce Wayne
(8, 8, 2500.00),  -- Author: Peter Parker
(9, 9, 4200.00),  -- Author: Diana Prince
(10, 10, 3800.00), -- Author: Natasha Romanoff
(11, 11, 3200.00), -- Author: Steve Rogers
(12, 12, 6500.00), -- Author: Indiana Jones
(13, 13, 2900.00), -- Author: Ellen Ripley
(14, 16, 5500.00), -- Author: Lara Croft
(15, 18, 4100.00); -- Author: Max Rockatansky

-- Shopping Carts with items from existing tours
-- Tourist 1 (frodobaggins) - has London and Greece tours in cart
INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items")
VALUES (1, 1, '[{"TourId": 3, "TourName": "London Espionage Trail", "ItemPrice": 80.00}, {"TourId": 4, "TourName": "Ancient Greece Mythology Tour", "ItemPrice": 90.00}]');

-- Tourist 2 (hermionegranger) - has Iceland tour as gift
INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items")
VALUES (2, 2, '[{"TourId": 27, "TourName": "Icelandic Glacier Trek", "ItemPrice": 195.00, "RecipientId": 1, "GiftMessage": "Happy birthday Frodo! Enjoy Iceland!"}]');

-- Tourist 3 (lukeskywalker) - has Chicago and New York tours
INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items")
VALUES (3, 3, '[{"TourId": 1, "TourName": "Chicago Night Architecture Tour", "ItemPrice": 75.00}, {"TourId": 2, "TourName": "New York Tour", "ItemPrice": 70.00}]');

-- Tourist 4 (walterwhite) - has Space Center tour
INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items")
VALUES (4, 4, '[{"TourId": 13, "TourName": "Space Center Houston Tour", "ItemPrice": 65.00}]');

-- Tourist 5 (jessepinkman) - empty cart
INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items")
VALUES (5, 5, '[]');

-- Tour Purchase Tokens (tourists who have purchased tours)
-- Tourist 1 purchased Ancient Greece tour
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (1, 4, 1, false, '2024-06-15 10:30:00');

-- Tourist 2 purchased London and Venice tours
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (2, 3, 2, false, '2024-05-20 14:15:00');

INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (3, 12, 2, false, '2024-06-01 09:45:00');

-- Tourist 3 purchased Egyptian Pyramids and Petra tours
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (4, 10, 3, false, '2024-04-10 11:20:00');

INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (5, 15, 3, false, '2024-06-05 16:30:00');

-- Tourist 4 purchased Space Centers and Chicago tour
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (6, 13, 4, false, '2024-07-10 08:00:00');

INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (7, 21, 4, false, '2024-08-01 10:15:00');

-- Tourist 5 purchased New York and Amazon tours, got one free
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (9, 2, 5, false, '2024-08-25 12:00:00');

INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (10, 9, 5, true, '2024-08-25 12:05:00');

-- Tourist 1 also purchased Iceland tour
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (11, 27, 1, false, '2024-03-05 14:30:00');

-- Tourist 3 purchased Machu Picchu
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt")
VALUES (12, 23, 3, false, '2024-03-20 07:15:00');

-- Coupons (Code must be exactly 8 characters, Percentage 1-100)
-- Bruce Wayne coupons for his tours
INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (1, 'CHICAGO1', 15, 7, 1, '2026-12-31 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (2, 'NEWPORT5', 20, 7, 5, '2026-10-31 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (3, 'NYPHOTO2', 10, 7, 11, '2027-03-31 23:59:59');

-- James Bond coupons
INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (4, 'LONDONSP', 25, 6, 3, '2026-08-15 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (5, 'VENICE50', 30, 6, 12, '2026-09-30 23:59:59');

-- Diana Prince coupon for Greece tour
INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (6, 'GREECE20', 20, 9, 4, '2027-06-30 23:59:59');

-- Indiana Jones coupons
INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (7, 'EGYPT100', 15, 12, 10, '2026-12-31 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (8, 'PETRA777', 10, 12, 15, '2027-01-31 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (9, 'MACHUP99', 25, 12, 23, '2026-11-30 23:59:59');

-- Lara Croft coupons
INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (10, 'ICELAND3', 20, 16, 27, '2026-12-31 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (11, 'ANGKOR88', 15, 16, 25, '2026-10-31 23:59:59');

-- Universal coupons (null TourId - applies to any tour from author)
INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (12, 'WELCOME5', 5, 7, NULL, '2027-12-31 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (13, 'SUMMER25', 10, 9, NULL, '2026-08-31 23:59:59');

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (14, 'WINTER15', 15, 16, NULL, '2027-02-28 23:59:59');

-- Expired coupon example
INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES (15, 'EXPIRED1', 50, 6, 3, '2025-12-31 23:59:59');

-- Tour Sales (discounts on specific tours, ExpirationDate must be within 14 days from CreationDate)
-- Bruce Wayne sale on Chicago tour
INSERT INTO payments."TourSales"("Id", "AuthorId", "TourIds", "CreationDate", "ExpirationDate", "DiscountPercentage")
VALUES (1, 7, '{1}', '2026-02-01', '2026-02-14', 20);

-- James Bond sale on London and Venice tours
INSERT INTO payments."TourSales"("Id", "AuthorId", "TourIds", "CreationDate", "ExpirationDate", "DiscountPercentage")
VALUES (2, 6, '{3, 12}', '2026-02-05', '2026-02-15', 25);

-- Indiana Jones sale on all his tours
INSERT INTO payments."TourSales"("Id", "AuthorId", "TourIds", "CreationDate", "ExpirationDate", "DiscountPercentage")
VALUES (3, 12, '{10, 15, 23}', '2026-02-03', '2026-02-10', 30);

-- Lara Croft winter sale
INSERT INTO payments."TourSales"("Id", "AuthorId", "TourIds", "CreationDate", "ExpirationDate", "DiscountPercentage")
VALUES (4, 16, '{27}', '2026-02-01', '2026-02-08', 15);

-- Diana Prince Greek mythology sale
INSERT INTO payments."TourSales"("Id", "AuthorId", "TourIds", "CreationDate", "ExpirationDate", "DiscountPercentage")
VALUES (5, 9, '{4, 7}', '2026-02-04', '2026-02-11', 10);

-- Bundles (tour packages)
-- Bundle 1: Bruce Wayne's US Architecture Bundle (Published)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (1, 'American Architecture Grand Tour', 299.00, 7, 1);

-- Bundle 2: James Bond's European Espionage Bundle (Published)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (2, 'European Spy Trail Package', 220.00, 6, 1);

-- Bundle 3: Indiana Jones' Ancient Wonders Bundle (Published)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (3, 'Ancient Civilizations Explorer', 520.00, 12, 1);

-- Bundle 4: Diana Prince's Greek Heritage Bundle (Published)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (4, 'Greek Mythology Complete Experience', 150.00, 9, 1);

-- Bundle 5: Lara Croft's Adventure Bundle (Published)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (5, 'Ultimate Adventure Package', 320.00, 16, 1);

-- Bundle 6: Peter Parker's New York Bundle (Draft)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (6, 'New York City Explorer', 100.00, 8, 0);

-- Bundle 7: Ellen Ripley's Space Bundle (Published)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (7, 'Space Exploration Package', 125.00, 13, 1);

-- Bundle 8: Steve Rogers' Historical Bundle (Archived)
INSERT INTO payments."Bundles" ("Id", "Name", "Price", "AuthorId", "Status")
VALUES (8, 'American History Trail', 80.00, 11, 2);

-- Bundle Items (tours in each bundle)
-- Bundle 1 items: Bruce Wayne's tours (Chicago, Newport, NY Photography)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (1, 1, 1);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (2, 1, 5);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (3, 1, 11);
	
-- Bundle 2 items: James Bond's tours (London, Venice, Swiss Alps)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (4, 2, 3);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (5, 2, 12);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (6, 2, 18);

-- Bundle 3 items: Indiana Jones' tours (Egypt, Petra, Machu Picchu)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (7, 3, 10);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (8, 3, 15);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (9, 3, 23);

-- Bundle 4 items: Diana Prince's tours (Greece, Amazon)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (10, 4, 4);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (11, 4, 7);

-- Bundle 5 items: Lara Croft's tours (Angkor Wat, Iceland)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (12, 5, 25);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (13, 5, 27);

-- Bundle 6 items: Peter Parker's tours (New York, Queens)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (14, 6, 2);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (15, 6, 14);

-- Bundle 7 items: Ellen Ripley's tours (Space Centers)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (16, 7, 13);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (17, 7, 21);

-- Bundle 8 items: Steve Rogers' tours (WWII Memorial, Brooklyn)
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (18, 8, 8);
INSERT INTO payments."BundleItems" ("Id", "BundleId", "TourId") VALUES (19, 8, 19);

-- Update sequences
SELECT setval(pg_get_serial_sequence('payments."ShoppingCarts"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."ShoppingCarts"));
SELECT setval(pg_get_serial_sequence('payments."TourPurchaseTokens"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."TourPurchaseTokens"));
SELECT setval(pg_get_serial_sequence('payments."Wallets"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."Wallets"));
SELECT setval(pg_get_serial_sequence('payments."TourSales"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."TourSales"));
SELECT setval(pg_get_serial_sequence('payments."Coupons"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."Coupons"));
SELECT setval(pg_get_serial_sequence('payments."Bundles"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."Bundles"));
SELECT setval(pg_get_serial_sequence('payments."BundleItems"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."BundleItems"));
