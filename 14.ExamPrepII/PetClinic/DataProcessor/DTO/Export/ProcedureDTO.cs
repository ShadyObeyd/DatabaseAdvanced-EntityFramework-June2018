using System.Xml.Serialization;

namespace PetClinic.DataProcessor.DTO.Export
{
    [XmlType("Procedure")]
    public class ProcedureDTO
    {
        [XmlElement("Passport")]
        public string PassportSerialNumber { get; set; }

        [XmlElement("OwnerNumber")]
        public string OwnerPhoneNumber { get; set; }

        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public ProcedureAnimalAidDTO[] ProcedureAnimalAids { get; set; }

        [XmlElement("TotalPrice")]
        public decimal TotalPrice { get; set; }
    }
}
