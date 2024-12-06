using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface ISearchService
    {
        public Task<List<UserProfile>> SearchUsersGeneral(string query);
        public Task<List<UserProfile>> SearchUsersByName(string query);
        public Task<List<UserProfile>> SearchUsersByLocation(string query);
    }
}
