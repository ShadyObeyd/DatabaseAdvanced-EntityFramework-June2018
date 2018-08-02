namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Xml.Serialization;

    [XmlType("product")]
    public class ProductDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public string Price { get; set; }
    }
}
