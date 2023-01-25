using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CaaS.Api.DTOs.ForCreation
{
    public record DiscountForCreationDto
    {
        public int idDiscount { get; set; }
        public int type { get; set; }
        public int value { get; set; }
        public int idShop { get; set; }
    }
}
