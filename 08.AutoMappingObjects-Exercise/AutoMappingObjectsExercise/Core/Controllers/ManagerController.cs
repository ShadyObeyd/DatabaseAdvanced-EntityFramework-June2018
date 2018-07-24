namespace AutoMappingObjectsExercise.Core.Controllers
{
    using Data.Models.DTOs;
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using AutoMapper;
    using Writers;
    using Microsoft.EntityFrameworkCore;

    public class ManagerController : IManagerController
    {
        private AutoMappingContext context;
        private IWriter writer;

        private const string InvalidEmployeeMessage = "Invalid Employee!";
        private const string InvalidManagerMessage = "Invalid Manager!";

        public ManagerController(AutoMappingContext context)
        {
            this.context = context;
            this.writer = new ConsoleWriter();
        }

        public ManagerDTO GetManagerInfo(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var manager = this.context.Employees.Include(e => e.ManagerEmployees).FirstOrDefault(e => e.EmployeeId == employeeId);

            if (manager == null)
            {
                throw new ArgumentException(InvalidManagerMessage);
            }

            var managerDTO = Mapper.Map<ManagerDTO>(manager);

            return managerDTO;
        }

        public void SetManager(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            int managerId = int.Parse(args[1]);

            var employee = this.context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                throw new ArgumentException(InvalidEmployeeMessage);
            }

            var manager = this.context.Employees.FirstOrDefault(m => m.EmployeeId == managerId);

            if (manager == null)
            {
                throw new ArgumentException(InvalidManagerMessage);
            }

            employee.Manager = manager;

            this.context.SaveChanges();

            this.writer.WriteLine($"{manager.FirstName} {manager.LastName} is now manager of {employee.FirstName} {employee.LastName}");
        }
    }
}
