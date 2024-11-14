CREATE OR REPLACE PROCEDURE register_user(
	email_new VARCHAR, 
	first_name_new VARCHAR,
	last_name_new VARCHAR
) 
LANGUAGE plpgsql
AS $$
	DECLARE
		new_user_id INT;
		new_ban_status_id INT;
		new_user_profile_id INT;
	BEGIN
	    INSERT INTO users(email)
		VALUES (email_new)
		RETURNING id INTO new_user_id;

		INSERT INTO ban_statuses(ban_end_date)
		VALUES (NULL)
		RETURNING id INTO new_ban_status_id;

		INSERT INTO user_profiles(user_id, first_name, last_name, ban_status_id)
		VALUES (new_user_id, first_name_new, last_name_new, new_ban_status_id)
		RETURNING id INTO new_user_profile_id;
	END;
$$;