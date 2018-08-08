namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.DTO.Import;
    using PetClinic.Models;
    using DataAnnotations = System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Error: Invalid data.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            AnimalAidDTO[] animalAidDTOs = JsonConvert.DeserializeObject<AnimalAidDTO[]>(jsonString);

            List<AnimalAid> animalAids = new List<AnimalAid>();

            StringBuilder sb = new StringBuilder();

            foreach (AnimalAidDTO animalAidDTO in animalAidDTOs)
            {
                if (!IsValid(animalAidDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (animalAids.Any(ai => ai.Name == animalAidDTO.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                AnimalAid animalAid = new AnimalAid
                {
                    Name = animalAidDTO.Name,
                    Price = animalAidDTO.Price
                };

                animalAids.Add(animalAid);

                sb.AppendLine(string.Format(SuccessMessage, animalAid.Name));
            }

            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            AnimalDTO[] animalDTOs = JsonConvert.DeserializeObject<AnimalDTO[]>(jsonString);
            List<Animal> animals = new List<Animal>();

            StringBuilder sb = new StringBuilder();

            foreach (AnimalDTO animalDTO in animalDTOs)
            {
                if (!IsValid(animalDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!IsValid(animalDTO.Passport))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (animals.Any(a => a.Passport.SerialNumber == animalDTO.Passport.SerialNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Animal animal = new Animal
                {
                    Name = animalDTO.Name,
                    Age = animalDTO.Age,
                    Type = animalDTO.Type,
                    Passport = new Passport
                    {
                        SerialNumber = animalDTO.Passport.SerialNumber,
                        OwnerName = animalDTO.Passport.OwnerName,
                        OwnerPhoneNumber = animalDTO.Passport.OwnerPhoneNumber,
                        RegistrationDate = DateTime.ParseExact(animalDTO.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    }
                };

                animals.Add(animal);

                sb.AppendLine($"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }

            context.Animals.AddRange(animals);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(VetDTO[]), new XmlRootAttribute("Vets"));

            StringBuilder sb = new StringBuilder();

            List<Vet> vets = new List<Vet>();

            using (var reader = new StringReader(xmlString))
            {
                VetDTO[] vetDTOs = (VetDTO[])serializer.Deserialize(reader);

                foreach (VetDTO vetDTO in vetDTOs)
                {
                    if (!IsValid(vetDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (vets.Any(v => v.PhoneNumber == vetDTO.PhoneNumber))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Vet vet = new Vet
                    {
                        Name = vetDTO.Name,
                        Profession = vetDTO.Profession,
                        Age = vetDTO.Age,
                        PhoneNumber = vetDTO.PhoneNumber
                    };

                    vets.Add(vet);

                    sb.AppendLine(string.Format(SuccessMessage, vet.Name));
                }

                context.Vets.AddRange(vets);
                context.SaveChanges();

                return sb.ToString().TrimEnd();
            }
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProcedureDTO[]), new XmlRootAttribute("Procedures"));

            StringBuilder sb = new StringBuilder();

            List<Procedure> procedures = new List<Procedure>();

            var procedureAnimalAids = new List<ProcedureAnimalAid>();

            using (var reader = new StringReader(xmlString))
            {
                ProcedureDTO[] procedureDTOs = (ProcedureDTO[])serializer.Deserialize(reader);

                foreach (ProcedureDTO procedureDTO in procedureDTOs)
                {
                    if (!IsValid(procedureDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Vet vet = context.Vets.FirstOrDefault(v => v.Name == procedureDTO.VetName);

                    if (vet == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Animal animal = context.Animals.FirstOrDefault(a => a.Passport.SerialNumber == procedureDTO.AnimalPassportSerialNumber);

                    if (animal == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool animalAidsValid = true;

                    foreach (var procedureAnimalAidDTO in procedureDTO.AnimalAids)
                    {
                        if (!IsValid(procedureAnimalAidDTO))
                        {
                            animalAidsValid = false;
                            break;
                        }
                    }

                    if (!animalAidsValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool animalAidsExist = true;

                    foreach (var procedureAnimalAidDTO in procedureDTO.AnimalAids)
                    {
                        if (!context.AnimalAids.Any(aa => aa.Name == procedureAnimalAidDTO.Name))
                        {
                            animalAidsExist = false;
                            break;
                        }
                    }

                    if (!animalAidsExist)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool animalAidsUnique = true;

                    var procedureAnimalAidsNames = new List<string>();

                    foreach (var procedureAnimalAidDTO in procedureDTO.AnimalAids)
                    {
                        if (procedureAnimalAidsNames.Contains(procedureAnimalAidDTO.Name))
                        {
                            animalAidsUnique = false;
                            break;
                        }

                        procedureAnimalAidsNames.Add(procedureAnimalAidDTO.Name);
                    }

                    if (!animalAidsUnique)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Procedure procedure = new Procedure
                    {
                        Vet = vet,
                        Animal = animal,
                        DateTime = DateTime.ParseExact(procedureDTO.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    };

                    procedures.Add(procedure);

                    foreach (var procedureAnimalAidDTO in procedureDTO.AnimalAids)
                    {
                        AnimalAid animalAid = context.AnimalAids.FirstOrDefault(ai => ai.Name == procedureAnimalAidDTO.Name);

                        ProcedureAnimalAid procedureAnimalAid = new ProcedureAnimalAid
                        {
                            Procedure = procedure,
                            AnimalAid = animalAid
                        };

                        procedureAnimalAids.Add(procedureAnimalAid);
                    }

                    sb.AppendLine("Record successfully imported.");
                }

                context.Procedures.AddRange(procedures);
                context.SaveChanges();

                context.ProceduresAnimalAids.AddRange(procedureAnimalAids);
                context.SaveChanges();
                

                return sb.ToString().TrimEnd();
            }
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new DataAnnotations.ValidationContext(obj);

            var validationResults = new List<DataAnnotations.ValidationResult>();

            return DataAnnotations.Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}