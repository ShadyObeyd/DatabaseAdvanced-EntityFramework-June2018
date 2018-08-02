namespace _03.ProductShopDatabase.Data.ReadingDTOs
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("product")]
    public class ProductDTO
    {
        [XmlElement("name")]
        [MinLength(3)]
        public string Name { get; set; }
        
        [XmlElement("price")]
        public string Price { get; set; }
    }
}
