namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    }).OrderBy(po => po.OfficerName).ToArray(),
                    TotalOfficerSalary = p.PrisonerOfficers.Sum(po => po.Officer.Salary)
                }).ToArray();

            return JsonConvert.SerializeObject(prisoners, Newtonsoft.Json.Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] prisonerNames = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var prisoners = context.Prisoners
                .Where(p => prisonersNames.Contains(p.FullName))
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .Select(p => new PrisonerDto
                {
                    Id = p.Id.ToString(),
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new MessageDto
                    {
                        Description = Reverse(m.Description)
                    }).ToArray()
                }).ToArray();

            StringBuilder sb = new StringBuilder();

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var serializer = new XmlSerializer(typeof(PrisonerDto[]), new XmlRootAttribute("Prisoners"));

            serializer.Serialize(new StringWriter(sb), prisoners, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }

        private static string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
    }
}