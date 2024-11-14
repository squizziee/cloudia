
-- search user by name
SELECT first_name, last_name, avatar_url, id
FROM user_profiles
WHERE CONCAT(first_name, ' ', last_name) ~'Dani';

-- search user with multiple filters
SELECT first_name, last_name, avatar_url, id, age, location
FROM user_profiles
WHERE (age BETWEEN 14 and 19) 
	AND (CONCAT(first_name, ' ', last_name) ~'ko')
	AND (location ~'Minsk');

-- search user with multiple non-empty filters
SELECT first_name, last_name, avatar_url, id, age, location
FROM user_profiles
WHERE (age BETWEEN 14 and 19) 
	AND (biography NOT LIKE '' OR biography IS NOT NULL)
	AND (location NOT LIKE '' OR location IS NOT NULL);

-- search posts by text
SELECT id, text_content
FROM posts
WHERE text_content ~'odo';

-- search posts without images
SELECT DISTINCT ON (posts.id) posts.id, text_content, source_url
FROM posts LEFT JOIN post_attachments ON post_attachments.post_id = posts.id
WHERE source_url NOT SIMILAR TO ('%(.png|.jpg)') OR source_url IS NULL
ORDER BY posts.id
