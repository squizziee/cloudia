DROP TABLE comments;

CREATE TABLE comments(
	id SERIAL PRIMARY KEY,
	user_profile_id INTEGER REFERENCES user_profiles(id) NOT NULL,
	post_id INTEGER REFERENCES posts(id) NOT NULL,
	text_content VARCHAR(40000)
);

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (2, 1, 'Lmaoooo');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (2, 2, 'Lmasdfsoooo');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (3, 5, 'sdLmsdfaoooo');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (2, 5, 'sdfLmaoofsdfsoo');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (2, 5, 'fdLmaoosdfsdf');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (1, 4, 'daLmaoooo');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (3, 1, 'Lmaosdfsooo');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (4, 5, 'kek');

INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (1, 2, 'asdfwsfLmaoooo');

SELECT * FROM comments;