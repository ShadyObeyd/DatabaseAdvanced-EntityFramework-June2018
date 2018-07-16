namespace P01_BillsPaymentSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CreditCard
    {
        [Key]
        public int CreditCardId { get; set; }

        [Required]
        public decimal Limit { get; set; } // Task 04 requires this setter to be private, but it causes problems in the seed method...

        [Required]
        public decimal MoneyOwed { get; set; } // // Task 04 requires this setter to be private, but it causes problems in the seed method...

        [Required]
        [NotMapped]
        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this.MoneyOwed -= amount;
            }
        }

        public void Withdraw(decimal amount)
        {
            if (this.LimitLeft - amount >= 0)
            {
                this.MoneyOwed += amount;
            }
        }
    }
}