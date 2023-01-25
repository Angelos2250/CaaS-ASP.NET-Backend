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
    public class AnalyticsTester
    {
        private readonly IAnalyticsDao analyticsDao;
        public AnalyticsTester()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.analyticsDao = new MySQLAnalyticsDao(connectionFactory);
        }

        [TestMethod]
        public async Task TestFindMostBoughtProductInShopAsync()
        {
            List<ProductWithQty> expected = new List<ProductWithQty>();
            expected.Add(new ProductWithQty(3, 3));
            List<ProductWithQty> res = (await analyticsDao.FindMostBoughtProductInShopAsync(2, 2021, 12)).ToList();
            res.ForEach(s => Console.WriteLine(s.qty + " " + s.idProduct));
            Assert.AreEqual(expected.ElementAt(0).qty, res.ElementAt(0).qty);
            Assert.AreEqual(expected.ElementAt(0).idProduct, res.ElementAt(0).idProduct);
        }

        [TestMethod]
        public async Task TestFindCountCartsInShopAsync()
        {
            int expected = 4;
            int res = await analyticsDao.FindCountCartsInShopAsync(1);
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public async Task TestFindAvgSalesPerMonthInShopAsync()
        {
            int expected = 2424;
            int res = await analyticsDao.FindAvgSalesPerMonthInShopAsync(1, 2021, 11);
            Assert.AreEqual(expected, res);
        }


    }
}
