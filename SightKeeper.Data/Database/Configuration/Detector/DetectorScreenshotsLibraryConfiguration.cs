using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorScreenshotsLibraryConfiguration : IEntityTypeConfiguration<DetectorScreenshotsLibrary>
{
	public void Configure(EntityTypeBuilder<DetectorScreenshotsLibrary> builder)
	{
		builder.ToTable("DetectorScreenshotsLibraries");
		builder.HasMany<DetectorScreenshot>("_screenshots")
			.WithOne(screenshot => screenshot.Library);
	}
}