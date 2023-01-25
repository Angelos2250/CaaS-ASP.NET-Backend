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
using System.Transactions;

namespace DalTests
{
    [TestClass]
    public class ProductTester
    {
        private readonly IProductDao productDao;
        public ProductTester()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.productDao = new MySQLProductDao(connectionFactory);
        }

        [TestMethod]
        public async Task TestFindAllAsync()
        {
            List<Product> expected = new List<Product>();
            expected.Add(new Product(1, "pellentesque ultrices phasellus id sapien in", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1));
            expected.Add(new Product(2, "odio elementum eu interdum eu tincidunt in leo maecenas pulvinar lobortis est phasellus sit amet erat nulla", "http://dummyimage.com/227x100.png/dddddd/000000", 808, "duis aliquam convallis nunc proin at turpis a pede posuere nonummy integer non velit donec diam neque vestibulum eget vulputate ut ultrices vel augue vestibulum ante ipsum", 1));

            List<Product> result = (await productDao.FindAllAsync(1)).ToList();

            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public async Task TestFTSAsync()
        {
            List<Product> expected = new List<Product>();
            expected.Add(new Product(1, "pellentesque ultrices phasellus id sapien in", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1));

            List<Product> result = (await productDao.FindByFullTextSearch("pellentesque", 1)).ToList();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            Product expected = new Product(1, "pellentesque ultrices phasellus id sapien in", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);

            Product result = (await productDao.FindByIdAsync(1));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestUpdateAsync()
        {
            Product expected = new Product(1, "updated", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            Product result = null;
            bool updateRes = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                updateRes = (await productDao.UpdateAsync(new Product(1, "updated", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1)));
                result = (await productDao.FindByIdAsync(1));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.IsTrue(updateRes);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestInsertAsync()
        {
            Product expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            Product result = null;
            int id = 0;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                id = (await productDao.InsertAsync(expected));
                result = (await productDao.FindByIdAsync(id));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.AreEqual(expected.idProduct, result.idProduct);
        }

        [TestMethod]
        public async Task TestDeleteAsync()
        {
            Product expected = new Product(1, "inserted", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            bool result = false;
            bool id = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                id = (await productDao.DeleteProduct(2));
                result = (await productDao.ProductNotDeleted(2));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.AreEqual(true,result);
        }
    }
}
