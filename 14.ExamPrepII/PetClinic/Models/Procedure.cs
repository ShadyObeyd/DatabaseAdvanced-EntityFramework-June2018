using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PetClinic.Models
{
    public class Procedure
    {
        public Procedure()
        {
            this.ProcedureAnimalAids = new List<ProcedureAnimalAid>();
        }

        [Key]
        public int Id { get; set; }

        public int AnimalId { get; set; }

        [Required]
        [ForeignKey("AnimalId")]
        public Animal Animal { get; set; }

        public int VetId { get; set; }

        [Required]
        [ForeignKey("VetId")]
        public Vet Vet { get; set; }

        [NotMapped]
        public decimal Cost => this.ProcedureAnimalAids.Sum(pai => pai.AnimalAid.Price);

        [Required]
        public DateTime DateTime { get; set; }

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }
    }
}
