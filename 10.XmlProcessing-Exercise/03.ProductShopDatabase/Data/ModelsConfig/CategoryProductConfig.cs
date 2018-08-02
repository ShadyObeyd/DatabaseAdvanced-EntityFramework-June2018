namespace _03.ProductShopDatabase.Data.ModelsConfig
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoryProductConfig : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.HasKey(cp => new { cp.CategoryId, cp.ProductId });

            builder
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(cp => cp.CategoryId);

            builder
                .HasOne(cp => cp.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(cp => cp.ProductId);
        }
    }
}
