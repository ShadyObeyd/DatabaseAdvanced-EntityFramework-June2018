namespace P03_UserDetails
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data;
    using P01_BillsPaymentSystem.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            {
                int userId = int.Parse(Console.ReadLine());

                User user = context.Users
                    .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.BankAccount)
                    .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.CreditCard)
                    .FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    Console.WriteLine($"User with id {userId} not found!");
                    return;
                }

                Console.WriteLine($"User: {user.FirstName} {user.LastName}");

                Console.WriteLine($"Bank Accounts:");

                var bankAccounts = user.PaymentMethods.Where(pm => pm.BankAccount != null).Select(pm => pm.BankAccount).ToArray();

                foreach (BankAccount ba in bankAccounts)
                {
                    Console.WriteLine($"-- ID: {ba.BankAccountId}");
                    Console.WriteLine($"--- Balance: {ba.Balance:f2}");
                    Console.WriteLine($"--- Bank: {ba.BankName}");
                    Console.WriteLine($"--- SWIFT: {ba.SWIFTCode}");
                }

                Console.WriteLine($"Credit Cards:");

                var creditCards = user.PaymentMethods.Where(pm => pm.CreditCard != null).Select(pm => pm.CreditCard).ToArray();

                foreach (CreditCard cc in creditCards)
                {
                    Console.WriteLine($"-- ID: {cc.CreditCardId}");
                    Console.WriteLine($"--- Limit: {cc.Limit:f2}");
                    Console.WriteLine($"--- Money Owed: {cc.MoneyOwed:f2}");
                    Console.WriteLine($"--- Limit Left: {cc.LimitLeft:f2}");
                    Console.WriteLine($"--- Expiration Date: {cc.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
                }
            }
        }
    }
}