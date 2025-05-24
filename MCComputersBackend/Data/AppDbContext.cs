using Microsoft.EntityFrameworkCore;
using MCComputersBackend.Models;

namespace MCComputersBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // Configure Invoice entity
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.BalanceAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.HasMany(e => e.Items)
                      .WithOne()
                      .HasForeignKey("InvoiceId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure InvoiceItem entity
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Product>()
                      .WithMany()
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed some sample products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Dell Laptop XPS 13",
                    Description = "High-performance ultrabook with Intel Core i7",
                    Price = 1299.99m,
                    StockQuantity = 50,
                    Category = "Laptops",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Name = "HP Desktop PC",
                    Description = "Powerful desktop computer for office use",
                    Price = 899.99m,
                    StockQuantity = 30,
                    Category = "Desktops",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 3,
                    Name = "Samsung Monitor 27\"",
                    Description = "4K UHD monitor with excellent color accuracy",
                    Price = 349.99m,
                    StockQuantity = 75,
                    Category = "Monitors",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 4,
                    Name = "Logitech Wireless Mouse",
                    Description = "Ergonomic wireless mouse with precision tracking",
                    Price = 29.99m,
                    StockQuantity = 200,
                    Category = "Accessories",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 5,
                    Name = "Mechanical Keyboard RGB",
                    Description = "Gaming mechanical keyboard with RGB lighting",
                    Price = 129.99m,
                    StockQuantity = 100,
                    Category = "Accessories",
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
