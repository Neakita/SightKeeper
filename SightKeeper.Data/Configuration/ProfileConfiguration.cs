using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Configuration;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasFlakeIdKey();
        builder.HasOne(profile => profile.PreemptionSettings).WithOne().HasPrincipalKey<Profile>();
        builder.HasMany(profile => profile.ItemClasses).WithOne().IsRequired();
        builder.HasIndex(profile => profile.Name).IsUnique();
        builder.Navigation(profile => profile.Weights).AutoInclude();
        builder.Property(profile => profile.PostProcessDelay).HasConversion(
            timeSpan => (ushort)timeSpan.TotalMilliseconds,
            number => TimeSpan.FromMilliseconds(number));
    }
}