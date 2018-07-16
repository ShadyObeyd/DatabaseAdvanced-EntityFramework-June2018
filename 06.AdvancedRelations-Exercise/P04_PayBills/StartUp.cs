namespace P04_PayBills
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data;
    using P01_BillsPaymentSystem.Data.Models;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            int userId = int.Parse(Console.ReadLine());

            PayBills(userId, 3000);
        }

        public static void PayBills(int userId, decimal amount)
        {
            User user = null;

            using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            {
                user = context.Users
                    .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.BankAccount)
                    .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.CreditCard)
                    .FirstOrDefault(u => u.UserId == userId);
            }

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
            }
            else
            {
                decimal bankAccountsSum = user.PaymentMethods.Where(pm => pm.BankAccount != null).Sum(pm => pm.BankAccount.Balance);
                decimal creditCardsSum = user.PaymentMethods.Where(pm => pm.CreditCard != null).Sum(pm => pm.CreditCard.LimitLeft);

                decimal totalSum = bankAccountsSum + creditCardsSum;

                if (totalSum >= amount)
                {
                    var bankAccounts = user.PaymentMethods.Where(pm => pm.BankAccount != null).Select(pm => pm.BankAccount).OrderBy(ba => ba.BankAccountId).ToArray();

                    foreach (BankAccount ba in bankAccounts)
                    {
                        if (ba.Balance >= amount)
                        {
                            ba.Withdraw(amount);
                            amount = 0;
                        }
                        else
                        {
                            amount -= ba.Balance;
                            ba.Withdraw(ba.Balance);
                        }

                        if (amount == 0)
                        {
                            return;
                        }
                    }

                    var creditCards = user.PaymentMethods.Where(pm => pm.CreditCard != null).Select(pm => pm.CreditCard).OrderBy(cc => cc.CreditCardId).ToArray();

                    foreach (var cc in creditCards)
                    {
                        if (cc.LimitLeft >= amount)
                        {
                            cc.Withdraw(amount);
                            amount = 0;
                        }
                        else
                        {
                            amount -= cc.LimitLeft;
                            cc.Withdraw(cc.LimitLeft);
                        }

                        if (amount == 0)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Insufficient funds!");
                }
            }
        }
    }
}
