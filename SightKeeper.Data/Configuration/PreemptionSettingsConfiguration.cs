using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Configuration;

internal sealed class PreemptionSettingsConfiguration : IEntityTypeConfiguration<PreemptionSettings>
{
	public void Configure(EntityTypeBuilder<PreemptionSettings> builder)
	{
		builder.HasFlakeIdKey();
		builder.ComplexProperty(preemptionSettings => preemptionSettings.Factor, factorBuilder =>
		{
			factorBuilder.Property(factor => factor.X).HasColumnName("PreemptionHorizontalFactor");
			factorBuilder.Property(factor => factor.Y).HasColumnName("PreemptionVerticalFactor");
		});
		builder.OwnsOne(preemptionSettings => preemptionSettings.StabilizationSettings, stabilizationSettingsBuilder =>
		{
			stabilizationSettingsBuilder.Property(settings => settings.Method).HasColumnName("PreemptionStabilizationMethod");
			stabilizationSettingsBuilder.Property(settings => settings.BufferSize).HasColumnName("PreemptionStabilizationBufferSize");
		});
	}
}