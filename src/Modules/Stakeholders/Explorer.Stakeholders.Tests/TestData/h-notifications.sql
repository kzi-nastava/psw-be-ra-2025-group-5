INSERT INTO stakeholders."Notifications"(
    "Id", "UserId", "Type", "Title", "Message", "IsRead", "CreatedAt", "TourProblemId", "TourId", "ActionUrl")
VALUES (-1, -11, 0, 'New Problem Reported', 'A new problem has been reported on your tour.', false, NOW() - interval '2 days', -11, -1, '/tour-problem/-11/comments');

INSERT INTO stakeholders."Notifications"(
    "Id", "UserId", "Type", "Title", "Message", "IsRead", "CreatedAt", "TourProblemId", "TourId", "ActionUrl")
VALUES (-2, -21, 1, 'New Comment on Problem', 'Administrator has added a comment to a tour problem discussion.', false, NOW() - interval '1 day', -11, -1, '/tour-problem/-11/comments');

INSERT INTO stakeholders."Notifications"(
    "Id", "UserId", "Type", "Title", "Message", "IsRead", "CreatedAt", "TourProblemId", "TourId", "ActionUrl")
VALUES (-3, -11, 2, 'Deadline Set for Problem Resolution', 'An administrator has set a deadline for resolving a problem on your tour.', true, NOW() - interval '5 days', -22, NULL, '/tour-problem/-22/comments');

INSERT INTO stakeholders."Notifications"(
    "Id", "UserId", "Type", "Title", "Message", "IsRead", "CreatedAt", "TourProblemId", "TourId", "ActionUrl")
VALUES (-4, -12, 3, 'Tour Closed', 'Your tour has been closed by an administrator. Reason: Unresolved problems with expired deadline', false, NOW() - interval '3 days', NULL, -3, '/tours');

INSERT INTO stakeholders."Notifications"(
    "Id", "UserId", "Type", "Title", "Message", "IsRead", "CreatedAt", "TourProblemId", "TourId", "ActionUrl")
VALUES (-5, -11, 4, 'Problem Status Changed', 'turista1 has marked the problem as resolved.', false, NOW() - interval '1 hour', -11, -1, '/tour-problem/-11/comments');


