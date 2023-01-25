using CaaS.Core.Interfaces;
using CaaS.Core;
using Data_Access_Layer.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Xunit;

namespace CaaSCoreTests
{
    public class CartLogicTests
    {
        private readonly Mock<ICartDao> cartDao;
        private readonly Mock<IProductDao> productDao;
        private readonly Mock<ICustomerDao> customerDao;
        private readonly Mock<IDiscountDao> discountDao;
        private readonly Mock<ICommonDao> commonDao;
        private readonly ICartManagementLogic cartLogic;

        public CartLogicTests()
        {
            cartDao = new Mock<ICartDao>();
            productDao = new Mock<IProductDao>();
            customerDao = new Mock<ICustomerDao>();
            discountDao = new Mock<IDiscountDao>();
            commonDao = new Mock<ICommonDao>();
            cartLogic = new CartManagementLogic(cartDao.Object,productDao.Object,customerDao.Object,discountDao.Object,commonDao.Object);
        }

        [Fact]
        public async Task AddValidProductToValidCartOfValidCustomer()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1,1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CartHasProduct(1,1)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.AddProductToCart(1,1,1,1));

            await cartLogic.AddProductToCart(1,1,1,1);

            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            productDao.Verify(dao => dao.ProductExist(1), Times.Once());
            cartDao.Verify(dao => dao.CustomerOwnsCart(1, 1), Times.Once());
            cartDao.Verify(dao => dao.CartHasProduct(1, 1), Times.Once());
            cartDao.Verify(dao => dao.AddProductToCart(1, 1, 1, 1), Times.Once());
        }

        [Fact]
        public async Task AddInValidProductToValidCartOfValidCustomer()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(2)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CartHasProduct(1, 1)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.AddProductToCart(1, 1, 1, 1));

            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.AddProductToCart(2, 1, 1, 1));
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            productDao.Verify(dao => dao.ProductExist(2), Times.Once());
        }

        [Fact]
        public async Task AddValidProductToInValidCartOfValidCustomer()
        {
            cartDao.Setup(dao => dao.CartExist(2)).ReturnsAsync(false);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CartHasProduct(1, 1)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.AddProductToCart(1, 1, 1, 1));

            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.AddProductToCart(1, 1, 2, 1));
            cartDao.Verify(dao => dao.CartExist(2), Times.Once());
        }

        [Fact]
        public async Task AddValidProductToValidCartOfInValidCustomer()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(2, 1)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.CartHasProduct(1, 1)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.AddProductToCart(1, 1, 1, 1));

            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.AddProductToCart(1, 1, 1, 2));
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            productDao.Verify(dao => dao.ProductExist(1), Times.Once());
            cartDao.Verify(dao => dao.CustomerOwnsCart(2, 1), Times.Once());
        }

        [Fact]
        public async Task AddDuplicateProductToValidCartOfValidCustomer()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CartHasProduct(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.AddProductToCart(1, 1, 1, 1));

            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.AddProductToCart(1, 1, 1, 1));
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            productDao.Verify(dao => dao.ProductExist(1), Times.Once());
            cartDao.Verify(dao => dao.CustomerOwnsCart(1, 1), Times.Once());
            cartDao.Verify(dao => dao.CartHasProduct(1, 1), Times.Once());
        }

        [Fact]
        public async Task GetValidCartById()
        {
            var expected = new Cart(1, 1);
            cartDao.Setup(dao => dao.FindByIdAsync(1)).ReturnsAsync(expected);

            var res = await cartLogic.GetCartById(1);

            Assert.Equal(expected, res);
            cartDao.Verify(dao => dao.FindByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task CreateValidCart()
        {
            var expected = new Cart(1, 1);
            cartDao.Setup(dao => dao.InsertAsync(expected)).ReturnsAsync(1);

            await cartLogic.CreateCart(expected, 1);

            cartDao.Verify(dao => dao.InsertAsync(expected), Times.Once());
        }

        [Fact]
        public async Task CreateInValidCart()
        {
            var expected = new Cart(1, 1);
            cartDao.Setup(dao => dao.InsertAsync(expected)).ReturnsAsync(1);

            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.CreateCart(expected,2));
        }

        [Fact]
        public async Task CreateValidOrderFromValidCartWith1Rule()
        {
            var expected = new Cart(1, 1);

            var products = new List<ProductWithQty>();
            products.Add(new ProductWithQty(1, "asdad", 12, "asdasd", "asdasd", 0, 11, 1));
            cartDao.Setup(dao => dao.GetProductsDetailedInCart(1)).ReturnsAsync(products);

            var discounts = new List<Discount>();
            var d1 = new Discount(1, 10, 0, 1);
            d1.rule = "idProduct";
            discounts.Add(d1);
            discountDao.Setup(dao => dao.GetDiscountsOfShop(1)).ReturnsAsync(discounts);

            commonDao.Setup(dao => dao.CheckDiscountCondition(d1.rule,1)).ReturnsAsync(true);

            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CreateOrderFromCart(expected,10,122)).ReturnsAsync(1);

            var res = await cartLogic.CreateOrderFromCart(expected,1);
            cartDao.Verify(dao => dao.GetProductsDetailedInCart(1), Times.Once());
            discountDao.Verify(dao => dao.GetDiscountsOfShop(1), Times.Once());
            commonDao.Verify(dao => dao.CheckDiscountCondition(d1.rule, 1), Times.Once());
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            cartDao.Verify(dao => dao.CustomerOwnsCart(1, 1), Times.Once());
            cartDao.Verify(dao => dao.CreateOrderFromCart(expected, 122, 10), Times.Once());
        }

        [Fact]
        public async Task CreateValidOrderFromValidCartWith2Rules()
        {
            var expected = new Cart(1, 1);

            var products = new List<ProductWithQty>();
            products.Add(new ProductWithQty(1, "asdad", 12, "asdasd", "asdasd", 0, 11, 1));
            cartDao.Setup(dao => dao.GetProductsDetailedInCart(1)).ReturnsAsync(products);

            var discounts = new List<Discount>();
            var d1 = new Discount(1, 10, 0, 1);
            d1.rule = "idProduct";
            var d2 = new Discount(2, 11, 1, 1);
            d2.rule = "s";
            discounts.Add(d1);
            discounts.Add(d2);
            discountDao.Setup(dao => dao.GetDiscountsOfShop(1)).ReturnsAsync(discounts);

            commonDao.Setup(dao => dao.CheckDiscountCondition(d1.rule, 1)).ReturnsAsync(true);
            commonDao.Setup(dao => dao.CheckDiscountCondition(d2.rule, 0)).ReturnsAsync(true);

            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CreateOrderFromCart(expected, (float)107.480003, (float)24.5200005)).ReturnsAsync(1);

            var res = await cartLogic.CreateOrderFromCart(expected, 1);
            cartDao.Verify(dao => dao.GetProductsDetailedInCart(1), Times.Once());
            discountDao.Verify(dao => dao.GetDiscountsOfShop(1), Times.Once());
            commonDao.Verify(dao => dao.CheckDiscountCondition(d1.rule, 1), Times.Once());
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            cartDao.Verify(dao => dao.CustomerOwnsCart(1, 1), Times.Once());
            cartDao.Verify(dao => dao.CreateOrderFromCart(expected, (float)107.480003, (float)24.5200005), Times.Once());
        }

        [Fact]
        public async Task DeleteValidCart()
        {
            var expected = new Cart(1, 1);
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1,1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.DeleteCart(expected)).ReturnsAsync(true);

            var res = await cartLogic.DeleteCart(expected, 1);
            Assert.Equal(res, true);

            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            cartDao.Verify(dao => dao.CustomerOwnsCart(1, 1), Times.Once());
            cartDao.Verify(dao => dao.DeleteCart(expected), Times.Once());
        }

        [Fact]
        public async Task GetCartsOfValidCustomer()
        {
            var expected = new List<Cart>();
            expected.Add(new Cart(1, 1));
            customerDao.Setup(dao => dao.FindAllCartsByCustomerIdAsync(1)).ReturnsAsync(expected);

            var res = await cartLogic.GetCartsOfCustomer(1);
            Assert.Equal(1, expected.Count());
            Assert.Contains(expected.ElementAt(0), res);
        }

        [Fact]
        public async Task GetProductsOfValidCart()
        {
            var expected = new List<ProductWithQty>();
            expected.Add(new ProductWithQty(1,"asd",1,"asd","asd",0,1,1));
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.GetProductsDetailedInCart(1)).ReturnsAsync(expected);
            //productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            //productDao.Setup(dao => dao.ProductNotDeleted(1)).ReturnsAsync(false);


            var res = await cartLogic.GetProductsOfCart(1);
            Assert.Equal(1, expected.Count());
            Assert.Contains(expected.ElementAt(0), res);
        }

        [Fact]
        public async Task RemoveValidProductFromValidCart()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductNotDeleted(1)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1,1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.RemoveProductFromCart(1,1)).ReturnsAsync(true);


            var res = await cartLogic.RemoveProductFromCart(1,1,1);
            Assert.Equal(res, true);
        }

        [Fact]
        public async Task RemoveInValidProductFromValidCart()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(false);
            productDao.Setup(dao => dao.ProductNotDeleted(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.RemoveProductFromCart(1, 1)).ReturnsAsync(true);


            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.RemoveProductFromCart(1,1, 1));
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
            productDao.Verify(dao => dao.ProductExist(1), Times.Once());
        }

        [Fact]
        public async Task RemoveValidProductFromInValidCart()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(false);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(false);
            productDao.Setup(dao => dao.ProductNotDeleted(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.RemoveProductFromCart(1, 1)).ReturnsAsync(true);


            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.RemoveProductFromCart(1, 1, 1));
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
        }

        [Fact]
        public async Task UpdateQtyOfValidProductInValidCart()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductNotDeleted(1)).ReturnsAsync(false);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.UpdateQtyOfProductInCart(1, 1, 1)).ReturnsAsync(true);


            var res = await cartLogic.UpdateQtyOfProductInCart(1, 1, 1,1);
            Assert.Equal(res, true);
        }

        [Fact]
        public async Task UpdateQtyOfInValidProductValidCart()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(true);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(false);
            productDao.Setup(dao => dao.ProductNotDeleted(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.UpdateQtyOfProductInCart(1, 1, 1)).ReturnsAsync(true);


            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.UpdateQtyOfProductInCart(1, 1, 1, 1));
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
        }

        [Fact]
        public async Task UpdateQtyOfInValidProductInValidCart()
        {
            cartDao.Setup(dao => dao.CartExist(1)).ReturnsAsync(false);
            productDao.Setup(dao => dao.ProductExist(1)).ReturnsAsync(false);
            productDao.Setup(dao => dao.ProductNotDeleted(1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.CustomerOwnsCart(1, 1)).ReturnsAsync(true);
            cartDao.Setup(dao => dao.UpdateQtyOfProductInCart(1, 1, 1)).ReturnsAsync(true);


            await Assert.ThrowsAsync<ArgumentException>(() => cartLogic.UpdateQtyOfProductInCart(1, 1, 1, 1));
            cartDao.Verify(dao => dao.CartExist(1), Times.Once());
        }
    }
}
