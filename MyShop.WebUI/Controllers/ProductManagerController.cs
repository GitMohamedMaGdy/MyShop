using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        ProductRepository context;
        public ProductManagerController()
        {
            context = new ProductRepository();
        }
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View(new Product());
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");

            }

        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);

            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToUpdate = context.Find(Id);
            if (productToUpdate == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    productToUpdate.Name = product.Name;
                    productToUpdate.Price = product.Price;
                    productToUpdate.Category = product.Category;
                    productToUpdate.Description = product.Description;
                    productToUpdate.Image = product.Image;
                    context.Commit();
                    return RedirectToAction("Index");
                }
            }


        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);

            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfiemDelete(Product product, string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(productToDelete);
                context.Commit();
                return RedirectToAction("Index");
            }
        }


    }

}
