namespace P01_BillsPaymentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data.EntityConfiguration;
    using P01_BillsPaymentSystem.Data.Models;

    public class BillsPaymentSystemContext : DbContext
    {
        public BillsPaymentSystemContext(DbContextOptions options) 
            : base(options)
        {
        }

        public BillsPaymentSystemContext()
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaymentMethodConfig());
        }
    }
}
