-- DROP TABLE posts;

CREATE TABLE posts(
	id SERIAL PRIMARY KEY,
	user_profile_id INTEGER REFERENCES user_profiles(id) NOT NULL,
	text_content VARCHAR(40000)
);

INSERT INTO posts (user_profile_id, text_content)
VALUES(2, 'Lorem ipsum odor amet, consectetuer adipiscing elit. Eget dignissim sodales bibendum malesuada tincidunt quam fermentum at netus.');

INSERT INTO posts (user_profile_id, text_content)
VALUES(2, 'Lorem ipsum odor amet, consectetuer adipiscing elit. Potenti donec phasellus ante sapien gravida integer hac conubia platea. Vehicula sodales id ad nisl nunc torquent rhoncus fames.');

INSERT INTO posts (user_profile_id, text_content)
VALUES(4, 'Lorem ipsum odor amet, consectetuer adipiscing elit. Viverra pharetra interdum at facilisi ipsum dui.');

INSERT INTO posts (user_profile_id, text_content)
VALUES(1, 'Lorem ipsum odor amet, consectetuer adipiscing elit. Mus lobortis lectus hac eros tempus justo.');

INSERT INTO posts (user_profile_id, text_content)
VALUES(3, 'Lorem ipsum odor amet, consectetuer adipiscing elit. Facilisis cursus commodo auctor cursus finibus.');

INSERT INTO posts (user_profile_id, text_content)
VALUES(3, 'Lorem ipsum odor amet, consectetuer adipiscing elit. Natoque dolor bibendum hendrerit vestibulum porta nascetur.');

INSERT INTO posts (user_profile_id, text_content)
VALUES(3, 'Lorem ipsum odor amet, consectetuer adipiscing elit. Urna consequat risus senectus efficitur ipsum.');

SELECT * FROM posts;