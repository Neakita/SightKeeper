using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Database.Configuration;

public sealed class ScreenshotConfiguration : IEntityTypeConfiguration<Screenshot>
{
	public void Configure(EntityTypeBuilder<Screenshot> builder)
	{
		builder.HasFlakeIdKey();
		builder.Ignore(screenshot => screenshot.Library);
		builder.ToTable("Screenshots");
		builder.HasOne<Image>().WithOne().HasPrincipalKey<Screenshot>();
	}
}