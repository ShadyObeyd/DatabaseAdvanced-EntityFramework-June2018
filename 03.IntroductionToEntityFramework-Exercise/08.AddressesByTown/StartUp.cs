namespace _08.AddressesByTown
{
    using System;
    using System.Linq;
    using System.IO;
    using P02_DatabaseFirst.Data;

    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var addresses = context.Addresses
                    .OrderByDescending(a => a.Employees.Count)
                    .ThenBy(a => a.Town.Name)
                    .ThenBy(a => a.AddressText)
                    .Select(a => new
                    {
                        a.AddressText,
                        TownName = a.Town.Name,
                        EmployeeCount = a.Employees.Count
                    })
                    .Take(10)
                    .ToArray();

                File.Delete("Output.txt");

                foreach (var a in addresses)
                {
                    string address = $"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees{Environment.NewLine}";

                    File.AppendAllText("Output.txt", address);
                }
            }
        }
    }
}
