using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cloudia.API.Services
{
    public class PostService : IPostService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<PostService> _logger;
        IPostAttachmentService _postAttachmentService;

        public PostService(IApplicationContext context, ILogger<PostService> logger, IPostAttachmentService postAttachmentService) { 
            this._context = context;
            this._logger = logger; 
            this._postAttachmentService = postAttachmentService;
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

        public List<Post> GetPosts(string filter)
        {
            throw new NotImplementedException();
        }

        public void UpdatePost(int id)
        {
            throw new NotImplementedException();
        }
    }
}
