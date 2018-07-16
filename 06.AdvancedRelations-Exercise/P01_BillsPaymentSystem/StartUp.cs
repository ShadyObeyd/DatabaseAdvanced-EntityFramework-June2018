namespace P01_BillsPaymentSystem
{
    using P01_BillsPaymentSystem.Data;
    using P01_BillsPaymentSystem.Data.Models;
    using P01_BillsPaymentSystem.Data.Models.Enums;
    using System;
    using System.Collections.Generic;

    public class StartUp
    {
        public static void Main()
        {
            using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                Seed(context);
            }
        }

        // 02.Seed Some Data
        public static void Seed(BillsPaymentSystemContext context)
        {
            User user = new User { FirstName = "Pesho", LastName = "Petrov", Email = "pesho_98@gmail.com", Password = "pesho123" };

            List<BankAccount> bankAccounts = new List<BankAccount>
            {
                new BankAccount { Balance = 48000, BankName = "DSK", SWIFTCode = "ABCDEF" },
                new BankAccount {Balance = 1000, BankName = "Unicredit", SWIFTCode = "QUERTY"}
            };

            List<CreditCard> creditCards = new List<CreditCard>
            {
                new CreditCard {Limit = 10_000, MoneyOwed = 500, ExpirationDate = new DateTime(2019, 12, 10)},
                new CreditCard {Limit = 100_000, MoneyOwed = 88_000, ExpirationDate = new DateTime(2030, 05, 22)}
            };

            List<PaymentMethod> paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod {User = user, BankAccount = bankAccounts[0], Type = PaymentMethodType.BankAccount},
                new PaymentMethod {User = user, BankAccount = bankAccounts[1], Type = PaymentMethodType.BankAccount},
                new PaymentMethod {User = user, CreditCard = creditCards[0], Type = PaymentMethodType.CreditCard},
                new PaymentMethod {User = user, CreditCard = creditCards[1], Type = PaymentMethodType.CreditCard}
            };

            context.Users.Add(user);
            context.BankAccounts.AddRange(bankAccounts);
            context.CreditCards.AddRange(creditCards);
            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();

        }
    }
}