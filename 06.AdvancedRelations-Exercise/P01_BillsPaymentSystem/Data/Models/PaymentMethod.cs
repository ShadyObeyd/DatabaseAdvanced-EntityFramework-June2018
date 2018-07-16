using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P01_BillsPaymentSystem.Data.Models.Attributes;
using P01_BillsPaymentSystem.Data.Models.Enums;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 2)]
        public PaymentMethodType Type { get; set; }
        
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Xor(nameof(BankAccountId))]
        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public int? BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}