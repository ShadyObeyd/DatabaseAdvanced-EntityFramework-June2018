namespace _05.EmployeesFromResearchAndDevelopment
{
    using System;
    using P02_DatabaseFirst.Data;
    using System.Linq;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => e.Department.Name == "Research and Development")
                    .OrderBy(e => e.Salary)
                    .ThenByDescending(e => e.FirstName)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.Department,
                        e.Salary
                    }).ToArray();

                File.Delete("Output.txt");

                foreach (var e in employees)
                {
                    string result = $"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}{Environment.NewLine}";
                    File.AppendAllText("Output.txt", result);
                }
            }
        }
    }
}