using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
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

        public Category DeleteCategory(int id)
        {
            using var db = new NorthwindContext();
            var category = db.Categories.Find(id);
            if (category == null)
                return null;
            db.Categories.Remove(category);
            db.SaveChanges();
            return category;
        }

    }
}
