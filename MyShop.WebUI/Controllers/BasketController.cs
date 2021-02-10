using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        // GET: Basket
        IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public ActionResult Index()
        {
            var model = _basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            _basketService.AddToBasket(this.HttpContext,Id);
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromBasket(string Id)
        {
            _basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");

        }
        public PartialViewResult BasketSummary()
        {
           var basketSummary =  _basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }



    }
}