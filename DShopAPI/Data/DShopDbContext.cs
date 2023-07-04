using DShopAPI.Interfaces;
using DShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DShopAPI.Data
{
    public class DShopDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DShopDbContext(DbContextOptions<DShopDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.CategoryItems)
                .WithOne(ci => ci.Category)
                .HasForeignKey(ci => ci.CategoryId);

            modelBuilder.Entity<CategoryItem>()
                .HasOne(ci => ci.Category)
                .WithMany(c => c.CategoryItems)
                .HasForeignKey(ci => ci.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.CategoryItem)
                .WithMany(ci => ci.Products)
                .HasForeignKey(p => p.CategoryItemId);

        }

    }

}
