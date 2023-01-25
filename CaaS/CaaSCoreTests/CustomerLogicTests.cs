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
    public class CustomerLogicTests
    {
        private readonly Mock<IShopDao> shopDao;
        private readonly Mock<ICustomerDao> customerDao;
        private readonly ICustomerManagementLogic customerLogic;

        public CustomerLogicTests()
        {
            shopDao = new Mock<IShopDao>();
            customerDao = new Mock<ICustomerDao>();
            customerLogic = new CustomerManagementLogic(shopDao.Object, customerDao.Object);
        }

        [Fact]
        public async Task CreateValidCustomer()
        {
            var expected = new Customer(1, "fn", "ln", "email");
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            customerDao.Setup(dao => dao.InsertAsync(expected,1)).ReturnsAsync(1);

            var res = await customerLogic.CreateCustomer(expected,1);
            Assert.Equal(res, 1);
            shopDao.Verify(dao => dao.ShopExists(1), Times.Once());
            customerDao.Verify(dao => dao.InsertAsync(expected, 1), Times.Once());
        }

        [Fact]
        public async Task CreateInValidCustomer()
        {
            var expected = new Customer(1, "fn", "ln", "email");
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(false);
            customerDao.Setup(dao => dao.InsertAsync(expected, 1)).ReturnsAsync(1);

            await Assert.ThrowsAsync<ArgumentException>(() => customerLogic.CreateCustomer(expected,1));
            shopDao.Verify(dao => dao.ShopExists(1), Times.Once());
        }

        [Fact]
        public async Task GetAllCartsOfValidCustomer()
        {
            List<Cart> expected = new List<Cart>();
            var c1 = new Cart(1,1);
            expected.Add(c1);
            customerDao.Setup(dao => dao.CustomerExists(1)).ReturnsAsync(true);
            customerDao.Setup(dao => dao.FindAllCartsByCustomerIdAsync(1)).ReturnsAsync(expected);

            var res = await customerLogic.FindAllCartsByCustomerIdAsync(1);
            Assert.Equal(1, expected.Count());
            Assert.Contains(expected.ElementAt(0), res);
            customerDao.Verify(dao => dao.CustomerExists(1), Times.Once());
            customerDao.Verify(dao => dao.FindAllCartsByCustomerIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetAllOrdersOfValidCustomer()
        {
            List<Order> expected = new List<Order>();
            var o1 = new Order(1, DateTime.Parse("2022-11-11"), 11, 1, 1);
            expected.Add(o1);
            customerDao.Setup(dao => dao.CustomerExists(1)).ReturnsAsync(true);
            customerDao.Setup(dao => dao.FindAllOrdersByCustomerIdAsync(1)).ReturnsAsync(expected);

            var res = await customerLogic.FindAllOrdersByCustomerIdAsync(1);
            Assert.Equal(1, expected.Count());
            Assert.Contains(expected.ElementAt(0), res);
            customerDao.Verify(dao => dao.CustomerExists(1), Times.Once());
            customerDao.Verify(dao => dao.FindAllOrdersByCustomerIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetValidCustomerById()
        {
            var expected = new Customer(1, "fn", "ln", "email");
            customerDao.Setup(dao => dao.CustomerExists(1)).ReturnsAsync(true);
            customerDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);

            var res = await customerLogic.FindById(1);
            Assert.Equal(expected, res);
            customerDao.Verify(dao => dao.CustomerExists(1), Times.Once());
            customerDao.Verify(dao => dao.FindByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task UpdateValidCustomer()
        {
            var expected = new Customer(1, "fn", "ln", "email");
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(true);
            customerDao.Setup(dao => dao.UpdateAsync(expected)).ReturnsAsync(true);

            var res = await customerLogic.UpdateCustomer(expected);
            Assert.Equal(res, true);
            shopDao.Verify(dao => dao.ShopExists(1), Times.Once());
            customerDao.Verify(dao => dao.UpdateAsync(expected), Times.Once());
        }

        [Fact]
        public async Task UpdateInValidCustomer()
        {
            var expected = new Customer(1, "fn", "ln", "email");
            shopDao.Setup(dao => dao.ShopExists(1)).ReturnsAsync(false);
            customerDao.Setup(dao => dao.UpdateAsync(expected)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => customerLogic.UpdateCustomer(expected));
            shopDao.Verify(dao => dao.ShopExists(1), Times.Once());
        }

    }
}
