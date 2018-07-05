namespace _10.DepartmentsWithMoreThanFiveEmployees
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
                var departments = context.Departments
                    .Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .Select(d => new
                    {
                        d.Name,
                        ManagerName = d.Manager.FirstName + " " + d.Manager.LastName,
                        Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            EmployeeName = e.FirstName + " " + e.LastName,
                            e.JobTitle
                        }).ToArray()
                    }).ToArray();

                File.Delete("Output.txt");

                foreach (var d in departments)
                {
                    string department = $"{d.Name} - {d.ManagerName}{Environment.NewLine}";

                    File.AppendAllText("Output.txt", department);

                    foreach (var e in d.Employees)
                    {
                        string employee = $"{e.EmployeeName} - {e.JobTitle}{Environment.NewLine}";

                        File.AppendAllText("Output.txt", employee);
                    }

                    File.AppendAllText("Output.txt", new string('-', 10) + Environment.NewLine);
                }
            }
        }
    }
}
