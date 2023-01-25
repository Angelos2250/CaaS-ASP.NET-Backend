using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core.Interfaces
{
    public interface IShopManagementLogic
    {
        Task<Shop?> FindByIdAsync(int id);
        Task<ShopOwner?> FindShopOwnerByIdAsync(int id);
        Task<IEnumerable<Product>> FindAllProductsByShopIdAsync(int id);
        Task<IEnumerable<Customer>> FindAllCustomersByShopIdAsync(int id);
        Task<bool> UpdateShop(Shop shop, int AppKey);
        Task<int> CreateShop(Shop shop);
    }
}
