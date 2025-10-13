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
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseNpgsql("host=localhost;db=northwind;uid=postgres;pwd=@ccess93");
      optionsBuilder.EnableSensitiveDataLogging();
      optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Category
      modelBuilder.Entity<Category>().ToTable("categories");
      modelBuilder.Entity<Category>().HasKey(x => x.Id);
      modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");
      modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
      modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");

      // Product
      modelBuilder.Entity<Product>().ToTable("products");
      modelBuilder.Entity<Product>().HasKey(x => x.Id);
      modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
      modelBuilder.Entity<Product>().Property(x => x.name).HasColumnName("productname");
      modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
      modelBuilder.Entity<Product>().Property(x => x.UnitsInStock).HasColumnName("unitsinstock");
      modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
      modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");

      // Order 
      modelBuilder.Entity<Order>().ToTable("orders");
      modelBuilder.Entity<Order>().HasKey(x => x.Id);
      modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("orderid");
      modelBuilder.Entity<Order>().Property(x => x.OrderDate).HasColumnName("orderdate");
      modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
      modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");

      // OrderDetail
      modelBuilder.Entity<OrderDetail>().ToTable("order_details");
      modelBuilder.Entity<OrderDetail>().HasKey(x => new { x.OrderId, x.ProductId });
      modelBuilder.Entity<OrderDetail>().Property(x => x.OrderId).HasColumnName("orderid");
      modelBuilder.Entity<OrderDetail>().Property(x => x.ProductId).HasColumnName("productid");
      modelBuilder.Entity<OrderDetail>().Property(x => x.UnitPrice).HasColumnName("unitprice");
      modelBuilder.Entity<OrderDetail>().Property(x => x.Quantity).HasColumnName("quantity");
      modelBuilder.Entity<OrderDetail>().Property(x => x.Discount).HasColumnName("discount");

    }
        
    }
}