using CaaS.Core;
using CaaS.Core.Interfaces;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CaaSCoreTests
{
    public class ShopOwnerLogicTests
    {
        private readonly Mock<IShopOwnerDao> shopOwnerDao;
        private readonly IShopOwnerManagement shopOwnerLogic;

        public ShopOwnerLogicTests()
        {
            shopOwnerDao = new Mock<IShopOwnerDao>();
            shopOwnerLogic = new ShopOwnerManagementLogic(shopOwnerDao.Object);
        }

        [Fact]
        public async Task CreateValidShopOwnerReturnsTrue()
        {
            var expected = new ShopOwner(1, "HazlettV2", "Bridal", 1);
            shopOwnerDao.Setup(dao => dao.ShopOwnerExists(1)).ReturnsAsync(false);
            shopOwnerDao.Setup(dao => dao.InsertAsync(expected)).ReturnsAsync(1);

            var result = await shopOwnerLogic.CreateOwner(expected);

            Assert.Equal(result, 1);
            shopOwnerDao.Verify(dao => dao.ShopOwnerExists(1), Times.Once());
            shopOwnerDao.Verify(dao => dao.InsertAsync(expected), Times.Once());
        }

        [Fact]
        public async Task CreateInValidShopOwnerReturnsFalse()
        {
            var expected = new ShopOwner(1, "HazlettV2", "Bridal", 1);
            shopOwnerDao.Setup(dao => dao.ShopOwnerExists(1)).ReturnsAsync(true);
            shopOwnerDao.Setup(dao => dao.InsertAsync(expected)).ReturnsAsync(1);

            Func<Task> act = () => shopOwnerLogic.CreateOwner(expected);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("ShopOwner already exist", ex.Message);
            shopOwnerDao.Verify(dao => dao.ShopOwnerExists(1), Times.Once());
        }

        [Fact]
        public async Task GetShopOwnerWithValidId()
        {
            var expected = new ShopOwner(1, "HazlettV2", "Bridal", 1);
            shopOwnerDao.Setup(dao => dao.ShopOwnerExists(1)).ReturnsAsync(true);
            shopOwnerDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);

            var res = await shopOwnerLogic.FindById(1);
            Assert.Equal(expected, res);
            shopOwnerDao.Verify(dao => dao.FindByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetShopWithInValidId()
        {
            var expected = new ShopOwner(1, "HazlettV2", "Bridal", 1);
            shopOwnerDao.Setup(dao => dao.ShopOwnerExists(1)).ReturnsAsync(false);
            shopOwnerDao.Setup(dao => dao.FindByIdAsync(1)).ThrowsAsync(new ArgumentException("ShopOwner does not exist"));

            Func<Task> act = () => shopOwnerLogic.FindById(1);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("ShopOwner does not exist", ex.Message);
            shopOwnerDao.Verify(dao => dao.ShopOwnerExists(1), Times.Once());
        }

        [Fact]
        public async Task GetShopWithValidOwnerId()
        {
            var expected = new Shop(1, "Stracke-Schulist", 1);
            shopOwnerDao.Setup(dao => dao.ShopOwnerExists(1)).ReturnsAsync(true);
            shopOwnerDao.Setup(dao => dao.FindShopByShopOwnerIdAsync(1)).ReturnsAsync(expected);

            var res = await shopOwnerLogic.FindShopByShopOwnerId(1);
            Assert.Equal(expected, res);
            shopOwnerDao.Verify(dao => dao.FindShopByShopOwnerIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task UpdateValidShopOwnerReturnsTrue()
        {
            var expected = new ShopOwner(1, "HazlettV2", "Bridal", 1);
            shopOwnerDao.Setup(dao => dao.ShopOwnerExists(1)).ReturnsAsync(true);
            shopOwnerDao.Setup(dao => dao.UpdateAsync(expected)).ReturnsAsync(true);

            var result = await shopOwnerLogic.UpdateOwner(expected);

            Assert.Equal(result, true);
            shopOwnerDao.Verify(dao => dao.UpdateAsync(expected), Times.Once());
        }
    }
}
