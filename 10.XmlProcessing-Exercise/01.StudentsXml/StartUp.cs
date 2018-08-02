namespace _01.StudentsXml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using Models;

    public class StartUp
    {
        public static void Main()
        {
            Student[] students = GetStudents();

            var serializer = new XmlSerializer(typeof(Student[]));

            using (var streamWriter = new StreamWriter("../../../students.xml"))
            {
                serializer.Serialize(streamWriter, students);
            }
        }

        private static Student[] GetStudents()
        {
            List<Exam> exams = GetExams();

            Student firstStudent = new Student
            {
                Name = "Pesho",
                Gender = "Male",
                BirthDate = "1992/07/31",
                PhoneNumber = "0883635436",
                Email = "pesho.peshev@softuni.bg",
                University = "Software Unviversity",
                Speciality = "C# Web Developer",
                FacultyNumber = "23421432423423",
                Exams = exams
            };

            Student secondStudent = new Student
            {
                Name = "Sara",
                Gender = "Female",
                BirthDate = "1990/03/02",
                PhoneNumber = "0888364563",
                Email = "sara@gmail.com",
                University = "Software Unviversity",
                Speciality = "Java Developer",
                FacultyNumber = "1388731568",
                Exams = exams
            };

            return new Student[] { firstStudent, secondStudent };
        }

        private static List<Exam> GetExams()
        {
            List<Exam> exams = new List<Exam>
            {
                new Exam
                {
                    Name = "Database Basics - MS SQL",
                    DateTaken = "2018/06/24",
                    Grade = "6.0"
                },

                new Exam
                {
                    Name = "Entity Framework Core",
                    DateTaken = "2018/08/12",
                    Grade = "6.0" // I hope :)
                }
            };

            return exams;
        }
    }
}