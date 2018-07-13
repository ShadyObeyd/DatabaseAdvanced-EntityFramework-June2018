namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class ColorConfig : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasKey(c => c.ColorId);

            builder
                .HasMany(c => c.PrimaryKitTeams)
                .WithOne(pkt => pkt.PrimaryKitColor)
                .HasForeignKey(pkt => pkt.PrimaryKitColorId);

            builder
                .HasMany(c => c.SecondaryKitTeams)
                .WithOne(skt => skt.SecondaryKitColor)
                .HasForeignKey(skt => skt.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
