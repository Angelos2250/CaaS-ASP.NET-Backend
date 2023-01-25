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
    public class DiscountManagementLogic : IDiscountManagementLogic
    {
        private readonly IDiscountDao discountDao;
        private readonly ICommonDao commonDao;
        public DiscountManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.discountDao = new MySQLDiscountDao(connectionFactory);
            this.commonDao = new MySQLCommonDao(connectionFactory);
        }

        public DiscountManagementLogic(IDiscountDao discountDao, ICommonDao commonDao)
        {
            this.discountDao = discountDao;
            this.commonDao = commonDao;
        }

        public async Task<int> CreateDiscount1(Discount discount, string qty, int AppKey)
        {
            if (commonDao.CheckAppKeyValidity(discount.idShop, AppKey).Result == false) 
                throw new ArgumentException("False AppKey");
            return await discountDao.CreateDiscount1(discount, qty);
        }

        public async Task<int> CreateDiscount2(Discount discount, string date1, string date2, int AppKey)
        {
            if (commonDao.CheckAppKeyValidity(discount.idShop, AppKey).Result == false) throw new ArgumentException("False AppKey");
            return await discountDao.CreateDiscount2(discount, date1, date2);
        }

        public async Task<bool> DeleteDiscount(int id, int AppKey)
        {
            if (commonDao.CheckAppKeyValidity((await discountDao.GetDiscountById(id)).idShop, AppKey).Result == false) throw new ArgumentException("False AppKey");
            return await discountDao.DeleteDiscount(id);
        }

        public Task<Discount?> GetDiscountById(int id)
        {
            return discountDao.GetDiscountById(id);
        }

        public Task<IEnumerable<Discount>> GetDiscountsOfShop(int shopId)
        {
            return discountDao.GetDiscountsOfShop(shopId);
        }
        public Task<bool> CheckAppKeyValidity(int shopId, int AppKey)
        {
            return commonDao.CheckAppKeyValidity(shopId, AppKey);
        }
    }
}
