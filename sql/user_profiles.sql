DROP TABLE user_profiles;

CREATE TABLE user_profiles(
	id SERIAL PRIMARY KEY,
	user_id INTEGER REFERENCES users(id) NOT NULL,
	first_name VARCHAR(128) NOT NULL,
	last_name VARCHAR(128) NOT NULL,
	avatar_url VARCHAR(512) NOT NULL,
	age INTEGER,
	location VARCHAR(256),
	biography VARCHAR(40000),
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	role_id INTEGER REFERENCES roles(id) NOT NULL
);

INSERT INTO user_profiles(user_id, first_name, last_name, avatar_url, age, location, biography, role_id)
VALUES (1, 'Ivan', 'Lianha', 'https://whatever.com/avatar1.png', 20, 'Minsk', 'Ivan`s bio', 3);

INSERT INTO user_profiles(user_id, first_name, last_name, avatar_url, age, location, biography, role_id)
VALUES (2, 'Egor', 'Kazakevich', 'https://whatever.com/avatar2.png', 14, 'Kopischa', 'Egor`s bio', 1);

INSERT INTO user_profiles(user_id, first_name, last_name, avatar_url, age, location, biography, role_id)
VALUES (3, 'Danik', 'Potrebko', 'https://whatever.com/avatar3.png', 19, 'Ostroshitzy', 'Danik`s bio', 2);

INSERT INTO user_profiles(user_id, first_name, last_name, avatar_url, age, location, biography, role_id)
VALUES (4, 'Slava', 'Sapronenko', 'https://whatever.com/avatar4.png', 19, 'Minsk', 'Slava`s bio', 1);

ALTER TABLE user_profiles
ADD ban_status_id INTEGER REFERENCES ban_statuses(id);

UPDATE user_profiles
SET ban_status_id = id;

ALTER TABLE user_profiles
ALTER COLUMN ban_status_id
SET NOT NULL;

SELECT * FROM user_profiles;