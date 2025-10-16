using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment4
{
    /// <summary>
    /// Represents a customer order in the Northwind database.
    /// Each order contains one or more OrderDetails (line items) representing ordered products.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Navigation property to the collection of order line items.
        /// Each OrderDetails represents one product in this order.
        /// Populated by EF Core when using Include(o => o.OrderDetails).
        /// </summary>
        public List<OrderDetails> OrderDetails { get; set; }
        
        /// <summary>
        /// Primary key mapped to 'orderid' column in the database.
        /// This is the property used by EF Core for database operations.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Alternative ID property, NOT mapped to database.
        /// Available for business logic if needed, but EF Core uses 'Id' property.
        /// The database only has one 'orderid' column, which maps to 'Id'.
        /// </summary>
        [NotMapped]
        public int OrderId { get; set; }
        
        /// <summary>
        /// Foreign key to Customer. Maps to 'customerid' column.
        /// Note: String type because Northwind uses alphanumeric customer codes (e.g., "ALFKI").
        /// </summary>
        public string CustomerId { get; set; }
        
        /// <summary>
        /// Foreign key to Employee who processed the order.
        /// Maps to 'employeeid' column.
        /// </summary>
        public int EmployeeId { get; set; }
        
        /// <summary>
        /// Date when the order was placed.
        /// Maps to 'orderdate' column.
        /// </summary>
        public DateTime OrderDate { get; set; }
        
        /// <summary>
        /// Date by which the customer needs the order.
        /// Maps to 'requireddate' column.
        /// </summary>
        public DateTime RequiredDate { get; set; }
        
        /// <summary>
        /// Actual shipping date (null if not yet shipped).
        /// Nullable because order might not be shipped yet.
        /// Maps to 'shippeddate' column.
        /// </summary>
        public DateTime? ShippedDate { get; set; }
        
        /// <summary>
        /// Shipping cost. Maps to 'freight' column.
        /// </summary>
        public decimal Freight { get; set; }
        
        /// <summary>
        /// Name of recipient/company for shipping.
        /// Maps to 'shipname' column.
        /// </summary>
        public string ShipName { get; set; }
        
        /// <summary>
        /// Shipping street address - NOT MAPPED.
        /// This column doesn't exist in the Northwind database schema being used.
        /// Marked [NotMapped] to prevent EF Core from looking for it.
        /// </summary>
        [NotMapped]
        public string ShipAddress { get; set; }
        
        /// <summary>
        /// City for shipping. Maps to 'shipcity' column.
        /// </summary>
        public string ShipCity { get; set; }
        
        /// <summary>
        /// Postal/ZIP code for shipping - NOT MAPPED.
        /// This column doesn't exist in the Northwind database schema being used.
        /// </summary>
        [NotMapped]
        public string ShipPostalCode { get; set; }
        
        /// <summary>
        /// Country for shipping - NOT MAPPED.
        /// This column doesn't exist in the Northwind database schema being used.
        /// </summary>
        [NotMapped]
        public string ShipCountry { get; set; }
    }
}
