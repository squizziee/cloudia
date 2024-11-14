CREATE OR REPLACE PROCEDURE add_violation(name_new VARCHAR, description_new VARCHAR, ban_days_new INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO violations(name, description, ban_days)
		VALUES (name_new, description_new, ban_days_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE update_violation(violation_id INT, name_new VARCHAR, description_new VARCHAR, ban_days_new INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    UPDATE violations
		SET name = name_new
		WHERE id = violation_id;

		UPDATE violations
		SET description = description_new
		WHERE id = violation_id;

		UPDATE violations
		SET ban_days = ban_days_new
		WHERE id = violation_id;
	END;
$$;

CREATE OR REPLACE PROCEDURE delete_violation(violation_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM violations
		WHERE id = violation_id;
	END;
$$;