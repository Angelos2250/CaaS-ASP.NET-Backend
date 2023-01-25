using Data_Access_Layer.Common;
using Data_Access_Layer.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Ados
{
    public abstract class AdoAnalyticsDao : IAnalyticsDao
    {
        private readonly AdoTemplate template;

        public AdoAnalyticsDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        public virtual async Task<IEnumerable<ProductWithQty>> FindMostBoughtProductInShopAsync(int id, int year, int month)
        {
            string sql = "select idProduct,(count(idProduct) * qty) as bought from `Order` join order_has_product on `Order`.idOrder = order_has_product.idOrder where year(dateOfOrder) = @year and month(dateOfOrder)= @month and idShop = @id group by idProduct;";
            return (await template.QueryAsync(
                    sql,
                    Mappers.Mappers.MapRowToProductWithQtyWithoutPrice,
                    new QueryParameter("@year",year),
                    new QueryParameter("@month",month),
                    new QueryParameter("@id",id)
                    ));
        }

        public virtual async Task<int> FindCountCartsInShopAsync(int id)
        {
            string sql = "select count(*) from cart join cart_has_product on cart.idCart = cart_has_product.idCart where idShop = @id group by idShop ;";
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    sql,
                    new QueryParameter("@id", id)
                    ));
        }


        public virtual async Task<int> FindAvgSalesPerMonthInShopAsync(int id, int year, int month)
        {
            string sql = "select Avg(price*qty) from `Order` Join order_has_product on `Order`.idOrder = order_has_product.idOrder Join Product on product.idProduct = order_has_product.idProduct where order_has_product.idShop=@id and month(dateOfOrder) = @month and year(dateOfOrder) = @year group by month(dateOfOrder) order by month(dateOfOrder);";
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    sql,
                    new QueryParameter("@year", year),
                    new QueryParameter("@month", month),
                    new QueryParameter("@id", id)
                    ));
        }

        public virtual async Task<int> FindAvgSalesPerYearInShopAsync(int id, int year)
        {
            string sql = "Select avg(avg2) " +
                "From ( " +
                "SELECT IFNULL(Avg(price*qty), 0) as avg2 " +
                "FROM ( " +
                "SELECT 'Jan' AS MONTH UNION " +
                "SELECT 'Feb' AS MONTH UNION " +
                "SELECT 'Mar' AS MONTH UNION " +
                "SELECT 'Apr' AS MONTH UNION " +
                "SELECT 'May' AS MONTH UNION " +
                "SELECT 'Jun' AS MONTH UNION " +
                "SELECT 'Jul' AS MONTH UNION " +
                "SELECT 'Aug' AS MONTH UNION " +
                "SELECT 'Sep' AS MONTH UNION " +
                "SELECT 'Oct' AS MONTH UNION " +
                "SELECT 'Nov' AS MONTH UNION " +
                "SELECT 'Dec' AS MONTH) AS m " +
                "LEFT JOIN `Order` ON MONTH(STR_TO_DATE(CONCAT(m.month, ' 2021'),'%M %Y')) = MONTH(`Order`.dateOfOrder) AND YEAR(`Order`.dateOfOrder) = @year" +
                " left Join order_has_product on `Order`.idOrder = order_has_product.idOrder " +
                "left Join Product on product.idProduct = order_has_product.idProduct " +
                "where order_has_product.idShop=@id OR order_has_product.idShop IS NULL GROUP BY m.month ORDER BY 1+1) AS T;";
            return Convert.ToInt32(await template.ExecuteScalarAsync<object>(
                    sql,
                    new QueryParameter("@year", year),
                    new QueryParameter("@id", id)
                    ));
        }
    }
}
