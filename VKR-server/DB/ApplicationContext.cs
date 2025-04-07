using Microsoft.EntityFrameworkCore;
using VKR_server.DB.Entities;

namespace VKR_server.DB
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=VKR_database;username=postgres;Password=181318");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //маппинг модели User
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("user_id");
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnName("first_name");
            modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnName("last_name");
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password_hash");
            modelBuilder.Entity<User>().Property(u => u.CreationDate).HasColumnName("created_at");
            modelBuilder.Entity<User>().Property(u => u.Role).HasColumnName("role_name");
        }
    }
}
