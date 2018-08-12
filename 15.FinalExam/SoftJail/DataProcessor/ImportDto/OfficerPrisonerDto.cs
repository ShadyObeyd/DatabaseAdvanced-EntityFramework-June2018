using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class OfficerPrisonerDto
    {
        [Required]
        [XmlAttribute("id")]
        public string Id { get; set; }
    }
}
