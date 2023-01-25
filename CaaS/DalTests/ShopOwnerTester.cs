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
    public class ShopOwnerTester
    {
        private readonly IShopOwnerDao shopOwnerDao;
        public ShopOwnerTester()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.shopOwnerDao = new MySQLShopOwnerDao(connectionFactory);
        }

        [TestMethod]
        public async Task TestFindAllAsync()
        {
            List<ShopOwner> expected = new List<ShopOwner>();
            expected.Add(new ShopOwner(1, "Hazlett", "Bridal", 1));
            expected.Add(new ShopOwner(2, "Nichols", "Sorby", 2));
            List<ShopOwner> result = (await shopOwnerDao.FindAllAsync()).ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            ShopOwner expected = new ShopOwner(1, "Hazlett", "Bridal", 1);
            ShopOwner result = (await shopOwnerDao.FindByIdAsync(1));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindShopOwnerByIdAsync()
        {
            Shop expected = new Shop(1, "Stracke-Schulist", 1);
            Shop result = (await shopOwnerDao.FindShopByShopOwnerIdAsync(1));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestUpdateAsync()
        {
            ShopOwner expected = new ShopOwner(1, "HazlettV2", "Bridal", 1);
            ShopOwner result = null;
            bool updateRes = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                updateRes = (await shopOwnerDao.UpdateAsync(new ShopOwner(1, "HazlettV2", "Bridal", 1)));
                result = (await shopOwnerDao.FindByIdAsync(1));
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
            ShopOwner expected = new ShopOwner(1, "HazlettV2", "Bridal", 1);
            ShopOwner result = null;
            int id = 0;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                id = (await shopOwnerDao.InsertAsync(expected));
                result = (await shopOwnerDao.FindByIdAsync(id));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.AreEqual(expected, result);
        }
    }
}
