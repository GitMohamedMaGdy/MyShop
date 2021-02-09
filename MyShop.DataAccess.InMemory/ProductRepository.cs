using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> Products;

        public ProductRepository()
        {
            Products = cache["Products"] as List<Product>;
            if (Products == null)
            {
                Products = new List<Product>();
            }
        }

        public void Commit()
        {
            cache["Products"] = Products;
        }

        public void Insert (Product p)
        {
            Products.Add(p);
        }
        public void Update(Product p)
        {
            Product productToUpdate = Products.Find(m => m.Id == p.Id);
            if (productToUpdate != null)
            {
                productToUpdate = p;

            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public Product Find(string Id)
        {
            Product product = Products.Find(m => m.Id == Id);
            if (product  != null)
            {
                return product;

            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public void Delete(Product p)
        {
            Product productToDelete = Products.Find(m => m.Id == p.Id);
            if (productToDelete != null)
            {
                Products.Remove(productToDelete);

            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }
        public IQueryable<Product> Collection()
        {
            return Products.AsQueryable();
        }
    }
}
