# Requirements
## Tables
* Users - crucial user data
  * id: int - primary key
  * email: varchar - user email
  * password_hash: int - hashed user's password 
* UserProfiles - diversified user data
  * id: int - primary key
  * user_id: int - Users foreign key
  * first_name: varchar - user first name (1 - 100 symbols)
  * last_name: varchar - user last name (1 - 100 symbols)
  * avatar_url: varchar - user avatar link (0 - 200 symbols)
  * age: int - user age (0 - 200)
  * location: varchar - user general location (0 - 200 symbols)
  * biography: varchar - user bio (0 to 1000 symbols)
  * created_at: timestamp - time when account was created    
* Roles - user roles (Administrator, Moderator, Basic, etc)
* Permissions - permissions for roles (Post moderation, etc)
* Posts - message posted by user. Can contain text and attachments (photo, video, audio, etc)
* PostAttachments - post file attachment (photo, video, audio, etc)
* Likes - likes on posts
* Comments - comments on posts
* Reports - reports of misbehaviour with particular type of violation
* Violation - violation of community guidelines and/or terms of service
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
