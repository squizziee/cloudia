-- DROP TABLE ban_statuses;


-- CREATE TABLE ban_statuses(
-- 	id SERIAL PRIMARY KEY,
-- 	ban_end_date DATE
-- );

-- INSERT INTO ban_statuses (start_date) VALUES(null);
-- INSERT INTO ban_statuses (start_date) VALUES(null);
-- INSERT INTO ban_statuses (start_date) VALUES(null);
-- INSERT INTO ban_statuses (start_date) VALUES(null);

ALTER TABLE ban_statuses
ADD COLUMN ban_end_date DATE; 

SELECT * FROM ban_statuses;