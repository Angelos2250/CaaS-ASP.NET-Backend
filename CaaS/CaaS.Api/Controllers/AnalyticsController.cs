using AutoMapper;
using CaaS.Api.DTOs;
using CaaS.Core.Interfaces;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class AnalyticsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAnalyticsManagementLogic logic;

        public AnalyticsController(IMapper mapper, IAnalyticsManagementLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        [HttpGet("FindAvgSalesPerMonthInShop")]
        public async Task<ResultDTO> FindAvgSalesPerMonthInShopAsync(int id, int year, int month)
        {
            Result res = new Result(await logic.FindAvgSalesPerMonthInShopAsync(id, year, month) + "");
            return mapper.Map<ResultDTO>(res);
        }

        [HttpGet("FindAvgSalesPerMonthInShopForYear")]
        public async Task<IEnumerable<string>> FindAvgSalesPerMonthInShopForYearAsync(int id, int year)
        {
            List<string> resu = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                resu.Add(await logic.FindAvgSalesPerMonthInShopAsync(id, year, i)+"");
            }
            return mapper.Map<IEnumerable<string>>(resu);
        }

        [HttpGet("FindAvgSalesPerYearInShop")]
        public async Task<ResultDTO> FindAvgSalesPerYearInShopAsync(int id, int year)
        {
            Result res = new Result(await logic.FindAvgSalesPerYearInShopAsync(id, year) + "");
            return mapper.Map<ResultDTO>(res);
        }

        [HttpGet("FindCountCartsInShop")]
        public async Task<ResultDTO> FindCountCartsInShopAsync(int id)
        {
            Result res =  new Result(await logic.FindCountCartsInShopAsync(id) + "");
            return mapper.Map<ResultDTO>(res);
        }

        [HttpGet("FindMostBoughtProductInShop")]
        public async Task<IEnumerable<ProductDto>> FindMostBoughtProductInShopAsync(int id, int year, int month)
        {
            IEnumerable<ProductWithQty> products = await logic.FindMostBoughtProductInShopAsync(id, year, month);
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
