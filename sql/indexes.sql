-- CREATE INDEX comments_by_post_id ON comments(post_id);
-- CREATE INDEX comments_by_user ON comments(user_profile_id);

-- CREATE INDEX likes_by_post_id ON likes(post_id);
-- CREATE INDEX likes_by_user ON likes(user_profile_id);

-- CREATE INDEX permissions_default ON permissions(id);

-- CREATE INDEX post_attachments_default ON post_attachments(post_id);

-- CREATE INDEX posts_default ON posts(user_profile_id);

-- CREATE INDEX reports_default ON reports(receiver_id);

-- CREATE INDEX roles_default ON roles(id);

-- CREATE INDEX subscribers_by_subscriber ON subscribers(subscriber_id);
-- CREATE INDEX subscribers_by_subscripton ON subscribers(subscription_id);

-- CREATE INDEX user_profiles_default ON user_profiles(id);
-- CREATE INDEX user_profiles_by_user_id ON user_profiles(user_id);

-- CREATE INDEX users_default ON users(id);
-- CREATE INDEX violations_default ON violations(id);

SELECT last_name 
FROM user_profiles
WHERE right(last_name,2)='ch'