INSERT INTO payments."Coupons"
    ("Id", "Code", "Percentage", "AuthorId", "TourId", "ExpirationDate")
VALUES
    (-1, 'TOUR0001', 20, -1, -1, TIMESTAMP '2027-12-31 23:59:59'),
    (-2, 'AUTH0001', 10, -5, NULL, TIMESTAMP '2027-06-30 12:00:00'),
    (-3, 'AUTH0002', 25, -5, NULL, TIMESTAMP '2027-09-15 00:00:00'),
    (-4, 'NOEXP001', 30, -2, NULL, NULL),
    (-5, 'EXP00001', 15, -3, NULL, TIMESTAMP '2027-01-01 00:00:00');
