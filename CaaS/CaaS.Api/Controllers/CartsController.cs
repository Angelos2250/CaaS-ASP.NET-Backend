using AutoMapper;
using CaaS.Api.DTOs;
using CaaS.Api.DTOs.ForCreation;
using CaaS.Core.Interfaces;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;

namespace CaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class CartsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICartManagementLogic logic;
        public CartsController(IMapper mapper, ICartManagementLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IEnumerable<CartDto>> GetCartsOfCustomer([FromRoute] int customerId)
        {
            IEnumerable<Cart> carts = await logic.GetCartsOfCustomer(customerId);
            return mapper.Map<IEnumerable<CartDto>>(carts);
        }

        [HttpGet("{cartId}/products")]
        public async Task<IEnumerable<ProductDto>> GetProductsOfCart([FromRoute] int cartId)
        {
            IEnumerable<ProductWithQty> products = await logic.GetProductsOfCart(cartId);
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        [HttpGet("{cartId}/price")]
        public async Task<ResultDTO> GetPriceOfCart([FromRoute] int cartId)
        {
            Result price = await logic.CalculateCartPrice(await logic.GetCartById(cartId));
            return mapper.Map<ResultDTO>(price);
        }

        [HttpPost("{cartId}/updateQtyOf/{productId}/customer/{customerId}")]
        public async Task<ActionResult> UpdateQtyOfProductInCart([FromRoute] int productId, [FromRoute] int cartId, int qty, [FromRoute] int customerId)
        {
            try
            {
                await logic.UpdateQtyOfProductInCart(productId, cartId, qty, customerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            return Accepted();
        }

        [HttpDelete("{cartId}/removeProduct/{productId}/customer/{customerId}")]
        public async Task<ActionResult> RemoveProductFromCart([FromRoute] int productId, [FromRoute] int cartId, [FromRoute] int customerId)
        {
            try
            {
                await logic.RemoveProductFromCart(productId,cartId,customerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            return Accepted();
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<CartDto>> GetCartById([FromRoute] int cartId)
        {
            Cart? cart = await logic.GetCartById(cartId);
            return NotFound();
            return Ok(mapper.Map<CartDto>(cart));
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCart([FromBody] CartForCreationDto cartDto, int customerId)
        {
            Cart cart = mapper.Map<Cart>(cartDto);
            await logic.CreateCart(cart, customerId);
            return CreatedAtAction(
                actionName: nameof(GetCartById),
                routeValues: new { cartId = cart.idCart },
                value: mapper.Map<CartDto>(cart)); //customer.ToDto()
        }

        [HttpPost("{idCart}/addProduct/{idProduct}")]
        public async Task<ActionResult> AddProductToCart(int idProduct, int idShop, int idCart, int idCustomer)
        {
            try
            {
                await logic.AddProductToCart(idProduct, idShop, idCart, idCustomer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            return Accepted();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(int cartId, int customerId)
        {
            try
            {
                await logic.DeleteCart(await logic.GetCartById(cartId),customerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("checkOut/{cartId}")]
        public async Task<ActionResult> CreateOrderFromCart(int cartId,int customerId)
        {
            string s = "";
            try
            {
                s = s + await logic.CreateOrderFromCart(await logic.GetCartById(cartId), customerId);
                await logic.DeleteCart(await logic.GetCartById(cartId), customerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            Response.Headers.Add("order_id", s);
            return Accepted();
        }
    }
}
