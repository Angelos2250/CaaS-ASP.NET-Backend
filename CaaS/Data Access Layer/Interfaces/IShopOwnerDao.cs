using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface IShopOwnerDao
    {
        Task<IEnumerable<ShopOwner>> FindAllAsync();
        Task<ShopOwner?> FindByIdAsync(int id);
        Task<Shop?> FindShopByShopOwnerIdAsync(int id);
        Task<bool> UpdateAsync(ShopOwner shopOwner);
        Task<int> InsertAsync(ShopOwner shopOwner);
        Task<bool> ShopOwnerExists(int id);
    }
}
