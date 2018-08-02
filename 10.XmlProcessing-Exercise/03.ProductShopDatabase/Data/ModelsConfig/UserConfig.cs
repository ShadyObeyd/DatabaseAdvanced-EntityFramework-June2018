namespace _03.ProductShopDatabase.Data.ModelsConfig
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(u => u.ProductsBought)
                .WithOne(pb => pb.Buyer)
                .HasForeignKey(pb => pb.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(u => u.ProductsSold)
                .WithOne(ps => ps.Seller)
                .HasForeignKey(ps => ps.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
