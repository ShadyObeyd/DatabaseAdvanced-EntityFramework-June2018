namespace _13.FindEmployeesByFirstNameStartingWithSa
{
    using System;
    using System.Linq;
    using System.IO;
    using P02_DatabaseFirst.Data;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => EF.Functions.Like(e.FirstName, "Sa%"))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        EmployeeName = e.FirstName + " " + e.LastName,
                        e.JobTitle,
                        e.Salary
                    }).ToArray();

                File.Delete("Output.txt");

                foreach (var e in employees)
                {
                    string result = $"{e.EmployeeName} - {e.JobTitle} - (${e.Salary:f2}){Environment.NewLine}";

                    File.AppendAllText("Output.txt", result);
                }
            }
        }
    }
}
