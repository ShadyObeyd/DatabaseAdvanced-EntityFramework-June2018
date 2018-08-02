namespace _01.StudentsXml.Models
{
    using System.Xml.Serialization;

    [XmlType("exam")]
    public class Exam
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("dateTaken")]
        public string DateTaken { get; set; }

        [XmlElement("grade")]
        public string Grade { get; set; }
    }
}
