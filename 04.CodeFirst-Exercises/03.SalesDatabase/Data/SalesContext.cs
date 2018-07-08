namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data.Models;

    public class SalesContext : DbContext
    {
        public SalesContext()
        {

        }

        public SalesContext(DbContextOptions options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasDefaultValue("No description");

            modelBuilder.Entity<Sale>()
                .Property(s => s.Date)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
