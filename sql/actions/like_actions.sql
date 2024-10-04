
-- like post
INSERT INTO likes (user_profile_id, post_id)
VALUES (0, 0);

-- unlike post
DELETE FROM likes 
WHERE user_profile_id = 0 AND post_id = 0
