using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core.Interfaces
{
    public interface IShopOwnerManagement
    {
        Task<ShopOwner?> FindById(int id);
        Task<Shop?> FindShopByShopOwnerId(int id);
        Task<bool> UpdateOwner(ShopOwner shopOwner);
        Task<int> CreateOwner(ShopOwner shopOwner);
    }
}
