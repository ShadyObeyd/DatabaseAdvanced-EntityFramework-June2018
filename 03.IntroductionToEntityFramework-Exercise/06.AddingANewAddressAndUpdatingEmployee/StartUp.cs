namespace _06.AddingANewAddressAndUpdatingEmployee
{
    using System;
    using System.Linq;
    using System.IO;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                Address address = new Address()
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                Employee employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

                employee.Address = address;

                context.SaveChanges();

                var employees = context.Employees
                    .OrderByDescending(e => e.AddressId)
                    .Select(e => new
                    {
                        AddressText = e.Address.AddressText
                    })
                    .ToArray();

                File.Delete("Output.txt");

                int cntr = 1;

                foreach (var e in employees)
                {
                    string result = $"{e.AddressText}{Environment.NewLine}";

                    File.AppendAllText("Output.txt", result);

                    if (cntr == 10)
                    {
                        break;
                    }

                    cntr++;
                }
            }
        }
    }
}
