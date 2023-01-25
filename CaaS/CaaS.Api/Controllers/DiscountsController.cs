using AutoMapper;
using CaaS.Api.DTOs.ForCreation;
using CaaS.Api.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using CaaS.Core.Interfaces;

namespace CaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class DiscountsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IDiscountManagementLogic logic;
        public DiscountsController(IMapper mapper, IDiscountManagementLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        [HttpPost("Discount1")]
        public async Task<ActionResult<DiscountDto>> CreateDiscount1([FromBody] DiscountForCreationDto discount, int qty, int AppKey)
        {
            Discount discount1 = mapper.Map<Discount>(discount);
            try
            {
                await logic.CreateDiscount1(discount1, Convert.ToString(qty), AppKey);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return CreatedAtAction(
                actionName: nameof(GetDiscountsById),
                routeValues: new { discountId = discount1.idDiscount },
                value: mapper.Map<DiscountDto>(discount1)); //customer.ToDto()
        }

        [HttpPost("Discount2")]
        public async Task<ActionResult<DiscountDto>> CreateDiscount2([FromBody] DiscountForCreationDto discount, string date1, string date2, int AppKey)
        {
            Discount discount1 = mapper.Map<Discount>(discount);
            try
            {
                await logic.CreateDiscount2(discount1, date1, date2, AppKey);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return CreatedAtAction(
                actionName: nameof(GetDiscountsById),
                routeValues: new { discountId = discount1.idDiscount },
                value: mapper.Map<DiscountDto>(discount1)); //customer.ToDto()
        }

        [HttpGet("shop/{shopId}")]
        public async Task<IEnumerable<DiscountDto>> GetDiscountsOfShop([FromRoute] int shopId)
        {
            IEnumerable<Discount> discounts = await logic.GetDiscountsOfShop(shopId);
            return mapper.Map<IEnumerable<DiscountDto>>(discounts);
        }

        [HttpGet("{discountId}")]
        public async Task<DiscountDto> GetDiscountsById([FromRoute] int discountId)
        {
            Discount discount = await logic.GetDiscountById(discountId);
            return mapper.Map<DiscountDto>(discount);
        }

        [HttpDelete("{discountId}")]
        public async Task<ActionResult> DeleteDiscount(int discountId, int AppKey)
        {
            try
            {
                await logic.DeleteDiscount(discountId, AppKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            return NoContent();
        }
    }
}
