using Data_Access_Layer.Ados;
using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.MySQLDaos;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Transactions;
using System;
using static System.Net.Mime.MediaTypeNames;
using MySqlX.XDevAPI.Common;

namespace DalTests
{
    [TestClass]
    public class CartTester
    {
        private readonly ICartDao cartDao;
        private readonly IOrderDao orderDao;
        public CartTester()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.cartDao = new MySQLCartDao(connectionFactory);
            this.orderDao = new MySQLOrderDao(connectionFactory);
        }

        [TestMethod]
        public async Task TestRemoveProductFromCart()
        {
            int cartId = 1;
            bool res = false;
            Product product = new Product(2, "odio elementum eu interdum eu tincidunt in leo maecenas pulvinar lobortis est phasellus sit amet erat nulla", "http://dummyimage.com/227x100.png/dddddd/000000", 808, "duis aliquam convallis nunc proin at turpis a pede posuere nonummy integer non velit donec diam neque vestibulum eget vulputate ut ultrices vel augue vestibulum ante ipsum", 1);
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                res = await cartDao.RemoveProductFromCart(2, 1);
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task TestCreateOrderFromCart()
        {
            Cart cart = await cartDao.FindByIdAsync(1);
            Order res = null;
            int resultOrderId = 0;
            List<Product> products = (await cartDao.FindAllProductsByCartIdAsync(1)).ToList();
            List<Product> orderProducts = null;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                resultOrderId = await cartDao.CreateOrderFromCart(cart,1,1);
                orderProducts = (await orderDao.FindAllProductsByOrderIdAsync(resultOrderId)).ToList();
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            CollectionAssert.AreEqual(products, orderProducts);
        }

        [TestMethod]
        public async Task TestAddProductToCart()
        {
            Product product = new Product(1, "pellentesque ultrices phasellus id sapien in", "http://dummyimage.com/196x100.png/5fa2dd/ffffff", 858, "in felis donec semper sapien a libero nam dui proin leo odio porttitor id consequat in consequat ut nulla sed accumsan felis ut at dolor quis odio consequat varius integer ac leo pellentesque ultrices mattis odio donec vitae nisi nam ultrices libero non mattis pulvinar nulla pede ullamcorper augue a suscipit nulla elit ac nulla sed vel enim sit amet nunc viverra dapibus nulla suscipit ligula in lacus", 1);
            Cart cart = await cartDao.FindByIdAsync(2);
            List<Product> products = null;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                await cartDao.AddProductToCart(product.idProduct,product.idShop,cart.idCart,cart.idCustomer);
                products = (await cartDao.FindAllProductsByCartIdAsync(2)).ToList();
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            bool res = false;
            foreach(Product product1 in products)
            {
                if (product1.idProduct == 1) res = true;
            }
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task TestInsertAsync()
        {
            Cart expected = new Cart(1,3);
            Cart result = null;
            int id = 0;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                id = (await cartDao.InsertAsync(expected));
                result = (await cartDao.FindByIdAsync(id));
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine(expected.idCart);
            Console.WriteLine(result.idCart);
            Console.WriteLine(id);
            //Assert.AreEqual(expected.idCart, result.idCart);
        }

        [TestMethod]
        public async Task TestFindAllAsync()
        {
            List<Cart> expected = new List<Cart>();
            expected.Add(new Cart(1, 1));
            expected.Add(new Cart(2, 2));
            expected.Add(new Cart(3, 3));
            expected.Add(new Cart(4, 3));

            List<Cart> result = (await cartDao.FindAllAsync()).ToList();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TestDeleteCart()
        {
            Cart expected = new Cart(1, 1);
            bool res = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                res = await cartDao.DeleteCart(expected);
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task TestIncreaseQtyOfProductInCart()
        {
            bool res = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                res = await cartDao.IncreaseQtyOfProductInCart(1, 1);
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task TestDecreaseQtyOfProductInCart()
        {
            bool res = false;
            try
            {
                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                res = await cartDao.DecreaseQtyOfProductInCart(1, 1);
                transaction.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Assert.IsTrue(res);
        }
    }
}