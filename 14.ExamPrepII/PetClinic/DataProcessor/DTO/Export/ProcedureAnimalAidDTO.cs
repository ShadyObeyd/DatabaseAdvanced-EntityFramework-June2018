using System.Xml.Serialization;

namespace PetClinic.DataProcessor.DTO.Export
{
    [XmlType("AnimalAid")]
    public class ProcedureAnimalAidDTO
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
