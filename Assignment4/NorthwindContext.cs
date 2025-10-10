using System;

using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
  public class NorthwindContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    // If no options are given, configure it manually

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseNpgsql("host=localhost;db=northwind;uid=postgres;pwd=admin");
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Category
      modelBuilder.Entity<Category>().ToTable("categories");
      modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");
      modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
      modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");

      // Product
      modelBuilder.Entity<Product>().ToTable("products");
      modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
      modelBuilder.Entity<Product>().Property(x => x.name).HasColumnName("productname");
      modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
      modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");

      // Relationship between Product and Category
      modelBuilder.Entity<Category>()
          .HasMany<Product>()
          .WithOne(p => p.Category)
          .HasForeignKey(p => p.CategoryId);

      // Order
      modelBuilder.Entity<Order>().ToTable("orders");
      modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("orderid");
      modelBuilder.Entity<Order>().Property(x => x.OrderDate).HasColumnName("orderdate");
      modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
      modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");

      // OrderDetail
      modelBuilder.Entity<OrderDetail>().ToTable("order_details");
      modelBuilder.Entity<OrderDetail>().Property(x => x.OrderId).HasColumnName("orderid");
      modelBuilder.Entity<OrderDetail>().Property(x => x.ProductId).HasColumnName("productid");
      modelBuilder.Entity<OrderDetail>().Property(x => x.UnitPrice).HasColumnName("unitprice");
      modelBuilder.Entity<OrderDetail>().Property(x => x.Quantity).HasColumnName("quantity");
      modelBuilder.Entity<OrderDetail>().Property(x => x.Discount).HasColumnName("discount");

      // Composite key
      modelBuilder.Entity<OrderDetail>()
          .HasKey(od => new { od.OrderId, od.ProductId });

      // Relationship between Order and OrderDetail
      modelBuilder.Entity<Order>()
          .HasMany(o => o.OrderDetails)
          .WithOne(od => od.Order)
          .HasForeignKey(od => od.OrderId);

      modelBuilder.Entity<Product>()
          .HasMany<OrderDetail>()
          .WithOne(od => od.Product)
          .HasForeignKey(od => od.ProductId);
    }
        
    }
}