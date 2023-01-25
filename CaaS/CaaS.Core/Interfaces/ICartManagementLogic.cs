using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core.Interfaces
{
    public interface ICartManagementLogic
    {
        Task CreateCart(Cart cart, int customerId);
        Task AddProductToCart(int idProduct, int idShop, int idCart, int idCustomer);
        Task<bool> UpdateQtyOfProductInCart(int productId, int cartId, int qty, int customerId);
        Task<bool> RemoveProductFromCart(int productId, int cartId, int customerId);
        Task<bool> DeleteCart(Cart cart, int customerId);
        Task<Cart> GetCartById(int cartId);
        Task<Result> CalculateCartPrice(Cart cart);
        Task<int> CreateOrderFromCart(Cart cart, int customerId);
        Task<IEnumerable<Cart>> GetCartsOfCustomer(int customerId);
        Task<IEnumerable<ProductWithQty>> GetProductsOfCart(int cartId);
    }
}
