namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Xml.Serialization;

    [XmlType("product")]
    public class ProductWithAttributesDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public string Price { get; set; }
    }
}
