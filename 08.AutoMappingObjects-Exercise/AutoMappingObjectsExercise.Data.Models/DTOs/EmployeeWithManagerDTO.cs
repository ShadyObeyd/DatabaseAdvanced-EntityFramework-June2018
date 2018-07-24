namespace AutoMappingObjectsExercise.Data.Models.DTOs
{
    public class EmployeeWithManagerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public string ManagerLastName { get; set; }

        public override string ToString()
        {
            string managerName = this.ManagerLastName ?? "[no manager]";

            return $"{this.FirstName} {this.LastName} - ${this.Salary} - Manager: {managerName}";
        }
    }
}
