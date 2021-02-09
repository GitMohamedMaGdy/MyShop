using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> Items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            Items = cache[className] as List<T>;
            if (Items == null)
            {
                Items = new List<T>();
            }

        }

        public void Commit()
        {
            cache[className] = Items;
        }

        public void Insert(T t)
        {
            Items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = Items.Find(i => i.Id == t.Id);
            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }

        public void Delete(T t)
        {
            T tToDelete = Items.Find(i => i.Id == t.Id);
            if (tToDelete != null)
            {
                Items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }

        public T Find(string Id)
        {
            T t = Items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + "Not Found");

            }
        }

        public IQueryable<T> Collection()
        {
            return Items.AsQueryable();
        }
    }
}
