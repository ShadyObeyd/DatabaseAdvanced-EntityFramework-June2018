namespace _09.Employee147
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
                var employee = context.Employees
                    .Where(e => e.EmployeeId == 147)
                    .Select(e => new
                    {
                        Name = e.FirstName + " " + e.LastName,
                        e.JobTitle,
                        Projects = e.EmployeesProjects
                        .OrderBy(p => p.Project.Name)
                        .Select(p => new
                        {
                            ProjectName = p.Project.Name
                        }).ToArray()
                    }).FirstOrDefault();

                File.Delete("Output.txt");

                File.AppendAllText("Output.txt", $"{employee.Name} - {employee.JobTitle}{Environment.NewLine}");

                foreach (var p in employee.Projects)
                {
                    File.AppendAllText("Output.txt", p.ProjectName + Environment.NewLine);
                }
            }
        }
    }
}
