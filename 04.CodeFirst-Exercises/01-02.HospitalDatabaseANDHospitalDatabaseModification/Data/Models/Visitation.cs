namespace P01_HospitalDatabase.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Visitation
    {
        [Key]
        public int VisitationId { get; set; }
        
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string Comments { get; set; }

        public int PatientId { get; set; }
        
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
    }
}
