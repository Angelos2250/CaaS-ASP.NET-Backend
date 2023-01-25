using Newtonsoft.Json;

namespace CaaS.Api.DTOs.ForCreation
{
    public record CartForCreationDto
    {
        public int idCart { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int idCustomer { get; set; }

    }
}
