using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.DTO.Import
{
    [XmlType("Procedure")]
    public class ProcedureDTO
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [XmlElement("Vet")]
        public string VetName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{7}[0-9]{3}$")]
        [XmlElement("Animal")]
        public string AnimalPassportSerialNumber { get; set; }

        [Required]
        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [Required]
        [XmlArray("AnimalAids")]
        public ProcedureAnimalAidDTO[] AnimalAids { get; set; }
    }
}
