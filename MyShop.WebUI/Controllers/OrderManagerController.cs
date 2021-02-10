using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class OrderManagerController : Controller
    {
        private readonly IOrderService orderSerive;

        // GET: OrderManager

        public OrderManagerController(IOrderService orderSerive)
        {
            this.orderSerive = orderSerive;
        }
        public ActionResult Index()
        {
            List<Order> orders = orderSerive.GetOrderList();
            return View(orders);
        }

        [HttpGet]
        public ActionResult UpdateOrder(string Id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Order Processed",
                "Order Shipped",
                "Order Completed"
            };
            Order order = orderSerive.GetOrder(Id);
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateOrder(Order OrderToUpdate,string Id)
        {
            Order order = orderSerive.GetOrder(Id);
            order.OrderStatus = OrderToUpdate.OrderStatus;
            orderSerive.UpdateOrder(order);
            return RedirectToAction("Index");
        }
    }
}