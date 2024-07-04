using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Database.Configuration.Classifier;

public sealed class ClassifierScreenshotsLibraryConfiguration : IEntityTypeConfiguration<ClassifierScreenshotsLibrary>
{
	public void Configure(EntityTypeBuilder<ClassifierScreenshotsLibrary> builder)
	{
		builder.ToTable("ClassifierScreenshotsLibraries");
		builder.HasMany<ClassifierScreenshot>("_screenshots")
			.WithOne(screenshot => screenshot.Library);
	}
}