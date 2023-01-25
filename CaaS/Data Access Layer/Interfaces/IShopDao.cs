using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface IShopDao
    {
        Task<IEnumerable<Shop>> FindAllAsync();
        Task<Shop?> FindByIdAsync(int id);
        Task<ShopOwner?> FindShopOwnerByIdAsync(int id);
        Task<IEnumerable<Product>> FindAllProductsByShopIdAsync(int id);
        Task<IEnumerable<Customer>> FindAllCustomersByShopIdAsync(int id);
        Task<bool> UpdateAsync(Shop shop);
        Task<int> InsertAsync(Shop shop);
        Task<bool> ShopExists(int id);
    }
}
