namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        [Column(TypeName = "nvarchar(80)")]
        public string Name { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}
