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
    public class OrderManagementLogic : IOrderManagementLogic
    {
        private readonly IOrderDao orderDao;
        public OrderManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.orderDao = new MySQLOrderDao(connectionFactory);
        }

        public OrderManagementLogic(IOrderDao orderDao)
        {
            this.orderDao = orderDao;
        }

        public async Task<Order?> FindByIdAsync(int id)
        {
            if (await orderDao.OrderExists(id) == false) throw new ArgumentException("Order does not exist");
            return await this.orderDao.FindByIdAsync(id);
        }

        public async Task<IEnumerable<ProductWithQty>> GetProductsInOrder(int orderId)
        {
            if (await orderDao.OrderExists(orderId) == false) throw new ArgumentException("Order does not exist");
            return await orderDao.GetProductsInOrder(orderId);
        }
    }
}
