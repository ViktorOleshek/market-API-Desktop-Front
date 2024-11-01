using System;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext()
        {
        }

        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<Customer>(e =>
            {
                e.HasOne(c => c.Person)
                 .WithOne()
                 .HasForeignKey<Customer>(c => c.PersonId);

                e.HasMany(c => c.Receipts)
                 .WithOne(r => r.Customer)
                 .HasForeignKey(r => r.CustomerId);
            });

            modelBuilder.Entity<Person>(e =>
            {
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.HasOne(p => p.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(p => p.ProductCategoryId);

                e.HasMany(p => p.ReceiptDetails)
                 .WithOne(rd => rd.Product)
                 .HasForeignKey(rd => rd.ProductId);

                e.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ProductCategory>(e =>
            {
            });

            modelBuilder.Entity<Receipt>(e =>
            {
                e.HasOne(r => r.Customer)
                 .WithMany(c => c.Receipts)
                 .HasForeignKey(r => r.CustomerId);

                e.HasMany(r => r.ReceiptDetails)
                 .WithOne(rd => rd.Receipt)
                 .HasForeignKey(rd => rd.ReceiptId);
            });

            modelBuilder.Entity<ReceiptDetail>(e =>
            {
                e.HasOne(rd => rd.Receipt)
                 .WithMany(r => r.ReceiptDetails)
                 .HasForeignKey(rd => rd.ReceiptId);

                e.HasOne(rd => rd.Product)
                 .WithMany(p => p.ReceiptDetails)
                 .HasForeignKey(rd => rd.ProductId);

                e.Property(rd => rd.DiscountUnitPrice).HasColumnType("decimal(18,2)");
                e.Property(rd => rd.UnitPrice).HasColumnType("decimal(18,2)");
            });
        }
    }
}
