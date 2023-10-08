using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Models;

namespace SmartShopAPI.Data
{
    public class SmartShopDbContext : DbContext
    {
        public SmartShopDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(45);
                entity.Property(p => p.Price).IsRequired().HasPrecision(8,2);
            });
        }
    }
}
