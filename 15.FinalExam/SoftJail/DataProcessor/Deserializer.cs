namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var desirializedDepartments = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            List<Department> departments = new List<Department>();

            foreach (DepartmentDto departmentDto in desirializedDepartments)
            {
                if (!IsValid(departmentDto) || !departmentDto.Cells.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department department = new Department
                {
                    Name = departmentDto.Name
                };

                List<Cell> cells = new List<Cell>();

                foreach (CellDto cellDto in departmentDto.Cells)
                {
                    Cell cell = new Cell
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow,
                        Department = department
                    };

                    cells.Add(cell);
                }

                department.Cells = cells;

                departments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            PrisonerDto[] desirializedPrisoners = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (var prisonerDto in desirializedPrisoners)
            {
                if (!IsValid(prisonerDto) || !prisonerDto.Mails.Any(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<Mail> mails = new List<Mail>();

                foreach (var mailDto in prisonerDto.Mails)
                {
                    Mail mail = new Mail
                    {
                        Address = mailDto.Address,
                        Description = mailDto.Description,
                        Sender = mailDto.Sender
                    };

                    mails.Add(mail);
                }

                DateTime? releaseDate = null;

                if (prisonerDto.ReleaseDate != null)
                {
                    releaseDate = DateTime.ParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                Prisoner prisoner = new Prisoner
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId,
                    Mails = mails
                };

                prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));

            var desirializedOfficers = (OfficerDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();

            List<Officer> officers = new List<Officer>();

            foreach (var officerDto in desirializedOfficers)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool allPrisonersValid = true;

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    int prisonerId = int.Parse(prisonerDto.Id);

                    if (!IsValid(prisonerDto))
                    {
                        allPrisonersValid = false;
                        break;
                    }
                }

                if (!allPrisonersValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Position position;

                if (!Enum.TryParse(officerDto.Position, out position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Weapon weapon;

                if (!Enum.TryParse(officerDto.Weapon, out weapon))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Officer officer = new Officer
                {
                    FullName = officerDto.Name,
                    Salary = decimal.Parse(officerDto.Money),
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = int.Parse(officerDto.DepartmentId)
                };

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    Prisoner prisoner = context.Prisoners.FirstOrDefault(p => p.Id == int.Parse(prisonerDto.Id));

                    OfficerPrisoner officerPrisoner = new OfficerPrisoner
                    {
                        Officer = officer,
                        Prisoner = prisoner
                    };

                    officerPrisoners.Add(officerPrisoner);
                }

                officer.OfficerPrisoners = officerPrisoners;

                officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}