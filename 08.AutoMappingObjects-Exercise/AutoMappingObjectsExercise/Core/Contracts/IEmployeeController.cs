namespace AutoMappingObjectsExercise.Core.Contracts
{
    using Data.Models.DTOs;
    using System.Collections.Generic;

    public interface IEmployeeController
    {
        void AddEmployee(string[] args);

        void SetBirthday(string[] args);

        void SetAddress(string[] args);

        EmployeeDTO GetEmployeeInfo(string[] args);

        EmployeeFullInfoDTO GetEmployeePersonalInfo(string[] args);

        List<EmployeeWithManagerDTO> GetEmployeesOlderThan(string[] args);
    }
}
