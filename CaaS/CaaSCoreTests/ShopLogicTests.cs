using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaaS.Core;
using CaaS.Core.Interfaces;
using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.MySQLDaos;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using static System.Formats.Asn1.AsnWriter;

namespace CaaSCoreTests
{
    public class ShopLogicTests
    {
        private readonly Mock<IShopDao> shopDao;
        private readonly Mock<ICommonDao> commonDao;
        private readonly IShopManagementLogic shopLogic;

        public ShopLogicTests()
        {
            shopDao = new Mock<IShopDao>();
            commonDao = new Mock<ICommonDao>();
            shopLogic = new ShopManagementLogic(shopDao.Object,commonDao.Object);
        }

        [Fact]
        public async Task GetShopWithValidId()
        {
            var shop1 = new Shop(1, "Stracke-Schulist", 1);
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            shopDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(shop1);

            var shops = await shopLogic.FindByIdAsync(1);
            Assert.Equal(shops,shop1);
            shopDao.Verify(dao => dao.FindByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetShopWithInValidId()
        {
            var shop1 = new Shop(1, "Stracke-Schulist", 1);
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(false);
            shopDao.Setup(dao => dao.FindByIdAsync(1)).ThrowsAsync(new ArgumentException("Shop does not exist"));

            Func<Task> act = () => shopLogic.FindByIdAsync(1);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("Shop does not exist", ex.Message);
            shopDao.Verify(dao => dao.ShopExists(1), Times.Once());
        }

        [Fact]
        public async Task GetAllCustomersWithValidShopId()
        {
            List<Customer> expected = new List<Customer>();
            expected.Add(new Customer(1, "Virgil", "Turmel", "vturmel0@ameblo.jp"));
            expected.Add(new Customer(2, "Lela", "Balfre", "lbalfre1@opensource.org"));
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            shopDao.Setup(dao => dao.FindAllCustomersByShopIdAsync(1)).ReturnsAsync(expected);

            var shops = await shopLogic.FindAllCustomersByShopIdAsync(1);

            Assert.Equal(2, expected.Count());
            Assert.Contains(expected.ElementAt(0), shops);
            Assert.Contains(expected.ElementAt(1), shops);
            shopDao.Verify(dao => dao.FindAllCustomersByShopIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task CreateValidShopReturnsTrue()
        {
            var shop = new Shop(77, "Stracke-SchulistV2", 1);
            commonDao.Setup(dao => dao.CheckAppKeyAvailability(1)).ReturnsAsync(false);
            shopDao.Setup(dao => dao.InsertAsync(shop)).ReturnsAsync(1);

            var result = await shopLogic.CreateShop(shop);

            Assert.Equal(result,1);
            shopDao.Verify(dao => dao.InsertAsync(shop), Times.Once());
            commonDao.Verify(dao => dao.CheckAppKeyAvailability(1), Times.Once());
        }

        [Fact]
        public async Task CreateInValidShopReturnsFalse()
        {
            var shop = new Shop(77, "Stracke-SchulistV2", 1);
            commonDao.Setup(dao => dao.CheckAppKeyAvailability(1)).ReturnsAsync(true);
            shopDao.Setup(dao => dao.InsertAsync(shop)).ReturnsAsync(1);

            Func<Task> act = () => shopLogic.CreateShop(shop);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("AppKey is already used", ex.Message);
            commonDao.Verify(dao => dao.CheckAppKeyAvailability(1), Times.Once());
        }

        [Fact]
        public async Task GetAllProductsWithValidShopId()
        {
            List<Product> expected = new List<Product>();
            expected.Add(new Product(1, "pellentesque ultrices phasellus id sapien in", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1));
            expected.Add(new Product(2, "odio elementum eu interdum eu tincidunt in leo maecenas pulvinar lobortis est phasellus sit amet erat nulla", "http://dummyimage.com/227x100.png/dddddd/000000", 808, "duis aliquam convallis nunc proin at turpis a pede posuere nonummy integer non velit donec diam neque vestibulum eget vulputate ut ultrices vel augue vestibulum ante ipsum", 1));
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            shopDao.Setup(dao => dao.FindAllProductsByShopIdAsync(1)).ReturnsAsync(expected);

            var products = await shopLogic.FindAllProductsByShopIdAsync(1);

            Assert.Equal(2, expected.Count());
            Assert.Contains(expected.ElementAt(0), products);
            Assert.Contains(expected.ElementAt(1), products);
            shopDao.Verify(dao => dao.FindAllProductsByShopIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetShopOwnerWithValidShopId()
        {
            var expected = new ShopOwner(1, "Hazlett", "Bridal", 1);
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            shopDao.Setup(dao => dao.FindShopOwnerByIdAsync(1)).ReturnsAsync(expected);

            var shopOwner = await shopLogic.FindShopOwnerByIdAsync(1);
            Assert.Equal(expected, shopOwner);
            shopDao.Verify(dao => dao.FindShopOwnerByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task UpdateValidShopReturnsTrue()
        {
            var shop = new Shop(1, "Stracke-SchulistV2", 1);
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            shopDao.Setup(dao => dao.UpdateAsync(shop)).ReturnsAsync(true);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1,1)).ReturnsAsync(true);

            var result = await shopLogic.UpdateShop(shop,1);

            Assert.Equal(result, true);
            shopDao.Verify(dao => dao.UpdateAsync(shop), Times.Once());
        }
    }
}
    