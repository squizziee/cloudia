-- DROP TABLE reports;

-- CREATE TABLE reports(
-- 	id SERIAL PRIMARY KEY,
-- 	sender_id INTEGER REFERENCES user_profiles(id) NOT NULL,
-- 	receiver_id INTEGER REFERENCES user_profiles(id) NOT NULL,
-- 	violation_id INTEGER REFERENCES violations(id) NOT NULL
--  post_id INTEGER REFERENCES posts(id) NOT NULL
-- );

-- INSERT INTO reports (sender_id, receiver_id, violation_id)
-- VALUES(2, 3, 1);

-- INSERT INTO reports (sender_id, receiver_id, violation_id)
-- VALUES(3, 4, 2);

-- INSERT INTO reports (sender_id, receiver_id, violation_id)
-- VALUES(1, 2, 3);

-- INSERT INTO reports (sender_id, receiver_id, violation_id)
-- VALUES(2, 3, 2);

SELECT * FROM reports;