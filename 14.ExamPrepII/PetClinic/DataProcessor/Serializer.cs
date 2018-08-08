namespace PetClinic.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.DTO.Export;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animals = context.Animals
                .Where(a => a.Passport.OwnerPhoneNumber == phoneNumber)
                .OrderBy(a => a.Age)
                .ThenBy(a => a.PassportSerialNumber)
                .Select(a => new
                {
                    OwnerName = a.Passport.OwnerName,
                    AnimalName = a.Name,
                    Age = a.Age,
                    SerialNumber = a.PassportSerialNumber,
                    RegisteredOn = a.Passport.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                }).ToArray();


            return JsonConvert.SerializeObject(animals, Newtonsoft.Json.Formatting.Indented);
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procedures = context.Procedures
                .OrderBy(p => p.DateTime)
                .ThenBy(p => p.Animal.PassportSerialNumber)
                .Select(p => new ProcedureDTO
                {
                    PassportSerialNumber = p.Animal.PassportSerialNumber,
                    OwnerPhoneNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    ProcedureAnimalAids = p.ProcedureAnimalAids.Select(pai => new ProcedureAnimalAidDTO
                    {
                        Name = pai.AnimalAid.Name,
                        Price = pai.AnimalAid.Price
                    }).ToArray(),
                    TotalPrice = p.Cost
                }).ToArray();

            StringBuilder sb = new StringBuilder();

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var serializer = new XmlSerializer(typeof(ProcedureDTO[]), new XmlRootAttribute("Procedures"));

            serializer.Serialize(new StringWriter(sb), procedures, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
