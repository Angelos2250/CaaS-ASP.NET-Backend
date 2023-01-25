using CaaS.Core.Interfaces;
using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.MySQLDaos;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaaS.Core
{
    public class CartManagementLogic : ICartManagementLogic
    {
        private readonly ICartDao cartDao;
        private readonly IProductDao productDao;
        private readonly ICustomerDao customerDao;
        private readonly IDiscountDao discountDao;
        private readonly ICommonDao commonDao;
        public CartManagementLogic()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConnectionFactory connectionFactory = DefaultConnectionFactory.FromConfiguration("PersonDbConnection");
            this.cartDao = new MySQLCartDao(connectionFactory);
            this.productDao = new MySQLProductDao(connectionFactory);
            this.customerDao = new MySQLCustomerDao(connectionFactory);
            this.discountDao = new MySQLDiscountDao(connectionFactory);
            this.commonDao = new MySQLCommonDao(connectionFactory);
        }

        public CartManagementLogic(ICartDao object1, IProductDao object2, ICustomerDao object3, IDiscountDao object4, ICommonDao object5)
        {
            this.cartDao = object1;
            this.productDao = object2;
            this.customerDao = object3;
            this.discountDao = object4;
            this.commonDao = object5;
        }

        public async Task AddProductToCart(int idProduct, int idShop, int idCart, int idCustomer)
        {
            //Check if Cart exists
            if (await cartDao.CartExist(idCart) == false) throw new ArgumentException("Cart does not exist");
            //Check if Product is not deleted
            if (await productDao.ProductExist(idProduct) == false || await productDao.ProductNotDeleted(idProduct) == true)
                throw new ArgumentException("Product does not exist");
            //Check if customer owns Cart
            if (await cartDao.CustomerOwnsCart(idCustomer, idCart) == false) throw new ArgumentException("You dont own this cart");
            //Check if Product not already in Cart
            if (await cartDao.CartHasProduct(idCart,idProduct) == true) throw new ArgumentException("This Product is already in your cart");
            //add Product to Cart
            await cartDao.AddProductToCart(idProduct, idShop, idCart, idCustomer);
        }

        public async Task<Cart> GetCartById(int cartId)
        {
            return await cartDao.FindByIdAsync(cartId);
        }

        public Task CreateCart(Cart cart,int customerId)
        {
            if(cart.idCustomer != customerId) throw new ArgumentException("Cant create a Cart for another customer");
            if (cart == null) throw new ArgumentNullException("Cart is null");
            cartDao.InsertAsync(cart);
            return Task.CompletedTask;
        }
        public async Task<Result> CalculateCartPrice(Cart cart)
        {
            //Get all Products From Cart
            List<ProductWithQty> products = (await cartDao.GetProductsDetailedInCart(cart.idCart)).ToList();
            //check if Cart empty
            if (products.IsNullOrEmpty()) return new Result("0");
            //Get All Discount Rules
            List<Discount> discountList = (await discountDao.GetDiscountsOfShop(1)).ToList();
            //Check if Rules apply for Products
            float sumPrice = 0;
            float sumDiscount = 0;
            foreach (ProductWithQty product in products)
            {
                sumPrice += product.price * product.qty;
                //Execute Conditions
                foreach (Discount discount in discountList)
                {
                    if (discount.rule.Contains("idProduct"))
                    {
                        if (await commonDao.CheckDiscountCondition(discount.rule, product.idProduct))
                        {
                            if (discount.type == 0)
                            {
                                //price*qty - discount.value
                                sumPrice -= discount.value;
                                sumDiscount += discount.value;
                            }
                            else
                            {
                                //price * qty - (price*qty)*(discount.value/100)
                                sumPrice -= ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                                sumDiscount += ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                            }
                        }
                    }
                    else
                    {
                        if (await commonDao.CheckDiscountCondition(discount.rule, 0))
                        {
                            if (discount.type == 0)
                            {
                                sumPrice -= discount.value;
                                sumDiscount += discount.value;
                            }
                            else
                            {
                                sumPrice -= ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                                sumDiscount += ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                            }
                        }
                    }
                }
            }
            return new Result(sumDiscount + " " + sumPrice);
        }
        public async Task<int> CreateOrderFromCart(Cart cart, int customerId)
        {
            //Get all Products From Cart
            List<ProductWithQty> products = (await cartDao.GetProductsDetailedInCart(cart.idCart)).ToList();
            //check if Cart empty
            if (products.IsNullOrEmpty()) throw new ArgumentException("Your Cart is Empty");
            //Get All Discount Rules
            Console.WriteLine(products.ElementAt(0).idShop);
            List<Discount> discountList = (await discountDao.GetDiscountsOfShop(1)).ToList();
            //Check if Rules apply for Products
            float sumPrice = 0;
            float sumDiscount = 0;
            foreach(ProductWithQty product in products)
            {
                sumPrice += product.price * product.qty;
                //Execute Conditions
                foreach (Discount discount in discountList)
                {
                    if (discount.rule.Contains("idProduct"))
                    {
                        if(await commonDao.CheckDiscountCondition(discount.rule, product.idProduct))
                        {
                            if(discount.type == 0)
                            {
                                //price*qty - discount.value
                                sumPrice -= discount.value;
                                sumDiscount += discount.value;
                            }
                            else
                            {
                                //price * qty - (price*qty)*(discount.value/100)
                                sumPrice -= ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                                sumDiscount += ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                            }
                        }
                    }
                    else
                    {
                        if (await commonDao.CheckDiscountCondition(discount.rule, 0))
                        {
                            if (discount.type == 0)
                            {
                                sumPrice -= discount.value;
                                sumDiscount += discount.value;
                            }
                            else
                            {
                                sumPrice -= ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                                sumDiscount += ((float)product.price * (float)product.qty) * ((float)discount.value / 100);
                            }
                        }
                    }
                }
            }
            if (cart == null || await cartDao.CartExist(cart.idCart) == false) throw new ArgumentException("Cart does not exist");
            //Check if customer owns Cart
            if (await cartDao.CustomerOwnsCart(customerId, cart.idCart) == false) throw new ArgumentException("You dont own this cart");
            //Console.WriteLine(sumDiscount + " " + sumPrice);
            return await cartDao.CreateOrderFromCart(cart,sumPrice,sumDiscount);//with sum of price as param
        }

        public async Task<bool> DeleteCart(Cart cart, int customerId)
        {
            if (cart == null || await cartDao.CartExist(cart.idCart) == false) throw new ArgumentException("Cart does not exist");
            //Check if customer owns Cart
            if (await cartDao.CustomerOwnsCart(customerId, cart.idCart) == false) throw new ArgumentException("You dont own this cart");
            return await cartDao.DeleteCart(cart);
        }

        public async Task<IEnumerable<Cart>> GetCartsOfCustomer(int customerId)
        {
            return await customerDao.FindAllCartsByCustomerIdAsync(customerId);
        }

        public async Task<IEnumerable<ProductWithQty>> GetProductsOfCart(int cartId)
        {
            //Check if Cart exists
            if (await cartDao.CartExist(cartId) == false) throw new ArgumentException("Cart does not exist");
            return await cartDao.GetProductsDetailedInCart(cartId);
        }

        public async Task<bool> RemoveProductFromCart(int productId, int cartId,int customerId)
        {
            //Check if Cart exists
            if (await cartDao.CartExist(cartId) == false) throw new ArgumentException("Cart does not exist");
            //Check if Product is not deleted
            if (await productDao.ProductExist(productId) == false || await productDao.ProductNotDeleted(productId) == true)
                throw new ArgumentException("Product does not exist");
            //Check if customer owns Cart
            if (await cartDao.CustomerOwnsCart(customerId, cartId) == false) throw new ArgumentException("You dont own this cart");
            return await cartDao.RemoveProductFromCart(productId, cartId);
        }

        public async Task<bool> UpdateQtyOfProductInCart(int productId, int cartId, int qty, int customerId)
        {
            //Check if Cart exists
            if (await cartDao.CartExist(cartId) == false) throw new ArgumentException("Cart does not exist");
            //Check if Product is not deleted
            if (await productDao.ProductExist(productId) == false || await productDao.ProductNotDeleted(productId) == true)
                throw new ArgumentException("Product does not exist");
            //Check if customer owns Cart
            if(await cartDao.CustomerOwnsCart(customerId,cartId) == false) throw new ArgumentException("You dont own this cart");

            return await cartDao.UpdateQtyOfProductInCart(productId, cartId, qty);
        }
    }
}
