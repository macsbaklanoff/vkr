using Microsoft.EntityFrameworkCore;
using VKR_server.DB.Entities;

namespace VKR_server.DB
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=VKR_database;username=postgres;Password=181318");
        }
    }
}
