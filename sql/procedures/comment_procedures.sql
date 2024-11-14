CREATE OR REPLACE PROCEDURE add_comment(user_profile_id_new INT, post_id_new INT, text_content_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO comments(user_profile_id, post_id, text_content)
		VALUES (user_profile_id_new, post_id_new, text_content_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE update_comment(comment_id INT, text_content_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    UPDATE comments
		SET text_content = text_content_new
		WHERE id = comment_id;
	END;
$$;

CREATE OR REPLACE PROCEDURE delete_comment(comment_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM comments
		WHERE id = comment_id;
	END;
$$;