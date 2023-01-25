using Data_Access_Layer.Ados;
using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.MySQLDaos;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Transactions;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace DalTests
{
    [TestClass]
    public class OrderTester
    {
        private readonly IOrderDao orderDao;
        public OrderTester()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.orderDao = new MySQLOrderDao(connectionFactory);
        }

        [TestMethod]
        public async Task TestFindAllAsync()
        {
            List<Order> expected = new List<Order>();
            expected.Add(new Order(1, DateTime.Parse("2022-11-11"), 11, 1,1));
            expected.Add(new Order(2, DateTime.Parse("2021 -11-26"), 22, 2,1));
            expected.Add(new Order(3, DateTime.Parse("2021-12-10"), 11, 3,1));

            List <Order> result = (await orderDao.FindAllAsync()).ToList();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            Order expected = new Order(1, DateTime.Parse("2022-11-11"), 11, 1,1);
            Order result = (await orderDao.FindByIdAsync(1));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestInsertAsync()
        {
            Order expected = new Order(4, DateTime.Parse("2022-11-11"), 13, 2,1);
            Order result = null;
            int id = 0;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                id = (await orderDao.InsertAsync(expected));
                Console.WriteLine(id);
                result = (await orderDao.FindByIdAsync(id));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.AreEqual(expected.idOrder, result.idOrder);
        }

        [TestMethod]
        public async Task TestCalculatePrice()
        {
            Order order = new Order(1, DateTime.Parse("2022-11-11"), 11, 1, 1);
            int expected = 2574 + 1374;
            int price = await orderDao.CalculatePrice(order);
            Assert.AreEqual(expected, price);
        }

    }
}