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
    public class ShopOwnerManagementLogic : IShopOwnerManagement
    {
        private readonly IShopOwnerDao shopOwnerDao;
        public ShopOwnerManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.shopOwnerDao = new MySQLShopOwnerDao(connectionFactory);
        }

        public ShopOwnerManagementLogic(IShopOwnerDao shopOwnerDao)
        {
            this.shopOwnerDao = shopOwnerDao;
        }

        public async Task<int> CreateOwner(ShopOwner shopOwner)
        {
            if (await shopOwnerDao.ShopOwnerExists(shopOwner.idShopOwner) == true) throw new ArgumentException("ShopOwner already exist");
            return await shopOwnerDao.InsertAsync(shopOwner);
        }

        public async Task<ShopOwner?> FindById(int id)
        {
            if (await shopOwnerDao.ShopOwnerExists(id) == false) throw new ArgumentException("ShopOwner does not exist");
            return await shopOwnerDao.FindByIdAsync(id);
        }

        public async Task<Shop?> FindShopByShopOwnerId(int id)
        {
            if (await shopOwnerDao.ShopOwnerExists(id) == false) throw new ArgumentException("ShopOwner does not exist");
            return await shopOwnerDao.FindShopByShopOwnerIdAsync(id);
        }

        public async Task<bool> UpdateOwner(ShopOwner shopOwner)
        {
            if (await shopOwnerDao.ShopOwnerExists(shopOwner.idShopOwner) == false) throw new ArgumentException("ShopOwner does not exist");
            return await shopOwnerDao.UpdateAsync(shopOwner);
        }
    }
}
