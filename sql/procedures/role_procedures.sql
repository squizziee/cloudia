CREATE OR REPLACE PROCEDURE add_role(name_new VARCHAR, description_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO roles(name, description)
		VALUES (name_new, description_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE update_role(role_id INT, name_new VARCHAR, description_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    UPDATE roles
		SET name = name_new
		WHERE id = role_id;

		UPDATE roles
		SET description = description_new
		WHERE id = role_id;
	END;
$$;

CREATE OR REPLACE PROCEDURE delete_role(role_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM roles
		WHERE id = role_id;
	END;
$$;

CREATE OR REPLACE PROCEDURE ban_user(ban_receiver_profile_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    UPDATE ban_statuses
		SET ban_end_date = (
			CURRENT_DATE + 
			(SELECT SUM(tmp) FROM 
				(SELECT DISTINCT ON(violation_id, post_id) SUM(ban_days) as tmp
				FROM (reports RIGHT JOIN violations ON reports.violation_id = violations.id) FULL JOIN user_profiles ON reports.receiver_id = user_profiles.id
				WHERE receiver_id = 3
				GROUP BY violation_id, post_id))::INTEGER
		)
		WHERE id = ban_receiver_profile_id;
	END;
$$;

CREATE OR REPLACE PROCEDURE unban_user(unban_receiver_profile_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    UPDATE ban_statuses
		SET ban_end_date = NULL
		WHERE id = unban_receiver_profile_id;
	END;
$$;