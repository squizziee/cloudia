using Cloudia.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cloudia.API.Data
{
    public interface IApplicationContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public string? GetConnectionString();
    }
}
