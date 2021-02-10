using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI;
using MyShop.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            // Arrange
            IRepository<Product> _productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> _productCategoryContext = new Mocks.MockContext<ProductCategory>();
            _productContext.Insert(new Product());
            HomeController controller = new HomeController(_productContext, _productCategoryContext);
            

            // Act
            var result = controller.Index() as ViewResult;
            var viewModel =(ProductListViewModel) result.ViewData.Model;

            // Assert
            Assert.AreEqual(1,viewModel.Products.Count());
        }

        
    }
}
