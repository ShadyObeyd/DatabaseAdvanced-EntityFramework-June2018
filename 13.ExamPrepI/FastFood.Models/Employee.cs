namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Employee
	{
        public Employee()
        {
            this.Orders = new List<Order>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(15, 80)]
        public int Age { get; set; }

        public int PositionId { get; set; }

        [Required]
        [ForeignKey("PositionId")]
        public Position Position { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}