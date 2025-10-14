using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class Order
    {
        public List<OrderDetails> OrderDetails { get; set; }
        public int Id { get; set; }
        public int OrdersId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public int ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}