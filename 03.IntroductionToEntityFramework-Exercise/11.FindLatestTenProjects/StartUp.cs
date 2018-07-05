namespace _11.FindLatestTenProjects
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
                var projects = context.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.StartDate
                    })
                    .OrderBy(p => p.Name)
                    .ToArray();

                File.Delete("Output.txt");

                foreach (var p in projects)
                {
                    File.AppendAllText("Output.txt", p.Name + Environment.NewLine);
                    File.AppendAllText("Output.txt", p.Description + Environment.NewLine);
                    File.AppendAllText("Output.txt", p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) + Environment.NewLine);
                }
            }
        }
    }
}
