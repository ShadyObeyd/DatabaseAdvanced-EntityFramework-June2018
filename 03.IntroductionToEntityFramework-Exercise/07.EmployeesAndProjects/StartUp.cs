namespace _07.EmployeesAndProjects
{
    using System;
    using System.Linq;
    using System.IO;
    using P02_DatabaseFirst.Data;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                        Projects = e.EmployeesProjects.Select(p => new
                        {
                            p.Project.Name,
                            p.Project.StartDate,
                            p.Project.EndDate
                        }).ToArray()
                    })
                    .Take(30)
                    .ToArray();


                File.Delete("Output.txt");

                foreach (var e in employees)
                {
                    string employee = $"{e.FirstName} {e.LastName} - Manager: {e.ManagerName}";

                    File.AppendAllText("Output.txt", employee + Environment.NewLine);

                    foreach (var p in e.Projects)
                    {
                        string startDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        string endDate = p.EndDate == null ? "not finished" : p.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                        string project = $"--{p.Name} - {startDate} - {endDate}";

                        File.AppendAllText("Output.txt", project + Environment.NewLine);
                    }
                }
            }
        }
    }
}
