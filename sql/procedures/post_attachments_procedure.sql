CREATE OR REPLACE PROCEDURE add_post_attachment(post_id_new INT, source_url_new VARCHAR) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO post_attachments(post_id, source_url)
		VALUES (post_id_new, source_url_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE delete_post_attachment(post_attachment_id INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM post_attachments
		WHERE id = post_attachment_id;
	END;
$$;