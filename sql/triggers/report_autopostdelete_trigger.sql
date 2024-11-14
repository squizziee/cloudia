-- CREATE OR REPLACE FUNCTION autodelete_post() RETURNS TRIGGER AS $autodelete_post$
-- 	DECLARE
-- 		same_type_violation_report_count_on_post INTEGER;
-- 	BEGIN
-- 		SELECT COUNT(id)
-- 		INTO same_type_violation_report_count_on_post
-- 		FROM reports
-- 		WHERE violation_id = NEW.violation_id 
-- 			AND receiver_id = NEW.receiver_id
-- 			AND post_id = NEW.post_id;
-- 		IF same_type_violation_report_count_on_post >= 3 THEN
-- 			DELETE FROM posts
-- 			WHERE id = NEW.post_id;
-- 		END IF;
-- 		RETURN NEW;
-- 	END;
-- $autodelete_post$ LANGUAGE plpgsql;

-- CREATE OR REPLACE TRIGGER autodelete_post
-- AFTER INSERT ON reports FOR EACH ROW EXECUTE PROCEDURE autodelete_post();

-- CREATE OR REPLACE FUNCTION report_guard() RETURNS TRIGGER AS $report_guard$
-- 	DECLARE
-- 		post_exists BOOL;
-- 	BEGIN
-- 		SELECT EXISTS(
-- 			SELECT 1
-- 			FROM posts
-- 			WHERE user_profile_id = NEW.receiver_id
-- 				AND id = NEW.post_id
-- 		)
-- 		INTO post_exists;
-- 		IF NOT post_exists THEN
-- 			RAISE EXCEPTION 'No such post for such user';
-- 		END IF;
-- 		RETURN NEW;
-- 	END;
-- $report_guard$ LANGUAGE plpgsql;

-- CREATE OR REPLACE TRIGGER report_guard
-- BEFORE INSERT ON reports FOR EACH ROW EXECUTE PROCEDURE report_guard();

DELETE FROM reports
WHERE receiver_id = 2;

-- SELECT * FROM posts;

INSERT INTO reports (sender_id, receiver_id, violation_id, post_id)
VALUES (1, 2, 1, 1);

INSERT INTO reports (sender_id, receiver_id, violation_id, post_id)
VALUES (2, 2, 1, 1);

INSERT INTO reports (sender_id, receiver_id, violation_id, post_id)
VALUES (3, 2, 1, 1);

-- -- SELECT * FROM reports;
-- SELECT * FROM ban_statuses;

-- SELECT receiver_id, violation_id, ban_days
-- FROM reports LEFT JOIN violations ON reports.violation_id = violations.id;

SELECT * FROM posts;