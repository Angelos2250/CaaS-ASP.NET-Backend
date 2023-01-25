using Newtonsoft.Json;

namespace CaaS.Api.DTOs.ForCreation
{
    public record ProductForCreationDto
    {
        public int idProduct { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string shortDesc { get; set; } = null!;
        [JsonProperty(Required = Required.Always)]
        public string downloadLink { get; set; } = null!;
        [JsonProperty(Required = Required.Always)]
        public int price { get; set; }
        public string description { get; set; } = null!;
        [JsonProperty(Required = Required.Always)]
        public int idShop { get; set; }
    }
}
