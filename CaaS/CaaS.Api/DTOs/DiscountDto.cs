using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace CaaS.Api.DTOs
{
    public class DiscountDto
    {
        public int idDiscount { get; set; }
        public string rule { get; set; }
        public int type { get; set; }
        public string value { get; set; }
        public int idShop { get; set; }

    }
}
