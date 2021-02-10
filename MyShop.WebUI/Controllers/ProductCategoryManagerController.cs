using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductCategoryManagerController : Controller
    {

        IRepository<ProductCategory> context;
        public ProductCategoryManagerController(IRepository<ProductCategory> context)
        {
            this.context = context;
        }
        public ActionResult Index()
        {
            List<ProductCategory> ProductCategoryCategories = context.Collection().ToList();
            return View(ProductCategoryCategories);
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View(new ProductCategory());
        }
        [HttpPost]
        public ActionResult Create(ProductCategory ProductCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(ProductCategory);
            }
            else
            {
                context.Insert(ProductCategory);
                context.Commit();
                return RedirectToAction("Index");

            }

        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            ProductCategory ProductCategory = context.Find(Id);
            if (ProductCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ProductCategory);

            }
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory ProductCategory, string Id)
        {
            ProductCategory ProductCategoryToUpdate = context.Find(Id);
            if (ProductCategoryToUpdate == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(ProductCategory);
                }
                else
                {

                    ProductCategoryToUpdate.Category = ProductCategory.Category;

                    context.Commit();
                    return RedirectToAction("Index");
                }
            }


        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            ProductCategory ProductCategoryToDelete = context.Find(Id);
            if (ProductCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ProductCategoryToDelete);

            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfiemDelete(ProductCategory ProductCategory, string Id)
        {
            ProductCategory ProductCategoryToDelete = context.Find(Id);
            if (ProductCategoryToDelete == null)
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
