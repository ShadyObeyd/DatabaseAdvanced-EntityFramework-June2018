namespace _12.IncreaseSalaries
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
                var employees = context.Employees
                    .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                    .ToArray();

                File.Delete("Output.txt");

                foreach (var e in employees)
                {
                    e.Salary *= 1.12m;
                }

                context.SaveChanges();

                File.Delete("Output.txt");

                var wantedEmployees = employees
                    .OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.Salary
                    }).ToArray();

                File.Delete("Output.txt");

                foreach (var e in wantedEmployees)
                {
                    string result = $"{e.FirstName} {e.LastName} (${e.Salary})";

                    File.AppendAllText("Output.txt", result);
                }
            }
        }
    }
}
