DELETE FROM stakeholders."TourProblems";
DELETE FROM stakeholders."People";
DELETE FROM stakeholders."Users";

INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-1, 'admin@gmail.com', 'admin', 'admin@gmail.com', 0, true);

INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-11, 'autor1@gmail.com', 'autor1', 'autor1@gmail.com', 1, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-12, 'autor2@gmail.com', 'autor2', 'autor2@gmail.com',1, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-13, 'autor3@gmail.com', 'autor3', 'autor3@gmail.com', 1, true);

INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-21, 'turista1@gmail.com', 'turista1', 'turista1@gmail.com', 2, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-22, 'turista2@gmail.com', 'turista2', 'turista2@gmail.com', 2, true);
INSERT INTO stakeholders."Users"(
    "Id", "Username", "Password", "Email", "Role", "IsActive")
VALUES (-23, 'turista3@gmail.com', 'turista3', 'turista3@gmail.com', 2, true);

INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-11, -11, 'Ana', 'Anić', 'autor1@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-12, -12, 'Lena', 'Lenić', 'autor2@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-13, -13, 'Sara', 'Sarić', 'autor3@gmail.com');

INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-21, -21, 'Pera', 'Perić', 'turista1@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-22, -22, 'Mika', 'Mikić', 'turista2@gmail.com');
INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email")
VALUES (-23, -23, 'Steva', 'Stević', 'turista3@gmail.com');

INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt")
VALUES (-1, 1, -21, 0, 2, 'Problem sa bezbednošću na turi', '2023-10-25T10:00:00Z', '2023-10-25T10:05:00Z');
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt")
VALUES (-2, 2, -22, 2, 1, 'Problem sa planom puta', '2023-10-26T11:00:00Z', '2023-10-26T11:05:00Z');
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt",  "CreatedAt")
VALUES (-3, 3, -23, 1, 0, 'Problem sa vodičem', '2023-10-27T12:00:00Z', '2023-10-27T12:05:00Z');