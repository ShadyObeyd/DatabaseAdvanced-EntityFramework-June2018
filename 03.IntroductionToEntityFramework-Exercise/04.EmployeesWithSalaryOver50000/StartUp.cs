namespace _04.EmployeesWithSalaryOver50000
{
    using System;
    using System.IO;
    using System.Linq;
    using P02_DatabaseFirst.Data;

    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context
                    .Employees.Where(e => e.Salary > 50000)
                    .OrderBy(e => e.FirstName)
                    .Select(e => new
                    {
                        e.FirstName
                    }).ToArray();

                File.Delete("Output.txt");

                foreach (var e in employees)
                {
                    File.AppendAllText("Output.txt", $"{e.FirstName}{Environment.NewLine}");
                }
            }   
        }
    }
}
