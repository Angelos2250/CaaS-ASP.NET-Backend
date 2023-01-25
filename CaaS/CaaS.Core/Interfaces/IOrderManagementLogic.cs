using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core.Interfaces
{
    public interface IOrderManagementLogic
    {
        Task<IEnumerable<ProductWithQty>> GetProductsInOrder(int orderId);
        Task<Order?> FindByIdAsync(int id);
    }
}
