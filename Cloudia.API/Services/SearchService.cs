using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cloudia.API.Services
{
    public class SearchService : ISearchService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<ViolationService> _logger;
        private readonly IUserProfileService _userProfileService;

        public SearchService(IApplicationContext context, ILogger<ViolationService> logger, IUserProfileService userProfileService)
        {
            this._context = context;
            this._logger = logger;
            this._userProfileService = userProfileService;
        }

        public async Task<List<UserProfile>> SearchUsersByLocation(string query)
        {
            return await _context.UserProfiles.FromSqlRaw($"SELECT * FROM user_profiles WHERE location ILIKE '%{query}%'").ToListAsync();       
        }

        public async Task<List<UserProfile>> SearchUsersByName(string query)
        {
            return await _context.UserProfiles.FromSqlRaw($"SELECT * FROM user_profiles WHERE CONCAT(first_name, ' ', last_name) ILIKE '%{query}%'").ToListAsync();
        }

        public async Task<List<UserProfile>> SearchUsersGeneral(string query)
        {
            return await _context.UserProfiles.FromSqlRaw($"SELECT * FROM user_profiles WHERE biography ILIKE '%{query}%'").ToListAsync();
        }
    }
}
