namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;
    using PetClinic.Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext() { }

        public PetClinicContext(DbContextOptions options)
            :base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        public DbSet<Procedure> Procedures { get; set; }

        public DbSet<Animal> Animals { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }

        public DbSet<AnimalAid> AnimalAids { get; set; }

        public DbSet<Vet> Vets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Vet>()
                .HasIndex(v => v.PhoneNumber)
                .IsUnique();

            builder.Entity<AnimalAid>()
                .HasIndex(ai => ai.Name)
                .IsUnique();

            builder.Entity<ProcedureAnimalAid>()
                .HasKey(pai => new { pai.AnimalAidId, pai.ProcedureId });

            builder.Entity<Passport>().HasKey(p => p.SerialNumber);
        }
    }
}
