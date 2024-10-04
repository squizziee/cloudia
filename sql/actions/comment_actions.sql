
-- add comment
INSERT INTO comments (user_profile_id, post_id, text_content)
VALUES (0, 0, '');

-- delete comment
DELETE FROM comments
WHERE id = 0;

-- update comment
UPDATE comments
SET text_content = ''
WHERE id = 0;

