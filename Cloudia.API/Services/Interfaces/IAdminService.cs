namespace Cloudia.API.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<bool> BanUser(int userId);
        public Task<bool> UnbanUser(int userId);

    }
}
