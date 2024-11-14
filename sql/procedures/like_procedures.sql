CREATE OR REPLACE PROCEDURE add_like(user_profile_id_new INT, post_id_new INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO likes(user_profile_id, post_id)
		VALUES (user_profile_id_new, post_id_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE remove_like(user_profile_id_ INT, post_id_ INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM likes
		WHERE user_profile_id = user_profile_id_ AND post_id = post_id_;
	END;
$$;