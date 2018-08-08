using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.DTO.Import
{
    [XmlType("AnimalAid")]
    public class ProcedureAnimalAidDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [XmlElement("Name")]
        public string Name { get; set; }
    }
}
