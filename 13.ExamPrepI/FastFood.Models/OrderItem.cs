namespace FastFood.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderItem
    {
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int ItemId { get; set; }

        [Required]
        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
