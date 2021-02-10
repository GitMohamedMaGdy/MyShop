using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {

            //Arrange
            IRepository<Product> _productsContext = new Mocks.MockContext<Product>();
            IRepository<Basket> _BasketContext = new Mocks.MockContext<Basket>();
            IBasketService basketService = new BasketService(_productsContext, _BasketContext);
            //Act
            var httpContext = new Mocks.MockHttpContext();
            basketService.AddToBasket(httpContext, "1");
            Basket basket = _BasketContext.Collection().FirstOrDefault();

            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }
        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //Arrange
            IRepository<Product> _productsContext = new Mocks.MockContext<Product>();
            IRepository<Basket> _BasketContext = new Mocks.MockContext<Basket>();
            IRepository<Order> _OrderContext = new Mocks.MockContext<Order>();
            IRepository<Customer> _CustomerContext = new Mocks.MockContext<Customer>();

            Basket basket = new Basket();
            IBasketService basketService;
            IOrderService orderService;
            var httpContext = new Mocks.MockHttpContext();


            //Act
            _productsContext.Insert(new Product { Id = "1", Price = 10.00m });
            _productsContext.Insert(new Product { Id = "2", Price = 20.00m });
            basket.BasketItems.Add(new BasketItem { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem { ProductId = "2", Quantity = 1 });
            _BasketContext.Insert(basket);
            basketService = new BasketService(_productsContext, _BasketContext);
            orderService = new OrderService(_OrderContext);


            var controller = new BasketController(basketService,orderService,_CustomerContext);
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            //Assert
            Assert.AreEqual(3, basketSummary.BasketCount);
            Assert.AreEqual(40, basketSummary.BasketTotal);




        }

        [TestMethod]
        public void CanCheckOutAndCreateOrder()
        {
            IRepository<Product> _productsContext = new Mocks.MockContext<Product>();
            IRepository<Basket> _BasketContext = new Mocks.MockContext<Basket>();
            IRepository<Order> _OrderContext = new Mocks.MockContext<Order>();
            IRepository<Customer> _CustomerContext = new Mocks.MockContext<Customer>();

            IBasketService basketService;
            IOrderService orderService;
            Basket basket;

            _productsContext.Insert(new Product { Id = "1", Price = 10.00m });
            _productsContext.Insert(new Product { Id = "2", Price = 20.00m });
            basket = new Basket();
            basket.BasketItems.Add(new BasketItem { ProductId = "1", Quantity = 2 ,BasketId=basket.Id});
            basket.BasketItems.Add(new BasketItem { ProductId = "2", Quantity = 1,BasketId = basket.Id });
            _BasketContext.Insert(basket);

            _CustomerContext.Insert(new Customer { Id = "1", Email = "fci.mohamedmagdy@gmail.com", ZipCode = "32323" });
            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("fci.mohamedmagdy@gmail.com", "Forms"), null);

            basketService = new BasketService(_productsContext, _BasketContext);
            orderService = new OrderService(_OrderContext);

            var controller = new BasketController(basketService, orderService,_CustomerContext);
            var httpContext = new Mocks.MockHttpContext();
            httpContext.User = FakeUser;
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act

            Order order = new Order();
            controller.CheckOut(order);

            //Assert

            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);

            Order orderInRepo = _OrderContext.Find(order.Id);
            Assert.AreEqual(2, orderInRepo.OrderItems.Count);
        }
    }
}
