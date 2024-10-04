-- DROP TABLE reports;

CREATE TABLE reports(
	id SERIAL PRIMARY KEY,
	sender_id INTEGER REFERENCES user_profiles(id) NOT NULL,
	receiver_id INTEGER REFERENCES user_profiles(id) NOT NULL,
	violation_id INTEGER REFERENCES violations(id) NOT NULL
);