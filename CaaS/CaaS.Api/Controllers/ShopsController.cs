using AutoMapper;
using CaaS.Api.DTOs;
using CaaS.Api.DTOs.ForCreation;
using CaaS.Core.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ShopsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IShopManagementLogic logic;

        public ShopsController(IMapper mapper, IShopManagementLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        [HttpGet("{shopId}")]
        public async Task<ShopDto> GetShopById([FromRoute] int shopId)
        {
            Shop? shop = await logic.FindByIdAsync(shopId);
            return mapper.Map<ShopDto>(shop);
        }

        [HttpGet("{shopId}/shopOwner")]
        public async Task<ShopOwnerDto> GetShopOwnerByShopId([FromRoute] int shopId)
        {
            ShopOwner? shop = await logic.FindShopOwnerByIdAsync(shopId);
            return mapper.Map<ShopOwnerDto>(shop);
        }

        [HttpGet("{shopId}/customers")]
        public async Task<IEnumerable<CustomerDto>> GetCustomersByShopId([FromRoute] int shopId)
        {
            IEnumerable<Customer>? customers = await logic.FindAllCustomersByShopIdAsync(shopId);
            return mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        [HttpGet("{shopId}/products")]
        public async Task<IEnumerable<ProductDto>> GetProductsByShopId([FromRoute] int shopId)
        {
            IEnumerable<Product>? customers = await logic.FindAllProductsByShopIdAsync(shopId);
            return mapper.Map<IEnumerable<ProductDto>>(customers);
        }

        [HttpPost]
        public async Task<ActionResult<ShopDto>> CreateProduct([FromBody] ShopForCreationDto shopDto)
        {
            Shop shop = mapper.Map<Shop>(shopDto);
            try
            {
                await logic.CreateShop(shop);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(
                actionName: nameof(GetShopById),
                routeValues: new { shopId = shop.idShop },
                value: mapper.Map<ShopDto>(shop)); //customer.ToDto()
        }

        [HttpPost("update-shop")]
        public async Task<ActionResult> UpdateShop([FromBody] ShopDto shopDto, int AppKey)
        {
            Shop shop = mapper.Map<Shop>(shopDto);
            try
            {
                await logic.UpdateShop(shop,AppKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            return Accepted();
        }
    }
}
