namespace ProductShop.App.Dto
{
    using Newtonsoft.Json;

    public class UserDTO
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ProductWithBuyerDTO[] SoldProducts { get; set; }
    }
}
