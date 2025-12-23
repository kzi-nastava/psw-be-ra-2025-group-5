INSERT INTO blog."BlogPosts" ("Id", "AuthorId", "Title", "Description", "CreatedAt", "LastUpdatedAt", "Status") VALUES
(-1, 1, 'Prvi test blog', 'Opis prvog test bloga', NOW(), NULL, 'Published'),
(-2, 1, 'Drugi test blog', 'Opis drugog test bloga', NOW(), NULL, 'Draft');

INSERT INTO blog."BlogImages" ("Id", "BlogPostId", "ImagePath", "ContentType", "Order") VALUES
(-1, -1, '/images/blog/alps1.jpg', 'image/jpeg', 0),
(-2, -2, '/images/blog/alps2.jpg', 'image/jpeg', 0);