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
    public class ShopTester
    {
        private readonly IShopDao shopDao;
        public ShopTester()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.shopDao = new MySQLShopDao(connectionFactory);
        }

        [TestMethod]
        public async Task TestFindAllAsync()
        {
            List<Shop> expected = new List<Shop>();
            expected.Add(new Shop(1, "Stracke-Schulist", 1));
            expected.Add(new Shop(2, "Homenick-Ledner", 2));

            List<Shop> result = (await shopDao.FindAllAsync()).ToList();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindAllCustomersByShopIdAsync()
        {
            List<Customer> expected = new List<Customer>();
            expected.Add(new Customer(1, "Virgil", "Turmel", "vturmel0@ameblo.jp"));
            expected.Add(new Customer(2, "Lela", "Balfre", "lbalfre1@opensource.org"));

            List<Customer> result = (await shopDao.FindAllCustomersByShopIdAsync(1)).ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindAllProductsByShopIdAsync()
        {
            List<Product> expected = new List<Product>();
            expected.Add(new Product(1, "pellentesque ultrices phasellus id sapien in", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1));
            expected.Add(new Product(2, "odio elementum eu interdum eu tincidunt in leo maecenas pulvinar lobortis est phasellus sit amet erat nulla", "http://dummyimage.com/227x100.png/dddddd/000000", 808, "duis aliquam convallis nunc proin at turpis a pede posuere nonummy integer non velit donec diam neque vestibulum eget vulputate ut ultrices vel augue vestibulum ante ipsum", 1));
            List<Product> result = (await shopDao.FindAllProductsByShopIdAsync(1)).ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            Shop expected = new Shop(1, "Stracke-Schulist", 1);
            Shop result = (await shopDao.FindByIdAsync(1));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindShopOwnerByIdAsync()
        {
            ShopOwner expected = new ShopOwner(1, "Hazlett", "Bridal", 1);
            ShopOwner result = (await shopDao.FindShopOwnerByIdAsync(1));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestUpdateAsync()
        {
            Shop expected = new Shop(1, "Stracke-SchulistV2", 1);
            Shop result = null;
            bool updateRes = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                updateRes = (await shopDao.UpdateAsync(new Shop(1, "Stracke-SchulistV2", 1)));
                result = (await shopDao.FindByIdAsync(1));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.IsTrue(updateRes);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestInsertAsync()
        {
            Shop expected = new Shop(3, "3rd-Shop", 4);
            Shop result =  null;
            int id = 0;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                id = (await shopDao.InsertAsync(expected));
                result = (await shopDao.FindByIdAsync(id));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.AreEqual(expected.name,result.name);
        }
    }
}