namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Xml.Serialization;

    [XmlType("product")]
    public class ProductInfoDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public string Price { get; set; }

        [XmlAttribute("buyer")]
        public string BuyerFullName { get; set; }
    }
}
