using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T : BaseEntity
    {

        List<T> Items;


        public MockContext()
        {

            Items = new List<T>();

        }

        public void Commit()
        {
            return;
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
                throw new Exception("Not Found");
            }
        }

        public void Delete(string Id)
        {
            T tToDelete = Items.Find(i => i.Id == Id);
            if (tToDelete != null)
            {
                Items.Remove(tToDelete);
            }
            else
            {
                throw new Exception("Not Found");
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
                throw new Exception("Not Found");

            }
        }

        public IQueryable<T> Collection()
        {
            return Items.AsQueryable();
        }
    }
}
