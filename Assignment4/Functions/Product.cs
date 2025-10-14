using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment4
{
    
    public partial class Product
    {
        [NotMapped]
        public string ProductName
        {
            get => Name;
            set => Name = value;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public Category Category { get; set; }
        public string CategoryName => Category?.Name;
    }
    
}
