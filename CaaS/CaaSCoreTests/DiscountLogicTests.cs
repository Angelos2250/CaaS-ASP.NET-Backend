using CaaS.Core.Interfaces;
using CaaS.Core;
using Data_Access_Layer.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Xunit;

namespace CaaSCoreTests
{
    public class DiscountLogicTests
    {
        private readonly Mock<IDiscountDao> discountDao;
        private readonly Mock<ICommonDao> commonDao;
        private readonly IDiscountManagementLogic discountLogic;

        public DiscountLogicTests()
        {
            discountDao = new Mock<IDiscountDao>();
            commonDao = new Mock<ICommonDao>();
            discountLogic = new DiscountManagementLogic(discountDao.Object, commonDao.Object);
        }

        [Fact]
        public async Task CreateValidDiscountRule1()
        {
            var expected = new Discount(1, "rule...", 1, 1, 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(true);
            discountDao.Setup(dao => dao.CreateDiscount1(expected, "11")).ReturnsAsync(1);

            await discountLogic.CreateDiscount1(expected, "11", 1);

            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
            discountDao.Verify(dao => dao.CreateDiscount1(expected, "11"), Times.Once());
        }

        [Fact]
        public async Task CreateValidDiscountRule2()
        {
            var expected = new Discount(1, "rule...", 1, 1, 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(true);
            discountDao.Setup(dao => dao.CreateDiscount2(expected, "2022-01-01", "2023-01-01")).ReturnsAsync(1);

            await discountLogic.CreateDiscount2(expected, "2022-01-01", "2023-01-01", 1);

            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
            discountDao.Verify(dao => dao.CreateDiscount2(expected, "2022-01-01", "2023-01-01"), Times.Once());
        }

        [Fact]
        public async Task CreateInValidDiscountRule1()
        {
            var expected = new Discount(1, "rule...", 1, 1, 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(false);
            discountDao.Setup(dao => dao.CreateDiscount1(expected, "11")).ReturnsAsync(1);

            await Assert.ThrowsAsync<ArgumentException>(() => discountLogic.CreateDiscount1(expected, "11", 1));
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
        }

        [Fact]
        public async Task CreateInValidDiscountRule2()
        {
            var expected = new Discount(1, "rule...", 1, 1, 1);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(false);
            discountDao.Setup(dao => dao.CreateDiscount2(expected, "2022-01-01", "2023-01-01")).ReturnsAsync(1);

            await Assert.ThrowsAsync<ArgumentException>(() => discountLogic.CreateDiscount2(expected, "2022-01-01", "2023-01-01", 1));
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
        }

        [Fact]
        public async Task DeleteValidDiscount()
        {
            var expected = new Discount(1, "rule...", 1, 1, 1);
            discountDao.Setup(dao => dao.GetDiscountById(1)).ReturnsAsync(expected);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(true);
            discountDao.Setup(dao => dao.DeleteDiscount(1)).ReturnsAsync(true);

            await discountLogic.DeleteDiscount(1,1);

            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
            discountDao.Verify(dao => dao.DeleteDiscount(1), Times.Once());
            discountDao.Verify(dao => dao.GetDiscountById(1), Times.Once());
        }

        [Fact]
        public async Task DeleteInValidDiscount()
        {
            var expected = new Discount(1, "rule...", 1, 1, 1);
            discountDao.Setup(dao => dao.GetDiscountById(1)).ReturnsAsync(expected);
            commonDao.Setup(dao => dao.CheckAppKeyValidity(1, 1)).ReturnsAsync(false);
            discountDao.Setup(dao => dao.DeleteDiscount(1)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => discountLogic.DeleteDiscount(1,1));
            commonDao.Verify(dao => dao.CheckAppKeyValidity(1, 1), Times.Once());
        }

        [Fact]
        public async Task GetValidDiscountById()
        {
            var expected = new Discount(1, "rule...", 1, 1, 1);
            discountDao.Setup(dao => dao.GetDiscountById(1)).ReturnsAsync(expected);

            var res = await discountLogic.GetDiscountById(1);

            Assert.Equal(expected, res);
            discountDao.Verify(dao => dao.GetDiscountById(1), Times.Once());
        }

        [Fact]
        public async Task GetValidDiscountsOfShopByShopId()
        {
            List<Discount> expected = new List<Discount>();
            var d1 = new Discount(1, "rule...", 1, 1, 1);
            expected.Add(d1);
            discountDao.Setup(dao => dao.GetDiscountsOfShop(1)).ReturnsAsync(expected);

            var res = await discountLogic.GetDiscountsOfShop(1);

            Assert.Equal(1, expected.Count());
            Assert.Contains(expected.ElementAt(0), res);
            discountDao.Verify(dao => dao.GetDiscountsOfShop(1), Times.Once());
        }


    }
}
