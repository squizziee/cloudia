using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cloudia.API.Services
{
    public class UserService : IUserService
	{
		private readonly IApplicationContext _context;
		private readonly ILogger<PostService> _logger;

		public UserService(IApplicationContext context, ILogger<PostService> logger)
		{
			this._context = context;
			this._logger = logger;
		}


		public async Task<User> RegisterUser(string email, string password, string firstName, string lastName)
		{
			using var connection = new NpgsqlConnection(_context.GetConnectionString());
			await connection.OpenAsync();

			var command = new NpgsqlCommand("register_user", connection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};
			command.Parameters.AddWithValue("email_new", email);
			command.Parameters.AddWithValue("password_hash", password);
			command.Parameters.AddWithValue("first_name_new", firstName);
			command.Parameters.AddWithValue("last_name_new", lastName);

			var id = (int?) await command.ExecuteScalarAsync();

			await connection.CloseAsync();

			var newlyRegistered = _context.Users.FromSql($"SELECT * FROM users WHERE id = {id}").FirstOrDefault();

			return newlyRegistered!;
		}

		public bool AuthenticateUser(string email, string password)
		{
			var user = _context.Users.FromSql($"SELECT * FROM users WHERE email = {email}").FirstOrDefault();

			if (user == null)
			{
				return false;
			}
            _logger.Log(LogLevel.Warning, (password.GetHashCode() << 2 | 0b01001101).ToString());
            _logger.Log(LogLevel.Warning, user.password_hash);
            return password == user.password_hash;
		}

		public async Task<User?> UserExists(string email)
		{

			var result =
				await _context.Users.FromSqlRaw($"SELECT * FROM users WHERE email = '{email}'").FirstOrDefaultAsync();
			_logger.Log(LogLevel.Warning, $"Executed: SELECT * FROM users WHERE email = '{email}'");
			_logger.Log(LogLevel.Warning, result == null ? "No result" : result.ToString());
			return result;
		}
	}
}
