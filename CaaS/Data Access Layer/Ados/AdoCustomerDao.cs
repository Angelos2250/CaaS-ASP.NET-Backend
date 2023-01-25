using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Ados
{
    public abstract class AdoCustomerDao : ICustomerDao
    {
        private readonly AdoTemplate template;
        protected abstract string LastInsertedIdQuery { get; }
        public AdoCustomerDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        public virtual async Task<IEnumerable<Customer>> FindAllAsync()
        {
            return await template.QueryAsync("select * from Customer", Mappers.Mappers.MapRowToCustomer);
        }

        public virtual async Task<IEnumerable<Cart>> FindAllCartsByCustomerIdAsync(int id)
        {
            return await template.QueryAsync("select * from Cart where idCustomer = @id",
                                            Mappers.Mappers.MapRowToCart,
                                            new QueryParameter("@id", id));
        }

        public virtual async Task<Order?> GetLastOrderOfCustomer(int id)
        {
            return await template.QuerySingleAsync("Select * From `Order` WHERE idCustomer = @id AND idOrder =  (select MAX(idOrder) FROM `Order` WHERE idCustomer = @id)",
                                            Mappers.Mappers.MapRowToOrder,
                                            new QueryParameter("@id", id));
        }

        public virtual async Task<IEnumerable<Order>> FindAllOrdersByCustomerIdAsync(int id)
        {
            return await template.QueryAsync("select * from `Order` where idCustomer = @id",
                                            Mappers.Mappers.MapRowToOrder,
                                            new QueryParameter("@id", id));
        }

        public virtual async Task<IEnumerable<Shop>> FindAllShopsByCustomerIdAsync(int id)
        {
            return await template.QueryAsync("select * from Shop " +
                "                           join Shop_has_Customer on Shop.idShop = Shop_has_Customer.idShop" +
                "                           where Shop_has_Customer.idCustomer = @id",
                                            Mappers.Mappers.MapRowToShop,
                                            new QueryParameter("@id", id));
        }

        public virtual async Task<Customer?> FindByIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from Customer where idCustomer=@id",
                Mappers.Mappers.MapRowToCustomer,
                new QueryParameter("@id", id));
        }

        public virtual async Task<int> InsertAsync(Customer customer, int idShop)
        {
            const string SQL_INSERT = @"insert into Customer (firstName,lastName,email) values(@firstName,@lastName,@email)";
            customer.idCustomer =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@firstName", customer.firstName),
                    new QueryParameter("@lastName", customer.lastName),
                    new QueryParameter("@email", customer.email)
                    ));
            const string SQL_INSERT2 = @"insert into Shop_has_Customer (idShop, idCustomer) values(@idShop, @idCustomer)";
            await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT2}",
                    new QueryParameter("@idShop", idShop),
                    new QueryParameter("@idCustomer", customer.idCustomer)
                    );
            return customer.idCustomer;
        }

        public virtual async Task<bool> UpdateAsync(Customer customer)
        {
            return (await template.ExecuteAsync(
                    "update Customer set firstName=@firstName, lastName=@lastName, email=@email where idCustomer = @idCustomer",
                    new QueryParameter("@idCustomer", customer.idCustomer),
                    new QueryParameter("@firstName", customer.firstName),
                    new QueryParameter("@lastName", customer.lastName),
                    new QueryParameter("@email", customer.email)
                    )) == 1;
        }

        public virtual async Task<bool> CustomerExists(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM Customer WHERE idCustomer = @idCustomer)",
                                new QueryParameter("@idCustomer", id)
                                )) == 1;
        }


    }
}
