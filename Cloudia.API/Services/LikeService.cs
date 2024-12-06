using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cloudia.API.Services
{
    public class LikeService : ILikeService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<PostService> _logger;

        public LikeService(IApplicationContext context, ILogger<PostService> logger)
        {
            this._context = context;
            this._logger = logger;
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

        public async Task<bool> AddLike(int userId, int postId)
        {
            var userProfile = GetUserProfile(userId)!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL add_like(@user_profile_id_new, @post_id_new)", connection);
            command.Parameters.AddWithValue("@user_profile_id_new", userProfile.id);
            command.Parameters.AddWithValue("@post_id_new", postId);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return true;
        }

        public async Task<bool> RemoveLike(int userId, int postId)
        {
            var userProfile = GetUserProfile(userId)!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL remove_like(@user_profile_id_, @post_id_)", connection);
            command.Parameters.AddWithValue("@user_profile_id_", userProfile.id);
            command.Parameters.AddWithValue("@post_id_", postId);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return true;
        }
    }
}