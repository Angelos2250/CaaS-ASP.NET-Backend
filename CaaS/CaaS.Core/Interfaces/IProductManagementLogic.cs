using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core.Interfaces
{
    public interface IProductManagementLogic
    {
        Task<IEnumerable<Product>> GetProductsOfShop(int shopId);
        Task<bool> ProductExist(int idProduct, int shopId);
        Product EnsureProductExist(Product product);
        Task<Product> GetProductById(int id);
        Task<int> AddProduct(Product product, int AppKey);
        Task<bool> UpdateProduct(Product product, int AppKey);
        Task<bool> DeleteProduct(int id, int AppKey);
        Task<IEnumerable<Product>> FindByFullTextSearch(string fullTextSearch, int shopId);
        Task<bool> CheckAppKeyValidity(int shopId, int AppKey);
        //1. Shopadmin ist angemeldet. 2. Will was an seinem shop ändern. 3. 

    }
}
