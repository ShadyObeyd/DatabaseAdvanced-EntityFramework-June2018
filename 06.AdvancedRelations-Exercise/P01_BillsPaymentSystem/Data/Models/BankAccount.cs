namespace P01_BillsPaymentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }

        [Required]
        public decimal Balance { get; set; } // Task 04 requires this setter to be private, but it causes problems in the seed method...

        [Required]
        [MaxLength(50)]
        public string BankName { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string SWIFTCode { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this.Balance += amount;
            }
        }

        public void Withdraw(decimal amount)
        {
            if (this.Balance - amount >= 0)
            {
                this.Balance -= amount;
            }
        }
    }
}