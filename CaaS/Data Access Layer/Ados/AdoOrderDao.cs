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
    public abstract class AdoOrderDao : IOrderDao
    {
        private readonly AdoTemplate template;
        protected abstract string LastInsertedIdQuery { get; }

        public AdoOrderDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        public virtual async Task<IEnumerable<Order>> FindAllAsync()
        {
            return await template.QueryAsync("select * from `Order`", Mappers.Mappers.MapRowToOrder);
        }

        public virtual async Task<IEnumerable<Product>> FindAllProductsByOrderIdAsync(int id)
        {
            return await template.QueryAsync("select * from Product " +
                "                           join Order_has_Product on Product.IdProduct = Order_has_Product.IdProduct" +
                "                           where Order_has_Product.IdOrder = @id",
                                            Mappers.Mappers.MapRowToProduct,
                                            new QueryParameter("@id", id));
        }

        public virtual async Task<Order?> FindByIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from `Order` where idOrder=@id",
                Mappers.Mappers.MapRowToOrder,
                new QueryParameter("@id", id));
        }

        public virtual async Task<int> InsertAsync(Order order)
        {
            const string SQL_INSERT = @"insert into `Order` (dateOfOrder, sumOfDiscount, idCustomer, sumAmount) values(@dateOfOrder, @sumOfDiscount, @idCustomer, @sumAmount)";
            order.idOrder =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@dateOfOrder", order.dateOfOrder),
                    new QueryParameter("@sumOfDiscount", order.sumOfDiscount),
                    new QueryParameter("@idCustomer", order.idCustomer),
                    new QueryParameter("@sumAmount", order.sumAmount)
                    ));
            return order.idOrder;
        }

        public virtual async Task<int> CalculatePrice(Order order)
        {
            int price = 0;
            List<Product> products = (await this.FindAllProductsByOrderIdAsync(order.idOrder)).ToList();
            string sql = "select * from order_has_product";
            List<ProductWithQty> productsWithQty = (await this.GetProductsInOrder(order.idOrder)).ToList();
            foreach(ProductWithQty product in productsWithQty)
            {
                price += product.price * product.qty;
            }
            return price;
        }

        public virtual async Task<IEnumerable<ProductWithQty>> GetProductsInOrder(int orderId)
        {
            return await template.QueryAsync("select Product.idProduct,Product.shortDesc,Product.price,Product.description,Product.downloadLink,Product.deletedFlag, Order_has_Product.qty, Product.idShop from Product " +
                "                             Join Order_has_Product on Product.idProduct = Order_has_Product.idProduct" +
                "                             where Order_has_Product.idOrder = @id",
                                            Mappers.Mappers.MapRowToProductWithQty,
                                            new QueryParameter("@id", orderId));
        }

        public virtual async Task<bool> OrderExists(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM `Order` WHERE idOrder = @idOrder)",
                                new QueryParameter("@idOrder", id)
                                )) == 1;
        }
    }
}
