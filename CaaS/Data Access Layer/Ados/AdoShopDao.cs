using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.Mappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Data_Access_Layer.Ados
{
    public abstract class AdoShopDao : IShopDao
    {
        private readonly AdoTemplate template;
        protected abstract string LastInsertedIdQuery { get; }
        public AdoShopDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        public virtual async Task<IEnumerable<Shop>> FindAllAsync()
        {
            return await template.QueryAsync("select * from Shop", Mappers.Mappers.MapRowToShop);
        }

        public virtual async Task<IEnumerable<Customer>> FindAllCustomersByShopIdAsync(int id)
        {
            return await template.QueryAsync("select * from Customer " +
                "                           join Shop_has_Customer on Customer.IdCustomer = Shop_has_Customer.IdCustomer" +
                "                           where Shop_has_Customer.IdShop = @id",
                                            Mappers.Mappers.MapRowToCustomer,
                                            new QueryParameter("@id",id));
        }

        public virtual async Task<IEnumerable<Product>> FindAllProductsByShopIdAsync(int id)
        {
            return await template.QueryAsync("select * from Product " +
                "                           join Shop on Product.idShop = Shop.idShop" +
                "                           where Shop.idShop = @id",
                                            Mappers.Mappers.MapRowToProduct,
                                            new QueryParameter("@id", id));
        }

        public virtual async Task<Shop?> FindByIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from Shop where idShop=@id",
                Mappers.Mappers.MapRowToShop,
                new QueryParameter("@id", id));
        }

        public virtual async Task<ShopOwner?> FindShopOwnerByIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from ShopOwner where idShop=@id",
                Mappers.Mappers.MapRowToShopOwner,
                new QueryParameter("@id", id));
        }

        public virtual async Task<int> InsertAsync(Shop shop)
        {
            const string SQL_INSERT = @"insert into Shop (name,appKey) values(@name,@appKey)";
            shop.idShop =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@idShop", shop.idShop),
                    new QueryParameter("@name", shop.name),
                    new QueryParameter("@appKey", shop.appKey)
                    ));
            return shop.idShop;
        }

        public virtual async Task<bool> UpdateAsync(Shop shop)
        {
            return (await template.ExecuteAsync(
                    "update shop set name=@name where idShop = @shopId",
                    new QueryParameter("@shopId", shop.idShop),
                    new QueryParameter("@name", shop.name)
                    )) == 1;
        }

        public virtual async Task<bool> ShopExists(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM Shop WHERE idShop = @idShop)",
                                new QueryParameter("@idShop", id)
                                )) == 1;
        }
    }
}
