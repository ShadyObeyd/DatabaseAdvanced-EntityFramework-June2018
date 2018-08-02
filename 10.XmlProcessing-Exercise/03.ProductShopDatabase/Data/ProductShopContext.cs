namespace _03.ProductShopDatabase.Data
{
    using ModelsConfig;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProductShopContext : DbContext
    {
        public ProductShopContext(DbContextOptions options) 
            : base(options)
        { }

        public ProductShopContext()
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new CategoryProductConfig());
        }
    }
}