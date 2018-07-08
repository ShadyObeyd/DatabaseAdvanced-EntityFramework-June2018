namespace P03_SalesDatabase.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        public DateTime Date { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }
    }
}
