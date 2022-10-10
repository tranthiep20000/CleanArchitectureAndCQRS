using CwkSocial.DAL.Configurations;
using CwkSocial.DOMAIN.Aggregates.PostAggregate;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.DAL.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostCommentConfig());
            modelBuilder.ApplyConfiguration(new PostInteractionConfig());
            modelBuilder.ApplyConfiguration(new UserProfileConfig());
            modelBuilder.ApplyConfiguration(new IdentityUserLoginConfig());
            modelBuilder.ApplyConfiguration(new IdentityUserRoleConfig());
            modelBuilder.ApplyConfiguration(new IdentityUserTokenConfig());
        }
    }
}