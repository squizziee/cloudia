DROP TABLE post_attachments;

CREATE TABLE post_attachments(
	id SERIAL PRIMARY KEY,
	post_id INTEGER REFERENCES posts(id) NOT NULL,
	source_url VARCHAR(512)
);

INSERT INTO post_attachments (post_id, source_url)
VALUES (1, 'https://whatever.com/attachment1.pdf');

INSERT INTO post_attachments (post_id, source_url)
VALUES (2, 'https://whatever.com/attachment2.pdf');

INSERT INTO post_attachments (post_id, source_url)
VALUES (3, 'https://whatever.com/attachment3.pdf');

INSERT INTO post_attachments (post_id, source_url)
VALUES (3, 'https://whatever.com/attachment4.pdf');

INSERT INTO post_attachments (post_id, source_url)
VALUES (5, 'https://whatever.com/attachment5.pdf');

SELECT * FROM post_attachments;