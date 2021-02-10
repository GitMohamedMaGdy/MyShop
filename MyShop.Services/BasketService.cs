using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
   public class BasketService : IBasketService
    {
        IRepository<Product> _ProductContext;
        IRepository<Basket> _BasketContext;

        public const string basketCookieName = "eCommerceBasket";
        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            _ProductContext = ProductContext;
            _BasketContext = BasketContext;
        }

         public Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie basketCookie = httpContext.Request.Cookies.Get(basketCookieName);
            Basket basket = new Basket();
            if (basketCookie != null)
            {
                string basketId = basketCookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = _BasketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                basket = CreateNewBasket(httpContext);

            }
            return basket;
        }

        public Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            _BasketContext.Insert(basket);
            _BasketContext.Commit();

            HttpCookie basketCookie = new HttpCookie(basketCookieName);
            basketCookie.Value = basket.Id;
            basketCookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(basketCookie);
            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem == null)
            {
                basketItem = new BasketItem()
                {
                    ProductId = productId,
                    Quantity = 1,
                    BasketId = basket.Id
                };
                basket.BasketItems.Add(basketItem);
            }
            else
            {
                basketItem.Quantity += 1;

            }
            _BasketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string basketItemId)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == basketItemId);
            if (basketItem != null)
            {
                basket.BasketItems.Remove(basketItem);
                _BasketContext.Commit();
            }

        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            if (basket != null)
            {
                var results = (from basket_Item in basket.BasketItems
                               join product in _ProductContext.Collection()
                               on basket_Item.ProductId equals product.Id
                               select new BasketItemViewModel()
                               {
                                   Id = basket_Item.Id,
                                   ProductName = product.Name,
                                   Image = product.Image,
                                   Quantity = basket_Item.Quantity,
                                   price = product.Price
                               }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel basketSummary = new BasketSummaryViewModel(0, 0);
            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in _ProductContext.Collection()
                                        on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                basketSummary.BasketCount = basketCount ?? 0;
                basketSummary.BasketTotal = basketTotal ?? decimal.Zero;
                return basketSummary;
            }
            else
            {
                return basketSummary;
            }
        }

        public void ClearBasket(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            _BasketContext.Commit();
        }

    }
}
