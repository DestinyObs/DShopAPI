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
        //public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }



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

            SeedShippingMethods(modelBuilder);
            //SeedPaymentMethods(modelBuilder);
        }
        private void SeedShippingMethods(ModelBuilder modelBuilder)
        {
            var shippingMethods = new[]
            {
        new ShippingMethod { Id = 1, Name = "Standard Shipping", Description = "Delivery within 5-7 business days", Price = 500.00m },
        new ShippingMethod { Id = 2, Name = "Express Shipping", Description = "Delivery within 2-3 business days", Price = 700.00m },
        // Add more shipping methods as needed
    };

            modelBuilder.Entity<ShippingMethod>().HasData(shippingMethods);
        }

    //    private void SeedPaymentMethods(ModelBuilder modelBuilder)
    //    {
    //        var paymentMethods = new[]
    //        {
    //    new PaymentMethod { PaymentMethodId = 1, Name = "Paystack" },
    //    new PaymentMethod { PaymentMethodId = 2, Name = "Flutterwave" },
    //    new PaymentMethod { PaymentMethodId = 3, Name = "VoguePay" },
    //    new PaymentMethod { PaymentMethodId = 4, Name = "Cashenvoy" },
    //    new PaymentMethod { PaymentMethodId = 5, Name = "PayU" },
    //    // Add more payment methods as needed
    //};

    //        modelBuilder.Entity<PaymentMethod>().HasData(paymentMethods);
    //    }


    }

}
