namespace ProductShop.App.Dto
{
    using Newtonsoft.Json;

    public class UserWithCountDTO
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("soldProducts")]
        public ProductsDTO Products { get; set; }
    }
}
