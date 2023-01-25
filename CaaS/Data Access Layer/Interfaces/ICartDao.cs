using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface ICartDao
    {
        Task<IEnumerable<Cart>> FindAllAsync();
        Task<Cart?> FindByIdAsync(int id);
        Task<IEnumerable<Product>> FindAllProductsByCartIdAsync(int id);
        Task AddProductToCart(int idProduct, int idShop, int idCart, int idCustomer);
        Task<bool> RemoveProductFromCart(int productId, int cartId);
        Task<int> CreateOrderFromCart(Cart cart, float price, float sumOfDiscount);
        Task<bool> DeleteCart(Cart cart);
        Task<int> InsertAsync(Cart cart);
        Task<bool> CartExist(int id);
        Task<IEnumerable<ProductWithQty>> GetProductsDetailedInCart(int cartId);
        Task<bool> CartHasProduct(int cartId, int productId);
        Task<bool> IncreaseQtyOfProductInCart(int productId, int cartId);
        Task<bool> DecreaseQtyOfProductInCart(int productId, int cartId);
        Task<bool> UpdateQtyOfProductInCart(int productId, int cartId, int qty);
        Task<bool> CustomerOwnsCart(int customerId, int cartId);
    }
}
