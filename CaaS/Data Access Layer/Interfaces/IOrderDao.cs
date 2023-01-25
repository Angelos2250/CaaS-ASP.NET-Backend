using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface IOrderDao
    {
        Task<IEnumerable<Order>> FindAllAsync();
        Task<Order?> FindByIdAsync(int id);
        Task<IEnumerable<Product>> FindAllProductsByOrderIdAsync(int id);
        Task<int> InsertAsync(Order order);
        Task<int> CalculatePrice(Order order);
        Task<IEnumerable<ProductWithQty>> GetProductsInOrder(int orderId);
        Task<bool> OrderExists(int id);
    }
}
