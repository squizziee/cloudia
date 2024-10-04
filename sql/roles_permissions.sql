--DROP TABLE roles;
-- DROP TABLE permissions;

-- CREATE TABLE roles(
-- 	id SERIAL PRIMARY KEY,
-- 	name VARCHAR(128) UNIQUE NOT NULL,
-- 	description VARCHAR(1000) NOT NULL
-- );

CREATE TABLE permissions(
	id SERIAL PRIMARY KEY,
	name VARCHAR(128) UNIQUE NOT NULL,
	description VARCHAR(1000) NOT NULL
);

CREATE TABLE roles_permissions(
	role_id INTEGER REFERENCES roles(id) NOT NULL,
	permission_id INTEGER REFERENCES permissions(id) NOT NULL
);

-- INSERT INTO roles(name, description)
-- VALUES ('Basic', 'Normal user without any special permissions');

-- INSERT INTO roles(name, description)
-- VALUES ('Moderator', 'Can delete any other user`s post');

-- INSERT INTO roles(name, description)
-- VALUES ('Admin', 'Can ban any other user');

-- default permissions
INSERT INTO permissions(name, description)
VALUES ('Login', 'Access to login');

INSERT INTO permissions(name, description)
VALUES ('Commenting', 'Ability to leave comments');

INSERT INTO permissions(name, description)
VALUES ('Posting', 'Access to posting');

-- moderation permissions
INSERT INTO permissions(name, description)
VALUES ('Moderated deletion', 'Deletion of other user`s post');

INSERT INTO permissions(name, description)
VALUES ('Moderated editing', 'Editing of other user`s post');

-- admin permissions
INSERT INTO permissions(name, description)
VALUES ('Purge', 'Banning any other user');

-- many-to-many for roles and permissions
INSERT INTO roles_permissions (role_id, permission_id) VALUES(1, 1);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(1, 2);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(1, 3);

INSERT INTO roles_permissions (role_id, permission_id) VALUES(2, 1);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(2, 2);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(2, 3);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(2, 4);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(2, 5);

INSERT INTO roles_permissions (role_id, permission_id) VALUES(3, 1);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(3, 2);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(3, 3);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(3, 4);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(3, 5);
INSERT INTO roles_permissions (role_id, permission_id) VALUES(3, 6);

SELECT * FROM roles_permissions;