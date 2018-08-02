namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("sold-products")]
    public class SoldProductDTO
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("product")]
        public List<ProductWithAttributesDTO> SoldProducts { get; set; }
    }
}
