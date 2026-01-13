INSERT INTO payments."Bundles"
    ("Id", "Name", "Price", "AuthorId", "Status")
VALUES
    (-1, 'Summer Adventure Package', 100.0, -11, 0),
    (-2, 'Winter Sports Bundle', 150.0, -11, 1),
    (-3, 'Spring Bundle', 80.0, -11, 0),
    (-4, 'Autumn Bundle', 90.0, -11, 0);

INSERT INTO payments."BundleItems"
    ("Id", "BundleId", "TourId")
VALUES
    (-1, -1, -1),
    (-2, -1, -2),
    (-3, -2, -1),
    (-4, -2, -3),
    (-5, -3, -1),
    (-6, -3, -2),
    (-7, -4, -1),
    (-8, -4, -3);