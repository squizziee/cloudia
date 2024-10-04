
-- ban user
UPDATE ban_statuses
SET ban_end_date = (
	CURRENT_DATE + 
	(SELECT SUM(ban_days)
	FROM (reports INNER JOIN violations ON reports.violation_id = violations.id) JOIN user_profiles ON reports.receiver_id = user_profiles.id
	WHERE receiver_id = 3)::INTEGER
)
WHERE id = 3;

-- get days left of ban
SELECT email, first_name, last_name, ban_end_date - CURRENT_DATE as days_left
FROM (ban_statuses INNER JOIN user_profiles ON ban_statuses.id = user_profiles.ban_status_id) INNER JOIN users ON user_profiles.user_id = users.id
WHERE user_profiles.id = 3;
