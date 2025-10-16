using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment4
{
  public interface IDataService
    {
        // products
        Product GetProduct(int id);
        List<Product> GetProductByCategory(int categoryid);
        List<Product> GetProductByName(string name);

        // Categories
        Category GetCategory(int id);
        List<Category> GetCategories();
        Category CreateCategory(string name, string description);
        bool DeleteCategory(int id);
        bool UpdateCategory(int id, string name, string description);
    }
}