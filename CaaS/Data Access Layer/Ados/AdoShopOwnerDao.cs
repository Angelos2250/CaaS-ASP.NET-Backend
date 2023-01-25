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
    public abstract class AdoShopOwnerDao : IShopOwnerDao
    {
        private readonly AdoTemplate template;
        protected abstract string LastInsertedIdQuery { get; }
        public AdoShopOwnerDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        public virtual async Task<IEnumerable<ShopOwner>> FindAllAsync()
        {
            return await template.QueryAsync("select * from ShopOwner", Mappers.Mappers.MapRowToShopOwner);
        }

        public virtual async Task<ShopOwner?> FindByIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from ShopOwner where idShopOwner=@id",
                Mappers.Mappers.MapRowToShopOwner,
                new QueryParameter("@id", id));
        }

        public virtual async Task<Shop?> FindShopByShopOwnerIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from Shop join ShopOwner on Shop.idShop = ShopOwner.idShop where ShopOwner.idShopOwner=@id",
                Mappers.Mappers.MapRowToShop,
                new QueryParameter("@id", id));
        }

        public virtual async Task<int> InsertAsync(ShopOwner shopOwner)
        {
            const string SQL_INSERT = @"insert into ShopOwner (firstName,lastName,idShop) values(@firstName,@lastName,@idShop)";
            shopOwner.idShopOwner =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@firstName", shopOwner.firstName),
                    new QueryParameter("@lastName", shopOwner.lastName),
                    new QueryParameter("@idShop", shopOwner.idShop)
                    ));
            return shopOwner.idShopOwner;
        }

        public virtual async Task<bool> UpdateAsync(ShopOwner shopOwner)
        {
            return (await template.ExecuteAsync(
                    "update ShopOwner set firstName=@firstName, lastName=@lastName, idShop=@idShop where idShopOwner = @id",
                    new QueryParameter("@firstName", shopOwner.firstName),
                    new QueryParameter("@lastName", shopOwner.lastName),
                    new QueryParameter("@idShop", shopOwner.idShop),
                    new QueryParameter("@id", shopOwner.idShopOwner)
                    )) == 1;
        }

        public virtual async Task<bool> ShopOwnerExists(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM ShopOwner WHERE idShopOwner = @idShopOwner)",
                                new QueryParameter("@idShopOwner", id)
                                )) == 1;
        }
    }
}
