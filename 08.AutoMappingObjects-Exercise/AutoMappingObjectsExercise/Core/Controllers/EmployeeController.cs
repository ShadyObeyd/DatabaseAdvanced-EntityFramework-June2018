namespace AutoMappingObjectsExercise.Core.Controllers
{
    using Contracts;
    using Data.Models.DTOs;
    using Data;
    using Data.Models;
    using System;
    using System.Linq;
    using System.Globalization;
    using AutoMapper;
    using Writers;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;

    public class EmployeeController : IEmployeeController
    {
        private readonly AutoMappingContext context;
        private IWriter writer;

        private const string InvalidEmployeeIdMessage = "Invalid Employee Id";
        private const string InvalidAddressInputMessage = "Address cannot be empty";
        private const string InvalidAddressOrBirthdayMessage = "Employee birthday and address must both have a value!";

        public EmployeeController(AutoMappingContext context)
        {
            this.context = context;
            this.writer = new ConsoleWriter();
        }

        public void AddEmployee(string[] args)
        {
            var employee = new Employee(args[0], args[1], decimal.Parse(args[2]));

            context.Employees.Add(employee);
            context.SaveChanges();

            this.writer.WriteLine($"Successfully added {employee.FirstName} {employee.LastName} to the database.");
        }

        public EmployeeDTO GetEmployeeInfo(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                throw new ArgumentException(InvalidEmployeeIdMessage);
            }

            var employeeDTO = Mapper.Map<EmployeeDTO>(employee);

            return employeeDTO;
        }

        public EmployeeFullInfoDTO GetEmployeePersonalInfo(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                throw new ArgumentException(InvalidEmployeeIdMessage);
            }

            if (employee.Birthday == null || employee.Address == null)
            {
                throw new InvalidOperationException(InvalidAddressOrBirthdayMessage);
            }

            var employeeFullInfoDTO = Mapper.Map<EmployeeFullInfoDTO>(employee);

            return employeeFullInfoDTO;
        }

        public void SetAddress(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            string[] addressInput = args.Skip(1).ToArray();
            string address = string.Join(" ", addressInput);

            var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                throw new ArgumentException(InvalidEmployeeIdMessage);
            }

            if (address == null || address == string.Empty)
            {
                throw new ArgumentException(InvalidAddressInputMessage);
            }

            employee.Address = address;

            context.SaveChanges();

            this.writer.WriteLine($"Successfully set address of employee {employee.FirstName} {employee.LastName} to {employee.Address}");
        }

        public void SetBirthday(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            DateTime date = DateTime.ParseExact(args[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                throw new ArgumentException(InvalidEmployeeIdMessage);
            }

            employee.Birthday = date;
            context.SaveChanges();

            this.writer.WriteLine($"Successfully set birthday to employee {employee.FirstName} {employee.LastName} to {employee.Birthday?.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}");
        }

        public List<EmployeeWithManagerDTO> GetEmployeesOlderThan(string[] args)
        {
            int age = int.Parse(args[0]);

            var employees = this.context.Employees.Where(e => DateTime.Now.Year - e.Birthday.Value.Year > age).ProjectTo<EmployeeWithManagerDTO>().ToList();


            return employees.OrderByDescending(e => e.Salary).ToList();
        }
    }
}