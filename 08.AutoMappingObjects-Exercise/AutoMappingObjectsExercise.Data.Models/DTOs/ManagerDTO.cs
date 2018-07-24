namespace AutoMappingObjectsExercise.Data.Models.DTOs
{
    using System.Collections.Generic;
    using System.Text;

    public class ManagerDTO
    {
        public ManagerDTO()
        {
            this.EmployeeDTOs = new List<EmployeeDTO>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<EmployeeDTO> EmployeeDTOs { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{this.FirstName} {this.LastName} | Employees: {this.EmployeeDTOs.Count}");

            foreach (EmployeeDTO employeeDTO in this.EmployeeDTOs)
            {
                sb.AppendLine($"- {employeeDTO.FirstName} {employeeDTO.LastName} - ${employeeDTO.Salary:f2}");
            }

            return sb.ToString().Trim();
        }
    }
}
