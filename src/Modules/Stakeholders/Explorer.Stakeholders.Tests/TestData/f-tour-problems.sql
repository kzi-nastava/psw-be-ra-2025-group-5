INSERT INTO stakeholders."TourProblems"(
    "Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt", "CreatedAt", "Comments" , "IsResolved", "Deadline")
	VALUES (-11, -1, -21, 0, 2, 'Problem sa autobusom', '2025-10-25T10:00:00Z', '2025-10-25T10:05:00Z', ARRAY[]::bigint[] , false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt", "CreatedAt", "Comments", "IsResolved", "Deadline")
	VALUES (-22, -2, -22, 0, 2,'Pre izlaska iz autobusa nismo dobili adekvatne informacije gde se grupa sastaje posle pauze, što je dovelo do toga da kasnimo 10 minuta', '2025-10-25T10:00:00Z', '2025-10-25T10:05:00Z', ARRAY[]::bigint[], false, null);
INSERT INTO stakeholders."TourProblems"(
	"Id", "TourId", "ReporterId", "Category", "Priority", "Description", "OccurredAt", "CreatedAt", "Comments", "IsResolved", "Deadline")
	VALUES (-33, -3, -23, 2, 1, 'Stvarno loše organizovan poslednji dan ture, gde smo videli samo predgra?e i autobus, a bilo nam je obecano da idemo do dvorca u centru', '2025-10-25T10:00:00Z', '2025-10-25T10:05:00Z', ARRAY[]::bigint[], false, null);

INSERT INTO stakeholders."Comments"("CommentId","AuthorId","CreatedAt","UpdatedAt","Content")
VALUES
(11,-11,NOW(),NULL,'Problem je resen');

UPDATE stakeholders."TourProblems"
	SET "Comments" = ARRAY[11]
	WHERE "Id" = -11;


