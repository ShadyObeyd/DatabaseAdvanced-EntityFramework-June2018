namespace _15.RemoveTowns
{
    using System;
    using System.Linq;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    class StartUp
    {
        static void Main()
        {
            string townInput = Console.ReadLine();

            using (SoftUniContext context = new SoftUniContext())
            {
                var town = context.Towns.FirstOrDefault(t => t.Name == townInput);

                var addresses = context.Addresses.Where(a => a.TownId == town.TownId).ToArray();

                int deletedAddresses = addresses.Length;

                foreach (var a in addresses)
                {
                    var employees = context.Employees.Where(e => e.AddressId == a.AddressId);
                
                    foreach (var e in employees)
                    {
                        e.AddressId = null;
                    }
                }

                context.RemoveRange(addresses);
                context.Remove(town);
                context.SaveChanges();

                string address = deletedAddresses == 1 ? "address" : "addresses";
                string wasOrWere = deletedAddresses == 1 ? "was" : "were";
                Console.WriteLine($"{deletedAddresses} {address} in {townInput} {wasOrWere} deleted");
            }
        }
    }
}
