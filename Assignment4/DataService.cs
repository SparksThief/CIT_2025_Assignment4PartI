using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    // Single public class with all methods and interface for DI
    public class DataService : IDataService
    {
        private readonly NorthwindContext _context;

        // DI injects a scoped DbContext per HTTP request
        public DataService(NorthwindContext context)
        {
            _context = context;
        }

        // Categories (req. 9–13)
        public List<Category> GetCategories() =>
            _context.Categories.OrderBy(c => c.Id).ToList();

        public Category GetCategory(int id) =>
            _context.Categories.SingleOrDefault(c => c.Id == id);

        public Category CreateCategory(string name, string description)
        {
            var c = new Category { Name = name, Description = description };
            _context.Categories.Add(c);
            _context.SaveChanges();
            return c; // Id now populated by DB (or by mapping, depending on schema)
        }

        public bool UpdateCategory(int id, string name, string description)
        {
            var c = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (c is null) return false;
            c.Name = name;
            c.Description = description;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var c = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (c is null) return false;
            _context.Categories.Remove(c);
            _context.SaveChanges();
            return true;
        }


        // Products (req. 6–8)
        public Product GetProduct(int id) =>
            _context.Products
                .Include(p => p.Category)
                .SingleOrDefault(p => p.Id == id);

        public List<Product> GetProductByCategory(int categoryId) =>
            _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.Id)
                .ToList();

        public List<Product> GetProductByName(string name) =>
            _context.Products
                .Include(p => p.Category)
                // PostgreSQL: case-insensitive substring
                .Where(p => EF.Functions.ILike(p.Name, $"%{name}%"))
                .OrderBy(p => p.Id)
                .ToList();


        // Orders / OrderDetails (if you still expose them in Part I)
        public Order GetOrder(int id) =>
            _context.Orders
                // Choose the property name that matches your model: Id or OrderId
                .Where(o => o.Id == id) // or o.OrderId == id
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Category)
                .SingleOrDefault();

        public List<Order> GetOrders() =>
            _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Category)
                .OrderBy(o => o.Id) // or o.OrderId
                .ToList();

        public List<OrderDetails> GetOrderDetailsByOrderId(int orderId) =>
            _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Category)
                .OrderBy(od => od.ProductId)
                .ToList();

        public List<OrderDetails> GetOrderDetailsByProductId(int productId) =>
            _context.OrderDetails
                .Where(od => od.ProductId == productId)
                .Include(od => od.Order)
                .OrderBy(od => od.OrderId) // assignment requires ordering by OrderId
                .ToList();
    }
}
