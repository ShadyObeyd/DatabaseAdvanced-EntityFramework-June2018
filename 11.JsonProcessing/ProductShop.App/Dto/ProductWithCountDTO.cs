namespace ProductShop.App.Dto
{
    using Newtonsoft.Json;

    public class ProductWithCountDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
