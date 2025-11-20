INSERT INTO stakeholders."Users" ("Id", "Username", "Password", "Role", "IsActive") VALUES
(-1,  'admin@gmail.com',   'admin',   0, true),
(-11, 'autor1@gmail.com',  'autor1',  1, true),
(-12, 'autor2@gmail.com',  'autor2',  1, true),
(-13, 'autor3@gmail.com',  'autor3',  1, true),
(-21, 'turista1@gmail.com','turista1',2, true),
(-22, 'turista2@gmail.com','turista2',2, true),
(-23, 'turista3@gmail.com','turista3',2, true);


INSERT INTO stakeholders."People" ("Id", "UserId", "Name", "Surname", "Email") VALUES
(-11, -11, 'Ana',  'Anić',   'autor1@gmail.com'),
(-12, -12, 'Lena', 'Lenić',  'autor2@gmail.com'),
(-13, -13, 'Sara', 'Sarić',  'autor3@gmail.com'),
(-21, -21, 'Pera', 'Perić',  'turista1@gmail.com'),
(-22, -22, 'Mika', 'Mikić',  'turista2@gmail.com'),
(-23, -23, 'Steva','Stević', 'turista3@gmail.com');


INSERT INTO stakeholders."AppRatings" ("Id", "UserId", "Rating", "Comment", "CreatedAt") VALUES
(-1, -21, 5, 'Odlicna aplikacija', NOW()),
(-2, -22, 4, 'Solidno', NOW());
