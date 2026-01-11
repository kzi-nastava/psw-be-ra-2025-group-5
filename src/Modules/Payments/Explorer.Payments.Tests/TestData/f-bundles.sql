-- payments-test-data.sql

-- Bundles test data
INSERT INTO payments."Bundles"
    ("Id", "Name", "Price", "AuthorId", "Status")
VALUES
    (-1, 'Summer Adventure Package', 100.0, -11, 0), -- Draft - za Delete test
    (-2, 'Winter Sports Bundle', 150.0, -11, 1),     -- Published - za Archive test
    (-3, 'Spring Bundle', 80.0, -11, 0),             -- Draft - za Update test
    (-4, 'Autumn Bundle', 90.0, -11, 0);             -- Draft - za Publish test (MORA imati 2+ Published tours!)

-- BundleItems test data
-- Pretpostavljam da su ture -1 i -3 Published (proveri u tours-test-data.sql!)
INSERT INTO payments."BundleItems"
    ("Id", "BundleId", "TourId")
VALUES
    (-1, -1, -1),  -- Summer Package
    (-2, -1, -2),
    (-3, -2, -1),  -- Winter Bundle
    (-4, -2, -3),
    (-5, -3, -1),  -- Spring Bundle
    (-6, -3, -2),
    (-7, -4, -1),  -- Autumn Bundle - OVE DVE TURE MORAJU BITI PUBLISHED!
    (-8, -4, -3);  -- Autumn Bundle - OVE DVE TURE MORAJU BITI PUBLISHED!