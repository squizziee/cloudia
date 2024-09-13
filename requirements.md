# Requirements
## Tables
* Users - crucial user data
  * id: int - primary key
  * email: varchar - user email
  * password_hash: int - hashed user's password 
* UserProfiles - diversified user data
  * id: int - primary key
  * user_id: int - Users foreign key
  * first_name: varchar - user first name
  * last_name: varchar - user last name 
  * avatar_url: varchar - user avatar link
  * age: int - user age
  * location: varchar - user general location 
  * biography: varchar - user bio 
  * created_at: timestamp - time when account was created    
* Roles - user roles (Administrator, Moderator, Basic, etc)
  * id: int - primary key
  * name: varchar - role name
  * description: varchar - role description
  * Permissions - permissions for roles (Post moderation, etc)
  * id: int - primary key
  * name: varchar - role name
  * description: varchar - role description
* Posts - message posted by user. Can contain text and attachments (photo, video, audio, etc)
  * id: int - primary key
  * user_profile_id: int - UserProfiles foreign key
  * text: varchar - post content 
* PostAttachments - post file attachment (photo, video, audio, etc)
  * id: int - primary key
  * post_id: int - Posts foreign key
  * source_url: varchar - link to source
* Likes - likes on posts
  * id: int - primary key
  * user_profile_id: int - UserProfiles foreign key
  * post_id: int - Posts foreign key
* Comments - comments on posts
  * id: int - primary key
  * user_profile_id: int - UserProfiles foreign key
  * post_id: int - Posts foreign key
  * text: varchar - comment content 
* Reports - reports of misbehaviour with particular type of violation
  * id: int - primary key
  * sender_id: int - UserProfiles foreign key
  * receiver_id: int - UserProfiles foreign key
  * violation_id: int - Violations foreign key
* Violation - violation of community guidelines and/or terms of service
  * id: int - primary key
  * name: varchar - violation name
  * description: varchar - violation description
  * ban_days: int - user ban for violation in days
## API
### Users
#### Basic users
* Can register/log into account
* Can edit personal information
* Can CRUD his posts
* Can CRUD attachments on his posts
* Can report any other user
* Can like/unlike any post
* Can CRUD comments on any post
#### Moderator
* Can delete any other user's post
#### Administrator
* Can ban any other user
