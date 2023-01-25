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
    public class CustomersController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICustomerManagementLogic logic;

        public CustomersController(IMapper mapper, ICustomerManagementLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        [HttpGet("{customerId}/carts")]
        public async Task<IEnumerable<CartDto>> GetCartsOfCustomer([FromRoute] int customerId)
        {
            IEnumerable<Cart> carts = await logic.FindAllCartsByCustomerIdAsync(customerId);
            return mapper.Map<IEnumerable<CartDto>>(carts);
        }

        [HttpGet("{customerId}/orders")]
        public async Task<IEnumerable<OrderDto>> GetOrdersOfCustomer([FromRoute] int customerId)
        {
            IEnumerable<Order> carts = await logic.FindAllOrdersByCustomerIdAsync(customerId);
            return mapper.Map<IEnumerable<OrderDto>>(carts);
        }

        [HttpGet("{customerId}/lastOrder")]
        public async Task<OrderDto> GetLastOrderOfCustomer([FromRoute] int customerId)
        {
            Order order = await logic.GetLastOrderOfCustomer(id: customerId);
            return mapper.Map<OrderDto>(order);
        }
    }
}
