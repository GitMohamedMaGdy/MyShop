using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;
namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> ProductCategories;

        public ProductCategoryRepository()
        {
            ProductCategories = cache["ProductCategory"] as List<ProductCategory>;
            if (ProductCategories == null)
            {
                ProductCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["ProductCategory"] = ProductCategories;
        }

        public void Insert(ProductCategory p)
        {
            ProductCategories.Add(p);
        }
        public void Update(ProductCategory p)
        {
            ProductCategory productToUpdate = ProductCategories.Find(m => m.Id == p.Id);
            if (productToUpdate != null)
            {
                productToUpdate = p;

            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = ProductCategories.Find(m => m.Id == Id);
            if (productCategory != null)
            {
                return productCategory;

            }
            else
            {
                throw new Exception("productCategory Not Found");
            }
        }

        public void Delete(ProductCategory p)
        {
            ProductCategory productCategoryToDelete = ProductCategories.Find(m => m.Id == p.Id);
            if (productCategoryToDelete != null)
            {
                ProductCategories.Remove(productCategoryToDelete);

            }
            else
            {
                throw new Exception("productCategory Not Found");
            }
        }
        public IQueryable<ProductCategory> Collection()
        {
            return ProductCategories.AsQueryable();
        }
    }
}
