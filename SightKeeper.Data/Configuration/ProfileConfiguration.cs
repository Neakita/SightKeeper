using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasShadowKey();
        builder.HasMany(profile => profile.ItemClasses).WithOne().IsRequired();
        builder.HasIndex(profile => profile.Name).IsUnique();
        builder.Navigation(profile => profile.Weights).AutoInclude();
        builder.Navigation(profile => profile.ItemClasses).AutoInclude();
        builder.Property(profile => profile.PostProcessDelay).HasConversion(
            timeSpan => (ushort)timeSpan.TotalMilliseconds,
            number => TimeSpan.FromMilliseconds(number));
    }
}