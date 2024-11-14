
-- -- report user
-- INSERT INTO reports (sender_id, receiver_id, violation_id)
-- VALUES (0, 1, 0);

-- wtf
SELECT DISTINCT ON(user_profiles.id) first_name, last_name 
FROM (user_profiles INNER JOIN comments ON user_profiles.id = comments.user_profile_id)
	INNER JOIN posts ON posts.id = comments.post_id
	WHERE (comments.posted_at BETWEEN '2024-10-10 15:00:00' AND '2024-10-10 23:00:00')
		AND CONCAT(first_name, last_name) ILIKE '%ko%'
		AND lower(posts.text_content) SIMILAR TO '%(lorem|amet)%';

-- SELECT * FROM comments;