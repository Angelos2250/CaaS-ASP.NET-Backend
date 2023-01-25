using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core.Interfaces
{
    public interface IDiscountManagementLogic
    {
        Task<IEnumerable<Discount>> GetDiscountsOfShop(int shopId);
        Task<Discount?> GetDiscountById(int id);
        Task<int> CreateDiscount1(Discount discount, string qty, int AppKey);
        Task<int> CreateDiscount2(Discount discount, string date1, string date2, int AppKey);
        Task<bool> DeleteDiscount(int id, int AppKey);
    }
}
