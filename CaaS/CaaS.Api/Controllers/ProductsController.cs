using AutoMapper;
using CaaS.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Data_Access_Layer.Interfaces;
using CaaS.Api.DTOs.ForCreation;
using CaaS.Core.Interfaces;

namespace CaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ProductsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IProductManagementLogic logic;

        public ProductsController(IMapper mapper, IProductManagementLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        [HttpGet("shop/{shopId}")]
        public async Task<IEnumerable<ProductDto>> GetProducts([FromRoute]int shopId)
        {
            IEnumerable<Product> products = await logic.GetProductsOfShop(shopId);
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<ProductDto>> SearchProductsInShop([Required] string fts, [BindRequired] int shopId)
        {
            IEnumerable<Product> products = await logic.FindByFullTextSearch(fts,shopId);
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        [HttpGet("{productId}")]
        public async Task<ProductDto> GetProductById([FromRoute] int productId)
        {
            Product? product = await logic.GetProductById(productId);
            return mapper.Map<ProductDto>(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductForCreationDto productDto, int AppKey)
        {
            if (await logic.ProductExist(productDto.idProduct, productDto.idShop))
            {
                return Conflict();
            }
            Product product = mapper.Map<Product>(productDto);
            await logic.AddProduct(product,AppKey);
            return CreatedAtAction(
                actionName: nameof(GetProductById),
                routeValues: new { productId = product.idProduct },
                value: mapper.Map<ProductDto>(product)); //customer.ToDto()
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId, int AppKey)
        {
            try
            {
                await logic.DeleteProduct(productId, AppKey);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured: " + ex);
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("{productId}/update-product")]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductForCreationDto productDto, int AppKey)
        {
            Product product = mapper.Map<Product>(productDto);
            try
            {
                await logic.UpdateProduct(product, AppKey);
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
