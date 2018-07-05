namespace _03.EmployeesFullInformation
{
    using P02_DatabaseFirst.Data;
    using System;
    using System.Linq;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees.Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.JobTitle,
                    x.Salary
                }).ToArray();

                File.Delete("Output.txt");

                foreach (var employee in employees)
                {
                    string outputText = $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}";
                    File.AppendAllText("Output.txt", outputText + Environment.NewLine);
                }
            }
        }
    }
}
