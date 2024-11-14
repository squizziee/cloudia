CREATE OR REPLACE PROCEDURE add_post(user_profile_id_new INT, text_content_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO posts(user_profile_id, text_content)
		VALUES (user_profile_id_new, text_content_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE update_post(post_id INT, text_content_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    UPDATE posts
		SET text_content = text_content_new
		WHERE id = post_id;
	END;
$$;

CREATE OR REPLACE PROCEDURE delete_post(post_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM posts
		WHERE id = post_id;
	END;
$$;