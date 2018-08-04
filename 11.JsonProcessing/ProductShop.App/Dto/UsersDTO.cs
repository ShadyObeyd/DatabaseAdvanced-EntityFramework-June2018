namespace ProductShop.App.Dto
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class UsersDTO
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public List<UserWithCountDTO> Users { get; set; }
    }
}
