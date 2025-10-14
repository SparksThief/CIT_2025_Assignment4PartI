using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
  public class OrderDetails
  {
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public float Discount { get; set; }
    public Product Product { get; set; }
    public Order Order { get; set; }
    }
}
