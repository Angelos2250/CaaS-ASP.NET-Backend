using CaaS.Core.Interfaces;
using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.MySQLDaos;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core
{
    public class ProductManagementLogic : IProductManagementLogic
    {
        private readonly IProductDao productDao;
        private readonly ICommonDao commonDao;
        private readonly IShopDao shopDao;
        public ProductManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.productDao = new MySQLProductDao(connectionFactory);
            this.commonDao = new MySQLCommonDao(connectionFactory);
            this.shopDao = new MySQLShopDao(connectionFactory);
        }

        public ProductManagementLogic(IProductDao productDao, ICommonDao commonDao, IShopDao shopDao)
        {
            this.productDao = productDao;
            this.commonDao = commonDao;
            this.shopDao = shopDao;
        }

        public async Task<int> AddProduct(Product product, int AppKey)
        {
            if (commonDao.CheckAppKeyValidity(product.idShop, AppKey).Result == false) throw new ArgumentException("False AppKey");
            return await productDao.InsertAsync(product);
        }


        public async Task<bool> DeleteProduct(int id, int AppKey)
        {
            if (await productDao.ProductExists(id) == false) throw new ArgumentException("Product doesnt exist");
            Product toBeDeleted = await productDao.FindByIdAsync(1);
            if (toBeDeleted != null)
            {
                if (commonDao.CheckAppKeyValidity(toBeDeleted.idShop, AppKey).Result == false)
                {
                    throw new ArgumentException("False AppKey");
                }
                else
                {
                    return await productDao.DeleteProduct(id);
                }
            }
            else
                throw new ArgumentException("Product doesnt exist");
        }

        public async Task<IEnumerable<Product>> FindByFullTextSearch(string fullTextSearch, int shopId)
        {
            return await productDao.FindByFullTextSearch(fullTextSearch, shopId);
        }

        public async Task<Product> GetProductById(int id)
        {
            if (await productDao.ProductExists(id) == false) throw new ArgumentException("Product doesnt exist");
            return await productDao.FindByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsOfShop(int shopId)
        {
            if (await shopDao.ShopExists(shopId) == false) throw new ArgumentException("Shop doesnt Exist");
            return await productDao.FindAllAsync(shopId);
        }

        public Product EnsureProductExist(Product product)
        {
            if (!productDao.ProductExist(product.idProduct).Result)
            {
                throw new ArgumentException($"Product with id {product.idProduct} does not exist in shop {product.idShop}");
            }
            return product;
        }

        public async Task<bool> UpdateProduct(Product product, int AppKey)
        {
            if (productDao.ProductExist(product.idProduct).Result == false)
            {
                throw new ArgumentException($"Product with id {product.idProduct} does not exist in shop {product.idShop}");
            }
            if (commonDao.CheckAppKeyValidity(product.idShop, AppKey).Result == false) throw new ArgumentException("False AppKey");
            return await productDao.UpdateAsync(product);
        }

        public async Task<bool> ProductExist(int idProduct, int shopId)
        {
            return await productDao.ProductExist(idProduct);
        }
        public async Task<bool> CheckAppKeyValidity(int shopId, int AppKey)
        {
            return await commonDao.CheckAppKeyValidity(shopId, AppKey);
        }
    }
}
