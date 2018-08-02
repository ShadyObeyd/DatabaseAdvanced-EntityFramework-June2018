namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserFullInfoDTO
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }

        [XmlElement("sold-products")]
        public SoldProductDTO SoldProduct { get; set; }
    }
}
