DELETE FROM payments."ShoppingCarts";
DELETE FROM payments."TourPurchaseTokens";
DELETE FROM payments."Wallets";
DELETE FROM payments."Coupons";

INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items") VALUES (1, 4, '[{"TourId": 2, "TourName": "Niš WWII History Trail", "ItemPrice": 5.05}]'), (2, 5, '[]');
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId") VALUES (1, 4, 5);
INSERT INTO payments."Wallets" ("Id", "TouristId", "Balance") VALUES 
(1, 4, 0), 
(2, 5, 0),
(3, 6, 20);
INSERT INTO payments."TourSales"("Id", "AuthorId", "TourIds", "CreationDate", "ExpirationDate", "DiscountPercentage") VALUES (1, 7, '{2}', '2026-01-04', '2026-01-14', 50);

INSERT INTO payments."Coupons" ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate") 
VALUES
    (1, 'COUPON01', 10, 1, NULL, TIMESTAMP '2027-12-31 23:59:59'),
    (2, 'COUPON02', 15, 2, 3, TIMESTAMP '2027-06-30 12:00:00'),
    (3, 'COUPON03', 20, 1, NULL, TIMESTAMP '2027-09-15 00:00:00');

SELECT setval(pg_get_serial_sequence('payments."ShoppingCarts"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."ShoppingCarts"));
SELECT setval(pg_get_serial_sequence('payments."TourPurchaseTokens"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."TourPurchaseTokens"));
SELECT setval(pg_get_serial_sequence('payments."Wallets"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."Wallets"));
SELECT setval(pg_get_serial_sequence('payments."TourSales"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."TourSales"));
SELECT setval(pg_get_serial_sequence('payments."Coupons"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."Coupons"));
