using System;

using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
  public class NorthwindContext : DbContext
    {
    public NorthwindContext()
    {
      // Ensure the database is created so that model seeding (HasData) is applied
      // This is safe for in-memory provider and will be a no-op for other providers
      Database.EnsureCreated();
    }

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

        // Allow tests or developers to override the connection via the NORTHWIND_CONNECTION
        // environment variable. If it's not set, fall back to an in-memory database with
        // seeded data so tests can run without a local PostgreSQL instance.
        var conn = Environment.GetEnvironmentVariable("NORTHWIND_CONNECTION");
        if (!string.IsNullOrEmpty(conn))
        {
          optionsBuilder.UseNpgsql(conn);
        }
        else
        {
          optionsBuilder.UseInMemoryDatabase("NorthwindInMemory");
        }
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Category
      modelBuilder.Entity<Category>().ToTable("categories");
      modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");
      modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
      modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");

    // Seed categories so tests can run without a real database connection.
    modelBuilder.Entity<Category>().HasData(
      new Category { Id = 1, Name = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
      new Category { Id = 2, Name = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
      new Category { Id = 3, Name = "Confections", Description = "Desserts, candies, and sweet breads" },
      new Category { Id = 4, Name = "Dairy Products", Description = "Cheeses" },
      new Category { Id = 5, Name = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" },
      new Category { Id = 6, Name = "Meat/Poultry", Description = "Prepared meats" },
      new Category { Id = 7, Name = "Produce", Description = "Dried fruit and bean curd" },
      new Category { Id = 8, Name = "Seafood", Description = "Seaweed and fish" }
    );

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