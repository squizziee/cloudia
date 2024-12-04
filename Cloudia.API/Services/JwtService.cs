using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cloudia.API.Services
{

    public class JwtService : IJwtService
	{
        private readonly IApplicationContext _context;
        private readonly ILogger<JwtService> _logger;

        private static readonly SymmetricSecurityKey _secretKey = 
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes("92nf8i3bfb383fb383dasdadfgbkgsduhfgquwkefqywgefiq3wy8ergfvqo8gbver87fg78fv1b238fg478f8"));

		public JwtService(IApplicationContext context, ILogger<JwtService> logger)
		{
            this._context = context;
            this._logger = logger;
        }


		public string GenerateToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			_logger.Log(LogLevel.Warning, $"Email: {user.email}");
			var claims = new List<Claim>
		{
            new Claim("_email", user.email), 
            new Claim("name", user.id.ToString()),
			new Claim(ClaimTypes.Role, user.role_id.ToString())
		};

			var token = new JwtSecurityToken(
				issuer: "Cloudia Api Server",
				audience: "any",
				claims: claims,
				expires: DateTime.UtcNow.AddDays(30),
				signingCredentials: new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256)
			);

			return tokenHandler.WriteToken(token);
		}
	}
}
