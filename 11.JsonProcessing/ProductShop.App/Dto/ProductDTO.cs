namespace ProductShop.App.Dto
{
    using Newtonsoft.Json;

    public class ProductDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("seller")]
        public string SellerFullName { get; set; }
    }
}
