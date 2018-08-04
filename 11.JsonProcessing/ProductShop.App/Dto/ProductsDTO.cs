namespace ProductShop.App.Dto
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ProductsDTO
    {
        [JsonProperty("count")]
        public string ProductCount { get; set; }

        [JsonProperty("products")]
        public List<ProductWithCountDTO> Products { get; set; }
    }
}
