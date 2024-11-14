CREATE OR REPLACE PROCEDURE add_report(sender_id_new INT, receiver_id_new INT, violation_id_new INT) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO reports(sender_id, receiver_id, violation_id)
		VALUES (sender_id, receiver_id, violation_id);
	END;
$$;