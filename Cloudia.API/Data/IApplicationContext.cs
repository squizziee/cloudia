using Cloudia.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cloudia.API.Data
{
    public interface IApplicationContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BanStatus> BanStatuses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Violation> Violations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public string? GetConnectionString();
    }
}
