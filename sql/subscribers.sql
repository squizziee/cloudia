-- DROP TABLE subscribers;

-- CREATE TABLE subscribers (
-- 	subscriber_id INTEGER REFERENCES user_profiles(id) NOT NULL,
-- 	subscription_id INTEGER REFERENCES user_profiles(id) NOT NULL,

-- 	CONSTRAINT not_subscribed_to_yourself CHECK (subscriber_id <> subscription_id)
-- );

-- CREATE UNIQUE INDEX subscribed_only_once ON subscribers(subscriber_id, subscription_id);

-- INSERT INTO subscribers (subscriber_id, subscription_id)
-- VALUES (1, 2);

-- INSERT INTO subscribers (subscriber_id, subscription_id)
-- VALUES (1, 3);

-- INSERT INTO subscribers (subscriber_id, subscription_id)
-- VALUES (3, 1);

-- INSERT INTO subscribers (subscriber_id, subscription_id)
-- VALUES (2, 4);

-- INSERT INTO subscribers (subscriber_id, subscription_id)
-- VALUES (3, 4);

-- SELECT * FROM subscribers;


INSERT INTO subscribers (subscriber_id, subscription_id)
VALUES (1, 4);