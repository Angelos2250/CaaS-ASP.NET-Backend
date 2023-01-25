using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface ICommonDao
    {
        Task<bool> CheckAppKeyValidity(int shopId, int AppKey);
        Task<bool> CheckDiscountCondition(string sql, int idProduct);
        Task<bool> CheckAppKeyAvailability(int AppKey);
    }
}
