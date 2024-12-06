using Cloudia.API.Data;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cloudia.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AdminService> _logger;
        private readonly IUserProfileService _userProfileService;

        public AdminService(IApplicationContext context, ILogger<AdminService> logger, IUserProfileService userProfileService)
        {
            this._context = context;
            this._logger = logger;
            this._userProfileService = userProfileService;
        }

        public async Task<bool> BanUser(int userId)
        {
            var profile = (await _userProfileService.GetUserProfile(userId))!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL ban_user(@ban_receiver_profile_id)", connection);

            command.Parameters.AddWithValue("@ban_receiver_profile_id", profile.id);

            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return true;
        }

        public async Task<bool> UnbanUser(int userId)
        {
            var profile = (await _userProfileService.GetUserProfile(userId))!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL unban_user(@unban_receiver_profile_id)", connection);

            command.Parameters.AddWithValue("@unban_receiver_profile_id", profile.id);

            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return true;
        }
    }
}
