using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cloudia.API.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<UserProfileService> _logger;
        private readonly IPostService _postService;
        private readonly IWebHostEnvironment _environment;

        public UserProfileService(IApplicationContext context, ILogger<UserProfileService> logger, IPostService postService, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._logger = logger;
            this._postService = postService;
            this._environment = webHostEnvironment;
        }

        private async Task<string> SaveAvatar(IFormFile attachment)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "img");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"file{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";

            _logger.LogWarning($"New file: {uniqueFileName}");

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            _logger.LogWarning($"New file path: {filePath}");

            var fileInfo = new FileInfo(filePath);
            using var fileStream = fileInfo.Create();
            await attachment.CopyToAsync(fileStream);

            return $"https://localhost:5001/img/{uniqueFileName}";
        }

        public async Task<List<(Post post, List<PostAttachment>? attachments, List<Comment>? comments, List<Like>? likes)>> GetFeed(int userId)
        {
            var profile = (await GetUserProfile(userId))!;

            var result = new List<(Post post, List<PostAttachment>? attachments, List<Comment>? comments, List<Like>? likes)>();

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT posts.id FROM (subscribers JOIN user_profiles ON subscribers.subscriber_id = user_profiles.id) JOIN posts ON posts.user_profile_id = user_profiles.id WHERE subscription_id = @profileId", connection);

            command.Parameters.AddWithValue("@profileId", profile.id);

            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var postId = reader.GetInt32(0);
                result.Add((await _postService.GetFullPost(postId))!);
            }

            await connection.CloseAsync();

            return result;
        }

        public async Task<List<UserProfile>> GetSubscribers(int userId)
        {
            var profile = (await GetUserProfile(userId))!;
            return await _context.UserProfiles
                .FromSql($"SELECT user_profiles.id, user_id, role_id, ban_status_id, first_name, last_name, avatar_url, location, biography, age, created_at FROM subscribers JOIN user_profiles ON subscribers.subscriber_id = user_profiles.id WHERE subscription_id = {profile.id}").ToListAsync();
        }

        public async Task<List<UserProfile>> GetSubscriptions(int userId)
        {
            var profile = (await GetUserProfile(userId))!;
            return await _context.UserProfiles
                .FromSql($"SELECT user_profiles.id, user_id, role_id, ban_status_id, first_name, last_name, avatar_url, location, biography, age, created_at FROM subscribers JOIN user_profiles ON subscribers.subscription_id = user_profiles.id WHERE subscriber_id = {profile.id}").ToListAsync();
        }

        public async Task<UserProfile?> GetUserProfile(int userId)
        {
            var user = _context.Users.FromSql($"SELECT * FROM users WHERE id = {userId}").FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return await _context.UserProfiles.FromSql($"SELECT * FROM user_profiles WHERE id = {user.user_profile_id}").FirstOrDefaultAsync();
        }

        public async Task<bool> SubscribeTo(int userId, int subscribeToProfileId)
        {
            var subscriberProfile = (await GetUserProfile(userId))!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL subscribe(@subscriber_id_new, @subscription_id_new)", connection);

            command.Parameters.AddWithValue("@subscriber_id_new", subscriberProfile.id);
            command.Parameters.AddWithValue("@subscription_id_new", subscribeToProfileId);

            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return true;
        }

        public async Task<bool> UnsubscribeFrom(int userId, int unsubscribeFromProfileId)
        {
            var subscriberProfile = (await GetUserProfile(userId))!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL unsubscribe(@subscriber_id_new, @subscription_id_new)", connection);

            command.Parameters.AddWithValue("@subscriber_id_new", subscriberProfile.id);
            command.Parameters.AddWithValue("@subscription_id_new", unsubscribeFromProfileId);

            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return true;
        }

        public async Task<UserProfile> UpdateUserProfile(int userId, string firstName, string lastName, IFormFile? avatar, string? location, string? biography, int? age)
        {
            string avatarUrl = "";
            if (avatar != null)
            {
                avatarUrl = await SaveAvatar(avatar);
            }

            var profile = (await GetUserProfile(userId))!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL update_user_profile(@user_profile_id, @first_name_new, @last_name_new, @avatar_url_new, @location_new, @biography_new, @age_new)", connection);

            command.Parameters.AddWithValue("@user_profile_id", profile.id);
            command.Parameters.AddWithValue("@first_name_new", firstName);
            command.Parameters.AddWithValue("@last_name_new", lastName);
            command.Parameters.AddWithValue("@avatar_url_new", avatarUrl);
            command.Parameters.AddWithValue("@location_new", location ?? "");
            command.Parameters.AddWithValue("@biography_new", biography ?? "");
            command.Parameters.AddWithValue("@age_new", age ?? -1);

            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return (await GetUserProfile(userId))!;
        }
    }
}
