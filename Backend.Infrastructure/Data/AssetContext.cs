using Backend.Domain.Entities;
using Backend.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Data
{
    public class AssetContext : DbContext
    {

        public AssetContext(DbContextOptions<AssetContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.StaffCode)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Asset)
                .WithMany()
                .HasForeignKey(a => a.AssetId);
            modelBuilder.Entity<Asset>()
               .HasOne(b => b.Category)
               .WithMany(c => c.Assets)
               .HasForeignKey(b => b.CategoryId);
            var users = new SeedUsersData().GenerateSeedData();
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}
