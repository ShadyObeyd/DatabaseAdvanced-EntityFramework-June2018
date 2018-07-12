namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.TeamId);

            builder
                .HasMany(t => t.HomeGames)
                .WithOne(hg => hg.HomeTeam)
                .HasForeignKey(hg => hg.HomeTeamId);

            builder
                .HasMany(t => t.AwayGames)
                .WithOne(ag => ag.AwayTeam)
                .HasForeignKey(ag => ag.AwayTeamId);

            builder
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId);
        }
    }
}
