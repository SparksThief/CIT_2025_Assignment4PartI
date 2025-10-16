using System;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    /// <summary>
    /// DbContext for the Northwind PostgreSQL database.
    /// Handles entity-to-table mappings and database configuration.
    /// Supports both dependency injection (Web API) and direct instantiation (tests).
    /// </summary>
    public class NorthwindContext : DbContext
    {
        /// <summary>
        /// Constructor for Dependency Injection scenarios (used by Web API).
        /// Options are configured externally (in Program.cs or Startup.cs).
        /// </summary>
        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options) { }

        /// <summary>
        /// Parameterless constructor for direct instantiation (used by tests and standalone usage).
        /// Connection configuration is handled in OnConfiguring method.
        /// </summary>
        public NorthwindContext() { }

        /// <summary>
        /// DbSet representing the Categories table.
        /// Provides LINQ queries and change tracking for Category entities.
        /// </summary>
        public DbSet<Category> Categories { get; set; }
        
        /// <summary>
        /// DbSet representing the Products table with navigation to Categories.
        /// </summary>
        public DbSet<Product> Products { get; set; }
        
        /// <summary>
        /// DbSet representing the Orders table with navigation to OrderDetails.
        /// </summary>
        public DbSet<Order> Orders { get; set; }
        
        /// <summary>
        /// DbSet representing the OrderDetails table (junction table between Orders and Products).
        /// </summary>
        public DbSet<OrderDetails> OrderDetails { get; set; }

        /// <summary>
        /// Configures the database connection when not using Dependency Injection.
        /// This method is called when using the parameterless constructor.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if not already configured by DI (avoids overriding DI settings)
            if (!optionsBuilder.IsConfigured)
            {
                // Prefer environment variable for CI/test flexibility, fall back to local dev connection
                var cs = Environment.GetEnvironmentVariable("NORTHWIND_CS")
                         ?? "Host=localhost;Port=5432;Database=northwind;Username=postgres;Password=@ccess93";
                optionsBuilder.UseNpgsql(cs);

                // Optional debugging: Uncomment to see generated SQL and parameter values
                // Useful for troubleshooting EF queries and understanding database interaction
                // optionsBuilder.EnableSensitiveDataLogging();
                // optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

        /// <summary>
        /// Configures entity-to-database mappings using Fluent API.
        /// Maps C# property names (PascalCase) to PostgreSQL column names (lowercase).
        /// Defines primary keys, foreign keys, and relationships between entities.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========================================
            // CATEGORY ENTITY CONFIGURATION
            // ========================================
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            
            // Important: ValueGeneratedNever() because the database doesn't have auto-increment
            // configured for categoryid. We manually generate IDs in CreateCategory().
            modelBuilder.Entity<Category>().Property(x => x.Id)
                  .HasColumnName("categoryid")
                  .ValueGeneratedNever(); // ID must be set manually, not auto-generated
                  
            modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
            modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");

            // ========================================
            // PRODUCT ENTITY CONFIGURATION
            // ========================================
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            
            // Map C# properties to database columns (PostgreSQL uses lowercase)
            modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
            modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("productname");
            modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");
            modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
            modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
            modelBuilder.Entity<Product>().Property(x => x.UnitsInStock).HasColumnName("unitsinstock");

            // Define one-to-many relationship: One Category has many Products
            // Product.Category navigation property allows accessing the parent category
            // Note: SupplierId property is marked [NotMapped] in the Product class
            // because it's not used in this assignment
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)    // Product has one Category
                .WithMany()                  // Category has many Products (no inverse navigation)
                .HasForeignKey(p => p.CategoryId);

            // ========================================
            // ORDER ENTITY CONFIGURATION
            // ========================================
            modelBuilder.Entity<Order>().ToTable("orders");
            
            // Important: Use 'Id' property as the key, which maps to 'orderid' column
            // The Order class has both Id and OrderId properties:
            // - Id: mapped to database, used by EF Core (this is the primary key)
            // - OrderId: marked [NotMapped], available for business logic if needed
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("orderid");
            
            // Map other order properties to their database columns
            modelBuilder.Entity<Order>().Property(x => x.CustomerId).HasColumnName("customerid");
            modelBuilder.Entity<Order>().Property(x => x.EmployeeId).HasColumnName("employeeid");
            modelBuilder.Entity<Order>().Property(x => x.OrderDate).HasColumnName("orderdate");
            modelBuilder.Entity<Order>().Property(x => x.RequiredDate).HasColumnName("requireddate");
            modelBuilder.Entity<Order>().Property(x => x.ShippedDate).HasColumnName("shippeddate");
            modelBuilder.Entity<Order>().Property(x => x.Freight).HasColumnName("freight");
            modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
            modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");
            
            // Note: ShipAddress, ShipPostalCode, and ShipCountry are marked [NotMapped]
            // in the Order class because they don't exist in the Northwind database schema
            

            // ========================================
            // ORDERDETAILS ENTITY CONFIGURATION
            // ========================================
            modelBuilder.Entity<OrderDetails>().ToTable("orderdetails");
            
            // Composite primary key: both OrderId and ProductId together form the key
            // This is a typical junction/association table pattern
            modelBuilder.Entity<OrderDetails>().HasKey(x => new { x.OrderId, x.ProductId });
            
            // Map properties to database columns
            modelBuilder.Entity<OrderDetails>().Property(x => x.OrderId).HasColumnName("orderid");
            modelBuilder.Entity<OrderDetails>().Property(x => x.ProductId).HasColumnName("productid");
            modelBuilder.Entity<OrderDetails>().Property(x => x.UnitPrice).HasColumnName("unitprice");
            modelBuilder.Entity<OrderDetails>().Property(x => x.Quantity).HasColumnName("quantity");
            modelBuilder.Entity<OrderDetails>().Property(x => x.Discount).HasColumnName("discount");

            // Define relationships for navigation properties
            // These enable EF Core to automatically load related data with Include()
            
            // OrderDetails -> Order relationship (many-to-one)
            // One OrderDetails belongs to one Order
            // One Order has many OrderDetails
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Order)              // OrderDetails has one Order
                .WithMany(o => o.OrderDetails)       // Order has many OrderDetails
                .HasForeignKey(od => od.OrderId);    // Foreign key is OrderId

            // OrderDetails -> Product relationship (many-to-one)
            // One OrderDetails refers to one Product
            // One Product can appear in many OrderDetails
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Product)            // OrderDetails has one Product
                .WithMany()                          // Product has many OrderDetails (no inverse navigation)
                .HasForeignKey(od => od.ProductId);  // Foreign key is ProductId
        }
    }
}