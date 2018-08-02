namespace _01.StudentsXml.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("student")]
    public class Student
    {
        public Student()
        {
            this.Exams = new List<Exam>();
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("gender")]
        public string Gender { get; set; }
        
        [XmlElement("birthDate")]
        public string BirthDate { get; set; }

        [XmlElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("university")]
        public string University { get; set; }

        [XmlElement("speciality")]
        public string Speciality { get; set; }

        [XmlElement("facultyNumber")]
        public string FacultyNumber { get; set; }

        [XmlArray("exams")]
        public List<Exam> Exams { get; set; }
    }
}