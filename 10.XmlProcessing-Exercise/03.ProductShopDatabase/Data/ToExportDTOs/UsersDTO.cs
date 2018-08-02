namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("users")]
    public class UsersDTO
    {
        [XmlAttribute("count")]
        public int UsersCount { get; set; }

        [XmlElement("user")]
        public List<UserFullInfoDTO> Users { get; set; }
    }
}
