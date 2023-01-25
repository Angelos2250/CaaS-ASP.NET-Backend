using CaaS.Core.Interfaces;
using CaaS.Core;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Formats.Asn1.AsnWriter;

namespace CaaSCoreTests
{
    public class ProductLogicTests
    {
        private readonly Mock<IProductDao> productDao;
        private readonly Mock<ICommonDao> commonDao;
        private readonly Mock<IShopDao> shopDao;
        private readonly IProductManagementLogic productManagementLogic;

        public ProductLogicTests()
        {
            productDao = new Mock<IProductDao>();
            commonDao = new Mock<ICommonDao>();
            shopDao = new Mock<IShopDao>();
            productManagementLogic = new ProductManagementLogic(productDao.Object, commonDao.Object, shopDao.Object);
        }

        [Fact]
        public async Task CreateProductWithValidAppKeyReturnsTrue()
        {
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1,1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.InsertAsync(expected)).ReturnsAsync(1);

            await productManagementLogic.AddProduct(expected,1);

            productDao.Verify(dao => dao.InsertAsync(expected), Times.Once());
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
        }

        [Fact]
        public async Task CreateProductWithInValidAppKeyReturnsTrue()
        {
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 2)).ReturnsAsync(false);
            productDao.Setup(dao => dao.InsertAsync(expected)).ThrowsAsync(new ArgumentException("False AppKey"));

            Func<Task> act = () => productManagementLogic.AddProduct(expected,2);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("False AppKey", ex.Message);
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 2), Times.Once());
        }

        [Fact]
        public async Task DeleteValidProductWithValidAppKey()
        {
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.DeleteProduct(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExists(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);

            var res = await productManagementLogic.DeleteProduct(1, 1);
            Assert.Equal(res, true);
            productDao.Verify(dao => dao.DeleteProduct(1), Times.Once());
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
        }

        [Fact]
        public async Task DeleteValidProductWithInValidAppKey()
        {
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 2)).ReturnsAsync(false);
            productDao.Setup(dao => dao.DeleteProduct(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExists(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);

            Func<Task> act = () => productManagementLogic.DeleteProduct(1, 2);
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("False AppKey", ex.Message);
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 2), Times.Once());
        }

        [Fact]
        public async Task DeleteInValidProductWithValidAppKey()
        {
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.DeleteProduct(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExists(1)).ReturnsAsync(false);

            Func<Task> act = () => productManagementLogic.DeleteProduct(1, 1);
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("Product doesnt exist", ex.Message);
            productDao.Verify(dao => dao.ProductExists(1), Times.Once());
        }

        [Fact]
        public async Task GetFullTextSeachProduct()
        {
            List<Product> products = new List<Product>();
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            products.Add(expected);
            productDao.Setup(dao => dao.FindByFullTextSearch("inserted",1)).ReturnsAsync(products);

            var res = await productManagementLogic.FindByFullTextSearch("inserted", 1);
            Assert.Equal(1, products.Count());
            Assert.Contains(products.ElementAt(0), res);
            productDao.Verify(dao => dao.FindByFullTextSearch("inserted", 1), Times.Once());
        }

        [Fact]
        public async Task GetValidProductById()
        {
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            productDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);
            productDao.Setup(dao => dao.ProductExists(1)).ReturnsAsync(true);

            var res = await productManagementLogic.GetProductById(1);
            Assert.Equal(expected, res);
            productDao.Verify(dao => dao.FindByIdAsync(1), Times.Once());
            productDao.Verify(dao => dao.ProductExists(1), Times.Once());
        }

        [Fact]
        public async Task GetInValidProductById()
        {
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            productDao.Setup(dao => dao.ProductExists(1)).ReturnsAsync(false);
            productDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);

            Func<Task> act = () => productManagementLogic.GetProductById(1);
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("Product doesnt exist", ex.Message);
            productDao.Verify(dao => dao.ProductExists(1), Times.Once());
        }

        [Fact]
        public async Task GetProductsOfValidShop()
        {
            List<Product> products = new List<Product>();
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            products.Add(expected);
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.FindAllAsync(1)).ReturnsAsync(products);

            var res = await productManagementLogic.GetProductsOfShop(1);
            Assert.Equal(1, products.Count());
            Assert.Contains(products.ElementAt(0), res);
            productDao.Verify(dao => dao.FindAllAsync(1), Times.Once());
            shopDao.Verify(dao => dao.ShopExists(1), Times.Once());
        }

        [Fact]
        public async Task GetProductsOfInValidShop()
        {
            List<Product> products = new List<Product>();
            var expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            products.Add(expected);
            shopDao.Setup(dao => dao.ShopExists(11)).ReturnsAsync(false);
            productDao.Setup(dao => dao.FindAllAsync(11)).ReturnsAsync(products);

            Func<Task> act = () => productManagementLogic.GetProductsOfShop(11);
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("Shop doesnt Exist", ex.Message);
            shopDao.Verify(dao => dao.ShopExists(11), Times.Once());
        }

        [Fact]
        public async Task UpdateValidProductReturnsTrue()
        {
            var expected = new Product(1, "updated", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.UpdateAsync(expected)).ReturnsAsync(true);

            await productManagementLogic.UpdateProduct(expected,1);

            productDao.Verify(dao => dao.UpdateAsync(expected), Times.Once());
            productDao.Verify(dao => dao.ProductExist(1), Times.Once());
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
        }
    }
}
