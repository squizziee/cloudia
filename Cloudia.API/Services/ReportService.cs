using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net.Mail;

namespace Cloudia.API.Services
{
    public class ReportService : IReportService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<PostService> _logger;

        public ReportService(IApplicationContext context, ILogger<PostService> logger)
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

        public async Task<Report> AddReport(int senderId, int postId, int violationId)
        {
            var senderProfile = GetUserProfile(senderId)!;

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT * FROM add_report(@sender_id_new, @post_id_new, @violation_id_new)", connection);

            command.Parameters.AddWithValue("@sender_id_new", senderProfile.id);
            command.Parameters.AddWithValue("@post_id_new", postId);
            command.Parameters.AddWithValue("@violation_id_new", violationId);

            var id = (int) (await command.ExecuteScalarAsync())!;
            await connection.CloseAsync();

            return (await GetReport(id))!;
        }

        public async Task<Report?> GetReport(int id)
        {
            return await _context.Reports.FromSql($"SELECT * FROM reports WHERE id = {id}").FirstOrDefaultAsync();
        }
    }
}
