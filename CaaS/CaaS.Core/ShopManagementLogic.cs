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
    public class ShopManagementLogic : IShopManagementLogic
    {
        private readonly IShopDao shopDao;
        private readonly ICommonDao commonDao;

        public ShopManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.shopDao = new MySQLShopDao(connectionFactory);
            this.commonDao = new MySQLCommonDao(connectionFactory);
        }

        public ShopManagementLogic(IShopDao shopDao, ICommonDao commonDao)
        {
            this.shopDao = shopDao;
            this.commonDao = commonDao;
        }

        public async Task<int> CreateShop(Shop shop)
        {
            if (await commonDao.CheckAppKeyAvailability(shop.appKey) == true) throw new ArgumentException("AppKey is already used");
            return await shopDao.InsertAsync(shop);
        }

        public async Task<IEnumerable<Customer>> FindAllCustomersByShopIdAsync(int id)
        {
            if (await shopDao.ShopExists(id) == false) throw new ArgumentException("Shop does not exist");
            return await shopDao.FindAllCustomersByShopIdAsync(id);
        }

        public async Task<IEnumerable<Product>> FindAllProductsByShopIdAsync(int id)
        {
            if (await shopDao.ShopExists(id) == false) throw new ArgumentException("Shop does not exist");
            return await shopDao.FindAllProductsByShopIdAsync(id);
        }

        public async Task<Shop?> FindByIdAsync(int id)
        {
            if (await shopDao.ShopExists(id) == false) throw new ArgumentException("Shop does not exist");
            return await shopDao.FindByIdAsync(id);
        }

        public async Task<ShopOwner?> FindShopOwnerByIdAsync(int id)
        {
            if (await shopDao.ShopExists(id) == false) throw new ArgumentException("Shop does not exist");
            return await shopDao.FindShopOwnerByIdAsync(id);
        }

        public async Task<bool> UpdateShop(Shop shop, int AppKey)
        {
            if (await shopDao.ShopExists(shop.idShop) == false) throw new ArgumentException("Shop does not exist");
            if (await commonDao.CheckAppKeyValidity(shop.idShop,AppKey) == false) throw new ArgumentException("Wrong AppKey");
            return await shopDao.UpdateAsync(shop);
        }
    }
}
