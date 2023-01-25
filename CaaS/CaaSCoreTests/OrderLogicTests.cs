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
    public class OrderLogicTests
    {
        private readonly Mock<IOrderDao> orderDao;
        private readonly IOrderManagementLogic orderLogic;

        public OrderLogicTests()
        {
            orderDao = new Mock<IOrderDao>();
            orderLogic = new OrderManagementLogic(orderDao.Object);
        }

        [Fact]
        public async Task GetValidOrderWithId()
        {
            var expected = new Order(1, DateTime.Parse("2022-11-11"), 11, 1, 1);
            orderDao.Setup(dao => dao.OrderExists(1)).ReturnsAsync(true);
            orderDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);

            var res = await orderLogic.FindByIdAsync(1);

            Assert.Equal(expected, res);
            orderDao.Verify(dao => dao.OrderExists(1), Times.Once());
            orderDao.Verify(dao => dao.FindByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetInValidOrderWithId()
        {
            var expected = new Order(1, DateTime.Parse("2022-11-11"), 11, 1, 1);
            orderDao.Setup(dao => dao.OrderExists(11)).ReturnsAsync(false);
            orderDao.Setup(dao => dao.FindByIdAsync(11)).ReturnsAsync(expected);

            Func<Task> act = () => orderLogic.FindByIdAsync(11);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Contains("Order does not exist", ex.Message);
            orderDao.Verify(dao => dao.OrderExists(11), Times.Once());
        }

        [Fact]
        public async Task GetAllCustomersWithValidShopId()
        {
            List<ProductWithQty> expected = new List<ProductWithQty>();
            expected.Add(new ProductWithQty(1, "test", 11, "asd", "asdasd",1,12,1));
            orderDao.Setup(dao => dao.OrderExists(1)).ReturnsAsync(true);
            orderDao.Setup(dao => dao.GetProductsInOrder(1)).ReturnsAsync(expected);

            var res = await orderLogic.GetProductsInOrder(1);

            Assert.Equal(1, expected.Count());
            Assert.Contains(expected.ElementAt(0), res);
            orderDao.Verify(dao => dao.GetProductsInOrder(1), Times.Once());
        }
    }
}
