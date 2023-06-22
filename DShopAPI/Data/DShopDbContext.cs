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

        public DShopDbContext(DbContextOptions<DShopDbContext> options) : base(options)
        {
        }
    }
}
