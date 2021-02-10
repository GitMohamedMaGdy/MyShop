using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> OrderContext;

        public OrderService(IRepository<Order> repository)
        {
            this.OrderContext = repository;
        }
        public void CreateOrder(Order order, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                order.OrderItems.Add(new OrderItem()
                {

                    ProductId = item.Id,
                    Price = item.price,
                    Image = item.Image,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });
                OrderContext.Insert(order);
                OrderContext.Commit();
            }



        }

        public List<Order> GetOrderList()
        {
            return OrderContext.Collection().ToList();
        }
        public Order GetOrder(string Id)
        {
            return OrderContext.Find(Id);
        }
        public void UpdateOrder(Order Order_Update)
        {
            OrderContext.Update(Order_Update);
            OrderContext.Commit();
        }
    }
}
