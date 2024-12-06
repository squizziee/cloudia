using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Xml.Linq;

namespace Cloudia.API.Services
{
    public class ViolationService : IViolationService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<ViolationService> _logger;
        private readonly IUserProfileService _userProfileService;

        public ViolationService(IApplicationContext context, ILogger<ViolationService> logger, IUserProfileService userProfileService)
        {
            this._context = context;
            this._logger = logger;
            this._userProfileService = userProfileService;
        }

        public async Task<Violation> AddViolation(string name, string description, int banDays)
        {

            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT * FROM add_violation(@name_new, @description_new, @ban_days_new)", connection);

            command.Parameters.AddWithValue("@name_new", name);
            command.Parameters.AddWithValue("@description_new", description);
            command.Parameters.AddWithValue("@ban_days_new", banDays);

            var id = (int)(await command.ExecuteScalarAsync())!;

            await connection.CloseAsync();

            return (await GetViolation(id))!;
        }

        public async Task<bool> DeleteViolation(int id)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL delete_violation(@violation_id)", connection);

            command.Parameters.AddWithValue("@violation_id", id);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return true;
        }

        public async Task<Violation?> GetViolation(int id)
        {
            return await _context.Violations.FromSql($"SELECT * FROM violations WHERE id = {id}").FirstOrDefaultAsync();
        }

        public async Task<Violation> UpdateViolation(int violationId, string name, string description, int banDays)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("CALL update_violation(@violation_id, @name_new, @description_new, @ban_days_new)", connection);

            command.Parameters.AddWithValue("@violation_id", violationId);
            command.Parameters.AddWithValue("@name_new", name);
            command.Parameters.AddWithValue("@description_new", description);
            command.Parameters.AddWithValue("@ban_days_new", banDays);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return (await GetViolation(violationId))!;
        }
    }
}
