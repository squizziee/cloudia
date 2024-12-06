using Cloudia.API.Data;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cloudia.API.Services
{
    public class ModeratorService : IModeratorService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<PostService> _logger;

        public ModeratorService(
                IApplicationContext context,
                ILogger<PostService> logger)
        {
            this._context = context;
            this._logger = logger;
        }
        public async Task<bool> DeletePost(int id)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            var command = new NpgsqlCommand("delete_post", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("post_id", id);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return true;
        }
    }
}
