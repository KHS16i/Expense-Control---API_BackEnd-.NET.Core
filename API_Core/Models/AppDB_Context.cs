using Microsoft.EntityFrameworkCore;

namespace API_Core.Models
{
    public class AppDB_Context : DbContext
    {
        public DbSet <Records> Records { get; set; }

        public DbSet <Users> Users { get; set; }

        public AppDB_Context(DbContextOptions<AppDB_Context> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
            .HasMany(u => u.Records)
            .WithOne(r => r.Users)
            .HasForeignKey(r => r.UserId);
        }
    }
}
