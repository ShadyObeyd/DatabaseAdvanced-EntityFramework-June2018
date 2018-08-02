﻿namespace _03.ProductShopDatabase.Data.ToExportDTOs
{
    using System.Xml.Serialization;

    [XmlType("category")]
    public class CategoryDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("products-count")]
        public int NumberOfProducts { get; set; }

        [XmlElement("average-price")]
        public decimal AveragePrice { get; set; }

        [XmlElement("total-revenue")]
        public decimal TotalRevenue { get; set; }
    }
}
