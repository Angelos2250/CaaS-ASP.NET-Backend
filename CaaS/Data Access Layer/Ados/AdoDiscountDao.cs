using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Ados
{
    public abstract class AdoDiscountDao : IDiscountDao
    {
        private readonly AdoTemplate template;
        protected abstract string LastInsertedIdQuery { get; }

        public AdoDiscountDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        public virtual async Task<int> CreateDiscount1(Discount discount, string qty)
        {
            const string SQL_INSERT = @"insert into Discount (rule,type,value,idShop) values(@rule, @type,@value, @idShop)";
            string ruleString = "Select IF(qty >= " + qty + ",1,0) FROM Cart_has_Product Where idProduct = @idProduct";
            discount.idDiscount = Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@rule", ruleString),
                    new QueryParameter("@type", discount.type),
                    new QueryParameter("@value", discount.value),
                    new QueryParameter("@idShop", discount.idShop)
                    ));
            return discount.idDiscount;
        }

        public virtual async Task<int> CreateDiscount2(Discount discount, string date1, string date2)
        {
            const string SQL_INSERT = @"insert into Discount (rule,type,value,idShop) values(@rule, @type,@value, @idShop)";
            string ruleString = "SELECT IF(current_timestamp() BETWEEN \'" + date1 +"\' AND \'" + date2 + "\',1,0)";
            discount.idDiscount =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@rule", ruleString),
                    new QueryParameter("@type", discount.type),
                    new QueryParameter("@value", discount.value),
                    new QueryParameter("@idShop", discount.idShop)
                    ));
            return discount.idDiscount;
        }

        public virtual async Task<bool> DeleteDiscount(int id)
        {
            return (await template.ExecuteAsync(
                    "delete from Discount where idDiscount = @id",
                    new QueryParameter("@id", id)
                    )) == 1;
        }

        public virtual async Task<IEnumerable<Discount>> GetDiscountsOfShop(int shopId)
        {
            return await template.QueryAsync("select * from Discount " +
                "                           where idShop = @id",
                                            Mappers.Mappers.MapRowToDiscount,
                                            new QueryParameter("@id", shopId));
        }

        public virtual async Task<Discount?> GetDiscountById(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from Discount where idDiscount=@id",
                Mappers.Mappers.MapRowToDiscount,
                new QueryParameter("@id", id));
        }
        public virtual async Task<bool> DiscountExists(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM Discount WHERE idDiscount = @idDiscount)",
                                new QueryParameter("@idDiscount", id)
                                )) == 1;
        }
    }
}
