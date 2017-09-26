namespace Octopost.DataAccess.Context
{
    using Microsoft.EntityFrameworkCore;
    using Octopost.Model.Data;

    public class OctopostDbContext : DbContext
    {
        public OctopostDbContext(DbContextOptions<OctopostDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
