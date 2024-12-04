using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> RegisterUser(string email, string password, string firstName, string lastName);

        public bool AuthenticateUser(string email, string password);
        public Task<User?> UserExists(string email);

    }
}
