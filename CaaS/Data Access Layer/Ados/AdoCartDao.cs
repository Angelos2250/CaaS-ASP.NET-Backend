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
    public abstract class AdoCartDao : ICartDao
    {
        private readonly AdoTemplate template;
        protected abstract string LastInsertedIdQuery { get; }

        public AdoCartDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        public virtual async Task<bool> DecreaseQtyOfProductInCart(int productId, int cartId)
        {
            return (await template.ExecuteAsync(
                    "update Cart_has_Product set qty=qty-1 where idProduct=@idProduct and idCart=@idCart",
                    new QueryParameter("@idProduct", productId),
                    new QueryParameter("@idCart", cartId)
                    )) == 1;
        }
        public virtual async Task<bool> IncreaseQtyOfProductInCart(int productId, int cartId)
        {
            return (await template.ExecuteAsync(
                    "update Cart_has_Product set qty=qty+1 where idProduct=@idProduct and idCart=@idCart",
                    new QueryParameter("@idProduct", productId),
                    new QueryParameter("@idCart", cartId)
                    )) == 1;
        }

        public virtual async Task AddProductToCart(int idProduct, int idShop, int idCart, int idCustomer)
        {
            const string SQL_INSERT = @"insert into Cart_has_Product (idProduct, idShop, idCart, idCustomer, qty) values(@idProduct, @idShop, @idCart, @idCustomer, @qty)";
            await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};",
                    new QueryParameter("@idProduct", idProduct),
                    new QueryParameter("@idShop", idShop),
                    new QueryParameter("@idCart", idCart),
                    new QueryParameter("@idCustomer", idCustomer),
                    new QueryParameter("@qty", 1)
                    );

        }

        public virtual async Task<int> CreateOrderFromCart(Cart cart, float price, float sumOfDiscount)
        {
            List<ProductWithQty> productsInCart = (await this.GetProductsDetailedInCart(cart.idCart)).ToList();
            Order order = new Order(1, DateTime.Now, 15, cart.idCustomer, price);
            const string SQL_INSERT = @"insert into `Order`(dateOfOrder, sumOfDiscount, idCustomer, sumAmount) values(@dOO, @sOD, @iC, @price)";
            order.idOrder =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@dOO", DateTime.Now),
                    new QueryParameter("@sOD", sumOfDiscount),
                    new QueryParameter("@iC", order.idCustomer),
                    new QueryParameter("@price", price)
                    ));

            List<Cart_has_Product> products = (await this.GetProductsInCart(cart.idCart)).ToList();
            foreach(Cart_has_Product product in products)
            {
                const string SQL_INSERT2 = @"insert into Order_has_Product (idOrder, idProduct, idShop, qty) values(@idOrder, @idProduct, @idShop, @qty)";
                await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT2};",
                    new QueryParameter("@idOrder", order.idOrder),
                    new QueryParameter("@idProduct", product.idProduct),
                    new QueryParameter("@idShop", product.idShop),
                    new QueryParameter("@qty", product.qty)
                    );
            }
            return order.idOrder;
        }

        public virtual async Task<IEnumerable<Cart_has_Product>> GetProductsInCart(int cartId)
        {
            return await template.QueryAsync("select * from Cart_has_Product " +
                "                           where Cart_has_Product.IdCart = @id",
                                            Mappers.Mappers.MapRowToCart_has_Product,
                                            new QueryParameter("@id", cartId));
        }

        public virtual async Task<IEnumerable<ProductWithQty>> GetProductsDetailedInCart(int cartId)
        {
            return await template.QueryAsync("SELECT Product.idProduct,Product.shortDesc,Product.price,Product.description,Product.downloadLink,Product.deletedFlag,Cart_has_Product.qty,Product.idShop From Cart Join Cart_has_Product ON Cart.idCart = Cart_has_Product.idCart Join Product ON Cart_has_Product.idProduct = Product.idProduct WHERE Cart.idCart = @id",
                                            Mappers.Mappers.MapRowToProductWithQty,
                                            new QueryParameter("@id", cartId));
        }

        public virtual async Task<bool> DeleteCart(Cart cart)
        {
            return (await template.ExecuteAsync(
                    "delete from Cart where idCart = @idCart and idCustomer = @idCustomer",
                    new QueryParameter("@idCart", cart.idCart),
                    new QueryParameter("@idCustomer", cart.idCustomer)
                    )) == 1;
        }

        public virtual async Task<IEnumerable<Cart>> FindAllAsync()
        {
            return await template.QueryAsync("select * from Cart", Mappers.Mappers.MapRowToCart);
        }

        public virtual async Task<IEnumerable<Product>> FindAllProductsByCartIdAsync(int id)
        {
            return await template.QueryAsync("select * from Product " +
                "                           join Cart_has_Product on Product.IdProduct = Cart_has_Product.IdProduct" +
                "                           where Cart_has_Product.IdCart = @id",
                                            Mappers.Mappers.MapRowToProduct,
                                            new QueryParameter("@id", id));
        }

        public virtual async Task<Cart?> FindByIdAsync(int id)
        {
            return await template.QuerySingleAsync(
                $"select * from Cart where idCart=@id",
                Mappers.Mappers.MapRowToCart,
                new QueryParameter("@id", id));
        }

        public virtual async Task<int> InsertAsync(Cart cart)
        {
            const string SQL_INSERT = @"insert into Cart (idCustomer) values(@idCustomer)";
            cart.idCart =
                Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    $"{SQL_INSERT};{LastInsertedIdQuery}",
                    new QueryParameter("@idCustomer", cart.idCustomer)
                    ));
            return cart.idCart;
        }

        public virtual async Task<bool> RemoveProductFromCart(int productId, int cartId)
        {
            return (await template.ExecuteAsync(
                    "delete from Cart_has_Product where idCart = @idCart and idProduct = @idProduct",
                    new QueryParameter("@idCart", cartId),
                    new QueryParameter("@idProduct", productId)
                    )) == 1;
        }

        public virtual async Task<bool> CartExist(int id)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM Cart WHERE idCart = @idCart)",
                                new QueryParameter("@idCart", id)
                                )) == 1;
        }

        public virtual async Task<bool> CartHasProduct(int cartId,int productId)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT EXISTS(SELECT * FROM Cart_has_Product WHERE idCart = @idCart AND idProduct = @idProduct)",
                                new QueryParameter("@idCart", cartId),
                                new QueryParameter("@idProduct", productId)
                                )) == 1;
        }

        public virtual async Task<bool> UpdateQtyOfProductInCart(int productId, int cartId, int qty)
        {
            return (await template.ExecuteAsync(
                    "update Cart_has_Product set qty=@qty where idProduct=@idProduct and idCart=@idCart",
                    new QueryParameter("@idProduct", productId),
                    new QueryParameter("@idCart", cartId),
                    new QueryParameter("@qty", qty)
                    )) == 1;
        }

        public virtual async Task<bool> CustomerOwnsCart(int customerId,int cartId)
        {
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                                "SELECT IF(idCustomer = @customerId,1,0) From cart where idCart =@cartId",
                                new QueryParameter("@customerId", customerId),
                                new QueryParameter("@cartId", cartId)
                                )) == 1;
        }
    }
}
