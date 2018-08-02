namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserDTO
    {
        public UserDTO()
        {
            this.Products = new List<ProductDTO>();
        }

        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        public List<ProductDTO> Products { get; set; }
    }
}
