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
    public class DataService
    {
        // Return all categories from the database
        public System.Collections.Generic.List<Category> GetCategories()
        {
            using var db = new NorthwindContext();
            return db.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            using var db = new NorthwindContext();
            return db.Categories.Find(id);
        }

        public Category CreateCategory(string name, string description)
        {
            using var db = new NorthwindContext();
            var nextId = (db.Categories.Max(c => (int?)c.Id) ?? 0) + 1;
            var category = new Category { Id = nextId, Name = name, Description = description };
            db.Categories.Add(category);
            db.SaveChanges();
            return category;
        }

        public bool DeleteCategory(int id)
        {
            using var db = new NorthwindContext();
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return false;
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return true;
        }
        public bool UpdateCategory(int id, string name, string description)
        {
            using var db = new NorthwindContext();
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return false;
            }
            category.Name = name;
            category.Description = description;
            db.SaveChanges();
            return true;
        }

        public System.Collections.Generic.List<Product> GetProducts()
        {
            using var db = new NorthwindContext();
            return
                db.Products.Include(p => p.Category).ToList();
        }
        public Product GetProduct(int id)
        {
            using var db = new NorthwindContext();
            return db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);

        }

        public System.Collections.Generic.List<Product> GetProductByCategory(int categoryid)
        {
            using var db = new NorthwindContext();
            return db.Products
                .Where(p => p.CategoryId == categoryid)
                .Include (p => p.Category)
                .OrderBy(c => c.Id)
                
                .ToList();

        }

        public int? GetCategoryIdByName(string categoryName)
        {
            using var db = new NorthwindContext();
            return db.Categories
                .Where(c => c.Name == categoryName)
                .Select(c => (int?)c.Id)
                .FirstOrDefault();
        }

        public System.Collections.Generic.List<Product> GetProductsByCategoryName(string categoryName)
        {
            var categoryId = GetCategoryIdByName(categoryName);
            if (categoryId == null)
            {
                return new System.Collections.Generic.List<Product>();
            }
            return GetProductByCategory(categoryId.Value);
        }

        public System.Collections.Generic.List<Product> GetProductByName(string name)
        {
            using var db = new NorthwindContext();
            return db.Products
                .Where(p => p.Name.Contains(name))
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .ToList();

        }

        public Order GetOrder(int id)
        {
            using var db = new NorthwindContext();
            return db.Orders
                .Where(o => o.OrderId == id) 
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefault();
        }

        public System.Collections.Generic.List<Order> GetOrders()
        {
            using var db = new NorthwindContext();
            return db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Category)
                    .OrderBy(o => o.OrderId)         
                .ToList();
        }

        public System.Collections.Generic.List<OrderDetails> GetOrderDetailsByOrderId(int orderid)
        {
            using var db = new NorthwindContext();
            return db.OrderDetails
                .Where(od => od.OrderId == orderid)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Category)
                .OrderBy(od => od.ProductId)
                .ToList();
        }

        public System.Collections.Generic.List<OrderDetails> GetOrderDetailsByProductId(int productid)
        {
            using var db = new NorthwindContext();
            return db.OrderDetails
                .Where(od => od.ProductId == productid)
                .Include(od => od.Order)
                .OrderByDescending(od => od.UnitPrice)
                .ThenBy(od => od.Order.OrderDate)
                .ThenBy(od => od.OrderId)
                .ToList();
        }
    }    
}
