INSERT INTO blog_post (id, author_id, title, description, created_at, last_update_at, status)
VALUES 
(-1, 1, 'Prvi test blog', 'Opis prvog test bloga', NOW(), NULL, 'Published'),
(-2, 1, 'Drugi test blog', 'Opis drugog test bloga', NOW(), NULL, 'Draft');

INSERT INTO blog_image (id, blog_post_id, data, content_type, "order")
VALUES
(-1, -1, decode('010203','hex'), 'image/png', 0),
(-2, -1, decode('040506','hex'), 'image/jpeg', 1),
(-3, -2, decode('0A0B0C','hex'), 'image/png', 0);