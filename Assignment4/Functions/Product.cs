using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment4
{
    /// <summary>
    /// Represents a product in the Northwind database.
    /// Products belong to Categories and can appear in multiple OrderDetails.
    /// </summary>
    public partial class Product
    {
        /// <summary>
        /// Alias property for Name. Not mapped to database.
        /// Provides alternative property name for API compatibility or legacy code support.
        /// </summary>
        [NotMapped]
        public string ProductName
        {
            get => Name;
            set => Name = value;
        }
        
        /// <summary>
        /// Primary key. Maps to 'productid' column.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Product name (e.g., "Chai", "Chang").
        /// Maps to 'productname' column.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Supplier ID - NOT MAPPED to database in this assignment.
        /// The Northwind database has suppliers, but they're not used in this project.
        /// Marked [NotMapped] to prevent EF Core from trying to map it to a database column.
        /// </summary>
        [NotMapped]
        public int SupplierId { get; set; }
        
        /// <summary>
        /// Foreign key to Category table.
        /// Links this product to its category. Maps to 'categoryid' column.
        /// </summary>
        public int CategoryId { get; set; }
        
        /// <summary>
        /// Package size/quantity information (e.g., "10 boxes x 20 bags").
        /// Maps to 'quantityperunit' column.
        /// </summary>
        public string QuantityPerUnit { get; set; }
        
        /// <summary>
        /// Price per unit of the product.
        /// Maps to 'unitprice' column.
        /// </summary>
        public decimal UnitPrice { get; set; }
        
        /// <summary>
        /// Current inventory level - how many units are in stock.
        /// Maps to 'unitsinstock' column.
        /// </summary>
        public int UnitsInStock { get; set; }
        
        /// <summary>
        /// Navigation property to the parent Category.
        /// Populated automatically by EF Core when using Include(p => p.Category).
        /// Enables accessing category information without separate queries.
        /// </summary>
        public Category Category { get; set; }
        
        /// <summary>
        /// Convenience property to get category name directly.
        /// Uses null-conditional operator (?.) to safely access Category.Name.
        /// Returns null if Category navigation property hasn't been loaded.
        /// </summary>
        public string CategoryName => Category?.Name;
    }
}
