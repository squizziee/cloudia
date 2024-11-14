
-- -- add post
-- INSERT INTO posts (user_profile_id, text_content)
-- VALUES (0, '');

-- -- delete post
-- DELETE FROM posts
-- WHERE post_id = 0;

-- -- update post
-- UPDATE posts
-- SET text_content = ''
-- WHERE post_id = 0;

-- -- get post attachments
-- SELECT source_url
-- FROM post_attachments
-- WHERE post_id = 3;

-- -- get comments for post
-- SELECT first_name, last_name, text_content, avatar_url, posted_at
-- FROM comments INNER JOIN user_profiles ON comments.user_profile_id = user_profiles.id
-- WHERE post_id = 5
-- ORDER BY posted_at DESC;

-- -- get likes for post
-- SELECT first_name, last_name, avatar_url, posted_at
-- FROM likes INNER JOIN user_profiles ON likes.user_profile_id = user_profiles.id
-- WHERE post_id = 1
-- ORDER BY posted_at DESC;

-- -- filter out post creator comments (suppose post creator id is 4)
-- SELECT first_name, last_name, created_at, text_content 
-- FROM COMMENTS INNER JOIN user_profiles ON comments.user_profile_id = user_profiles.id
-- WHERE comments.user_profile_id = 4

