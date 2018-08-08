using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Models
{
    public class ProcedureAnimalAid
    {
        public int ProcedureId { get; set; }

        [Required]
        [ForeignKey("ProcedureId")]
        public Procedure Procedure { get; set; }

        public int AnimalAidId { get; set; }

        [Required]
        [ForeignKey("AnimalAidId")]
        public AnimalAid AnimalAid { get; set; }
    }
}
