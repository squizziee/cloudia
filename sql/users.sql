DROP TABLE users;

CREATE TABLE users(
	id SERIAL PRIMARY KEY,
	email VARCHAR(128) UNIQUE NOT NULL
);

INSERT INTO users (email)
VALUES ('email1@gmail.com');

INSERT INTO users (email)
VALUES ('email2@gmail.com');

INSERT INTO users (email)
VALUES ('email3@gmail.com');

INSERT INTO users (email)
VALUES ('email4@gmail.com');

SELECT * FROM users;