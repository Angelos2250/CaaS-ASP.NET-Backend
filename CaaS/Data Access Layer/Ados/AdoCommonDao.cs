using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Ados
{
    public abstract class AdoCommonDao : ICommonDao
    {
        private readonly AdoTemplate template;

        public AdoCommonDao(IConnectionFactory connectionFactory)
        {
            this.template = new AdoTemplate(connectionFactory);
        }
        public virtual async Task<bool> CheckAppKeyValidity(int shopId, int AppKey)
        {
            //SELECT IF(appKey = 2,1,0) FROM Shop Where idShop = 2;
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT IF(appKey = @appKey,1,0) FROM Shop Where idShop = @idShop",
                                new QueryParameter("@idShop", shopId),
                                new QueryParameter("@appKey", AppKey)
                                )) == 1;
        }

        public virtual async Task<bool> CheckAppKeyAvailability(int AppKey)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM SHOP WHERE AppKey = @appKey)",
                                new QueryParameter("@appKey", AppKey)
                                )) == 1;
        }

        public virtual async Task<bool> CheckDiscountCondition(string sql,int idProduct)
        {
           if (idProduct <= 0)
            {
                return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                    sql
                                    )) == 1;
            }
            else
            {
                return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                sql,
                                new QueryParameter("@idProduct", idProduct)
                                )) == 1;
            }
        }
    }
}
