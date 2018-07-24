namespace AutoMappingObjectsExercise.Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class AutoMappingContext : DbContext
    {
        public AutoMappingContext(DbContextOptions options) 
            : base(options)
        {
        }

        public AutoMappingContext()
        {

        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.ManagerEmployees)
                .HasForeignKey(e => e.ManagerId);
        }
    }
}