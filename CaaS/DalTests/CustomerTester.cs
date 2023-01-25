using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.MySQLDaos;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DalTests
{
    [TestClass]
    public class CustomerTester
    {
        private readonly ICustomerDao customerDao;
        public CustomerTester()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.customerDao = new MySQLCustomerDao(connectionFactory);
        }

        [TestMethod]
        public async Task TestFindAllAsync()
        {
            List<Customer> expected = new List<Customer>();
            expected.Add(new Customer(1, "Virgil", "Turmel", "vturmel0@ameblo.jp"));
            expected.Add(new Customer(2, "Lela", "Balfre", "lbalfre1@opensource.org"));
            expected.Add(new Customer(3, "Maisey", "Bazoge", "mbazoge2@telegraph.co.uk"));

            List<Customer> result = (await customerDao.FindAllAsync()).ToList();

            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public async Task TestFindAllCartsByCustomerIdAsync()
        {
            List<Cart> expected = new List<Cart>();
            expected.Add(new Cart(1, 1));

            List<Cart> result = (await customerDao.FindAllCartsByCustomerIdAsync(1)).ToList();
            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public async Task TestFindAllOrdersByCustomerIdAsync()
        {
            List<Order> expected = new List<Order>();
            expected.Add(new Order(1, DateTime.Parse("2022-11-11"), 11, 1,1));

            List<Order> result = (await customerDao.FindAllOrdersByCustomerIdAsync(1)).ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindAllShopsByCustomerIdAsync()
        {
            List<Shop> expected = new List<Shop>();
            expected.Add(new Shop(1, "Stracke-Schulist", 1));

            List<Shop> result = (await customerDao.FindAllShopsByCustomerIdAsync(1)).ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            Customer expected = new Customer(1, "Virgil", "Turmel", "vturmel0@ameblo.jp");
            Customer result = (await customerDao.FindByIdAsync(1));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestInsertAsync()
        {
            Customer expected = new Customer(4, "Angelos", "Angelis", "sadasd@amesadblo.jp");
            Customer result = null;
            int id = 0;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                id = (await customerDao.InsertAsync(expected,2));
                result = (await customerDao.FindByIdAsync(id));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestUpdateAsync()
        {
            Customer expected = new Customer(1, "Virgila", "Turmel", "vturmel0@ameblo.jp");
            Customer result = null;
            bool updateRes = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                updateRes = (await customerDao.UpdateAsync(new Customer(1, "Virgila", "Turmel", "vturmel0@ameblo.jp")));
                result = (await customerDao.FindByIdAsync(1));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.IsTrue(updateRes);
            Assert.AreEqual(expected, result);
        }

    }
}
