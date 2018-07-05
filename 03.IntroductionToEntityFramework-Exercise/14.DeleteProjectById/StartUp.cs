namespace _14.DeleteProjectById
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
                var project = context.Projects.FirstOrDefault(p => p.ProjectId == 2);
                var employeeProjects = context.EmployeesProjects.Where(p => p.ProjectId == 2).ToArray();
                
                context.EmployeesProjects.RemoveRange(employeeProjects);
                context.Projects.Remove(project);
                
                context.SaveChanges();
                
                var projects = context.Projects
                    .Select(p => new
                    {
                        p.Name
                    })
                    .Take(10)
                    .ToArray();

                File.Delete("Output.txt");

                foreach (var p in projects)
                {
                    File.AppendAllText("Output.txt", p.Name + Environment.NewLine);
                }
            }
        }
    }
}
