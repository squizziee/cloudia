
-- ban user
UPDATE ban_statuses
SET start_date CURRENT_DATE,
	report_id 0;
