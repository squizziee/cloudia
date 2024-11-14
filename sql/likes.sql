DROP TABLE likes;

CREATE TABLE likes(
	id SERIAL PRIMARY KEY NOT NULL,
	user_profile_id INTEGER REFERENCES user_profiles(id) NOT NULL,
	post_id INTEGER REFERENCES user_profiles(id) ON DELETE CASCADE NOT NULL,
	posted_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE UNIQUE INDEX unique_user_profile_id_post_id ON likes(user_profile_id, post_id);

INSERT INTO likes (user_profile_id, post_id)
VALUES(1, 1);

INSERT INTO likes (user_profile_id, post_id)
VALUES(2, 1);

INSERT INTO likes (user_profile_id, post_id)
VALUES(2, 2);

INSERT INTO likes (user_profile_id, post_id)
VALUES(3, 4);

SELECT * FROM likes;