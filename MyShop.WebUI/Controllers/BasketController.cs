using MyShop.Core.Contracts;
using MyShop.Core.Models;
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
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IRepository<Customer> customerRepository;

        public BasketController(IBasketService basketService,IOrderService orderService,IRepository<Customer>CustomerRepository)
        {
            _basketService = basketService;
            this._orderService = orderService;
            customerRepository = CustomerRepository;
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

        [HttpGet]
        [Authorize]
        public ActionResult CheckOut()
        {
            Customer customer = customerRepository.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);
            if (customer != null)
            {
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    FirstName = customer.FirstName,
                    SurName = customer.LastName,
                    ZipCode = customer.ZipCode
                };
                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CheckOut(Order order)
        {
            var basketItems = _basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            // payment
            order.OrderStatus = "payment processed";
            _orderService.CreateOrder(order, basketItems);
            _basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = order.Id });
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}