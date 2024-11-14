
-- ban user
-- UPDATE ban_statuses
-- SET ban_end_date = (
-- 	CURRENT_DATE + 
-- 	(SELECT SUM(tmp) FROM 
-- 		(SELECT DISTINCT ON(violation_id, post_id) SUM(ban_days) as tmp
-- 		FROM (reports RIGHT JOIN violations ON reports.violation_id = violations.id) FULL JOIN user_profiles ON reports.receiver_id = user_profiles.id
-- 		WHERE receiver_id = 3
-- 		GROUP BY violation_id, post_id))::INTEGER
-- )
-- WHERE id = 3;

-- -- delete all reports after ban
-- DELETE FROM reports
-- WHERE receiver_id = 3

-- -- get banned users
-- SELECT email, first_name, last_name, ban_end_date 
-- FROM (user_profiles FULL JOIN ban_statuses ON user_profiles.ban_status_id = ban_statuses.id) 
-- 	FULL JOIN users on user_profiles.user_id = users.id
-- WHERE ban_end_date IS NOT NULL

-- -- get days left of ban
-- SELECT email, first_name, last_name, ban_end_date - CURRENT_DATE as days_left
-- FROM (ban_statuses FULL JOIN user_profiles ON ban_statuses.id = user_profiles.ban_status_id) FULL JOIN users ON user_profiles.user_id = users.id
-- WHERE user_profiles.id = 3;

SELECT first_name, last_name, COUNT(comments.id)
FROM user_profiles 
FULL JOIN reports ON sender_id = user_profiles.id
FULL JOIN comments ON comments.user_profile_id = user_profiles.id
WHERE reports.id IS NOT NULL
GROUP BY first_name, last_name


-- INSERT INTO comments(user_profile_id, text_content, post_id)
-- VALUES (1, 'aboba', 2);