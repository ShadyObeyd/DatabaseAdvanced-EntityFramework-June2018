﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Item")]
    public class OrderItemDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [XmlElement("Quantity")]
        public string Quantity { get; set; }
    }
}
