using Microsoft.EntityFrameworkCore;
using VKR_server.DB.Entities;

namespace VKR_server.DB
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=VKR_database;username=postgres;Password=181318");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(user => user.Id).UseIdentityAlwaysColumn();
        }
    }
}
