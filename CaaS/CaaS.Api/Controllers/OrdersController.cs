using AutoMapper;
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
    public class OrdersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IOrderManagementLogic logic;

        public OrdersController(IMapper mapper, IOrderManagementLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        [HttpGet("{orderId}")]
        public async Task<OrderDto> GetOrderById([FromRoute] int orderId)
        {
            Order? order = await logic.FindByIdAsync(orderId);
            return mapper.Map<OrderDto>(order);
        }

        [HttpGet("productsIn/{orderId}")]
        public async Task<IEnumerable<ProductDto>> GetProductsInOrder([FromRoute] int orderId)
        {
            IEnumerable<ProductWithQty> products = await logic.GetProductsInOrder(orderId);
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
