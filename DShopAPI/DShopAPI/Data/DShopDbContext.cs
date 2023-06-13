using DShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DShopAPI.Data
{
    public class DShopDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }

        public DShopDbContext(DbContextOptions<DShopDbContext> options) : base(options)
        {
        }
    }
}
