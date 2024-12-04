using Cloudia.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cloudia.API.Data
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        protected readonly IConfiguration Configuration;

        public ApplicationContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BanStatus> BanStatuses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Violation> Violations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public string? GetConnectionString()
        {
            return Configuration.GetConnectionString("WebApiDatabase");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }
    }
}
