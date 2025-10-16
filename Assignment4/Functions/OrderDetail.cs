using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
  /// <summary>
  /// Represents a single line item in an order (junction/association table).
  /// Links Orders and Products in a many-to-many relationship.
  /// Each OrderDetails represents one product within a specific order,
  /// with its quantity, price, and discount information.
  /// </summary>
  public class OrderDetails
  {
    /// <summary>
    /// Foreign key to Order table. Part of composite primary key.
    /// Together with ProductId, uniquely identifies this order line item.
    /// Maps to 'orderid' column.
    /// </summary>
    public int OrderId { get; set; }
    
    /// <summary>
    /// Foreign key to Product table. Part of composite primary key.
    /// Together with OrderId, uniquely identifies this order line item.
    /// Maps to 'productid' column.
    /// </summary>
    public int ProductId { get; set; }
    
    /// <summary>
    /// Price per unit for this product in this order.
    /// Can differ from Product.UnitPrice if special pricing was applied.
    /// Maps to 'unitprice' column.
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Number of units of this product ordered.
    /// Maps to 'quantity' column.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Discount applied to this line item (as a percentage, e.g., 0.15 = 15% off).
    /// Maps to 'discount' column.
    /// </summary>
    public float Discount { get; set; }
    
    /// <summary>
    /// Navigation property to the Product ordered.
    /// Populated by EF Core when using Include(od => od.Product).
    /// Allows accessing product details without separate queries.
    /// </summary>
    public Product Product { get; set; }
    
    /// <summary>
    /// Navigation property to the parent Order.
    /// Populated by EF Core when using Include(od => od.Order).
    /// Allows accessing order header information (dates, customer, etc.).
    /// </summary>
    public Order Order { get; set; }
  }
}
