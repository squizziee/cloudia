CREATE OR REPLACE FUNCTION autoban() RETURNS TRIGGER AS $autoban$
	DECLARE
		same_type_violation_report_count INTEGER;
	BEGIN
		SELECT COUNT(id)
		INTO same_type_violation_report_count
		FROM reports
		WHERE violation_id = NEW.violation_id AND receiver_id = NEW.receiver_id;
		IF same_type_violation_report_count >= 3 THEN
			UPDATE ban_statuses
			SET ban_end_date = (
				CURRENT_DATE + 
				(
					SELECT DISTINCT ON (violation_id) SUM(ban_days) FROM reports LEFT JOIN violations ON reports.violation_id = violations.id
					WHERE receiver_id = NEW.receiver_id AND violation_id = NEW.violation_id
					GROUP BY violation_id
				)::INTEGER / same_type_violation_report_count
			)
			WHERE id = NEW.receiver_id;
		END IF;
		RETURN NEW;
	END;
$autoban$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER autoban
AFTER INSERT ON reports FOR EACH ROW EXECUTE PROCEDURE autoban();

-- -- CREATE UNIQUE INDEX one_report_to_one_user ON reports(sender_id, receiver_id, violation_id, post_id);

-- -- INSERT INTO reports (sender_id, receiver_id, violation_id, post_id)
-- -- VALUES (1, 2, 1, 1);

DELETE FROM reports
WHERE receiver_id = 2;

INSERT INTO reports (sender_id, receiver_id, violation_id, post_id)
VALUES (1, 2, 1, 1);

INSERT INTO reports (sender_id, receiver_id, violation_id, post_id)
VALUES (2, 2, 1, 1);

INSERT INTO reports (sender_id, receiver_id, violation_id, post_id)
VALUES (3, 2, 1, 1);

-- SELECT * FROM reports;
SELECT * FROM ban_statuses;

-- SELECT receiver_id, violation_id, ban_days
-- FROM reports LEFT JOIN violations ON reports.violation_id = violations.id;