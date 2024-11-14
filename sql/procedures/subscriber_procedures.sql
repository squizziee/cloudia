CREATE OR REPLACE PROCEDURE subscribe(
	subscriber_id_new INT, 
	subscription_id_new INT
) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    INSERT INTO subscribers(subscriber_id, subscription_id)
		VALUES (subscriber_id_new, subscription_id_new);
	END;
$$;

CREATE OR REPLACE PROCEDURE unsubscribe(
	subscriber_id_ INT, 
	subscription_id_ INT
) 
LANGUAGE plpgsql
AS $$
	BEGIN
	    DELETE FROM subscribers
		WHERE subscriber_id = subscriber_id_ AND subscription_id = subscription_id_;
	END;
$$;

CALL unsubscribe(1, 3);

SELECT * FROM subscribers;