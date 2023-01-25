using CaaS.Core.Interfaces;
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

namespace CaaS.Core
{
    public class AnalyticsManagementLogic : IAnalyticsManagementLogic
    {
        private readonly IAnalyticsDao analyticsDao;
        public AnalyticsManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.analyticsDao = new MySQLAnalyticsDao(connectionFactory);
        }
        public Task<int> FindAvgSalesPerMonthInShopAsync(int id, int year, int month)
        {
            return analyticsDao.FindAvgSalesPerMonthInShopAsync(id, year, month);
        }

        public Task<int> FindAvgSalesPerYearInShopAsync(int id, int year)
        {
            return analyticsDao.FindAvgSalesPerYearInShopAsync(id, year);
        }

        public Task<int> FindCountCartsInShopAsync(int id)
        {
            return analyticsDao.FindCountCartsInShopAsync(id);
        }

        public Task<IEnumerable<ProductWithQty>> FindMostBoughtProductInShopAsync(int id, int year, int month)
        {
            return analyticsDao.FindMostBoughtProductInShopAsync(id,year,month);
        }
    }
}
