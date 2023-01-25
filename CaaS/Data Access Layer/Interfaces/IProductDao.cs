using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface IProductDao
    {
        Task<IEnumerable<Product>> FindAllAsync(int shopId);
        Task<Product?> FindByIdAsync(int id);
        Task<bool> ProductExist(int id);
        Task<bool> UpdateAsync(Product product);
        Task<int> InsertAsync(Product product);
        Task<bool> DeleteProduct(int id);
        Task<IEnumerable<Product>> FindByFullTextSearch(string fullTextSearch, int shopId);
        Task<bool> ProductNotDeleted(int id);
        Task<bool> ProductExists(int id);
    }
}
