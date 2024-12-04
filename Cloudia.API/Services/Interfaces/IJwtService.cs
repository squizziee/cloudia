using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
        // public string ValidateToken(User user);
    }
}
