namespace AutoMappingObjectsExercise.Core
{
    using Contracts;
    using System.Reflection;
    using System.Linq;
    using System;
    using Controllers;
    using Data;
    using AutoMapper;
    using Data.Models;
    using Data.Models.DTOs;

    public class CommandInterpreter : ICommandInterpreter
    {
        private IEmployeeController employeeController;
        private IManagerController managerController;

        private const string InvalidCommandMessage = "Invalid Command!";

        public CommandInterpreter(AutoMappingContext context)
        {
            this.employeeController = new EmployeeController(context);
            this.managerController = new ManagerController(context);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDTO>();
                cfg.CreateMap<Employee, ManagerDTO>()
                   .ForMember(dto => dto.EmployeeDTOs, opt => opt.MapFrom(src => src.ManagerEmployees));
                cfg.CreateMap<Employee, EmployeeWithManagerDTO>()
                   .ForMember(dto => dto.ManagerLastName, opt => opt.MapFrom(src => src.Manager.LastName));
                cfg.CreateMap<Employee, EmployeeFullInfoDTO>()
                   .ForMember(dto => dto.DayOfBirth, opt => opt.MapFrom(src => src.Birthday.Value.Day))
                   .ForMember(dto => dto.BirthdayMonth, opt => opt.MapFrom(src => src.Birthday.Value.Month))
                   .ForMember(dto => dto.BirthdayYear, opt => opt.MapFrom(src => src.Birthday.Value.Year));
            });
        }

        public void Read(string[] inputTokens)
        {
            var commandName = inputTokens[0] + "Command";

            var commandType = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(c => c.Name == commandName);

            if (commandType == null)
            {
                throw new ArgumentException(InvalidCommandMessage);
            }

            var commandArgs = inputTokens.Skip(1).ToArray();

            var command = (ICommand)Activator.CreateInstance(commandType, this.employeeController, this.managerController);

            command.Execute(commandArgs);
        }
    }
}
