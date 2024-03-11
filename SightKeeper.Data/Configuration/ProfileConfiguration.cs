using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Configuration;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasFlakeIdKey();
        builder.ComplexProperty(profile => profile.PreemptionSettings,
	        settingsBuilder =>
	        {
		        settingsBuilder.Property(preemptionSettings => preemptionSettings.Value.Factor.X).HasColumnName("PreemptionHorizontalFactor");
		        settingsBuilder.Property(preemptionSettings => preemptionSettings.Value.Factor.Y).HasColumnName("PreemptionVerticalFactor");
		        settingsBuilder.ComplexProperty(preemptionSettings => preemptionSettings.Value.StabilizationSettings,
			        stabilizationSettingsBuilder =>
			        {
				        stabilizationSettingsBuilder.Property(settings => settings.Value.Method).HasColumnName("PreemptionStabilizationMethod");
				        stabilizationSettingsBuilder.Property(settings => settings.Value.BufferSize).HasColumnName("PreemptionStabilizationBufferSize");
			        });
	        });
        builder.HasMany(profile => profile.ItemClasses).WithOne().IsRequired();
        builder.HasIndex(profile => profile.Name).IsUnique();
        builder.Navigation(profile => profile.Weights).AutoInclude();
        builder.Navigation(profile => profile.ItemClasses).AutoInclude();
        builder.Property(profile => profile.PostProcessDelay).HasConversion(
            timeSpan => (ushort)timeSpan.TotalMilliseconds,
            number => TimeSpan.FromMilliseconds(number));
    }
}