namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PatientMedicament
    {
        public int PatientId { get; set; }
        
        public int MedicamentId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        
        [ForeignKey("MedicamentId")]
        public Medicament Medicament { get; set; }
    }
}
