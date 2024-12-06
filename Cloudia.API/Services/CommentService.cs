using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cloudia.API.Services
{
    public class CommentService : ICommentService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<PostService> _logger;

        public CommentService(IApplicationContext context, ILogger<PostService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Comment?> GetComment(int id)
        {
            return  await _context.Comments.FromSql($"SELECT * FROM comments WHERE id = {id}").FirstOrDefaultAsync();
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

        public async Task<Comment> AddComment(int userId, int postId, string textContent)
        {
            var userProfile = GetUserProfile(userId)!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT * FROM add_comment(@user_profile_id_new, @post_id_new, @text_content_new)", connection);
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@user_profile_id_new", userProfile.id);
            command.Parameters.AddWithValue("@post_id_new", postId);
            command.Parameters.AddWithValue("@text_content_new", textContent);

            var id = (int) (await command.ExecuteScalarAsync())!;

            await connection.CloseAsync();

            return (await GetComment(id))!;
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL delete_comment(@comment_id)", connection);
            command.Parameters.AddWithValue("@comment_id", commentId);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return true;
        }

        public async Task<Comment> UpdateComment(int commentId, string newTextContent)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL update_comment(@comment_id, @text_content_new)", connection);
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@comment_id", commentId);
            command.Parameters.AddWithValue("@text_content_new", newTextContent);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return (await GetComment(commentId))!;
        }
    }
}
