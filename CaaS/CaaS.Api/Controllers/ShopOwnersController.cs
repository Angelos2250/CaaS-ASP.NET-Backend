using AutoMapper;
using CaaS.Api.DTOs.ForCreation;
using CaaS.Api.DTOs;
using CaaS.Core.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ShopOwnersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IShopOwnerManagement logic;

        public ShopOwnersController(IMapper mapper, IShopOwnerManagement logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }
        [HttpGet("{shopOwnerId}/shop")]
        public async Task<ShopDto> GetShopByShopOwnerId([FromRoute] int shopOwnerId)
        {
            Shop? shop = await logic.FindShopByShopOwnerId(shopOwnerId);
            return mapper.Map<ShopDto>(shop);
        }

        [HttpGet("{id}")]
        public async Task<ShopOwnerDto> GetShopOwnerById([FromRoute] int id)
        {
            ShopOwner? shop = await logic.FindById(id);
            return mapper.Map<ShopOwnerDto>(shop);
        }

        [HttpPost]
        public async Task<ActionResult<ShopOwnerDto>> CreateOwner([FromBody] ShopOwnerForCreationDto shopOwnerDto)
        {
            ShopOwner shopOwner = mapper.Map<ShopOwner>(shopOwnerDto);
            try
            {
                await logic.CreateOwner(shopOwner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(
                actionName: nameof(GetShopOwnerById),
                routeValues: new { id = shopOwner.idShopOwner },
                value: mapper.Map<ShopOwnerDto>(shopOwner)); //customer.ToDto()
        }

        [HttpPost("update-ShopOwner")]
        public async Task<ActionResult> UpdateShopOwner([FromBody] ShopOwnerForCreationDto shopDto)
        {
            ShopOwner shopOwner = mapper.Map<ShopOwner>(shopDto);
            try
            {
                await logic.UpdateOwner(shopOwner);
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
