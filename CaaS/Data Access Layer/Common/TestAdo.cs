using Data_Access_Layer.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Common
{
    public class TestAdo
    {
        private readonly IShopDao shopDao;
        public TestAdo(IShopDao shopDao)
        {
            this.shopDao = shopDao;
        }

        public async Task FindAllAsync()
        {
            (await shopDao.FindAllAsync()).ToList().ForEach(s => Console.WriteLine(s));
        }

    }
}
