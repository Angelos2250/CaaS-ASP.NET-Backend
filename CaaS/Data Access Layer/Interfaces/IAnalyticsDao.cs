using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface IAnalyticsDao
    {
        Task<IEnumerable<ProductWithQty>> FindMostBoughtProductInShopAsync(int id, int year, int month);
        Task<int> FindCountCartsInShopAsync(int id);
        Task<int> FindAvgSalesPerMonthInShopAsync(int id, int year, int month);
        Task<int> FindAvgSalesPerYearInShopAsync(int id, int year);


    }
}
