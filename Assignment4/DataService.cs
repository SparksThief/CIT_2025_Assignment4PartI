using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    /// <summary>
    /// Data service implementing the IDataService interface for accessing Northwind database.
    /// Provides CRUD operations for Categories, Products, Orders, and OrderDetails.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly NorthwindContext _context;

        /// <summary>
        /// Constructor used by Web API with Dependency Injection.
        /// DI container injects a scoped DbContext instance.
        /// </summary>
        public DataService(NorthwindContext context) => _context = context;

        /// <summary>
        /// Parameterless constructor for tests and standalone usage.
        /// Creates a new NorthwindContext which falls back to OnConfiguring for connection setup.
        /// </summary>
        public DataService() => _context = new NorthwindContext();


        #region Categories

        /// <summary>
        /// Retrieves all categories from the database, ordered by ID.
        /// </summary>
        /// <returns>List of all Category objects</returns>
        public List<Category> GetCategories() =>
            _context.Categories.OrderBy(c => c.Id).ToList();

        /// <summary>
        /// Retrieves a single category by its ID.
        /// </summary>
        /// <param name="id">The category ID to search for</param>
        /// <returns>Category object if found, null otherwise</returns>
        public Category GetCategory(int id) =>
            _context.Categories.SingleOrDefault(c => c.Id == id);

        /// <summary>
        /// Creates a new category in the database.
        /// Note: The Northwind database doesn't have auto-increment configured for categoryid,
        /// so we manually generate the next ID by finding the current max and adding 1.
        /// </summary>
        /// <param name="name">Category name</param>
        /// <param name="description">Category description</param>
        /// <returns>The newly created Category object with its assigned ID</returns>
        public Category CreateCategory(string name, string description)
        {
            // Get the next categoryid by finding the max and adding 1
            // Using (int?) allows Max to return null if no categories exist, which we coalesce to 0
            var maxId = _context.Categories.Max(c => (int?)c.Id) ?? 0;
            var c = new Category { Id = maxId + 1, Name = name, Description = description };
            _context.Categories.Add(c);
            _context.SaveChanges();
            return c;
        }

        /// <summary>
        /// Updates an existing category's name and description.
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="name">New category name</param>
        /// <param name="description">New category description</param>
        /// <returns>True if category was found and updated, false if not found</returns>
        public bool UpdateCategory(int id, string name, string description)
        {
            var c = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (c is null) return false;
            c.Name = name;
            c.Description = description;
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Deletes a category from the database.
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>True if category was found and deleted, false if not found</returns>
        public bool DeleteCategory(int id)
        {
            var c = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (c is null) return false;
            _context.Categories.Remove(c);
            _context.SaveChanges();
            return true;
        }

        #endregion

        #region Products

        /// <summary>
        /// Retrieves a single product by ID with its associated Category eagerly loaded.
        /// Uses Include to perform a JOIN and populate the Category navigation property.
        /// </summary>
        /// <param name="id">The product ID to search for</param>
        /// <returns>Product object with Category populated, or null if not found</returns>
        public Product GetProduct(int id) =>
            _context.Products
                    .Include(p => p.Category)
                    .SingleOrDefault(p => p.Id == id);

        /// <summary>
        /// Searches for products by name using case-insensitive pattern matching.
        /// Returns products where the name contains the specified substring (case-insensitive).
        /// Uses EF.Functions.ILike for PostgreSQL case-insensitive LIKE (ILIKE).
        /// </summary>
        /// <param name="substring">The substring to search for in product names</param>
        /// <returns>List of matching products with their Category populated, ordered by ID</returns>
        public List<Product> GetProductByName(string substring) =>
            _context.Products
                    .Include(p => p.Category)
                    .Where(p => EF.Functions.ILike(p.Name, $"%{substring}%"))
                    .OrderBy(p => p.Id)
                    .ToList();

        /// <summary>
        /// Retrieves all products belonging to a specific category.
        /// Eagerly loads the Category navigation property for each product.
        /// </summary>
        /// <param name="categoryId">The category ID to filter by</param>
        /// <returns>List of products in the specified category with Category populated, ordered by ID</returns>
        public List<Product> GetProductByCategory(int categoryId) =>
            _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId)
                    .OrderBy(p => p.Id)
                    .ToList();

        #endregion

        #region Orders & OrderDetails

        /// <summary>
        /// Retrieves a complete order by ID with all related data eagerly loaded.
        /// Includes: OrderDetails -> Product -> Category (using ThenInclude for nested relationships).
        /// This creates a fully populated object graph for displaying order information.
        /// </summary>
        /// <param name="id">The order ID to retrieve</param>
        /// <returns>Order object with complete hierarchy, or null if not found</returns>
        public Order GetOrder(int id) =>
            _context.Orders
                    .Where(o => o.Id == id) // Order.Id is mapped to orderid column in OnModelCreating
                    .Include(o => o.OrderDetails)           // Load all order details
                        .ThenInclude(od => od.Product)      // For each order detail, load the product
                            .ThenInclude(p => p.Category)   // For each product, load its category
                    .SingleOrDefault();

        /// <summary>
        /// Retrieves all orders with their complete hierarchy of related data.
        /// This creates a large object graph - use with caution in production scenarios.
        /// </summary>
        /// <returns>List of all orders with OrderDetails -> Product -> Category eagerly loaded</returns>
        public List<Order> GetOrders() =>
            _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.Category)
                    .OrderBy(o => o.Id)
                    .ToList();

        /// <summary>
        /// Retrieves all order details for a specific order.
        /// Includes the Product and its Category for each order detail line.
        /// Useful for displaying what products were ordered in a specific order.
        /// </summary>
        /// <param name="orderId">The order ID to retrieve details for</param>
        /// <returns>List of order details with Product and Category loaded, ordered by ProductId</returns>
        public List<OrderDetails> GetOrderDetailsByOrderId(int orderId) =>
            _context.OrderDetails
                    .Where(od => od.OrderId == orderId)
                    .Include(od => od.Product)
                        .ThenInclude(p => p.Category)
                    .OrderBy(od => od.ProductId)
                    .ToList();

        /// <summary>
        /// Retrieves all order details for a specific product (order history for that product).
        /// Includes the Order information for each order detail.
        /// </summary>
        /// <param name="productId">The product ID to retrieve order history for</param>
        /// <returns>List of order details with Order loaded, ordered by price (desc) then date (asc)</returns>
        public List<OrderDetails> GetOrderDetailsByProductId(int productId) =>
            _context.OrderDetails
                    .Where(od => od.ProductId == productId)
                    .Include(od => od.Order)
                    .ToList();

        #endregion
    }
}
