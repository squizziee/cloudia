# Requirements
## Tables
* Users - crucial user data
* UserProfiles - diversified user data
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
