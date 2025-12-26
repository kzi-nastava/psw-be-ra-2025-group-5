DELETE FROM payments."ShoppingCarts";
DELETE FROM payments."TourPurchaseTokens";

INSERT INTO payments."ShoppingCarts" ("Id", "TouristId", "Items") VALUES (1, 4, '[{"TourId": 2, "TourName": "Niš WWII History Trail", "ItemPrice": 5.05}]'), (2, 5, '[]');
INSERT INTO payments."TourPurchaseTokens" ("Id", "TourId", "TouristId") VALUES (1, 4, 5);

SELECT setval(pg_get_serial_sequence('payments."ShoppingCarts"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."ShoppingCarts"));
SELECT setval(pg_get_serial_sequence('payments."TourPurchaseTokens"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM payments."TourPurchaseTokens"));
