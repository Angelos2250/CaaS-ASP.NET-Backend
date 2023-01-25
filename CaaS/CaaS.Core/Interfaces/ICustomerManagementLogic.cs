using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core.Interfaces
{
    public interface ICustomerManagementLogic
    {
        Task<IEnumerable<Cart>> FindAllCartsByCustomerIdAsync(int id);
        Task<IEnumerable<Order>> FindAllOrdersByCustomerIdAsync(int id);
        Task<Customer?> FindById(int id);
        Task<Order?> GetLastOrderOfCustomer(int id);
        Task<int> CreateCustomer(Customer customer, int idShop);
        Task<bool> UpdateCustomer(Customer customer);
    }
}
