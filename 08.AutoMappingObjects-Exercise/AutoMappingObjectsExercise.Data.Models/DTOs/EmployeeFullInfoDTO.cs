namespace AutoMappingObjectsExercise.Data.Models.DTOs
{
    using System.Text;

    public class EmployeeFullInfoDTO
    {
        public int EmployeeId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public decimal Salary { get; set; }

        public int DayOfBirth { get; set; }

        public int BirthdayMonth { get; set; }

        public int BirthdayYear { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"ID:{this.EmployeeId} - {this.FirstName} {this.LastName} - ${this.Salary:f2}");
            builder.AppendLine($"Birthday: {this.DayOfBirth:d2}-{this.BirthdayMonth:d2}-{this.BirthdayYear}");
            builder.AppendLine($"Address: {this.Address}");

            return builder.ToString().Trim();
        }
    }
}
