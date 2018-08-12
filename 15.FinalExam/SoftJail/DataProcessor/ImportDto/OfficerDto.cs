using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement("Money")]
        public string Money { get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [Required]
        [XmlElement("DepartmentId")]
        public string DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public OfficerPrisonerDto[] Prisoners { get; set; }
    }
}
