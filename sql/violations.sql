DROP TABLE violations;

CREATE TABLE violations(
	id SERIAL PRIMARY KEY,
	name VARCHAR(128) NOT NULL,
	description VARCHAR(1000) NOT NULL,
	ban_days INTEGER NOT NULL
);

INSERT INTO violations(name, description, ban_days)
VALUES('Children abuse', 'Legal description', 28);

INSERT INTO violations(name, description, ban_days)
VALUES('Animal abuse', 'Legal description', 14);

INSERT INTO violations(name, description, ban_days)
VALUES('Nudity', 'Legal description', 7);

INSERT INTO violations(name, description, ban_days)
VALUES('Misinformation', 'Legal description', 1001);

SELECT * FROM violations;
