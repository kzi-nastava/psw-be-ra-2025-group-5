INSERT INTO blog."BlogPosts" ("Id", "AuthorId", "Title", "Description", "CreatedAt", "LastUpdatedAt", "Status") VALUES
(1, 1, 'Journey Through the Alps', 'A fantastic description of the trip with beautiful photos and tips.', '2023-10-01 10:00:00', '2023-11-01 12:00:00', 'Famous'),
(2, 2, 'Top 10 Cities in Europe', 'A list of must-see places for every tourist.', '2023-10-15 09:00:00', '2023-11-05 10:00:00', 'Active'),
(3, 3, 'Packing Tips', 'How to pack everything you need for 7 days into a small backpack.', '2023-10-20 14:00:00', NULL, 'Published'),
(4, 1, 'Hidden Beaches of the Adriatic', 'Places you must not miss if you love peace and quiet.', '2023-11-01 16:00:00', NULL, 'Published'),
(5, 2, 'Himalayan Diary (Draft)', 'The first part of the ascent adventure (still under construction).', '2023-11-10 11:00:00', NULL, 'Draft'),
(6, 3, 'Camping Equipment - Review', 'Review of new equipment and price comparison.', '2023-11-15 13:00:00', '2023-11-16 13:00:00', 'ReadOnly');

INSERT INTO blog."Comments" ("CommentId", "AuthorId", "CreatedAt", "UpdatedAt", "Content", "BlogPostId") VALUES
(1, 4, '2023-11-01 15:00:00', NULL, 'Excellent blog! I tried the route, I recommend it!', 1),
(2, 5, '2023-11-01 16:30:00', NULL, 'Beautiful photos, great job!', 1),
(3, 6, '2023-11-01 17:00:00', NULL, 'Which lens do you use for these pictures?', 1),
(4, 4, '2023-11-05 12:00:00', '2023-11-05 14:00:00', 'Solid list, but Prague is missing, you should add it!', 2),
(5, 5, '2023-11-05 16:00:00', NULL, 'Vienna is deservedly on the list.', 2),
(6, 6, '2023-10-21 10:00:00', NULL, 'Great tips, it helped me with my packing!', 3);

INSERT INTO blog."BlogVotes" ("Id", "UserId", "BlogPostId", "VoteDate", "VoteType") VALUES
(1, 4, 1, '2023-11-01 14:00:00', 1), 
(2, 5, 1, '2023-11-01 14:15:00', 1), 
(3, 6, 1, '2023-11-01 14:30:00', 1), 
(4, 1, 1, '2023-11-01 14:40:00', 1), 
(5, 2, 1, '2023-11-01 14:50:00', 1), 
(6, 4, 2, '2023-11-05 11:00:00', 1), 
(7, 5, 2, '2023-11-05 11:30:00', -1),
(8, 6, 2, '2023-11-05 12:30:00', 1),
(9, 4, 3, '2023-10-20 18:00:00', 1),
(10, 5, 3, '2023-10-20 19:00:00', 1), 
(11, 4, 6, '2023-11-16 10:00:00', -1),
(12, 5, 6, '2023-11-16 10:15:00', -1),
(13, 6, 6, '2023-11-16 10:30:00', -1);

SELECT setval(pg_get_serial_sequence('blog."BlogPosts"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM blog."BlogPosts"));
SELECT setval(pg_get_serial_sequence('blog."Comments"', 'CommentId'), (SELECT COALESCE(MAX("CommentId"),0) FROM blog."Comments"));
SELECT setval(pg_get_serial_sequence('blog."BlogVotes"', 'Id'), (SELECT COALESCE(MAX("Id"),0) FROM blog."BlogVotes"));
