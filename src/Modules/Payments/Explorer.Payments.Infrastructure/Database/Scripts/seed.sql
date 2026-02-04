DELETE FROM payments."ShoppingCarts";
DELETE FROM payments."TourPurchaseTokens";
DELETE FROM payments."Wallets";
DELETE FROM payments."Coupons";
DELETE FROM payments."TourSales";
DELETE FROM payments."Bundles";
DELETE FROM payments."BundleItems";

INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items") VALUES (1, 4, '[{"TourId": 2, "TourName": "Niš WWII History Trail", "ItemPrice": 5.05}]'), (2, 5, '[]');
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId", "IsFree", "PurchasedAt") VALUES (1, 4, 5, False, TIMESTAMP '2027-06-30 12:00:00');
INSERT INTO payments."Wallets" ("Id", "UserId", "Balance") VALUES 
(1, 4, 5000), 
(2, 5, 5000),
(3, 6, 4000),
(4, 1, 1400), 
(5, 2, 3040),
(6, 3, 2040),
(7, 7, 2040);

INSERT INTO payments."TourSales"("Id", "AuthorId", "TourIds", "CreationDate", "ExpirationDate", "DiscountPercentage") VALUES (1, 7, '{2}', '2026-01-04', '2026-01-14', 50);

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate") 
VALUES
    (1, 'COUPON01', 10, 1, NULL, TIMESTAMP '2027-12-31 23:59:59'),
    (2, 'COUPON02', 15, 2, 3, TIMESTAMP '2027-06-30 12:00:00'),
    (3, 'COUPON03', 20, 1, NULL, TIMESTAMP '2027-09-15 00:00:00');

INSERT INTO payments."Bundles" ("Id", "Name", "Price", "Status", "AuthorId")
VALUES (1, 'Serbia Nature Pack', 50.0, 0, 7); 

INSERT INTO payments."BundleItems" ("BundleId", "TourId") VALUES (1, 2);
INSERT INTO payments."BundleItems" ("BundleId", "TourId") VALUES (1, 7);

SELECT setval(pg_get_serial_sequence('payments."ShoppingCarts"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."ShoppingCarts"));
SELECT setval(pg_get_serial_sequence('payments."TourPurchaseTokens"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."TourPurchaseTokens"));
SELECT setval(pg_get_serial_sequence('payments."Wallets"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."Wallets"));
SELECT setval(pg_get_serial_sequence('payments."TourSales"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."TourSales"));
SELECT setval(pg_get_serial_sequence('payments."Coupons"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."Coupons"));
SELECT setval(pg_get_serial_sequence('payments."Bundles"', 'Id'),(SELECT COALESCE(MAX("Id"),0) FROM payments."Bundles")
);
