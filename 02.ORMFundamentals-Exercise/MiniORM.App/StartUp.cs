﻿namespace MiniORM.App
{
    using MiniORM.App.Data;
    using MiniORM.App.Data.Entities;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            string connectionString = @""; // <-- Insert Connection String Here;

            SoftUniDbContext context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            });

            Employee employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}
