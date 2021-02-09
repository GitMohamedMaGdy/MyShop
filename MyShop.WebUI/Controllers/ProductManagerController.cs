using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public ProductManagerController(IRepository<Product> context, IRepository<ProductCategory> productCategories)
        {
            this.context = context;
            this.productCategories = productCategories;
        }
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {

            ProductManagerViewModel productManagerVM = new ProductManagerViewModel();
            productManagerVM.Product = new Product();
            productManagerVM.Categories = productCategories.Collection();
            return View(productManagerVM);
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else

            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//Images//") + product.Image);
                }
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

                ProductManagerViewModel productManagerVM = new ProductManagerViewModel();
                productManagerVM.Product = product;
                productManagerVM.Categories = productCategories.Collection();
                return View(productManagerVM);

            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
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
                    if (file != null)
                    {
                        productToUpdate.Image = product.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//Images//") + productToUpdate.Image);
                    }
                    productToUpdate.Name = product.Name;
                    productToUpdate.Price = product.Price;
                    productToUpdate.Category = product.Category;
                    productToUpdate.Description = product.Description;
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
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }


    }

}
