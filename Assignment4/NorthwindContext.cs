using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
  public class NorthwindContext : DbContext
  {
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Employees> Employees { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<Suppliers> Suppliers { get; set; }

  
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
      modelBuilder.Entity<Category>().Property(x => x.Id)
        .HasColumnName("categoryid")
        .ValueGeneratedOnAdd();
      modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
      modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");

      modelBuilder.Entity<Customers>().ToTable("customers");
      modelBuilder.Entity<Customers>().HasKey(x => x.Id);
      modelBuilder.Entity<Customers>().Property(x => x.Id).HasColumnName("customerid");
      modelBuilder.Entity<Customers>().Property(x => x.Name).HasColumnName("companyname");
      modelBuilder.Entity<Customers>().Property(x => x.Title).HasColumnName("contacttitle");
      modelBuilder.Entity<Customers>().Property(x => x.Address).HasColumnName("address");
      modelBuilder.Entity<Customers>().Property(x => x.City).HasColumnName("city");
      modelBuilder.Entity<Customers>().Property(x => x.PostalCode).HasColumnName("postalcode");
      modelBuilder.Entity<Customers>().Property(x => x.Country).HasColumnName("country");
      modelBuilder.Entity<Customers>().Property(x => x.Phone).HasColumnName("phone");
      modelBuilder.Entity<Customers>().Property(x => x.Fax).HasColumnName("fax");

      // Employees
      modelBuilder.Entity<Employees>().ToTable("employees");
      modelBuilder.Entity<Employees>().HasKey(x => x.Id);
      modelBuilder.Entity<Employees>().Property(x => x.Id).HasColumnName("employeeid");
      modelBuilder.Entity<Employees>().Property(x => x.LastName).HasColumnName("lastname");
      modelBuilder.Entity<Employees>().Property(x => x.FirstName).HasColumnName("firstname");
      modelBuilder.Entity<Employees>().Property(x => x.Title).HasColumnName("title");
      modelBuilder.Entity<Employees>().Property(x => x.BirthDate).HasColumnName("birthdate");
      modelBuilder.Entity<Employees>().Property(x => x.HireDate).HasColumnName("hiredate");
      modelBuilder.Entity<Employees>().Property(x => x.Address).HasColumnName("address");
      modelBuilder.Entity<Employees>().Property(x => x.City).HasColumnName("city");
      modelBuilder.Entity<Employees>().Property(x => x.PostalCode).HasColumnName("postalcode");
      modelBuilder.Entity<Employees>().Property(x => x.Country).HasColumnName("country");

      // Orders
      modelBuilder.Entity<Order>().ToTable("orders");
      modelBuilder.Entity<Order>().HasKey(x => x.OrderId);
      modelBuilder.Entity<Order>().Property(x => x.OrderId).HasColumnName("orderid");
      modelBuilder.Entity<Order>().Property(x => x.CustomerId).HasColumnName("customerid");
      modelBuilder.Entity<Order>().Property(x => x.EmployeeId).HasColumnName("employeeid");
      modelBuilder.Entity<Order>().Property(x => x.OrderDate).HasColumnName("orderdate");
      modelBuilder.Entity<Order>().Property(x => x.RequiredDate).HasColumnName("requireddate");
      modelBuilder.Entity<Order>().Property(x => x.ShippedDate).HasColumnName("shippeddate");
      modelBuilder.Entity<Order>().Property(x => x.Freight).HasColumnName("freight");
      modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
      modelBuilder.Entity<Order>().Property(x => x.ShipAddress).HasColumnName("shipaddress");
      modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");
      modelBuilder.Entity<Order>().Property(x => x.ShipPostalCode).HasColumnName("shippostalcode");
      modelBuilder.Entity<Order>().Property(x => x.ShipCountry).HasColumnName("shipcountry");

      // OrderDetails
      modelBuilder.Entity<OrderDetails>().ToTable("orderdetails");
      modelBuilder.Entity<OrderDetails>().HasKey(x => new { x.OrderId, x.ProductId });
      modelBuilder.Entity<OrderDetails>().Property(x => x.OrderId).HasColumnName("orderid");
      modelBuilder.Entity<OrderDetails>().Property(x => x.ProductId).HasColumnName("productid");
      modelBuilder.Entity<OrderDetails>().Property(x => x.UnitPrice).HasColumnName("unitprice");
      modelBuilder.Entity<OrderDetails>().Property(x => x.Quantity).HasColumnName("quantity");
      modelBuilder.Entity<OrderDetails>().Property(x => x.Discount).HasColumnName("discount");

      // Product
      modelBuilder.Entity<Product>().ToTable("products");
      modelBuilder.Entity<Product>().HasKey(x => x.Id);
      modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
      modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("productname");
      modelBuilder.Entity<Product>().Property(x => x.SupplierId).HasColumnName("supplierid");
      modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");
      modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
      modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
      modelBuilder.Entity<Product>().Property(x => x.UnitsInStock).HasColumnName("unitsinstock");

      // Suppliers
      modelBuilder.Entity<Suppliers>().ToTable("suppliers");
      modelBuilder.Entity<Suppliers>().HasKey(x => x.Id);
      modelBuilder.Entity<Suppliers>().Property(x => x.Id).HasColumnName("supplierid");
      modelBuilder.Entity<Suppliers>().Property(x => x.CompanyName).HasColumnName("companyname");
      modelBuilder.Entity<Suppliers>().Property(x => x.ContactName).HasColumnName("contactname");
      modelBuilder.Entity<Suppliers>().Property(x => x.ContactTitle).HasColumnName("contacttitle");
      modelBuilder.Entity<Suppliers>().Property(x => x.Address).HasColumnName("address");
      modelBuilder.Entity<Suppliers>().Property(x => x.City).HasColumnName("city");
      modelBuilder.Entity<Suppliers>().Property(x => x.PostalCode).HasColumnName("postalcode");
      modelBuilder.Entity<Suppliers>().Property(x => x.Country).HasColumnName("country");
      modelBuilder.Entity<Suppliers>().Property(x => x.Phone).HasColumnName("phone");
      modelBuilder.Entity<Suppliers>().Property(x => x.Fax).HasColumnName("fax");

  
    }
        
    }
}
