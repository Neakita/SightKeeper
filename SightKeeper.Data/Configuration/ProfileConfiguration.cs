using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Configuration;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(profile => profile.Id);
        builder.HasFlakeId(profile => profile.Id);
        builder.HasMany(profile => profile.ItemClasses).WithOne().IsRequired();
        builder.HasIndex(profile => profile.Name).IsUnique();
        builder.Navigation(profile => profile.Weights).AutoInclude();
        builder.Navigation(profile => profile.ItemClasses).AutoInclude();
        builder.Property(profile => profile.PostProcessDelay).HasConversion(
            timeSpan => (ushort)timeSpan.TotalMilliseconds,
            number => TimeSpan.FromMilliseconds(number));
        builder.OwnsOne(profile => profile.PreemptionSettings,
            preemptionSettingsBuilder =>
            {
                preemptionSettingsBuilder.Property(preemptionSettings => preemptionSettings.HorizontalFactor).HasColumnName("PreemptionHorizontalFactor");
                preemptionSettingsBuilder.Property(preemptionSettings => preemptionSettings.VerticalFactor).HasColumnName("PreemptionVerticalFactor");
                preemptionSettingsBuilder.OwnsOne(preemptionSettings => preemptionSettings.StabilizationSettings,
                    stabilizationSettingsBuilder =>
                    {
                        stabilizationSettingsBuilder.Property(settings => settings.Method).HasColumnName("PreemptionStabilizationMethod");
                        stabilizationSettingsBuilder.Property(settings => settings.BufferSize).HasColumnName("PreemptionStabilizationBufferSize");
                    });
            });
    }
}