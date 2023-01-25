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
    public abstract class AdoProductDao : IProductDao
    {
        private readonly AdoTemplate template;
        protected abstract string LastInsertedIdQuery { get; }
        public AdoProductDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        public virtual async Task<IEnumerable<Product>> FindAllAsync(int shopId)
        {
            return await template.QueryAsync($"select * from Product where idShop = @shopId", Mappers.Mappers.MapRowToProduct,
                 new QueryParameter("@shopId", shopId));
        }

        public virtual async Task<Product?> FindByIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from Product where idProduct = @id",
                Mappers.Mappers.MapRowToProduct,
                new QueryParameter("@id", id));
        }

        public virtual async Task<int> InsertAsync(Product product)
        {
            const string SQL_INSERT = @"insert into Product (shortDesc,downloadLink,price,description,idShop) values(@shortDesc,@downloadLink,@price,@description,@idShop)";
            product.idProduct =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@shortDesc", product.shortDesc),
                    new QueryParameter("@downloadLink", product.downloadLink),
                    new QueryParameter("@price", product.price),
                    new QueryParameter("@description", product.description),
                    new QueryParameter("@idShop", product.idShop)
                    ));
            return product.idProduct;
        }

        public virtual async Task<bool> UpdateAsync(Product product)
        {
            return (await template.ExecuteAsync(
                                "update Product set shortDesc=@shortDesc, downloadLink=@downloadLink, price=@price, description=@description where idProduct = @idProduct",
                                new QueryParameter("@shortDesc", product.shortDesc),
                                new QueryParameter("@downloadLink", product.downloadLink),
                                new QueryParameter("@price", product.price),
                                new QueryParameter("@description", product.description),
                                new QueryParameter("@idProduct", product.idProduct)
                                )) == 1;
        }

        //TEST THESE 3
        public virtual async Task<bool> DeleteProduct(int id)
        {
            return (await template.ExecuteAsync(
                                "update Product set deletedFlag=1 where idProduct = @idProduct",
                                new QueryParameter("@idProduct", id)
                                )) == 1;
        }

        public virtual async Task<IEnumerable<Product>> FindByFullTextSearch(string fullTextSearch, int shopId)
        {
            return await template.QueryAsync(
                $"SELECT * FROM Product WHERE MATCH(shortDesc,description) AGAINST (@fts) and idShop = @idShop",
                Mappers.Mappers.MapRowToProduct,
                new QueryParameter("@fts", fullTextSearch),
                new QueryParameter("@idShop", shopId));
        }

        public virtual async Task<bool> ProductExist(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM Product WHERE idProduct = @idProduct)",
                                new QueryParameter("@idProduct", id)
                                )) == 1;
        }

        public virtual async Task<bool> ProductNotDeleted(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT deletedFlag FROM Product WHERE idProduct = @idProduct",
                                new QueryParameter("@idProduct", id)
                                )) == 1;
        }

        public virtual async Task<bool> ProductExists(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM Product WHERE idProduct = @idProduct)",
                                new QueryParameter("@idProduct", id)
                                )) == 1;
        }
    }
}
