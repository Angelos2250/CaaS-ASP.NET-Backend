using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface ICustomerDao
    {
        Task<IEnumerable<Customer>> FindAllAsync();
        Task<Customer?> FindByIdAsync(int id);
        Task<IEnumerable<Shop>> FindAllShopsByCustomerIdAsync(int id);
        Task<IEnumerable<Cart>> FindAllCartsByCustomerIdAsync(int id);
        Task<IEnumerable<Order>> FindAllOrdersByCustomerIdAsync(int id);
        Task<Order?> GetLastOrderOfCustomer(int id);
        Task<bool> UpdateAsync(Customer customer);
        Task<int> InsertAsync(Customer customer, int idShop);
        Task<bool> CustomerExists(int id);
    }
}
