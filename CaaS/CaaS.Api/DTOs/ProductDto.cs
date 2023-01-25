
namespace CaaS.Api.DTOs
{
    public class ProductDto
    {
        public int idProduct { get; set; }
        public string shortDesc { get; set; }
        public string downloadLink { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public int idShop { get; set; }
        public int qty { get; set; }
        public int deletedFlag { get; set; }

    }
}
