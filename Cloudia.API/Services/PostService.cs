using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace Cloudia.API.Services
{
    public class PostService : IPostService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<PostService> _logger;
        private readonly IPostAttachmentService _postAttachmentService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;

        public PostService(
                IApplicationContext context, 
                ILogger<PostService> logger, 
                IPostAttachmentService postAttachmentService,
                ILikeService likeService,
                ICommentService commentService) { 
            this._context = context;
            this._logger = logger; 
            this._postAttachmentService = postAttachmentService;
            this._likeService = likeService;
            this._commentService = commentService;
        }


        private UserProfile? GetUserProfile(int userId)
        {
            var user = _context.Users.FromSql($"SELECT * FROM users WHERE id = {userId}").FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return _context.UserProfiles.FromSql($"SELECT * FROM user_profiles WHERE id = {user.user_profile_id}").FirstOrDefault();
        }

        public async Task<Post> AddPost(int userId, string textContent, IFormFileCollection attachments)
        {
            var userProfile = GetUserProfile(userId);

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("add_post", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("user_profile_id_new", userProfile!.id);
            command.Parameters.AddWithValue("text_content_new", textContent);

            var id = (int) await command.ExecuteScalarAsync();

            await connection.CloseAsync();

            _postAttachmentService.CreatePostAttachments(id, attachments);

            return (await GetPost(id))!;
        }

        public async void DeletePost(int id)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("delete_post", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("post_id", id);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task<Post?> GetPost(int id)
        {
            return await _context.Posts.FromSql($"SELECT * FROM posts WHERE id = {id}").FirstOrDefaultAsync();
        }

        public async Task<List<Post>> GetPosts(string filter)
        {
            return await _context.Posts.FromSqlRaw($"SELECT * FROM posts WHERE text_content LIKE '%{filter}%'").ToListAsync();
        }

        public async Task<Post> UpdatePost(int postId, string textContent, IFormFileCollection newAttachments)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL update_post(@post_id, @text_content_new)", connection);
            command.Parameters.AddWithValue("@post_id", postId);
            command.Parameters.AddWithValue("@text_content_new", textContent);
            
            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            _postAttachmentService.UpdatePostAttachments(postId, newAttachments);

            return (await GetPost(postId))!;
        }

        public async Task<(Post? post, List<PostAttachment>? attachments, List<Comment>? comments, List<Like>? likes)> GetFullPost(int id)
        {
            var _likes = await _context.Likes.FromSql($"SELECT * FROM likes WHERE post_id = {id}").ToListAsync();
            var _comments = await _context.Comments.FromSql($"SELECT * FROM comments WHERE post_id = {id}").ToListAsync();
            var _attachments = await _context.PostAttachments.FromSql($"SELECT * FROM post_attachments WHERE post_id = {id}").ToListAsync();
            var _post = await GetPost(id);

            return (post: _post, attachments: _attachments, comments: _comments, likes: _likes);
        }
    }
}
