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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<User>();
            //modelBuilder.Entity<User>().Ignore("RoleName1");
            //modelBuilder.Entity<User>().Ignore("RoleName2");
            foreach (var property in entity.Metadata.GetProperties())
            {
                Console.WriteLine($"Property: {property.Name}, Column: {property.GetColumnName()}");
            }
            //маппинг модели User
            #region
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("user_id");
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnName("first_name");
            modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnName("last_name");
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password_hash");
            modelBuilder.Entity<User>().Property(u => u.CreationDate).HasColumnName("created_at");
            modelBuilder.Entity<User>().Property(u => u.RoleName).HasColumnName("role_name");
            #endregion
            //маппинг модели Roles
            //modelBuilder.Entity<Role>().Property(r => r.RoleName).HasColumnName("role_name");

        }
    }
}
