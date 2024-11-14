CREATE OR REPLACE PROCEDURE add_permission(name_new VARCHAR, description_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO permissions(name, description)
		VALUES (name_new, description_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE update_permission(permission_id INT, name_new VARCHAR, description_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    UPDATE permissions
		SET name = name_new
		WHERE id = permission_id;

		UPDATE permissions
		SET description = description_new
		WHERE id = permission_id;
	END;
$$;

CREATE OR REPLACE PROCEDURE delete_permission(permission_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM permissions
		WHERE id = permission_id;
	END;
$$;

-- CREATE UNIQUE INDEX non_repeating_role_permissions ON roles_permissions(role_id, permission_id);

CREATE OR REPLACE PROCEDURE link_role_to_permission(role_id_ INT, permission_id_ INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO roles_permissions(role_id, permission_id)
		VALUES (role_id_, permission_id_);
	END;
$$;

CREATE OR REPLACE PROCEDURE unlink_role_to_permission(role_id_ INT, permission_id_ INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM roles_permissions
		WHERE role_id = role_id_ AND permission_id = permission_id_;
	END;
$$;