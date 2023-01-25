using CaaS.Core.Interfaces;
using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.MySQLDaos;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core
{
    public class CustomerManagementLogic : ICustomerManagementLogic
    {
        private readonly ICustomerDao customerDao;
        private readonly IShopDao shopDao;
        public CustomerManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.customerDao = new MySQLCustomerDao(connectionFactory);
            this.shopDao = new MySQLShopDao(connectionFactory);
        }

        public CustomerManagementLogic(IShopDao shopDao, ICustomerDao customerDao)
        {
            this.shopDao = shopDao;
            this.customerDao = customerDao;
        }

        public async Task<int> CreateCustomer(Customer customer, int idShop)
        {
            if (await shopDao.ShopExists(idShop) == false) throw new ArgumentException("Shop does not exist");
            return await customerDao.InsertAsync(customer, idShop);
        }

        public async Task<IEnumerable<Cart>> FindAllCartsByCustomerIdAsync(int id)
        {
            if (await customerDao.CustomerExists(id) == false) throw new ArgumentException("Customer does not exist");
            return await customerDao.FindAllCartsByCustomerIdAsync(id);
        }

        public async Task<IEnumerable<Order>> FindAllOrdersByCustomerIdAsync(int id)
        {
            if (await customerDao.CustomerExists(id) == false) throw new ArgumentException("Customer does not exist");
            return await customerDao.FindAllOrdersByCustomerIdAsync(id);
        }

        public async Task<Customer?> FindById(int id)
        {
            if (await customerDao.CustomerExists(id) == false) throw new ArgumentException("Customer does not exist");
            return await customerDao.FindByIdAsync(id);
        }

        public async Task<Order?> GetLastOrderOfCustomer(int id)
        {
            if (await customerDao.CustomerExists(id) == false) throw new ArgumentException("Customer does not exist");
            return await customerDao.GetLastOrderOfCustomer(id);
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            if (await shopDao.ShopExists(customer.idCustomer) == false) throw new ArgumentException("Shop does not exist");
            return await customerDao.UpdateAsync(customer);
        }
    }
}
