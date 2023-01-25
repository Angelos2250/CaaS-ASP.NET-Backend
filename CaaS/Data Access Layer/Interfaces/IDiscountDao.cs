using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data_Access_Layer.Interfaces
{
    public interface IDiscountDao
    {
        Task<IEnumerable<Discount>> GetDiscountsOfShop(int shopId);
        Task<int> CreateDiscount1(Discount discount, string qty);
        Task<Discount?> GetDiscountById(int id);
        Task<int> CreateDiscount2(Discount discount, string date1, string date2);
        Task<bool> DeleteDiscount(int id);
        Task<bool> DiscountExists(int id);
    }
}
