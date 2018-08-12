using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentDto
    {
        [Required]
        [MinLength(3), MaxLength(25)]
        public string Name { get; set; }

        [Required]
        public CellDto[] Cells { get; set; }
    }
}
