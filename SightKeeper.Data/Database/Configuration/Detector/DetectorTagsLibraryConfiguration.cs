using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public class DetectorTagsLibraryConfiguration : IEntityTypeConfiguration<DetectorTagsLibrary>
{
	public void Configure(EntityTypeBuilder<DetectorTagsLibrary> builder)
	{
		builder.ToTable("DetectorTagsLibraries");
		builder.HasMany<DetectorTag>("_tags").WithOne(tag => tag.Library);
	}
}