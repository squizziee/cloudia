-- -- get_subscribtions
-- SELECT first_name, last_name
-- FROM subscribers INNER JOIN user_profiles ON user_profiles.id = subscribers.subscription_id
-- WHERE subscriber_id = 1;

-- -- get_subscribers
-- SELECT first_name, last_name
-- FROM subscribers INNER JOIN user_profiles ON user_profiles.id = subscribers.subscriber_id
-- WHERE subscription_id = 4;

-- -- get subscriber count
-- SELECT COUNT(subscriber_id)
-- FROM subscribers
-- WHERE subscription_id = 4
-- GROUP BY subscription_id;

-- get subscriptions and their sub count

-- CREATE FUNCTION COUNT_SUBSCRIBERS(id_to_search INTEGER)
-- 	returns INTEGER
-- 	language plpgsql
-- as 
-- $$
-- declare 
-- 	sub_count INTEGER;
-- begin
-- 	SELECT COUNT(subscriber_id)
-- 	INTO sub_count
-- 	FROM subscribers
-- 	WHERE subscription_id = id_to_search;
-- 	return sub_count;
-- end
-- $$;

-- SELECT first_name, last_name, id, COUNT_SUBSCRIBERS(id)
-- FROM subscribers LEFT JOIN user_profiles ON subscription_id = user_profiles.id
-- WHERE subscriber_id = 1